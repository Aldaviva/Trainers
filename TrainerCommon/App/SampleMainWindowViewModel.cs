#nullable enable
using System.Collections.Generic;
using Gma.System.MouseKeyHook;
using KoKo.Property;
using TrainerCommon.Cheats;
using TrainerCommon.Games;
using TrainerCommon.Trainer;

namespace TrainerCommon.App;

/// <summary>
/// For design time
/// </summary>
internal class SampleMainWindowViewModel: MainWindowViewModel {

    public SampleMainWindowViewModel(): base(new SampleGame(), new TrainerServiceImpl()) { }

    private class SampleGame: Game {

        public string name { get; } = "My Game";
        public string processName { get; } = "mygame.exe";
        public string supportedVersion { get; } = "1.0.0";

        public ICollection<Cheat> cheats { get; } = new List<Cheat> {
            new SampleCheat("Infinite ammo", "Control+Alt+LWin+A"),
            new SampleCheat("Infinite lives", "Shift+Control+Alt+L")
        };

        public string getVersion(string executableSha256Hash) => "1";

        private class SampleCheat: Cheat {

            public string name { get; }
            public Combination keyboardShortcut { get; }
            public SettableProperty<bool> isEnabled { get; } = new StoredProperty<bool>();
            public void applyIfNecessary(ProcessHandle processHandle, string gameVersionCode) { }

            public SampleCheat(string name, string keyBoardShortcut) {
                this.name        = name;
                keyboardShortcut = Combination.FromString(keyBoardShortcut);
            }

        }

    }

}