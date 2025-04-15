using Nitchii.InputManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nitchii.InputManager;

public static class KeyboardInput
{
    public static void KeyDown(Keys k)
    {
        Native.INPUTS pInputs = default(Native.INPUTS);
        int cbSize = Marshal.SizeOf(typeof(Native.INPUTS));
        pInputs.type = 1;
        pInputs.inputs.ki.wScan = (short)Native.MapVirtualKey((uint)k, 0u);
        pInputs.inputs.ki.dwFlags = 8;
        Native.SendInput(1u, ref pInputs, cbSize);
    }

    public static void KeyUp(Keys k)
    {
        Native.INPUTS pInputs = default(Native.INPUTS);
        int cbSize = Marshal.SizeOf(typeof(Native.INPUTS));
        pInputs.type = 1;
        pInputs.inputs.ki.wScan = (short)Native.MapVirtualKey((uint)k, 0u);
        pInputs.inputs.ki.dwFlags = 10;
        Native.SendInput(1u, ref pInputs, cbSize);
    }

    public static Task KeyPress(Keys k, int delay = 50)
    {
        return Task.Factory.StartNew((Func<Task>)async delegate
        {
            KeyDown(k);
            if (delay > 0)
            {
                await Task.Delay(delay);
            }
            KeyUp(k);
        });
    }
}
