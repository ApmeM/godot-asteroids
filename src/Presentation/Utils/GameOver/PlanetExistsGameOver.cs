using System.Collections.Generic;
using Godot;

namespace DodgeTheCreeps.Presentation.Utils.GameOver
{
    public class PlanetExistsGameOver : IGameOver
    {
        public GameOverState CheckGameOver(Game game)
        {
            return game.GetTree().GetNodesInGroup(Groups.EnemyUnit).Count == 0 ? GameOverState.Loose : GameOverState.None;
        }
    }

    public class NoMapEventsGameOver : IGameOver
    {
        public GameOverState CheckGameOver(Game game)
        {
            return game.ActionsLeft == 0 ? GameOverState.Win : GameOverState.None;
        }
    }

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
