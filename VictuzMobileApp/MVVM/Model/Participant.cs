using SQLite;

public class Participant
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Name { get; set; }

    [NotNull]
    public string Email { get; set; }

    [NotNull]
    public string Password { get; set; }

    public string ProfilePicturePath { get; set; }
    public string BirthDate { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public bool IsMale { get; set; }
}
