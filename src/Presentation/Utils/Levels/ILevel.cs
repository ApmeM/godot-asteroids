using System.Collections.Generic;
using DodgeTheCreeps.Presentation.Utils.MapEvents;

namespace DodgeTheCreeps.Presentation.Utils.Levels
{
    public interface ILevel
    {
        string Name { get; }
        List<MapEvent> GenerateField();
    }
}