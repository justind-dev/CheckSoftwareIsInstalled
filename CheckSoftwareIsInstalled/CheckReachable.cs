using System.Net;

namespace CheckSoftwareIsInstalled;

public class CheckReachable
{
    public static List<Computer> CheckComputers(List<string> adComputers)
    {
        IPAddress computerIpAddress;
        bool computerReachable;
        bool computerCarbonBlackInstalled;
        bool computerMcAfeeInstalled;

        List<Computer> computers = new List<Computer>();

        foreach (string computerName in adComputers)
        {
            computerIpAddress = NetworkHelper.GetHostIpAddress(computerName);

            computerReachable = NetworkHelper.IsReachable(computerName);

            if (computerReachable && CheckSoftware.CheckInstalled("Carbon Black Cloud Sensor", computerName).Length > 0)
            {
                computerCarbonBlackInstalled = true;
            }
            else
            {
                computerCarbonBlackInstalled = false;
            }

            if (computerReachable && CheckSoftware.CheckInstalled("McAfee", computerName).Length > 0)
            {
                computerMcAfeeInstalled = true;
            }
            else
            {
                computerMcAfeeInstalled = false;
            }

            var computer = new Computer(computerName,
                computerIpAddress.ToString(),
                computerReachable,
                computerCarbonBlackInstalled,
                computerMcAfeeInstalled);

            computers.Add(computer);
        }

        return computers;
    }
}