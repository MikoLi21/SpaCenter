using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SpaCenter;

[Serializable]
public class Branch
{
    // -----------------------
    // Branch extent
    // -----------------------
    private static List<Branch> branches_List = new List<Branch>();
    public static IReadOnlyList<Branch> Branches => branches_List.AsReadOnly();

    // Composition: Branch 1..* Room
    private readonly HashSet<Room> _rooms = new HashSet<Room>();
    public IEnumerable<Room> Rooms => _rooms.ToHashSet();

    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("Name can't be empty");
            _name = value;
        }
    }

    public Address Address { get; private set; }

    public List<string> PhoneNumbers { get; set; } = new();

    private static TimeSpan _openingTime = new TimeSpan(9, 0, 0);
    public static TimeSpan OpeningTime
    {
        get => _openingTime;
        set => _openingTime = value;
    }

    private static TimeSpan _closingTime = new TimeSpan(21, 0, 0);
    public static TimeSpan ClosingTime
    {
        get => _closingTime;
        set => _closingTime = value;
    }

    private static readonly Regex PhoneRegex = new(@"^(?:\+48)? ?\d{9}$");

    // -----------------------------------------------------------------
    // Base constructor: VALIDATES ATTRIBUTES ONLY, does NOT handle rooms
    // Made private so normal code cannot create a Branch without Rooms
    // -----------------------------------------------------------------
    private Branch(string name, Address address, List<string> phoneNumbers)
    {
        Name = name;
        Address = address ?? throw new ArgumentNullException(nameof(address));

        if (phoneNumbers == null)
            throw new ArgumentNullException("Phone numbers can't be empty");

        foreach (var number in phoneNumbers)
        {
            if (string.IsNullOrEmpty(number))
                throw new ArgumentNullException("Phone number can't be empty");

            if (!PhoneRegex.IsMatch(number))
                throw new ArgumentException("Invalid phone number");

            PhoneNumbers.Add(number);
        }

        if (PhoneNumbers.Count == 0)
            throw new ArgumentException("At least one phone number is required");

        addBranch(this);
    }

    private static void addBranch(Branch branch)
    {
        if (branch == null)
            throw new ArgumentException("Branch cannot be null");

        branches_List.Add(branch);
    }

    public static void LoadExtent(IEnumerable<Branch>? list)
    {
        branches_List.Clear();
        if (list == null) return;
        branches_List.AddRange(list);
    }

    public static Branch CreateBranchWithRooms(
        string name,
        Address address,
        List<string> phoneNumbers,
        IEnumerable<(int roomNumber, string type, double temperature, double humidityLevel)> roomsData)
    {
        if (roomsData == null)
            throw new ArgumentNullException(nameof(roomsData));

        var roomList = roomsData.ToList();
        if (roomList.Count == 0)
            throw new ArgumentException("Branch must have at least one room at creation.");

        var branch = new Branch(name, address, phoneNumbers);

        foreach (var r in roomList)
        {
            new Room(r.roomNumber, r.type, r.temperature, r.humidityLevel, branch);
        }

        return branch;
    }

    public Room AddRoom(int roomNumber, string type, double temperature, double humidityLevel)
    {
        var room = new Room(roomNumber, type, temperature, humidityLevel, this);
        return room;
    }

    internal void AddRoomInternal(Room room)
    {
        if (room == null)
            throw new ArgumentNullException(nameof(room));

        if (room.Branch != null && room.Branch != this)
            throw new InvalidOperationException("Room already belongs to a different branch");

        _rooms.Add(room);
    }

    public void RemoveRoom(Room room)
    {
        if (room == null) return;

        if (_rooms.Count <= 1 && _rooms.Contains(room))
            throw new InvalidOperationException("Branch must have at least one room.");

        RemoveRoomInternal(room);
    }

    private void RemoveRoomInternal(Room room)
    {
        if (room == null) return;

        if (_rooms.Remove(room))
        {
            room.RemoveBranchReverse();
            Room.DeleteRoom(room);
        }
    }

    public void DeleteBranch()
    {
        foreach (var room in _rooms.ToList())
        {
            RemoveRoomInternal(room);
        }

        branches_List.Remove(this);
    }
}
