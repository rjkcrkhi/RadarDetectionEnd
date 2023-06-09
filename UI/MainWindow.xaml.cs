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
            int a = globalVars.brightness_threshold;

            MyTrying();

        }

        private void MyTrying()
        {
            Random random = new Random();
            //random.Next(256);

            List<RadarDetectionEnd.Drone> drones1 = new List<RadarDetectionEnd.Drone>();
            List<RadarDetectionEnd.Drone> drones2 = new List<RadarDetectionEnd.Drone>();
            for (int i = 1; i < 10; i++)
            {
                int zavit = random.Next(-45, 46);
                int dis = random.Next(globalVars.radar_radius);
                Drone drone = new Drone(zavit, dis);//UInt32.Parse((zavit).ToString()), UInt32.Parse((dis).ToString()));
                drones1.Add(drone);

                if (i % 2 == 0)
                    drone.color = ColorRadar.blue;
                else
                    drone.color = ColorRadar.red;
                drones2.Add(drone);

            }

            for (int i = 1; i < 10; i++)
            {
                int zavit = random.Next(360);
                int dis = random.Next(globalVars.radar_radius);
                Drone drone = new Drone(zavit, dis);//UInt32.Parse((zavit).ToString()), UInt32.Parse((dis).ToString()));
                drones1.Add(drone);
                drone.color = ColorRadar.red;
                drones2.Add(drone);


            }
            EnterDrone(drones1, drones2);


            CreateMatrix();

        }




        private void AddPointAtPolarCoordinates1(double angle, double radius)
        {
            double centerX = xAxis.X1;
            double centerY = yAxis.Y1;

            double radians = angle * Math.PI / 180;
            double x = centerX + radius * Math.Cos(radians);
            double y = centerY - radius * Math.Sin(radians); // Negative sign for Y-coordinate due to flipped coordinate system

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

        private void AddPointAtPolarCoordinates2(double angle, double radius, ColorRadar color)
        {
            double centerX = xAxis2.X1;
            double centerY = yAxis2.Y1;

            double radians = angle * Math.PI / 180;
            double x = centerX + radius * Math.Cos(radians);
            double y = centerY - radius * Math.Sin(radians); // Negative sign for Y-coordinate due to flipped coordinate system
            Ellipse point = null;
            switch (color)
            {
                case ColorRadar.white:
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
                case ColorRadar.blue:
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
                case ColorRadar.red:
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


        public void EnterDrone(List<Drone> drones_unRecognize, List<Drone> drones_recognize)
        {
            foreach (Drone drone in drones_unRecognize)
            {
                AddDrone1(drone);
            }

            foreach (Drone drone in drones_recognize)
            {
                AddDrone2(drone);
            }
        }


        public void AddDrone1(Drone drone)
        {
            double d = drone.distance;
            d = (d * globalVars.sizeCoridinatesPlot) / globalVars.radar_radius;
            AddPointAtPolarCoordinates1(drone.degree, d);

        }
        public void AddDrone2(Drone drone)
        {
            double d = drone.distance;
            d = (d * globalVars.sizeCoridinatesPlot) / globalVars.radar_radius;
            AddPointAtPolarCoordinates2(drone.degree, d, drone.color);
        }



        public void CreateImage(List<int> numberList, int width, int height, string filePath)
        {

            WriteableBitmap image = new WriteableBitmap(width, height, 96, 96, PixelFormats.Gray8, null);

            byte[] pixels = new byte[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    int number = numberList[index];

                    byte grayValue = (byte)number;

                    int pixelOffset = y * width + x;
                    pixels[pixelOffset] = grayValue;
                }
            }

            Int32Rect rect = new Int32Rect(0, 0, width, height);
            int stride = width;
            image.WritePixels(rect, pixels, stride, 0);

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }

        }



        private void CreateMatrix()
        {

            MatrixGrid.Children.Clear();
            MatrixGrid.RowDefinitions.Clear();
            MatrixGrid.ColumnDefinitions.Clear();

            Random random = new Random();
            int index1 = random.Next(Rows * Columns);
            int index2 = random.Next(Rows * Columns);
            int index3 = random.Next(Rows * Columns);

            for (int i = 0; i < Rows; i++)
            {
                MatrixGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                for (int j = 0; j < Columns; j++)
                {
                    MatrixGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                    int brash = 0;

                    if ((index1 - 10 <= i * j && i * j <= index1 + 10) ||
                        (index2 - 10 <= i * j && i * j <= index2 + 10) ||
                        (index3 - 10 <= i * j && i * j <= index3 + 10))
                    { brash = 255; }
                    Border square = new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1),
                        Background = GetColorBrush(brash)// * (MaxValue / (Rows * Columns)))

                    };

                    Grid.SetRow(square, i);
                    Grid.SetColumn(square, j);

                    MatrixGrid.Children.Add(square);
                }
            }

        }

        private SolidColorBrush GetColorBrush(int value)
        {
            byte intensity = (byte)value;// (byte)(255 - (value * 255 / MaxValue));
            return new SolidColorBrush(Color.FromRgb(intensity, intensity, intensity));
        }

    }
