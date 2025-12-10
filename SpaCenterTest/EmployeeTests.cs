using NUnit.Framework;
using SpaCenter;
using System;

namespace SpaCenterTest
{
    [TestFixture]
    public class EmployeeTests
    {
        private DateTime _validHireDate;
        private string _name = null!;
        private string _surname = null!;
        private string _email = null!;
        private string _phone = null!;
        private long _pesel;
        private DateTime _hireDate;
        private decimal _years;
        private List<Service> _services = new List<Service>();
        private Service _s1;

        [SetUp]
        public void Setup()
        {
            _name = "Eva";
            _surname = "Kowalska";
            _email = "eva@example.com";
            _phone = "999888777";
            _pesel = 12345678901;
            _hireDate = DateTime.Today.AddYears(-5);
            _years = 4;

            
            _validHireDate = DateTime.Today.AddYears(-1);
            _s1 = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            _services.Add(_s1);

            
        }


        [Test]
        public void Constructor_SetsNameCorrectly()
        {
            var e = new Employee(_name, _surname, _email, _phone, _pesel, _hireDate, _years, _services);
            Assert.That(e.Name, Is.EqualTo(_name));
        }

        [Test]
        public void Constructor_SetsSurnameCorrectly()
        {
            var e = new Employee(_name, _surname, _email, _phone, _pesel, _hireDate, _years, _services);
            Assert.That(e.Surname, Is.EqualTo(_surname));
        }

        [Test]
        public void Constructor_SetsEmailCorrectly()
        {
            var e = new Employee(_name, _surname, _email, _phone, _pesel, _hireDate, _years, _services);
            Assert.That(e.Email, Is.EqualTo(_email));
        }

        [Test]
        public void Constructor_SetsPhoneCorrectly()
        {
            var e = new Employee(_name, _surname, _email, _phone, _pesel, _hireDate, _years, _services);
            Assert.That(e.PhoneNumber, Is.EqualTo(_phone));
        }

        [Test]
        public void Constructor_SetsPeselCorrectly()
        {
            var e = new Employee(_name, _surname, _email, _phone, _pesel, _hireDate, _years, _services);
            Assert.That(e.Pesel, Is.EqualTo(_pesel));
        }

        [Test]
        public void Constructor_SetsHireDateCorrectly()
        {
            var e = new Employee(_name, _surname, _email, _phone, _pesel, _hireDate, _years, _services);
            Assert.That(e.HireDate, Is.EqualTo(_hireDate));
        }

        [Test]
        public void Constructor_SetsYearsOfExperienceCorrectly()
        {
            var e = new Employee(_name, _surname, _email, _phone, _pesel, _hireDate, _years, _services);
            Assert.That(e.YearsOfExperience, Is.EqualTo(_years));
        }

        
        [Test]
        public void InvalidPesel()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Employee(
                    name: "Anna",
                    surname: "Smith",
                    email: "anna@example.com",
                    phoneNumber: "123456789",
                    pesel: 1234,               
                    hireDate: _validHireDate,
                    yearsOfExperience: 5,
                    services: _services
                ));

