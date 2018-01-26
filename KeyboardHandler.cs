using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Qellatalo.Nin.TheHands.WIHandler;

namespace Qellatalo.Nin.TheHands
{
    /// <summary>
    /// Make windows keyboard actions.
    /// </summary>
    public class KeyboardHandler
    {
        /// <summary>
        /// Default delay (milliseconds) after a keyboard action.
        /// </summary>
        public int DefaultKeyboardActionDelay { get; set; } = 0;

        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public KeyboardEventFlags dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        /// <remarks>
        /// The extended keys consist of the ALT and CTRL keys on the right-hand side of the keyboard; the INS, DEL, HOME, END, PAGE UP, PAGE DOWN, and arrow keys in the clusters to the left of the numeric keypad; the NUM LOCK key; the BREAK (CTRL+PAUSE) key; the PRINT SCRN key; and the divide (/) and ENTER keys in the numeric keypad.
        /// 
        /// See http://msdn.microsoft.com/en-us/library/ms646267(v=vs.85).aspx Section "Extended-Key Flag"
        /// </remarks>
        internal bool IsExtendedKey(Keys keyCode)
        {
            if (keyCode == Keys.Menu ||
                keyCode == Keys.LMenu ||
                keyCode == Keys.RMenu ||
                keyCode == Keys.Control ||
                keyCode == Keys.RControlKey ||
                keyCode == Keys.Insert ||
                keyCode == Keys.Delete ||
                keyCode == Keys.Home ||
                keyCode == Keys.End ||
                keyCode == Keys.Prior ||
                keyCode == Keys.Next ||
                keyCode == Keys.Right ||
                keyCode == Keys.Up ||
                keyCode == Keys.Left ||
                keyCode == Keys.Down ||
                keyCode == Keys.NumLock ||
                keyCode == Keys.Cancel ||
                keyCode == Keys.Snapshot ||
                keyCode == Keys.Divide)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// According to http://inputsimulator.codeplex.com, this need to be checked for character input
        /// </summary>
        /// <param name="wScan"></param>
        /// <returns></returns>
        internal bool IsExtendedKey(ushort wScan)
        {
            return (wScan & 0xFF00) == 0xE000;
        }

        internal enum InputType : uint // UInt32
        {
            /// <summary>
            /// INPUT_MOUSE = 0x00 (The event is a mouse event. Use the mi structure of the union.)
            /// </summary>
            Mouse = 0,

            /// <summary>
            /// INPUT_KEYBOARD = 0x01 (The event is a keyboard event. Use the ki structure of the union.)
            /// </summary>
            Keyboard = 1,

            /// <summary>
            /// INPUT_HARDWARE = 0x02 (Windows 95/98/Me: The event is from input hardware other than a keyboard or mouse. Use the hi structure of the union.)
            /// </summary>
            Hardware = 2,
        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms646271(v=vs.85).aspx
        /// Remarks
        ///
        /// INPUT_KEYBOARD supports nonkeyboard-input methods—such as handwriting recognition or voice recognition—as if it were text input by using the KEYEVENTF_UNICODE flag.
        /// If KEYEVENTF_UNICODE is specified, SendInput sends a WM_KEYDOWN or WM_KEYUP message to the foreground thread's message queue with wParam equal to VK_PACKET.
        /// Once GetMessage or PeekMessage obtains this message, passing the message to TranslateMessage posts a WM_CHAR message with the Unicode character originally specified by wScan.
        /// This Unicode character will automatically be converted to the appropriate ANSI value if it is posted to an ANSI window.
        /// Sets the KEYEVENTF_SCANCODE flag to define keyboard input in terms of the scan code.This is useful to simulate a physical keystroke regardless of which keyboard is currently being used.The virtual key value of a key may alter depending on the current keyboard layout or what other keys were pressed, but the scan code will always be the same.
        /// </summary>
        [Flags]
        internal enum KeyboardEventFlags : uint
        {
            /// <summary>
            /// If specified, the scan code was preceded by a prefix byte that has the value 0xE0 (224).
            /// </summary>
            KEYEVENTF_EXTENDEDKEY = 0x0001,

            /// <summary>
            /// If specified, the key is being released.
            /// If not specified, the key is being pressed.
            /// </summary>
            KEYEVENTF_KEYUP = 0x0002,

            /// <summary>
            /// If specified, the system synthesizes a VK_PACKET keystroke.
            /// The wVk parameter must be zero.
            /// This flag can only be combined with the KEYEVENTF_KEYUP flag.
            /// For more information, see the Remarks section.
            /// </summary>
            KEYEVENTF_UNICODE = 0x0004,

            /// <summary>
            /// If specified, wScan identifies the key and wVk is ignored.
            /// </summary>            
            KEYEVENTF_SCANCODE = 0x0008

        }
        /// <summary>
        /// Releases a specified key.
        /// </summary>
        /// <param name="keyCode">The key to be released.</param>
        public void KeyUp(Keys keyCode)
        {
            INPUT input = new INPUT
            {
                type = SendInputEventType.InputKeyboard,
                mkhi =
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = (ushort) keyCode,
                        //wScan = 0,
                        dwFlags = (IsExtendedKey(keyCode) ?
                            KeyboardEventFlags.KEYEVENTF_KEYUP | KeyboardEventFlags.KEYEVENTF_EXTENDEDKEY :
                            KeyboardEventFlags.KEYEVENTF_KEYUP)
                        //time = 0,
                        //dwExtraInfo = IntPtr.Zero
                    }
                }
            };
            SendInput(1, ref input, INPUT_SIZE);
            System.Threading.Thread.Sleep(DefaultKeyboardActionDelay);
        }
        /// <summary>
        /// Releases a specified key.
        /// </summary>
        /// <param name="character">The key to be released.</param>
        private void characterUp(char character)
        {
            INPUT input = new INPUT
            {
                type = SendInputEventType.InputKeyboard,
                mkhi =
                {
                    ki = new KEYBDINPUT
                    {
                        wScan = character,
                        dwFlags = (IsExtendedKey(character) ?
                            KeyboardEventFlags.KEYEVENTF_KEYUP | KeyboardEventFlags.KEYEVENTF_EXTENDEDKEY :
                            KeyboardEventFlags.KEYEVENTF_KEYUP)
                    }
                }
            };
            SendInput(1, ref input, INPUT_SIZE);
            System.Threading.Thread.Sleep(DefaultKeyboardActionDelay);
        }

