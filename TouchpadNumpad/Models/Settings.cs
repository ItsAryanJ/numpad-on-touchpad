using System;
using System.Collections.Generic;
using System.Text;

namespace TouchpadNumpad.Models;
public class Settings
{
    public string ActiveProfile { get; set; } = "Default";

    public bool StartWithWindows { get; set; }

    public List<string?> ToggleShortcut { get; set; }
        = new()
        {
            "Ctrl",
            "Shift",
            "F12",
            null,
            null
        };
}