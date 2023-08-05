using DodgeTheCreeps.Utils;
using Godot;
using Godot.Collections;
using GodotAnalysers;

[SceneReference("Minimap.tscn")]
public partial class Minimap
{
    [Export]
    public NodePath PlayerPath;
    
    private Vector2 fieldScale;
    private Dictionary<Node2D, Node2D> knownMarkers = new Dictionary<Node2D, Node2D>();
    private Dictionary<Node2D, Node2D> newKnownMarkers = new Dictionary<Node2D, Node2D>();

    public override void _Ready()
    {
        base._Ready();
        this.FillMembers();

        this.playerMarker.Position = field.RectSize / 2;

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (PlayerPath == null)
        {
            return;
        }

        var player = (Player)this.GetNode(PlayerPath);
        this.playerMarker.Rotation = player.Rotation;

        var minimapEnemies = this.GetTree().GetNodesInGroup(Groups.MinimapIconEnemy);

        foreach (Node2D enemy in minimapEnemies)
        {
            if (!knownMarkers.ContainsKey(enemy))
            {
                var newEnemyMarker = (Node2D)this.enemyMarker.Duplicate();
                this.field.AddChild(newEnemyMarker);
                newEnemyMarker.Show();
                newKnownMarkers[enemy] = newEnemyMarker;
            }
            else
            {
                newKnownMarkers[enemy] = knownMarkers[enemy];
                knownMarkers.Remove(enemy);
            }

            var marker = newKnownMarkers[enemy];
            marker.Position = (enemy.Position - player.Position) * this.fieldScale + this.field.RectSize / 2;
            if (field.GetRect().HasPoint(marker.Position))
            {
                marker.Scale = Vector2.One;
            }
            else
            {
                marker.Scale = Vector2.One * 0.75f;
            }

            marker.Position = new Vector2(
                x: Mathf.Clamp(marker.Position.x, 0, field.RectSize.x),
                y: Mathf.Clamp(marker.Position.y, 0, field.RectSize.y)
            );
        }

        var minimapBlocks = this.GetTree().GetNodesInGroup(Groups.MinimapIconBlock);
        foreach (Node2D block in minimapBlocks)
        {
            if (!knownMarkers.ContainsKey(block))
            {
                var newBlockMarker = (Node2D)this.blockMarker.Duplicate();
                this.field.AddChild(newBlockMarker);
                newBlockMarker.Show();
                newKnownMarkers[block] = newBlockMarker;
            }
            else
            {
                newKnownMarkers[block] = knownMarkers[block];
                knownMarkers.Remove(block);
            }

            var marker = newKnownMarkers[block];
            marker.Position = (block.Position - player.Position) * this.fieldScale + this.field.RectSize / 2;
            if (field.GetRect().HasPoint(marker.Position))
            {
                marker.Visible = true;
            }
            else
            {
                marker.Visible = false;
            }
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

    public void SetMapSize(Rect2 rect)
    {
        this.fieldScale = this.field.RectSize / rect.Size;
    }
}