        /// <summary>
        /// Presses a specified key.
        /// </summary>
        /// <param name="keyCode">The key to be pressed.</param>
        public void KeyDown(Keys keyCode)
        {
            INPUT input =
                new INPUT
                {
                    type = SendInputEventType.InputKeyboard,
                    mkhi =
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = (ushort) keyCode,
                            //wScan = 0,
                            dwFlags = IsExtendedKey(keyCode) ? KeyboardEventFlags.KEYEVENTF_UNICODE | KeyboardEventFlags.KEYEVENTF_EXTENDEDKEY : KeyboardEventFlags.KEYEVENTF_UNICODE
                            //time = 0,
                            //dwExtraInfo = IntPtr.Zero
                        }
                    }
                };
            SendInput(1, ref input, INPUT_SIZE);
            System.Threading.Thread.Sleep(DefaultKeyboardActionDelay);
        }

        /// <summary>
        /// Presses a specified key.
        /// </summary>
        /// <param name="character">The key to be pressed.</param>
        private void characterDown(char character)
        {
            INPUT input =
                new INPUT
                {
                    type = SendInputEventType.InputKeyboard,
                    mkhi =
                    {
                        ki = new KEYBDINPUT
                        {
                            wScan = character,
                            dwFlags = IsExtendedKey(character) ? KeyboardEventFlags.KEYEVENTF_UNICODE | KeyboardEventFlags.KEYEVENTF_EXTENDEDKEY : KeyboardEventFlags.KEYEVENTF_UNICODE
                        }
                    }
                };
            SendInput(1, ref input, INPUT_SIZE);
            System.Threading.Thread.Sleep(DefaultKeyboardActionDelay);
        }

        /// <summary>
        /// Types a specified key.
        /// </summary>
        /// <param name="key">The key to be typed.</param>
        public void KeyTyping(Keys key)
        {
            KeyDown(key);
            KeyUp(key);
        }

        /// <summary>
        /// Types a specified key.
        /// </summary>
        /// <param name="character">The key to be typed.</param>
        public void CharacterInput(char character)
        {
            characterDown(character);
            characterUp(character);
        }
        /// <summary>
        /// Types a text.
        /// </summary>
        /// <param name="text">The text to be typed.</param>
        public void StringInput(string text)
        {
            foreach(char c in text)
            {
                CharacterInput(c);
            }
        }
    }
}
