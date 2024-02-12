using System.Collections.Generic;
using DodgeTheCreeps.Presentation.Utils.GameOver;
using DodgeTheCreeps.Presentation.Utils.MapEvents;
using DodgeTheCreeps.Utils;
using FateRandom;
using MazeGenerators;
using MazeGenerators.Utils;
using static DodgeTheCreeps.Utils.MazeGeneratorWrapper;

namespace DodgeTheCreeps.Presentation.Utils.Levels
{
    public class Level2 : ILevel
    {
        private readonly MaseGeneratorWrapperState State = new MaseGeneratorWrapperState();
        private readonly GeneratorResult generatorResult = new GeneratorResult();
        private readonly GeneratorSettings generatorSettings = new GeneratorSettings();

        public string Name => "Level 2";

        public MaseGeneratorWrapperState GenerateField()
        {
            
            const int size = 9;

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
            this.State.UnitsList.Add(new MapEvent
            {
                Condition = new TimeoutMapEventCondition(0.3f),
                Action = new ShowDialogMapEventAction("На этих метеоритах можно заработать кучу денег...")
            });
            this.State.UnitsList.Add(new MapEvent
            {
                Condition = new DialogVisibleMapEventCondition(),
                Action = new ShowDialogMapEventAction("Пилота №123 просим срочно вернуться к земле. \nПовторяю: срочное распоряжение, всем пилотам вернуться к земле.")
            });
            this.State.UnitsList.Add(new MapEvent
            {
                Condition = new DialogVisibleMapEventCondition(),
                Action = new SpawnUnitMapEventAction(new Godot.Vector2(1, 1), UnitType.Planet)
            });
            this.State.UnitsList.Add(new MapEvent
            {
                Condition = new DialogVisibleMapEventCondition(),
                Action = new ShowDialogMapEventAction("Ты вовремя, на землю летит куча метеоритов неизвестного происхождения. Они могут уничтожить землю!")
            });
            this.State.UnitsList.Add(new MapEvent
            {
                Condition = new TimeoutMapEventCondition(0),
                Action = new ChangeGameOverMapEventAction(
                    new CombinedGameOver(
                        new List<IGameOver> { new NoMapEventsGameOver() },
                        new List<IGameOver> { new PlanetExistsGameOver(), new PlayerExistsGameOver() }))
            });
            this.State.UnitsList.Add(new MapEvent
            {
                Condition = new DialogVisibleMapEventCondition(),
                Action = new SpawnUnitMapEventAction(new Godot.Vector2(size - 2, size - 2), UnitType.BlackHole)
            });
            this.State.UnitsList.Add(new MapEvent
            {
                Condition = new DialogVisibleMapEventCondition(),
                Action = new ShowDialogMapEventAction("Оборона земли может принять на себя некоторое количество метеоритов, но их слишком много. Как лучший пилот планеты ты должен защитить нас - затолкай все метеориты в портал.")
            });
            this.State.UnitsList.Add(new MapEvent
            {
                Condition = new DialogVisibleMapEventCondition(),
                Action = new ShowDialogMapEventAction("А где остальные пилоты?")
            });
            this.State.UnitsList.Add(new MapEvent
            {
                Condition = new DialogVisibleMapEventCondition(),
                Action = new ShowDialogMapEventAction("Оу, тебе не сказали. Все пилоты это ты, потому и лучший. Не отвлекайся, они приближаются.")
            });
            this.State.UnitsList.Add(new MapEvent
            {
                Condition = new DialogVisibleMapEventCondition(),
                Action = new DoNothingMapEventAction()
            });

            for (var i = 0; i < 10; i++)
            {
                this.State.UnitsList.Add(new MapEvent
                {
                    Condition = new TimeoutMapEventCondition(1),
                    Action = new SpawnUnitMapEventAction(new Godot.Vector2(Fate.GlobalFate.NextInt(size - 4) + 2, Fate.GlobalFate.NextInt(size - 4) + 2),
                                                             UnitType.Meteor
                    )
                });
            }

            this.State.UnitsList.Add(new MapEvent
            {
                Condition = new NoEnemyUnitsCondition(),
                Action = new DoNothingMapEventAction()
            });
            return this.State;
        }
    }
}