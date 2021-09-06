using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using KoKo.Property;
using TrainerCommon.Cheats;
using TrainerCommon.Games;
using TrainerCommon.Trainer;

#nullable enable

namespace TrainerCommon.App {

    public class MainWindowViewModel {

        public Property<string> statusBarAttachmentMessage { get; }

        public Game game { get; }

        public string windowTitle => $"{game.name} {game.supportedVersion} +{game.cheats.Count:N0} Trainer";

        // ReSharper disable once UnusedMember.Global - used by design time DataContext DesignInstance
        public MainWindowViewModel(): this(new SampleGame(), new TrainerServiceImpl()) { }

        public MainWindowViewModel(Game game, TrainerService trainerService) {
            this.game = game;

            statusBarAttachmentMessage = DerivedProperty<string>.Create(trainerService.isAttachedToGame, attached =>
                attached ? "Attached to game process" : "Detached from game process");

            statusBarAttachmentMessage.EventSynchronizationContext = SynchronizationContext.Current;
            foreach (Cheat cheat in game.cheats) {
                cheat.isEnabled.EventSynchronizationContext = SynchronizationContext.Current;
            }
        }

        private class SampleGame: Game {

            public string name { get; } = "My Game";
            public string processName { get; } = "mygame.exe";
            public string supportedVersion { get; } = "1.0.0";
            public IList<Cheat> cheats { get; } = Enumerable.Repeat(new SampleCheat(), 2).Cast<Cheat>().ToList();

            private class SampleCheat: Cheat {

                public string name { get; set; } = "Infinite ammo";
                public Combination keyboardShortcut { get; set; } = Combination.TriggeredBy(Keys.A).Alt().Control();
                public SettableProperty<bool> isEnabled { get; } = new StoredProperty<bool>();
                public void applyIfNecessary(ProcessHandle processHandle, MemoryEditor memoryEditor) { }

            }

        }

    }

}