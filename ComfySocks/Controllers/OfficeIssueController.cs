using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.Request;
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
    public class OfficeIssueController : Controller  
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Request
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult OfficeIssueList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var OfficeIsssueInfo = (from office in db.StoreRequestInformation orderby office.ID ascending select office).ToList();

            return View(OfficeIsssueInfo);
        }
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult NewOfficeIssueEntry()
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
            ViewBag.StockID = (from S in db.Stocks where S.Item.StoreType == StoreType.RowMaterial orderby S.ID descending select S).ToList();
                
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult NewOfficeIssueEntry(StoreRequest storeRequest)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<StoreRequestVM> selectedStoreRequests = new List<StoreRequestVM>();
            bool found = false;

            if (TempData[User.Identity.GetUserId() + "selectedStoreRequests"] != null)
            {
                selectedStoreRequests = (List<StoreRequestVM>)TempData[User.Identity.GetUserId() + "selectedStoreRequests"];
            }
            storeRequest.StoreRequestInformationID = 1;
            storeRequest.RemaningDelivery = (float)storeRequest.Quantity;
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
                            ViewBag.infoMessage = "Item is Added !!!";
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
                        storeRequestVM.ItemDescription = item.Name;
                        storeRequestVM.Type = item.ItemType.Name;
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
            ViewBag.StockID = (from S in db.Stocks where S.Item.StoreType == StoreType.RowMaterial orderby S.ID descending select S).ToList();

            return View();
        }

        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult Remove(int id = 0)
        {
            List<StoreRequestVM> selectedStoreRequests = new List<StoreRequestVM>();
            selectedStoreRequests = (List<StoreRequestVM>)TempData[User.Identity.GetUserId() + "selectedStoreRequests"];

            foreach (StoreRequestVM s in selectedStoreRequests)
            {
                if (s.StoreRequest.ID == id)
                {
                }
                selectedStoreRequests.Remove(s);
                break;
            }
            TempData[User.Identity.GetUserId() + "selectedStoreRequests"] = selectedStoreRequests;
            ViewBag.selectedStoreRequests = selectedStoreRequests;
            if (selectedStoreRequests.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.StockID = (from S in db.Stocks where S.Item.StoreType == StoreType.RowMaterial orderby S.ID descending select S).ToList();
            return View("NewRequestEntry");
        }
       
        
        /// <summary>
        /// submitteng the requested form to controller and database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult NewOfficeIssueInfo()
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
        [Authorize(Roles ="Super Admin, Admin, Production,")]
        public ActionResult NewOfficeIssueInfo(StoreRequestInformation StoreRequestInformation)
        {   
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
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
            StoreRequestInformation.ApplicationUserID = User.Identity.GetUserId();
            try
            {
                int LastId = (from sr in db.StoreRequestInformation orderby sr.ID descending select sr.ID).First();
                StoreRequestInformation.StoreRequestNumber = "SR.No:-" + (LastId + 1);
            }
            catch
            {
                StoreRequestInformation.StoreRequestNumber = "SR.No-1";
            }
            StoreRequestInformation.Date = DateTime.Now;
            bool pass = true;
            if (ModelState.IsValid)
            {
                try
                {
                    StoreRequestInformation.Status = "Submmited";
                    db.StoreRequestInformation.Add(StoreRequestInformation);
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
                            sr.StoreRequest.StoreRequestInformationID = StoreRequestInformation.ID;
                            db.StoreRequest.Add(sr.StoreRequest);
                            db.SaveChanges();
                        }
                        ViewBag.succsessMessage = "Store Request is Succesfully Submited!!";
                        return RedirectToAction("StoreRequestDetial", new { id = StoreRequestInformation.ID });
                    }
                    catch(Exception e)
                    {   
                        ViewBag.errorMessage = "Unable to preform the request you need View error detail" + e;
                    }
                }
            }
            return View();
        }

        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult OfficeIssueDetial(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected! Try agian";
                return RedirectToAction("StoreRequestionList");
            }
            StoreRequestInformation StoreRequestInformation = db.StoreRequestInformation.Find(id);
            if (StoreRequestInformation == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Store Request Information";
                return RedirectToAction("StoreRequestionList");
            }
            return View(StoreRequestInformation);
        }


        //Request is approved 
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult IssueApproved(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected!. Try again";
                return RedirectToAction("StoreRequestionList");
            }
            StoreRequestInformation StoreRequestInformation = db.StoreRequestInformation.Find(id);
            if (StoreRequestInformation == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Reterive Store Request Information";
                return RedirectToAction("StoreRequestionList");
            }
            List<StoreRequstVMForError> ErrorList = new List<StoreRequstVMForError>();
            bool pass = true;
            foreach (StoreRequest storeRequest in StoreRequestInformation.StoreRequest)
            {
                //StoreRequest request = (from sr in db.StoreRequest where sr.ItemID == storeRequest.ItemID select sr).First();
                //request.Quantity += storeRequest.Quantity;
                //db.Entry(request).State = EntityState.Modified;
                //db.SaveChanges();
                Stock stock = (from s in db.Stocks where s.ItemID == storeRequest.ItemID && s.StoreID == StoreRequestInformation.StoreID select s).First();
                AvaliableOnStock avaliableOnStock = db.AvaliableOnStocks.Find(stock.ItemID);
                if (stock == null)
                {
                    StoreRequstVMForError storeRequstVMForError = new StoreRequstVMForError()
                    {
                        StoreRequest = storeRequest,
                        Error = "Unable to load store Information Posible reason is no information registed about the item"
                    }; pass = false;
                    ViewBag.errorMessage = "Some error found. see error detail for more information";
                    ErrorList.Add(storeRequstVMForError);
                }
                else if(avaliableOnStock.Avaliable < storeRequest.Quantity)
                {
                    StoreRequstVMForError storeRequstVMError = new StoreRequstVMForError()
                    {
                        StoreRequest = storeRequest,
                        Error = "The Avaliable stock in "+ StoreRequestInformation.Store.Name + "store is less than requested Quantity" + stock.Total
                    };
                    pass = false;
                    ViewBag.errorMessage = "The Avaliable stock in " + StoreRequestInformation.Store.Name + " is less than requested Quantity" + stock.Total;
                    ErrorList.Add(storeRequstVMError);
                }
            }
            if (pass)
            {
                foreach (StoreRequest storeRequest in StoreRequestInformation.StoreRequest)
                {
                    Stock stock = (from s in db.Stocks where s.ItemID == storeRequest.ItemID && s.StoreID == StoreRequestInformation.StoreID select s).First();
                    //stock.Total -= (float)storeRequest.Quantity;
                    db.Entry(stock).State = EntityState.Modified;
                    db.SaveChanges();
                    AvaliableOnStock avaliableOnStock = db.AvaliableOnStocks.Find(storeRequest.ItemID);
                    Item i = db.Items.Find(avaliableOnStock.ID);
                    float deference = avaliableOnStock.RecentlyReduced - storeRequest.Quantity;

                    if (deference > 0)
                    {
                        avaliableOnStock.RecentlyReduced -= (float)storeRequest.Quantity;
                    }
                    else {
                        avaliableOnStock.RecentlyReduced = 0;
                        avaliableOnStock.Avaliable += deference;
                        
                    }
                    db.Entry(avaliableOnStock).State = EntityState.Modified;
                    db.SaveChanges();
                }
                StoreRequestInformation.Status = "Approved";
                StoreRequestInformation.ApprovedBy = User.Identity.GetUserName();
                db.Entry(StoreRequestInformation).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.succsessMessage = "Store Request is approved!!.";
            }

            else
            {
                ViewBag.erroList = ErrorList;
            }

            return View("StoreRequestDetial", StoreRequestInformation);
        }

        //Request is Rejected

        public ActionResult IssueRejected(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected!. Try again";
                return RedirectToAction("StoreRequestionList");
            }
            StoreRequestInformation StoreRequestInformation = db.StoreRequestInformation.Find(id);
            if (StoreRequestInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Reterive Store Request Information";
                return RedirectToAction("StoreRequestionList");
            }
            List<StoreRequstVMForError> ErrorList = new List<StoreRequstVMForError>();
            bool pass = true;
            foreach (StoreRequest storeRequest in StoreRequestInformation.StoreRequest)
            {
                Stock stock = (from s in db.Stocks where s.ItemID == storeRequest.ItemID && s.StoreID == StoreRequestInformation.StoreID select s).First();
                if (stock == null)
                {
                    StoreRequstVMForError storeRequstVMForError = new StoreRequstVMForError()
                    {
                        StoreRequest = storeRequest,
                        Error = "Unable to load store Information Posible reason is no information registed about the item"
                    }; pass = false;
                    ViewBag.errorMessage = "Some error found. see error detail for more information";
                    ErrorList.Add(storeRequstVMForError);
                }
                else if (stock.Total < storeRequest.Quantity)
                {
                    StoreRequstVMForError storeRequstVMError = new StoreRequstVMForError()
                    {
                        StoreRequest = storeRequest,
                        Error = "The Avaliable stock in " + StoreRequestInformation.Store.Name + "store is less than requested Quantity" + stock.Total
                    };
                    pass = false;
                    ViewBag.errorMessage = "Some error found. see error in detail for more information";
                    ErrorList.Add(storeRequstVMError);
                }
            }
            if (pass)
            {
                foreach (StoreRequest storeRequest in StoreRequestInformation.StoreRequest)
                {
                    Stock stock = (from s in db.Stocks where s.ItemID == storeRequest.ItemID && s.StoreID == StoreRequestInformation.StoreID select s).First();
                    stock.Total = stock.Total;
                    db.Entry(stock).State = EntityState.Modified;
                    db.SaveChanges();
                    AvaliableOnStock avaliableOnStock = db.AvaliableOnStocks.Find(storeRequest.ItemID);
                    Item i = db.Items.Find(avaliableOnStock.ID);
                    avaliableOnStock.RecentlyReduced = 0;
                    avaliableOnStock.Avaliable = stock.Total;
                    db.Entry(avaliableOnStock).State = EntityState.Modified;
                    db.SaveChanges();
                }
                StoreRequestInformation.Status = "Rejected";
                StoreRequestInformation.ApprovedBy = User.Identity.GetUserName();
                db.Entry(StoreRequestInformation).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.succsessMessage = "Store Request is Rejected!!.";
            }

            else
            {
                ViewBag.erroList = ErrorList;
            }
            return View("RowIssueDetial", StoreRequestInformation);
        }

   

        //to check avaliable on stock 
        float AvalableQuantity(int item)
        {
            RowMaterialRepositery avalable = db.RowMaterialRepositeries.Find(item);
            if (avalable != null)
            {
                return avalable.RowMaterialAavliable;
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