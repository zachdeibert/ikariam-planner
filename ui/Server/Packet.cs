﻿using System;
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
        public readonly XmlNamespaceManager Xmlns;

        public Packet(HttpListenerRequest req) {
            string[] path = req.Url.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            Server = path[path.Length - 1];
            Query = req.Url.Query.Split('&').Select(x => x.Split(new[] { '=' }, 2)).ToDictionary(x => x[0], x => x[1]);
            Page = new XmlDocument();
            Page.Load(req.InputStream);
            Xmlns = new XmlNamespaceManager(Page.NameTable);
            Xmlns.AddNamespace("html", "http://www.w3.org/1999/xhtml");
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

        private string townName = null;
        public string TownName {
            get {
                if (townName == null) {
                    townName = Page.SelectSingleNode("//html:div[@id=\"js_citySelectContainer\"]//html:a", Xmlns).InnerText.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                }
                return townName;
            }
        }
    }
}
