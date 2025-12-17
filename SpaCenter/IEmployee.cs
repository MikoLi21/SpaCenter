namespace SpaCenter;


public interface IEmployee
{
    Person Prsn{ get; }
    long Pesel { get; set; }
    DateTime HireDate { get; set; }
    DateTime? LeaveDate { get; set; }
    decimal YearsOfExperience { get; set; }

    int YearsOfService { get; }
    double AverageServiceMinutes { get; }

    // === Associations ===
    IEnumerable<Service> ProvidesServices { get; }
    IEnumerable<Booking> AssignedTo { get; }
    IEnumerable<Branch> Branches { get; }

    // === Association management ===
    void AddServiceToEmployee(Service service);
    void RemoveServiceFromEmployee(Service service);

    void AddBookingEmployeeAssignedTo(Booking booking);
    void RemoveBookingEmployeeAssignedTo(Booking booking);
}