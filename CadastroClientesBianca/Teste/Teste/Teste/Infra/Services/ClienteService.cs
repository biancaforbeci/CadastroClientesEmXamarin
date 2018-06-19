using System;
using System.Collections.Generic;
using System.Text;
using AppClientes.Models;

namespace AppClientes.Infra.Services
{
    public class ClienteService : IService
    {
        public List<Cliente> ListagemGeral()
        {
            return contexto.Clientes.ToList();
        }

        public List<Cliente> ListagemPorIdade()
        {
            var listaord = (from x in contexto.Clientes
                            orderby x.Idade
                            select x).ToList();

            return listaord;
        }

        public List<Cliente> ProcuraPorID(int ClienteID)
        {
            var busca = (from c in contexto.Clientes
                         where c.ClienteID.Equals(ClienteID)
                         select c).ToList();

            return busca;
        }

        public List<Cliente> ProcuraPorNome(string ClienteNome)
        {
            var busca = (from c in contexto.Clientes
                         where c.Nome.ToLower().Equals(ClienteNome)
                         select c).ToList();

            return busca;
        }

        public bool SalvaBanco(Cliente cli)
        {
            try
            {
                contexto.Add(cli);
                contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ExcluirCliente(Cliente c)
        {
            try
            {
                contexto.Entry(c).State = EntityState.Deleted;
                contexto.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }           
        }

    }
}
