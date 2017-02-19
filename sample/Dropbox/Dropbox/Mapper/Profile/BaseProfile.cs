namespace Dropbox.Mapper.Profile
{
    public abstract class BaseProfile : AutoMapper.Profile
    {
        protected BaseProfile(string profileName)
        {
            ProfileName = profileName;
        }

        public override string ProfileName { get; }

        protected override void Configure()
        {
            CreateMaps();
        }

        protected abstract void CreateMaps();
    }
}
