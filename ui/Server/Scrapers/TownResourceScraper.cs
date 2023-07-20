using System;
using System.Linq;
using IkariamPlanner.Model;

namespace IkariamPlanner.Server.Scrapers {
    internal class TownResourceScraper : IScraper {
        private static readonly (
                Action<StandardResources, ulong> Setter,
                string InWarehouseId,
                string HourlyProductionId,
                string StorageCapacityId,
                string TradingPostId
            )[] Resources = new (
                Action<StandardResources, ulong> Setter,
                string InWarehouseId,
                string HourlyProductionId,
                string StorageCapacityId,
                string TradingPostId)[] {
            (
                (StandardResources sr, ulong v) => sr.Wood = v,
                "js_GlobalMenu_wood_Total",
                "js_GlobalMenu_resourceProduction",
                "js_GlobalMenu_max_wood",
                "js_GlobalMenu_branchOffice_wood"
            ),
            (
                (StandardResources sr, ulong v) => sr.Wine = v,
                "js_GlobalMenu_wine_Total",
                "js_GlobalMenu_production_wine",
                "js_GlobalMenu_max_wine",
                "js_GlobalMenu_branchOffice_wine"
            ),
            (
                (StandardResources sr, ulong v) => sr.Marble = v,
                "js_GlobalMenu_marble_Total",
                "js_GlobalMenu_production_marble",
                "js_GlobalMenu_max_marble",
                "js_GlobalMenu_branchOffice_marble"
            ),
            (
                (StandardResources sr, ulong v) => sr.Crystal = v,
                "js_GlobalMenu_crystal_Total",
                "js_GlobalMenu_production_crystal",
                "js_GlobalMenu_max_crystal",
                "js_GlobalMenu_branchOffice_crystal"
            ),
            (
                (StandardResources sr, ulong v) => sr.Sulfur = v,
                "js_GlobalMenu_sulfur_Total",
                "js_GlobalMenu_production_sulfur",
                "js_GlobalMenu_max_sulfur",
                "js_GlobalMenu_branchOffice_sulfur"
            ),
        };

        public void Scrape(Packet packet, StoredModel model) {
            Town town = model.Towns.First(t => t.Name == packet.TownName);
            town.ResourcesUpdated = DateTime.MinValue;
            foreach ((Action<StandardResources, ulong> setter,
                    string inWarehouseId,
                    string hourlyProductionId,
                    string storageCapacityId,
                    string tradingPostId) in Resources) {
                foreach ((StandardResources res, string id) in new[] {
                    (town.ResourcesInWarehouse, inWarehouseId),
                    (town.ResourcesPerHour, hourlyProductionId),
                    (town.ResourcesCapacity, storageCapacityId),
                    (town.ResourcesInTradingPost, tradingPostId),
                }) {
                    setter(res, Format.ParseInt(packet.Page.SelectSingleNode($"//html:td[@id=\"{id}\"]", packet.Xmlns)));
                }
            }
            town.ResourcesWineUsage = Format.ParseInt(packet.Page.SelectSingleNode("//html:td[@id=\"js_GlobalMenu_WineConsumption\"]", packet.Xmlns));
            town.ResourcesUpdated = DateTime.Now;
        }
    }
}
