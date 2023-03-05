#nullable enable

using System.Collections.Generic;
using Dakar18Trainer.Cheats;
using TrainerCommon.Cheats;
using TrainerCommon.Games;

namespace Dakar18Trainer.Games;

public class Dakar18: BaseGame {

    public override string name { get; } = "Dakar 18";
    public override string processName { get; } = "Dakar18Game-Win64-Shipping";
    public override string supportedVersion { get; } = "v.13";
    public override ICollection<Cheat> cheats { get; } = new List<Cheat> { new NoSpeedLimitCheat() };

    public override string getVersion(string executableSha256Hash) => "v.13";

}