namespace SpaCenter;

public interface ICustomer
{
    Person Prsn{ get; }
    DateTime DateOfBirth { get; set; }
    int Age { get; }
    
    IEnumerable<Booking> ListOfBookings { get; }
    
    Booking BookService(
        PaymentMethod paymentMethod,
        DateTime date,
        Service service,
        Employee employee
    );

    void Login();
}