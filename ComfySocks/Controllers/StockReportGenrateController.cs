using ComfySocks.Models;
using ComfySocks.Models.GenerateReport;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.Repository;
using ComfySocks.Repository;
using ComfySocks.Repository.Reports;
using ComfySocks.ViewModel.ForReports;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    public class StockReportGenrateController : Controller
    { private ApplicationDbContext db = new ApplicationDbContext();
        // GET: StockReportGenrate
        [Authorize(Roles ="Store Manager, Admin, Super Admin, Sales, Finance, Production")]
        public ActionResult StockReportList()
        {
            return View(db.Stocks.ToList());
        }
        [Authorize(Roles ="Super Admin, Store Manager")]
        public ActionResult NewStockReport(int id = 0)
        {
            if (TempData[User.Identity.GetUserId() + "successMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            {
                var stock = db.Stocks.ToList();
                ViewBag.stock = "";

                if (stock.Count() == 0)
                {
                    ViewBag.stock = "Register Stock Information Frist";
                    ViewBag.RequiredItem = true;
                }
            }
            ViewBag.ItemID = (from S in db.Items where S.StoreType == StoreType.RowMaterial orderby S.ID descending select S).ToList();

            if (id != 0)
            {
                List<ReportVM> selectedReport = new List<ReportVM>();
                selectedReport = (List<ReportVM>)TempData[User.Identity.GetUserId() + "selectedReport"];
                TempData[User.Identity.GetUserId() + "selectedReport"] = selectedReport;
                ViewBag.selectedReport = selectedReport.ToList();
            }
            else
            {
                TempData[User.Identity.GetUserId() + "selectedReport"] = null;
            }
            return View();
        }

        //HTTPPost
        [HttpPost]
        [Authorize(Roles ="Super Admin, Store Manager")]
        public ActionResult NewStockReport(Report report)
        {
            if (TempData[User.Identity.GetUserId() + "successMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<ReportVM> selectedReport = new List<ReportVM>();
            bool found = false;

            if (TempData[User.Identity.GetUserId() + "selectedReport"] != null)
            {
                selectedReport = (List<ReportVM>)TempData[User.Identity.GetUserId() + "selectedReport"];
            }
            report.ReportInformationID = 1;
            if (ModelState.IsValid)
            {
                foreach (ReportVM rp in selectedReport)
                {
                    if (rp.Report.ItemID == report.ItemID)
                    {
                        rp.Report.OnTransit += report.OnTransit;
                        found = true;
                        ViewBag.infoMessage = "Reciving Item is add !!!";
                        ModelState.Clear();
                    }
                }
                if (found == false)
                {
                    AvaliableOnStock avaliable = db.AvaliableOnStocks.Find(report.ItemID);
                        ReportVM reportVM = new ReportVM();
                        Item item = db.Items.Find(report.ItemID);
                        reportVM.ItemDescripton = item.Name;
                        reportVM.ItemType = item.ItemType.Name;
                        reportVM.Code = item.Code;
                    try
                    {
                        reportVM.BiginingBalance = avaliable.Avaliable;
                    }
                    catch {
                    }
                        reportVM.Report = report;
                        selectedReport.Add(reportVM);
                        ModelState.Clear();
                }
            }
            else
            {
                ViewBag.errorMessage = "State is not valid";
            }
            TempData[User.Identity.GetUserId() + "selectedReport"] = selectedReport;
            ViewBag.selectedReport = selectedReport;
            if (selectedReport.Count > 0)
            {
                ViewBag.haveItem = true;
            }

            ViewBag.ItemID = (from S in db.Items where S.StoreType == StoreType.RowMaterial orderby S.ID descending select S).ToList();

            return View();
        }
        [Authorize(Roles ="Super Admin, Store Manager")]
        public ActionResult Remove(int id = 0)
        {
            List<ReportVM> selectedReport = new List<ReportVM>();
            selectedReport = (List<ReportVM>)TempData[User.Identity.GetUserId() + "selectedReport"];

            foreach (ReportVM s in selectedReport)
            {
                if (s.Report.ItemID == id)
                {
                    selectedReport.Remove(s);
                    ViewBag.successMessage = "Report with ItemCode:-" + s.Code + "Removed";
                    break;
                }
               
            }
            TempData[User.Identity.GetUserId() + "selectedReport"] = selectedReport;
            ViewBag.selectedReport = selectedReport;
            if (selectedReport.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.ItemID = (from S in db.Items where S.StoreType == StoreType.RowMaterial orderby S.ID descending select S).ToList();
            return View("NewStockReport");
        }


        //Information
        [Authorize(Roles = "Super Admin, Store Manager")]
        public ActionResult NewStockReportInfo()
        {
            if (TempData[User.Identity.GetUserId() + "successMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<ReportVM> selectedReport = new List<ReportVM>();

            if (TempData[User.Identity.GetUserId() + "selectedReport"] != null)
            {
                selectedReport = (List<ReportVM>)TempData[User.Identity.GetUserId() + "selectedReport"];
                TempData[User.Identity.GetUserId() + "selectedReport"] = selectedReport;
            }
            else{
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to extract selected Report Generation";
                return RedirectToAction("NewStockReport");
            }
            return View();
        }
        
        //post Method
        [HttpPost]
        [Authorize(Roles = "Super Admin, Store Manager")]
        public ActionResult NewStockReportInfo(ReportInformation reportInformation)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "selectedReport"] == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Selected Report Information. try agin";
                return RedirectToAction("NewStockReport");
            }
            List<ReportVM> reports = new List<ReportVM>();
            reports = (List<ReportVM>)TempData[User.Identity.GetUserId() + "selectedReport"];
            TempData[User.Identity.GetUserId() + "selectedReport"] = reports;
            reportInformation.Date = DateTime.Now;
            reportInformation.ApplicationUserID = User.Identity.GetUserId();
            db.ReportInformation.Add(reportInformation);
            db.SaveChanges();

            List<Report> insertedReport = new List<Report>();
            ReportInformation reportInformations = db.ReportInformation.Find(reportInformation.ID);
           
            if (reportInformations != null)
            {
                foreach (ReportVM items in reports)
                {
                    try
                    {   
                        try
                        {
                            List<Report> R = (from i in db.Report where items.Report.ItemID <= i.ID select i).ToList();
                            db.Entry(R).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            ViewBag.errorMessage = "Unable to save Report Info " + e;
                        }

                        Item I = db.Items.Find(items.Report.ItemID);
                        items.Report.ReportInformationID = reportInformation.ID;
                        db.Report.Add(items.Report);
                        db.SaveChanges();

                        LogicalOnTransit transit = db.LogicalOnTransit.Find(items.Report.ItemID);
                        if (transit == null)
                        {
                            transit = new LogicalOnTransit()
                            {
                                ID = items.Report.ItemID,
                                OnTransitAvaliable = items.Report.OnTransit
                            };
                            db.LogicalOnTransit.Add(transit);
                            db.SaveChanges();
                        }
                        else {
                            transit.OnTransitAvaliable += items.Report.OnTransit;
                            db.Entry(transit).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        insertedReport.Add(items.Report);
                    }
                    catch (Exception)
                    {
                        List<Report> report = (from r in db.Report where r.ReportInformationID == reportInformation.ID select r).ToList();
                        foreach (var item in insertedReport)
                        {
                            db.Report.Remove(item);
                            db.SaveChanges();
                            Item I = db.Items.Find(item.ItemID);

                            LogicalOnTransit transit = db.LogicalOnTransit.Find(item.ItemID);
                            transit.OnTransitAvaliable -= items.Report.OnTransit;
                            db.Entry(transit).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        db.ReportInformation.Remove(reportInformation);
                        db.SaveChanges();
                        TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to register OnTransit information";
                        return RedirectToAction("NewStockReport");
                    }
                }
                TempData[User.Identity.GetUserId() + "succsessMessage"] = "Report information is Saved";
                return RedirectToAction("StockReportList");
            }
            else
            {
                ViewBag.errorMessage = "Unable to load Report Information";
            }

            TempData[User.Identity.GetUserId() + "selectedReport"] = TempData[User.Identity.GetUserId() + "selectedReport"];
            return View();
        }
        //Search Stock Report


        [HttpPost]
        public ActionResult StockReportList(StockSearchVM vm)
        {
            var filter = new StockFilterRepository();
            var model = filter.FilterStock(vm);
            return View(model);
        }
        // end of Stock report

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