using DodgeTheCreeps.Presentation.Utils.Levels;
using System.Collections.Generic;

namespace DodgeTheCreeps.Utils
{
    public class MazeGeneratorWrapper
    {
        public List<ILevel> Levels = new List<ILevel>{
            new Level1(),
            new Level2(),
            new LevelInfinity()
        };

        public static readonly MazeGeneratorWrapper DefaultInstance = new MazeGeneratorWrapper();
    }
}
