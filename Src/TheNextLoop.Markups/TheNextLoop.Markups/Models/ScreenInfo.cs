using System;
using TheNextLoop.Markups.Enums;

namespace TheNextLoop.Markups.Models
{
    public class ScreenInfo
    {
        public ScreenInfo(int width, int height, eScreenSizes screenSize)
        {
            this.ScreenSize = screenSize;
            Width = width;
            Height = height;
        }

        public eScreenSizes ScreenSize { get; set; }
        public int Width { get; }
        public int Height { get;  }
    }
}
