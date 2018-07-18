using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.Media;
using AppClientes.Infra.API;
using AppClientes.Infra.Services;
using AppClientes.Models;
using DotNetXmlHttpRequest;
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
        private string sContentType = "application/json";
        private MediaFile _mediaFile;
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
            //client.DefaultRequestHeaders.Remove("Accept-Encoding");
            //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
        }

        public async void PostAsync(List<Client> listCli)
        {
            try
            {

                HttpResponseMessage response=null;
                HttpClient client = API_Singleton.Instance;
                var uri = new Uri(string.Format("http://admin:admin@10.108.70.125:5984/_gzip/my_db", string.Empty));

                foreach (var item in listCli)
                {
                    try
                    {                        
                        MultipartFormDataContent form = new MultipartFormDataContent();
                        form.Add(new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, sContentType));
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(item.PathPhoto);
                        form.Add(new ByteArrayContent byteContent = new ByteArrayContent(data));
                        response = await client.PostAsync(uri, form);

                    }
                    catch (Exception e)
                    {
                        await _pageDialog.DisplayAlertAsync("Ocorreu erro", "Erro em enviar o item:" + item, "OK");
                    }
                }

                //response = await client.PostAsync(uri, new StringContent(files, Encoding.UTF8, sContentType));                

                string contentStr = await response.Content.ReadAsStringAsync();
                               
                await _pageDialog.DisplayAlertAsync("Resposta servidor", " " + contentStr, "OK");
                
            }
            catch (Exception e)
            {
                await _pageDialog.DisplayAlertAsync("Erro", "Erro: " + e, "OK");
            }
        }
    }       
 }




