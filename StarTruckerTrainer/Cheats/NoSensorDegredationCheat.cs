#nullable enable

using Gma.System.MouseKeyHook;

namespace StarTruckerTrainer.Cheats;

public class NoSensorDegredationCheat: ShipUpgradeLevelCheat {

    public override string name { get; } = "Sensors don't degrade";
    public override Combination? keyboardShortcut { get; } = null;
    protected override byte offset5 { get; } = 0x68;

}