using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using kodArkadasim.Models.EntityFramework;

namespace kodArkadasim.Controllers
{
    

    public class FikirlerController : Controller
    {
        private kodArkadasimDbEntities db = new kodArkadasimDbEntities();

        // GET: Fikirler
        [AllowAnonymous]
        public ActionResult Index()
        {
            var fikirler = db.Fikirler.Include(f => f.Uyeler);
            
            return View(fikirler.ToList());
        }


        
        public ActionResult KullaniciProfili(int kullaniciId)
        {
            var uye = db.Uyeler.FirstOrDefault(x => x.Id == kullaniciId);

            ViewBag.gmail = uye.Gmail;
            ViewBag.hakkimda = uye.Hakkimda;
            ViewBag.fikirSayisi = uye.FikirSayisi;
            ViewBag.rank = uye.Rank;
            ViewBag.projeSayisi = uye.ProjeSayisi;
            ViewBag.avatar = uye.Avatar;
            ViewBag.cinsiyet = uye.Cinsiyet;
            ViewBag.isim = uye.KullaniciAdi;
            return View();
        }
        [HttpGet]
        public ActionResult YeniFikir()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniFikir(FormCollection form)
        {
            
            var userName = System.Web.HttpContext.Current.User.Identity.Name;
            var uye = db.Uyeler.FirstOrDefault(m => m.KullaniciAdi == userName);
            Fikirler fikirler = new Fikirler();
            fikirler.Baslik = form["Baslik"].Trim();
            fikirler.Aciklama = form["Aciklama"].Trim();
            fikirler.UyeId = uye.Id;
            var tarih = DateTime.Now;
            fikirler.Tarih = tarih;
            db.Fikirler.Add(fikirler);
            db.SaveChanges();




            return RedirectToAction("Index");
        }
        public ActionResult KullaniciFikirleri(string userName)
        {

            var uye = db.Uyeler.FirstOrDefault(m => m.KullaniciAdi == userName);
            var model = db.Fikirler.Where(m => m.UyeId == uye.Id).ToList();
            ViewBag.uyeid = uye.Id;

            return View(model);
        }
        public ActionResult KullaniciProjeleri(string userName)
        {
            var uye = db.Uyeler.FirstOrDefault(m => m.KullaniciAdi == userName);
            var model = db.Projeler.Where(m => m.UyeId == uye.Id).ToList();


            return View(model);
        }

        


       
    }
}
