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
    public partial class StockProDic
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string ProModel { get; set; }
        [DataMember]
        public int PCSQty { get; set; }
        [DataMember]
        public int StockLotID { get; set; }
        [DataMember]
        public System.DateTime CreateDt { get; set; }
    }
}
