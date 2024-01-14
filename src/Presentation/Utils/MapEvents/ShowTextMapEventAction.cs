
namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class ShowDialogMapEventAction : IMapEventAction
    {
        private readonly string text;

        public ShowDialogMapEventAction(string text)
        {
            this.text = text;
        }
        
        public void Action(Game game)
        {
            game.ShowDialog(text);
        }
    }
}