
namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{

    public class GameOverMapEventAction : IMapEventAction
    {
        public GameOverMapEventAction()
        {
        }
        
        public void Action(Game game)
        {
            game.FinishGame();
        }
    }
}