using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.ProductInfo;
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
    [Authorize]
    public class SalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Request
        public ActionResult SalesList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var salesinfo = (from sales in db.SalesInformation orderby sales.ID ascending select sales).Include(J => J.Customer).ToList();

            return View(salesinfo);
        }

        [Authorize(Roles = "Super Admin, Admin, Finance")]
        public ActionResult NewSalesEntry()
        {
            if (TempData[User.Identity.GetUserId() + "successMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            //Frist required Item /1 Material is in the stock or not
            {
                ViewBag.customer = "";
                if (db.Customers.ToList().Count == 0)
                {
                    ViewBag.custumer = "Register Customer frist";
                }
            }
            ViewBag.ProductID = (from S in db.Items where S.StoreType == StoreType.ProductItem orderby S.ID descending select S).ToList();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Admin, Finance")]

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
            sales.RemaningDelivery = (float)sales.Quantity;

            if (ModelState.IsValid)
            {
                foreach (SalesVM s in selectedSales)
                {
                    if (s.Sales.ItemID == sales.ItemID)
                    {
                        if (AvalableQuantity(s.Sales.ItemID) > 0 && (s.Sales.Quantity + sales.Quantity) <= AvalableQuantity(s.Sales.ItemID))
                        {
                            s.Sales.Quantity += sales.Quantity;
                            found = true;
                            ViewBag.infoMessage = "Item is Added !!!";
                            break;
                        }
                        else if (AvalableQuantity(s.Sales.ItemID) == -2)
                        {
                            ViewBag.errorMessage = "Unable to load Item Information!.";
                            found = true;
                        }
                        else
                        {
                            ViewBag.errorMessage = "Low Stock previously " + s.Sales.Quantity + " Selected Only " + AvalableQuantity(s.Sales.ItemID) + " avaliable!.";
                            found = true;
                        }
                    }
                }

                if (found == false)
                {
                    if (AvalableQuantity(sales.ItemID) > 0 && (sales.Quantity) <= AvalableQuantity(sales.ItemID))
                    {
                        SalesVM salesVM = new SalesVM();

                        Item item = db.Items.Find(sales.ItemID);
                        salesVM.TypeOfProduct = item.Name;
                        salesVM.Code = item.Code;
                        salesVM.Unit = item.Unit.Name;
                        salesVM.Sales = sales;
                        selectedSales.Add(salesVM);

                    }
                    else if (AvalableQuantity(sales.ItemID) == -2)
                    {
                        ViewBag.errorMessage = "Unable to lode Item Information!.";
                    }
                    else
                    {
                        ViewBag.errorMessage = "Only " + AvalableQuantity(sales.ItemID) + " avalable!.";
                    }
                }
            }
            else
            {
                ViewBag.errorMessage = "State is not valid";
            }
            TempData[User.Identity.GetUserId() + "selectedSales"] = selectedSales;
            ViewBag.selectedSales = selectedSales;
            if (selectedSales.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.ProductID = (from S in db.Items where S.StoreType == StoreType.ProductItem orderby S.ID descending select S).ToList();
           

            return View();
        }

        [Authorize(Roles = "Super Admin, Admin, Finance")]

        public ActionResult Remove(int id)
        {
            if (TempData[User.Identity.GetUserId() + "selectedSales"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Selected Sales. try again.";
                return RedirectToAction("NewSalesEntry");
            }
            List<SalesVM> selectedSales = new List<SalesVM>();
            selectedSales = (List<SalesVM>)TempData[User.Identity.GetUserId() + "selectedSales"];
            foreach (SalesVM s in selectedSales)
            {
                if (s.Sales.ItemID == id)
                {
                    selectedSales.Remove(s);
                    ViewBag.succsessMessage = "Sale Item is Successfully Removed";
                    break;
                }
            }
            if (selectedSales.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.selectedSales = selectedSales;
            TempData[User.Identity.GetUserId() + "selectedSales"] = selectedSales;
            ViewBag.ProductID = (from S in db.Items where S.StoreType == StoreType.ProductItem orderby S.ID descending select S).ToList();
            return View("NewSalesEntry");
        }

        [Authorize(Roles = "Super Admin, Admin")]
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
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected Sales request";
                return RedirectToAction("NewSalesEntry");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName","LastName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Admin, Finance")]

        public ActionResult NewSalesInfo(SalesInformation salesInformation)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name");

            List<SalesVM> selectedSales = new List<SalesVM>();

            if (TempData[User.Identity.GetUserId() + "selectedSales"] != null)
            {
                selectedSales = (List<SalesVM>)TempData[User.Identity.GetUserId() + "selectedSales"];
            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected sales";
                return RedirectToAction("NewSalesEntry");
            }
            salesInformation.ApplicationUserID = User.Identity.GetUserId();
            try
            {
                int LastId = (from sr in db.SalesInformation orderby sr.ID descending select sr.ID).First();
                salesInformation.FsNo = "No:-" + (LastId + 1).ToString("D4");
            }
            catch
            {
                salesInformation.FsNo = "No:-" + 1.ToString("D4");
            }
            float totalPrice = 0;
            salesInformation.Date = DateTime.Now;
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
                            double Tax = 1.15;
                            double ETax = 0.08;
                            SalesInformation si = db.SalesInformation.Find(salesInformation.ID);
                            si.ExciseTax = totalPrice * (float)ETax;
                            si.Total = totalPrice + si.ExciseTax;
                            si.TotalSellingPrice = si.Total * (float)Tax;
                            si.VAT = si.TotalSellingPrice - si.Total;
                            db.Entry(si).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        ViewBag.succsessMessage = "Sales is Succesfully Submited!!";
                        return RedirectToAction("SalesDetail", new { id = salesInformation.ID });
                    }
                    catch (Exception e)
                    {
                        ViewBag.errorMessage = "Unable to preform the request you need View error detail" + e;
                    }
                }
            }
            return View();
        }

        public ActionResult SalesDetail(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected! Try agian";
                return RedirectToAction("SalesList");
            }
            SalesInformation salesInformation = db.SalesInformation.Find(id);
            if (salesInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Store Request Information";
                return RedirectToAction("SalesList");
            }
            return View(salesInformation);
        }


        //Request is approved 
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult SaleApproved(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected!. Try again";
                return RedirectToAction("SalesList");
            }
            SalesInformation salesInformation = db.SalesInformation.Find(id);
            if (salesInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Reterive Store Request Information";
                return RedirectToAction("SalesList");
            }
            List<SalesVMForError> ErrorList = new List<SalesVMForError>();
            bool pass = true;
            foreach (Sales sales in salesInformation.Sales)
            {
                ProductlogicalAvaliable productlogicalAvaliable = db.ProductlogicalAvaliables.Find(sales.ItemID);
                if (sales == null)
                {
                    SalesVMForError storeRequstVMForError = new SalesVMForError()
                    {
                        Sales = sales,
                        Error = "Unable to load store Information Posible reason is no information registed about the item"
                    }; pass = false;
                    ViewBag.errorMessage = "Some error found. see error detail for more information";
                    ErrorList.Add(storeRequstVMForError);
                }
                else if (productlogicalAvaliable.LogicalProductAvaliable < sales.Quantity)
                {
                    SalesVMForError storeRequstVMError = new SalesVMForError()
                    {
                        Sales = sales,
                        Error = "The Avaliable stock in Comfy store is less than requested Quantity" + productlogicalAvaliable.LogicalProductAvaliable
                    };
                    pass = false;
                    ViewBag.errorMessage = "The Avaliable stock in Comfy  is less than requested Quantity" + productlogicalAvaliable.LogicalProductAvaliable;
                    ErrorList.Add(storeRequstVMError);
                }
            }
            if (pass)
            {
                foreach (Sales sales in salesInformation.Sales)
                {
                    Transfer transfer = (from s in db.Transfers where s.ItemID == sales.ItemID select s).First();
                    //stock.Total -= (float)storeRequest.Quantity;
                    db.Entry(transfer).State = EntityState.Modified;
                    db.SaveChanges();
                    ProductlogicalAvaliable productlogicalAvaliable = db.ProductlogicalAvaliables.Find(sales.ItemID);
                    Item i = db.Items.Find(productlogicalAvaliable.ID);
                    float deference = productlogicalAvaliable.RecentlyReduced - sales.Quantity;

                    if (deference > 0)
                    {
                        productlogicalAvaliable.RecentlyReduced -= (float)sales.Quantity;
                    }
                    else
                    {
                        productlogicalAvaliable.RecentlyReduced = 0;
                        productlogicalAvaliable.LogicalProductAvaliable += deference;

                    }
                    db.Entry(productlogicalAvaliable).State = EntityState.Modified;
                    db.SaveChanges();
                }
                salesInformation.Status = "Approved";
                salesInformation.Approvedby = User.Identity.GetUserName();
                db.Entry(salesInformation).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.succsessMessage = "Sales is approved!!.";
            }

            else
            {
                ViewBag.erroList = ErrorList;
            }

            return View("SalesDetail", salesInformation);
        }

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
                if (pass)
                {
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
            }
            
            return View("SalesDetail", salesInformation);
        }
       
        //to check avaliable on stock 
        float AvalableQuantity(int item)
        {
            ProductlogicalAvaliable avalable = db.ProductlogicalAvaliables.Find(item);
            if (avalable != null)
            {
                return avalable.LogicalProductAvaliable;
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