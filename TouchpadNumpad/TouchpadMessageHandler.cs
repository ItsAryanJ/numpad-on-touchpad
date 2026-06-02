using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

internal class TouchpadMessageHandler : NativeWindow
{
    private struct RAWINPUTHEADER
    {
        public uint dwType;

        public uint dwSize;

        public nint hDevice;

        public nint wParam;
    }

    private struct RAWHID
    {
        public uint dwSizeHid;

        public uint dwCount;

        public nint bRawData;
    }

    private struct RAWINPUT
    {
        public RAWINPUTHEADER Header;

        public RAWHID Hid;
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

    private struct HIDP_CAPS
    {
        public ushort Usage;

        public ushort UsagePage;

        public ushort InputReportByteLength;

        public ushort OutputReportByteLength;

        public ushort FeatureReportByteLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public ushort[] Reserved;

        public ushort NumberLinkCollectionNodes;

        public ushort NumberInputButtonCaps;

        public ushort NumberInputValueCaps;

        public ushort NumberInputDataIndices;

        public ushort NumberOutputButtonCaps;

        public ushort NumberOutputValueCaps;

        public ushort NumberOutputDataIndices;

        public ushort NumberFeatureButtonCaps;

        public ushort NumberFeatureValueCaps;

        public ushort NumberFeatureDataIndices;
    }

    private struct HIDP_VALUE_CAPS
    {
        public ushort UsagePage;

        public byte ReportID;

        [MarshalAs(UnmanagedType.U1)]
        public bool IsAlias;

        public ushort BitField;

        public ushort LinkCollection;

        public ushort LinkUsage;

        public ushort LinkUsagePage;

        [MarshalAs(UnmanagedType.U1)]
        public bool IsRange;

        [MarshalAs(UnmanagedType.U1)]
        public bool IsStringRange;

        [MarshalAs(UnmanagedType.U1)]
        public bool IsDesignatorRange;

        [MarshalAs(UnmanagedType.U1)]
        public bool IsAbsolute;

        [MarshalAs(UnmanagedType.U1)]
        public bool HasNull;

        public byte Reserved;

        public ushort BitSize;

        public ushort ReportCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public ushort[] Reserved2;

        public uint UnitsExp;

        public uint Units;

        public int LogicalMin;

        public int LogicalMax;

        public int PhysicalMin;

        public int PhysicalMax;

        public ushort UsageMin;

        public ushort UsageMax;

        public ushort StringMin;

        public ushort StringMax;

        public ushort DesignatorMin;

        public ushort DesignatorMax;

        public ushort DataIndexMin;

        public ushort DataIndexMax;

        public ushort Usage => UsageMin;

        public ushort StringIndex => StringMin;

        public ushort DesignatorIndex => DesignatorMin;

        public ushort DataIndex => DataIndexMin;
    }

    private enum HIDP_REPORT_TYPE
    {
        HidP_Input,
        HidP_Output,
        HidP_Feature
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct HIDP_DATA
    {
        [FieldOffset(0)]
        public short DataIndex;

        [FieldOffset(2)]
        public short Reserved;

        [FieldOffset(4)]
        public int RawValue;

        [FieldOffset(4)]
        [MarshalAs(UnmanagedType.U1)]
        public bool On;
    }

    private const int WM_INPUT = 255;

    private const uint RID_INPUT = 268435459u;

    private const uint RIDI_PREPARSEDDATA = 536870917u;

    private const uint HIDP_STATUS_SUCCESS = 1114112u;

    private byte _touchThrottlePercent;

    private TouchpadContact[]? _previousMessage;

    private Dictionary<int, (ulong? messageTick, TouchpadContact contact)> _previousMessagePerConcactId;

    private System.Threading.Timer _throttleTimer;

    private bool _properTouchPadExists;

    private ulong _messageTick;

    internal event EventHandler<TouchpadContact[]>? MessageRecieved;

    [DllImport("user32.dll")]
    private static extern uint GetRawInputData(nint hRawInput, uint uiCommand, nint pData, ref uint pcbSize, uint cbSizeHeader);

