using Nitchii.InputManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nitchii.InputManager;

public delegate void KeyEventHandler(object? sender, Keys key);

public sealed class KeyboardHook
{
    private static KeyboardHook? instance = null;

    private static readonly object padlock = new object();

    private Native.HookProc keyboardCallback;

    private IntPtr hInstance = IntPtr.Zero;

    public static KeyboardHook Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new KeyboardHook();
                }
                return instance;
            }
        }
    }

    public event KeyEventHandler? KeyDown;

    public event KeyEventHandler? KeyUp;

    private KeyboardHook()
    {
        keyboardCallback = keyboardHookProc;
        hInstance = Native.SetWindowsHookEx(13, keyboardCallback, Native.User32Lib, 0u);
    }

    private int keyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            Native.KBDLLHOOKSTRUCT kBDLLHOOKSTRUCT = new Native.KBDLLHOOKSTRUCT();
            Marshal.PtrToStructure(lParam, kBDLLHOOKSTRUCT);
            int num = (int)wParam;
            bool num2 = num == 256 || num == 260;
            bool flag = num == 257 || num == 261;
            Keys key = (Keys)kBDLLHOOKSTRUCT.vkCode;
            if (num2 && this.KeyDown != null)
            {
                this.KeyDown(this, key);
            }
            else if (flag && this.KeyUp != null)
            {
                this.KeyUp(this, key);
            }
        }
        return Native.CallNextHookEx(hInstance, nCode, wParam, lParam);
    }
}
