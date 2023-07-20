using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using IkariamPlanner.Model;

namespace IkariamPlanner.Server.Scrapers {
    internal class HelpBuildingScraper : IScraper {
        private static readonly Regex AllowUnitsTitle = new Regex("^To the description of (.*)$");
        private static readonly Action<BuildingLevel, XmlNode> ColumnWood = (model, node) => model.Resources.Wood = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnWine = (model, node) => model.Resources.Wine = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnMarble = (model, node) => model.Resources.Marble = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnCrystal = (model, node) => model.Resources.Crystal = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnSulfur = (model, node) => model.Resources.Crystal = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnTime = (model, node) => model.Time = new Time(node.InnerText.Trim());
        private static readonly Action<BuildingLevel, XmlNode> ColumnMaxCitizens = (model, node) => model.MaxCitizens = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnMaxScientists = (model, node) => model.MaxScientists = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnStorageCapacity = (model, node) => model.StorageCapacity = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnMaxWine = (model, node) => model.MaxWine = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnSatisfactionFromBuilding = (model, node) => model.SatisfactionFromBuilding = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnSatisfactionFromWine = (model, node) => model.SatisfactionFromWine = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnSatisfactionFromCulture = (model, node) => model.SatisfactionFromCulture = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnLoadingSpeed = (model, node) => model.LoadingSpeed = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnDiplomacyPoints = (model, node) => model.DiplomacyPoints = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnMaxPriests = (model, node) => model.MaxPriests = Format.ParseInt(node);
        private static readonly Action<BuildingLevel, XmlNode> ColumnTaxBreakPercent = (model, node) => model.TaxBreakPercent = Format.ParsePercent(node);
        private static readonly Dictionary<string, Action<BuildingLevel, XmlNode>[]> ColumnDefinitions = new Dictionary<string, Action<BuildingLevel, XmlNode>[]> {
            { "townHall", new [] { ColumnWood, ColumnMarble, ColumnTime, ColumnMaxCitizens } },
            { "academy", new [] { ColumnWood, ColumnCrystal, ColumnTime, ColumnMaxScientists } },
            { "warehouse", new [] { ColumnWood, ColumnMarble, ColumnTime, ColumnStorageCapacity } },
            { "tavern", new [] { ColumnWood, ColumnMarble, ColumnTime, ColumnMaxWine, ColumnSatisfactionFromBuilding, ColumnSatisfactionFromWine } },
            { "palace", new [] { ColumnWood, ColumnWine, ColumnMarble, ColumnCrystal, ColumnSulfur, ColumnTime } },
            { "palaceColony", new [] { ColumnWood, ColumnWine, ColumnMarble, ColumnCrystal, ColumnSulfur, ColumnTime } },
            { "museum", new [] { ColumnWood, ColumnMarble, ColumnTime, ColumnSatisfactionFromBuilding, ColumnSatisfactionFromCulture } },
            { "port", new [] { ColumnWood, ColumnMarble, ColumnTime, ColumnLoadingSpeed } },
            { "shipyard", new [] { ColumnWood, ColumnMarble, ColumnTime, ColumnAllowUnits } },
            { "barracks", new [] { ColumnWood, ColumnMarble, ColumnTime, ColumnAllowUnits } },
            { "wall", new [] { ColumnWood, ColumnMarble, ColumnTime } },
            { "embassy", new [] { ColumnWood, ColumnMarble, ColumnTime, ColumnDiplomacyPoints } },
            { "branchOffice", new [] { ColumnWood, ColumnMarble, ColumnTime, ColumnStorageCapacity } },
            { "workshop", new [] { ColumnWood, ColumnMarble, ColumnTime } },
            { "safehouse", new [] { ColumnWood, ColumnMarble, ColumnTime } },
            { "forester", new [] { ColumnWood, ColumnMarble, ColumnTime } },
            { "glassblowing", new [] { ColumnWood, ColumnMarble, ColumnTime } },
            { "alchemist", new [] { ColumnWood, ColumnMarble, ColumnTime } },
            { "winegrower", new [] { ColumnWood, ColumnMarble, ColumnTime } },
            { "stonemason", new [] { ColumnWood, ColumnMarble, ColumnTime } },
            { "carpentering", new [] { ColumnWood, ColumnMarble, ColumnTime } },
            { "optician", new [] { ColumnWood, ColumnMarble, ColumnTime } },
            { "fireworker", new [] { ColumnWood, ColumnMarble, ColumnTime } },
            { "vineyard", new [] { ColumnWood, ColumnMarble, ColumnTime } },
            { "architect", new [] { ColumnWood, ColumnMarble, ColumnTime } },
            { "temple", new [] { ColumnWood, ColumnCrystal, ColumnTime, ColumnMaxPriests } },
            { "dump", new [] { ColumnWood, ColumnMarble, ColumnCrystal, ColumnSulfur, ColumnTime, ColumnStorageCapacity } },
            { "pirateFortress", new [] { ColumnWood, ColumnMarble, ColumnTime } },
            { "blackMarket", new [] { ColumnWood, ColumnMarble, ColumnTime, ColumnTaxBreakPercent, ColumnAllowUnits } },
            { "marineChartArchive", new [] { ColumnWood, ColumnMarble, ColumnCrystal, ColumnTime } },
            { "dockyard", new [] { ColumnWood, ColumnMarble, ColumnCrystal, ColumnTime } },
        };

