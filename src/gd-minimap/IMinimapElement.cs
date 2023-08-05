using Godot;

public interface IMinimapElement
{
    // If true - minimap element will be visible even if it is out of the minimap rectangle.
    // Its size will be slightly reduced, and it will be sticked to the minimap border.
    // If false - element will not be visible outside of a minimap area.
    bool VisibleOnBorder { get; }

    // Minimap texture element
    Sprite Sprite { get; }
}