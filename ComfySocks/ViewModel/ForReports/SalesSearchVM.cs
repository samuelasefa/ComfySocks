using ComfySocks.Models.SalesInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComfySocks.ViewModel.ForReports
{
    public class SalesSearchVM
    {
        public List<SalesInformation> SalesInformation { get; set; }

        public string option { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
    }
}