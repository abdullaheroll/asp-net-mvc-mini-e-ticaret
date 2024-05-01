using AbdullahErolEticaret.App_Classes;
using AbdullahErolEticaret.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Cursor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbdullahErolEticaret.Controllers
{
    public class YtSiparislerController : Controller
    {
        // GET: YtSiparisler
        Baglanti baglanti = new Baglanti();
        public ActionResult Index()
        {
            //4 farklı tabloyu birleştirip tüm kullanıcıların siparişlerini görebiliyorum.
            List<SiparislerT> siparisler = new List<SiparislerT>();
            string siparisSorgu = "SELECT s.SiparisID, u.UrunAdi, d.SatisFiyati, s.OlusturmaTarihi, d.TeslimatAdresi, d.FaturaAdresi, s.Durum," +
    "k.Isim,k.Eposta " +
    "FROM siparislert s " +
    "INNER JOIN siparisdetayt d ON s.SiparisID = d.SiparisID " +
    "INNER JOIN urunlert u ON u.UrunID = s.UrunID INNER JOIN kullanicilart k on k.KullaniciID=s.KullaniciID";
            using (MySqlCommand cmd = new MySqlCommand(siparisSorgu, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        siparisler.Add(new SiparislerT()
                        {
                            SiparisID = int.Parse(dr["SiparisID"].ToString()),
                            UrunAdi = dr["UrunAdi"].ToString(),
                            SatisFiyati = decimal.Parse(dr["SatisFiyati"].ToString()),
                            OlusturmaTarihi = DateTime.Parse(dr["OlusturmaTarihi"].ToString()),
                            TeslimatAdresi = dr["TeslimatAdresi"].ToString(),
                            FaturaAdresi = dr["FaturaAdresi"].ToString(),
                            Durum = dr["Durum"].ToString(),
                            Isim = dr["Isim"].ToString(),
                            Eposta = dr["Eposta"].ToString()
                        });

                    }
                }
            }
            return View(siparisler);
        }


        public ActionResult SipDurum(int id)
        {
            string siparisdurum = "select SiparisID,Durum,Isim from siparislert s inner join kullanicilart k on s.KullaniciID=k.KullaniciID where SiparisID=" + id + "";
            SiparislerT siparisler = new SiparislerT();
            using (MySqlCommand cmd = new MySqlCommand(siparisdurum, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        siparisler.SiparisID = int.Parse(dr["SiparisID"].ToString());
                        siparisler.Durum = dr["Durum"].ToString();
                        siparisler.Isim = dr["Isim"].ToString();
                    }
                }
                return View(siparisler);

            }
        }
        [HttpPost]
 
        public ActionResult SipDurum(int id, SiparislerT siparisler)
        {
            string spdurum = "update siparislert set Durum=" + siparisler.Durum + " where SiparisID=" + id + "";
            using (MySqlCommand cmd = new MySqlCommand(spdurum, baglanti.Open()))
            {
                int durum = cmd.ExecuteNonQuery();
                if (durum == 1)
                {
                    ViewBag.msg = "<div class='alert alert-success'>Kullanıcın Sipariş Durumu Değiştirildi.</div>";
                }
                else
                {
                    ViewBag.msg = "<div class='alert alert-danger'>Kullanıcın Sipariş Durumu Değiştirilemedi.</div>";
                }
            }
            return View(siparisler);
        }

        public ActionResult Fatura(int id)
        {
            string siparisdurum = "select SiparisID,Durum,Isim from siparislert s inner join kullanicilart k on s.KullaniciID=k.KullaniciID where SiparisID=" + id + "";
            SiparislerT siparisler = new SiparislerT();
            using (MySqlCommand cmd = new MySqlCommand(siparisdurum, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        siparisler.SiparisID = int.Parse(dr["SiparisID"].ToString());
                        siparisler.Durum = dr["Durum"].ToString();
                        siparisler.Isim = dr["Isim"].ToString();
                    }
                }
                return View(siparisler);

            }
        }

        [HttpPost]
        public ActionResult Fatura(int id, HttpPostedFileBase FaturaURL)
        {
            //kullanıcnın oluşturulmuş olduğu siparişe fatura tanımlama
            string faturaYolu = "/Fatura/" + Guid.NewGuid() + Path.GetExtension(FaturaURL.FileName);
            FaturaURL.SaveAs(Server.MapPath(faturaYolu));
            string faturaKaydet = "update siparislert set FaturaURL='" + faturaYolu + "' where SiparisID=" + id + "";
            using (MySqlCommand cmd = new MySqlCommand(faturaKaydet, baglanti.Open()))
            {
                int durum = cmd.ExecuteNonQuery();
                if (durum == 1)
                {
                    ViewBag.msg = "<div class='alert alert-success'>Siparişe fatura tanımlanmıştır.</div>";
                }
                else
                {
                    ViewBag.msg = "<div class='alert alert-danger'>Fatura oluşturulamadı.</div>";
                }
            }

            string siparisdurum = "select Isim from siparislert s inner join kullanicilart k on s.KullaniciID=k.KullaniciID where SiparisID=" + id + "";
            SiparislerT siparisler = new SiparislerT();
            using (MySqlCommand cmd = new MySqlCommand(siparisdurum, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        siparisler.Isim = dr["Isim"].ToString();
                    }
                }
                return View(siparisler);

            }
        }
    }
}