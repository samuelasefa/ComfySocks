using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.OfficeIssueInfo;
using ComfySocks.Models.RowIssueInfo;
using ComfySocks.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    [Authorize(Roles = "Super Admin, Admin, Store Manager, Finance, Production")]
    public class OfficeIssueController : Controller  
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Request
        public ActionResult OfficeIssueList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var OfficeIsssueInfo = (from issue in db.OfficeIssueInformation orderby issue.ID ascending select issue).ToList();

            return View(OfficeIsssueInfo);
        }
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
            ViewBag.StockID = (from S in db.Stocks where S.Item.StoreType == StoreType.OfficeMaterial orderby S.ID descending select S).ToList();
                
            return View();
        }

        [HttpPost]
        public ActionResult NewOfficeIssueEntry(OfficeIssue officeIssue)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<OfficeIssueVM> selectedOfficeIssue = new List<OfficeIssueVM>();
            bool found = false;

            if (TempData[User.Identity.GetUserId() + "selectedOfficeIssue"] != null)
            {
                selectedOfficeIssue = (List<OfficeIssueVM>)TempData[User.Identity.GetUserId() + "selectedRowIssue"];
            }
            officeIssue.OfficeIssueInformationID = 1;

            if (ModelState.IsValid)
            {
                foreach (OfficeIssueVM oi in selectedOfficeIssue)
                {
                    if (oi.OfficeIssue.ItemID == officeIssue.ItemID)
                    {
                        if (AvalableQuantity(oi.OfficeIssue.ItemID) > 0 && (oi.OfficeIssue.Quantity + officeIssue.Quantity) <= AvalableQuantity(oi.OfficeIssue.ItemID))
                        {
                            oi.OfficeIssue.Quantity += officeIssue.Quantity;
                            found = true;
                            ViewBag.infoMessage = "Item is Added !!!";
                            break;
                        }
                        else if (AvalableQuantity(oi.OfficeIssue.ItemID) == -2)
                        {
                            ViewBag.errorMessage = "Unable to load Item Information!.";
                            found = true;
                        }
                        else
                        {
                            ViewBag.errorMessage = "Low Stock previously " + oi.OfficeIssue.Quantity + " Selected Only " + AvalableQuantity(oi.OfficeIssue.ItemID) + " avaliable!.";
                            found = true;
                        }
                    }
                }

                if (found == false)
                {
                    if (AvalableQuantity(officeIssue.ItemID) > 0 && (officeIssue.Quantity) <= AvalableQuantity(officeIssue.ItemID))
                    {
                        OfficeIssueVM officeIssueVM = new OfficeIssueVM();

                        Item item = db.Items.Find(officeIssue.ItemID);
                        officeIssueVM.ItemDescription = item.Name;
                        officeIssueVM.ItemType = item.ItemType.Name;
                        officeIssueVM.ItemCode = item.Code;
                        officeIssueVM.Unit = item.Unit.Name;
                        officeIssueVM.OfficeIssue = officeIssue;
                        selectedOfficeIssue.Add(officeIssueVM);
                        
                    }
                    else if (AvalableQuantity(officeIssue.ItemID) == -2)
                    {
                        ViewBag.errorMessage = "Unable to lode Item Information!.";
                    }
                    else
                    {
                        ViewBag.errorMessage = "Only " + AvalableQuantity(officeIssue.ItemID) + " avalable!.";
                    }
                }
            }
            else
            {
                ViewBag.errorMessage = "State is not valid";
            }
            TempData[User.Identity.GetUserId() + "selectedOfficeIssue"] = selectedOfficeIssue;
            ViewBag.selectedOfficeIssue = selectedOfficeIssue;
            if (selectedOfficeIssue.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.StockID = (from S in db.Stocks where S.Item.StoreType == StoreType.OfficeMaterial orderby S.ID descending select S).ToList();

            return View();
        }

        public ActionResult Remove(int id = 0)
        {
            List<OfficeIssueVM> selectedOfficeIssue = new List<OfficeIssueVM>();
            selectedOfficeIssue = (List<OfficeIssueVM>)TempData[User.Identity.GetUserId() + "selectedOfficeIssue"];

            foreach (OfficeIssueVM s in selectedOfficeIssue)
            {
                if (s.OfficeIssue.ID == id)
                {
                }
                selectedOfficeIssue.Remove(s);
                break;
            }
            TempData[User.Identity.GetUserId() + "selectedOfficeIssue"] = selectedOfficeIssue;
            ViewBag.selectedOfficeIssue = selectedOfficeIssue;
            if (selectedOfficeIssue.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.StockID = (from S in db.Stocks where S.Item.StoreType == StoreType.OfficeMaterial orderby S.ID descending select S).ToList();
            return View("NewOfficeIssueEntry");
        }
       
        
        /// <summary>
        /// submitteng the requested form to controller and database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>

        public ActionResult NewOfficeIssueInfo()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            List<OfficeIssueVM> selectedOfficeIssue = new List<OfficeIssueVM>();
            if (TempData[User.Identity.GetUserId() + "selectedOfficeIssue"] != null)
            {
                selectedOfficeIssue = (List<OfficeIssueVM>)TempData[User.Identity.GetUserId() + "selectedOfficeIssue"];
                TempData[User.Identity.GetUserId() + "selectedOfficeIssue"] = selectedOfficeIssue;
            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected store request";
                return RedirectToAction("NewOfficeIssueEntry");
            }
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult NewOfficeIssueInfo(OfficeIssueInformation officeIssueInformation)
        {   
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            List<OfficeIssueVM> selectedOfficeIssue = new List<OfficeIssueVM>();

            if (TempData[User.Identity.GetUserId() + "selectedOfficeIssue"] != null)
            {
                selectedOfficeIssue = (List<OfficeIssueVM>)TempData[User.Identity.GetUserId() + "selectedOfficeIssue"];
            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected Store Order";
                return RedirectToAction("NewOfficeIssueEntry");
            }
            officeIssueInformation.ApplicationUserID = User.Identity.GetUserId();
            try
            {
                int LastId = (from sr in db.OfficeIssueInformation orderby sr.ID descending select sr.ID).First();
                officeIssueInformation.OfficeIssueNumber = "No:-" + (LastId + 1).ToString("D5");
            }
            catch
            {
                officeIssueInformation.OfficeIssueNumber = "No-" + 1.ToString("D5");
            }
            officeIssueInformation.Date = DateTime.Now;
            bool pass = true;
            if (ModelState.IsValid)
            {
                try
                {
                    officeIssueInformation.Status = "Submmited";
                    db.OfficeIssueInformation.Add(officeIssueInformation);
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
                        foreach (OfficeIssueVM oi in selectedOfficeIssue)
                        {
                            oi.OfficeIssue.OfficeIssueInformationID = officeIssueInformation.ID;
                            db.OfficeIssues.Add(oi.OfficeIssue);
                            db.SaveChanges();
                        }
                        ViewBag.succsessMessage = "Issued is Succesfully Submited!!";
                        return RedirectToAction("OfficeIssueDetial", new { id = officeIssueInformation.ID });
                    }
                    catch(Exception e)
                    {   
                        ViewBag.errorMessage = "Unable to preform the request you need View error detail" + e;
                    }
                }
            }
            return View();
        }

        public ActionResult OfficeIssueDetial(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected! Try agian";
                return RedirectToAction("OfficeIssueList");
            }
            OfficeIssueInformation officeIssueInformation = db.OfficeIssueInformation.Find(id);
            if (officeIssueInformation == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Store Request Information";
                return RedirectToAction("RowIssueList");
            }
            return View(officeIssueInformation);
        }


        //Issue is approved 
        public ActionResult OfficeIssueApproved(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected!. Try again";
                return RedirectToAction("RowIssueList");
            }
            OfficeIssueInformation officeIssueInformation = db.OfficeIssueInformation.Find(id);
            if (officeIssueInformation == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Reterive Store Request Information";
                return RedirectToAction("OfficeIssueList");
            }
            List<OfficeIssueVMForError> ErrorList = new List<OfficeIssueVMForError>();
            bool pass = true;
            foreach (OfficeIssue officeIssue in officeIssueInformation.OfficeIssue)
            {
                Stock stock = (from s in db.Stocks where s.ItemID == officeIssue.ItemID select s).First();
                RowMaterialRepositery rowMaterialRepositery = db.RowMaterialRepositeries.Find(stock.ItemID);
                if (stock == null)
                {
                    OfficeIssueVMForError officeIssueVMForError = new OfficeIssueVMForError()
                    {
                        OfficeIssue = officeIssue,
                        Error = "Unable to load store Information Posible reason is no information registed about the item"
                    }; pass = false;
                    ViewBag.errorMessage = "Some error found. see error detail for more information";
                    ErrorList.Add(officeIssueVMForError);
                }
                else if(rowMaterialRepositery.RowMaterialAavliable < officeIssue.Quantity)
                {
                    OfficeIssueVMForError officeIssueVMForError = new OfficeIssueVMForError()
                    {
                        OfficeIssue = officeIssue,
                        Error = "The Avaliable stock in "+ officeIssueInformation.Store.Name + "store is less than requested Quantity" + stock.Total
                    };
                    pass = false;
                    ViewBag.errorMessage = "The Avaliable stock in " + officeIssueInformation.Store.Name + " is less than requested Quantity" + stock.Total;
                    ErrorList.Add(officeIssueVMForError);
                }
            }
            if (pass)
            {
                foreach (OfficeIssue officeIssue  in officeIssueInformation.OfficeIssue)
                {
                    Stock stock = (from s in db.Stocks where s.ItemID == officeIssue.ItemID select s).First();
                    //stock.Total -= (float)storeRequest.Quantity;
                    db.Entry(stock).State = EntityState.Modified;
                    db.SaveChanges();
                    RowMaterialRepositery rowMaterialRepositery = db.RowMaterialRepositeries.Find(officeIssue.ItemID);
                    Item i = db.Items.Find(rowMaterialRepositery.ID);
                    float deference = rowMaterialRepositery.RecentlyReducedRowMaterialAvaliable - officeIssue.Quantity;

                    if (deference > 0)
                    {
                        rowMaterialRepositery.RecentlyReducedRowMaterialAvaliable -= (float)officeIssue.Quantity;
                    }
                    else {
                        rowMaterialRepositery.RecentlyReducedRowMaterialAvaliable = 0;
                        rowMaterialRepositery.RowMaterialAavliable += deference;
                        
                    }
                    db.Entry(rowMaterialRepositery).State = EntityState.Modified;
                    db.SaveChanges();
                }
                officeIssueInformation.Status = "Approved";
                officeIssueInformation.ApprovedBy = User.Identity.GetUserName();
                db.Entry(officeIssueInformation).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.succsessMessage = "Row Issue is approved!!.";
            }

            else
            {
                ViewBag.erroList = ErrorList;
            }

            return View("OfficeIssueDetial", officeIssueInformation);
        }
        //Production order Rejection
        public ActionResult OfficeIssueRejected(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("OfficeIssueList");
            }

            OfficeIssueInformation officeIssueInformation = db.OfficeIssueInformation.Find(id);

            if (officeIssueInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Temporary product Information";
                return RedirectToAction("OfficeIssueList");
            }

            List<OfficeIssueVMForError> ErrorList = new List<OfficeIssueVMForError>();
            bool pass = true;
            foreach (OfficeIssue officeIssue in officeIssueInformation.OfficeIssue)
            {
                if (officeIssue == null)
                {
                    OfficeIssueVMForError officeIssueVMForError = new OfficeIssueVMForError()
                    {
                        OfficeIssue = officeIssue,
                        Error = "Unable to load Production Order"
                    }; pass = false;

                }
            }
            if (pass)
            {
                foreach (OfficeIssue officeIssue in officeIssueInformation.OfficeIssue)
                {
                    officeIssueInformation.Status = "Rejected";
                    db.Entry(officeIssueInformation).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.succsessMessage = "Succesfully Rejected";
                }

            }
            else
            {
                ViewBag.errorList = ErrorList;
            }
            return View("OfficeIssueDetail", officeIssueInformation);
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