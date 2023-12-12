using System.Text;

namespace IpToGeo.Utilities
{
    public static class IpFormatter
    {
        /// <summary>
        /// IPv4 to decimal
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static uint Ipv4ToNum(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return uint.Parse(items[0]) << 24
                    | uint.Parse(items[1]) << 16
                    | uint.Parse(items[2]) << 8
                    | uint.Parse(items[3]);
        }

        /// <summary>
        /// decimal to IPv4
        /// </summary>
        /// <param name="ipInt"></param>
        /// <returns></returns>
        public static string IntToIpv4(uint ipInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((ipInt >> 24) & 0xFF).Append(".");
            sb.Append((ipInt >> 16) & 0xFF).Append(".");
            sb.Append((ipInt >> 8) & 0xFF).Append(".");
            sb.Append(ipInt & 0xFF);
            return sb.ToString();
        }
    }
}
