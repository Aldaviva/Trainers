#nullable enable

using StarTruckerTrainer.Games;
using System;
using System.Linq;
using TrainerCommon.Cheats;
using TrainerCommon.Trainer;

namespace StarTruckerTrainer.Cheats;

public abstract class ShipUpgradeLevelCheat: BaseCheat {

    private static readonly int[] OFFSETS_TEMPLATE_1_0_64 = [0x0290F700, 0xB8, 0, 0x80, 0x30, -1 /* offset5 */, 0x68];

    protected abstract byte offset5 { get; }

    private int[]? offsets;
    private int    maxLevelSeen;

    protected override void apply(ProcessHandle processHandle, string gameVersionCode) {
        if (offsets == null) {
            int[] offsetTemplate = gameVersionCode switch {
                StarTrucker.Versions.V1_0_64_STEAM => OFFSETS_TEMPLATE_1_0_64,
                _                                  => []
            };

            offsets = [..offsetTemplate];

            if (offsets.Length > 5) {
                offsets[5] = offset5;
            }
        }

        if (offsets?.Any() ?? false) {
            IndirectMemoryAddress systemLevelAddress = new(processHandle, "GameAssembly.dll", offsets);
            int                   oldLevel           = MemoryEditor.readFromProcessMemory<int>(processHandle, systemLevelAddress);

            maxLevelSeen = Math.Max(maxLevelSeen, oldLevel);

            if (oldLevel < maxLevelSeen) {
                MemoryEditor.writeToProcessMemory(processHandle, systemLevelAddress, maxLevelSeen);
            }
        }
    }

}