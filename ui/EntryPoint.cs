using System;
using System.Windows;

namespace IkariamPlanner {
    public class EntryPoint : IDisposable {
        private readonly Server.Server server = new Server.Server();
        private readonly Ui.Ui ui = new Ui.Ui();
        private bool disposedValue = false;

        [STAThread]
        public static void Main(string[] args) {
            new EntryPoint();
        }

        private EntryPoint() {
            server.PacketReceived += PacketReceived;
            ui.Shutdown += Quit;
            ui.Start();
        }

        private void PacketReceived() {
            Application.Current.Dispatcher.Invoke(() => ui.Open(false));
        }

        private void Quit() {
            Dispose();
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    ui.Shutdown -= Quit;
                    server.PacketReceived -= PacketReceived;
                    ui.Dispose();
                    server.Dispose();
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
