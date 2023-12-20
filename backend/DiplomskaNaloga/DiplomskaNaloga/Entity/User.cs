using MongoDB.Bson.Serialization.Attributes;

namespace Entity;

[BsonIgnoreExtraElements]
public partial class User
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string PasswordHash { get; set; }

    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public string Email { get; set; }

    public string RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiration { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
}