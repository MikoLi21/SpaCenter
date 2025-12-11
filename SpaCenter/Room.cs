using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpaCenter;

[Serializable]
public class Room
{
    private static List<Room> rooms_List = new List<Room>();
    public static IReadOnlyList<Room> Rooms => rooms_List.AsReadOnly();

    public Branch Branch { get; private set; }

    private int _roomNumber;
    public int RoomNumber
    {
        get => _roomNumber;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Room number must be greater than 0");
            _roomNumber = value;
        }
    }

    private string _type = null!;
    public string Type
    {
        get => _type;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("Room type cannot be empty");
            _type = value;
        }
    }

    private double _temperature;
    public double Temperature
    {
        get => _temperature;
        set
        {
            if (value < 0)
                throw new ArgumentException("Temperature can't be below 0°C");
            _temperature = value;
        }
    }

    private double _humidityLevel;
    public double HumidityLevel
    {
        get => _humidityLevel;
        set
        {
            if (value < 0 || value > 100)
                throw new ArgumentException("Humidity level must be between 0% and 100%");
            _humidityLevel = value;
        }
    }
    
    internal Room(int roomNumber, string type, double temperature, double humidityLevel, Branch branch)
    {
        if (branch == null)
            throw new ArgumentNullException("Room must belong to a branch");

        RoomNumber = roomNumber;
        Type = type;
        Temperature = temperature;
        HumidityLevel = humidityLevel;

        Branch = branch;
        addRoom(this);
        branch.AddRoomInternal(this);
    }

    private static void addRoom(Room room)
    {
        if (room == null)
            throw new ArgumentException("Room cannot be null");

        rooms_List.Add(room);
    }

    public static void LoadExtent(IEnumerable<Room>? list)
    {
        rooms_List.Clear();
        if (list == null) return;
        rooms_List.AddRange(list);
    }

    internal void RemoveBranchReverse()
    {
        Branch = null!;
    }

    internal static void DeleteRoom(Room room)
    {
        rooms_List.Remove(room);
    }
}
