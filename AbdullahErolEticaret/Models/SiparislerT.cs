using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbdullahErolEticaret.Models
{
    public class SiparislerT
    {
        //public int SiparisID { get; set; }
        //public int UrunID { get; set; }


        public int SiparisID { get; set; }
        public string UrunAdi { get; set; }
        public decimal SatisFiyati { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public string TeslimatAdresi { get; set; }
        public string FaturaAdresi { get; set; }
        public string Durum { get; set; }
        public string Isim { get; set; }
        public string Eposta { get; set; }
        public string FaturaURL { get; set; }
    }
}