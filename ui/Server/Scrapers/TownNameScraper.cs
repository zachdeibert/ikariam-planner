using System;
using System.Collections.Generic;
using System.Xml;
using IkariamPlanner.Model;

namespace IkariamPlanner.Server.Scrapers {
    internal class TownNameScraper : IScraper {
        public void Scrape(Packet packet, StoredModel model) {
            IEnumerator<Town> it = model.Towns.GetEnumerator();
            LinkedList<Town> toChange = new LinkedList<Town>();
            foreach (XmlNode node in packet.Page.SelectNodes("//html:div[@id=\"dropDown_js_citySelectContainer\"]//html:a", packet.Xmlns)) {
                Town town;
                if (it.MoveNext()) {
                    town = it.Current;
                } else {
                    town = new Town();
                    toChange.AddLast(town);
                }
                string[] parts = node.InnerText.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Coordinate coords = new Coordinate(parts[0]);
                string name = parts[1];
                if (!town.Coords.Equals(coords) || town.Name != name) {
                    town.Invalidate();
                    town.Coords = coords;
                    town.Name = name;
                }
            }
            foreach (Town town in toChange) {
                model.Towns.Add(town);
            }
            if (toChange.Count == 0) {
                while (it.MoveNext()) {
                    toChange.AddLast(it.Current);
                }
                foreach (Town town in toChange) {
                    model.Towns.Remove(town);
                }
            }
            it.Dispose();
        }
    }
}
