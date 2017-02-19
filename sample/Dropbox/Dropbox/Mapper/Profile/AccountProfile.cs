using Dropbox.Api.Users;
using Dropbox.DTO;

namespace Dropbox.Mapper.Profile
{
    class AccountProfile : BaseProfile
    {
        public AccountProfile() : base("AccountProfile")
        {
        }

        protected override void CreateMaps()
        {
            CreateMap<FullAccount, AccountDTO>()
                .ForMember(x => x.CountryCode, opt => opt.MapFrom(c => c.Country))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(c => c.Name.GivenName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(c => c.Name.Surname))
                .ForMember(x => x.AccountType, opt => opt.MapFrom(c => GetAccountType(c.AccountType)));
        }

        private static Enum.AccountType GetAccountType(AccountType type)
        {
            if (type.AsBusiness != null) return Enum.AccountType.Business;
            if (type.AsPro != null) return Enum.AccountType.Pro;

            return Enum.AccountType.Basic;
        }
    }
}
