using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using SpaCenter;

namespace SpaCenterTest
{
    [TestFixture]
    public class CustomerTests
    {
        private string _name = null!;
        private string _surname = null!;
        private string _email = null!;
        private string _phone = null!;
        private DateTime _dob;

        private readonly List<string> _therapistCertifications = new() { "Massage certificate" };

        [SetUp]
        public void SetUp()
        {
            Customer.LoadExtent(new List<Customer>());
            Booking.LoadExtent(new List<Booking>());
            Employee.LoadExtent(new List<Employee>());
            Service.LoadExtent(new List<Service>());

            _name = "Anna";
            _surname = "Nowak";
            _email = "anna@example.com";
            _phone = "123456789";
            _dob = new DateTime(2000, 1, 1);
        }

        [Test]
        public void Constructor_SetsNameCorrectly()
        {
            var c = new Customer(_name, _surname, _email, _phone, _dob);
            Assert.That(c.Name, Is.EqualTo(_name));
        }

        [Test]
        public void Constructor_SetsSurnameCorrectly()
        {
            var c = new Customer(_name, _surname, _email, _phone, _dob);
            Assert.That(c.Surname, Is.EqualTo(_surname));
        }

        [Test]
        public void Constructor_SetsEmailCorrectly()
        {
            var c = new Customer(_name, _surname, _email, _phone, _dob);
            Assert.That(c.Email, Is.EqualTo(_email));
        }

        [Test]
        public void Constructor_SetsPhoneCorrectly()
        {
            var c = new Customer(_name, _surname, _email, _phone, _dob);
            Assert.That(c.PhoneNumber, Is.EqualTo(_phone));
        }

        [Test]
        public void Constructor_SetsDateOfBirthCorrectly()
        {
            var c = new Customer(_name, _surname, _email, _phone, _dob);
            Assert.That(c.DateOfBirth, Is.EqualTo(_dob));
        }

        [Test]
        public void Constructor_ThrowsException_WhenBirthDateInFuture()
        {
            var future = DateTime.Today.AddDays(1);

            var ex = Assert.Throws<ArgumentException>(() =>
                new Customer(_name, _surname, _email, _phone, future));

            Assert.That(ex!.Message, Is.EqualTo("Birth date can't be in the future"));
        }

        [Test]
        public void BirthDateInFuture()
        {
            var futureBirthDate = DateTime.Today.AddDays(1);

            var ex = Assert.Throws<ArgumentException>(() =>
                new Customer(
                    name: "Anna",
                    surname: "Smith",
                    email: "anna@example.com",
                    phoneNumber: "123456789",
                    dateOfBirth: futureBirthDate
                ));

            Assert.That(ex!.Message, Is.EqualTo("Birth date can't be in the future"));
        }

        [Test]
        public void Extent_Should_Be_ReadOnly()
        {
            var c = new Customer("John", "Smith", "john@test.com", "+48123456789", new DateTime(1990, 1, 1));

            var extent = Customer.Customers;

            Assert.That(extent, Is.AssignableTo<IReadOnlyList<Customer>>());

            Assert.Throws<NotSupportedException>(() =>
            {
                ((IList<Customer>)extent).Add(c);
            });
        }

        [Test]
        public void Modifying_Property_Should_Update_Object_Inside_Extent()
        {
            var c = new Customer(_name, _surname, _email, _phone, _dob);

            c.DateOfBirth = new DateTime(1985, 5, 5);

            var first = Customer.Customers[0];
            Assert.That(first.DateOfBirth, Is.EqualTo(new DateTime(1985, 5, 5)));
        }

        [Test]
        public void BookService_ShouldCreateBookingAndReverseConnections()
        {
            var service = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);
            var customer = new Customer(_name, _surname, _email, _phone, _dob);

            var services = new List<Service> { service };

            var employee = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, services,
                roles: EmployeeRole.Therapist,
                certifications: _therapistCertifications
            );

            var booking = customer.BookService(PaymentMethod.AtTheSPA, DateTime.Today, service, employee);

            Assert.That(
                customer.ListOfBookings,
                Has.One.Matches<Booking>(b =>
                    b.PaymentMethod == booking.PaymentMethod &&
                    b.Date == booking.Date &&
                    b.Service!.Name == booking.Service!.Name &&
                    b.Employee!.Pesel == booking.Employee!.Pesel &&
                    b.Customer!.Name == booking.Customer!.Name &&
                    b.Customer!.Surname == booking.Customer!.Surname
                ));

            Assert.That(service.ListOfBookings, Has.One.Matches<Booking>(b => b == booking));

