namespace SpaCenter;

public class Senior
{
    private static List<Senior> seniors_List = new List<Senior>();
    public static IReadOnlyList<Senior> Seniors => seniors_List.AsReadOnly();

    private decimal _bonusCoefficient;

    public decimal BonusCoefficient
    {
        get => _bonusCoefficient;
        set
        {
            if (value < 0)
                throw new ArgumentException("BonusCoefficient cannot be less than 0.");

            _bonusCoefficient = value;
        }
    }

    public Senior(decimal bonusCoefficient)
    {
        BonusCoefficient = bonusCoefficient;
        AddSenior(this);
    }

    private static void AddSenior(Senior senior)
    {
        if (senior == null)
            throw new ArgumentException("Senior cannot be null");

        seniors_List.Add(senior);
    }

    public static void LoadExtent(IEnumerable<Senior>? list)
    {
        seniors_List.Clear();

        if (list == null) return;

        seniors_List.AddRange(list);
    }
}