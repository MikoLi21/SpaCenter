using NUnit.Framework;
using SpaCenter;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SpaCenterTest
{
    [TestFixture]
    public class BookingTests
    {
        private Customer _cust = null!;
        private Employee _emp = null!;
        private Service _svc = null!;
        private List<Service> _services = null!;
        private SpaPerson _person1;
        private SpaPerson _person2;

        private readonly List<string> _therapistCertifications = new() { "Massage certificate" };

        [SetUp]
        public void SetUp()
        {
            Booking.LoadExtent(new List<Booking>());
            Employee.LoadExtent(new List<Employee>());
            Customer.LoadExtent(new List<Customer>());
            Service.LoadExtent(new List<Service>());

            _svc = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            _services = new List<Service> { _svc };
            
            _person1 = new SpaPerson("Anna", "Nowak", "anna@example.com", "111222333");
            _person2 = new SpaPerson("Eva", "Kowalska", "eva@example.com", "999888777");
            
            _person1.AssignToCustomer(new DateTime(2000, 1, 1));
            _person2.AssignToEmployee(12345678901, DateTime.Today.AddYears(-5), 4, _services,
                roles: EmployeeRole.Therapist,
                certifications: _therapistCertifications);

            _cust = (Customer)_person1.Cstmr;
            _emp = (Employee)_person2.Empl;

            //_cust = new Customer("Anna", "Nowak", "anna@example.com", "111222333", new DateTime(2000, 1, 1));

            /*_emp = new Employee(
                "Eva", "Kowalska", "eva@example.com", "999888777",
                12345678901, DateTime.Today.AddYears(-5), 4, _services,
                roles: EmployeeRole.Therapist,
                certifications: _therapistCertifications
            );*/
        }

        // --------------------------------------------------------------
        // Constructor attribute tests
        // --------------------------------------------------------------

        [Test]
        public void Constructor_SetsCustomerCorrectly()
        {
            var b = new Booking(_cust, _svc, _emp, DateTime.Today.AddDays(1), PaymentMethod.AtTheSPA);
            Assert.That(b.Customer, Is.EqualTo(_cust));
        }

        [Test]
        public void Constructor_SetsEmployeeCorrectly()
        {
            var b = new Booking(_cust, _svc, _emp, DateTime.Today.AddDays(1), PaymentMethod.AtTheSPA);
            Assert.That(b.Employee, Is.EqualTo(_emp));
        }

        [Test]
        public void Constructor_SetsServiceCorrectly()
        {
            var b = new Booking(_cust, _svc, _emp, DateTime.Today.AddDays(1), PaymentMethod.AtTheSPA);
            Assert.That(b.Service, Is.EqualTo(_svc));
        }

        [Test]
        public void Constructor_SetsDateCorrectly()
        {
            var d = DateTime.Today.AddDays(3);
            var b = new Booking(_cust, _svc, _emp, d, PaymentMethod.AtTheSPA);
            Assert.That(b.Date, Is.EqualTo(d));
        }

        [Test]
        public void Constructor_SetsPaymentMethodCorrectly()
        {
            var b = new Booking(_cust, _svc, _emp, DateTime.Today.AddDays(1), PaymentMethod.AtTheSPA);
            Assert.That(b.PaymentMethod, Is.EqualTo(PaymentMethod.AtTheSPA));
        }

        [Test]
        public void Constructor_SetsStatusToAccepted()
        {
            var b = new Booking(_cust, _svc, _emp, DateTime.Today.AddDays(1), PaymentMethod.AtTheSPA);
            Assert.That(b.Status, Is.EqualTo(BookingStatus.Accepted));
        }

        // --------------------------------------------------------------
        // Date exception test (constructor only)
        // --------------------------------------------------------------

        [Test]
        public void Constructor_ThrowsException_WhenDateEarlierThanToday()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Booking(_cust, _svc, _emp, DateTime.Today.AddDays(-1), PaymentMethod.AtTheSPA));

            Assert.That(ex!.Message, Is.EqualTo("Booking can't be planned on date earlier than today"));
        }

        [Test]
        public void Bookings_Extent_Is_ReadOnly()
        {
            Booking.LoadExtent(null);

            var booking = new Booking(_cust, _svc, _emp, DateTime.Today.AddDays(1), PaymentMethod.AtTheSPA);

            var extent = Booking.Bookings;

            Assert.That(extent, Is.InstanceOf<IReadOnlyList<Booking>>());
            Assert.Throws<NotSupportedException>(() =>
            {
                ((IList<Booking>)extent).Add(booking);
            });
        }

        [Test]
        public void Changing_Property_Updates_Object_In_Booking_Extent()
        {
            Booking.LoadExtent(null);

            var booking = new Booking(_cust, _svc, _emp, DateTime.Today.AddDays(1), PaymentMethod.AtTheSPA);

            booking.PaymentMethod = PaymentMethod.PaymentGateway;

            Assert.That(Booking.Bookings[0].PaymentMethod, Is.EqualTo(PaymentMethod.PaymentGateway));
        }

        // --------------------------------------------------------------
        // Extent test
        // --------------------------------------------------------------

        [Test]
        public void Extent_StoresAllCreatedBookings()
        {
            var b1 = new Booking(_cust, _svc, _emp, DateTime.Today.AddDays(1), PaymentMethod.AtTheSPA);
            var b2 = new Booking(_cust, _svc, _emp, DateTime.Today.AddDays(2), PaymentMethod.AtTheSPA);

            Assert.That(Booking.Bookings.Count, Is.EqualTo(2));
            Assert.That(Booking.Bookings, Does.Contain(b1));
            Assert.That(Booking.Bookings, Does.Contain(b2));
        }

        [Test]
        public void CreatingBooking_WithInvalidDate_ThrowsException_AndIsNotAddedToExtent()
        {
            var invalidDate = DateTime.Today.AddDays(-2);

            var ex = Assert.Throws<ArgumentException>(() =>
                new Booking(_cust, _svc, _emp, invalidDate, PaymentMethod.AtTheSPA));

            Assert.That(ex!.Message, Is.EqualTo("Booking can't be planned on date earlier than today"));
            Assert.That(Booking.Bookings.Count, Is.EqualTo(0));
        }

        // --------------------------------------------------------------
        // Aggregation association tests (overlapping dates)
        // --------------------------------------------------------------

        [Test]
        public void Constructor_Throws_WhenEmployeeAlreadyHasBookingOnSameDate()
        {
            var date = DateTime.Today.AddDays(1);

            _ = new Booking(_cust, _svc, _emp, date, PaymentMethod.AtTheSPA);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                new Booking(_cust, _svc, _emp, date, PaymentMethod.AtTheSPA));

            Assert.That(ex!.Message, Is.EqualTo("Employee already has appointment at this time"));
        }

        [Test]
        public void SetEmployee_ChangesEmployee_AndUpdatesReverseConnection()
        {
            var date = DateTime.Today.AddDays(2);
            
            var person3 = new SpaPerson("John", "Black", "john@example.com", "777666555");
            person3.AssignToEmployee(98765432101, DateTime.Today.AddYears(-3), 3, _services,
                roles: EmployeeRole.Therapist,
                certifications: new List<string> { "Massage certificate" });
            
            var emp2 = (Employee)person3.Empl;

            var booking = new Booking(_cust, _svc, _emp, date, PaymentMethod.AtTheSPA);

            booking.SetEmployee(emp2);

            Assert.That(booking.Employee, Is.EqualTo(emp2));
            Assert.That(_emp.AssignedTo.Contains(booking), Is.False);
            Assert.That(emp2.AssignedTo.Contains(booking), Is.True);
        }

        [Test]
        public void SetEmployee_SameEmployee_DoesNothing()
        {
            var date = DateTime.Today.AddDays(3);
            var booking = new Booking(_cust, _svc, _emp, date, PaymentMethod.AtTheSPA);

            var countBefore = _emp.AssignedTo.Count();

            booking.SetEmployee(_emp);

            var countAfter = _emp.AssignedTo.Count();

            Assert.That(booking.Employee, Is.EqualTo(_emp));
            Assert.That(countAfter, Is.EqualTo(countBefore));
        }

        [Test]
        public void SetEmployee_Throws_WhenEmployeeIsNull()
        {
            var date = DateTime.Today.AddDays(4);
            var booking = new Booking(_cust, _svc, _emp, date, PaymentMethod.AtTheSPA);

            var ex = Assert.Throws<ArgumentNullException>(() => booking.SetEmployee(null!));
            Assert.That(ex!.ParamName, Is.EqualTo("employee"));
        }

        [Test]
        public void SetEmployee_Throws_WhenNewEmployeeAlreadyHasBookingOnSameDate()
        {
            var date = DateTime.Today.AddDays(5);
            
            var person3 = new SpaPerson("John", "Black", "john@example.com", "777666555");
            person3.AssignToEmployee(98765432101, DateTime.Today.AddYears(-3), 3, _services,
                roles: EmployeeRole.Therapist,
                certifications: new List<string> { "Massage certificate" });
            
            var emp2 = (Employee)person3.Empl;

            _ = new Booking(_cust, _svc, emp2, date, PaymentMethod.AtTheSPA);

            var booking2 = new Booking(_cust, _svc, _emp, date, PaymentMethod.AtTheSPA);

            var ex = Assert.Throws<InvalidOperationException>(() => booking2.SetEmployee(emp2));
            Assert.That(ex!.Message, Is.EqualTo("Employee already has appointment at this time"));
        }

        [Test]
        public void RemoveEmployee_RemovesAssociationFromBothSides()
        {
            var date = DateTime.Today.AddDays(6);
            var booking = new Booking(_cust, _svc, _emp, date, PaymentMethod.AtTheSPA);

            booking.RemoveEmployee();

            Assert.That(booking.Employee, Is.Null);
            Assert.That(_emp.AssignedTo.Contains(booking), Is.False);
        }
    }
}