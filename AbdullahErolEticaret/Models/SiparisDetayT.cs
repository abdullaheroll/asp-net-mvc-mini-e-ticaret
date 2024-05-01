using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbdullahErolEticaret.Models
{
    public class SiparisDetayT
    {
        public int DetayID { get; set; }
        public int SiparisID { get; set; }
        public decimal SiparisFiyati { get; set; }
        public string FaturaAdresi { get; set; }
        public string TeslimatAdresi { get; set; }
    }
}