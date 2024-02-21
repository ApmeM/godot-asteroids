using System.Collections.Generic;
using DodgeTheCreeps.Presentation.Utils.GameOver;
using DodgeTheCreeps.Presentation.Utils.MapEvents;
using DodgeTheCreeps.Utils;
using FateRandom;

namespace DodgeTheCreeps.Presentation.Utils.Levels
{
    public class Level2 : ILevel
    {
        private readonly List<MapEvent> State = new List<MapEvent>();
        public string Name => "Level 2";

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
                        this.State.Add(new MapEvent(new BuildBlockMapEventAction(new Godot.Vector2(x, y))));
                    }
                }
            }

            this.State.Add(new MapEvent(new TeleportPlayerInMapEventAction(size, new Godot.Vector2(3, 3))));
            this.State.Add(new MapEvent(new ShowDialogMapEventAction("На этих метеоритах можно заработать кучу денег...")));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ShowDialogMapEventAction("Пилота №123 просим срочно вернуться к земле. \nПовторяю: срочное распоряжение, всем пилотам вернуться к земле.")));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new TeleportPlayerOutMapEventAction(new Godot.Vector2(-1, -1))));
            this.State.Add(new MapEvent(new PlayerExistsMapEventCondition(), new SpawnUnitMapEventAction(new Godot.Vector2(1, 1), UnitType.Planet)));
            this.State.Add(new MapEvent(new TeleportPlayerInMapEventAction(size, new Godot.Vector2(3, 3))));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ShowDialogMapEventAction("Ты вовремя. На землю летит куча метеоритов неизвестного происхождения. Они могут уничтожить землю!")));
            this.State.Add(new MapEvent(new ChangeGameOverMapEventAction(
                    new CombinedGameOver(
                        new List<IGameOver> { new NoMapEventsGameOver() },
                        new List<IGameOver> { new PlanetExistsGameOver(), new PlayerExistsGameOver() }))
            ));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ShowDialogMapEventAction("Оборона земли может принять на себя некоторое количество метеоритов, но их слишком много. Как лучший пилот планеты ты должен защитить нас - затолкай все метеориты в портал.")));
            this.State.Add(new MapEvent(new SpawnUnitMapEventAction(new Godot.Vector2(size - 2, size - 2), UnitType.BlackHole)));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ShowDialogMapEventAction("А где остальные пилоты?", false)));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ShowDialogMapEventAction("Оу, тебе не сказали. Все пилоты это ты, потому и лучший. Не отвлекайся, они приближаются.")));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new DoNothingMapEventAction()));

            for (var i = 0; i < 10; i++)
            {
                this.State.Add(new MapEvent(new TimeoutMapEventCondition(1), new SpawnUnitMapEventAction(new Godot.Vector2(Fate.GlobalFate.NextInt(size - 4) + 2, Fate.GlobalFate.NextInt(size - 4) + 2), UnitType.Meteor)));
            }

            this.State.Add(new MapEvent(new NoEnemyUnitsCondition(), new DoNothingMapEventAction()));
            return this.State;
        }
    }
}