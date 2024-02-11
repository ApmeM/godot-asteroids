using static DodgeTheCreeps.Utils.MazeGeneratorWrapper;

namespace DodgeTheCreeps.Presentation.Utils.Levels
{
    public interface ILevel
    {
        string Name { get; }
        MaseGeneratorWrapperState GenerateField();
    }
}