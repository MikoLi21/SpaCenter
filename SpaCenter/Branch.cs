using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SpaCenter;

[Serializable]
public class Branch
{
    //Branch Container
    private static List<Branch> branches_List = new List<Branch>();
    public static IReadOnlyList<Branch> Branches => branches_List.AsReadOnly();
    
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Name can't be empty");
            }
            _name = value;
        }
    }
    
    public Address Address { get; private set; }
    
    //private readonly List<string> _phoneNumbers = new();
    public List<string> PhoneNumbers { get; set; } = new();

    private static TimeSpan _openingTime = new TimeSpan(9, 0, 0);
    public static TimeSpan OpeningTime
    {
        get => _openingTime;
        set
        {
            _openingTime = value;
        }
    }
    
    private static TimeSpan _closingTime = new TimeSpan(21, 0, 0);
    public static TimeSpan ClosingTime
    {
        get => _closingTime;
        set
        {
            _closingTime = value;
        }
    }
    
    private static readonly Regex PhoneRegex = new(@"^(?:\+48)? ?\d{9}$");
    
    //Qualified association
    private readonly Dictionary<long, Employee> _employeesByPesel = new();

    public IReadOnlyDictionary<long, Employee> EmployeesByPesel => _employeesByPesel;
    public IEnumerable<Employee> Employees => _employeesByPesel.Values;

    public Employee? GetEmployeeByPesel(long pesel)
    {
        _employeesByPesel.TryGetValue(pesel, out var employee);
        return employee;
    }

    
    public void AddEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        long pesel = employee.Pesel;

        
        if (_employeesByPesel.ContainsKey(pesel))
            throw new ArgumentException("Employee with same PESEL already assigned to this branch");

       
        if (employee.Branches.Any() && !employee.Branches.Contains(this))
            throw new InvalidOperationException("Employee already assigned to a different branch");

        _employeesByPesel.Add(pesel, employee);

        
        employee.AddBranchReverse(this);
    }

    
    public void UpdateEmployeePesel(long oldPesel, long newPesel)
    {
        if (!_employeesByPesel.ContainsKey(oldPesel))
            throw new ArgumentException("Employee with old PESEL does not exist");

        if (_employeesByPesel.ContainsKey(newPesel))
            throw new ArgumentException("New PESEL already exists");

        var employee = _employeesByPesel[oldPesel];

       
        employee.Pesel = newPesel;

        _employeesByPesel.Remove(oldPesel);
        _employeesByPesel.Add(newPesel, employee);
    }

    
    public void RemoveEmployee(long pesel)
    {
        
        if (_employeesByPesel.Count == 1 && _employeesByPesel.ContainsKey(pesel))
            throw new InvalidOperationException("Branch must have at least one employee");

        if (_employeesByPesel.TryGetValue(pesel, out var employee))
        {
            _employeesByPesel.Remove(pesel);
            employee.RemoveBranchReverse(this); // reverse removal
        }
    }

    [JsonConstructor]
    public Branch(string name, Address address, List<string> phoneNumbers)
    {
        Name = name;
        Address = address ?? throw new ArgumentNullException(nameof(address));

        if (phoneNumbers == null)
            throw new ArgumentNullException("Phone numbers can't be empty");

        foreach (var number in phoneNumbers)
        {
            if (String.IsNullOrEmpty(number))
            {
                throw new ArgumentNullException("Phone number can't be empty");
            }
            
            if (!(PhoneRegex.IsMatch(number)))
            {
                throw new ArgumentException("Invalid phone number");
            }
            
            PhoneNumbers.Add(number);
        }

        if (PhoneNumbers.Count == 0)
            throw new ArgumentException("At least one phone number is required");

        addBranch(this);
    }
    
    private static void addBranch(Branch branch)
    {
        if (branch == null)
        {
            throw new ArgumentException("Branch cannot be null");
        }
        branches_List.Add(branch);
    }
    
    public static void LoadExtent(IEnumerable<Branch>? list)
    {
        branches_List.Clear();

        if (list == null) return;

        branches_List.AddRange(list);
    }
}