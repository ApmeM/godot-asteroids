namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class SpawnTimeMapEventCondition : IMapEventCondition
    {
        public SpawnTimeMapEventCondition(float spawnTime)
        {
            SpawnTime = spawnTime;
        }

        public float SpawnTime;

        public bool IsReady(Game game)
        {
            return this.SpawnTime < game.GameTime;
        }
    }
}