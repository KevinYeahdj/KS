using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.WEB.Models
{
    public class PageModels<T>
    {
        public int total { get; set; }
        public int page { get; set; }
        public List<T> rows { get; set; }
    }

    public class ItemContent
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string name { get; set; }
        public bool open { get; set; }
        public bool isChecked { get; set; }
    }

    public class ItemContent2
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string name { get; set; }
        public bool open { get; set; }
        public List<ItemContent2> children { get; set; }
    }
}