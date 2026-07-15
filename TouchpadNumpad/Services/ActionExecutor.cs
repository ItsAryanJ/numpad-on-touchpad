using System.Windows.Forms;
using TouchpadNumpad.Models;

namespace TouchpadNumpad.Services;

public static class ActionExecutor
{
    public static void Execute(CellAction action)
    {
        switch (action.Type)
        {
            case ActionType.KeyboardKey:
                ExecuteKeyboard(action.Value);
                break;

            case ActionType.Media:
                ExecuteMedia(action.Value);
                break;

            case ActionType.Brightness:
                ExecuteBrightness(action.Value);
                break;

            default:
                throw new NotSupportedException(
                    $"Unsupported action type: {action.Type}");
        }
    }

    private static void ExecuteKeyboard(string value)
    {
        if (KeyDefinitions.KeyboardKeys.TryGetValue(value, out byte key))
        {
            KeyboardService.PressKey(key);
        }
    }

    private static void ExecuteMedia(string value)
    {
        if (KeyDefinitions.MediaActions.TryGetValue(value, out var action))
        {
            action();
        }
    }

    private static void ExecuteBrightness(string value)
    {
        if (KeyDefinitions.BrightnessActions.TryGetValue(value, out var action))
        {
            action();
        }
    }
}