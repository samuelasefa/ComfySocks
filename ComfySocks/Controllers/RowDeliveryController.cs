using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.Request;
using ComfySocks.Models.RowDeliveryInfo;
using ComfySocks.Models.SalesInfo;
using ComfySocks.Repository;
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
    public class RowDeliveryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: RowMaterialDelivery

        public ActionResult RowDeliveryList(int id)
        {
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = null;
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            ViewBag.ID = id;

            TempData[User.Identity.GetUserId() + "RequestID"] = id;
            var RowMaterialDelivery = (from d in db.RowDeliveryInformation where d.StoreRequestInformationID == id orderby d.ID descending select d).ToList();

            return View(RowMaterialDelivery);
        }


        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult NewRowDelivery(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected! Try again";
                return RedirectToAction("RowDeliveryList");
            }
            StoreRequestInformation storeRequestInformation = db.StoreRequestInformation.Find(id);
            if (storeRequestInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected! Try again";
                return RedirectToAction("RowDeliveryList");
            }
            ViewBag.StoreRequestID = (from d in db.StoreRequest where d.StoreRequestInformationID == id select d).ToList();
            ViewBag.id = id;
            TempData[User.Identity.GetUserId() + "RequestInfoID"] = id;
            return View();
        }

        [Authorize(Roles = "Super Admin, Admin,Store Manager")]
        [HttpPost]
        public ActionResult NewRowDelivery(RowDelivery rowDelivery)
        {
            if (TempData[User.Identity.GetUserId() + "RequestInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out! Try again";
                TempData[User.Identity.GetUserId() + "selectedDelivery"] = null;
                return RedirectToAction("RowDeliveryList", "Request", null);
            }
            int id = (int)TempData[User.Identity.GetUserId() + "RequestInfoID"];
            TempData[User.Identity.GetUserId() + "RequestInfoID"] = id;
            ViewBag.id = id;
            bool found = false;
            List<RowDeliveryVM> selectedDelivery = new List<RowDeliveryVM>();
            if (TempData[User.Identity.GetUserId() + "selectedDelivery"] != null)
            {
                selectedDelivery = (List<RowDeliveryVM>)TempData[User.Identity.GetUserId() + "selectedDelivery"];
            }
            StoreRequest sr = db.StoreRequest.Find(rowDelivery.StoreRequestID);

            if (ModelState.IsValid)
            {
                if (sr == null)
                {
                    TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to lode Request information! Try again";
                    TempData[User.Identity.GetUserId()+"errorMessage"]="RowMaterial ID is = "+ rowDelivery.StoreRequestID;
                    TempData[User.Identity.GetUserId() + "selectedDelivery"] = null;
                    return RedirectToAction("RowDeliveryList", new { id = id });
                }

                foreach (RowDeliveryVM d in selectedDelivery)
                {
                    if (d.RowDelivery.StoreRequestID == rowDelivery.StoreRequestID)
                    {

                        if (d.RowDelivery.DeliveryQuantity + rowDelivery.DeliveryQuantity > sr.RemaningDelivery)
                        {
                            ViewBag.errorMessage = "Only " + sr.RemaningDelivery + "items remain, You can’t deliver " + rowDelivery.DeliveryQuantity + "items";
                        }
                        else
                        {
                            d.RowDelivery.DeliveryQuantity += rowDelivery.DeliveryQuantity;
                            d.Remaining -= (float)rowDelivery.DeliveryQuantity;
                        }
                        found = true;
                    }
                }
                if (!found)
                {
                    if (sr.RemaningDelivery < rowDelivery.DeliveryQuantity)
                    {
                        ViewBag.errorMessage = "Only " + sr.RemaningDelivery + " Items remain, You can’t add" + rowDelivery.DeliveryQuantity + " item for delivery";

                    }
                    else
                    {
                        RowDeliveryVM deliveryVM = new RowDeliveryVM();
                        Item I = db.Items.Find(sr.ItemID);
                        deliveryVM.Code = I.Code;
                        deliveryVM.ItemDescription = I.Name;
                        deliveryVM.Unit = sr.Item.Unit.Name;
                        deliveryVM.RowDelivery = rowDelivery;
                        deliveryVM.Remaining = sr.RemaningDelivery - (float)rowDelivery.DeliveryQuantity;
                        selectedDelivery.Add(deliveryVM);
                    }
                }
            }
            ViewBag.selectedDelivery = selectedDelivery;
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = selectedDelivery;
            if (selectedDelivery.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.StoreRequestID = (from d in db.StoreRequest where d.StoreRequestInformationID == id select d).ToList();
            return View(rowDelivery);
        }

        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult Remove(int id)
        {
            if (TempData[User.Identity.GetUserId() + "selectedDelivery"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find selected Request. try again.";
                return RedirectToAction("NewRowDelivery");
            }
            List<RowDeliveryVM> selectedDelivery = new List<RowDeliveryVM>();
            selectedDelivery = (List<RowDeliveryVM>)TempData[User.Identity.GetUserId() + "selectedDelivery"];
            foreach (var s in selectedDelivery)
            {
                if (s.RowDelivery.StoreRequestID == id)
                {
                    selectedDelivery.Remove(s);
                    ViewBag.succsessMessage = "Row Delivery successfully Removed";
                    break;
                }
            }
            if (selectedDelivery.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.selectedDelivery = selectedDelivery;
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = selectedDelivery;
            ViewBag.StoreRequestID = (from d in db.StoreRequest where d.StoreRequestInformationID == id select d).ToList();
            return View("NewRowDelivery");
        }

        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult NewRowDeliveryInfo()
        {
            if (TempData[User.Identity.GetUserId() + "selectedDelivery"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract delivery information!.";
                return RedirectToAction("RowMaterialDeliveryList");
            }
            if (TempData[User.Identity.GetUserId() + "RequestInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out!. Try again ";
                return RedirectToAction("RowMaterialDeliveryList", "storerequestinformtion", null);
            }
            int requestId = (int)TempData[User.Identity.GetUserId() + "RequestInfoID"];
            TempData[User.Identity.GetUserId() + "RequestInfoID"] = requestId;
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = TempData[User.Identity.GetUserId() + "selectedDelivery"];
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult NewRowDeliveryInfo(RowDeliveryInformation rowdeliveryInformation)
        {
            if (TempData[User.Identity.GetUserId() + "selectedDelivery"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract delivery information!.";
                return RedirectToAction("index", "Request", null);
            }
            if (TempData[User.Identity.GetUserId() + "RequestInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out!. Try again";
                return RedirectToAction("RowMaterialDeliveryList", "storerequestinformation", null);
            }
            int RequestId = (int)TempData[User.Identity.GetUserId() + "RequestInfoID"];
            TempData[User.Identity.GetUserId() + "RequestInfoID"] = RequestId;
            List<RowDeliveryVM> deliverylist = (List<RowDeliveryVM>)TempData[User.Identity.GetUserId() + "selectedDelivery"];
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = deliverylist;


            if (rowdeliveryInformation.Date > DateTime.Now)
            {
                ViewBag.errorMessage = "The date information you provide is not valid!.";
                return View(rowdeliveryInformation);
            }
            RowDeliveryVM d = deliverylist.FirstOrDefault();
            rowdeliveryInformation.ApplicationUserID = User.Identity.GetUserId();

            try
            {
                RowDeliveryInformation lastDeliverd = (from del in db.RowDeliveryInformation orderby del.ID descending select del).First();

                rowdeliveryInformation.StoreRequestInformationID = RequestId;
                rowdeliveryInformation.StoreIssueNumber = "NO-" + lastDeliverd.ID.ToString("D5");
            }
            catch
            {
                rowdeliveryInformation.StoreRequestInformationID = RequestId;
                rowdeliveryInformation.StoreIssueNumber = "No-"+ 1.ToString("D5");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.RowDeliveryInformation.Add(rowdeliveryInformation);
                    db.SaveChanges();

                    foreach (RowDeliveryVM de in deliverylist)
                    {
                        de.RowDelivery.RowDeliveryInformationID = rowdeliveryInformation.ID;
                        db.RowDeliveries.Add(de.RowDelivery);
                        db.SaveChanges();
                        StoreRequest sr = db.StoreRequest.Find(de.RowDelivery.StoreRequestID);
                        sr.RemaningDelivery = de.Remaining;
                        db.Entry(sr).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    foreach (RowDelivery rowDelivery in rowdeliveryInformation.RowDeliveries)
                    {
                        Stock stock = (from sk in db.Stocks where sk.ItemID == rowDelivery.StoreRequest.ItemID select sk).First();
                        db.Entry(stock).State = EntityState.Modified;
                        db.SaveChanges();
                        RowMaterialRepositery rowMaterialRepositery = db.RowMaterialRepositeries.Find(rowDelivery.StoreRequest.ItemID);
                        Item i = db.Items.Find(rowMaterialRepositery.ID);
                        float deference = rowMaterialRepositery.RecentlyReducedRowMaterialAvaliable - (float)rowDelivery.DeliveryQuantity;

                        if (deference > 0)
                        {
                            rowMaterialRepositery.RecentlyReducedRowMaterialAvaliable -= (float)rowDelivery.DeliveryQuantity;
                        }
                        else
                        {
                            rowMaterialRepositery.RecentlyReducedRowMaterialAvaliable = 0;
                            rowMaterialRepositery.RowMaterialAavliable += deference;

                        }
                        db.Entry(rowMaterialRepositery).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    TempData[User.Identity.GetUserId() + "succsessMessage"] = "Delivery information registered!.";

                    return RedirectToAction("StoreRequestDetial", "Request", new { id = rowdeliveryInformation.StoreRequestInformationID });

                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = e;

                }
            }

            return View(rowdeliveryInformation);
        }

        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult DropDelivery(int id = 0)
        {
            int RequestID = (int)TempData[User.Identity.GetUserId() + "RequestID"];
            if (id == 0)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected";
                return RedirectToAction("RowDeliveryList", new { id = RequestID });
            }

            RowDeliveryInformation deliveryInfo = db.RowDeliveryInformation.Find(id);
            if (deliveryInfo == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find delivery information !";
                return RedirectToAction("RowDeliveryList", new { id = RequestID });
            }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<RowDelivery> RedusedDelivery = new List<RowDelivery>();
            bool pass = true;
            foreach (RowDelivery d in deliveryInfo.RowDeliveries)
            {
                StoreRequest storeRequest = db.StoreRequest.Find(d.StoreRequestID);
                if (storeRequest == null)
                {
                    pass = false;
                    break;
                }
                storeRequest.RemaningDelivery += (float)d.DeliveryQuantity;

                db.Entry(storeRequest).State = EntityState.Modified;
                db.SaveChanges();
                RedusedDelivery.Add(d);

            }
            if (!pass)
            {
                foreach (RowDelivery d in RedusedDelivery)
                {
                    StoreRequest storeRequest = db.StoreRequest.Find(d.StoreRequestID);
                    if (storeRequest == null)
                    {
                        pass = false;
                        break;
                    }
                    storeRequest.RemaningDelivery -= (float)d.DeliveryQuantity;
                    db.Entry(storeRequest).State = EntityState.Modified;
                    db.SaveChanges();
                    RedusedDelivery.Add(d);

                }
                TempData[User.Identity.GetUserId() + "succsessMessage"] = "Delivery information dropped !";

                return RedirectToAction("RowDeliveryList", new { id = RequestID });
            }
            else
            {
                deliveryInfo.Status = "Rejected Delivery";
                db.Entry(deliveryInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RowDeliveryList", new { id = RequestID });
            }


        }

        //Disposing
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