//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DlApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class U8CUSTDEF_0046_E002
    {
        public string cInvCode { get; set; }
        public Nullable<decimal> fQuantity { get; set; }
        public Nullable<decimal> fPrice { get; set; }
        public Nullable<decimal> fAmount { get; set; }
        public string cDetailMemo { get; set; }
        public string cStaffName { get; set; }
        public int U8CUSTDEF_0046_E002_PK { get; set; }
        public Nullable<int> U8CUSTDEF_0046_E001_PK { get; set; }
        public Nullable<int> UAPRuntime_RowNO { get; set; }
        public string UAP_VoucherTransform_Rowkey { get; set; }
        public string RefMainID { get; set; }
        public string RefRowID { get; set; }
    }
}