            Assert.That(ex.Message, Is.EqualTo("Invalid pesel number"));
        }

        
        [Test]
        public void HireDateInFuture()
        {
            var futureHire = DateTime.Today.AddDays(1);

            var ex = Assert.Throws<ArgumentException>(() =>
                new Employee(
                    name: "Anna",
                    surname: "Smith",
                    email: "anna@example.com",
                    phoneNumber: "123456789",
                    pesel: 12345678901,
                    hireDate: futureHire,     
                    yearsOfExperience: 5,
                    services: _services
                ));

            Assert.That(ex.Message, Is.EqualTo("Hire date can't be in the future"));
        }

        
        [Test]
        public void LeaveDateBeforeHireDate()
        {
            var employee = new Employee(
                "Anna", "Smith", "anna@example.com", "123456789",
                12345678901, _validHireDate, 5, _services
            );

            var invalidLeave = _validHireDate.AddDays(-1);

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                employee.LeaveDate = invalidLeave;
            });

            Assert.That(ex.Message, Is.EqualTo("Leave date can't be before hire date"));
        }

       
        [Test]
        public void LeaveDateInFuture()
        {
            var employee = new Employee(
                "Anna", "Smith", "anna@example.com", "123456789",
                12345678901, _validHireDate, 5, _services
            );

            var futureLeave = DateTime.Today.AddDays(1);

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                employee.LeaveDate = futureLeave;
            });

            Assert.That(ex.Message, Is.EqualTo("Leave date can't be in the future"));
        }

      
        [Test]
        public void YearsOfExperienceOutOfRange()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Employee(
                    "Anna", "Smith", "anna@example.com", "123456789",
                    12345678901, _validHireDate,
                    yearsOfExperience: 50   ,
                    services: _services   
                ));

            Assert.That(ex.Message,
                Is.EqualTo("Years of experience should be in the range of 0 to 40"));
        }
        
        [Test]
        public void Employees_Extent_Should_Be_ReadOnly()
        {
            var emp = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5 ,
                services: _services);

            var extent = Employee.Employees;
            
            Assert.That(extent, Is.AssignableTo<IReadOnlyList<Employee>>());
            
            Assert.Throws<NotSupportedException>(() =>
            {
                ((IList<Employee>)extent).Add(emp);
            });
        }
        
        [Test]
        public void Changing_Property_Should_Update_Employee_In_Extent()
        {
            var emp = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5,
                services: _services);
            
            emp.YearsOfExperience = 10;
            
            var stored = Employee.Employees[0];
            
            Assert.That(stored.YearsOfExperience, Is.EqualTo(10));
        }
        private Customer CreateCustomer(string name = "Test")
        {
            return new Customer(
                name,
                "Customer",
                $"{name.ToLower()}@example.com",
                "123456789",
                new DateTime(1990, 1, 1));
        }

        private Booking CreateBooking(Employee emp, DateTime date)
        {
            var cust = CreateCustomer("Anna");
            var svc = new Service("MassageX", "Test", TimeSpan.FromMinutes(30), 100m, 16);
            return new Booking(cust, svc, emp, date, PaymentMethod.AtTheSPA);
        }

        // Aggregation association Tests

        [Test]
        public void AddBookingEmployeeAssignedTo_Throws_WhenBookingIsNull()
        {
            var emp = new Employee(_name, _surname, _email, _phone, _pesel, _hireDate, _years, _services);

            var ex = Assert.Throws<ArgumentNullException>(() => emp.AddBookingEmployeeAssignedTo(null!));
            Assert.That(ex!.ParamName, Is.EqualTo("booking"));
        }

        

        [Test]
        public void AddBookingEmployeeAssignedTo_AddsBooking_AndSetsReverseConnection()
        {
            var emp1 = new Employee(_name, _surname, _email, _phone, _pesel, _hireDate, _years, _services);

            
            var tempEmp = new Employee("Temp", "E", "temp@example.com", "999999999",
                55555555555, DateTime.Today.AddYears(-2), 2, _services);

            var booking = CreateBooking(tempEmp, DateTime.Today.AddDays(1));

            
            booking.RemoveEmployee();

            
            emp1.AddBookingEmployeeAssignedTo(booking);

            Assert.That(emp1.AssignedTo, Does.Contain(booking));
            Assert.That(booking.Employee, Is.EqualTo(emp1));
        }

        

        [Test]
        public void RemoveBookingEmployeeAssignedTo_Throws_WhenBookingIsNull()
        {
            var emp = new Employee(_name, _surname, _email, _phone, _pesel, _hireDate, _years, _services);

            var ex = Assert.Throws<ArgumentNullException>(() => emp.RemoveBookingEmployeeAssignedTo(null!));
            Assert.That(ex!.ParamName, Is.EqualTo("booking"));
        }

        

        [Test]
        public void RemoveBookingEmployeeAssignedTo_RemovesBooking_AndReverseConnection()
        {
            var emp = new Employee(_name, _surname, _email, _phone, _pesel, _hireDate, _years, _services);

            var booking = CreateBooking(emp, DateTime.Today.AddDays(2));
       
            Assert.That(emp.AssignedTo, Does.Contain(booking));
            Assert.That(booking.Employee, Is.EqualTo(emp));

            
            emp.RemoveBookingEmployeeAssignedTo(booking);

            Assert.That(emp.AssignedTo, Does.Not.Contain(booking));
            Assert.That(booking.Employee, Is.Null);
        }
    

        
        //Employee provides Service association (basic) tests
        [Test]
        public void EmployeeConstructor_ShouldRequireAtLeastOneService()
        {
            Assert.Throws<ArgumentException>(() => new Employee("John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, Enumerable.Empty<Service>()));
        }
        
        [Test]
        public void EmployeeConstructor_ShouldCreateReverseAssociations()
        {
            var emp = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, _services);

            Assert.That(emp.ProvidesServices, Contains.Item(_s1));
            Assert.That(_s1.ProvidedBy, Contains.Item(emp));
        }
        
        [Test]
        public void AddEmployeeServiceProvidedBy_ShouldCreateReverseAssociation()
        {
            var emp = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, _services);
            
            var s2 = new Service("Mask", "face mask", TimeSpan.FromMinutes(60), 200m, 16);

            s2.AddEmployeeServiceProvidedBy(emp);

            Assert.That(s2.ProvidedBy, Contains.Item(emp));
            Assert.That(emp.ProvidesServices, Contains.Item(s2));
        }
        
        [Test]
        public void AddServiceToEmployee_ShouldCreateReverseAssociation()
        {
            var emp = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, _services);
            
            var s2 = new Service("Mask", "face mask", TimeSpan.FromMinutes(60), 200m, 16);

            emp.AddServiceToEmployee(s2);

            Assert.That(emp.ProvidesServices, Contains.Item(s2));
            Assert.That(s2.ProvidedBy, Contains.Item(emp));
        }
        
        [Test]
        public void AddEmployeeServiceProvidedBy_ShouldAvoidDuplicates()
        {
            var emp = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, _services);
            
            var s2 = new Service("Mask", "face mask", TimeSpan.FromMinutes(60), 200m, 16);

            s2.AddEmployeeServiceProvidedBy(emp);
            s2.AddEmployeeServiceProvidedBy(emp);

            Assert.That(s2.ProvidedBy.Count, Is.EqualTo(1));
            Assert.That(emp.ProvidesServices.Count, Is.EqualTo(2));
        }
        
        [Test]
        public void RemoveEmployeeServiceProvidedBy_ShouldRemoveReverseAssociation()
        {
            var emp = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, _services);
            
            var s2 = new Service("Mask", "face mask", TimeSpan.FromMinutes(60), 200m, 16);

            s2.AddEmployeeServiceProvidedBy(emp);
            s2.RevomeEmployeeServiceProvidedBy(emp);

            Assert.That(s2.ProvidedBy, Does.Not.Contain(emp));
            Assert.That(emp.ProvidesServices, Does.Not.Contain(s2));
        }
        
        [Test]
        public void RemoveServiceFromEmployee_ShouldRemoveReverseAssociation()
        {
            var emp = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, _services);
            
            var s2 = new Service("Mask", "face mask", TimeSpan.FromMinutes(60), 200m, 16);
            
            emp.AddServiceToEmployee(s2);
            emp.RemoveServiceFromEmployee(s2);

            Assert.That(emp.ProvidesServices, Does.Not.Contain(s2));
            Assert.That(s2.ProvidedBy, Does.Not.Contain(emp));
        }
        
        [Test]
        public void RemoveServiceFromEmployee_ShouldThrow_WhenRemovingLastService()
        {
            var emp = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, _services);

            Assert.Throws<InvalidOperationException>(() => emp.RemoveServiceFromEmployee(_s1));
        }

        [Test]
        public void AddEmployeeServiceProvidedBy_ShouldAllowServiceToHaveZeroEmployees()
        {
            var s2 = new Service("Mask", "face mask", TimeSpan.FromMinutes(60), 200m, 16);

            Assert.That(s2.ProvidedBy.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void ReverseCreation_ShouldNotCauseInfiniteRecursion()
        {
            var emp = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, _services);

            // If there was infinite recursion, test would never end
            _s1.AddEmployeeServiceProvidedBy(emp);

            Assert.Pass(); // If we reach here, no infinite recursion happened
        }
        
        
        [Test]
        public void ChainedAssociations_ShouldAllBeConsistent()
        {
            var emp = new Employee(
                "John", "Doe", "john@test.com", "+48123456789",
                12345678901, DateTime.Today.AddYears(-1), 5, _services);
            
            var s2 = new Service("Mask", "face mask", TimeSpan.FromMinutes(60), 200m, 16);

            emp.AddServiceToEmployee(s2);

            Assert.That(emp.ProvidesServices, Contains.Item(_s1));
            Assert.That(emp.ProvidesServices, Contains.Item(s2));

            Assert.That(_s1.ProvidedBy, Contains.Item(emp));
            Assert.That(s2.ProvidedBy, Contains.Item(emp));
        }
    }
}