        private static void ColumnAllowUnits(BuildingLevel model, XmlNode node) {
            foreach (XmlNode child in node.ChildNodes) {
                if (child.NodeType == XmlNodeType.Element && child.Name == "a") {
                    model.AllowsUnits.Add(AllowUnitsTitle.Match(child.Attributes["title"].Value.Trim()).Groups[1].Value);
                }
            }
        }

        public void Scrape(Packet packet, StoredModel model) {
            XmlNode page = packet.Page.SelectSingleNode("//html:div[@id=\"buildingDetail\"]", packet.Xmlns);
            if (page == null) {
                return;
            }
            XmlNode selected = page.SelectSingleNode(".//html:div[contains(@class, \"button_building\") and contains(@class, \"selected\")]/@class", packet.Xmlns);
            if (selected == null) {
                return;
            }
            string[] classes = selected.Value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Action<BuildingLevel, XmlNode>[] columns = null;
            Building building = null;
            foreach (string className in classes) {
                if (ColumnDefinitions.TryGetValue(className, out Action<BuildingLevel, XmlNode>[] cols)) {
                    if (columns == null) {
                        columns = cols;
                        building = model.Buildings.FirstOrDefault(b => b.Id == className);
                        if (building == null) {
                            building = new Building() {
                                Id = className
                            };
                            model.Buildings.Add(building);
                        }
                    } else {
                        throw new Exception("Multiple classes found in column definition");
                    }
                }
            }
            if (columns == null) {
                throw new Exception("Unknown building class found");
            }
            building.Levels.Clear();
            ulong level = 0;
            foreach (XmlNode tr in page.SelectNodes(".//html:div[@class=\"content\"]//html:table[@class=\"table01 center\"]//html:tr", packet.Xmlns)) {
                if (level > 0) {
                    XmlNodeList tds = tr.SelectNodes("./html:td", packet.Xmlns);
                    if (tds.Count != columns.Length + 1) {
                        throw new Exception("Invalid column count");
                    }
                    if (Format.ParseInt(tds[0]) != level) {
                        throw new Exception("Bad building level number");
                    }
                    BuildingLevel lvl = new BuildingLevel();
                    foreach ((XmlNode node, Action<BuildingLevel, XmlNode> col) in tds.OfType<XmlNode>().Skip(1).Zip(columns, (a, b) => (a, b))) {
                        col(lvl, node);
                    }
                    building.Levels.Add(lvl);
                }
                ++level;
            }
        }
    }
}
