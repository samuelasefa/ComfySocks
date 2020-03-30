using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.Order;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Order
        public ActionResult OrderHistory()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var productionOrderInfos = db.ProductionOrderInfos.Include(c=> c.Customer);

            return View(productionOrderInfos.ToList());
        }
        [Authorize(Roles = "Admin, Super Admin")]
        public ActionResult NewOrderEntry(int id = 0)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            //required item before order can preformed

            ViewBag.productCode = "";
            ViewBag.customre = "";
            var customer = db.Customers.ToList();
            var productCode = db.Items.ToList();
            {
                if (productCode.Count() == 0)
                {
                    ViewBag.productCode = "Register Product Code";
                    ViewBag.RequiredItems = true;
                }
                if (customer.Count() == 0)
                {
                    ViewBag.customer = "Register Customer information";
                    ViewBag.RequiredItems = true;
                }
            }

            if (id != 0)
            {

                List<ProductionOrderVM> productionOrders = new List<ProductionOrderVM>();
               
                    productionOrders = (List<ProductionOrderVM>)TempData[User.Identity.GetUserId() + "ProductionOrders"];
                    TempData[User.Identity.GetUserId() + "ProductionOrders"] = null;
                
                ViewBag.productionOrders = productionOrders.ToList();
            }
            else
            {
                TempData[User.Identity.GetUserId() + "ProductionOrders"] = null;
            }
            if(db.Customers.ToList().Count == 0)
            {
                ViewBag.Customer = "Register Customer Information Frist!!,";
            }
            ViewBag.ItemID = new SelectList((from i in db.Items where i.StoreType == Models.Items.StoreType.ProductItem orderby i.Code select i), "ID", "Code");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Super Admin")]
        public ActionResult NewOrderEntry(ProductionOrder productionOrder)
        {
            //This is to display error message
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }

            List<ProductionOrderVM> productionOrders = new List<ProductionOrderVM>();
            bool found = false;
            if (TempData[User.Identity.GetUserId() + "ProductionOrders"] != null)
            {
                productionOrders = (List<ProductionOrderVM>)TempData[User.Identity.GetUserId() + "ProductionOrders"];
            }
            productionOrder.ProductionOrderInfoID = 1;
            productionOrder.deliverd = false;
            productionOrder.RemaningDelivery = (float)productionOrder.Quantity;
            if (productionOrder.Date <= DateTime.Now){
                ViewBag.errorMessage = "Production Order Must be Greater than current Date";
                return RedirectToAction("NewOrderEntry");
            }
            if (ModelState.IsValid)
            {
                foreach (ProductionOrderVM o in productionOrders)
                {
                    if (o.ProductionOrder.ItemID == productionOrder.ItemID)
                    {
                        o.ProductionOrder.Quantity += productionOrder.Quantity;
                        found = true;
                        ViewBag.successMessage = "Added";
                        break;
                    }
                }

                if (found == false)
                {
                    ProductionOrderVM orderVM = new ProductionOrderVM();
                    Item it = db.Items.Find(productionOrder.ItemID);
                    if (it == null && productionOrder.Quantity == 0 && productionOrder.Date == null)
                    {
                        ViewBag.errorMessage = "Unable to load selected information";
                    }
                    else
                    {
                        orderVM.ProductCode = it.Code;
                        orderVM.ProductionOrder = productionOrder;
                        productionOrders.Add(orderVM);
                    }
                }
            }
            else
            {
                ViewBag.errorMessage = "State is not vaild";
            }
            TempData[User.Identity.GetUserId() + "ProductionOrders"] = productionOrders;
            ViewBag.productionOrders = productionOrders.ToList();
            if (productionOrders.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.ItemID = new SelectList((from i in db.Items where i.StoreType == Models.Items.StoreType.ProductItem orderby i.Code select i), "ID", "Code");
            return View();
        }

        //removing item from Production List Item
        [Authorize(Roles = "Admin, Super Admin")]
        public ActionResult RemoveSelected(int id)
        {
            if (TempData[User.Identity.GetUserId() + "ProductionOrders"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find selected orders. try again.";
                return RedirectToAction("NewOrderEntry");
            }
            List<ProductionOrderVM> productionOrders = new List<ProductionOrderVM>();
            productionOrders = (List<ProductionOrderVM>)TempData[User.Identity.GetUserId() + "ProductionOrders"];
            foreach (ProductionOrderVM p in productionOrders)
            {
                if (p.ProductionOrder.ItemID == id)
                {
                    productionOrders.Remove(p);
                    ViewBag.succsessMessage = "Production Order is Successfully Removed";
                    break;
                }
            }
            if (productionOrders.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.productionOrders = productionOrders;
            TempData[User.Identity.GetUserId() + "ProductionOrder"] = productionOrders;
            ViewBag.ItemID = new SelectList((from i in db.Items where i.StoreType == Models.Items.StoreType.ProductItem orderby i.Code select i), "ID", "Code");
            return View("NewOrderEntry");
        }
        [Authorize(Roles = "Admin, Super Admin")]
        public ActionResult NewOrderInfo()
        {
            if (TempData[User.Identity.GetUserId() + "ProductionOrders"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find selected Production Orders.try again";
                return RedirectToAction("NewOrderEntry");
            }
            TempData[User.Identity.GetUserId() + "ProductionOrders"] = TempData[User.Identity.GetUserId() + "ProductionOrders"];

            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName");
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Super Admin")]

        public ActionResult NewOrderInfo(ProductionOrderInfo productionOrderInfo)
        {
            if (TempData[User.Identity.GetUserId() + "ProductionOrders"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Information about production order please try agian";
                return RedirectToAction("NewOrderEntry");
            }

            List<ProductionOrderVM> productionOrders = new List<ProductionOrderVM>();
            productionOrders = (List<ProductionOrderVM>)TempData[User.Identity.GetUserId() + "ProductionOrders"];

            TempData[User.Identity.GetUserId() + "ProductionOrders"] = productionOrders;
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName", productionOrderInfo.CustomerID);
            productionOrderInfo.ApplicationUserID = User.Identity.GetUserId();
            productionOrderInfo.Date = DateTime.Now;
            productionOrderInfo.Status = "Submmited";
            //if item state is valid

            if (ModelState.IsValid)
            {
                try
                {
                    db.ProductionOrderInfos.Add(productionOrderInfo);
                    db.SaveChanges();
                    TempData[User.Identity.GetUserId() + "succsessMessage"] = "Production Order Info is  Succesfully add to database";

                    foreach (ProductionOrderVM p in productionOrders)
                    {
                        p.ProductionOrder.ProductionOrderInfoID = productionOrderInfo.ID;
                        p.ProductionOrder.RemaningDelivery = (float)p.ProductionOrder.Quantity;
                        db.ProductionOrders.Add(p.ProductionOrder);
                        db.SaveChanges();
                        TempData[User.Identity.GetUserId() + "succsessMessage"] = "Production Order is saved";  
                    }

                    ProductionOrderInfo po = db.ProductionOrderInfos.Find(productionOrderInfo.ID);
                    po.OrderNumber = po.ID.ToString("D4");
                    db.Entry(po).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData[User.Identity.GetUserId() + "succsessMessage"] = " Production Order Created!";
                    return RedirectToAction("OrderDetial", new { id = productionOrderInfo.ID });
                }
                catch
                {

                }
                
            }
            return View(productionOrderInfo);

        }


        //Detail
        public ActionResult OrderDetial(int id = 0)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (id == 0)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("OrderHistory");
            }

            ProductionOrderInfo POI = db.ProductionOrderInfos.Find(id);
            if (POI == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected!!";
                return RedirectToAction("OrderHistory");
            }
            return View(POI);
        }
        // Production order Approval
        [Authorize(Roles = "Admin, Super Admin")]
        public ActionResult OrderApproved(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("OrderHistory");
            }

            ProductionOrderInfo productionOrderInfo = db.ProductionOrderInfos.Find(id);

            if (productionOrderInfo == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Temporary product Information";
                return RedirectToAction("TemporaryProductList");
            }

            List<ProductionOrderVMForError> ErrorList = new List<ProductionOrderVMForError>();
            bool pass = true;
            foreach (ProductionOrder productionOrder in productionOrderInfo.ProductionOrders)
            {
                if (productionOrder == null)
                {
                    ProductionOrderVMForError productionOrderVMForError = new ProductionOrderVMForError()
                    {
                        ProductionOrder = productionOrder,
                        Error = "Unable to load Production Order"
                    }; pass = false;

                }
            }
            if (pass)
            {
                foreach (ProductionOrder productionOrder in productionOrderInfo.ProductionOrders)
                {
                    productionOrderInfo.Status = "Approved";
                    db.Entry(productionOrderInfo).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.succsessMessage = "Succesfully Approved";
                }

            }
            else
            {
                ViewBag.errorList = ErrorList;
            }
            return View("OrderDetial", productionOrderInfo);
        }

        //Production order Rejection
        [Authorize(Roles = "Admin, Super Admin")]
        public ActionResult OrderRejected(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("OrderHistory");
            }

            ProductionOrderInfo productionOrderInfo = db.ProductionOrderInfos.Find(id);

            if (productionOrderInfo == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Temporary product Information";
                return RedirectToAction("TemporaryProductList");
            }

            List<ProductionOrderVMForError> ErrorList = new List<ProductionOrderVMForError>();
            bool pass = true;
            foreach (ProductionOrder productionOrder in productionOrderInfo.ProductionOrders)
            {
                if (productionOrder == null)
                {
                    ProductionOrderVMForError productionOrderVMForError = new ProductionOrderVMForError()
                    {
                        ProductionOrder = productionOrder,
                        Error = "Unable to load Production Order"
                    }; pass = false;

                }
            }
            if (pass)
            {
                foreach (ProductionOrder productionOrder in productionOrderInfo.ProductionOrders)
                {
                    productionOrderInfo.Status = "Rejected";
                    db.Entry(productionOrderInfo).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.succsessMessage = "Succesfully Rejected";
                }

            }
            else
            {
                ViewBag.errorList = ErrorList;
            }
            return View("OrderDetial", productionOrderInfo);
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