using System.Net;
using System.Text;
using Microsoft.Win32;

namespace CheckSoftwareIsInstalled;

public class Cli
{
    public class CommandLineInterface
    {
        private List<string> _adComputers = new();
        public void Run()
        {
            var domainName = NetworkHelper.GetLocalDomainName();
            Console.WriteLine("Connected domain is: " + domainName);
            
            if (String.IsNullOrWhiteSpace(domainName))
            {
                return;
            }


            Console.WriteLine("Gathering all enabled computers from Active Directory...");
            _adComputers = ActiveDirectoryHelper.GetAllComputerNames(domainName);
            if (_adComputers.Count < 1)
            {
                return;
            }

            var computers = CheckReachable.CheckComputers(_adComputers);
            if (computers.Count > 0)
            {
                DataWriter.WriteComputersToFile(@"C:\1\SoftwareInstalled-xrxlon-CarbonBlack.csv", computers);
            }
            
        }


    }
}