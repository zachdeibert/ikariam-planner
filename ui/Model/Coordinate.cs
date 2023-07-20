using System;
using System.Text.RegularExpressions;

namespace IkariamPlanner.Model {
    public class Coordinate {
        public class XmlCoordinate {
            public int X;
            public int Y;

            public XmlCoordinate() {
            }

            public XmlCoordinate(Coordinate coord) {
                X = coord.X;
                Y = coord.Y;
            }
        }

        private static readonly Regex Regex = new Regex("^\\[?([0-9]+):([0-9]+)\\]?$");

        public readonly int X;
        public readonly int Y;

        public Coordinate(int x, int y) {
            X = x;
            Y = y;
        }

        public Coordinate(XmlCoordinate coord) {
            X = coord.X;
            Y = coord.Y;
        }

        public Coordinate(string coord) {
            Match match = Regex.Match(coord);
            if (!match.Success) {
                throw new ArgumentException("Invalid coordinate");
            }
            X = int.Parse(match.Groups[1].Value);
            Y = int.Parse(match.Groups[2].Value);
        }

        public override string ToString() {
            return $"[{X}:{Y}]";
        }

        public override bool Equals(object obj) {
            return obj is Coordinate coord ? coord.X == X && coord.Y == Y : base.Equals(obj);
        }

        public override int GetHashCode() {
            return X * 100 + Y;
        }
    }
}
