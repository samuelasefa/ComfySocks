﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComfySocks.Models.Items
{
    [Table("Unit")]
    public partial class Unit
    {
        public int ID { get; set; }
        [Required]
        [Display(Name="Unit")]
        public string Name { get; set; }

        public string ApplicationUserID { get; set; }

        //ref
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Item> Item { get; set; }
    }
}