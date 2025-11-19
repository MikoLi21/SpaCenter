using NUnit.Framework;
using SpaCenter;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
/*
namespace SpaCenterTest
{
    [TestFixture]
    public class BookingTests
    {
        private Customer _cust = null!;
        private TestEmployee _emp = null!;
        private Service _svcAdult = null!;
        private Service _svcKids = null!;

        [SetUp]
        public void SetUp()
        {
            Customer.AllCustomers.Clear();
            Employee.AllEmployees.Clear();
            Booking.AllBookings.Clear();
            Service.AllServices.Clear();

            _cust = new Customer("Anna", "Nowak", "a@b.com", "123456789", new DateTime(2000, 1, 1)); // взрослый
            _emp  = new TestEmployee("Eva", "Kowalska", "e@b.com", "987654321", "12345678901", DateTime.Today.AddYears(-3));

            _svcAdult = new Service("Massage", "Relax", 60, 100m, minimalAge: 16);
            _svcKids  = new Service("KidsCare", "Soft", 20, 30m, minimalAge: 10);

            Service.AllServices.Add(_svcAdult);
            Service.AllServices.Add(_svcKids);
        }

        [Test]
        public void Ctor_DefaultStatus_IsAccepted()
        {
            var b = new Booking(_cust, _emp, DateTime.Today, new TimeSpan(10,0,0), "cash");
            Assert.That(b.Status, Is.EqualTo("accepted"));
        }

        
        [Test]
        public void ChangeBookingStatus_InvalidValue_DoesNotChange()
        {
            var b = new Booking(_cust, _emp, DateTime.Today, new TimeSpan(10,0,0), "cash");
            var prev = b.Status;

            using var sw = new StringWriter();
            Console.SetOut(sw);

            b.ChangeBookingStatus("weird-status");

            Assert.That(b.Status, Is.EqualTo(prev));
            Assert.That(sw.ToString(), Does.Contain("Invalid status value."));
        }

        [Test]
        public void CheckBookings_PrintsSummary()
        {
            var t = new TimeSpan(11, 0, 0);
            var b = new Booking(_cust, _emp, new DateTime(2025, 11, 12), t, "cash");

            using var sw = new StringWriter();
            Console.SetOut(sw);

            Booking.CheckBookings();

            var output = sw.ToString();
            Assert.That(output, Does.Contain(_cust.Name));
            Assert.That(output, Does.Contain("Status"));
            Assert.That(output, Does.Contain("11:00:00"));
        }

        [Test]
        public void MakeBooking_EmployeeNotFound_PrintsMessage_AndReturns()
        {
            var b = new Booking(_cust, _emp, DateTime.Today, new TimeSpan(9,0,0), "cash");

            using var sw = new StringWriter();
            Console.SetOut(sw);

            b.MakeBooking(DateTime.Today.AddDays(1), "no-such-employee", new TimeSpan(14, 0, 0), _svcAdult.Name);

            var output = sw.ToString();
            Assert.That(output, Does.Contain("Employee not found"));
            Assert.That(b.Employee, Is.Null);
            Assert.That(b.Service, Is.Not.Null); // сервис найден, но из-за сотрудника ранний return
            Assert.That(b.Status, Is.EqualTo("accepted")); // не менялся
        }

        [Test]
        public void MakeBooking_ServiceNotFound_PrintsMessage_AndReturns()
        {
            var b = new Booking(_cust, _emp, DateTime.Today, new TimeSpan(9,0,0), "cash");

            using var sw = new StringWriter();
            Console.SetOut(sw);

            b.MakeBooking(DateTime.Today.AddDays(1), _emp.Name, new TimeSpan(14, 0, 0), "no-such-service");

            var output = sw.ToString();
            Assert.That(output, Does.Contain("Service not found."));
            Assert.That(b.Employee, Is.Not.Null);
            Assert.That(b.Service, Is.Null);
            Assert.That(b.Status, Is.EqualTo("accepted"));
        }

        [Test]
        public void MakeBooking_NotLoggedIn_PrintsLoginMessage_AndReturns()
        {
            _cust.IsLoggedIn = false;

            var b = new Booking(_cust, null!, DateTime.Today, new TimeSpan(9,0,0), "cash");

            using var sw = new StringWriter();
            Console.SetOut(sw);

            b.MakeBooking(DateTime.Today.AddDays(1), _emp.Name, new TimeSpan(14, 0, 0), _svcAdult.Name);

            var output = sw.ToString();
            Assert.That(output, Does.Contain("You must log in to continue."));
            Assert.That(b.Employee, Is.EqualTo(_emp));
            Assert.That(b.Service, Is.EqualTo(_svcAdult));
            Assert.That(b.Status, Is.EqualTo("accepted"));
        }

        [Test]
        public void MakeBooking_UnderAge_PrintsMinAgeMessage_AndReturns()
        {
            var teen = new Customer("Teen", "K", "t@b.com", "111", DateTime.Today.AddYears(-12)); // 12 лет
            teen.IsLoggedIn = true;

            var b = new Booking(teen, null!, DateTime.Today, new TimeSpan(9,0,0), "cash");

            using var sw = new StringWriter();
            Console.SetOut(sw);

            b.MakeBooking(DateTime.Today.AddDays(1), _emp.Name, new TimeSpan(14, 0, 0), _svcAdult.Name);

            var output = sw.ToString();
            Assert.That(output, Does.Contain($"Minimum age for this service is {_svcAdult.MinimalAge}"));
            Assert.That(b.Status, Is.EqualTo("accepted"));
        }

        [Test]
        public void MakeBooking_Success_SetsAccepted_AndFields()
        {
            _cust.IsLoggedIn = true;

            var b = new Booking(_cust, null!, DateTime.Today, new TimeSpan(9,0,0), "cash");

            var selDate = DateTime.Today.AddDays(3);
            var selTime = new TimeSpan(13, 15, 0);

            using var sw = new StringWriter();
            Console.SetOut(sw);

            b.MakeBooking(selDate, _emp.Name, selTime, _svcAdult.Name);

            var output = sw.ToString();
            Assert.That(output, Does.Contain("Booking created successfully"));
            Assert.That(b.Employee, Is.EqualTo(_emp));
            Assert.That(b.Service, Is.EqualTo(_svcAdult));
            Assert.That(b.Date.Date, Is.EqualTo(selDate.Date));
            Assert.That(b.Time, Is.EqualTo(selTime));
            Assert.That(b.Status, Is.EqualTo("accepted"));
        }

        private sealed class TestEmployee : Employee
        {
            public TestEmployee(string name, string surname, string email, string phoneNumber, string pesel, DateTime hireDate)
                : base(name, surname, email, phoneNumber, pesel, hireDate) { }

            public override string ToString() => Name;
        }
    }
}*/

