namespace DiplomskaNaloga.Models
{
    public class UserDto : UserDtoData
    {
        public Guid Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }

    public class UserDtoData
    {
        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }
    }
    public class UserLogin
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class UserActivate
    {
        public bool IsActive { get; set; }
    }
}
