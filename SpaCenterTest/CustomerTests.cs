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
        
        private SpaPerson _person1;
        private Customer _cust = null!;
        private SpaPerson _person2;
        private Employee _emp = null!;
        private Service _svc = null!;
        private List<Service> _services = null!;

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
            
            _svc = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            _services = new List<Service> { _svc };
            
            _person1 = new SpaPerson(_name, _surname, _email, _phone);
            _person1.AssignToCustomer(_dob);
            _cust = (Customer)_person1.Cstmr;
            
            _person2 = new SpaPerson("Eva", "Kowalska", "eva@example.com", "999888777");
            _person2.AssignToEmployee(12345678901, DateTime.Today.AddYears(-5), 4, _services,
                roles: EmployeeRole.Therapist,
                certifications: _therapistCertifications);
            _emp = (Employee)_person2.Empl;
        }

        [Test]
        public void Constructor_SetsNameCorrectly()
        {
            Assert.That(_person1.Name, Is.EqualTo(_name));
        }

        [Test]
        public void Constructor_SetsSurnameCorrectly()
        {
            Assert.That(_person1.Surname, Is.EqualTo(_surname));
        }

        [Test]
        public void Constructor_SetsEmailCorrectly()
        {
            Assert.That(_person1.Email, Is.EqualTo(_email));
        }

        [Test]
        public void Constructor_SetsPhoneCorrectly()
        {
            Assert.That(_person1.PhoneNumber, Is.EqualTo(_phone));
        }

        [Test]
        public void Constructor_SetsDateOfBirthCorrectly()
        {
            Assert.That(_cust.DateOfBirth, Is.EqualTo(_dob));
        }

        [Test]
        public void Constructor_ThrowsException_WhenBirthDateInFuture()
        {
            var future = DateTime.Today.AddDays(1);

            var ex = Assert.Throws<ArgumentException>(() =>
                _person1.AssignToCustomer(future));

            Assert.That(ex!.Message, Is.EqualTo("Birth date can't be in the future"));
        }

        [Test]
        public void BirthDateInFuture()
        {
            var futureBirthDate = DateTime.Today.AddDays(1);

            var ex = Assert.Throws<ArgumentException>(() =>
                _person1.AssignToCustomer(futureBirthDate));

            Assert.That(ex!.Message, Is.EqualTo("Birth date can't be in the future"));
        }

        [Test]
        public void Extent_Should_Be_ReadOnly()
        {
            var extent = Customer.Customers;

            Assert.That(extent, Is.AssignableTo<IReadOnlyList<Customer>>());

            Assert.Throws<NotSupportedException>(() =>
            {
                ((IList<Customer>)extent).Add(_cust);
            });
        }

        [Test]
        public void Modifying_Property_Should_Update_Object_Inside_Extent()
        {
            _cust.DateOfBirth = new DateTime(1985, 5, 5);

            var first = Customer.Customers[0];
            Assert.That(first.DateOfBirth, Is.EqualTo(new DateTime(1985, 5, 5)));
        }

        [Test]
        public void BookService_ShouldCreateBookingAndReverseConnections()
        {
            var service = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);

            var services = new List<Service> { service };

            var booking = _cust.BookService(PaymentMethod.AtTheSPA, DateTime.Today, service, _emp);

            Assert.That(
                _cust.ListOfBookings,
                Has.One.Matches<Booking>(b =>
                    b.PaymentMethod == booking.PaymentMethod &&
                    b.Date == booking.Date &&
                    b.Service!.Name == booking.Service!.Name &&
                    b.Employee!.Pesel == booking.Employee!.Pesel &&
                    b.Customer!.Prsn.Name == booking.Customer!.Prsn.Name &&
                    b.Customer!.Prsn.Surname == booking.Customer!.Prsn.Surname
                ));

            Assert.That(service.ListOfBookings, Has.One.Matches<Booking>(b => b == booking));

            Assert.That(booking.Customer, Is.EqualTo(_cust));
            Assert.That(booking.Service, Is.EqualTo(service));
            Assert.That(booking.Employee, Is.EqualTo(_emp));
        }

        [Test]
        public void BookService_ShouldAllowMultipleBookingForSameService()
        {
            var service = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);
            
            var person3 = new SpaPerson("John", "Smith", "john@test.com", "+48123456789");
            person3.AssignToCustomer(new DateTime(1990, 1, 1));
            var customer2 = (Customer)person3.Cstmr;
            var services = new List<Service> { service };
            
    
            var booking1 = _cust.BookService(PaymentMethod.AtTheSPA, DateTime.Today, service, _emp);
            var booking2 = _cust.BookService(PaymentMethod.AtTheSPA, new DateTime(2026, 12, 18), service, _emp);
            var booking3 = customer2.BookService(PaymentMethod.AtTheSPA, new DateTime(2027, 12, 20), service, _emp);

            Assert.That(_cust.ListOfBookings.Count(), Is.EqualTo(2));
            Assert.That(customer2.ListOfBookings.Count(), Is.EqualTo(1));
            Assert.That(service.ListOfBookings.Count(), Is.EqualTo(3));

            Assert.That(_cust.ListOfBookings, Has.Exactly(1).EqualTo(booking1));
            Assert.That(_cust.ListOfBookings, Has.Exactly(1).EqualTo(booking2));
            Assert.That(customer2.ListOfBookings, Has.Exactly(1).EqualTo(booking3));
        }

        [Test]
        public void BookService_ShouldNotAllowMultipleBookingForSameServiceAndEmpAtTheSameTime()
        {
            var service = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);
            var person3 = new SpaPerson("John", "Smith", "john@test.com", "+48123456789");
            person3.AssignToCustomer(new DateTime(1990, 1, 1));
            var customer2 = (Customer)person3.Cstmr;

            var services = new List<Service> { service };
            var booking1 = _cust.BookService(
                PaymentMethod.AtTheSPA,
                new DateTime(2025, 12, 18, 14, 30, 0),
                service,
                _emp);

            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                _ = customer2.BookService(
                    PaymentMethod.AtTheSPA,
                    new DateTime(2025, 12, 18, 14, 30, 0),
                    service,
                    _emp);
            });

            Assert.That(ex!.Message, Is.EqualTo("Employee already has appointment at this time"));
            Assert.That(_cust.ListOfBookings.Count(), Is.EqualTo(1));
            Assert.That(customer2.ListOfBookings.Count(), Is.EqualTo(0));
            Assert.That(_cust.ListOfBookings, Has.Exactly(1).EqualTo(booking1));
        }

        [Test]
        public void RemoveBooking_ShouldRemoveAllReferences()
        {
            var service = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);

            var services = new List<Service> { service };
            

            var booking = _cust.BookService(
                PaymentMethod.AtTheSPA,
                new DateTime(2025, 12, 18, 14, 30, 0),
                service,
                _emp);

            booking.RemoveBooking();

            Assert.That(_cust.ListOfBookings, Is.Empty);
            Assert.That(service.ListOfBookings, Is.Empty);

            Assert.That(booking.Customer, Is.Null);
            Assert.That(booking.Service, Is.Null);
        }

        [Test]
        public void RemoveBooking_ShouldNotAffectOtherBookingsInBag()
        {
            var service = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);

            var services = new List<Service> { service };

            var booking1 = _cust.BookService(PaymentMethod.AtTheSPA, DateTime.Today, service, _emp);
            var booking2 = _cust.BookService(PaymentMethod.AtTheSPA, new DateTime(2028, 12, 18), service, _emp);

            booking1.RemoveBooking();

            Assert.That(_cust.ListOfBookings.Count, Is.EqualTo(1));
            Assert.That(service.ListOfBookings.Count, Is.EqualTo(1));
            Assert.That(_cust.ListOfBookings, Has.One.EqualTo(booking2));
        }

        [Test]
        public void ListOfBookings_ShouldReturnCopiesNotReferences()
        {
            var service = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);

            var services = new List<Service> { service };
            

            _ = _cust.BookService(PaymentMethod.AtTheSPA, DateTime.Today, service, _emp);

            var copy = _cust.ListOfBookings as HashSet<Booking>;
            var fieldValue = typeof(Customer)
                .GetField("_listOfBookings",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .GetValue(_cust);

            Assert.That(copy, Is.Not.SameAs(fieldValue));
        }

        [Test]
        public void ListOfBookings_ModifyingCopyShouldNotAffectOriginal()
        {
            var service = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);

            var services = new List<Service> { service };

            _ = _cust.BookService(PaymentMethod.AtTheSPA, DateTime.Today, service, _emp);

            var copy = _cust.ListOfBookings.ToList();
            copy.Clear();

            Assert.That(_cust.ListOfBookings.Count(), Is.EqualTo(1));
        }

        [Test]
        public void BookService_NullService_ShouldThrow()
        {

            Assert.Throws<ArgumentNullException>(() =>
                _cust.BookService(PaymentMethod.AtTheSPA, DateTime.Today, null!, null!));
        }
    }
}