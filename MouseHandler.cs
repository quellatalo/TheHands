using System;
using System.Drawing;
using System.Runtime.InteropServices;
using static Quellatalo.Nin.TheHands.WIHandler;

namespace Quellatalo.Nin.TheHands
{
    /// <summary>
    /// Make windows mouse actions.
    /// </summary>
    public class MouseHandler
    {
        /// <summary>
        /// Default delay (milliseconds) after a mouse action.
        /// </summary>
        public int DefaultMouseActionDelay { get; set; } = 0;

        /// <summary>
        /// Default offset bound.
        /// </summary>
        public int DefaultOffsetBound { get; set; } = 5;

        internal struct MouseInputData
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public MouseEventFlags dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [Flags]
        internal enum MouseEventFlags : uint
        {
            /// <summary>
            /// Movement occurred.
            /// </summary>
            MOUSEEVENTF_MOVE = 0x0001,

            /// <summary>
            /// The WM_MOUSEMOVE messages will not be coalesced.
            /// The default behavior is to coalesce WM_MOUSEMOVE messages.
            /// Windows XP/2000:  This value is not supported.
            /// </summary>
            MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000,

            /// <summary>
            /// The left button was pressed.
            /// </summary>
            MOUSEEVENTF_LEFTDOWN = 0x0002,

            /// <summary>
            /// The left button was released.
            /// </summary>
            MOUSEEVENTF_LEFTUP = 0x0004,

            /// <summary>
            /// The right button was pressed.
            /// </summary>
            MOUSEEVENTF_RIGHTDOWN = 0x0008,

            /// <summary>
            /// The right button was released.
            /// </summary>
            MOUSEEVENTF_RIGHTUP = 0x0010,

            /// <summary>
            /// The middle button was pressed.
            /// </summary>
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,

            /// <summary>
            /// The middle button was released.
            /// </summary>
            MOUSEEVENTF_MIDDLEUP = 0x0040,

            /// <summary>
            /// Maps coordinates to the entire desktop. Must be used with MOUSEEVENTF_ABSOLUTE.
            /// </summary>
            MOUSEEVENTF_VIRTUALDESK = 0x4000,

            /// <summary>
            /// An X button was pressed.
            /// </summary>
            MOUSEEVENTF_XDOWN = 0x0080,

            /// <summary>
            /// An X button was released.
            /// </summary>
            MOUSEEVENTF_XUP = 0x0100,

            /// <summary>
            /// The wheel was moved, if the mouse has a wheel. The amount of movement is specified in mouseData.
            /// </summary>
            MOUSEEVENTF_WHEEL = 0x0800,

            /// <summary>
            /// The wheel was moved horizontally, if the mouse has a wheel.
            /// The amount of movement is specified in mouseData.
            /// Windows XP/2000:  This value is not supported.
            /// </summary>
            MOUSEEVENTF_HWHEEL = 0x01000,

            /// <summary>
            /// The dx and dy members contain normalized absolute coordinates.
            /// If the flag is not set, dxand dy contain relative data (the change in position since the last reported position).
            /// This flag can be set, or not set, regardless of what kind of mouse or other pointing device, if any, is connected to the system.
            /// For further information about relative mouse motion, see the following Remarks section.
            /// </summary>
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }

        private enum SystemMetric
        {
            SM_CXSCREEN = 0,
            SM_CYSCREEN = 1
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out Point lpMousePoint);

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(SystemMetric smIndex);

