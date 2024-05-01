using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbdullahErolEticaret.Models
{
    public class UrunlerT
    {
        public int urunID { get; set; }
        public string UrunAdi { get; set; }
        public string UrunAciklamasi { get; set; }
        public decimal Fiyat { get; set; }
        public int StokAdet { get; set; }
        public string UrunGorselURL { get; set; }
        public int KategoriID { get; set; }
        public string KategoriAdi { get; set; }
        public string KategoriAciklamasi { get; set; }
    }
}