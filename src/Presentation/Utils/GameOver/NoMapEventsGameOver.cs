using Godot;

namespace DodgeTheCreeps.Presentation.Utils.GameOver
{
    public class NoMapEventsGameOver : IGameOver
    {
        private HBoxContainer container;
        private Label label;
        private TextureProgress progress;

        public GameOverState CheckGameOver(Game game)
        {
            return game.MapEventsLeft == 0 ? GameOverState.Win : GameOverState.None;
        }

        public void InitializeStatus(HUD hUD)
        {
            this.container = new HBoxContainer();
            this.label = new Label { Text = "Clear the area." };
            this.container.AddChild(this.label);
            this.progress = new TextureProgress { TextureProgress_ = ResourceLoader.Load<Texture>("res://art/ships/ships.laserRed06.tres") };
            this.progress.FillMode = (int)TextureProgress.FillModeEnum.BottomToTop;
            this.container.AddChild(this.progress);
            hUD.AddStatus(container);
        }

        public void UpdateStatus(Game game)
        {
            this.label.Text = $"Clear the area : {game.MapEventsTotal - game.MapEventsLeft} / {game.MapEventsTotal}";
            this.progress.Value = game.MapEventsTotal - game.MapEventsLeft;
            this.progress.MaxValue = game.MapEventsTotal;
        }
    }
}
