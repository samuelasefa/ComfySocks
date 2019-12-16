using ComfySocks.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComfySocks.Models;
using ComfySocks.Models.PurchaseModel;

namespace ComfySocks.ViewModel
{
    public class PurchaseEntryVM
    {
        MyContext db = new MyContext();
        public string ID { get; set; }
        public DateTime Date { get; set; }
        public int SupplierID { get; set; }
        public string SupplierInvoiceNo { get; set; }
        public decimal Total { get; set; }
        public decimal? Vat { get; set; }
        public decimal GrandTotal { get; set; }

        public List<PurchaseItem> PurchaseItems { get; set; }
    }
}