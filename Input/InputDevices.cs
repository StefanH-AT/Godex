using Godot;

namespace Godex.Input;

public enum InputDeviceType
{
    KeyboardAndMouse,
    Joypad
}

public record struct InputDevice
{
    public required InputDeviceType Type;
    public required int EngineDeviceId;
}

public static class InputDevices
{
    public static List<InputDevice> GetAvailableInputDevices()
    {
        var result = new List<InputDevice>();
        result.Add(new InputDevice{ Type = InputDeviceType.KeyboardAndMouse, EngineDeviceId = 0 });

        var joypads = Godot.Input.GetConnectedJoypads();
        foreach (var joypad in joypads)
        {
            result.Add(new InputDevice{ Type = InputDeviceType.Joypad, EngineDeviceId = joypad });
        }

        return result;
    }

    public static int? GetInputId(int engineDeviceId, InputDeviceType type)
    {
        var devices = GetAvailableInputDevices();
        for (var i = 0; i < devices.Count; i++)
        {
            var inputDevice = devices[i];
            if (inputDevice.Type == type && inputDevice.EngineDeviceId == engineDeviceId)
            {
                return i;
            }
        }

        return null;
    }

    public static int? GetInputIdFromInputEvent(in InputEvent e)
    {
        if (e is InputEventKey)
        {
            return GetInputId(e.Device, InputDeviceType.KeyboardAndMouse);
        }
        if (e is InputEventMouse)
        {
            return GetInputId(e.Device, InputDeviceType.KeyboardAndMouse);
        }
        if (e is InputEventJoypadButton)
        {
            return GetInputId(e.Device, InputDeviceType.Joypad);
        }
        if (e is InputEventJoypadMotion)
        {
            return GetInputId(e.Device, InputDeviceType.Joypad);
        }

        return null;
    }
}