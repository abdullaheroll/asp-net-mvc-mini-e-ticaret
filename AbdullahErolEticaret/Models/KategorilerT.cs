using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbdullahErolEticaret.Models
{
    public class KategorilerT
    {
        public int KategoriID { get; set; }
        public string Baslik { get; set; }
        public string KategoriAciklamasi { get; set; }
        public virtual ICollection<UrunlerT> UrunlerT { get; set; }
    }
}