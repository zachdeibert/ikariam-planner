using System;
using System.ComponentModel;

namespace IkariamPlanner.Model {
    [Serializable]
    public class StoredModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
