#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Dark.Net;
using Gma.System.MouseKeyHook;
using TrainerCommon.App.Skins;
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
        SkinManager.register(
            getResourceDictionary("TrainerCommon", "App/Skins/Skin.Light.xaml"),
            getResourceDictionary("TrainerCommon", "App/Skins/Skin.Dark.xaml"));

        keyboardShortcuts = Hook.GlobalEvents();
        keyboardShortcuts.OnCombination(game.cheats.Select(cheat =>
            new KeyValuePair<Combination, Action>(cheat.keyboardShortcut, () => cheat.isEnabled.Value ^= true)));

        trainerService = new TrainerServiceImpl();

        MainWindowViewModel viewModel = new(game, trainerService);
        MainWindow = new MainWindow(viewModel);
        DarkNet.Instance.SetWindowThemeWpf(MainWindow, Theme.Auto);
        MainWindow!.Show();

        trainerService.attachToGame(game);
    }

    private static ResourceDictionary getResourceDictionary(string unpackedAssemblyName, string resourceDictionaryXamlPath) {
        resourceDictionaryXamlPath = Uri.EscapeUriString(resourceDictionaryXamlPath.TrimStart('/'));
        unpackedAssemblyName       = Uri.EscapeUriString(unpackedAssemblyName);
        string executingAssembly = Uri.EscapeUriString(Assembly.GetExecutingAssembly().GetName().Name);

        ResourceDictionary resourceDictionary = new();
        try {
            resourceDictionary.Source = new Uri($"pack://application:,,,/{executingAssembly};component/{unpackedAssemblyName}/{resourceDictionaryXamlPath}", UriKind.Absolute);
        } catch (IOException) {
            resourceDictionary.Source = new Uri($"pack://application:,,,/{unpackedAssemblyName};component/{resourceDictionaryXamlPath}", UriKind.Absolute);
        }

        return resourceDictionary;
    }

    protected override void OnExit(ExitEventArgs e) {
        keyboardShortcuts?.Dispose();
        trainerService?.Dispose();
        base.OnExit(e);
    }

}