using Godot;

namespace DodgeTheCreeps.Presentation.Utils.GameOver
{

    public interface IGameOver
    {
        GameOverState CheckGameOver(Game game);
        void InitializeStatus(HUD hUD);
        void UpdateStatus(Game game);
    }
}
