using NUnit.Framework;
using System;
using SpaCenter;

namespace SpaCenterTest
{
    [TestFixture]
    public class AddressTests
    {
        // --------------------------------------------------------------
        // Constructor attribute tests
        // --------------------------------------------------------------

        [Test]
        public void Constructor_SetsStreetCorrectly()
        {
            var a = new Address("Main Street", 10, "Warsaw", "00-001", "Poland");
            Assert.That(a.Street, Is.EqualTo("Main Street"));
        }

        [Test]
        public void Constructor_SetsBuildingCorrectly()
        {
            var a = new Address("Main Street", 10, "Warsaw", "00-001", "Poland");
            Assert.That(a.Building, Is.EqualTo(10));
        }

        [Test]
        public void Constructor_SetsCityCorrectly()
        {
            var a = new Address("Main Street", 10, "Warsaw", "00-001", "Poland");
            Assert.That(a.City, Is.EqualTo("Warsaw"));
        }

        [Test]
        public void Constructor_SetsPostalCodeCorrectly()
        {
            var a = new Address("Main Street", 10, "Warsaw", "00-001", "Poland");
            Assert.That(a.PostalCode, Is.EqualTo("00-001"));
        }

        [Test]
        public void Constructor_SetsCountryCorrectly()
        {
            var a = new Address("Main Street", 10, "Warsaw", "00-001", "Poland");
            Assert.That(a.Country, Is.EqualTo("Poland"));
        }

        // --------------------------------------------------------------
        // Constructor exception tests
        // --------------------------------------------------------------

        [Test]
        public void Constructor_ThrowsException_WhenStreetEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Address("", 10, "Warsaw", "00-001", "Poland"));

            Assert.That(ex!.Message, Does.Contain("Street can't be empty"));
        }

        [Test]
        public void Constructor_ThrowsException_WhenBuildingInvalid()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Address("Main Street", 0, "Warsaw", "00-001", "Poland"));

            Assert.That(ex!.Message, Does.Contain("Building can't be less or equal to 0"));
        }

        [Test]
        public void Constructor_ThrowsException_WhenCityEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Address("Main Street", 10, "", "00-001", "Poland"));

            Assert.That(ex!.Message, Does.Contain("City can't be empty"));
        }

        [Test]
        public void Constructor_ThrowsException_WhenPostalCodeEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Address("Main Street", 10, "Warsaw", "", "Poland"));

            Assert.That(ex!.Message, Does.Contain("Postal code can't be empty"));
        }

        [Test]
        public void Constructor_ThrowsException_WhenCountryEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Address("Main Street", 10, "Warsaw", "00-001", ""));

            Assert.That(ex!.Message, Does.Contain("Country can't be empty"));
        }
    }
}
