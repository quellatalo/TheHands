using System.Runtime.InteropServices;

namespace Quellatalo.Nin.TheHands
{
    internal static class WIHandler
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            public SendInputEventType type;
            public MouseKeybdhardwareInputUnion mkhi;
        }

        internal static readonly int INPUT_SIZE = Marshal.SizeOf(typeof(INPUT));

        [StructLayout(LayoutKind.Explicit)]
        internal struct MouseKeybdhardwareInputUnion
        {
            [FieldOffset(0)]
            public MouseHandler.MouseInputData mi;

            [FieldOffset(0)]
            public KeyboardHandler.KEYBDINPUT ki;

            [FieldOffset(0)]
            public Hardware.HARDWAREINPUT hi;
        }


        internal enum SendInputEventType : uint
        {
            InputMouse,
            InputKeyboard,
            InputHardware
        }

        internal class Hardware
        {
            [StructLayout(LayoutKind.Sequential)]
            internal struct HARDWAREINPUT
            {
                public int uMsg;
                public short wParamL;
                public short wParamH;
            }
        }
    }
}