    [DllImport("user32.dll")]
    private static extern uint GetRawInputDeviceInfo(nint hDevice, uint uiCommand, nint pData, ref uint pcbSize);

    [DllImport("user32.dll")]
    private static extern uint GetRawInputDeviceInfo(nint hDevice, uint uiCommand, ref RID_DEVICE_INFO pData, ref uint pcbSize);

    [DllImport("Hid.dll")]
    private static extern uint HidP_GetCaps(nint PreparsedData, out HIDP_CAPS Capabilities);

    [DllImport("Hid.dll", CharSet = CharSet.Auto)]
    private static extern uint HidP_GetValueCaps(HIDP_REPORT_TYPE ReportType, [Out] HIDP_VALUE_CAPS[] ValueCaps, ref ushort ValueCapsLength, nint PreparsedData);

    [DllImport("Hid.dll", CharSet = CharSet.Auto)]
    private static extern uint HidP_GetUsageValue(HIDP_REPORT_TYPE ReportType, ushort UsagePage, ushort LinkCollection, ushort Usage, out uint UsageValue, nint PreparsedData, nint Report, uint ReportLength);

    internal TouchpadMessageHandler(byte touchThrottlePercent)
    {
        _touchThrottlePercent = touchThrottlePercent;
        _previousMessagePerConcactId = new Dictionary<int, (ulong?, TouchpadContact)>();
        CreateHandle(new CreateParams());
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == 255)
        {
            ProcessMessage(m.LParam);
        }
        base.WndProc(ref m);
    }

