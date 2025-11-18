using System.Text.Json;
using SpaCenter;

namespace SpaCenter.Repository;

public static class PersistenceManager
{
    public static string FilePath { get; set; } = @"C:\Users\Home\RiderProjects\BYT1\SpaCenter\data.json";

    public static void Save()
    {
        try
        {
            var allData = new AllDataWrapper
            {
                Customers = Customer.Customers.ToList(),
                Employees = Employee.Employees.ToList(),
                Services = Service.Services.ToList(),
                Bookings = Booking.Bookings.ToList(),
                Branches = Branch.Branches.ToList()
            };

            var json = JsonSerializer.Serialize(
                allData,
                new JsonSerializerOptions { WriteIndented = true }
            );

            File.WriteAllText(FilePath, json);
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }
    
    public static void Load()
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("File not found");
            }

            var json = File.ReadAllText(FilePath);
            AllDataWrapper? allData = JsonSerializer.Deserialize<AllDataWrapper>(json);

            if (allData != null)
            {
                Customer.LoadExtent(allData.Customers);
                Employee.LoadExtent(allData.Employees);
                Service.LoadExtent(allData.Services);
                Booking.LoadExtent(allData.Bookings);
                Branch.LoadExtent(allData.Branches);
            }
        } catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
        }
    }

    private class AllDataWrapper
    {
        public List<Customer>? Customers { get; set; }
        public List<Employee>? Employees { get; set; }
        public List<Service>? Services { get; set; }
        public List<Booking>? Bookings { get; set; }
        public List<Branch>? Branches { get; set; }
    }
}