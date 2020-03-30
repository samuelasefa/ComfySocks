using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    [Authorize(Roles ="Super Admin, Admin, Sales, Store Manager, Finance, Production")]
    public class StockAvailableController : Controller
    {
        ComfySocks.Models.ApplicationDbContext db = new Models.ApplicationDbContext();
        // GET: StockAvailable
        public ActionResult StockAvailable()
        {
            var Items = (from I in db.Items where I.StoreType == Models.Items.StoreType.OfficeMaterial || I.StoreType == Models.Items.StoreType.RowMaterial || I.StoreType == Models.Items.StoreType.ProductItem select I);
            return View(Items.ToList());
        }
    }
}