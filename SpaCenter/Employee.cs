
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SpaCenter;
[Flags]
public enum EmployeeRole
{
    None = 0,
    Receptionist = 1 << 0,
    Therapist = 1 << 1,
    SaunaSupervisor = 1 << 2,
    NailTechnician = 1 << 3
}

[Serializable]
public class Employee : IEmployee
{
    //Employee Container
    private static List<Employee> employees_List = new List<Employee>();
    public static IReadOnlyList<Employee> Employees => employees_List.AsReadOnly();
    
    private HashSet<Service> _providesServices = new HashSet<Service>();
    public IEnumerable<Service> ProvidesServices => _providesServices.ToHashSet();
    
    private long _pesel;
    private DateTime _hireDate;
    private DateTime? _leaveDate;
    private decimal _yearsOfExperience;
    //start overlapping
    public EmployeeRole Roles { get; private set; }
    private string? _firstAidCertification;
    public string? FirstAidCertification
    {
        get => _firstAidCertification;
        private set
        {
            _firstAidCertification = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }

    private string? _certificationLevel;
    public string? CertificationLevel
    {
        get => _certificationLevel;
        private set
        {
            _certificationLevel = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }

    // Receptionist
    private readonly HashSet<string> _languages = new();
    public IReadOnlyCollection<string> Languages => _languages;

    // Therapist
    private readonly HashSet<string> _certifications = new();
    public IReadOnlyCollection<string> Certifications => _certifications;
    //finish overlapping
    public void AddServiceToEmployee(Service service)
    {
        if (service == null)
            throw new ArgumentNullException(nameof(service));

        if (_providesServices.Contains(service))
            return;

        _providesServices.Add(service);
        service.AddEmployeeReverse(this); // reverse connection
    }
    public void RemoveServiceFromEmployee(Service service)
    {
        if (_providesServices.Count == 1 && _providesServices.Contains(service))
            throw new InvalidOperationException("An employee must provide at least one service");
        
        if (service == null)
            throw new ArgumentNullException(nameof(service));

        if (!_providesServices.Contains(service))
            return;

        _providesServices.Remove(service);
        service.RevomeEmployeeReverse(this); 
    }
    internal void AddServiceToEmployeeReverse(Service service)
    {
        _providesServices.Add(service);
    }

    // PRIVATE reverse method
    internal void RemoveServiceFromEmployeeReverse(Service service)
    {
        _providesServices.Remove(service);
    }
    public long Pesel
    {
        get => _pesel;
        set
        {
            if (value <= 0 || value.ToString().Length != 11)
            {
                throw new ArgumentException("Invalid pesel number");
            }
            _pesel = value;
        }
    }
    
    public DateTime HireDate
    {
        get => _hireDate;
        set
        {
            if (value > DateTime.Today)
            {
                throw new ArgumentException("Hire date can't be in the future");
            }
            _hireDate = value;
        }
    }

    public DateTime? LeaveDate
    {
        get => _leaveDate;
        set
        {
            if (value != null)
            {
                if (value <= HireDate)
                {
                    throw new ArgumentException("Leave date can't be before hire date");
                }

                if (value > DateTime.Today)
                {
                    throw new ArgumentException("Leave date can't be in the future");
                }

                _leaveDate = value;
            }
        }
    }

    public decimal YearsOfExperience
    {
        get => _yearsOfExperience;
        set
        {
            if (value < 0 || value > 40)
            {
                throw new ArgumentException("Years of experience should be in the range of 0 to 40");
            }
            _yearsOfExperience = value;
        }
    }
    public int YearsOfService => CalculateYearsOfService();
    public double AverageServiceMinutes => CalculateAverageServiceMinute();
    
    //private List<Booking> Bookings { get; set; } = new List<Booking>();
    //Aggregation Association 
    private HashSet<Booking> _assignedTo = new HashSet<Booking>();

    public IEnumerable<Booking> AssignedTo => _assignedTo.ToHashSet();
    
    //Qualified association 
    private readonly HashSet<Branch> _branches = new();
    public IEnumerable<Branch> Branches => _branches.ToHashSet();

    internal void AddBranchReverse(Branch branch)
    {
        _branches.Add(branch);
    }

    internal void RemoveBranchReverse(Branch branch)
    {
        _branches.Remove(branch);
    }
    
    // public Employee(string name, string surname, string email, string phoneNumber, long pesel, DateTime hireDate,
    //     decimal yearsOfExperience, IEnumerable<Service> services,EmployeeRole roles,IEnumerable<string>? languages = null,
    //     IEnumerable<string>? certifications = null,
    //     string? firstAidCertification = null,
    //     string? certificationLevel = null)
    //     : base(name, surname, email, phoneNumber)
    // {
    //     Pesel = pesel;
    //     HireDate = hireDate;
    //     YearsOfExperience = yearsOfExperience;
    //     
    //     if (services == null || !services.Any())
    //         throw new ArgumentException("An employee must provide at least one service");
    //
    //     foreach (var service in services)
    //         AddServiceToEmployee(service);
    //     //start overlapping
    //     Roles = roles;
    //     // Languages (Receptionist)
    //     _languages.Clear();
    //     if (languages != null)
    //     {
    //         foreach (var l in languages)
    //         {
    //             if (string.IsNullOrWhiteSpace(l))
    //                 throw new ArgumentException("Language can't be empty");
    //             _languages.Add(l.Trim());
    //         }
    //     }
    //
    //     // Certifications (Therapist)
    //     _certifications.Clear();
    //     if (certifications != null)
    //     {
    //         foreach (var c in certifications)
    //         {
    //             if (string.IsNullOrWhiteSpace(c))
    //                 throw new ArgumentException("Certification can't be empty");
    //             _certifications.Add(c.Trim());
    //         }
    //     }
    //
    //     // These go through "set" validation
    //     FirstAidCertification = firstAidCertification;
    //     CertificationLevel = certificationLevel;
    //     if (Roles == EmployeeRole.None)
    //         throw new ArgumentException("Employee must have at least one role");
    //
    //     // If role NOT present -> related data must be empty/null
    //     if (!Roles.HasFlag(EmployeeRole.Receptionist) && _languages.Count > 0)
    //         throw new ArgumentException("Languages allowed only for Receptionist role");
    //
    //     if (!Roles.HasFlag(EmployeeRole.Therapist) && _certifications.Count > 0)
    //         throw new ArgumentException("Certifications allowed only for Therapist role");
    //
    //     if (!Roles.HasFlag(EmployeeRole.SaunaSupervisor) && FirstAidCertification != null)
    //         throw new ArgumentException("FirstAidCertification allowed only for SaunaSupervisor role");
    //
    //     if (!Roles.HasFlag(EmployeeRole.NailTechnician) && CertificationLevel != null)
    //         throw new ArgumentException("CertificationLevel allowed only for NailTechnician role");
    //
    //     // If role present -> minimal data required
    //     if (Roles.HasFlag(EmployeeRole.Receptionist) && _languages.Count == 0)
    //         throw new ArgumentException("Receptionist must have at least one language");
    //
    //     if (Roles.HasFlag(EmployeeRole.Therapist) && _certifications.Count == 0)
    //         throw new ArgumentException("Therapist must have at least one certification");
    //
    //     if (Roles.HasFlag(EmployeeRole.SaunaSupervisor) && FirstAidCertification == null)
    //         throw new ArgumentException("SaunaSupervisor must have first aid certification");
    //
    //     if (Roles.HasFlag(EmployeeRole.NailTechnician) && CertificationLevel == null)
    //         throw new ArgumentException("NailTechnician must have certification level");
    //     //finish overlapping
    //     addEmployee(this);
    // }
    
    
    public Junior? Jnr { get; private set; }
    public Mid? Md { get; private set; }
    public Senior? Snr { get; private set; }
    
    //Overlapping start (Person->Employee)
    public Person Prsn{ get; }
   
    [JsonConstructor]
    internal Employee(Person person, long pesel, DateTime hireDate,
        decimal yearsOfExperience, IEnumerable<Service> services,EmployeeRole roles,IEnumerable<string>? languages = null,
        IEnumerable<string>? certifications = null,
        string? firstAidCertification = null,
        string? certificationLevel = null, DateTime? leaveDate = null)
    {
        Prsn = person ?? throw new ArgumentNullException(nameof(person));
        //Overlapping ends Person -> Employee
        Pesel = pesel;
        HireDate = hireDate;
        LeaveDate = leaveDate;
        YearsOfExperience = yearsOfExperience;
        
        if (services == null || !services.Any())
            throw new ArgumentException("An employee must provide at least one service");

        foreach (var service in services)
            AddServiceToEmployee(service);
        //start overlapping
        Roles = roles;

        // Languages (Receptionist)
        _languages.Clear();
        if (languages != null)
        {
            foreach (var l in languages)
            {
                if (string.IsNullOrWhiteSpace(l))
                    throw new ArgumentException("Language can't be empty");
                _languages.Add(l.Trim());
            }
        }

        // Certifications (Therapist)
        _certifications.Clear();
        if (certifications != null)
        {
            foreach (var c in certifications)
            {
                if (string.IsNullOrWhiteSpace(c))
                    throw new ArgumentException("Certification can't be empty");
                _certifications.Add(c.Trim());
            }
        }

        // These go through "set" validation
        FirstAidCertification = firstAidCertification;
        CertificationLevel = certificationLevel;
        if (Roles == EmployeeRole.None)
            throw new ArgumentException("Employee must have at least one role");

        // If role NOT present -> related data must be empty/null
        if (!Roles.HasFlag(EmployeeRole.Receptionist) && _languages.Count > 0)
            throw new ArgumentException("Languages allowed only for Receptionist role");

        if (!Roles.HasFlag(EmployeeRole.Therapist) && _certifications.Count > 0)
            throw new ArgumentException("Certifications allowed only for Therapist role");

        if (!Roles.HasFlag(EmployeeRole.SaunaSupervisor) && FirstAidCertification != null)
            throw new ArgumentException("FirstAidCertification allowed only for SaunaSupervisor role");

        if (!Roles.HasFlag(EmployeeRole.NailTechnician) && CertificationLevel != null)
            throw new ArgumentException("CertificationLevel allowed only for NailTechnician role");

        // If role present -> minimal data required
        if (Roles.HasFlag(EmployeeRole.Receptionist) && _languages.Count == 0)
            throw new ArgumentException("Receptionist must have at least one language");

        if (Roles.HasFlag(EmployeeRole.Therapist) && _certifications.Count == 0)
            throw new ArgumentException("Therapist must have at least one certification");

        if (Roles.HasFlag(EmployeeRole.SaunaSupervisor) && FirstAidCertification == null)
            throw new ArgumentException("SaunaSupervisor must have first aid certification");

        if (Roles.HasFlag(EmployeeRole.NailTechnician) && CertificationLevel == null)
            throw new ArgumentException("NailTechnician must have certification level");
        //finish overlaping
        addEmployee(this);
    }

    // Dynamic overlapping Employee <-- Junior, Mid, Senior start
    
    public void AssignToJunior(int learningPeriod, IEnumerable<Mid> mids)
    {
        if (Jnr != null || Md != null || Snr != null)
        {
            throw new InvalidOperationException("Employee already has a level assigned.");
        }
        Jnr = new Junior(this, learningPeriod, mids);
    }

    public void AssignToMid()
    {
        if (Jnr != null || Md != null || Snr != null)
        {
            throw new InvalidOperationException("Employee already has a level assigned.");
        }
        Md = new Mid(this);
    }

    public void AssignToSenior(decimal bonusCoefficient)
    {
        if (Jnr != null || Md != null || Snr != null)
        {
            throw new InvalidOperationException("Employee already has a level assigned.");
        }
        Snr = new Senior(this, bonusCoefficient);
    }
    
    // ===== promotions triggered from children =====

    internal void PromoteToMidFromJunior()
    {
        if (Jnr == null) throw new ArgumentNullException(nameof(Jnr));
        
        // prevent dangling links and remove from class extent
        Jnr.DetachAllSupervisorsAndEmp();
        Junior.DeleteJunior(Jnr);

        Jnr = null;
        
        Md = new Mid(this);
    }

    internal void PromoteToSeniorFromMid(decimal bonusCoefficient)
    {
        if (Md == null) throw new ArgumentNullException(nameof(Md));

        // prevent dangling links and remove Md from class extent
        Md.DetachAllSupervisedJuniorsAndEmp();
        Mid.DeleteMid(Md);

        Md = null;
        
        Snr = new Senior(this, bonusCoefficient);
    }
    
    // Dynamic overlapping Employee <-- Junior, Mid, Senior end
    

    
    private static void addEmployee(Employee employee)
    {
        if (employee == null)
        {
            throw new ArgumentException("Employee cannot be null");
        }
        employees_List.Add(employee);
    }

    private int CalculateYearsOfService()
    {
        return DateTime.Now.Year - HireDate.Year;
    }

    private double CalculateAverageServiceMinute()
    {
        if (AssignedTo == null || AssignedTo.Count() == 0)
            return 0;

        // Sum all service durations
        double totalMinutes = AssignedTo
            .Where(b => b.Service != null)
            .Sum(b => b.Service.Duration.TotalMinutes);

        int count = AssignedTo.Count(b => b.Service != null);

        return count == 0 ? 0 : totalMinutes / count;
    }
    
    public static void LoadExtent(IEnumerable<Employee>? list)
    {
        employees_List.Clear();

        if (list == null) return;

        employees_List.AddRange(list);
    }
    
    public void AddBookingEmployeeAssignedTo(Booking booking)
    {
        if (booking == null)
            throw new ArgumentNullException(nameof(booking));

        if (_assignedTo.Contains(booking))
            return; // already connected

        _assignedTo.Add(booking);
        booking.SetEmployeeReverse(this);  // reverse connection
    }
    
    public void RemoveBookingEmployeeAssignedTo(Booking booking)
    {
        if (booking == null)
            throw new ArgumentNullException(nameof(booking));

        if (!_assignedTo.Contains(booking))
            return;

        _assignedTo.Remove(booking);
        booking.RemoveEmployee(); // reverse removal
    }
    
    internal void AddBookingReverse(Booking booking)
    {
        _assignedTo.Add(booking);
    }
    
    internal void RemoveBookingReverse(Booking booking)
    {
        _assignedTo.Remove(booking);
    }

    
    
    
    
    
    
    
    
    
    
    
    
    /*public  void Promote()
    {
        return;
    }

    public static void CheckYearsOfService()
    {

        foreach (var emp in AllEmployees)
        {
            if (emp.YearsOfService > 0 && emp.YearsOfService % 2 == 0)
            {
                emp.Promote();
            }
        }
    }*/
}