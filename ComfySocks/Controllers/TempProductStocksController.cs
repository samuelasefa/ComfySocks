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
using ComfySocks.Models.ProductStock;
using Microsoft.AspNet.Identity;

namespace ComfySocks.Controllers
{
    public class TempProductStocksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TempProductStocks
        public ActionResult TemporaryProductList()
        {
            //succsessMessage
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null)
            { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            //errorMessage handler
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null)
            { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var TempInfoList = (from t in db.TempProductInfos select t).Include(t => t.TempProductStock).Include(t => t.ApplicationUser);

            ViewBag.center = true;
            return View(TempInfoList.ToList());
        }

        //New Temprory Product Entry 
        //GET:Product
        [Authorize(Roles = "Super Admin, Admin, StoreManager, Production")]
        public ActionResult NewTempoProductEntry(int id = 0)
        {
            //succsessMessage
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null)
            { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            //errorMessage handler
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null)
            { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            {
                var unit = db.Units.ToList();
                var store = db.Stores.ToList();

                ViewBag.unit = "";
                ViewBag.store = "";

                if (unit.Count() == 0)
                {
                    ViewBag.unit = "Register Unit Inforamtion before adding Product";
                }
                if (store.Count() == 0) {
                    ViewBag.store = "Register Store Information before adding Product";
                }
            }
            ViewBag.ItemID = (from I in db.Items where I.StoreType == StoreType.Product orderby I.Code ascending select I);
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            if (id != 0)
            {
                List<TempProductViewModel> SelectedList = new List<TempProductViewModel>();
                SelectedList = (List<TempProductViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
                TempData[User.Identity.GetUserId() + "SelectedList"] = SelectedList;
                ViewBag.SelectedList = SelectedList.ToList();

            }
            else
            {
                TempData[User.Identity.GetUserId() + "SelectedList"] = null;
            }
            return View();
        }


        //Post:TempProduct Item

        [HttpPost]
        [Authorize(Roles = "Store Manager, Super Admin, Admin")]
        public ActionResult NewTempoProductEntry(TempProductStock tempProductStock)
        {
            //succsessMessage
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null)
            { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            //errorMessage handler
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null)
            { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }


            //List of Product item
            ViewBag.ItemID = (from I in db.Items where I.StoreType == StoreType.Product orderby I.Code ascending select I);
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");

            List<TempProductViewModel> SelectedList = new List<TempProductViewModel>();
            if (TempData[User.Identity.GetUserId() + "SelectedList"] != null)
            {
                try
                {
                    SelectedList = (List<TempProductViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
                    foreach (TempProductViewModel item in SelectedList)
                    {
                        if (item.TempProductStock.Item.Code== tempProductStock.Item.Code)
                        {
                            item.TempProductStock.Quantity += tempProductStock.Quantity;
                            ViewBag.succsessMessage = "Product Item is Added to Code =" + item.TempProductStock.Item.Code;
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
            tempProductStock.TempProductInfoID = 1;
            TempProductViewModel model = new TempProductViewModel();
            Item i = db.Items.Find(tempProductStock.ItemID);
            Store s = db.Stores.Find(tempProductStock.PStoreID);

            if (i == null || s == null)
            {
                ViewBag.erroressage = "Unable to extract item information!.";
            }
            else {
                model.TypeOfProduct = i.Name;
                model.ProductCode = i.Code;
                model.ProductUnit = i.Unit.Name;
                model.TempProductStock = tempProductStock;

                SelectedList.Add(model);
            }
            TempData[User.Identity.GetUserId() + "SelectedList"] = SelectedList;
            ViewBag.SelectedList = SelectedList.ToList();
            return View();
        }
        //Remove selecte List item

            [Authorize(Roles ="Store Manager, Super Admin, Admin")]
        public ActionResult RemoveSelected(int id = 0)
        {
            //succsessMessage
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null)
            { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            //errorMessage handler
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null)
            { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            //Item and store List Item
            ViewBag.ItemID = (from I in db.Items where I.StoreType == StoreType.Product orderby I.Code ascending select I);
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");


            List<TempProductViewModel> SelectedList = new List<TempProductViewModel>();
            if (TempData[User.Identity.GetUserId()+ "SelectedList"] != null)
            {
                try
                {
                    SelectedList = (List<TempProductViewModel>)TempData[User.Identity.GetUserId()+ "SelectedList"];
                    foreach (TempProductViewModel items in SelectedList)
                    {
                        if (items.TempProductStock.ItemID == id)
                        {
                            SelectedList.Remove(items);
                            db.Entry(SelectedList).State = EntityState.Modified;
                            db.SaveChanges();
                            if (SelectedList.Count() > 0)
                            {
                                TempData[User.Identity.GetUserId() + "SelectedList"] = SelectedList;
                                ViewBag.SelectedList = SelectedList;
                            }
                            return View("NewTempoProductEntry");
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = e;
                }
            }
            ViewBag.errorMessage = "You process can not be done now please try again latter";
            return View("NewTempoProductEntry");
        }


        //GET:TempoProduct Information
        [Authorize(Roles ="Super Admin, Admin, Store Manager")]
        public ActionResult TemProductInfo()
        {
            //succsessMessage
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null)
            { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            //errorMessage handler
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null)
            { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

          

            if (TempData[User.Identity.GetUserId() + "SelectedList"] == null)
            {
                ViewBag.errorMessage = "Unable to extract Selected Product information";
                return RedirectToAction("NewTempoProductEntry");
            }
            TempData[User.Identity.GetUserId() + "SelectedList"] = TempData[User.Identity.GetUserId() + "SelectedList"];
            return View();
        }


        //Post: TempProduct information
        [Authorize(Roles ="Super Admin, Admin, Store Manager")]
        [HttpPost]
        public ActionResult TemProductInfo(TempProductInfo tempProductInfos)
        {
            //succsessMessage
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null)
            { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            //errorMessage handler
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null)
            { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId() + "SelectedList"] == null)
            {
                ViewBag.errorMessage = "Unable to extract Selected Product Item information";
                return RedirectToAction("NewTempoProductEntry");
            }

            List<TempProductViewModel> productStock = new List<TempProductViewModel>();
            productStock = (List<TempProductViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
            TempData[User.Identity.GetUserId() + "SelectedList"] = productStock;

            if (tempProductInfos.Date == null ||  tempProductInfos.Date > DateTime.Now)
            {
                ViewBag.errorMessage = "The Date you Provide is not valid";
                return View(tempProductInfos);
            }
            tempProductInfos.ApplicationUserID = User.Identity.GetUserId();
            db.TempProductInfos.Add(tempProductInfos);
            db.SaveChanges();
        
            List<TempProductViewModel> insertedProduct = new List<TempProductViewModel>();
            TempProductInfo tempProductInfo = db.TempProductInfos.Find(tempProductInfos.ID);
            if (tempProductInfo !=null) {
                foreach (TempProductViewModel items in productStock)
                {
                    try
                    {
                        try
                        {
                            TempProductStock Ps = (from t in db.TempProductStocks where items.TempProductStock.ItemID == t.ItemID && items.TempProductStock.PStoreID == t.PStoreID select t).First();
                            db.Entry(Ps).State = EntityState.Modified;
                            db.SaveChanges();
                            items.TempProductStock.TempProductTotal += Ps.TempProductTotal + items.TempProductStock.Quantity;
                        }
                        catch 
                        {
                            items.TempProductStock.TempProductTotal += items.TempProductStock.Quantity;
                        }
                        items.TempProductStock.TempProductTotal += items.TempProductStock.Quantity;
                        items.TempProductStock.TempProductInfoID = tempProductInfos.ID;
                        db.TempProductStocks.Add(items.TempProductStock);
                        db.SaveChanges();

                        ProductAvialableOnStock PA = db.ProductAvialableOnStock.Find(items.TempProductStock.ItemID);
                        if (PA == null)
                        {
                            PA = new ProductAvialableOnStock()

                            {
                                ID = items.TempProductStock.ItemID,
                                ProductAvaliable = items.TempProductStock.Quantity
                            };
                            db.ProductAvialableOnStock.Add(PA);
                            db.SaveChanges();
                        }
                    else
                        {
                            PA.ProductAvaliable += items.TempProductStock.Quantity;
                            db.Entry(PA).State = EntityState.Modified;
                            db.SaveChanges();
                    }
                        insertedProduct.Add(items);
                    
                    }
                    catch 
                    {
                        List<TempProductStock> productStocks = (from s in db.TempProductStocks where s.TempProductInfoID == tempProductInfo.ID select s).ToList();
                        foreach (var item in insertedProduct)
                        {
                            db.TempProductStocks.Remove(item.TempProductStock);
                            db.SaveChanges();
                            Item I = db.Items.Find(item.TempProductStock.ItemID);
                            
                            ProductAvialableOnStock AV = db.ProductAvialableOnStock.Find(item.TempProductStock.ItemID);
                            AV.ProductAvaliable -= item.TempProductStock.Quantity;
                            db.Entry(AV).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        db.TempProductInfos.Remove(tempProductInfo);
                        db.SaveChanges();
                        TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to register stock information";
                        return RedirectToAction("TemporaryProductList");
                    }
                }
                TempData[User.Identity.GetUserId() + "succsessMessage"] = "Product Stock information Saved";
                return RedirectToAction("TemporaryProductList");
            }
            else
            {
                ViewBag.errorMessage = "Unable to lode Product Stock Information";
            }

            TempData[User.Identity.GetUserId() + "SelectedList"] = TempData[User.Identity.GetUserId() + "SelectedList"];
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
