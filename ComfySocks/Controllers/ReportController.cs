using ComfySocks.Models;
using ComfySocks.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult StockReport()
        {
            var Items = (from I in db.Items where I.StoreType == Models.Items.StoreType.OfficeMaterial || I.StoreType == Models.Items.StoreType.RowMaterial select I);
            return View(Items.ToList());
        }
        // GET: Report
        public ActionResult SalesReport()
        {
            var Items = (from I in db.Items where I.StoreType == Models.Items.StoreType.ProductItem select I);
            return View(Items.ToList());
        }
       
    }
}
    