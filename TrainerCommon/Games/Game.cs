#nullable enable

using System.Collections.Generic;
using TrainerCommon.Cheats;

namespace TrainerCommon.Games;

public interface Game {

    string name { get; }
    string processName { get; }
    string supportedVersion { get; }
    ICollection<Cheat> cheats { get; }

    string? getVersion(string executableSha256Hash);

}

public abstract class BaseGame: Game {

    public abstract string name { get; }

    public abstract string processName { get; }

    public abstract string supportedVersion { get; }

    public abstract ICollection<Cheat> cheats { get; }

    public abstract string? getVersion(string executableSha256Hash);

}