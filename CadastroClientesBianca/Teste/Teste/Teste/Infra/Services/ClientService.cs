using AppClientes.DAL;
using AppClientes.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppClientes.Infra.Services
{
    public class ClientService : IDownData
    {
        private readonly IDownData database;

        public ClientService(IDownData Database)
        {
            database = Database;     
        }
     
        public List<Client> All()
        {
            return database.ConexaoBanco().Clients.ToList();
        }

        public List<Client> AgeListing()
        {

            var listaord = (from x in database.ConexaoBanco().Clients
                            orderby x.Age
                            select x).ToList();

            return listaord;
        }

        public List<Client> SearchID(int ClientID)
        {
            var busca = (from c in database.ConexaoBanco().Clients
                         where c.ClientID.Equals(ClientID)
                         select c).ToList();

            return busca;
        }

        public List<Client> SearchName(string ClientName)
        {
            var busca = (from c in database.ConexaoBanco().Clients
                         where c.Name.ToLower().Equals(ClientName)
                         select c).ToList();

            return busca;
        }

        public bool SaveClient(Client cli)
        {
            try
            {
                database.ConexaoBanco().Clients.Add(cli);
                database.ConexaoBanco().SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteClient(Client c)
        {
            try
            {
                using (database.ConexaoBanco())
                {
                    database.ConexaoBanco().Entry(c).State = EntityState.Deleted;
                    database.ConexaoBanco().SaveChanges();
                }
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public Client SearchClient(int id)
        {

            return database.ConexaoBanco().Clients.Find(id);

        }

        public DatabaseContext ConexaoBanco()
        {
            DatabaseContext context = new DatabaseContext();
            return context;
        }
    }
}
