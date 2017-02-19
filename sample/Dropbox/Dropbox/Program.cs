using System;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.DTO;
using System.IO;
using Dropbox.Mapper.Profile;

namespace Dropbox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AccountProfile>();
                cfg.AddProfile<MetaDataProfile>();
                cfg.AddProfile<SharedMetaDataProfile>();
            });

            var task = Task.Run(Run);
            task.Wait();
        }

        private static async Task Run()
        {
            try
            {
                var dropbox = await DropboxProvider.CreateAsync();

                /*
                 * Пример реализации интерфейса, 
                 * для проверки работоспособности необходимо изменить входные данные для методов (например, path, url и т.д.)
                 */

                await AccountInfo(dropbox);

                //await ListOfEntries(dropbox);
                //await ListOfFolders(dropbox);
                //await ListOfFiles(dropbox);
                //await MetaData(dropbox);
                //await Search(dropbox);

                //await Copy(dropbox);
                //await Move(dropbox);
                //await Delete(dropbox);
                //await CreateFolder(dropbox);

                //await Download(dropbox);
                //await Upload(dropbox);

                //await ListOfSharedLinks(dropbox);
                //await Share(dropbox);
                //await UnShare(dropbox);
            }
            catch (DropboxException ex)
            {
                Console.WriteLine($"Dropbox exception: type {ex.GetType()}, message {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: type {ex.GetType()}, message {ex.Message}");
            }
        }

        #region Account methods

        private static async Task AccountInfo(DropboxProvider dropbox)
        {
            var account = await dropbox.GetAccountAsync();

            Console.WriteLine("Account Info:");
            Console.WriteLine($"Full name: {account.Fullname}");
            Console.WriteLine($"Email: { account.Email}");
            Console.WriteLine($"Type: { account.AccountType}");
            Console.WriteLine($"Country code: { account.CountryCode}");
            Console.WriteLine($"Locale: { account.Locale}");
            Console.WriteLine($"Image URL: { (string.IsNullOrEmpty(account.ProfilePhotoUrl) ? "NONE" : account.ProfilePhotoUrl)}");
            Console.WriteLine();
        }

        #endregion

        #region File methods

        private static async Task ListOfEntries(DropboxProvider dropbox)
        {
            const string path = "/DROPBOX_API/";

            var entries = await dropbox.GetListOfEntriesAsyns(path);
            foreach (var entry in entries)
            {
                PrintMetaData(entry);
            }
        }

        private static async Task ListOfFolders(DropboxProvider dropbox)
        {
            const string path = "/DROPBOX_API/Test1";

            var folders = await dropbox.GetListOfFoldersAsyns(path);
            foreach (var folder in folders)
            {
                PrintFolder(folder);
            }
        }

        private static async Task ListOfFiles(DropboxProvider dropbox)
        {
            const string path = "/DROPBOX_API/Test1";

            var files = await dropbox.GetListOfFilesAsyns(path);
            foreach (var file in files)
            {
                PrintFile(file);
            }
        }

        private static async Task MetaData(DropboxProvider dropbox)
        {
            const string path = "/DROPBOX_API/Test1";

            var metaData = await dropbox.GetMetaDataAsync(path);
            PrintMetaData(metaData);
        }

        private static async Task Search(DropboxProvider dropbox)
        {
            const string path = "/DROPBOX_API/",
                query = ".txt";
            const int startIndex = 0,
                takeCount = 10;

            var files = await dropbox.SearchAsync(path, query, startIndex, takeCount, true);
            foreach (var file in files)
            {
                PrintFile(file);
            }
        }

        private static async Task Copy(DropboxProvider dropbox)
        {
            const string pathFrom = "/DROPBOX_API/Test1",
                pathTo = "/DROPBOX_API/Test6";

            var entry = await dropbox.CopyAsync(pathFrom, pathTo);
            PrintMetaData(entry);
        }

        private static async Task Delete(DropboxProvider dropbox)
        {
            const string path = "/DROPBOX_API/Test6";

            var entry = await dropbox.DeleteAsync(path);
            PrintMetaData(entry);
        }

        private static async Task Move(DropboxProvider dropbox)
        {
            const string pathFrom = "/DROPBOX_API/Test2/Hello.txt",
                pathTo = "/DROPBOX_API/Test6/Hello.txt";

            var entry = await dropbox.MoveAsync(pathFrom, pathTo);
            PrintMetaData(entry);
        }

        private static async Task CreateFolder(DropboxProvider dropbox)
        {
            const string path = "/DROPBOX_API/Test10";

            var entry = await dropbox.CreateFolderAsync(path);
            PrintMetaData(entry);
        }

        private static async Task Download(DropboxProvider dropbox)
        {
            const string path = "/DROPBOX_API/Test1/Test.txt";
            var stream = await dropbox.DownloadAsync(path);

            using (var stReader = new StreamReader(stream))
            {
                var content = await stReader.ReadToEndAsync();
                Console.WriteLine("Download file");
                Console.WriteLine($"Content: {content}");
            }
        }

        private static async Task Upload(DropboxProvider dropbox)
        {
            const string content = "Hello World",
                path = "/DROPBOX_API/Test13.txt";
            var butes = System.Text.Encoding.Default.GetBytes(content);
            var stream = new MemoryStream(butes);

            var entry = await dropbox.UploadAsync(path, stream);
            PrintMetaData(entry);
        }

        private static async Task ListOfSharedLinks(DropboxProvider dropbox)
        {
            const string path = "/DROPBOX_API/Test1";

            var links = await dropbox.ListOfSharedLinksAsync(path);
            foreach (var link in links)
            {
                PrintSharedMetaData(link);
            }
        }

        public static async Task Share(DropboxProvider dropbox)
        {
            const string path = "/DROPBOX_API/Test2";

            var link = await dropbox.ShareAsync(path);
            PrintSharedMetaData(link);
        }

        public static async Task UnShare(DropboxProvider dropbox)
        {
            const string url = "https://www.dropbox.com/sh/xjmcz0h8ai94lfl/AABdQDn9OUQpokCTKuQgmWTGa";

            await dropbox.UnShareAsync(url);
        }

        #endregion

        #region MetaData helper methods

        private static void PrintMetaData(MetaDataDTO metaData)
        {
            if (metaData is FolderDTO)
            {
                PrintFolder((FolderDTO)metaData);
            }
            else
            {
                PrintFile((FileDTO)metaData);
            }
        }

        private static void PrintFolder(FolderDTO folder)
        {
            Console.WriteLine("Folder");
            Console.WriteLine($"Name: {folder.Name}");
            Console.WriteLine($"Id: {folder.Id}");
            Console.WriteLine($"Path: {folder.PathDisplay}");
            Console.WriteLine();
        }

        private static void PrintFile(FileDTO file)
        {
            Console.WriteLine("File");
            Console.WriteLine($"Name: {file.Name}");
            Console.WriteLine($"Id: {file.Id}");
            Console.WriteLine($"Path: {file.PathDisplay}");
            Console.WriteLine($"Modified: {file.ClientModified}");
            Console.WriteLine($"Revision: {file.Rev}");
            Console.WriteLine($"Size: {file.Size}");
            Console.WriteLine();
        }

        #endregion

        #region SHareMetaData helper methods

        private static void PrintSharedMetaData(SharedMetaDataDTO sharedMetaData)
        {
            if (sharedMetaData is SharedFileDTO)
            {
                PrintSharedFile((SharedFileDTO)sharedMetaData);
            }
            else
            {
                PrintSharedFolder((SharedFolderDTO)sharedMetaData);
            }
        }

        private static void PrintSharedFolder(SharedFolderDTO folder)
        {
            Console.WriteLine("Folder");
            Console.WriteLine($"Name: {folder.Name}");
            Console.WriteLine($"Id: {folder.Id}");
            Console.WriteLine($"Url: {folder.Url}");
            Console.WriteLine();
        }

        private static void PrintSharedFile(SharedFileDTO file)
        {
            Console.WriteLine("File");
            Console.WriteLine($"Name: {file.Name}");
            Console.WriteLine($"Id: {file.Id}");
            Console.WriteLine($"Modified: {file.ClientModified}");
            Console.WriteLine($"Revision: {file.Rev}");
            Console.WriteLine($"Size: {file.Size}");
            Console.WriteLine($"Url: {file.Url}");
            Console.WriteLine();
        }

        #endregion
    }
}
