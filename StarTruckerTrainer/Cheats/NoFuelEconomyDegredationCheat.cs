#nullable enable

using Gma.System.MouseKeyHook;

namespace StarTruckerTrainer.Cheats;

public class NoFuelEconomyDegredationCheat: ShipUpgradeLevelCheat {

    public override string name { get; } = "Engine Control Unit doesn't degrade";
    public override Combination? keyboardShortcut { get; } = null;
    protected override byte offset5 { get; } = 0x38;

}