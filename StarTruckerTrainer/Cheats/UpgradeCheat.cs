#nullable enable

using Gma.System.MouseKeyHook;
using StarTruckerTrainer.Games;
using System;
using System.Linq;
using System.Windows.Forms;
using TrainerCommon.Cheats;
using TrainerCommon.Trainer;

namespace StarTruckerTrainer.Cheats;

public class UpgradeCheat: BaseCheat {

    private const string MODULE_NAME       = "GameAssembly.dll";
    private const int    MAX_UPGRADE_LEVEL = 10;

    private static readonly int[]        OFFSETS_TEMPLATE_1_0_64 = [0x0290F700, 0xB8, 0x0, 0x80, 0x30, -1 /* ShipSystem */, 0x64]; // Assembly-CSharp.dll:UpgradeDef.m_currentLevel
    private static readonly ShipSystem[] SHIP_SYSTEMS            = (ShipSystem[]) Enum.GetValues(typeof(ShipSystem));

    public override string name { get; } = "Upgrade and repair all ship systems to maximum level";
    public override Combination? keyboardShortcut { get; } = Combination.TriggeredBy(Keys.U).Alt().Control();

    private int[]? offsets;

    protected override void apply(ProcessHandle processHandle, string gameVersionCode) {
        if (offsets == null) {
            int[] offsetTemplate = gameVersionCode switch {
                StarTrucker.Versions.V1_0_64_STEAM => OFFSETS_TEMPLATE_1_0_64,
                _                                  => []
            };

            offsets = [..offsetTemplate];
        }

        if (offsets?.Any() ?? false) {
            foreach (ShipSystem shipSystem in SHIP_SYSTEMS) {
                offsets[5] = (int) shipSystem;
                IndirectMemoryAddress currentLevelAddress           = new(processHandle, MODULE_NAME, offsets);
                OffsetMemoryAddress   currentFunctionalLevelAddress = new(currentLevelAddress, 4);
                OffsetMemoryAddress   currentHealthAddress          = new(currentLevelAddress, 8);

                int oldLevel = MemoryEditor.readFromProcessMemory<int>(processHandle, currentFunctionalLevelAddress);
                if (oldLevel < MAX_UPGRADE_LEVEL) {
                    MemoryEditor.writeToProcessMemory(processHandle, currentLevelAddress, MAX_UPGRADE_LEVEL);
                    MemoryEditor.writeToProcessMemory(processHandle, currentFunctionalLevelAddress, MAX_UPGRADE_LEVEL);
                    MemoryEditor.writeToProcessMemory(processHandle, currentHealthAddress, 1.0f);
                }
            }
        }
    }

    private enum ShipSystem {

        ACCELERATION = 0x20,
        SPEED        = 0x28,
        COOLING      = 0x30,
        FUEL_ECONOMY = 0x38,
        ARMOR        = 0x40,
        INSULATION   = 0x48,
        SHIELDS      = 0x50,
        MAGLOCK      = 0x58,
        SUIT         = 0x60,
        SENSORS      = 0x68

    }

}