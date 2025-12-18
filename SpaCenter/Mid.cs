namespace SpaCenter;

public class Mid
{
    private static List<Mid> mids_List = new List<Mid>();
    public static IReadOnlyList<Mid> Mids => mids_List.AsReadOnly();
    private HashSet<Junior> _supervisedJuniors = new HashSet<Junior>();
    public IEnumerable<Junior> SupervisedJuniors => _supervisedJuniors.ToHashSet();
    public Employee Emp { get; private set; }

    internal Mid(Employee emp)
    {
        Emp = emp ?? throw new ArgumentNullException(nameof(emp));
        AddMid(this);
    }

    private static void AddMid(Mid mid)
    {
        if (mid == null)
            throw new ArgumentException("Mid cannot be null");

        mids_List.Add(mid);
    }

    public static void LoadExtent(IEnumerable<Mid>? list)
    {
        mids_List.Clear();

        if (list == null) return;

        mids_List.AddRange(list);
    }
    
    public void AddJuniorToMid(Junior junior)
    {
        if (junior == null)
            throw new ArgumentNullException(nameof(junior));

        if (_supervisedJuniors.Contains(junior))
            return; 

        _supervisedJuniors.Add(junior);
        junior.AddMidToJuniorReverse(this);  
    }
    public void RevomeJuniorFromMid(Junior junior)
    {
        if (junior == null)
            throw new ArgumentNullException(nameof(junior));

        if (!_supervisedJuniors.Contains(junior))
            return;

        _supervisedJuniors.Remove(junior);
        junior.RemoveMidFromJuniorReverse(this);
    }
    internal void AddJuniorToMidReverse(Junior junior)
    {
        _supervisedJuniors.Add(junior);
    }

   
    internal void RemoveJuniorFromMidReverse(Junior junior)
    {
        _supervisedJuniors.Remove(junior);
    }
    
    // Dynamic overlapping Employee <-- Junior, Mid, Senior start
    // switching is initiated from the child
    public void SwitchToSenior(decimal bonusCoefficient)
    {
        Emp.PromoteToSeniorFromMid(bonusCoefficient);
    }

    // used by Employee during promotion to prevent dangling links
    internal void DetachAllSupervisedJuniorsAndEmp()
    {
        foreach (var junior in _supervisedJuniors.ToList())
        {
            _supervisedJuniors.Remove(junior);
            junior.RemoveMidFromJuniorReverse(this); 
        }

        Emp = null!;
    }
    
    internal static void DeleteMid(Mid mid)
    {
        mids_List.Remove(mid);
    }
    // Dynamic overlapping Employee <-- Junior, Mid, Senior end
}