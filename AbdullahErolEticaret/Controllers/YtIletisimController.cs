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
    public class YtIletisimController : Controller
    {
        // GET: YtIletisim
        Baglanti baglanti = new Baglanti();
        public ActionResult Index()
        {
            //gelen mesajlar 

            string gelenmesajar = "select*from iletisimt";
            using (MySqlCommand cmd = new MySqlCommand(gelenmesajar, baglanti.Open()))
            {

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    List<IletisimT> iletisimlist = new List<IletisimT>();
                    while (dr.Read())
                    {
                        IletisimT iletisim = new IletisimT();
                        iletisim.MesajID = int.Parse(dr["MesajID"].ToString());
                        iletisim.Isim = dr["Isim"].ToString();
                        iletisim.Eposta = dr["Eposta"].ToString();
                        iletisim.MesajIcerik = dr["MesajIcerik"].ToString();
                        iletisim.OlusturmaTarihi = DateTime.Parse(dr["OlusturmaTarihi"].ToString());
                        iletisim.IpAdresi = dr["IpAdresi"].ToString();
                        iletisimlist.Add(iletisim);
                    }
                    return View(iletisimlist);
                }
            
            }

        }

        //mesajları silme
        public ActionResult Sil(int id)
        {
            string sil = "delete from iletisimt where MesajID=" + id + "";
            using (MySqlCommand cmd = new MySqlCommand(sil, baglanti.Open()))
            {
                int durum = cmd.ExecuteNonQuery();
                if (durum == 1)
                {
                    return RedirectToAction("Index", "YtIletisim");
                }
                else
                {
                    return RedirectToAction("Index", "YtIletisim");
                }
            }
        }
    }
}