using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarDetectionEnd
{
    public enum Color { white, blue, red }
    public class Drone
    {
        public int degree { get; set; }
        public int distance { get; set; }
        public Color color { get; set; } = Color.white;
        public Drone(int deg, int dis)
        {
            degree = deg;
            distance = dis;
            color = Color.white;
        }
    }
}

