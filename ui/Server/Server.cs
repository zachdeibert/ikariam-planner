using System;
using System.Net;
using System.Text;
using IkariamPlanner.Model;
using IkariamPlanner.Server.Scrapers;

namespace IkariamPlanner.Server {
    internal class Server : IDisposable {
        private static readonly IScraper[] Scrapers = new IScraper[] {
            new HelpBuildingScraper(),
            new TownBuildingScraper(),
            new TownNameScraper(),
            new TownResourceScraper()
        };

        private static readonly byte[] SuccessResponse = Encoding.UTF8.GetBytes("OK");
        private readonly HttpListener Listener = new HttpListener();
        private readonly FileStore FileStore = new FileStore();
        private bool DisposedValue = false;

        public readonly StoredModel Model;
        public event Action PacketReceived;

        public Server() {
            Model = FileStore.LoadModel();
            Listener.Prefixes.Add("http://*:5357/ikariam-planner/");
            Listener.Start();
            StartReceive();
        }

        private void StartReceive() {
            Listener.GetContextAsync().ContinueWith(ctx => {
                Packet pkt = null;
                try {
                    if (ctx.IsCompleted) {
                        ctx.Result.Response.AddHeader("Access-Control-Allow-Origin", "*");
                        switch (ctx.Result.Request.HttpMethod) {
                            case "OPTIONS":
                                ctx.Result.Response.AddHeader("Access-Control-Allow-Methods", "POST");
                                break;
                            case "POST":
                                pkt = new Packet(ctx.Result.Request);
                                ctx.Result.Response.ContentLength64 = SuccessResponse.Length;
                                ctx.Result.Response.OutputStream.Write(SuccessResponse, 0, SuccessResponse.Length);
                                break;
                            default:
                                break;
                        }
                        ctx.Result.Response.Close();
                    }
                    if (Listener.IsListening) {
                        StartReceive();
                    }
                } catch (AggregateException ex) {
                    if (ex.InnerExceptions.Count != 1 || !(ex.InnerException is HttpListenerException)) {
                        throw;
                    }
                }
                if (pkt != null) {
                    FileStore.SavePacket(pkt);
                    FileStore.SaveModel(Model);
                    PacketReceived?.Invoke();
                    foreach (IScraper scraper in Scrapers) {
                        scraper.Scrape(pkt, Model);
                    }
                }
            });
        }

        protected virtual void Dispose(bool disposing) {
            if (!DisposedValue) {
                if (disposing) {
                    Listener.Stop();
                    FileStore.Dispose();
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
