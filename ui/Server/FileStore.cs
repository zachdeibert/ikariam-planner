using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using IkariamPlanner.Model;

namespace IkariamPlanner.Server {
    internal class FileStore : IDisposable {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(StoredModel));
        private static readonly string DataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ikariam-planner");
        private static readonly string SaveFile = Path.Combine(DataDir, "data.xml");
        private static readonly string NewFile = Path.Combine(DataDir, "data-new.xml");
        private static readonly string OldFile = Path.Combine(DataDir, "data-old.xml");
        private const int MaxPacketLogs = 10;
        private static readonly string PacketLogFileName = "debug-{0}.html";
        private static readonly TimeSpan SaveAfterInactivity = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan SaveAfterUpdate = TimeSpan.FromMinutes(2);

        private readonly Queue<Packet> DebugPackets = new Queue<Packet>();
        private readonly Timer SaveTimer;
        private readonly object SaveProcessLock = new object();
        private readonly object SaveTimerLock = new object();
        private bool DisposedValue = false;
        private DateTime LatestSaveTime;
        private StoredModel ToSave = null;

        public FileStore() {
            SaveTimer = new Timer(SaveNow);
            if (!Directory.Exists(DataDir)) {
                Directory.CreateDirectory(DataDir);
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (!DisposedValue) {
                if (disposing) {
                    SaveTimer.Dispose();
                }
                SaveNow(null);
                DisposedValue = true;
            }
        }

        ~FileStore() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public StoredModel LoadModel() {
            try {
                using (FileStream stream = new FileStream(SaveFile, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    return Serializer.Deserialize(stream) as StoredModel;
                }
            } catch (FileNotFoundException) {
                return new StoredModel();
            }
        }

        public void SaveModel(StoredModel model) {
            lock (SaveTimerLock) {
                SaveTimer.Change(Timeout.Infinite, Timeout.Infinite);
                DateTime now;
                lock (SaveProcessLock) {
                    now = DateTime.Now;
                    if (ToSave == null) {
                        ToSave = model;
                        LatestSaveTime = now + SaveAfterUpdate;
                    } else if (ToSave != model) {
                        throw new ArgumentException("Each call to SaveModel() should have the same argument");
                    }
                }
                TimeSpan untilSave = LatestSaveTime - now;
                if (untilSave > SaveAfterInactivity) {
                    untilSave = SaveAfterInactivity;
                }
                SaveTimer.Change((int) untilSave.TotalMilliseconds, Timeout.Infinite);
            }
        }

        public void SavePacket(Packet packet) {
            lock (SaveProcessLock) {
                DebugPackets.Enqueue(packet);
                if (DebugPackets.Count > MaxPacketLogs) {
                    DebugPackets.Dequeue();
                }
            }
        }

        private void SaveNow(object state) {
            lock (SaveProcessLock) {
                if (ToSave == null) {
                    return;
                }
                using (FileStream stream = new FileStream(NewFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)) {
                    Serializer.Serialize(stream, ToSave);
                }
                if (File.Exists(SaveFile)) {
                    File.Replace(NewFile, SaveFile, OldFile, true);
                } else {
                    File.Move(NewFile, SaveFile);
                }
                if (Debugger.IsAttached) {
                    for (int i = 0; DebugPackets.Count > 0; ++i) {
                        using (FileStream stream = new FileStream(Path.Combine(DataDir, string.Format(PacketLogFileName, i)), FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read)) {
                            DebugPackets.Dequeue().Page.Save(stream);
                        }
                    }
                }
                ToSave = null;
            }
        }
    }
}
