using System;
using System.Globalization;
using System.Xml;

namespace IkariamPlanner.Model {
    internal class Format {
        public static ulong ParseInt(XmlNode node) {
            string text = node.InnerText.Trim();
            ulong factor = 1;
            if (text.Length == 0 || text == "-") {
                return 0;
            } else if (text.EndsWith("k")) {
                factor = 1000;
                text = text.Remove(text.Length - 1);
            }
            return ulong.Parse(text.Trim(), NumberStyles.AllowThousands) * factor;
        }

        public static int ParsePercent(XmlNode node) {
            string text = node.InnerText.Trim();
            if (!text.EndsWith("%")) {
                throw new FormatException("Missing '%'");
            }
            text.Remove(text.Length - 1);
            return int.Parse(text.Trim());
        }
    }
}
