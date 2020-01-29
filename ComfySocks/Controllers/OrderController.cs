using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Order;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Order
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult OrderHistory()
        {
            //display if error message send from onther controller
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null;}
            //displaying the success Message if other controller have
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }

            var productionOrderInfos = db.ProductionOrderInfos.Include(c=> c.Customer);

            return View(productionOrderInfos.ToList());
        }
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult OrderDetial(int id = 0)
        {
            //display if error message send from onther controller
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            //displaying the success Message if other controller have
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }

            if (id == 0)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("OrderHistory");
            }

            ProductionOrderInfo POI = db.ProductionOrderInfos.Find(id);
            if (POI == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected!!";
                return RedirectToAction("OrderHistory");
            }
            return View(POI);
        }
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult NewOrderEntry(int id = 0)
        {
            //This is to deisplay error message
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            //this is to display success message
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }

            if (id != 0)
            {

                List<ProductionOrder> productionOrders = new List<ProductionOrder>();
                if (TempData[User.Identity.GetUserId() + "ProductionOrders"] != null)
                {
                    productionOrders = (List<ProductionOrder>)TempData[User.Identity.GetUserId() + "ProductionOrders"];
                    TempData[User.Identity.GetUserId() + "ProductionOrders"] = null;
                }
                ViewBag.productionOrders = productionOrders;
            }
            else
            {
                TempData[User.Identity.GetUserId() + "ProductionOrders"] = null;
            }
            if(db.Customers.ToList().Count == 0)
            {
                ViewBag.Customer = "Register Customer Information Frist!!,";
            }
            ViewBag.ProductionOrder = null;
            ViewBag.ItemID = new SelectList((from i in db.Items orderby i.Code select i), "ID","Code");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult NewOrderEntry(ProductionOrder productionOrder)
        {
            //This is to deisplay error message
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            //this is to display success message
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }

            List<ProductionOrder> productionOrders = new List<ProductionOrder>();

            if (TempData[User.Identity.GetUserId() + "ProductionOrders"] != null)
            {
                productionOrders = (List<ProductionOrder>)TempData[User.Identity.GetUserId() + "ProductionOrders"];
                TempData[User.Identity.GetUserId() + "ProductionOrders"] = null;
            }
            productionOrder.ProductionOrderInfoID = 1;
            productionOrder.RemaningDelivery = (float)productionOrder.Quantity;

            bool found = false;
            if (ModelState.IsValid)
            {
                double selectedQuantity = 0;
                foreach (ProductionOrder po in productionOrders)
                {
                    if (po.ItemID == productionOrder.ItemID)
                    {
                        selectedQuantity += po.Quantity;
                    }
                }
                if (!found)
                {
                    productionOrders.Add(productionOrder);
                }
            }
            if (productionOrders.Count > 0)
            {
                ViewBag.haveItem = true;
                ViewBag.productionOrders = productionOrders;
                TempData[User.Identity.GetUserId() + "ProductionOrders"] = productionOrders;
            }
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Code");
            return View();
        }

        //removing item from Production List Item
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult RemoveSelected(int id)
        {
            if (TempData[User.Identity.GetUserId() + "ProductionOrders"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find selected orders. try again.";
                return RedirectToAction("NewOrderEntry");
            }
            List<ProductionOrder> productionOrders = new List<ProductionOrder>();
            productionOrders = (List<ProductionOrder>)TempData[User.Identity.GetUserId() + "ProductionOrders"];
            foreach (var p in productionOrders)
            {
                if (p.ID == id)
                {
                    productionOrders.Remove(p);
                    ViewBag.succsessMessage = "Production Order is Successfully Removed";
                    break;
                }
            }
            if (productionOrders.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.productionOrders = productionOrders;
            TempData[User.Identity.GetUserId() + "ProductionOrder"] = productionOrders;
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Code");
            return View("NewOrderEntry");
        }

        [Authorize(Roles ="Sales, Super Admin, Admin")]
        public ActionResult NewOrderInfo()
        {
            if (TempData[User.Identity.GetUserId() + "ProductionOrders"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find selected Production Orders.try again";
                return RedirectToAction("NewOrderEntry");
            }
            TempData[User.Identity.GetUserId() + "ProductionOrders"] = TempData[User.Identity.GetUserId() + "ProductionOrders"];

            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName");
            return View();
        }

        [Authorize(Roles ="Super Admin, Admin, Sales")]
        [HttpPost]
        public ActionResult NewOrderInfo(ProductionOrderInfo productionOrderInfo)
        {
            if (TempData[User.Identity.GetUserId() + "ProductionOrders"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Information about production order please try agian";
                return RedirectToAction("NewOrderEntry");
            }

            List<ProductionOrder> productionOrders = new List<ProductionOrder>();
            productionOrders = (List<ProductionOrder>)TempData[User.Identity.GetUserId() + "ProductionOrders"];

            TempData[User.Identity.GetUserId() + "ProductionOrders"] = "productionOrders";
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName", productionOrderInfo.CustomerID);
            productionOrderInfo.ApplicationUserID = User.Identity.GetUserId();
            productionOrderInfo.Date = DateTime.Now;

            //if item state is valid

            if (ModelState.IsValid)
            {
                try
                {
                    db.ProductionOrderInfos.Add(productionOrderInfo);
                    db.SaveChanges();
                    TempData[User.Identity.GetUserId() + "succsessMessage"] = "Production Order Info is  Succesfully add to database";

                    foreach (ProductionOrder p in productionOrders)
                    {
                        p.ProductionOrderInfoID = productionOrderInfo.ID;
                        p.RemaningDelivery = (float)p.Quantity;
                        db.ProductionOrders.Add(p);
                        db.SaveChanges();
                        TempData[User.Identity.GetUserId() + "succsessMessage"] = "Production Order is saved";  
                    }

                    ProductionOrderInfo po = db.ProductionOrderInfos.Find(productionOrderInfo.ID);
                    po.OrderNumber = "OR-No: " + po.ID;
                    db.Entry(po).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData[User.Identity.GetUserId() + "succsessMessage"] = " Production Order Created!";
                    return RedirectToAction("OrderDetial", new { id = productionOrderInfo.ID });
                }
                catch
                {

                }
                
            }
            return View(productionOrderInfo);

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