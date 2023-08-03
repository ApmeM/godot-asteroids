using Godot;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public interface IMapEventAction
    {
        void Action(Vector2 playerPosition, int pathSize, Node toAdd);
    }
}