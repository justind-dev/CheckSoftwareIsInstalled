using System.Net;
using System.Text;
using Microsoft.Win32;

namespace CheckSoftwareIsInstalled;

public class Cli
{
    public class CommandLineInterface
    {
        private List<Computer> _computers = new();
        private List<string> _adComputers = new();
        private int _totalComputers;
        private float _progress;
        public void Run()
        {
            //Get domain name
            var domainName = NetworkHelper.GetLocalDomainName();
            Console.WriteLine("Connected domain is: " + domainName);
            if (String.IsNullOrWhiteSpace(domainName))
            {
                return;
            }

            // get domain computers
            Console.WriteLine("Gathering all enabled computers from Active Directory...");
            _adComputers = ActiveDirectoryHelper.GetAllComputerNames(domainName);
            if (_adComputers.Count < 1)
            {
                return;
            }

            _totalComputers = _adComputers.Count();
            //for each computer in domain perform network connectivity tests and build our computer objects. 
            string computerIpAddress;
            bool computerIsPingable;
            bool computerSmbPortReachable;
            bool computerCarbonBlackInstalled;
            bool computerMcAfeeInstalled;
            var count = 0;
            foreach (string computerName in _adComputers)
            {
                if (count >= 3)
                {
                    break;
                }

                Console.WriteLine("Processing: " + computerName + "...");
                computerIpAddress = NetworkHelper.GetHostIpAddress(computerName);
                if (string.IsNullOrWhiteSpace(computerIpAddress))
                {
                    computerIpAddress = "";
                    computerIsPingable = false;
                    computerSmbPortReachable = false;
                    computerCarbonBlackInstalled = false;
                    computerMcAfeeInstalled = false;
                }
                else
                {
                    computerIsPingable = NetworkHelper.GetHostPingable(IPAddress.Parse(computerIpAddress));
                    computerSmbPortReachable = NetworkHelper.SmbPortReachable(IPAddress.Parse(computerIpAddress));
                    if (computerIsPingable && computerSmbPortReachable)
                    {
                        computerCarbonBlackInstalled =
                            (CheckInstalled("Carbon Black Cloud Sensor", computerName)).Length > 0 ? true : false;
                        computerMcAfeeInstalled =
                            (CheckInstalled("DOESNOTEXIST_TEST", computerName)).Length > 0 ? true : false;
                    }
                    else
                    {
                        computerCarbonBlackInstalled = false;
                        computerMcAfeeInstalled = false;
                    }
                }

                _computers.Add(new Computer(computerName,
                    computerIpAddress,
                    computerIsPingable,
                    computerSmbPortReachable,
                    computerCarbonBlackInstalled,
                    computerMcAfeeInstalled));
                count++;
            }
            
            WriteComputersToFile(@"C:\1\SoftwareInstalled-xrxlon-CarbonBlack.csv");
        }

        private void WriteComputersToFile(string filePath)
        {
            var csv = new StringBuilder();
            var headerLine = string.Format("HOSTNAME,IPADDRESS,PINGABLE,SMBREACHABLE,CB_INSTALLED,MCAFEE_INSTALLED");
            csv.AppendLine(headerLine);

            foreach (Computer computer in _computers)
            {
                var cName = computer.HostName;
                var cIpAddress = computer.HostIpAddress;
                var cIsPingable = (computer.HostPingable) ? "true" : "false";
                var cIsSmbReachable = (computer.HostSmbReachable) ? "true" : "false";
                var cCarbonBlackInstalled = (computer.HostCarbonBlackInstalled) ? "true" : "false";
                var cMcAfeeInstalled = (computer.HostMcAfeeInstalled) ? "true" : "false";
                var newLine = string.Format("{0},{1},{2},{3}, {4}, {5}",
                    cName,
                    cIpAddress,
                    cIsPingable,
                    cIsSmbReachable,
                    cCarbonBlackInstalled,
                    cMcAfeeInstalled);

                csv.AppendLine(newLine);
            }

            File.WriteAllText(filePath, csv.ToString());
        }

        public static string[] CheckInstalled(string findByName, string hostName)
        {
            string[] info = new string[3];

            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            //64 bits computer
            RegistryKey key64 =
                RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, hostName);
            RegistryKey key = key64.OpenSubKey(registryKey);
            if (key != null)
            {
                foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                {
                    string displayName = subkey.GetValue("DisplayName") as string;
                    if (displayName != null && displayName.Contains(findByName))
                    {
                        info[0] = displayName;

                        info[1] = subkey.GetValue("InstallLocation").ToString();

                        info[2] = subkey.GetValue("Version").ToString();
                    }
                }

                key.Close();
            }

            return info;
        }
    }
}