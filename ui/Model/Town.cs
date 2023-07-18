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

        public void Invalidate() {
        }
    }
}
