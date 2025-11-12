using System;
using System.Collections.Generic;

namespace SpaCenter;

public class Service
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Duration { get; set; }
    public decimal Price { get; set; }
    public int MinimalAge { get; set; }

    public static List<Service> AllServices { get; } = new List<Service>();

    public Service(string name, string description, double duration, decimal price, int minimalAge)
    {
        Name = name;
        Description = description;
        Duration = duration;
        Price = price;
        MinimalAge = minimalAge;

        AllServices.Add(this);
    }

    public static List<Service> ViewServices()
    {
        return AllServices;
    }
}