using System.Net;
using System.Text;
using Microsoft.Win32;
namespace CheckSoftwareIsInstalled;

public class CheckSoftware
{
    public static bool CheckInstalled(string findByName, string hostName)
    {

        string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        try
        {
            //64 bits computer
            using (RegistryKey key64 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, hostName))
            {
                using (RegistryKey key = key64.OpenSubKey(registryKey))
                {

                    if (key != null)
                    {
                        foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                        {
                            string displayName = subkey.GetValue("DisplayName") as string;
                            if (displayName != null && displayName.Contains(findByName))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
}