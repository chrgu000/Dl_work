USE [UFDATA_001_2015]
GO
/****** Object:  Table [dbo].[Dl_opInventory]    Script Date: 2015-11-30 18:01:58 ******/
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
/****** Object:  Table [dbo].[Dl_opInventoryControl]    Script Date: 2015-11-30 18:01:58 ******/
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
/****** Object:  Table [dbo].[Dl_opOrder]    Script Date: 2015-11-30 18:01:58 ******/
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
	[RelateU8NO] [varchar](30) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opOrderDetail]    Script Date: 2015-11-30 18:01:58 ******/
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
	[cn1cComUnitName] [varchar](20) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opRight]    Script Date: 2015-11-30 18:01:58 ******/
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
/****** Object:  Table [dbo].[Dl_opRightGroup]    Script Date: 2015-11-30 18:01:58 ******/
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
/****** Object:  Table [dbo].[Dl_opRightGroupDetail]    Script Date: 2015-11-30 18:01:58 ******/
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
/****** Object:  Table [dbo].[Dl_opStockControl]    Script Date: 2015-11-30 18:01:58 ******/
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
/****** Object:  Table [dbo].[Dl_opUser]    Script Date: 2015-11-30 18:01:58 ******/
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
	[lngCreater] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opUserAddress]    Script Date: 2015-11-30 18:01:58 ******/
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
	[strVehicleType] [varchar](80) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dl_opUserInfo]    Script Date: 2015-11-30 18:01:58 ******/
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
/****** Object:  Table [dbo].[Dl_opUserRightGroup]    Script Date: 2015-11-30 18:01:58 ******/
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
ALTER TABLE [dbo].[Dl_opOrderDetail] ADD  CONSTRAINT [DF_Dl_opOrderDetail_idiscount]  DEFAULT ((0)) FOR [idiscount]
GO
ALTER TABLE [dbo].[Dl_opOrderDetail] ADD  CONSTRAINT [DF_Dl_opOrderDetail_inatdiscount]  DEFAULT ((0)) FOR [inatdiscount]
GO
ALTER TABLE [dbo].[Dl_opUserAddress] ADD  CONSTRAINT [DF_Dl_opUserAddress_strProvince]  DEFAULT (' ') FOR [strProvince]
GO
ALTER TABLE [dbo].[Dl_opUserAddress] ADD  CONSTRAINT [DF_Dl_opUserAddress_strCity]  DEFAULT (' ') FOR [strCity]
GO
ALTER TABLE [dbo].[Dl_opUserAddress] ADD  CONSTRAINT [DF_Dl_opUserAddress_strDistrict]  DEFAULT (' ') FOR [strDistrict]
GO
ALTER TABLE [dbo].[Dl_opUserAddress] ADD  CONSTRAINT [DF_Dl_opUserAddress_strVehicleType]  DEFAULT (' ') FOR [strVehicleType]
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'存货名称 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dl_opOrderDetail', @level2type=N'COLUMN',@level2name=N'cinvname'
GO
