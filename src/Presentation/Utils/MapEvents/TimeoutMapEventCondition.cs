namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class TimeoutMapEventCondition : IMapEventCondition
    {
        public TimeoutMapEventCondition(float timeout)
        {
            Timeout = timeout;
        }

        public float Timeout;
        private float StartTime = float.MinValue;

        public bool IsReady(Game game)
        {
            if (StartTime == float.MinValue)
            {
                StartTime = game.GameTime;
            }

            var isReady = this.Timeout <= game.GameTime - StartTime;

            if (isReady)
            {
                this.StartTime = float.MinValue;
            }

            return isReady;
        }
    }
}