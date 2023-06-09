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
            int a = RadarDetectionEnd.GlobalVariables.brightness_threshold;
            RadarDetectionEnd.Program start = new RadarDetectionEnd.Program();
            start.Main();
            ///MyTrying();
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
    }
}