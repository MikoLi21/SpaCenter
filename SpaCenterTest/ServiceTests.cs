using NUnit.Framework;
using SpaCenter;
/*
namespace SpaCenterTest;

[TestFixture]
public class ServiceTests
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
    public void Constructor_Assigns_All_Properties()
    {
        var s = new Service("Massage", "Relax full body", 60, 100m, 16);
        Assert.That(s.Name, Is.EqualTo("Massage"));
        Assert.That(s.Description, Is.EqualTo("Relax full body"));
        Assert.That(s.Duration, Is.EqualTo(60));
        Assert.That(s.Price, Is.EqualTo(100m));
        Assert.That(s.MinimalAge, Is.EqualTo(16));
    }

    
    
    [Test]
    public void ViewServices_Returns_AllServices_List()
    {
        var s1 = new Service("Massage", "Relax full body", 60, 120m, 16);
        var s2 = new Service("Facial", "Skin treatment", 45, 80m, 14);

        var result = Service.ViewServices();

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result, Does.Contain(s1));
        Assert.That(result, Does.Contain(s2));
    }
    

        
    }


    */

namespace SpaCenterTest
{
    [TestFixture]
    public class ServiceTests
    {
        // --------------------------------------------------------------
        // Constructor attribute tests
        // --------------------------------------------------------------

        [Test]
        public void Constructor_SetsNameCorrectly()
        {
            var s = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            Assert.That(s.Name, Is.EqualTo("Massage"));
        }

        [Test]
        public void Constructor_SetsDescriptionCorrectly()
        {
            var s = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            Assert.That(s.Description, Is.EqualTo("Relaxing massage"));
        }

        [Test]
        public void Constructor_SetsDurationCorrectly()
        {
            var duration = TimeSpan.FromMinutes(45);
            var s = new Service("Peeling", "Skin peeling", duration, 150m, 18);
            Assert.That(s.Duration, Is.EqualTo(duration));
        }

        [Test]
        public void Constructor_SetsPriceCorrectly()
        {
            var s = new Service("Mask", "Face mask", TimeSpan.FromMinutes(30), 99.99m, 12);
            Assert.That(s.Price, Is.EqualTo(99.99m));
        }

        [Test]
        public void Constructor_SetsMinimalAgeCorrectly()
        {
            var s = new Service("Sauna", "Steam sauna", TimeSpan.FromMinutes(20), 50m, 18);
            Assert.That(s.MinimalAge, Is.EqualTo(18));
        }

        // --------------------------------------------------------------
        // Constructor exception tests
        // --------------------------------------------------------------

        [Test]
        public void Constructor_ThrowsException_WhenNameIsEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Service("", "Desc", TimeSpan.FromMinutes(10), 10m, 1));

            Assert.That(ex!.Message, Does.Contain("Name can't be empty"));
        }

        [Test]
        public void Constructor_ThrowsException_WhenDescriptionIsEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Service("Test", "", TimeSpan.FromMinutes(10), 10m, 1));

            Assert.That(ex!.Message, Does.Contain("Description can't be empty"));
        }

        [Test]
        public void Constructor_ThrowsException_WhenDurationZeroOrNegative()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Service("Test", "Desc", TimeSpan.Zero, 10m, 1));

            Assert.That(ex!.Message, Is.EqualTo("Duration can't be zero"));
        }

        [Test]
        public void Constructor_ThrowsException_WhenPriceZeroOrNegative()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Service("Test", "Desc", TimeSpan.FromMinutes(10), 0m, 1));

            Assert.That(ex!.Message, Is.EqualTo("Price can't be zero or negative"));
        }

        [Test]
        public void Constructor_ThrowsException_WhenMinimalAgeZeroOrNegative()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Service("Test", "Desc", TimeSpan.FromMinutes(10), 10m, 0));

            Assert.That(ex!.Message, Is.EqualTo("Age can't be zero or negative"));
        }
    }
}





