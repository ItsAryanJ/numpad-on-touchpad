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
        switch (value)
        {
            case "Volume Up":
                MediaService.VolumeUp();
                break;

            case "Volume Down":
                MediaService.VolumeDown();
                break;

            case "Mute":
                MediaService.Mute();
                break;

            case "Play/Pause":
                MediaService.PlayPause();
                break;

            case "Next Track":
                MediaService.NextTrack();
                break;

            case "Previous Track":
                MediaService.PreviousTrack();
                break;
        }
    }

    private static void ExecuteBrightness(string value)
    {
        switch (value)
        {
            case "Brightness Up":
                BrightnessService.BrightnessUp();
                break;

            case "Brightness Down":
                BrightnessService.BrightnessDown();
                break;
        }
    }
}