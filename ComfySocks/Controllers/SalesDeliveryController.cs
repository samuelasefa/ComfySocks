using ComfySocks.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    //public class SalesDeliveryController : Controller
    //{
    //    private Models.ApplicationDbContext db = new ApplicationDbContext();
    //    // GET: RowMaterialDelivery
    //    [Authorize(Roles = "Super Admin, Admin")]

    //    public ActionResult SaleDeliveryList(int id)
    //    {
    //        TempData[User.Identity.GetUserId() + "selectedDelivery"] = null;
    //        if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
    //        if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
    //        ViewBag.ID = id;

    //        TempData[User.Identity.GetUserId() + "RequestID"] = id;
    //        var RowMaterialDeliverd = (from rd in db.DeliveryInformation where rd.StoreRequestInfoID == id orderby rd.ID descending select rd).ToList();

    //        return View(RowMaterialDeliverd);
    //    }


    //    [Authorize(Roles = "Super Admin, Admin")]

    //    public ActionResult NewRowMaterialDelivery(int? id)
    //    {
    //        if (id == null)
    //        {
    //            TempData[User.Identity.GetUserId() + "errorMessage"] = "Inavalid Navigation is detected";
    //            return RedirectToAction("RowMaterialDeliveryList");
    //        }
    //        StoreRequestInformation storeRequestInfo = db.StoreRequestInformation.Find(id);

    //        if (storeRequestInfo == null)
    //        {
    //            TempData[User.Identity.GetUserId() + "errorMessage"] = "Inavalid Navigation is detected";
    //            return RedirectToAction("RowMaterialDeliveryList");
    //        }
    //        ViewBag.StoreRequestID = (from sr in db.StoreRequest where sr.StoreRequestInformationID == id select sr);
    //        ViewBag.id = id;
    //        TempData[User.Identity.GetUserId() + "RequestInfoID"] = id;
    //        return View();
    //    }

    //    [Authorize(Roles = "Super Admin, Admin")]

    //    [HttpPost]
    //    public ActionResult NewSalesDelivery(Delivery delivery)
    //    {
    //        if (TempData[User.Identity.GetUserId() + "RequestInfoID"] == null)
    //        {
    //            TempData[User.Identity.GetUserId() + "errorMessage"] = "Request is Time out Please try again!!";
    //            TempData[User.Identity.GetUserId() + "selectedDelivery"] = null;
    //            return RedirectToAction("RowMaterialDeliveryList", "storerequestinformation", null);
    //        }
    //        int id = (int)TempData[User.Identity.GetUserId() + "RequestInfoID"];
    //        TempData[User.Identity.GetUserId() + "RequestInfoID"] = id;
    //        ViewBag.id = id;
    //        bool found = false;

    //        List<DeliveryVM> selectedDelivery = new List<DeliveryVM>();
    //        if (TempData[User.Identity.GetUserId() + "selectedDelivery"] != null)
    //        {
    //            selectedDelivery = (List<DeliveryVM>)TempData[User.Identity.GetUserId() + "selectedDelivery"];
    //        }
    //        if (ModelState.IsValid)
    //        {
    //            StoreRequest sr = db.StoreRequest.Find(delivery.StoreRequestID);
    //            if (sr == null)
    //            {
    //                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to lode order information! Try again";
    //                TempData[User.Identity.GetUserId() + "selectedDelivery"] = null;
    //                return RedirectToAction("RowMaterialDeliveryList", new { id = id });
    //            }
    //            foreach (DeliveryVM d in selectedDelivery)
    //            {
    //                if (d.Delivery.StoreRequestID == delivery.StoreRequestID)
    //                {
    //                    if (d.Delivery.Quantity + delivery.Quantity > sr.RemaningDelivery)
    //                    {
    //                        ViewBag.errorMessage = "Only" + sr.RemaningDelivery + "Item remain you can not deliver";
    //                    }
    //                    else
    //                    {
    //                        d.Delivery.Quantity += delivery.Quantity;
    //                        d.Remaining -= (float)delivery.Quantity;
    //                    }
    //                    found = true;
    //                }
    //            }
    //            if (!found)
    //            {
    //                if (sr.RemaningDelivery < delivery.Quantity)
    //                {
    //                    ViewBag.errorMessage = "Only" + sr.RemaningDelivery + "Items remain you can not add" + delivery.Quantity + "items for delivery";
    //                    ViewBag.haveItem = false;
    //                }
    //                else
    //                {
    //                    DeliveryVM deliveryVM = new DeliveryVM();
    //                    Item I = db.Items.Find(sr.ItemID);
    //                    StoreRequest srs = db.StoreRequest.Find(sr.ID);
    //                    deliveryVM.ItemDescription = I.Name;
    //                    deliveryVM.Itemtype = I.ItemType.Name;
    //                    deliveryVM.ItemCode = I.Code;
    //                    deliveryVM.Unit = I.Unit.Name;
    //                    deliveryVM.Remark = srs.Remark;
    //                    deliveryVM.Delivery = delivery;
    //                    deliveryVM.Remaining = sr.RemaningDelivery - (float)delivery.Quantity;
    //                    selectedDelivery.Add(deliveryVM);
    //                }

    //            }
    //        }
    //        ViewBag.selectedDeliver = selectedDelivery;
    //        TempData[User.Identity.GetUserId() + "selectedDelivery"] = selectedDelivery;
    //        if (selectedDelivery.Count > 0)
    //        {
    //            ViewBag.haveItem = true;
    //        }
    //        ViewBag.StoreRequestID = (from d in db.StoreRequest where d.StoreRequestInformationID == id select d).ToList();
    //        return View(delivery);
    //    }

    //    [Authorize(Roles = "Super Admin, Admin")]

    //    public ActionResult SalesDeliveryDetail(int? id)
    //    {
    //        if (id == 0)
    //        {
    //            TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected!!";
    //            return RedirectToAction("RowMaterialDeliveryList");
    //        }
    //        DeliveryInformation deliveryInformation = db.DeliveryInformation.Find(id);
    //        if (deliveryInformation == null)
    //        {
    //            TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load selected delivery list";
    //            return RedirectToAction("RowMaterialDeliveryList");
    //        }
    //        return View(deliveryInformation);
    //    }

    //    [Authorize(Roles = "Super Admin, Admin")]

    //    public ActionResult SalesDeliveryInfo()
    //    {
    //        if (TempData[User.Identity.GetUserId() + "selectedDelivery"] == null)
    //        {
    //            TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load selected Delivery";
    //            return RedirectToAction("NewMaterialDelivery");
    //        }
    //        if (TempData[User.Identity.GetUserId() + "RequestInfoID"] == null)
    //        {
    //            TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out! try agian";
    //            return RedirectToAction("RowMaterialDeliveryList", "storerequestinformations", null);
    //        }
    //        int RequestId = (int)TempData[User.Identity.GetUserId() + "RequestInfoID"];
    //        TempData[User.Identity.GetUserId() + "RequestInfoID"] = RequestId;
    //        TempData[User.Identity.GetUserId() + "selectedDelivery"] = TempData[User.Identity.GetUserId() + "selectedDelivery"];
    //        return View();
    //    }

    //    [Authorize(Roles = "Super Admin, Admin")]

    //    [System.Web.Mvc.HttpPost]
    //    public ActionResult SalesDeliveryInfo(DeliveryInformation deliveryInformation)
    //    {
    //        if (TempData[User.Identity.GetUserId() + "selectedDelivery"] == null)
    //        {
    //            TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load selected Delivery";
    //            return RedirectToAction("RowMaterialDeliveryList");
    //        }
    //        if (TempData[User.Identity.GetUserId() + "RequestInfoID"] == null)
    //        {
    //            TempData[User.Identity.GetUserId() + "errorMessage"] = "Request time out! try agian";
    //            return RedirectToAction("RowMaterialDeliveryList", "storerequestinformations", null);
    //        }
    //        deliveryInformation.ApplictionUserID = User.Identity.GetUserId();

    //        int RequestId = (int)TempData[User.Identity.GetUserId() + "RequestInfoID"];
    //        TempData[User.Identity.GetUserId() + "RequestInfoID"] = RequestId;
    //        List<DeliveryVM> deliverylist = (List<DeliveryVM>)TempData[User.Identity.GetUserId() + "selectedDelivery"];
    //        TempData[User.Identity.GetUserId() + "selectedDelivery"] = deliverylist;

    //        if (deliveryInformation.Date > DateTime.Now)
    //        {
    //            ViewBag.errorMessage = "The date information you provide is not valid";
    //            return View(deliveryInformation);
    //        }
    //        DeliveryVM d = deliverylist.FirstOrDefault();

    //        try
    //        {
    //            DeliveryInformation lastDeliverd = (from del in db.DeliveryInformation orderby del.ID descending select del).First();

    //            deliveryInformation.StoreRequestInfoID = RequestId;
    //            deliveryInformation.DeliveryNumber = "De-" + lastDeliverd.ID + 1;
    //        }
    //        catch
    //        {
    //            deliveryInformation.StoreRequestInfoID = RequestId;
    //            deliveryInformation.DeliveryNumber = "De-" + 1;
    //            deliveryInformation.ApplictionUserID = User.Identity.GetUserId();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                deliveryInformation.Status = "Submmited";
    //                db.DeliveryInformation.Add(deliveryInformation);
    //                db.SaveChanges();
    //                foreach (DeliveryVM de in deliverylist)
    //                {
    //                    de.Delivery.DeliveryInformationID = deliveryInformation.ID;
    //                    db.Deliveries.Add(de.Delivery);
    //                    db.SaveChanges();
    //                    StoreRequest sr = db.StoreRequest.Find(de.Delivery.StoreRequestID);
    //                    sr.RemaningDelivery = de.Remaining;
    //                    db.Entry(sr).State = EntityState.Modified;
    //                    db.SaveChanges();
    //                }
    //                TempData[User.Identity.GetUserId() + "succsessMessage"] = "Delivery Information reqisterd!!";
    //                return RedirectToAction("RowMaterialDeliveryList", "RowMaterialDelivery", new { id = deliveryInformation.ID });

    //            }

    //            catch (Exception e)
    //            {
    //                ViewBag.errorMessage = e;
    //            }

    //        }

    //        return View(deliveryInformation);
    //    }


    //    public ActionResult SalesApproved(int? id)
    //    {
    //        if (id == null)
    //        {
    //            TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected!. Try again";
    //            return RedirectToAction("RowMaterialDeliveryList");
    //        }
    //        DeliveryInformation deliveryInformation = db.DeliveryInformation.Find(id);
    //        if (deliveryInformation == null)
    //        {
    //            TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Reterive Store Request Information";
    //            return RedirectToAction("RowMaterialDeliveryList");
    //        }
    //        List<DeliveryVMForError> ErrorList = new List<DeliveryVMForError>();
    //        bool pass = true;
    //        foreach (Delivery delivery in deliveryInformation.Deliveries)
    //        {
    //            Stock stock = (from s in db.Stocks where s.ItemID == delivery.StoreRequest.ItemID select s).First();
    //            if (stock == null)
    //            {
    //                DeliveryVMForError deliveryVMForError = new DeliveryVMForError()
    //                {
    //                    Delivery = delivery,
    //                    Error = "Unable to load store Information Posible reason is no information registed about the item"
    //                }; pass = false;
    //                ViewBag.errorMessage = "Some error found. see error detail for more information";
    //                ErrorList.Add(deliveryVMForError);
    //            }
    //            else if (stock.Total < delivery.Quantity)
    //            {
    //                DeliveryVMForError deliveryVMForError = new DeliveryVMForError()
    //                {
    //                    Delivery = delivery,
    //                    Error = "The Avaliable stock in store is less than requested Quantity" + stock.Total
    //                };
    //                pass = false;
    //                ViewBag.errorMessage = "Some error found2. see error in detail for more information";
    //                ErrorList.Add(deliveryVMForError);
    //            }
    //        }
    //        if (pass)
    //        {
    //            foreach (Delivery delivery in deliveryInformation.Deliveries)
    //            {
    //                Stock stock = (from s in db.Stocks where s.ItemID == delivery.StoreRequest.ItemID select s).First();
    //                stock.Total -= (float)delivery.StoreRequest.Quantity;
    //                db.Entry(stock).State = EntityState.Modified;
    //                db.SaveChanges();
    //                RowMaterialRepositery repositery = db.RowMaterialRepositeries.Find(delivery.StoreRequest.ItemID);
    //                Item i = db.Items.Find(repositery.ID);
    //                float deference = repositery.RecentlyReducedRowMaterialAvaliable - delivery.Quantity;

    //                if (deference > 0)
    //                {
    //                    repositery.RecentlyReducedRowMaterialAvaliable -= (float)delivery.Quantity;
    //                }
    //                else
    //                {
    //                    repositery.RecentlyReducedRowMaterialAvaliable = 0;
    //                    repositery.RowMaterialAavliable += deference;

    //                }
    //                db.Entry(repositery).State = EntityState.Modified;
    //                db.SaveChanges();
    //            }
    //            deliveryInformation.Status = "Approved";
    //            deliveryInformation.Approvedby = User.Identity.GetUserName();
    //            db.Entry(deliveryInformation).State = EntityState.Modified;
    //            db.SaveChanges();
    //            ViewBag.succsessMessage = "Delivery is approved!!.";
    //        }

    //        else
    //        {
    //            ViewBag.erroList = ErrorList;
    //        }
    //        return View("RowMaterialDeliveryDetail", deliveryInformation);
    //    }
    //    //rejecting delivery

    //    public ActionResult SalesRejected(int? id)
    //    {
    //        if (id == null)
    //        {
    //            TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation detected!. Try again";
    //            return RedirectToAction("StoreRequestionList");
    //        }
    //        DeliveryInformation deliveryInformation = db.DeliveryInformation.Find(id);
    //        if (deliveryInformation == null)
    //        {
    //            TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Reterive Delivery item from database Information";
    //            return RedirectToAction("StoreRequestionList");
    //        }
    //        List<DeliveryVMForError> ErrorList = new List<DeliveryVMForError>();
    //        bool pass = true;
    //        foreach (Delivery delivery in deliveryInformation.Deliveries)
    //        {
    //            Stock stock = (from s in db.Stocks where s.ItemID == delivery.StoreRequest.ItemID select s).First();
    //            if (stock == null)
    //            {
    //                DeliveryVMForError storeRequstVMForError = new DeliveryVMForError()
    //                {
    //                    Delivery = delivery,
    //                    Error = "Unable to load store Information Posible reason is no information registed about the item"
    //                }; pass = false;
    //                ViewBag.errorMessage = "Some error found. see error detail for more information";
    //                ErrorList.Add(storeRequstVMForError);
    //            }
    //            else if (stock.Total < delivery.StoreRequest.Quantity)
    //            {
    //                DeliveryVMForError deliveryVMForError = new DeliveryVMForError()
    //                {
    //                    Delivery = delivery,
    //                    Error = "The Avaliable stock in store is less than requested Quantity" + stock.Total
    //                };
    //                pass = false;
    //                ViewBag.errorMessage = "Some error found. see error in detail for more information";
    //                ErrorList.Add(deliveryVMForError);
    //            }
    //        }
    //        if (pass)
    //        {
    //            foreach (Delivery delivery in deliveryInformation.Deliveries)
    //            {
    //                Stock stock = (from s in db.Stocks where s.ItemID == delivery.StoreRequest.ItemID select s).First();
    //                stock.Total = stock.Total;
    //                db.Entry(stock).State = EntityState.Modified;
    //                db.SaveChanges();
    //                AvaliableOnStock avaliableOnStock = db.AvaliableOnStocks.Find(delivery.StoreRequest.ItemID);
    //                Item i = db.Items.Find(avaliableOnStock.ID);
    //                avaliableOnStock.RecentlyReduced = 0;
    //                avaliableOnStock.Avaliable = stock.Total;
    //                db.Entry(avaliableOnStock).State = EntityState.Modified;
    //                db.SaveChanges();
    //            }
    //            deliveryInformation.Status = "Rejected";
    //            deliveryInformation.Rejectedby = User.Identity.GetUserName();
    //            db.Entry(deliveryInformation).State = EntityState.Modified;
    //            db.SaveChanges();
    //            ViewBag.succsessMessage = "Delivery Request is Rejected!!.";
    //        }

    //        else
    //        {
    //            ViewBag.erroList = ErrorList;
    //        }
    //        return View("RowMaterialDeliveryDetail", deliveryInformation);
    //    }
    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }
    //}
}

