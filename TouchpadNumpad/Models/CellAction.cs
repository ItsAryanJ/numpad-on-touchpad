using System;
using System.Collections.Generic;
using System.Text;

namespace TouchpadNumpad.Models;

public enum ActionType
{
    KeyboardKey,
    Media,
    Brightness
}
    public class CellAction
    {
        public ActionType Type { get; set; }
        public string Value { get; set; } = "";
    }

