using System;
using System.Collections.Generic;
using System.Threading;
using Gma.System.MouseKeyHook;
using KoKo.Property;

#nullable enable

namespace SuperhotMindControlDeleteTrainer {

    public class MainWindowViewModel: IDisposable {

        private readonly TrainerService       trainerService = new TrainerServiceImpl();
        private readonly IKeyboardMouseEvents keyboardShortcuts;

        public Property<bool> isInfiniteHealthEnabled { get; }
        public Property<string> statusBarAttachmentMessage { get; }

        public MainWindowViewModel() {
            isInfiniteHealthEnabled = new PassthroughProperty<bool>(trainerService.isInfiniteHealthEnabled);

            statusBarAttachmentMessage = DerivedProperty<string>.Create(trainerService.isAttachedToGame, attached =>
                attached ? "Attached to game process" : "Detached from game process");

            isInfiniteHealthEnabled.EventSynchronizationContext    = SynchronizationContext.Current;
            statusBarAttachmentMessage.EventSynchronizationContext = SynchronizationContext.Current;

            keyboardShortcuts = Hook.GlobalEvents();
            keyboardShortcuts.OnCombination(new Dictionary<Combination, Action> {
                { Combination.FromString("Control+Alt+H"), () => setInfiniteHealthEnabled(!isInfiniteHealthEnabled.Value) } 
            });
            
        }

        public void setInfiniteHealthEnabled(bool infiniteHealthEnabled) {
            trainerService.isInfiniteHealthEnabled.Value = infiniteHealthEnabled;
        }

        public void Dispose() {
            trainerService.Dispose();
            keyboardShortcuts.Dispose();
        }

    }

}