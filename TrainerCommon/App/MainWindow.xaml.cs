using System;
using TrainerCommon.Trainer;

#nullable enable

namespace TrainerCommon.App {

    public partial class MainWindow {

        private readonly MainWindowViewModel viewModel;

        public MainWindow(MainWindowViewModel viewModel) {
            this.viewModel = viewModel;
            DataContext    = viewModel;

            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e) {
            base.OnSourceInitialized(e);
            Win32.hideMinimizeAndMaximizeButtons(this);
        }

    }

}