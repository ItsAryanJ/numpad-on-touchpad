using System;
using System.Linq;
using System.Runtime.InteropServices;

public class TouchpadHandler : IDisposable
{
    private struct RAWINPUTDEVICELIST
    {
        public nint hDevice;

        public uint dwType;
    }

    private struct RID_DEVICE_INFO_HID
    {
        public uint dwVendorId;

        public uint dwProductId;

        public uint dwVersionNumber;

        public ushort usUsagePage;

        public ushort usUsage;
    }

    private struct RID_DEVICE_INFO
    {
        public uint cbSize;

        public uint dwType;

        public RID_DEVICE_INFO_HID hid;
    }

    private struct RAWINPUTDEVICE
    {
        public ushort usUsagePage;

        public ushort usUsage;

        public uint dwFlags;

        public nint hwndTarget;
    }

    private const uint RIM_TYPEHID = 2u;

    private const uint RIDI_DEVICEINFO = 536870923u;

    private const uint RIDEV_REMOVE = 1u;

    private const uint RIDEV_INPUTSINK = 256u;

    private readonly TouchpadMessageHandler _touchpadMessageHandler;

    public event EventHandler<TouchpadContact[]>? Touched;

    [DllImport("user32.dll")]
    private static extern uint GetRawInputDeviceList([Out] RAWINPUTDEVICELIST[]? pRawInputDeviceList, ref uint deviceCount, uint listEntrySize);

    [DllImport("user32.dll")]
    private static extern uint GetRawInputDeviceInfo(nint hDevice, uint uiCommand, nint deviceInfo, ref uint deviceInfoSize);

    [DllImport("user32.dll")]
    private static extern uint GetRawInputDeviceInfo(nint hDevice, uint uiCommand, ref RID_DEVICE_INFO deviceInfo, ref uint deviceInfoSize);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevices, uint uiNumDevices, uint listEntrySize);

    public TouchpadHandler(byte touchThrottlePercent)
    {
        _touchpadMessageHandler = new TouchpadMessageHandler(touchThrottlePercent);
        _touchpadMessageHandler.MessageRecieved += (_, contacts) => Touched?.Invoke(this, contacts);
    }
    // HID Usage Page for Digitizer devices
    private const ushort HID_USAGE_PAGE_DIGITIZER = 0x0D;
    // HID Usage for Precision Touchpads
    private const ushort HID_USAGE_TOUCH_PAD = 0x05;
    private static RAWINPUTDEVICE CreateRawInputDevice(uint flags,nint target)
    {
        return new RAWINPUTDEVICE
        {
            usUsagePage = HID_USAGE_PAGE_DIGITIZER,
            usUsage = HID_USAGE_TOUCH_PAD,
            dwFlags = flags,
            hwndTarget = target
        };
    }
    public bool IsTouchpadExists()
    {
        uint deviceCount = 0u;
        uint listEntrySize = (uint)Marshal.SizeOf<RAWINPUTDEVICELIST>();
        if (GetRawInputDeviceList(null, ref deviceCount, listEntrySize) != 0)
        {
            return false;
        }
        RAWINPUTDEVICELIST[] devices = new RAWINPUTDEVICELIST[deviceCount];
        if (GetRawInputDeviceList(devices, ref deviceCount, listEntrySize) != deviceCount)
        {
            return false;
        }
        RAWINPUTDEVICELIST[] hidDevices = devices.Where(device => device.dwType == RIM_TYPEHID).ToArray();
        foreach (RAWINPUTDEVICELIST device in hidDevices)
        {
            uint deviceInfoSize = 0;
            if (GetRawInputDeviceInfo(device.hDevice, RIDI_DEVICEINFO, IntPtr.Zero, ref deviceInfoSize) == 0)
            {
                RID_DEVICE_INFO deviceInfo = new RID_DEVICE_INFO
                {
                    cbSize = deviceInfoSize
                };
                if (GetRawInputDeviceInfo(device.hDevice, RIDI_DEVICEINFO, ref deviceInfo, ref deviceInfoSize) != uint.MaxValue && deviceInfo.hid.usUsagePage == HID_USAGE_PAGE_DIGITIZER && deviceInfo.hid.usUsage == HID_USAGE_TOUCH_PAD)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void StartCapture()
    {
        RAWINPUTDEVICE device = CreateRawInputDevice(RIDEV_INPUTSINK,_touchpadMessageHandler.Handle);
        RegisterRawInputDevices(new[] { device }, 1u, (uint)Marshal.SizeOf<RAWINPUTDEVICE>());
    }

    public void StopCapture() => Dispose(true);

    protected virtual void Dispose(bool disposing)
    {
        RAWINPUTDEVICE device = CreateRawInputDevice(RIDEV_REMOVE,IntPtr.Zero);
        RegisterRawInputDevices(new[] { device }, 1u, (uint)Marshal.SizeOf<RAWINPUTDEVICE>());
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    ~TouchpadHandler()
    {
        Dispose(disposing: false);
    }
}
