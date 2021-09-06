using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using TrainerCommon.Cheats;
using TrainerCommon.Trainer;

#nullable enable

namespace Dakar18Trainer.Cheats {

    public class NoSpeedLimitCheat: BaseCheat {

        private static readonly int[] SPEED_LIMIT_OFFSETS = { 0x04194850, 0x28, 0x1A0, 0x1B0, 0xA0, 0x20, 0x20, 0x830, 0x3A8 };

        public override string name { get; } = "No speed limits";

        public override Combination keyboardShortcut { get; } = Combination.TriggeredBy(Keys.L).Control().Alt();

        public override void applyIfNecessary(ProcessHandle processHandle, MemoryEditor memoryEditor) {
            if (!isEnabled.Value) return;

            IndirectMemoryAddress speedLimitAddress = new(processHandle, null, SPEED_LIMIT_OFFSETS);
            memoryEditor.writeToProcessMemory(processHandle, speedLimitAddress, 9999);
        }

    }

}