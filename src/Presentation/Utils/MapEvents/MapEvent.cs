namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class MapEvent
    {
        public readonly IMapEventAction Action;
        public readonly IMapEventCondition Condition;

        public MapEvent(IMapEventCondition condition, IMapEventAction action){
            Condition = condition;
            Action = action;
        }

        public MapEvent(IMapEventAction action){
            Condition = new TimeoutMapEventCondition(0);
            Action = action;
        }
    }
}