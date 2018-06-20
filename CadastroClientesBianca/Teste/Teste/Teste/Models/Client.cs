using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppClientes.Models
{
    public class Client
    {
        [Key]
        public int ClientID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
    }
}
