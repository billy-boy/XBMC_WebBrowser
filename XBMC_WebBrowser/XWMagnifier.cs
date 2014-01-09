using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace XBMC_WebBrowser
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
    public class Gdi32
    {
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    }
    public static class ApiConstants
    {
        public const int SRCCOPY = 13369376;
    }
    public static class User32
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
    }
    public class XWMagnifier
    {
        public static Image CaptureMagnifier(IntPtr handle, Point cursor, Size magnifierSize)
        {
            //Take shoot User32.GetDesktopWindow()
            Image fullWindow = CaptureWindow(handle);
            fullWindow.Save("E:\\xbmc_webbrowser\\screen_" + DateTime.Now.Ticks.ToString() + ".png");

            //Detecting X/Y position from which we would take range
            int wb_x = cursor.X;
            wb_x -= magnifierSize.Width / 2;
            if (wb_x < 0) wb_x = 0;
            int wb_y = cursor.Y;
            wb_y -= magnifierSize.Height / 2;
            if (wb_y < 0) wb_y = 0;

            //Detecting w/h size from range we would take
            int mg_w = magnifierSize.Width;
            if (wb_x + mg_w > fullWindow.Width) mg_w = fullWindow.Width - wb_x;
            int mg_h = magnifierSize.Height;
            if (wb_y + mg_h > fullWindow.Height) mg_h = fullWindow.Height - wb_y;

            Bitmap magnifierRange = new Bitmap(magnifierSize.Width, magnifierSize.Height);
            Graphics g = Graphics.FromImage(magnifierRange);
            g.DrawImage(fullWindow, 0, 0, new Rectangle(wb_x, wb_y, mg_w, mg_h), GraphicsUnit.Pixel);
            g.Dispose();
            fullWindow.Dispose();
            return (Image)magnifierRange;
        }

        public static Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }

        public static Image CaptureWindow(IntPtr handle)
        {

            IntPtr hdcSrc = User32.GetWindowDC(handle);

            RECT windowRect = new RECT();
            User32.GetWindowRect(handle, ref windowRect);

            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;

            IntPtr hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, width, height);

            IntPtr hOld = Gdi32.SelectObject(hdcDest, hBitmap);
            Gdi32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, ApiConstants.SRCCOPY);
            Gdi32.SelectObject(hdcDest, hOld);
            Gdi32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);

            Image image = Image.FromHbitmap(hBitmap);
            Gdi32.DeleteObject(hBitmap);

            return image;
        }
    }
}
