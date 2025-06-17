#nullable enable

using Gma.System.MouseKeyHook;

namespace StarTruckerTrainer.Cheats;

public class NoSpeedDegredationCheat: ShipUpgradeLevelCheat {

    public override string name { get; } = "Combustion Chambers don't degrade";
    public override Combination? keyboardShortcut { get; } = null;
    protected override byte offset5 { get; } = 0x28;

}