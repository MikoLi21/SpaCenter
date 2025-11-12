namespace SpaCenter;

public class Customer : Person
{
    public DateTime DateOfBirth { get; set; }

    public Customer(string name, string surname, string email, string phoneNumber, DateTime dateOfBirth)
        : base(name, surname, email, phoneNumber)
    {
        DateOfBirth = dateOfBirth;
    }

    // Static Register method
    public static Customer Register(string name, string surname, string email, string phoneNumber, DateTime dateOfBirth)
    {
        Console.WriteLine($"Registering new customer: {name} {surname}");
        return new Customer(name, surname, email, phoneNumber, dateOfBirth);
    }

    public void Login()
    {
        Console.WriteLine($"{Name} {Surname} has logged in.");

    }
}