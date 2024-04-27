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
        VersionSha256Hashes.V2_3_0_STEAM_2403  => Versions.V2_3_0_STEAM_2403,
        VersionSha256Hashes.V2_3_420_EPIC_2403 => Versions.V2_3_420_EPIC_2403,
        VersionSha256Hashes.V2_3_0_STEAM_2402  => Versions.V2_3_0_STEAM_2402,
        VersionSha256Hashes.V1_9_0_STEAM_2304  => Versions.V1_9_0_STEAM_2304,
        VersionSha256Hashes.V1_7_0_STEAM_2302  => Versions.V1_7_0_STEAM_2302,
        VersionSha256Hashes.V1_6_0_STEAM_2301  => Versions.V1_6_0_STEAM_2301,
        VersionSha256Hashes.V1_5_0_STEAM_2211  => Versions.V1_5_0_STEAM_2211,
        _                                      => null
    };

    public static class Versions {

        public const string V2_3_0_STEAM_2403  = "2.3.0 (Steam) March 2024 hotfix";
        public const string V2_3_420_EPIC_2403 = "2.3.0 (Epic) March 2024 hotfix";
        public const string V2_3_0_STEAM_2402  = "2.3.0 (Steam) February 2024 hotfix";
        public const string V1_9_0_STEAM_2304  = "1.9.0 (Steam)";
        public const string V1_7_0_STEAM_2302  = "1.7.0 (Steam)";
        public const string V1_6_0_STEAM_2301  = "1.6.0 (Steam)";
        public const string V1_5_0_STEAM_2211  = "1.5.0 (Steam)";

    }

    private static class VersionSha256Hashes {

        public const string V2_3_0_STEAM_2403  = "991340b5e7e49723b71092ecdc1cc5e6e5509fe4408cb7c6704075b6298febb1";
        public const string V2_3_420_EPIC_2403 = "fd51adaea2b582697521878d4f104eb3d6a6916cf1ee1492ca304f98048e5225";
        public const string V2_3_0_STEAM_2402  = "b608ec0671207e8883b1bc0221cbbfe25b8a015adc1ddc645d34d6635ef21263";
        public const string V1_9_0_STEAM_2304  = "9781f5d4af4a7ba45b9b5f179589fd06c7ebe84ae5cc8cf05fbd31ab538b5e1d";
        public const string V1_7_0_STEAM_2302  = "b3ebb61fef5dc643f6eb165e409a5c426d861d49bb990b5086b6b599534109b1";
        public const string V1_6_0_STEAM_2301  = "b3e1317208674b47ef4f651c1198057830ad209cf99ba717d74ede333ea10696";
        public const string V1_5_0_STEAM_2211  = "ccb3cdedd0541f483e2f7e95a9bd4ab307f63c13cbd7d8ae0edf321bfc66be82";

    }

}