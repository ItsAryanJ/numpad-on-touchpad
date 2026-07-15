using System;
using System.Collections.Generic;
using System.Linq;

namespace TouchpadNumpad.Services;

public static class KeyDefinitions
{
    public static IReadOnlyDictionary<string, byte> KeyboardKeys { get; } = new Dictionary<string, byte>()
    {
        // Letters
        { "A", 0x41 }, { "B", 0x42 }, { "C", 0x43 },
        { "D", 0x44 }, { "E", 0x45 }, { "F", 0x46 },
        { "G", 0x47 }, { "H", 0x48 }, { "I", 0x49 },
        { "J", 0x4A }, { "K", 0x4B }, { "L", 0x4C },
        { "M", 0x4D }, { "N", 0x4E }, { "O", 0x4F },
        { "P", 0x50 }, { "Q", 0x51 }, { "R", 0x52 },
        { "S", 0x53 }, { "T", 0x54 }, { "U", 0x55 },
        { "V", 0x56 }, { "W", 0x57 }, { "X", 0x58 },
        { "Y", 0x59 }, { "Z", 0x5A },

        // Numbers
        { "0", 0x30 }, { "1", 0x31 }, { "2", 0x32 },
        { "3", 0x33 }, { "4", 0x34 }, { "5", 0x35 },
        { "6", 0x36 }, { "7", 0x37 }, { "8", 0x38 },
        { "9", 0x39 },

        // Numpad
        { "NumPad0", 0x60 },
        { "NumPad1", 0x61 },
        { "NumPad2", 0x62 },
        { "NumPad3", 0x63 },
        { "NumPad4", 0x64 },
        { "NumPad5", 0x65 },
        { "NumPad6", 0x66 },
        { "NumPad7", 0x67 },
        { "NumPad8", 0x68 },
        { "NumPad9", 0x69 },

        { "Decimal", 0x6E },
        { "Add", 0x6B },
        { "Subtract", 0x6D },
        { "Multiply", 0x6A },
        { "Divide", 0x6F },

        // Editing & Common Keys
        { "Enter", 0x0D },
        { "Tab", 0x09 },
        { "Space", 0x20 },
        { "Escape", 0x1B },
        { "Backspace", 0x08 },

        // Navigation
        { "Left", 0x25 },
        { "Up", 0x26 },
        { "Right", 0x27 },
        { "Down", 0x28 },

        { "Home", 0x24 },
        { "End", 0x23 },

        { "PageUp", 0x21 },
        { "PageDown", 0x22 },

        { "Insert", 0x2D },
        { "Delete", 0x2E },

        // Function Keys
        { "F1", 0x70 }, { "F2", 0x71 }, { "F3", 0x72 },
        { "F4", 0x73 }, { "F5", 0x74 }, { "F6", 0x75 },
        { "F7", 0x76 }, { "F8", 0x77 }, { "F9", 0x78 },
        { "F10", 0x79 }, { "F11", 0x7A }, { "F12", 0x7B }
    };

    public static IReadOnlyDictionary<string, Action> MediaActions { get; } = new Dictionary<string, Action>
     {
        ["Volume Up"] = MediaService.VolumeUp,
        ["Volume Down"] = MediaService.VolumeDown,
        ["Mute"] = MediaService.Mute,
        ["Play/Pause"] = MediaService.PlayPause,
        ["Next Track"] = MediaService.NextTrack,
        ["Previous Track"] = MediaService.PreviousTrack
    };

    public static readonly string[] MediaKeys =
        MediaActions.Keys.ToArray();

    public static IReadOnlyDictionary<string, Action> BrightnessActions { get; } = new Dictionary<string, Action>
    {
        ["Brightness Up"] = BrightnessService.BrightnessUp,
        ["Brightness Down"] = BrightnessService.BrightnessDown
    };

    public static readonly string[] BrightnessKeys =
        BrightnessActions.Keys.ToArray();

    public static string GetDisplayLabel(string value)
    {
        if (value.StartsWith("NumPad"))
        {
            return value.Substring(6);
        }

        return value switch
        {
            "Decimal" => ".",
            "Add" => "+",
            "Subtract" => "-",
            "Multiply" => "*",
            "Divide" => "/",

            "Volume Up" => "Vol+",
            "Volume Down" => "Vol-",

            "Brightness Up" => "Bri+",
            "Brightness Down" => "Bri-",

            "Capital" => "Caps Lock",
            "NumLock" => "Num Lock",
            "Scroll" => "Scroll Lock",
            "PrintScreen" => "Prt Sc",
            "Escape" => "Esc",
            "Return" => "Enter",

            _ => value
        };
    }
}
