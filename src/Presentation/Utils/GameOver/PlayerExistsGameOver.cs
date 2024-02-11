using Godot;

namespace DodgeTheCreeps.Presentation.Utils.GameOver
{
    public class PlayerExistsGameOver : IGameOver
    {
        private Label label;

        public GameOverState CheckGameOver(Game game)
        {
            return game.GetTree().GetNodesInGroup(Groups.PlayerUnit).Count == 0 ? GameOverState.Loose : GameOverState.None;
        }

        public void InitializeStatus(HUD hUD)
        {
            this.label = new Label
            {
                Text = "Your spaceship should survive."
            };
            hUD.AddStatus(label);
        }

        public void UpdateStatus(Game game)
        {
        }
    }
}
