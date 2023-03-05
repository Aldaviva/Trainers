#nullable enable

using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using TrainerCommon.Cheats;
using TrainerCommon.Trainer;

namespace Dakar18Trainer.Cheats;

public class NoSpeedLimitCheat: BaseCheat {

    private static readonly int[] SPEED_LIMIT_OFFSETS = { 0x04194850, 0x28, 0x1A0, 0x1B0, 0xA0, 0x20, 0x20, 0x830, 0x3A8 };

    public override string name { get; } = "No speed limit";

    public override Combination keyboardShortcut { get; } = Combination.TriggeredBy(Keys.L).Control().Alt();

    protected override void apply(ProcessHandle processHandle, string gameVersionCode) {
        IndirectMemoryAddress speedLimitAddress = new(processHandle, null, SPEED_LIMIT_OFFSETS);
        MemoryEditor.writeToProcessMemory(processHandle, speedLimitAddress, 9999);
    }

}