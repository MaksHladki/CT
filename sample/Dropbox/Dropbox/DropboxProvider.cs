using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dropbox.Mapper;

namespace Dropbox
{
    public class DropboxProvider
    {
        #region Fields

        private const string TokenKey = "access_token";
        private readonly DropboxClient _dropbox;

        #endregion

        #region Constructors

        //конструкторы не могут быть вызваны из вне класса
        private DropboxProvider()
        {
            Token = SettingsProvider.GetValue(TokenKey);
            _dropbox = new DropboxClient(Token);
        }

        private DropboxProvider(string token)
        {
            Token = token;
            _dropbox = new DropboxClient(Token);
        }

        #endregion

        #region Properties

        public string Token { get; }

        #endregion

        #region Initialize methods

        private async Task<DropboxProvider> InitializeAsync()
        {
            //любой вид полезной нагрузки (например, получение аккаунта)
            await _dropbox.Users.GetCurrentAccountAsync();
            return this;
        }

        public static async Task<DropboxProvider> CreateAsync()
        {
            var drProvider = new DropboxProvider();
            return await drProvider.InitializeAsync();
        }

        public static async Task<DropboxProvider> CreateAsync(string token)
        {
            var dropbox = new DropboxProvider(token);
            return await dropbox.InitializeAsync();
        }

        #endregion

        #region Account methods

        public async Task<AccountDTO> GetAccountAsync()
        {
            var account = await _dropbox.Users.GetCurrentAccountAsync();
            return MapConverter.Convert<AccountDTO>(account);
        }

        #endregion

        #region File methods

        private async Task<IEnumerable<Metadata>> GetListOfEntriesRawAsyns(string path, Func<Metadata, bool> condition = null)
        {
            var list = await _dropbox.Files.ListFolderAsync(path, recursive: false, includeMediaInfo: true);
            return condition == null ? list.Entries : list.Entries.Where(condition);
        }

        public async Task<IList<MetaDataDTO>> GetListOfEntriesAsyns(string path = "")
        {
            var entries = await GetListOfEntriesRawAsyns(path);
            return entries.Select(MapConverter.Convert<MetaDataDTO>).ToList();
        }

        public async Task<IList<FolderDTO>> GetListOfFoldersAsyns(string path = "")
        {
            Func<Metadata, bool> condition = m => m.IsFolder;

            var folders = await GetListOfEntriesRawAsyns(path, condition);
            return folders.Select(AutoMapper.Mapper.Map<FolderDTO>).ToList();
        }

        public async Task<IList<FileDTO>> GetListOfFilesAsyns(string path = "")
        {
            Func<Metadata, bool> condition = m => m.IsFile;

            var files = await GetListOfEntriesRawAsyns(path, condition);
            return files.Select(AutoMapper.Mapper.Map<FileDTO>).ToList();
        }

        public async Task<MetaDataDTO> GetMetaDataAsync(string path)
        {
            var metadata = await _dropbox.Files.GetMetadataAsync(path);
            return MapConverter.Convert<MetaDataDTO>(metadata);
        }

        public async Task<MetaDataDTO> CopyAsync(string pathFrom, string pathTo)
        {
            var metadata = await _dropbox.Files.CopyAsync(pathFrom, pathTo, allowSharedFolder: true, autorename: true);
            return MapConverter.Convert<MetaDataDTO>(metadata);
        }

        public async Task<MetaDataDTO> DeleteAsync(string path)
        {
            var metadata = await _dropbox.Files.DeleteAsync(path);
            return MapConverter.Convert<MetaDataDTO>(metadata);
        }

        public async Task<MetaDataDTO> MoveAsync(string pathFrom, string pathTo)
        {
            var metadata = await _dropbox.Files.MoveAsync(pathFrom, pathTo, allowSharedFolder: true, autorename: true);
            return MapConverter.Convert<MetaDataDTO>(metadata);
        }

        public async Task<IList<FileDTO>> SearchAsync(string path, string query, uint startIndex = 0, uint takeCount = UInt16.MaxValue, bool includeContent = false)
        {
            SearchMode serachMode = includeContent
                ? SearchMode.FilenameAndContent.Instance
                : (SearchMode)SearchMode.Filename.Instance;

            var result = await _dropbox.Files.SearchAsync(path, query, startIndex, takeCount, serachMode);
            return result.Matches.Select(x => MapConverter.Convert<FileDTO>(x.Metadata)).ToList();
        }

        public async Task<FolderDTO> CreateFolderAsync(string path)
        {
            var folderMetadata = await _dropbox.Files.CreateFolderAsync(path, autorename: true);
            return MapConverter.Convert<FolderDTO>(folderMetadata);
        }

        public async Task<Stream> DownloadAsync(string path)
        {
            var fileMetadata = await _dropbox.Files.DownloadAsync(path);

            var stream = await fileMetadata.GetContentAsStreamAsync();
            return stream;
        }

        public async Task<FileDTO> UploadAsync(string path, Stream stream)
        {
            var fileMetadata = await _dropbox.Files.UploadAsync(path, WriteMode.Overwrite.Instance, body: stream);
            return MapConverter.Convert<FileDTO>(fileMetadata);
        }

        #endregion

        #region Share methods

        public async Task<SharedMetaDataDTO> ShareAsync(string path)
        {
            var sharedMetadata = await _dropbox.Sharing.CreateSharedLinkWithSettingsAsync(path);
            return MapConverter.Convert<SharedMetaDataDTO>(sharedMetadata);
        }

        public async Task UnShareAsync(string url)
        {
            await _dropbox.Sharing.RevokeSharedLinkAsync(url);
        }

        public async Task<IList<SharedMetaDataDTO>> ListOfSharedLinksAsync(string path)
        {
            var list = await _dropbox.Sharing.ListSharedLinksAsync(path, null, false);
            return list.Links.Select(MapConverter.Convert<SharedMetaDataDTO>).ToList();
        }

        #endregion
    }
}
