using ComfySocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    public class DeliveryController : Controller
    {
        // GET: Delivery
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult DeliveryList()
        {
            return View();
        }


        //New: Delivery
        public ActionResult NewDeliveryEntry()
        {
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Name");
            return View();
        }

        //Detial:Delivery

        public ActionResult DeliveryDetail()
        {
            return View();
        }

        //NewInformation:Delivery
        public ActionResult NewDeliveryInfo()
        {
            return View();
        }

    }
}