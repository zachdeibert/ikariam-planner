using System;
using System.Net;
using System.Text;

namespace IkariamPlanner.Server {
    internal class Server : IDisposable {
        private static readonly byte[] successResponse = Encoding.UTF8.GetBytes("OK");
        private readonly HttpListener listener = new HttpListener();
        private bool disposedValue = false;

        public Server() {
            listener.Prefixes.Add("http://*:5357/ikariam-planner/");
            listener.Start();
            StartReceive();
        }

        private void StartReceive() {
            listener.GetContextAsync().ContinueWith(ctx => {
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
                                ctx.Result.Response.ContentLength64 = successResponse.Length;
                                ctx.Result.Response.OutputStream.Write(successResponse, 0, successResponse.Length);
                                break;
                            default:
                                break;
                        }
                        ctx.Result.Response.Close();
                    }
                    if (listener.IsListening) {
                        StartReceive();
                    }
                } catch (AggregateException ex) {
                    if (ex.InnerExceptions.Count != 1 || !(ex.InnerException is HttpListenerException)) {
                        throw;
                    }
                }
                if (pkt != null) {
                    Console.WriteLine(pkt.ToString());
                }
            });
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    listener.Stop();
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
