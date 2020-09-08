using ComfySocks.Models;
using ComfySocks.Models.GenerateReport;
using ComfySocks.Models.InventoryModel;
using ComfySocks.ViewModel.ForReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Repository.Reports
{
    public class StockFilterRepository
    {
        private ApplicationDbContext db;

        public StockFilterRepository()
        {
            db = new ApplicationDbContext();
        }

        public IQueryable<Stock> FilterStock(StockSearchVM vm)
        {
            var result = db.Stocks.AsQueryable();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (vm != null)
            {
                if (vm.fromDate != null || vm.toDate != null)
                {
                    if (vm.fromDate != null && vm.toDate == null)
                    {
                        result = result.Where(p => p.StockInformation.Date >= vm.fromDate);
                    }
                    else if (vm.fromDate == null && vm.toDate != null)
                    {
                        result = result.Where(p => p.StockInformation.Date <= vm.fromDate);
                    }
                    else if (vm.fromDate != null && vm.toDate != null)
                    {
                        result = result.Where(p => p.StockInformation.Date >= vm.fromDate && p.StockInformation.Date <= vm.toDate);
                    }
                }
            }
            return result;
        }
    }
}