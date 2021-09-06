using System.Collections.Generic;
using Dakar18Trainer.Cheats;
using TrainerCommon.Cheats;
using TrainerCommon.Games;

#nullable enable

namespace Dakar18Trainer.Games {

    public class Dakar18: Game {

        public string name { get; } = "Dakar 18";
        public string processName { get; } = "Dakar18Game-Win64-Shipping";
        public string supportedVersion { get; } = "v.13";
        public IList<Cheat> cheats { get; } = new List<Cheat> { new NoSpeedLimitCheat() };

    }

}