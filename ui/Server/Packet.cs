using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace IkariamPlanner.Server {
    internal class Packet {
        public readonly string server;
        public readonly IReadOnlyDictionary<string, string> query;
        public readonly XmlDocument page;

        public Packet(HttpListenerRequest req) {
            string[] path = req.Url.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            server = path[path.Length - 1];
            query = req.Url.Query.Split('&').Select(x => x.Split(new[] { '=' }, 2)).ToDictionary(x => x[0], x => x[1]);
            page = new XmlDocument();
            page.Load(req.InputStream);
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Server = '{server}'");
            sb.AppendLine("Query:");
            foreach (KeyValuePair<string, string> kvp in query) {
                sb.AppendLine($"    '{kvp.Key}' = '{kvp.Value}'");
            }
            page.WriteTo(XmlWriter.Create(sb));
            return sb.ToString();
        }
    }
}
