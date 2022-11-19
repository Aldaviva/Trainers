#nullable enable

using System.Collections.Generic;
using TrainerCommon.Cheats;

namespace TrainerCommon.Games; 

public interface Game {

    string name { get; }
    string processName { get; }
    string supportedVersion { get; }

    IList<Cheat> cheats { get; }

}