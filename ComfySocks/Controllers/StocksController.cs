using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Repository;
using Microsoft.AspNet.Identity;

namespace ComfySocks.Controllers
{
    public class StocksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Stocks
        public ActionResult StockList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var StockInformation = (from s in db.StockReferances select s).Include(e => e.Stocks).Include(e => e.ApplicationUser);
            ViewBag.center = true;

            return View(StockInformation.ToList());
        }

        // GET: Stocks/Details/5
        public ActionResult StockDetails(int? id)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Stock Information. try agin";
                return RedirectToAction("StockList");
            }
        
            StockReferance stockinfo = db.StockReferances.Find(id);
            if (stockinfo == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Stock Information. try agin type:'error'";
                return RedirectToAction("StockList");
            }
            return View(stockinfo);
        }

        // GET: Stocks/Create
        [Authorize(Roles="Store Manager, Super Admin, Admin,")]
        public ActionResult NewPurchaseEntry(int id = 0)
        {   
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId()+"succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            {
                var item = db.Items.ToList();
                var store = db.Stores.ToList();
                var supplier = db.Suppliers.ToList();

                ViewBag.store = "";
                ViewBag.item = "";
                ViewBag.supplier = "";
                if (store.Count() == 0) {
                    ViewBag.store = "Register Store Information";
                    ViewBag.RequiredItems = true;
                }
                if (supplier.Count() == 0)
                {
                    ViewBag.supplier = "Register Supplier Information";
                    ViewBag.RequiredItems = true;
                }
                if (item.Count() == 0)
                {
                    ViewBag.item = "Register item Information";
                    ViewBag.RequiredItems = true;
                }
            }
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Name");
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            if (id != 0)
            {
                List<StockViewModel> SelectedList = new List<StockViewModel>();
                SelectedList = (List<StockViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
                TempData[User.Identity.GetUserId() + "SelectedList"] = SelectedList;
                ViewBag.SelectedList = SelectedList.ToList();
            }
            else
            {
                TempData[User.Identity.GetUserId() + "SelectedList"] = null;
            }
            TempData[User.Identity.GetUserId() + "SelectedList"] = TempData[User.Identity.GetUserId() + "SelectedList"];
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Store Manager, Super Admin, Admin,")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPurchaseEntry(Stock stock)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            //ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name");
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Name", stock.ItemID);
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name", stock.StoreID);
            
            List<StockViewModel> SelectedList = new List<StockViewModel>();
            if (TempData[User.Identity.GetUserId() + "SelectedList"] != null)
            {
                try
                {
                    SelectedList = (List<StockViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
                    foreach (StockViewModel items in SelectedList)
                    {
                        if (items.Stock.ItemID == stock.ItemID && items.Stock.StoreID == stock.StoreID)
                        {
                            items.Stock.Quantity += stock.Quantity;
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

            stock.StockReferanceID = 1;
            StockViewModel model = new StockViewModel();
            Item i = db.Items.Find(stock.ItemID);
            Store s = db.Stores.Find(stock.StoreID);

            if (i == null || s == null)
            {
                ViewBag.errorMessage = "Unable to extract Item Information";
            }
            else {
                model.Code = i.Code;
                model.ItemDescription = i.Name;
                model.Type = i.ItemType.Name;
                model.Unit = i.Unit.Name;
                model.Stock = stock;

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

            
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Name");
            List<StockViewModel> SelectedList = new List<StockViewModel>();
            if (TempData[User.Identity.GetUserId() + "SelectedList"] != null)
            {
                try
                {
                    SelectedList = (List<StockViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
                    foreach (StockViewModel items in SelectedList)
                    {
                        if (items.Stock.ItemID == id)
                        {
                            SelectedList.Remove(items);
                            if (SelectedList.Count() > 0)
                            {
                                TempData[User.Identity.GetUserId() + "SelectedList"] = SelectedList;
                                ViewBag.SelectedList = SelectedList;
                            }
                            return View("NewPurchaseEntry");
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
        public ActionResult NewStockInformation(){
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId() + "SelectedList"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Selected Stock Information. try agin";
                return RedirectToAction("NewPurchaseEntry");
            }
            TempData[User.Identity.GetUserId() + "SelectedList"] = TempData[User.Identity.GetUserId() + "SelectedList"];
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name");
            ViewBag.InvoiceID = new SelectList(db.Suppliers, "ID", "No");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Admin, Store Manager,")]
        public ActionResult NewStockInformation(StockReferance stockReferances)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId() + "SelectedList"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Selected Stock Information. try agin";
                return RedirectToAction("NewPurchaseEntry");
            }

            List<StockViewModel> stock = new List<StockViewModel>();
            stock = (List<StockViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
            TempData[User.Identity.GetUserId() + "SelectedList"] = stock;
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name");
            if (stockReferances.Date == null || stockReferances.Date > DateTime.Now)
            {
                ViewBag.errorMessage = "The date you provide is not valid!.";
                return View(stockReferances);
            }
            try
            {
                stockReferances.ApplicationUserID = User.Identity.GetUserId();
                try
                {
                    var lastID = (from s in db.StockReferances orderby s.ID orderby s.ID descending select s).First();
                    stockReferances.StoreNumber = "GR-No-" + (lastID.ID + 1);
                }
                catch
                {
                    stockReferances.StoreNumber = "GR-No-1";
                }
                db.StockReferances.Add(stockReferances);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.errorMessage = "Please Provide date Information";
                return View(stockReferances);
            }

            List<Stock> insertedStock = new List<Stock>();
            StockReferance StockReferances = db.StockReferances.Find(stockReferances.ID);
            if (StockReferances != null)
            {
                foreach (StockViewModel items in stock)
                {
                    try
                    {
                        try
                        {
                            Stock S = (from i in db.Stocks where items.Stock.ItemID == i.ItemID && items.Stock.StoreID == i.StoreID select i).First();
                            db.Entry(S).State = EntityState.Modified;
                            db.SaveChanges();
                            items.Stock.Total += S.Total + items.Stock.Quantity;
                            //added
                            items.Stock.ProwTotal += S.ProwTotal + items.Stock.Quantity;
                        }
                        catch
                        {
                            items.Stock.Total += items.Stock.Quantity;
                            //added
                            items.Stock.ProwTotal += items.Stock.Quantity;

                        }
                        items.Stock.ProwTotal += items.Stock.Quantity;
                        items.Stock.Total += items.Stock.Quantity;
                        Item I = db.Items.Find(items.Stock.ItemID);
                        items.Stock.StockReferanceID = stockReferances.ID;
                        db.Stocks.Add(items.Stock);
                        db.SaveChanges();
                        AvaliableOnStock AV = db.AvaliableOnStocks.Find(items.Stock.ItemID);
                        RowMaterialRepositery repositery = db.RowMaterialRepositeries.Find(items.Stock.ItemID);
                        if (AV == null && repositery == null)
                        {
                            AV = new AvaliableOnStock()
                            {
                                ID = items.Stock.ItemID,
                                Avaliable = items.Stock.Quantity,

                            };
                            //added
                            repositery = new RowMaterialRepositery()
                            {
                                ID = items.Stock.ItemID,
                                RowMaterialAavliable = items.Stock.Quantity,
                            };
                            db.AvaliableOnStocks.Add(AV);
                            //added
                            db.RowMaterialRepositeries.Add(repositery);
                            db.SaveChanges();
                        }
                        else
                        {
                            AV.Avaliable += items.Stock.Quantity;
                            repositery.RowMaterialAavliable += items.Stock.Quantity;
                            db.Entry(AV).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        insertedStock.Add(items.Stock);

                    }
                    catch
                    {
                        List<Stock> stocks = (from s in db.Stocks where s.StockReferanceID == stockReferances.ID select s).ToList();
                        foreach (var item in insertedStock)
                        {
                            db.Stocks.Remove(item);
                            db.SaveChanges();
                            Item I = db.Items.Find(item.ItemID);

                            AvaliableOnStock AV = db.AvaliableOnStocks.Find(item.ItemID);
                            RowMaterialRepositery repositery = db.RowMaterialRepositeries.Find(item.ItemID);
                            AV.Avaliable -= items.Stock.Quantity;
                            repositery.RowMaterialAavliable -= items.Stock.Quantity;
                            db.Entry(AV).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        db.StockReferances.Remove(stockReferances);
                        db.SaveChanges();
                        TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to register stock information";
                        return RedirectToAction("NewPurchaseEntry");
                    }
                }
                TempData[User.Identity.GetUserId() + "infoMessage"] = "Stock information Saved";
                return RedirectToAction("NewPurchaseEntry");

            }
            else
            {
                ViewBag.errorMessage = "Unable to lode Stock Information";
            }
            TempData[User.Identity.GetUserId() + "SelectedList"] = TempData[User.Identity.GetUserId() + "SelectedList"];
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name");
            ViewBag.InvoiceID = new SelectList(db.Suppliers, "ID", "No");
            return View();
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
