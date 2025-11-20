using System;
using NUnit.Framework;
using SpaCenter;

namespace SpaCenterTest
{
    [TestFixture]
    public class CustomerTests
    {
        private string _name = null!;
        private string _surname = null!;
        private string _email = null!;
        private string _phone = null!;
        private DateTime _dob;

        [SetUp]
        public void SetUp()
        {
            Customer.LoadExtent(new List<Customer>());
            _name = "Anna";
            _surname = "Nowak";
            _email = "anna@example.com";
            _phone = "123456789";
            _dob = new DateTime(2000, 1, 1);
        }

    
        [Test]
        public void Constructor_SetsNameCorrectly()
        {
            var c = new Customer(_name, _surname, _email, _phone, _dob);
            Assert.That(c.Name, Is.EqualTo(_name));
        }

        [Test]
        public void Constructor_SetsSurnameCorrectly()
        {
            var c = new Customer(_name, _surname, _email, _phone, _dob);
            Assert.That(c.Surname, Is.EqualTo(_surname));
        }

        [Test]
        public void Constructor_SetsEmailCorrectly()
        {
            var c = new Customer(_name, _surname, _email, _phone, _dob);
            Assert.That(c.Email, Is.EqualTo(_email));
        }

        [Test]
        public void Constructor_SetsPhoneCorrectly()
        {
            var c = new Customer(_name, _surname, _email, _phone, _dob);
            Assert.That(c.PhoneNumber, Is.EqualTo(_phone));
        }

        [Test]
        public void Constructor_SetsDateOfBirthCorrectly()
        {
            var c = new Customer(_name, _surname, _email, _phone, _dob);
            Assert.That(c.DateOfBirth, Is.EqualTo(_dob));
        }

      
        [Test]
        public void Constructor_ThrowsException_WhenBirthDateInFuture()
        {
            var future = DateTime.Today.AddDays(1);

            var ex = Assert.Throws<ArgumentException>(() =>
                new Customer(_name, _surname, _email, _phone, future));

            Assert.That(ex!.Message, Is.EqualTo("Birth date can't be in the future"));
        }
        
        [Test]
        public void BirthDateInFuture()
        {
            var futureBirthDate = DateTime.Today.AddDays(1);

            var ex = Assert.Throws<ArgumentException>(() =>
                new Customer(
                    name: "Anna",
                    surname: "Smith",
                    email: "anna@example.com",
                    phoneNumber: "123456789",
                    dateOfBirth: futureBirthDate   
                ));

            Assert.That(ex.Message, Is.EqualTo("Birth date can't be in the future"));
        }
        
        [Test]
        public void Extent_Should_Be_ReadOnly()
        {
            var c = new Customer("John", "Smith", "john@test.com", "+48123456789", new DateTime(1990, 1, 1));

            var extent = Customer.Customers;
            
            Assert.That(extent, Is.AssignableTo<IReadOnlyList<Customer>>());
            
            Assert.Throws<NotSupportedException>(() =>
            {
                ((IList<Customer>)extent).Add(c);
            });
        }

        [Test]
        public void Modifying_Property_Should_Update_Object_Inside_Extent()
        {
            var c = new Customer(_name, _surname, _email, _phone, _dob);

            c.DateOfBirth = new DateTime(1985, 5, 5);

            var first = Customer.Customers[0];
            Assert.That(first.DateOfBirth, Is.EqualTo(new DateTime(1985, 5, 5)));
        }
    }
}