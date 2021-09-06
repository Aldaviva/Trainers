using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Gma.System.MouseKeyHook;
using TrainerCommon.Games;
using TrainerCommon.Trainer;

#nullable enable

namespace TrainerCommon.App {

    public abstract class CommonApp: Application {

        private IKeyboardMouseEvents? keyboardShortcuts;
        private TrainerService?       trainerService;

        protected abstract Game game { get; }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            keyboardShortcuts = Hook.GlobalEvents();
            keyboardShortcuts.OnCombination(game.cheats.Select(cheat =>
                new KeyValuePair<Combination, Action>(cheat.keyboardShortcut, () => cheat.isEnabled.Value ^= true)));

            trainerService = new TrainerServiceImpl();

            MainWindowViewModel viewModel = new(game, trainerService);
            MainWindow = new MainWindow(viewModel);
            MainWindow!.Show();

            trainerService.attachToGame(game);
        }

        protected override void OnExit(ExitEventArgs e) {
            keyboardShortcuts?.Dispose();
            trainerService?.Dispose();
            base.OnExit(e);
        }

    }

}