using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace IkariamPlanner.Ui {
    internal class SystemTray : IDisposable {
        private readonly Container container;
        private readonly MenuItem openMenuItem;
        private readonly MenuItem launchMenuItem;
        private readonly MenuItem quitMenuItem;
        private readonly ContextMenu menu;
        private readonly NotifyIcon notifyIcon;
        private bool disposedValue = false;

        public event Action OpenPlanner;
        public event Action Quit;

        public SystemTray() {
            container = new Container();
            openMenuItem = new MenuItem() {
                Index = 0,
                Text = "Open Planner"
            };
            openMenuItem.Click += DoOpenPlanner;
            launchMenuItem = new MenuItem() {
                Index = 1,
                Text = "Launch Ikariam"
            };
            launchMenuItem.Click += DoLaunchIkariam;
            quitMenuItem = new MenuItem() {
                Index = 2,
                Text = "Quit"
            };
            quitMenuItem.Click += DoQuit;
            menu = new ContextMenu();
            menu.MenuItems.AddRange(new[] { openMenuItem, launchMenuItem, quitMenuItem });
            notifyIcon = new NotifyIcon(container) {
                ContextMenu = menu,
                Icon = Properties.Resources.AppIcon,
                Text = "Ikariam Planner",
                Visible = true
            };
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    quitMenuItem.Click -= DoQuit;
                    quitMenuItem.Dispose();
                    launchMenuItem.Click -= DoLaunchIkariam;
                    launchMenuItem.Dispose();
                    openMenuItem.Click -= DoOpenPlanner;
                    openMenuItem.Dispose();
                    menu.Dispose();
                    notifyIcon.Dispose();
                    container.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void DoOpenPlanner(object sender, EventArgs e) {
            OpenPlanner?.Invoke();
        }

        private void DoLaunchIkariam(object sender, EventArgs e) {
            Process.Start(new ProcessStartInfo() {
                FileName = "https://lobby.ikariam.gameforge.com/",
                UseShellExecute = true
            });
        }

        private void DoQuit(object sender, EventArgs e) {
            Quit?.Invoke();
        }
    }
}
