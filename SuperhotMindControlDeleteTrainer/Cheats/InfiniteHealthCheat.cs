using System.Diagnostics;

#nullable enable

namespace SuperhotMindControlDeleteTrainer.Cheats {

    public class InfiniteHealthCheat: Cheat {

        private const string MODULE_NAME = "UnityPlayer.dll";

        private static readonly int[] CURRENT_HEARTS_OFFSETS = { 0x01079274, 0x74, 0x40, 0xA8, 0x4, 0xC, 0x10, 0x10, 0x2C };
        private static readonly int[] MAX_HEARTS_OFFSETS     = { 0x01079274, 0x74, 0x40, 0xA8, 0x4, 0xC, 0x10, 0x10, 0x20 };

        public void apply(bool isEnabled, ProcessHandle processHandle, MemoryEditor memoryEditor) {
            if (!isEnabled) return;

            var currentHeartsAddress = new FixedMemoryAddress(new IndirectMemoryAddress(processHandle, MODULE_NAME, CURRENT_HEARTS_OFFSETS).address);
            var maxHeartsAddress     = new IndirectMemoryAddress(processHandle, MODULE_NAME, MAX_HEARTS_OFFSETS);

            int currentHearts = memoryEditor.readFromProcessMemory<int>(processHandle, currentHeartsAddress);
            int maxHearts     = memoryEditor.readFromProcessMemory<int>(processHandle, maxHeartsAddress);

            if (currentHearts < maxHearts) {
                Trace.WriteLine($"Setting health to {maxHearts:N0}...");
                memoryEditor.writeToProcessMemory(processHandle, currentHeartsAddress, maxHearts);
            }
        }

    }

}