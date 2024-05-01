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
    public class YtKategoriController : Controller
    {
        // GET: YtKategori
        Baglanti baglanti = new Baglanti();
        EticaretDB db;
        public ActionResult Index()
        {
            string kategoriler = "select*from kategorilert";
            using (MySqlCommand cmd = new MySqlCommand(kategoriler, baglanti.Open()))
            {
                using (MySqlDataReader rd = cmd.ExecuteReader())
                {
                    List<KategorilerT> ktlist = new List<KategorilerT>();
                    while (rd.Read())
                    {
                        KategorilerT kategori = new KategorilerT();
                        kategori.KategoriID = int.Parse(rd["KategoriID"].ToString());
                        kategori.Baslik = rd["KategoriAdi"].ToString();
                        kategori.KategoriAciklamasi = rd["KategoriAciklamasi"].ToString();
                        ktlist.Add(kategori);
                    }
                    return View(ktlist);
                }
            }
        }

        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ekle(KategorilerT kategoriler)
        {
            //kategori ekleme sorgusu
            string kategoriekle = "insert into kategorilert(KategoriAdi,KategoriAciklamasi)" +
                "values('" + kategoriler.Baslik + "','" + kategoriler.KategoriAciklamasi + "')";

            using (MySqlCommand cmd = new MySqlCommand(kategoriekle, baglanti.Open()))
            {
                int durum = cmd.ExecuteNonQuery();
                if (durum == 1)
                {
                    ViewBag.msg = "<div class='alert alert-success'>Kategori Eklendi.</div>";
                }
                else
                {
                    ViewBag.msg = "<div class='alert alert-danger'>Kategori Eklenemedi.</div>";
                }
            }

            return View(kategoriler);
        }

        public ActionResult Duzenle(int id)
        {
            //id değerine göre kategoriyi çağır
            KategorilerT kategoriler = new KategorilerT();
            string kategorilist = "select*from kategorilert where KategoriID=" + id + "";
            using (MySqlCommand cmd = new MySqlCommand(kategorilist, baglanti.Open()))
            {
                using (MySqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        kategoriler.KategoriID = int.Parse(rd["KategoriID"].ToString());
                        kategoriler.Baslik = rd["KategoriAdi"].ToString();
                        kategoriler.KategoriAciklamasi = rd["KategoriAciklamasi"].ToString();
                    }
                }
            }
            return View(kategoriler);
        }

        [HttpPost]
        public ActionResult Duzenle(int id, KategorilerT kategoriler)
        {
            //id değerine göre gelen kategori bilgilerini düzenleme
            string kategoriduzenle = "update kategorilert set KategoriAdi='" + kategoriler.Baslik + "', KategoriAciklamasi='" + kategoriler.KategoriAciklamasi + "' where KategoriID=" + id + " ";
            using (MySqlCommand cmd = new MySqlCommand(kategoriduzenle, baglanti.Open()))
            {
                int durum = cmd.ExecuteNonQuery();
                if (durum == 1)
                {
                    ViewBag.msg = "<div class='alert alert-success'>Kategori Güncellendi.</div>";
                }
                else
                {
                    ViewBag.msg = "<div class='alert alert-danger'>Kategori Güncellenmedi.</div>";
                }

            }

            return View(kategoriler);
        }

        public JsonResult Sil(int id)
        {

            string kategorisil = "delete from kategorilert where KategoriID=" + id + "";

            using (MySqlCommand cmd = new MySqlCommand(kategorisil, baglanti.Open()))
            {
                int durum = cmd.ExecuteNonQuery();
                if (durum == 1)
                {
                    return Json(JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonRequestBehavior.AllowGet);
                }
            }

        }
    }
}