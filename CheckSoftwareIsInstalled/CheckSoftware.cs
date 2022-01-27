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
            RegistryKey key64 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, hostName);
            RegistryKey key = key64.OpenSubKey(registryKey);
            if (key != null)
            {
                foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                {
                    string displayName = subkey.GetValue("DisplayName") as string;
                    if (displayName != null && displayName.Contains(findByName))
                    {
                        subkey.Close();
                        key64.Close();
                        key.Close();
                        return true;
                    }
                }
            }
            key64.Close();
            key.Close();
            return false;
        }
        catch
        {
            return false;
        }
    }
}