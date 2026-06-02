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
    private static extern uint GetRawInputDeviceList([Out] RAWINPUTDEVICELIST[] pRawInputDeviceList, ref uint puiNumDevices, uint cbSize);

    [DllImport("user32.dll")]
    private static extern uint GetRawInputDeviceInfo(nint hDevice, uint uiCommand, nint pData, ref uint pcbSize);

    [DllImport("user32.dll")]
    private static extern uint GetRawInputDeviceInfo(nint hDevice, uint uiCommand, ref RID_DEVICE_INFO pData, ref uint pcbSize);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevices, uint uiNumDevices, uint cbSize);

    public TouchpadHandler(byte touchThrottlePercent)
    {
        _touchpadMessageHandler = new TouchpadMessageHandler(touchThrottlePercent);
        _touchpadMessageHandler.MessageRecieved += delegate (object? sender, TouchpadContact[] args)
        {
            this.Touched?.Invoke(this, args);
        };
    }

    public bool IsTouchpadExists()
    {
        uint puiNumDevices = 0u;
        uint cbSize = (uint)Marshal.SizeOf<RAWINPUTDEVICELIST>();
        if (GetRawInputDeviceList(null, ref puiNumDevices, cbSize) != 0)
        {
            return false;
        }
        RAWINPUTDEVICELIST[] array = new RAWINPUTDEVICELIST[puiNumDevices];
        if (GetRawInputDeviceList(array, ref puiNumDevices, cbSize) != puiNumDevices)
        {
            return false;
        }
        RAWINPUTDEVICELIST[] array2 = array.Where((RAWINPUTDEVICELIST x) => x.dwType == 2).ToArray();
        for (int num = 0; num < array2.Length; num++)
        {
            RAWINPUTDEVICELIST rAWINPUTDEVICELIST = array2[num];
            uint pcbSize = 0u;
            if (GetRawInputDeviceInfo(rAWINPUTDEVICELIST.hDevice, 536870923u, IntPtr.Zero, ref pcbSize) == 0)
            {
                RID_DEVICE_INFO pData = new RID_DEVICE_INFO
                {
                    cbSize = pcbSize
                };
                if (GetRawInputDeviceInfo(rAWINPUTDEVICELIST.hDevice, 536870923u, ref pData, ref pcbSize) != uint.MaxValue && pData.hid.usUsagePage == 13 && pData.hid.usUsage == 5)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void StartCapture()
    {
        RAWINPUTDEVICE rAWINPUTDEVICE = new RAWINPUTDEVICE
        {
            usUsagePage = 13,
            usUsage = 5,
            dwFlags = 256u,
            hwndTarget = _touchpadMessageHandler.Handle
        };
        RegisterRawInputDevices(new RAWINPUTDEVICE[1] { rAWINPUTDEVICE }, 1u, (uint)Marshal.SizeOf<RAWINPUTDEVICE>());
    }

    public void StopCapture()
    {
        Dispose(disposing: true);
    }

    protected virtual void Dispose(bool disposing)
    {
        RAWINPUTDEVICE rAWINPUTDEVICE = new RAWINPUTDEVICE
        {
            usUsagePage = 13,
            usUsage = 5,
            dwFlags = 1u,
            hwndTarget = IntPtr.Zero
        };
        RegisterRawInputDevices(new RAWINPUTDEVICE[1] { rAWINPUTDEVICE }, 1u, (uint)Marshal.SizeOf<RAWINPUTDEVICE>());
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
