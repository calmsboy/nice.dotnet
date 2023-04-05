using Nice.Dotnet.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nice.Dotnet.Core.Services
{
    public class FileService : IFileService
    {
        public Task<TValue> OpenFileToModel<TValue>(string path)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> OpenForReadFileAsync(string path)
        {
            throw new NotImplementedException();
        }
    }
}
