using System;
using System.Collections.Generic;

[Serializable]
public class Service
{
    //Service Container
    private static List<Service> services_List = new List<Service>();
    public static IReadOnlyList<Service> Services => services_List.AsReadOnly();
    private string _name;
    private string _description;
    private TimeSpan _duration;
    private decimal _price;
    private int _minimalAge;

    public string Name
    {
        get => _name;
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Name can't be empty");
            }
            _name = value;
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Description can't be empty");
            }
            _description = value;
        }
    }

    public TimeSpan Duration
    {
        get => _duration;
        set
        {
            if (value <= TimeSpan.Zero)
            {
                throw new ArgumentException("Duration can't be zero");
            }
            _duration = value;
        }
    }

    public decimal Price
    {
        get => _price;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("Price can't be zero or negative");
            }
            _price = value;
        }
    }

    public int MinimalAge
    {
        get => _minimalAge;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("Age can't be zero or negative");
            }
            _minimalAge = value;
        }
    }

    public Service(string name, string description, TimeSpan duration, decimal price, int minimalAge)
    {
        Name = name;
        Description = description;
        Duration = duration;
        Price = price;
        MinimalAge = minimalAge;

        addService(this);
    }
    
    private static void addService(Service service)
    {
        if (service == null)
        {
            throw new ArgumentException("Service cannot be null");
        }
        services_List.Add(service);
    }
    
    public static void LoadExtent(IEnumerable<Service>? list)
    {
        services_List.Clear();

        if (list == null) return;

        services_List.AddRange(list);
    }
    
    
    
    
    
    
    

    /*public static List<Service> ViewServices()
    {
        return AllServices;
    }*/
}