using System;
using System.Windows;

namespace SuperhotMindControlDeleteTrainer {

    public partial class MainWindow: IDisposable {

        private readonly MainWindowViewModel viewModel;

        public MainWindow() {
            viewModel   = new MainWindowViewModel();
            DataContext = viewModel;
            
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e) {
            base.OnSourceInitialized(e);
            Win32.hideMinimizeAndMaximizeButtons(this);
        }

        private void onInfiniteHealthChecked(object sender, RoutedEventArgs e) {
            viewModel.setInfiniteHealthEnabled(infiniteHealthCheckbox.IsChecked ?? false);
        }

        public void Dispose() {
            viewModel?.Dispose();
        }
    }

}