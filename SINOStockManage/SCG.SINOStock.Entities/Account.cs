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
    public partial class Account
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string LoginNumber { get; set; }
        [DataMember]
        public string LoginPwd { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Nullable<System.DateTime> CreateDt { get; set; }
        [DataMember]
        public Nullable<System.DateTime> ModifyDt { get; set; }
        [DataMember]
        public string CheckCode { get; set; }
        [DataMember]
        public int RoleID { get; set; }
        [DataMember]
        public Nullable<System.DateTime> LoginDt { get; set; }
    
        [DataMember]
        public virtual Role Role { get; set; }
    }
}
