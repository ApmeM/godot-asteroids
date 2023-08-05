using Godot;
using Godot.Collections;
using GodotAnalysers;

[SceneReference("Minimap.tscn")]
[Tool]
public partial class Minimap
{
    [Export]
    public Vector2 WorldRectSize = new Vector2(480, 800);

    [Export]
    public Vector2 WorldRectPosition = Vector2.Zero;

    [Export]
    public NodePath CenterNodePath
    {
        get => centerNodePath;
        set
        {
            centerNodePath = value;
            centerNodePathDity = true;
        }
    }

    private bool centerNodePathDity;
    private NodePath centerNodePath;
    private Node2D centerNode;

    // Dictionaries to map world elements to minimap elements
    private Dictionary<Node2D, Node2D> knownMarkers = new Dictionary<Node2D, Node2D>();
    private Dictionary<Node2D, Node2D> newKnownMarkers = new Dictionary<Node2D, Node2D>();

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        this.FillMembers();

        if (centerNodePathDity)
        {
            if (centerNodePath == null || centerNodePath.IsEmpty())
                centerNode = null;
            else
                centerNode = (Node2D)this.GetNode(centerNodePath);
        }

        var minimapElements = this.GetTree().GetNodesInGroup(Groups.MinimapElement);
        foreach (Node2D element in minimapElements)
        {
            if (!(element is IMinimapElement) && !((element is Sprite)))
            {
                GD.PrintErr($"{element} is in {Groups.MinimapElement} group but do not implement {nameof(IMinimapElement)} and not {nameof(Sprite)}");
                continue;
            }

            var minimapElement = element as IMinimapElement;
            var minimapSprite = (element as Sprite) ?? (minimapElement?.Sprite);

            if (!knownMarkers.ContainsKey(element))
            {
                var newEnemyMarker = (Sprite)this.marker.Duplicate();
                newEnemyMarker.Texture = minimapSprite.Texture;
                
                this.ySort.AddChild(newEnemyMarker);
                newEnemyMarker.Show();
                newKnownMarkers[element] = newEnemyMarker;
            }
            else
            {
                newKnownMarkers[element] = knownMarkers[element];
                knownMarkers.Remove(element);
            }

            var markerPosition = GetMarkerPosition(element);

            var isWithinMap = field.GetRect().HasPoint(markerPosition);

            var knownMarker = newKnownMarkers[element];
            knownMarker.Rotation = element.Rotation + minimapSprite.Rotation;
            knownMarker.Position = new Vector2(
                x: Mathf.Clamp(markerPosition.x, 0, field.RectSize.x),
                y: Mathf.Clamp(markerPosition.y, 0, field.RectSize.y)
            );
            knownMarker.Scale = (isWithinMap) ? minimapSprite.Scale : minimapSprite.Scale * 0.75f;
            knownMarker.Visible = isWithinMap || (minimapElement?.VisibleOnBorder ?? true);
        }

        foreach (var removedMarkers in knownMarkers.Values)
        {
            removedMarkers.QueueFree();
        }
        knownMarkers.Clear();

        var tmpKnownMarkers = newKnownMarkers;
        newKnownMarkers = knownMarkers;
        knownMarkers = tmpKnownMarkers;
    }

    private Vector2 GetMarkerPosition(Node2D element)
    {
        var fieldScale = this.field.RectSize / this.WorldRectSize;
        var elementRelativePosition = element.Position - (centerNode?.Position ?? (this.WorldRectPosition + this.WorldRectSize / 2));
        var markerPosition = elementRelativePosition * fieldScale + this.field.RectSize / 2;
        return markerPosition;
    }

    public void SetMapSizeToNode(Node2D followNode, Vector2 distance)
    {
        this.centerNode = followNode;
        this.WorldRectSize = distance;
        this.WorldRectPosition = Vector2.Zero;
    }

    public void SetMapSizeToNode(Rect2 worldRect)
    {
        this.centerNode = null;
        this.WorldRectSize = worldRect.Size;
        this.WorldRectPosition = worldRect.Position;
    }
}
