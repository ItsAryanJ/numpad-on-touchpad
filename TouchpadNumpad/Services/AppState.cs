using System;
using TouchpadNumpad.Models;

namespace TouchpadNumpad.Services
{
    public static class AppState
    {
        public static Settings Settings { get; set; } = new();
        public static Profile CurrentProfile { get; set; } = new();
    }
}
