using System;
using System.Linq;
using DodgeTheCreeps.Presentation.Utils.MapEvents;
using DodgeTheCreeps.Utils;
using FateRandom;
using MazeGenerators;
using MazeGenerators.Utils;
using static DodgeTheCreeps.Utils.MazeGeneratorWrapper;

namespace DodgeTheCreeps.Presentation.Utils.Levels
{
    public class LevelInfinity : ILevel
    {
        private readonly MaseGeneratorWrapperState State = new MaseGeneratorWrapperState();
        private readonly GeneratorResult generatorResult = new GeneratorResult();
        private readonly GeneratorSettings generatorSettings = new GeneratorSettings();

        public string Name => "Level Infinity";

        public MaseGeneratorWrapperState GenerateField()
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
                    Condition = new TimeoutMapEventCondition(3),
                    Action = new SpawnUnitMapEventAction(new Godot.Vector2(Fate.GlobalFate.NextInt(size - 4) + 2, Fate.GlobalFate.NextInt(size - 4) + 2),
                                                             Fate.GlobalFate.Choose(Enum.GetValues(typeof(UnitType)).Cast<UnitType>().Skip(2).Take(i + 1).ToArray())
                    )
                });

                if (i % 2 == 0)
                {
                    this.State.UnitsList.Add(new MapEvent
                    {
                        Condition = new TimeoutMapEventCondition(0),
                        Action = new SpawnBonusMapEventAction(new Godot.Vector2(Fate.GlobalFate.NextInt(size - 4) + 2, Fate.GlobalFate.NextInt(size - 4) + 2),
                                                                 Fate.GlobalFate.Choose(Enum.GetValues(typeof(BonusType)).Cast<BonusType>().Take(i + 1).ToArray())
                        )
                    });
                }
            }

            var restartAction = new AddMoreMapEventAction();

            this.State.UnitsList.Add(new MapEvent
            {
                Condition = new TimeoutMapEventCondition(0),
                Action = restartAction
            });

            restartAction.Actions.AddRange(this.State.UnitsList);

            return this.State;
        }
    }
}