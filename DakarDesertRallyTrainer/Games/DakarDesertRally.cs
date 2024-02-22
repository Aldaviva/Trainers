#nullable enable

using DakarDesertRallyTrainer.Cheats;
using System.Collections.Generic;
using TrainerCommon.Cheats;
using TrainerCommon.Games;

namespace DakarDesertRallyTrainer.Games;

public class DakarDesertRally: BaseGame {

    public override string name { get; } = "Dakar Desert Rally";
    public override string processName { get; } = "Dakar2Game-Win64-Shipping";
    public override string supportedVersion { get; } = "1.5.0\u20132.3.0";

    public override ICollection<Cheat> cheats { get; } = [
        new NoSpeedLimitCheat(),
        new NoDamageCheat()
    ];

    public override string? getVersion(string executableSha256Hash) => executableSha256Hash switch {
        VersionSha256Hashes.V2_3_0 => Versions.V2_3_0,
        VersionSha256Hashes.V1_9_0 => Versions.V1_9_0,
        VersionSha256Hashes.V1_7_0 => Versions.V1_7_0,
        VersionSha256Hashes.V1_6_0 => Versions.V1_6_0,
        VersionSha256Hashes.V1_5_0 => Versions.V1_5_0,
        _                          => null
    };

    public static class Versions {

        public const string V2_3_0 = "2.3.0";
        public const string V1_9_0 = "1.9.0";
        public const string V1_7_0 = "1.7.0";
        public const string V1_6_0 = "1.6.0";
        public const string V1_5_0 = "1.5.0";

    }

    private static class VersionSha256Hashes {

        public const string V2_3_0 = "b608ec0671207e8883b1bc0221cbbfe25b8a015adc1ddc645d34d6635ef21263";
        public const string V1_9_0 = "9781f5d4af4a7ba45b9b5f179589fd06c7ebe84ae5cc8cf05fbd31ab538b5e1d";
        public const string V1_7_0 = "b3ebb61fef5dc643f6eb165e409a5c426d861d49bb990b5086b6b599534109b1";
        public const string V1_6_0 = "b3e1317208674b47ef4f651c1198057830ad209cf99ba717d74ede333ea10696";
        public const string V1_5_0 = "ccb3cdedd0541f483e2f7e95a9bd4ab307f63c13cbd7d8ae0edf321bfc66be82";

    }

}