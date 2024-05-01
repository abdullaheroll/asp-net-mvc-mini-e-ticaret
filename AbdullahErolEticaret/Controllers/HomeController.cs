using AbdullahErolEticaret.App_Classes;
using AbdullahErolEticaret.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace AbdullahErolEticaret.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        Baglanti baglanti = new Baglanti(); //Yeni bağlantı nesnesi türetme

        //tüm ürünleri listeleme
        public ActionResult Index()
        {
            string urunler = "select UrunID,UrunAdi,UrunAciklamasi,Fiyat,UrunGorselURL from urunlert";
            using (MySqlCommand cmd = new MySqlCommand(urunler, baglanti.Open()))
            {
                List<UrunlerT> UrunlerT = new List<UrunlerT>();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        UrunlerT urun = new UrunlerT();
                        urun.urunID = int.Parse(dr["UrunID"].ToString());
                        urun.UrunAdi = dr["UrunAdi"].ToString();
                        urun.UrunAciklamasi = dr["UrunAciklamasi"].ToString();
                        urun.Fiyat = decimal.Parse(dr["Fiyat"].ToString());
                        urun.UrunGorselURL = dr["UrunGorselURL"].ToString();
                        UrunlerT.Add(urun);
                    }
                }
                return View(UrunlerT);
            }




        }


        public ActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Giris(KullanicilarT kullanicilar)
        {
            //string dnmhash = Crypto.Hash(kullanicilar.Parola, "md5");
            string email = "";
            string sifre = "";
            int Rolid = 0;
            int kullaniciId = 0;
            string sif = Crypto.Hash(kullanicilar.Parola, "md5");
            bool aktiflik = false;
            bool varMi = false;
            string giris = "select*from kullanicilart where Eposta='" + kullanicilar.Eposta + "'";
            using (MySqlCommand cmd = new MySqlCommand(giris, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        kullaniciId = int.Parse(dr["KullaniciID"].ToString());
                        email = dr["Eposta"].ToString();
                        sifre = dr["Parola"].ToString();
                        Rolid = int.Parse(dr["RolID"].ToString());
                        aktiflik = bool.Parse(dr["Aktiflik"].ToString());
                        varMi = true;
                    }
                }
            }
            if (varMi == true)
            {
                if (aktiflik == false)
                {
                    ViewBag.msg = "<div class='alert alert-danger'>Blokeli hesap.</div";
                    return View();

                }
                if (email == kullanicilar.Eposta && sifre == Crypto.Hash(kullanicilar.Parola, "md5"))
                {
                    Session["kullaniciId"] = kullaniciId;
                    Session["email"] = email;
                    Session["rolid"] = Rolid;
                    return RedirectToAction("index", "home");
                }
                else
                {
                    ViewBag.msg = "<div class='alert alert-danger'>Giriş bilgilerinizi kontrol ediniz.</div";
                }
            }
            else
            {
                ViewBag.msg = "<div class='alert alert-danger'>Böyle bir hesap bulunmamaktadır.</div";
            }

            return View(kullanicilar);
        }
        [HttpPost]
        public JsonResult Kayit(KullanicilarT kullanicilar)
        {
            //Rol=1 Yönetici,Rol=2 Kullanıcı Rol=3 Süper Kullanıcı

            string epostakontrol = "select*from kullanicilart where Eposta='" + kullanicilar.Eposta + "'";
            using (MySqlCommand cmd = new MySqlCommand(epostakontrol, baglanti.Open()))
            {
                int durum = Convert.ToInt32(cmd.ExecuteScalar());
                if (durum > 1)
                {
                    Response.StatusCode = 404;
                    return Json(new
                    {
                        message = "Bu eposta sisteme zaten kayıtlı.",
                        success = false
                    });
                }
                else
                {
                    string sifrehash = Crypto.Hash(kullanicilar.Parola, "md5");
                    string kullanicikayit = "insert into kullanicilart(Isim,Eposta,Parola,Aktiflik,KayitTarihi,RolID) " +
                        "values('" + kullanicilar.Isim + "','" + kullanicilar.Eposta + "','" + sifrehash + "','1','" + DateTime.Now.ToString("yyyy-MM-dd") + "','2')";
                    using (MySqlCommand cmd2 = new MySqlCommand(kullanicikayit, baglanti.Open()))
                    {
                        int durum2 = cmd2.ExecuteNonQuery();
                        if (durum2 == 1)
                        {
                            return Json(new
                            {
                                message = "Kayıt işlemi başarılı, yönlendiriliyorsunuz...",
                                success = true,
                                redirectUrl = "/home/giris"
                            });
                        }
                        else
                        {
                            Response.StatusCode = 404;
                            return Json(new
                            {
                                message = "Kayıt işlemi başarısız...",
                                success = false
                            });
                        }
                    }
                }
            }

        }
        public ActionResult Cikis()
        {
            Session.Abandon();
            Session["kullaniciId"] = null;
            Session["email"] = null;
            return RedirectToAction("index", "home");
        }

        public ActionResult Hesabim()
        {
            int kullaniciId = Convert.ToInt32(Session["kullaniciId"]);
            if (kullaniciId == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            KullanicilarT kullaniciverigoster = new KullanicilarT();

            string hesapayarlari = "select Isim,Eposta,TeslimatAdresi,FaturaAdresi, KayitTarihi from kullanicilart " +
                "where KullaniciID =" + kullaniciId + "";
            using (MySqlCommand cmd = new MySqlCommand(hesapayarlari, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        kullaniciverigoster.Isim = dr["Isim"].ToString();
                        kullaniciverigoster.Eposta = dr["Eposta"].ToString();
                        kullaniciverigoster.TeslimatAdresi = dr["TeslimatAdresi"].ToString();
                        kullaniciverigoster.FaturaAdresi = dr["FaturaAdresi"].ToString();
                        kullaniciverigoster.KayitTarihi = DateTime.Parse(dr["KayitTarihi"].ToString());

                    }
                }
            }
            return View(kullaniciverigoster);

        }

        [HttpPost]
        public JsonResult HesabimKaydet(KullanicilarT kullanicilar)
        {
            //kullanıcının kişisel bilgilerini düzenlediği alan
            int kullaniciId = Convert.ToInt32(Session["kullaniciId"]);
            string bilgikaydet = "update kullanicilart set Isim='" + kullanicilar.Isim + "', Eposta='" + kullanicilar.Eposta + "',TeslimatAdresi='" + kullanicilar.TeslimatAdresi + "'," +
                "FaturaAdresi='" + kullanicilar.FaturaAdresi + "' where KullaniciID=" + kullaniciId + "";
            if (string.IsNullOrEmpty(kullanicilar.Isim) || string.IsNullOrEmpty(kullanicilar.Eposta))
            {
                Response.StatusCode = 500;
                return Json(new
                {
                    message = "İsim ve Eposta Alanları Zorunludur!",
                    success = false,
                });
            }
            using (MySqlCommand cmd = new MySqlCommand(bilgikaydet, baglanti.Open()))
            {
                int durum = cmd.ExecuteNonQuery();
                if (durum == 1)
                {
                    return Json(new
                    {
                        message = "Bilgileriniz Güncellenmiştir.",
                        success = true,
                    });
                }
                else
                {
                    Response.StatusCode = 500;
                    return Json(new
                    {
                        message = "Bilgileriniz Güncellenemedi!",
                        success = false,
                    });
                }
            }


        }

        [HttpPost]
        public JsonResult SifreKaydet(string EskiSifre, string YeniSifre)
        {
            if (string.IsNullOrEmpty(EskiSifre) && string.IsNullOrEmpty(YeniSifre))
            {
                Response.StatusCode = 404;
                return Json(new
                {
                    message = "Zorunlu alanları doldurunuz.",
                    success = false
                });
            }
            int kullaniciId = Convert.ToInt32(Session["kullaniciId"]);
            string eskisifrehash = Crypto.Hash(EskiSifre, "md5");
            string tablodanGelenParola = "";
            //kullanıcı önce eski şifresini girecek girilen şifre md5 formatında şifrenelenecek.
            //Daha sonra veri tabanı ile şifrelenmiş şifre karşılaştırılacak. İşlem doğru ise şifre sıfırlama işlemi gerçekleştirilecek.
            string eskisif = "select Parola from kullanicilart where KullaniciID=" + kullaniciId + "";
            using (MySqlCommand cmd = new MySqlCommand(eskisif, baglanti.Open()))
            {
                //cmd.ExecuteScalar();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tablodanGelenParola = reader["Parola"].ToString();
                    }
                }
            }

            //tablodan gelen md5 formatındaki şifre, kullanıcının girmiş olduğu şifre md5 formatına dönüştürülme
            //işleminde sonra karşılaştırma yapılacak. Doğru ise şifre güncelleme işlemi yapılabilecek.
            if (tablodanGelenParola == eskisifrehash)
            {
                string yeniSifreHash = Crypto.Hash(YeniSifre, "md5");
                string sifrekaydet = "update kullanicilart set Parola='" + yeniSifreHash + "' where KullaniciID=" + kullaniciId + "";
                using (MySqlCommand cmd = new MySqlCommand(sifrekaydet, baglanti.Open()))
                {
                    int durum = cmd.ExecuteNonQuery();
                    if (durum == 1)
                    {
                        return Json(new
                        {
                            message = "Şireniz güncellenmiştir.",
                            success = true
                        });
                    }
                    else
                    {
                        Response.StatusCode = 404;
                        return Json(new
                        {
                            message = "Server hatası!",
                            success = false
                        });
                    }
                }
            }

            else
            {
                Response.StatusCode = 404;
                return Json(new
                {
                    message = "Eski şifrenizi yanlış girdiniz.",
                    success = false
                });
            }




        }

        public JsonResult Siparislerim()
        {
            //giriş yapan kullanıcının bilgilerine göre 3 farklı tabloyı işikilendirip gerekli bilgileri çekiyorum.
            int kullaniciId = Convert.ToInt32(Session["kullaniciId"]);
            string siparislarim = "SELECT s.SiparisID,s.FaturaURL, u.UrunAdi, d.SatisFiyati, s.OlusturmaTarihi, d.TeslimatAdresi, d.FaturaAdresi, s.Durum " +
     "FROM siparislert s " +
     "INNER JOIN siparisdetayt d ON s.SiparisID = d.SiparisID " +
     "INNER JOIN urunlert u ON u.UrunID = s.UrunID " +
     "WHERE s.KullaniciID = " + kullaniciId;
            List<SiparislerT> siparisler = new List<SiparislerT>();
            using (MySqlCommand cmd = new MySqlCommand(siparislarim, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        siparisler.Add(new SiparislerT
                        {
                            SiparisID = int.Parse(dr["SiparisID"].ToString()),
                            UrunAdi = dr["UrunAdi"].ToString(),
                            SatisFiyati = decimal.Parse(dr["SatisFiyati"].ToString()),
                            OlusturmaTarihi = DateTime.Parse(dr["OlusturmaTarihi"].ToString()),
                            TeslimatAdresi = dr["TeslimatAdresi"].ToString(),
                            FaturaAdresi = dr["FaturaAdresi"].ToString(),
                            Durum = dr["Durum"].ToString(),
                            FaturaURL = dr["FaturaURL"].ToString()

                        });
                    }
                }
            }
            //değerleri json formatı ile gönderip, ajax ile verileri daha hızlı listeliyorum.
            return Json(
                new
                {
                    Result = from obj in siparisler
                             select new
                             {
                                 obj.SiparisID,
                                 obj.UrunAdi,
                                 obj.SatisFiyati,
                                 obj.OlusturmaTarihi,
                                 obj.TeslimatAdresi,
                                 obj.FaturaAdresi,
                                 obj.Durum,
                                 obj.FaturaURL,
                             }
                }, JsonRequestBehavior.AllowGet);
        }

        //üründetay sayfası
        public ActionResult UrunDetay(int id)
        {
            string urundetayi = "select*from urunlert where UrunID=" + id + ""; //ürün detayını çağır
            //ürün hangi kategoride ise onu o kategoriyi çağır
            string urunkategorisi = "select KategoriAdi,u.KategoriID from urunlert u INNER JOIN kategorilert k on u.KategoriID=k.KategoriID where u.UrunID=" + id + "";
            string kategoriAdi = "";
            int kategoriId = 0;
            using (MySqlCommand cmd = new MySqlCommand(urunkategorisi, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        kategoriId = int.Parse(dr["KategoriID"].ToString());
                        kategoriAdi = dr["KategoriAdi"].ToString();
                    }
                }
            }
            //Ürünleri Çağır
            UrunlerT urunlerT = new UrunlerT();
            using (MySqlCommand cmd = new MySqlCommand(urundetayi, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        urunlerT.urunID = int.Parse(dr["UrunID"].ToString());
                        urunlerT.UrunAdi = dr["UrunAdi"].ToString();
                        urunlerT.UrunAciklamasi = dr["UrunAciklamasi"].ToString();
                        urunlerT.Fiyat = decimal.Parse(dr["Fiyat"].ToString());
                        urunlerT.StokAdet = int.Parse(dr["StokAdet"].ToString());
                        urunlerT.UrunGorselURL = dr["UrunGorselURL"].ToString();
                        urunlerT.KategoriID = kategoriId;
                        urunlerT.KategoriAdi = kategoriAdi;
                    }
                }
                return View(urunlerT);
            }

        }

        //kategorilerde olan ürünleri listeleme
        public ActionResult KategoriDetay(int id)
        {

            string katDetay = "select k.KategoriID,k.KategoriAdi,k.KategoriAciklamasi,UrunID, UrunAdi, UrunAciklamasi,Fiyat,UrunGorselURL from urunlert u INNER JOIN kategorilert k " +
                "on u.KategoriID=k.KategoriID where k.KategoriID=" + id + "";
            using (MySqlCommand cmd = new MySqlCommand(katDetay, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    List<UrunlerT> urunler = new List<UrunlerT>();
                    while (dr.Read())
                    {
                        UrunlerT urunlerT = new UrunlerT();
                        urunlerT.urunID = int.Parse(dr["UrunID"].ToString());
                        urunlerT.UrunAdi = dr["UrunAdi"].ToString();
                        urunlerT.UrunAciklamasi = dr["UrunAciklamasi"].ToString();
                        urunlerT.Fiyat = decimal.Parse(dr["Fiyat"].ToString());
                        urunlerT.UrunGorselURL = dr["UrunGorselURL"].ToString();
                        urunlerT.KategoriID = int.Parse(dr["KategoriID"].ToString());
                        urunlerT.KategoriAdi = dr["KategoriAdi"].ToString();
                        urunlerT.KategoriAciklamasi = dr["KategoriAciklamasi"].ToString();
                        ViewBag.katAciklama = dr["KategoriAciklamasi"].ToString();
                        ViewBag.katAdi = dr["KategoriAdi"].ToString();
                        urunler.Add(urunlerT);
                    }
                    return View(urunler);
                }

            }

        }


        //header menü içerisinde kategorileri listeleme
        public PartialViewResult PartialKategori()
        {
            string kategori = "select*from kategorilert";

            using (MySqlCommand cmd = new MySqlCommand(kategori, baglanti.Open()))
            {
                List<KategorilerT> kategorimenu = new List<KategorilerT>();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        KategorilerT kategorilerT = new KategorilerT();
                        kategorilerT.KategoriID = int.Parse(dr["KategoriID"].ToString());
                        kategorilerT.Baslik = dr["KategoriAdi"].ToString();
                        kategorimenu.Add(kategorilerT);
                    }
                }
                return PartialView(kategorimenu);
            }

        }

        //ürün detay sayfasında bulunan diğer ürünler alanı
        public PartialViewResult DigerUrunler()
        {
            string digerUrunler = "select*from urunlert";
            int urunAdet = 0;
            using (MySqlCommand cmd = new MySqlCommand(digerUrunler, baglanti.Open()))
            {
                List<UrunlerT> UrunlerT = new List<UrunlerT>();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        urunAdet++;
                        UrunlerT Urunler = new UrunlerT();
                        Urunler.urunID = int.Parse(dr["UrunID"].ToString());
                        Urunler.UrunAdi = dr["UrunAdi"].ToString();
                        Urunler.UrunAciklamasi = dr["UrunAciklamasi"].ToString();
                        Urunler.Fiyat = decimal.Parse(dr["Fiyat"].ToString());
                        Urunler.UrunGorselURL = dr["UrunGorselURL"].ToString();
                        UrunlerT.Add(Urunler);
                        if (urunAdet == 4) break;
                    }
                }
                return PartialView(UrunlerT);
            }

        }

        public ActionResult SiparisOlusturma(int id)
        {
            int kullaniciId = Convert.ToInt32(Session["kullaniciId"]);
            if (kullaniciId == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            //UrunlerT UrunlerT = new UrunlerT();
            //KullanicilarT kullanicilarT = new KullanicilarT();
            string satinAlinanUrun = "select UrunID,UrunAdi,UrunGorselURL,Fiyat,UrunAciklamasi" +
                " from urunlert where UrunID=" + id + "";
            string satinAlmakIsteyenKullanici = "select KullaniciID,FaturaAdresi,TeslimatAdresi " +
                "from kullanicilart where KullaniciID=" + kullaniciId + "";
            //satın alınmak istenen ürün bilgilerini çağır
            using (MySqlCommand cmd = new MySqlCommand(satinAlinanUrun, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ViewBag.urunID = int.Parse(dr["UrunID"].ToString());
                        ViewBag.UrunAdi = dr["UrunAdi"].ToString();
                        ViewBag.UrunAciklamasi = dr["UrunAciklamasi"].ToString();
                        ViewBag.Fiyat = decimal.Parse(dr["Fiyat"].ToString());
                        ViewBag.UrunGorselURL = dr["UrunGorselURL"].ToString();
                    }
                }
            }
            //ürünü satın almak isteyen kullanıcının bilgilerini çağır
            using (MySqlCommand cmd = new MySqlCommand(satinAlmakIsteyenKullanici, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ViewBag.KullaniciID = int.Parse(dr["KullaniciID"].ToString());
                        ViewBag.FaturaAdresi = dr["FaturaAdresi"].ToString();
                        ViewBag.TeslimatAdresi = dr["TeslimatAdresi"].ToString();
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public JsonResult UrunSatinAl(int UrunID, decimal SatisFiyati, string FaturaAdresi, string TeslimatAdresi)
        {
            int kullaniciId = Convert.ToInt32(Session["kullaniciId"]);
            int siparisId = 0;
            string siparisOlustur = "insert into siparislert(UrunID,KullaniciID,OlusturmaTarihi,Durum) " +
                "values('" + UrunID + "','" + kullaniciId + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','1');";

            //sipariş oluşturma
            using (MySqlCommand cmd = new MySqlCommand(siparisOlustur, baglanti.Open()))
            {
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT LAST_INSERT_ID();";
                siparisId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            //sipariş detayı oluşturma
            if (siparisId > 0)
            {
                string siparisDetayOlustur = "insert into siparisdetayt(SiparisID,SatisFiyati,FaturaAdresi,TeslimatAdresi) " +
                    "values('" + siparisId + "','" + SatisFiyati + "','" + FaturaAdresi + "','" + TeslimatAdresi + "')";

                using (MySqlCommand cmd = new MySqlCommand(siparisDetayOlustur, baglanti.Open()))
                {
                    int durum = cmd.ExecuteNonQuery();
                    if (durum == 1)
                    {
                        string stokDusur = "update urunlert set StokAdet=StokAdet-1 where UrunID=" + UrunID + "";
                        cmd.CommandText = stokDusur;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        return Json(new
                        {
                            message = "eklendi",
                            success = true,
                            redirectUrl = "/home/hesabim"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            message = "hata",
                            success = false
                        });
                    }
                }
            }
            else
            {
                return Json(new
                {
                    message = "siparis olusturma hatasi",
                    success = false
                });
            }
        }

        public ActionResult Hakkimizda()
        {
            string hakkimizda = "select*from hakkimizdat";
            HakkimizdaT hakkimizdaT = new HakkimizdaT();
            using (MySqlCommand cmd = new MySqlCommand(hakkimizda, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        hakkimizdaT.Baslik = dr["Baslik"].ToString();
                        hakkimizdaT.Icerik = dr["Icerik"].ToString();
                        hakkimizdaT.GorselURL = dr["GorselURL"].ToString();
                    }

                }
            }
            return View(hakkimizdaT);
        }

        public ActionResult Iletisim()
        {
            // 
            return View();
        }
        [HttpPost]
        //iletişim sayfasında mesaj gönderme kısmı
        public ActionResult Iletisim(IletisimT iletisimT)
        {
            string mesaj = "insert into iletisimt(Isim,Eposta,MesajIcerik,OlusturmaTarihi,IpAdresi)" +
                " values('" + iletisimT.Isim + "','" + iletisimT.Eposta + "','" + iletisimT.MesajIcerik + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Request.ServerVariables["REMOTE_ADDR"].ToString() + "')";

            using (MySqlCommand cmd = new MySqlCommand(mesaj, baglanti.Open()))
            {
                int durum = cmd.ExecuteNonQuery();
                if (durum > 0)
                {
                    ViewBag.msj = "<p class='alert alert-success'>Mesajınız Gönderilmiştir.</p>";
                }
                else
                {
                    ViewBag.msj = "<p class='alert alert-danger'>Mesajınız Gönderilmemiştir..</p>";
                }
            }
            return View(iletisimT);
        }
    }
}