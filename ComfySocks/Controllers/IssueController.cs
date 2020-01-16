using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Issue;
using Microsoft.AspNet.Identity;

namespace ComfySocks.Controllers
{
    public class IssueController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Issue
        public ActionResult IssueList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var storeIssueList = db.StoreIssueInfos;
            return View(storeIssueList.ToList());
        }

        [Authorize(Roles ="Super Admin, Admin, Store Manager")]
        public ActionResult NewIssueEntry(int id=0)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (id != 0)
            {
                List<StoreIssue> storeIssues = new List<StoreIssue>();
                if (TempData[User.Identity.GetUserId() + "StoreIssues"] != null)
                {
                    storeIssues = (List<StoreIssue>)TempData[User.Identity.GetUserId() + "StoreIssues"];
                    TempData[User.Identity.GetUserId() + "StoreIssues"] = null;
                }
                ViewBag.storeIssues = storeIssues;
            }
            else
            {
                TempData[User.Identity.GetUserId() + "StoreIssues"] = null;
            }
            if (db.StoreRequest.ToList().Count == 0)
            {
                ViewBag.StoreRequests = "Register request information";
            }

            ViewBag.orders = null;
            ViewBag.StoreRequestID = new SelectList(db.StoreRequest, "ID", "ID");
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Name");
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult NewIssueEntry(StoreIssue storeIssue)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<StoreIssue> storeIssues = new List<StoreIssue>();

            if (TempData[User.Identity.GetUserId() + "StoreIssues"] != null)
            {
                storeIssues = (List<StoreIssue>)TempData[User.Identity.GetUserId() + "StoreIssues"];
                TempData[User.Identity.GetUserId() + "StoreIssues"] = null;
            }
            storeIssue.StoreIssueInfoID = 1;
            storeIssue.RemaningDelivery = (float)storeIssue.Quantity;
            float totalPrice = 0;

            bool found = false;
            if (ModelState.IsValid)
            {
                AvaliableOnStock av = db.AvaliableOnStocks.Find(storeIssue.ItemID);
                if (av == null)
                {
                    ViewBag.errorMessage = "Low Stock!Only 0 item available";
                }
                else
                {
                    double selectedQuantity = 0;
                    foreach (StoreIssue si in storeIssues)
                    {
                        if (si.ItemID == storeIssue.ItemID)
                        {
                            selectedQuantity += si.Quantity;
                        }
                        totalPrice += (float)(si.Quantity * si.UnitPrice);
                    }
                    if (av.Avaliable > selectedQuantity + storeIssue.Quantity)
                    {
                        foreach (var i in storeIssues)
                        {
                            //&& i.JobsID == storeIssue.JobsID
                            if (i.ItemID == storeIssue.ItemID)
                            {

                                i.Quantity += storeIssue.Quantity;
                                found = true;
                                break;

                            }
                        }
                        if (!found)
                        {
                            storeIssues.Add(storeIssue);
                        }
                    }
                    else
                    {
                        ViewBag.errorMessage = "Low Stock! Only " + av.Avaliable + " item available";
                    }
                }
            }
            if (storeIssues.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.storeIssues = storeIssues;
            TempData[User.Identity.GetUserId() + "StoreIssues"] = storeIssues;
            TempData[User.Identity.GetUserId() + "totalPrice"] = totalPrice;
            ViewBag.StoreRequestID = new SelectList(db.StoreRequest, "ID", "ID");
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Name");
            return View();
        }

        public ActionResult RemoveSelected(int id)
        {
            if (TempData[User.Identity.GetUserId() + "StoreIssues"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find selected orders. try again.";
                return RedirectToAction("NewIssueEntry");
            }
            List<StoreIssue> storeIssues = new List<StoreIssue>();
            storeIssues = (List<StoreIssue>)TempData[User.Identity.GetUserId() + "StoreIssues"];
            float totalPrice = (float)TempData[User.Identity.GetUserId() + "totalPrice"];
            foreach (var i in storeIssues)
            {
                if (i.ID == id)
                {
                    storeIssues.Remove(i);
                    ViewBag.succsessMessage = "Issued item Removed";
                    break;
                }
            }
            if (storeIssues.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.storeIssues = storeIssues;
            TempData[User.Identity.GetUserId() + "StoreIssues"] = storeIssues;
            TempData[User.Identity.GetUserId() + "totalPrice"] = totalPrice;
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Name");
            return View("NewIssueEntry");
        }

        public ActionResult NewStoreIssueInfo()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId()+ "StoreIssues"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Load store Issue Item.";
                return RedirectToAction("NewIssueEntry");
            }
            ViewBag.StoreRequestInfoID = new SelectList(db.StoreRequest, "ID", "StoreRequestInfoID");
            return View();
        }

        [HttpPost]
        public ActionResult NewStoreIssueInfo(StoreIssueInfo storeIssueInfo)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId() + "StoreIssues"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Load store Issue Item.";
                return RedirectToAction("NewIssueEntry");
            }
            ViewBag.StoreRequestInfoID = new SelectList(db.StoreRequest, "ID", "StoreRequestInfoID");
            storeIssueInfo.ApplicationUserID = User.Identity.GetUserId();
            storeIssueInfo.Date = DateTime.Now;

            return View();
        }


        [Authorize(Roles = "Salse,Super Admin,Admin,")]
        public ActionResult IssueDetail(int id=0)
        {
            if (id == 0)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation";

                return RedirectToAction("IssueList");
            }
            StoreIssueInfo SI = db.StoreIssueInfos.Find(id);
            if (SI == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Issued Item is not Found";
            }
            return View(SI);
        }
    }
}