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
