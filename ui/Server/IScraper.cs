using System;
using IkariamPlanner.Model;

namespace IkariamPlanner.Server {
    internal interface IScraper {
        void Scrape(Packet packet, StoredModel model);
    }
}
