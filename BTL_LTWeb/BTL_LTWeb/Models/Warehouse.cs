//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BTL_LTWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Warehouse
    {
        public int warehouse_id { get; set; }
        public Nullable<int> product_id { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public Nullable<int> quantity { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
