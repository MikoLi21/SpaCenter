namespace SpaCenter;

[Serializable]
public class Address
{
    private string _street;
    private int _building;
    private string _city;
    private string _postalCode;
    private string _country;

    public string Street
    {
        get => _street;
        private set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Street can't be empty");
            }
            _street = value;
        }
    }
    
    public int Building
    {
        get => _building;
        private set
        {
            if (value <= 0)
            {
                throw new ArgumentNullException("Building can't be less or equal to 0");
            }
            _building = value;
        }
    }

    public string City
    {
        get => _city;
        private set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("City can't be empty");
            }
            _city = value;
        }
    }

    public string PostalCode
    {
        get => _postalCode;
        private set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Postal code can't be empty");
            }
            _postalCode = value;
        }
    }

    public string Country
    {
        get => _country;
        private set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Country can't be empty");
            }
            _country = value;
        }
    }
    
    public Address(string street, int building, string city, string postalCode, string country)
    {
        Street = street;
        Building = building;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }

    public override string ToString()
    {
        return $"{Street} {Building}, {PostalCode} {City}, {Country}";
    }
}