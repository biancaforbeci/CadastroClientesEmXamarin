using AppClientes.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppClientes.Infra.Api
{
    public interface IApiClient
    {
        Task<IEnumerable<Client>> GetAsync(string apiRoute);
        string Read_JSON();
    }
}