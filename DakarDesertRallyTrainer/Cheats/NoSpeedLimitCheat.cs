#nullable enable

using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using TrainerCommon.Cheats;
using TrainerCommon.Trainer;

namespace DakarDesertRallyTrainer.Cheats;

public class NoSpeedLimitCheat: BaseCheat {

    private static readonly int[] SPEED_LIMIT_OFFSETS = { 0x57C3550, 0x10, 0xF0, 0x30C };
    private static readonly int[] BANNER_OFFSETS      = { 0x5C62C80, 0x30, 0x2B0, 0x408, 0x658, 0x364 };

    public override string name { get; } = "No speed limit";

    public override Combination keyboardShortcut { get; } = Combination.TriggeredBy(Keys.L).Control().Alt();

    protected override void apply(ProcessHandle processHandle) {
        IndirectMemoryAddress speedLimitAddress = new(processHandle, null, SPEED_LIMIT_OFFSETS);
        MemoryEditor.writeToProcessMemory(processHandle, speedLimitAddress, 9999);

        IndirectMemoryAddress bannerAddress = new(processHandle, null, BANNER_OFFSETS);
        MemoryEditor.writeToProcessMemory(processHandle, bannerAddress, 9999);
    }

}