namespace SpaCenterTest
{
    [TestFixture]
    public class BookingTests
    {
        private Customer _cust = null!;
        private Employee _emp = null!;
        private Service _svc = null!;

        [SetUp]
        public void SetUp()
        {
            //I used reflection to clear the booking_list instead of creating a simple public method in Booking class to clear the booking_List, because it shouldn't be possible 
            typeof(Booking)
                .GetField("bookings_List", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                !.SetValue(null, new List<Booking>());
            
            _cust = new Customer("Anna", "Nowak", "anna@example.com", "111222333", new DateTime(2000, 1, 1));
            _emp  = new Employee("Eva", "Kowalska", "eva@example.com", "999888777", 12345678901, DateTime.Today.AddYears(-5), 4);
            _svc = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
        }

        // --------------------------------------------------------------
        // Constructor attribute tests
        // --------------------------------------------------------------

        [Test]
        public void Constructor_SetsCustomerCorrectly()
        {
            var b = new Booking(_cust, _emp, _svc, DateTime.Today.AddDays(1), PaymentMethod.AtTheSPA);
            Assert.That(b.Customer, Is.EqualTo(_cust));
        }

        [Test]
        public void Constructor_SetsEmployeeCorrectly()
        {
            var b = new Booking(_cust, _emp, _svc, DateTime.Today.AddDays(1), PaymentMethod.AtTheSPA);
            Assert.That(b.Employee, Is.EqualTo(_emp));
        }

        [Test]
        public void Constructor_SetsServiceCorrectly()
        {
            var b = new Booking(_cust, _emp, _svc, DateTime.Today.AddDays(1), PaymentMethod.AtTheSPA);
            Assert.That(b.Service, Is.EqualTo(_svc));
        }

        [Test]
        public void Constructor_SetsDateCorrectly()
        {
            DateTime d = DateTime.Today.AddDays(3);
            var b = new Booking(_cust, _emp, _svc, d, PaymentMethod.AtTheSPA);
            Assert.That(b.Date, Is.EqualTo(d));
        }

        [Test]
        public void Constructor_SetsPaymentMethodCorrectly()
        {
            var b = new Booking(_cust, _emp, _svc, DateTime.Today.AddDays(1), PaymentMethod.AtTheSPA);
            Assert.That(b.PaymentMethod, Is.EqualTo(PaymentMethod.AtTheSPA));
        }

        [Test]
        public void Constructor_SetsStatusToAccepted()
        {
            var b = new Booking(_cust, _emp, _svc, DateTime.Today.AddDays(1), PaymentMethod.AtTheSPA);
            Assert.That(b.Status, Is.EqualTo(BookingStatus.Accepted));
        }

        // --------------------------------------------------------------
        // Date exception test (constructor only)
        // --------------------------------------------------------------

        [Test]
        public void Constructor_ThrowsException_WhenDateEarlierThanToday()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Booking(_cust, _emp, _svc, DateTime.Today.AddDays(-1), PaymentMethod.AtTheSPA));

            Assert.That(ex!.Message, Is.EqualTo("Booking can't be planned on date earlier than today"));
        }
        
        // --------------------------------------------------------------
        // Extent test
        // --------------------------------------------------------------
        
        
        [Test]
        public void Extent_StoresAllCreatedBookings()
        {
            // Arrange
            var b1 = new Booking(_cust, _emp, _svc, DateTime.Today.AddDays(1), PaymentMethod.AtTheSPA);
            var b2 = new Booking(_cust, _emp, _svc, DateTime.Today.AddDays(2), PaymentMethod.AtTheSPA);

            // Assert
            Assert.That(Booking.Bookings.Count, Is.EqualTo(2));
            Assert.That(Booking.Bookings, Does.Contain(b1));
            Assert.That(Booking.Bookings, Does.Contain(b2));
        }
        
        [Test]
        public void CreatingBooking_WithInvalidDate_ThrowsException_AndIsNotAddedToExtent()
        {
            // Arrange
            var invalidDate = DateTime.Today.AddDays(-2);

            // Act & Assert – constructor should throw
            var ex = Assert.Throws<ArgumentException>(() =>
                new Booking(_cust, _emp, _svc, invalidDate, PaymentMethod.AtTheSPA));

            Assert.That(ex!.Message, Is.EqualTo("Booking can't be planned on date earlier than today"));

            // Extent should remain empty
            Assert.That(Booking.Bookings.Count, Is.EqualTo(0));
        }
    }
}
