using ComfySocks.Models;
using ComfySocks.Models.Items;
using ComfySocks.Models.ProductInfo;
using ComfySocks.Models.ProductStock;
using ComfySocks.Models.ProductTransferInfo;
using ComfySocks.Models.SalesInfo;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        public ActionResult NewSalesEntry()
        {
            //display if error mesage send from other controler
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            //display if succsess mesage send from other controler
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            //precondition for New 
            ViewBag.customer = "";
            if (db.Customers.ToList().Count == 0) {
                ViewBag.custumer = "Register Customer frist";
            }
            ViewBag.TransferID = (from p in db.Transfers where p.TransferInformation.Status == "Recivied" orderby p.ID descending select p).ToList();
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Authorize(Roles = "Super Admin, Admin, Sales")]
        public ActionResult NewSalesEntry(Sales sales)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<SalesVM> selectedSales = new List<SalesVM>();
            bool found = false;
            if (TempData[User.Identity.GetUserId() + "selectedSales"] != null)
            {
                selectedSales = (List<SalesVM>)TempData[User.Identity.GetUserId() + "selectedSales"];
            }
            sales.SalesInformationID = 1;
            sales.RemaningDelivery += (float)sales.Quantity;
            float totalPrice = 0;

            var item = db.ProStock.Find(sales.ItemID);

            if (ModelState.IsValid)
            {

                foreach (SalesVM t in selectedSales)
                {
                    if (t.Sales.ItemID == sales.ItemID)
                    {
                        if (ProductTransferdAvaliable(t.Sales.ItemID) > 0 && (t.Sales.Quantity + sales.Quantity) <= ProductTransferdAvaliable(t.Sales.ItemID))
                        {
                            t.Sales.Quantity += sales.Quantity;
                            found = true;
                            ViewBag.infoMessage = "Product Item is Added!";
                            break;
                        }
                        else if (ProductTransferdAvaliable(t.Sales.ItemID) == -2)
                        {
                            ViewBag.errorMessage = "Unable to load product information!.";
                            found = true;
                        }
                        else
                        {
                            ViewBag.errorMessage = "Low Stock previously " + t.Sales.Quantity + " Selected Only " + ProductTransferdAvaliable(t.Sales.ItemID) + " avaliable!.";
                            found = true;
                        }
                    }
                }
                if (!found)
                {
                    if (ProductTransferdAvaliable(sales.ItemID) > 0 && (sales.Quantity) <= ProductTransferdAvaliable(sales.ItemID))
                    {
                        SalesVM salesVM = new SalesVM();
                        Item I = db.Items.Find(sales.ItemID);
                        salesVM.TypeOfProduct = I.Name;
                        salesVM.Code = I.Code;
                        salesVM.Unit = I.Unit.Name;
                        salesVM.Sales = sales;
                        selectedSales.Add(salesVM);
                    }
                    else if (ProductTransferdAvaliable(sales.ItemID) == -2)
                    {
                        ViewBag.errorMessage = "Unable to load Product Information!";
                    }
                    else
                    {
                        ViewBag.errorMessage = "Only " + ProductTransferdAvaliable(sales.ItemID) + "Avaliable";
                    }
                }
            }
            else
            {
                ViewBag.errorMessage = "State is not valid";
            }
            ViewBag.selectedSales = selectedSales;
            TempData[User.Identity.GetUserId() + "selectedSales"] = selectedSales;
            TempData[User.Identity.GetUserId() + "totalPrice"] = totalPrice;

            if (selectedSales.Count() > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.TransferID = (from p in db.Transfers where p.TransferInformation.Status == "Recivied" orderby p.ID descending select p).ToList();

            return View(sales);
        }

        [Authorize(Roles = "Super Admin, Admin, Sales")]
        public ActionResult NewSalesInfo()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            List<SalesVM> selectedSales = new List<SalesVM>();


            if (TempData[User.Identity.GetUserId() + "selectedSales"] != null)
            {
                selectedSales = (List<SalesVM>)TempData[User.Identity.GetUserId() + "selectedSales"];
                TempData[User.Identity.GetUserId() + "selectedSales"] = selectedSales;

            }
            else { 
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load selected sales information!! try again";
                return RedirectToAction("NewSalesEntry");
            }
            TempData[User.Identity.GetUserId() + "selectedSales"] = TempData[User.Identity.GetUserId() + "selectedSales"];
            TempData[User.Identity.GetUserId() + "totalPrice"] = TempData[User.Identity.GetUserId() + "totalPrice"];
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult NewSalesInfo(SalesInformation salesInformation)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<SalesVM> selectedSales = new List<SalesVM>();
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName");
            if (TempData[User.Identity.GetUserId() + "selectedSales"] != null) {
                selectedSales = (List<SalesVM>)TempData[User.Identity.GetUserId() + "selectedSales"];
                TempData[User.Identity.GetUserId() + "selectedSales"] = selectedSales;
            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load selected sales information!! try again";
                return RedirectToAction("NewSalesEntry");
            }

            salesInformation.ApplicationUserID = User.Identity.GetUserId();

            try
            {
                int LastId = (from sr in db.SalesInformation orderby sr.ID descending select sr.ID).First();
                salesInformation.FsNo = "FS.No:-" + (LastId + 1);
            }
            catch 
            {
                salesInformation.FsNo = "FS.No:-1";
            }
            salesInformation.Date = DateTime.Now;
            float totalPrice = 0;
            bool pass = true;
            if (ModelState.IsValid)
            {
                try
                {
                    salesInformation.Status = "Submmited";
                    db.SalesInformation.Add(salesInformation);
                    db.SaveChanges();
                    pass = true;
                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = "Unable to Preform the Requste you need! View Error detial" + e;
                    pass = false;
                }
                if (pass)
                {
                    try
                    {
                        foreach (SalesVM s in selectedSales)
                        {
                            s.Sales.SalesInformationID = salesInformation.ID;
                            s.Sales.RemaningDelivery = (float)s.Sales.Quantity;
                            db.Sales.Add(s.Sales);
                            db.SaveChanges();
                            totalPrice += (float)s.Sales.Quantity * (float)s.Sales.UnitPrice;
                            //ProductlogicalAvaliable PV = db.ProductlogicalAvaliables.Find(s.Sales.ItemID);
                            //PV.LogicalProductAvaliable -= (float)s.Sales.Quantity;
                            //PV.RecentlyReduced += (float)s.Sales.Quantity;
                            //db.Entry(PV).State = EntityState.Modified;
                            //db.SaveChanges();
                            double Tax = 1.15;
                            SalesInformation si = db.SalesInformation.Find(salesInformation.ID);
                            si.SubTotal = totalPrice;
                            si.GrandTotal = totalPrice * (float)Tax;
                            si.Tax = si.GrandTotal - si.SubTotal;
                            db.Entry(si).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            return View(salesInformation);
        }

        [Authorize(Roles = "Salse,Super Admin,Admin,")]
        public ActionResult RemoveSelected(int id)
        {
            if (TempData[User.Identity.GetUserId() + "selectedSales"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find selected orders. try again.";
                return RedirectToAction("NewSalesEntry");
            }
            List<SalesVM> selectedSales = new List<SalesVM>();
            selectedSales = (List<SalesVM>)TempData[User.Identity.GetUserId() + "selectedSales"];
            float totalPrice = (float)TempData[User.Identity.GetUserId() + "totalPrice"];
            foreach (var s in selectedSales)
            {
                if (s.Sales.ItemID == id)
                {
                    selectedSales.Remove(s);
                    ViewBag.succsessMessage = "Sales successfully Removed";
                    break;
                }
            }
            if (selectedSales.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.selectedSales = selectedSales;
            TempData[User.Identity.GetUserId() + "selectedSales"] = selectedSales;
            TempData[User.Identity.GetUserId() + "totalPrice"] = totalPrice;
            ViewBag.TransferID = (from p in db.Transfers where p.TransferInformation.Status == "Transferd" orderby p.ID descending select p).ToList();
            return View("NewSalesEntry");
        }

        //sales approval
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult SaleApproved(int? id)
        {
            if (id == null) {
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
                Transfer transfer = (from p in db.Transfers where p.ItemID == sales.ItemID orderby p.ID descending select p).First();
                if (transfer == null)
                {
                    SalesVMForError salesVMForError = new SalesVMForError()
                    {
                        Sales = sales,
                        Error = "Unable to load store Information. Posiable No Information registered about the product"
                    }; pass = false;
                    ViewBag.errorMessage = "Some error found see error detail for more information";
                    ErrorList.Add(salesVMForError);
                }
                else if(transfer.Total < sales.Quantity) {
                    SalesVMForError salesVMForError = new SalesVMForError()
                    {
                        Sales = sales,
                        Error = "The avalable on product in store is less than requested quantity" + transfer.Total
                }; pass = false;
                ViewBag.errorMessage = "Some error found see error detail for more information";
                ErrorList.Add(salesVMForError);
                }   
            }
            if (pass)
            {
                foreach (Sales sales in salesInformation.Sales)
                {
                    Transfer transfer = (from p in db.Transfers where p.ItemID == sales.ItemID orderby p.ID descending select p).First();
                    transfer.Total -= (float)sales.Quantity;
                    db.Entry(transfer).State = EntityState.Modified;
                    db.SaveChanges();

                    ProductlogicalAvaliable productlogicalAvaliable = db.ProductlogicalAvaliables.Find(sales.ItemID);
                    Item I = db.Items.Find(productlogicalAvaliable.ID);

                    float deference = productlogicalAvaliable.LogicalProductAvaliable - sales.Quantity;
                    if (deference > 0)
                    {
                        productlogicalAvaliable.RecentlyReduced -= (float)sales.Quantity;
                    }
                    else
                    {
                        productlogicalAvaliable.RecentlyReduced= 0;
                        productlogicalAvaliable.RecentlyReduced += deference;
                    }
                    db.Entry(productlogicalAvaliable).State = EntityState.Modified;
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
        return View("SalesDetail", salesInformation);
        }

        //sales Rejection
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult SalesRejected(int? id)
        {
            if (id == null)
            {
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
                Transfer transfer = (from p in db.Transfers where p.ItemID == sales.ItemID orderby p.ID descending select p).First();
                if (transfer == null)
                {
                    SalesVMForError salesVMForError = new SalesVMForError()
                    {
                        Sales = sales,
                        Error = "Unable to load store Information. Posiable No Information registered about the product"
                    }; pass = false;
                    ViewBag.errorMessage = "Some error found see error detail for more information";
                    ErrorList.Add(salesVMForError);
                }
                else if (transfer.Total < sales.Quantity)
                {
                    SalesVMForError salesVMForError = new SalesVMForError()
                    {
                        Sales = sales,
                        Error = "The avalable on product in store is less than requested quantity" + transfer.Total
                    }; pass = false;
                    ViewBag.errorMessage = "Some error found see error detail for more information";
                    ErrorList.Add(salesVMForError);
                }
            }
            if (pass)
            {
                foreach (Sales sales in salesInformation.Sales)
                {
                    Transfer transfer = (from p in db.Transfers where p.ItemID == sales.ItemID orderby p.ID descending select p).First();
                    transfer.Total = (float)sales.Quantity;
                    db.Entry(transfer).State = EntityState.Modified;
                    db.SaveChanges();
                }
                salesInformation.Status = "Rejected";
                salesInformation.Approvedby = User.Identity.GetUserName();
                db.Entry(salesInformation).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.succsessMessage = "Sale is Rejected!!";
            }
            else
            {
                ViewBag.errorList = ErrorList;
            }
            return View("SalesDetail", salesInformation);
        }

        private float ProductTransferdAvaliable(int item)
        {
            ProductlogicalAvaliable productlogical = db.ProductlogicalAvaliables.Find(item);

            if (productlogical != null)
            {

                return productlogical.LogicalProductAvaliable;
            }
            return -2;
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