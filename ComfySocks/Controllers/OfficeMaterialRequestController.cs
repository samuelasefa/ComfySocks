using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.OfficeRequest;
using ComfySocks.Models.Request;
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
    public class OfficeMaterialRequestController : Controller  
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Request
        public ActionResult OfficeMaterialRequestionList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var OfficeMaterial = (from officereq in db.OfficeMaterialRequestInformation orderby officereq.Date descending select officereq).ToList();

            return View(OfficeMaterial);
        }
        [Authorize(Roles = "Super Admin, Production")]
        public ActionResult NewOfficeMaterialRequest(int id = 0)
        {
            if (TempData[User.Identity.GetUserId() + "successMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null;}
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            //Frist required Item /1 Material is in the stock or not
            {
                var stock = db.Stocks.ToList();
                ViewBag.stock = "";

                if (stock.Count() == 0)
                {
                    ViewBag.stock = "Register Stock Information Frist";
                }
                
            }
            ViewBag.ItemID = (from S in db.Items where S.StoreType == StoreType.OfficeMaterial orderby S.ID ascending select S).ToList();
            if (id != 0)
            {
                List<OfficeMaterialRequestVM> selectedOfficeMaterialRequests = new List<OfficeMaterialRequestVM>();
                selectedOfficeMaterialRequests = (List<OfficeMaterialRequestVM>)TempData[User.Identity.GetUserId() + "selectedOfficeMaterialRequests"];
                TempData[User.Identity.GetUserId() + "selectedOfficeMaterialRequests"] = selectedOfficeMaterialRequests;
                ViewBag.selectedOfficeMaterialRequests = selectedOfficeMaterialRequests;
            }
            else {
                TempData[User.Identity.GetUserId() + "selectedOfficeMaterialRequests"] = null;
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Production")]
        public ActionResult NewOfficeMaterialRequest(OfficeMaterialRequest officeMaterialRequest)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<OfficeMaterialRequestVM> selectedOfficeMaterialRequests = new List<OfficeMaterialRequestVM>();
            bool found = false;

            if (TempData[User.Identity.GetUserId() + "selectedOfficeMaterialRequests"] != null)
            {
                selectedOfficeMaterialRequests = (List<OfficeMaterialRequestVM>)TempData[User.Identity.GetUserId() + "selectedOfficeMaterialRequests"];
            }
            officeMaterialRequest.OfficeMaterialRequestInformationID = 1;
            officeMaterialRequest.RemaningDelivery = officeMaterialRequest.Quantity;
            officeMaterialRequest.Deliverd = false;

            if (ModelState.IsValid)
            {
                foreach (OfficeMaterialRequestVM sr in selectedOfficeMaterialRequests)
                {
                    if (sr.OfficeMaterialRequest.ItemID == officeMaterialRequest.ItemID)
                    {
                        if (AvalableQuantity(sr.OfficeMaterialRequest.ItemID) > 0 && (sr.OfficeMaterialRequest.Quantity + officeMaterialRequest.Quantity) <= AvalableQuantity(sr.OfficeMaterialRequest.ItemID))
                        {
                            sr.OfficeMaterialRequest.Quantity += officeMaterialRequest.Quantity;
                            found = true;
                            ViewBag.succsessMessage = "Item is Added Congrates!!!";
                            break;
                        }
                        else if (AvalableQuantity(sr.OfficeMaterialRequest.ItemID) == -2)
                        {
                            ViewBag.errorMessage = "Unable to load Item Information!.";
                            found = true;
                        }
                        else
                        {
                            ViewBag.errorMessage = "Low Stock previously " + sr.OfficeMaterialRequest.Quantity + " Selected Only " + AvalableQuantity(sr.OfficeMaterialRequest.ItemID) + " avaliable!.";
                            found = true;
                        }
                    }
                }

                if (found == false)
                {
                    if (AvalableQuantity(officeMaterialRequest.ItemID) > 0 && (officeMaterialRequest.Quantity) <= AvalableQuantity(officeMaterialRequest.ItemID))
                    {
                        OfficeMaterialRequestVM officeMaterialRequestVM = new OfficeMaterialRequestVM();

                        Item item = db.Items.Find(officeMaterialRequest.ItemID);
                        officeMaterialRequestVM.ItemDescription = item.Name;
                        officeMaterialRequestVM.Type = item.ItemType.Name;
                        officeMaterialRequestVM.Code = item.Code;
                        officeMaterialRequestVM.Unit = item.Unit.Name;
                        officeMaterialRequestVM.OfficeMaterialRequest = officeMaterialRequest;
                        selectedOfficeMaterialRequests.Add(officeMaterialRequestVM);
                        
                    }
                    else if (AvalableQuantity(officeMaterialRequest.ItemID) == -2)
                    {
                        ViewBag.errorMessage = "Unable to lode Item Information!.";
                    }
                    else
                    {
                        ViewBag.errorMessage = "Only " + AvalableQuantity(officeMaterialRequest.ItemID) + " avalable!.";
                    }
                }
            }
            else
            {
                ViewBag.errorMessage = "State is not valid";
            }
            TempData[User.Identity.GetUserId() + "selectedOfficeMaterialRequests"] = selectedOfficeMaterialRequests;
            ViewBag.selectedOfficeMaterialRequests = selectedOfficeMaterialRequests;
            if (selectedOfficeMaterialRequests.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.ItemID = (from S in db.Items where S.StoreType == StoreType.OfficeMaterial orderby S.ID ascending select S).ToList();
            

            return View();
        }

        [Authorize(Roles = "Super Admin, Production")]
        public ActionResult Remove(int id = 0)
        {
            List<OfficeMaterialRequestVM> selectedOfficeMaterialRequests = new List<OfficeMaterialRequestVM>();
            selectedOfficeMaterialRequests = (List<OfficeMaterialRequestVM>)TempData[User.Identity.GetUserId() + "selectedOfficeMaterialRequests"];

            foreach (OfficeMaterialRequestVM sr in selectedOfficeMaterialRequests)
            {
                if (sr.OfficeMaterialRequest.ID == id)
                {
                }
                selectedOfficeMaterialRequests.Remove(sr);
                break;
            }
            TempData[User.Identity.GetUserId() + "selectedOfficeMaterialRequests"] = selectedOfficeMaterialRequests;
            ViewBag.selectedOfficeMaterialRequests = selectedOfficeMaterialRequests;
            if (selectedOfficeMaterialRequests.Count > 0)
            {
                ViewBag.haveItem = true;
               
            }
            ViewBag.ItemID = (from S in db.Items where S.StoreType == StoreType.OfficeMaterial orderby S.ID ascending select S).ToList();
            return View("NewOfficeMaterialRequest");
        }


        /// <summary>
        /// submitteng the requested form to controller and database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        
        [Authorize(Roles = "Super Admin, Production")]
        public ActionResult NewOfficeMaterialInfo()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            List<OfficeMaterialRequestVM> selectedOfficeMaterialRequests = new List<OfficeMaterialRequestVM>();
            if (TempData[User.Identity.GetUserId() + "selectedOfficeMaterialRequests"] != null)
            {
                selectedOfficeMaterialRequests = (List<OfficeMaterialRequestVM>)TempData[User.Identity.GetUserId() + "selectedOfficeMaterialRequests"];
                TempData[User.Identity.GetUserId() + "selectedOfficeMaterialRequests"] = selectedOfficeMaterialRequests;

            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected store request";
                return RedirectToAction("NewOfficeMaterialRequest");
            }
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Production")]
        public ActionResult NewOfficeMaterialInfo(OfficeMaterialRequestInformation officeMaterialRequestInformation)
        {   
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            List<OfficeMaterialRequestVM> selectedOfficeMaterialRequests = new List<OfficeMaterialRequestVM>();

            if (TempData[User.Identity.GetUserId() + "selectedOfficeMaterialRequests"] != null)
            {
                selectedOfficeMaterialRequests = (List<OfficeMaterialRequestVM>)TempData[User.Identity.GetUserId() + "selectedOfficeMaterialRequests"];
            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected Store Order";
                return RedirectToAction("NewOfficeMaterialRequest");
            }
            officeMaterialRequestInformation.ApplicationUserID = User.Identity.GetUserId();
            try
            {
                int LastId = (from sr in db.OfficeMaterialRequestInformation orderby sr.ID descending select sr.ID).First();
                officeMaterialRequestInformation.StoreRequestNumber = "No:-" + (LastId + 1).ToString("D4");
            }
            catch
            {
                officeMaterialRequestInformation.StoreRequestNumber = "No:-" + 1.ToString("D4");
            }
            officeMaterialRequestInformation.Date = DateTime.Now;
            bool pass = true;
            if (ModelState.IsValid)
            {
                try
                {
                    officeMaterialRequestInformation.Status = "Submmited";
                    db.OfficeMaterialRequestInformation.Add(officeMaterialRequestInformation);
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
                        foreach (OfficeMaterialRequestVM sr in selectedOfficeMaterialRequests)
                        {
                            sr.OfficeMaterialRequest.OfficeMaterialRequestInformationID = officeMaterialRequestInformation.ID;
                            db.OfficeMaterialRequest.Add(sr.OfficeMaterialRequest);
                            db.SaveChanges();
                        }
                        ViewBag.succsessMessage = "Office Material Request is Succesfully Submited!!";
                        return RedirectToAction("OfficeMaterialRequestDetail", new { id = officeMaterialRequestInformation.ID });
                    }
                    catch(Exception e)
                    {   
                        ViewBag.errorMessage = "Unable to preform the request you need View error detail" + e;
                    }
                }
            }
            return View();
        }

        public ActionResult OfficeMaterialRequestDetail(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected! Try agian";
                return RedirectToAction("OfficeMaterialRequestionList");
            }
            OfficeMaterialRequestInformation officeMaterialRequestInformation = db.OfficeMaterialRequestInformation.Find(id);
            if (officeMaterialRequestInformation == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Store Request Information";
                return RedirectToAction("OfficeMaterialRequestionList");
            }
            return View(officeMaterialRequestInformation);
        }


        //Request is approved 
        public ActionResult RequestApproved(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected!. Try again";
                return RedirectToAction("OfficeMaterialRequestionList");
            }
            OfficeMaterialRequestInformation officeMaterialRequestInformation = db.OfficeMaterialRequestInformation.Find(id);
            if (officeMaterialRequestInformation == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Reterive Store Request Information";
                return RedirectToAction("OfficeMaterialRequestionList");
            }
            List<OfficeMaterialRequstVMForError> ErrorList = new List<OfficeMaterialRequstVMForError>();
            bool pass = true;
            foreach (OfficeMaterialRequest officeMaterialRequest in officeMaterialRequestInformation.OfficeMaterialRequest)
            {
                Stock stock = (from s in db.Stocks where s.ItemID == officeMaterialRequest.ItemID && s.StoreID == officeMaterialRequestInformation.StoreID select s).First();
                if (stock == null)
                {
                    OfficeMaterialRequstVMForError officeMaterialRequstVMForError = new OfficeMaterialRequstVMForError()
                    {
                        OfficeMaterialRequest = officeMaterialRequest,
                        Error = "Unable to load store Information Posible reason is no information registed about the item"
                    }; pass = false;
                    ViewBag.errorMessage = "Some error found. see error detail for more information";
                    ErrorList.Add(officeMaterialRequstVMForError);
                }
                else if(stock.Total < officeMaterialRequest.Quantity)
                {
                    OfficeMaterialRequstVMForError officeMaterialRequstVMForError = new OfficeMaterialRequstVMForError()
                    {
                        OfficeMaterialRequest = officeMaterialRequest,
                        Error = "The Avaliable stock in " + officeMaterialRequestInformation.Store.Name + "store is less than requested Quantity" + stock.Total
                    };
                    pass = false;
                    ViewBag.errorMessage = "Some error found2. see error in detail for more information";
                    ErrorList.Add(officeMaterialRequstVMForError);
                }
            }
            if (pass)
            {
                foreach (OfficeMaterialRequest officeMaterialRequest in officeMaterialRequestInformation.OfficeMaterialRequest)
                {
                    Stock stock = (from s in db.Stocks where s.ItemID == officeMaterialRequest.ItemID && s.StoreID == officeMaterialRequestInformation.StoreID select s).First();
                    stock.Total -= officeMaterialRequest.Quantity;
                    db.Entry(stock).State = EntityState.Modified;
                    db.SaveChanges();
                    AvaliableOnStock avaliableOnStock = db.AvaliableOnStocks.Find(officeMaterialRequest.ItemID);
                    Item i = db.Items.Find(avaliableOnStock.ID);
                    var deference = avaliableOnStock.RecentlyReduced - officeMaterialRequest.Quantity;

                    if (deference > 0)
                    {
                        avaliableOnStock.RecentlyReduced -= officeMaterialRequest.Quantity;
                    }
                    else {
                        avaliableOnStock.RecentlyReduced = 0;
                        avaliableOnStock.Avaliable += deference;
                        
                    }
                    db.Entry(avaliableOnStock).State = EntityState.Modified;
                    db.SaveChanges();
                }
                officeMaterialRequestInformation.Status = "Approved";
                officeMaterialRequestInformation.ApprovedBy = User.Identity.GetUserName();
                db.Entry(officeMaterialRequestInformation).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.succsessMessage = "Store Request is approved!!.";
            }

            else
            {
                ViewBag.erroList = ErrorList;
            }

            return View("OfficeMaterialRequestDetail", officeMaterialRequestInformation);
        }

        //Request is Rejected

        public ActionResult RequestRejected(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected!. Try again";
                return RedirectToAction("OfficeMaterialRequestionList");
            }
            OfficeMaterialRequestInformation officeMaterialRequestInformation = db.OfficeMaterialRequestInformation.Find(id);
            if (officeMaterialRequestInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Reterive Store Request Information";
                return RedirectToAction("OfficeMaterialRequestionList");
            }
            List<OfficeMaterialRequstVMForError> ErrorList = new List<OfficeMaterialRequstVMForError>();
            bool pass = true;
            foreach (OfficeMaterialRequest officeMaterialRequest in officeMaterialRequestInformation.OfficeMaterialRequest)
            {
                Stock stock = (from s in db.Stocks where s.ItemID == officeMaterialRequest.ItemID && s.StoreID == officeMaterialRequestInformation.StoreID select s).First();
                if (stock == null)
                {
                    OfficeMaterialRequstVMForError officeMaterialRequstVMForError = new OfficeMaterialRequstVMForError()
                    {
                        OfficeMaterialRequest = officeMaterialRequest,
                        Error = "Unable to load store Information Posible reason is no information registed about the item"
                    }; pass = false;
                    ViewBag.errorMessage = "Some error found. see error detail for more information";
                    ErrorList.Add(officeMaterialRequstVMForError);
                }
                else if (stock.Total < officeMaterialRequest.Quantity)
                {
                    OfficeMaterialRequstVMForError officeMaterialRequstVMForError = new OfficeMaterialRequstVMForError()
                    {
                        OfficeMaterialRequest = officeMaterialRequest,
                        Error = "The Avaliable stock in " + officeMaterialRequestInformation.Store.Name + "store is less than requested Quantity" + stock.Total
                    };
                    pass = false;
                    ViewBag.errorMessage = "Some error found. see error in detail for more information";
                    ErrorList.Add(officeMaterialRequstVMForError);
                }
            }
            if (pass)
            {
                foreach (OfficeMaterialRequest officeMaterialRequest in officeMaterialRequestInformation.OfficeMaterialRequest)
                {
                    Stock stock = (from s in db.Stocks where s.ItemID == officeMaterialRequest.ItemID && s.StoreID == officeMaterialRequestInformation.StoreID select s).First();
                    stock.Total = stock.Total;
                    db.Entry(stock).State = EntityState.Modified;
                    db.SaveChanges();
                    AvaliableOnStock avaliableOnStock = db.AvaliableOnStocks.Find(officeMaterialRequest.ItemID);
                    Item i = db.Items.Find(avaliableOnStock.ID);
                    avaliableOnStock.RecentlyReduced = 0;
                    avaliableOnStock.Avaliable = stock.Total;
                    db.Entry(avaliableOnStock).State = EntityState.Modified;
                    db.SaveChanges();
                }
                officeMaterialRequestInformation.Status = "Rejected";
                officeMaterialRequestInformation.ApprovedBy = User.Identity.GetUserName();
                db.Entry(officeMaterialRequestInformation).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.succsessMessage = "Store Row Material Request is Rejected!!.";
            }

            else
            {
                ViewBag.erroList = ErrorList;
            }
            return View("OfficeMaterialRequestDetail", officeMaterialRequestInformation);
        }

   

        //to check avaliable on stock 
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