    private void ProcessMessage(nint lParam)
    {
        uint pcbSize = 0u;
        uint cbSizeHeader = (uint)Marshal.SizeOf<RAWINPUTHEADER>();
        if (GetRawInputData(lParam, 268435459u, IntPtr.Zero, ref pcbSize, cbSizeHeader) != 0)
        {
            return;
        }
        nint num = IntPtr.Zero;
        RAWINPUT rAWINPUT;
        byte[] array2;
        try
        {
            num = Marshal.AllocHGlobal((int)pcbSize);
            if (GetRawInputData(lParam, 268435459u, num, ref pcbSize, cbSizeHeader) != pcbSize)
            {
                return;
            }
            rAWINPUT = Marshal.PtrToStructure<RAWINPUT>(num);
            byte[] array = new byte[pcbSize];
            Marshal.Copy(num, array, 0, array.Length);
            array2 = new byte[rAWINPUT.Hid.dwSizeHid * rAWINPUT.Hid.dwCount];
            int srcOffset = (int)pcbSize - array2.Length;
            Buffer.BlockCopy(array, srcOffset, array2, 0, array2.Length);
        }
        finally
        {
            Marshal.FreeHGlobal(num);
        }
        nint num2 = Marshal.AllocHGlobal(array2.Length);
        Marshal.Copy(array2, 0, num2, array2.Length);
        nint num3 = IntPtr.Zero;
        try
        {
            uint pcbSize2 = 0u;
            if (GetRawInputDeviceInfo(rAWINPUT.Header.hDevice, 536870917u, IntPtr.Zero, ref pcbSize2) != 0)
            {
                return;
            }
            num3 = Marshal.AllocHGlobal((int)pcbSize2);
            if (GetRawInputDeviceInfo(rAWINPUT.Header.hDevice, 536870917u, num3, ref pcbSize2) != pcbSize2 || HidP_GetCaps(num3, out var Capabilities) != 1114112)
            {
                return;
            }
            ushort ValueCapsLength = Capabilities.NumberInputValueCaps;
            HIDP_VALUE_CAPS[] array3 = new HIDP_VALUE_CAPS[ValueCapsLength];
            if (HidP_GetValueCaps(HIDP_REPORT_TYPE.HidP_Input, array3, ref ValueCapsLength, num3) != 1114112)
            {
                return;
            }
            uint num4 = 0u;
            TouchpadContactCreator touchpadContactCreator = new TouchpadContactCreator
            {
                EqualityDeltaPercent = _touchThrottlePercent
            };
            List<TouchpadContact> list = new List<TouchpadContact>();
            foreach (HIDP_VALUE_CAPS item in array3.OrderBy((HIDP_VALUE_CAPS x) => x.LinkCollection))
            {
                if (HidP_GetUsageValue(HIDP_REPORT_TYPE.HidP_Input, item.UsagePage, item.LinkCollection, item.Usage, out var UsageValue, num3, num2, (uint)array2.Length) != 1114112)
                {
                    continue;
                }
                if (item.LinkCollection == 0)
                {
                    ushort usagePage = item.UsagePage;
                    ushort usage = item.Usage;
                    if (usagePage == 13 && usage == 84)
                    {
                        num4 = UsageValue;
                    }
                }
                else
                {
                    ushort usage = item.UsagePage;
                    ushort usage2 = item.Usage;
                    switch (usage)
                    {
                        case 13:
                            if (usage2 == 81)
                            {
                                touchpadContactCreator.ContactId = (int)UsageValue;
                            }
                            break;
                        case 1:
                            switch (usage2)
                            {
                                case 48:
                                    touchpadContactCreator.X = (int)UsageValue;
                                    touchpadContactCreator.MaxX = item.LogicalMax;
                                    break;
                                case 49:
                                    touchpadContactCreator.Y = (int)UsageValue;
                                    touchpadContactCreator.MaxY = item.LogicalMax;
                                    break;
                            }
                            break;
                    }
                }
                if (touchpadContactCreator.TryCreate(out var contact) && contact.HasValue)
                {
                    list.Add(contact.Value);
                    if (list.Count >= num4)
                    {
                        break;
                    }
                    touchpadContactCreator.Clear();
                }
            }
            TouchpadContact[] currentMessage = (from el in list
                                                orderby el.X, el.Y
                                                select el).ToArray();
            TouchpadContact[]? previousMessage = _previousMessage;
            int num5 = ((previousMessage != null) ? previousMessage.Length : 0);
            _properTouchPadExists = _properTouchPadExists || currentMessage.Length > 1;
            if (!_properTouchPadExists && currentMessage.Length == 1)
            {
                TouchpadContact touchpadContact = currentMessage.First();
                if (touchpadContact.ContactId != 0)
                {
                    _previousMessagePerConcactId[touchpadContact.ContactId] = (_messageTick, touchpadContact);
                    return;
                }
                ulong num6 = _messageTick++;
                List<TouchpadContact> list2 = new List<TouchpadContact>();
                foreach (KeyValuePair<int, (ulong?, TouchpadContact)> item2 in _previousMessagePerConcactId)
                {
                    if (item2.Key != 0 && _previousMessagePerConcactId.TryGetValue(item2.Key, out (ulong?, TouchpadContact) value) && value.Item1 == num6)
                    {
                        list2.Add(value.Item2);
                        _previousMessagePerConcactId[item2.Key] = (null, value.Item2);
                    }
                }
                if (list2.Any())
                {
                    list2.Add(touchpadContact);
                    currentMessage = (from el in list2
                                      orderby el.X, el.Y
                                      select el).ToArray();
                }
            }
            bool flag = num5 != currentMessage.Length;
            if (!flag)
            {
                for (int num7 = 0; num7 < num5; num7++)
                {
                    flag = _previousMessage[num7] != currentMessage[num7];
                }
            }
            _previousMessage = currentMessage;
            if (!flag)
            {
                return;
            }
            if (_properTouchPadExists)
            {
                this.MessageRecieved?.Invoke(this, currentMessage);
                return;
            }
            _throttleTimer?.Dispose();
            _throttleTimer = new System.Threading.Timer(delegate
            {
                _messageTick = 0uL;
                this.MessageRecieved?.Invoke(this, currentMessage);
            }, null, 25, -1);
        }
        finally
        {
            Marshal.FreeHGlobal(num2);
            Marshal.FreeHGlobal(num3);
        }
    }
}
