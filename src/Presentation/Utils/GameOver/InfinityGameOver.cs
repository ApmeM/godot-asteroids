namespace DodgeTheCreeps.Presentation.Utils.GameOver
{
    public class InfinityGameOver : IGameOver
    {
        public GameOverState CheckGameOver(Game game)
        {
            return GameOverState.None;
        }

        public void InitializeStatus(HUD hUD)
        {
        }

        public void UpdateStatus(Game game)
        {
        }
    }
}
