using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using GameCore;
using System.Net.Http;
using Log = Exiled.API.Features.Log;
namespace VpnChecker
{
    public class VpnChecker
    {
        private static IntervalTree itree;
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly int timeoutMs = 20000;

        public static async Task UpdateListAsync()
        {
            var cts = new System.Threading.CancellationTokenSource(timeoutMs);
            try
            {
                var ipv4CidrRanges = await httpClient.GetStringAsync("https://raw.githubusercontent.com/josephrocca/is-vpn/main/vpn-or-datacenter-ipv4-ranges.txt");
                var ipv4CidrList = ipv4CidrRanges.Trim().Split('\n');
                itree = new IntervalTree();
                foreach (var range in ipv4CidrList.Select(ipv4CidrToRange))
                {
                    itree.Insert(range);
                }
                Log.Info("Vpn Ranges Got!");
            }
            catch (Exception e)
            {
                Log.Error("Failed to get VPN ip ranges!");
            }
        }


        private static int IpToBinary(string ip)
        {
            return ip.Split('.').Aggregate(0, (acc, octet) => (acc << 8) + int.Parse(octet));
        }

        private static Range ipv4CidrToRange(string cidr)
        {
            var parts = cidr.Split('/');
            var baseIp = parts[0];
            var subnetMask = int.Parse(parts[1]);

            var ipBinary = IpToBinary(baseIp);
            var rangeStart = ipBinary;
            var rangeEnd = ipBinary | ((1 << (32 - subnetMask)) - 1);

            return new Range(rangeStart, rangeEnd);
        }

        public static bool IsVpn(string ip)
        {
            var ipBinary = IpToBinary(ip);
            return itree.Query(ipBinary);
        }
    }

    public class IntervalTree
    {
        private List<Range> ranges = new List<Range>();

        public void Insert(Range range)
        {
            ranges.Add(range);
        }

        public bool Query(int value)
        {
            return ranges.Any(range => range.Start <= value && range.End >= value);
        }
    }

    public struct Range
    {
        public int Start { get; }
        public int End { get; }

        public Range(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}
