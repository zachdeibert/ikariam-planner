using System;
using System.Windows.Threading;

namespace IkariamPlanner.Ui {
    internal class Ui : IDisposable {
        private readonly SystemTray SystemTray = new SystemTray();
        private readonly App App = new App();
        private MainWindow Window;
        private bool DisposedValue = false;

        public event Action Shutdown;

        public Ui() {
            SystemTray.OpenPlanner += Open;
            SystemTray.Quit += Quit;
            App.InitializeComponent();
        }

        public void Start() {
            Dispatcher.Run();
        }

        private void Open() {
            Open(true);
        }

        public void Open(bool activate) {
            if (Window == null) {
                Window = new MainWindow();
                Window.Show();
                Window.Closed += WindowClosed;
            } else if (activate) {
                Window.Activate();
            }
        }

        private void WindowClosed(object sender, EventArgs e) {
            Window.Closed -= WindowClosed;
            Window = null;
        }

        private void Quit() {
            Dispose();
            Dispatcher.ExitAllFrames();
            Shutdown?.Invoke();
        }

        protected virtual void Dispose(bool disposing) {
            if (!DisposedValue) {
                if (disposing) {
                    App.Shutdown();
                    SystemTray.Quit -= Quit;
                    SystemTray.OpenPlanner -= Open;
                    SystemTray.Dispose();
                }
                DisposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
