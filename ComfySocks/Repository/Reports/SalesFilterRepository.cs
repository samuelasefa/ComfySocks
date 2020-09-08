using ComfySocks.Models;
using ComfySocks.Models.SalesInfo;
using ComfySocks.ViewModel.ForReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComfySocks.Repository.Reports
{
    public class SalesFilterRepository
    {
        private ApplicationDbContext db;

        public SalesFilterRepository()
        {
            db = new ApplicationDbContext();
        }

        public IQueryable<Sales> FilterSales(SalesSearchVM vm)
        {
            var result = db.Sales.AsQueryable();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (vm != null)
            {
                if (!string.IsNullOrEmpty(vm.option))
                {
                    if (vm.option == "today")
                    {
                        result = result.Where(p => p.SalesInformation.Date.Date == DateTime.Now.Date);
                    }
                    else if (vm.option == "yesterday")
                    {
                        result = result.Where(p => p.SalesInformation.Date.Date == DateTime.Now.Date.AddDays(-1));
                    }
                    else if (vm.option == "thisWeek")
                    {
                        DateTime startDayOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
                        DateTime endDayOfWeek = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

                        result = result.Where(x => x.SalesInformation.Date >= startDayOfWeek && x.SalesInformation.Date <= endDayOfWeek);
                    }
                    else if (vm.option == "thisMonth")
                    {
                        result = result.Where(p => p.SalesInformation.Date.Month == month);
                    }
                    else if (vm.option == "lastMonth")
                    {
                        result = result.Where(p => p.SalesInformation.Date.Month == month - 1);
                    }
                    else if (vm.option == "thisYear")
                    {
                        result = result.Where(p => p.SalesInformation.Date.Year == year);
                    }
                    else if (vm.option == "lastYear")
                    {
                        result = result.Where(p => p.SalesInformation.Date.Year == year - 1);
                    }
                }
                if (vm.fromDate != null || vm.toDate != null)
                {
                    if (vm.fromDate != null && vm.toDate == null)
                    {
                        result = result.Where(p => p.SalesInformation.Date >= vm.fromDate);
                    }
                    else if (vm.fromDate == null && vm.toDate != null)
                    {
                        result = result.Where(p => p.SalesInformation.Date <= vm.fromDate);
                    }
                    else if (vm.fromDate != null && vm.toDate != null)
                    {
                        result = result.Where(p => p.SalesInformation.Date >= vm.fromDate && p.SalesInformation.Date <= vm.toDate);
                    }
                }
            }
            return result;
        }

    }
}