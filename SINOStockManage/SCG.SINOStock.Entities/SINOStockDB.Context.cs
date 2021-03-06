﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SINOStockDBEntities : DbContext
    {
        public SINOStockDBEntities()
            : base("name=SINOStockDBEntities")
        {
    		this.Configuration.ProxyCreationEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Account> Accounts { get; set; }
        public DbSet<FormWork> FormWorks { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<FunctionDetail> FunctionDetails { get; set; }
        public DbSet<QualityInfo> QualityInfoes { get; set; }
        public DbSet<Remove_StockBox> Remove_StockBox { get; set; }
        public DbSet<Remove_Tray> Remove_Tray { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RuleMapFunction> RuleMapFunctions { get; set; }
        public DbSet<StockBox> StockBoxes { get; set; }
        public DbSet<StockDetail> StockDetails { get; set; }
        public DbSet<StockLot> StockLots { get; set; }
        public DbSet<StockLotOut> StockLotOuts { get; set; }
        public DbSet<StockOutDetail> StockOutDetails { get; set; }
        public DbSet<StockProDic> StockProDics { get; set; }
        public DbSet<Tray> Trays { get; set; }
    }
}
