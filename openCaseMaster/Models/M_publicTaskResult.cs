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
    
    public partial class M_publicTaskResult
    {
        public int ID { get; set; }
        public Nullable<int> deviceID { get; set; }
        public string result { get; set; }
        public Nullable<System.DateTime> creatDate { get; set; }
        public Nullable<int> state { get; set; }
    
        public virtual M_deviceConfig M_deviceConfig { get; set; }
    }
}