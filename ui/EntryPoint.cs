﻿using System;
using System.Windows.Threading;

namespace IkariamPlanner {
    public class EntryPoint : IDisposable {
        private readonly SystemTray systemTray = new SystemTray();
        private readonly App app = new App();
        private MainWindow window;
        private bool disposedValue = false;

        [STAThread]
        public static void Main(string[] args) {
            new EntryPoint();
        }

        private EntryPoint() {
            systemTray.OpenPlanner += Open;
            systemTray.Quit += Quit;
            app.InitializeComponent();
            Dispatcher.Run();
        }

        private void Open() {
            if (window == null) {
                window = new MainWindow();
                window.Show();
                window.Closed += WindowClosed;
            } else {
                window.Activate();
            }
        }

        private void WindowClosed(object sender, EventArgs e) {
            window.Closed -= WindowClosed;
            window = null;
        }

        private void Quit() {
            Dispose();
            Dispatcher.ExitAllFrames();
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    app.Shutdown();
                    systemTray.Quit -= Quit;
                    systemTray.OpenPlanner -= Open;
                    systemTray.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}