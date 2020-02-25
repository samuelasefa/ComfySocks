using ComfySocks.Models;
using ComfySocks.Models.Items;
using ComfySocks.Models.SalesDeliveryInfo;
using ComfySocks.Models.SalesInfo;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    public class SalesDeliveryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: RowMaterialDelivery
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult SalesDeliveryList(int id)
        {
            TempData[User.Identity.GetUserId() + "saleDelivery"] = null;
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            ViewBag.ID = id;

            TempData[User.Identity.GetUserId() + "SaleID"] = id;
            var SalesDeliverd = (from rd in db.SalesDeliveryInformation where rd.SalesInformationID == id orderby rd.ID descending select rd).ToList();

            return View(SalesDeliverd);
        }


        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult NewSalesDelivery(int? id)
        {
            if (id == null)
            {

                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected! Try again";
                return RedirectToAction("SalesDeliveryList");
            }
            SalesInformation salesInformation = db.SalesInformation.Find(id);
            if (salesInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected! Try again";
                return RedirectToAction("SalesDeliveryList");
            }
            ViewBag.SalesID = (from d in db.Sales where d.SalesInformationID == id select d).ToList();
            ViewBag.id = id;
            TempData[User.Identity.GetUserId() + "SaleInfoID"] = id;
            return View();
        }

        [Authorize(Roles = "Super Admin, Admin")]
        [HttpPost]
        public ActionResult NewSalesDelivery(SalesDelivery salseDelivery)
        {
            if (TempData[User.Identity.GetUserId() + "SaleInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out! Try again";
                TempData[User.Identity.GetUserId() + "saleDelivery"] = null;
                return RedirectToAction("SalesDeliveryList", "SalesDelivery", null);
            }
            int id = (int)TempData[User.Identity.GetUserId() + "SaleInfoID"];
            TempData[User.Identity.GetUserId() + "SaleInfoID"] = id;
            ViewBag.id = id;
            bool found = false;
            List<SalesDeliveryVM> saleDelivery = new List<SalesDeliveryVM>();
            if (TempData[User.Identity.GetUserId() + "saleDelivery"] != null)
            {
                saleDelivery = (List<SalesDeliveryVM>)TempData[User.Identity.GetUserId() + "saleDelivery"];
            }
            if (ModelState.IsValid)
            {
                Sales s = db.Sales.Find(salseDelivery.SalesID);
                if (s == null)
                {
                    TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to lode Request information! Try again";
                    TempData[User.Identity.GetUserId() + "saleDelivery"] = null;
                    return RedirectToAction("SalesDeliveryList", new { id });
                }

                foreach (SalesDeliveryVM d in saleDelivery)
                {
                    if (d.SalesDelivery.SalesID == salseDelivery.SalesID)
                    {

                        if (d.SalesDelivery.Quantity + salseDelivery.Quantity > s.RemaningDelivery)
                        {
                            ViewBag.errorMessage = "Only " + s.RemaningDelivery + "items remain, You can’t deliver " + salseDelivery.Quantity + "items";
                        }
                        else
                        {
                            d.SalesDelivery.Quantity += salseDelivery.Quantity;
                            d.Remaining -= (float)salseDelivery.Quantity;
                        }
                        found = true;
                    }
                }
                if (!found)
                {
                    if (s.RemaningDelivery < salseDelivery.Quantity)
                    {
                        ViewBag.errorMessage = "Only " + s.RemaningDelivery + " Items remain, You can’t add" + salseDelivery.Quantity + " item for delivery";

                    }
                    else
                    {
                        SalesDeliveryVM salesDelivery = new SalesDeliveryVM();
                        Item I = db.Items.Find(s.ItemID);
                        salesDelivery.ItemCode = I.Code;
                        salesDelivery.ItemDescription = I.Name;
                        salesDelivery.Unit = s.Item.Unit.Name;
                        salesDelivery.UnitPrice = s.UnitPrice;
                        salesDelivery.SalesDelivery = salseDelivery;
                        salesDelivery.Remaining = s.RemaningDelivery - (float)salseDelivery.Quantity;
                        salesDelivery.Remark = salseDelivery.Remark;
                        saleDelivery.Add(salesDelivery);
                    }
                }
            }
            ViewBag.saleDelivery = saleDelivery;
            TempData[User.Identity.GetUserId() + "saleDelivery"] = saleDelivery;
            if (saleDelivery.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.SalesID = (from d in db.Sales where d.SalesInformationID == id select d).ToList();
            return View(salseDelivery);
        }

        [Authorize(Roles = "Salse,Super Admin,Admin,")]
        public ActionResult Remove(int id)
        {
            if (TempData[User.Identity.GetUserId() + "saleDelivery"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find selected orders. try again.";
                return RedirectToAction("NewSalesDelivery");
            }
            List<SalesDeliveryVM> saleDelivery = new List<SalesDeliveryVM>();
            saleDelivery = (List<SalesDeliveryVM>)TempData[User.Identity.GetUserId() + "saleDelivery"];
            foreach (var s in saleDelivery)
            {
                if (s.SalesDelivery.SalesID== id)
                {
                    saleDelivery.Remove(s);
                    ViewBag.succsessMessage = "Sale item successfully Removed";
                    break;
                }
            }
            if (saleDelivery.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.saleDelivery = saleDelivery;
            TempData[User.Identity.GetUserId() + "saleDelivery "] = saleDelivery;
            ViewBag.SalesID = (from d in db.Sales where d.SalesInformationID == id select d).ToList();
            return View("NewSalesDelivery");
        }


        [Authorize(Roles = "Super Admin, Admin")]
        //row material delivery information
        public ActionResult NewSalesDeliveryInfo()
        {
            if (TempData[User.Identity.GetUserId() + "saleDelivery"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load selected Delivery";
                return RedirectToAction("NewSalesDelivery");
            }
            if (TempData[User.Identity.GetUserId() + "SalesInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out! try agian";
                return RedirectToAction("SalesDeliveryList");
            }
            int SalesId = (int)TempData[User.Identity.GetUserId() + "SalesInfoID"];
            TempData[User.Identity.GetUserId()+ "SalesInfoID"] = SalesId;
            TempData[User.Identity.GetUserId() + "saleDelivery"] = TempData[User.Identity.GetUserId() + "selectedDelivery"];
            return View();
        }

        [Authorize(Roles = "Super Admin, Admin")]

        //row material delivery information
        [System.Web.Mvc.HttpPost]
        public ActionResult NewSalesDeliveryInfo(SalesDeliveryInformation saleDeliveryInformation)
        {
            if (TempData[User.Identity.GetUserId() + "selectedDelivery"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load selected Delivery";
                return RedirectToAction("SalesDeliveryList");
            }
            if (TempData[User.Identity.GetUserId() + "SalesInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out! try agian";
                return RedirectToAction("SalesDeliveryList", "salesInformation", null);
            }
            saleDeliveryInformation.ApplictionUserID = User.Identity.GetUserId();

            int SalesId = (int)TempData[User.Identity.GetUserId() + "SalesInfoID"];
            TempData[User.Identity.GetUserId() + "SalesInfoID"] = SalesId;
            List<SalesDeliveryVM> deliverylist = (List<SalesDeliveryVM>)TempData[User.Identity.GetUserId() + "selectedDelivery"];
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = deliverylist;

            if (saleDeliveryInformation.Date > DateTime.Now)
            {
                ViewBag.errorMessage = "The date information you provide is not valid";
                return View(saleDeliveryInformation);
            }
            SalesDeliveryVM d = deliverylist.FirstOrDefault();
            saleDeliveryInformation.ApplictionUserID = User.Identity.GetUserId();
            try
            {
                SalesDeliveryInformation lastDeliverd = (from del in db.SalesDeliveryInformation orderby del.ID descending select del).First();

                saleDeliveryInformation.SalesInformationID = SalesId;
                saleDeliveryInformation.DeliveryNumber = "De-No:" + lastDeliverd.ID++;
            }
            catch
            {
                saleDeliveryInformation.SalesInformationID = SalesId;
                saleDeliveryInformation.DeliveryNumber = "De-No:" + 1;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.SalesDeliveryInformation.Add(saleDeliveryInformation);
                    db.SaveChanges();
                    foreach (SalesDeliveryVM de in deliverylist)
                    {
                        de.SalesDelivery.SalesDeliveryInformationID = saleDeliveryInformation.ID;
                        db.SalesDeliveries.Add(de.SalesDelivery);
                        db.SaveChanges();
                        Sales s = db.Sales.Find(de.SalesDelivery.SalesID);
                        s.RemaningDelivery = de.Remaining;
                        db.Entry(s).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    TempData[User.Identity.GetUserId() + "succsessMessage"] = "Delivery Information reqisterd!!";
                    return RedirectToAction("SalesDeliveryList", "SalesDelivery", new {id = saleDeliveryInformation.ID });

                }
                
                catch (Exception e)
                {
                    ViewBag.errorMessage = e;
                }

            }

            return View(saleDeliveryInformation);
        }




        public ActionResult DropDelivery(int id = 0)
        {
            int SalesID = (int)TempData[User.Identity.GetUserId() + "SalesID"];
            if (id == 0)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected";
                return RedirectToAction("SalesDeliveryList", new { id = SalesID });
            }

            SalesDeliveryInformation salesdeliveryInfo = db.SalesDeliveryInformation.Find(id);
            if (salesdeliveryInfo == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find delivery information !";
                return RedirectToAction("SalesDeliveryList", new { id = SalesID });
            }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<SalesDelivery> RedusedDelivery = new List<SalesDelivery>();
            bool pass = true;
            foreach (SalesDelivery d in salesdeliveryInfo.SalesDeliveries)
            {
                Sales sales = db.Sales.Find(d.SalesID);
                if (sales == null)
                {
                    pass = false;
                    break;
                }
                if (sales.RemaningDelivery >= sales.Quantity)
                {
                    ViewBag.errorMessage = "You rich Maximum Quantity";
                }
                sales.RemaningDelivery += (float)d.Quantity;
                
                db.Entry(sales).State = EntityState.Modified;
                db.SaveChanges();
                RedusedDelivery.Add(d);

            }
            if (!pass)
            {
                foreach (SalesDelivery d in RedusedDelivery)
                {
                    Sales sales = db.Sales.Find(d.SalesID);
                    if (sales == null)
                    {
                        pass = false;
                        break;
                    }
                    sales.RemaningDelivery -= (float)d.Quantity;
                    db.Entry(sales).State = EntityState.Modified;
                    db.SaveChanges();
                    RedusedDelivery.Add(d);

                }
                TempData[User.Identity.GetUserId() + "succsessMessage"] = "Delivery information dropped !";

                return RedirectToAction("SalesDeliveryList", new { id = SalesID });
            }
            else
            {
                salesdeliveryInfo.Status = "Rejected Delivery";
                db.Entry(salesdeliveryInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SalesDeliveryList", new { id = SalesID });
            }


        }

        //Disposing
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