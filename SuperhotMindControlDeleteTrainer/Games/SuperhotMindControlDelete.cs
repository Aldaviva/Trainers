#nullable enable

using SuperhotMindControlDeleteTrainer.Cheats;
using System.Collections.Generic;
using System.Diagnostics;
using TrainerCommon.Cheats;
using TrainerCommon.Games;

namespace SuperhotMindControlDeleteTrainer.Games;

public class SuperhotMindControlDelete: BaseGame {

    public override string name { get; } = "Superhot: Mind Control Delete";
    public override string processName { get; } = "SHMCD";
    public override string supportedVersion { get; } = "1.0.0";
    public override ICollection<Cheat> cheats { get; } = new List<Cheat> { new InfiniteHealthCheat() };

    public override string? getVersion(Process gameProcess) => "1.0.0";

}