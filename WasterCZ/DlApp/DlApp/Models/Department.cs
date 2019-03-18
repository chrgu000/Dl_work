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
    
    public partial class Department
    {
        public Department()
        {
            this.Warehouse = new HashSet<Warehouse>();
            this.QMCHECKVOUCHER = new HashSet<QMCHECKVOUCHER>();
            this.QMCHECKVOUCHER1 = new HashSet<QMCHECKVOUCHER>();
            this.QMINSPECTVOUCHER = new HashSet<QMINSPECTVOUCHER>();
            this.QMINSPECTVOUCHER1 = new HashSet<QMINSPECTVOUCHER>();
            this.QMREJECTVOUCHER = new HashSet<QMREJECTVOUCHER>();
            this.QMREJECTVOUCHERS = new HashSet<QMREJECTVOUCHERS>();
        }
    
        public string cDepCode { get; set; }
        public bool bDepEnd { get; set; }
        public string cDepName { get; set; }
        public byte iDepGrade { get; set; }
        public string cDepPerson { get; set; }
        public string cDepProp { get; set; }
        public string cDepPhone { get; set; }
        public string cDepAddress { get; set; }
        public string cDepMemo { get; set; }
        public Nullable<double> iCreLine { get; set; }
        public string cCreGrade { get; set; }
        public Nullable<int> iCreDate { get; set; }
        public string cOfferGrade { get; set; }
        public Nullable<double> iOfferRate { get; set; }
        public byte[] pubufts { get; set; }
        public bool bShop { get; set; }
        public System.Guid cDepGUID { get; set; }
        public Nullable<System.DateTime> dDepBeginDate { get; set; }
        public Nullable<System.DateTime> dDepEndDate { get; set; }
        public string vAuthorizeDoc { get; set; }
        public string vAuthorizeUnit { get; set; }
        public string cDepFax { get; set; }
        public string cDepPostCode { get; set; }
        public string cDepEmail { get; set; }
        public string cDepType { get; set; }
        public Nullable<int> bInheritDutyBasic { get; set; }
        public Nullable<int> bInheritWorkCalendar { get; set; }
        public string cDutyCode { get; set; }
        public string cRestCode { get; set; }
        public Nullable<bool> bIM { get; set; }
        public string cDepNameEn { get; set; }
        public Nullable<bool> bRetail { get; set; }
        public string cDepFullName { get; set; }
        public Nullable<int> iDepOrder { get; set; }
        public string cDepLeader { get; set; }
        public Nullable<System.DateTime> dModifyDate { get; set; }
        public string cESpaceMembID { get; set; }
    
        public virtual ICollection<Warehouse> Warehouse { get; set; }
        public virtual ICollection<QMCHECKVOUCHER> QMCHECKVOUCHER { get; set; }
        public virtual ICollection<QMCHECKVOUCHER> QMCHECKVOUCHER1 { get; set; }
        public virtual ICollection<QMINSPECTVOUCHER> QMINSPECTVOUCHER { get; set; }
        public virtual ICollection<QMINSPECTVOUCHER> QMINSPECTVOUCHER1 { get; set; }
        public virtual ICollection<QMREJECTVOUCHER> QMREJECTVOUCHER { get; set; }
        public virtual ICollection<QMREJECTVOUCHERS> QMREJECTVOUCHERS { get; set; }
    }
}
