#nullable enable

using DakarDesertRallyTrainer.Games;
using Gma.System.MouseKeyHook;
using System.Linq;
using System.Windows.Forms;
using TrainerCommon.Cheats;
using TrainerCommon.Trainer;

namespace DakarDesertRallyTrainer.Cheats;

public class NoSpeedLimitCheat: BaseCheat {

    private static readonly int[] SPEED_LIMIT_OFFSETS_2_3_0_STEAM_2403 = [0x5854580, 0x10, 0xF0, 0x30C];
    private static readonly int[] SPEED_LIMIT_OFFSETS_2_3_0_EPIC_2404  = [0x5854580, 0x10, 0xF0, 0x30C];
    private static readonly int[] SPEED_LIMIT_OFFSETS_2_3_0_STEAM_2402 = [0x5854530, 0x10, 0xF0, 0x30C];
    private static readonly int[] SPEED_LIMIT_OFFSETS_1_9_0_STEAM_2304 = [0x5589060, 0x10, 0xF0, 0x30C];
    private static readonly int[] SPEED_LIMIT_OFFSETS_1_7_0_STEAM_2302 = [0x55BECA0, 0x10, 0xF0, 0x30C];
    private static readonly int[] SPEED_LIMIT_OFFSETS_1_6_0_STEAM_2301 = [0x555A7B0, 0x10, 0xF0, 0x30C];
    private static readonly int[] SPEED_LIMIT_OFFSETS_1_5_0_STEAM_2211 = [0x57C3550, 0x10, 0xF0, 0x30C];

    private static readonly int[] BANNER_OFFSETS_2_3_0_STEAM_2403 = [0x5CEE920, 0x30, 0x2B0, 0x3F0, 0x670, 0x364];
    private static readonly int[] BANNER_OFFSETS_2_3_0_EPIC_2404  = [0x5CEE920, 0x30, 0x2B0, 0x3F0, 0x670, 0x364];
    private static readonly int[] BANNER_OFFSETS_2_3_0_STEAM_2402 = [0x5CEE8E0, 0x30, 0x2B0, 0x3F0, 0x670, 0x364];
    private static readonly int[] BANNER_OFFSETS_1_9_0_STEAM_2304 = [0x5B200B8, 0x3B0, 0x10, 0x7F0, 0x658, 0x364];
    private static readonly int[] BANNER_OFFSETS_1_7_0_STEAM_2302 = [0x5B5DC38, 0x3B0, 0x10, 0x7F0, 0x658, 0x364];
    private static readonly int[] BANNER_OFFSETS_1_6_0_STEAM_2301 = [0x5AF6638, 0x3B0, 0x10, 0x7F0, 0x658, 0x364];
    private static readonly int[] BANNER_OFFSETS_1_5_0_STEAM_2211 = [0x5C62C80, 0x30, 0x2B0, 0x408, 0x658, 0x364];

    public override string name { get; } = "No speed limit";

    public override Combination keyboardShortcut { get; } = Combination.TriggeredBy(Keys.L).Control().Alt();

    protected override void apply(ProcessHandle processHandle, string gameVersionCode) {
        (int[] speedLimitOffsets, int[] bannerOffsets) = gameVersionCode switch {
            DakarDesertRally.Versions.V2_3_0_STEAM_2403  => (SPEED_LIMIT_OFFSETS_2_3_0_STEAM_2403, BANNER_OFFSETS_2_3_0_STEAM_2403),
            DakarDesertRally.Versions.V2_3_420_EPIC_2404 => (SPEED_LIMIT_OFFSETS_2_3_0_EPIC_2404, BANNER_OFFSETS_2_3_0_EPIC_2404),
            DakarDesertRally.Versions.V2_3_0_STEAM_2402  => (SPEED_LIMIT_OFFSETS_2_3_0_STEAM_2402, BANNER_OFFSETS_2_3_0_STEAM_2402),
            DakarDesertRally.Versions.V1_9_0_STEAM_2304  => (SPEED_LIMIT_OFFSETS_1_9_0_STEAM_2304, BANNER_OFFSETS_1_9_0_STEAM_2304),
            DakarDesertRally.Versions.V1_7_0_STEAM_2302  => (SPEED_LIMIT_OFFSETS_1_7_0_STEAM_2302, BANNER_OFFSETS_1_7_0_STEAM_2302),
            DakarDesertRally.Versions.V1_6_0_STEAM_2301  => (SPEED_LIMIT_OFFSETS_1_6_0_STEAM_2301, BANNER_OFFSETS_1_6_0_STEAM_2301),
            DakarDesertRally.Versions.V1_5_0_STEAM_2211  => (SPEED_LIMIT_OFFSETS_1_5_0_STEAM_2211, BANNER_OFFSETS_1_5_0_STEAM_2211),
            _                                            => ([], [])
        };

        if (speedLimitOffsets.Any()) {
            IndirectMemoryAddress speedLimitAddress = new(processHandle, null, speedLimitOffsets);
            IndirectMemoryAddress bannerAddress     = new(processHandle, null, bannerOffsets);

            MemoryEditor.writeToProcessMemory(processHandle, speedLimitAddress, 9999);
            MemoryEditor.writeToProcessMemory(processHandle, bannerAddress, 9999);
        }
    }

}