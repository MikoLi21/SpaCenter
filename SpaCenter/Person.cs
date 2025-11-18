using System.Text.RegularExpressions;

namespace SpaCenter;

[Serializable]
public abstract class Person
{
    private static readonly Regex EmailRegex =
        new(@"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$");

    private static readonly Regex PhoneRegex =
        new(@"^(?:\+48)? ?\d{9}$");

    private string _name;
    private string _surname;
    private string _email;
    private string _phoneNumber;

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

    public string Surname
    {
        get => _surname;
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Surname can't be empty");
            }
            _surname = value;
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Email can't be empty");
            }
            
            if (!(EmailRegex.IsMatch(value)))
            {
                throw new ArgumentException("Invalid email address");
            }
            _email = value;
        }
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Phone number can't be empty");
            }
            
            if (!(PhoneRegex.IsMatch(value)))
            {
                throw new ArgumentException("Invalid phone number");
            }
            _phoneNumber = value;
        }
    }

    protected Person(string name, string surname, string email, string phoneNumber)
    {
            Name = name;
            Surname = surname;
            Email = email;
            PhoneNumber = phoneNumber;
    }
}