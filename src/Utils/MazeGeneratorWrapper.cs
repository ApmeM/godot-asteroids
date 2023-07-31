using MazeGenerators;
using MazeGenerators.Utils;
using System.Collections.Generic;

namespace DodgeTheCreeps.Utils
{
    public class MazeGeneratorWrapper
    {
        public class Unit
        {
            public Godot.Vector2 Position;
            public UnitType UnitType;
            public float SpawnTime;
        }

        private readonly GeneratorResult generatorResult = new GeneratorResult();
        private readonly GeneratorSettings generatorSettings = new GeneratorSettings();

        public class MaseGeneratorWrapperState
        {
            public int[,] Map;
            public readonly List<Unit> UnitsList = new List<Unit>();
            public Godot.Vector2 StartPosition;
        }

        private readonly MaseGeneratorWrapperState State = new MaseGeneratorWrapperState();

        public static readonly MazeGeneratorWrapper DefaultInstance = new MazeGeneratorWrapper();

        public MaseGeneratorWrapperState GenerateLevel1()
        {
            generatorSettings.Height = 21;
            generatorSettings.Width = 27;
            generatorSettings.MazeText =
            "###########################\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "#.........................#\n" +
            "###########################\n";

            CommonAlgorithm.GenerateField(generatorResult, generatorSettings);
            StringParserAlgorithm.Parse(generatorResult, generatorSettings);

            this.State.Map = generatorResult.Paths;
            this.State.StartPosition = new Godot.Vector2(3, 3);
            this.State.UnitsList.Clear();
            this.State.UnitsList.Add(new Unit { Position = new Godot.Vector2(23, 1), UnitType = UnitType.LargeMeteor, SpawnTime = 0 });
            this.State.UnitsList.Add(new Unit { Position = new Godot.Vector2(1, 17), UnitType = UnitType.LargeMeteor, SpawnTime = 0 });
            this.State.UnitsList.Add(new Unit { Position = new Godot.Vector2(23, 17), UnitType = UnitType.BlackHole, SpawnTime = 10 });
            
            this.State.UnitsList.Add(new Unit { Position = new Godot.Vector2(17, 10), UnitType = UnitType.BlackHole, SpawnTime = 40 });
            this.State.UnitsList.Add(new Unit { Position = new Godot.Vector2(15, 10), UnitType = UnitType.BlackHole, SpawnTime = 40 });
            this.State.UnitsList.Add(new Unit { Position = new Godot.Vector2(15, 12), UnitType = UnitType.BlackHole, SpawnTime = 40 });

            return this.State;
        }
    }
}
