//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace openCaseMaster.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class admin_log
    {
        public int ID { get; set; }
        public string Weblog { get; set; }
        public Nullable<int> UserCode { get; set; }
        public string Ip { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<System.DateTime> tDate { get; set; }
    
        public virtual admin_user admin_user { get; set; }
    }
}
