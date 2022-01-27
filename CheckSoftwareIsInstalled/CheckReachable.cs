using System.Net;

namespace CheckSoftwareIsInstalled;

public class CheckReachable
{
    public static List<Computer> CheckComputers(List<string> adComputers)
    {
        int totalComputers = adComputers.Count();
        int count = 0;
        string computerIpAddress;
        bool computerReachable;
        bool computerCarbonBlackInstalled;
        bool computerMcAfeeInstalled;

        List<Computer> computers = new List<Computer>();

        foreach (string computerName in adComputers)
        {
            Console.WriteLine($"Checking {count} / {totalComputers}");
            try
            {

                var ipAddress = NetworkHelper.GetHostIpAddress(computerName);
                computerIpAddress = ipAddress.MapToIPv4().ToString();
            }
            catch
            {
                computerIpAddress = "UNREACHABLE";
            }
            
            
            computerReachable = NetworkHelper.IsReachable(computerName);

            if (computerReachable && CheckSoftware.CheckInstalled("Carbon Black Cloud Sensor", computerName))
            {
                computerCarbonBlackInstalled = true;
            }
            else
            {
                computerCarbonBlackInstalled = false;
            }

            if (computerReachable && CheckSoftware.CheckInstalled("DOESNOTEXIST", computerName))
            {
                computerMcAfeeInstalled = true;
            }
            else
            {
                computerMcAfeeInstalled = false;
            }

            var computer = new Computer(computerName,
                computerIpAddress,
                computerReachable,
                computerCarbonBlackInstalled,
                computerMcAfeeInstalled);
            computers.Add(computer);
            Console.WriteLine($"{computerName} | Carbon Black Installed {computerCarbonBlackInstalled} | McAfee Installed {computerMcAfeeInstalled}");
            count++;
        }

        return computers;
    }
}