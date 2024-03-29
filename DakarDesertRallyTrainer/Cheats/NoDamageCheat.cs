﻿#nullable enable

using DakarDesertRallyTrainer.Games;
using Gma.System.MouseKeyHook;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrainerCommon.Cheats;
using TrainerCommon.Trainer;

namespace DakarDesertRallyTrainer.Cheats;

public class NoDamageCheat: BaseCheat {

    private const string DISABLED_MEMORY_VALUE = "bBanGetMechDamage"; // defaults to "bCanGetMechDamage"

    private static readonly Encoding ENCODING             = new ASCIIEncoding();
    private static readonly int[]    DAMAGE_OFFSETS_2_3_0 = [0x5b8f810, 0x19aae];
    private static readonly int[]    DAMAGE_OFFSETS_1_9_0 = [0x59a8c40, 0x1edde];

    public override string name { get; } = "No damage";

    public override Combination keyboardShortcut { get; } = Combination.TriggeredBy(Keys.D).Control().Alt();

    protected override void apply(ProcessHandle processHandle, string gameVersionCode) {
        int[] damageOffsets = gameVersionCode switch {
            DakarDesertRally.Versions.V2_3_0 => DAMAGE_OFFSETS_2_3_0,
            DakarDesertRally.Versions.V1_9_0 => DAMAGE_OFFSETS_1_9_0,
            _                                => []
        };

        if (damageOffsets.Any()) {
            IndirectMemoryAddress damageAddress = new(processHandle, null, damageOffsets);

            MemoryEditor.writeToProcessMemory(processHandle, damageAddress, DISABLED_MEMORY_VALUE, ENCODING);
        }
    }

}