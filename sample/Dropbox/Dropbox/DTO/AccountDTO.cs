using Dropbox.Enum;

namespace Dropbox.DTO
{
    public class AccountDTO
    {
        public AccountType AccountType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Fullname => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string Locale { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string ReferralLink { get; set; }
    }
}
