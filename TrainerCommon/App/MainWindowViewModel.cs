using System;
using System.Collections.Generic;
using System.Threading;
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

        public string windowTitle => $"{game.name} {game.supportedVersion} +{game.cheats.Count:N0} Trainer by Ben";

        // ReSharper disable once UnusedMember.Global - used by design time DataContext DesignInstance
        public MainWindowViewModel(): this(new SampleGame(), new TrainerServiceImpl()) { }

        public MainWindowViewModel(Game game, TrainerService trainerService) {
            this.game = game;

            statusBarAttachmentMessage = DerivedProperty<string>.Create(trainerService.isAttachedToGame, attached => attached switch {
                AttachmentState.TRAINER_STOPPED                  => "Attaching to game…",
                AttachmentState.PROGRAM_NOT_RUNNING              => "Waiting for game to start",
                AttachmentState.MEMORY_ADDRESS_NOT_FOUND         => "Attached, memory address not found",
                AttachmentState.MEMORY_ADDRESS_COULD_NOT_BE_READ => "Attached, memory unreadable",
                AttachmentState.ATTACHED                         => "Attached to game process",
                _                                                => throw new ArgumentOutOfRangeException(nameof(attached), attached, null)
            });

            statusBarAttachmentMessage.EventSynchronizationContext = SynchronizationContext.Current;
            foreach (Cheat cheat in game.cheats) {
                cheat.isEnabled.EventSynchronizationContext = SynchronizationContext.Current;
            }
        }

        /// <summary>
        /// For design time
        /// </summary>
        private class SampleGame: Game {

            public string name { get; } = "My Game";
            public string processName { get; } = "mygame.exe";
            public string supportedVersion { get; } = "1.0.0";

            public IList<Cheat> cheats { get; } = new List<Cheat> {
                new SampleCheat("Infinite ammo", "Control+Alt+LWin+A"),
                new SampleCheat("Infinite lives", "Shift+Control+Alt+L")
            };

            private class SampleCheat: Cheat {

                public string name { get; }
                public Combination keyboardShortcut { get; }
                public SettableProperty<bool> isEnabled { get; } = new StoredProperty<bool>();
                public void applyIfNecessary(ProcessHandle processHandle) { }

                public SampleCheat(string name, string keyBoardShortcut) {
                    this.name        = name;
                    keyboardShortcut = Combination.FromString(keyBoardShortcut);
                }

            }

        }

    }

}