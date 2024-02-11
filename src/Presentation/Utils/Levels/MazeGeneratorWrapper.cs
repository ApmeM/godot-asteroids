using DodgeTheCreeps.Presentation.Utils.Levels;
using DodgeTheCreeps.Presentation.Utils.MapEvents;
using System.Collections.Generic;

namespace DodgeTheCreeps.Utils
{
    public class MazeGeneratorWrapper
    {
        public List<ILevel> Levels = new List<ILevel>{
            new Level1(),
            new LevelInfinity()
        };

        public class MaseGeneratorWrapperState
        {
            public int[,] Map;
            public readonly List<MapEvent> UnitsList = new List<MapEvent>();
            public Godot.Vector2 StartPosition;
        }

        public static readonly MazeGeneratorWrapper DefaultInstance = new MazeGeneratorWrapper();

        public MaseGeneratorWrapperState GenerateLevel(int gameId)
        {
            return Levels[gameId].GenerateField();
        }
    }
}
