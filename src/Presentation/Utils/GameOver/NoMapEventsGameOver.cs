namespace DodgeTheCreeps.Presentation.Utils.GameOver
{
    public class NoMapEventsGameOver : IGameOver
    {
        public GameOverState CheckGameOver(Game game)
        {
            return game.MapEventsLeft == 0 ? GameOverState.Win : GameOverState.None;
        }
    }
}
