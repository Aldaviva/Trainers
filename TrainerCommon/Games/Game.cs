#nullable enable

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using TrainerCommon.Cheats;

namespace TrainerCommon.Games;

public interface Game {

    string name { get; }
    string processName { get; }
    string supportedVersion { get; }
    ICollection<Cheat> cheats { get; }

    string? getVersion(Process gameProcess);

}

public abstract class BaseGame: Game {

    public abstract string name { get; }

    public abstract string processName { get; }

    public abstract string supportedVersion { get; }

    public abstract ICollection<Cheat> cheats { get; }

    public abstract string? getVersion(Process gameProcess);

    /// <returns>lowercase hexadecimal SHA-256 hash of the file specified by <paramref name="filename"/></returns>
    protected static string readFileHash(string filename) {
        using FileStream fileStream = File.OpenRead(filename);
        using SHA256     sha256     = SHA256.Create();
        return string.Join(string.Empty, sha256.ComputeHash(fileStream).Select(b => b.ToString("x2")));
    }

}