        private Random random = new Random();
        /// <summary>
        /// Gets current mouse point.
        /// </summary>
        /// <returns></returns>
        public Point GetPosition()
        {
            var gotPoint = GetCursorPos(out Point currentMousePoint);
            if (!gotPoint) { currentMousePoint = new Point(0, 0); }
            return currentMousePoint;
        }
        /// <summary>
        /// Press left mouse button.
        /// </summary>
        public void LeftDown()
        {
            INPUT mouseDownInput = new INPUT();
            mouseDownInput.type = SendInputEventType.InputMouse;
            mouseDownInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTDOWN;
            SendInput(1, ref mouseDownInput, INPUT_SIZE);
            System.Threading.Thread.Sleep(DefaultMouseActionDelay);
        }
        /// <summary>
        /// Release left mouse button.
        /// </summary>
        public void LeftUp()
        {
            INPUT mouseUpInput = new INPUT();
            mouseUpInput.type = SendInputEventType.InputMouse;
            mouseUpInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTUP;
            SendInput(1, ref mouseUpInput, INPUT_SIZE);
            System.Threading.Thread.Sleep(DefaultMouseActionDelay);
        }
        /// <summary>
        /// Press right mouse button.
        /// </summary>
        public void RightDown()
        {
            INPUT mouseDownInput = new INPUT();
            mouseDownInput.type = SendInputEventType.InputMouse;
            mouseDownInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTDOWN;
            SendInput(1, ref mouseDownInput, INPUT_SIZE);
            System.Threading.Thread.Sleep(DefaultMouseActionDelay);
        }
        /// <summary>
        /// Release right mouse button.
        /// </summary>
        public void RightUp()
        {
            INPUT mouseUpInput = new INPUT();
            mouseUpInput.type = SendInputEventType.InputMouse;
            mouseUpInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTUP;
            SendInput(1, ref mouseUpInput, INPUT_SIZE);
            System.Threading.Thread.Sleep(DefaultMouseActionDelay);
        }
        /// <summary>
        /// Move mouse.
        /// </summary>
        /// <param name="x">Move x.</param>
        /// <param name="y">Move y</param>
        public void Move(int x, int y)
        {
            Point mousePoint = GetPosition();
            MoveTo(mousePoint.X + x, mousePoint.Y + y);
        }
        /// <summary>
        /// Move mouse based on point value.
        /// </summary>
        /// <param name="point">Move value.</param>
        public void Move(Point point)
        {
            Move(point.X, point.Y);
        }
        /// <summary>
        /// Move mouse to a specified point.
        /// </summary>
        /// <param name="x">Target x.</param>
        /// <param name="y">Target y.</param>
        public void MoveTo(int x, int y)
        {
            INPUT mouseUpInput = new INPUT();
            mouseUpInput.type = SendInputEventType.InputMouse;
            mouseUpInput.mkhi.mi.dx = (x + 1) * 65535 / GetSystemMetrics(SystemMetric.SM_CXSCREEN);
            mouseUpInput.mkhi.mi.dy = (y + 1) * 65535 / GetSystemMetrics(SystemMetric.SM_CYSCREEN);
            mouseUpInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_MOVE | MouseEventFlags.MOUSEEVENTF_ABSOLUTE;
            SendInput(1, ref mouseUpInput, INPUT_SIZE);
            System.Threading.Thread.Sleep(DefaultMouseActionDelay);
        }
        /// <summary>
        /// Move mouse to a specified point.
        /// </summary>
        /// <param name="mousePoint">Target.</param>
        public void MoveTo(Point mousePoint)
        {
            MoveTo(mousePoint.X, mousePoint.Y);
        }
        /// <summary>
        /// Mouse left click.
        /// </summary>
        public void Click()
        {
            LeftDown();
            LeftUp();
        }
        /// <summary>
        /// Mouse left click at a specified point.
        /// </summary>
        /// <param name="x">Target x.</param>
        /// <param name="y">Target y.</param>
        public void Click(int x, int y)
        {
            MoveTo(x, y);
            Click();
        }
        /// <summary>
        /// Mouse left click at a point.
        /// </summary>
        /// <param name="point">Target.</param>
        public void Click(Point point)
        {
            Click(point.X, point.Y);
        }
        /// <summary>
        /// Mouse right click.
        /// </summary>
        public void RightClick()
        {
            RightDown();
            RightUp();
        }
        /// <summary>
        /// Mouse right click at a point.
        /// </summary>
        /// <param name="x">Target x.</param>
        /// <param name="y">Target y.</param>
        public void RightClick(int x, int y)
        {
            MoveTo(x, y);
            RightClick();
        }
        /// <summary>
        /// Right click at a point.
        /// </summary>
        /// <param name="point">Target.</param>
        public void RightClick(Point point)
        {
            RightClick(point.X, point.Y);
        }
        /// <summary>
        /// Left mouse button drag to a point.
        /// </summary>
        /// <param name="x">Target x.</param>
        /// <param name="y">Target y.</param>
        public void LeftDragTo(int x, int y)
        {
            LeftDown();
            MoveTo(x, y);
            LeftUp();
        }
        /// <summary>
        /// Left mouse button dra to a point.
        /// </summary>
        /// <param name="point">Target.</param>
        public void LeftDragTo(Point point)
        {
            LeftDragTo(point.X, point.Y);
        }
        /// <summary>
        /// Left mouse button drag.
        /// </summary>
        /// <param name="fromX">From x.</param>
        /// <param name="fromY">From y.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        public void LeftDrag(int fromX, int fromY, int toX, int toY)
        {
            MoveTo(fromX, fromY);
            LeftDragTo(toX, toY);
        }
        /// <summary>
        /// Left mouse button drag.
        /// </summary>
        /// <param name="fromX">From x.</param>
        /// <param name="fromY">From y.</param>
        /// <param name="to">To point.</param>
        public void LeftDrag(int fromX, int fromY, Point to)
        {
            LeftDrag(fromX, fromY, to.X, to.Y);
        }
        /// <summary>
        /// Left mouse button drag.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        public void LeftDrag(Point from, int toX, int toY)
        {
            LeftDrag(from.X, from.Y, toX, toY);
        }
        /// <summary>
        /// Left mouse button drag.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="to">To point.</param>
        public void LeftDrag(Point from, Point to)
        {
            LeftDrag(from.X, from.Y, to.X, to.Y);
        }
        /// <summary>
        /// Right mouse button drag to a point.
        /// </summary>
        /// <param name="x">Target x.</param>
        /// <param name="y">Target y.</param>
        public void RightDragTo(int x, int y)
        {
            RightDown();
            MoveTo(x, y);
            RightUp();
        }
        /// <summary>
        /// Right mouse button dra to a point.
        /// </summary>
        /// <param name="point">Target.</param>
        public void RightDragTo(Point point)
        {
            RightDragTo(point.X, point.Y);
        }
        /// <summary>
        /// Right mouse button drag.
        /// </summary>
        /// <param name="fromX">From x.</param>
        /// <param name="fromY">From y.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        public void RightDrag(int fromX, int fromY, int toX, int toY)
        {
            MoveTo(fromX, fromY);
            RightDragTo(toX, toY);
        }
        /// <summary>
        /// Right mouse button drag.
        /// </summary>
        /// <param name="fromX">From x.</param>
        /// <param name="fromY">From y.</param>
        /// <param name="to">To point.</param>
        public void RightDrag(int fromX, int fromY, Point to)
        {
            RightDrag(fromX, fromY, to.X, to.Y);
        }
        /// <summary>
        /// Right mouse button drag.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        public void RightDrag(Point from, int toX, int toY)
        {
            RightDrag(from.X, from.Y, toX, toY);
        }
        /// <summary>
        /// Right mouse button drag.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="to">To point.</param>
        public void RightDrag(Point from, Point to)
        {
            RightDrag(from.X, from.Y, to.X, to.Y);
        }
        /// <summary>
        /// Move mouse to a target point with random offset.
        /// </summary>
        /// <param name="x">Target x.</param>
        /// <param name="y">Target y.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void MoveToWithRandomOffset(int x, int y, int xOffset, int yOffset)
        {
            MoveTo(x + random.Next(xOffset), y + random.Next(yOffset));
        }
        /// <summary>
        /// Move mouse to a target point with random default offset.
        /// </summary>
        /// <param name="x">Target x.</param>
        /// <param name="y">Target y.</param>
        public void MoveToWithRandomOffset(int x, int y)
        {
            MoveTo(x + random.Next(DefaultOffsetBound), y + random.Next(DefaultOffsetBound));
        }
        /// <summary>
        /// Move mouse to a target point with random offset.
        /// </summary>
        /// <param name="point">Target.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void MoveToWithRandomOffset(Point point, int xOffset, int yOffset)
        {
            MoveToWithRandomOffset(point.X, point.Y, xOffset, yOffset);
        }
        /// <summary>
        /// Move mouse to a target point with random default offset.
        /// </summary>
        /// <param name="point">Target.</param>
        public void MoveToWithRandomOffset(Point point)
        {
            MoveToWithRandomOffset(point.X, point.Y);
        }
        /// <summary>
        /// Click a target point with random offset.
        /// </summary>
        /// <param name="x">Tartget x.</param>
        /// <param name="y">Target y.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void ClickWithRandomOffset(int x, int y, int xOffset, int yOffset)
        {
            MoveToWithRandomOffset(x, y, xOffset, yOffset);
            Click();
        }
        /// <summary>
        /// Click a target point with random default offset.
        /// </summary>
        /// <param name="x">Target x.</param>
        /// <param name="y">Target y.</param>
        public void ClickWithRandomOffset(int x, int y)
        {
            MoveToWithRandomOffset(x, y);
            Click();
        }
        /// <summary>
        /// Click a target point with random offset.
        /// </summary>
        /// <param name="point">Target.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void ClickWithRandomOffset(Point point, int xOffset, int yOffset)
        {
            ClickWithRandomOffset(point.X, point.Y, xOffset, yOffset);
        }
        /// <summary>
        /// Click a target point with random default offset.
        /// </summary>
        /// <param name="point">Target.</param>
        public void ClickWithRandomOffset(Point point)
        {
            ClickWithRandomOffset(point.X, point.Y);
        }
        /// <summary>
        /// Left mouse drag to a point with random offset.
        /// </summary>
        /// <param name="x">Tartet x.</param>
        /// <param name="y">Target y.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void LeftDragToWithRandomOffset(int x, int y, int xOffset, int yOffset)
        {
            LeftDown();
            MoveToWithRandomOffset(x, y, xOffset, yOffset);
            LeftUp();
        }
        /// <summary>
        /// Left mouse drag to a point with random default offset.
        /// </summary>
        /// <param name="x">x offset.</param>
        /// <param name="y">y offset.</param>
        public void LeftDragToWithRandomOffset(int x, int y)
        {
            LeftDown();
            MoveToWithRandomOffset(x, y);
            LeftUp();
        }
        /// <summary>
        /// Left mouse drag to a point with random offset.
        /// </summary>
        /// <param name="point">Target.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void LeftDragToWithRandomOffset(Point point, int xOffset, int yOffset)
        {
            LeftDragToWithRandomOffset(point.X, point.Y, xOffset, yOffset);
        }
        /// <summary>
        /// Left mouse drag to a point with random default offset.
        /// </summary>
        /// <param name="point">Target.</param>
        public void LeftDragToWithRandomOffset(Point point)
        {
            LeftDragToWithRandomOffset(point.X, point.Y);
        }
        /// <summary>
        /// Left mouse drag with random offset.
        /// </summary>
        /// <param name="fromX">From x.</param>
        /// <param name="fromY">From y.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void LeftDragWithRandomOffset(int fromX, int fromY, int toX, int toY, int xOffset, int yOffset)
        {
            MoveToWithRandomOffset(fromX, fromY, xOffset, yOffset);
            LeftDragToWithRandomOffset(toX, toY, xOffset, yOffset);
        }
        /// <summary>
        /// Left mouse drag with random default offset.
        /// </summary>
        /// <param name="fromX">From x.</param>
        /// <param name="fromY">From y.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        public void LeftDragWithRandomOffset(int fromX, int fromY, int toX, int toY)
        {
            MoveToWithRandomOffset(fromX, fromY);
            LeftDragToWithRandomOffset(toX, toY);
        }
        /// <summary>
        /// Left mouse drag with random offset.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void LeftDragWithRandomOffset(Point from, int toX, int toY, int xOffset, int yOffset)
        {
            LeftDragWithRandomOffset(from.X, from.Y, toX, toY, xOffset, yOffset);
        }
        /// <summary>
        /// Left mouse drag with random default offset.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        public void LeftDragWithRandomOffset(Point from, int toX, int toY)
        {
            LeftDragWithRandomOffset(from.X, from.Y, toX, toY);
        }
        /// <summary>
        /// Left mouse drag with random offset.
        /// </summary>
        /// <param name="fromX">From x.</param>
        /// <param name="fromY">From y.</param>
        /// <param name="to">To point.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void LeftDragWithRandomOffset(int fromX, int fromY, Point to, int xOffset, int yOffset)
        {
            LeftDragWithRandomOffset(fromX, fromY, to.X, to.Y, xOffset, yOffset);
        }
        /// <summary>
        /// Left mouse drag with random default offset.
        /// </summary>
        /// <param name="fromX">From x.</param>
        /// <param name="fromY">From y.</param>
        /// <param name="to">To point.</param>
        public void LeftDragWithRandomOffset(int fromX, int fromY, Point to)
        {
            LeftDragWithRandomOffset(fromX, fromY, to.X, to.Y);
        }
        /// <summary>
        /// Left mouse drag with random offset.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="to">To point.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void LeftDragWithRandomOffset(Point from, Point to, int xOffset, int yOffset)
        {
            LeftDragWithRandomOffset(from.X, from.Y, to.X, to.Y, xOffset, yOffset);
        }
        /// <summary>
        /// Left mouse drag with random default offset.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="to">To point.</param>
        public void LeftDragWithRandomOffset(Point from, Point to)
        {
            LeftDragWithRandomOffset(from.X, from.Y, to.X, to.Y);
        }
        /// <summary>
        /// Right mouse drag to a point with random offset.
        /// </summary>
        /// <param name="x">Tartet x.</param>
        /// <param name="y">Target y.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void RightDragToWithRandomOffset(int x, int y, int xOffset, int yOffset)
        {
            RightDown();
            MoveToWithRandomOffset(x, y, xOffset, yOffset);
            RightUp();
        }
        /// <summary>
        /// Right mouse drag to a point with random default offset.
        /// </summary>
        /// <param name="x">x offset.</param>
        /// <param name="y">y offset.</param>
        public void RightDragToWithRandomOffset(int x, int y)
        {
            RightDown();
            MoveToWithRandomOffset(x, y);
            RightUp();
        }
        /// <summary>
        /// Right mouse drag to a point with random offset.
        /// </summary>
        /// <param name="point">Target.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void RightDragToWithRandomOffset(Point point, int xOffset, int yOffset)
        {
            RightDragToWithRandomOffset(point.X, point.Y, xOffset, yOffset);
        }
        /// <summary>
        /// Right mouse drag to a point with random default offset.
        /// </summary>
        /// <param name="point">Target.</param>
        public void RightDragToWithRandomOffset(Point point)
        {
            RightDragToWithRandomOffset(point.X, point.Y);
        }
        /// <summary>
        /// Right mouse drag with random offset.
        /// </summary>
        /// <param name="fromX">From x.</param>
        /// <param name="fromY">From y.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void RightDragWithRandomOffset(int fromX, int fromY, int toX, int toY, int xOffset, int yOffset)
        {
            MoveToWithRandomOffset(fromX, fromY, xOffset, yOffset);
            RightDragToWithRandomOffset(toX, toY, xOffset, yOffset);
        }
        /// <summary>
        /// Right mouse drag with random default offset.
        /// </summary>
        /// <param name="fromX">From x.</param>
        /// <param name="fromY">From y.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        public void RightDragWithRandomOffset(int fromX, int fromY, int toX, int toY)
        {
            MoveToWithRandomOffset(fromX, fromY);
            RightDragToWithRandomOffset(toX, toY);
        }
        /// <summary>
        /// Right mouse drag with random offset.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void RightDragWithRandomOffset(Point from, int toX, int toY, int xOffset, int yOffset)
        {
            RightDragWithRandomOffset(from.X, from.Y, toX, toY, xOffset, yOffset);
        }
        /// <summary>
        /// Right mouse drag with random default offset.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="toX">To x.</param>
        /// <param name="toY">To y.</param>
        public void RightDragWithRandomOffset(Point from, int toX, int toY)
        {
            RightDragWithRandomOffset(from.X, from.Y, toX, toY);
        }
        /// <summary>
        /// Right mouse drag with random offset.
        /// </summary>
        /// <param name="fromX">From x.</param>
        /// <param name="fromY">From y.</param>
        /// <param name="to">To point.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void RightDragWithRandomOffset(int fromX, int fromY, Point to, int xOffset, int yOffset)
        {
            RightDragWithRandomOffset(fromX, fromY, to.X, to.Y, xOffset, yOffset);
        }
        /// <summary>
        /// Right mouse drag with random default offset.
        /// </summary>
        /// <param name="fromX">From x.</param>
        /// <param name="fromY">From y.</param>
        /// <param name="to">To point.</param>
        public void RightDragWithRandomOffset(int fromX, int fromY, Point to)
        {
            RightDragWithRandomOffset(fromX, fromY, to.X, to.Y);
        }
        /// <summary>
        /// Right mouse drag with random offset.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="to">To point.</param>
        /// <param name="xOffset">x offset.</param>
        /// <param name="yOffset">y offset.</param>
        public void RightDragWithRandomOffset(Point from, Point to, int xOffset, int yOffset)
        {
            RightDragWithRandomOffset(from.X, from.Y, to.X, to.Y, xOffset, yOffset);
        }
        /// <summary>
        /// Right mouse drag with random default offset.
        /// </summary>
        /// <param name="from">From point.</param>
        /// <param name="to">To point.</param>
        public void RightDragWithRandomOffset(Point from, Point to)
        {
            RightDragWithRandomOffset(from.X, from.Y, to.X, to.Y);
        }

