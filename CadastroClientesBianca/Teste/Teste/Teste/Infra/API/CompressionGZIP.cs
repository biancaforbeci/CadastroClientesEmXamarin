using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AppClientes.Infra.API
{
    public static class CompressionGZIP 
    {
        public static string CompressString(string uncompressedString)
        {
            var compressedStream = new MemoryStream();
            var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString));

            using (var compressorStream = new DeflateStream(compressedStream, CompressionMode.Compress, true))
            {
                uncompressedStream.CopyTo(compressorStream);
            }

            return Convert.ToBase64String(compressedStream.ToArray());
        }

        public static string Decompress(this string compressedString)   //implementar quando necessário.
        {
            var decompressedStream = new MemoryStream();
            var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));

            using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                decompressorStream.CopyTo(decompressedStream);
            }

            return Encoding.UTF8.GetString(decompressedStream.ToArray());
        }
    }
}
