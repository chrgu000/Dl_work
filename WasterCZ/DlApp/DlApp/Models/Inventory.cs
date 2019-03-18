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
    
    public partial class Inventory
    {
        public string cInvCode { get; set; }
        public string cInvAddCode { get; set; }
        public string cInvName { get; set; }
        public string cInvStd { get; set; }
        public string cInvCCode { get; set; }
        public string cVenCode { get; set; }
        public string cReplaceItem { get; set; }
        public string cPosition { get; set; }
        public bool bSale { get; set; }
        public bool bPurchase { get; set; }
        public bool bSelf { get; set; }
        public bool bComsume { get; set; }
        public bool bProducing { get; set; }
        public bool bService { get; set; }
        public bool bAccessary { get; set; }
        public Nullable<double> iTaxRate { get; set; }
        public Nullable<double> iInvWeight { get; set; }
        public Nullable<double> iVolume { get; set; }
        public Nullable<double> iInvRCost { get; set; }
        public Nullable<double> iInvSPrice { get; set; }
        public Nullable<double> iInvSCost { get; set; }
        public Nullable<double> iInvLSCost { get; set; }
        public Nullable<double> iInvNCost { get; set; }
        public Nullable<double> iInvAdvance { get; set; }
        public Nullable<double> iInvBatch { get; set; }
        public Nullable<double> iSafeNum { get; set; }
        public Nullable<double> iTopSum { get; set; }
        public Nullable<double> iLowSum { get; set; }
        public Nullable<double> iOverStock { get; set; }
        public string cInvABC { get; set; }
        public bool bInvQuality { get; set; }
        public bool bInvBatch { get; set; }
        public bool bInvEntrust { get; set; }
        public bool bInvOverStock { get; set; }
        public Nullable<System.DateTime> dSDate { get; set; }
        public Nullable<System.DateTime> dEDate { get; set; }
        public bool bFree1 { get; set; }
        public bool bFree2 { get; set; }
        public string cInvDefine1 { get; set; }
        public string cInvDefine2 { get; set; }
        public string cInvDefine3 { get; set; }
        public int I_id { get; set; }
        public Nullable<bool> bInvType { get; set; }
        public Nullable<double> iInvMPCost { get; set; }
        public string cQuality { get; set; }
        public Nullable<double> iInvSaleCost { get; set; }
        public Nullable<double> iInvSCost1 { get; set; }
        public Nullable<double> iInvSCost2 { get; set; }
        public Nullable<double> iInvSCost3 { get; set; }
        public bool bFree3 { get; set; }
        public bool bFree4 { get; set; }
        public bool bFree5 { get; set; }
        public bool bFree6 { get; set; }
        public bool bFree7 { get; set; }
        public bool bFree8 { get; set; }
        public bool bFree9 { get; set; }
        public bool bFree10 { get; set; }
        public string cCreatePerson { get; set; }
        public string cModifyPerson { get; set; }
        public Nullable<System.DateTime> dModifyDate { get; set; }
        public Nullable<double> fSubscribePoint { get; set; }
        public Nullable<double> fVagQuantity { get; set; }
        public string cValueType { get; set; }
        public bool bFixExch { get; set; }
        public Nullable<double> fOutExcess { get; set; }
        public Nullable<double> fInExcess { get; set; }
        public Nullable<short> iMassDate { get; set; }
        public Nullable<short> iWarnDays { get; set; }
        public Nullable<double> fExpensesExch { get; set; }
        public bool bTrack { get; set; }
        public bool bSerial { get; set; }
        public bool bBarCode { get; set; }
        public Nullable<int> iId { get; set; }
        public string cBarCode { get; set; }
        public string cInvDefine4 { get; set; }
        public string cInvDefine5 { get; set; }
        public string cInvDefine6 { get; set; }
        public string cInvDefine7 { get; set; }
        public string cInvDefine8 { get; set; }
        public string cInvDefine9 { get; set; }
        public string cInvDefine10 { get; set; }
        public Nullable<int> cInvDefine11 { get; set; }
        public Nullable<int> cInvDefine12 { get; set; }
        public Nullable<double> cInvDefine13 { get; set; }
        public Nullable<double> cInvDefine14 { get; set; }
        public Nullable<System.DateTime> cInvDefine15 { get; set; }
        public Nullable<System.DateTime> cInvDefine16 { get; set; }
        public byte iGroupType { get; set; }
        public string cGroupCode { get; set; }
        public string cComUnitCode { get; set; }
        public string cAssComUnitCode { get; set; }
        public string cSAComUnitCode { get; set; }
        public string cPUComUnitCode { get; set; }
        public string cSTComUnitCode { get; set; }
        public string cCAComUnitCode { get; set; }
        public string cFrequency { get; set; }
        public Nullable<short> iFrequency { get; set; }
        public Nullable<short> iDays { get; set; }
        public Nullable<System.DateTime> dLastDate { get; set; }
        public Nullable<double> iWastage { get; set; }
        public bool bSolitude { get; set; }
        public string cEnterprise { get; set; }
        public string cAddress { get; set; }
        public string cFile { get; set; }
        public string cLabel { get; set; }
        public string cCheckOut { get; set; }
        public string cLicence { get; set; }
        public bool bSpecialties { get; set; }
        public string cDefWareHouse { get; set; }
        public Nullable<double> iHighPrice { get; set; }
        public Nullable<double> iExpSaleRate { get; set; }
        public string cPriceGroup { get; set; }
        public string cOfferGrade { get; set; }
        public Nullable<double> iOfferRate { get; set; }
        public string cMonth { get; set; }
        public Nullable<short> iAdvanceDate { get; set; }
        public string cCurrencyName { get; set; }
        public string cProduceAddress { get; set; }
        public string cProduceNation { get; set; }
        public string cRegisterNo { get; set; }
        public string cEnterNo { get; set; }
        public string cPackingType { get; set; }
        public string cEnglishName { get; set; }
        public bool bPropertyCheck { get; set; }
        public string cPreparationType { get; set; }
        public string cCommodity { get; set; }
        public byte iRecipeBatch { get; set; }
        public string cNotPatentName { get; set; }
        public byte[] pubufts { get; set; }
        public bool bPromotSales { get; set; }
        public Nullable<short> iPlanPolicy { get; set; }
        public Nullable<short> iROPMethod { get; set; }
        public Nullable<short> iBatchRule { get; set; }
        public Nullable<double> fBatchIncrement { get; set; }
        public Nullable<int> iAssureProvideDays { get; set; }
        public Nullable<short> iTestStyle { get; set; }
        public Nullable<short> iDTMethod { get; set; }
        public Nullable<double> fDTRate { get; set; }
        public Nullable<double> fDTNum { get; set; }
        public string cDTUnit { get; set; }
        public Nullable<short> iDTStyle { get; set; }
        public Nullable<int> iQTMethod { get; set; }
        public Nullable<System.Guid> PictureGUID { get; set; }
        public bool bPlanInv { get; set; }
        public bool bProxyForeign { get; set; }
        public bool bATOModel { get; set; }
        public bool bCheckItem { get; set; }
        public bool bPTOModel { get; set; }
        public bool bEquipment { get; set; }
        public string cProductUnit { get; set; }
        public Nullable<double> fOrderUpLimit { get; set; }
        public Nullable<short> cMassUnit { get; set; }
        public Nullable<double> fRetailPrice { get; set; }
        public string cInvDepCode { get; set; }
        public Nullable<int> iAlterAdvance { get; set; }
        public Nullable<double> fAlterBaseNum { get; set; }
        public string cPlanMethod { get; set; }
        public bool bMPS { get; set; }
        public bool bROP { get; set; }
        public bool bRePlan { get; set; }
        public string cSRPolicy { get; set; }
        public bool bBillUnite { get; set; }
        public Nullable<int> iSupplyDay { get; set; }
        public Nullable<double> fSupplyMulti { get; set; }
        public Nullable<double> fMinSupply { get; set; }
        public bool bCutMantissa { get; set; }
        public string cInvPersonCode { get; set; }
        public Nullable<int> iInvTfId { get; set; }
        public string cEngineerFigNo { get; set; }
        public bool bInTotalCost { get; set; }
        public short iSupplyType { get; set; }
        public bool bConfigFree1 { get; set; }
        public bool bConfigFree2 { get; set; }
        public bool bConfigFree3 { get; set; }
        public bool bConfigFree4 { get; set; }
        public bool bConfigFree5 { get; set; }
        public bool bConfigFree6 { get; set; }
        public bool bConfigFree7 { get; set; }
        public bool bConfigFree8 { get; set; }
        public bool bConfigFree9 { get; set; }
        public bool bConfigFree10 { get; set; }
        public Nullable<short> iDTLevel { get; set; }
        public string cDTAQL { get; set; }
        public bool bPeriodDT { get; set; }
        public string cDTPeriod { get; set; }
        public Nullable<int> iBigMonth { get; set; }
        public Nullable<int> iBigDay { get; set; }
        public Nullable<int> iSmallMonth { get; set; }
        public Nullable<int> iSmallDay { get; set; }
        public bool bOutInvDT { get; set; }
        public bool bBackInvDT { get; set; }
        public Nullable<short> iEndDTStyle { get; set; }
        public Nullable<bool> bDTWarnInv { get; set; }
        public Nullable<double> fBackTaxRate { get; set; }
        public string cCIQCode { get; set; }
        public string cWGroupCode { get; set; }
        public string cWUnit { get; set; }
        public Nullable<double> fGrossW { get; set; }
        public string cVGroupCode { get; set; }
        public string cVUnit { get; set; }
        public Nullable<double> fLength { get; set; }
        public Nullable<double> fWidth { get; set; }
        public Nullable<double> fHeight { get; set; }
        public Nullable<int> iDTUCounter { get; set; }
        public Nullable<int> iDTDCounter { get; set; }
        public Nullable<int> iBatchCounter { get; set; }
        public string cShopUnit { get; set; }
        public string cPurPersonCode { get; set; }
        public bool bImportMedicine { get; set; }
        public bool bFirstBusiMedicine { get; set; }
        public bool bForeExpland { get; set; }
        public string cInvPlanCode { get; set; }
        public double fConvertRate { get; set; }
        public Nullable<System.DateTime> dReplaceDate { get; set; }
        public bool bInvModel { get; set; }
        public bool bKCCutMantissa { get; set; }
        public bool bReceiptByDT { get; set; }
        public Nullable<double> iImpTaxRate { get; set; }
        public Nullable<double> iExpTaxRate { get; set; }
        public bool bExpSale { get; set; }
        public Nullable<double> iDrawBatch { get; set; }
        public bool bCheckBSATP { get; set; }
        public string cInvProjectCode { get; set; }
        public Nullable<short> iTestRule { get; set; }
        public string cRuleCode { get; set; }
        public bool bCheckFree1 { get; set; }
        public bool bCheckFree2 { get; set; }
        public bool bCheckFree3 { get; set; }
        public bool bCheckFree4 { get; set; }
        public bool bCheckFree5 { get; set; }
        public bool bCheckFree6 { get; set; }
        public bool bCheckFree7 { get; set; }
        public bool bCheckFree8 { get; set; }
        public bool bCheckFree9 { get; set; }
        public bool bCheckFree10 { get; set; }
        public bool bBomMain { get; set; }
        public bool bBomSub { get; set; }
        public bool bProductBill { get; set; }
        public short iCheckATP { get; set; }
        public Nullable<int> iInvATPId { get; set; }
        public Nullable<int> iPlanTfDay { get; set; }
        public Nullable<int> iOverlapDay { get; set; }
        public bool bPiece { get; set; }
        public bool bSrvItem { get; set; }
        public bool bSrvFittings { get; set; }
        public Nullable<double> fMaxSupply { get; set; }
        public Nullable<double> fMinSplit { get; set; }
        public bool bSpecialOrder { get; set; }
        public bool bTrackSaleBill { get; set; }
        public string cInvMnemCode { get; set; }
        public Nullable<short> iPlanDefault { get; set; }
        public Nullable<double> iPFBatchQty { get; set; }
        public Nullable<int> iAllocatePrintDgt { get; set; }
        public bool bCheckBatch { get; set; }
        public bool bMngOldpart { get; set; }
        public Nullable<short> iOldpartMngRule { get; set; }
    }
}
