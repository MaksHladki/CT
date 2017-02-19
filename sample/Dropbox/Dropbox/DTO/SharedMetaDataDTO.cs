using System;

namespace Dropbox.DTO
{
    public class SharedMetaDataDTO
    {
        public string Id { get; set; }
        public DateTime? Expires { get; set; }
        public string Name { get; set; }
        public string PathLower { get; set; }
        public string Url { get; set; }
    }

    public class SharedFileDTO : SharedMetaDataDTO
    {
        public DateTime ClientModified { get; set; }
        public DateTime ServerModified { get; set; }
        public string Rev { get; set; }
        public ulong Size { get; set; }
    }

    public class SharedFolderDTO : SharedMetaDataDTO
    {

    }
}
