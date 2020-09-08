using ComfySocks.Models.InventoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComfySocks.ViewModel.ForReports
{
    public class PurchaseSearchVM
    {
        public List<StockInformation> StockInformation { get; set; }
        //addded
        public string option { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
    }
}