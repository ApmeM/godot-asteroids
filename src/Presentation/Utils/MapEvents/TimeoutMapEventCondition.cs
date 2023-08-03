namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class TimeoutMapEventCondition : IMapEventCondition
    {
        public TimeoutMapEventCondition(float spawnTime)
        {
            SpawnTime = spawnTime;
        }

        public float SpawnTime;

        public bool IsReady(double progress)
        {
            return this.SpawnTime < progress;
        }
    }
}