using System.Net;

namespace CheckSoftwareIsInstalled;

public class Computer
{
    public string HostName { get; set; }
    public string HostIpAddress { get; set; }
    public bool HostPingable { get; set; }
    public bool HostSmbReachable { get; set; }
    public bool HostCarbonBlackInstalled { get; set; }
    public bool HostMcAfeeInstalled { get; set; }

    public Computer(string hostName, string hostIpAddress, bool hostPingable, bool hostSmbReachable, 
                    bool hostCarbonBlackInstalled, bool hostMcAfeeInstalled)
    {
        HostName = hostName;
        HostIpAddress = hostIpAddress;
        HostPingable = hostPingable;
        HostSmbReachable = hostSmbReachable;
        HostCarbonBlackInstalled = hostCarbonBlackInstalled;
        HostMcAfeeInstalled = hostMcAfeeInstalled;
    }
}