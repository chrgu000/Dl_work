﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DlApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UFDATA_009_2015Entities1 : DbContext
    {
        public UFDATA_009_2015Entities1()
            : base("name=UFDATA_009_2015Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DL_U8_Options> DL_U8_Options { get; set; }
        public virtual DbSet<DL_U8_Tare> DL_U8_Tare { get; set; }
        public virtual DbSet<DL_U8_Users> DL_U8_Users { get; set; }
    }
}
