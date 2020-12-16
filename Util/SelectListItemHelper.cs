using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Util
{
    public class SelectListItemHelper
    {
        public static IEnumerable<SelectListItem> GetShiftTypeList()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "Ca hành chính", Value = "hc"},
                new SelectListItem{Text = "Ca làm thêm", Value = "lt"},
            };
            return items;
        }
    }
}