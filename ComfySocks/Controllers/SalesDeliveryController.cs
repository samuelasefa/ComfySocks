﻿using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.ProductTransferInfo;
using ComfySocks.Models.Repository;
using ComfySocks.Models.SalesDeliveryInfo;
using ComfySocks.Models.SalesInfo;
using ComfySocks.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    [Authorize(Roles = "Super Admin, Store Manager, Finance")]
    public class SalesDeliveryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult SalesDeliveryList(int id)
        {
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = null;
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            ViewBag.ID = id;

            TempData[User.Identity.GetUserId() + "SaleID"] = id;
            var deliverd = (from d in db.SalesDeliveryInformation where d.SalesInformationID == id orderby d.Date descending select d).ToList();
            return View(deliverd);
        }

        public ActionResult NewSalesDelivery(int? id)
        {
            if (id == null)
            {

                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected! Try again";
                return RedirectToAction("DeliveryList");
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
        [System.Web.Mvc.HttpPost]
        public ActionResult NewSalesDelivery(SalesDelivery salesDelivery)
        {
            if (TempData[User.Identity.GetUserId() + "SaleInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out! Try again";
                TempData[User.Identity.GetUserId() + "selectedDelivery"] = null;
                return RedirectToAction("SalesDeliveryList", "", null);
            }
            int id = (int)TempData[User.Identity.GetUserId() + "SaleInfoID"];
            TempData[User.Identity.GetUserId() + "SaleInfoID"] = id;
            ViewBag.id = id;
            bool found = false;
            List<SalesDeliveryVM> selectedDelivery = new List<SalesDeliveryVM>();
            if (TempData[User.Identity.GetUserId() + "selectedDelivery"] != null)
            {
                selectedDelivery = (List<SalesDeliveryVM>)TempData[User.Identity.GetUserId() + "selectedDelivery"];
            }
            if (ModelState.IsValid)
            {
                Sales sale = db.Sales.Find(salesDelivery.SalesID);
                if (sale == null)
                {
                    TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable To load sales Inforamtion From Database";
                    TempData[User.Identity.GetUserId() + "errorMessage"] = "Sales id for saleItem =" + salesDelivery.SalesID;
                    TempData[User.Identity.GetUserId() + "selectedDelivery"] = null;
                    return RedirectToAction("SalesDeliveryList");
                }

                foreach (SalesDeliveryVM d in selectedDelivery)
                {
                    if (d.SalesDelivery.SalesID == salesDelivery.SalesID)
                    {

                        if (d.SalesDelivery.Quantity + salesDelivery.Quantity > sale.RemaningDelivery)
                        {
                            ViewBag.errorMessage = "Only " + sale.RemaningDelivery + "items remain, You can’t deliver " + salesDelivery.Quantity + "items";
                        }
                        else
                        {
                            d.SalesDelivery.Quantity += salesDelivery.Quantity;
                            d.Remaining -= (float)salesDelivery.Quantity;
                        }
                        found = true;
                    }
                }
                if (!found)
                {
                    if (sale.RemaningDelivery < salesDelivery.Quantity)
                    {
                        ViewBag.errorMessage = "Only " + sale.RemaningDelivery + " Items remain, You can’t add" + salesDelivery.Quantity + " item for delivery";

                    }
                    else
                    {
                        SalesDeliveryVM deliveryVM = new SalesDeliveryVM();
                        Item I = db.Items.Find(sale.ItemID);
                        deliveryVM.ItemCode = I.Code;
                        deliveryVM.ItemDescription = I.Name;
                        deliveryVM.Unit = I.Unit.Name;
                        deliveryVM.SalesDelivery = salesDelivery;
                        deliveryVM.Remaining = sale.RemaningDelivery - (float)salesDelivery.Quantity;
                        selectedDelivery.Add(deliveryVM);
                    }
                }
            }
            ViewBag.selectedDelivery = selectedDelivery;
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = selectedDelivery;
            if (selectedDelivery.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.SalesID = (from d in db.Sales where d.SalesInformationID == id select d).ToList();
            return View(salesDelivery);
        }


        public ActionResult NewSalesDeliveryInfo()
        {
            if (TempData[User.Identity.GetUserId() + "selectedDelivery"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract delivery information!.";
                return RedirectToAction("");
            }
            if (TempData[User.Identity.GetUserId() + "SaleInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out!. Try again ";
                return RedirectToAction("SalesDeliveryList", "joborderinformations", null);
            }
            int saleId = (int)TempData[User.Identity.GetUserId() + "SaleInfoID"];
            TempData[User.Identity.GetUserId() + "SaleInfoID"] = saleId;
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = TempData[User.Identity.GetUserId() + "selectedDelivery"];
            return View();
        }
        public ActionResult Remove(int id)
        {
            if (TempData[User.Identity.GetUserId() + "selectedDelivery"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find selected Sales. try again.";
                return RedirectToAction("NewSalesDelivery");
            }
            List<SalesDeliveryVM> selectedDelivery = new List<SalesDeliveryVM>();
            selectedDelivery = (List<SalesDeliveryVM>)TempData[User.Identity.GetUserId() + "selectedDelivery"];
            foreach (var s in selectedDelivery)
            {
                if (s.SalesDelivery.Sales.ItemID == id)
                {
                    selectedDelivery.Remove(s);
                    ViewBag.succsessMessage = "Sales Delivery successfully Removed";
                    break;
                }
            }
            if (selectedDelivery.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.selectedDelivery = selectedDelivery;
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = selectedDelivery;
            ViewBag.SalesID = (from d in db.Sales where d.SalesInformationID == id select d).ToList();
            return View("NewSalesDelivery");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult NewSalesDeliveryInfo(SalesDeliveryInformation salesDeliveryInformation)
        {
            if (TempData[User.Identity.GetUserId() + "selectedDelivery"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract delivery information!.";
                return RedirectToAction("index", "JobOrderInformations", null);
            }
            if (TempData[User.Identity.GetUserId() + "SaleInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out!. Try again";
                return RedirectToAction("SalesDeliveryList", "joborderinformations", null);
            }


            int saleId = (int)TempData[User.Identity.GetUserId() + "SaleInfoID"];
            TempData[User.Identity.GetUserId() + "SaleInfoID"] = saleId;
            List<SalesDeliveryVM> deliverylist = (List<SalesDeliveryVM>)TempData[User.Identity.GetUserId() + "selectedDelivery"];
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = deliverylist;
            
            SalesDeliveryVM d = deliverylist.FirstOrDefault();
            salesDeliveryInformation.ApplictionUserID = User.Identity.GetUserId();
            salesDeliveryInformation.Date = DateTime.Now;

            try
            {
                SalesDeliveryInformation lastDeliverd = (from del in db.SalesDeliveryInformation orderby del.ID descending select del).First();

                salesDeliveryInformation.SalesInformationID = saleId;
                salesDeliveryInformation.DeliveryNumber = "No-" + lastDeliverd.ID.ToString("D4");
            }
            catch
            {
                salesDeliveryInformation.SalesInformationID = saleId;
                salesDeliveryInformation.DeliveryNumber = "No" + 0.ToString("D4"); ;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.SalesDeliveryInformation.Add(salesDeliveryInformation);
                    db.SaveChanges();

                    foreach (SalesDeliveryVM de in deliverylist)
                    {
                        de.SalesDelivery.SalesDeliveryInformationID = salesDeliveryInformation.ID;
                        db.SalesDeliveries.Add(de.SalesDelivery);
                        db.SaveChanges();
                        Sales s = db.Sales.Find(de.SalesDelivery.SalesID);
                        s.RemaningDelivery = de.Remaining;
                        db.Entry(s).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    foreach (SalesDelivery salesDelivery in salesDeliveryInformation.SalesDeliveries)
                    {
                        Transfer transfer = (from s in db.Transfers where s.ItemID == salesDelivery.Sales.ItemID select s).First();
                        //stock.Total -= (float)storeRequest.Quantity;
                        db.Entry(transfer).State = EntityState.Modified;
                        db.SaveChanges();
                        ProductMaterialRepository productMaterialRepository = db.ProductMaterialRepositories.Find(salesDelivery.Sales.ItemID);
                        Item i = db.Items.Find(productMaterialRepository.ID);
                        float deference = productMaterialRepository.RecentlyReducedProductMaterialAvaliable - salesDelivery.Quantity;

                        if (deference > 0)
                        {
                            productMaterialRepository.RecentlyReducedProductMaterialAvaliable -= (float)salesDelivery.Quantity;
                        }
                        else
                        {
                            productMaterialRepository.RecentlyReducedProductMaterialAvaliable = 0;
                            productMaterialRepository.ProductMaterialAavliable += deference;

                        }
                        db.Entry(productMaterialRepository).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    TempData[User.Identity.GetUserId() + "succsessMessage"] = "Delivery information registered!.";

                    return RedirectToAction("SalesDeliveryList", "SalesDelivery", new { id = salesDeliveryInformation.ID });

                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = e;

                }
            }

            return View(salesDeliveryInformation);
        }

        public ActionResult DropDelivery(int id = 0)
        {
            int SaleID = (int)TempData[User.Identity.GetUserId() + "SaleID"];
            if (id == 0)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected";
                return RedirectToAction("SalesDeliveryList", new { id = SaleID });
            }

            SalesDeliveryInformation deliveryInfo = db.SalesDeliveryInformation.Find(id);
            if (deliveryInfo == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find delivery information !";
                return RedirectToAction("SalesDeliveryList", new { id = SaleID });
            }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<SalesDelivery> RedusedDelivery = new List<SalesDelivery>();
            bool pass = true;
            foreach (SalesDelivery d in deliveryInfo.SalesDeliveries)
            {
                Sales sale = db.Sales.Find(d.SalesID);
                if (sale == null)
                {
                    pass = false;
                    break;
                }
                sale.RemaningDelivery += (float)d.Quantity;
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                RedusedDelivery.Add(d);

            }
            if (!pass)
            {
                foreach (SalesDelivery d in RedusedDelivery)
                {
                    Sales sale = db.Sales.Find(d.SalesID);

                    if (sale == null)
                    {
                        pass = false;
                        break;
                    }
                    sale.RemaningDelivery -= (float)d.Quantity;
                    db.Entry(sale).State = EntityState.Modified;
                    db.SaveChanges();
                    RedusedDelivery.Add(d);

                }
                TempData[User.Identity.GetUserId() + "succsessMessage"] = "Delivery information dropped !";

                return RedirectToAction("SalesDeliveryList", new { id = SaleID });
            }
            else
            {
                deliveryInfo.Status = "Rejected Delivery";
                db.Entry(deliveryInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SalesDeliveryList", new { id = SaleID });
            }


        }

        //to check avaliable on stock 
        float AvalableQuantity(int item)
        {
            ProductMaterialRepository avalable = db.ProductMaterialRepositories.Find(item);
            if (avalable != null)
            {
                return avalable.ProductMaterialAavliable;
            }
            return -2;
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