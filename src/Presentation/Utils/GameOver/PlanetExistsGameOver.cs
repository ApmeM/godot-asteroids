using Godot;

namespace DodgeTheCreeps.Presentation.Utils.GameOver
{
    public class PlanetExistsGameOver : IGameOver
    {
        public GameOverState CheckGameOver(Game game)
        {
            return game.GetTree().GetNodesInGroup(Groups.PlanetUnit).Count == 0 ? GameOverState.Loose : GameOverState.None;
        }
    }
}
