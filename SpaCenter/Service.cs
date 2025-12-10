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
    private readonly List<Service> _serviceTo = new List<Service>(); 
    private readonly List<Service> _serviceOf = new List<Service>();
    public IReadOnlyList<Service> ServiceTo => _serviceTo.AsReadOnly();
    public IReadOnlyList<Service> ServiceOf => _serviceOf.AsReadOnly();
    //Employee provides > Service association (Basic)
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
    
    //Reflexive association
    public void AddServiceTo(Service service)
    {
        if (service == null)
            throw new ArgumentNullException(nameof(service));

        if (service == this)
            throw new InvalidOperationException("A service can't be a service to itself");

        if (_serviceTo.Contains(service))
            return; // already linked

        _serviceTo.Add(service);

        // Add reverse association
        if (!service._serviceOf.Contains(this))
            service._serviceOf.Add(this);
    }
    
    public void RemoveServiceTo(Service service)
    {
        if (service== null) return;

        if (_serviceTo.Remove(service))
        {
            // Remove reverse association
            service._serviceOf.Remove(this);
        }
    }
    
    public void AddServiceOf(Service service)
    {
        if (service == null)
            throw new ArgumentNullException(nameof(service));

        if (service == this)
            throw new InvalidOperationException("A service can't consist of itself");

        if (_serviceOf.Contains(service))
            return;

        _serviceOf.Add(service);

        // Add reverse association
        if (!service._serviceTo.Contains(this))
            service._serviceTo.Add(this);
    }
    
    public void RemoveServiceOf(Service service)
    {
        if (service == null) return;

        if (_serviceOf.Remove(service))
        {
            service._serviceTo.Remove(this);
        }
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