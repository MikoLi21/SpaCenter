using SpaCenter;
using NUnit.Framework;
using SpaCenter.Repository;

namespace SpaCenterTest;

public class PersistenceTest
{
    [Test]
    public void SaveAndLoad_Success()
    {
        Customer.LoadExtent(new List<Customer>());
        Employee.LoadExtent(new List<Employee>());
        Service.LoadExtent(new List<Service>());
        Booking.LoadExtent(new List<Booking>());
        Branch.LoadExtent(new List<Branch>());
        Room.LoadExtent(new List<Room>());

        if (File.Exists(PersistenceManager.FilePath))
            File.Delete(PersistenceManager.FilePath);

        var c1 = new Customer("Anna", "Brown", "annabrown@gmail.com", "+48111222333",
            new DateTime(1999, 01, 01));

        var e1 = new Employee("Bob", "Smith", "bobsmith666@gmail.com", "+48000555999",
            45678901177, DateTime.Today, 3);

        var e2 = new Employee("John", "Black", "john7876@gmail.com", "+48777111777",
            78903156789, new DateTime(2024, 05, 06), 5.5m, new DateTime(2025, 01, 01));

        var s1 = new Service("Thai Massage", "Message in thai style",
            new TimeSpan(1, 0, 0), 300, 10);

        var bk1 = new Booking(c1, s1, e1,
            new DateTime(2025, 12, 25, 14, 30, 0),
            PaymentMethod.AtTheSPA);

        List<string> phones = new List<string>
        {
            "+48565787999",
            "+48565788888"
        };

        var b1 = new Branch("SPA Wola",
            new Address("ul.Koszykowa", 80, "Warsaw", "04-565", "Poland"),
            phones);

        var r1 = new Room(101, "Massage Room", 23.5, 55.0);

        // Save everything
        PersistenceManager.Save();

        // Clear all extents again before loading
        Customer.LoadExtent(new List<Customer>());
        Employee.LoadExtent(new List<Employee>());
        Service.LoadExtent(new List<Service>());
        Booking.LoadExtent(new List<Booking>());
        Branch.LoadExtent(new List<Branch>());
        Room.LoadExtent(new List<Room>());

        // Load from file
        PersistenceManager.Load();

        // Verify counts
        Assert.That(Customer.Customers, Has.Count.EqualTo(1));
        Assert.That(Employee.Employees, Has.Count.EqualTo(2));
        Assert.That(Service.Services, Has.Count.EqualTo(1));
        Assert.That(Branch.Branches, Has.Count.EqualTo(1));
        Assert.That(Room.Rooms, Has.Count.EqualTo(1));

        // Verify example fields
        Assert.That(Customer.Customers[0].Name, Is.EqualTo("Anna"));
        Assert.That(Employee.Employees[0].Name, Is.EqualTo("Bob"));
        Assert.That(Employee.Employees[1].Name, Is.EqualTo("John"));
        Assert.That(Service.Services[0].Name, Is.EqualTo("Thai Massage"));
        Assert.That(Booking.Bookings[0].Customer.Name, Is.EqualTo("Anna"));
        Assert.That(Branch.Branches[0].Name, Is.EqualTo("SPA Wola"));
        Assert.That(Room.Rooms[0].RoomNumber, Is.EqualTo(101));

        // File exists
        Assert.That(File.Exists(PersistenceManager.FilePath), Is.True);
    }


    [Test]
    public void Load_FileIsNotFound()
    {
        PersistenceManager.FilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");

        var ex = Assert.Throws<FileNotFoundException>(() =>
            PersistenceManager.Load()
        );

        Assert.That(ex.Message, Does.StartWith("File not found"));
    }
}