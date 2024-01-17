using System.Collections.Generic;
using DodgeTheCreeps.Presentation.Utils.GameOver;

namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class ChangeGameOverMapEventAction : IMapEventAction
    {
        private IGameOver gameOver;

        public ChangeGameOverMapEventAction(IGameOver gameOver)
        {
            this.gameOver = gameOver;
        }

        public void Action(Game game)
        {
            game.GameOverStatus = this.gameOver;
        }
    }
}