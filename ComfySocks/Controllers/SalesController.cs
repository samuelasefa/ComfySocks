using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.ProductInfo;
using ComfySocks.Models.ProductStock;
using ComfySocks.Models.SalesInfo;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    public class SalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult SalesList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var saleslist = db.SalesInformation.Include(J => J.Customer);
            return View(saleslist);
        }

        //sales :Detail
        [Authorize(Roles = "Super Admin, Admin")]

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
                List<Sales> salesitem = new List<Sales>();
                if (TempData[User.Identity.GetUserId() + "Sales"] != null)
                {
                    salesitem = (List<Sales>)TempData[User.Identity.GetUserId() + "Sales"];
                    TempData[User.Identity.GetUserId() + "Sales"] = null;
                }
                ViewBag.salesitem = salesitem;
            }
            else
            {
                TempData[User.Identity.GetUserId() + "Sales"] = null;
            }
            if (db.Customers.ToList().Count == 0) {
                ViewBag.custumer = "Register Customer frist";
            }
            ViewBag.salesitem = null;
            ViewBag.TempProductStockID = new SelectList(db.TempProductStocks, "ID", "ProductName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="Super Admin, Admin, Sales")]
        public ActionResult NewSalesEntry(Sales sales)
        {
            //This is to deisplay error message
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            //this is to display success message
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }

            List<Sales> salesitem = new List<Sales>();
            if (TempData[User.Identity.GetUserId()+"Sales"]!= null)
            {
                salesitem = (List<Sales>)TempData[User.Identity.GetUserId() + "Sales"];
                TempData[User.Identity.GetUserId() + "Sales"] = null;
            }
            sales.SalesInformationID = 1;
            sales.RemaningDelivery = (float)sales.Quantity;
            float totalPrice = 0;

            bool found = false;
            if (ModelState.IsValid)
            {
                ProductAvialableOnStock av = db.ProductAvialableOnStock.Find(sales.TempProductStockID);
                if (av == null)
                {
                    ViewBag.errorMessage = "Low Stock Only 0 Sales item is found";
                }
                else
                {
                    double selectedQuantity = 0;
                    foreach (Sales item in salesitem)
                    {
                        if (item.TempProductStockID == sales.TempProductStockID)
                        {
                            selectedQuantity += item.Quantity;
                        }
                        totalPrice += (float)(item.Quantity * item.UnitPrice);
                    }
                    if (av.ProductAvaliable > selectedQuantity + sales.Quantity)
                    {
                        foreach (var i in salesitem)
                        {
                            if (i.TempProductStockID == sales.TempProductStockID)
                            {
                                i.Quantity += sales.Quantity;
                                found = true;
                                break;
                            }

                        }
                        if (!found)
                        {
                            salesitem.Add(sales);
                        }
                        else
                        {
                            ViewBag.succsessMessage = "sales item is add";
                        }
                    }
                }
            }
            if (salesitem.Count > 0)
            {
                ViewBag.haveItem = true;
                ViewBag.salesitem = salesitem;
                TempData[User.Identity.GetUserId() + "Sales"] = salesitem;
                TempData[User.Identity.GetUserId() + "totalPrice"] = totalPrice;
            }
            ViewBag.TempProductStockID = new SelectList(db.TempProductStocks, "ID", "ProductName");
            return View();

        }

        [Authorize(Roles = "Super Admin, Admin, Sales")]
        public ActionResult NewSalesInfo()
        {
            if (TempData[User.Identity.GetUserId() + "Sales"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load selected sales information!! try again";
                return RedirectToAction("NewSalesEntry");
            }
            TempData[User.Identity.GetUserId() + "Sales"] = TempData[User.Identity.GetUserId() + "Sales"];
            TempData[User.Identity.GetUserId() + "totalPrice"] = TempData[User.Identity.GetUserId() + "totalPrice"];
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult NewSalesInfo(SalesInformation salesInformation)
        {
            if (TempData[User.Identity.GetUserId() + "Sales"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load selected sales information!! try again";
                return RedirectToAction("NewSalesEntry");
            }
            List<Sales> salesitem = new List<Sales>();
            salesitem = (List<Sales>)TempData[User.Identity.GetUserId() + "Sales"];

            float totalPrice = 0;
            TempData[User.Identity.GetUserId() + "Sales"] = salesitem;
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName", salesInformation.CustomerID);
            salesInformation.ApplicationUserID = User.Identity.GetUserId();
            salesInformation.Date = DateTime.Now;


            if (ModelState.IsValid)
            {
                try
                {
                    salesInformation.Status = "Submitted";
                    db.SalesInformation.Add(salesInformation);
                    db.SaveChanges();
                    foreach (Sales s in salesitem)
                    {
                        s.SalesInformationID = salesInformation.ID;
                        s.RemaningDelivery = (float)s.Quantity;
                        db.Sales.Add(s);
                        db.SaveChanges();
                        totalPrice += (float)s.Quantity * (float)s.UnitPrice;
                        ProductAvialableOnStock PV = db.ProductAvialableOnStock.Find(s.TempProductStockID);
                        PV.ProductAvaliable -= (float)s.Quantity;
                        PV.RecentlyReducedProduct += (float)s.Quantity;
                        db.Entry(PV).State = EntityState.Modified;
                        db.SaveChanges();
                        double Tax = 1.15;
                        SalesInformation si = db.SalesInformation.Find(salesInformation.ID);
                        si.SubTotal = totalPrice;
                        si.GrandTotal = totalPrice * (float)Tax;
                        si.Tax = si.GrandTotal - si.SubTotal;
                        db.Entry(si).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData[User.Identity.GetUserId() + "succsessMessage"] = "New Sale Item is Created";
                        return RedirectToAction("SalesDetail");
                    }
                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = e;
                }
            }
            return View(salesInformation);
        }

        [Authorize(Roles = "Salse,Super Admin,Admin,")]
        public ActionResult RemoveSelected(int id)
        {
            if (TempData[User.Identity.GetUserId() + "Sales"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find selected orders. try again.";
                return RedirectToAction("NewSalesEntry");
            }
            List<Sales> salesitem = new List<Sales>();
            salesitem = (List<Sales>)TempData[User.Identity.GetUserId() + "Sales"];
            float totalPrice = (float)TempData[User.Identity.GetUserId() + "totalPrice"];
            foreach (var s in salesitem)
            {
                if (s.TempProductStockID == id)
                {
                    salesitem.Remove(s);
                    ViewBag.succsessMessage = "Sales successfully Removed";
                    break;
                }
            }
            if (salesitem.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.salesitem = salesitem;
            TempData[User.Identity.GetUserId() + "Sales"] = salesitem;
            TempData[User.Identity.GetUserId() + "totalPrice"] = totalPrice;
            ViewBag.TempProductStockID = new SelectList(db.TempProductStocks, "ID", "ProductName");
            return View("NewSalesEntry");
        }

        //sales approval
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult SaleApproved(int? id)
        {
            if (id == 0) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected!!";
                return RedirectToAction("SalesList");
            }
            SalesInformation salesInformation = db.SalesInformation.Find(id);
            if (salesInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Load Sales item from Stock";
                return RedirectToAction("SalesList");
            }

            List<SalesVMForError> ErrorList = new List<SalesVMForError>();
            bool pass = true;

            foreach (Sales sales in salesInformation.Sales)
            {
                Product product = (from p in db.Products where p.TempProductStockID == sales.TempProductStockID orderby p.ID descending select p).First();
                if (product == null)
                {
                    SalesVMForError salesVMForError = new SalesVMForError()
                    {
                        Sales = sales,
                        Error = "Unable to load store Information. Posiable No Information registered about the product"
                    }; pass = false;
                    ViewBag.errorMessage = "Some error found see error detail for more information";
                    ErrorList.Add(salesVMForError);
                }
                else {
                    SalesVMForError salesVMForError = new SalesVMForError()
                    {
                        Sales = sales,
                        Error = "The avalable on product in store is less than requested quantity" + product.Total
                }; pass = false;
                ViewBag.errorMessage = "Some error found see error detail for more information";
                ErrorList.Add(salesVMForError);
                }   
            }
            if (pass)
            {
                foreach (Sales sales in salesInformation.Sales)
                {
                    Product product = (from pr in db.Products where pr.TempProductStockID == sales.TempProductStockID orderby pr.ID descending select pr).First();
                    product.Total -= (float)sales.Quantity;
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();

                    ProductAvialableOnStock productAvialableOnStock = db.ProductAvialableOnStock.Find(sales.TempProductStockID);
                    TempProductStock p = db.TempProductStocks.Find(productAvialableOnStock.ID);

                    float deference = productAvialableOnStock.RecentlyReducedProduct - sales.Quantity;
                    if (deference > 0)
                    {
                        productAvialableOnStock.RecentlyReducedProduct -= (float)sales.Quantity;
                    }
                    else
                    {
                        productAvialableOnStock.RecentlyReducedProduct = 0;
                        productAvialableOnStock.ProductAvaliable += deference;
                    }
                    db.Entry(productAvialableOnStock).State = EntityState.Modified;
                    db.SaveChanges();
                }
                salesInformation.Status = "Approved";
                salesInformation.Approvedby = User.Identity.GetUserName();
                db.Entry(salesInformation).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.succsessMessage = "Product Sale is Approved!!";
            }
            else {
                ViewBag.errorList = ErrorList;
            }
        return View("SalesList", salesInformation);
        }

        //sales Rejection
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult SalesRejected()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}