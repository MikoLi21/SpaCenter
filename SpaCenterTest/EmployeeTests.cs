using NUnit.Framework;
using SpaCenter;
using System;

namespace SpaCenter.Tests
{
    [TestFixture]
    public class EmployeeTests
    {
        [SetUp]
        public void SetUp()
        {
            Customer.AllCustomers.Clear();
            Employee.AllEmployees.Clear();
            Booking.AllBookings.Clear();
            Service.AllServices.Clear();
        }

        [Test]
        public void YearsOfService_Computed_From_HireDate()
        {
            var years = 4;
            var e = new TestEmployee("Eva", "K", "e@b.com", "1", "12345678901", DateTime.Today.AddYears(-years));
            Assert.That(e.YearsOfService, Is.EqualTo(years));
        }

        [Test]
        public void AverageServiceMinutes_From_Bookings_With_Service()
        {
            var e = new TestEmployee("Eva", "K", "e@b.com", "1", "12345678901", DateTime.Today.AddYears(-5));
            var c = new Customer("Anna", "N", "a@b.com", "2", new DateTime(2000,1,1));

            var b1 = new Booking(c, e, DateTime.Today, new TimeSpan(10,0,0), "cash") { Service = new Service("S1", "D1", 60, 10m, 0) };
            var b2 = new Booking(c, e, DateTime.Today, new TimeSpan(11,0,0), "cash") { Service = new Service("S2", "D2", 30, 10m, 0) };
            var b3 = new Booking(c, e, DateTime.Today, new TimeSpan(12,0,0), "cash") { Service = new Service("S3", "D3", 45, 10m, 0) };

            e.Bookings.Add(b1);
            e.Bookings.Add(b2);
            e.Bookings.Add(b3);

            Assert.That(e.AverageServiceMinutes, Is.EqualTo((60 + 30 + 45) / 3.0).Within(0.001));
        }

        [Test]
        public void AverageServiceMinutes_NoBookings_Or_NullService_Returns_Zero()
        {
            var e = new TestEmployee("Eva", "K", "e@b.com", "1", "12345678901", DateTime.Today.AddYears(-5));
            Assert.That(e.AverageServiceMinutes, Is.EqualTo(0));

            var c = new Customer("Anna", "N", "a@b.com", "2", new DateTime(2000,1,1));
            var b = new Booking(c, e, DateTime.Today, new TimeSpan(10,0,0), "cash"); // Service == null
            e.Bookings.Add(b);
            Assert.That(e.AverageServiceMinutes, Is.EqualTo(0));
        }

        [Test]
        public void CheckYearsOfService_DoesNotThrow()
        {
            var e1 = new TestEmployee("E1", "A", "e1@b.com", "1", "11111111111", DateTime.Today.AddYears(-2));
            var e2 = new TestEmployee("E2", "B", "e2@b.com", "2", "22222222222", DateTime.Today.AddYears(-3));
            Employee.AllEmployees.Add(e1);
            Employee.AllEmployees.Add(e2);

            Assert.DoesNotThrow(() => Employee.CheckYearsOfService());
        }

        private sealed class TestEmployee : Employee
        {
            public TestEmployee(string name, string surname, string email, string phoneNumber, string pesel, DateTime hireDate)
                : base(name, surname, email, phoneNumber, pesel, hireDate) { }
        }
    }
}
