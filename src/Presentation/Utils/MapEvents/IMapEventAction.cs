using Godot;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public interface IMapEventAction
    {
        void Action(Game game);
    }
}