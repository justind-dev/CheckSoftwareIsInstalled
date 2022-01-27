using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

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

    public static IPAddress GetHostIpAddress(string hostName)
    {
        try
        {
            var host = Dns.GetHostEntry(hostName);
            var hostIp = host.AddressList.FirstOrDefault().MapToIPv4();
            if (hostIp == null)
            {
                return null;
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

    public static bool IsReachable(string computerName)
    {
        var computerIP = GetHostIpAddress(computerName);
        if (computerIP == null)
        {
            return false;
        }
        if (!GetHostPingable(computerIP))
        {
            return false;
        }

        return true;
    }
}