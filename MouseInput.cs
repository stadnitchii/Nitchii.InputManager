using Nitchii.InputManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nitchii.InputManager;

public static class MouseInput
{
    public static int ScreenWidth = -1;

    public static int ScreenHeight = -1;

    public static void Move(int x, int y, int screenWidth, int screenHeight)
    {
        Native.INPUTS pInputs = default(Native.INPUTS);
        int cbSize = Marshal.SizeOf(typeof(Native.INPUTS));
        pInputs.type = 0;
        pInputs.inputs.mi.dx = x * (65535 / screenWidth);
        pInputs.inputs.mi.dy = y * (65535 / screenHeight);
        pInputs.inputs.mi.dwFlags = 32769;
        Native.SendInput(1u, ref pInputs, cbSize);
    }

    public static void Move(int x, int y)
    {
        if (ScreenWidth < 0 || ScreenHeight < 0)
        {
            throw new Exception("screen width and height must be greater than 1");
        }
        Move(x, y, ScreenWidth, ScreenHeight);
    }

    public static void MoveBy(int x, int y)
    {
        Native.INPUTS pInputs = default(Native.INPUTS);
        int cbSize = Marshal.SizeOf(typeof(Native.INPUTS));
        pInputs.type = 0;
        pInputs.inputs.mi.dx = x;
        pInputs.inputs.mi.dy = y;
        pInputs.inputs.mi.dwFlags = 1;
        Native.SendInput(1u, ref pInputs, cbSize);
    }

    public static void ButtonDown(MouseButtons button)
    {
        if (button == MouseButtons.None)
        {
            return;
        }
        Native.INPUTS pInputs = default(Native.INPUTS);
        int cbSize = Marshal.SizeOf(typeof(Native.INPUTS));
        int dwFlags = 0;
        switch (button)
        {
            case MouseButtons.Left:
                dwFlags = 2;
                break;
            case MouseButtons.Right:
                dwFlags = 8;
                break;
            case MouseButtons.Middle:
                dwFlags = 32;
                break;
            case MouseButtons.XButton1:
            case MouseButtons.XButton2:
                dwFlags = 128;
                if (button == MouseButtons.XButton1)
                {
                    pInputs.inputs.mi.mouseData = 1;
                }
                else
                {
                    pInputs.inputs.mi.mouseData = 2;
                }
                break;
        }
        pInputs.type = 0;
        pInputs.inputs.mi.dwFlags = dwFlags;
        Native.SendInput(1u, ref pInputs, cbSize);
    }

    public static void ButtonUp(MouseButtons button)
    {
        if (button == MouseButtons.None)
        {
            return;
        }
        Native.INPUTS pInputs = default(Native.INPUTS);
        int cbSize = Marshal.SizeOf(typeof(Native.INPUTS));
        int dwFlags = 0;
        switch (button)
        {
            case MouseButtons.Left:
                dwFlags = 4;
                break;
            case MouseButtons.Right:
                dwFlags = 16;
                break;
            case MouseButtons.Middle:
                dwFlags = 64;
                break;
            case MouseButtons.XButton1:
            case MouseButtons.XButton2:
                dwFlags = 256;
                if (button == MouseButtons.XButton1)
                {
                    pInputs.inputs.mi.mouseData = 1;
                }
                else
                {
                    pInputs.inputs.mi.mouseData = 2;
                }
                break;
        }
        pInputs.type = 0;
        pInputs.inputs.mi.dwFlags = dwFlags;
        Native.SendInput(1u, ref pInputs, cbSize);
    }

    public static Task ButtonPress(MouseButtons button, int delay = 50)
    {
        return Task.Factory.StartNew(async () =>
        {
            ButtonDown(button);
            if (delay > 0)
            {
                await Task.Delay(delay);
            }
            ButtonUp(button);
        });
    }

    public static void MouseWheel(int delta)
    {
        Native.INPUTS pInputs = default(Native.INPUTS);
        int cbSize = Marshal.SizeOf(typeof(Native.INPUTS));
        pInputs.type = 0;
        pInputs.inputs.mi.dwFlags = 2048;
        pInputs.inputs.mi.mouseData = delta;
        Native.SendInput(1u, ref pInputs, cbSize);
    }
}
