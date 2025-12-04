
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SpaCenter;

[Serializable]
public class Employee : Person
{
    //Employee Container
    private static List<Employee> employees_List = new List<Employee>();
    public static IReadOnlyList<Employee> Employees => employees_List.AsReadOnly();
    
    private long _pesel;
    private DateTime _hireDate;
    private DateTime? _leaveDate;
    private decimal _yearsOfExperience;

    public long Pesel
    {
        get => _pesel;
        set
        {
            if (value <= 0 || value.ToString().Length != 11)
            {
                throw new ArgumentException("Invalid pesel number");
            }
            _pesel = value;
        }
    }

    public DateTime HireDate
    {
        get => _hireDate;
        set
        {
            if (value > DateTime.Today)
            {
                throw new ArgumentException("Hire date can't be in the future");
            }
            _hireDate = value;
        }
    }

    public DateTime? LeaveDate
    {
        get => _leaveDate;
        set
        {
            if (value != null)
            {
                if (value <= HireDate)
                {
                    throw new ArgumentException("Leave date can't be before hire date");
                }

                if (value > DateTime.Today)
                {
                    throw new ArgumentException("Leave date can't be in the future");
                }

                _leaveDate = value;
            }
        }
    }

    public decimal YearsOfExperience
    {
        get => _yearsOfExperience;
        set
        {
            if (value < 0 || value > 40)
            {
                throw new ArgumentException("Years of experience should be in the range of 0 to 40");
            }
            _yearsOfExperience = value;
        }
    }
    public int YearsOfService => CalculateYearsOfService();
    public double AverageServiceMinutes => CalculateAverageServiceMinute();
    
    //private List<Booking> Bookings { get; set; } = new List<Booking>();
    
    private HashSet<Booking> _assignedTo = new HashSet<Booking>();

    public IEnumerable<Booking> AssignedTo => _assignedTo.ToHashSet();
    
    public Employee(string name, string surname, string email, string phoneNumber, long pesel, DateTime hireDate,
        decimal yearsOfExperience)
        : base(name, surname, email, phoneNumber)
    {
        Pesel = pesel;
        HireDate = hireDate;
        YearsOfExperience = yearsOfExperience;

        addEmployee(this);
    }

    [JsonConstructor]
    public Employee(string name, string surname, string email, string phoneNumber, long pesel, DateTime hireDate,
        decimal yearsOfExperience, DateTime? leaveDate = null)
        : base(name, surname, email, phoneNumber)
    {
        Pesel = pesel;
        HireDate = hireDate;
        LeaveDate = leaveDate;
        YearsOfExperience = yearsOfExperience;
        
        addEmployee(this);
    }

    private static void addEmployee(Employee employee)
    {
        if (employee == null)
        {
            throw new ArgumentException("Employee cannot be null");
        }
        employees_List.Add(employee);
    }

    private int CalculateYearsOfService()
    {
        return DateTime.Now.Year - HireDate.Year;
    }

    private double CalculateAverageServiceMinute()
    {
        if (AssignedTo == null || AssignedTo.Count() == 0)
            return 0;

        // Sum all service durations
        double totalMinutes = AssignedTo
            .Where(b => b.Service != null)
            .Sum(b => b.Service.Duration.TotalMinutes);

        int count = AssignedTo.Count(b => b.Service != null);

        return count == 0 ? 0 : totalMinutes / count;
    }
    
    public static void LoadExtent(IEnumerable<Employee>? list)
    {
        employees_List.Clear();

        if (list == null) return;

        employees_List.AddRange(list);
    }
    
    public void AddBookingEmployeeAssignedTo(Booking booking)
    {
        if (booking == null)
            throw new ArgumentNullException(nameof(booking));

        if (_assignedTo.Contains(booking))
            return; // already connected

        _assignedTo.Add(booking);
        booking.SetEmployeeReverse(this);  // reverse connection
    }
    
    public void RemoveBookingEmployeeAssignedTo(Booking booking)
    {
        if (booking == null)
            throw new ArgumentNullException(nameof(booking));

        if (!_assignedTo.Contains(booking))
            return;

        _assignedTo.Remove(booking);
        booking.RemoveEmployee(); // reverse removal
    }
    
    internal void AddBookingReverse(Booking booking)
    {
        _assignedTo.Add(booking);
    }
    
    internal void RemoveBookingReverse(Booking booking)
    {
        _assignedTo.Remove(booking);
    }

    
    
    
    
    
    
    
    
    
    
    
    
    /*public  void Promote()
    {
        return;
    }

    public static void CheckYearsOfService()
    {

        foreach (var emp in AllEmployees)
        {
            if (emp.YearsOfService > 0 && emp.YearsOfService % 2 == 0)
            {
                emp.Promote();
            }
        }
    }*/
}