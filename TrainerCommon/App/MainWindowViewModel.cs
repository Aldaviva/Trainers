#nullable enable

using KoKo.Property;
using System;
using System.Threading;
using TrainerCommon.Cheats;
using TrainerCommon.Games;
using TrainerCommon.Trainer;

namespace TrainerCommon.App;

public class MainWindowViewModel {

    public Property<string> statusBarAttachmentMessage { get; }

    public Game game { get; }

    public string windowTitle => $"{game.name} {game.supportedVersion} +{game.cheats.Count:N0} Trainer by Ben";

    public MainWindowViewModel(Game game, TrainerService trainerService) {
        this.game = game;

        statusBarAttachmentMessage = DerivedProperty<string>.Create(trainerService.attachmentState, attached => attached switch {
            AttachmentState.TRAINER_STOPPED             => "Attaching to game…",
            AttachmentState.PROGRAM_NOT_RUNNING         => "Waiting for game to start",
            AttachmentState.ATTACHED                    => "Attached to game process",
            AttachmentState.UNSUPPORTED_PROGRAM_VERSION => "Unsupported game version",
            _                                           => throw new ArgumentOutOfRangeException(nameof(attached), attached, null)
        });

        statusBarAttachmentMessage.EventSynchronizationContext = SynchronizationContext.Current;
        foreach (Cheat cheat in game.cheats) {
            cheat.isEnabled.EventSynchronizationContext = SynchronizationContext.Current;
        }
    }

}