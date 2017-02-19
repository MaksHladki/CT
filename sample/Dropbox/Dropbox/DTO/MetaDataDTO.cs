using System;

namespace Dropbox.DTO
{
    public class MetaDataDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PathDisplay { get; set; }
        public string PathLower => PathDisplay?.ToLower() ?? string.Empty;
    }

    public class FileDTO : MetaDataDTO
    {
        public DateTime ClientModified { get; set; }
        public DateTime ServerModified { get; set; }
        public string Rev { get; set; }
        public ulong Size { get; set; }
    }

    public class FolderDTO : MetaDataDTO
    {

    }
}
