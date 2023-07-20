using System;
using System.ComponentModel;

namespace IkariamPlanner.Model {
    public class TownBuilding : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private int position = 0;
        public int Position {
            get => position;
            set {
                position = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Position"));
            }
        }

        private string buildingId = null;
        public string BuildingId {
            get => buildingId;
            set {
                buildingId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BuildingId"));
            }
        }

        private int level = 0;
        public int Level {
            get => level;
            set {
                level = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Level"));
            }
        }
    }
}
