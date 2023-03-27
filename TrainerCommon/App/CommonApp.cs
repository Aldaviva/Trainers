#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Dark.Net;
using Dark.Net.Wpf;
using Gma.System.MouseKeyHook;
using McMaster.Extensions.CommandLineUtils;
using TrainerCommon.Cheats;
using TrainerCommon.Games;
using TrainerCommon.Trainer;

namespace TrainerCommon.App;

public abstract class CommonApp: Application {

    private IKeyboardMouseEvents? keyboardShortcuts;
    private TrainerService?       trainerService;

    protected abstract Game game { get; }

    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);

        DarkNet.Instance.SetCurrentProcessTheme(Theme.Auto);
        new SkinManager().RegisterSkins(
            getResourceUri("TrainerCommon", "App/Skins/Skin.Light.xaml"),
            getResourceUri("TrainerCommon", "App/Skins/Skin.Dark.xaml"));

        keyboardShortcuts = Hook.GlobalEvents();
        keyboardShortcuts.OnCombination(game.cheats.Select(cheat =>
            new KeyValuePair<Combination, Action>(cheat.keyboardShortcut, () => cheat.isEnabled.Value ^= true)));

        trainerService = new TrainerServiceImpl();

        MainWindowViewModel viewModel = new(game, trainerService);

        if (e.Args.Any(arg => arg.ToLower() is "-?" or "-h" or "--help" or "/?" or "/h" or "/help")) {
            string executableName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
            MessageBox.Show($"""
                {executableName}

                    Trainer window is shown, and no cheats are enabled by default.

                {executableName} --enable-cheat "{game.cheats.First().name}"

                    Trainer window is shown, and the specified cheat is enabled automatically.
                    You can pass --enable-cheat more than once to enable multiple cheats at startup.
                """, "Usage", MessageBoxButton.OK, MessageBoxImage.Information);
            Current.Shutdown(0);
        }

        try {
            foreach (Cheat cheat in getCheatsToEnableOnStartup(e.Args)) {
                cheat.isEnabled.Value = true;
            }
        } catch (ApplicationException exception) {
            MessageBox.Show(exception.Message, viewModel.windowTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            Current.Shutdown(1);
        }

        MainWindow = new MainWindow(viewModel);
        DarkNet.Instance.SetWindowThemeWpf(MainWindow, Theme.Auto);
        MainWindow!.Show();

        trainerService.attachToGame(game);
    }

    /// <exception cref="IOException">if the resource dictionary XAML file can't be found in the executing assembly</exception>
    private static Uri getResourceUri(string unpackedAssemblyName, string resourceDictionaryXamlPath) {
        resourceDictionaryXamlPath = Uri.EscapeUriString(resourceDictionaryXamlPath.TrimStart('/'));
        unpackedAssemblyName       = Uri.EscapeUriString(unpackedAssemblyName);
        string executingAssembly = Uri.EscapeUriString(Assembly.GetExecutingAssembly().GetName().Name);

        Uri uri;
        try {
            uri = new Uri($"pack://application:,,,/{executingAssembly};component/{unpackedAssemblyName}/{resourceDictionaryXamlPath}", UriKind.Absolute);
            GetResourceStream(uri)?.Stream.Dispose();
            return uri;
        } catch (IOException) {
            uri = new Uri($"pack://application:,,,/{unpackedAssemblyName};component/{resourceDictionaryXamlPath}", UriKind.Absolute);
            GetResourceStream(uri)?.Stream.Dispose();
            return uri;
        }
    }

    private IEnumerable<Cheat> getCheatsToEnableOnStartup(string[] args) {
        CommandLineApplication argParser = new();
        argParser.Conventions.UseDefaultConventions();

        CommandOption<string> enableCheatOption = argParser.Option<string>("--enable-cheat", "Cheat to automatically enable on startup", CommandOptionType.MultipleValue);

        argParser.Parse(args);
        return enableCheatOption.ParsedValues.Select(cheatName => {
            try {
                return game.cheats.First(cheat => cheat.name.Equals(cheatName, StringComparison.CurrentCultureIgnoreCase));
            } catch (InvalidOperationException) {
                throw new ApplicationException($"No cheat found with name \"{cheatName}\".");
            }
        });
    }

    protected override void OnExit(ExitEventArgs e) {
        keyboardShortcuts?.Dispose();
        trainerService?.Dispose();
        base.OnExit(e);
    }

}