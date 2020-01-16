using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.ProductStock;
using ComfySocks.Models.SalesInfo;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    public class SalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult SalesList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            //var saleslist = db.Sales;
            return View();
        }

        public ActionResult SalesDetail(int? id)
        {
            if (id == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Inavalid Navigation is detected!!";
                return RedirectToAction("SalesList");
            }
            SalesInformation salesInformation = db.SalesInformation.Find(id);
            if (salesInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable To load sales information";
                return RedirectToAction("SalesList");
            }
            return View(salesInformation);
        }
        // New  SALES
        [Authorize(Roles ="Super Admin, Admin, Sales")]
        public ActionResult NewSalesEntry(int id = 0)
        {
            //display if error mesage send from other controler
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            //display if succsess mesage send from other controler
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            //precondition for New 
            if (id != 0)
            {
                List<Sales> sales = new List<Sales>();
                if (TempData[User.Identity.GetUserId() + "Sales"] != null)
                {
                    sales = (List<Sales>)TempData[User.Identity.GetUserId() + "Sales"];
                    TempData[User.Identity.GetUserId() + "Sales"] = null;
                }
                ViewBag.sales = sales;
            }
            else
            {
                TempData[User.Identity.GetUserId() + "Sales"] = null;
            }
            if (db.Customers.ToList().Count == 0) {
                ViewBag.custumer = "Register Customer frist";
            }
            ViewBag.sales = null;
            ViewBag.ProductCodeID = new SelectList(db.ProductCodes, "ID", "Code");
            return View();
        }
        [HttpPost]
        [Authorize(Roles ="Super Admin, Admin, Sales")]
        public ActionResult NewSalesEntry(Sales sales)
        {
            List<Sales> Sales = new List<Sales>();
            if (TempData[User.Identity.GetUserId()+"Sales"]!= null) {
                Sales = (List<Sales>)TempData[User.Identity.GetUserId() + "Sales"];
                TempData[User.Identity.GetUserId() + "Sales"] = null;
            }
            sales.SalesInfoID = 1;
            sales.RemaningDelivery = (float)sales.Quantity;
            float totalPrice = 0;

            bool found = false;
            //if (ModelState.IsValid) {
            //    ProductAvialableOnStock av = db.ProductAvialableOnStock.Find(sales.TempProductStockID);
            //    if (av == null)
            //    {
            //        ViewBag.errorMessage = "Low Stock Only 0 Sales item is found";
            //    }
            //    else
            //    {
            //        double selectedQuantity = 0;
            //        foreach (Sales item in Sales)
            //        {
            //            if (item.TempProductStockID == sales.TempProductStockID)
            //            {
            //                selectedQuantity += item.Quantity;
            //            }
            //            totalPrice += (float)(item.Quantity * item.UnitPrice);
            //        }
            //        if (av.ProductAvaliable > selectedQuantity + sales.Quantity)
            //        {
            //            foreach (var i in Sales)
            //            {
            //                if (i.TempProductStockID == sales.TempProductStockID)
            //                {
            //                    i.Quantity += sales.Quantity;
            //                    found = true;
            //                    break;
            //                }

            //            }
            //            if (!found)
            //            {
            //                Sales.Add(sales);
            //            }
            //            else
            //            {
            //                ViewBag.errorMessage = "Low Stock! Only " + av.ProductAvaliable + " item available";
            //            }
            //        }
            //    }
            //}
            if (Sales.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.sales = sales;
            TempData[User.Identity.GetUserId() + "Sales"] = sales;
            TempData[User.Identity.GetUserId() + "totalPrice"] = totalPrice;

            ViewBag.ProductCodeID = new SelectList(db.ProductCodes, "ID", "Code");
            return View();

        }

        //Detail information of sales


    }
}