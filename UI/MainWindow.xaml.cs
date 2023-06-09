using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Drawing.Image;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data;
using System.Security.AccessControl;


namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int Rows = 100;//768;
        private const int Columns = 200;//1024;
        private const int MaxValue = 255;

        private RadarDetectionEnd.GlobalVariables globalVars = new RadarDetectionEnd.GlobalVariables();

        public MainWindow()
        {
            InitializeComponent();
            /*
            int a = RadarDetectionEnd.GlobalVariables.brightness_threshold;
            int numDrones = 20;
            int maxDistance = 20000; // Maximum distance from the center of the radar (in meters)
            int maxSpeed = 10;
            List<RadarDetectionEnd.Drone> droneCoordinates = GenerateDroneCoordinates(numDrones, maxDistance);
            CoordToPicConverter converter = new CoordToPicConverter();

            for (int i = 0; i < 10; i++) 
            {
                //InitializeComponent();
                droneCoordinates = UpdateDroneCoordinates(droneCoordinates);
                List<RadarDetectionEnd.Pixel> coordPics = converter.Convert(droneCoordinates);

                // Plot radar coordinates using coordPics
                // Implement your plot_radar_coordinates method here
                RadarDetectionEnd.Processor pro = new RadarDetectionEnd.Processor();
                List<RadarDetectionEnd.Drone> colored_drone = pro.radar_and_picture(droneCoordinates, coordPics);

                EnterDrone(droneCoordinates, colored_drone);

                System.Threading.Thread.Sleep(1000);
            }

            ///MyTrying();*/
        }





       




        private void AddPointAtPolarCoordinates1(RadarDetectionEnd.Drone d)
        {
            int centerX = Convert.ToInt16(xAxis.X1);
            int centerY = Convert.ToInt16(yAxis.Y1);

            int radians = Convert.ToInt32(d.degree * Math.PI / 180);
            int x = Convert.ToInt32(centerX + d.distance * Math.Cos(radians));
            int y = Convert.ToInt32(centerY - d.distance * Math.Sin(radians)); // Negative sign for Y-coordinate due to flipped coordinate system

            Ellipse point = new Ellipse
            {
                Width = 8,
                Height = 8,
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Margin = new Thickness(x - 3, y - 3, 0, 0) // Adjust position based on size of ellipse
            };



            coordinateSystem1.Children.Add(point);
        }

        private void AddPointAtPolarCoordinates2(RadarDetectionEnd.Drone drone)
        {
            int centerX = Convert.ToInt16(xAxis2.X1);
            int centerY = Convert.ToInt16(yAxis2.Y1);

            int radians = Convert.ToInt16(drone.degree * Math.PI / 180);
            int x = Convert.ToInt16(centerX + drone.distance * Math.Cos(radians));
            int y = Convert.ToInt16(centerY - drone.distance * Math.Sin(radians)); // Negative sign for Y-coordinate due to flipped coordinate system
            Ellipse point = null;
            switch (drone.color)
            {
                case RadarDetectionEnd.Color.white:
                    point = new Ellipse
                    {
                        Width = 8,
                        Height = 8,
                        Fill = Brushes.White,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Margin = new Thickness(x - 3, y - 3, 0, 0) // Adjust position based on size of ellipse
                    };
                    break;
                case RadarDetectionEnd.Color.blue:
                    point = new Ellipse
                    {
                        Width = 8,
                        Height = 8,
                        Fill = Brushes.Aqua,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Margin = new Thickness(x - 3, y - 3, 0, 0) // Adjust position based on size of ellipse
                    };

                    break;
                case RadarDetectionEnd.Color.red:
                    point = new Ellipse
                    {
                        Width = 8,
                        Height = 8,
                        Fill = Brushes.Red,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Margin = new Thickness(x - 3, y - 3, 0, 0) // Adjust position based on size of ellipse
                    };

                    break;
                default:
                    point = new Ellipse
                    {
                        Width = 8,
                        Height = 8,
                        Fill = Brushes.White,
                        Stroke = Brushes.Gray,
                        StrokeThickness = 1,
                        Margin = new Thickness(x - 3, y - 3, 0, 0) // Adjust position based on size of ellipse
                    };

                    break;
            }


            coordinateSystem2.Children.Add(point);
        }


        public void EnterDrone(List<RadarDetectionEnd.Drone> drones_unRecognize, List<RadarDetectionEnd.Drone> drones_recognize)
        {
            foreach (RadarDetectionEnd.Drone drone in drones_unRecognize)
            {
                AddDrone1(drone);
            }

            foreach (RadarDetectionEnd.Drone drone in drones_recognize)
            {
                AddDrone2(drone);
            }
        }


        public void AddDrone1(RadarDetectionEnd.Drone drone)
        {
            int d = drone.distance;
            d = (d * 300) / RadarDetectionEnd.GlobalVariables.radar_radius;
            RadarDetectionEnd.Drone dr = new RadarDetectionEnd.Drone(drone.degree, d);
            AddPointAtPolarCoordinates1(dr);

        }
        public void AddDrone2(RadarDetectionEnd.Drone drone)
        {
            int d = drone.distance;
            d = (d * 300) / RadarDetectionEnd.GlobalVariables.radar_radius;
            drone.distance = d;
            AddPointAtPolarCoordinates2(drone);
        }

        private Random random = new Random();
        private int maxDistance;
        private int maxSpeed;

        public List<RadarDetectionEnd.Drone> GenerateDroneCoordinates(int numDrones, int maxDistance)
        {
            int[] angles = Enumerable.Range(0, numDrones)
                                     .Select(_ => random.Next(-180, 180))
                                     .ToArray();

            int[] distances = Enumerable.Range(0, numDrones)
                                        .Select(_ => random.Next(0, maxDistance))
                                        .ToArray();
            var droneList = new List<RadarDetectionEnd.Drone>();
            for (int i = 0; i < numDrones; i++)
            {
                RadarDetectionEnd.Drone drone = new RadarDetectionEnd.Drone(angles[i], distances[i]);
                droneList.Add(drone);
            }

            return droneList;
        }

        public List<RadarDetectionEnd.Drone> UpdateDroneCoordinates(List<RadarDetectionEnd.Drone> droneCoordinates)
        {
            List<RadarDetectionEnd.Drone> updatedCoordinates = new List<RadarDetectionEnd.Drone>();

            for (int i = 0; i < droneCoordinates.Count; i++)
            {
                int angle = droneCoordinates[i].degree;
                int distance = droneCoordinates[i].distance;

                bool clockwise = random.Next(0, 2) == 1;

                int sign = 1;
                if (angle != 0)
                {
                    sign = angle / Math.Abs(angle);
                }

                if (clockwise)
                {
                    angle = sign * (Math.Abs(angle + random.Next(1, 5)) % 180); // Move the drone clockwise by 1-5 degrees
                }
                else
                {
                    angle = sign * (Math.Abs(angle - random.Next(1, 5)) % 180); // Move the drone counterclockwise by 1-5 degrees
                }

                if (distance >= maxDistance || distance <= 0)
                {
                    clockwise = !clockwise; // Change movement direction when drone reaches max or min distance
                }

                clockwise = random.Next(0, 2) == 1; // Randomly move inward or outward

                if (clockwise)
                {
                    distance = Math.Min(distance + random.Next(0, maxSpeed), maxDistance); // Move the drone inward
                }
                else
                {
                    distance = Math.Max(distance - random.Next(0, maxSpeed), 0); // Move the drone outward
                }

                updatedCoordinates.Add(new RadarDetectionEnd.Drone(angle, distance));
            }

            return updatedCoordinates;
        }

    }
    public class CoordToPicConverter
    {
        private Random random = new Random();
        private int rangeFromRadar = 90;
        private int biasBright = 121;
        private int maxDistance = 20000;

        public List<RadarDetectionEnd.Pixel> Convert(List<RadarDetectionEnd.Drone> droneCoordinates)
        {
            int x = 0; // for the angle
            int y = 0; // for the height and is random
            int b = 0; // brightness above 121
            RadarDetectionEnd.Pixel pixel = new RadarDetectionEnd.Pixel(x, y, b);
            List<RadarDetectionEnd.Pixel> imCoordinates = new List<RadarDetectionEnd.Pixel>();

            foreach (RadarDetectionEnd.Drone d in droneCoordinates)
            {
                int angle = d.degree;
                int range = d.distance;

                if (angle <= rangeFromRadar / 2 && angle >= -rangeFromRadar / 2)
                {
                    x = 512 + (angle * 1024) / rangeFromRadar;
                    y = random.Next(0, 768);
                    b = biasBright + (255 - biasBright) * ((maxDistance - range) / maxDistance);

                    imCoordinates.Add(new RadarDetectionEnd.Pixel(x, y, b));
                }
            }

            return imCoordinates;
        }
    }


}