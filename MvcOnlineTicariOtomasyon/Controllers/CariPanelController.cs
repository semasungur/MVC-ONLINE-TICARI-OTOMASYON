using MvcOnlineTicariOtomasyon.Models.Siniflar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class CariPanelController : Controller
    {
        // GET: CariPanel
       
        public object FormAuthentication { get; private set; }
        Context c = new Context();

        [Authorize]
        public ActionResult Index()
        {
            var mail = (string)Session["CariMail"];
            var degerler = c.Mesajlars.Where(x => x.Alici == mail).ToList();
            ViewBag.m = mail;
            var mailid=c.Carilers.Where(x => x.CariMail == mail).Select(y => y.CariID).FirstOrDefault();
            ViewBag.mid = mailid;
            var toplamsatis = c.SatisHarekets.Where(x => x.CariId == mailid).Count();
            ViewBag.toplamsatis=toplamsatis;
            var toplamtutar= c.SatisHarekets.Where(x => x.CariId == mailid).Sum(y => y.ToplamTutar);
            ViewBag.toplamtutar = toplamtutar;
            var toplamurunsayisi = c.SatisHarekets.Where(x => x.CariId == mailid).Sum(y => y.Adet);
            ViewBag.toplamurunsayisi = toplamurunsayisi;
            var adsoyad = c.Carilers.Where(x => x.CariMail == mail).Select(y => y.CariAd + " " + y.CariSoyad).FirstOrDefault();
            ViewBag.adsoyad = adsoyad;
            return View(degerler);
        }
        [Authorize]
        public ActionResult Siparişlerim()
        {
            var mail = (string)Session["CariMail"];
            var id = c.Carilers.Where(x => x.CariMail == mail.ToString()).Select(y => y.CariID).FirstOrDefault();
            var degerler = c.SatisHarekets.Where(x => x.CariId == id).ToList();
            return View(degerler);
        }
        [Authorize]
        public ActionResult GelenMesajlar()
        {
            var mail = (string)Session["CariMail"];
            var mesajlar = c.Mesajlars.Where(x => x.Alici == mail).OrderByDescending(X => X.Tarih).ToList();
            var gelensayisi = c.Mesajlars.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;
            var gidensayisi = c.Mesajlars.Count(x => x.Gonderici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            return View(mesajlar);
        }
        [Authorize]
        public ActionResult GidenMesajlar()
        {
            var mail = (string)Session["CariMail"];
            var mesajlar = c.Mesajlars.Where(x => x.Gonderici == mail).OrderByDescending(X => X.Tarih).ToList();
            var gelensayisi = c.Mesajlars.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;
            var gidensayisi = c.Mesajlars.Count(x => x.Gonderici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            return View(mesajlar);
        }
        [Authorize]
        public ActionResult MesajDetay(int id)
        {
            var degerler = c.Mesajlars.Where(x => x.MesajId == id).ToList();
            var mail = (string)Session["CariMail"];
            var gelensayisi = c.Mesajlars.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;
            var gidensayisi = c.Mesajlars.Count(x => x.Gonderici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            return View(degerler);
        }
        [Authorize]
        [HttpGet]
        public ActionResult YeniMesaj()
        {
            var mail = (string)Session["CariMail"];
            var gelensayisi = c.Mesajlars.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;
            var gidensayisi = c.Mesajlars.Count(x => x.Gonderici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            return View();
        }
        [HttpPost]
        public ActionResult YeniMesaj(Mesajlar m)
        {
            var mail = (string)Session["CariMail"];
            m.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            m.Gonderici = mail;
            c.Mesajlars.Add(m);
            c.SaveChanges();
            return View();
        }
        [Authorize]
        public ActionResult KargoTakip(string p)
        {
            var kargolar = from x in c.KargoDetays select x;
            kargolar = kargolar.Where(y => y.TakipKodu.Contains(p));
            return View(kargolar.ToList());
        }
        [Authorize]
        public ActionResult CariKargoTakip(string id)
        {
            var degerler = c.KargoTakips.Where(x => x.TakipKodu == id).ToList();
            return View(degerler);
        }
        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index","Login");
        }
        public PartialViewResult Partial1()
        {
            var mail = (string)Session["CariMail"];
            var id=c.Carilers.Where(x=>x.CariMail==mail).Select(y=>y.CariID).FirstOrDefault();
            var caribul = c.Carilers.Find(id);
            return PartialView("Partial1", caribul);
        }
        public PartialViewResult Partial2()
        {
            var veriler = c.Mesajlars.Where(x => x.Gonderici == "admin").ToList();
            return PartialView("Partial2", veriler);
        }
        public ActionResult CariBilgiGuncelle(Cariler cr)
        {
            var cari = c.Carilers.Find(cr.CariID);
            cari.CariAd = cr.CariAd;
            cari.CariSoyad = cr.CariSoyad;
            cari.CariSifre = cr.CariSifre;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}