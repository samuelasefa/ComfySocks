using ComfySocks.Models.InventoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComfySocks.ViewModel.ForReports
{
    public partial class StockSearchVM
    {
        public virtual StockInformation StockInformation { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
    }
}