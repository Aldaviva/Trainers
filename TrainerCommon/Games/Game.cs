using System.Collections.Generic;
using TrainerCommon.Cheats;

#nullable enable

namespace TrainerCommon.Games {

    public interface Game {

        string name { get; }
        string processName { get; }
        string supportedVersion { get; }

        IList<Cheat> cheats { get; }

    }

}