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
    
    public partial class M_testCase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public M_testCase()
        {
            this.M_testCase1 = new HashSet<M_testCase>();
        }
    
        public int ID { get; set; }
        public Nullable<int> projectID { get; set; }
        public string Name { get; set; }
        public string mark { get; set; }
        public string testXML { get; set; }
        public Nullable<int> type { get; set; }
        public Nullable<int> baseID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_testCase> M_testCase1 { get; set; }
        public virtual M_testCase M_testCase2 { get; set; }
    }
}
