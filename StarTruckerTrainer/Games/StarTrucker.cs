#nullable enable

using StarTruckerTrainer.Cheats;
using System.Collections.Generic;
using TrainerCommon.Cheats;
using TrainerCommon.Games;

namespace StarTruckerTrainer.Games;

public class StarTrucker: BaseGame {

    public override string name { get; } = "Star Trucker";
    public override string processName { get; } = "Star Trucker";
    public override string supportedVersion { get; } = "1.0.64";

    public override ICollection<Cheat> cheats { get; } = [
        new NoAccelerationDegredationCheat(),
        new NoSpeedDegredationCheat(),
        new NoNozzleCoolerDegredationCheat(),
        new NoFuelEconomyDegredationCheat(),
        new NoSensorDegredationCheat()
    ];

    public override string? getVersion(string executableSha256Hash) => executableSha256Hash switch {
        VersionSha256Hashes.V1_0_64_STEAM => Versions.V1_0_64_STEAM,
        _                                 => null
    };

    public static class Versions {

        public const string V1_0_64_STEAM = "1.0.64.0 (Steam)";

    }

    private static class VersionSha256Hashes {

        public const string V1_0_64_STEAM = "777ff270c9df48290cf388b67483958d3ec17b963e44f45ab26257af1af13e7e";

    }

}