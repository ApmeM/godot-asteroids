using System.Collections.Generic;
using Godot;
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

            const int size = 20;

            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    if (x == 0 || y == 0 || x == size - 1 || y == size - 1)
                    {
                        this.State.Add(new MapEvent(new SpawnUnitMapEventAction(new Vector2(x * 100, y * 100), UnitType.Block)));
                    }
                }
            }

            this.State.Add(new MapEvent(new TeleportPlayerInMapEventAction(new Rect2(0, 0, size * 100, size * 100), new Vector2(300, 300))));
            this.State.Add(new MapEvent(new ChangeGameOverMapEventAction(
                    new CombinedGameOver(
                        new List<IGameOver> { new NoMapEventsGameOver() },
                        new List<IGameOver> { new PlayerExistsGameOver() }))
            ));
            this.State.Add(new MapEvent(new ShowDialogMapEventAction("Добро пожаловать Пилот №123! \nВ этом секторе космоса летает множество метеоритов, богатых полезными ископаемыми.")));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ShowDialogMapEventAction("В твои задачи входит затолкать все пролетающие мимо метеориты в перерабатывающий портал. Будь осторожен и не свались туда сам.")));
            this.State.Add(new MapEvent(new SpawnUnitMapEventAction(new Vector2(size * 100 - 200, 0), UnitType.BlackHole)));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ShowDialogMapEventAction("А где все это 'множество' метеоритов? Ни одного не вижу.", false)));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ShowDialogMapEventAction("Как первый раз в подобной игре...  Очевидно они ждут, когда мы закончим болтать.")));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ShowDialogMapEventAction("Какой игре???", false)));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new DoNothingMapEventAction()));

            for (var i = 0; i < 10; i++)
            {
                this.State.Add(new MapEvent(new TimeoutMapEventCondition(1), new SpawnUnitMapEventAction(new Vector2(Fate.GlobalFate.NextInt(size * 100 - 400) + 200, Fate.GlobalFate.NextInt(size * 100 - 400) + 200), UnitType.Meteor)));
            }

            this.State.Add(new MapEvent(new NoEnemyUnitsCondition(), new DoNothingMapEventAction()));

            return this.State;
        }
    }
}