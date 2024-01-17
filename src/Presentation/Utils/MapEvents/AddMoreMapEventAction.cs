using System.Collections.Generic;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class AddMoreMapEventAction : IMapEventAction
    {
        public readonly List<MapEvent> Actions = new List<MapEvent>();

        public void Action(Game game)
        {
            game.AddMapEvents(this.Actions);
        }
    }
}