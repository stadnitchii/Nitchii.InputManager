using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nitchii.InputManager;

internal static class Native
{
    public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

    [StructLayout(LayoutKind.Sequential)]
    public class KBDLLHOOKSTRUCT
    {
        public int vkCode;

        public int scanCode;

        public int flags;

        public int time;

        public UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class MSLLHOOKSTRUCT
    {
        public POINT pt;

        public int mouseData;

        public int flags;

        public int time;

        public IntPtr dwExtraInfo;
    }

    public struct POINT
    {
        public int x;

        public int y;
    }

    public struct INPUTS
    {
        public int type;

        public MOUSEKEYBDHARDWAREINPUT inputs;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct MOUSEKEYBDHARDWAREINPUT
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;

        [FieldOffset(0)]
        public KEYBDINPUT ki;

        [FieldOffset(0)]
        public HARDWAREINPUT hi;
    }

    public struct MOUSEINPUT
    {
        public int dx;

        public int dy;

        public int mouseData;

        public int dwFlags;

        public int time;

        private IntPtr dwExtraInfo;
    }

    public struct KEYBDINPUT
    {
        public short wVK;

        public short wScan;

        public int dwFlags;

        public int time;

        private IntPtr dwExtraInfo;
    }

    public struct HARDWAREINPUT
    {
        public int uMsg;

        public short wParamL;

        public short wParamH;
    }

    public static IntPtr User32Lib = LoadLibrary("User32");

    public const int WH_KEYBOARD_LL = 13;

    public const int WH_MOUSE_LL = 14;

    public const int WM_KEYDOWN = 256;

    public const int WM_KEYUP = 257;

    public const int WM_SYSKEYDOWN = 260;

    public const int WM_SYSKEYUP = 261;

    public const int WM_MOUSEMOVE = 512;

    public const int WM_LBUTTONDOWN = 513;

    public const int WM_LBUTTONUP = 514;

    public const int WM_RBUTTONDOWN = 516;

    public const int WM_RBUTTONUP = 517;

    public const int WM_MBUTTONDOWN = 519;

    public const int WM_MBUTTONUP = 520;

    public const int WM_MOUSEWHEEL = 522;

    public const int WM_XBUTTONDOWN = 523;

    public const int WM_XBUTTONUP = 524;

    public const int INPUT_MOUSE = 0;

    public const int INPUT_KEYBOARD = 1;

    public const int INPUT_HARDWARE = 2;

    public const int MOUSEEVENTF_MOVE = 1;

    public const int MOUSEEVENTF_LEFTDOWN = 2;

    public const int MOUSEEVENTF_LEFTUP = 4;

    public const int MOUSEEVENTF_RIGHTDOWN = 8;

    public const int MOUSEEVENTF_RIGHTUP = 16;

    public const int MOUSEEVENTF_MIDDLEDOWN = 32;

    public const int MOUSEEVENTF_MIDDLEUP = 64;

    public const int MOUSEEVENTF_XDOWN = 128;

    public const int MOUSEEVENTF_XUP = 256;

    public const int MOUSEEVENTF_WHEEL = 2048;

    public const int MOUSEEVENTF_ABSOLUTE = 32768;

    public const int KEYEVENTF_EXTENDEDKEY = 1;

    public const int KEYEVENTF_KEYUP = 2;

    public const int KEYEVENTF_SCANCODE = 8;

    public const int MAPVK_VK_TO_VSC = 0;

    public const int VK_LSHIFT = 160;

    public const int VK_RSHIFT = 161;

    public const int VK_LCONTROL = 162;

    public const int VK_RCONTROL = 163;

    public const int VK_LMENU = 164;

    public const int VK_RMENU = 165;

    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowsHookEx(int idHook, HookProc callback, IntPtr hInstance, uint threadId);

    [DllImport("user32.dll")]
    public static extern bool UnhookWindowsHookEx(IntPtr hInstance);

    [DllImport("user32.dll")]
    public static extern int CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    public static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("user32.dll")]
    public static extern uint SendInput(uint nInputs, ref INPUTS pInputs, int cbSize);

    [DllImport("user32.dll")]
    public static extern uint MapVirtualKey(uint uCode, uint uMapType);

    [DllImport("user32.dll")]
    public static extern bool GetAsyncKeyState(int vKey);
}