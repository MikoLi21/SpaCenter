using System;

namespace SpaCenter;

[Serializable]
public class Customer : ICustomer
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
    
    private HashSet<Booking> _listOfBookings = new HashSet<Booking>();
    public IEnumerable<Booking> ListOfBookings => _listOfBookings.ToHashSet();
    
    //Overlapping starts (Person -> Customer)
    public Person Prsn{ get; }
    internal Customer(Person person, DateTime dateOfBirth)
    {
        Prsn = person ?? throw new ArgumentNullException(nameof(person));
        DateOfBirth = dateOfBirth;
        addCustomer(this);
    }
    //Overlapping ends (Person -> Customer)
    
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
    
    internal void AddBookingReverse(Booking booking)
    {
        _listOfBookings.Add(booking);
    }

    internal void RemoveBookingReverse(Booking booking)
    {
        _listOfBookings.Remove(booking);
    }
    
    // Factory method that serves as the constructor for Booking
    public Booking BookService(PaymentMethod paymentMethod, DateTime date, Service service, Employee employee)
    {
        return new Booking(this, service,employee, date, paymentMethod);
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
    
    /*public static Customer Register(string name, string surname, string email, string phoneNumber, DateTime dateOfBirth)
    {
        return new Customer(name, surname, email, phoneNumber, dateOfBirth);
    }*/

    public void Login()
    {
    }
}