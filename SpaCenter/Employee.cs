using System;

namespace SpaCenter;

public abstract class Employee : Person
{
    public static List<Employee> AllEmployees { get; } = new List<Employee>();
    public List<Booking> Bookings { get; set; } = new List<Booking>();
    public string Pesel { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? LeaveDate { get; set; }

    public int YearsOfExperience { get; set; }
    public int YearsOfService => CalculateYearsOfService();
    public double AverageServiceMinutes => CalculateAverageServiceMinute();

    protected Employee(string name, string surname, string email, string phoneNumber, string pesel, DateTime hireDate)
        : base(name, surname, email, phoneNumber)
    {
        Pesel = pesel;
        HireDate = hireDate;
        AllEmployees.Add(this);
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
            .Sum(b => b.Service.Duration);

        int count = Bookings.Count(b => b.Service != null);

        return count == 0 ? 0 : totalMinutes / count;
    }

    public  void Promote()
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
    }
}