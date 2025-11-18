
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
    private double _yearsOfExperience;

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

    public double YearsOfExperience
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
    
    private List<Booking> Bookings { get; set; } = new List<Booking>();
    
    public Employee(string name, string surname, string email, string phoneNumber, long pesel, DateTime hireDate,
        double yearsOfExperience)
        : base(name, surname, email, phoneNumber)
    {
        Pesel = pesel;
        HireDate = hireDate;
        YearsOfExperience = yearsOfExperience;

        addEmployee(this);
    }

    [JsonConstructor]
    public Employee(string name, string surname, string email, string phoneNumber, long pesel, DateTime hireDate,
        double yearsOfExperience, DateTime? leaveDate = null)
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
        if (Bookings == null || Bookings.Count == 0)
            return 0;

        // Sum all service durations
        double totalMinutes = Bookings
            .Where(b => b.Service != null)
            .Sum(b => b.Service.Duration.TotalMinutes);

        int count = Bookings.Count(b => b.Service != null);

        return count == 0 ? 0 : totalMinutes / count;
    }
    
    public static void LoadExtent(IEnumerable<Employee>? list)
    {
        employees_List.Clear();

        if (list == null) return;

        employees_List.AddRange(list);
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