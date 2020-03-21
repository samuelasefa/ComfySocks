using ComfySocks.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.Report
{
    public class SalesReportVM
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string ItemCode { get; set; }
        public string Unit  { get; set; }
        public float UnitPrice { get; set; }
        public float TotalPrice { get; set; }
        public float LogicalProductAvalaiableID { get; set; }
        public float LogicalRecentlyReducedQuantityID { get; set; }
        public float PhysicalProductAvalaiableID { get; set; }
        public float PhysicalRecentlyReducedQuantityID { get; set; }
        //referance
        public virtual ProductMaterialRepository ProductMaterialRepositery { get; set; }
    }
}