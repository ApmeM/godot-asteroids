
namespace DodgeTheCreeps.Presentation.Utils.MapEvents
{
    public class ShowDialogMapEventAction : IMapEventAction
    {
        private readonly string text;
        private readonly bool left;

        public ShowDialogMapEventAction(string text, bool left = true)
        {
            this.text = text;
            this.left = left;
        }
        
        public void Action(Game game)
        {
            game.ShowDialog(text, left);
        }
    }
}