            Assert.That(booking.Customer, Is.EqualTo(customer));
            Assert.That(booking.Service, Is.EqualTo(service));
            Assert.That(booking.Employee, Is.EqualTo(employee));
        }

        [Test]
        public void BookService_ShouldAllowMultipleBookingForSameService()
        {
            var service = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);
            var customer1 = new Customer(_name, _surname, _email, _phone, _dob);
            var customer2 = new Customer("John", "Smith", "john@test.com", "+48123456789", new DateTime(1990, 1, 1));

            var services = new List<Service> { service };

            var employee = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, services,
                roles: EmployeeRole.Therapist,
                certifications: _therapistCertifications
            );

            var booking1 = customer1.BookService(PaymentMethod.AtTheSPA, DateTime.Today, service, employee);
            var booking2 = customer1.BookService(PaymentMethod.AtTheSPA, new DateTime(2025, 12, 18), service, employee);
            var booking3 = customer2.BookService(PaymentMethod.AtTheSPA, new DateTime(2025, 12, 20), service, employee);

            Assert.That(customer1.ListOfBookings.Count(), Is.EqualTo(2));
            Assert.That(customer2.ListOfBookings.Count(), Is.EqualTo(1));
            Assert.That(service.ListOfBookings.Count(), Is.EqualTo(3));

            Assert.That(customer1.ListOfBookings, Has.Exactly(1).EqualTo(booking1));
            Assert.That(customer1.ListOfBookings, Has.Exactly(1).EqualTo(booking2));
            Assert.That(customer2.ListOfBookings, Has.Exactly(1).EqualTo(booking3));
        }

        [Test]
        public void BookService_ShouldNotAllowMultipleBookingForSameServiceAndEmpAtTheSameTime()
        {
            var service = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);
            var customer1 = new Customer(_name, _surname, _email, _phone, _dob);
            var customer2 = new Customer("John", "Smith", "john@test.com", "+48123456789", new DateTime(1990, 1, 1));

            var services = new List<Service> { service };

            var employee = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, services,
                roles: EmployeeRole.Therapist,
                certifications: _therapistCertifications
            );

            var booking1 = customer1.BookService(
                PaymentMethod.AtTheSPA,
                new DateTime(2025, 12, 18, 14, 30, 0),
                service,
                employee);

            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                _ = customer2.BookService(
                    PaymentMethod.AtTheSPA,
                    new DateTime(2025, 12, 18, 14, 30, 0),
                    service,
                    employee);
            });

            Assert.That(ex!.Message, Is.EqualTo("Employee already has appointment at this time"));
            Assert.That(customer1.ListOfBookings.Count(), Is.EqualTo(1));
            Assert.That(customer2.ListOfBookings.Count(), Is.EqualTo(0));
            Assert.That(customer1.ListOfBookings, Has.Exactly(1).EqualTo(booking1));
        }

        [Test]
        public void RemoveBooking_ShouldRemoveAllReferences()
        {
            var service = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);
            var customer = new Customer(_name, _surname, _email, _phone, _dob);

            var services = new List<Service> { service };

            var employee = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, services,
                roles: EmployeeRole.Therapist,
                certifications: _therapistCertifications
            );

            var booking = customer.BookService(
                PaymentMethod.AtTheSPA,
                new DateTime(2025, 12, 18, 14, 30, 0),
                service,
                employee);

            booking.RemoveBooking();

            Assert.That(customer.ListOfBookings, Is.Empty);
            Assert.That(service.ListOfBookings, Is.Empty);

            Assert.That(booking.Customer, Is.Null);
            Assert.That(booking.Service, Is.Null);
        }

        [Test]
        public void RemoveBooking_ShouldNotAffectOtherBookingsInBag()
        {
            var service = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);
            var customer = new Customer(_name, _surname, _email, _phone, _dob);

            var services = new List<Service> { service };

            var employee = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, services,
                roles: EmployeeRole.Therapist,
                certifications: _therapistCertifications
            );

            var booking1 = customer.BookService(PaymentMethod.AtTheSPA, DateTime.Today, service, employee);
            var booking2 = customer.BookService(PaymentMethod.AtTheSPA, new DateTime(2025, 12, 18), service, employee);

            booking1.RemoveBooking();

            Assert.That(customer.ListOfBookings.Count, Is.EqualTo(1));
            Assert.That(service.ListOfBookings.Count, Is.EqualTo(1));
            Assert.That(customer.ListOfBookings, Has.One.EqualTo(booking2));
        }

        [Test]
        public void ListOfBookings_ShouldReturnCopiesNotReferences()
        {
            var service = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);
            var customer = new Customer(_name, _surname, _email, _phone, _dob);

            var services = new List<Service> { service };

            var employee = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, services,
                roles: EmployeeRole.Therapist,
                certifications: _therapistCertifications
            );

            _ = customer.BookService(PaymentMethod.AtTheSPA, DateTime.Today, service, employee);

            var copy = customer.ListOfBookings as HashSet<Booking>;
            var fieldValue = typeof(Customer)
                .GetField("_listOfBookings",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .GetValue(customer);

            Assert.That(copy, Is.Not.SameAs(fieldValue));
        }

        [Test]
        public void ListOfBookings_ModifyingCopyShouldNotAffectOriginal()
        {
            var service = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);
            var customer = new Customer(_name, _surname, _email, _phone, _dob);

            var services = new List<Service> { service };

            var employee = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, services,
                roles: EmployeeRole.Therapist,
                certifications: _therapistCertifications
            );

            _ = customer.BookService(PaymentMethod.AtTheSPA, DateTime.Today, service, employee);

            var copy = customer.ListOfBookings.ToList();
            copy.Clear();

            Assert.That(customer.ListOfBookings.Count(), Is.EqualTo(1));
        }

        [Test]
        public void BookService_NullService_ShouldThrow()
        {
            var customer = new Customer(_name, _surname, _email, _phone, _dob);

            Assert.Throws<ArgumentNullException>(() =>
                customer.BookService(PaymentMethod.AtTheSPA, DateTime.Today, null!, null!));
        }
    }
}