#nullable enable

using Gma.System.MouseKeyHook;

namespace StarTruckerTrainer.Cheats;

public class NoNozzleCoolerDegredationCheat: ShipUpgradeLevelCheat {

    public override string name { get; } = "Nozzle Coolers don't degrade";
    public override Combination? keyboardShortcut { get; } = null;
    protected override byte offset5 { get; } = 0x30;

}