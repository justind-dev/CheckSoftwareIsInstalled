using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace CheckSoftwareIsInstalled;

public static class NetworkHelper
{
    public static bool SmbPortReachable(IPAddress hostIpAddress)
    {
        using (TcpClient tcpClient = new TcpClient())
        {
            try
            {
                IPEndPoint ipe = new IPEndPoint(hostIpAddress, 445);
                tcpClient.Connect(ipe);
                tcpClient.Close();
                return true;
            }
            catch (SocketException ex)
            {
                tcpClient.Close();
                return false;
            }
        }
    }

    public static string GetHostIpAddress(string hostName)
    {
        try
        {
            var host = Dns.GetHostEntry(hostName);
            var hostIp = host.AddressList.FirstOrDefault().MapToIPv4().ToString();
            if (String.IsNullOrWhiteSpace(hostIp))
            {
                return "";
            }
            
            return hostIp;
            
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static bool GetHostPingable(IPAddress hostIpAddress)
    {
        var pinger = new Ping();
        PingReply reply;
        reply = pinger.Send(hostIpAddress, 1000);
        if (reply.Status != IPStatus.Success)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static string GetLocalDomainName()
    {
        try
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            var domainName = properties.DomainName;
            return domainName;
        }
        catch (Exception e)
        {
            return "";
        }
        
        
    }
}