using AppClientes.DAL;
using AppClientes.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppClientes.Infra.Services
{
    public class ClientService : IService
    {        
        DatabaseContext _databaseContext;
        
        public ClientService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;     
        }    
        
        public List<Client> AgeListing()
        {

            var listaord = (from x in _databaseContext.Clients
                            orderby x.Age
                            select x).ToList();

            return listaord;
        }

        public List<Client> SearchID(int ClientID)
        {
            var busca = (from c in _databaseContext.Clients
                         where c.ClientID.Equals(ClientID)
                         select c).ToList();

            return busca;
        }

        public List<Client> SearchName(string ClientName)
        {
            var busca = (from c in _databaseContext.Clients
                         where c.Name.ToLower().Equals(ClientName)
                         select c).ToList();

            return busca;
        }

        public bool SaveClient(Client cli)
        {
            try
            {
                _databaseContext.Clients.Add(cli);
                _databaseContext.SaveChanges();
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
                using (_databaseContext)
                {
                    _databaseContext.Entry(c).State = EntityState.Deleted;
                    _databaseContext.Dispose();                    
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
            return _databaseContext.Clients.Find(id);
        }

         public List<Client> AllClient()
        {
            return _databaseContext.Clients.ToList();
        }
    }
}
