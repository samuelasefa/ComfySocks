using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.PurchaseRequestInfo;
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
    public class PurchaseRequestController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Request
        public ActionResult PurchaseRequsetList()
        {
            //errormessage display
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            var purchaseRequest = (from purchase in db.PurchaseRequestInformation orderby purchase.ID descending select purchase).ToList();

            return View(purchaseRequest);
        }

        // controller of Finshed product transfer
        public ActionResult NewPurchaseRequestEntry()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            ViewBag.PurchaseRequestID = (from p in db.Items where p.StoreType == StoreType.OfficeMaterial || p.StoreType == StoreType.RowMaterial  orderby p.ID select p).ToList();
            return View();
        }

        //post method  for transfering product
        [System.Web.Mvc.HttpPost]
        public ActionResult NewPurchaseRequestEntry(PurchaseRequest purchaseRequest)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<PurchaseRequestVM> SelectedPurchaseRequest = new List<PurchaseRequestVM>();
            bool found = false;
            if (TempData[User.Identity.GetUserId() + "SelectedPurchaseRequest"] != null)
            {
                SelectedPurchaseRequest = (List<PurchaseRequestVM>)TempData[User.Identity.GetUserId() + "SelectedPurchaseRequest"];
            }
            purchaseRequest.PurchaseRequestInformationID= 1;

            var item = db.PurchaseRequests.Find(purchaseRequest.ItemID);

            if (ModelState.IsValid)
            {

                foreach (PurchaseRequestVM t in SelectedPurchaseRequest)
                {
                    if (t.PurchaseRequest.ItemID == purchaseRequest.ItemID)
                    {
                        t.PurchaseRequest.Quantity += purchaseRequest.Quantity;
                        found = true;
                        ViewBag.infoMessage = "Purchase Request Item is Added!";
                        break;
                    }
                }
                if (!found)
                {
                    Item I = db.Items.Find(purchaseRequest.ItemID);

                    if (I == null)
                    {
                        ViewBag.errorMessage = "Unable to find Item";
                    }
                    else
                    {
                        PurchaseRequestVM purchaseRequestVM = new PurchaseRequestVM();
                        purchaseRequestVM.ItemDescription = I.Name;
                        purchaseRequestVM.Code = I.Code;
                        purchaseRequestVM.Unit = I.Unit.Name;
                        purchaseRequestVM.PurchaseRequest = purchaseRequest;
                        SelectedPurchaseRequest.Add(purchaseRequestVM);
                    }
                }
            }
            else
            {
                ViewBag.errorMessage = "State is not valid";
            }
            ViewBag.SelectedPurchaseRequest = SelectedPurchaseRequest;
            TempData[User.Identity.GetUserId() + "SelectedPurchaseRequest"] = SelectedPurchaseRequest;
            if (SelectedPurchaseRequest.Count() > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.PurchaseRequestID = (from p in db.Items where p.StoreType == StoreType.OfficeMaterial || p.StoreType == StoreType.RowMaterial orderby p.ID select p).ToList();
            return View(purchaseRequest);
        }

        public ActionResult Remove(int id = 0)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            ViewBag.PurchaseRequestID = (from p in db.Items where p.StoreType == StoreType.OfficeMaterial || p.StoreType == StoreType.RowMaterial orderby p.ID select p).ToList();


            List<PurchaseRequestVM> SelectedPurchaseRequest = new List<PurchaseRequestVM>();
            if (TempData[User.Identity.GetUserId() + "SelectedPurchaseRequest"] != null)
            {
                try
                {
                    SelectedPurchaseRequest = (List<PurchaseRequestVM>)TempData[User.Identity.GetUserId() + "SelectedPurchaseRequest"];
                    foreach (PurchaseRequestVM items in SelectedPurchaseRequest)
                    {
                        if (items.PurchaseRequest.ItemID == id)
                        {
                            SelectedPurchaseRequest.Remove(items);
                            if (SelectedPurchaseRequest.Count() > 0)
                            {
                                TempData[User.Identity.GetUserId() + "SelectedPurchaseRequest"] = SelectedPurchaseRequest;
                                ViewBag.selectedTransfer = SelectedPurchaseRequest;
                            }
                            return View("NewPurchaseRequestEntry");
                        }
                    }

                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = e;
                }

            }
            ViewBag.errorMesage = "You process can't be performed for now try again";
            return View("NewPurchaseRequestEntry");
        }

        public ActionResult NewPurchaseRequsteInfo()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            List<PurchaseRequestVM> SelectedPurchaseRequest = new List<PurchaseRequestVM>();

            if (TempData[User.Identity.GetUserId() + "SelectedPurchaseRequest"] != null)
            {
                SelectedPurchaseRequest = (List<PurchaseRequestVM>)TempData[User.Identity.GetUserId() + "SelectedPurchaseRequest"];
                TempData[User.Identity.GetUserId() + "SelectedPurchaseRequest"] = SelectedPurchaseRequest;
            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected Purchase";
                return RedirectToAction("NewPurchaseRequestEntry");
            }
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            return View();
        }


        [HttpPost]
        public ActionResult NewPurchaseRequsteInfo(PurchaseRequestInformation purchaseRequestInformation)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            List<PurchaseRequestVM> SelectedPurchaseRequest = new List<PurchaseRequestVM>();
            if (TempData[User.Identity.GetUserId() + "SelectedPurchaseRequest"] != null)
            {
                SelectedPurchaseRequest = (List<PurchaseRequestVM>)TempData[User.Identity.GetUserId() + "SelectedPurchaseRequest"];
            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected Store Transfer";
                return RedirectToAction("NewPurchaseRequestEntry");
            }
            purchaseRequestInformation.ApplicationUserID = User.Identity.GetUserId();
            try
            {
                int LastId = (from s in db.PurchaseRequestInformation orderby s.ID orderby s.ID descending select s.ID).First();
                purchaseRequestInformation.PurchaseRequestNumber = "No:-" + (LastId + 1).ToString("D4");
            }
            catch
            {
                purchaseRequestInformation.PurchaseRequestNumber = "No:-" + 1.ToString("D4");
            }
            purchaseRequestInformation.Date = DateTime.Now;
            bool pass = true;
            if (ModelState.IsValid)
            {
                try
                {
                    purchaseRequestInformation.Status = "Submitted";
                    db.PurchaseRequestInformation.Add(purchaseRequestInformation);
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
                        foreach (PurchaseRequestVM s in SelectedPurchaseRequest)
                        {
                            s.PurchaseRequest.PurchaseRequestInformationID= purchaseRequestInformation.ID;
                            db.PurchaseRequests.Add(s.PurchaseRequest);
                            db.SaveChanges();
                        }
                        ViewBag.succsessMessage = "Purchase Request item is created!.";
                        return RedirectToAction("PurchaseRequestDetial", new { id = purchaseRequestInformation.ID });
                    }
                    catch (Exception e)
                    {
                        ViewBag.errorMessage = "Unable to perform the request you need!view error detail" + e;
                    }
                }
            }
            return View();
        }
        public ActionResult PurchaseRequestDetial(int? id)
        {
            //errormessage display
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }

            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected!";
                return RedirectToAction("PurchaseRequsetList");
            }
            PurchaseRequestInformation purchase = db.PurchaseRequestInformation.Find(id);

            if (purchase == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected!";
                return RedirectToAction("PurchaseRequsetList");
            }
            return View(purchase);
        }
        //Transfer approval
        public ActionResult PurchaseApproved(int? id)
        {

            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("PurchaseRequestDetail");
            }

            PurchaseRequestInformation purchaseRequestInformation = db.PurchaseRequestInformation.Find(id);
            if (purchaseRequestInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Find Product Transfer Information";
                return RedirectToAction("PurchaseRequestDetail");
            }
            List<PurchaseRequstVMForError> ErrorList = new List<PurchaseRequstVMForError>();
            bool pass = true;

            foreach (PurchaseRequest purchases in purchaseRequestInformation.PurchaseRequest)
            {
                Item items = (from t in db.Items where t.ID == purchases.ItemID select t).First();
                if (items == null)
                {
                    PurchaseRequstVMForError purchaseRequstVMForError = new PurchaseRequstVMForError()
                    {
                        PurchaseRequest = purchases,
                        Error = "Unable to load Product Information Posible reason is no information registerd about the item"
                    }; pass = false;
                    ViewBag.errorMessage = "Some error found. see error details for more information";
                    ErrorList.Add(purchaseRequstVMForError);
                }
            }
            if (pass)
            {
                foreach (PurchaseRequest purchase in purchaseRequestInformation.PurchaseRequest)
                {
                    Item items = (from t in db.Items where t.ID == purchase.ItemID select t).First();
                    db.Entry(purchase).State = EntityState.Modified;
                    db.SaveChanges();
                }
                purchaseRequestInformation.Status = "Approved";
                purchaseRequestInformation.ApprovedBy = User.Identity.GetUserName();
                db.Entry(purchaseRequestInformation).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.successMessage = "Successfully Requested!!";
            }
            else
            {
                ViewBag.errorList = ErrorList;
            }

            return View("PurchaseRequestDetail", purchaseRequestInformation);
        }

        //Transfer rejection
        public ActionResult PurchaseRejected(int? id)
        {

            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("PurchaseRequestDetail");
            }

            PurchaseRequestInformation purchaseRequestInformation = db.PurchaseRequestInformation.Find(id);
            if (purchaseRequestInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Find Purchase Request Information";
                return RedirectToAction("PurchaseRequestDetail");
            }
            List<PurchaseRequstVMForError> ErrorList = new List<PurchaseRequstVMForError>();
            bool pass = true;

            foreach (PurchaseRequest purchase in purchaseRequestInformation.PurchaseRequest)
            {
                if (pass)
                {
                    purchaseRequestInformation.Status = "Rejected";
                    purchaseRequestInformation.ApprovedBy = User.Identity.GetUserName();
                    db.Entry(purchaseRequestInformation).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.successMessage = "Successfully Rejected!!";
                }
                else
                {
                    ViewBag.errorList = ErrorList;
                }
            }
            return View("PurchaseRequestDetail", purchaseRequestInformation);
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