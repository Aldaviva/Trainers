#nullable enable

using System.Collections.Generic;
using DakarDesertRallyTrainer.Cheats;
using TrainerCommon.Cheats;
using TrainerCommon.Games;

namespace DakarDesertRallyTrainer.Games;

public class DakarDesertRally: Game {

    public string name { get; } = "Dakar Desert Rally";
    public string processName { get; } = "Dakar2Game-Win64-Shipping";
    public string supportedVersion { get; } = "1.6.0";
    public IList<Cheat> cheats { get; } = new List<Cheat> { new NoSpeedLimitCheat() };

}