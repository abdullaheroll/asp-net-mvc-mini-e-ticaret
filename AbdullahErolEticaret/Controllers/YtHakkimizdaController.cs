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
    public class YtHakkimizdaController : Controller
    {
        // GET: YtHakkimizda
        Baglanti baglanti = new Baglanti();
        public ActionResult Index()
        {
            //hakkımızda bölümü
            string hakkimizdasorgu = "select*from hakkimizdat";
            using (MySqlCommand cmd = new MySqlCommand(hakkimizdasorgu, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    List<HakkimizdaT> hakkimizdalist = new List<HakkimizdaT>();
                    while (dr.Read())
                    {
                        HakkimizdaT hakkimizda = new HakkimizdaT();
                        hakkimizda.HakkimizdaID = int.Parse(dr["HakkimizdaID"].ToString());
                        hakkimizda.Baslik = dr["Baslik"].ToString();
                        hakkimizda.Icerik = dr["Icerik"].ToString();
                        hakkimizda.GorselURL = dr["GorselURL"].ToString();
                        hakkimizdalist.Add(hakkimizda);
                    }
                    return View(hakkimizdalist);
                }
            }

        }

        public ActionResult Duzenle(int id)
        {
            string listduzen = "select*from hakkimizdat where HakkimizdaID=" + id + "";
            HakkimizdaT hakkimizda = new HakkimizdaT();
            using (MySqlCommand cmd = new MySqlCommand(listduzen, baglanti.Open()))
            {
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        hakkimizda.HakkimizdaID = int.Parse(dr["HakkimizdaID"].ToString());
                        hakkimizda.Baslik = dr["Baslik"].ToString();
                        hakkimizda.Icerik = dr["Icerik"].ToString();
                    }
                }
            }
            return View(hakkimizda);
        }
        [HttpPost]
        public ActionResult Duzenle(int id, HakkimizdaT hakkimizdaT, HttpPostedFileBase Gorsel)
        {
            string hakimizdaduzenle = "";
            if (Gorsel != null)
            {
                string gorselduzenle = "select GorselURL from hakkimizdat where HakkimizdaID=" + id + "";

                using (MySqlCommand cmd = new MySqlCommand(gorselduzenle, baglanti.Open()))
                {
                    using (MySqlDataReader hakkimizdaresimdr = cmd.ExecuteReader())
                    {
                        while (hakkimizdaresimdr.Read())
                        {
                            if (System.IO.File.Exists(Server.MapPath(hakkimizdaresimdr["GorselURL"].ToString())))
                            {
                                System.IO.File.Delete(Server.MapPath(hakkimizdaresimdr["GorselURL"].ToString()));
                            }
                        }
                    }

                    Image img = Image.FromStream(Gorsel.InputStream);
                    Bitmap bmp = new Bitmap(img, 1200, 1200);
                    string resimyolu = "/Gorsel/" + Guid.NewGuid() + Path.GetExtension(Gorsel.FileName); //gorsel klasörüne random bir isimle kayıt etme
                    bmp.Save(Server.MapPath(resimyolu)); //görseli klasöre kayıt etme

                    hakimizdaduzenle = "update hakkimizdat set Baslik='" + hakkimizdaT.Baslik + "', Icerik='" + hakkimizdaT.Icerik + "', GorselURL='" + resimyolu + "' where HakkimizdaID=" + id + "";
                }
            }
            else
            {
                hakimizdaduzenle = "update hakkimizdat set Baslik='" + hakkimizdaT.Baslik + "', Icerik='" + hakkimizdaT.Icerik + "' where HakkimizdaID=" + id + "";
            }

            using (MySqlCommand cmd2 = new MySqlCommand(hakimizdaduzenle, baglanti.Open()))
            {
                int durum = cmd2.ExecuteNonQuery();
                if (durum == 1)
                {
                    ViewBag.msg = "<div class='alert alert-success'>Hakkımızda Alanı Güncellendi.</div>";
                }
                else
                {
                    ViewBag.msg = "<div class='alert alert-danger'>Hakkımızda Alanı Güncellenemedi.</div>";
                }
            }
            return View(hakkimizdaT);

        }
    }
}