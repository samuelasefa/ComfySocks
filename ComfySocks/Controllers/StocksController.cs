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
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult StockList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var StockInfo = (from s in db.StockInformation select s).Include(e => e.Stocks).Include(e => e.ApplicationUser);
            ViewBag.center = true;

            return View(StockInfo.ToList());
        }

        // GET: Stocks/Create
        [Authorize(Roles = "Store Manager, Super Admin, Admin,")]
        public ActionResult NewPurchaseEntry(int id = 0)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            //validate prerequired informtion  exsts

            {
                var item = db.Items.ToList();
                var supplier = db.Suppliers.ToList();
                var store = db.Stores.ToList();
                ViewBag.item = "";
                ViewBag.supplier = "";
                ViewBag.store = "";
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
                if (store.Count() == 0)
                {
                    ViewBag.store = "Register store informtion frist";
                    ViewBag.RequiredItems = true;
                }
            }
            ViewBag.ItemID = (from I in db.Items where I.StoreType == StoreType.RowMaterial || I.StoreType == StoreType.OfficeMaterial orderby I.Code ascending select I);
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
            return View();
        }


        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Store Manager, Super Admin, Admin,")]
        [HttpPost]
        public ActionResult NewPurchaseEntry(Stock stock)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            ViewBag.ItemID = (from I in db.Items where I.StoreType == StoreType.RowMaterial || I.StoreType == StoreType.OfficeMaterial orderby I.Code ascending select I);
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
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
            stock.StockInformationID = 1;
            StockViewModel model = new StockViewModel();
            Item i = db.Items.Find(stock.ItemID);
            Store s = db.Stores.Find(stock.StoreID);
            if (i == null || s == null || stock.Quantity == 0)
            {
                ViewBag.errorMessage = "Unable to extract Item Information/Field is not Fullfilled";
            }
            else
            {
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

            ViewBag.ItemID = (from I in db.Items where I.StoreType == StoreType.RowMaterial || I.StoreType == StoreType.OfficeMaterial orderby I.Code ascending select I);
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");

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
        public ActionResult NewStockInformation()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId() + "SelectedList"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Selected Stock Information. try agin";
                return RedirectToAction("NewOfficePurchaseEntry");
            }
            TempData[User.Identity.GetUserId() + "SelectedList"] = TempData[User.Identity.GetUserId() + "SelectedList"];
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Admin, Store Manager,")]
        public ActionResult NewStockInformation(StockInformation StockInformation)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId() + "SelectedList"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Selected Stock Information. try agin";
                return RedirectToAction("NewPurchaseEntry");
            }
            float totalPrice = 0;
            List<StockViewModel> stocks = new List<StockViewModel>();
            stocks = (List<StockViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
            TempData[User.Identity.GetUserId() + "SelectedList"] = stocks;
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name");
            StockInformation.Date = DateTime.Now;
                StockInformation.ApplicationUserID = User.Identity.GetUserId();
                try
                {
                    var lastID = (from s in db.StockInformation orderby s.ID orderby s.ID descending select s).First();
                    StockInformation.StoreNumber = "P-Info-" + (lastID.ID + 1);
                }
                catch
                {
                    StockInformation.StoreNumber = "P-Info-1";
                }

                db.StockInformation.Add(StockInformation);
                db.SaveChanges();
            List<Stock> insertedStock = new List<Stock>();
            StockInformation stockInformation = db.StockInformation.Find(StockInformation.ID);
            if (stockInformation != null)
            {
                foreach (StockViewModel items in stocks)
                {
                    try
                    {
                        try
                        {
                            Stock S = (from i in db.Stocks where items.Stock.ItemID <= i.ID select i).First();
                            totalPrice += (float)items.Stock.Quantity * (float)items.Stock.UnitPrice;
                            db.Entry(S).State = EntityState.Modified;
                            db.SaveChanges();
                            double Tax = 1.15;
                            stockInformation.SubTotal = totalPrice;
                            stockInformation.GrandTotal = totalPrice * (float)Tax;
                            stockInformation.Tax = stockInformation.GrandTotal - stockInformation.SubTotal;
                            db.Entry(stockInformation).State = EntityState.Modified;
                            db.SaveChanges();
                            //items.Stock.Total += S.Total + items.Stock.Quantity;
                            //items.Stock.ProwTotal += S.ProwTotal + items.Stock.Quantity;
                            if (items.Stock.ItemID == S.ItemID) {
                                TempData[User.Identity.GetUserId() + "SelectedList"] = stocks;
                                ViewBag.stocks = stocks;
                            }
                        }
                        catch
                        {
                            items.Stock.Total +=  items.Stock.Quantity;
                            items.Stock.ProwTotal += items.Stock.Quantity;
                        }
                        Item I = db.Items.Find(items.Stock.ItemID);
                        items.Stock.StockInformationID = StockInformation.ID;
                        db.Stocks.Add(items.Stock);
                        db.SaveChanges();

                        AvaliableOnStock AV = db.AvaliableOnStocks.Find(items.Stock.ItemID);
                        RowMaterialRepositery repositery = db.RowMaterialRepositeries.Find(items.Stock.ItemID);

                        if (AV == null && repositery == null)
                        {
                            AV = new AvaliableOnStock()
                            {
                                ID = items.Stock.ItemID,
                                Avaliable = items.Stock.Quantity
                            };
                            repositery = new RowMaterialRepositery()
                            {
                                ID = items.Stock.ItemID,
                                RowMaterialAavliable = items.Stock.Quantity
                            };
                            db.AvaliableOnStocks.Add(AV); 
                            db.RowMaterialRepositeries.Add(repositery);
                            db.SaveChanges();
                        }
                        else
                        {
                            AV.Avaliable += items.Stock.Quantity;
                            repositery.RowMaterialAavliable += items.Stock.Quantity;
                            db.Entry(AV).State = EntityState.Modified;
                            db.Entry(repositery).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        insertedStock.Add(items.Stock);

                    }
                    catch (Exception)
                    {
                        List<Stock> Stocks = (from s in db.Stocks where s.StockInformationID == StockInformation.ID select s).ToList();
                        foreach (var item in insertedStock)
                        {
                            db.Stocks.Remove(item);
                            db.SaveChanges();
                            Item I = db.Items.Find(item.ItemID);

                            AvaliableOnStock AV = db.AvaliableOnStocks.Find(item.ItemID);
                            AV.Avaliable -= items.Stock.Quantity;
                            db.Entry(AV).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        db.StockInformation.Remove(StockInformation);
                        db.SaveChanges();
                        TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to register stock information";
                        return RedirectToAction("NewPurchaseEntry");
                    }
                }
                TempData[User.Identity.GetUserId() + "succsessMessage"] = "Stock information is Saved";
                return RedirectToAction("NewPurchaseEntry");

            }
            else
            {
                ViewBag.errorMessage = "Unable to load Stock Information";
            }

            TempData[User.Identity.GetUserId() + "SelectedList"] = TempData[User.Identity.GetUserId() + "SelectedList"];
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name");
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
