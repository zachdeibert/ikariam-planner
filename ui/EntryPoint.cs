using System;

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
            ui.Shutdown += Quit;
            ui.Start();
        }

        private void Quit() {
            Dispose();
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
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
