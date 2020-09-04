using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace aaatemp_blazorwebassembly.Shared.Models
{
    public class NavBarMenuItem
    {
        [Key]
        public int MenuId { get; set; }
        //public string MenuID { get; set; }
        public string MenuDisplayName { get; set; }
        public int ParentMenuId { get; set; }
        public string UserPolicy { get; set; }
        //public string MenuFileName { get; set; }
        public string MenuURL { get; set; }
        //public string USE_YN { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ItemOrder { get; set; }

        [NotMapped]
        public List<NavBarMenuItem> ChildItems { get; set; }
    }
}
