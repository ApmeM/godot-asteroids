using Godot;

namespace DodgeTheCreeps.Utils
{
    public static class BugFixExt
    {
        // Godot HTML5 export bug: https://github.com/godotengine/godot/issues/58168
        public static Vector2 InputGetVector(string negativeX, string positiveX, string negativeY, string positiveY, float deadzone = 0.5f)
        {
            var strength = new Vector2(
                Input.GetActionStrength(positiveX) - Input.GetActionStrength(negativeX),
                Input.GetActionStrength(positiveY) - Input.GetActionStrength(negativeY)
            ).Normalized();
            return strength.Length() > deadzone ? strength : Vector2.Zero;
        }
    }
}
