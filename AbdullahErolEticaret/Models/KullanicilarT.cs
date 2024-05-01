using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbdullahErolEticaret.Models
{
    public class KullanicilarT
    {
        public int KullaniciID  { get; set; }
        public string Isim { get; set; }
        public string Eposta { get; set; }
        public string Parola { get; set; }
        public bool Aktiflik { get; set; }
        public DateTime KayitTarihi { get; set; }
        public int RolID { get; set; }
        public string TeslimatAdresi { get; set; }
        public string FaturaAdresi { get; set; }
    }
}