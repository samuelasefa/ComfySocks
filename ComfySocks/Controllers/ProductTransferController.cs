using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Order;
using ComfySocks.Models.ProductTransferInfo;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    public class ProductTransferController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ProductTransfer
        [Authorize(Roles ="Super Admin, Admin, Production")]
        public ActionResult ProductTransferList()
        {
            //errormessage display
            if (TempData[User.Identity.GetUserId()+"errorMessage"]!= null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            var transfers = (from transfer in db.TransferInformation orderby transfer.ID descending select transfer).ToList();

            return View(transfers);
        }

        // controller of Finshed product transfer
        [Authorize(Roles = "Super Admin, Admin, Production")]
        public ActionResult NewTransferEntry(int id = 0)
        {
            //errormessage display
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            
            if (id != 0)
            {
                List<Transfer> transfers = new List<Transfer>();
                if (TempData[User.Identity.GetUserId() + "Transfers"] != null)
                {
                    transfers = (List<Transfer>)TempData[User.Identity.GetUserId() + "Transfers"];
                    TempData[User.Identity.GetUserId() + "Transfers"] = null;
                }
                ViewBag.transfers = transfers;
            }
            else {
                TempData[User.Identity.GetUserId() + "Transfers"] = null;
            }
            ViewBag.transfers = null;
            ViewBag.TempProductStockID = new SelectList(db.TempProductStocks, "ID", "ProductName");
            return View();
        }

        //post method  for transfering product
        [HttpPost]
        public ActionResult NewTransferEntry(Transfer transfer)
        {
          
            //errormessage display
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }

           

            List<Transfer> transfers = new List<Transfer>();
            //foreach (Transfer items in transfers) {

            //    TempProductStock ps = db.TempProductStocks.Find(items.TempProductStock.Quantity);
            //    ProductionOrder order = db.ProductionOrders.Find(items.ProductionOrder.Quantity);

              
            //}
            
            if (TempData[User.Identity.GetUserId() + "Transfers"] != null)
            {
                transfers = (List<Transfer>)TempData[User.Identity.GetUserId() + "Transfers"];
                TempData[User.Identity.GetUserId() + "Transfers"] = null;
            }
            transfer.TransferInformationID = 1;
            bool found = false;
            if (ModelState.IsValid)
            {
                AvaliableOnStock av = db.AvaliableOnStocks.Find(transfer.TempProductStockID);

                if (av == null)
                {
                    ViewBag.errorMessage = "Low Stock! Only 0 item avaliable So please Add item first";
                }
                else
                {
                    double selectedQuantity = 0;
                    foreach (Transfer pt in transfers)
                    {
                        if (pt.TempProductStockID == transfer.TempProductStockID)
                        {

                            selectedQuantity += pt.Quantity;
                        }
                    }
                    if (av.Avaliable > selectedQuantity + transfer.Quantity)
                    {
                        foreach (var i in transfers)
                        {
                            if (i.TempProductStockID == transfer.TempProductStockID)
                            {
                                i.Quantity += transfer.Quantity;
                                ViewBag.succsessMessage = "Item is succsessfully Added";
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            transfers.Add(transfer);
                        }
                    }
                    else
                    {
                        ViewBag.errorMessage = "Low Stock! Only" + av.Avaliable + "Item is available please add other Item";
                    }

                }
            }   
            else {
                ViewBag.errorMessage = "State is Not Valid";
            }
            if (transfers.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.transfers = transfers;
            TempData[User.Identity.GetUserId() + "Transfers"] = transfers;
            ViewBag.TempProductStockID = new SelectList(db.TempProductStocks, "ID", "ProductName");
            return View();
        }

        public ActionResult RemoveSelected(int id)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId()+"Transfers"]==null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find selected Item try agian!";
                return RedirectToAction("NewTransferEntry");
            }
            List<Transfer> transfers = new List<Transfer>();
            transfers = (List<Transfer>)TempData[User.Identity.GetUserId() + "Transfers"];

            foreach (var i in transfers)
            {
                if (i.ID == id)
                {
                    transfers.Remove(i);
                    ViewBag.succsessMessage = "ProductTransfering Item is removed";
                    break;
                }
            }
            if (transfers.Count > 0)
                ViewBag.haveItem = true;
            ViewBag.transfers = transfers;
            TempData[User.Identity.GetUserId() + "Transfers"] = transfers;
            ViewBag.TempProductStockID = new SelectList(db.TempProductStocks, "ID", "ProductName");
            ViewBag.errorMesage = "You process can't be performed for now try agin";
            return View("NewTransferEntry");
        }

        public ActionResult NewTransferInfo()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId() + "Transfers"] == null) { 
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract Transfer information";
                return RedirectToAction("NewTransferEntry");
            }

            return View();
        }


        [HttpPost]
        public ActionResult NewTransferInfo(TransferInformation transferInformation)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId() + "Transfers"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract Transfer information";
                return RedirectToAction("NewTransferEntry");
            }

            List<Transfer> transfers = new List<Transfer>();
            
            if (TempData[User.Identity.GetUserId() + "Transfers"] != null) {
                transfers = (List<Transfer>)TempData[User.Identity.GetUserId() + "Transfer"];
            }
            else
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected Store Order";
                return RedirectToAction("NewTransferEntry");
            ;}
            transferInformation.ApplicationUserID = User.Identity.GetUserId();
            try
            {
                int LastId = (from ti in db.TransferInformation orderby ti.ID descending select ti.ID).First();
                transferInformation.FPTNo = "FPT.No:-" + (LastId + 1);
            }
            catch
            {
                transferInformation.FPTNo = "FTP.No-1";
            }
            transferInformation.Date = DateTime.Now;
            bool pass = true;

            if (ModelState.IsValid)
            {
                try
                {
                    transferInformation.Status = "Transfering";
                    db.TransferInformation.Add(transferInformation);
                    db.SaveChanges();
                    pass = true;
                }
                catch (Exception e)
                {
                    ViewBag.errorMessage = "Unable to Preform the Requste you need! View Error detial" + e;
                    pass = false;
                }
                //if (pass)
                //{
                //    try
                //    {
                //        foreach (Transfer t in transfers)
                //        {
                //            t = storeRequestInfo.ID;
                //            db.StoreRequest.Add(sr.StoreRequest);
                //            db.SaveChanges();
                //        }
                //        ViewBag.succsessMessage = "Store Request is Succesfully Submited!!";
                //        return RedirectToAction("StoreRequestDetial", new { id = storeRequestInfo.ID });
                //    }
                //    catch (Exception e)
                //    {
                //        ViewBag.errorMessage = "Unable to preform the request you need View error detail" + e;
                //    }
                //}
            }
            return View();
        }


        [Authorize(Roles ="Super Admin, Admin, Production")]
        public ActionResult TransferDetail(int id = 0)
        {
            //errormessage display
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }

            if (id == 0) {
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


    }
}