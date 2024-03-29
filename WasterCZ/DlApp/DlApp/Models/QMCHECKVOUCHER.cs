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
    
    public partial class QMCHECKVOUCHER
    {
        public Nullable<System.Guid> CHECKGUID { get; set; }
        public string CVOUCHTYPE { get; set; }
        public int ID { get; set; }
        public string CCHECKCODE { get; set; }
        public Nullable<int> INSPECTID { get; set; }
        public string CINSPECTCODE { get; set; }
        public Nullable<int> INSPECTAUTOID { get; set; }
        public Nullable<int> SOURCEAUTOID { get; set; }
        public Nullable<int> SOURCEID { get; set; }
        public string SOURCECODE { get; set; }
        public string CSOURCE { get; set; }
        public string CPOCODE { get; set; }
        public string CINSPECTDEPCODE { get; set; }
        public Nullable<System.DateTime> DARRIVALDATE { get; set; }
        public string CINSPECTPERSON { get; set; }
        public string CITEMCLASS { get; set; }
        public string CITEMCODE { get; set; }
        public string CITEMCNAME { get; set; }
        public string CITEMNAME { get; set; }
        public Nullable<System.DateTime> DDATE { get; set; }
        public string CTIME { get; set; }
        public string CDEPCODE { get; set; }
        public string CVENCODE { get; set; }
        public string CINVCODE { get; set; }
        public string CUNITID { get; set; }
        public Nullable<int> ITESTSTYLE { get; set; }
        public Nullable<int> PROJECTID { get; set; }
        public Nullable<int> IDTMETHOD { get; set; }
        public Nullable<int> IDTSTYLE { get; set; }
        public Nullable<decimal> FDTRATE { get; set; }
        public string CCHECKUNIT { get; set; }
        public Nullable<decimal> FCHANGRATE { get; set; }
        public Nullable<decimal> FCHECKRATE { get; set; }
        public Nullable<decimal> FQUANTITY { get; set; }
        public Nullable<decimal> FNUM { get; set; }
        public Nullable<decimal> FDTQUANTITY { get; set; }
        public Nullable<decimal> FDTNUM { get; set; }
        public string CBATCH { get; set; }
        public string CPROBATCH { get; set; }
        public Nullable<System.DateTime> DPRODATE { get; set; }
        public Nullable<System.DateTime> DVDATE { get; set; }
        public string CFREE1 { get; set; }
        public string CFREE2 { get; set; }
        public string CFREE3 { get; set; }
        public string CFREE4 { get; set; }
        public string CFREE5 { get; set; }
        public string CFREE6 { get; set; }
        public string CFREE7 { get; set; }
        public string CFREE8 { get; set; }
        public string CFREE9 { get; set; }
        public string CFREE10 { get; set; }
        public string CDEFINE1 { get; set; }
        public string CDEFINE2 { get; set; }
        public string CDEFINE3 { get; set; }
        public Nullable<System.DateTime> CDEFINE4 { get; set; }
        public Nullable<int> CDEFINE5 { get; set; }
        public Nullable<System.DateTime> CDEFINE6 { get; set; }
        public Nullable<decimal> CDEFINE7 { get; set; }
        public string CDEFINE8 { get; set; }
        public string CDEFINE9 { get; set; }
        public string CDEFINE10 { get; set; }
        public string CDEFINE11 { get; set; }
        public string CDEFINE12 { get; set; }
        public string CDEFINE13 { get; set; }
        public string CDEFINE14 { get; set; }
        public Nullable<int> CDEFINE15 { get; set; }
        public Nullable<decimal> CDEFINE16 { get; set; }
        public Nullable<decimal> FREGBREAKQUANTITY { get; set; }
        public Nullable<decimal> FREGBREAKNUM { get; set; }
        public Nullable<decimal> FREGUBREAKQUANTITY { get; set; }
        public Nullable<decimal> FREGUBREAKNUM { get; set; }
        public Nullable<decimal> FDISBREAKQUANTITY { get; set; }
        public Nullable<decimal> FDISBREAKNUM { get; set; }
        public Nullable<decimal> FDISUBREAKQUANTITY { get; set; }
        public Nullable<decimal> FDISUBREAKNUM { get; set; }
        public Nullable<decimal> FREGQUANTITY { get; set; }
        public Nullable<decimal> FREGNUM { get; set; }
        public Nullable<decimal> FCONQUANTIY { get; set; }
        public Nullable<decimal> FCONNUM { get; set; }
        public Nullable<decimal> FDISQUANTITY { get; set; }
        public Nullable<decimal> FDISNUM { get; set; }
        public string CCHECKPERSONCODE { get; set; }
        public string CMAKER { get; set; }
        public string CVERIFIER { get; set; }
        public byte[] UFTS { get; set; }
        public Nullable<int> IVTID { get; set; }
        public Nullable<bool> BPUINFLAG { get; set; }
        public Nullable<bool> BPROINFLAG { get; set; }
        public Nullable<bool> BREJFLAG { get; set; }
        public string CWHCODE { get; set; }
        public Nullable<bool> BSTNEXTYEAR { get; set; }
        public Nullable<int> IORDERTYPE { get; set; }
        public string ISOORDERAUTOID { get; set; }
        public Nullable<int> IPROORDERID { get; set; }
        public Nullable<int> IPROORDERAUTOID { get; set; }
        public string CCUSCODE { get; set; }
        public string CCONTRACTCODE { get; set; }
        public string CPOSITION { get; set; }
        public Nullable<decimal> FISCOST { get; set; }
        public Nullable<decimal> IAC { get; set; }
        public Nullable<decimal> IRE { get; set; }
        public string CINSLEVEL { get; set; }
        public Nullable<bool> BEXIGENCY { get; set; }
        public string CREASONCODE { get; set; }
        public string CYIELDERCODE { get; set; }
        public Nullable<System.DateTime> DYIELDDATE { get; set; }
        public string CMEMO { get; set; }
        public string CPROCESSAUTOID { get; set; }
        public Nullable<int> IWORKCENTER { get; set; }
        public string CCONTRACTSTRCODE { get; set; }
        public string CBYPRODUCT { get; set; }
        public string CCHECKTYPECODE { get; set; }
        public string FAQL { get; set; }
        public string CCOSTTYPE { get; set; }
        public Nullable<decimal> FCOST { get; set; }
        public string CMASSUNIT { get; set; }
        public Nullable<short> IMASSDATE { get; set; }
        public string CSOORDERCODE { get; set; }
        public string CPROORDERCODE { get; set; }
        public Nullable<int> IVERIFYSTATE { get; set; }
        public Nullable<int> IDISBREAKQTYDEALTYPE { get; set; }
        public Nullable<int> IBATCHCHKRESULT { get; set; }
        public Nullable<decimal> FsumQuantity { get; set; }
        public Nullable<decimal> FsumNum { get; set; }
        public string ITESTRULE { get; set; }
        public string CRULECODE { get; set; }
        public string PcsTransType { get; set; }
        public string CVMIVENCODE { get; set; }
        public int iReturnCount { get; set; }
        public int iVerifyStateNew { get; set; }
        public int IsWfControlled { get; set; }
        public string CMODIFIER { get; set; }
        public Nullable<decimal> FCHECKCHANGRATE { get; set; }
        public Nullable<decimal> FCHECKQTY { get; set; }
        public Nullable<decimal> FCHECKNUM { get; set; }
        public Nullable<int> IFREGBREAKQTYDEALTYPE { get; set; }
        public Nullable<System.DateTime> DMODIFYDATE { get; set; }
        public Nullable<System.DateTime> DVERIFYDATE { get; set; }
        public Nullable<System.DateTime> DMAKETIME { get; set; }
        public Nullable<System.DateTime> DMODIFYTIME { get; set; }
        public Nullable<System.DateTime> DVERIFYTIME { get; set; }
        public string CSOURCEPROORDERCODE { get; set; }
        public Nullable<int> ISOURCEPROORDERROWNO { get; set; }
        public Nullable<int> ISOURCEPROORDERID { get; set; }
        public Nullable<int> ISOURCEPROORDERAUTOID { get; set; }
        public Nullable<short> iExpiratDateCalcu { get; set; }
        public string cExpirationdate { get; set; }
        public Nullable<System.DateTime> dExpirationdate { get; set; }
        public Nullable<decimal> cBatchProperty1 { get; set; }
        public Nullable<decimal> cBatchProperty2 { get; set; }
        public Nullable<decimal> cBatchProperty3 { get; set; }
        public Nullable<decimal> cBatchProperty4 { get; set; }
        public Nullable<decimal> cBatchProperty5 { get; set; }
        public string cBatchProperty6 { get; set; }
        public string cBatchProperty7 { get; set; }
        public string cBatchProperty8 { get; set; }
        public string cBatchProperty9 { get; set; }
        public Nullable<System.DateTime> cBatchProperty10 { get; set; }
        public Nullable<int> IORDERDID { get; set; }
        public Nullable<byte> ISOORDERTYPE { get; set; }
        public string CORDERCODE { get; set; }
        public Nullable<int> IORDERSEQ { get; set; }
        public string CCHKNORMALCODE { get; set; }
        public Nullable<bool> BRANDORPROJECT { get; set; }
        public string CRETURNREASONCODE { get; set; }
        public string CCHKCONCLUSION { get; set; }
        public string CCHANGER { get; set; }
        public Nullable<System.DateTime> DCHANGEDATE { get; set; }
        public Nullable<System.DateTime> DCHANGETIME { get; set; }
        public Nullable<System.DateTime> DINSPECTDATE { get; set; }
        public string CINSPECTTIME { get; set; }
        public Nullable<System.DateTime> DCOMPLETEDATE { get; set; }
        public Nullable<decimal> FCOMPLETEDAYS { get; set; }
        public Nullable<System.DateTime> DPLANCOMPLETEDATE { get; set; }
        public Nullable<decimal> FPLANCOMPLETEDAYS { get; set; }
        public Nullable<int> iPrintCount { get; set; }
        public string CCLEANVER { get; set; }
        public string PFCODE { get; set; }
        public Nullable<decimal> FSAMPQTY { get; set; }
        public Nullable<decimal> FSAMPQTYINS { get; set; }
        public Nullable<bool> BMERGECHECKFLAG { get; set; }
        public string MERGECHECKRULE { get; set; }
        public Nullable<bool> BWITHINSPECTINFO { get; set; }
        public Nullable<bool> BPRODUCECHECKDETAIL { get; set; }
        public string CSYSBARCODE { get; set; }
        public string cCurrentAuditor { get; set; }
        public string FCVOUCHERCODE { get; set; }
        public Nullable<int> FCVOUCHERSID { get; set; }
        public string PLANLOTNUMBER { get; set; }
    
        public virtual Reason Reason { get; set; }
        public virtual Department Department { get; set; }
        public virtual Department Department1 { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual sfc_workcenter sfc_workcenter { get; set; }
    }
}
