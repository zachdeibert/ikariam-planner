using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IkariamPlanner.Model {
    public class Building : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private string id;
        public string Id {
            get => id;
            set {
                id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id"));
            }
        }

        public readonly ObservableCollection<BuildingLevel> Levels = new ObservableCollection<BuildingLevel>();
    }
}
