namespace SpaCenter;

public class Junior
{
    
    private static List<Junior> juniors_List = new List<Junior>();
    public static IReadOnlyList<Junior> Juniors => juniors_List.AsReadOnly();
    private HashSet<Mid> _supervisedBy = new HashSet<Mid>();
    public IEnumerable<Mid> SupervisedBy => _supervisedBy.ToHashSet();
    
    private int _learningPeriod;

    public int LearningPeriod
    {
        get => _learningPeriod;
        set
        {
            if (value < 0)
                throw new ArgumentException("LearningPeriod cannot be less than 0.");

            _learningPeriod = value;
        }
    }

    public Junior(int learningPeriod, IEnumerable<Mid> mids)
    {
        LearningPeriod = learningPeriod;
        
        if (mids == null || !mids.Any())
            throw new ArgumentException("Junior must be supervised by at least one mid");

        foreach (var mid in mids)
            AddMidToJunior(mid);
        
        AddJunior(this);
    }
    
    private static void AddJunior(Junior junior)
    {
        if (junior == null)
            throw new ArgumentException("Junior cannot be null");

        juniors_List.Add(junior);
    }

    public static void LoadExtent(IEnumerable<Junior>? list)
    {
        juniors_List.Clear();

        if (list == null) return;

        juniors_List.AddRange(list);
    }
    
    public void AddMidToJunior(Mid mid)
    {
        if (mid == null)
            throw new ArgumentNullException(nameof(mid));

        if (_supervisedBy.Contains(mid))
            return;

        _supervisedBy.Add(mid);
        mid.AddJuniorToMidReverse(this); // reverse connection
    }
    public void RemoveMidFromJunior(Mid mid)
    {
        if (_supervisedBy.Count == 1 && _supervisedBy.Contains(mid))
            throw new InvalidOperationException("Junior must be supervised by at least one mid.");
        
        if (mid == null)
            throw new ArgumentNullException(nameof(mid));

        if (!_supervisedBy.Contains(mid))
            return;

        _supervisedBy.Remove(mid);
        mid.RemoveJuniorFromMidReverse(this); 
    }
    internal void AddMidToJuniorReverse(Mid mid)
    {
        _supervisedBy.Add(mid);
    }

    internal void RemoveMidFromJuniorReverse(Mid mid)
    {
        _supervisedBy.Remove(mid);
    }
}