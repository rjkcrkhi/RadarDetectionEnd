﻿using RadarDetectionEnd;
using System;
using System.Collections.Generic;
using System.Linq;

public class CoordToPicConverter
{
    private Random random = new Random();
    private int rangeFromRadar = 90;
    private int biasBright = 121;
    private int maxDistance = 20000;

    public List<Pixel> Convert(List<Drone> droneCoordinates)
    {
        int x = 0; // for the angle
        int y = 0; // for the height and is random
        int b = 0; // brightness above 121
        Pixel pixel = new Pixel(x, y, b);
        List<Pixel> imCoordinates = new List<Pixel>();

        foreach (Drone d in droneCoordinates)
        {
            int angle = d.degree;
            int range = d.distance;

            if (angle <= rangeFromRadar / 2 && angle >= -rangeFromRadar / 2)
            {
                x = 512 + (angle * 1024) / rangeFromRadar;
                y = random.Next(0, 768);
                b = biasBright + (255 - biasBright) * ((maxDistance - range) / maxDistance);

                imCoordinates.Add(new Pixel( x, y, b ));
            }
        }

        return imCoordinates;
    }
}
public class DroneSimulator
{
    private Random random = new Random();
    private int maxDistance;
    private int maxSpeed;

    public List<Drone> GenerateDroneCoordinates(int numDrones, int maxDistance)
    {
        int[] angles = Enumerable.Range(0, numDrones)
                                 .Select(_ => random.Next(-180, 180))
                                 .ToArray();

        int[] distances = Enumerable.Range(0, numDrones)
                                    .Select(_ => random.Next(0, maxDistance))
                                    .ToArray();
        var droneList = new List<Drone>();
        for (int i = 0; i < numDrones; i++)
        {
            Drone drone = new Drone(angles[i], distances[i]);
            droneList.Add(drone);
        }

        return droneList;
    }

    public List<Drone> UpdateDroneCoordinates(List<Drone> droneCoordinates)
    {
        List<Drone> updatedCoordinates = new List<Drone>();

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

            updatedCoordinates.Add(new Drone(angle, distance));
        }

        return updatedCoordinates;
    }

    public void Simulate(int numDrones, int maxDistance, int maxSpeed)
    {
        this.maxDistance = maxDistance;
        this.maxSpeed = maxSpeed;

        List<Drone> droneCoordinates = GenerateDroneCoordinates(numDrones, maxDistance);
        CoordToPicConverter converter = new CoordToPicConverter();

        while (true)
        {
            droneCoordinates = UpdateDroneCoordinates(droneCoordinates);
            List<Pixel> coordPics = converter.Convert(droneCoordinates);

            // Plot radar coordinates using coordPics
            // Implement your plot_radar_coordinates method here

            System.Threading.Thread.Sleep(1000); // Wait for 1 second before updating the drone positions
        }
    }
}

public class Program
{
    public static void Main()
    {
        int numDrones = 20; // Number of drones
        int maxDistance = 20000; // Maximum distance from the center of the radar (in meters)
        int maxSpeed = 10; // Maximum speed of drones (in meters per second)

        DroneSimulator simulator = new DroneSimulator();
        simulator.Simulate(numDrones, maxDistance, maxSpeed);
    }
}