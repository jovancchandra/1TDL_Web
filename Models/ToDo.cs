using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1TDL_Web.Models
{
    public class ToDo  //User generated Model Class
    {
        public int Id { get; set; }                         //Entity Framework will automatically detect this as a primary key
        public string Description { get; set; }             //String to describe the task
        public DateTime Due { get; set; }                   //DateTime to represent when the task is due by
        public bool Completed { get; set; }                 //Bool to represent whether the task is completed or not
        public virtual ApplicationUser User { get; set; }   //Foreign key
    }
}