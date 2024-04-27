#nullable enable

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

    private static readonly Encoding ENCODING                         = new ASCIIEncoding();
    private static readonly int[]    DAMAGE_OFFSETS_2_3_0_EPIC_2403   = [0x5B8F850, 0x196BE];
    private static readonly int[]    DAMAGE_OFFSETS_2_3_420_EPIC_2404 = [0x5B8F850, 0x1966A];
    private static readonly int[]    DAMAGE_OFFSETS_2_3_0_STEAM_2402  = [0x5B8F810, 0x19AAE];
    private static readonly int[]    DAMAGE_OFFSETS_1_9_0_STEAM_2304  = [0x59A8C40, 0x1EDDE];

    public override string name { get; } = "No damage";

    public override Combination keyboardShortcut { get; } = Combination.TriggeredBy(Keys.D).Control().Alt();

    protected override void apply(ProcessHandle processHandle, string gameVersionCode) {
        int[] damageOffsets = gameVersionCode switch {
            DakarDesertRally.Versions.V2_3_0_STEAM_2403  => DAMAGE_OFFSETS_2_3_0_EPIC_2403,
            DakarDesertRally.Versions.V2_3_420_EPIC_2404 => DAMAGE_OFFSETS_2_3_420_EPIC_2404,
            DakarDesertRally.Versions.V2_3_0_STEAM_2402  => DAMAGE_OFFSETS_2_3_0_STEAM_2402,
            DakarDesertRally.Versions.V1_9_0_STEAM_2304  => DAMAGE_OFFSETS_1_9_0_STEAM_2304,
            _                                            => []
        };

        if (damageOffsets.Any()) {
            IndirectMemoryAddress damageAddress = new(processHandle, null, damageOffsets);

            MemoryEditor.writeToProcessMemory(processHandle, damageAddress, DISABLED_MEMORY_VALUE, ENCODING);
        }
    }

}