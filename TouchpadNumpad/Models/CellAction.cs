using System;
using System.Collections.Generic;
using System.Text;

namespace TouchpadNumpad.Models;

public enum ActionType
{
    KeyboardKey,
    Shortcut,
    Media,
    Brightness,
    System
}
    public class CellAction
    {
        public ActionType Type { get; set; }
        public string Value { get; set; } = "";
    }

