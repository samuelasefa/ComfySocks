using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.OfficeDeliveryInfo;
using ComfySocks.Models.OfficeRequest;
using ComfySocks.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    [Authorize]
    public class OfficeMaterialDeliveryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: RowMaterialDelivery

        public ActionResult OfficeMaterialDeliveryList(int id)
        {
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = null;
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            ViewBag.ID = id;

            TempData[User.Identity.GetUserId() + "OfficeRequestID"] = id;
            var OfficeMaterialDelivery = (from d in db.OfficeDeliveryInformation where d.OfficeMaterialRequestInformationID == id orderby d.Date descending select d).ToList();

            return View(OfficeMaterialDelivery);
        }


        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult NewOfficeMaterialDelivery(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected! Try again";
                return RedirectToAction("OfficeMaterialDeliveryList");
            }
            OfficeMaterialRequest officeMaterialRequest = db.OfficeMaterialRequest.Find(id);
            if (officeMaterialRequest == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected! Try again";
                return RedirectToAction("OfficeMaterialDeliveryList");
            }
            ViewBag.OfficeMaterialRequestID = (from d in db.OfficeMaterialRequest where d.OfficeMaterialRequestInformationID == id select d).ToList();
            ViewBag.id = id;
            TempData[User.Identity.GetUserId() + "OfficeRequestInfoID"] = id;
            if (id != null)
            {
                List<OfficeDeliveryVM> selectedDelivery = new List<OfficeDeliveryVM>();
                selectedDelivery = (List<OfficeDeliveryVM>)TempData[User.Identity.GetUserId() + "selectedDelivery"];
                TempData[User.Identity.GetUserId() + "selectedDelivery"] = selectedDelivery;
                ViewBag.selectedDelivery = selectedDelivery;
            }
            else {
                TempData[User.Identity.GetUserId() + "selectedDelivery"] = null;
            }
            return View();

            
        }

        [Authorize(Roles = "Super Admin, Admin,Store Manager")]
        [HttpPost]
        public ActionResult NewOfficeMaterialDelivery(OfficeDelivery officeDelivery)
        {
            if (TempData[User.Identity.GetUserId() + "OfficeRequestInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out! Try again";
                TempData[User.Identity.GetUserId() + "selectedDelivery"] = null;
                return RedirectToAction("OfficeMaterialDeliveryList", "OfficeMaterialDelivery", null);
            }
            int id = (int)TempData[User.Identity.GetUserId() + "OfficeRequestInfoID"];
            TempData[User.Identity.GetUserId() + "OfficeRequestInfoID"] = id;
            ViewBag.id = id;
            bool found = false;
            List<OfficeDeliveryVM> selectedDelivery = new List<OfficeDeliveryVM>();
            if (TempData[User.Identity.GetUserId() + "selectedDelivery"] != null)
            {
                selectedDelivery = (List<OfficeDeliveryVM>)TempData[User.Identity.GetUserId() + "selectedDelivery"];
            }
            OfficeMaterialRequest omr = db.OfficeMaterialRequest.Find(officeDelivery.OfficeMaterialRequestID);

            if (ModelState.IsValid)
            {
                if (omr == null)
                {
                    TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to lode Request information! Try again";
                    TempData[User.Identity.GetUserId() + "errorMessage"] = "RowMaterial ID is = " + officeDelivery.OfficeMaterialRequestID;
                    TempData[User.Identity.GetUserId() + "selectedDelivery"] = null;
                    return RedirectToAction("OfficeMaterialDeliveryList");
                }

                foreach (OfficeDeliveryVM d in selectedDelivery)
                {
                    if (d.OfficeDelivery.OfficeMaterialRequestID == officeDelivery.OfficeMaterialRequestID)
                    {

                        if (d.OfficeDelivery.Quantity + officeDelivery.Quantity > omr.RemaningDelivery)
                        {
                            ViewBag.errorMessage = "Only " + omr.RemaningDelivery + "items remain, You can’t deliver " + officeDelivery.Quantity + "items";
                        }
                        else
                        {
                            d.OfficeDelivery.Quantity += officeDelivery.Quantity;
                            d.Remaining -= officeDelivery.Quantity;
                        }
                        found = true;
                    }
                }
                if (!found)
                {
                    if (omr.RemaningDelivery < officeDelivery.Quantity)
                    {
                        ViewBag.errorMessage = "Only " + omr.RemaningDelivery + " Items remain, You can’t add" + officeDelivery.Quantity + " item for delivery";

                    }
                    else
                    {
                        OfficeDeliveryVM deliveryVM = new OfficeDeliveryVM();
                        Item I = db.Items.Find(omr.ItemID);
                        deliveryVM.ItemCode = I.Code;
                        deliveryVM.ItemDescription = I.Name;
                        deliveryVM.Unit = omr.Item.Unit.Name;
                        deliveryVM.OfficeDelivery = officeDelivery;
                        deliveryVM.Remaining = omr.RemaningDelivery - officeDelivery.Quantity;
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
            ViewBag.OfficeMaterialRequestID = (from d in db.OfficeMaterialRequest where d.OfficeMaterialRequestInformationID == id select d).ToList();
            return View(officeDelivery);
        }

        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult Remove(int id)
        {
            if (TempData[User.Identity.GetUserId() + "selectedDelivery"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find selected Request. try again.";
                return RedirectToAction("NewOfficeMaterialDelivery");
            }
            List<OfficeDeliveryVM> selectedDelivery = new List<OfficeDeliveryVM>();
            selectedDelivery = (List<OfficeDeliveryVM>)TempData[User.Identity.GetUserId() + "selectedDelivery"];
            foreach (var s in selectedDelivery)
            {
                if (s.OfficeDelivery.OfficeMaterialRequestID == id)
                {
                    selectedDelivery.Remove(s);
                    ViewBag.succsessMessage = "Office Delivery successfully Removed";
                    break;
                }
            }
            if (selectedDelivery.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.selectedDelivery = selectedDelivery;
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = selectedDelivery;
            ViewBag.OfficeMaterialRequestID = (from d in db.OfficeMaterialRequest where d.OfficeMaterialRequestInformationID == id select d).ToList();
            return View("NewOfficeMaterialDelivery");
        }

        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult NewOfficeMaterilDeliveryInfo()
        {
            if (TempData[User.Identity.GetUserId() + "selectedDelivery"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract delivery information!.";
                return RedirectToAction("RowMaterialDeliveryList");
            }
            if (TempData[User.Identity.GetUserId() + "OfficeRequestInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out!. Try again ";
                return RedirectToAction("RowMaterialDeliveryList", "storerequestinformtion", null);
            }
            int requestId = (int)TempData[User.Identity.GetUserId() + "OfficeRequestInfoID"];
            TempData[User.Identity.GetUserId() + "OfficeRequestInfoID"] = requestId;
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = TempData[User.Identity.GetUserId() + "selectedDelivery"];
            return View();
        }

        [System.Web.Mvc.HttpPost]
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult NewOfficeMaterilDeliveryInfo(OfficeDeliveryInformation officeDeliveryInformation)
        {
            if (TempData[User.Identity.GetUserId() + "selectedDelivery"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract delivery information!.";
                return RedirectToAction("index", "Request", null);
            }
            if (TempData[User.Identity.GetUserId() + "OfficeRequestInfoID"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out!. Try again";
                return RedirectToAction("OfficeMaterialDeliveryList", "storerequestinformation", null);
            }
            int RequestId = (int)TempData[User.Identity.GetUserId() + "OfficeRequestInfoID"];
            TempData[User.Identity.GetUserId() + "OfficeRequestInfoID"] = RequestId;
            List<OfficeDeliveryVM> deliverylist = (List<OfficeDeliveryVM>)TempData[User.Identity.GetUserId() + "selectedDelivery"];
            TempData[User.Identity.GetUserId() + "selectedDelivery"] = deliverylist;


            if (officeDeliveryInformation.Date > DateTime.Now)
            {
                ViewBag.errorMessage = "The date information you provide is not valid!.";
                return View(officeDeliveryInformation);
            }
            OfficeDeliveryVM d = deliverylist.FirstOrDefault();
            officeDeliveryInformation.ApplictionUserID = User.Identity.GetUserId();

            try
            {
                OfficeDeliveryInformation lastDeliverd = (from del in db.OfficeDeliveryInformation orderby del.ID descending select del).First();

                officeDeliveryInformation.OfficeMaterialRequestInformationID = RequestId;
                officeDeliveryInformation.OfficeDeliveryNumber = "NO-" + lastDeliverd.ID.ToString("D5");
            }
            catch
            {
                officeDeliveryInformation.OfficeMaterialRequestInformationID = RequestId;
                officeDeliveryInformation.OfficeDeliveryNumber = "No-" + 0.ToString("D5");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.OfficeDeliveryInformation.Add(officeDeliveryInformation);
                    db.SaveChanges();

                    foreach (OfficeDeliveryVM de in deliverylist)
                    {
                        de.OfficeDelivery.OfficeDeliveryInformationID = officeDeliveryInformation.ID;
                        db.OfficeDeliveries.Add(de.OfficeDelivery);
                        db.SaveChanges();
                        OfficeMaterialRequest omr = db.OfficeMaterialRequest.Find(de.OfficeDelivery.OfficeMaterialRequestID);
                        omr.RemaningDelivery = de.Remaining;
                        db.Entry(omr).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    foreach (OfficeDelivery officeDelivery in officeDeliveryInformation.OfficeDeliveries)
                    {
                        Stock stock = (from sk in db.Stocks where sk.ItemID == officeDelivery.OfficeMaterialRequest.ItemID select sk).First();
                        db.Entry(stock).State = EntityState.Modified;
                        db.SaveChanges();
                        RowMaterialRepositery rowMaterialRepositery = db.RowMaterialRepositeries.Find(officeDelivery.OfficeMaterialRequest.ItemID);
                        Item i = db.Items.Find(rowMaterialRepositery.ID);
                        var deference = rowMaterialRepositery.RecentlyReducedRowMaterialAvaliable - officeDelivery.Quantity;

                        if (deference > 0)
                        {
                            rowMaterialRepositery.RecentlyReducedRowMaterialAvaliable -= officeDelivery.Quantity;
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

                    return RedirectToAction("OfficeMaterialDeliveryList", "OfficeMaterialDelivery", new { id = officeDeliveryInformation.ID });

                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = e;

                }
            }

            return View(officeDeliveryInformation);
        }

        [Authorize(Roles ="Store Manager, Super Admin")]
        public ActionResult DropDelivery(int id = 0)
        {
            int OfficeRequestID = (int)TempData[User.Identity.GetUserId() + "OfficeRequestID"];
            if (id == 0)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected";
                return RedirectToAction("OfficeMaterialDeliveryList", new { id = OfficeRequestID });
            }

            OfficeDeliveryInformation deliveryInfo = db.OfficeDeliveryInformation.Find(id);
            if (deliveryInfo == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find delivery information !";
                return RedirectToAction("OfficeMaterialDeliveryList", new { id = OfficeRequestID });
            }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<OfficeDelivery> RedusedDelivery = new List<OfficeDelivery>();
            bool pass = true;
            foreach (OfficeDelivery d in deliveryInfo.OfficeDeliveries)
            {
                OfficeMaterialRequest storeRequest = db.OfficeMaterialRequest.Find(d.OfficeMaterialRequestID);
                if (storeRequest == null)
                {
                    pass = false;
                    break;
                }
                storeRequest.RemaningDelivery += d.Quantity;

                db.Entry(storeRequest).State = EntityState.Modified;
                db.SaveChanges();
                RedusedDelivery.Add(d);

            }
            if (!pass)
            {
                foreach (OfficeDelivery d in RedusedDelivery)
                {
                    OfficeMaterialRequest storeRequest = db.OfficeMaterialRequest.Find(d.OfficeMaterialRequestID);
                    if (storeRequest == null)
                    {
                        pass = false;
                        break;
                    }
                    storeRequest.RemaningDelivery -= d.Quantity;
                    db.Entry(storeRequest).State = EntityState.Modified;
                    db.SaveChanges();
                    RedusedDelivery.Add(d);

                }
                TempData[User.Identity.GetUserId() + "succsessMessage"] = "Delivery information dropped !";

                return RedirectToAction("OfficeMaterialDeliveryList", new { id = OfficeRequestID });
            }
            else
            {
                deliveryInfo.Status = "Rejected Delivery";
                db.Entry(deliveryInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("OfficeMaterialDeliveryList", new { id = OfficeRequestID });
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