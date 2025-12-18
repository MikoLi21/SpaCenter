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
        private Employee _emp1 = null!;
        private Employee _emp2 = null!;
        private SpaPerson _person1;
        private SpaPerson _person2;
        private Service _svc = null!;
        private List<Service> _services = null!;
        [SetUp]
        public void SetUp()
        {
            _address = new Address("Main Street", 10, "Warsaw", "00-001", "Poland");

            Branch.LoadExtent(null);
            Room.LoadExtent(null);
            
            _svc = new Service("Massage", "Relaxing massage", TimeSpan.FromMinutes(60), 200m, 16);
            _services = new List<Service> { _svc };
            
            _person1 = new SpaPerson("Eva", "Kowalska", "eva@example.com", "999888777");
            _person2 = new SpaPerson("E2", "B", "e2@example.com", "222222222");
            _person1.AssignToEmployee(12345678901, DateTime.Today.AddYears(-1), 4m, _services,
                roles: EmployeeRole.Receptionist,
                languages: new List<string> { "English" });
            _person2.AssignToEmployee(22222222222, DateTime.Today.AddYears(-3), 5m, _services,
                roles: EmployeeRole.Receptionist,
                languages: new List<string> { "English" });
            _emp1 = (Employee)_person1.Empl;
            _emp2 = (Employee)_person2.Empl;
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

        [Test]
        public void Branches_Extent_Is_ReadOnly()
        {
            Branch.LoadExtent(null);

            var address = new Address("Main", 1, "City", "11-111", "Country");
            var branch = new Branch("Branch A", address, new List<string> { "+48123456789" });

            var extent = Branch.Branches;

            Assert.That(extent, Is.AssignableTo<IReadOnlyList<Branch>>());

            Assert.Throws<NotSupportedException>(() =>
            {
                ((IList<Branch>)extent).Add(branch);
            });
        }

        [Test]
        public void Modifying_Property_Updates_Extent_Object()
        {
            Branch.LoadExtent(null);

            var address = new Address("Main", 1, "City", "11-111", "Country");
            var branch = new Branch("Old Name", address, new List<string> { "+48123456789" });

            branch.Name = "New Name";

            var first = Branch.Branches[0];
            Assert.That(first.Name, Is.EqualTo("New Name"));
        }

        // --------------------------------------------------------------
        // Qualified association tests: Branch <-> Employee
        // --------------------------------------------------------------

        [Test]
        public void AddEmployee_CreatesQualifiedAssociation_AndReverseConnection()
        {
            var branch = new Branch("SPA Warsaw", _address, new List<string> { "+48123456789" });
            
            
            branch.AddEmployee(_emp1);

            Assert.That(branch.EmployeesByPesel.ContainsKey(_emp1.Pesel), Is.True);
            Assert.That(branch.EmployeesByPesel[_emp1.Pesel], Is.EqualTo(_emp1));
            Assert.That(_emp1.Branches, Contains.Item(branch));
        }

        [Test]
        public void RemoveEmployee_RemovesFromBothSides_WhenMoreThanOneEmployee()
        {
            var branch = new Branch("SPA Warsaw", _address, new List<string> { "+48123456789" });
            
            branch.AddEmployee(_emp1);
            branch.AddEmployee(_emp2);

            branch.RemoveEmployee(_emp1.Pesel);

            Assert.That(branch.EmployeesByPesel.ContainsKey(_emp1.Pesel), Is.False);
            Assert.That(_emp1.Branches, Does.Not.Contain(branch));
            Assert.That(branch.EmployeesByPesel.ContainsKey(_emp2.Pesel), Is.True);
            Assert.That(_emp2.Branches, Contains.Item(branch));
        }

        [Test]
        public void RemoveEmployee_Throws_WhenRemovingLastEmployeeFromBranch()
        {
            var branch = new Branch("SPA Warsaw", _address, new List<string> { "+48123456789" });

            branch.AddEmployee(_emp1);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                branch.RemoveEmployee(_emp1.Pesel));

            Assert.That(ex!.Message, Is.EqualTo("Branch must have at least one employee"));
        }

        [Test]
        public void UpdateEmployeePesel_ChangesDictionaryKey_AndKeepsReverse()
        {
            var branch = new Branch("SPA Warsaw", _address, new List<string> { "+48123456789" });
            

            long oldPesel = 12345678901;
            long newPesel = 98765432109;


            branch.AddEmployee(_emp1);

            branch.UpdateEmployeePesel(oldPesel, newPesel);

            Assert.That(branch.GetEmployeeByPesel(oldPesel), Is.Null);
            Assert.That(branch.GetEmployeeByPesel(newPesel), Is.EqualTo(_emp1));
            Assert.That(_emp1.Pesel, Is.EqualTo(newPesel));
        }

        [Test]
        public void AddEmployee_Throws_WhenPeselAlreadyUsedInBranch()
        {
            var branch = new Branch("SPA Warsaw", _address, new List<string> { "+48123456789" });

            long pesel = 12345678901;
            
            branch.AddEmployee(_emp1);
            
            var person3 = new SpaPerson("John", "Black", "john7876@gmail.com", "+48777111777");
            person3.AssignToEmployee(12345678901, new DateTime(2024, 05, 06), 5.5m, _services,
                roles: EmployeeRole.SaunaSupervisor,
                firstAidCertification: "First Aid Level 1",
                leaveDate: new DateTime(2025, 01, 01));
            var emp2 = (Employee)person3.Empl;

            var ex = Assert.Throws<ArgumentException>(() =>
                branch.AddEmployee(emp2));

            Assert.That(ex!.Message, Is.EqualTo("Employee with same PESEL already assigned to this branch"));
        }

        [Test]
        public void AddEmployee_Throws_WhenEmployeeAlreadyAssignedToAnotherBranch()
        {
            var branch1 = new Branch("SPA Warsaw", _address, new List<string> { "+48123456789" });
            var branch2 = new Branch("SPA Krakow", _address, new List<string> { "+48123456780" });
            
            branch1.AddEmployee(_emp1);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                branch2.AddEmployee(_emp1));

            Assert.That(ex!.Message, Is.EqualTo("Employee already assigned to a different branch"));
        }

        // --------------------------------------------------------------
        // Branch <-> Room association & composition tests
        // --------------------------------------------------------------

        [Test]
        public void RemoveRoom_RemovesAssociation_AndDeletesRoomFromExtent()
        {
            var branch = new Branch("SPA Warsaw", _address, new List<string> { "+48123456789" });
            var room = new Room(101, "Massage", 22.0, 50.0, branch);

            branch.RemoveRoom(room);

            Assert.That(branch.Rooms, Does.Not.Contain(room));

            Assert.That(Room.Rooms, Does.Not.Contain(room));

            Assert.That(room.Branch, Is.Null);
        }

        [Test]
        public void DeleteBranch_DeletesAllItsRoomsFromExtent()
        {
            var branch = new Branch("SPA Warsaw", _address, new List<string> { "+48123456789" });
            var room1 = new Room(101, "Massage", 22.0, 50.0, branch);
            var room2 = new Room(102, "Sauna", 25.0, 40.0, branch);

            branch.DeleteBranch();

            Assert.That(Branch.Branches, Does.Not.Contain(branch));

            Assert.That(Room.Rooms, Is.Empty);

            Assert.That(branch.Rooms, Is.Empty);
        }

        [Test]
        public void AddRoom_Null_ThrowsArgumentNullException()
        {
            var branch = new Branch("SPA Warsaw", _address, new List<string> { "+48123456789" });

            Assert.Throws<ArgumentNullException>(() => branch.AddRoom(null!));
        }
        
         [Test]
        public void AddRoom_WhenRoomAlreadyAssignedToSameBranch_DoesNotDuplicate()
        {
            var branch = new Branch("SPA Warsaw", _address, new List<string> { "+48123456789" });
            var room = new Room(101, "Massage", 22.0, 50.0, branch);

            var roomsBefore = new List<Room>(branch.Rooms);

            branch.AddRoom(room);

            var roomsAfter = new List<Room>(branch.Rooms);

            Assert.That(roomsAfter.Count, Is.EqualTo(roomsBefore.Count));
            Assert.That(roomsAfter, Is.EquivalentTo(roomsBefore));

            Assert.That(room.Branch, Is.SameAs(branch));
        }

        [Test]
        public void AddRoom_WhenRoomAlreadyAssignedToAnotherBranch_ThrowsAndKeepsOriginalAssociation()
        {
            var branch1 = new Branch("SPA Warsaw", _address, new List<string> { "+48123456789" });
            var branch2 = new Branch("SPA Krakow", _address, new List<string> { "+48123456780" });

            var room = new Room(101, "Massage", 22.0, 50.0, branch1);

            var ex = Assert.Throws<InvalidOperationException>(() => branch2.AddRoom(room));
            Assert.That(ex!.Message, Does.Contain("Room already belongs to a different branch"));

            Assert.That(room.Branch, Is.SameAs(branch1));
            Assert.That(branch1.Rooms, Does.Contain(room));
            Assert.That(branch2.Rooms, Does.Not.Contain(room));
        }
    }
}
