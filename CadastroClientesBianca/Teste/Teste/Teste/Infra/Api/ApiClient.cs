using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AppClientes.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AppClientes.Infra.Api
{
    public class ApiClient : IApiClient
    {
        private HttpClient _restClient;
        private string _apiUrlBase;

        public ApiClient(string apiUrlBase)
        {
            if (string.IsNullOrEmpty(apiUrlBase))
            {
                throw new ArgumentNullException("apiUrlBase", "Uma url base de API deve ser informada");
            }

            _apiUrlBase = apiUrlBase;
        }

        public void Dispose()
        {
           
        }

        public async Task<List<Client>> GetAsync<T>(string apiRoute, Action<Task<List<Client>>> callback = null)
        {
            try
            {
                var json = await GetAsync(apiRoute);
                var data = JsonConvert.DeserializeObject<Client>(json, GetConverter());
                var result = new OkApiResult<Client>(data);

                callback?.Invoke(result);

                return result;
            }
            catch (Exception ex)
            {
                return new InvalidApiResult<Client>(ex);
            }
        }

        public async Task<string> PostAsync<Client>(string apiRoute, object body = null, Action<Task<string>> callback = null)
        {
            var url = _apiUrlBase + "/" + apiRoute;

            _restClient = _restClient ?? new HttpClient();
            _restClient.BaseAddress = new Uri(url);

            ClearReponseHeaders();
            AddReponseHeaders();

            var bodySerialize = JsonConvert.SerializeObject(body);
            StringContent content = new StringContent(bodySerialize, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(_restClient.BaseAddress, content);
            response.EnsureSuccessStatusCode();
            var data = response.Content.ReadAsStringAsync().Result;

            return data;
        }

        private void AddReponseHeaders()
        {
           
        }

        private void ClearReponseHeaders()
        {
            _restClient.DefaultRequestHeaders.Clear();
        }

        public async Task<List<Client>> PostResultAsync<Client>(string apiRoute, object body = null, Action<Task<List<Client>>> callback = null)
        {
            try
            {
                var data = await PostAsync(apiRoute, body);
                var result = JsonConvert.DeserializeObject<OkApiResult<Client>>(data, GetConverter());

                callback?.Invoke(result);

                return result;
            }
            catch (Exception ex)
            {
                return new InvalidApiResult<Client>(ex);
            }
        }

        private IsoDateTimeConverter GetConverter()
        {
            return new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
        }

        public IApiClient UseSufix(string urlSufix)
        {
            if (!_apiUrlBase.EndsWith(urlSufix))
            {
                _apiUrlBase = _apiUrlBase + "/" + urlSufix;
            }
            return this;
        }

        private async Task<string> GetAsync(string apiRoute)
        {
            var url = _apiUrlBase + "/" + apiRoute;

            _restClient = _restClient ?? new HttpClient();
            _restClient.BaseAddress = new Uri(url);

            ClearReponseHeaders();
            AddReponseHeaders();

            var response = await _restClient.GetAsync(_restClient.BaseAddress);
            response.EnsureSuccessStatusCode();
            var data = response.Content.ReadAsStringAsync().Result;

            return data;
        }
    }
}
