using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
//using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppClientes.Models
{
    [DataContract]
    [KnownType(typeof(Client))]
    public class Client
    {
        [Key]
        [DataMember]
        public int ClientID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Age { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string PathPhoto { get; set; }
    }    
}
