using System;
using System.Collections.Generic;

namespace SpaCenter;

public abstract class Employee : Person
{
    public static List<Employee> AllEmployees { get; } = new List<Employee>();
    public string Pesel { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? LeaveDate { get; set; }

    public int YearsOfExperience { get; set; }
    public int YearsOfService => CalculateYearsOfService();
    public double AverageServiceMinutes => CalculateAverageServiceMinutes();

    protected Employee(string name, string surname, string email, string phoneNumber, string pesel, DateTime hireDate)
        : base(name, surname, email, phoneNumber)
    {
        Pesel = pesel;
        HireDate = hireDate;
        
        AllEmployees.Add(this);
    }

    public void Promote()
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
    
    private int CalculateYearsOfService()
    {
        return DateTime.Now.Year - HireDate.Year;
    }
    
    public double CalculateAverageServiceMinutes()
    {
        return 0;
    }
}