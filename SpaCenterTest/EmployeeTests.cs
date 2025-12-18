using NUnit.Framework;
using SpaCenter;
using System;
using System.Collections.Generic;
using System.Linq;

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

        // Role data (must satisfy validation)
        private readonly List<string> _receptionistLanguages = new() { "English" };
        private readonly List<string> _therapistCertifications = new() { "Massage certificate" };
        private const string _firstAid = "First Aid Level 1";
        private const string _nailLevel = "Level A";

        [SetUp]
        public void Setup()
        {
            // IMPORTANT: clear extents before every test (static list)
            Employee.LoadExtent(new List<Employee>());
            Junior.LoadExtent(new List<Junior>());
            Mid.LoadExtent(new List<Mid>());
            Senior.LoadExtent(new List<Senior>());

            _name = "Eva";
            _surname = "Kowalska";
            _email = "eva@example.com";
            _phone = "999888777";
            _pesel = 12345678901;
            _hireDate = DateTime.Today.AddYears(-5);
            _years = 4;

            _validHireDate = DateTime.Today.AddYears(-1);

            _services.Clear();
            _s1 = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            _services.Add(_s1);
        }

        [Test]
        public void Constructor_SetsNameCorrectly()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            Assert.That(person.Name, Is.EqualTo(_name));
        }

        [Test]
        public void Constructor_SetsSurnameCorrectly()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            Assert.That(person.Surname, Is.EqualTo(_surname));
        }

        [Test]
        public void Constructor_SetsEmailCorrectly()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            Assert.That(person.Email, Is.EqualTo(_email));
        }

        [Test]
        public void Constructor_SetsPhoneCorrectly()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            Assert.That(person.PhoneNumber, Is.EqualTo(_phone));
        }

        [Test]
        public void Constructor_SetsPeselCorrectly()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            Assert.That(person.Empl.Pesel, Is.EqualTo(_pesel));
        }

        [Test]
        public void Constructor_SetsHireDateCorrectly()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            Assert.That(person.Empl.HireDate, Is.EqualTo(_hireDate));
        }

        [Test]
        public void Constructor_SetsYearsOfExperienceCorrectly()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            Assert.That(person.Empl.YearsOfExperience, Is.EqualTo(_years));
        }

        [Test]
        public void InvalidPesel()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            var ex = Assert.Throws<ArgumentException>(() =>
                person.AssignToEmployee(
                    pesel: 1234,
                    hireDate: _hireDate,
                    yearsOfExperience: _years,
                    services: _services,
                    roles: EmployeeRole.Receptionist,
                    languages: _receptionistLanguages));

            Assert.That(ex.Message, Is.EqualTo("Invalid pesel number"));
        }

        [Test]
        public void HireDateInFuture()
        {
            var futureHire = DateTime.Today.AddDays(1);

            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            var ex = Assert.Throws<ArgumentException>(() =>
                person.AssignToEmployee(
                    pesel: _pesel,
                    hireDate: futureHire,
                    yearsOfExperience: _years,
                    services: _services,
                    roles: EmployeeRole.Receptionist,
                    languages: _receptionistLanguages));

            Assert.That(ex.Message, Is.EqualTo("Hire date can't be in the future"));
        }

        [Test]
        public void LeaveDateBeforeHireDate()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );

            var invalidLeave = DateTime.Today.AddYears(-10);
            ;

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                person.AssignToEmployee(
                    pesel: _pesel,
                    hireDate: _hireDate,
                    leaveDate: invalidLeave,
                    yearsOfExperience: _years,
                    services: _services,
                    roles: EmployeeRole.Receptionist,
                    languages: _receptionistLanguages);
            });

            Assert.That(ex.Message, Is.EqualTo("Leave date can't be before hire date"));
        }

        [Test]
        public void LeaveDateInFuture()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            var futureLeave = DateTime.Today.AddDays(1);

            var ex = Assert.Throws<ArgumentException>(() => { person.Empl.LeaveDate = futureLeave; });

            Assert.That(ex.Message, Is.EqualTo("Leave date can't be in the future"));
        }

        [Test]
        public void YearsOfExperienceOutOfRange()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            var ex = Assert.Throws<ArgumentException>(() =>
                person.AssignToEmployee(
                    pesel: _pesel,
                    hireDate: _hireDate,
                    yearsOfExperience: 50,
                    services: _services,
                    roles: EmployeeRole.Receptionist,
                    languages: _receptionistLanguages));

            Assert.That(ex.Message,
                Is.EqualTo("Years of experience should be in the range of 0 to 40"));
        }

        [Test]
        public void Employees_Extent_Should_Be_ReadOnly()
        {
            var extent = Employee.Employees;

            Assert.That(extent, Is.AssignableTo<IReadOnlyList<Employee>>());

            Assert.Throws<NotSupportedException>(() => { ((ICollection<Employee>)extent).Add((Employee)null!); });
        }

        [Test]
        public void Changing_Property_Should_Update_Employee_In_Extent()
        {
            Employee.LoadExtent(new List<Employee>());

            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);


            person.Empl.YearsOfExperience = 10m;


            Assert.That(person.Empl.YearsOfExperience, Is.EqualTo(10m));


            var stored = Employee.Employees.Single(e => ReferenceEquals(e, person.Empl));
            Assert.That(stored.YearsOfExperience, Is.EqualTo(10m));
        }

        private Customer CreateCustomer(string name = "Test")
        {
            var person = new SpaPerson(
                name: name,
                surname: _surname,
                email: _email,
                phone: _phone
            );

            person.AssignToCustomer(new DateTime(1998, 5, 20));

            return person.Cstmr as Customer
                   ?? throw new InvalidOperationException("Customer role was not assigned");
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
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            var ex = Assert.Throws<ArgumentNullException>(() => person.Empl.AddBookingEmployeeAssignedTo(null!));
            Assert.That(ex!.ParamName, Is.EqualTo("booking"));
        }

        [Test]
        public void AddBookingEmployeeAssignedTo_AddsBooking_AndSetsReverseConnection()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            var person1 = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person1.AssignToEmployee(
                pesel: 67892345678,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            var booking = CreateBooking((Employee)person1.Empl, DateTime.Today.AddDays(1));
            booking.RemoveEmployee();

            var employee = (Employee)person.Empl;
            employee.AddBookingEmployeeAssignedTo(booking);

            Assert.That(employee.AssignedTo, Does.Contain(booking));
            Assert.That(booking.Employee, Is.EqualTo(employee));
        }

        [Test]
        public void RemoveBookingEmployeeAssignedTo_Throws_WhenBookingIsNull()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            var employee = (Employee)person.Empl;
            var ex = Assert.Throws<ArgumentNullException>(() => employee.RemoveBookingEmployeeAssignedTo(null!));
            Assert.That(ex!.ParamName, Is.EqualTo("booking"));
        }

        [Test]
        public void RemoveBookingEmployeeAssignedTo_RemovesBooking_AndReverseConnection()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);
            var employee = (Employee)person.Empl;

            var booking = CreateBooking(employee, DateTime.Today.AddDays(2));

            Assert.That(employee.AssignedTo, Does.Contain(booking));
            Assert.That(booking.Employee, Is.EqualTo(employee));

            employee.RemoveBookingEmployeeAssignedTo(booking);

            Assert.That(employee.AssignedTo, Does.Not.Contain(booking));
            Assert.That(booking.Employee, Is.Null);
        }

        //Employee provides Service association (basic) tests
        [Test]
        public void EmployeeConstructor_ShouldRequireAtLeastOneService()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );

            Assert.Throws<ArgumentException>(() => person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: Enumerable.Empty<Service>(),
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages));
        }

        [Test]
        public void EmployeeConstructor_ShouldCreateReverseAssociations()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            var employee = (Employee)person.Empl;
            Assert.That(employee.ProvidesServices, Contains.Item(_s1));
            Assert.That(_s1.ProvidedBy, Contains.Item(employee));
        }

        [Test]
        public void AddEmployeeServiceProvidedBy_ShouldCreateReverseAssociation()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            var employee = (Employee)person.Empl;

            var s2 = new Service("Mask", "face mask", TimeSpan.FromMinutes(60), 200m, 16);

            s2.AddEmployeeServiceProvidedBy(employee);

            Assert.That(s2.ProvidedBy, Contains.Item(employee));
            Assert.That(employee.ProvidesServices, Contains.Item(s2));
        }

        [Test]
        public void AddServiceToEmployee_ShouldCreateReverseAssociation()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            var employee = (Employee)person.Empl;

            var s2 = new Service("Mask", "face mask", TimeSpan.FromMinutes(60), 200m, 16);

            employee.AddServiceToEmployee(s2);

            Assert.That(employee.ProvidesServices, Contains.Item(s2));
            Assert.That(s2.ProvidedBy, Contains.Item(employee));
        }

        [Test]
        public void AddEmployeeServiceProvidedBy_ShouldAvoidDuplicates()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            var employee = (Employee)person.Empl;

            var s2 = new Service("Mask", "face mask", TimeSpan.FromMinutes(60), 200m, 16);

            s2.AddEmployeeServiceProvidedBy(employee);
            s2.AddEmployeeServiceProvidedBy(employee);

            Assert.That(s2.ProvidedBy.Count, Is.EqualTo(1));
            Assert.That(employee.ProvidesServices.Count, Is.EqualTo(2));
        }

        [Test]
        public void RemoveEmployeeServiceProvidedBy_ShouldRemoveReverseAssociation()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            var employee = (Employee)person.Empl;

            var s2 = new Service("Mask", "face mask", TimeSpan.FromMinutes(60), 200m, 16);

            s2.AddEmployeeServiceProvidedBy(employee);
            s2.RevomeEmployeeServiceProvidedBy(employee);

            Assert.That(s2.ProvidedBy, Does.Not.Contain(employee));
            Assert.That(employee.ProvidesServices, Does.Not.Contain(s2));
        }

        [Test]
        public void RemoveServiceFromEmployee_ShouldRemoveReverseAssociation()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            var emp = (Employee)person.Empl;

            var s2 = new Service("Mask", "face mask", TimeSpan.FromMinutes(60), 200m, 16);

            emp.AddServiceToEmployee(s2);
            emp.RemoveServiceFromEmployee(s2);

            Assert.That(emp.ProvidesServices, Does.Not.Contain(s2));
            Assert.That(s2.ProvidedBy, Does.Not.Contain(emp));
        }

        [Test]
        public void RemoveServiceFromEmployee_ShouldThrow_WhenRemovingLastService()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            var emp = (Employee)person.Empl;

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
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            var emp = (Employee)person.Empl;

            _s1.AddEmployeeServiceProvidedBy(emp);

            Assert.Pass();
        }

        [Test]
        public void ChainedAssociations_ShouldAllBeConsistent()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: _receptionistLanguages);

            var emp = (Employee)person.Empl;

            var s2 = new Service("Mask", "face mask", TimeSpan.FromMinutes(60), 200m, 16);

            emp.AddServiceToEmployee(s2);

            Assert.That(emp.ProvidesServices, Contains.Item(_s1));
            Assert.That(emp.ProvidesServices, Contains.Item(s2));

            Assert.That(_s1.ProvidedBy, Contains.Item(emp));
            Assert.That(s2.ProvidedBy, Contains.Item(emp));
        }

        // ===================== Overlapping / Roles (Flags) tests =====================

        [Test]
        public void Constructor_Allows_TwoRoles_ReceptionistAndTherapist_WithRequiredData()
        {
            var languages = new List<string> { "English", "Polish" };
            var certs = new List<string> { "Massage certificate" };

            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist | EmployeeRole.Therapist,
                languages: languages,
                certifications: certs);

            var emp = (Employee)person.Empl;

            Assert.That(emp.Roles.HasFlag(EmployeeRole.Receptionist), Is.True);
            Assert.That(emp.Roles.HasFlag(EmployeeRole.Therapist), Is.True);

            Assert.That(emp.Languages, Is.EquivalentTo(languages));
            Assert.That(emp.Certifications, Is.EquivalentTo(certs));
        }

        [Test]
        public void Constructor_ShouldThrow_When_ReceptionistRoleButNoLanguages()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            var ex = Assert.Throws<ArgumentException>(() =>
                person.AssignToEmployee(
                    pesel: _pesel,
                    hireDate: _hireDate,
                    yearsOfExperience: _years,
                    services: _services,
                    roles: EmployeeRole.Receptionist,
                    languages: null));
            var emp = (Employee)person.Empl;

            Assert.That(ex!.Message, Is.EqualTo("Receptionist must have at least one language"));
        }

        [Test]
        public void Constructor_ShouldThrow_When_TherapistRoleButNoCertifications()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            var ex = Assert.Throws<ArgumentException>(() =>
                person.AssignToEmployee(
                    pesel: _pesel,
                    hireDate: _hireDate,
                    yearsOfExperience: _years,
                    services: _services,
                    roles: EmployeeRole.Therapist,
                    certifications: null));
            var emp = (Employee)person.Empl;

            Assert.That(ex!.Message, Is.EqualTo("Therapist must have at least one certification"));
        }

        [Test]
        public void Constructor_ShouldThrow_When_SaunaSupervisorRoleButNoFirstAid()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            var ex = Assert.Throws<ArgumentException>(() =>
                person.AssignToEmployee(
                    pesel: _pesel,
                    hireDate: _hireDate,
                    yearsOfExperience: _years,
                    services: _services,
                    roles: EmployeeRole.SaunaSupervisor,
                    firstAidCertification: null));
            var emp = (Employee)person.Empl;

            Assert.That(ex!.Message, Is.EqualTo("SaunaSupervisor must have first aid certification"));
        }

        [Test]
        public void Constructor_ShouldThrow_When_NailTechnicianRoleButNoCertificationLevel()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            var ex = Assert.Throws<ArgumentException>(() =>
                person.AssignToEmployee(
                    pesel: _pesel,
                    hireDate: _hireDate,
                    yearsOfExperience: _years,
                    services: _services,
                    roles: EmployeeRole.NailTechnician,
                    certificationLevel: null));
            var emp = (Employee)person.Empl;

            Assert.That(ex!.Message, Is.EqualTo("NailTechnician must have certification level"));
        }

        [Test]
        public void Constructor_ShouldThrow_When_LanguagesProvidedButNotReceptionist()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            var ex = Assert.Throws<ArgumentException>(() =>
                person.AssignToEmployee(
                    pesel: _pesel,
                    hireDate: _hireDate,
                    yearsOfExperience: _years,
                    services: _services,
                    roles: EmployeeRole.Therapist,
                    languages: new List<string> { "English" },
                    certifications: new List<string> { "Massage certificate" }));
            var emp = (Employee)person.Empl;


            Assert.That(ex!.Message, Is.EqualTo("Languages allowed only for Receptionist role"));
        }

        [Test]
        public void Constructor_ShouldThrow_When_CertificationsProvidedButNotTherapist()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            var ex = Assert.Throws<ArgumentException>(() =>
                person.AssignToEmployee(
                    pesel: _pesel,
                    hireDate: _hireDate,
                    yearsOfExperience: _years,
                    services: _services,
                    roles: EmployeeRole.Receptionist,
                    languages: new List<string> { "English" },
                    certifications: new List<string> { "Massage certificate" }));
            var emp = (Employee)person.Empl;

            Assert.That(ex!.Message, Is.EqualTo("Certifications allowed only for Therapist role"));
        }

        [Test]
        public void Constructor_ShouldThrow_When_FirstAidProvidedButNotSaunaSupervisor()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            var ex = Assert.Throws<ArgumentException>(() =>
                person.AssignToEmployee(
                    pesel: _pesel,
                    hireDate: _hireDate,
                    yearsOfExperience: _years,
                    services: _services,
                    roles: EmployeeRole.Receptionist,
                    languages: new List<string> { "English" },
                    firstAidCertification: "First Aid Level 1"));
            var emp = (Employee)person.Empl;

            Assert.That(ex!.Message, Is.EqualTo("FirstAidCertification allowed only for SaunaSupervisor role"));
        }

        [Test]
        public void Constructor_ShouldThrow_When_CertificationLevelProvidedButNotNailTechnician()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            var ex = Assert.Throws<ArgumentException>(() =>
                person.AssignToEmployee(
                    pesel: _pesel,
                    hireDate: _hireDate,
                    yearsOfExperience: _years,
                    services: _services,
                    roles: EmployeeRole.Therapist,
                    certifications: new List<string> { "Massage certificate" },
                    certificationLevel: "Level A"));
            var emp = (Employee)person.Empl;

            Assert.That(ex!.Message, Is.EqualTo("CertificationLevel allowed only for NailTechnician role"));
        }

        [Test]
        public void Constructor_ShouldStore_Languages_AsSet_NoDuplicates_AndTrim()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Receptionist,
                languages: new List<string> { " English ", "English", "Polish" });

            var emp = (Employee)person.Empl;

            Assert.That(emp.Languages.Count, Is.EqualTo(2));
            Assert.That(emp.Languages, Does.Contain("English"));
            Assert.That(emp.Languages, Does.Contain("Polish"));
        }

        [Test]
        public void Constructor_ShouldStore_Certifications_AsSet_NoDuplicates_AndTrim()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            person.AssignToEmployee(
                pesel: _pesel,
                hireDate: _hireDate,
                yearsOfExperience: _years,
                services: _services,
                roles: EmployeeRole.Therapist,
                certifications: new List<string> { " Cert1 ", "Cert1", "Cert2" });

            var emp = (Employee)person.Empl;

            Assert.That(emp.Certifications.Count, Is.EqualTo(2));
            Assert.That(emp.Certifications, Does.Contain("Cert1"));
            Assert.That(emp.Certifications, Does.Contain("Cert2"));
        }

        [Test]
        public void Constructor_ShouldThrow_When_LanguageIsWhitespace()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            var ex = Assert.Throws<ArgumentException>(() =>
                person.AssignToEmployee(
                    pesel: _pesel,
                    hireDate: _hireDate,
                    yearsOfExperience: _years,
                    services: _services,
                    roles: EmployeeRole.Receptionist,
                    languages: new List<string> { "English", "   " }));
            var emp = (Employee)person.Empl;

            Assert.That(ex!.Message, Is.EqualTo("Language can't be empty"));
        }

        [Test]
        public void Constructor_ShouldThrow_When_CertificationIsWhitespace()
        {
            var person = new SpaPerson(
                name: _name,
                surname: _surname,
                email: _email,
                phone: _phone
            );
            var ex = Assert.Throws<ArgumentException>(() =>
                person.AssignToEmployee(
                    pesel: _pesel,
                    hireDate: _hireDate,
                    yearsOfExperience: _years,
                    services: _services,
                    roles: EmployeeRole.Therapist,
                    certifications: new List<string> { "Massage certificate", "" }));
            var emp = (Employee)person.Empl;

            Assert.That(ex!.Message, Is.EqualTo("Certification can't be empty"));
        }

       // ===================== Disjoint + Dynamic inheritance (Employee -> Junior/Mid/Senior) =====================
       
       [Test]
       public void AssignToJunior_ShouldSetOnlyJunior_Disjoint()
       {
           var supervisorPerson = new SpaPerson(
               name: "Sup",
               surname: _surname,
               email: "sup@example.com",
               phone: _phone
           );
           supervisorPerson.AssignToEmployee(
               pesel: 11111111111,
               hireDate: _hireDate,
               yearsOfExperience: _years,
               services: _services,
               roles: EmployeeRole.Receptionist,
               languages: _receptionistLanguages
           );
           var supervisorEmp = (Employee)supervisorPerson.Empl;
           supervisorEmp.AssignToMid();
           var supervisorMid = supervisorEmp.Md!;
       
           var person = new SpaPerson(
               name: _name,
               surname: _surname,
               email: _email,
               phone: _phone
           );
           person.AssignToEmployee(
               pesel: _pesel,
               hireDate: _hireDate,
               yearsOfExperience: _years,
               services: _services,
               roles: EmployeeRole.Receptionist,
               languages: _receptionistLanguages
           );
           var emp = (Employee)person.Empl;
       
           emp.AssignToJunior(learningPeriod: 6, mids: new[] { supervisorMid });
       
           Assert.That(emp.Jnr, Is.Not.Null);
           Assert.That(emp.Md, Is.Null);
           Assert.That(emp.Snr, Is.Null);
       
           Assert.That(emp.Jnr!.Emp, Is.EqualTo(emp));
           Assert.That(Junior.Juniors, Does.Contain(emp.Jnr));
       }
       
       [Test]
       public void AssignToMid_ShouldSetOnlyMid_Disjoint()
       {
           var person = new SpaPerson(
               name: _name,
               surname: _surname,
               email: _email,
               phone: _phone
           );
           person.AssignToEmployee(
               pesel: _pesel,
               hireDate: _hireDate,
               yearsOfExperience: _years,
               services: _services,
               roles: EmployeeRole.Receptionist,
               languages: _receptionistLanguages
           );
           var emp = (Employee)person.Empl;
       
           emp.AssignToMid();
       
           Assert.That(emp.Jnr, Is.Null);
           Assert.That(emp.Md, Is.Not.Null);
           Assert.That(emp.Snr, Is.Null);
       
           Assert.That(emp.Md!.Emp, Is.EqualTo(emp));
       }
       
       [Test]
       public void AssignToSenior_ShouldSetOnlySenior_Disjoint()
       {
           var person = new SpaPerson(
               name: _name,
               surname: _surname,
               email: _email,
               phone: _phone
           );
           person.AssignToEmployee(
               pesel: _pesel,
               hireDate: _hireDate,
               yearsOfExperience: _years,
               services: _services,
               roles: EmployeeRole.Receptionist,
               languages: _receptionistLanguages
           );
           var emp = (Employee)person.Empl;
       
           emp.AssignToSenior(1.25m);
       
           Assert.That(emp.Jnr, Is.Null);
           Assert.That(emp.Md, Is.Null);
           Assert.That(emp.Snr, Is.Not.Null);
       
           Assert.That(emp.Snr!.Emp, Is.EqualTo(emp));
       }
       
       [Test]
       public void AssigningLevelTwice_ShouldThrow_DisjointConstraint()
       {
           var supervisorPerson = new SpaPerson(
               name: "Sup",
               surname: _surname,
               email: "sup2@example.com",
               phone: _phone
           );
           supervisorPerson.AssignToEmployee(
               pesel: 22222222222,
               hireDate: _hireDate,
               yearsOfExperience: _years,
               services: _services,
               roles: EmployeeRole.Receptionist,
               languages: _receptionistLanguages
           );
           var supervisorEmp = (Employee)supervisorPerson.Empl;
           supervisorEmp.AssignToMid();
           var supervisorMid = supervisorEmp.Md!;
       
           var person = new SpaPerson(
               name: _name,
               surname: _surname,
               email: _email,
               phone: _phone
           );
           person.AssignToEmployee(
               pesel: _pesel,
               hireDate: _hireDate,
               yearsOfExperience: _years,
               services: _services,
               roles: EmployeeRole.Receptionist,
               languages: _receptionistLanguages
           );
           var emp = (Employee)person.Empl;
       
           emp.AssignToJunior(learningPeriod: 3, mids: new[] { supervisorMid });
       
           var ex = Assert.Throws<InvalidOperationException>(() => emp.AssignToMid());
           Assert.That(ex!.Message, Is.EqualTo("Employee already has a level assigned."));
       }
       
       [Test]
       public void Junior_SwitchToMid_ShouldReplaceState_UpdateExtents_AndCleanupSupervisionLinks()
       {
           var supervisorPerson = new SpaPerson(
               name: "Sup",
               surname: _surname,
               email: "sup3@example.com",
               phone: _phone
           );
           supervisorPerson.AssignToEmployee(
               pesel: 33333333333,
               hireDate: _hireDate,
               yearsOfExperience: _years,
               services: _services,
               roles: EmployeeRole.Receptionist,
               languages: _receptionistLanguages
           );
           var supervisorEmp = (Employee)supervisorPerson.Empl;
           supervisorEmp.AssignToMid();
           var supervisorMid = supervisorEmp.Md!;
       
           var person = new SpaPerson(
               name: _name,
               surname: _surname,
               email: "junior.switch@example.com",
               phone: _phone
           );
           person.AssignToEmployee(
               pesel: 44444444444,
               hireDate: _hireDate,
               yearsOfExperience: _years,
               services: _services,
               roles: EmployeeRole.Receptionist,
               languages: _receptionistLanguages
           );
           var emp = (Employee)person.Empl;
       
           emp.AssignToJunior(learningPeriod: 4, mids: new[] { supervisorMid });
           var junior = emp.Jnr!;
       
           Assert.That(junior.SupervisedBy, Does.Contain(supervisorMid));
           Assert.That(supervisorMid.SupervisedJuniors, Does.Contain(junior));
           Assert.That(Junior.Juniors, Does.Contain(junior));
       
           junior.SwitchToMid();
       
           Assert.That(emp.Jnr, Is.Null);
           Assert.That(emp.Md, Is.Not.Null);
           Assert.That(emp.Snr, Is.Null);
       
           Assert.That(Junior.Juniors, Does.Not.Contain(junior));
       
           Assert.That(supervisorMid.SupervisedJuniors, Does.Not.Contain(junior));
       }
       
       [Test]
       public void Mid_SwitchToSenior_ShouldReplaceState_UpdateExtents_AndCleanupSupervisedJuniorsLinks()
       {
           var person = new SpaPerson(
               name: _name,
               surname: _surname,
               email: "mid.switch@example.com",
               phone: _phone
           );
           person.AssignToEmployee(
               pesel: 55555555555,
               hireDate: _hireDate,
               yearsOfExperience: _years,
               services: _services,
               roles: EmployeeRole.Receptionist,
               languages: _receptionistLanguages
           );
           var emp = (Employee)person.Empl;
       
           emp.AssignToMid();
           var mid = emp.Md!;
           Assert.That(Mid.Mids, Does.Contain(mid));
       
           var juniorPerson = new SpaPerson(
               name: "Junior",
               surname: _surname,
               email: "junior.for.mid@example.com",
               phone: _phone
           );
           juniorPerson.AssignToEmployee(
               pesel: 66666666666,
               hireDate: _hireDate,
               yearsOfExperience: _years,
               services: _services,
               roles: EmployeeRole.Receptionist,
               languages: _receptionistLanguages
           );
           var juniorEmp = (Employee)juniorPerson.Empl;
       
           juniorEmp.AssignToJunior(learningPeriod: 2, mids: new[] { mid });
           var junior = juniorEmp.Jnr!;
       
           Assert.That(mid.SupervisedJuniors, Does.Contain(junior));
           Assert.That(junior.SupervisedBy, Does.Contain(mid));
       
           mid.SwitchToSenior(bonusCoefficient: 1.50m);
       
           Assert.That(emp.Jnr, Is.Null);
           Assert.That(emp.Md, Is.Null);
           Assert.That(emp.Snr, Is.Not.Null);
       
           Assert.That(Mid.Mids, Does.Not.Contain(mid));
       
           Assert.That(junior.SupervisedBy, Does.Not.Contain(mid));
       }
       
       [Test]
       public void OldJuniorObject_AfterPromotion_ShouldNotBeUsable_ForSecondSwitch()
       {
           var supervisorPerson = new SpaPerson(
               name: "Sup",
               surname: _surname,
               email: "sup4@example.com",
               phone: _phone
           );
           supervisorPerson.AssignToEmployee(
               pesel: 77777777777,
               hireDate: _hireDate,
               yearsOfExperience: _years,
               services: _services,
               roles: EmployeeRole.Receptionist,
               languages: _receptionistLanguages
           );
           var supervisorEmp = (Employee)supervisorPerson.Empl;
           supervisorEmp.AssignToMid();
           var supervisorMid = supervisorEmp.Md!;
       
           var person = new SpaPerson(
               name: _name,
               surname: _surname,
               email: "stale.junior@example.com",
               phone: _phone
           );
           person.AssignToEmployee(
               pesel: 88888888888,
               hireDate: _hireDate,
               yearsOfExperience: _years,
               services: _services,
               roles: EmployeeRole.Receptionist,
               languages: _receptionistLanguages
           );
           var emp = (Employee)person.Empl;
       
           emp.AssignToJunior(learningPeriod: 5, mids: new[] { supervisorMid });
           var oldJunior = emp.Jnr!;
       
           oldJunior.SwitchToMid();
       
           Assert.Throws<NullReferenceException>(() => oldJunior.SwitchToMid());
       }
       
       [Test]
       public void OldMidObject_AfterPromotion_ShouldNotBeUsable_ForSecondSwitch()
       {
           var person = new SpaPerson(
               name: _name,
               surname: _surname,
               email: "stale.mid@example.com",
               phone: _phone
           );
           person.AssignToEmployee(
               pesel: 99999999999,
               hireDate: _hireDate,
               yearsOfExperience: _years,
               services: _services,
               roles: EmployeeRole.Receptionist,
               languages: _receptionistLanguages
           );
           var emp = (Employee)person.Empl;
       
           emp.AssignToMid();
           var oldMid = emp.Md!;
       
           oldMid.SwitchToSenior(1.10m);
       
           Assert.Throws<NullReferenceException>(() => oldMid.SwitchToSenior(1.20m));
       }
    }
}