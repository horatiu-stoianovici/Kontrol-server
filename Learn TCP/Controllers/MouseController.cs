using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Kontrol.Controllers
{
    public class MouseController
    {
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData,
          int dwExtraInfo);

        public enum MouseEventFlags : uint
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010,
            WHEEL = 0x00000800,
            XDOWN = 0x00000080,
            XUP = 0x00000100,
            HORIZONTAL_WHEEL = 0x01000
        }

        public static void LeftClickDown()
        {
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
        }

        public static void LeftClickUp()
        {
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
        }

        public static void RightClickDown()
        {
            mouse_event((int)(MouseEventFlags.RIGHTDOWN), 0, 0, 0, 0);
        }

        public static void RightClickUp()
        {
            mouse_event((int)(MouseEventFlags.RIGHTUP), 0, 0, 0, 0);
        }

        private const float DECELERATION_RATE = 0.005f; //in px/ms
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;
        private const uint MOUSEEVENTF_WHEEL = 0x0800;
        private const uint MOUSEEVENTF_HWHEEL = 0x01000;

        public static void SetMousePosition(Point pos)
        {
            System.Windows.Forms.Cursor.Position = pos;
        }

        public static Point GetCurrentMousePosition()
        {
            return new Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
        }

        public static void ScrollVertically(int amount, Point currentPos = new Point())
        {
            if (currentPos.IsEmpty)
                currentPos = GetCurrentMousePosition();

            mouse_event(MOUSEEVENTF_WHEEL, Convert.ToUInt32(currentPos.X), Convert.ToUInt32(currentPos.Y), Convert.ToInt32(amount), 0);
        }

        public static void ScrollHorizontally(int amount, Point currentPos = new Point())
        {
            if (currentPos.IsEmpty)
                currentPos = GetCurrentMousePosition();

            mouse_event(MOUSEEVENTF_HWHEEL, Convert.ToUInt32(currentPos.X), Convert.ToUInt32(currentPos.Y), Convert.ToInt32(amount), 0);
        }

        private static volatile bool scrolling;

        public static void StopScrolling()
        {
            scrolling = false;
        }

        public static void ScrollWithInitialVelocity(float velocityX, float velocityY) //velocity is in px / 10ms
        {
            new Thread(new ThreadStart(delegate()
            {
                Point currentPos = GetCurrentMousePosition();

                scrolling = true;

                int signX = velocityX > 0 ? -1 : 1;
                velocityX = velocityX > 0 ? velocityX : -velocityX;

                int signY = velocityY > 0 ? -1 : 1;
                velocityY = velocityY > 0 ? velocityY : -velocityY;

                int amountScrolledX = 0, amountScrolledY = 0;
                Stopwatch stopwatch = new Stopwatch();

                bool scrollingX = true;
                bool scrollingY = true;

                stopwatch.Start();
                while (scrolling)
                {
                    long time = stopwatch.ElapsedMilliseconds;

                    //calculate current position of the scroll based on newton's equations
                    float currentDistX = velocityX * time * 0.1f - DECELERATION_RATE * time * time * 1.0f / 2;
                    float currentDistY = velocityY * time * 0.1f - DECELERATION_RATE * time * time * 1.0f / 2;

                    //scroll is only done in relative int values, amountScrolled is how much we scrolled, and currentDist is how much we should have scrolled
                    if (scrollingX)
                    {
                        if ((int)currentDistX > amountScrolledX)
                        {
                            ScrollHorizontally(((int)currentDistX - amountScrolledX) * signX, currentPos);
                            amountScrolledX = (int)currentDistX;
                        }
                        else if ((int)currentDistX < amountScrolledX)
                        {
                            scrollingX = false;
                        }
                    }

                    if (scrollingY)
                    {
                        if ((int)currentDistY > amountScrolledY)
                        {
                            ScrollVertically(((int)currentDistY - amountScrolledY) * signY, currentPos);
                            amountScrolledY = (int)currentDistY;
                        }
                        else if ((int)currentDistY < amountScrolledY)
                        {
                            scrollingY = false;
                        }
                    }

                    if (!(scrollingY || scrollingX))
                        scrolling = false;
                }
                stopwatch.Stop();
            })).Start();
        }
    }
}