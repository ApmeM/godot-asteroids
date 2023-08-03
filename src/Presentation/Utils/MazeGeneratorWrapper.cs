using DodgeTheCreeps.Presentation.Utils.MapEvents;
using FateRandom;
using MazeGenerators;
using MazeGenerators.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DodgeTheCreeps.Utils
{
    public class MazeGeneratorWrapper
    {
        public class MaseGeneratorWrapperState
        {
            public int[,] Map;
            public readonly List<MapEvent> UnitsList = new List<MapEvent>();
            public Godot.Vector2 StartPosition;
        }

        private readonly GeneratorResult generatorResult = new GeneratorResult();
        private readonly GeneratorSettings generatorSettings = new GeneratorSettings();
        private readonly MaseGeneratorWrapperState State = new MaseGeneratorWrapperState();
        public static readonly MazeGeneratorWrapper DefaultInstance = new MazeGeneratorWrapper();

        public MaseGeneratorWrapperState GenerateLevel1()
        {
            const int size = 17;

            generatorSettings.Height = size;
            generatorSettings.Width = size;
            generatorSettings.MazeText = "";
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    if (x == 0 || y == 0 || x == size - 1 || y == size - 1)
                        generatorSettings.MazeText += "#";
                    else
                        generatorSettings.MazeText += ".";
                }
                generatorSettings.MazeText += "\n";
            }

            CommonAlgorithm.GenerateField(generatorResult, generatorSettings);
            StringParserAlgorithm.Parse(generatorResult, generatorSettings);

            this.State.Map = generatorResult.Paths;
            this.State.StartPosition = new Godot.Vector2(3, 3);
            this.State.UnitsList.Clear();
            for (var i = 0; i < 30; i++)
            {
                this.State.UnitsList.Add(new MapEvent
                {
                    Condition = new TimeoutMapEventCondition(3 * i),
                    Action = new SpawnUnitMapEventAction(new Godot.Vector2(Fate.GlobalFate.NextInt(size - 4) + 2, Fate.GlobalFate.NextInt(size - 4) + 2),
                                                             Fate.GlobalFate.Choose(Enum.GetValues(typeof(UnitType)).Cast<UnitType>().Take(i + 1).ToArray())
                    )
                });
            }

            return this.State;
        }
    }
}
