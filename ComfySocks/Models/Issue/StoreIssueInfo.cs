using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ComfySocks.Models.Request;

namespace ComfySocks.Models.Issue
{
    [Table("StoreIssueInformation")]
    public partial class StoreIssueInfo
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="Enter issued Date?")]
        [DataType(DataType.Date)]
        public DateTime Date{ get; set; }

        [Display(Name ="Is-No")]
        public int IssueNumber { get; set; }

        [Display(Name = "SR-NO")]
        public int StoreRequstionID { get; set; }


        public string ApplicationUserID { get; set; }


        //reference
        public virtual StoreRequestInfo StoreRequestInfo { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<StoreIssue> StoreIssues { get; set; }
    }
}