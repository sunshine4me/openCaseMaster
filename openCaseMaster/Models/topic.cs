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
    
    public partial class topic
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public topic()
        {
            this.M_publicTask = new HashSet<M_publicTask>();
            this.topicReply = new HashSet<topicReply>();
        }
    
        public int ID { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public int userID { get; set; }
        public int node { get; set; }
        public Nullable<int> state { get; set; }
        public System.DateTime creatDate { get; set; }
    
        public virtual admin_user admin_user { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_publicTask> M_publicTask { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<topicReply> topicReply { get; set; }
    }
}