        /// <summary>
        /// Drag through a chain of points using right mouse button.
        /// (The right mouse button is pressed before the drag, and is released after the drag.
        /// </summary>
        /// <param name="points">The points to drag in sequence.</param>
        public void RightDrag(Point[] points)
        {
            if (points.Length > 0)
            {
                MoveTo(points[0]);
                RightDown();
                for (int i = 1; i < points.Length; i++)
                {
                    MoveTo(points[i]);
                }
                RightUp();
            }
        }
        /// <summary>
        /// Drag through a chain of points using right mouse button.
        /// (The right mouse button is pressed before the drag, and is released after the drag.
        /// </summary>
        /// <param name="points">The points to drag in sequence.</param>
        public void RightDrag(System.Collections.Generic.List<Point> points)
        {
            RightDrag(points.ToArray());
        }
        /// <summary>
        /// Drag through a chain of points using left mouse button.
        /// (The left mouse button is pressed before the drag, and is released after the drag.
        /// </summary>
        /// <param name="points">The points to drag in sequence.</param>
        public void LeftDrag(Point[] points)
        {
            if (points.Length > 0)
            {
                MoveTo(points[0]);
                LeftDown();
                for (int i = 1; i < points.Length; i++)
                {
                    MoveTo(points[i]);
                }
                LeftUp();
            }
        }
        /// <summary>
        /// Drag through a chain of points using left mouse button.
        /// (The left mouse button is pressed before the drag, and is released after the drag.
        /// </summary>
        /// <param name="points">The points to drag in sequence.</param>
        public void LeftDrag(System.Collections.Generic.List<Point> points)
        {
            LeftDrag(points.ToArray());
        }

        /// <summary>
        /// Move the mouse cursor through a chain of points.
        /// </summary>
        /// <param name="points">The points to move though in sequence.</param>
        public void MoveTo(Point[] points)
        {
            foreach(Point point in points)
            {
                MoveTo(point);
            }
        }

        /// <summary>
        /// Move the mouse cursor through a chain of points.
        /// </summary>
        /// <param name="points">The points to move though in sequence.</param>
        public void MoveTo(System.Collections.Generic.List<Point> points)
        {
            foreach (Point point in points)
            {
                MoveTo(point);
            }
        }
    }
}
