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
    public partial class Remove_Tray
    {
        public Remove_Tray()
        {
            this.Remove_StockBox = new HashSet<Remove_StockBox>();
        }
    
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string BarCode { get; set; }
        [DataMember]
        public System.DateTime CreateDt { get; set; }
    
        [DataMember]
        public virtual ICollection<Remove_StockBox> Remove_StockBox { get; set; }
    }
}
