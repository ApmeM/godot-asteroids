using System.Collections.Generic;
using DodgeTheCreeps.Presentation.Utils.GameOver;
using DodgeTheCreeps.Presentation.Utils.MapEvents;
using DodgeTheCreeps.Utils;
using FateRandom;
using Godot;

namespace DodgeTheCreeps.Presentation.Utils.Levels
{
    public class Level2 : ILevel
    {
        private readonly List<MapEvent> State = new List<MapEvent>();
        public string Name => "Level 2";

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
            this.State.Add(new MapEvent(new ShowDialogMapEventAction("На этих метеоритах можно заработать кучу денег...")));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ShowDialogMapEventAction("Пилота №123 просим срочно вернуться к земле. \nПовторяю: срочное распоряжение, всем пилотам вернуться к земле.")));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new TeleportPlayerOutMapEventAction(new Vector2(-1, -1))));
            this.State.Add(new MapEvent(new NoPlayerMapEventCondition(), new ShowDialogMapEventAction("Вжух.")));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new SpawnUnitMapEventAction(new Vector2(200, 200), UnitType.Planet)));
            this.State.Add(new MapEvent(new TeleportPlayerInMapEventAction(new Rect2(0, 0, size * 100, size * 100), new Vector2(300, 300))));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ShowDialogMapEventAction("Ты вовремя. Сюда летит куча метеоритов неизвестного происхождения. Они могут уничтожить землю!")));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ChangeGameOverMapEventAction(
                    new CombinedGameOver(
                        new List<IGameOver> { new NoMapEventsGameOver() },
                        new List<IGameOver> { new PlanetExistsGameOver(), new PlayerExistsGameOver() }))
            ));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ShowDialogMapEventAction("Оборона земли может принять на себя некоторое количество метеоритов, но их слишком много. Как лучший пилот планеты ты должен защитить нас - затолкай все метеориты в портал.")));
            this.State.Add(new MapEvent(new SpawnUnitMapEventAction(new Vector2(size * 100 - 200, size * 100 - 200), UnitType.BlackHole)));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ShowDialogMapEventAction("А где остальные пилоты?", false)));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new ShowDialogMapEventAction("Оу, тебе не сказали. Все пилоты это ты, потому и лучший. Не отвлекайся, они приближаются.")));
            this.State.Add(new MapEvent(new DialogVisibleMapEventCondition(), new DoNothingMapEventAction()));

            for (var i = 0; i < 10; i++)
            {
                this.State.Add(new MapEvent(new TimeoutMapEventCondition(1), new SpawnUnitMapEventAction(new Vector2(Fate.GlobalFate.NextInt(size * 100 - 500) + 400, Fate.GlobalFate.NextInt(size * 100 - 500) + 400), UnitType.Meteor)));
            }

            this.State.Add(new MapEvent(new NoEnemyUnitsCondition(), new DoNothingMapEventAction()));
            return this.State;
        }

        public int GetScore(Game game)
        {
            var cur = ((Planet)game.GetTree().GetNodesInGroup(Groups.PlanetUnit)[0]).CurrentLife;
            var max = ((Planet)game.GetTree().GetNodesInGroup(Groups.PlanetUnit)[0]).TotalLife;
            GD.Print($"{cur}/{max}");
            if (cur == max) return 3;
            if (cur > max / 2) return 2;
            return 1;
        }
    }
}