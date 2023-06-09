using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarDetectionEnd
{
    public class Pixel
    {
        public int x { get; set; }
        public int y { get; set; }
        public int bright { get; set; }
        public Pixel(int x_, int y_, int b)
        {
            x = x_;
            y = y_;
            bright = b;
        }
    }
}
