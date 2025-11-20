using System;
using NUnit.Framework;
using SpaCenter;

namespace SpaCenterTest
{
    [TestFixture]
    public class CustomerTests
    {
        
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
            var c = new Customer("John", "Smith", "john@test.com", "+48123456789", new DateTime(1990, 1, 1));
            
            c.DateOfBirth = new DateTime(1985, 5, 5);

            var first = Customer.Customers[0];

            Assert.That(first.DateOfBirth, Is.EqualTo(new DateTime(1985, 5, 5)));
        }
    }
}
