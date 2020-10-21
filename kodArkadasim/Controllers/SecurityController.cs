using kodArkadasim.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace kodArkadasim.Controllers
{
    [AllowAnonymous]
    
    public class SecurityController : Controller
    {
       
        
        private kodArkadasimDbEntities db = new kodArkadasimDbEntities();
        
        // GET: Security
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Uyeler kullanici)
        {
            var giris = db.Uyeler.FirstOrDefault(x => x.KullaniciAdi == kullanici.KullaniciAdi && x.Sifre == kullanici.Sifre);
            if (giris != null)
            {
                FormsAuthentication.SetAuthCookie(giris.KullaniciAdi, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Mesaj = "Geçersiz kullanıcı adı veya şifre";
                return View();
            }
            
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Register()
        {
            
            return PartialView();
        }
        [HttpPost]
        public ActionResult Register(Uyeler uye)
        {
            if (Request.Files.Count > 0)
            {
                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string yol = "~/Resimler/Avatarlar/" + dosyaadi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                uye.Avatar = "/Resimler/Avatarlar/" + dosyaadi + uzanti;
            }

            var uyem = db.Uyeler.FirstOrDefault(x => x.KullaniciAdi == uye.KullaniciAdi);
            var uyem2 = db.Uyeler.FirstOrDefault(x => x.Gmail == uye.Gmail);
            if (uyem != null || uyem2 != null)
            {
                ViewBag.hata = "Kullanıcı adı veya mail sisteme zaten kayıtlı!";
                return PartialView("Register");
            }
            

            db.Uyeler.Add(uye);
            db.SaveChanges();
            return RedirectToAction("Login");
           // return View();
        

          

            
        }
    }
}