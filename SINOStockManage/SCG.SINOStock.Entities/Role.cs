//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SCG.SINOStock.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    
    [DataContract(IsReference = true)]
    public partial class Role
    {
        public Role()
        {
            this.Accounts = new HashSet<Account>();
        }
    
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string RuleName { get; set; }
        [DataMember]
        public System.DateTime CreateDt { get; set; }
        [DataMember]
        public Nullable<System.DateTime> ModifyDt { get; set; }
        [DataMember]
        public string RoleMain { get; set; }
        [DataMember]
        public string RoleDetail { get; set; }
    
        [DataMember]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
