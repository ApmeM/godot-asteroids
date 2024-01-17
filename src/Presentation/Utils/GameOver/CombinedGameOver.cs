using System.Collections.Generic;

namespace DodgeTheCreeps.Presentation.Utils.GameOver
{
    public class CombinedGameOver : IGameOver
    {
        private readonly List<IGameOver> winConditions;
        private readonly List<IGameOver> looseConditions;

        public CombinedGameOver(List<IGameOver> winConditions, List<IGameOver> looseConditions)
        {
            this.winConditions = winConditions;
            this.looseConditions = looseConditions;
        }

        public GameOverState CheckGameOver(Game game)
        {
            foreach (var over in looseConditions)
            {
                if (over.CheckGameOver(game) == GameOverState.Loose)
                {
                    return GameOverState.Loose;
                }
            }

            foreach (var over in winConditions)
            {
                if (over.CheckGameOver(game) == GameOverState.Win)
                {
                    return GameOverState.Win;
                }
            }

            return GameOverState.None;
        }
    }
}
