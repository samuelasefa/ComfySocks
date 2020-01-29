using ComfySocks.Models;
using ComfySocks.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult StockReport()
        {
            var Items = db.Items.ToList();
            return View(Items);
        }

        public ActionResult Search(string Key)
        {
            var Items = (from d in db.Items where d.ItemType.Name.Contains(Key) || d.Name.Contains(Key) || d.Unit.Name.Contains(Key) orderby d.ID select d).ToList();
            return View("StockReport", Items);
        }
        // GET: Report
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult SalesReport()
        {
            var sales = (from j in db.Sales select j).First();
            List<SalesReportVM> SalesView = new List<SalesReportVM>();
            bool found = false;
            if (found)
            {
                foreach (SalesReportVM s in SalesView)
                {
                    if (s.ID == sales.ID)
                    {
                        s.UnitPrice = (float)sales.UnitPrice;
                        s.TotalPrice = (float)sales.UnitPrice * sales.Quantity;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    SalesReportVM s = new SalesReportVM()
                    {
                        //ProductName = sales.TempProductStock.Item.Name,
                        //ItemCode = sales.TempProductStock.Item.Code,
                        //Unit = sales.TempProductStock.Item.Unit.Name,
                        //UnitPrice = sales.UnitPrice,
                        TotalPrice = (float)sales.UnitPrice * sales.Quantity,
                    };
                    SalesView.Add(s);
                }
            }
            return View(SalesView);
        }
       
    }
}
    