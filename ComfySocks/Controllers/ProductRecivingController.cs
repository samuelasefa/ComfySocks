using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.Order;
using ComfySocks.Models.ProductInfo;
using ComfySocks.Models.ProductTransferInfo;
using ComfySocks.Models.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    [Authorize(Roles = "Super Admin, Production, Store Manager")]
    public class ProductRecivingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ProductTransfer
        public ActionResult ProductRecivingList()
        {
            //errormessage display
            if (TempData[User.Identity.GetUserId()+"errorMessage"]!= null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            var productRecivingList = (from recivie in db.ProductInformation orderby recivie.ProductNumber ascending select recivie).ToList();

            return View(productRecivingList);
        }

        // controller of Finshed product transfer
        public ActionResult NewRecivingEntry()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            ViewBag.ProductID = (from p in db.Items where p.StoreType == StoreType.ProductItem orderby p.ID select p).ToList();
            return View();
        }

        //post method  for transfering product
        [System.Web.Mvc.HttpPost]
        public ActionResult NewRecivingEntry(Product product)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<ProductViewModel> selectedProduct = new List<ProductViewModel>();
            bool found = false;
            if (TempData[User.Identity.GetUserId()+ "selectedProduct"] != null)
            {
                selectedProduct = (List<ProductViewModel>)TempData[User.Identity.GetUserId() + "selectedProduct"];     
            }
            product.ProductInformationID = 1;

               var item = db.Products.Find(product.ItemID);

            if (ModelState.IsValid)
            {
                foreach (ProductViewModel p in selectedProduct)
                {
                    if (p.Product.ItemID == product.ItemID)
                    {
                        p.Product.Quantity += product.Quantity;
                        found = true;
                        ViewBag.infoMessage = "Product Reciving Item is Added!";
                        break;
                    }
                }
                if (!found)
                {
                    Item I = db.Items.Find(product.ItemID);

                    if (I == null)
                    {
                        ViewBag.errorMessage = "Unable to find Item";
                    }
                    else {
                        ProductViewModel productViewModel = new ProductViewModel();
                        productViewModel.TypeOfProduct = I.Name;
                        productViewModel.Code = I.Code;
                        productViewModel.Unit = I.Unit.Name;
                        productViewModel.Product = product;
                        selectedProduct.Add(productViewModel);
                    }
                }
            }
            else
            {
                ViewBag.errorMessage = "State is not valid";
            }
            ViewBag.selectedProduct = selectedProduct;
            TempData[User.Identity.GetUserId() + "selectedProduct"] = selectedProduct;
            if (selectedProduct.Count() > 0 ) {
                ViewBag.haveItem = true;
            }
            ViewBag.ProductID = (from p in db.Items where p.StoreType == StoreType.ProductItem orderby p.ID select p).ToList();

            return View(product);
        }

        public ActionResult Remove(int id = 0)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            ViewBag.ProductID = (from p in db.Items where p.StoreType == StoreType.ProductItem orderby p.ID select p).ToList();
            
            List<ProductViewModel> selectedProduct = new List<ProductViewModel>();
            if (TempData[User.Identity.GetUserId() + "selectedProduct"] != null)
            {
                try
                {
                    selectedProduct = (List<ProductViewModel>)TempData[User.Identity.GetUserId() + "selectedProduct"];
                    foreach (ProductViewModel items in selectedProduct)
                    {  
                        if (items.Product.ItemID == id)
                        {
                            selectedProduct.Remove(items);
                            if (selectedProduct.Count() > 0)
                            {
                                TempData[User.Identity.GetUserId() + "selectedProduct"] = selectedProduct;
                                ViewBag.selectedProduct = selectedProduct;
                            }
                            return View("NewRecivingEntry");
                        }
                    }

                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = e;
                }

            }
            ViewBag.errorMesage = "You process can't be performed for now try again";
            return View("NewRecivingEntry");
        }

        public ActionResult NewRecivingInfo()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            List<ProductViewModel> selectedProduct = new List<ProductViewModel>();

            if (TempData[User.Identity.GetUserId() + "selectedProduct"] != null)
            {
                selectedProduct = (List<ProductViewModel>)TempData[User.Identity.GetUserId() + "selectedProduct"];
                TempData[User.Identity.GetUserId() + "selectedProduct"] = selectedProduct;
            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected Product";
                return RedirectToAction("NewRecivingEntry");
            }
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            return View();
        }


        [HttpPost]
        public ActionResult NewRecivingInfo(ProductInformation productInformation)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            List<ProductViewModel> selectedProduct = new List<ProductViewModel>();
            if (TempData[User.Identity.GetUserId() + "selectedProduct"] != null)
            {
                selectedProduct = (List<ProductViewModel>)TempData[User.Identity.GetUserId() + "selectedProduct"];
            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected Store Product";
                return RedirectToAction("NewRecivingEntry");
            }
            productInformation.ApplicationUserID = User.Identity.GetUserId();
            try
            {
                int LastId = (from s in db.ProductInformation orderby s.ID ascending select s.ID).First();
                productInformation.ProductNumber = "No:-" + (LastId + 1).ToString("D6");
            }
            catch
            {
                productInformation.ProductNumber = "No:-" +1.ToString("D6");
            }
            productInformation.Date = DateTime.Now;
            bool pass = true;
            if (ModelState.IsValid)
            {
                try
                {
                    productInformation.Status = "Submmited";
                    db.ProductInformation.Add(productInformation);
                    db.SaveChanges();
                    pass = true;
                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = "Unable to perform the request you need! view error detail" + e;
                    pass = false;
                }
                if (pass)
                {
                    try
                    {
                        foreach (ProductViewModel s in selectedProduct)
                        {
                            s.Product.ProductInformationID = productInformation.ID;
                            db.Products.Add(s.Product);
                            db.SaveChanges();
                        }
                        ViewBag.succsessMessage = "Product Reciving item is created!.";
                        return RedirectToAction("ProductRecivingDetail", new { id = productInformation.ID });
                    }
                    catch (Exception e)
                    {
                        ViewBag.errorMessage = "Unable to perform the request you need!view error detail" + e;
                    }
                }
            }
            return View();
        }
        public ActionResult ProductRecivingDetail(int? id)
        {
            //errormessage display
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }

            if (id == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected!";
                return RedirectToAction("ProductRecivingList");
            }
            ProductInformation product = db.ProductInformation.Find(id);

            if (product == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected!";
                return RedirectToAction("ProductRecivingList");
            }
            return View(product);
        }
        //Transfer approval
        public ActionResult RecivingApproved(int? id)
        {

            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("TransferDetail");
            }

            TransferInformation transferInformation = db.TransferInformation.Find(id);
            if(transferInformation == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Find Product Transfer Information";
                return RedirectToAction("TransferDetail");
            }
            List<TransferViewModelForError> ErrorList = new List<TransferViewModelForError>();
            bool pass = true;

            foreach (Transfer transfer in transferInformation.Transfers) {
                Item items = (from t in db.Items where t.ID == transfer.ItemID select t).First();
                if (items == null)
                {
                    TransferViewModelForError transferViewModelForError = new TransferViewModelForError()
                    {
                        Transfer = transfer,
                        Error = "Unable to load Product Information Posible reason is no information registerd about the item"
                    }; pass = false;
                    ViewBag.errorMessage = "Some error found. see error details for more information";
                    ErrorList.Add(transferViewModelForError);
                }
            }
            if (pass)
            {
                foreach (Transfer transfer in transferInformation.Transfers)
                {
                    Item items= (from t in db.Items where t.ID == transfer.ItemID select t).First();
                    transfer.Total += (float)transfer.Quantity;
                    transfer.PPT += (float)transfer.Quantity;
                    db.Entry(transfer).State = EntityState.Modified;
                    db.SaveChanges();
                    ProductlogicalAvaliable productlogical = db.ProductlogicalAvaliables.Find(transfer.ItemID);
                    ProductMaterialRepository productMaterial = db.ProductMaterialRepositories.Find(transfer.ItemID);
                    if (productMaterial == null && productlogical == null)
                    {
                        productMaterial = new ProductMaterialRepository()
                        {
                            ID = transfer.ItemID,
                            ProductMaterialAavliable = transfer.Quantity
                        };
                        productlogical = new ProductlogicalAvaliable()
                        {
                            ID = transfer.ItemID,
                            LogicalProductAvaliable = transfer.Quantity
                        };
                        db.ProductMaterialRepositories.Add(productMaterial);
                        db.ProductlogicalAvaliables.Add(productlogical);
                        db.SaveChanges();
                    }
                    else {
                        productMaterial.ProductMaterialAavliable += transfer.Quantity;
                        productlogical.LogicalProductAvaliable += transfer.Quantity;
                        db.Entry(productMaterial).State = EntityState.Modified;
                        db.Entry(productlogical).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                transferInformation.Status = "Transferd";
                transferInformation.Approvedby = User.Identity.GetUserName();
                db.Entry(transferInformation).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.successMessage = "Successfully transferd!!";
            }
            else {
                ViewBag.errorList = ErrorList;
            }

            return View("TransferDetail", transferInformation);
        }

        //Transfer rejection
        public ActionResult TransferRejected(int? id)
        {

            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("TransferDetail");
            }

            TransferInformation transferInformation = db.TransferInformation.Find(id);
            if (transferInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Find Product Transfer Information";
                return RedirectToAction("TransferDetail");
            }
            List<TransferViewModelForError> ErrorList = new List<TransferViewModelForError>();
            bool pass = true;

            foreach (Transfer transfer in transferInformation.Transfers)
            {
                if (pass)
                {
                    transferInformation.Status = "Rejected";
                    transferInformation.Approvedby = User.Identity.GetUserName();
                    db.Entry(transferInformation).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.successMessage = "Successfully Rejected!!";
                }
                else
                {
                    ViewBag.errorList = ErrorList;
                }
            }
            return View("TransferDetail", transferInformation);
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