using Godot;

namespace GodotRts.Presentation.Utils
{
    public static class NodeExt
    {
        public static void ClearChildren(this Node node)
        {
            while(node.GetChildCount() > 0){
                var child = node.GetChild(0);
                child.QueueFree();
                node.RemoveChild(child);
            }
        }
    }
}
