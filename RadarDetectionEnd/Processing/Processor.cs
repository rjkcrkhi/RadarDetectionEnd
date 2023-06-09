using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarDetectionEnd
{
    public class Processor
    {
        public Processor()
        {
        }

        public List<Drone> radar_and_picture(List<Drone> drones, List<Pixel> pixels)
        {

            List<Pixel> highIntensityPixels = new List<Pixel>();
            foreach (Pixel pixel in pixels)
            {
                if (pixel.bright > GlobalVariables.brightness_threshold)
                {
                    highIntensityPixels.Add(pixel);
                }
            }

            List<Drone> coloredDrones = new List<Drone>();
            foreach (Drone drone in drones)
            {
                bool foundMatch = false;
                var x_drone = drone.degree / (GlobalVariables.image_degree / GlobalVariables.image_size_x) + GlobalVariables.image_size_x / 2;
                foreach (Pixel pixel in highIntensityPixels)
                {
                    if (Math.Abs(x_drone - pixel.x) <= GlobalVariables.degree_error_threshold)
                    {
                        drone.color = Color.blue;
                        foundMatch = true;
                        break;
                    }
                }

                if (!foundMatch)
                {
                    drone.color = Color.red;
                }
                coloredDrones.Add(drone);
            }
            Console.Write(coloredDrones.ToArray());
            return coloredDrones;
        }
    }
}
