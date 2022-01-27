using System.Net;

namespace CheckSoftwareIsInstalled;

public class Computer
{
    public string HostName { get; set; }
    public string HostIpAddress { get; set; }
    public bool HostIsReachable { get; set; }
    public bool HostCarbonBlackInstalled { get; set; }
    public bool HostMcAfeeInstalled { get; set; }

    public Computer(string hostName, string hostIpAddress, bool hostIsReachable, 
                    bool hostCarbonBlackInstalled, bool hostMcAfeeInstalled)
    {
        HostName = hostName;
        HostIpAddress = hostIpAddress;
        HostIsReachable = hostIsReachable;
        HostCarbonBlackInstalled = hostCarbonBlackInstalled;
        HostMcAfeeInstalled = hostMcAfeeInstalled;
    }
}