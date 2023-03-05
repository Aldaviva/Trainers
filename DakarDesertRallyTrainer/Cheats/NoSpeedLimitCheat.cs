#nullable enable

using System.Windows.Forms;
using DakarDesertRallyTrainer.Games;
using Gma.System.MouseKeyHook;
using TrainerCommon.Cheats;
using TrainerCommon.Trainer;

namespace DakarDesertRallyTrainer.Cheats;

public class NoSpeedLimitCheat: BaseCheat {

    private static readonly int[] SPEED_LIMIT_OFFSETS_1_7_0 = { 0x55BECA0, 0x10, 0xF0, 0x30C };
    private static readonly int[] SPEED_LIMIT_OFFSETS_1_6_0 = { 0x555A7B0, 0x10, 0xF0, 0x30C };
    private static readonly int[] SPEED_LIMIT_OFFSETS_1_5_0 = { 0x57C3550, 0x10, 0xF0, 0x30C };

    private static readonly int[] BANNER_OFFSETS_1_7_0 = { 0x5B5DC38, 0x3B0, 0x10, 0x7F0, 0x658, 0x364 };
    private static readonly int[] BANNER_OFFSETS_1_6_0 = { 0x5AF6638, 0x3B0, 0x10, 0x7F0, 0x658, 0x364 };
    private static readonly int[] BANNER_OFFSETS_1_5_0 = { 0x5C62C80, 0x30, 0x2B0, 0x408, 0x658, 0x364 };

    public override string name { get; } = "No speed limit";

    public override Combination keyboardShortcut { get; } = Combination.TriggeredBy(Keys.L).Control().Alt();

    protected override void apply(ProcessHandle processHandle, string gameVersionCode) {
        int[] speedLimitOffsets;
        int[] bannerOffsets;

        switch (gameVersionCode) {
            case DakarDesertRally.Versions.V1_7_0:
                speedLimitOffsets = SPEED_LIMIT_OFFSETS_1_7_0;
                bannerOffsets     = BANNER_OFFSETS_1_7_0;
                break;
            case DakarDesertRally.Versions.V1_6_0:
                speedLimitOffsets = SPEED_LIMIT_OFFSETS_1_6_0;
                bannerOffsets     = BANNER_OFFSETS_1_6_0;
                break;
            case DakarDesertRally.Versions.V1_5_0:
                speedLimitOffsets = SPEED_LIMIT_OFFSETS_1_5_0;
                bannerOffsets     = BANNER_OFFSETS_1_5_0;
                break;
            default:
                //unsupported game version
                return;
        }

        IndirectMemoryAddress speedLimitAddress = new(processHandle, null, speedLimitOffsets);
        MemoryEditor.writeToProcessMemory(processHandle, speedLimitAddress, 9999);

        IndirectMemoryAddress bannerAddress = new(processHandle, null, bannerOffsets);
        MemoryEditor.writeToProcessMemory(processHandle, bannerAddress, 9999);
    }

}