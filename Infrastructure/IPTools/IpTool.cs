using IP2Region.Net.Abstractions;
using System;
using ZR.Infrastructure.IPTools.Model;

namespace ZR.Infrastructure.IPTools
{
    public class IpTool
    {
        private static ISearcher? _searcher;

        public static void Configure(ISearcher searcher)
        {
            _searcher = searcher ?? throw new ArgumentNullException(nameof(searcher));
        }

        private static ISearcher GetSearcher()
        {
            return _searcher ?? throw new InvalidOperationException("IpTool is not initialized. Please configure it in Program.cs by calling IpTool.Configure(...).");
        }

        public static string GetRegion(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                throw new ArgumentException("IP为空", nameof(ip));
            }

            try
            {
                return GetSearcher().Search(ip);
            }
            catch (Exception ex)
            {
                throw new Exception($"搜索IP异常IP={ip}", ex);
            }
        }

        public static IpInfo Search(string ip)
        {
            try
            {
                var region = GetRegion(ip);
                var array = region.Split('|');

                return new IpInfo
                {
                    Country = array.Length > 0 ? array[0] : string.Empty,
                    Province = array.Length > 1 ? array[1] : string.Empty,
                    City = array.Length > 2 ? array[2] : string.Empty,
                    NetworkOperator = array.Length > 3 ? array[3] : string.Empty,
                    IpAddress = ip
                };
            }
            catch (Exception e)
            {
                throw new Exception("Error converting ip address information to ipinfo object", e);
            }
        }
    }
}
