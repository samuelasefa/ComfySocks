using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.Request;
using ComfySocks.Models.RowIssueInfo;
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
   [Authorize(Roles = "Super Admin, Admin, Store Manager, Finance, Production")]
    public class RowIssueController : Controller  
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Request
        public ActionResult RowIssueList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var RowIsssueInfo = (from issue in db.RowIssueInformation orderby issue.ID ascending select issue).ToList();

            return View(RowIsssueInfo);
        }
        public ActionResult NewRowIssueEntry()
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
        public ActionResult NewRowIssueEntry(RowIssue rowIssue)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<RowIssueVM> selectedRowIssue = new List<RowIssueVM>();
            bool found = false;

            if (TempData[User.Identity.GetUserId() + "selectedRowIssue"] != null)
            {
                selectedRowIssue = (List<RowIssueVM>)TempData[User.Identity.GetUserId() + "selectedRowIssue"];
            }
            rowIssue.RowIssueInformationID = 1;

            if (ModelState.IsValid)
            {
                foreach (RowIssueVM sr in selectedRowIssue)
                {
                    if (sr.RowIssue.ItemID == rowIssue.ItemID)
                    {
                        if (AvalableQuantity(sr.RowIssue.ItemID) > 0 && (sr.RowIssue.Quantity + rowIssue.Quantity) <= AvalableQuantity(sr.RowIssue.ItemID))
                        {
                            sr.RowIssue.Quantity += rowIssue.Quantity;
                            found = true;
                            ViewBag.infoMessage = "Item is Added !!!";
                            break;
                        }
                        else if (AvalableQuantity(sr.RowIssue.ItemID) == -2)
                        {
                            ViewBag.errorMessage = "Unable to load Item Information!.";
                            found = true;
                        }
                        else
                        {
                            ViewBag.errorMessage = "Low Stock previously " + sr.RowIssue.Quantity + " Selected Only " + AvalableQuantity(sr.RowIssue.ItemID) + " avaliable!.";
                            found = true;
                        }
                    }
                }

                if (found == false)
                {
                    if (AvalableQuantity(rowIssue.ItemID) > 0 && (rowIssue.Quantity) <= AvalableQuantity(rowIssue.ItemID))
                    {
                        RowIssueVM rowIssueVM = new RowIssueVM();

                        Item item = db.Items.Find(rowIssue.ItemID);
                        rowIssueVM.ItemDescription = item.Name;
                        rowIssueVM.ItemType = item.ItemType.Name;
                        rowIssueVM.ItemCode = item.Code;
                        rowIssueVM.Unit = item.Unit.Name;
                        rowIssueVM.RowIssue = rowIssue;
                        selectedRowIssue.Add(rowIssueVM);
                        
                    }
                    else if (AvalableQuantity(rowIssue.ItemID) == -2)
                    {
                        ViewBag.errorMessage = "Unable to lode Item Information!.";
                    }
                    else
                    {
                        ViewBag.errorMessage = "Only " + AvalableQuantity(rowIssue.ItemID) + " avalable!.";
                    }
                }
            }
            else
            {
                ViewBag.errorMessage = "State is not valid";
            }
            TempData[User.Identity.GetUserId() + "selectedRowIssue"] = selectedRowIssue;
            ViewBag.selectedRowIssue = selectedRowIssue;
            if (selectedRowIssue.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.StockID = (from S in db.Stocks where S.Item.StoreType == StoreType.RowMaterial orderby S.ID descending select S).ToList();

            return View();
        }

        public ActionResult Remove(int id = 0)
        {
            List<RowIssueVM> selectedRowIssue = new List<RowIssueVM>();
            selectedRowIssue = (List<RowIssueVM>)TempData[User.Identity.GetUserId() + "selectedRowIssue"];

            foreach (RowIssueVM s in selectedRowIssue)
            {
                if (s.RowIssue.ID == id)
                {
                }
                selectedRowIssue.Remove(s);
                break;
            }
            TempData[User.Identity.GetUserId() + "selectedRowIssue"] = selectedRowIssue;
            ViewBag.selectedRowIssue = selectedRowIssue;
            if (selectedRowIssue.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.StockID = (from S in db.Stocks where S.Item.StoreType == StoreType.RowMaterial orderby S.ID descending select S).ToList();
            return View("NewRowIssueEntry");
        }
       
        
        /// <summary>
        /// submitteng the requested form to controller and database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>

        public ActionResult NewRowIssueInfo()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            List<RowIssueVM> selectedRowIssue = new List<RowIssueVM>();
            if (TempData[User.Identity.GetUserId() + "selectedRowIssue"] != null)
            {
                selectedRowIssue = (List<RowIssueVM>)TempData[User.Identity.GetUserId() + "selectedRowIssue"];
                TempData[User.Identity.GetUserId() + "selectedRowIssue"] = selectedRowIssue;

            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected store request";
                return RedirectToAction("NewRowIssueEntry");
            }
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult NewRowIssueInfo(RowIssueInformation rowIssueInformation)
        {   
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            List<RowIssueVM> selectedRowIssue = new List<RowIssueVM>();

            if (TempData[User.Identity.GetUserId() + "selectedRowIssue"] != null)
            {
                selectedRowIssue = (List<RowIssueVM>)TempData[User.Identity.GetUserId() + "selectedRowIssue"];
            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected Store Order";
                return RedirectToAction("NewRowIssueEntry");
            }
            rowIssueInformation.ApplicationUserID = User.Identity.GetUserId();
            try
            {
                int LastId = (from sr in db.RowIssueInformation orderby sr.ID descending select sr.ID).First();
                rowIssueInformation.RowIssueNumber = "No:-" + (LastId + 1).ToString("D5");
            }
            catch
            {
                rowIssueInformation.RowIssueNumber = "No:-" + 1.ToString("D5");
            }
            rowIssueInformation.Date = DateTime.Now;
            bool pass = true;
            if (ModelState.IsValid)
            {
                try
                {
                    rowIssueInformation.Status = "Submmited";
                    db.RowIssueInformation.Add(rowIssueInformation);
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
                        foreach (RowIssueVM sr in selectedRowIssue)
                        {
                            sr.RowIssue.RowIssueInformationID = rowIssueInformation.ID;
                            db.RowIssues.Add(sr.RowIssue);
                            db.SaveChanges();
                        }
                        ViewBag.succsessMessage = "Issued is Succesfully Submited!!";
                        return RedirectToAction("RowIssueDetial", new { id = rowIssueInformation.ID });
                    }
                    catch(Exception e)
                    {   
                        ViewBag.errorMessage = "Unable to preform the request you need View error detail" + e;
                    }
                }
            }
            return View();
        }

        public ActionResult RowIssueDetial(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected! Try agian";
                return RedirectToAction("RowIssueList");
            }
            RowIssueInformation rowIssueInformation = db.RowIssueInformation.Find(id);
            if (rowIssueInformation == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Store Request Information";
                return RedirectToAction("RowIssueList");
            }
            return View(rowIssueInformation);
        }


        //Issue is approved 
        public ActionResult IssueApproved(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected!. Try again";
                return RedirectToAction("RowIssueList");
            }
            RowIssueInformation rowIssueInformation = db.RowIssueInformation.Find(id);
            if (rowIssueInformation == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Reterive Store Request Information";
                return RedirectToAction("RowIssueList");
            }
            List<RowIssueVMForError> ErrorList = new List<RowIssueVMForError>();
            bool pass = true;
            foreach (RowIssue rowIssue in rowIssueInformation.RowIssue)
            {
                Stock stock = (from s in db.Stocks where s.ItemID == rowIssue.ItemID && s.StoreID == rowIssueInformation.StoreID select s).First();
                RowMaterialRepositery rowMaterialRepositery = db.RowMaterialRepositeries.Find(stock.ItemID);
                if (stock == null)
                {
                    RowIssueVMForError rowIssueVMForError = new RowIssueVMForError()
                    {
                        RowIssue = rowIssue,
                        Error = "Unable to load store Information Posible reason is no information registed about the item"
                    }; pass = false;
                    ViewBag.errorMessage = "Some error found. see error detail for more information";
                    ErrorList.Add(rowIssueVMForError);
                }
                else if(rowMaterialRepositery.RowMaterialAavliable < rowIssue.Quantity)
                {
                    RowIssueVMForError rowIssueVMForError = new RowIssueVMForError()
                    {
                        RowIssue = rowIssue,
                        Error = "The Avaliable stock in "+ rowIssueInformation.Store.Name + "store is less than requested Quantity" + stock.Total
                    };
                    pass = false;
                    ViewBag.errorMessage = "The Avaliable stock in " + rowIssueInformation.Store.Name + " is less than requested Quantity" + stock.Total;
                    ErrorList.Add(rowIssueVMForError);
                }
            }
            if (pass)
            {
                foreach (RowIssue rowIssue  in rowIssueInformation.RowIssue)
                {
                    Stock stock = (from s in db.Stocks where s.ItemID == rowIssue.ItemID && s.StoreID == rowIssueInformation.StoreID select s).First();
                    //stock.Total -= (float)storeRequest.Quantity;
                    db.Entry(stock).State = EntityState.Modified;
                    db.SaveChanges();
                    RowMaterialRepositery rowMaterialRepositery = db.RowMaterialRepositeries.Find(rowIssue.ItemID);
                    Item i = db.Items.Find(rowMaterialRepositery.ID);
                    float deference = rowMaterialRepositery.RecentlyReducedRowMaterialAvaliable - rowIssue.Quantity;

                    if (deference > 0)
                    {
                        rowMaterialRepositery.RecentlyReducedRowMaterialAvaliable -= (float)rowIssue.Quantity;
                    }
                    else {
                        rowMaterialRepositery.RecentlyReducedRowMaterialAvaliable = 0;
                        rowMaterialRepositery.RowMaterialAavliable += deference;
                        
                    }
                    db.Entry(rowMaterialRepositery).State = EntityState.Modified;
                    db.SaveChanges();
                }
                rowIssueInformation.Status = "Approved";
                rowIssueInformation.ApprovedBy = User.Identity.GetUserName();
                db.Entry(rowIssueInformation).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.succsessMessage = "Row Issue is approved!!.";
            }

            else
            {
                ViewBag.erroList = ErrorList;
            }

            return View("RowIssueDetial", rowIssueInformation);
        }
        //Production order Rejection
        public ActionResult OrderRejected(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("RowIssueList");
            }

            RowIssueInformation rowIssueInformation = db.RowIssueInformation.Find(id);

            if (rowIssueInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Temporary product Information";
                return RedirectToAction("RowIssueList");
            }

            List<RowIssueVMForError> ErrorList = new List<RowIssueVMForError>();
            bool pass = true;
            foreach (RowIssue rowIssue in rowIssueInformation.RowIssue)
            {
                if (rowIssue == null)
                {
                    RowIssueVMForError rowIssueVMForError = new RowIssueVMForError()
                    {
                        RowIssue = rowIssue,
                        Error = "Unable to load Production Order"
                    }; pass = false;

                }
            }
            if (pass)
            {
                foreach (RowIssue rowIssue in rowIssueInformation.RowIssue)
                {
                    rowIssueInformation.Status = "Rejected";
                    db.Entry(rowIssueInformation).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.succsessMessage = "Succesfully Rejected";
                }

            }
            else
            {
                ViewBag.errorList = ErrorList;
            }
            return View("RowIssueDetail", rowIssueInformation);
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