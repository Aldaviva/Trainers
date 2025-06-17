#nullable enable

using Gma.System.MouseKeyHook;

namespace StarTruckerTrainer.Cheats;

public class NoAccelerationDegredationCheat: ShipUpgradeLevelCheat {

    public override string name { get; } = "Turbopump System doesn't degrade";
    public override Combination? keyboardShortcut { get; } = null;
    protected override byte offset5 { get; } = 0x20;

}