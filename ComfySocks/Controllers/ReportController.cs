using ComfySocks.Models;
using ComfySocks.Models.Items;
using ComfySocks.Models.SalesInfo;
using ComfySocks.Repository.Reports;
using ComfySocks.ViewModel.ForReports;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    [Authorize(Roles = "Super Admin, Admin, Sales, Store Manager, Finance, Production")]
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Stock()
        {
            var Items = (from I in db.Items where I.StoreType == Models.Items.StoreType.OfficeMaterial || I.StoreType == Models.Items.StoreType.RowMaterial  select I);
            return View(Items.Distinct().ToList());
        }
        //==========================Purchase Report==================
        /// <summary>
        /// Purchase Report
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchaseReport()
        {
            return View(db.Stocks.ToList());
        }


        [HttpPost]
        public ActionResult PurchaseReport(PurchaseSearchVM vm)
        {
            var filter = new PurchaseFilterRepository();
            var model = filter.FilterPurchase(vm);
            return View(model);
        }

        //end of Purchase Report

        /// <summary>
        /// Stock report
        /// </summary>
        /// <returns></returns>
        // GET: Sale Report
        public ActionResult SalseReport()
        {
            return View(db.Sales.ToList());
        }

        [HttpPost]
        public ActionResult SalseReport(SalesSearchVM vm)
        {
            var filter = new SalesFilterRepository();
            var model = filter.FilterSales(vm);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
    