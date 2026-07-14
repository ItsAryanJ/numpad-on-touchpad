using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace TouchpadNumpad.Services
{
    public static class BrightnessService
    {
        private static void ChangeBrightness(byte brightness)
        {
            ManagementScope scope = new ManagementScope(@"\\.\root\WMI");

            using ManagementClass mclass =
                new ManagementClass(scope, new ManagementPath("WmiMonitorBrightnessMethods"), null);

            foreach (ManagementObject instance in mclass.GetInstances())
            {
                instance.InvokeMethod(
                    "WmiSetBrightness",
                    new object[] { 1, brightness });
            }
        }

        private static byte GetBrightness()
        {
            using ManagementClass mclass =
                new ManagementClass("root\\WMI", "WmiMonitorBrightness", null);

            foreach (ManagementObject instance in mclass.GetInstances())
            {
                return (byte)instance["CurrentBrightness"];
            }

            return 50;
        }

        public static void BrightnessUp()
        {
            byte current = GetBrightness();

            current = (byte)Math.Min(current + 10, 100);

            ChangeBrightness(current);
        }

        public static void BrightnessDown()
        {
            byte current = GetBrightness();

            current = (byte)Math.Max(current - 10, 0);

            ChangeBrightness(current);
        }

    }
}
