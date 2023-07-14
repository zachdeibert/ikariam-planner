using System;
using System.Net;
using System.Text;

namespace IkariamPlanner.Server {
    internal class Server : IDisposable {
        private static readonly byte[] SuccessResponse = Encoding.UTF8.GetBytes("OK");
        private readonly HttpListener Listener = new HttpListener();
        private bool DisposedValue = false;

        public event Action PacketReceived;

        public Server() {
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
                    Console.WriteLine(pkt.ToString());
                    PacketReceived?.Invoke();
                }
            });
        }

        protected virtual void Dispose(bool disposing) {
            if (!DisposedValue) {
                if (disposing) {
                    Listener.Stop();
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
