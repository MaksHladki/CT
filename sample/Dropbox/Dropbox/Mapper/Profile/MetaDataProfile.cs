using Dropbox.Api.Files;
using Dropbox.DTO;

namespace Dropbox.Mapper.Profile
{
    public class MetaDataProfile : BaseProfile
    {
        public MetaDataProfile() : base("MetaDataProfile")
        {
        }

        protected override void CreateMaps()
        {
            CreateMap<Metadata, MetaDataDTO>();

            CreateMap<Metadata, FolderDTO>()
                .IncludeBase<Metadata, MetaDataDTO>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.AsFolder.Id));

            CreateMap<Metadata, FileDTO>()
                .IncludeBase<Metadata, MetaDataDTO>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.AsFile.Id))
                .ForMember(x => x.ClientModified, opt => opt.MapFrom(c => c.AsFile.ClientModified))
                .ForMember(x => x.ServerModified, opt => opt.MapFrom(c => c.AsFile.ServerModified))
                .ForMember(x => x.Rev, opt => opt.MapFrom(c => c.AsFile.Rev))
                .ForMember(x => x.Size, opt => opt.MapFrom(c => c.AsFile.Size));
        }
    }
}
