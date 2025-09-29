#nullable enable

using StarTruckerTrainer.Cheats;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TrainerCommon.Cheats;
using TrainerCommon.Games;

namespace StarTruckerTrainer.Games;

public class StarTrucker: BaseGame {

    public override string name { get; } = "Star Trucker";
    public override string processName { get; } = "Star Trucker";
    public override string supportedVersion { get; } = "1.0.64\u20131.0.70";

    public override ICollection<Cheat> cheats { get; } = [
        new UpgradeCheat()
    ];

    public override string? getVersion(Process gameProcess) {
        string installDir           = Path.GetDirectoryName(gameProcess.MainModule!.FileName)!;
        string gameManagersFilepath = Path.Combine(installDir, "Star Trucker_Data", "globalgamemanagers"); // EXE doesn't always change
        string gameManagersHash     = readFileHash(gameManagersFilepath);
        return gameManagersHash switch {
            VersionSha256Hashes.V1_0_64_STEAM => Versions.V1_0_64_STEAM,
            VersionSha256Hashes.V1_0_67_STEAM => Versions.V1_0_67_STEAM,
            VersionSha256Hashes.V1_0_70_STEAM => Versions.V1_0_70_STEAM,
            _                                 => null
        };
    }

    public static class Versions {

        public const string V1_0_64_STEAM = "1.0.64.0 (Steam)";
        public const string V1_0_67_STEAM = "1.0.67.0 (Steam)";
        public const string V1_0_70_STEAM = "1.0.70.0 (Steam)";

    }

    private static class VersionSha256Hashes {

        public const string V1_0_64_STEAM = "3c8301dce62b297bee2d8157071a7eae5287568103d5b155d1dd54af4c174c47";
        public const string V1_0_67_STEAM = "064d545f7ba1f7147655702f9d497cc0f426c4ee99edd57bb591c70abab2eecb";
        public const string V1_0_70_STEAM = "fdf23ef0e35095a00d2d422db590689e965934279cb989cc88a6b92b9616222f";

    }

}