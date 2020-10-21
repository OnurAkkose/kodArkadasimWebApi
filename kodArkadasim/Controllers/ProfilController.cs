using kodArkadasim.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace kodArkadasim.Controllers
{
   
    public class ProfilController : Controller
    {
        kodArkadasimDbEntities db = new kodArkadasimDbEntities();

        // GET: Profil
        public ActionResult Profilim()
        {


            var userName = System.Web.HttpContext.Current.User.Identity.Name;
            var uye = db.Uyeler.FirstOrDefault(m => m.KullaniciAdi == userName);

            ViewBag.gmail = uye.Gmail;
            ViewBag.hakkimda = uye.Hakkimda;
            ViewBag.fikirSayisi = uye.FikirSayisi;            
            ViewBag.rank = uye.Rank;
            ViewBag.projeSayisi = uye.ProjeSayisi;
            ViewBag.avatar = uye.Avatar;
            ViewBag.cinsiyet = uye.Cinsiyet;
            ViewBag.IsimSoyisim = uye.IsimSoyisim;
            ViewBag.Meslek = uye.Meslek;




            return View();
        }
        [HttpGet]
        public ActionResult sendMessage()
        {



            return View();
        }

        [HttpPost]
        public ActionResult sendMessage(Mesajlar mesaj)
        {

            mesaj.GonderenKullanici = System.Web.HttpContext.Current.User.Identity.Name;
            db.Mesajlar.Add(mesaj);
            db.SaveChanges();

            ViewBag.Mesaj = "Mesajınız gönderildi";


            return View();
        }
        public ActionResult MesajKutusu()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var model = db.Mesajlar.Where(x => x.GonderenKullanici == userId);
            return View(model);
        }

        public PartialViewResult MesajGoster(int mesajId)
        {
            var model = db.Mesajlar.Where(x => x.id == mesajId).ToList();
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var user = db.Uyeler.FirstOrDefault(x => x.KullaniciAdi == userId);
            ViewBag.Avatar = user.Avatar;
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult MesajCevapla(int mesajId)
        {
            var mesaj = db.Mesajlar.FirstOrDefault(m => m.id == mesajId);

            var model = db.Uyeler.Where(m => m.KullaniciAdi == mesaj.GonderenKullanici);

            return View(model);

        }
        [HttpGet]

        public ActionResult Hakkimda()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Hakkimda(FormCollection form)
        {
            var userName = System.Web.HttpContext.Current.User.Identity.Name;
            var uye = db.Uyeler.FirstOrDefault(x => x.KullaniciAdi == userName);

            uye.Hakkimda = form["Hakkimda"];
            db.SaveChanges();


            return RedirectToAction("Profilim");
        }

        [HttpGet]

        public ActionResult YeniResim()
        {
            return View();
        }
        [HttpPost]

        public ActionResult YeniResim(FormCollection form)
        {
            var userName = System.Web.HttpContext.Current.User.Identity.Name;
            var uye = db.Uyeler.FirstOrDefault(x => x.KullaniciAdi == userName);

            if (Request.Files.Count > 0)
            {
                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string yol = "~/Resimler/Avatarlar/" + dosyaadi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                uye.Avatar = "/Resimler/Avatarlar/" + dosyaadi + uzanti;
            }

            db.SaveChanges();


            return RedirectToAction("Profilim");
        }
        [HttpGet]

        public ActionResult KullaniciyaMesajGonder()
        {
            return View();
        }
        [HttpPost]

        public ActionResult KullaniciyaMesajGonder(FormCollection form)
        {
            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            Mesajlar mesaj = new Mesajlar();
            mesaj.AlanKullanici = form["Id"];
            mesaj.GonderenKullanici = userName;
            mesaj.Aciklama = form["Aciklama"];
            mesaj.Baslik = form["Baslik"];

            db.Mesajlar.Add(mesaj);
            db.SaveChanges();

            return RedirectToAction("Profilim");
        }

        public ActionResult Fikirlerim()
        {
            var userName = System.Web.HttpContext.Current.User.Identity.Name;
            var uye = db.Uyeler.FirstOrDefault(x => x.KullaniciAdi == userName);
            var model = db.Fikirler.Where(m => m.UyeId == uye.Id).ToList();
            

            return View(model);
        }
        public ActionResult Projelerim()
        {
            var userName = System.Web.HttpContext.Current.User.Identity.Name;
            var uye = db.Uyeler.FirstOrDefault(x => x.KullaniciAdi == userName);
            var model = db.Projeler.Where(m => m.UyeId == uye.Id).ToList();
            var sayi = model.Count();
            ViewBag.proje = sayi;
            ViewBag.id = uye.Id;

            return View(model);
        }
        [HttpGet]
        public ActionResult MesajiCevapla()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MesajiCevapla(FormCollection form)
        {
            var userName = System.Web.HttpContext.Current.User.Identity.Name;

            Mesajlar mesaj = new Mesajlar();
            mesaj.AlanKullanici = form["AlanKullanici"];
            mesaj.GonderenKullanici = userName;
            mesaj.Aciklama = form["Aciklama"];
            mesaj.Baslik = form["Baslik"];
            mesaj.GonderenAvatar = form["GonderenAvatar"];

            db.Mesajlar.Add(mesaj);
            db.SaveChanges();
            return RedirectToAction("Profilim");
        }
        public ActionResult MesajiSil(int id)
        {
            var mesaj = db.Mesajlar.Where(m => m.id == id).FirstOrDefault();
            db.Mesajlar.Remove(mesaj);
            db.SaveChanges();

            return RedirectToAction("MesajKutusu");
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
            proje.UyeName = userName;
            proje.Tarih = tarih.ToString();
            proje.Baslik = form["Baslik"];
            proje.Aciklama = form["Aciklama"];
            db.Projeler.Add(proje);
            db.SaveChanges();
            return RedirectToAction("Projelerim");

        }

        
        public ActionResult FikriSil(int id)
        {
            var fikir = db.Fikirler.Where(z => z.Id == id).FirstOrDefault();
            db.Fikirler.Remove(fikir);
            db.SaveChanges();

            return RedirectToAction("Fikirlerim");
        }

        public ActionResult ProjeyiSil(int id)
        {
            var projem = db.Projeler.Where(x => x.Id == id).FirstOrDefault();
            db.Projeler.Remove(projem);
            db.SaveChanges();
            return RedirectToAction("Projelerim");
        }
        [HttpGet]

        public ActionResult Kisisel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Kisisel(FormCollection form)
        {
            var userName = System.Web.HttpContext.Current.User.Identity.Name;
            var uye = db.Uyeler.FirstOrDefault(x => x.KullaniciAdi == userName);

            uye.Meslek = form["Meslek"];
            uye.IsimSoyisim = form["IsimSoyisim"];
            db.SaveChanges();


            return RedirectToAction("Profilim");
        }

    }
}