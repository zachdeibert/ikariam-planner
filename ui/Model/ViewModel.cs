using System;
using System.ComponentModel;

namespace IkariamPlanner.Model {
    internal class ViewModel : INotifyPropertyChanged {
        public StoredModel Database { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel(StoredModel database) {
            Database = database;
        }
    }
}
