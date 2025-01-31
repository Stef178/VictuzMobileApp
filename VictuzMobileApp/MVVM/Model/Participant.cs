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

    private string _profilePicturePath = "person.png"; // Standaard afbeelding

    public string ProfilePicturePath
    {
        get => _profilePicturePath;
        set
        {
            // Controleer of de waarde leeg is en zet standaardwaarde in dat geval
            value = string.IsNullOrEmpty(value) ? "person.png" : value;

            // Update alleen als de waarde echt verandert
            if (_profilePicturePath != value)
            {
                _profilePicturePath = value;
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
}
