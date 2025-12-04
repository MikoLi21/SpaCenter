using System;
using System.Collections.Generic;
using SpaCenter;

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

    private Service? _nextService;
    private Service? _previousService;
    
    //reflex association 
    private readonly List<Service> _subServices = new List<Service>();
    public IReadOnlyList<Service> SubServices => _subServices.AsReadOnly();
    //reverse connection
    private HashSet<Employee> _providedBy = new HashSet<Employee>();
    public IEnumerable<Employee> ProvidedBy => _providedBy.ToHashSet();
    
    public void AddEmployeeServiceProvidedBy(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        if (_providedBy.Contains(employee))
            return; 

        _providedBy.Add(employee);
        employee.AddServiceToEmployeeReverse(this);  
    }
    public void RevomeEmployeeServiceProvidedBy(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        if (!_providedBy.Contains(employee))
            return;

        _providedBy.Remove(employee);
        employee.RemoveServiceFromEmployeeReverse(this);
    }
    internal void AddEmployeeReverse(Employee employee)
    {
        _providedBy.Add(employee);
    }

   
    internal void RevomeEmployeeReverse(Employee employee)
    {
        _providedBy.Remove(employee);
    }
    
    public void AddSubService(Service subService)
    {
        if (subService == null)
            throw new ArgumentNullException(nameof(subService), "Sub-service cannot be null");

        if (ReferenceEquals(this, subService))
            throw new ArgumentException("Service cannot be part of itself");

        if (_subServices.Contains(subService))
            return; 

        _subServices.Add(subService);
    }

   
    public void RemoveSubService(Service subService)
    {
        if (subService == null)
            throw new ArgumentNullException(nameof(subService), "Sub-service cannot be null");

        _subServices.Remove(subService);
    }

      
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
    
    private HashSet<Booking> _listOfBookings = new HashSet<Booking>();
    public IEnumerable<Booking> ListOfBookings => _listOfBookings.ToHashSet();

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
    
    internal void AddBookingReverse(Booking booking)
    {
        _listOfBookings.Add(booking);
    }

    internal void RemoveBookingReverse(Booking booking)
    {
        _listOfBookings.Remove(booking);
    }
    
    
    
    
    
    
    

    /*public static List<Service> ViewServices()
    {
        return AllServices;
    }*/
}