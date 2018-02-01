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
    public class PageModels2<T> : PageModels<T>
    {
        public decimal sum { get; set; }
    }
    public class PageModels3<T> : PageModels<T>
    {
        public decimal personalSum { get; set; }
        public decimal companySum { get; set; }
        public decimal totalSum { get; set; }
    }
    public class PageModels4<T> : PageModels<T>
    {
        public decimal sum { get; set; }
        public bool AdvanceFundPay { get; set; }  //用于流水账，返费等对于备用金核销
    }
    public class PageModels5<T> : PageModels<T>
    {
        public decimal sum { get; set; }
        public decimal appAmount { get; set; }  //用于备用金核销时记录流程中当时核销的数字

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