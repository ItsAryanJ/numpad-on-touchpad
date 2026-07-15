using System;
using System.Management;

namespace TouchpadNumpad.Services
{
    public static class BrightnessService
    {
        private const string WmiNamespace = @"\\.\root\WMI";
        private const byte BrightnessStep = 10;
        private const byte DefaultBrightness = 50;
        private static void ChangeBrightness(byte brightness)
        {
            ManagementScope scope = new(WmiNamespace);

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
                new ManagementClass(WmiNamespace,"WmiMonitorBrightness",null);

            foreach (ManagementObject instance in mclass.GetInstances())
                using (instance)
                {
                    return (byte)instance["CurrentBrightness"];
                }

            return DefaultBrightness;
        }
        public static void SetBrightness(int brightness)
        {
            brightness = (byte)Math.Clamp(brightness, 0, 100);

            ChangeBrightness((byte)brightness);
        }

        public static void BrightnessUp()
        {
            SetBrightness((byte)(GetBrightness() + BrightnessStep));
        }

        public static void BrightnessDown()
        {
            SetBrightness((byte)(GetBrightness() - BrightnessStep));
        }

    }
}
