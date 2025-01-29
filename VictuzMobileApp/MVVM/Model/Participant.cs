using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class Participant : INotifyPropertyChanged
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Name { get; set; }

    [NotNull, Unique]
    public string Email { get; set; }

    [NotNull]
    public string PhoneNumber { get; set; }

    [NotNull]
    public string Password { get; set; }

    private string _profilePicturePath;
    public string ProfilePicturePath
    {
        get => _profilePicturePath;
        set
        {
            _profilePicturePath = value;
            OnPropertyChanged();
        }
    }

    public string BirthDate { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public bool IsMale { get; set; }
    public bool IsActive { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
