using NUnit.Framework;
using System;
using System.Collections.Generic;
using SpaCenter;

namespace SpaCenterTest
{
    [TestFixture]
    public class BranchTests
    {
        private Address _address = null!;

        [SetUp]
        public void SetUp()
        {
            _address = new Address("Main Street", 10, "Warsaw", "00-001", "Poland");
        }

        // --------------------------------------------------------------
        // Constructor attribute tests
        // --------------------------------------------------------------

        [Test]
        public void Constructor_SetsNameCorrectly()
        {
            var b = new Branch("SPA Warsaw", _address, new List<string> { "+48123456789" });
            Assert.That(b.Name, Is.EqualTo("SPA Warsaw"));
        }

        [Test]
        public void Constructor_SetsAddressCorrectly()
        {
            var b = new Branch("SPA Krakow", _address, new List<string> { "+48123456789" });
            Assert.That(b.Address, Is.EqualTo(_address));
        }

        [Test]
        public void Constructor_SetsPhoneNumbersCorrectly()
        {
            var numbers = new List<string> { "+48123456789", "123456789" };
            var b = new Branch("SPA Gdansk", _address, numbers);

            Assert.That(b.PhoneNumbers, Is.EquivalentTo(numbers));
        }

        [Test]
        public void OpeningTime_IsInitializedCorrectly()
        {
            Assert.That(Branch.OpeningTime, Is.EqualTo(new TimeSpan(9, 0, 0)));
        }

        [Test]
        public void ClosingTime_IsInitializedCorrectly()
        {
            Assert.That(Branch.ClosingTime, Is.EqualTo(new TimeSpan(21, 0, 0)));
        }

        // --------------------------------------------------------------
        // Constructor exception tests
        // --------------------------------------------------------------

        [Test]
        public void Constructor_ThrowsException_WhenNameEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Branch("", _address, new List<string> { "+48123456789" }));

            Assert.That(ex!.Message, Does.Contain("Name can't be empty"));
        }

        [Test]
        public void Constructor_ThrowsException_WhenAddressNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Branch("SPA", null!, new List<string> { "+48123456789" }));

            Assert.That(ex!.ParamName, Is.EqualTo("address"));
        }

        [Test]
        public void Constructor_ThrowsException_WhenPhoneNumbersNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Branch("SPA", _address, null!));

            Assert.That(ex!.Message, Does.Contain("Phone numbers can't be empty"));
        }

        [Test]
        public void Constructor_ThrowsException_WhenPhoneNumberEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Branch("SPA", _address, new List<string> { "" }));

            Assert.That(ex!.Message, Does.Contain("Phone number can't be empty"));
        }

        [Test]
        public void Constructor_ThrowsException_WhenPhoneNumberInvalid()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Branch("SPA", _address, new List<string> { "abc123" }));

            Assert.That(ex!.Message, Is.EqualTo("Invalid phone number"));
        }

        [Test]
        public void Constructor_ThrowsException_WhenNoPhoneNumbersProvided()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Branch("SPA", _address, new List<string>()));

            Assert.That(ex!.Message, Is.EqualTo("At least one phone number is required"));
        }
    }
}
