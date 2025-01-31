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
    public string Password { get; set; }

    private string _profilePicturePath = "person.png"; // Standaardwaarde instellen

    public string ProfilePicturePath
    {
        get => _profilePicturePath;
        set
        {
            if (_profilePicturePath != value)
            {
                _profilePicturePath = string.IsNullOrEmpty(value) ? "person.png" : value;
                OnPropertyChanged();
            }
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

    // Constructor om ervoor te zorgen dat nieuwe gebruikers standaard "person.png" krijgen
    public Participant()
    {
        if (string.IsNullOrEmpty(ProfilePicturePath))
        {
            ProfilePicturePath = "person.png";
        }
    }
}
