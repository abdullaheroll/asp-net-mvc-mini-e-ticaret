using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbdullahErolEticaret.Models
{
    public class IletisimT
    {
        public int MesajID { get; set; }
        public string Isim { get; set; }
        public string Eposta { get; set; }
        public string MesajIcerik { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public string IpAdresi { get; set; }
    }
}