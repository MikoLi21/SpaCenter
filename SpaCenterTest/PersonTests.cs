using System;
using NUnit.Framework;
using SpaCenter;

namespace SpaCenterTest
{
    [TestFixture]
    public class PersonValidationTests
    {
        private DateTime _validBirthDate;

        [SetUp]
        public void Setup()
        {
            _validBirthDate = DateTime.Today.AddYears(-20);
        }

       
        [Test]
        public void NameIsEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Customer(
                    name: "",
                    surname: "Smith",
                    email: "test@example.com",
                    phoneNumber: "123456789",
                    dateOfBirth: _validBirthDate
                ));

            Assert.That(ex.ParamName, Is.EqualTo("Name can't be empty"));
        }

        
        [Test]
        public void SurnameIsEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Customer(
                    name: "Anna",
                    surname: "",
                    email: "test@example.com",
                    phoneNumber: "123456789",
                    dateOfBirth: _validBirthDate
                ));

            Assert.That(ex.ParamName, Is.EqualTo("Surname can't be empty"));
        }

       
        [Test]
        public void EmailIsEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Customer(
                    name: "Anna",
                    surname: "Smith",
                    email: "",
                    phoneNumber: "123456789",
                    dateOfBirth: _validBirthDate
                ));

            Assert.That(ex.ParamName, Is.EqualTo("Email can't be empty"));
        }

        [Test]
        public void EmailInvalidFormat()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Customer(
                    name: "Anna",
                    surname: "Smith",
                    email: "invalid-email-format",
                    phoneNumber: "123456789",
                    dateOfBirth: _validBirthDate
                ));

            Assert.That(ex.Message, Is.EqualTo("Invalid email address"));
        }

        
        [Test]
        public void PhoneNumberIsEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Customer(
                    name: "Anna",
                    surname: "Smith",
                    email: "test@example.com",
                    phoneNumber: "",
                    dateOfBirth: _validBirthDate
                ));

            Assert.That(ex.ParamName, Is.EqualTo("Phone number can't be empty"));
        }

        [Test]
        public void PhoneNumberInvalidFormat()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Customer(
                    name: "Anna",
                    surname: "Smith",
                    email: "test@example.com",
                    phoneNumber: "123-INVALID",
                    dateOfBirth: _validBirthDate
                ));

            Assert.That(ex.Message, Is.EqualTo("Invalid phone number"));
        }
    }
}
