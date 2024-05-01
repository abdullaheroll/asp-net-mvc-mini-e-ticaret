using AbdullahErolEticaret.App_Classes;
using AbdullahErolEticaret.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbdullahErolEticaret.Controllers
{
    public class YtUrunController : Controller
    {
        // GET: YtUrun
        Baglanti baglanti = new Baglanti();
        EticaretDB db;
        public ActionResult Index()
        {
            string urunler = "SELECT u.UrunID,u.UrunAdi,u.UrunAciklamasi,u.Fiyat,u.StokAdet,u.UrunGorselURL,k.KategoriAdi from urunlert u" +
                " inner JOIN kategorilert k on u.KategoriID=k.KategoriID ";
            using (MySqlCommand cmd = new MySqlCommand(urunler, baglanti.Open()))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    List<UrunlerT> urunlist = new List<UrunlerT>();
                    while (reader.Read())
                    {
                        UrunlerT urun = new UrunlerT();
                        urun.urunID = int.Parse(reader["UrunID"].ToString());
                        urun.UrunAdi = reader["UrunAdi"].ToString();
                        urun.UrunAciklamasi = reader["UrunAciklamasi"].ToString();
                        urun.Fiyat = decimal.Parse(reader["Fiyat"].ToString());
                        urun.StokAdet = int.Parse(reader["StokAdet"].ToString());
                        urun.UrunGorselURL = reader["UrunGorselURL"].ToString();
                        urun.KategoriAdi = reader["KategoriAdi"].ToString();
                        urunlist.Add(urun);

                    }
                    baglanti.Close();
                    return View(urunlist);

                }
            }

        }

        public ActionResult Ekle()
        {
            // Kategori id ve kategori adını çekmek için sorgu
            string sorgu = "select KategoriID, KategoriAdi from kategorilert";
            // Kategori id ve kategori adını tutmak için bir liste oluşturma
            List<SelectListItem> kategoriler = new List<SelectListItem>();
            // Bağlantıyı aç
            using (MySqlConnection conn = baglanti.Open())
            {
                // Sorguyu çalıştır
                using (MySqlCommand cmd = new MySqlCommand(sorgu, conn))
                {
                    // Sorgu sonucunu oku
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        // Her bir satır için
                        while (dr.Read())
                        {
                            // Kategori id ve kategori adını al
                            int kategoriId = dr.GetInt32("KategoriID");
                            string kategoriAdi = dr.GetString("KategoriAdi");
                            // Kategori id ve kategori adını listeye ekle
                            kategoriler.Add(new SelectListItem { Value = kategoriId.ToString(), Text = kategoriAdi });
                        }
                    }
                }
            }
            // Listeyi viewbag içinde sakla
            ViewBag.kategorilist = kategoriler;
            return View();

        }

        [HttpPost]
        public ActionResult Ekle(UrunlerT urun, HttpPostedFileBase Gorsel)
        {
            //resim yükleme işlemi
            Image img = Image.FromStream(Gorsel.InputStream);
            Bitmap bmp = new Bitmap(img, 600, 700); //orjinal boyuttaki görseli alıp 600x700 şeklinde kırpıyorum.
            string resimyolu = "/Gorsel/" + Guid.NewGuid() + Path.GetExtension(Gorsel.FileName); //gorsel klasörüne random bir isimle kayıt etme
            bmp.Save(Server.MapPath(resimyolu)); //görseli klasöre kayıt etme

            // Kategori id ve kategori adını çekmek için sorgu
            string sorgu1 = "select KategoriID, KategoriAdi from kategorilert";
            // Kategori id ve kategori adını tutmak için bir liste oluşturma
            List<SelectListItem> kategoriler = new List<SelectListItem>();
            // Bağlantıyı aç
            using (MySqlConnection conn = baglanti.Open())
            {
                // Sorguyu çalıştır
                using (MySqlCommand cmd1 = new MySqlCommand(sorgu1, conn))
                {
                    // Sorgu sonucunu oku
                    using (MySqlDataReader dr = cmd1.ExecuteReader())
                    {
                        // Her bir satır için
                        while (dr.Read())
                        {
                            // Kategori id ve kategori adını al
                            int katId = dr.GetInt32("KategoriID");
                            string katAdi = dr.GetString("KategoriAdi");
                            // Kategori id ve kategori adını listeye ekle
                            kategoriler.Add(new SelectListItem { Value = katId.ToString(), Text = katAdi });
                        }
                    }
                }
                // Listeyi viewbag içinde sakla
                ViewBag.kategorilist = kategoriler;

                // Ürün eklemek için sorgu
                string sorgu2 = "insert into urunlert(UrunAdi,UrunAciklamasi,Fiyat,StokAdet,KategoriID,UrunGorselURL)" +
                    "values('" + urun.UrunAdi + "','" + urun.UrunAciklamasi + "','" + urun.Fiyat + "','" + urun.StokAdet + "','" + urun.KategoriID + "','" + resimyolu + "') ";
                // Sorguyu çalıştır
                using (MySqlCommand cmd2 = new MySqlCommand(sorgu2, conn))
                {
                    int sonuc = cmd2.ExecuteNonQuery(); // sorguyu execute et ve etkilenen kayıt sayısını al
                    if (sonuc == 1) // eğer etkilenen kayıt varsa
                    {
                        ViewBag.msg = "<div class='alert alert-success'>Ürün Eklendi.</div>";

                    }
                    else // eğer etkilenen kayıt yoksa
                    {
                        ViewBag.msg = "<div class='alert alert-success'>Ürün Eklenmedi!.</div>";
                    }
                }
            }
            // Bağlantıyı kapat
            baglanti.Close();
            // View'e dön
            return View(urun);

        }

        public ActionResult Duzenle(int id)
        {
            // Kategori id ve kategori adını çekmek için sorgu
            string kategori = "select KategoriID, KategoriAdi from kategorilert";
            // Kategori id ve kategori adını tutmak için bir liste oluşturma
            List<SelectListItem> kategoriler = new List<SelectListItem>();
            // Bağlantıyı aç
            // Sorguyu çalıştır
            using (MySqlCommand cmd = new MySqlCommand(kategori, baglanti.Open()))
            {
                // Sorgu sonucunu oku
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    // Her bir satır için
                    while (dr.Read())
                    {
                        // Kategori id ve kategori adını al
                        int katId = dr.GetInt32("KategoriID");
                        string katAdi = dr.GetString("KategoriAdi");
                        // Kategori id ve kategori adını listeye ekle
                        kategoriler.Add(new SelectListItem { Value = katId.ToString(), Text = katAdi });
                    }
                }
            }
            // Listeyi viewbag içinde sakla
            ViewBag.kategorilist = kategoriler;



            //ürünleri çek
            UrunlerT urunlerT = new UrunlerT();
            string urunler = "select*from urunlert where UrunID=" + id + "";
            using (MySqlCommand cmd = new MySqlCommand(urunler, baglanti.Open()))
            {

                using (MySqlDataReader urun = cmd.ExecuteReader())
                {
                    while (urun.Read())
                    {
                        urunlerT.urunID = int.Parse(urun["UrunID"].ToString());
                        urunlerT.UrunAdi = urun["UrunAdi"].ToString();
                        urunlerT.UrunAciklamasi = urun["UrunAciklamasi"].ToString();
                        urunlerT.Fiyat = decimal.Parse(urun["Fiyat"].ToString());
                        urunlerT.StokAdet = int.Parse(urun["StokAdet"].ToString());
                    }

                }
            }

            return View(urunlerT);
        }

        [HttpPost]
        public ActionResult Duzenle(int id, UrunlerT urunler, HttpPostedFileBase Gorsel)
        {
            string urunguncelle = "";
            // Kategori id ve kategori adını çekmek için sorgu
            string kategori = "select KategoriID, KategoriAdi from kategorilert";
            // Kategori id ve kategori adını tutmak için bir liste oluşturma
            List<SelectListItem> kategoriler = new List<SelectListItem>();
            // Bağlantıyı aç
            // Sorguyu çalıştır
            using (MySqlCommand cmd = new MySqlCommand(kategori, baglanti.Open()))
            {
                // Sorgu sonucunu oku
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    // Her bir satır için
                    while (dr.Read())
                    {
                        // Kategori id ve kategori adını al
                        int katId = dr.GetInt32("KategoriID");
                        string katAdi = dr.GetString("KategoriAdi");
                        // Kategori id ve kategori adını listeye ekle
                        kategoriler.Add(new SelectListItem { Value = katId.ToString(), Text = katAdi });
                    }
                }
            }
            // Listeyi viewbag içinde sakla
            ViewBag.kategorilist = kategoriler;

            //ürün bilgilerini güncelleme


            //resim düzenleme
            //ürünleri çekme, ürün görseli için

            if (Gorsel != null)
            {
                string urunresim = "select UrunGorselURL from urunlert where UrunID=" + id + "";
                using (MySqlCommand cmd = new MySqlCommand(urunresim, baglanti.Open()))
                {

                    using (MySqlDataReader urunresimdr = cmd.ExecuteReader())
                    {
                        while (urunresimdr.Read())
                        {
                            if (System.IO.File.Exists(Server.MapPath(urunresimdr["UrunGorselURL"].ToString())))
                            {
                                System.IO.File.Delete(Server.MapPath(urunresimdr["UrunGorselURL"].ToString()));
                            }
                        }

                    }
                }

                Image img = Image.FromStream(Gorsel.InputStream);
                Bitmap bmp = new Bitmap(img, 600, 700);
                string resimyolu = "/Gorsel/" + Guid.NewGuid() + Path.GetExtension(Gorsel.FileName); //gorsel klasörüne random bir isimle kayıt etme
                bmp.Save(Server.MapPath(resimyolu)); //görseli klasöre kayıt etme
                                                     //

                urunguncelle = "update urunlert set UrunAdi='" + urunler.UrunAdi + "', UrunAciklamasi='" + urunler.UrunAciklamasi + "'," +
          "Fiyat='" + urunler.Fiyat + "', StokAdet='" + urunler.StokAdet + "', KategoriID='" + urunler.KategoriID + "'," +
          "UrunGorselURL='" + resimyolu + "' where UrunID=" + id + "";

            }
            else
            {

                urunguncelle = "update urunlert set UrunAdi='" + urunler.UrunAdi + "', UrunAciklamasi='" + urunler.UrunAciklamasi + "'," +
          "Fiyat='" + urunler.Fiyat + "', StokAdet='" + urunler.StokAdet + "', KategoriID='" + urunler.KategoriID + "' where UrunID=" + id + "";
            }


            using (MySqlCommand cmd2 = new MySqlCommand(urunguncelle, baglanti.Open()))
            {
                int durum = cmd2.ExecuteNonQuery();
                if (durum == 1)
                {
                    ViewBag.msg = "<div class='alert alert-success'>Ürün Güncellendi.</div>";
                }
                else
                {
                    ViewBag.msg = "<div class='alert alert-danger'>Ürün Güncellenemedi.</div>";
                }
            }
            return View(urunler);
        }

        public JsonResult Sil(int id)
        {
            string resimvt = "";
            string resimyolu = "select UrunGorselURL from urunlert where UrunID=" + id + "";
            using (MySqlCommand cmd = new MySqlCommand(resimyolu, baglanti.Open()))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resimvt = reader["UrunGorselURL"].ToString();
                    }
                }
            }
            if (System.IO.File.Exists(Server.MapPath(resimvt)))
            {
                System.IO.File.Delete(Server.MapPath(resimvt));
            }

            string sil = "delete from urunlert where UrunID= " + id + " ";
            using (MySqlCommand cmd2 = new MySqlCommand(sil, baglanti.Open()))
            {
                int durum = cmd2.ExecuteNonQuery(); // 1 ise işlem başarılı 0 ise başarısız
                if (durum == 1)
                {
                    return Json(JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonRequestBehavior.DenyGet);
                }
            }
        }

    }
}