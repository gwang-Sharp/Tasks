using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceDAL
{
    public class users
    {
        public string id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public string gflag { get; set; }
        public string cardId { get; set; }
        public string realname { get; set; }
        public string gender { get; set; }
        public string birthdate { get; set; }
        public string phone { get; set; }
        public string employer { get; set; }
        public string nationalId { get; set; }
        public string addr { get; set; }
        public string iprange { get; set; }
        public bool state { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}