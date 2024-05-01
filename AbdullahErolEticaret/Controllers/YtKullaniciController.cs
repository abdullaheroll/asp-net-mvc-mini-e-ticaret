using AbdullahErolEticaret.App_Classes;
using AbdullahErolEticaret.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AbdullahErolEticaret.Controllers
{
    public class YtKullaniciController : Controller
    {
        // GET: YtKullanici
        EticaretDB db = new EticaretDB();
        Baglanti baglanti = new Baglanti();
        public ActionResult Index()
        {
            //sisteme kayıtlı tüm kullanıcıları listleme
            string kullanici = "select*from kullanicilart";
            using (MySqlCommand cmd = new MySqlCommand(kullanici, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    List<KullanicilarT> kulanicilist = new List<KullanicilarT>();
                    while (dr.Read())
                    {
                        KullanicilarT kullanicilarT = new KullanicilarT();
                        kullanicilarT.KullaniciID = int.Parse(dr["KullaniciID"].ToString());
                        kullanicilarT.Isim = dr["Isim"].ToString();
                        kullanicilarT.Eposta = dr["Eposta"].ToString();
                        kullanicilarT.Aktiflik = bool.Parse(dr["Aktiflik"].ToString());
                        kullanicilarT.KayitTarihi = DateTime.Parse(dr["KayitTarihi"].ToString());
                        kullanicilarT.RolID = int.Parse(dr["RolID"].ToString());
                        kullanicilarT.TeslimatAdresi = dr["TeslimatAdresi"].ToString();
                        kullanicilarT.FaturaAdresi = dr["FaturaAdresi"].ToString();
                        kulanicilist.Add(kullanicilarT);
                    }

                    return View(kulanicilist);
                }
            }

        }

        public ActionResult AktifEt(int id)
        {
            //kullanıcının üyeliğini aktif etme
            string aktifsorgu = "update kullanicilart set Aktiflik=1 where KullaniciID=" + id + "";
            using (MySqlCommand cmd = new MySqlCommand(aktifsorgu, baglanti.Open()))
            {
                cmd.ExecuteReader();
            }
            return RedirectToAction("index", "ytkullanici");
        }

        public ActionResult PasifEt(int id)
        {
            //kullanıcının üyeliğini devredışı bırakma
            string aktifsorgu = "update kullanicilart set Aktiflik=0 where KullaniciID=" + id + "";
            using (MySqlCommand cmd = new MySqlCommand(aktifsorgu, baglanti.Open()))
            {
                cmd.ExecuteReader();
            }
            return RedirectToAction("index", "ytkullanici");
        }

        public ActionResult RolAta(int id)
        {
            //id değerine göre kullanıcının eposta,rolid ve id değerini çağırma
            string rol = "select RolID,KullaniciID,Eposta from kullanicilart where KullaniciID=" + id + "";

            KullanicilarT kullanicilar = new KullanicilarT();
            using (MySqlCommand cmd = new MySqlCommand(rol, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        kullanicilar.KullaniciID = int.Parse(dr["KullaniciID"].ToString());
                        kullanicilar.RolID = int.Parse(dr["RolID"].ToString());
                        kullanicilar.Eposta = dr["Eposta"].ToString();
                    }
                }
            }
            if (kullanicilar.RolID == 3)
            {
                return HttpNotFound();
            }
            else
            {
                return View(kullanicilar);
            }

        }

        [HttpPost]
        public ActionResult RolAta(int id, KullanicilarT kullanicilar)
        {
            /*                       kullanıcın rolünü değiştirme
            3 Rol var:
            1)Yönetici
            2)Normal Kullanıcı
            3)Süper Admin(Silinmemesi gerek!)
             */
            //gelen rol id 3 ise silme işlmenine izin verilmeyecek.
            string rolduzenle = "update kullanicilart set RolID=" + kullanicilar.RolID + " where KullaniciID=" + id + "";
            using (MySqlCommand cmd = new MySqlCommand(rolduzenle, baglanti.Open()))
            {
                int durum = cmd.ExecuteNonQuery();
                if (durum == 1)
                {
                    ViewBag.msg = "<div class='alert alert-success'>Kullanıcının Rolü Güncellendi.</div>";
                }
                else
                {
                    ViewBag.msg = "<div class='alert alert-danger'>Kullanıcının Rolü Güncellenmedi.</div>";
                }
            }
            return View(kullanicilar);
        }

        [HttpPost]
        public JsonResult Sil(int id)
        {
            int suparadmin = 0;
            string sil = "delete from kullanicilart where KullaniciID=" + id + "";
            //kullanıcı silme işlmeinde süper admini silmemek için, gelen id değerine göre kullanıcının rol idisini çekiyorum.
            string suparadminrolid = "select RolID from kullanicilart where KullaniciID=" + id + "";

            using (MySqlCommand cmd = new MySqlCommand(suparadminrolid, baglanti.Open()))
            {
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        suparadmin = int.Parse(rdr["RolID"].ToString());
                    }
                }
            }
            /*
             3 Rol var:
            1)Yönetici
            2)Normal Kullanıcı
            3)Süper Admin(Silinmemesi gerek!)
             */
            //gelen rol id 3 ise silme işlmenine izin verilmeyecek.
            if (suparadmin == 3)
            {
                Response.StatusCode = 401; //izin yok
                return Json(new
                {
                    message = "Hata!",
                    success = false
                });
            }
            else
            {
                using (MySqlCommand cmd = new MySqlCommand(sil, baglanti.Open()))
                {
                    int durum = cmd.ExecuteNonQuery();
                    if (durum == 1)
                    {
                        return Json(new
                        {
                            message = "Kullanıcı Silindi.",
                            success = true
                        });
                    }
                    else
                    {
                        Response.StatusCode = 404;
                        return Json(new
                        {
                            message = "Kullanıcı Silinemedi!",
                            success = false
                        });
                    }
                }
            }


        }
    }
}