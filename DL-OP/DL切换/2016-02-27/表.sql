USE [UFDATA_005_2015]
GO
/****** Object:  Table [dbo].[Dl_opInventory]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opInventory](
	[cInvCode] [nvarchar](60) NOT NULL,
	[cInvAddCode] [nvarchar](255) NULL,
	[cInvName] [nvarchar](255) NULL,
	[cInvStd] [nvarchar](255) NULL,
	[cInvCCode] [nvarchar](12) NULL,
	[cVenCode] [nvarchar](20) NULL,
	[cReplaceItem] [nvarchar](60) NULL,
	[cPosition] [nvarchar](20) NULL,
	[bSale] [bit] NOT NULL,
	[bPurchase] [bit] NOT NULL,
	[bSelf] [bit] NOT NULL,
	[bComsume] [bit] NOT NULL,
	[bProducing] [bit] NOT NULL,
	[bService] [bit] NOT NULL,
	[bAccessary] [bit] NOT NULL,
	[iTaxRate] [float] NULL,
	[iInvWeight] [float] NULL,
	[iVolume] [float] NULL,
	[iInvRCost] [float] NULL,
	[iInvSPrice] [float] NULL,
	[iInvSCost] [float] NULL,
	[iInvLSCost] [float] NULL,
	[iInvNCost] [float] NULL,
	[iInvAdvance] [float] NULL,
	[iInvBatch] [float] NULL,
	[iSafeNum] [float] NULL,
	[iTopSum] [float] NULL,
	[iLowSum] [float] NULL,
	[iOverStock] [float] NULL,
	[cInvABC] [nvarchar](1) NULL,
	[bInvQuality] [bit] NOT NULL,
	[bInvBatch] [bit] NOT NULL,
	[bInvEntrust] [bit] NOT NULL,
	[bInvOverStock] [bit] NOT NULL,
	[dSDate] [smalldatetime] NULL,
	[dEDate] [smalldatetime] NULL,
	[bFree1] [bit] NOT NULL,
	[bFree2] [bit] NOT NULL,
	[cInvDefine1] [nvarchar](20) NULL,
	[cInvDefine2] [nvarchar](20) NULL,
	[cInvDefine3] [nvarchar](20) NULL,
	[I_id] [int] NOT NULL,
	[bInvType] [bit] NULL,
	[iInvMPCost] [float] NULL,
	[cQuality] [nvarchar](100) NULL,
	[iInvSaleCost] [float] NULL,
	[iInvSCost1] [float] NULL,
	[iInvSCost2] [float] NULL,
	[iInvSCost3] [float] NULL,
	[bFree3] [bit] NOT NULL,
	[bFree4] [bit] NOT NULL,
	[bFree5] [bit] NOT NULL,
	[bFree6] [bit] NOT NULL,
	[bFree7] [bit] NOT NULL,
	[bFree8] [bit] NOT NULL,
	[bFree9] [bit] NOT NULL,
	[bFree10] [bit] NOT NULL,
	[cCreatePerson] [nvarchar](20) NULL,
	[cModifyPerson] [nvarchar](20) NULL,
	[dModifyDate] [smalldatetime] NULL,
	[fSubscribePoint] [float] NULL,
	[fVagQuantity] [float] NULL,
	[cValueType] [nvarchar](20) NULL,
	[bFixExch] [bit] NOT NULL,
	[fOutExcess] [float] NULL,
	[fInExcess] [float] NULL,
	[iMassDate] [smallint] NULL,
	[iWarnDays] [smallint] NULL,
	[fExpensesExch] [float] NULL,
	[bTrack] [bit] NOT NULL,
	[bSerial] [bit] NOT NULL,
	[bBarCode] [bit] NOT NULL,
	[iId] [int] NULL,
	[cBarCode] [nvarchar](30) NULL,
	[cInvDefine4] [nvarchar](60) NULL,
	[cInvDefine5] [nvarchar](60) NULL,
	[cInvDefine6] [nvarchar](60) NULL,
	[cInvDefine7] [nvarchar](120) NULL,
	[cInvDefine8] [nvarchar](120) NULL,
	[cInvDefine9] [nvarchar](120) NULL,
	[cInvDefine10] [nvarchar](120) NULL,
	[cInvDefine11] [int] NULL,
	[cInvDefine12] [int] NULL,
	[cInvDefine13] [float] NULL,
	[cInvDefine14] [float] NULL,
	[cInvDefine15] [smalldatetime] NULL,
	[cInvDefine16] [smalldatetime] NULL,
	[iGroupType] [tinyint] NOT NULL,
	[cGroupCode] [nvarchar](35) NULL,
	[cComUnitCode] [nvarchar](35) NULL,
	[cAssComUnitCode] [nvarchar](35) NULL,
	[cSAComUnitCode] [nvarchar](35) NULL,
	[cPUComUnitCode] [nvarchar](35) NULL,
	[cSTComUnitCode] [nvarchar](35) NULL,
	[cCAComUnitCode] [nvarchar](35) NULL,
	[cFrequency] [nvarchar](10) NULL,
	[iFrequency] [smallint] NULL,
	[iDays] [smallint] NULL,
	[dLastDate] [smalldatetime] NULL,
	[iWastage] [float] NULL,
	[bSolitude] [bit] NOT NULL,
	[cEnterprise] [nvarchar](100) NULL,
	[cAddress] [nvarchar](255) NULL,
	[cFile] [nvarchar](40) NULL,
	[cLabel] [nvarchar](30) NULL,
	[cCheckOut] [nvarchar](30) NULL,
	[cLicence] [nvarchar](30) NULL,
	[bSpecialties] [bit] NOT NULL,
	[cDefWareHouse] [nvarchar](10) NULL,
	[iHighPrice] [float] NULL,
	[iExpSaleRate] [float] NULL,
	[cPriceGroup] [nvarchar](20) NULL,
	[cOfferGrade] [nvarchar](20) NULL,
	[iOfferRate] [float] NULL,
	[cMonth] [nvarchar](6) NULL,
	[iAdvanceDate] [smallint] NULL,
	[cCurrencyName] [nvarchar](60) NULL,
	[cProduceAddress] [nvarchar](255) NULL,
	[cProduceNation] [nvarchar](60) NULL,
	[cRegisterNo] [nvarchar](60) NULL,
	[cEnterNo] [nvarchar](60) NULL,
	[cPackingType] [nvarchar](60) NULL,
	[cEnglishName] [nvarchar](100) NULL,
	[bPropertyCheck] [bit] NOT NULL,
	[cPreparationType] [nvarchar](30) NULL,
	[cCommodity] [nvarchar](60) NULL,
	[iRecipeBatch] [tinyint] NOT NULL,
	[cNotPatentName] [nvarchar](30) NULL,
	[pubufts] [varbinary](max) NOT NULL,
	[bPromotSales] [bit] NOT NULL,
	[iPlanPolicy] [smallint] NULL,
	[iROPMethod] [smallint] NULL,
	[iBatchRule] [smallint] NULL,
	[fBatchIncrement] [float] NULL,
	[iAssureProvideDays] [int] NULL,
	[iTestStyle] [smallint] NULL,
	[iDTMethod] [smallint] NULL,
	[fDTRate] [float] NULL,
	[fDTNum] [float] NULL,
	[cDTUnit] [nvarchar](35) NULL,
	[iDTStyle] [smallint] NULL,
	[iQTMethod] [int] NULL,
	[PictureGUID] [uniqueidentifier] NULL,
	[bPlanInv] [bit] NOT NULL,
	[bProxyForeign] [bit] NOT NULL,
	[bATOModel] [bit] NOT NULL,
	[bCheckItem] [bit] NOT NULL,
	[bPTOModel] [bit] NOT NULL,
	[bEquipment] [bit] NOT NULL,
	[cProductUnit] [nvarchar](35) NULL,
	[fOrderUpLimit] [float] NULL,
	[cMassUnit] [smallint] NULL,
	[fRetailPrice] [float] NULL,
	[cInvDepCode] [nvarchar](12) NULL,
	[iAlterAdvance] [int] NULL,
	[fAlterBaseNum] [float] NULL,
	[cPlanMethod] [nvarchar](1) NULL,
	[bMPS] [bit] NOT NULL,
	[bROP] [bit] NOT NULL,
	[bRePlan] [bit] NOT NULL,
	[cSRPolicy] [nvarchar](2) NULL,
	[bBillUnite] [bit] NOT NULL,
	[iSupplyDay] [int] NULL,
	[fSupplyMulti] [float] NULL,
	[fMinSupply] [float] NULL,
	[bCutMantissa] [bit] NOT NULL,
	[cInvPersonCode] [nvarchar](20) NULL,
	[iInvTfId] [int] NULL,
	[cEngineerFigNo] [nvarchar](60) NULL,
	[bInTotalCost] [bit] NOT NULL,
	[iSupplyType] [smallint] NOT NULL,
	[bConfigFree1] [bit] NOT NULL,
	[bConfigFree2] [bit] NOT NULL,
	[bConfigFree3] [bit] NOT NULL,
	[bConfigFree4] [bit] NOT NULL,
	[bConfigFree5] [bit] NOT NULL,
	[bConfigFree6] [bit] NOT NULL,
	[bConfigFree7] [bit] NOT NULL,
	[bConfigFree8] [bit] NOT NULL,
	[bConfigFree9] [bit] NOT NULL,
	[bConfigFree10] [bit] NOT NULL,
	[iDTLevel] [smallint] NULL,
	[cDTAQL] [nvarchar](20) NULL,
	[bPeriodDT] [bit] NOT NULL,
	[cDTPeriod] [nvarchar](30) NULL,
	[iBigMonth] [int] NULL,
	[iBigDay] [int] NULL,
	[iSmallMonth] [int] NULL,
	[iSmallDay] [int] NULL,
	[bOutInvDT] [bit] NOT NULL,
	[bBackInvDT] [bit] NOT NULL,
	[iEndDTStyle] [smallint] NULL,
	[bDTWarnInv] [bit] NULL,
	[fBackTaxRate] [float] NULL,
	[cCIQCode] [nvarchar](30) NULL,
	[cWGroupCode] [nvarchar](35) NULL,
	[cWUnit] [nvarchar](35) NULL,
	[fGrossW] [float] NULL,
	[cVGroupCode] [nvarchar](35) NULL,
	[cVUnit] [nvarchar](35) NULL,
	[fLength] [float] NULL,
	[fWidth] [float] NULL,
	[fHeight] [float] NULL,
	[iDTUCounter] [int] NULL,
	[iDTDCounter] [int] NULL,
	[iBatchCounter] [int] NULL,
	[cShopUnit] [nvarchar](35) NULL,
	[cPurPersonCode] [nvarchar](20) NULL,
	[bImportMedicine] [bit] NOT NULL,
	[bFirstBusiMedicine] [bit] NOT NULL,
	[bForeExpland] [bit] NOT NULL,
	[cInvPlanCode] [nvarchar](20) NULL,
	[fConvertRate] [float] NOT NULL,
	[dReplaceDate] [smalldatetime] NULL,
	[bInvModel] [bit] NOT NULL,
	[bKCCutMantissa] [bit] NOT NULL,
	[bReceiptByDT] [bit] NOT NULL,
	[iImpTaxRate] [float] NULL,
	[iExpTaxRate] [float] NULL,
	[bExpSale] [bit] NOT NULL,
	[iDrawBatch] [float] NULL,
	[bCheckBSATP] [bit] NOT NULL,
	[cInvProjectCode] [nvarchar](16) NULL,
	[iTestRule] [smallint] NULL,
	[cRuleCode] [nvarchar](20) NULL,
	[bCheckFree1] [bit] NOT NULL,
	[bCheckFree2] [bit] NOT NULL,
	[bCheckFree3] [bit] NOT NULL,
	[bCheckFree4] [bit] NOT NULL,
	[bCheckFree5] [bit] NOT NULL,
	[bCheckFree6] [bit] NOT NULL,
	[bCheckFree7] [bit] NOT NULL,
	[bCheckFree8] [bit] NOT NULL,
	[bCheckFree9] [bit] NOT NULL,
	[bCheckFree10] [bit] NOT NULL,
	[bBomMain] [bit] NOT NULL,
	[bBomSub] [bit] NOT NULL,
	[bProductBill] [bit] NOT NULL,
	[iCheckATP] [smallint] NOT NULL,
	[iInvATPId] [int] NULL,
	[iPlanTfDay] [int] NULL,
	[iOverlapDay] [int] NULL,
	[bPiece] [bit] NOT NULL,
	[bSrvItem] [bit] NOT NULL,
	[bSrvFittings] [bit] NOT NULL,
	[fMaxSupply] [float] NULL,
	[fMinSplit] [float] NULL,
	[bSpecialOrder] [bit] NOT NULL,
	[bTrackSaleBill] [bit] NOT NULL,
	[cInvMnemCode] [nvarchar](40) NULL,
	[iPlanDefault] [smallint] NULL,
	[iPFBatchQty] [float] NULL,
	[iAllocatePrintDgt] [int] NULL,
	[bCheckBatch] [bit] NOT NULL,
	[bMngOldpart] [bit] NOT NULL,
	[iOldpartMngRule] [smallint] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opInventoryControl]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opInventoryControl](
	[opInventoryControlId] [int] NOT NULL,
	[cInvCode] [varchar](50) NULL,
	[dblSafeqty] [decimal](18, 6) NULL,
	[datCreatetime] [smalldatetime] NULL,
	[lngCreater] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opOrder]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opOrder](
	[lngopOrderId] [int] IDENTITY(1,1) NOT NULL,
	[lngopUserId] [varchar](50) NULL,
	[strBillNo] [varchar](50) NULL,
	[strBillName] [varchar](50) NULL,
	[datCreateTime] [datetime] NULL,
	[strAuditor] [varchar](50) NULL,
	[datAuditordTime] [datetime] NULL,
	[cSOCode] [varchar](50) NULL,
	[bytStatus] [tinyint] NULL,
	[strRemarks] [varchar](300) NULL,
	[ccuscode] [varchar](50) NULL,
	[cdefine1] [varchar](50) NULL,
	[cdefine2] [varchar](50) NULL,
	[cdefine3] [varchar](50) NULL,
	[cdefine8] [varchar](50) NULL,
	[cdefine9] [varchar](50) NULL,
	[cdefine10] [varchar](50) NULL,
	[cdefine11] [varchar](150) NULL,
	[cdefine12] [varchar](50) NULL,
	[cdefine13] [varchar](50) NULL,
	[ccusname] [varchar](50) NULL,
	[cpersoncode] [varchar](50) NULL,
	[cSCCode] [varchar](50) NULL,
	[datBillTime] [datetime] NULL,
	[strManagers] [varchar](50) NULL,
	[datDeliveryDate] [datetime] NULL,
	[strLoadingWays] [varchar](100) NULL,
	[cSTCode] [varchar](10) NULL,
	[lngopUseraddressId] [varchar](10) NULL,
	[RelateU8NO] [varchar](30) NULL,
	[lngBillType] [int] NULL,
	[datRejectTime] [datetime] NULL,
	[datAffirmTime] [datetime] NULL,
	[InvalidTime] [datetime] NULL,
	[InvalidPersonCode] [varchar](10) NULL,
	[strRejectRemarks] [varchar](500) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opOrderBillNoSetting]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opOrderBillNoSetting](
	[lngOrderBillNoSettingId] [int] IDENTITY(1,1) NOT NULL,
	[lngBillType] [int] NULL,
	[strBillName] [varchar](100) NULL,
	[strPrefix] [varchar](20) NULL,
	[strYear] [varchar](10) NULL,
	[strMonth] [varchar](4) NULL,
	[strSeriaNo] [varchar](20) NULL,
	[lngSerialNoLength] [int] NULL,
	[datCreateDateTime] [datetime] NULL,
 CONSTRAINT [PK_Dl_opOrderBillNoSetting] PRIMARY KEY CLUSTERED 
(
	[lngOrderBillNoSettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opOrderDetail]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opOrderDetail](
	[lngopOrderDetailId] [int] IDENTITY(1,1) NOT NULL,
	[lngopOrderId] [int] NOT NULL,
	[cinvcode] [varchar](50) NULL,
	[iquantity] [decimal](18, 6) NULL,
	[inum] [decimal](18, 6) NULL,
	[iquotedprice] [decimal](18, 6) NULL,
	[iunitprice] [decimal](18, 6) NULL,
	[itaxunitprice] [decimal](18, 6) NULL,
	[imoney] [decimal](18, 6) NULL,
	[itax] [decimal](18, 6) NULL,
	[isum] [decimal](18, 6) NULL,
	[inatunitprice] [decimal](18, 6) NULL,
	[inatmoney] [decimal](18, 6) NULL,
	[inattax] [decimal](18, 6) NULL,
	[inatsum] [decimal](18, 6) NULL,
	[kl] [decimal](18, 6) NULL,
	[itaxrate] [decimal](18, 6) NULL,
	[cdefine22] [varchar](100) NULL,
	[iinvexchrate] [decimal](18, 6) NULL,
	[cunitid] [varchar](50) NULL,
	[irowno] [int] NULL,
	[cinvname] [varchar](100) NULL,
	[idiscount] [decimal](18, 6) NULL,
	[inatdiscount] [decimal](18, 6) NULL,
	[cComUnitName] [varchar](20) NULL,
	[cInvDefine1] [varchar](20) NULL,
	[cInvDefine2] [varchar](20) NULL,
	[cInvDefine13] [decimal](18, 2) NULL,
	[cInvDefine14] [decimal](18, 2) NULL,
	[UnitGroup] [varchar](100) NULL,
	[cComUnitQTY] [decimal](18, 4) NULL,
	[cInvDefine1QTY] [decimal](18, 4) NULL,
	[cInvDefine2QTY] [decimal](18, 4) NULL,
	[cn1cComUnitName] [varchar](20) NULL,
	[cpreordercode] [varchar](30) NULL,
	[iaoids] [varchar](30) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opPreOrder]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opPreOrder](
	[lngPreOrderId] [int] IDENTITY(1,1) NOT NULL,
	[strBillNo] [varchar](50) NOT NULL,
	[ddate] [date] NULL,
	[ccode] [varchar](50) NULL,
	[csysbarcode] [varchar](80) NULL,
	[lngopUserId] [varchar](50) NULL,
	[cmaker] [varchar](10) NULL,
	[bytStatus] [tinyint] NULL,
	[ccuscode] [varchar](10) NULL,
	[ccusname] [varchar](100) NULL,
	[datBillTime] [datetime] NULL,
	[lngBillType] [tinyint] NULL,
	[strManagers] [varchar](20) NULL,
	[InvalidTime] [datetime] NULL,
	[InvalidPersonCode] [varchar](10) NULL,
	[strAuditor] [varchar](20) NULL,
	[datAuditordTime] [datetime] NULL,
 CONSTRAINT [PK_Dl_opPreOrder] PRIMARY KEY CLUSTERED 
(
	[lngPreOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opPreOrderControl]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opPreOrderControl](
	[lngPreOrderControlId] [int] IDENTITY(1,1) NOT NULL,
	[IsEnable] [varchar](2) NULL,
	[IsTimeControl] [varchar](2) NULL,
	[datStartTime] [datetime] NULL,
	[datEndTime] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opPreOrderDetail]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opPreOrderDetail](
	[lngPreOrderDetailId] [int] IDENTITY(1,1) NOT NULL,
	[lngPreOrderId] [int] NOT NULL,
	[cinvcode] [varchar](40) NOT NULL,
	[dexpectationdate] [varchar](30) NULL,
	[borderbom] [bit] NULL,
	[borderbomover] [bit] NULL,
	[batpcal] [bit] NULL,
	[dacceptabledate] [varchar](30) NULL,
	[dsatiabledate] [varchar](30) NULL,
	[iquantity] [decimal](18, 6) NULL,
	[bsaleprice] [bit] NULL,
	[bgift] [bit] NULL,
	[itaxrate] [decimal](18, 6) NULL,
	[kl] [decimal](18, 6) NULL,
	[kl2] [decimal](18, 6) NULL,
	[dkl1] [decimal](18, 6) NULL,
	[dkl2] [decimal](18, 6) NULL,
	[cbsysbarcode] [varchar](80) NULL,
	[iquotedprice] [decimal](18, 6) NULL,
	[inatdiscount] [decimal](18, 6) NULL,
	[isum] [decimal](18, 6) NULL,
	[idiscount] [decimal](18, 6) NULL,
	[imoney] [decimal](18, 6) NULL,
	[inatmoney] [decimal](18, 6) NULL,
	[inatsum] [decimal](18, 6) NULL,
	[inattax] [decimal](18, 6) NULL,
	[itax] [decimal](18, 6) NULL,
	[itaxunitprice] [decimal](18, 6) NULL,
	[inatunitprice] [decimal](18, 6) NULL,
	[iunitprice] [decimal](18, 6) NULL,
	[iinvexchrate] [decimal](18, 6) NULL,
	[cunitid] [varchar](35) NULL,
	[fcusminprice] [decimal](18, 6) NULL,
	[irowno] [int] NULL,
	[id] [int] NULL,
	[inum] [decimal](18, 6) NULL,
	[cComUnitName] [varchar](20) NULL,
	[cInvDefine1] [varchar](20) NULL,
	[cInvDefine2] [varchar](20) NULL,
	[cInvDefine13] [decimal](18, 2) NULL,
	[cInvDefine14] [decimal](18, 2) NULL,
	[UnitGroup] [varchar](100) NULL,
	[cComUnitQTY] [decimal](18, 4) NULL,
	[cInvDefine1QTY] [decimal](18, 4) NULL,
	[cInvDefine2QTY] [decimal](18, 4) NULL,
	[cn1cComUnitName] [varchar](20) NULL,
	[cDefine22] [varchar](100) NULL,
	[cinvname] [varchar](160) NULL,
PRIMARY KEY CLUSTERED 
(
	[lngPreOrderDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opQQService]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opQQService](
	[lngQQServiceId] [int] IDENTITY(1,1) NOT NULL,
	[strQQName] [varchar](100) NULL,
	[strOpUserId] [varchar](100) NULL,
 CONSTRAINT [PK_Dl_opQQService] PRIMARY KEY CLUSTERED 
(
	[lngQQServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opRight]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opRight](
	[lngRightId] [int] NOT NULL,
	[strRightName] [varchar](50) NULL,
	[strRightProject] [varchar](50) NOT NULL,
	[strRightExp] [varchar](200) NULL,
	[datCreatetime] [smalldatetime] NULL,
	[lngCreater] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opRightGroup]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opRightGroup](
	[lngopRightgroupId] [int] NOT NULL,
	[strGroupExp] [varchar](200) NULL,
	[datCreateTime] [smalldatetime] NULL,
	[lngCreater] [int] NULL,
	[bytStatus] [tinyint] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opRightGroupDetail]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dl_opRightGroupDetail](
	[lngopRightGroupDetailId] [int] IDENTITY(1,1) NOT NULL,
	[lngopRightGroupId] [int] NULL,
	[lngRightId] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dl_opSOASetting]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opSOASetting](
	[lngopSOASettingId] [int] IDENTITY(1,1) NOT NULL,
	[SOAName] [varchar](100) NULL,
	[SOACreateTime] [varchar](100) NULL,
	[SOASendTime] [varchar](100) NULL,
	[IsEnable] [varchar](2) NULL,
	[Description] [varchar](500) NULL,
 CONSTRAINT [PK_Dl_opSOASetting] PRIMARY KEY CLUSTERED 
(
	[lngopSOASettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opSOASettingDetail]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opSOASettingDetail](
	[lngopSOASettingDetailId] [int] IDENTITY(1,1) NOT NULL,
	[lngopSOASettingId] [int] NOT NULL,
	[ccuscode] [varchar](50) NULL,
	[ccusname] [varchar](50) NULL,
	[SOATime] [datetime] NULL,
 CONSTRAINT [PK_DL_opSOASettingDetail] PRIMARY KEY CLUSTERED 
(
	[lngopSOASettingDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opStockControl]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opStockControl](
	[opStockcontrolId] [int] NOT NULL,
	[cInvCode] [varchar](50) NOT NULL,
	[strStocktype] [varchar](50) NULL,
	[datCreatetime] [smalldatetime] NULL,
	[lngCreater] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opSystemConfiguration]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opSystemConfiguration](
	[lngSystemConfigurationId] [int] IDENTITY(1,1) NOT NULL,
	[IsExercisePrice] [varchar](2) NULL,
 CONSTRAINT [PK_Dl_opSystemConfiguration] PRIMARY KEY CLUSTERED 
(
	[lngSystemConfigurationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opTel]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dl_opTel](
	[序号] [float] NULL,
	[客户编码] [nvarchar](255) NULL,
	[客户名称] [nvarchar](255) NULL,
	[助记码] [nvarchar](255) NULL,
	[客户姓名] [nvarchar](255) NULL,
	[电话] [nvarchar](255) NULL,
	[F7] [nvarchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dl_opU8SOA]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opU8SOA](
	[lngSOAid] [int] IDENTITY(1,1) NOT NULL,
	[ccuscode] [varchar](20) NULL,
	[ccusname] [varchar](80) NULL,
	[strEndDate] [datetime] NULL,
	[datSendTime] [datetime] NULL,
	[dblAmount] [decimal](20, 6) NULL,
	[strUper] [varchar](200) NULL,
	[strOper] [varchar](20) NULL,
	[strOperName] [nchar](10) NULL,
	[bytCheck] [bit] NULL,
	[datCheckTime] [datetime] NULL,
	[intperiodid] [int] NULL,
	[intPeriod] [int] NULL,
	[intPeriodYear] [int] NULL,
 CONSTRAINT [PK_Dl_opU8SOA] PRIMARY KEY CLUSTERED 
(
	[lngSOAid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opUser]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opUser](
	[lngopUserId] [int] IDENTITY(1,1) NOT NULL,
	[strLoginName] [varchar](50) NULL,
	[strUserPwd] [varchar](50) NULL,
	[strUserName] [varchar](50) NULL,
	[strUserTel] [varchar](50) NULL,
	[strUserMail] [varchar](50) NULL,
	[strUserQQ] [varchar](50) NULL,
	[strUserIdCard] [varchar](50) NULL,
	[strUserCompName] [varchar](100) NULL,
	[strUserCompAddress] [varchar](200) NULL,
	[cCusCode] [varchar](50) NULL,
	[strStatus] [varchar](2) NULL,
	[strUserLevel] [varchar](2) NULL,
	[dlbCreditLine] [decimal](18, 6) NULL,
	[dblAvailableCreditLine] [decimal](18, 6) NULL,
	[datCreatetime] [smalldatetime] NULL,
	[lngCreater] [int] NULL,
 CONSTRAINT [PK_Dl_opUser] PRIMARY KEY CLUSTERED 
(
	[lngopUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opUserAddress]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opUserAddress](
	[lngopUseraddressId] [int] IDENTITY(1,1) NOT NULL,
	[lngopUserId] [int] NOT NULL,
	[strDistributionType] [varchar](20) NULL,
	[strConsigneeName] [varchar](50) NULL,
	[strConsigneeTel] [varchar](50) NULL,
	[strReceivingAddress] [varchar](200) NULL,
	[strCarplateNumber] [varchar](50) NULL,
	[strDriverName] [varchar](50) NULL,
	[strDriverTel] [varchar](50) NULL,
	[strIdCard] [varchar](50) NULL,
	[datCreatetime] [smalldatetime] NULL,
	[lngCreater] [int] NULL,
	[bitSYSDefault] [bit] NULL,
	[bitDistributionTypeDefault] [bit] NULL,
	[strProvince] [varchar](80) NULL,
	[strCity] [varchar](80) NULL,
	[strDistrict] [varchar](80) NULL,
	[strVehicleType] [varchar](80) NULL,
	[bytStatus] [tinyint] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opUserInfo]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dl_opUserInfo](
	[lngopUserInfoId] [int] IDENTITY(1,1) NOT NULL,
	[lngopUserId] [int] NULL,
	[cCusCode] [varchar](50) NULL,
	[cCusName] [varchar](50) NULL,
	[blnDefault] [bit] NULL,
	[blnDisable] [bit] NULL,
	[datCreatetime] [varchar](50) NULL,
	[lngCreater] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opUserRightGroup]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dl_opUserRightGroup](
	[lngopUserrightgroupId] [int] IDENTITY(1,1) NOT NULL,
	[lngopRightGroupId] [int] NULL,
	[lngopUserId] [int] NULL,
	[datCreatetime] [smalldatetime] NULL,
	[lngCreater] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dl_SmsRecord]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dl_SmsRecord](
	[lngSmsRecordId] [int] NULL,
	[SmsRecordCount] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[dl_TEST]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dl_TEST](
	[isum] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DL_test1]    Script Date: 2016-02-27 8:43:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DL_test1](
	[dlname] [nvarchar](50) NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_lngopUserId]  DEFAULT (' ') FOR [lngopUserId]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_strBillNo]  DEFAULT (' ') FOR [strBillNo]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_strBillName]  DEFAULT (' ') FOR [strBillName]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_datCreateTime]  DEFAULT (((1999)-(1))-(1)) FOR [datCreateTime]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_strAuditor]  DEFAULT (' ') FOR [strAuditor]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_datAuditordTime]  DEFAULT (((1999)-(1))-(1)) FOR [datAuditordTime]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_cSOCode]  DEFAULT (' ') FOR [cSOCode]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_bytStatus]  DEFAULT ((0)) FOR [bytStatus]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_strRemarks]  DEFAULT (' ') FOR [strRemarks]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_ccuscode]  DEFAULT (' ') FOR [ccuscode]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_cdefine1]  DEFAULT (' ') FOR [cdefine1]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_cdefine2]  DEFAULT (' ') FOR [cdefine2]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_cdefine3]  DEFAULT (' ') FOR [cdefine3]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_cdefine8]  DEFAULT (' ') FOR [cdefine8]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_cdefine9]  DEFAULT (' ') FOR [cdefine9]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_cdefine10]  DEFAULT (' ') FOR [cdefine10]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_cdefine11]  DEFAULT (' ') FOR [cdefine11]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_cdefine12]  DEFAULT (' ') FOR [cdefine12]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_cdefine13]  DEFAULT (' ') FOR [cdefine13]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_ccusname]  DEFAULT (' ') FOR [ccusname]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_cpersoncode]  DEFAULT (' ') FOR [cpersoncode]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_cSCCode]  DEFAULT (' ') FOR [cSCCode]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_datBillTime]  DEFAULT (getdate()) FOR [datBillTime]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_Managers]  DEFAULT (' ') FOR [strManagers]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_datDeliveryDate]  DEFAULT (((1999)-(1))-(1)) FOR [datDeliveryDate]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_strLoadingWays]  DEFAULT (' ') FOR [strLoadingWays]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_cSTCode]  DEFAULT (' ') FOR [cSTCode]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_lngopUseraddressId]  DEFAULT (' ') FOR [lngopUseraddressId]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_lngBillType]  DEFAULT ((0)) FOR [lngBillType]
GO
ALTER TABLE [dbo].[Dl_opOrder] ADD  CONSTRAINT [DF_Dl_opOrder_strRejectRemarks]  DEFAULT ('') FOR [strRejectRemarks]
GO
ALTER TABLE [dbo].[Dl_opOrderBillNoSetting] ADD  CONSTRAINT [DF_Dl_opOrderBillNoSetting_datCreateDateTime]  DEFAULT (getdate()) FOR [datCreateDateTime]
GO
ALTER TABLE [dbo].[Dl_opOrderDetail] ADD  CONSTRAINT [DF_Dl_opOrderDetail_idiscount]  DEFAULT ((0)) FOR [idiscount]
GO
ALTER TABLE [dbo].[Dl_opOrderDetail] ADD  CONSTRAINT [DF_Dl_opOrderDetail_inatdiscount]  DEFAULT ((0)) FOR [inatdiscount]
GO
ALTER TABLE [dbo].[Dl_opOrderDetail] ADD  CONSTRAINT [DF_Dl_opOrderDetail_cpreordercode]  DEFAULT (' ') FOR [cpreordercode]
GO
ALTER TABLE [dbo].[Dl_opOrderDetail] ADD  CONSTRAINT [DF_Dl_opOrderDetail_iaoids]  DEFAULT (' ') FOR [iaoids]
GO
ALTER TABLE [dbo].[Dl_opPreOrder] ADD  CONSTRAINT [DF_Dl_opPreOrder_cmaker]  DEFAULT (' ') FOR [cmaker]
GO
ALTER TABLE [dbo].[Dl_opPreOrder] ADD  CONSTRAINT [DF_Dl_opPreOrder_strManagers]  DEFAULT (' ') FOR [strManagers]
GO
ALTER TABLE [dbo].[Dl_opPreOrderControl] ADD  CONSTRAINT [DF_Dl_opPreOrderControl_IsEnable]  DEFAULT ((0)) FOR [IsEnable]
GO
ALTER TABLE [dbo].[Dl_opPreOrderControl] ADD  CONSTRAINT [DF_Dl_opPreOrderControl_IsTimeControl]  DEFAULT ((0)) FOR [IsTimeControl]
GO
ALTER TABLE [dbo].[Dl_opPreOrderControl] ADD  CONSTRAINT [DF_Dl_opPreOrderControl_datStartTime]  DEFAULT (((1900)-(1))-(1)) FOR [datStartTime]
GO
ALTER TABLE [dbo].[Dl_opPreOrderControl] ADD  CONSTRAINT [DF_Dl_opPreOrderControl_datEndTime]  DEFAULT (((1900)-(1))-(1)) FOR [datEndTime]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT (getdate()) FOR [dexpectationdate]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [borderbom]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [borderbomover]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [batpcal]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT (getdate()) FOR [dacceptabledate]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT (getdate()) FOR [dsatiabledate]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [iquantity]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [bsaleprice]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [bgift]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [itaxrate]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [kl]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [kl2]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [dkl1]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [dkl2]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT (' ') FOR [cbsysbarcode]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [iquotedprice]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [inatdiscount]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [isum]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [idiscount]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [imoney]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [inatmoney]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [inatsum]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [inattax]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [itax]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [itaxunitprice]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [inatunitprice]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [iunitprice]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [iinvexchrate]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT (' ') FOR [cunitid]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [fcusminprice]
GO
ALTER TABLE [dbo].[Dl_opPreOrderDetail] ADD  DEFAULT ((0)) FOR [inum]
GO
ALTER TABLE [dbo].[Dl_opSystemConfiguration] ADD  CONSTRAINT [DF_Dl_opSystemConfiguration_IsExercisePrice]  DEFAULT ((1)) FOR [IsExercisePrice]
GO
ALTER TABLE [dbo].[Dl_opU8SOA] ADD  CONSTRAINT [DF_Dl_opU8SOA_ccuscode]  DEFAULT (' ') FOR [ccuscode]
GO
ALTER TABLE [dbo].[Dl_opU8SOA] ADD  CONSTRAINT [DF_Dl_opU8SOA_ccusname]  DEFAULT (' ') FOR [ccusname]
GO
ALTER TABLE [dbo].[Dl_opU8SOA] ADD  CONSTRAINT [DF_Dl_opU8SOA_dblAmount]  DEFAULT ((0)) FOR [dblAmount]
GO
ALTER TABLE [dbo].[Dl_opU8SOA] ADD  CONSTRAINT [DF_Dl_opU8SOA_strUper]  DEFAULT (' ') FOR [strUper]
GO
ALTER TABLE [dbo].[Dl_opU8SOA] ADD  CONSTRAINT [DF_Dl_opU8SOA_strOper]  DEFAULT (' ') FOR [strOper]
GO
ALTER TABLE [dbo].[Dl_opU8SOA] ADD  CONSTRAINT [DF_Dl_opU8SOA_strOperName]  DEFAULT (' ') FOR [strOperName]
GO
ALTER TABLE [dbo].[Dl_opU8SOA] ADD  CONSTRAINT [DF_Dl_opU8SOA_bytCheck]  DEFAULT ((0)) FOR [bytCheck]
GO
ALTER TABLE [dbo].[Dl_opU8SOA] ADD  CONSTRAINT [DF_Dl_opU8SOA_datCheckTime]  DEFAULT (((1900)-(1))-(1)) FOR [datCheckTime]
GO
ALTER TABLE [dbo].[Dl_opUserAddress] ADD  CONSTRAINT [DF_Dl_opUserAddress_strProvince]  DEFAULT (' ') FOR [strProvince]
GO
ALTER TABLE [dbo].[Dl_opUserAddress] ADD  CONSTRAINT [DF_Dl_opUserAddress_strCity]  DEFAULT (' ') FOR [strCity]
GO
ALTER TABLE [dbo].[Dl_opUserAddress] ADD  CONSTRAINT [DF_Dl_opUserAddress_strDistrict]  DEFAULT (' ') FOR [strDistrict]
GO
ALTER TABLE [dbo].[Dl_opUserAddress] ADD  CONSTRAINT [DF_Dl_opUserAddress_strVehicleType]  DEFAULT (' ') FOR [strVehicleType]
GO
ALTER TABLE [dbo].[Dl_opUserAddress] ADD  CONSTRAINT [DF_Dl_opUserAddress_bytStatus]  DEFAULT ((0)) FOR [bytStatus]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单主表id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'lngopOrderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id(DL系统用户ID)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'lngopUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单编号(网单号)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'strBillNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单别名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'strBillName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单创建时间(日期)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'datCreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'strAuditor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'datAuditordTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'U8订单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'cSOCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单状态(1,未审核;2,待用户确认;3,被驳回;4,已审核;99,作废)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'bytStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'strRemarks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开票单位编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'ccuscode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'车型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'cdefine3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收货人地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'cdefine11'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'司机电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'cdefine13'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开票单位名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'ccusname'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'业务员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'cpersoncode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发运方式编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'cSCCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下单时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'datBillTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单据操作员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'strManagers'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交货日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'datDeliveryDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'装车方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'strLoadingWays'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售类型编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'cSTCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'lngopUseraddressId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联U8订单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'RelateU8NO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单据类型(0:普通订单;1:酬宾订单;2:特殊订单)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'lngBillType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驳回时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'datRejectTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'确认时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'datAffirmTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作废时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'InvalidTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作废人id(内码)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrder', @level2type=N'COLUMN',@level2name=N'InvalidPersonCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderBillNoSetting', @level2type=N'COLUMN',@level2name=N'lngOrderBillNoSettingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单据类别(0普通订单,1样品资料,2参照酬宾订单,3参照特殊订单,4酬宾订单,5特殊订单)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderBillNoSetting', @level2type=N'COLUMN',@level2name=N'lngBillType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单据名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderBillNoSetting', @level2type=N'COLUMN',@level2name=N'strBillName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水号位长' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderBillNoSetting', @level2type=N'COLUMN',@level2name=N'lngSerialNoLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderBillNoSetting', @level2type=N'COLUMN',@level2name=N'datCreateDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单表体id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'lngopOrderDetailId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单主表id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'lngopOrderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'存货编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cinvcode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'基本单位数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'iquantity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'辅计量数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'inum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'报价 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'iquotedprice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原币无税单价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'iunitprice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原币含税单价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'itaxunitprice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原币无税金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'imoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原币税额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'itax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原币价税合计' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'isum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本币无税单价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'inatunitprice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本币无税金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'inatmoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本币税额 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'inattax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本币价税合计' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'inatsum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扣率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'kl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'税率 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'itaxrate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表体自定义项22' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cdefine22'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'换算率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'iinvexchrate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'计量单位编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cunitid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'行号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'irowno'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'存货名称 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cinvname'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原币折扣额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'idiscount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本币折扣额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'inatdiscount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'基本单位名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cComUnitName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大包装单位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cInvDefine1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小包装单位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cInvDefine2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大包装换算率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cInvDefine13'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小包装换算率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cInvDefine14'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单位换算率组' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'UnitGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'基本单位数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cComUnitQTY'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大包装单位数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cInvDefine1QTY'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'小包装单位数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cInvDefine2QTY'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销售单位名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cn1cComUnitName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预订单号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cpreordercode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预订单autoid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'iaoids'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主表id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'lngPreOrderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单据编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'strBillNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单据日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'ddate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'U8预订单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'ccode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单据条码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'csysbarcode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'lngopUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'制单人,登录人strloginname' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'cmaker'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单据状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'bytStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开票单位代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'ccuscode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开票单位名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'ccusname'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下单时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'datBillTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单据类型(1,酬宾订单,2特殊订单)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'lngBillType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'strManagers'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作废时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'InvalidTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作废人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'InvalidPersonCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'strAuditor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrder', @level2type=N'COLUMN',@level2name=N'datAuditordTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrderControl', @level2type=N'COLUMN',@level2name=N'lngPreOrderControlId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用预订单(0,禁止,1开启)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrderControl', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否开启时间控制(0禁止,1开启)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrderControl', @level2type=N'COLUMN',@level2name=N'IsTimeControl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrderControl', @level2type=N'COLUMN',@level2name=N'datStartTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opPreOrderControl', @level2type=N'COLUMN',@level2name=N'datEndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主表id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opSOASetting', @level2type=N'COLUMN',@level2name=N'lngopSOASettingId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主表Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opSystemConfiguration', @level2type=N'COLUMN',@level2name=N'lngSystemConfigurationId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否显示执行价1(否则显示报价0)金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opSystemConfiguration', @level2type=N'COLUMN',@level2name=N'IsExercisePrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opU8SOA', @level2type=N'COLUMN',@level2name=N'lngSOAid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opU8SOA', @level2type=N'COLUMN',@level2name=N'ccuscode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opU8SOA', @level2type=N'COLUMN',@level2name=N'ccusname'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'截至日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opU8SOA', @level2type=N'COLUMN',@level2name=N'strEndDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账单发送时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opU8SOA', @level2type=N'COLUMN',@level2name=N'datSendTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账单金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opU8SOA', @level2type=N'COLUMN',@level2name=N'dblAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账单金额大写' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opU8SOA', @level2type=N'COLUMN',@level2name=N'strUper'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员userid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opU8SOA', @level2type=N'COLUMN',@level2name=N'strOper'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opU8SOA', @level2type=N'COLUMN',@level2name=N'strOperName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户是否确认账单' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opU8SOA', @level2type=N'COLUMN',@level2name=N'bytCheck'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户确认账单时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opU8SOA', @level2type=N'COLUMN',@level2name=N'datCheckTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'期间id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opU8SOA', @level2type=N'COLUMN',@level2name=N'intperiodid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'期间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opU8SOA', @level2type=N'COLUMN',@level2name=N'intPeriod'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账单年' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opU8SOA', @level2type=N'COLUMN',@level2name=N'intPeriodYear'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主表id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'lngopUseraddressId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'lngopUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'送货方式(配送,自提)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'strDistributionType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收货人姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'strConsigneeName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收货人电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'strConsigneeTel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收货地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'strReceivingAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'车牌号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'strCarplateNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'司机姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'strDriverName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'司机电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'strDriverTel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'司机身份证' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'strIdCard'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'datCreatetime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'lngCreater'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否系统默认' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'bitSYSDefault'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否配送类型默认' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'bitDistributionTypeDefault'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'省' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'strProvince'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'城市' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'strCity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区县' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'strDistrict'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'车辆类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'strVehicleType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态(0:使用;1:禁用)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opUserAddress', @level2type=N'COLUMN',@level2name=N'bytStatus'
GO
