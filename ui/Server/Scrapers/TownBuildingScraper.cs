using System;
using System.Linq;
using System.Xml;
using IkariamPlanner.Model;

namespace IkariamPlanner.Server.Scrapers {
    internal class TownBuildingScraper : IScraper {
        private static readonly int NumberOfPositions = 25;

        public void Scrape(Packet packet, StoredModel model) {
            Town town = model.Towns.First(t => t.Name == packet.TownName);
            for (int i = 0; i <= NumberOfPositions; ++i) {
                XmlNode node = packet.Page.SelectSingleNode($"//html:div[@id=\"position{i}\"]/@class", packet.Xmlns);
                if (node == null && (i == 0 || i == NumberOfPositions)) {
                    return;
                }
                string[] classes = node.InnerText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string levelClass = classes.FirstOrDefault(c => c.StartsWith("level"));
                int level = 0;
                string id = null;
                if (levelClass != null) {
                    level = int.Parse(levelClass.Substring(5));
                    id = classes.First(c => HelpBuildingScraper.ValidBuildingIds.Contains(c));
                }
                TownBuilding building;
                if (town.Buildings.Count > i) {
                    building = town.Buildings[i];
                } else {
                    building = new TownBuilding();
                    town.Buildings.Add(building);
                }
                building.Position = i;
                building.BuildingId = id;
                building.Level = level;
            }
            throw new Exception("Were new building spots added again?");
        }
    }
}
