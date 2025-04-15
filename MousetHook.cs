using Nitchii.InputManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nitchii.InputManager;

public delegate void MouseEventHandler(object? sender, MouseEventArgs e);

public class MouseHook
{
    private static MouseHook? instance = null;

    private static readonly object padlock = new object();

    private Native.HookProc mouseCallback;

    private IntPtr hInstance = IntPtr.Zero;

    public static MouseHook Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new MouseHook();
                }
                return instance;
            }
        }
    }

    public event MouseEventHandler? MouseMoved;

    public event MouseEventHandler? MouseDown;

    public event MouseEventHandler? MouseUp;

    public event MouseEventHandler? MouseWheel;

    private MouseHook()
    {
        mouseCallback = mouseHookProc;
        hook();
    }

    private void hook()
    {
        hInstance = Native.SetWindowsHookEx(14, mouseCallback, Native.User32Lib, 0u);
    }

    private int mouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            Native.MSLLHOOKSTRUCT mSLLHOOKSTRUCT = new Native.MSLLHOOKSTRUCT();
            Marshal.PtrToStructure(lParam, mSLLHOOKSTRUCT);
            int num = (int)wParam;
            int num2 = mSLLHOOKSTRUCT.mouseData >> 16;
            if (this.MouseMoved != null && num == 512)
            {
                MouseEventArgs e = new MouseEventArgs(MouseButtons.None, 0, mSLLHOOKSTRUCT.pt.x, mSLLHOOKSTRUCT.pt.y, 0);
                this.MouseMoved(this, e);
            }
            else if (this.MouseDown != null && (num == 513 || num == 516 || num == 519 || num == 523))
            {
                MouseEventArgs e2 = new MouseEventArgs(num switch
                {
                    513 => MouseButtons.Left,
                    516 => MouseButtons.Right,
                    519 => MouseButtons.Middle,
                    _ => (num2 != 1) ? MouseButtons.XButton2 : MouseButtons.XButton1,
                }, 1, mSLLHOOKSTRUCT.pt.x, mSLLHOOKSTRUCT.pt.y, 0);
                this.MouseDown(this, e2);
            }
            else if (this.MouseUp != null && (num == 514 || num == 517 || num == 520 || num == 524))
            {
                MouseEventArgs e3 = new MouseEventArgs(num switch
                {
                    514 => MouseButtons.Left,
                    517 => MouseButtons.Right,
                    520 => MouseButtons.Middle,
                    _ => (num2 != 1) ? MouseButtons.XButton2 : MouseButtons.XButton1,
                }, 1, mSLLHOOKSTRUCT.pt.x, mSLLHOOKSTRUCT.pt.y, 0);
                this.MouseUp(this, e3);
            }
            else if (this.MouseWheel != null && num == 522)
            {
                int delta = mSLLHOOKSTRUCT.mouseData >> 16;
                MouseEventArgs e4 = new MouseEventArgs(MouseButtons.None, 0, mSLLHOOKSTRUCT.pt.x, mSLLHOOKSTRUCT.pt.y, delta);
                this.MouseWheel(this, e4);
            }
        }
        return Native.CallNextHookEx(hInstance, nCode, wParam, lParam);
    }
}
