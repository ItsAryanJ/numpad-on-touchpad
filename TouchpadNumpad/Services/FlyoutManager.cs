using System;
using System.IO;

namespace TouchpadNumpad.Services
{
    public static class FlyoutManager
    {
        public static string GetFlyoutPath()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            string[] potentialPaths = {
            
                // Path 1: Local folder
                Path.Combine(baseDir, @"NumpadFlyout\NumpadFlyout.exe"),
        
                // Path 2: Development environment
                Path.Combine(baseDir, @"..\..\..\..\NumpadFlyout\bin\Release\net10.0-windows\NumpadFlyout.exe")
            };

            foreach (string path in potentialPaths)
            {
                if (File.Exists(path))
                {
                    return Path.GetFullPath(path);
                }
            }

            return string.Empty;
        }
    }
}
