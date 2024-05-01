using AbdullahErolEticaret.App_Classes;
using AbdullahErolEticaret.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbdullahErolEticaret.Controllers
{
    public class YtPanelController : Controller
    {
        // GET: YtPanel
        Baglanti baglanti = new Baglanti();
        public ActionResult Index()
        {
            string toplamSiparis = "select count(SiparisID) as SiparisID from siparislert";
            string toplamUrunler = "select count(UrunID) as UrunID from urunlert";
            string toplamKullanicilar = "select count(KullaniciID) as KullaniciID from kullanicilart";
            string toplamSatis = "select SUM(SatisFiyati) as SatisFiyati from siparisdetayt";

            //Toplam sipariş adeti

            using (MySqlCommand cmd = new MySqlCommand(toplamSiparis, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ViewBag.topSiparis = dr["SiparisID"];
                    }
                }
            }

            //Toplam kayıtlı ürün adeti

            using (MySqlCommand cmd = new MySqlCommand(toplamUrunler, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ViewBag.topUrun = dr["UrunID"];
                    }
                }
            }

            //Toplam kayıtlı kullanıcı adeti
            using (MySqlCommand cmd = new MySqlCommand(toplamKullanicilar, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ViewBag.topKullanici = dr["KullaniciID"];
                    }
                }
            }

            //Toplam Satış turatı belirleme
            using (MySqlCommand cmd = new MySqlCommand(toplamSatis, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ViewBag.topSatis = dr["SatisFiyati"];
                    }
                }
            }

            return View();
        }

        public PartialViewResult PartialSonSiparisler()
        {
            //4 farklı tabloyu birleştirip tarihe göre sıralayıp son siparişleri görme
            List<SiparislerT> siparisler = new List<SiparislerT>();
            string siparisSorgu = "SELECT s.SiparisID, u.UrunAdi, d.SatisFiyati, s.OlusturmaTarihi, d.TeslimatAdresi, d.FaturaAdresi, s.Durum," +
    "k.Isim,k.Eposta " +
    "FROM siparislert s " +
    "INNER JOIN siparisdetayt d ON s.SiparisID = d.SiparisID " +
    "INNER JOIN urunlert u ON u.UrunID = s.UrunID INNER JOIN kullanicilart k on k.KullaniciID=s.KullaniciID ORDER BY s.OlusturmaTarihi DESC;";
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
            return PartialView(siparisler);
        }
    }
}