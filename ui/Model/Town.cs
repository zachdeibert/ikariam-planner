using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IkariamPlanner.Model {
    public class Town : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private string name;
        public string Name {
            get => name;
            set {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        private Coordinate coords;
        [XmlIgnore]
        public Coordinate Coords {
            get => coords;
            set {
                coords = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Coords"));
            }
        }
        [XmlElement("Coords")]
        public Coordinate.XmlCoordinate XmlCoords {
            get => new Coordinate.XmlCoordinate(Coords);
            set => Coords = new Coordinate(value);
        }

        private StandardResources resourcesInWarehouse = new StandardResources();
        public StandardResources ResourcesInWarehouse {
            get => resourcesInWarehouse;
            set {
                resourcesInWarehouse = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ResourcesInWarehouse"));
            }
        }

        private StandardResources resourcesPerHour = new StandardResources();
        public StandardResources ResourcesPerHour {
            get => resourcesPerHour;
            set {
                resourcesPerHour = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ResourcesPerHour"));
            }
        }

        private StandardResources resourcesCapacity = new StandardResources();
        public StandardResources ResourcesCapacity {
            get => resourcesCapacity;
            set {
                resourcesCapacity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ResourcesCapacity"));
            }
        }

        private StandardResources resourcesInTradingPost = new StandardResources();
        public StandardResources ResourcesInTradingPost {
            get => resourcesInTradingPost;
            set {
                resourcesInTradingPost = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ResourcesInTradingPost"));
            }
        }

        private ulong resourcesWineUsage = 0;
        public ulong ResourcesWineUsage {
            get => resourcesWineUsage;
            set {
                resourcesWineUsage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ResourcesWineUsage"));
            }
        }

        private DateTime resourcesUpdated = DateTime.MinValue;
        [XmlElement(DataType = "dateTime")]
        public DateTime ResourcesUpdated {
            get => resourcesUpdated;
            set {
                resourcesUpdated = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ResourcesUpdated"));
            }
        }

        public void Invalidate() {
            ResourcesInWarehouse = new StandardResources();
            ResourcesPerHour = new StandardResources();
            ResourcesCapacity = new StandardResources();
            ResourcesInTradingPost = new StandardResources();
            ResourcesWineUsage = 0;
            ResourcesUpdated = DateTime.MinValue;
        }
    }
}
