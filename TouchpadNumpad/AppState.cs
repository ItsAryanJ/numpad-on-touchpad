using System;
using System.Collections.Generic;
using System.Text;
using TouchpadNumpad.Models;

namespace TouchpadNumpad
{
    public static class AppState
    {
        public static Settings Settings { get; set; } = new();
        public static Profile CurrentProfile { get; set; } = new();
    }
}
