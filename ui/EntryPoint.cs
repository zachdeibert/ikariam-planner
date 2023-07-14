using System;
using System.Windows;

namespace IkariamPlanner {
    public class EntryPoint : IDisposable {
        private readonly Server.Server Server = new Server.Server();
        private readonly Ui.Ui Ui;
        private bool DisposedValue = false;

        [STAThread]
        public static void Main(string[] args) {
            new EntryPoint();
        }

        private EntryPoint() {
            Ui = new Ui.Ui(Server.Model);
            Server.PacketReceived += PacketReceived;
            Ui.Shutdown += Quit;
            Ui.Start();
        }

        private void PacketReceived() {
            Application.Current.Dispatcher.Invoke(() => Ui.Open(false));
        }

        private void Quit() {
            Dispose();
        }

        protected virtual void Dispose(bool disposing) {
            if (!DisposedValue) {
                if (disposing) {
                    Ui.Shutdown -= Quit;
                    Server.PacketReceived -= PacketReceived;
                    Ui.Dispose();
                    Server.Dispose();
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
