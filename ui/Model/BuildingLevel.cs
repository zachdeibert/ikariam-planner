using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IkariamPlanner.Model {
    public class BuildingLevel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private StandardResources resources = new StandardResources();
        public StandardResources Resources {
            get => resources;
            set {
                resources = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Resources"));
            }
        }

        private Time time = new Time(0, 0, 0, 0);
        [XmlIgnore]
        public Time Time {
            get => time;
            set {
                time = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
            }
        }
        [XmlElement("Time")]
        public Time.XmlTime XmlTime {
            get => new Time.XmlTime(Time);
            set => Time = new Time(value);
        }

        private ulong maxCitizens = 0;
        [DefaultValue(0)]
        public ulong MaxCitizens {
            get => maxCitizens;
            set {
                maxCitizens = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxCitizens"));
            }
        }

        private ulong maxScientists = 0;
        [DefaultValue(0)]
        public ulong MaxScientists {
            get => maxScientists;
            set {
                maxScientists = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxScientists"));
            }
        }

        private ulong storageCapacity = 0;
        [DefaultValue(0)]
        public ulong StorageCapacity {
            get => storageCapacity;
            set {
                storageCapacity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StorageCapacity"));
            }
        }

        private ulong maxWine = 0;
        [DefaultValue(0)]
        public ulong MaxWine {
            get => maxWine;
            set {
                maxWine = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxWine"));
            }
        }

        private ulong satisfactionFromBuilding = 0;
        [DefaultValue(0)]
        public ulong SatisfactionFromBuilding {
            get => satisfactionFromBuilding;
            set {
                satisfactionFromBuilding = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SatisfactionFromBuilding"));
            }
        }

        private ulong satisfactionFromWine = 0;
        [DefaultValue(0)]
        public ulong SatisfactionFromWine {
            get => satisfactionFromWine;
            set {
                satisfactionFromWine = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SatisfactionFromWine"));
            }
        }

        private ulong satisfactionFromCulture = 0;
        [DefaultValue(0)]
        public ulong SatisfactionFromCulture {
            get => satisfactionFromCulture;
            set {
                satisfactionFromCulture = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SatisfactionFromCulture"));
            }
        }

        private ulong loadingSpeed = 0;
        [DefaultValue(0)]
        public ulong LoadingSpeed {
            get => loadingSpeed;
            set {
                loadingSpeed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LoadingSpeed"));
            }
        }

        public readonly ObservableCollection<string> AllowsUnits = new ObservableCollection<string>();

        private ulong diplomacyPoints = 0;
        [DefaultValue(0)]
        public ulong DiplomacyPoints {
            get => diplomacyPoints;
            set {
                diplomacyPoints = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DiplomacyPoints"));
            }
        }

        private ulong maxPriests = 0;
        [DefaultValue(0)]
        public ulong MaxPriests {
            get => maxPriests;
            set {
                maxPriests = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxPriests"));
            }
        }

        private int taxBreakPercent = 0;
        [DefaultValue(0)]
        public int TaxBreakPercent {
            get => taxBreakPercent;
            set {
                taxBreakPercent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TaxBreakPercent"));
            }
        }
    }
}
