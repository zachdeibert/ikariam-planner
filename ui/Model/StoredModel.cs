using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IkariamPlanner.Model {
    [Serializable]
    public class StoredModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public readonly ObservableCollection<Town> Towns = new ObservableCollection<Town>();

        public readonly ObservableCollection<Building> Buildings = new ObservableCollection<Building>();
    }
}
