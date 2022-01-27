using System.Text;
namespace CheckSoftwareIsInstalled;

public static class DataWriter
{
    public static void WriteComputersToFile(string filePath, List<Computer> _computers)
    {
        var csv = new StringBuilder();
        var headerLine = string.Format("HOSTNAME,IPADDRESS,REACHABLE,CB_INSTALLED,MCAFEE_INSTALLED");
        csv.AppendLine(headerLine);

        foreach (Computer computer in _computers)
        {
            var cName = computer.HostName;
            var cIpAddress = computer.HostIpAddress;
            var cIsReachable = (computer.HostIsReachable) ? "true" : "false";
            var cCarbonBlackInstalled = (computer.HostCarbonBlackInstalled) ? "true" : "false";
            var cMcAfeeInstalled = (computer.HostMcAfeeInstalled) ? "true" : "false";
            var newLine = string.Format("{0},{1},{2},{3},{4}",
                cName,
                cIpAddress,
                cIsReachable,
                cCarbonBlackInstalled,
                cMcAfeeInstalled);

            csv.AppendLine(newLine);
        }

        File.WriteAllText(filePath, csv.ToString());
    }
}