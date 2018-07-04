using AppClientes.Infra.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AppClientes.Infra.Services
{
    public sealed class API_Singleton
    {
        private static object syncRoot = new object();
        private API_Singleton()
        {

        }

        private static HttpClient _instance;

        public static HttpClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        _instance = new HttpClient();
                    }
                }

                return _instance;
            }
        }
    }
}