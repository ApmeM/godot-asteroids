using Godot;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class DialogVisibleMapEventCondition : IMapEventCondition
    {
        public DialogVisibleMapEventCondition()
        {
        }

        public bool IsReady(Game game)
        {
            return !game.DialogVisible;
        }
    }
}