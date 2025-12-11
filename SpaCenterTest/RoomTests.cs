using SpaCenter;

namespace SpaCenterTest
{
    [TestFixture]
    public class RoomTests
    {
        private Address _address = null!;

        [SetUp]
        public void SetUp()
        {
            _address = new Address("Main Street", 10, "Warsaw", "00-001", "Poland");

            Branch.LoadExtent(null);
            Room.LoadExtent(null);
        }

        private Branch CreateBranch(string name = "SPA Warsaw")
        {
            var phones = new List<string> { "+48123456789" };
            return new Branch(name, _address, phones);
        }
        
        [Test]
        public void CreatingRoom_AssignsBranch()
        {
            var branch = CreateBranch();

            var room = new Room(101, "Massage", 22.0, 50.0, branch);

            Assert.That(branch.Rooms, Does.Contain(room));

            Assert.That(room.Branch, Is.SameAs(branch));
        }
        
        [Test]
        public void RoomConstructor_NullBranch_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Room(101, "Massage", 22.0, 50.0, null!)
            );

            Assert.That(ex!.Message, Does.Contain("Room must belong to a branch"));
        }
    }
}
