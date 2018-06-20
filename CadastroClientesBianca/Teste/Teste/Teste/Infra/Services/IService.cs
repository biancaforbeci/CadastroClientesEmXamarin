using AppClientes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppClientes.Infra.Services
{
    public interface IService
    {

        bool SaveClient(Client cli);

        bool DeleteClient(Client cli);

        Client SearchClient(int ID);

        List<Client> AgeListing();

        List<Client> AllClient();

        List<Client> SearchID(int ClientID);

        List<Client> SearchName(string ClientName);
    }
}
