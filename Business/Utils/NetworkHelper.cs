using System;
using System.Net.NetworkInformation;

namespace Business.Utils
{
    internal static class NetworkHelper
    {
        public static bool IsAvailable
        {
            get
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                    return false;

                foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.OperationalStatus != OperationalStatus.Up ||
                        ni.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                        ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                        continue;

                    const long minimumSpeed = 0;
                    if (ni.Speed < minimumSpeed) continue;

                    if ((ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
                        continue;

                    if (ni.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
                        continue;

                    return true;
                }
                return false;
            }
        }
    }
}
