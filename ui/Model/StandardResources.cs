using System;
using System.ComponentModel;

namespace IkariamPlanner.Model {
    public class StandardResources : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private ulong wood = 0;
        [DefaultValue(0)]
        public ulong Wood {
            get => wood;
            set {
                wood = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Wood"));
            }
        }

        private ulong wine = 0;
        [DefaultValue(0)]
        public ulong Wine {
            get => wine;
            set {
                wine = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Wine"));
            }
        }

        private ulong marble = 0;
        [DefaultValue(0)]
        public ulong Marble {
            get => marble;
            set {
                marble = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Marble"));
            }
        }

        private ulong crystal = 0;
        [DefaultValue(0)]
        public ulong Crystal {
            get => crystal;
            set {
                crystal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Crystal"));
            }
        }

        private ulong sulfur = 0;
        [DefaultValue(0)]
        public ulong Sulfur {
            get => sulfur;
            set {
                sulfur = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sulfur"));
            }
        }

        public override bool Equals(object obj) {
            return obj is StandardResources resources &&
                   Wood == resources.Wood &&
                   Wine == resources.Wine &&
                   Marble == resources.Marble &&
                   Crystal == resources.Crystal &&
                   Sulfur == resources.Sulfur;
        }

        public override int GetHashCode() {
            int hashCode = 623363513;
            hashCode = hashCode * -1521134295 + Wood.GetHashCode();
            hashCode = hashCode * -1521134295 + Wine.GetHashCode();
            hashCode = hashCode * -1521134295 + Marble.GetHashCode();
            hashCode = hashCode * -1521134295 + Crystal.GetHashCode();
            hashCode = hashCode * -1521134295 + Sulfur.GetHashCode();
            return hashCode;
        }
    }
}
