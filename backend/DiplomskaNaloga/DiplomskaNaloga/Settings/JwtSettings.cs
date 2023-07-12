namespace DiplomskaNaloga.Settings
{
    public class JwtSettings
    {
        public const string Position = "JWT";
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
        public int TokenValidityInMinutes { get; set; }
        public int RefreshTokenValidityInDays { get; set; }
    }
}
