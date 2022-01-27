using System.Net;
using System.Text;
using Microsoft.Win32;
namespace CheckSoftwareIsInstalled;

public class CheckSoftware
{
    public static string[] CheckInstalled(string findByName, string hostName)
    {
        string[] info = new string[3];

        string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        //64 bits computer
        RegistryKey key64 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, hostName);
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