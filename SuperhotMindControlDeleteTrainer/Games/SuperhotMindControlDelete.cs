#nullable enable

using System.Collections.Generic;
using SuperhotMindControlDeleteTrainer.Cheats;
using TrainerCommon.Cheats;
using TrainerCommon.Games;

namespace SuperhotMindControlDeleteTrainer.Games; 

public class SuperhotMindControlDelete: Game {

    public string name { get; } = "Superhot: Mind Control Delete";
    public string processName { get; } = "SHMCD";
    public string supportedVersion { get; } = "1.0.0";
    public IList<Cheat> cheats { get; } = new List<Cheat> { new InfiniteHealthCheat() };

}