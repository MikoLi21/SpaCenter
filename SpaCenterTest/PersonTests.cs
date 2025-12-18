using System;
using NUnit.Framework;
using SpaCenter;

namespace SpaCenterTest
{
    [TestFixture]
    public class PersonValidationTests
    {
        private DateTime _validBirthDate;
        private SpaPerson _person;

        [SetUp]
        public void Setup()
        {
            _validBirthDate = DateTime.Today.AddYears(-20);
            
            _person = new SpaPerson(
                name: "Anna",
                surname: "Smith",
                email: "test@example.com",
                phone: "123456789"
            );
        }

       
        [Test]
        public void NameIsEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new SpaPerson(
                name: "",
                surname: "Smith",
                email: "test@example.com",
                phone: "123456789"
            ));

            Assert.That(ex.ParamName, Is.EqualTo("Name can't be empty"));
        }

        
        [Test]
        public void SurnameIsEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new SpaPerson(
                    name: "jyjyht",
                    surname: "",
                    email: "test@example.com",
                    phone: "123456789"
                ));

            Assert.That(ex.ParamName, Is.EqualTo("Surname can't be empty"));
        }

       
        [Test]
        public void EmailIsEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new SpaPerson(
                    name: "trjjyk",
                    surname: "Smith",
                    email: "",
                    phone: "123456789"
                ));

            Assert.That(ex.ParamName, Is.EqualTo("Email can't be empty"));
        }

        [Test]
        public void EmailInvalidFormat()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new SpaPerson(
                    name: "fkyulu",
                    surname: "Smith",
                    email: "ghdjs-shgf-dhsg",
                    phone: "123456789"
                ));

            Assert.That(ex.Message, Is.EqualTo("Invalid email address"));
        }

        
        [Test]
        public void PhoneNumberIsEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new SpaPerson(
                    name: "jrjktkyt",
                    surname: "Smith",
                    email: "test@example.com",
                    phone: ""
                ));

            Assert.That(ex.ParamName, Is.EqualTo("Phone number can't be empty"));
        }

        [Test]
        public void PhoneNumberInvalidFormat()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new SpaPerson(
                name: "stjdtykykt",
                surname: "Smith",
                email: "test@example.com",
                phone: "invalid"));

            Assert.That(ex.Message, Is.EqualTo("Invalid phone number"));
        }
    }
}
