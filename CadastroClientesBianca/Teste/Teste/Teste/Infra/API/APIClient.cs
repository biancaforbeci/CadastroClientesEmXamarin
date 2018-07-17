using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AppClientes.Infra.API;
using AppClientes.Infra.Services;
using AppClientes.Models;
using MyCouch;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using Prism.Services;

namespace AppClientes.Infra.Api
{
    public class APIClient : IApiClient
    {
        private readonly IFileSystem _fileSystem;
        public APIClient(IPageDialogService pageDialog, IFileSystem fileSystem)
        {
            _pageDialog = pageDialog;
            _fileSystem = fileSystem;
        }

        IPageDialogService _pageDialog;
        public string content { get; set; }
        protected HttpClient modernHttpClient { get; set; }
        private static string BaseCouchDbApiAddress = "http://localhost:5984";
        private static HttpClient Client = new HttpClient() { BaseAddress = new Uri(BaseCouchDbApiAddress) };
        private string sContentType = "application/json";

        public async Task<IEnumerable<Client>> GetAsync(string apiRoute)
        {
            try
            {
                var uri = new Uri(string.Format(apiRoute, string.Empty));

                var response = await API_Singleton.Instance.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<Client>>(content);
                }
                else
                {
                    await _pageDialog.DisplayAlertAsync("Ocorreu um erro", "Erro na solicitação HTTP: " + response, "OK");
                    return null;
                }
            }
            catch (Exception)
            {
                await _pageDialog.DisplayAlertAsync("Ocorreu um erro", "Tente novamente !", "OK");
                return null;
            }
        }

        public string Read_JSON()
        {
            return content;
        }

        public string CompressionGZIPAsync(string json)
        {
            return CompressionGZIP.CompressString(json);
        }

        private void InitHttpClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Remove("Accept-Encoding");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
        }

        public async void PostAsync(string files)
        {
            try
            {
                var uri = new Uri(string.Format("http://localhost:5984/my_db", string.Empty));
                var authResult = await Client.PostAsync(uri, new StringContent(files, Encoding.UTF8, sContentType));

                if (authResult.IsSuccessStatusCode)
                {
                    await _pageDialog.DisplayAlertAsync("Sucesso", " ", "OK");
                }
            }
            catch (Exception e)
            {
                await _pageDialog.DisplayAlertAsync("Erro", "Erro: " + e, "OK");
            }
        }
    }       
 }




