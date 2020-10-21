using kodArkadasim.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using System.Web.Services.Description;
using System.Web.Routing;

namespace kodArkadasim.Controllers
{
 
    public class ProjelerController : Controller
    {
        // GET: Projeler
        private kodArkadasimDbEntities db = new kodArkadasimDbEntities();
        [AllowAnonymous]
        public ActionResult Projeler()
        {
          
            var projeler = db.Projeler.Include(p => p.Uyeler).ToList();
            return View(projeler);
        }

        public ActionResult ProjeyiIncele(int id)
        {
            var projem = db.Projeler.FirstOrDefault(m => m.Id == id);
            ViewBag.baslik = projem.Baslik;
            ViewBag.aciklama = projem.Aciklama;
            ViewBag.resim = projem.Resim;
            ViewBag.yukleyen = projem.UyeName;
            ViewBag.yukleyenId = projem.Id;
           

            return View(projem);
        }
        [HttpGet]
        public ActionResult YeniProje()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniProje(FormCollection form)
        {
            Projeler proje = new Projeler();
            if (Request.Files.Count > 0)
            {

                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string yol = "~/Resimler/Projeler/" + dosyaadi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                proje.Resim = "/Resimler/Projeler/" + dosyaadi + uzanti;
            }
            var tarih = DateTime.Now;
            var userName = System.Web.HttpContext.Current.User.Identity.Name;
            var uye = db.Uyeler.FirstOrDefault(x => x.KullaniciAdi == userName);
            proje.UyeName = userName;
            proje.Tarih = tarih.ToString();
            proje.Baslik = form["Baslik"];
            proje.Aciklama = form["Aciklama"];
            proje.UyeId = uye.Id;
            db.Projeler.Add(proje);
            db.SaveChanges();
            return RedirectToAction("Projeler");
        }

        
    }
}