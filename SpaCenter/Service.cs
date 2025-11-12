public class Service
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Duration { get; set; }
    public decimal Price { get; set; }
    public int MinimalAge { get; set; }

    public Service(string name, string description, double duration, decimal price, int minimalAge)
    {
        Name = name;
        Description = description;
        Duration = duration;
        Price = price;
        MinimalAge = minimalAge;
    }

    public static void ViewServices(List<Service> services)
    {
        Console.WriteLine("Available Services:");
        foreach (var s in services)
        {
            Console.WriteLine($"- {s.Name}: {s.Description}, {s.Duration} min, ${s.Price}, min age {s.MinimalAge}");
        }
    }
}