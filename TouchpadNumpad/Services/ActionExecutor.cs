using System.Windows.Forms;
using TouchpadNumpad.Models;

namespace TouchpadNumpad.Services;

public static class ActionExecutor
{
    public static void Execute(CellAction action)
    {
        if (action.Type != ActionType.KeyboardKey)
            return;

        switch (action.Value)
        {
            case "NumPad0":
                Program.SimulateNumpadKey(0x60);
                break;

            case "NumPad1":
                Program.SimulateNumpadKey(0x61);
                break;

            case "NumPad2":
                Program.SimulateNumpadKey(0x62);
                break;

            case "NumPad3":
                Program.SimulateNumpadKey(0x63);
                break;

            case "NumPad4":
                Program.SimulateNumpadKey(0x64);
                break;

            case "NumPad5":
                Program.SimulateNumpadKey(0x65);
                break;

            case "NumPad6":
                Program.SimulateNumpadKey(0x66);
                break;

            case "NumPad7":
                Program.SimulateNumpadKey(0x67);
                break;

            case "NumPad8":
                Program.SimulateNumpadKey(0x68);
                break;

            case "NumPad9":
                Program.SimulateNumpadKey(0x69);
                break;

            case "Decimal":
                Program.SimulateNumpadKey(0x6E);
                break;
        }
    }
}