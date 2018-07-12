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
    public class CompressionGZIP : HttpContent
    {
        private JsonSerializer serializer { get; }
        private object value { get; }

        public CompressionGZIP(object value)
        {
            this.serializer = new JsonSerializer();
            this.value = value;
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
            Headers.ContentEncoding.Add("gzip");
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }        

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var gzip = new GZipStream(stream, CompressionMode.Compress, true))
                using (var writer = new StreamWriter(gzip))
                {
                    serializer.Serialize(writer, value);
                }
            });
        }
    }
}
