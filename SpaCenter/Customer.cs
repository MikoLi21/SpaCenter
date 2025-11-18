using System;

namespace SpaCenter;

[Serializable]
public class Customer : Person
{
    //Customer Container
    private static List<Customer> customers_List = new List<Customer>();
    public static IReadOnlyList<Customer> Customers => customers_List.AsReadOnly();
    
    private DateTime _dateOfBirth;
    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (value > DateTime.Today)
            {
                throw new ArgumentException("Birth date can't be in the future");
            }
            _dateOfBirth = value;
        }
    }   
    public Customer(string name, string surname, string email, string phoneNumber, DateTime dateOfBirth)
        : base(name, surname, email, phoneNumber)
    {
        DateOfBirth = dateOfBirth;
        
        addCustomer(this);
    }
    
    private static void addCustomer(Customer customer)
    {
        if (customer == null)
        {
            throw new ArgumentException("Customer cannot be null");
        }
        customers_List.Add(customer);
    }
    
    public static void LoadExtent(IEnumerable<Customer>? list)
    {
        customers_List.Clear();

        if (list == null) return;

        customers_List.AddRange(list);
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
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
    
    public static Customer Register(string name, string surname, string email, string phoneNumber, DateTime dateOfBirth)
    {
        return new Customer(name, surname, email, phoneNumber, dateOfBirth);
    }

    public void Login()
    {
    }
}