using NUnit.Framework;
using SpaCenter;

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
        
        
        //association
        [Test]
        public void AddSubService_CreatesAssociation()
        {
            var main = new Service("Main", "Main desc", TimeSpan.FromMinutes(30), 100m, 10);
            var sub = new Service("Sub", "Sub desc", TimeSpan.FromMinutes(15), 50m, 5);

            main.AddSubService(sub);

            Assert.That(main.SubServices.Count, Is.EqualTo(1));
            Assert.That(main.SubServices[0], Is.EqualTo(sub));
        }

        [Test]
        public void AddSubService_DoesNotAddDuplicate()
        {
            var main = new Service("Main", "Main desc", TimeSpan.FromMinutes(30), 100m, 10);
            var sub = new Service("Sub", "Sub desc", TimeSpan.FromMinutes(15), 50m, 5);

            main.AddSubService(sub);
            main.AddSubService(sub); 

            Assert.That(main.SubServices.Count, Is.EqualTo(1));
        }

        [Test]
        public void RemoveSubService_RemovesAssociation()
        {
            var main = new Service("Main", "Main desc", TimeSpan.FromMinutes(30), 100m, 10);
            var sub = new Service("Sub", "Sub desc", TimeSpan.FromMinutes(15), 50m, 5);

            main.AddSubService(sub);
            main.RemoveSubService(sub);

            Assert.That(main.SubServices.Count, Is.EqualTo(0));
        }

        [Test]
        public void RemoveSubService_DoesNothing_WhenSubServiceNotPresent()
        {
            var main = new Service("Main", "Main desc", TimeSpan.FromMinutes(30), 100m, 10);
            var sub = new Service("Sub", "Sub desc", TimeSpan.FromMinutes(15), 50m, 5);

           
            main.RemoveSubService(sub);

            Assert.That(main.SubServices.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddSubService_ThrowsException_WhenSubServiceIsNull()
        {
            var main = new Service("Main", "Main desc", TimeSpan.FromMinutes(30), 100m, 10);

            var ex = Assert.Throws<ArgumentNullException>(() =>
                main.AddSubService(null!)
            );

            Assert.That(ex!.Message, Does.Contain("Sub-service cannot be null"));
        }

        [Test]
        public void AddSubService_ThrowsException_WhenServiceAddsItself()
        {
            var main = new Service("Main", "Main desc", TimeSpan.FromMinutes(30), 100m, 10);

            var ex = Assert.Throws<ArgumentException>(() =>
                main.AddSubService(main)
            );

            Assert.That(ex!.Message, Does.Contain("Service cannot be part of itself"));
        }

        [Test]
        public void RemoveSubService_ThrowsException_WhenSubServiceIsNull()
        {
            var main = new Service("Main", "Main desc", TimeSpan.FromMinutes(30), 100m, 10);

            var ex = Assert.Throws<ArgumentNullException>(() =>
                main.RemoveSubService(null!)
            );

            Assert.That(ex!.Message, Does.Contain("Sub-service cannot be null"));
        }
    }
}





