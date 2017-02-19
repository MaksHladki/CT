using System;
using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using Dropbox.DTO;

namespace Dropbox.Mapper
{
    public static class MapConverter
    {
        public static T Convert<T>(Object x) where T : class
        {
            return (T)AutoMapper.Mapper.Map(x, x.GetType(), typeof(T));
        }

        public static T Convert<T>(Metadata x) where T : MetaDataDTO
        {
            var destinationType = x.GetType() == typeof(FileMetadata) ? typeof(FileDTO) : typeof(FolderDTO);

            return (T)AutoMapper.Mapper.Map(x, x.GetType(), destinationType);
        }

        public static T Convert<T>(SharedLinkMetadata x) where T : SharedMetaDataDTO
        {
            var destinationType = x.GetType() == typeof(SharedFileMetadata) ? typeof(SharedFileDTO) : typeof(SharedFolderDTO);

            return (T)AutoMapper.Mapper.Map(x, x.GetType(), destinationType);
        }
    }
}
