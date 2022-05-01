using Godot;
using MazeGenerators;
using MazeGenerators.Utils;
using System.Collections.Generic;

namespace DodgeTheCreeps.Utils
{
    public class MazeGeneratorWrapper
    {
        private readonly GeneratorResult generatorResult = new GeneratorResult();
        private readonly GeneratorSettings generatorSettings = new GeneratorSettings();

        public class MaseGeneratorWrapperState
        {
            public int[,] Map;
            public readonly Dictionary<Godot.Vector2, UnitType> UnitsList = new Dictionary<Godot.Vector2, UnitType>();
            public Godot.Vector2 StartPosition;
        }

        public readonly MaseGeneratorWrapperState State = new MaseGeneratorWrapperState();

        public static readonly MazeGeneratorWrapper DefaultInstance = new MazeGeneratorWrapper();

        public void GenerateLevel1()
        {
            generatorSettings.Height = 11;
            generatorSettings.Width = 11;
            generatorSettings.MazeText =
            "###########\n" +
            "#.........#\n" +
            "#.........#\n" +
            "#.........#\n" +
            "#...###...#\n" +
            "#...###...#\n" +
            "#...###...#\n" +
            "#.........#\n" +
            "#.........#\n" +
            "#.........#\n" +
            "###########\n";

            CommonAlgorithm.GenerateField(generatorResult, generatorSettings);
            StringParserAlgorithm.Parse(generatorResult, generatorSettings);

            this.State.Map = generatorResult.Paths;
            this.State.StartPosition = new Godot.Vector2(5, 2);
            this.State.UnitsList.Clear();
            this.State.UnitsList[new Godot.Vector2(5, 8)] = UnitType.LargeMeteor;
            this.State.UnitsList[new Godot.Vector2(2, 8)] = UnitType.LargeMeteor;
            this.State.UnitsList[new Godot.Vector2(8, 8)] = UnitType.LargeMeteor;
        }
    }
}
