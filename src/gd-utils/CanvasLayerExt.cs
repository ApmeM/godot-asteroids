using Godot;
using System.Linq;

namespace DodgeTheCreeps.Utils
{
    public static class CanvasLayerExt
    {
        public static void Show(this CanvasLayer layer)
        {
            foreach (var item in layer.GetChildren().OfType<Node2D>())
            {
                item.Show();
            }
            foreach (var item in layer.GetChildren().OfType<Control>())
            {
                item.Show();
            }
        }

        public static void Hide(this CanvasLayer layer)
        {
            foreach (var item in layer.GetChildren().OfType<Node2D>())
            {
                item.Hide();
            }
            foreach (var item in layer.GetChildren().OfType<Control>())
            {
                item.Hide();
            }
        }

    }
}
