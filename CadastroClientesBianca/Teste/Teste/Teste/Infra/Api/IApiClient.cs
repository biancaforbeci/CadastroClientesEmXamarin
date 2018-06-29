using AppClientes.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppClientes.Infra.Api
{
    public interface IApiClient : IDisposable
    {
        Task<List<Client>> GetAsync<T>(string apiRoute, Action<Task<List<Client>>> callback = null);
        IApiClient UseSufix(string urlSufix);
        Task<List<Client>> PostResultAsync<Client>(string apiRoute, object body = null, Action<Task<List<Client>>> callback = null);
        Task<string[]> PostAsync<Client>(string apiRoute, object body = null, Action<Task<string[]>> callback = null);
    }
}
