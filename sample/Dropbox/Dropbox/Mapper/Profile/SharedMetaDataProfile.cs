using Dropbox.Api.Sharing;
using Dropbox.DTO;

namespace Dropbox.Mapper.Profile
{
    public class SharedMetaDataProfile : BaseProfile
    {
        public SharedMetaDataProfile() : base("SharedMetaDataProfile")
        {
        }

        protected override void CreateMaps()
        {
            CreateMap<SharedLinkMetadata, SharedMetaDataDTO>();

            CreateMap<SharedLinkMetadata, SharedFolderDTO>()
                .IncludeBase<SharedLinkMetadata, MetaDataDTO>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.AsFolder.Id))
                .ForMember(x => x.Url, opt => opt.MapFrom(c => c.AsFolder.Url));

            CreateMap<SharedLinkMetadata, SharedFileDTO>()
                .IncludeBase<SharedLinkMetadata, SharedMetaDataDTO>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.AsFile.Id))
                .ForMember(x => x.Url, opt => opt.MapFrom(c => c.AsFile.Url))
                .ForMember(x => x.ClientModified, opt => opt.MapFrom(c => c.AsFile.ClientModified))
                .ForMember(x => x.ServerModified, opt => opt.MapFrom(c => c.AsFile.ServerModified))
                .ForMember(x => x.Rev, opt => opt.MapFrom(c => c.AsFile.Rev))
                .ForMember(x => x.Size, opt => opt.MapFrom(c => c.AsFile.Size));
        }
    }
}
