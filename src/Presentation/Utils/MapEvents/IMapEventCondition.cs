namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public interface IMapEventCondition
    {
        bool IsReady(double progress);
    }
}