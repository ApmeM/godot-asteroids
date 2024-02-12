using System.Collections.Generic;
using DodgeTheCreeps.Presentation.Utils.GameOver;
using DodgeTheCreeps.Presentation.Utils.MapEvents;
using DodgeTheCreeps.Utils;
using FateRandom;

namespace DodgeTheCreeps.Presentation.Utils.Levels
{
    public class Level1 : ILevel
    {
        private readonly List<MapEvent> State = new List<MapEvent>();
        public string Name => "Level 1";

        public List<MapEvent> GenerateField()
        {
            this.State.Clear();

            const int size = 9;

            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    if (x == 0 || y == 0 || x == size - 1 || y == size - 1)
                    {
                        this.State.Add(new MapEvent
                        {
                            Condition = new TimeoutMapEventCondition(0),
                            Action = new BuildBlockMapEventAction(new Godot.Vector2(x, y))
                        });
                    }
                }
            }

            this.State.Add(new MapEvent
            {
                Condition = new TimeoutMapEventCondition(0),
                Action = new TeleportPlayerInMapEventAction(size, new Godot.Vector2(3, 3))
            });
            this.State.Add(new MapEvent
            {
                Condition = new TimeoutMapEventCondition(0.1f),
                Action = new ChangeGameOverMapEventAction(
                    new CombinedGameOver(
                        new List<IGameOver> { new NoMapEventsGameOver() },
                        new List<IGameOver> { new PlayerExistsGameOver() }))
            });
            this.State.Add(new MapEvent
            {
                Condition = new TimeoutMapEventCondition(0.3f),
                Action = new ShowDialogMapEventAction("Добро пожаловать Пилот №123! \nВ этом секторе космоса летает множество метеоритов, богатых полезными ископаемыми.")
            });

            this.State.Add(new MapEvent
            {
                Condition = new TimeoutMapEventCondition(0),
                Action = new SpawnUnitMapEventAction(new Godot.Vector2(size - 2, 0), UnitType.BlackHole)
            });
            this.State.Add(new MapEvent
            {
                Condition = new DialogVisibleMapEventCondition(),
                Action = new ShowDialogMapEventAction("В твои задачи входит затолкать все пролетающие мимо метеориты в перерабатывающий портал. Будь осторожен и не свались туда сам.")
            });
            this.State.Add(new MapEvent
            {
                Condition = new DialogVisibleMapEventCondition(),
                Action = new ShowDialogMapEventAction("Ты спросишь где же все это множество метеоритов. Очевидно они ждут, когда мы закончим болтать.")
            });
            this.State.Add(new MapEvent
            {
                Condition = new DialogVisibleMapEventCondition(),
                Action = new DoNothingMapEventAction()
            });

            for (var i = 0; i < 10; i++)
            {
                this.State.Add(new MapEvent
                {
                    Condition = new TimeoutMapEventCondition(1),
                    Action = new SpawnUnitMapEventAction(new Godot.Vector2(Fate.GlobalFate.NextInt(size - 4) + 2, Fate.GlobalFate.NextInt(size - 4) + 2),
                                                             UnitType.Meteor
                    )
                });
            }

            this.State.Add(new MapEvent
            {
                Condition = new NoEnemyUnitsCondition(),
                Action = new DoNothingMapEventAction()
            });
            return this.State;
        }
    }
}