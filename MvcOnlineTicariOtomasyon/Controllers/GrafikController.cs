using MvcOnlineTicariOtomasyon.Models.Siniflar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class GrafikController : Controller
    {
        // GET: Grafik
        // view üzerinden veriler manuel olarak bu viewden grafiklendiriliyoruz
        public ActionResult Index()
        {
            return View();
        }
        //controller üzerinden veriler manuel olarak bu şekilde grafiklendirilir
        public ActionResult Index2()
        {
            var grafikciz = new Chart(600, 600);
            grafikciz.AddTitle("Kategori-Ürün Stok Sayısı").AddLegend("Stok").AddSeries("Degerler", xValue: new[] { "Beyaz Eşya", "Telefon", "Küçük Ev Aletleri", "Bilgisayar" }, yValues: new[] { 500, 250, 340, 620 }).Write() ;
            return File(grafikciz.ToWebImage().GetBytes(),"image/jpeg");
        }
        //veritabanı üzerinden veriler otomatik olarak bu şekilde grafiklendirilir
        Context c = new Context();
        public ActionResult Index3()
        {
            ArrayList xvalue = new ArrayList();
            ArrayList yvalue = new ArrayList();
            var sonuclar = c.Uruns.ToList();
            sonuclar.ToList().ForEach(x => xvalue.Add(x.UrunAd));
            sonuclar.ToList().ForEach(y => yvalue.Add(y.Stok));
            var grafik = new Chart(width: 500, height: 500)
                .AddTitle("Stoklar")
                .AddSeries(chartType: "Pie", name: "Stok", xValue: xvalue, yValues: yvalue);
            return File(grafik.ToWebImage().GetBytes(), "image/jpeg"); 
        }
        //Google Chart verileri manuel çekme
        public ActionResult Index4()
        {
            return View();
        }
        ///Google Chart verileri manuel çekme
        public ActionResult VisualizeUrunResult()
        {
            return Json(Urunlistesi(), JsonRequestBehavior.AllowGet);
        }
        //Google Chart verileri manuel çekme
        public List<Sinif1> Urunlistesi()
        {
            List<Sinif1> snf = new List<Sinif1>();
            snf.Add(new Sinif1()
            {
                UrunAd="Bilgisayar",
                Stok=120
            });
            snf.Add(new Sinif1()
            {
                UrunAd = "Beyaz Eşya",
                Stok = 150
            });
            snf.Add(new Sinif1()
            {
                UrunAd = "Mobilya",
                Stok = 70
            });
            snf.Add(new Sinif1()
            {
                UrunAd = "Küçükev Aletleri",
                Stok = 180
            });
            snf.Add(new Sinif1()
            {
                UrunAd = "Telefon",
                Stok = 80
            });
            return snf;
        }
        //Google Chart verileri veritabanından otomatik çekme
        public ActionResult Index5()
        {
            return View();
        }
        //Google Chart verileri veritabanından otomatik çekme
        public ActionResult VisualizeUrunResult2()
        {
            return Json(UrunListesi2(), JsonRequestBehavior.AllowGet);
        }
        //Google Chart verileri veritabanından otomatik çekme
        public List<Sinif2> UrunListesi2()
        {
            List<Sinif2> snf = new List<Sinif2>();
            using(var context=new Context())
            {
                snf = context.Uruns.Select(x => new Sinif2
                {
                    Urn = x.UrunAd,
                    Stk = x.Stok
                }).ToList();
            }
            return snf;
        }
        public ActionResult Index6()
        {
            return View();
        }
        public ActionResult Index7()
        {
            return View();
        }
    }
}