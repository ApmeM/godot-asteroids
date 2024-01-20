using Godot;

namespace DodgeTheCreeps.Presentation.Utils.GameOver
{
    public class PlanetExistsGameOver : IGameOver
    {
        private Label label;

        public GameOverState CheckGameOver(Game game)
        {
            return game.GetTree().GetNodesInGroup(Groups.PlanetUnit).Count == 0 ? GameOverState.Loose : GameOverState.None;
        }

        public void InitializeStatus(HUD hUD)
        {
            this.label = new Label
            {
                Text = "Planet should survive."
            };
            hUD.AddStatus(label);
        }

        public void UpdateStatus(Game game)
        {
            var planets = game.GetTree().GetNodesInGroup(Groups.PlanetUnit);
            foreach (Planet planet in planets)
            {
                this.label.Text = $"Planet should survive : {planet.CurrentLife} / {planet.TotalLife}";
                var remainingPct = ((float)planet.CurrentLife) / planet.TotalLife;
                if (remainingPct > 0.7)
                {
                    this.label.AddColorOverride("font_color", new Color("00ff00"));
                }
                else if (remainingPct > 0.3)
                {
                    this.label.AddColorOverride("font_color", new Color("ffff00"));
                }
                else
                {
                    this.label.AddColorOverride("font_color", new Color("ff0000"));
                }
            }
        }
    }
}
