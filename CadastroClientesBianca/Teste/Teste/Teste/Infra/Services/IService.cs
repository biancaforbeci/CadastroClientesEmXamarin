using AppClientes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppClientes.Infra.Services
{
    public interface IService
    {

        bool SalvaBanco(Cliente cli);

        List<Cliente> ListagemPorIdade();

        List<Cliente> ListagemGeral();

        List<Cliente> ProcuraPorID(int ClienteID);

        List<Cliente> ProcuraPorNome(string ClienteNome);
    }
}
