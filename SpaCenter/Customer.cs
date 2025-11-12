

namespace SpaCenter;

public class Customer : Person
{
    public DateTime DateOfBirth { get; set; }
    public bool IsLoggedIn { get; set; }

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            int age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

    public Customer(string name, string surname, string email, string phoneNumber, DateTime dateOfBirth)
        : base(name, surname, email, phoneNumber)
    {
        DateOfBirth = dateOfBirth;
        IsLoggedIn = false;
    }

    public static Customer Register(string name, string surname, string email, string phoneNumber, DateTime dateOfBirth)
    {
        return new Customer(name, surname, email, phoneNumber, dateOfBirth);
    }

    public void Login()
    {
        IsLoggedIn = true;
    }
}