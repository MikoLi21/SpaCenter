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
        
        
        //reflexive association testing
        [Test]
        public void AddServiceTo_ShouldCreateForwardAndReverseAssociation()
        {
            var s1 = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            var s2 = new Service("Face Mask", "face mask grey", TimeSpan.FromMinutes(40), 200m, 16);

            s2.AddServiceTo(s1);

            Assert.That(s2.ServiceTo, Contains.Item(s1));
            Assert.That(s1.ServiceOf, Contains.Item(s2));
        }
        
        [Test]
        public void AddServiceTo_ShouldThrow_WhenServiceReferencesItself()
        {
            var s1 = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);

            Assert.Throws<InvalidOperationException>(() => s1.AddServiceTo(s1));
        }
        
        [Test]
        public void AddServiceTo_ShouldAllowMultipleServicesTo()
        {
            var s1 = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            var s2 = new Service("Face Mask", "face mask grey", TimeSpan.FromMinutes(40), 200m, 16);
            var s3 = new Service("Aromatherapy", "oil aroma", TimeSpan.FromMinutes(40), 200m, 5);

            s3.AddServiceTo(s2);
            s3.AddServiceTo(s1);
            
            Assert.That(s3.ServiceTo.Count, Is.EqualTo(2));
            Assert.That(s2.ServiceOf, Contains.Item(s3));
            Assert.That(s1.ServiceOf, Contains.Item(s3));
        }
        
        [Test]
        public void AddServiceTo_ShouldNotDuplicateEntries()
        {
            var s1 = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            var s2 = new Service("Face Mask", "face mask grey", TimeSpan.FromMinutes(40), 200m, 16);

            s1.AddServiceTo(s2);
            s1.AddServiceTo(s2);   // second call should be ignored

            Assert.That(s1.ServiceTo.Count, Is.EqualTo(1));
            Assert.That(s2.ServiceOf.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void RemoveServiceTo_ShouldRemoveForwardAndReverseAssociation()
        {
            var s1 = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            var s2 = new Service("Face Mask", "face mask grey", TimeSpan.FromMinutes(40), 200m, 16);

            s2.AddServiceTo(s1);
            s2.RemoveServiceTo(s1);

            Assert.That(s2.ServiceTo.Count, Is.EqualTo(0));
            Assert.That(s1.ServiceOf.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void AddServiceOf_ShouldCreateForwardAndReverseAssociation()
        {
            var s1 = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            var s2 = new Service("Face Mask", "face mask grey", TimeSpan.FromMinutes(40), 200m, 16);

            s1.AddServiceOf(s2);

            Assert.That(s1.ServiceOf, Contains.Item(s2));
            Assert.That(s2.ServiceTo, Contains.Item(s1));
        }
        
        [Test]
        public void AddServiceOf_ShouldAllowMultipleServicesOf()
        {
            var s1 = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            var s2 = new Service("Face Mask", "face mask grey", TimeSpan.FromMinutes(40), 200m, 16);
            var s3 = new Service("Aromatherapy", "oil aroma", TimeSpan.FromMinutes(40), 200m, 5);

            s1.AddServiceOf(s3);
            s2.AddServiceOf(s3);

            Assert.That(s3.ServiceTo.Count, Is.EqualTo(2));
            Assert.That(s2.ServiceOf, Contains.Item(s3));
            Assert.That(s1.ServiceOf, Contains.Item(s3));
        }
        
        [Test]
        public void RemoveServiceOf_ShouldRemoveForwardAndReverseAssociation()
        {
            var s1 = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            var s2 = new Service("Face Mask", "face mask grey", TimeSpan.FromMinutes(40), 200m, 16);

            s1.AddServiceOf(s2);
            s1.RemoveServiceOf(s2);

            Assert.That(s2.ServiceTo.Count, Is.EqualTo(0));
            Assert.That(s1.ServiceOf.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void ShouldSupportLongChainsOfServicesTo()
        {
            var s1 = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            var s2 = new Service("Face Mask", "face mask grey", TimeSpan.FromMinutes(40), 200m, 16);
            var s3 = new Service("Aromatherapy", "oil aroma", TimeSpan.FromMinutes(40), 200m, 5);

            s3.AddServiceTo(s2);
            s2.AddServiceTo(s1);

            Assert.That(s1.ServiceOf, Contains.Item(s2));
            Assert.That(s2.ServiceOf, Contains.Item(s3));
            Assert.That(s2.ServiceTo, Contains.Item(s1));
            Assert.That(s3.ServiceTo, Contains.Item(s2));
        }
    }
}





