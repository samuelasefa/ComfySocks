using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComfySocks.Models;
using ComfySocks.Models.ProductStock;
using ComfySocks.Models.Items;
using ComfySocks.Repository;
using Microsoft.AspNet.Identity;
using ComfySocks.Models.InventoryModel;

namespace ComfySocks.Controllers
{
    public class ProductStocksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Stocks
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult ProductStockList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var ProStockInfo = (from s in db.ProStockInformation select s).Include(e => e.ProStocks).Include(e => e.ApplicationUser);
            ViewBag.center = true;

            return View(ProStockInfo.ToList());
        }

        // GET: Stocks/Create
        [Authorize(Roles = "Store Manager, Super Admin, Admin,")]
        public ActionResult NewProductEntry(int id = 0)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            //validate prerequired informtion  exsts

            {
                var item = db.Items.ToList();
                var store = db.Stores.ToList();
                ViewBag.item = "";
                ViewBag.store = "";
               
                if (item.Count() == 0)
                {
                    ViewBag.item = "Register item Information";
                    ViewBag.RequiredItems = true;
                }
            }
            ViewBag.ItemID = (from I in db.Items where I.StoreType == StoreType.ProductItem  orderby I.Code ascending select I);
            //ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            if (id != 0)
            {
                List<ProStockViewModel> SelectedList = new List<ProStockViewModel>();
                SelectedList = (List<ProStockViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
                TempData[User.Identity.GetUserId() + "SelectedList"] = SelectedList;
                ViewBag.SelectedList = SelectedList.ToList();
            }
            else
            {
                TempData[User.Identity.GetUserId() + "SelectedList"] = null;
            }
            return View();
        }


        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Store Manager, Super Admin, Admin,")]
        [HttpPost]
        public ActionResult NewProductEntry(ProStock proStock)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            ViewBag.ItemID = (from I in db.Items where I.StoreType == StoreType.ProductItem orderby I.Code ascending select I);
            //ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            List<ProStockViewModel> SelectedList = new List<ProStockViewModel>();
            if (TempData[User.Identity.GetUserId() + "SelectedList"] != null)
            {
                try
                {
                    SelectedList = (List<ProStockViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
                    foreach (ProStockViewModel items in SelectedList)
                    {
                        if (items.ProStock.ItemID == proStock.ItemID)
                        {
                            items.ProStock.Quantity += proStock.Quantity;
                            ViewBag.infoMessage = "Item is Added";
                            TempData[User.Identity.GetUserId() + "SelectedList"] = SelectedList;
                            ViewBag.SelectedList = SelectedList;
                            return View();
                        }
                    }

                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = e;
                }

            }
            proStock.ProStockInformationID = 1;
            ProStockViewModel model = new ProStockViewModel();
            Item i = db.Items.Find(proStock.ItemID);
            //Store s = db.Stores.Find(proStock.StoreID);
            if (i == null|| proStock.Quantity == 0)
            {
                ViewBag.errorMessage = "Unable to extract Item Information/Field is not Fullfilled";
            }
            else
            {
                model.Code = i.Code;
                model.ItemDescription = i.Name;
                model.Unit = i.Unit.Name;
                model.ProStock = proStock;

                SelectedList.Add(model);
            };
            TempData[User.Identity.GetUserId() + "SelectedList"] = SelectedList;
            ViewBag.SelectedList = SelectedList.ToList();
            return View();
        }

        [Authorize(Roles = "Store Manager,Super Admin,Admin,")]
        public ActionResult RemoveSelected(int id = 0)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            ViewBag.ItemID = (from I in db.Items where I.StoreType == StoreType.ProductItem orderby I.Code ascending select I);
            //ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");

            List<ProStockViewModel> SelectedList = new List<ProStockViewModel>();
            if (TempData[User.Identity.GetUserId() + "SelectedList"] != null)
            {
                try
                {
                    SelectedList = (List<ProStockViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
                    foreach (ProStockViewModel items in SelectedList)
                    {
                        if (items.ProStock.ItemID == id)
                        {
                            SelectedList.Remove(items);
                            if (SelectedList.Count() > 0)
                            {
                                TempData[User.Identity.GetUserId() + "SelectedList"] = SelectedList;
                                ViewBag.SelectedList = SelectedList;
                            }
                            return View("NewProductEntry");
                        }
                    }

                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = e;
                }

            }
            ViewBag.errorMesage = "You process can't be performed for now try agin";
            return View("NewPurchaseEntry");
        }

        [Authorize(Roles = "Super Admin, Admin, Store Manager,")]
        public ActionResult NewProductStockInformation()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId() + "SelectedList"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Selected Stock Information. try agin";
                return RedirectToAction("NewProductEntry");
            }
            TempData[User.Identity.GetUserId() + "SelectedList"] = TempData[User.Identity.GetUserId() + "SelectedList"];
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Admin, Store Manager,")]
        public ActionResult NewProductStockInformation(ProStockInformation proStockInformation)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId() + "SelectedList"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Selected Stock Information. try agin";
                return RedirectToAction("NewProductEntry");
            }

            List<ProStockViewModel> stocks = new List<ProStockViewModel>();
            stocks = (List<ProStockViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
            TempData[User.Identity.GetUserId() + "SelectedList"] = stocks;
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name");
            proStockInformation.Date = DateTime.Now;
            proStockInformation.Status = "Submmited";
            proStockInformation.ApplicationUserID = User.Identity.GetUserId();
                try
                {
                    var lastID = (from s in db.StockInformation orderby s.ID descending select s).First();
                    proStockInformation.StoreNumber = "P-Info-" + (lastID.ID + 1);
                }
                catch
                {
                    proStockInformation.StoreNumber = "P-Info-1";
                }

                db.ProStockInformation.Add(proStockInformation);
                db.SaveChanges();
            List<ProStock> insertedStock = new List<ProStock>();
            ProStockInformation proStockInformations = db.ProStockInformation.Find(proStockInformation.ID);
            if (proStockInformation != null)
            {
                foreach (ProStockViewModel items in stocks)
                {
                    try
                    {
                        try
                        {
                            ProStock S = (from i in db.ProStock where items.ProStock.ItemID == i.ItemID select i).First();
                            db.Entry(S).State = EntityState.Modified;
                            db.SaveChanges();
                            items.ProStock.Total += S.Total + items.ProStock.Quantity;

                        }
                        catch
                        {
                            items.ProStock.Total += items.ProStock.Quantity;

                        }
                        Item I = db.Items.Find(items.ProStock.ItemID);
                        items.ProStock.ProStockInformationID = proStockInformation.ID;
                        db.ProStock.Add(items.ProStock);
                        db.SaveChanges();

                        ProductAvialableOnStock PV = db.ProductAvialableOnStock.Find(items.ProStock.ItemID);

                        if (PV == null)
                        {
                            PV = new ProductAvialableOnStock()
                            {
                                ID = items.ProStock.ItemID,
                                ProductAvaliable = items.ProStock.Quantity
                            };
                            
                            db.ProductAvialableOnStock.Add(PV);
                            db.SaveChanges();
                        }
                        else
                        {
                            PV.ProductAvaliable += items.ProStock.Quantity;
                            db.Entry(PV).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        insertedStock.Add(items.ProStock);

                    }
                    catch (Exception)
                    {
                        List<ProStock> proStocks = (from s in db.ProStock where s.ProStockInformationID == proStockInformations.ID select s).ToList();
                        foreach (var item in insertedStock)
                        {
                            db.ProStock.Remove(item);
                            db.SaveChanges();
                            Item I = db.Items.Find(item.ItemID);

                            ProductAvialableOnStock PV = db.ProductAvialableOnStock.Find(item.ItemID);
                            PV.ProductAvaliable -= items.ProStock.Quantity;
                            db.Entry(PV).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        db.ProStockInformation.Remove(proStockInformation);
                        db.SaveChanges();
                        TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to register Product Stock information";
                        return RedirectToAction("NewProductEntry");
                    }
                }
                TempData[User.Identity.GetUserId() + "succsessMessage"] = "Product Stock information is Saved";
                return RedirectToAction("NewProductEntry");

            }
            else
            {
                ViewBag.errorMessage = "Unable to load Product Stock Information";
            }

            TempData[User.Identity.GetUserId() + "SelectedList"] = TempData[User.Identity.GetUserId() + "SelectedList"];
            return View();
        }

        // GET: Stocks/Details/5
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult StockDetails(int? id)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Stock Information. try agin";
                return RedirectToAction("StockList");
            }

            StockInformation stockinfo = db.StockInformation.Find(id);
            if (stockinfo == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Stock Information. try agin type:'error'";
                return RedirectToAction("StockList");
            }
            return View(stockinfo);
        }


        //product entry is approved

        public ActionResult ProductApproved(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("ProductStocksList");
            }

            ProStockInformation proStockInformation = db.ProStockInformation.Find(id);

            if (proStockInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load product sock Information";
                return RedirectToAction("ProductStocksList");
            }

            List<ProStockVMForError> ErrorList = new List<ProStockVMForError>();
            bool pass = true;
            foreach (ProStock proStock in proStockInformation.ProStocks)
            {
                if (proStock == null)
                {
                    ProStockVMForError proStockVMForError = new ProStockVMForError()
                    {
                        ProStock = proStock,
                        Error = "Unable to load Product"
                    }; pass = false;

                }
            }
            if (pass)
            {
                foreach (ProStock proStock in proStockInformation.ProStocks)
                {
                    proStockInformation.Status = "Approved";
                    proStockInformation.Approvedby = User.Identity.GetUserName();
                    db.Entry(proStockInformation).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.succsessMessage = "Succesfully Approved";
                }

            }
            else
            {
                ViewBag.errorList = ErrorList;
            }
            return View("ProductStockDetails", proStockInformation);
        }

        //Product entry rejected for transfering product to Stock

        public ActionResult ProductRejected(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("ProductStockList");
            }

            ProStockInformation proStockInformation = db.ProStockInformation.Find(id);

            if (proStockInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Temporary product Information";
                return RedirectToAction("ProductStockList");
            }

            List<ProStockVMForError> ErrorList = new List<ProStockVMForError>();
            bool pass = true;
            foreach (ProStock proStock in proStockInformation.ProStocks)
            {
                if (proStock == null)
                {
                    ProStockVMForError proStockVMForError = new ProStockVMForError()
                    {
                        ProStock = proStock,
                        Error = "Unable to load Product"
                    }; pass = false;

                }
            }
            if (pass)
            {
                foreach (ProStock proStock in proStockInformation.ProStocks)
                {
                    proStockInformation.Status = "Rejected";
                    proStockInformation.Approvedby = User.Identity.GetUserName();
                    db.Entry(proStockInformation).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.succsessMessage = "Succesfully Rejected";

                }

            }
            else
            {
                ViewBag.errorList = ErrorList;
            }
            return View("ProductStockDetails", proStockInformation);
        }

        //Detail of Product
        // GET: Products/Details/5
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult ProductStockDetails(int? id)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Product Information. try agin";
                return RedirectToAction("ProductStockList");

            }
            ProStockInformation proStockInformation = db.ProStockInformation.Find(id);
            if (proStockInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Product Information. try agin type:'error'";
                return RedirectToAction("ProductStockList");
            }
            return View(proStockInformation);
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
