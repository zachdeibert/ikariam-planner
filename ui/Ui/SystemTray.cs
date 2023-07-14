using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace IkariamPlanner.Ui {
    internal class SystemTray : IDisposable {
        private readonly Container Container;
        private readonly MenuItem OpenMenuItem;
        private readonly MenuItem LaunchMenuItem;
        private readonly MenuItem QuitMenuItem;
        private readonly ContextMenu Menu;
        private readonly NotifyIcon NotifyIcon;
        private bool DisposedValue = false;

        public event Action OpenPlanner;
        public event Action Quit;

        public SystemTray() {
            Container = new Container();
            OpenMenuItem = new MenuItem() {
                Index = 0,
                Text = "Open Planner"
            };
            OpenMenuItem.Click += DoOpenPlanner;
            LaunchMenuItem = new MenuItem() {
                Index = 1,
                Text = "Launch Ikariam"
            };
            LaunchMenuItem.Click += DoLaunchIkariam;
            QuitMenuItem = new MenuItem() {
                Index = 2,
                Text = "Quit"
            };
            QuitMenuItem.Click += DoQuit;
            Menu = new ContextMenu();
            Menu.MenuItems.AddRange(new[] { OpenMenuItem, LaunchMenuItem, QuitMenuItem });
            NotifyIcon = new NotifyIcon(Container) {
                ContextMenu = Menu,
                Icon = Properties.Resources.AppIcon,
                Text = "Ikariam Planner",
                Visible = true
            };
        }

        protected virtual void Dispose(bool disposing) {
            if (!DisposedValue) {
                if (disposing) {
                    QuitMenuItem.Click -= DoQuit;
                    QuitMenuItem.Dispose();
                    LaunchMenuItem.Click -= DoLaunchIkariam;
                    LaunchMenuItem.Dispose();
                    OpenMenuItem.Click -= DoOpenPlanner;
                    OpenMenuItem.Dispose();
                    Menu.Dispose();
                    NotifyIcon.Dispose();
                    Container.Dispose();
                }
                DisposedValue = true;
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
