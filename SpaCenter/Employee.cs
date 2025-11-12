namespace SpaCenter;

public abstract class Employee : Person
{
    public string Pesel { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? LeaveDate { get; set; }

    // Computed Properties
    public int YearsOfExperience => CalculateYearsOfExperience();
    public int YearsOfService => CalculateYearsOfService();
    public double AverageServiceMinutes { get; set; }

    protected Employee(string name, string surname, string email, string phoneNumber, string pesel, DateTime hireDate)
        : base(name, surname, email, phoneNumber)
    {
        Pesel = pesel;
        HireDate = hireDate;
    }

    private int CalculateYearsOfExperience()
    {
        var endDate = LeaveDate ?? DateTime.Now;
        int years = endDate.Year - HireDate.Year;
        if (endDate < HireDate.AddYears(years)) years--;
        return years;
    }

    private int CalculateYearsOfService()
    {
        // Assuming service years = same as experience years
        return CalculateYearsOfExperience();
    }

    public virtual void Promote()
    {
        Console.WriteLine($"{Name} {Surname} has been promoted!");
    }

    public virtual void CheckYearsOfService()
    {
        Console.WriteLine($"{Name} {Surname} has {YearsOfService} years of service.");
    }
}