using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.Order;
using ComfySocks.Models.ProductTransferInfo;
using ComfySocks.Models.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    [Authorize(Roles = "Super Admin, Production, Store Manager, Finance, Sales, Admin")]
    public class ProductTransferController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ProductTransfer
        public ActionResult ProductTransferList()
        {
            //errormessage display
            if (TempData[User.Identity.GetUserId()+"errorMessage"]!= null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            var transfers = (from transfer in db.TransferInformation where transfer.Status == "Transfering" || transfer.Status == "Transferd" || transfer.Status =="Rejected" orderby transfer.Date descending select transfer).ToList();

            return View(transfers);
        }

        // controller of Finshed product transfer
        public ActionResult NewTransferEntry(int id = 0)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            //required item
            {
                var item = (from i in db.Items where i.StoreType == StoreType.ProductItem select i).ToList();

                ViewBag.item = "";

                if (item.Count() == 0) {
                    ViewBag.item = "Please Product Item Information Frist";
                    ViewBag.RequiredItems = true;
                }
            }
            ViewBag.TransferID = (from t in db.Items where t.StoreType == StoreType.ProductItem orderby t.ID select t).ToList();
            if (id != 0)
            {
                List<TransferVM> selectedTransfer = new List<TransferVM>();
                selectedTransfer = (List<TransferVM>)TempData[User.Identity.GetUserId() + "selectedTransfer"];
                TempData[User.Identity.GetUserId() + "selectedTransfer"] = selectedTransfer;
                ViewBag.selectedTransfer = selectedTransfer;
            }
            else {
                TempData[User.Identity.GetUserId() + "selectedTransfer"] = null;
            }
            return View();
        }

        //post method  for transfering product
        [System.Web.Mvc.HttpPost]
        public ActionResult NewTransferEntry(Transfer transfer)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<TransferVM> selectedTransfer = new List<TransferVM>();
            bool found = false;
            if (TempData[User.Identity.GetUserId()+ "selectedTransfer"] != null)
            {
                selectedTransfer = (List<TransferVM>)TempData[User.Identity.GetUserId() + "selectedTransfer"];     
            }
            transfer.TransferInformationID = 1;
            transfer.RemaningDelivery = (float)transfer.Quantity;

            var item = db.Transfers.Find(transfer.ItemID);

            if (ModelState.IsValid)
            {

                foreach (TransferVM t in selectedTransfer)
                {
                    if (t.Transfer.ItemID == transfer.ItemID)
                    {
                        t.Transfer.Quantity += transfer.Quantity;
                        found = true;
                        ViewBag.infoMessage = "Product Transfer Item is Added!";
                        break;
                    }
                }
                if (!found)
                {
                    Item I = db.Items.Find(transfer.ItemID);

                    if (I == null)
                    {
                        ViewBag.errorMessage = "Unable to find Item";
                    }
                    else {
                        TransferVM transferVM = new TransferVM();
                        transferVM.TypeOfProduct = I.Name;
                        transferVM.Code = I.Code;
                        transferVM.Unit = I.Unit.Name;
                        transferVM.Transfer = transfer;
                        selectedTransfer.Add(transferVM);
                    }
                }
            }
            else
            {
                ViewBag.errorMessage = "State is not valid";
            }
            ViewBag.selectedTransfer = selectedTransfer;
            TempData[User.Identity.GetUserId() + "selectedTransfer"] = selectedTransfer;
            if (selectedTransfer.Count() > 0 ) {
                ViewBag.haveItem = true;
            }
            ViewBag.TransferID = (from t in db.Items where t.StoreType == StoreType.ProductItem orderby t.ID select t).ToList();
            return View(transfer);
        }

        public ActionResult Remove(int id = 0)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            ViewBag.TransferID = (from t in db.Items where t.StoreType == StoreType.ProductItem orderby t.ID select t).ToList();


            List<TransferVM> selectedTransfer = new List<TransferVM>();
            if (TempData[User.Identity.GetUserId() + "selectedTransfer"] != null)
            {
                try
                {
                    selectedTransfer = (List<TransferVM>)TempData[User.Identity.GetUserId() + "selectedTransfer"];
                    foreach (TransferVM items in selectedTransfer)
                    {
                        if (items.Transfer.ItemID == id)
                        {
                            selectedTransfer.Remove(items);
                            if (selectedTransfer.Count() > 0)
                            {
                                TempData[User.Identity.GetUserId() + "selectedTransfer"] = selectedTransfer;
                                ViewBag.selectedTransfer = selectedTransfer;
                            }
                            return View("NewTransferEntry");
                        }
                    }

                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = e;
                }

            }
            ViewBag.errorMesage = "You process can't be performed for now try again";
            return View("NewTransferEntry");
        }

        public ActionResult NewTransferInfo()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            List<TransferVM> selectedTransfer = new List<TransferVM>();

            if (TempData[User.Identity.GetUserId() + "selectedTransfer"] != null)
            {
                selectedTransfer = (List<TransferVM>)TempData[User.Identity.GetUserId() + "selectedTransfer"];
                TempData[User.Identity.GetUserId() + "selectedTransfer"] = selectedTransfer;
            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected Transfer";
                return RedirectToAction("NewTransferEntry");
            }
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            return View();
        }


        [HttpPost]
        public ActionResult NewTransferInfo(TransferInformation transferInformation)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            List<TransferVM> selectedTransfer = new List<TransferVM>();
            if (TempData[User.Identity.GetUserId() + "selectedTransfer"] != null)
            {
                selectedTransfer = (List<TransferVM>)TempData[User.Identity.GetUserId() + "selectedTransfer"];
            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected Store Transfer";
                return RedirectToAction("NewTransferEntry");
            }
            transferInformation.ApplicationUserID = User.Identity.GetUserId();
            
            try
            {
                int LastId = (from s in db.TransferInformation orderby s.ID descending select s.ID).First();
                transferInformation.FPTNo = "No:-" + (LastId + 1).ToString("D4");
            }
            catch
            {
                transferInformation.FPTNo = "No:-"+ 1.ToString("D4");
            }
            transferInformation.Date = DateTime.Now;
            TransferInformation transferinfo = (TransferInformation)TempData[User.Identity.GetUserId() + "transferinfo"];
            bool pass = true;
            if (ModelState.IsValid)
            {
                try
                {
                    transferInformation.Status = "Transfering";
                    db.TransferInformation.Add(transferInformation);
                    db.SaveChanges();
                    ModelState.Clear();
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
                        foreach (TransferVM s in selectedTransfer)
                        {
                            s.Transfer.TransferInformationID = transferInformation.ID;
                            db.Transfers.Add(s.Transfer);
                            db.SaveChanges();
                            ModelState.Clear();
                            Response.Redirect(Request.Url.ToString(), false);
                        }
                        ViewBag.succsessMessage = "Transfer item is created!.";
                        return RedirectToAction("TransferDetail", new { id = transferInformation.ID });
                    }
                    catch (Exception e)
                    {
                        ViewBag.errorMessage = "Unable to perform the request you need!view error detail" + e;
                    }
                    TransferInformation transfer = (from s in db.TransferInformation where s.FPTNo == transferInformation.FPTNo select s).First();
                    if (transfer != null)
                    {
                        ViewBag.errorMessage = "Duplicated Transfer Number";
                        TempData[User.Identity.GetUserId() + "transferinfo"] = transferinfo;
                        return View();
                    }
                }
            }
            return View();
        }
        public ActionResult TransferDetail(int? id)
        {
            //errormessage display
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }

            if (id == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected!";
                return RedirectToAction("ProductTransferList");
            }
            TransferInformation TI = db.TransferInformation.Find(id);

            if (TI == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected!";
                return RedirectToAction("ProductTransferList");
            }
            return View(TI);
        }
        //Transfer approval
        public ActionResult TransferApproved(int? id)
        {

            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("TransferDetail");
            }

            TransferInformation transferInformation = db.TransferInformation.Find(id);
            if(transferInformation == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Find Product Transfer Information";
                return RedirectToAction("TransferDetail");
            }
            List<TransferViewModelForError> ErrorList = new List<TransferViewModelForError>();
            bool pass = true;

            foreach (Transfer transfer in transferInformation.Transfers) {
                Item items = (from t in db.Items where t.ID == transfer.ItemID select t).First();
                if (items == null)
                {
                    TransferViewModelForError transferViewModelForError = new TransferViewModelForError()
                    {
                        Transfer = transfer,
                        Error = "Unable to load Product Information Posible reason is no information registerd about the item"
                    }; pass = false;
                    ViewBag.errorMessage = "Some error found. see error details for more information";
                    ErrorList.Add(transferViewModelForError);
                }
            }
            if (pass)
            {
                foreach (Transfer transfer in transferInformation.Transfers)
                {
                    Item items= (from t in db.Items where t.ID == transfer.ItemID select t).First();
                    db.Entry(transfer).State = EntityState.Modified;
                    db.SaveChanges();

                    //ProductlogicalAvaliable productlogical = db.ProductlogicalAvaliables.Find(transfer.ItemID);
                    //if (productlogical == null)
                    //{
                    //    productlogical = new ProductlogicalAvaliable()
                    //    {
                    //        ID = transfer.ItemID,
                    //        LogicalProductAvaliable = transfer.Quantity
                    //    };
                    //    db.ProductlogicalAvaliables.Add(productlogical);
                    //    db.SaveChanges();
                    //}
                    //else
                    //{
                    //    productlogical.LogicalProductAvaliable += transfer.Quantity;
                    //    db.Entry(productlogical).State = EntityState.Modified;
                    //    db.SaveChanges();
                    //}
                }
                transferInformation.Status = "Transferd";
                transferInformation.Approvedby = User.Identity.GetUserName();
                db.Entry(transferInformation).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.successMessage = "Successfully transferd!!";
            }
            else {
                ViewBag.errorList = ErrorList;
            }

            return View("TransferDetail", transferInformation);
        }

        //Transfer rejection
        public ActionResult TransferRejected(int? id)
        {

            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("TransferDetail");
            }

            TransferInformation transferInformation = db.TransferInformation.Find(id);
            if (transferInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Find Product Transfer Information";
                return RedirectToAction("TransferDetail");
            }
            List<TransferViewModelForError> ErrorList = new List<TransferViewModelForError>();
            bool pass = true;

            foreach (Transfer transfer in transferInformation.Transfers)
            {
                if (pass)
                {
                    transferInformation.Status = "Rejected";
                    transferInformation.Approvedby = User.Identity.GetUserName();
                    db.Entry(transferInformation).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.successMessage = "Successfully Rejected!!";
                }
                else
                {
                    ViewBag.errorList = ErrorList;
                }
            }
            return View("TransferDetail", transferInformation);
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