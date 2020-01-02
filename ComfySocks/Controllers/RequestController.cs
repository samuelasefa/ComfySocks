using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.Request;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    public class RequestController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Request
        public ActionResult StoreRequestionList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var storeRequest = (from request in db.StoreRequestInfo orderby request.ID descending select request).ToList();

            return View(storeRequest);
        }
        public ActionResult NewRequestEntry()
        {
            if (TempData[User.Identity.GetUserId() + "successMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null;}
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult NewRequestEntry(StoreRequest storeRequest)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<StoreRequestVM> selectedStoreRequests = new List<StoreRequestVM>();
            bool found = false;

            if (TempData[User.Identity.GetUserId() + "selectedStoreRequests"] != null)
            {
                selectedStoreRequests = (List<StoreRequestVM>)TempData[User.Identity.GetUserId() + "selectedStoreRequests"];
            }
            storeRequest.StoreRequestInfoID = 1;
            storeRequest.Deliverd = false;

            if (ModelState.IsValid)
            {
                foreach (StoreRequestVM sr in selectedStoreRequests)
                {
                    if (sr.StoreRequest.ItemID == storeRequest.ItemID)
                    {
                        if (AvalableQuantity(sr.StoreRequest.ItemID) > 0 && (sr.StoreRequest.Quantity + storeRequest.Quantity) <= AvalableQuantity(sr.StoreRequest.ItemID))
                        {
                            sr.StoreRequest.Quantity += storeRequest.Quantity;
                            found = true;
                            ViewBag.succsessMessage = "Added";
                            break;
                        }
                        else if (AvalableQuantity(sr.StoreRequest.ItemID) == -2)
                        {
                            ViewBag.errorMessage = "Unable to load Item Information!.";
                            found = true;
                        }
                        else
                        {
                            ViewBag.errorMessage = "Low Stock previously " + sr.StoreRequest.Quantity + " Selected Only " + AvalableQuantity(sr.StoreRequest.ItemID) + " avaliable!.";
                            found = true;
                        }
                    }
                }

                if (found == false)
                {
                    if (AvalableQuantity(storeRequest.ItemID) > 0 && (storeRequest.Quantity) <= AvalableQuantity(storeRequest.ItemID))
                    {
                        StoreRequestVM storeRequestVM = new StoreRequestVM();

                        Item item = db.Items.Find(storeRequest.ItemID);
                        storeRequestVM.ItemDescription = item.ItemType.Name;
                        storeRequestVM.Code = item.Code;
                        storeRequestVM.Unit = item.Unit.Name;
                        storeRequestVM.StoreRequest = storeRequest;
                        selectedStoreRequests.Add(storeRequestVM);
                        
                    }
                    else if (AvalableQuantity(storeRequest.ItemID) == -2)
                    {
                        ViewBag.errorMessage = "Unable to lode Item Information!.";
                    }
                    else
                    {
                        ViewBag.errorMessage = "Only " + AvalableQuantity(storeRequest.ItemID) + " avalable!.";
                    }
                }
            }
            else
            {
                ViewBag.errorMessage = "State is not valid";
            }
            TempData[User.Identity.GetUserId() + "selectedStoreRequests"] = selectedStoreRequests;
            ViewBag.selectedStoreRequests = selectedStoreRequests;
            if (selectedStoreRequests.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Name");

            return View();
        }
        public ActionResult Remove(int id = 0)
        {
            List<StoreRequestVM> selectedStoreRequests = new List<StoreRequestVM>();
            selectedStoreRequests = (List<StoreRequestVM>)TempData[User.Identity.GetUserId() + "selectedStoreRequests"];

            foreach (StoreRequestVM sr in selectedStoreRequests)
            {
                if (sr.StoreRequest.ID == id)
                {
                }
                selectedStoreRequests.Remove(sr);
                break;
            }
            TempData[User.Identity.GetUserId() + "selectedStoreRequests"] = selectedStoreRequests;
            ViewBag.selectedStoreRequests = selectedStoreRequests;
            if (selectedStoreRequests.Count > 0)
            {
                ViewBag.haveItem = true;
               
            }
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Name");
            return View("NewRequestEntry");
        }
        /// <summary>
        /// submitteng the requested form to controller and database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>

        public ActionResult NewStoreRequsteInfo()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<StoreRequestVM> selectedStoreRequests = new List<StoreRequestVM>();

            if (TempData[User.Identity.GetUserId() + "selectedStoreRequests"] != null)
            {
                selectedStoreRequests = (List<StoreRequestVM>)TempData[User.Identity.GetUserId() + "selectedStoreRequests"];
                TempData[User.Identity.GetUserId() + "selectedStoreRequests"] = selectedStoreRequests;

            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected store request";
                return RedirectToAction("NewRequestEntry");
            }
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="Super Admin, Admin, Store Manager,")]
        public ActionResult NewStoreRequsteInfo(StoreRequestInfo storeRequestInfo)
        {   
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            ViewBag.StoreID = new SelectList(db.Stores, "ID", "NAME");
            List<StoreRequestVM> selectedStoreRequests = new List<StoreRequestVM>();

            if (TempData[User.Identity.GetUserId() + "selectedStoreRequests"] != null)
            {
                selectedStoreRequests = (List<StoreRequestVM>)TempData[User.Identity.GetUserId() + "selectedStoreRequests"];
            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected Store Order";
                return RedirectToAction("NewRequestEntry");
            }
            storeRequestInfo.ApplicationUserID = User.Identity.GetUserId();
            try
            {
                int LastId = (from sr in db.StoreRequestInfo orderby sr.ID descending select sr.ID).First();
                storeRequestInfo.StoreRequestNumber = "SR.No" + (LastId + 1);
            }
            catch
            {
                storeRequestInfo.StoreRequestNumber = "SR.No-1";
            }
            storeRequestInfo.Date = DateTime.Now;
            bool pass = true;
            if (ModelState.IsValid)
            {
                try
                {
                    storeRequestInfo.Status = "Submmited";
                    db.StoreRequestInfo.Add(storeRequestInfo);
                    db.SaveChanges();
                    pass = true;
                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = "Unable to Preform the Requste you need! View Error detial" + e;
                    pass = false;
                }
                if (pass)
                {
                    try
                    {
                        foreach (StoreRequestVM sr in selectedStoreRequests)
                        {
                            sr.StoreRequest.StoreRequestInfoID = storeRequestInfo.ID;
                            db.StoreRequest.Add(sr.StoreRequest);
                            db.SaveChanges();
                        }
                        ViewBag.succsessMessage = "Store Request is Succesfully Submited!!";
                        return RedirectToAction("StoreRequestDetial", new { id = storeRequestInfo.ID });
                    }
                    catch(Exception e)
                    {   
                        ViewBag.errorMessage = "Unable to preform the request you need View error detail" + e;
                    }
                }
            }
            return View();
        }

        public ActionResult StoreRequestDetial(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected! Try agian";
                return RedirectToAction("StoreRequestionList");
            }
            StoreRequestInfo storeRequestInfo = db.StoreRequestInfo.Find(id);
            if (storeRequestInfo == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Store Request Information";
                return RedirectToAction("StoreRequestionList");
            }
            return View(storeRequestInfo);
        }

        float AvalableQuantity(int item)
        {
            AvaliableOnStock avalable = db.AvaliableOnStocks.Find(item);
            if (avalable != null)
            {
                return avalable.Avaliable;
            }
            return -2;
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