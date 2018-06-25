using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppClientes.Infra
{
    public interface IFileSystem
    {
        Task<string[]> ListDirectoriesAsync(string directoryPath);
        Task<string[]> ListFilesAsync(string directoryPath);
        Task<IEnumerable<string>> SearchFilesAsync(string directory, string searchPattern, bool recursive = true);       
        bool FileExists(string filePath);
        string GetDatabasePath();
        string GetStoragePath();
    }
}
