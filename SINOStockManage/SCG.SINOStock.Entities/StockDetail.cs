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
    public partial class StockDetail
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string GlassID { get; set; }
        [DataMember]
        public int Qty { get; set; }
        [DataMember]
        public System.DateTime CreateDt { get; set; }
        [DataMember]
        public int StockLotID { get; set; }
        [DataMember]
        public int AccountID { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public bool IsPaoGuang { get; set; }
        [DataMember]
        public Nullable<System.DateTime> StockInDT { get; set; }
        [DataMember]
        public string JianBaoNum { get; set; }
        [DataMember]
        public Nullable<System.DateTime> JianBaoDT { get; set; }
        [DataMember]
        public string PaoguangType { get; set; }
        [DataMember]
        public string PaoGuangMian { get; set; }
        [DataMember]
        public string PaoGuangNum { get; set; }
        [DataMember]
        public Nullable<System.DateTime> PaoGuangDT { get; set; }
        [DataMember]
        public string DuMoNum { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DuMoDT { get; set; }
        [DataMember]
        public Nullable<int> StockBoxID { get; set; }
        [DataMember]
        public string StockInInfo { get; set; }
        [DataMember]
        public string JianBaoInfo { get; set; }
        [DataMember]
        public string JianBaoImgInfo { get; set; }
        [DataMember]
        public string DuMoInfo { get; set; }
        [DataMember]
        public string DuMoImgInfo { get; set; }
        [DataMember]
        public string PaoGuangInfo { get; set; }
        [DataMember]
        public string PaoGuangImgInfo { get; set; }
        [DataMember]
        public bool IsHOLD { get; set; }
        [DataMember]
        public int FanGongNum { get; set; }
        [DataMember]
        public bool IsFanGong { get; set; }
        [DataMember]
        public Nullable<int> JianBaoAccountID { get; set; }
        [DataMember]
        public string JianBaoAccountName { get; set; }
        [DataMember]
        public Nullable<int> PaoGuangAccountID { get; set; }
        [DataMember]
        public string PaoGuangAccountName { get; set; }
        [DataMember]
        public Nullable<int> DuMoAccountID { get; set; }
        [DataMember]
        public string DuMoAccountName { get; set; }
        [DataMember]
        public string IsPaoGuangOverInfo { get; set; }
        [DataMember]
        public bool IsTuiHuo { get; set; }
        [DataMember]
        public string StockInImgInfo { get; set; }
    
        [DataMember]
        public virtual StockBox StockBox { get; set; }
        [DataMember]
        public virtual StockLot StockLot { get; set; }
    }
}
