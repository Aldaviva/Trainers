#nullable enable

using System;
using TrainerCommon.Trainer;

namespace TrainerCommon.App;

public partial class MainWindow {

    public MainWindow(MainWindowViewModel viewModel) {
        DataContext = viewModel;

        InitializeComponent();
    }

    protected override void OnSourceInitialized(EventArgs e) {
        base.OnSourceInitialized(e);
        Win32.hideMinimizeAndMaximizeButtons(this);
    }

}