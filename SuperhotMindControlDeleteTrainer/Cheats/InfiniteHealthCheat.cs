﻿#nullable enable

using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.Windows.Forms;
using TrainerCommon.Cheats;
using TrainerCommon.Trainer;

namespace SuperhotMindControlDeleteTrainer.Cheats;

public class InfiniteHealthCheat: BaseCheat {

    private const string MODULE_NAME = "UnityPlayer.dll";

    private static readonly int[] CURRENT_HEARTS_OFFSETS = [0x01079274, 0x74, 0x40, 0xA8, 0x4, 0xC, 0x10, 0x10, 0x2C];
    private static readonly int[] MAX_HEARTS_OFFSETS     = [0x01079274, 0x74, 0x40, 0xA8, 0x4, 0xC, 0x10, 0x10, 0x20];

    public override string name { get; } = "Infinite health";

    public override Combination keyboardShortcut { get; } = Combination.TriggeredBy(Keys.H).Alt().Control();

    protected override void apply(ProcessHandle processHandle, string gameVersionCode) {
        FixedMemoryAddress    currentHeartsAddress = new(new IndirectMemoryAddress(processHandle, MODULE_NAME, CURRENT_HEARTS_OFFSETS).address); //used twice, so don't reevaluate address
        IndirectMemoryAddress maxHeartsAddress     = new(processHandle, MODULE_NAME, MAX_HEARTS_OFFSETS);

        int currentHearts = MemoryEditor.readFromProcessMemory<int>(processHandle, currentHeartsAddress);
        int maxHearts     = MemoryEditor.readFromProcessMemory<int>(processHandle, maxHeartsAddress);

        if (currentHearts < maxHearts) {
            Trace.WriteLine($"Setting health to {maxHearts:N0}...");
            MemoryEditor.writeToProcessMemory(processHandle, currentHeartsAddress, maxHearts);
        }
    }

}