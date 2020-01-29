using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComfySocks.Models;
using ComfySocks.Models.ProductInfo;
using ComfySocks.Models.Items;
using Microsoft.AspNet.Identity;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.ProductStock;
using ComfySocks.Models.Repository;

namespace ComfySocks.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Stocks
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult ProductList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var ProductInformation = (from p in db.ProductInformation select p).Include(e => e.Products).Include(e => e.ApplicationUser);
            ViewBag.center = true;

            return View(ProductInformation.ToList());
        }

        // GET: Stocks/Details/5
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult ProductDetail(int? id)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Product Information. try agin";
                return RedirectToAction("ProductList");
            }

            ProductInformation productinfo = db.ProductInformation.Find(id);
            if (productinfo == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Product Information. try agin type:'error'";
                return RedirectToAction("ProductList");
            }
            return View(productinfo);
        }

        // GET: Stocks/Create
        [Authorize(Roles = "Store Manager, Super Admin, Admin,")]
        public ActionResult NewProductEntry(int id = 0)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            {
                var product = db.TempProductStocks.ToList();
                var store = db.Stores.ToList();

                ViewBag.store = "";
                ViewBag.product = "";
                if (store.Count() == 0)
                {
                    ViewBag.store = "Register Supplier Information";
                    ViewBag.RequiredItems = true;
                }
                if (product.Count() == 0)
                {
                    ViewBag.product = "Register Product Information frist";
                    ViewBag.RequiredItems = true;
                }
            }

            //ViewBag.ProductCodeID = new SelectList(db.ProductCodes, "ID", "Code");
            ViewBag.TempProductStockID = new SelectList(db.TempProductStocks, "ID", "ProductName");
            if (id != 0)
            {
                List<ProductViewModel> SelectedList = new List<ProductViewModel>();
                SelectedList = (List<ProductViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
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
        public ActionResult NewProductEntry(Product product)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            //ViewBag.ProductCodeID = new SelectList(db.ProductCodes, "ID", "ProductCode");
            ViewBag.TempProductStockID = new SelectList(db.TempProductStocks, "ID", "ProductName", product.TempProductStockID);

            List<ProductViewModel> SelectedList = new List<ProductViewModel>();
            
            if (TempData[User.Identity.GetUserId() + "SelectedList"] != null)
            {
                try
                {
                    SelectedList = (List<ProductViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
                    foreach (ProductViewModel items in SelectedList)
                    {
                     if (items.Product.TempProductStockID == product.TempProductStockID && items.Product.Quantity > 0)
                            {
                                items.Product.Quantity += product.Quantity;
                                ViewBag.infoMessage = "Product is Added";
                                TempData[User.Identity.GetUserId() + "SelectedList"] = SelectedList;
                                ViewBag.SelectedList = SelectedList;
                                return View();

                            }
                    }

                }
                catch (Exception)
                {
                    ViewBag.errorMessage = "Please Fill all Product Entry Information First!!";
                }

            }

            product.ProductInformationID = 1;
            ProductViewModel model = new ProductViewModel();
            TempProductStock i = db.TempProductStocks.Find(product.TempProductStockID);
            //Store s = db.Stores.Find(product.StoreID);

            if (i == null)
            {
                ViewBag.errorMessage = "Unable to extract Item Information";
            }
            else
            {
                model.ProductName = i.Item.Name;
                model.Code = i.Item.Code;
                model.Unit = i.Item.Unit.Name;
                model.Product = product;

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


            ViewBag.TempProductStockID = new SelectList(db.TempProductStocks, "ID", "ProductName");
            List<ProductViewModel> SelectedList = new List<ProductViewModel>();
            if (TempData[User.Identity.GetUserId() + "SelectedList"] != null)
            {
                try
                {
                    SelectedList = (List<ProductViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
                    foreach (ProductViewModel items in SelectedList)
                    {
                        if (items.Product.TempProductStockID == id)
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
            return View("NewProductEntry");
        }

        [Authorize(Roles = "Super Admin, Admin, Store Manager,")]
        public ActionResult NewProductInfo()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId() + "SelectedList"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Selected Stock Information. try agin";
                return RedirectToAction("NewProductEntry");
            }
            TempData[User.Identity.GetUserId() + "SelectedList"] = TempData[User.Identity.GetUserId() + "SelectedList"];
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Admin, Store Manager,")]
        public ActionResult NewProductInfo(ProductInformation productInformation)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId() + "SelectedList"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Selected Product Information. try agin";
                return RedirectToAction("NewProductEntry");
            }

            List<ProductViewModel> product = new List<ProductViewModel>();
            product = (List<ProductViewModel>)TempData[User.Identity.GetUserId() + "SelectedList"];
            TempData[User.Identity.GetUserId() + "SelectedList"] = product;
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");

            if (productInformation.Date == null || productInformation.Date > DateTime.Now)
            {
                ViewBag.errorMessage = "The date you provide is not valid!.";
                return View(productInformation);
            }
            try
            {
                productInformation.ApplicationUserID = User.Identity.GetUserId();
                try
                {
                    var lastID = (from p in db.ProductInformation orderby p.ID orderby p.ID descending select p).First();
                    productInformation.ProductNumber = "P-No-" + (lastID.ID + 1);
                }
                catch
                {
                productInformation.ProductNumber = "P-No-1";
                }
                db.ProductInformation.Add(productInformation);
                db.SaveChanges();
            }
            catch(Exception)
            {
                ViewBag.errorMessage = "Please Provide date Information";
                return View(productInformation);
            }

            List<Product> insertedProduct = new List<Product>();
            ProductInformation ProductInformation = db.ProductInformation.Find(productInformation.ID);
            if(ProductInformation != null)
            {
                foreach (ProductViewModel items in product)
                {
                    try
                    {
                        try
                        {
                            Product P = (from i in db.Products where items.Product.TempProductStockID == i.TempProductStockID select i).First();
                            db.Entry(P).State = EntityState.Modified;
                            db.SaveChanges();
                            items.Product.Total += P.Total + items.Product.Quantity;
                            //added
                            items.Product.PPTotal += P.PPTotal + items.Product.Quantity;
                        }
                        catch
                        {
                            items.Product.Total += items.Product.Quantity;
                            //added
                            items.Product.PPTotal += items.Product.Quantity;
                        }
                        items.Product.Total += items.Product.Quantity;
                        items.Product.PPTotal += items.Product.Quantity;
                        items.Product.ProductInformationID = productInformation.ID;
                        db.Products.Add(items.Product);
                        db.SaveChanges();
                        ProductAvialableOnStock AV = db.ProductAvialableOnStock.Find(items.Product.TempProductStockID);
                        ProductMaterialRepository repository = db.ProductMaterialRepositories.Find(items.Product.TempProductStockID);
                        if (AV == null && repository == null)
                        {
                            AV = new ProductAvialableOnStock()
                            {
                                ID = items.Product.TempProductStockID,
                                ProductAvaliable = items.Product.Quantity,

                            };
                            repository = new ProductMaterialRepository()
                            {
                                //ID = items.Product.TempProductStockID,
                                //ProductMaterialAavliable = items.Product.Quantity
                            };
                            db.ProductAvialableOnStock.Add(AV);
                            db.ProductMaterialRepositories.Add(repository);
                            db.SaveChanges();
                        }
                        else
                        {
                            AV.ProductAvaliable += items.Product.Quantity;
                            //repository.ProductMaterialAavliable += items.Product.Quantity;
                            db.Entry(AV).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        insertedProduct.Add(items.Product);
                    }
                    catch
                    {
                        List<Product> products = (from s in db.Products where s.ProductInformationID == productInformation.ID select s).ToList();
                        foreach (var item in insertedProduct)
                        {
                            db.Products.Remove(item);
                            db.SaveChanges();
                           

                            ProductAvialableOnStock AV = db.ProductAvialableOnStock.Find(item.TempProductStockID);
                            AV.ProductAvaliable -= items.Product.Quantity;
                            db.Entry(AV).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        db.ProductInformation.Remove(productInformation);
                        db.SaveChanges();
                        TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to register stock information";
                        return RedirectToAction("NewProductEntry");
                    }
                }
                TempData[User.Identity.GetUserId() + "succsessMessage"] = "Product information Saved";
                return RedirectToAction("NewProductEntry");

            }
            else
            {
                ViewBag.errorMessage = "Unable to lode Stock Information";
            }
            TempData[User.Identity.GetUserId() + "SelectedList"] = TempData[User.Identity.GetUserId() + "SelectedList"];
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name");
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
