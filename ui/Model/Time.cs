using System;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace IkariamPlanner.Model {
    public class Time {
        public class XmlTime {
            [DefaultValue(0)]
            public int Seconds = 0;
            [DefaultValue(0)]
            public int Minutes = 0;
            [DefaultValue(0)]
            public int Hours = 0;
            [DefaultValue(0)]
            public int Days = 0;

            public XmlTime() {
            }

            public XmlTime(Time time) {
                Seconds = time.Seconds;
                Minutes = time.Minutes;
                Hours = time.Hours;
                Days = time.Days;
            }
        }

        private static readonly Regex Regex = new Regex("^(?:([0-9]+) *D *)?(?:([0-9]+) *h *)?(?:([0-9]+) *m *)?(?:([0-9]+) *s *)?$");

        public readonly int Seconds;
        public readonly int Minutes;
        public readonly int Hours;
        public readonly int Days;

        public TimeSpan Total
            => TimeSpan.FromSeconds(Seconds)
             + TimeSpan.FromMinutes(Minutes)
             + TimeSpan.FromHours(Hours)
             + TimeSpan.FromDays(Days);

        public Time(int seconds, int minutes, int hours, int days) {
            Seconds = seconds;
            Minutes = minutes;
            Hours = hours;
            Days = days;
        }

        public Time(XmlTime time) {
            Seconds = time.Seconds;
            Minutes = time.Minutes;
            Hours = time.Hours;
            Days = time.Days;
        }

        public Time(string time) {
            Match match = Regex.Match(time);
            if (!match.Success) {
                throw new ArgumentException("Invalid time");
            }
            Days = match.Groups[1].Length > 0 ? int.Parse(match.Groups[1].Value) : 0;
            Hours = match.Groups[2].Length > 0 ? int.Parse(match.Groups[2].Value) : 0;
            Minutes = match.Groups[3].Length > 0 ? int.Parse(match.Groups[3].Value) : 0;
            Seconds = match.Groups[4].Length > 0 ? int.Parse(match.Groups[4].Value) : 0;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            if (Days != 0) {
                sb.AppendFormat("{0}D", Days);
            }
            if (Hours != 0) {
                if (sb.Length > 0) {
                    sb.Append(" ");
                }
                sb.AppendFormat("{0}h", Hours);
            }
            if (Minutes != 0) {
                if (sb.Length > 0) {
                    sb.Append(" ");
                }
                sb.AppendFormat("{0}m", Minutes);
            }
            if (Seconds != 0) {
                if (sb.Length > 0) {
                    sb.Append(" ");
                }
                sb.AppendFormat("{0}s", Seconds);
            }
            return sb.ToString();
        }

        public override bool Equals(object obj) {
            return obj is Time time ? time.Total == Total : base.Equals(obj);
        }

        public override int GetHashCode() {
            return Total.GetHashCode();
        }
    }
}
