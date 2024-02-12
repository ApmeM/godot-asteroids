using System;
using System.Collections.Generic;
using System.Linq;
using DodgeTheCreeps.Presentation.Utils.MapEvents;
using DodgeTheCreeps.Utils;
using FateRandom;

namespace DodgeTheCreeps.Presentation.Utils.Levels
{
    public class LevelInfinity : ILevel
    {
        private readonly List<MapEvent> State = new List<MapEvent>();

        public string Name => "Level Infinity";

        public List<MapEvent> GenerateField()
        {
            this.State.Clear();

            const int size = 17;

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

            var lvl = new List<MapEvent>();

            for (var i = 0; i < 30; i++)
            {
                lvl.Add(new MapEvent(new TimeoutMapEventCondition(3), new SpawnUnitMapEventAction(new Godot.Vector2(Fate.GlobalFate.NextInt(size - 4) + 2, Fate.GlobalFate.NextInt(size - 4) + 2),
                                                             Fate.GlobalFate.Choose(Enum.GetValues(typeof(UnitType)).Cast<UnitType>().Skip(2).Take(i + 1).ToArray()))));

                if (i % 2 == 0)
                {
                    lvl.Add(new MapEvent(new SpawnBonusMapEventAction(new Godot.Vector2(Fate.GlobalFate.NextInt(size - 4) + 2, Fate.GlobalFate.NextInt(size - 4) + 2),
                                                                 Fate.GlobalFate.Choose(Enum.GetValues(typeof(BonusType)).Cast<BonusType>().Take(i + 1).ToArray()))));
                }
            }

            var restartAction = new AddMoreMapEventAction();
            lvl.Add(new MapEvent(restartAction));
            restartAction.Actions.AddRange(lvl);

            this.State.Add(new MapEvent(restartAction));

            return this.State;
        }
    }
}