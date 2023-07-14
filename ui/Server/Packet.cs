using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace IkariamPlanner.Server {
    internal class Packet {
        public readonly string Server;
        public readonly IReadOnlyDictionary<string, string> Query;
        public readonly XmlDocument Page;

        public Packet(HttpListenerRequest req) {
            string[] path = req.Url.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            Server = path[path.Length - 1];
            Query = req.Url.Query.Split('&').Select(x => x.Split(new[] { '=' }, 2)).ToDictionary(x => x[0], x => x[1]);
            Page = new XmlDocument();
            Page.Load(req.InputStream);
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Server = '{Server}'");
            sb.AppendLine("Query:");
            foreach (KeyValuePair<string, string> kvp in Query) {
                sb.AppendLine($"    '{kvp.Key}' = '{kvp.Value}'");
            }
            Page.WriteTo(XmlWriter.Create(sb));
            return sb.ToString();
        }
    }
}
