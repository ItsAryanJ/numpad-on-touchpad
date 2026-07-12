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
                KeyboardService.PressKey(0x60);
                break;

            case "NumPad1":
                KeyboardService.PressKey(0x61);
                break;

            case "NumPad2":
                KeyboardService.PressKey(0x62);
                break;

            case "NumPad3":
                KeyboardService.PressKey(0x63);
                break;

            case "NumPad4":
                KeyboardService.PressKey(0x64);
                break;

            case "NumPad5":
                KeyboardService.PressKey(0x65);
                break;

            case "NumPad6":
                KeyboardService.PressKey(0x66);
                break;

            case "NumPad7":
                KeyboardService.PressKey(0x67);
                break;

            case "NumPad8":
                KeyboardService.PressKey(0x68);
                break;

            case "NumPad9":
                KeyboardService.PressKey(0x69);
                break;

            case "Decimal":
                KeyboardService.PressKey(0x6E);
                break;
        }
    }
}