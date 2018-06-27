using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppClientes.Infra;

namespace AppClientes.Droid.PlatformCode
{
    class AndroidFileSystem : IFileSystem
    {
        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string GetDatabasePath()
        {
            var dbFolderName = "Client";
            var dbFileName = "Client.db";
           

            var externalStorageDirectory = GetStoragePath();
            var absoluteDatabaseDirectory = Path.Combine(externalStorageDirectory, dbFolderName);

            if (!Directory.Exists(absoluteDatabaseDirectory))
                Directory.CreateDirectory(absoluteDatabaseDirectory);            

            var absoluteDatabaseFileName = Path.Combine(absoluteDatabaseDirectory, dbFileName);
            
            return absoluteDatabaseFileName;
        }

        public string GetStoragePath()
        {
            return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
        }

        public Task<string[]> ListDirectoriesAsync(string directoryPath)
        {
            return Task.Factory.StartNew(delegate
            {
                return Directory.GetDirectories(directoryPath);
            });
        }

        public Task<string[]> ListFilesAsync(string directoryPath)
        {
            return Task.Factory.StartNew(delegate
            {
                return Directory.GetFiles(directoryPath);
            });
        }

        public Task<IEnumerable<string>> SearchFilesAsync(string directory, string searchPattern, bool recursive = true)
        {
            return Task.Factory.StartNew(delegate
            {
                if (recursive)
                    return Directory.EnumerateFiles(directory, searchPattern, SearchOption.AllDirectories);
                return Directory.EnumerateFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);
            });
        }        
    }
}