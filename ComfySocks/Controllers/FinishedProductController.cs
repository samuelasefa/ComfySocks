using ComfySocks.Models;
using ComfySocks.Models.Items;
using ComfySocks.Models.ProductRecivingInfo;
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
    public class FinishedProductController : Controller
    {
        // GET: FinishedProduct
        private ApplicationDbContext db = new ApplicationDbContext();
        //
        public ActionResult ProductRecivingList(int id)
        {
            TempData[User.Identity.GetUserId() + "selectedReciving"] = null;
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }


            ViewBag.ID = id;
            TempData[User.Identity.GetUserId() + "ProductID"] = id;
            var product = (from p in db.ProductInformation where p.TransferInformationID == id orderby p.Date descending select p).ToList();
            return View(product);
        }

        public ActionResult NewProductReciving(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation Detected, Please Try agin";
                return RedirectToAction("ProductRecivingList");
            }
            TransferInformation transferInformation = db.TransferInformation.Find(id);

            if (transferInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation Detected, Please Try agin";
                return RedirectToAction("ProductTransferList");
            }
            ViewBag.TransferID = (from t in db.Transfers where t.TransferInformationID == id select t).ToList();
            ViewBag.id = id;
            TempData[User.Identity.GetUserId() + "ProductInfoID"] = id;
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult NewProductReciving(Product product)
        {
            if (TempData[User.Identity.GetUserId() + "ProductInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out! Try again";
                TempData[User.Identity.GetUserId() + "selectedReciving"] = null;
                return RedirectToAction("ProductRecivingList", "FinishedProduct", null);
            }
            int id = (int)TempData[User.Identity.GetUserId() + "ProductInfoID"];
            TempData[User.Identity.GetUserId() + "ProductInfoID"] = id;
            ViewBag.id = id;
            bool found = false;
            List<ProductVM> selectedReciving = new List<ProductVM>();
            if (TempData[User.Identity.GetUserId() + "selectedReciving"] != null)
            {
                selectedReciving = (List<ProductVM>)TempData[User.Identity.GetUserId() + "selectedReciving"];
            }
            if (ModelState.IsValid)
            {
                Transfer transfer = db.Transfers.Find(product.TransferID);
                if (transfer == null)
                {
                    TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to lode order information! Try again";
                    TempData[User.Identity.GetUserId() + "selectedReciving"] = null;
                    return RedirectToAction("ProductRecivingList");
                }

                foreach (ProductVM item in selectedReciving)
                {
                    if (item.Product.TransferID == product.TransferID)
                    {

                        if (item.Product.ProductQuantity + product.ProductQuantity > transfer.RemaningDelivery)
                        {
                            ViewBag.errorMessage = "Only " + transfer.RemaningDelivery + "items remain, You can’t deliver " + product.ProductQuantity + "items";
                        }
                        else
                        {
                            item.Product.ProductQuantity += product.ProductQuantity;
                            item.Remaining -= (float)product.ProductQuantity;
                        }
                        found = true;
                    }
                }
                if (!found)
                {
                    if (transfer.RemaningDelivery < product.ProductQuantity)
                    {
                        ViewBag.errorMessage = "Only " + transfer.RemaningDelivery + " Items remain, You can’t add" + product.ProductQuantity + " item for delivery";

                    }
                    else
                    {
                        ProductVM productVM = new ProductVM();
                        Item I = db.Items.Find(transfer.ItemID);
                        productVM.Code = I.Code;
                        productVM.TypeOfProduct = I.Name;
                        productVM.Unit = I.Unit.Name;
                        productVM.Product = product;
                        productVM.Remaining = transfer.RemaningDelivery - (float)product.ProductQuantity;
                        selectedReciving.Add(productVM);
                    }
                }
            }
            ViewBag.selectedReciving = selectedReciving;
            TempData[User.Identity.GetUserId() + "selectedReciving"] = selectedReciving;
            if (selectedReciving.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.TransferID = (from d in db.Transfers where d.TransferInformationID == id select d).ToList();
            return View(product);
        }
        //Remove
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult Remove(int id = 0)
        {
            if (TempData[User.Identity.GetUserId() + "selectedReciving"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find selected Item to Recive. try again.";
                return RedirectToAction("NewProductReciving");
            }
            List<ProductVM> selectedReciving = new List<ProductVM>();
            selectedReciving = (List<ProductVM>)TempData[User.Identity.GetUserId() + "selectedReciving"];
            foreach (var s in selectedReciving)
            {
                if (s.Product.ID == id)
                {
                    selectedReciving.Remove(s);
                    ViewBag.succsessMessage = "Product is successfully Removed";
                    break;
                }
            }
            if (selectedReciving.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.selectedReciving = selectedReciving;
            TempData[User.Identity.GetUserId() + "selectedReciving"] = selectedReciving;
            ViewBag.TransferID = (from d in db.Transfers where d.TransferInformationID == id select d).ToList();
            return View("NewProductReciving");
        }

        public ActionResult NewProductRecivingInfo()
        {
            if (TempData[User.Identity.GetUserId() + "selectedReciving"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract Reciving information!.";
                return RedirectToAction("");
            }
            if (TempData[User.Identity.GetUserId() + "ProductInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out!. Try again ";
                return RedirectToAction("ProductRecivingList", "FinishedProduct", null);
            }
            int transferId = (int)TempData[User.Identity.GetUserId() + "ProductInfoID"];
            TempData[User.Identity.GetUserId() + "ProductInfoID"] = transferId;
            TempData[User.Identity.GetUserId() + "selectedReciving"] = TempData[User.Identity.GetUserId() + "selectedReciving"];
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult NewProductRecivingInfo(ProductInformation productInformation)
        {
            if (TempData[User.Identity.GetUserId() + "selectedReciving"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract reciving information!.";
                return RedirectToAction("ProductTransferList", "ProductTransfer", null);
            }
            if (TempData[User.Identity.GetUserId() + "ProductInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out!. Try again";
                return RedirectToAction("ProductRecivingList", "FinishedProduct", null);
            }


            int ProductId = (int)TempData[User.Identity.GetUserId() + "ProductInfoID"];
            TempData[User.Identity.GetUserId() + "ProductInfoID"] = ProductId;
            List<ProductVM> deliverylist = (List<ProductVM>)TempData[User.Identity.GetUserId() + "selectedReciving"];
            TempData[User.Identity.GetUserId() + "selectedReciving"] = deliverylist;
            
            ProductVM p = deliverylist.FirstOrDefault();
            productInformation.ApplicationUserID = User.Identity.GetUserId();
            productInformation.Date = DateTime.Now;
            try
            {
                ProductInformation lastDeliverd = (from del in db.ProductInformation orderby del.ID descending select del).First();
                productInformation.TransferInformationID = ProductId;
                productInformation.FPRNumber = "No-" + lastDeliverd.ID.ToString("D6");
            }
            catch
            {
                productInformation.TransferInformationID = ProductId;
                productInformation.FPRNumber = "No-" + 0.ToString("D5");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.ProductInformation.Add(productInformation);
                    db.SaveChanges();

                    foreach (ProductVM de in deliverylist)
                    {
                        de.Product.ProductInformationID = productInformation.ID;
                        db.Products.Add(de.Product);
                        db.SaveChanges();
                        Transfer tr = db.Transfers.Find(de.Product.TransferID);
                        tr.RemaningDelivery = de.Remaining;
                        db.Entry(tr).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    foreach (Product product in productInformation.Products)
                    {
                        Transfer transfer = (from s in db.Transfers where s.ItemID == product.Transfer.ItemID select s).First();
                        //stock.Total -= (float)storeRequest.Quantity;
                        db.Entry(transfer).State = EntityState.Modified;
                        db.SaveChanges();
                        ProductMaterialRepository productMaterial = db.ProductMaterialRepositories.Find(product.Transfer.ItemID);
                        ProductlogicalAvaliable productlogical = db.ProductlogicalAvaliables.Find(product.Transfer.ItemID);
                        if (productMaterial == null && productlogical == null)
                        {
                            productMaterial = new ProductMaterialRepository()
                            {
                                ID = product.Transfer.ItemID,
                                ProductMaterialAavliable = product.ProductQuantity
                            };
                            productlogical = new ProductlogicalAvaliable()
                            {
                                ID = product.Transfer.ItemID,
                                LogicalProductAvaliable = product.ProductQuantity
                            };
                            db.ProductlogicalAvaliables.Add(productlogical);
                            db.ProductMaterialRepositories.Add(productMaterial);
                            db.SaveChanges();
                        }
                        else
                        {
                            productMaterial.ProductMaterialAavliable += product.ProductQuantity;
                            productlogical.LogicalProductAvaliable += product.ProductQuantity;
                            db.Entry(productMaterial).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    TempData[User.Identity.GetUserId() + "succsessMessage"] = "Reciving information registered!.";

                    return RedirectToAction("ProductRecivingList", "FinishedProduct", new { id = productInformation.ID });

                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = e;

                }
            }
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            return View(productInformation);
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