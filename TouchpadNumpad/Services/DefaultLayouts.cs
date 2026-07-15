using System;
using System.Collections.Generic;
using System.Text;
using TouchpadNumpad.Models;

namespace TouchpadNumpad.Services;

public static class DefaultLayouts
{
    public static readonly (int Row, int Column, string Label, string Value)[] Numpad =
    {
        (0, 0, "7", "NumPad7"),
        (0, 1, "8", "NumPad8"),
        (0, 2, "9", "NumPad9"),

        (1, 0, "4", "NumPad4"),
        (1, 1, "5", "NumPad5"),
        (1, 2, "6", "NumPad6"),

        (2, 0, "1", "NumPad1"),
        (2, 1, "2", "NumPad2"),
        (2, 2, "3", "NumPad3"),

        (3, 0, "0", "NumPad0"),
        (3, 1, "0", "NumPad0"),
        (3, 2, ".", "Decimal")
    };
}
