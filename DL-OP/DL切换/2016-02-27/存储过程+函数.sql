USE [UFDATA_005_2015]
GO
/****** Object:  StoredProcedure [dbo].[DLproc_CheckPacking]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [DLproc_CheckPacking] '0'
-- =============================================
-- Author:echo
-- Create date:2016-01-25
-- Description:	用于查询我的工作详情
-- =============================================

create PROCEDURE [dbo].[DLproc_CheckPacking]
@strBillNo varchar(50)  
AS

BEGIN
SET NOCOUNT ON;
declare @strsql varchar(3000), @Fix varchar(20),@NoLength int ,@SeriaNo varchar(20),@year varchar(10),@month varchar(10)
 


END
--select * from dl_oporderbillnosetting
--exec DLproc_GetBillNo '0'

--declare @s varchar(100),@strBillNo varchar(100)
--exec DLproc_GetBillNo '0',@s output
--select @s
GO
/****** Object:  StoredProcedure [dbo].[DLproc_GetBillNo]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [DLproc_GetBillNo] '0'
-- =============================================
-- Author:echo
-- Create date:2016-01-25
-- Description:	用于查询我的工作详情
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_GetBillNo]
@strBillType varchar(10)	--订单类型,0普通订单,1样品资料,2参照酬宾订单,3参照特殊订单,4酬宾订单,5特殊订单,6:U8普通订单,7:U8样品资料,8:U8参照酬宾订单,9:U8参照特殊订单,10:U8酬宾订单,11:U8特殊订单,
,@strBillNo varchar(50)  OUTPUT
AS

BEGIN
SET NOCOUNT ON;
declare @strsql varchar(3000), @Fix varchar(20),@NoLength int ,@SeriaNo varchar(20),@year varchar(10),@month varchar(10)

--获取前缀,流水号长度,年,月,流水号
select @Fix=strPrefix,@NoLength=lngSerialNoLength,@year=strYear,@month=strMonth,@SeriaNo=strSeriaNo  from dl_oporderbillnosetting where lngbilltype=@strBillType
--流水号+1
select @SeriaNo=convert(varchar(20),CAST(@SeriaNo AS int)+1)
--更新表的最大流水号@SeriaNo
update dl_oporderbillnosetting set strSeriaNo=@SeriaNo where lngbilltype=@strBillType
--获取流水号,并补位
select @SeriaNo=right('00000000000000000000000'+@SeriaNo,@NoLength)
--获取最大订单编号,dl_oporderbillnosetting的strSeriaNo
select @strBillNo=@Fix+@year+@month+@SeriaNo



END
--select * from dl_oporderbillnosetting
--exec DLproc_GetBillNo '0'

--declare @s varchar(100),@strBillNo varchar(100)
--exec DLproc_GetBillNo '0',@s output
--select @s
GO
/****** Object:  StoredProcedure [dbo].[DLproc_getCusCreditInfo]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*U8信用额度*/
/*20151203,echo修改,原名称DL_getCusCreditInfo,改为DLproc_getCusCreditInfo,增加扣减网上下单系统里的顾客金额,订单状态为1,2,3*/
/*2016-01-16,修改为取执行金额,*/
CREATE proc [dbo].[DLproc_getCusCreditInfo]     -- 原函数名称为 usp_sa_getCusInfoForVouchHelp  ycc 添加函数取客户实际信用额
 (     
    @cCusCode as nvarchar(60)=N''    
 )    
as    
BEGIN 

declare @iSum decimal(20,6)  --网上下单金额
  create table #temp(
  cCusCode decimal(20,6),
  CusCredit decimal(20,6),
  iCusCreLine decimal(20,6)
  )

insert into #temp exec DL_getCusCreditInfo @cCusCode
 
 --返回数据    
-- /*20151203,查找网上下单系统里的用户金额,并扣减,执行金额*/
 select @iSum=isnull(sum(isnull(iSum,0)),0)  from Dl_opOrder aa 
left join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId
where aa.ccuscode=@cCusCode and aa.bytStatus in (1,2,3)

 /*20151219,查找网上下单系统里的用户金额,并扣减,报价金额*/
-- select @iSum=isnull(sum(isnull(iquantity*iquotedprice,0)),0)  from Dl_opOrder aa 
--inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId
--where aa.ccuscode=@cCusCode and aa.bytStatus in (1,2,3)

 select  cCusCode,  
CASE WHEN iCusCreLine =0 THEN -99999999
else CusCredit-@iSum end iCusCreLine		--信用额度(0为现金顾客)
from #temp 

-- select  cCusCode,  
--CASE WHEN iCusCreLine =0 THEN -99999999
--else @iCreBalance-@iSum end iCusCreLine		--信用额度(0为现金顾客)
--from customer where cCusCode=@cCusCode    


 drop table #temp
 END
 --exec DLproc_getCusCreditInfo '010101'
-- declare @iSum decimal(20,6)
--  select  sum(isnull(iSum,0))  from Dl_opOrder aa 
--inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId
--where aa.ccuscode='010124' and aa.bytStatus in (1,2,3)
--select @iSum


--  select  isnull(iSum,0) from Dl_opOrder aa 
--inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId
--where aa.ccuscode='010124' and aa.bytStatus in (1,2,3)
 




GO
/****** Object:  StoredProcedure [dbo].[DLproc_getCusCreditInfoWithBillno]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*U8信用额度*/
/*20151203,echo修改,原名称DL_getCusCreditInfo,改为DLproc_getCusCreditInfo,增加扣减网上下单系统里的顾客金额,订单状态为1,2,3*/
/*20151218,修改,扣除传递过来的修改单据的金额*/
/*2016-01-16,修改为取执行金额*/
CREATE proc [dbo].[DLproc_getCusCreditInfoWithBillno]     -- 原函数名称为 usp_sa_getCusInfoForVouchHelp  ycc 添加函数取客户实际信用额
 (     
    @cCusCode as nvarchar(60)=N'',
   @strBillNo varchar(30)    
 )    
 as    
declare @cus varchar(30),@iSum decimal(20,6),@iprice decimal(20,6)

  create table #temp(
  cCusCode decimal(20,6),
  CusCredit decimal(20,6),
  iCusCreLine decimal(20,6)
  )

insert into #temp exec DL_getCusCreditInfo @cCusCode

/*20151218,查询billno对应的顾客,及订单金额,报价金额*/
--select @cus=aa.ccuscode,@iprice=sum(bb.iQuotedPrice*bb.iQuantity)   from Dl_opOrder aa 
--inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId 
--where aa.strBillNo=@strBillNo
--group by aa.ccuscode 
/*20160116,查询billno对应的顾客,及订单金额,执行金额*/
select @cus=aa.ccuscode,@iprice=isnull(sum(isnull(bb.isum,0)),0)   from Dl_opOrder aa 
inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId 
where aa.strBillNo=@strBillNo
group by aa.ccuscode 
 --返回数据    
-- /*20151203,查找网上下单系统里的用户金额,并扣减,执行金额*/
 select @iSum=isnull(sum(isnull(iSum,0)),0)  from Dl_opOrder aa 
inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId
where aa.ccuscode=@cCusCode and aa.bytStatus in (1,2,3)

 /*20151219,查找网上下单系统里的用户金额,并扣减,报价金额*/
-- select @iSum=isnull(sum(isnull(iquantity*iquotedprice,0)),0)  from Dl_opOrder aa 
--inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId
--where aa.ccuscode=@cCusCode and aa.bytStatus in (1,2,3)

if @cus=@cCusCode
begin
set @iSum=@iSum-@iprice
end


 select  cCusCode,  
CASE WHEN iCusCreLine =0 THEN -99999999
else CusCredit-@iSum end iCusCreLine		--信用额度(0为现金顾客)
from #temp 


drop table #temp





 --exec DLproc_getCusCreditInfoWithBillno '010101',''
-- declare @iSum decimal(20,6)
--  select  sum(isnull(iSum,0))  from Dl_opOrder aa 
--inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId
--where aa.ccuscode='010124' and aa.bytStatus in (1,2,3)
--select @iSum


--  select  isnull(iSum,0) from Dl_opOrder aa 
--inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId
--where aa.ccuscode='010124' and aa.bytStatus in (1,2,3)
  

  




GO
/****** Object:  StoredProcedure [dbo].[DLproc_getPreOrderCusCreditInfo]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*U8信用额度*/
/*20151203,echo修改,原名称DL_getCusCreditInfo,改为DLproc_getPreOrderCusCreditInfo,增加扣减网上下单系统里的顾客预订单金额,订单状态为1,2,3*/
CREATE proc [dbo].[DLproc_getPreOrderCusCreditInfo]     -- 原函数名称为 usp_sa_getCusInfoForVouchHelp  ycc 添加函数取客户实际信用额
 (     
   @cCusCode as nvarchar(60)=N''  ,
   @lngBillType int 
     
 )    
as    

 declare @iCusCreLine decimal(20,6)  --信用总额    
  declare @iSum decimal(20,6)		--网上下单预订单总金额

  --返回数据    
 /*20151203,查找网上下单系统里的用户金额,并扣减*/
 select @iSum=isnull(sum(isnull(iSum,0)),0)  from Dl_opPreOrder aa 
inner join Dl_opPreOrderDetail bb on aa.lngPreOrderId=bb.lngPreOrderId
where aa.ccuscode=@cCusCode and aa.bytStatus in (1,2,3,4) and aa.lngBillType=@lngBillType

 select  cCusCode,  
--iCusCreLine,	--信用总额  
CASE WHEN iCusCreLine =0 THEN -99999999		
else iCusCreLine-@iSum end iCusCreLine		--信用额度(0为现金顾客)
 from customer where cCusCode=@cCusCode    

  --exec DLproc_getPreOrderCusCreditInfo '010101',1
  --exec DLproc_getPreOrderCusCreditInfo '99999903',1 
-- declare @iSum decimal(20,6)
--  select  sum(isnull(iSum,0))  from Dl_opOrder aa 
--inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId
--where aa.ccuscode='010124' and aa.bytStatus in (1,2,3)
--select @iSum


--  select  isnull(iSum,0) from Dl_opOrder aa 
--inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId
--where aa.ccuscode='010124' and aa.bytStatus in (1,2,3)
  

  



GO
/****** Object:  StoredProcedure [dbo].[DLproc_InventoryBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================

-- Author:echo

-- Create date:2015-09-28

-- Description:	用于查询物料大类结构,生成物料树结构
--marks:2016-01-14修改:启用新的查询方式,当某一个分类下面所有商品被限销时,禁止显示该分类
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_InventoryBySel]

	-- Add the parameters for the stored procedure here

	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 

	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
@cSTCode varchar(10),
@ccuscode varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from

	-- interfering with SELECT statements.

	SET NOCOUNT ON;
    -- Insert statements for procedure here
if @cSTCode='0' or @cSTCode='00'
begin
select KeyFieldName,ParentFieldName,NodeName from (
select C.cInvCCode KeyFieldName ,case iInvCGrade
		when 1 then 'null'
		when 2 then SUBSTRING(C.cInvCCode,1,2)
		when 3 then SUBSTRING(C.cInvCCode,1,4)
		when 4 then SUBSTRING(C.cInvCCode,1,6)
		when 5 then SUBSTRING(C.cInvCCode,1,8)
		when 6 then SUBSTRING(C.cInvCCode,1,10)
		end 'ParentFieldName' ,c.cInvCName NodeName from InventoryClass C,(select I.cInvCCode from Inventory I
where   I.bSale=1 and I.cInvCode not in (select  cInvCode from SA_CusInvLimited where cCusCode=@ccuscode) GROUP BY I.cInvCCode) T
where c.cInvCCode=T.cInvCCode and (C.cInvCCode like '01%' or C.cInvCCode like '02%')
union 
select C.cInvCCode KeyFieldName ,case iInvCGrade
		when 1 then 'null'
		when 2 then SUBSTRING(C.cInvCCode,1,2)
		when 3 then SUBSTRING(C.cInvCCode,1,4)
		when 4 then SUBSTRING(C.cInvCCode,1,6)
		when 5 then SUBSTRING(C.cInvCCode,1,8)
		when 6 then SUBSTRING(C.cInvCCode,1,10)
		end 'ParentFieldName' ,c.cInvCName NodeName from InventoryClass C where len(C.cInvCCode)<9 and (C.cInvCCode like '01%' or C.cInvCCode like '02%')) TT
ORDER BY TT.KeyFieldName
end
Else
begin
select KeyFieldName,ParentFieldName,NodeName from (
select C.cInvCCode KeyFieldName ,case iInvCGrade
		when 1 then 'null'
		when 2 then SUBSTRING(C.cInvCCode,1,2)
		when 3 then SUBSTRING(C.cInvCCode,1,4)
		when 4 then SUBSTRING(C.cInvCCode,1,6)
		when 5 then SUBSTRING(C.cInvCCode,1,8)
		when 6 then SUBSTRING(C.cInvCCode,1,10)
		end 'ParentFieldName' ,c.cInvCName NodeName from InventoryClass C,(select I.cInvCCode from Inventory I
where   I.bSale=1 and I.cInvCode not in (select  cInvCode from SA_CusInvLimited where cCusCode=@ccuscode) GROUP BY I.cInvCCode) T
where c.cInvCCode=T.cInvCCode  and (C.cInvCCode like '32%')
union 
select C.cInvCCode KeyFieldName ,case iInvCGrade
		when 1 then 'null'
		when 2 then SUBSTRING(C.cInvCCode,1,2)
		when 3 then SUBSTRING(C.cInvCCode,1,4)
		when 4 then SUBSTRING(C.cInvCCode,1,6)
		when 5 then SUBSTRING(C.cInvCCode,1,8)
		when 6 then SUBSTRING(C.cInvCCode,1,10)
		end 'ParentFieldName' ,c.cInvCName NodeName from InventoryClass C where len(C.cInvCCode)<9 and (C.cInvCCode like '32%')) TT
ORDER BY TT.KeyFieldName
end



END


--exec [DLproc_InventoryBySel] '00','010101'
--exec [DLproc_InventoryBySel] '01','010108'

--select left(cInvCode,9) from SA_CusInvLimited where cCusCode='010101'

GO
/****** Object:  StoredProcedure [dbo].[DLproc_KYLBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [DLproc_NewSampleOrderByIns]

-- =============================================
-- Author:echo
-- Create date:2015-12-26
-- Description:	可用量查询(物料大类)
-- 参数:
-- =============================================
--exec [DLproc_KYLBySel]
CREATE PROCEDURE [dbo].[DLproc_KYLBySel]

AS

BEGIN

	SET NOCOUNT ON;
	declare @ddate varchar(20)
	select @ddate=convert(varchar(10),GETDATE(),120)
----
  CREATE TABLE #TempNo1 ( cInvCode nvarchar(60),  YQGL float, DDZT float ,DHDJ float ,SCDD float ,SCWL float default 0, WWDD float default 0, WWWL float default 0, DBZT float ,  YDGL float ,DFHL float ,DBDF  float , BLJH  float ,dDate datetime ,
  cFree1 nvarchar(20), cFree2 nvarchar(20), cFree3 nvarchar(20),cFree4 nvarchar(20),cFree5 nvarchar(20),cFree6 nvarchar(20), cFree7 nvarchar(20),cFree8 nvarchar(20),cFree9 nvarchar(20),cFree10 nvarchar(20),isotype smallint default 0,
  isodid  nvarchar(40) default N'') 
  CREATE INDEX kcview_1_inx_inv ON #TempNo1 (cinvcode) 
 ----
  INSERT INTO #TempNo1 
  SELECT SO_SODetails.cInvCode ,  0 As YQGL,0 As DDZT , 0 As DHDJ , 0 As SCDD ,0 AS SCWL, 0 AS WWDD, 0 AS WWWL, 0 As DBZT , (Case When (iQuantity - IsNull(iFHQuantity ,0)) < 0 Then 0 
  Else (iQuantity - IsNull(iFHQuantity ,0) ) End) As YDGL, 0 As DFHL, 0 As DBDF , 0 AS BLJH ,   (Case When IsNull(SO_SODetails.dPreDate,N'') = N'' Then SO_SOMain.dDate Else SO_SODetails.dPreDate End ) AS dDate ,SO_SODetails.cFree1,SO_SODetails.cFree2,   
  SO_SODetails.cFree3,SO_SODetails.cFree4,SO_SODetails.cFree5,SO_SODetails.cFree6,SO_SODetails.cFree7, SO_SODetails.cFree8,SO_SODetails.cFree9,SO_SODetails.cFree10,isnull(so_sodetails.idemandtype,0) ,  (case when isnull(so_sodetails.idemandtype,0)=1 
  then convert(nvarchar(40),ISNULL(SO_SODetails.isosid,N'')) when isnull(so_sodetails.idemandtype,0) = 4 then (case when isnull(so_sodetails.cdemandcode,N'')=N'' then N'Systemdefault' else isnull(so_sodetails.cdemandcode,N'') end )  
  when isnull(so_sodetails.idemandtype,0) = 5 then ISNULL(SO_SODetails.csocode,N'') else N'' end )  FROM SO_SOMain INNER JOIN SO_SODetails ON SO_SOMain.ID = SO_SODetails.ID   INNER JOIN Inventory ON SO_SODetails.cInvCode = Inventory.cInvCode  
  WHERE  IsNull(SO_SOMain.cVerifier,N'') <> N'' AND IsNull(SO_SODetails.cSCloser ,N'') = N'' AND   IsNull(SO_SOMain.cBustype,N'') <> N'直运销售'  and Isnull(SO_SODetails.cparentcode,N'')=N''  AND (Case When IsNull(SO_SODetails.dPreDate,N'') = N'' 
  Then SO_SOMain.dDate Else SO_SODetails.dPreDate End ) <=@ddate and (Inventory.cInvCCode Like N'01%' or Inventory.cInvCCode Like N'02%')
 ----
  INSERT INTO #TempNo1 
  SELECT v_ex_order_inuseFotSt.cInvCode ,  0 As YQGL,0 As DDZT , 0 As DHDJ , 0 As SCDD ,0 AS SCWL, 0 AS WWDD, 0 AS WWWL, 0 As DBZT , (abs(fQuantity)) As YDGL, 0 As DFHL, 0 As DBDF , 0 AS BLJH ,   
  convert(nvarchar(10),v_ex_order_inuseFotSt.dExpDate,121) AS dDate ,v_ex_order_inuseFotSt.cFree1,v_ex_order_inuseFotSt.cFree2,   v_ex_order_inuseFotSt.cFree3,v_ex_order_inuseFotSt.cFree4,v_ex_order_inuseFotSt.cFree5,v_ex_order_inuseFotSt.cFree6,
  v_ex_order_inuseFotSt.cFree7, v_ex_order_inuseFotSt.cFree8,v_ex_order_inuseFotSt.cFree9,v_ex_order_inuseFotSt.cFree10,isnull(v_ex_order_inuseFotSt.idemandtype,0), (case when isnull(v_ex_order_inuseFotSt.idemandtype,0)=3 then convert(nvarchar(40),
  ISNULL(v_ex_order_inuseFotSt.autoid,N'')) when isnull(v_ex_order_inuseFotSt.idemandtype,0) = 4 then isnull(v_ex_order_inuseFotSt.cdemandcode,N'') when isnull(v_ex_order_inuseFotSt.idemandtype,0) = 6 then ISNULL(v_ex_order_inuseFotSt.ccode,N'') 
  else N'' end )  FROM v_ex_order_inuseFotSt  INNER JOIN Inventory ON v_ex_order_inuseFotSt.cInvCode = Inventory.cInvCode  AND convert(nvarchar(10),v_ex_order_inuseFotSt.dExpDate,121) <=@ddate and (Inventory.cInvCCode Like N'01%' or Inventory.cInvCCode Like N'02%')
 ----
 select * into #TempNo2 from   (select    k.cinvcode,k.YQGL ,k.DDZT ,k.DHDJ ,k.SCDD ,k.SCWL ,k.WWDD , k.WWWL , k.DBZT ,   k.YDGL  ,k.DFHL  ,k.DBDF  , k.BLJH   ,k.dDate  ,k.cFree1 , k.cFree2 , k.cFree3 ,k.cFree4 ,k.cFree5 ,k.cFree6 ,  k.cFree7 ,k.cFree8 ,
 k.cFree9 ,k.cFree10 ,(CASE when inventory.csrpolicy=N'LP' and isnull(inventory.bspecialorder,0)=1 then k.isotype else 0 end) as isotype ,  (case when inventory.csrpolicy=N'LP' and isnull(inventory.bspecialorder,0)=1 then (case when k.isotype=1 
 then so_sodetails.csocode  when k.isotype=3 then ex_order.ccode when k.isotype = 0  then  N'' else k.isodid  end ) else N'' end) as csocode,  (CASE when inventory.csrpolicy=N'LP' and isnull(inventory.bspecialorder,0)=1 then (case when k.isotype=1 
 then convert(varchar(10),so_sodetails.irowno) when k.isotype=3 then convert(varchar(10),ex_orderdetail.irowno) else N'' end ) else N'' end) as isoseq  from #TempNo1 as k   inner join inventory on inventory.cinvcode= k.cinvcode 
 left join so_sodetails ON k.isodid = convert(nvarchar(40),so_sodetails.isosid) and k.isotype=1    left join ex_orderdetail on k.isodid = convert(nvarchar(40),ex_orderdetail.autoid) and k.isotype=3    left join ex_order on ex_orderdetail.id=ex_order.id  ) T    where  1=1
 ----
  CREATE INDEX kcview_6_inx_inv ON #TempNo2 (cinvcode) 
 ----
  Select cInvCode ,IsNull(iSafeNum,0) As iSafeNum ,IsNull(iTopSum,0) AS iTopSum ,IsNull(iLowSum,0) AS iLowSum  
  INTO #TempNo5 
  From Inventory where 1=1  and (Inventory.cInvCCode Like N'01%' or Inventory.cInvCCode Like N'02%')
 ----
  CREATE INDEX kcview_3_inx_inv ON #TempNo5 (cinvcode) 
 ----
  SELECT TopLowSafeRefer.cInvCode ,  SPACE(20) As cFree1 , SPACE(20) As cFree2, SPACE(20) AS cFree3 , SPACE(20) AS cFree4, SPACE(20) AS cFree5 , SPACE(20) As cFree6 , SPACE(20) As cFree7,  SPACE(20) AS cFree8, SPACE(20) AS cFree9, 
  SPACE(20) AS cFree10 , T.isotype,T.csocode,isnull(T.isoseq,N'') as isoseq,  SUM(YQGL) AS YQGL , SUM(DDZT) AS DDZT  ,SUM(DHDJ) AS DHDJ  ,SUM(SCDD) AS SCDD ,SUM(SCWL) AS SCWL ,SUM(WWDD) AS WWDD ,SUM(WWWL) AS WWWL,
  SUM(DBZT) AS DBZT  ,(IsNull(SUM(YQGL) ,0) + IsNull(SUM(DDZT),0) + IsNull(SUM(DHDJ),0) + IsNull(SUM(SCDD),0) + IsNull(SUM(WWDD),0)  + IsNull(SUM(DBZT),0)) AS RKHJ ,SUM(YDGL) AS YDGL ,SUM(DFHL) AS DFHL ,SUM(DBDF) AS DBDF ,  
  SUM(BLJH) AS BLJH , (IsNull(SUM(YDGL),0) + IsNull(SUM(DFHL),0) + IsNull(SUM(DBDF),0)  + IsNull(SUM(BLJH),0) + IsNull(SUM(SCWL),0) + IsNull(SUM(WWWL),0)  ) AS CKHJ , IsNull(TopLowSafeRefer.iSafeNum,0) AS iSafeNum, 
  IsNull(TopLowSafeRefer.iTopSum,0) AS iTopSum ,IsNull(TopLowSafeRefer.iLowSum,0) AS iLowSum  
  INTO #TempNo3 
  FROM #TempNo2 AS T  RIGHT JOIN #TempNo5 TopLowSafeRefer  ON T.cInvCode = TopLowSafeRefer.cInvCode  WHERE (1=1)  
  GROUP BY TopLowSafeRefer.cInvCode ,T.isotype,T.csocode,T.isoseq , TopLowSafeRefer.iSafeNum ,TopLowSafeRefer.iTopSum ,TopLowSafeRefer.iLowSum 
 ----
  SELECT  Inventory.cInvCode  ,cInvName ,cInvAddCode ,cInvStd,cComUnitname , SPACE(20) As cFree1 , SPACE(20) As cFree2, SPACE(20) AS cFree3 , SPACE(20) AS cFree4, SPACE(20) AS cFree5 , SPACE(20) As cFree6 , SPACE(20) As cFree7,  
  SPACE(20) AS cFree8, SPACE(20) AS cFree9, SPACE(20) AS cFree10 ,T.isotype,T.csocode,isnull(T.isoseq,N'') as isoseq, convert(float, 0.0) AS XCL ,convert(float, 0.0) AS DJL,convert(float, 0.0) AS BHGP,  YQGL  ,  DDZT ,   DHDJ ,  DBZT ,  SCDD , SCWL,WWDD,WWWL, 
  RKHJ , YDGL , DFHL , DBDF ,   BLJH , CKHJ ,  IsNull(T.iSafeNum,convert(float, 0.0)) AS iSafeNum  , IsNull(T.iTopSum,convert(float, 0.0)) As iTopSum, IsNull(T.iLowSum,convert(float, 0.0)) As iLowSum ,  cInvDefine1, cInvDefine2, cInvDefine3,cInvDefine4,cInvDefine5,
  cInvDefine6,cInvDefine7,cInvDefine8,cInvDefine9,cInvDefine10, cInvDefine11,cInvDefine12,cInvDefine13,cInvDefine14,cInvDefine15,cInvDefine16   
  INTO #TempNo4 
  FROM ComputationUnit INNER JOIN  Inventory ON ComputationUnit.cComunitCode = Inventory.cComUnitCode LEFT OUTER JOIN #TempNo3 T ON T.cInvCode = Inventory.cInvCode Where (Inventory.cInvCCode Like N'01%' or Inventory.cInvCCode Like N'02%')
 ----
  INSERT INTO #TempNo4 (cInvCode ,cInvName ,cInvAddCode ,cInvStd,cComUnitname ,XCL ,DJL ,BHGP   ,cFree1  ,cFree2  ,cFree3 ,cFree4  ,cFree5 ,cFree6 ,cFree7  ,cFree8 ,cFree9  ,cFree10  ,isotype,csocode,isoseq ,cInvDefine1, cInvDefine2, cInvDefine3,
  cInvDefine4,cInvDefine5,cInvDefine6,cInvDefine7,cInvDefine8,cInvDefine9,cInvDefine10, cInvDefine11,cInvDefine12,cInvDefine13,cInvDefine14,cInvDefine15,cInvDefine16,iSafeNum,iTopSum,iLowSum )  
  SELECT Inventory.cInvCode ,cInvName ,cInvAddCode ,cInvStd,cComUnitname ,(IsNull(iQuantity,0)+isnull(ipeqty,0)) As XCL , (CASE WHEN (IsNull(bStopFlag,0) <> 0 OR IsNull(BGSPSTOP,0) <> 0) THEN iQuantity ELSE fStopQuantity END) As DJL ,
  (fDisableQuantity) AS BHGP ,  SPACE(20) As cFree1 , SPACE(20) As cFree2, SPACE(20) AS cFree3 , SPACE(20) AS cFree4, SPACE(20) AS cFree5 , SPACE(20) As cFree6 , SPACE(20) As cFree7,  SPACE(20) AS cFree8, SPACE(20) AS cFree9, SPACE(20) AS cFree10,  
  isnull(isotype,0) as isotype,csocode,isnull(convert(varchar(10),isoseq),N'') as isoseq, cInvDefine1, cInvDefine2, cInvDefine3,cInvDefine4,cInvDefine5,cInvDefine6,cInvDefine7,cInvDefine8,cInvDefine9,cInvDefine10, cInvDefine11,cInvDefine12,cInvDefine13,cInvDefine14,
  cInvDefine15,cInvDefine16,(#TempNo5.iSafeNum),(#TempNo5.itopsum),(#TempNo5.iLowSum)   FROM V_CurrentStock_PE T INNER JOIN Inventory ON T.cInvCode = Inventory.cInvCode 
  INNER JOIN Warehouse ON T.cWhCode = Warehouse.cWhCode INNER JOIN ComputationUnit ON Inventory.cComUnitCode = ComputationUnit.cComUnitCode  left join #TempNo5 on t.cinvcode=#TempNo5.cinvcode  
  WHERE bInAvailCalcu = 1 and  1=1 AND (Inventory.cInvCCode Like N'01%' or Inventory.cInvCCode Like N'02%')
 ----
 --Select (Case When IsNull(cValue,N'') = N'' Then cDefault Else cValue End) As cValue ,* From Accinformation Where cSysid = N'ST' And cName = N'bShowAllDataForKCView'
 ----
 SELECT  cInvCode  ,cInvName ,cInvAddCode ,cInvStd,cComUnitname , SPACE(20) As cFree1 , SPACE(20) As cFree2, SPACE(20) AS cFree3 , SPACE(20) AS cFree4, SPACE(20) AS cFree5 , SPACE(20) As cFree6 , SPACE(20) As cFree7,  SPACE(20) AS cFree8, 
 SPACE(20) AS cFree9, SPACE(20) AS cFree10 , LTRIM(STR(Sum(IsNull(XCL,0)),30,2)) As XCL ,LTRIM(STR(Sum(IsNull(DJL,0)),30,2)) As DJL , LTRIM(STR(Sum(IsNull(YQGL,0)),30,2)) As YQGL  ,  LTRIM(STR(Sum(IsNull(DDZT,0)),30,2)) AS DDZT ,  
 LTRIM(STR(Sum(IsNull(DHDJ,0)),30,2)) AS DHDJ ,  LTRIM(STR(Sum(IsNull(DBZT,0)),30,2)) AS DBZT ,   LTRIM(STR(Sum(IsNull(SCDD,0)),30,2)) AS SCDD ,LTRIM(STR(Sum(IsNull(WWDD,0)),30,2)) AS WWDD,  LTRIM(STR(Sum(IsNull(RKHJ,0)),30,2)) AS RKHJ , 
 LTRIM(STR(Sum(IsNull(YDGL,0)),30,2)) As YDGL ,  LTRIM(STR(Sum(IsNull(DFHL,0)),30,2)) As DFHL ,   LTRIM(STR(Sum(IsNull(DBDF,0)),30,2)) As DBDF ,   LTRIM(STR(Sum(IsNull(BLJH,0)),30,2)) As BLJH ,LTRIM(STR(Sum(IsNull(SCWL,0)),30,2)) AS SCWL ,
 LTRIM(STR(Sum(IsNull(WWWL,0)),30,2)) AS WWWL, LTRIM(STR(Sum(IsNull(CKHJ,0)),30,2)) AS CKHJ ,  LTRIM(STR(Sum(BHGP),30,2)) AS BHGP  , LTRIM(STR(Sum(IsNull(( IsNull(XCL,0) -IsNull(DJL,0)  -  IsNull(YDGL,0) ),0)) ,30,2)) As KYL ,  
 LTRIM(STR(max(IsNull(iSafeNum,0)),30,2)) AS iSafeNum  ,  LTRIM(STR(CASE WHEN max(IsNull(iSafeNum,0)) - Sum(IsNull(( IsNull(XCL,0) -IsNull(DJL,0)  -  IsNull(YDGL,0) ),0)) > 0 THEN max(IsNull(iSafeNum,0)) - Sum(IsNull(( IsNull(XCL,0) -IsNull(DJL,0)  -  IsNull(YDGL,0) ),0)) 
 ELSE NULL END ,30,2)) As DYAQL ,  LTRIM(STR(max(IsNull(iTopSum,0)),30,2)) As iTopSum,  LTRIM(STR(CASE WHEN Sum(IsNull(( IsNull(XCL,0) -IsNull(DJL,0)  -  IsNull(YDGL,0) ),0)) - max(IsNull(iTopSum ,0)) > 0 
 THEN Sum(IsNull(( IsNull(XCL,0) -IsNull(DJL,0)  -  IsNull(YDGL,0) ),0)) - max(IsNull(iTopSum ,0)) ELSE NULL END ,30, 2)) As CCL ,  LTRIM(STR(max(IsNull(iLowSum,0)),30,2)) As iLowSum ,  
 LTRIM(STR(CASE WHEN max(IsNull(iLowSum,0)) - Sum(IsNull(( IsNull(XCL,0) -IsNull(DJL,0)  -  IsNull(YDGL,0) ),0)) > 0 THEN max(IsNull(iLowSum,0)) - Sum(IsNull(( IsNull(XCL,0) -IsNull(DJL,0)  -  IsNull(YDGL,0) ),0)) ELSE NULL END ,30,2)) As DQL ,  cInvDefine1, 
 cInvDefine2, cInvDefine3,cInvDefine4,cInvDefine5,cInvDefine6,cInvDefine7,cInvDefine8,cInvDefine9,cInvDefine10, cInvDefine11,cInvDefine12,cast(cInvDefine13  as  decimal(30,6)) as cinvdefine13, cast(cInvDefine14  as  decimal(30,6)) as cinvdefine14, 
 convert(char(10),cInvDefine15,121) as cinvdefine15, convert(char(10),cInvDefine16,121) as cinvdefine16   ,(select enumname from v_aa_enum where enumcode=isnull(isotype,0) and enumtype=N'ST.Sotype') as isotypename,
 isnull(csocode,N'') as csocode,isnull(isoseq,N'') as isoseq  FROM #TempNo4 GROUP BY cInvCode  ,cInvName ,cInvAddCode ,cInvStd,cComUnitname,  cInvDefine1, cInvDefine2, cInvDefine3,cInvDefine4,cInvDefine5,cInvDefine6,cInvDefine7,cInvDefine8,
 cInvDefine9,cInvDefine10, cInvDefine11,cInvDefine12,cast(cInvDefine13  as  decimal(30,6)) ,cast(cInvDefine14  as  decimal(30,6)), convert(char(10),cInvDefine15,121), convert(char(10),cInvDefine16,121)  ,isnull(isotype,0),isnull(csocode,N''),isnull(isoseq,N'')  
 HAVING (Sum(IsNull(XCL,0)) <> 0 OR Sum(IsNull(DJL,0)) <> 0 OR Sum(IsNull(YQGL,0)) <> 0 OR Sum(IsNull(DDZT,0)) <> 0 OR Sum(IsNull(DHDJ,0)) <> 0 OR Sum(IsNull(DBZT,0)) <> 0  OR Sum(IsNull(SCDD,0)) <> 0 OR Sum(IsNull(SCWL,0)) <> 0 OR 
 Sum(IsNull(WWDD,0)) <> 0 OR Sum(IsNull(WWWL,0)) <> 0  OR Sum(IsNull(YDGL,0)) <> 0 OR Sum(IsNull(DFHL ,0)) <> 0 OR Sum(IsNull(DBDF,0)) <> 0 OR Sum(IsNull(BHGP,0)) <> 0  OR Sum(IsNull(BLJH,0)) <> 0 ) 
 order by cinvcode,isnull(isotype,0),isnull(csocode,N''),isnull(isoseq,N'') 
 ----
 END
 ----
 
 ----

 ----

 ----
 
 ----

 ----

GO
/****** Object:  StoredProcedure [dbo].[DLproc_MyWorkBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec DLproc_MyWorkBySel '01010100101','010101','','',0
-- =============================================
-- Author:echo
-- Create date:2015-11-25
-- Description:	用于查询我的工作详情
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_MyWorkBySel]
@strBillNo varchar(50),	--订单编号
@beginDate varchar(20),		--开始日期
@endDate  varchar(20),	--结束日期
@cSTCode  varchar(10),	--销售类型
@bytStatus int,	--订单状态
@strManagers varchar(10)	--操作员内码id
AS

BEGIN
SET NOCOUNT ON;
declare @sqlstrBillNo varchar(100),@sqlbeginDate varchar(100),@sqlendDate varchar(100),@sqlcSTCode varchar(100),@sqlbytStatus varchar(100),@sql varchar(1000),@sqlstrManagers varchar(100)

if @strBillNo=''
begin
set @sqlstrBillNo='1=1 and '
end
else
begin
set @sqlstrBillNo='strBillNo='+''''+@strBillNo+''''+' and '
end

if @beginDate =''
begin
set @sqlbeginDate='datCreateTime>'+''''+'1900-01-01'+''''+' and ' 
end
else
begin
set @sqlbeginDate='datCreateTime>='+''''+@beginDate+''''+' and '
end

if @endDate =''
begin
set @sqlendDate='datCreateTime<'+''''+'2099-01-01'+''''+' and ' 
end
else
begin
set @sqlendDate='datCreateTime<='+''''+@endDate+''''+' and '
end

if @cSTCode ='-1'
begin
set @sqlcSTCode='1=1 and ' 
end
if @cSTCode ='00' or @cSTCode ='01'
begin
set @sqlcSTCode='cSTCode='+''''+@cSTCode+''''+' and lngBillType=0 and '
end
if @cSTCode ='1' or @cSTCode ='2'
begin
set @sqlcSTCode='lngBillType='+''''+@cSTCode+''''+' and '
end

if @bytStatus =0
begin
set @sqlbytStatus='1=1 and ' 
end
else if @bytStatus =4
begin
set @sqlbytStatus='bytStatus=4 and '
end
else if @bytStatus =1
begin
set @sqlbytStatus='bytStatus in (1,2,3) and '
end
else if @bytStatus =99
begin
set @sqlbytStatus='bytStatus=99 and '
end

set @sqlstrManagers='strManagers='+''''+@strManagers+''''

set @sql='select strBillNo,isnull(cSOCode,'+''''+' '+''''+') cSOCode,convert(varchar(10),datCreateTime,120) datCreateTime,ccusname,cdefine11,strRemarks,strLoadingWays,
case when bytStatus='+''''+'1'+''''+' then '+''''+'未审核'+''''+'
when bytStatus='+''''+'2'+''''+' then '+''''+'待确认'+''''+'
when bytStatus='+''''+'3'+''''+' then '+''''+'被驳回'+''''+'
when bytStatus='+''''+'4'+''''+' then '+''''+'已审核'+''''+'
when bytStatus='+''''+'99'+''''+' and lngopUserId=isnull(InvalidPersonCode,0)'+' then '+''''+'顾客作废'+''''+'
when bytStatus='+''''+'99'+''''+' and lngopUserId!=isnull(InvalidPersonCode,0)'+' then '+''''+'操作员作废'+''''+'
 end bytStatus,datBillTime,
 case when bytStatus='+''''+'1'+''''+' then   null
when bytStatus='+''''+'2'+''''+' then datAffirmTime
when bytStatus='+''''+'3'+''''+' then datRejectTime
when bytStatus='+''''+'4'+''''+' then datAuditordTime
when bytStatus='+''''+'99'+''''+' then InvalidTime end stime
  from Dl_opOrder where '+@sqlstrBillNo+@sqlbeginDate+@sqlendDate+@sqlcSTCode+@sqlbytStatus+@sqlstrManagers+' order by strBillNo desc'

--select @sql
exec (@sql)
END

--exec DLproc_MyWorkBySel '','2015-11-21','2016-11-24','-1',0,87




GO
/****** Object:  StoredProcedure [dbo].[DLproc_MyWorkPreOrderBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec DLproc_MyWorkBySel '01010100101','010101','','',0
-- =============================================
-- Author:echo
-- Create date:2015-12-14
-- Description:	用于查询我的工作详情
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_MyWorkPreOrderBySel]
@strBillNo varchar(50),
@beginDate varchar(20), 
@endDate  varchar(20),
--@cSTCode  varchar(10),
@bytStatus int,
@strManagers varchar(10)
AS

BEGIN
SET NOCOUNT ON;
declare @sqlstrBillNo varchar(100),@sqlbeginDate varchar(100),@sqlendDate varchar(100),@sqlcSTCode varchar(100),@sqlbytStatus varchar(100),@sql varchar(500),@sqlstrManagers varchar(100)

if @strBillNo=''
begin
set @sqlstrBillNo='1=1 and '
end
else
begin
set @sqlstrBillNo='strBillNo='+''''+@strBillNo+''''+' and '
end

if @beginDate =''
begin
set @sqlbeginDate='ddate>'+''''+'1900-01-01'+''''+' and ' 
end
else
begin
set @sqlbeginDate='ddate>='+''''+@beginDate+''''+' and '
end

if @endDate =''
begin
set @sqlendDate='ddate<'+''''+'2099-01-01'+''''+' and ' 
end
else
begin
set @sqlendDate='ddate<='+''''+@endDate+''''+' and '
end

--if @cSTCode ='-1'
--begin
--set @sqlcSTCode='1=1 and ' 
--end
--else
--begin
--set @sqlcSTCode='cSTCode='+''''+@cSTCode+''''+' and '
--end

if @bytStatus =0
begin
set @sqlbytStatus='1=1 and ' 
end
else if @bytStatus =4
begin
set @sqlbytStatus='bytStatus=4 and '
end
else if @bytStatus =1
begin
set @sqlbytStatus='bytStatus in (1,2,3) and '
end

set @sqlstrManagers='strManagers='+''''+@strManagers+''''

set @sql='select strBillNo,isnull(ccode,'+''''+' '+''''+') cSOCode,convert(varchar(10),datBillTime,120)  xdsj,ddate,ccusname,
case lngBillType
when '+''''+'1'+''''+' then '+''''+'酬宾订单'+''''+'
when '+''''+'2'+''''+' then '+''''+'特殊订单'+''''+'
 end lngBillType,
case bytStatus
when '+''''+'1'+''''+' then '+''''+'未审核'+''''+'
when '+''''+'2'+''''+' then '+''''+'待确认'+''''+'
when '+''''+'3'+''''+' then '+''''+'被驳回'+''''+'
when '+''''+'4'+''''+' then '+''''+'已审核'+''''+'
when '+''''+'99'+''''+' then '+''''+'已作废'+''''+'
 end bytStatus from Dl_opPreOrder where '+@sqlstrBillNo+@sqlbeginDate+@sqlendDate+@sqlbytStatus+@sqlstrManagers+' order by strBillNo desc'

--select @sql
exec (@sql)

END

--exec DLproc_MyWorkPreOrderBySel '','2015-11-21','2015-12-24',4,90

--select * from Dl_opPreOrder
 


GO
/****** Object:  StoredProcedure [dbo].[DLproc_MyWorkPreYOrderForCustomerBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [DLproc_MyWorkPreYOrderForCustomerBySel] '01010100101','010101','','',0
-- =============================================
-- Author:echo
-- Create date:2016-01-19
-- Description:	用于查询我的工作详情
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_MyWorkPreYOrderForCustomerBySel]
@strBillNo varchar(50),
@beginDate varchar(20), 
@endDate  varchar(20),
--@cSTCode  varchar(10),
@bytStatus int,
@ConstcCusCode varchar(10),
@lngBillType varchar(10)
AS

BEGIN
SET NOCOUNT ON;
declare @sqlstrBillNo varchar(100),@sqlbeginDate varchar(100),@sqlendDate varchar(100),@sqlcSTCode varchar(100),@sqlbytStatus varchar(100),@sql varchar(1000),@lngopUserIds varchar(100),@lngBillTypes varchar(100)

select @lngopUserIds=lngopUserId from Dl_opUser where strLoginName=@ConstcCusCode

set @lngBillTypes='lngBillType='+@lngBillType

if @strBillNo=''
begin
set @sqlstrBillNo='1=1 and '
end
else
begin
set @sqlstrBillNo='strBillNo='+''''+@strBillNo+''''+' and '
end

if @beginDate =''
begin
set @sqlbeginDate='ddate>'+''''+'1900-01-01'+''''+' and ' 
end
else
begin
set @sqlbeginDate='ddate>='+''''+@beginDate+''''+' and '
end

if @endDate =''
begin
set @sqlendDate='ddate<'+''''+'2099-01-01'+''''+' and ' 
end
else
begin
set @sqlendDate='ddate<='+''''+@endDate+''''+' and '
end

if @bytStatus =0
begin
set @sqlbytStatus='1=1 and ' 
end
else if @bytStatus =4
begin
set @sqlbytStatus='bytStatus=4 and '
end
else if @bytStatus =1
begin
set @sqlbytStatus='bytStatus in (1,2,3) and '
end

set @lngopUserIds='lngopUserId='+''''+@lngopUserIds+''''+' and '

set @sql='select strBillNo,isnull(ccode,'+''''+' '+''''+') cSOCode,convert(varchar(10),datBillTime,120)  xdsj,ddate,ccusname,
case lngBillType
when '+''''+'1'+''''+' then '+''''+'酬宾订单'+''''+'
when '+''''+'2'+''''+' then '+''''+'特殊订单'+''''+'
 end lngBillType,
case bytStatus
when '+''''+'1'+''''+' then '+''''+'未审核'+''''+'
when '+''''+'2'+''''+' then '+''''+'待确认'+''''+'
when '+''''+'3'+''''+' then '+''''+'被驳回'+''''+'
when '+''''+'4'+''''+' then '+''''+'已审核'+''''+'
when '+''''+'99'+''''+' then '+''''+'已作废'+''''+'
 end bytStatus from Dl_opPreOrder where '+@sqlstrBillNo+@sqlbeginDate+@sqlendDate+@sqlbytStatus+@lngopUserIds+@lngBillTypes+' order by strBillNo desc'

--select @sql
exec (@sql)

END

--exec [DLproc_MyWorkPreYOrderForCustomerBySel] 'Y20151200056','2015-12-22','2015-12-31',0,'010101',1
--exec [DLproc_MyWorkPreYOrderForCustomerBySel] '','2015-11-21','2015-12-24',4,'010101',2
--exec [DLproc_MyWorkPreYOrderForCustomerBySel] '','','',0,'010101',1
--select * from Dl_opPreOrder
 


GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewOrderByIns]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec DLproc_NewOrderByIns

-- =============================================
-- Author:echo
-- Create date:2015-10-23
-- Description:	新增加订单表头插入数据[DLproc_NewOrderByIns]
-- 参数:
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewOrderByIns]
@lngopUserId varchar(50),--登陆人,制单人id
@datCreateTime varchar(50),--创建日期,单据日期,预发货,预完成日期
@bytStatus varchar(50),--单据状态
@strRemarks varchar(500),--备注
@ccuscode varchar(50),--客户编码,开票单位编码
@cdefine1 varchar(50),--自定义项1
@cdefine2 varchar(50),--自定义项2
@cdefine3 varchar(500),--自定义项3
@cdefine9 varchar(50), --自定义项9
@cdefine10 varchar(50),--自定义项10
@cdefine11 varchar(200),--自定义项11
@cdefine12 varchar(50),--自定义项12
@cdefine13 varchar(50),--自定义项13
@ccusname varchar(50),--客户名称
@cpersoncode varchar(50),--业务员编码
@cSCCode varchar(50), --发运方式编码
@datDeliveryDate varchar(50), --交货日期
@strLoadingWays varchar(100), --装车方式
@cSTCode varchar(10),		--销售类型
@lngopUseraddressId varchar(10),--useraddress的ID
@strU8BillNo varchar(30)	--U8单据号,样品资料写入,改为DL系统单据号

AS

BEGIN

	SET NOCOUNT ON;
declare @strBillNo varchar(50),@maxbillno int
--创建最大订单号@BillNo=@csocode=@strBillNo --销售订单号
--exec DLproc_OrderGetNewBillNoBySel 1,@strBillNo OUTPUT
--exec @strBillNo=DLproc_OrderGetNewBillNoBySel	--使用return返回值
--print @strBillNo
if @cSTCode='00'
begin
set @strU8BillNo=' '
end
/*2016-01-25更新获取单据编号方式*/--begin
--获取最大单号
--select @maxbillno=isnull(max(convert(int,substring(strBillNo,5,9))),0)+1 from Dl_opOrder where strBillNo like 'DLOP%' 
----select @maxbillno
----IF @maxbillno=100000
----BEGIN
----select 'error'  'BillNo'
----END
----ELSE
----BEGIN
------创建新的订单编号,--获取两位年,两位月,组合,如1510,1509
----select @strBillNo='DLOP'+convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+right('0000'+convert(varchar(5),@maxbillno),5)
----END
--select @strBillNo='DLOP'+convert(varchar(9),@maxbillno)
/*2016-01-25更新获取单据编号方式*/--end
--select @strBillNo
--2016-01-25获取单据编号方式
exec DLproc_GetBillNo '0',@strBillNo OUTPUT
--插入表头数据
insert into Dl_opOrder (
lngopUserId,strBillNo,datCreateTime,bytStatus,strRemarks,ccuscode,cdefine1,cdefine2,cdefine3,cdefine9,cdefine10,cdefine11,cdefine12,cdefine13,ccusname,cpersoncode,cSCCode,datDeliveryDate, strLoadingWays,cSTCode,lngopUseraddressId,RelateU8NO
) 
values (
@lngopUserId,@strBillNo,@datCreateTime,@bytStatus,@strRemarks,@ccuscode,@cdefine1,@cdefine2,@cdefine3,@cdefine9,@cdefine10,@cdefine11,@cdefine12,@cdefine13,@ccusname,@cpersoncode,@cSCCode,@datDeliveryDate, @strLoadingWays, @cSTCode,@lngopUseraddressId,@strU8BillNo
)

--20151129添加,清除临时授权,
update Customer set cCusEmail=' ' where cCusCode=@ccuscode --客户编码,开票单位编码

--20151129添加,样品资料直接给关联的U8订单专员负责,如果订单员已经接单,则直接关联转接
if @cSTCode='01'
begin
declare @strManagers varchar(10)
select @strManagers=isnull(strManagers,' ') from Dl_opOrder where strBillNo=@strU8BillNo
update Dl_opOrder set strManagers=@strManagers where strBillNo=@strBillNo
end

--返回订单表头id
--select top(1) lngopOrderId from Dl_opOrder where strBillNo=@strBillNo
select max(lngopOrderId) 'lngopOrderId',@strBillNo 'strBillNo' from Dl_opOrder where strBillNo=@strBillNo

END




GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewOrderByUpd]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec DLproc_NewOrderByIns

-- =============================================
-- Author:echo
-- Create date:2015-11-13
-- Description:	修改(更新)订单表头插入数据[DLproc_NewOrderByUpd]
-- 参数:
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewOrderByUpd]
@strBillNo varchar (50), --DL订单编号
@lngopUserId varchar(50),--登陆人,制单人id
@datCreateTime varchar(50),--创建日期,单据日期,预发货,预完成日期
@bytStatus varchar(50),--单据状态
@strRemarks varchar(500),--备注
@ccuscode varchar(50),--客户编码,开票单位编码
@cdefine1 varchar(50),--自定义项1
@cdefine2 varchar(50),--自定义项2
@cdefine3 varchar(500),--自定义项3
@cdefine9 varchar(50),--自定义项9
@cdefine10 varchar(50), --自定义项10	
@cdefine11 varchar(50),--自定义项11
@cdefine12 varchar(50),--自定义项12
@cdefine13 varchar(50),--自定义项13
@ccusname varchar(50),--客户名称
@cpersoncode varchar(50),--业务员编码
@cSCCode varchar(50), --发运方式编码
@datDeliveryDate varchar(50),--交货时间
@strLoadingWays varchar(50),--装车方式
@lngopUseraddressId varchar(10)--useraddress的ID


AS

BEGIN

	SET NOCOUNT ON;
declare @maxbillno int
--创建最大订单号@BillNo=@csocode=@strBillNo --销售订单号
--exec DLproc_OrderGetNewBillNoBySel 1,@strBillNo OUTPUT
--exec @strBillNo=DLproc_OrderGetNewBillNoBySel	--使用return返回值
--print @strBillNo

--获取最大单号
--select @maxbillno=isnull(max(convert(int,substring(strBillNo,5,9))),0)+1 from Dl_opOrder where strBillNo like 'DLOP%' 
--select @maxbillno
--IF @maxbillno=100000
--BEGIN
--select 'error'  'BillNo'
--END
--ELSE
--BEGIN
----创建新的订单编号,--获取两位年,两位月,组合,如1510,1509
--select @strBillNo='DLOP'+convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+right('0000'+convert(varchar(5),@maxbillno),5)
--END
--select @strBillNo='DLOP'+convert(varchar(9),@maxbillno)
--select @strBillNo
--更新表头数据
--insert into Dl_opOrder (
--lngopUserId,strBillNo,datCreateTime,bytStatus,strRemarks,ccuscode,cdefine1,cdefine2,cdefine3,cdefine8,cdefine9,cdefine10,cdefine11,cdefine12,ccusname,cpersoncode,cSCCode
--) 
--values (
--@lngopUserId,@strBillNo,@datCreateTime,@bytStatus,@strRemarks,@ccuscode,@cdefine1,@cdefine2,@cdefine3,@cdefine8,@cdefine9,@cdefine10,@cdefine11,@cdefine12,@ccusname,@cpersoncode,@cSCCode
--)
update Dl_opOrder set 
lngopUserId=@lngopUserId,datCreateTime=@datCreateTime,bytStatus=@bytStatus,strRemarks=@strRemarks,ccuscode=@ccuscode,cdefine1=@cdefine1,cdefine2=@cdefine2,cdefine3=@cdefine3,cdefine9=@cdefine9,
cdefine10=@cdefine10,cdefine11=@cdefine11,cdefine12=@cdefine12,cdefine13=@cdefine13,ccusname=@ccusname,cpersoncode=@cpersoncode,cSCCode=@cSCCode
,datDeliveryDate=@datDeliveryDate,strLoadingWays=@strLoadingWays,lngopUseraddressId=@lngopUseraddressId
 where strBillNo=@strBillNo
--返回订单表头id
--select top(1) lngopOrderId from Dl_opOrder where strBillNo=@strBillNo
select max(lngopOrderId) 'lngopOrderId' from Dl_opOrder where strBillNo=@strBillNo

END





GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewOrderDetailByIns]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [DLproc_NewOrderDetailByIns]

-- =============================================
-- Author:echo
-- Create date:2015-10-23
-- Description:	新增加订单插入表体数据[DLproc_NewOrderDetailByIns]
-- 参数:
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewOrderDetailByIns]
@lngopOrderId int,	--订单id
@cinvcode varchar(50),	--存货编码 
@iquantity decimal(18,6),	--数量 
@inum decimal(18,6),	--辅计量数量 
@iquotedprice decimal(18,6),	--报价 
@iunitprice decimal(18,6),	--原币无税单价 
@itaxunitprice decimal(18,6),	--原币含税单价 
@imoney decimal(18,6),	--原币无税金额 
@itax decimal(18,6),	--原币税额 
@isum decimal(18,6),	--原币价税合计 
@inatunitprice decimal(18,6),	--本币无税单价 
@inatmoney decimal(18,6),	--本币无税金额 
@inattax decimal(18,6),	--本币税额 
@inatsum decimal(18,6),	--本币价税合计 
@kl decimal(18,6),	--扣率 
@itaxrate decimal(18,6),	--税率 
@cdefine22 varchar(100),	--表体自定义项22 
@iinvexchrate decimal(18,6),	--换算率 
@cunitid varchar(50),	--计量单位编码 
@irowno int,	--行号 
@cinvname varchar(100),	--存货名称  
@idiscount decimal(18,6), --原币折扣额 
@inatdiscount decimal(18,6), --本币折扣额
/*11-09新增加表体字段*/
@cComUnitName varchar(20),	--基本单位名称
@cInvDefine1 varchar(20),		--大包装单位名称
@cInvDefine2 varchar(20),		--小包装单位名称
@cInvDefine13 decimal(18,6),	--大包装换算率
@cInvDefine14 decimal(18,6),	--小包装换算率
@unitGroup varchar(100),		--单位换算率组
@cComUnitQTY decimal(18,6),		--基本单位数量
@cInvDefine1QTY decimal(18,6),	--大包装单位数量
@cInvDefine2QTY decimal(18,6),	--小包装单位数量
@cn1cComUnitName varchar(20)	--销售单位名称


AS

BEGIN

	SET NOCOUNT ON;

--插入表体数据

insert into Dl_opOrderDetail(
lngopOrderId,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,inatunitprice,inatmoney,inattax,inatsum,kl,
itaxrate,cdefine22,iinvexchrate,cunitid,irowno,cinvname,idiscount,inatdiscount
,cComUnitName,cInvDefine1,cInvDefine2,cInvDefine13,cInvDefine14,unitGroup,cComUnitQTY,cInvDefine1QTY,cInvDefine2QTY,cn1cComUnitName
) 
values (
@lngopOrderId,@cinvcode,@iquantity,@inum,@iquotedprice,@iunitprice,@itaxunitprice,@imoney,@itax,@isum,@inatunitprice,@inatmoney,@inattax,@inatsum,@kl,
@itaxrate,@cdefine22,@iinvexchrate,@cunitid,@irowno,@cinvname,@idiscount,@inatdiscount
,@cComUnitName,@cInvDefine1,@cInvDefine2,@cInvDefine13,@cInvDefine14,@unitGroup,@cComUnitQTY,@cInvDefine1QTY,@cInvDefine2QTY,@cn1cComUnitName
)

END







GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewOrderDetailByUpd]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [DLproc_NewOrderDetailByIns]

-- =============================================
-- Author:echo
-- Create date:2015-10-23
-- Description:	更新订单插入表体数据[DLproc_NewOrderDetailByUpd]
-- 参数:
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewOrderDetailByUpd]
@lngopOrderId int,	--订单id
@cinvcode varchar(50),	--存货编码 
@iquantity decimal(18,6),	--数量 
@inum decimal(18,6),	--辅计量数量 
@iquotedprice decimal(18,6),	--报价 
@iunitprice decimal(18,6),	--原币无税单价 
@itaxunitprice decimal(18,6),	--原币含税单价 
@imoney decimal(18,6),	--原币无税金额 
@itax decimal(18,6),	--原币税额 
@isum decimal(18,6),	--原币价税合计 
@inatunitprice decimal(18,6),	--本币无税单价 
@inatmoney decimal(18,6),	--本币无税金额 
@inattax decimal(18,6),	--本币税额 
@inatsum decimal(18,6),	--本币价税合计 
@kl decimal(18,6),	--扣率 
@itaxrate decimal(18,6),	--税率 
@cdefine22 varchar(100),	--表体自定义项22 
@iinvexchrate decimal(18,6),	--换算率 
@cunitid varchar(50),	--计量单位编码 
@irowno int,	--行号 
@cinvname varchar(100),	--存货名称  
@idiscount decimal(18,6), --原币折扣额 
@inatdiscount decimal(18,6), --本币折扣额
/*11-09新增加表体字段*/
@cComUnitName varchar(20),	--基本单位名称
@cInvDefine1 varchar(20),		--大包装单位名称
@cInvDefine2 varchar(20),		--小包装单位名称
@cInvDefine13 decimal(18,6),	--大包装换算率
@cInvDefine14 decimal(18,6),	--小包装换算率
@unitGroup varchar(100),		--单位换算率组
@cComUnitQTY decimal(18,6),		--基本单位数量
@cInvDefine1QTY decimal(18,6),	--大包装单位数量
@cInvDefine2QTY decimal(18,6),	--小包装单位数量
@cn1cComUnitName varchar(20)	--销售单位名称


AS

BEGIN

	SET NOCOUNT ON;

--插入表体数据

insert into Dl_opOrderDetail(
lngopOrderId,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,inatunitprice,inatmoney,inattax,inatsum,kl,
itaxrate,cdefine22,iinvexchrate,cunitid,irowno,cinvname,idiscount,inatdiscount
,cComUnitName,cInvDefine1,cInvDefine2,cInvDefine13,cInvDefine14,unitGroup,cComUnitQTY,cInvDefine1QTY,cInvDefine2QTY,cn1cComUnitName
) 
values (
@lngopOrderId,@cinvcode,@iquantity,@inum,@iquotedprice,@iunitprice,@itaxunitprice,@imoney,@itax,@isum,@inatunitprice,@inatmoney,@inattax,@inatsum,@kl,
@itaxrate,@cdefine22,@iinvexchrate,@cunitid,@irowno,@cinvname,@idiscount,@inatdiscount
,@cComUnitName,@cInvDefine1,@cInvDefine2,@cInvDefine13,@cInvDefine14,@unitGroup,@cComUnitQTY,@cInvDefine1QTY,@cInvDefine2QTY,@cn1cComUnitName
)

END







GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewOrderDetailU8ByIns]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [DLproc_NewOrderDetailU8ByIns]

-- =============================================
-- Author:echo
-- Create date:2015-10-23
-- Description:	生成U8订单表体数据[DLproc_NewOrderDetailU8ByIns]
-- 参数:
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewOrderDetailU8ByIns]
@strBillNo varchar(50)

AS

BEGIN

	SET NOCOUNT ON;


--插入表头数据

--insert into SO_SODetails(
--cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,inatunitprice,inatmoney,inattax,inatsum,kl,itaxrate,cdefine22,iinvexchrate,cunitid,irowno,cinvname
--) 
--cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,inatunitprice,inatmoney,inattax,inatsum,kl,itaxrate,cdefine22,iinvexchrate,cunitid,irowno,cinvname
--from Dl_opOrderDetail




--select * from Dl_opOrderDetail

END






GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewOrderU8ByIns]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec DLproc_NewOrderU8ByIns 'DLOP110400016'

-- =============================================
-- Author:echo
-- Create date:2015-10-23
-- Description:	生成U8订单表头数据[DLproc_NewOrderU8ByIns]
-- 参数:@strBillNo:DLorder中的单据编号,生成U8订单,并返回U8订单的单据号
--2015-12-12,add:cpreordercode 预订单号,iaoids (U872)  预订单autoid  ,添加到insert中,先判断是否有值
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewOrderU8ByIns]
@strBillNo varchar(50)--DL单据编号
--@ddate varchar(50),--单据日期
--@ccuscode varchar(50),--客户编码 
--@cdefine1 varchar(50),--自定义项1
--@cdefine2 varchar(50),--自定义项2
--@cdefine3 varchar(500),--自定义项3
--@cdefine8 varchar(50),--自定义项8
--@cdefine9 varchar(50), --自定义项9
--@cdefine10 varchar(50),--自定义项10
--@cdefine11 varchar(50),--自定义项11
--@cdefine12 varchar(50),--自定义项12
--@dpredatebt varchar(50),--预发货日期 
--@dpremodatebt varchar(50),--预完工日期
--@ccusname varchar(50),--客户名称
--@cinvoicecompany varchar(50),--开票单位编码
--@cmemo varchar(500),--备注 
--@cpersoncode varchar(50)--业务员编码
AS

BEGIN

	--SET NOCOUNT ON;


declare @csocode varchar(50),@csysbarcode varchar(50),@maxbillno int,@id varchar(50),@cbsysbarcodeDetail varchar(50),@ddate varchar(50),@lngopOrderId varchar(50),
@isosid varchar(50),@bytStatus varchar(2),@cmaker varchar(10),@lngopUseraddressId varchar(10),@cdefine11 varchar(500),@cSCCode varchar(10),@icount int,
@p5 varchar(50),@p6 varchar(50)
--useraddress的ID
--@cDefine9 varchar(20),	--收货人姓名
--@cDefine12 varchar(20),	--收货人电话
--@cDefine11 varchar(150),--收货人地址
--@cDefine10 varchar(50),	--车牌号	
--@cDefine1 varchar(20),	--司机姓名
--@cDefine13 varchar(20),	--司机电话
--@cDefine2 varchar(30)	--司机身份证
/*2015-11-04,判断是否已经生成过U8订单,避免重复生成*/
select @bytStatus=bytStatus from Dl_opOrder where strBillNo=@strBillNo
if @bytStatus=1 or @bytStatus=2
begin

--创建最大订单号@BillNo=@csocode --销售订单号
--exec DLproc_OrderGetNewBillNoBySelU8 @csocode OUTPUT

/*2016-02-01注视,启用新编号获取方式start*/
----获取最大单号
--select @maxbillno=isnull(max(convert(int,replace(csocode,'W',''))+1),convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+'00001')  from SO_SOMain where csocode like 'W1%' or csocode like 'W2%'  
----IF @maxbillno=100000
----BEGIN
------select 'error'  'BillNo'
----set @csocode='error'
----END
----ELSE
----BEGIN
------创建新的订单编号,--获取两位年,两位月,组合,如1510,
----select @csocode='WSXD'+convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+right('0000'+convert(varchar(5),@maxbillno),5)
----END
--select @csocode='W'+convert(varchar(10),@maxbillno)
/*2016-02-01注视,启用新编号获取方式,end*/

--2016-02-01获取单据编号方式
exec DLproc_GetBillNo '6',@csocode OUTPUT

--组合字段:@csysbarcode 表头单据条码 --表头单据条码 
set @csysbarcode='||SA17|'+@csocode
--set @csysbarcode='N'+''''+'||SA17|'+@csocode+''''
--set @csocode='N'+''''+@csocode+''''

/*插入表头数据V1.0*/
--insert into SO_SOMain (
----主动数据
--csysbarcode,ddate,csocode,ccuscode,cdefine1,cdefine2,cdefine3,cdefine8,cdefine9,cdefine10,cdefine11,cdefine12,dpredatebt,dpremodatebt,ccusname,cinvoicecompany,cmemo,cpersoncode,id,
----被动数据
--cstcode,cdepcode,cexch_name,iexchrate,itaxrate,cbustype,cmaker,bdisflag,breturnflag,iverifystate,iswfcontrolled,bcashsale,bmustbook,ivtid,
----null
--caddcode,ccrmpersoncode,ccrmpersonname,cmainpersoncode,ccurrentauditor,cchanger,ccrechpname,outid,csccode,ccusoaddress,cpaycode,imoney,istatus,cverifier,ccloser,cdefine5,cdefine7,
--cdefine13,cdefine14,cdefine15,cdefine16,clocker,ccusperson,coppcode,cmodifier,ccuspersoncode,ccontactname,cmobilephone,iflowid,cgatheringplan,ireturncount,icreditstate,cchangeverifier,
--cgathingcode,iprintcount,fbookratio,fbooksum,fbooknatsum,fgbooksum,fgbooknatsum,contract_status,optnty_name,ioppid,csvouchtype,csscode,cattachment,cebtrnumber,cebbuyer,cebbuyernote,
--cebprovince,cebcity,cebdistrict,cinvoicecusname
--)
--values (
----主动数据
--@csysbarcode,@ddate,@csocode,@ccuscode,@cdefine1,@cdefine2,@cdefine3,@cdefine8,@cdefine9,@cdefine10,@cdefine11,@cdefine12,@dpredatebt,
--@dpremodatebt,@ccusname,@cinvoicecompany,@cmemo,@cpersoncode,@id,
----被动数据
--N'00',N'0603',N'人民币',1,17,N'普通销售',N'demo',0,0,0,1,0,0,131395,
----null
--Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,
--Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null
--)

--获取表头最大id,2015改用exec sp_getID '00','002','Somain',1,@p5 output,@p6 output获取表头ID和表体ID
--select @id=isnull(MAX(id),0)+1 from SO_SOMain
select @ddate=datCreateTime,@lngopOrderId=lngopOrderId from Dl_opOrder where strBillNo=@strBillNo
select @icount=count(lngopOrderId) from Dl_opOrderDetail where lngopOrderId=@lngopOrderId
set @p5=''
set @p6=''
exec sp_getID '00','003','Somain',@icount,@p5 output,@p6 output
select @isosid=@p6-@icount,@id=@p5
/*获取地址信息,2015-11-21增加*/
select @lngopUseraddressId=lngopUseraddressId from Dl_opOrder where strBillNo=@strBillNo
--select 
--@cDefine9=strConsigneeName,	 
--@cDefine12=strConsigneeTel,	 		
--@cDefine11=strReceivingAddress,	 	 
--@cDefine10=strCarplateNumber,	 				
--@cDefine1=strDriverName,	 		
--@cDefine13=strDriverTel,	 		
--@cDefine2=strIdCard from Dl_opUserAddress where lngopUseraddressId=@lngopUseraddressId	
/*2015-11-24,获取订单员的U8编号*/	
select @cmaker=p.cPersonName from Dl_opOrder  do
left join Dl_opUser dou on do.strManagers=dou.lngopUserId
left join Person p on dou.strLoginName=p.cPersonCode
where strBillNo=@strBillNo

/*插入U8表头数据V2.0*/
insert into SO_SOMain(
--主动数据
csysbarcode,ddate,csocode,ccuscode,cdefine1,cdefine2,cdefine3,cdefine8,cdefine9,cdefine10,cdefine11,cdefine12,cdefine13,dpredatebt,dpremodatebt,ccusname,cinvoicecompany,cmemo,
cpersoncode,id,cSCCode,cDefine4, cDefine14, cSTCode,cDefine6,cmaker,
--被动数据
cdepcode,cexch_name,iexchrate,itaxrate,cbustype,bdisflag,breturnflag,iverifystate,iswfcontrolled,bcashsale,bmustbook,ivtid,
--null
caddcode,ccrmpersoncode,ccrmpersonname,cmainpersoncode,ccurrentauditor,cchanger,ccrechpname,outid,ccusoaddress,cpaycode,imoney,istatus,cverifier,ccloser,cdefine5,cdefine7,
cdefine15,cdefine16,clocker,ccusperson,coppcode,cmodifier,ccuspersoncode,ccontactname,cmobilephone,iflowid,cgatheringplan,ireturncount,icreditstate,cchangeverifier,
cgathingcode,iprintcount,fbookratio,fbooksum,fbooknatsum,fgbooksum,fgbooknatsum,contract_status,optnty_name,ioppid,csvouchtype,csscode,cattachment,cebtrnumber,cebbuyer,cebbuyernote,
cebprovince,cebcity,cebdistrict,cinvoicecusname
)
select 
--主动数据
@csysbarcode,convert(varchar(10),datCreateTime,120),@csocode,ccuscode,cdefine1,cdefine2,cdefine3,cdefine8,cdefine9,cdefine10,
right(cdefine11,charindex(',',reverse(cdefine11))-1),cdefine12,cdefine13,convert(varchar(10),datCreateTime,120),
convert(varchar(10),datCreateTime,120),ccusname,ccuscode,strRemarks,cpersoncode,@id,cSCCode,datDeliveryDate, strLoadingWays, cSTCode,datBillTime,@cmaker,
--被动数据
N'0601',N'人民币',1,17,N'普通销售',0,0,0,1,0,0,131395,
--null
Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,
Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null 
from Dl_opOrder where strBillNo=@strBillNo 
/*2015-12-02 更新送货地址详情,如果为自提,则清除地址*/
update SO_SOMain set cdefine11=' ' where csocode=@csocode and cSCCode='00'


--插入扩展字段:(地址id对应的省市区信息)/*2015-11-21 增加*/;11-26修改,添加新的扩展字段2,样品资料关联U8的订单号
--insert into SO_SOMain_ExtraDefine (ID,chdefine1) select @id,strDistrict from Dl_opUserAddress where lngopUseraddressId=@lngopUseraddressId
declare @RelateU8NO varchar(50)
select @RelateU8NO=cSOCode from Dl_opOrder where strBillNo=(select RelateU8NO from Dl_opOrder where strBillNo=@strBillNo)
insert into SO_SOMain_ExtraDefine (ID,chdefine1,chdefine2,chdefine13) select @id,strDistrict,@RelateU8NO,'网上下单' from Dl_opUserAddress
where lngopUseraddressId=@lngopUseraddressId 

--插入U8表体数据V2.0
--插入U8表体数据V3.0,2015-12-12 modtify
--获取 cpreordercode 和 iaoids (U872)  预订单autoid 
	--完成表头数据插入之后,获取表头的:销售订单主表标识
set @cbsysbarcodeDetail=@csysbarcode+'|'
--select @isosid=isnull(max(isosid),1) from SO_SODetails
----将dl_oporderDetail数据插入到U8表体中
insert into SO_SODetails(
--变量
isosid,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,inatunitprice,inatmoney,inattax,inatsum,kl,itaxrate,cdefine22,iinvexchrate,cunitid,id,irowno,cinvname,cSOCode,cpreordercode,iaoids
--常量
,cbsysbarcode,dpredate,idiscount,inatdiscount,kl2,fsalecost,fsaleprice,dpremodate,fcusminprice,ballpurchase,borderbom,borderbomover,busecusbom,bsaleprice,bgift
--null值
,ifhnum,ifhquantity,ifhmoney,ikpquantity,ikpnum,ikpmoney,cmemo,cfree1,cfree2,cdefine23,cdefine24,cdefine25,cdefine26,cdefine27,citemcode,citem_class,citemname,citem_cname,cfree3,
cfree4,cfree5,cfree6,cfree7,cfree8,cfree9,cfree10,cdefine28,cdefine29,cdefine30,cdefine31,cdefine32,cdefine33,cdefine34,cdefine35,fpurquan,cquocode,iquoid,cscloser,icusbomid,
imoquantity,ccontractid,ccontracttagcode,ccontractrowguid,ippartseqid,ippartid,ippartqty,ccusinvcode,ccusinvname,iprekeepquantity,iprekeepnum,iprekeeptotquantity,iprekeeptotnum,
fimquantity,fomquantity,finquantity,foutquantity,iexchsum,imoneysum,fretquantity,fretnum,idemandtype,cdemandcode,cdemandmemo,iimid,ccorvouchtype,icorrowno,
body_outid,forecastdid,cdetailsdemandcode,cdetailsdemandmemo,fappqty,cparentcode,cchildcode,fchildqty,fchildrate,icalctype
)
select @isosid+irowno,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,inatunitprice,inatmoney,inattax,inatsum,kl,itaxrate,cdefine22,iinvexchrate,cunitid,@id,irowno,cinvname,@csocode,cpreordercode,iaoids
,@cbsysbarcodeDetail+convert(varchar(5),irowno),@ddate,0,0,100,0,0,@ddate,0,0,0,0,0,1,0
,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,
Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,
Null,Null,Null,Null,Null,Null,Null,Null
from Dl_opOrderDetail
where lngopOrderId=@lngopOrderId
--更新dl_oporder表头中的U8订单编号,并且更新dl订单的状态为4:已提交审核状态//1,提交,待审核;2,已修改,待顾客确认;3,提交,被驳回;,4,提交,已审核,生成U8订单
--11-16,并且更新订单表头字段strManagers,操作员代码,
update Dl_opOrder set cSOCode=@csocode,bytStatus=4,datAuditordTime=GETDATE(),strAuditor=strManagers where strBillNo=@strBillNo
end
END

GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewOrderU8ByIns_test]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec DLproc_NewOrderU8ByIns 'DLOP110400016'

-- =============================================
-- Author:echo
-- Create date:2015-10-23
-- Description:	生成U8订单表头数据[DLproc_NewOrderU8ByIns]
-- 参数:@strBillNo:DLorder中的单据编号,生成U8订单,并返回U8订单的单据号
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewOrderU8ByIns_test]
@strBillNo varchar(50)--DL单据编号
--@ddate varchar(50),--单据日期
--@ccuscode varchar(50),--客户编码 
--@cdefine1 varchar(50),--自定义项1
--@cdefine2 varchar(50),--自定义项2
--@cdefine3 varchar(500),--自定义项3
--@cdefine8 varchar(50),--自定义项8
--@cdefine9 varchar(50), --自定义项9
--@cdefine10 varchar(50),--自定义项10
--@cdefine11 varchar(50),--自定义项11
--@cdefine12 varchar(50),--自定义项12
--@dpredatebt varchar(50),--预发货日期 
--@dpremodatebt varchar(50),--预完工日期
--@ccusname varchar(50),--客户名称
--@cinvoicecompany varchar(50),--开票单位编码
--@cmemo varchar(500),--备注 
--@cpersoncode varchar(50)--业务员编码
AS

BEGIN

	SET NOCOUNT ON;

declare @csocode varchar(50),@csysbarcode varchar(50),@maxbillno int,@id varchar(50),@cbsysbarcodeDetail varchar(50),@ddate varchar(50),@lngopOrderId varchar(50),
@isosid varchar(50),@bytStatus varchar(2),@cmaker varchar(10),@lngopUseraddressId varchar(10),@cdefine11 varchar(500),@cSCCode varchar(10),@icount int,
@p5 varchar(50),@p6 varchar(50)
--useraddress的ID
--@cDefine9 varchar(20),	--收货人姓名
--@cDefine12 varchar(20),	--收货人电话
--@cDefine11 varchar(150),--收货人地址
--@cDefine10 varchar(50),	--车牌号	
--@cDefine1 varchar(20),	--司机姓名
--@cDefine13 varchar(20),	--司机电话
--@cDefine2 varchar(30)	--司机身份证
/*2015-11-04,判断是否已经生成过U8订单,避免重复生成*/
select @bytStatus=bytStatus from Dl_opOrder where strBillNo=@strBillNo
if @bytStatus=1 or @bytStatus=2
begin

--创建最大订单号@BillNo=@csocode --销售订单号
--exec DLproc_OrderGetNewBillNoBySelU8 @csocode OUTPUT


--获取最大单号
select @maxbillno=isnull(max(convert(int,replace(csocode,'W',''))+1),convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+'00001')  from SO_SOMain where csocode like 'W1%' or csocode like 'W2%'  
--IF @maxbillno=100000
--BEGIN
----select 'error'  'BillNo'
--set @csocode='error'
--END
--ELSE
--BEGIN
----创建新的订单编号,--获取两位年,两位月,组合,如1510,
--select @csocode='WSXD'+convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+right('0000'+convert(varchar(5),@maxbillno),5)
--END
select @csocode='W'+convert(varchar(10),@maxbillno)


--组合字段:@csysbarcode 表头单据条码 --表头单据条码 
set @csysbarcode='||SA17|'+@csocode
--set @csysbarcode='N'+''''+'||SA17|'+@csocode+''''
--set @csocode='N'+''''+@csocode+''''

/*插入表头数据V1.0*/
--insert into SO_SOMain (
----主动数据
--csysbarcode,ddate,csocode,ccuscode,cdefine1,cdefine2,cdefine3,cdefine8,cdefine9,cdefine10,cdefine11,cdefine12,dpredatebt,dpremodatebt,ccusname,cinvoicecompany,cmemo,cpersoncode,id,
----被动数据
--cstcode,cdepcode,cexch_name,iexchrate,itaxrate,cbustype,cmaker,bdisflag,breturnflag,iverifystate,iswfcontrolled,bcashsale,bmustbook,ivtid,
----null
--caddcode,ccrmpersoncode,ccrmpersonname,cmainpersoncode,ccurrentauditor,cchanger,ccrechpname,outid,csccode,ccusoaddress,cpaycode,imoney,istatus,cverifier,ccloser,cdefine5,cdefine7,
--cdefine13,cdefine14,cdefine15,cdefine16,clocker,ccusperson,coppcode,cmodifier,ccuspersoncode,ccontactname,cmobilephone,iflowid,cgatheringplan,ireturncount,icreditstate,cchangeverifier,
--cgathingcode,iprintcount,fbookratio,fbooksum,fbooknatsum,fgbooksum,fgbooknatsum,contract_status,optnty_name,ioppid,csvouchtype,csscode,cattachment,cebtrnumber,cebbuyer,cebbuyernote,
--cebprovince,cebcity,cebdistrict,cinvoicecusname
--)
--values (
----主动数据
--@csysbarcode,@ddate,@csocode,@ccuscode,@cdefine1,@cdefine2,@cdefine3,@cdefine8,@cdefine9,@cdefine10,@cdefine11,@cdefine12,@dpredatebt,
--@dpremodatebt,@ccusname,@cinvoicecompany,@cmemo,@cpersoncode,@id,
----被动数据
--N'00',N'0603',N'人民币',1,17,N'普通销售',N'demo',0,0,0,1,0,0,131395,
----null
--Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,
--Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null
--)

--获取表头最大id,2015改用exec sp_getID '00','002','Somain',1,@p5 output,@p6 output获取表头ID和表体ID
--select @id=isnull(MAX(id),0)+1 from SO_SOMain
select @icount=count(lngopOrderId) from Dl_opOrderDetail where lngopOrderId=@lngopOrderId
set @p5=''
set @p6=''
exec sp_getID '00','002','Somain',@icount,@p5 output,@p6 output
select @isosid=@p6-@icount-1,@id=@p5


 
/*获取地址信息,2015-11-21增加*/
select @lngopUseraddressId=lngopUseraddressId from Dl_opOrder where strBillNo=@strBillNo
--select 
--@cDefine9=strConsigneeName,	 
--@cDefine12=strConsigneeTel,	 		
--@cDefine11=strReceivingAddress,	 	 
--@cDefine10=strCarplateNumber,	 				
--@cDefine1=strDriverName,	 		
--@cDefine13=strDriverTel,	 		
--@cDefine2=strIdCard from Dl_opUserAddress where lngopUseraddressId=@lngopUseraddressId	
/*2015-11-24,获取订单员的U8编号*/	
select @cmaker=p.cPersonName from Dl_opOrder  do
left join Dl_opUser dou on do.strManagers=dou.lngopUserId
left join Person p on dou.strLoginName=p.cPersonCode
where strBillNo=@strBillNo

/*插入U8表头数据V2.0*/
insert into SO_SOMain(
--主动数据
csysbarcode,ddate,csocode,ccuscode,cdefine1,cdefine2,cdefine3,cdefine8,cdefine9,cdefine10,cdefine11,cdefine12,cdefine13,dpredatebt,dpremodatebt,ccusname,cinvoicecompany,cmemo,
cpersoncode,id,cSCCode,cDefine4, cDefine14, cSTCode,cDefine6,cmaker,
--被动数据
cdepcode,cexch_name,iexchrate,itaxrate,cbustype,bdisflag,breturnflag,iverifystate,iswfcontrolled,bcashsale,bmustbook,ivtid,
--null
caddcode,ccrmpersoncode,ccrmpersonname,cmainpersoncode,ccurrentauditor,cchanger,ccrechpname,outid,ccusoaddress,cpaycode,imoney,istatus,cverifier,ccloser,cdefine5,cdefine7,
cdefine15,cdefine16,clocker,ccusperson,coppcode,cmodifier,ccuspersoncode,ccontactname,cmobilephone,iflowid,cgatheringplan,ireturncount,icreditstate,cchangeverifier,
cgathingcode,iprintcount,fbookratio,fbooksum,fbooknatsum,fgbooksum,fgbooknatsum,contract_status,optnty_name,ioppid,csvouchtype,csscode,cattachment,cebtrnumber,cebbuyer,cebbuyernote,
cebprovince,cebcity,cebdistrict,cinvoicecusname
)
select 
--主动数据
@csysbarcode,convert(varchar(10),datCreateTime,120),@csocode,ccuscode,cdefine1,cdefine2,cdefine3,cdefine8,cdefine9,cdefine10,
right(cdefine11,charindex(',',reverse(cdefine11))-1),cdefine12,cdefine13,convert(varchar(10),datCreateTime,120),
convert(varchar(10),datCreateTime,120),ccusname,ccuscode,strRemarks,cpersoncode,@id,cSCCode,datDeliveryDate, strLoadingWays, cSTCode,datBillTime,@cmaker,
--被动数据
N'0601',N'人民币',1,17,N'普通销售',0,0,0,1,0,0,131395,
--null
Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,
Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null 
from Dl_opOrder where strBillNo=@strBillNo 
/*2015-12-02 更新送货地址详情,如果为自提,则清除地址*/
update SO_SOMain set cdefine11=' ' where csocode=@csocode and cSCCode='00'


--插入扩展字段:(地址id对应的省市区信息)/*2015-11-21 增加*/;11-26修改,添加新的扩展字段2,样品资料关联U8的订单号
--insert into SO_SOMain_ExtraDefine (ID,chdefine1) select @id,strDistrict from Dl_opUserAddress where lngopUseraddressId=@lngopUseraddressId
insert into SO_SOMain_ExtraDefine (ID,chdefine1,chdefine2) select @id,dou.strDistrict,do.RelateU8NO from Dl_opUserAddress dou,Dl_opOrder do 
where dou.lngopUseraddressId=@lngopUseraddressId and do.strBillNo=@strBillNo

--插入U8表体数据V2.0
	--完成表头数据插入之后,获取表头的:销售订单主表标识
set @cbsysbarcodeDetail=@csysbarcode+'|'
select @ddate=datCreateTime,@lngopOrderId=lngopOrderId from Dl_opOrder where strBillNo=@strBillNo
--select @isosid=isnull(max(isosid),1) from SO_SODetails
--将dl_oporderDetail数据插入到U8表体中
insert into SO_SODetails(
--变量
isosid,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,inatunitprice,inatmoney,inattax,inatsum,kl,itaxrate,cdefine22,iinvexchrate,cunitid,id,irowno,cinvname,cSOCode
--常量
,cbsysbarcode,dpredate,idiscount,inatdiscount,kl2,fsalecost,fsaleprice,dpremodate,fcusminprice,ballpurchase,borderbom,borderbomover,busecusbom,bsaleprice,bgift
--null值
,ifhnum,ifhquantity,ifhmoney,ikpquantity,ikpnum,ikpmoney,cmemo,cfree1,cfree2,cdefine23,cdefine24,cdefine25,cdefine26,cdefine27,citemcode,citem_class,citemname,citem_cname,cfree3,
cfree4,cfree5,cfree6,cfree7,cfree8,cfree9,cfree10,cdefine28,cdefine29,cdefine30,cdefine31,cdefine32,cdefine33,cdefine34,cdefine35,fpurquan,cquocode,iquoid,cscloser,icusbomid,
imoquantity,ccontractid,ccontracttagcode,ccontractrowguid,ippartseqid,ippartid,ippartqty,ccusinvcode,ccusinvname,iprekeepquantity,iprekeepnum,iprekeeptotquantity,iprekeeptotnum,
fimquantity,fomquantity,finquantity,foutquantity,iexchsum,imoneysum,iaoids,cpreordercode,fretquantity,fretnum,idemandtype,cdemandcode,cdemandmemo,iimid,ccorvouchtype,icorrowno,
body_outid,forecastdid,cdetailsdemandcode,cdetailsdemandmemo,fappqty,cparentcode,cchildcode,fchildqty,fchildrate,icalctype
)
select @isosid+irowno,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,inatunitprice,inatmoney,inattax,inatsum,kl,itaxrate,cdefine22,iinvexchrate,cunitid,@id,irowno,cinvname,@csocode
,@cbsysbarcodeDetail+convert(varchar(5),irowno),@ddate,0,0,100,0,0,@ddate,0,0,0,0,0,1,0
,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,
Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,
Null,Null,Null,Null,Null,Null,Null,Null
from Dl_opOrderDetail
where lngopOrderId=@lngopOrderId
--更新dl_oporder表头中的U8订单编号,并且更新dl订单的状态为4:已提交审核状态//1,提交,待审核;2,已修改,待顾客确认;3,提交,被驳回;,4,提交,已审核,生成U8订单
--11-16,并且更新订单表头字段strManagers,操作员代码,

update Dl_opOrder set cSOCode=@csocode,bytStatus=4 where strBillNo=@strBillNo



end


END




GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewSampleOrderByIns]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [DLproc_NewSampleOrderByIns]

-- =============================================
-- Author:echo
-- Create date:2015-10-23
-- Description:	新增加订单表头插入数据(样品资料)[[DLproc_NewSampleOrderByIns]]
-- 参数:
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewSampleOrderByIns]
--@lngopUserId varchar(50),--登陆人,制单人id
--@datCreateTime varchar(50),--创建日期,单据日期,预发货,预完成日期
--@bytStatus varchar(50),--单据状态
--@strRemarks varchar(500),--备注
--@ccuscode varchar(50),--客户编码,开票单位编码
--@cdefine1 varchar(50),--自定义项1
--@cdefine2 varchar(50),--自定义项2
--@cdefine3 varchar(500),--自定义项3
--@cdefine9 varchar(50), --自定义项9
--@cdefine10 varchar(50),--自定义项10
--@cdefine11 varchar(200),--自定义项11
--@cdefine12 varchar(50),--自定义项12
--@cdefine13 varchar(50),--自定义项13
--@ccusname varchar(50),--客户名称
--@cpersoncode varchar(50),--业务员编码
--@cSCCode varchar(50), --发运方式编码
--@datDeliveryDate varchar(50), --交货日期
--@strLoadingWays varchar(100), --装车方式
--@cSTCode varchar(10),		--销售类型
--@lngopUseraddressId varchar(10),--useraddress的ID
@strU8BillNo varchar(30),	--U8单据号,样品资料写入,改为DL系统单据号
@strRemarks varchar(500),--备注
@strLoadingWays varchar(100) --装车方式
AS

BEGIN

	SET NOCOUNT ON;
declare @strBillNo varchar(50),@maxbillno int
--创建最大订单号@BillNo=@csocode=@strBillNo --销售订单号
--exec DLproc_OrderGetNewBillNoBySel 1,@strBillNo OUTPUT
--exec @strBillNo=DLproc_OrderGetNewBillNoBySel	--使用return返回值
--print @strBillNo

/*2016-01-25更新获取单据编号方式*/--begin
--获取最大单号
--select @maxbillno=isnull(max(convert(int,substring(strBillNo,5,9))),0)+1 from Dl_opOrder where strBillNo like 'DLOP%' 
----select @maxbillno
----IF @maxbillno=100000
----BEGIN
----select 'error'  'BillNo'
----END
----ELSE
----BEGIN
------创建新的订单编号,--获取两位年,两位月,组合,如1510,1509
----select @strBillNo='DLOP'+convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+right('0000'+convert(varchar(5),@maxbillno),5)
----END
--select @strBillNo='DLOP'+convert(varchar(9),@maxbillno)
/*2016-01-25更新获取单据编号方式*/--end
--select @strBillNo
--2016-01-25获取单据编号方式
exec DLproc_GetBillNo '1',@strBillNo OUTPUT
select @strBillNo=@strBillNo
--插入表头数据
insert into Dl_opOrder (
lngopUserId,strBillNo,datCreateTime,bytStatus,strRemarks,ccuscode,cdefine1,cdefine2,cdefine3,cdefine9,cdefine10,cdefine11,cdefine12,cdefine13,
ccusname,cpersoncode,cSCCode,datDeliveryDate, strLoadingWays,cSTCode,lngopUseraddressId,RelateU8NO
) 
select 
lngopUserId,@strBillNo,datCreateTime,1,@strRemarks,ccuscode,cdefine1,cdefine2,cdefine3,cdefine9,cdefine10,cdefine11,cdefine12,cdefine13,
ccusname,cpersoncode,cSCCode,datDeliveryDate, @strLoadingWays, '01',lngopUseraddressId,@strU8BillNo from Dl_opOrder where strBillNo=@strU8BillNo

----20151129添加,清除临时授权,
--update Customer set cCusEmail=' ' where cCusCode=@ccuscode --客户编码,开票单位编码

--20151129添加,样品资料直接给关联的U8订单专员负责,如果订单员已经接单,则直接关联转接

declare @strManagers varchar(10)
select @strManagers=isnull(strManagers,' ') from Dl_opOrder where strBillNo=@strU8BillNo
update Dl_opOrder set strManagers=@strManagers where strBillNo=@strBillNo


--返回订单表头id
--select top(1) lngopOrderId from Dl_opOrder where strBillNo=@strBillNo
select max(lngopOrderId) 'lngopOrderId',@strBillNo 'strBillNo' from Dl_opOrder where strBillNo=@strBillNo

END




GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewYOrderByIns]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec DLproc_NewOrderByIns

-- =============================================
-- Author:echo
-- Create date:2015-12-09
-- Description:	新增加销售预订单表头插入数据[DLproc_NewYOrderByIns]
-- 参数:
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewYOrderByIns]

--@strBillNo varchar(50)	,		--系统编号
@ddate varchar(50)	,				--订单日期
--@ccode varchar(50)	,			--单据号
--@csysbarcode varchar(50)	,		--单据条码 
@lngopUserId varchar(6)	,		--系统用户id
--@cmaker varchar(6),				--制单人
@bytStatus varchar(6),			--单据状态
@ccuscode varchar(20),			--开票单位编码
@ccusname	 varchar(80),		--开票单位名称
--@datBillTime		下单时间
@lngBillType int



AS

BEGIN

	SET NOCOUNT ON;
declare @strBillNo varchar(50),@maxbillno bigint
/*2016-01-25更新获取单据编号方式*/--begin
----创建最大订单号@BillNo=@csocode=@strBillNo --销售预定订单号
----获取最大单号
--select @maxbillno=isnull(max(convert(int,substring(strBillNo,6,11))),0)+1 from Dl_opPreOrder where strBillNo like 'Y%' 

--select @strBillNo='Y'+ convert(varchar(4),year(GETDATE()))+convert(varchar(9),@maxbillno)
----select @strBillNo
----select @csysbarcode='||SA26|'+@strBillNo
/*2016-01-25更新获取单据编号方式*/--end
--2016-01-25获取单据编号方式
if @lngBillType=1 
begin	
exec DLproc_GetBillNo '4',@strBillNo OUTPUT
end
if @lngBillType=2
begin	
exec DLproc_GetBillNo '5',@strBillNo OUTPUT
end
--插入表头数据
insert into Dl_opPreOrder (
strBillNo,ddate,lngopUserId,bytStatus,ccuscode,ccusname,datBillTime,lngBillType
) 
select 
@strBillNo,convert(varchar(10),GETDATE(),120),@lngopUserId,@bytStatus,@ccuscode,@ccusname, convert(varchar(20),GETDATE(),120),@lngBillType


----20151129添加,清除临时授权,
--update Customer set cCusEmail=' ' where cCusCode=@ccuscode --客户编码,开票单位编码

--返回订单表头id
--select top(1) lngopOrderId from Dl_opPreOrder where strBillNo=@strBillNo
select max(lngPreOrderId) 'lngopOrderId',@strBillNo 'strBillNo' from Dl_opPreOrder where strBillNo=@strBillNo

END




GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewYOrderByUpd]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec DLproc_NewOrderByIns

-- =============================================
-- Author:echo
-- Create date:2015-11-13
-- Description:	修改(更新)订单表头插入数据[DLproc_NewYOrderByUpd]
-- 参数:
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewYOrderByUpd]
@strBillNo varchar (50), --DL订单编号
@lngopUserId varchar(50),--登陆人,制单人id
@datCreateTime varchar(50),--创建日期,单据日期,预发货,预完成日期
@bytStatus varchar(50),--单据状态
@strRemarks varchar(500),--备注
@ccuscode varchar(50),--客户编码,开票单位编码
@cdefine1 varchar(50),--自定义项1
@cdefine2 varchar(50),--自定义项2
@cdefine3 varchar(500),--自定义项3
@cdefine9 varchar(50),--自定义项9
@cdefine10 varchar(50), --自定义项10	
@cdefine11 varchar(50),--自定义项11
@cdefine12 varchar(50),--自定义项12
@cdefine13 varchar(50),--自定义项13
@ccusname varchar(50),--客户名称
@cpersoncode varchar(50),--业务员编码
@cSCCode varchar(50), --发运方式编码
@datDeliveryDate varchar(50),--交货时间
@strLoadingWays varchar(50),--装车方式
@lngopUseraddressId varchar(10)--useraddress的ID


AS

BEGIN

	SET NOCOUNT ON;
declare @maxbillno int

--根据@lngopUseraddressId获取对应的发货信息,然后更新
select @cdefine1=strDriverName,@cdefine2=strIdCard,@cdefine9=strConsigneeName,@cdefine10=strCarplateNumber,@cdefine12=strConsigneeTel,@cdefine13=strDriverTel from Dl_opUserAddress where lngopUseraddressId=@lngopUseraddressId

update Dl_opOrder set 
datCreateTime=@datCreateTime,bytStatus=@bytStatus,strRemarks=@strRemarks,ccuscode=@ccuscode,cdefine1=@cdefine1,cdefine2=@cdefine2,cdefine3=@cdefine3,cdefine9=@cdefine9,
cdefine10=@cdefine10,cdefine11=@cdefine11,cdefine12=@cdefine12,cdefine13=@cdefine13,ccusname=@ccusname,cpersoncode=@cpersoncode,cSCCode=@cSCCode
,datDeliveryDate=@datDeliveryDate,strLoadingWays=@strLoadingWays,lngopUseraddressId=@lngopUseraddressId
 where strBillNo=@strBillNo
--返回订单表头id
--select top(1) lngopOrderId from Dl_opOrder where strBillNo=@strBillNo
select max(lngopOrderId) 'lngopOrderId' from Dl_opOrder where strBillNo=@strBillNo

END





GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewYOrderDetailByIns]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [DLproc_NewYOrderDetailByIns]

-- =============================================
-- Author:echo
-- Create date:2015-12-09
-- Description:	新增加预订单插入表体数据[DLproc_NewYOrderDetailByIns]
-- 参数:
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewYOrderDetailByIns]
@lngPreOrderId int,	--预订单id
@cinvcode varchar(50),	--存货编码 
@iquantity decimal(18,6),	--数量 
@inum decimal(18,6),	--辅计量数量 
@iquotedprice decimal(18,6),	--报价 
@iunitprice decimal(18,6),	--原币无税单价 
@itaxunitprice decimal(18,6),	--原币含税单价 
@imoney decimal(18,6),	--原币无税金额 
@itax decimal(18,6),	--原币税额 
@isum decimal(18,6),	--原币价税合计 
@inatunitprice decimal(18,6),	--本币无税单价 
@inatmoney decimal(18,6),	--本币无税金额 
@inattax decimal(18,6),	--本币税额 
@inatsum decimal(18,6),	--本币价税合计 
@kl decimal(18,6),	--扣率 
@itaxrate decimal(18,6),	--税率 
@cDefine22 varchar(100),--包装量
@iinvexchrate decimal(18,6),	--换算率 
@cunitid varchar(50),	--计量单位编码 ,销售单位编码
@irowno int,	--行号 
@cinvname varchar(100),	--存货名称  
@idiscount decimal(18,6), --原币折扣额 
@inatdiscount decimal(18,6), --本币折扣额
/*11-09新增加表体字段*/
@cComUnitName varchar(20),	--基本单位名称
@cInvDefine1 varchar(20),		--大包装单位名称
@cInvDefine2 varchar(20),		--小包装单位名称
@cInvDefine13 decimal(18,6),	--大包装换算率
@cInvDefine14 decimal(18,6),	--小包装换算率
@unitGroup varchar(100),		--单位换算率组
@cComUnitQTY decimal(18,6),		--基本单位数量
@cInvDefine1QTY decimal(18,6),	--大包装单位数量
@cInvDefine2QTY decimal(18,6),	--小包装单位数量
@cn1cComUnitName varchar(20)	--销售单位名称


AS

BEGIN

	SET NOCOUNT ON;

--插入表体数据

insert  into  Dl_opPreOrderDetail (
lngPreOrderId,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,inatunitprice,inatmoney,inattax,inatsum,kl,
itaxrate,iinvexchrate,cunitid,irowno,cinvname,idiscount,inatdiscount,cDefine22
,cComUnitName,cInvDefine1,cInvDefine2,cInvDefine13,cInvDefine14,unitGroup,cComUnitQTY,cInvDefine1QTY,cInvDefine2QTY,cn1cComUnitName 
) 
values (
@lngPreOrderId,@cinvcode,@iquantity,@inum,@iquotedprice,@iunitprice,@itaxunitprice,@imoney,@itax,@isum,@inatunitprice,@inatmoney,@inattax,@inatsum,@kl,
@itaxrate,@iinvexchrate,@cunitid,@irowno,@cinvname,@idiscount,@inatdiscount,@cDefine22
,@cComUnitName,@cInvDefine1,@cInvDefine2,@cInvDefine13,@cInvDefine14,@unitGroup,@cComUnitQTY,@cInvDefine1QTY,@cInvDefine2QTY,@cn1cComUnitName
)

END

--select * from [Dl_opPreOrderDetail]



  


GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewYOrderU8ByIns]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec DLproc_NewYOrderU8ByIns 'DLOP110400016'

-- =============================================
-- Author:echo
-- Create date:2015-10-23
-- Description:	生成U8订单表头数据[DLproc_NewYOrderU8ByIns]
-- 参数:@strBillNo:DLorder中的单据编号,生成U8订单,并返回U8订单的单据号
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewYOrderU8ByIns]
@strBillNo varchar(50)--DL单据编号
AS
BEGIN
	SET NOCOUNT ON;
declare @csocode varchar(50),@csysbarcode varchar(50),@maxbillno int,@id varchar(50),@cbsysbarcodeDetail varchar(50),@ddate varchar(50),@lngPreOrderId varchar(50),
@isosid varchar(50),@bytStatus varchar(2),@cmaker varchar(10),@lngopUseraddressId varchar(10),@cdefine11 varchar(500),@cSCCode varchar(10),@icount int,
@p5 varchar(50),@p6 varchar(50),@cpersoncode varchar(20)

/*2015-11-04,判断是否已经生成过U8订单,避免重复生成*/
select @bytStatus=bytStatus from Dl_opPreOrder where strBillNo=@strBillNo
if @bytStatus=1 or @bytStatus=2
begin

/*2016-02-01注视,启用新的编号获取方式,start*/
----获取最大单号
----declare @maxbillno int
--select @maxbillno=isnull(max(convert(int,replace(cCode,'Y',''))+1),convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+'00001')  from SA_PreOrderMain where cCode like 'Y1%' or cCode like 'Y2%'  
----select @maxbillno
--select @csocode='Y'+convert(varchar(10),@maxbillno)
/*2016-02-01注视,启用新的编号获取方式,end*/

--2016-02-01获取单据编号方式
exec DLproc_GetBillNo '10',@csocode OUTPUT

--组合字段:@csysbarcode 表头单据条码 --表头单据条码 
set @csysbarcode='||SA26|'+@csocode

--获取表头最大id,2015改用exec sp_getID '00','002','Somain',1,@p5 output,@p6 output获取表头ID和表体ID
select @ddate=ddate,@lngPreOrderId=lngPreOrderId from Dl_opPreOrder where strBillNo=@strBillNo
select @icount=count(lngPreOrderId) from Dl_opPreOrderDetail where lngPreOrderId=@lngPreOrderId
set @p5=''
set @p6=''
exec sp_getID '','003','SA_PreOrderMain',@icount,@p5 output,@p6 output
select @isosid=@p6-@icount,@id=@p5

/*2015-11-24,获取订单员的U8编号*/	
select @cmaker=p.cPersonName from Dl_opPreOrder  do
left join Dl_opUser dou on do.strManagers=dou.lngopUserId
left join Person p on dou.strLoginName=p.cPersonCode
where strBillNo=@strBillNo
--获取业务员
select @cpersoncode=cCusPPerson from Customer where cCusCode in (select top (1) ccuscode from Dl_opPreOrder where strBillNo=@strBillNo)

/*插入U8表头数据V1.0*/
insert into SA_PreOrderMain(
--主动数据
id,cmaker,ddate,ccode,csysbarcode,ccuscode,ccusname,cpersoncode,
--被动数据
cdepcode,cexch_name,iswfcontrolled,ireturncount,iverifystate,iexchrate,ivtid,cstcode,itaxrate
--null
)
select 
--主动数据
@id,@cmaker,@ddate,@csocode,@csysbarcode,ccuscode,ccusname,@cpersoncode,
--被动数据
N'0601',N'人民币',N'0',N'0',N'0',1,N'131439',N'00',N'17.00'
--null
from Dl_opPreOrder where strBillNo=@strBillNo 

--插入U8表体数据V1.0
	--完成表头数据插入之后,获取表头的:销售订单主表标识
set @cbsysbarcodeDetail=@csysbarcode+'|'

----将Dl_opPreOrderDetail数据插入到U8表体中
insert into SA_PreOrderDetails(
--变量
autoid,id,cinvcode,iquantity,itaxrate,kl,iquotedprice,inatdiscount,isum,idiscount,imoney,inatmoney,inatsum,inattax,itax,itaxunitprice,inatunitprice,iunitprice,iinvexchrate,cunitid,irowno,inum,cDefine22,
cbsysbarcode,
dexpectationdate,
dacceptabledate,
dsatiabledate,
kl2,
--常量
borderbom,borderbomover,batpcal,bsaleprice,bgift,dkl1,dkl2,fcusminprice
--null值
)
select @isosid+irowno,@id,cinvcode,iquantity,itaxrate,kl,iquotedprice,inatdiscount,isum,idiscount,imoney,inatmoney,inatsum,inattax,itax,itaxunitprice,inatunitprice,iunitprice,iinvexchrate,cunitid,irowno,inum,cDefine22
,@cbsysbarcodeDetail+convert(varchar(5),irowno),@ddate,@ddate,@ddate,100,
0,0,0,1,0,0,0,0
from Dl_opPreOrderDetail
where lngPreOrderId=@lngPreOrderId
--更新Dl_opPreOrder表头中的U8订单编号,并且更新dl订单的状态为4:已提交审核状态//1,提交,待审核;2,已修改,待顾客确认;3,提交,被驳回;,4,提交,已审核,生成U8订单
--11-16,并且更新订单表头字段strManagers,操作员代码,
update Dl_opPreOrder set ccode=@csocode,bytStatus=4,strAuditor=strManagers,datAuditordTime=GETDATE() where strBillNo=@strBillNo

end

END


GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewYYOrderByIns]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec DLproc_NewYYOrderByIns

-- =============================================
-- Author:echo
-- Create date:2015-12-12
-- Description:	新增加订单表头插入数据[DLproc_NewYYOrderByIns],酬宾订单,特殊订单
-- 参数:
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewYYOrderByIns]
@lngopUserId varchar(50),--登陆人,制单人id
@datCreateTime varchar(50),--创建日期,单据日期,预发货,预完成日期
@bytStatus varchar(50),--单据状态
@strRemarks varchar(500),--备注
@ccuscode varchar(50),--客户编码,开票单位编码
@cdefine1 varchar(50),--自定义项1
@cdefine2 varchar(50),--自定义项2
@cdefine3 varchar(500),--自定义项3
@cdefine9 varchar(50), --自定义项9
@cdefine10 varchar(50),--自定义项10
@cdefine11 varchar(200),--自定义项11
@cdefine12 varchar(50),--自定义项12
@cdefine13 varchar(50),--自定义项13
@ccusname varchar(50),--客户名称
@cpersoncode varchar(50),--业务员编码
@cSCCode varchar(50), --发运方式编码
@datDeliveryDate varchar(50), --交货日期
@strLoadingWays varchar(100), --装车方式
@cSTCode varchar(10),		--销售类型
@lngopUseraddressId varchar(10),--useraddress的ID
@strU8BillNo varchar(30),	--U8单据号,样品资料写入,改为DL系统单据号
@lngBillType varchar(50) --单据类型(0,普通订单,1,酬宾订单,2特殊订单)
AS

BEGIN

	SET NOCOUNT ON;
declare @strBillNo varchar(50),@maxbillno int
--创建最大订单号@BillNo=@csocode=@strBillNo --销售订单号

if @cSTCode='00'
begin
set @strU8BillNo=' '
end
/*2016-01-25更新获取单据编号方式*/--begin
----获取最大单号
--select @maxbillno=isnull(max(convert(int,substring(strBillNo,5,9))),0)+1 from Dl_opOrder where strBillNo like 'DLOP%' 

--select @strBillNo='DLOP'+convert(varchar(9),@maxbillno)
----select @strBillNo
/*2016-01-25更新获取单据编号方式*/--end
--2016-01-25获取单据编号方式
if	@lngBillType=1 
begin
exec DLproc_GetBillNo '2',@strBillNo OUTPUT
end
if	@lngBillType=2 
begin
exec DLproc_GetBillNo '3',@strBillNo OUTPUT
end
--插入表头数据
insert into Dl_opOrder (
lngopUserId,strBillNo,datCreateTime,bytStatus,strRemarks,ccuscode,cdefine1,cdefine2,cdefine3,cdefine9,cdefine10,cdefine11,cdefine12,cdefine13,ccusname,cpersoncode,cSCCode,datDeliveryDate, strLoadingWays,cSTCode,lngopUseraddressId,RelateU8NO,lngBillType
,strManagers
) 
values (
@lngopUserId,@strBillNo,@datCreateTime,@bytStatus,@strRemarks,@ccuscode,@cdefine1,@cdefine2,@cdefine3,@cdefine9,@cdefine10,@cdefine11,@cdefine12,@cdefine13,@ccusname,@cpersoncode,@cSCCode,@datDeliveryDate, @strLoadingWays, @cSTCode,@lngopUseraddressId,@strU8BillNo,@lngBillType
,' '
)

--20151129添加,清除临时授权,
update Customer set cCusEmail=' ' where cCusCode=@ccuscode --客户编码,开票单位编码

--20151129添加,样品资料直接给关联的U8订单专员负责,如果订单员已经接单,则直接关联转接
if @cSTCode='01'
begin
declare @strManagers varchar(10)
select @strManagers=isnull(strManagers,' ') from Dl_opOrder where strBillNo=@strU8BillNo
update Dl_opOrder set strManagers=@strManagers where strBillNo=@strBillNo
end

--返回订单表头id
--select top(1) lngopOrderId from Dl_opOrder where strBillNo=@strBillNo
select max(lngopOrderId) 'lngopOrderId',@strBillNo 'strBillNo' from Dl_opOrder where strBillNo=@strBillNo

END




GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewYYOrderDetailByIns]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [DLproc_NewYYOrderDetailByIns]

-- =============================================
-- Author:echo
-- Create date:2015-12-12
-- Description:	新增加订单插入表体数据(预订单参照生成)[DLproc_NewYYOrderDetailByIns]
-- 参数:
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewYYOrderDetailByIns]
@lngopOrderId int,	--订单id
@cinvcode varchar(50),	--存货编码 
@iquantity decimal(18,6),	--数量 
@inum decimal(18,6),	--辅计量数量 
@iquotedprice decimal(18,6),	--报价 
@iunitprice decimal(18,6),	--原币无税单价 
@itaxunitprice decimal(18,6),	--原币含税单价 
@imoney decimal(18,6),	--原币无税金额 
@itax decimal(18,6),	--原币税额 
@isum decimal(18,6),	--原币价税合计 
@inatunitprice decimal(18,6),	--本币无税单价 
@inatmoney decimal(18,6),	--本币无税金额 
@inattax decimal(18,6),	--本币税额 
@inatsum decimal(18,6),	--本币价税合计 
@kl decimal(18,6),	--扣率 
@itaxrate decimal(18,6),	--税率 
@cdefine22 varchar(100),	--表体自定义项22 
@iinvexchrate decimal(18,6),	--换算率 
@cunitid varchar(50),	--计量单位编码 
@irowno int,	--行号 
@cinvname varchar(100),	--存货名称  
@idiscount decimal(18,6), --原币折扣额 
@inatdiscount decimal(18,6), --本币折扣额
/*11-09新增加表体字段*/
@cComUnitName varchar(20),	--基本单位名称
@cInvDefine1 varchar(20),		--大包装单位名称
@cInvDefine2 varchar(20),		--小包装单位名称
@cInvDefine13 decimal(18,6),	--大包装换算率
@cInvDefine14 decimal(18,6),	--小包装换算率
@unitGroup varchar(100),		--单位换算率组
@cComUnitQTY decimal(18,6),		--基本单位数量
@cInvDefine1QTY decimal(18,6),	--大包装单位数量
@cInvDefine2QTY decimal(18,6),	--小包装单位数量
@cn1cComUnitName varchar(20),	--销售单位名称
@cpreordercode varchar(50),	--销售预订单号
@iaoids varchar(50)		--预订单表体autoid,iaoids
AS

BEGIN

	SET NOCOUNT ON;

--插入表体数据

insert into Dl_opOrderDetail(
lngopOrderId,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,inatunitprice,inatmoney,inattax,inatsum,kl,
itaxrate,cdefine22,iinvexchrate,cunitid,irowno,cinvname,idiscount,inatdiscount
,cComUnitName,cInvDefine1,cInvDefine2,cInvDefine13,cInvDefine14,unitGroup,cComUnitQTY,cInvDefine1QTY,cInvDefine2QTY,cn1cComUnitName,cpreordercode,iaoids
) 
values (
@lngopOrderId,@cinvcode,@iquantity,@inum,@iquotedprice,@iunitprice,@itaxunitprice,@imoney,@itax,@isum,@inatunitprice,@inatmoney,@inattax,@inatsum,@kl,
@itaxrate,@cdefine22,@iinvexchrate,@cunitid,@irowno,@cinvname,@idiscount,@inatdiscount
,@cComUnitName,@cInvDefine1,@cInvDefine2,@cInvDefine13,@cInvDefine14,@unitGroup,@cComUnitQTY,@cInvDefine1QTY,@cInvDefine2QTY,@cn1cComUnitName,@cpreordercode,@iaoids
)
--更新表头单据类型


END







GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewYYOrderU8ByIns]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec DLproc_NewOrderU8ByIns 'DLOP110400016'

-- =============================================
-- Author:echo
-- Create date:2015-12-12
-- Description:	生成U8订单表头数据(预订单参照生成)[DLproc_NewYYOrderU8ByIns]
-- 参数:@strBillNo:DLorder中的单据编号,生成U8订单,并返回U8订单的单据号
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_NewYYOrderU8ByIns]
@strBillNo varchar(50)--DL单据编号
AS
BEGIN
	SET NOCOUNT ON;
declare @csocode varchar(50),@csysbarcode varchar(50),@maxbillno int,@id varchar(50),@cbsysbarcodeDetail varchar(50),@ddate varchar(50),@lngopOrderId varchar(50),
@isosid varchar(50),@bytStatus varchar(2),@cmaker varchar(10),@lngopUseraddressId varchar(10),@cdefine11 varchar(500),@cSCCode varchar(10),@icount int,@p5 varchar(50),@p6 varchar(50)

/*2015-11-04,判断是否已经生成过U8订单,避免重复生成*/
select @bytStatus=bytStatus from Dl_opOrder where strBillNo=@strBillNo
if @bytStatus=1 or @bytStatus=2
begin

--创建最大订单号@BillNo=@csocode --销售订单号
--exec DLproc_OrderGetNewBillNoBySelU8 @csocode OUTPUT


--获取最大单号
select @maxbillno=isnull(max(convert(int,replace(csocode,'W',''))+1),convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+'00001')  from SO_SOMain where csocode like 'W1%' or csocode like 'W2%'  
--IF @maxbillno=100000
--BEGIN
----select 'error'  'BillNo'
--set @csocode='error'
--END
--ELSE
--BEGIN
----创建新的订单编号,--获取两位年,两位月,组合,如1510,
--select @csocode='WSXD'+convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+right('0000'+convert(varchar(5),@maxbillno),5)
--END
select @csocode='W'+convert(varchar(10),@maxbillno)


--组合字段:@csysbarcode 表头单据条码 --表头单据条码 
set @csysbarcode='||SA17|'+@csocode
--set @csysbarcode='N'+''''+'||SA17|'+@csocode+''''
--set @csocode='N'+''''+@csocode+''''

/*插入表头数据V1.0*/
--insert into SO_SOMain (
----主动数据
--csysbarcode,ddate,csocode,ccuscode,cdefine1,cdefine2,cdefine3,cdefine8,cdefine9,cdefine10,cdefine11,cdefine12,dpredatebt,dpremodatebt,ccusname,cinvoicecompany,cmemo,cpersoncode,id,
----被动数据
--cstcode,cdepcode,cexch_name,iexchrate,itaxrate,cbustype,cmaker,bdisflag,breturnflag,iverifystate,iswfcontrolled,bcashsale,bmustbook,ivtid,
----null
--caddcode,ccrmpersoncode,ccrmpersonname,cmainpersoncode,ccurrentauditor,cchanger,ccrechpname,outid,csccode,ccusoaddress,cpaycode,imoney,istatus,cverifier,ccloser,cdefine5,cdefine7,
--cdefine13,cdefine14,cdefine15,cdefine16,clocker,ccusperson,coppcode,cmodifier,ccuspersoncode,ccontactname,cmobilephone,iflowid,cgatheringplan,ireturncount,icreditstate,cchangeverifier,
--cgathingcode,iprintcount,fbookratio,fbooksum,fbooknatsum,fgbooksum,fgbooknatsum,contract_status,optnty_name,ioppid,csvouchtype,csscode,cattachment,cebtrnumber,cebbuyer,cebbuyernote,
--cebprovince,cebcity,cebdistrict,cinvoicecusname
--)
--values (
----主动数据
--@csysbarcode,@ddate,@csocode,@ccuscode,@cdefine1,@cdefine2,@cdefine3,@cdefine8,@cdefine9,@cdefine10,@cdefine11,@cdefine12,@dpredatebt,
--@dpremodatebt,@ccusname,@cinvoicecompany,@cmemo,@cpersoncode,@id,
----被动数据
--N'00',N'0603',N'人民币',1,17,N'普通销售',N'demo',0,0,0,1,0,0,131395,
----null
--Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,
--Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null
--)

--获取表头最大id,2015改用exec sp_getID '00','002','Somain',1,@p5 output,@p6 output获取表头ID和表体ID
--select @id=isnull(MAX(id),0)+1 from SO_SOMain
select @ddate=datCreateTime,@lngopOrderId=lngopOrderId from Dl_opOrder where strBillNo=@strBillNo
select @icount=count(lngopOrderId) from Dl_opOrderDetail where lngopOrderId=@lngopOrderId
set @p5=''
set @p6=''
exec sp_getID '00','003','Somain',@icount,@p5 output,@p6 output
select @isosid=@p6-@icount,@id=@p5
/*获取地址信息,2015-11-21增加*/
select @lngopUseraddressId=lngopUseraddressId from Dl_opOrder where strBillNo=@strBillNo
--select 
--@cDefine9=strConsigneeName,	 
--@cDefine12=strConsigneeTel,	 		
--@cDefine11=strReceivingAddress,	 	 
--@cDefine10=strCarplateNumber,	 				
--@cDefine1=strDriverName,	 		
--@cDefine13=strDriverTel,	 		
--@cDefine2=strIdCard from Dl_opUserAddress where lngopUseraddressId=@lngopUseraddressId	
/*2015-11-24,获取订单员的U8编号*/	
select @cmaker=p.cPersonName from Dl_opOrder  do
left join Dl_opUser dou on do.strManagers=dou.lngopUserId
left join Person p on dou.strLoginName=p.cPersonCode
where strBillNo=@strBillNo

/*插入U8表头数据V2.0*/
insert into SO_SOMain(
--主动数据
csysbarcode,ddate,csocode,ccuscode,cdefine1,cdefine2,cdefine3,cdefine8,cdefine9,cdefine10,cdefine11,cdefine12,cdefine13,dpredatebt,dpremodatebt,ccusname,cinvoicecompany,cmemo,
cpersoncode,id,cSCCode,cDefine4, cDefine14, cSTCode,cDefine6,cmaker,
--被动数据
cdepcode,cexch_name,iexchrate,itaxrate,cbustype,bdisflag,breturnflag,iverifystate,iswfcontrolled,bcashsale,bmustbook,ivtid,
--null
caddcode,ccrmpersoncode,ccrmpersonname,cmainpersoncode,ccurrentauditor,cchanger,ccrechpname,outid,ccusoaddress,cpaycode,imoney,istatus,cverifier,ccloser,cdefine5,cdefine7,
cdefine15,cdefine16,clocker,ccusperson,coppcode,cmodifier,ccuspersoncode,ccontactname,cmobilephone,iflowid,cgatheringplan,ireturncount,icreditstate,cchangeverifier,
cgathingcode,iprintcount,fbookratio,fbooksum,fbooknatsum,fgbooksum,fgbooknatsum,contract_status,optnty_name,ioppid,csvouchtype,csscode,cattachment,cebtrnumber,cebbuyer,cebbuyernote,
cebprovince,cebcity,cebdistrict,cinvoicecusname
)
select 
--主动数据
@csysbarcode,convert(varchar(10),datCreateTime,120),@csocode,ccuscode,cdefine1,cdefine2,cdefine3,cdefine8,cdefine9,cdefine10,
right(cdefine11,charindex(',',reverse(cdefine11))-1),cdefine12,cdefine13,convert(varchar(10),datCreateTime,120),
convert(varchar(10),datCreateTime,120),ccusname,ccuscode,strRemarks,cpersoncode,@id,cSCCode,datDeliveryDate, strLoadingWays, cSTCode,datBillTime,@cmaker,
--被动数据
N'0601',N'人民币',1,17,N'普通销售',0,0,0,1,0,0,131395,
--null
Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,
Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null 
from Dl_opOrder where strBillNo=@strBillNo 
/*2015-12-02 更新送货地址详情,如果为自提,则清除地址*/
update SO_SOMain set cdefine11=' ' where csocode=@csocode and cSCCode='00'


--插入扩展字段:(地址id对应的省市区信息)/*2015-11-21 增加*/;11-26修改,添加新的扩展字段2,样品资料关联U8的订单号
--insert into SO_SOMain_ExtraDefine (ID,chdefine1) select @id,strDistrict from Dl_opUserAddress where lngopUseraddressId=@lngopUseraddressId
insert into SO_SOMain_ExtraDefine (ID,chdefine1,chdefine2) select @id,dou.strDistrict,do.RelateU8NO from Dl_opUserAddress dou,Dl_opOrder do 
where dou.lngopUseraddressId=@lngopUseraddressId and do.strBillNo=@strBillNo

--插入U8表体数据V2.0
	--完成表头数据插入之后,获取表头的:销售订单主表标识
set @cbsysbarcodeDetail=@csysbarcode+'|'
--select @isosid=isnull(max(isosid),1) from SO_SODetails
----将dl_oporderDetail数据插入到U8表体中
insert into SO_SODetails(
--变量
isosid,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,inatunitprice,inatmoney,inattax,inatsum,kl,itaxrate,cdefine22,iinvexchrate,cunitid,id,irowno,cinvname,cSOCode
--常量
,cbsysbarcode,dpredate,idiscount,inatdiscount,kl2,fsalecost,fsaleprice,dpremodate,fcusminprice,ballpurchase,borderbom,borderbomover,busecusbom,bsaleprice,bgift
--null值
,ifhnum,ifhquantity,ifhmoney,ikpquantity,ikpnum,ikpmoney,cmemo,cfree1,cfree2,cdefine23,cdefine24,cdefine25,cdefine26,cdefine27,citemcode,citem_class,citemname,citem_cname,cfree3,
cfree4,cfree5,cfree6,cfree7,cfree8,cfree9,cfree10,cdefine28,cdefine29,cdefine30,cdefine31,cdefine32,cdefine33,cdefine34,cdefine35,fpurquan,cquocode,iquoid,cscloser,icusbomid,
imoquantity,ccontractid,ccontracttagcode,ccontractrowguid,ippartseqid,ippartid,ippartqty,ccusinvcode,ccusinvname,iprekeepquantity,iprekeepnum,iprekeeptotquantity,iprekeeptotnum,
fimquantity,fomquantity,finquantity,foutquantity,iexchsum,imoneysum,iaoids,cpreordercode,fretquantity,fretnum,idemandtype,cdemandcode,cdemandmemo,iimid,ccorvouchtype,icorrowno,
body_outid,forecastdid,cdetailsdemandcode,cdetailsdemandmemo,fappqty,cparentcode,cchildcode,fchildqty,fchildrate,icalctype
)
select @isosid+irowno,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,inatunitprice,inatmoney,inattax,inatsum,kl,itaxrate,cdefine22,iinvexchrate,cunitid,@id,irowno,cinvname,@csocode
,@cbsysbarcodeDetail+convert(varchar(5),irowno),@ddate,0,0,100,0,0,@ddate,0,0,0,0,0,1,0
,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,
Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,
Null,Null,Null,Null,Null,Null,Null,Null
from Dl_opOrderDetail
where lngopOrderId=@lngopOrderId
--更新dl_oporder表头中的U8订单编号,并且更新dl订单的状态为4:已提交审核状态//1,提交,待审核;2,已修改,待顾客确认;3,提交,被驳回;,4,提交,已审核,生成U8订单
--11-16,并且更新订单表头字段strManagers,操作员代码,

update Dl_opOrder set cSOCode=@csocode,bytStatus=4 where strBillNo=@strBillNo



end


END



GO
/****** Object:  StoredProcedure [dbo].[DLproc_OrderDetailModifyBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:echo
-- Create date:2015-11-08
-- Description:	用于查询用户指定的被驳回需要修改的订单[DLproc_OrderDetailModifyBySel]
--参数:单据编号
-- =============================================


CREATE PROCEDURE [dbo].[DLproc_OrderDetailModifyBySel]
@strBillNo varchar(50) 
AS

BEGIN

	SET NOCOUNT ON;

	--插入库存数据--151230禁用,启用新的可用量算法
--	select cInvCode,cInvCName,sum(isnull(fAvailQtty,0)*0.7) 'fAvailQtty' into #temp from (
--SELECT  CS.cExpirationdate as 有效期至,E1.enumname as 有效期推算方式,W.cWhCode, W.cWhName, I.cInvCode, I.cInvAddCode,  I.cInvName, I.cInvStd, I.cInvCCode , IC.cInvCName, 
-- CU_M.cComUnitName AS cInvM_Unit, CASE WHEN I.iGroupType = 0 THEN NULL  WHEN I.iGrouptype = 2 THEN CU_A.cComUnitName  WHEN I.iGrouptype = 1 THEN CU_G.cComUnitName END  AS cInvA_Unit,convert(nvarchar(38),
--  convert(decimal(38,2),CASE WHEN I.iGroupType = 0 THEN NULL      WHEN I.iGroupType = 2 THEN (CASE WHEN CS.iQuantity = 0.0 OR CS.iNum = 0.0 THEN NULL ELSE CS.iQuantity/CS.iNum END)      
-- WHEN I.iGroupType = 1 THEN CU_G.iChangRate END)) AS iExchRate,
--  Null as cInvDefine1, Null as cInvDefine2, Null as cInvDefine3, Null as cFree1, Null as cFree2, Null as cFree3, Null as cFree4, Null as cFree5, Null as cFree6, Null as cFree7, Null as cFree8, Null as cFree9, 
-- Null as cFree10, Null as cInvDefine4, Null as cInvDefine5, Null as cInvDefine6, Null as cInvDefine7, Null as cInvDefine8, Null as cInvDefine9, Null as cInvDefine10, Null as cInvDefine11, Null as cInvDefine12, 
--  Null as cInvDefine13, Null as cInvDefine14, Null as cInvDefine15, Null as cInvDefine16,cs.cBatch, cs.EnumName As iSoTypeName, cs.csocode as SOCode, cs.cdemandmemo,convert(nvarchar,cs.isoseq) as iRowNo,
--cs.cvmivencode,v1.cvenabbname as cvmivenname , isnull(E.enumname,N'') as cMassUnitName,CS.dVDate, CS.dMdate,convert(varchar(20),CS.iMassDate) as iMassDate,
-- (iQuantity) AS iQtty,( CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(iNum,0) WHEN iGroupType = 1 THEN iQuantity/ CU_G.iChangRate END) AS iNum,
--  CASE WHEN CS.bStopFlag = 1 OR CS.bGspStop = 1 THEN iQuantity ELSE IsNull(fStopQuantity,0) END AS iStopQtty,
--  CASE WHEN CS.bStopFlag = 1 OR CS.bGspStop = 1 THEN (CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(iNum,0) WHEN iGroupType = 1 THEN iQuantity/ CU_G.iChangRate END) 
-- ELSE (CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(fStopNum,0) WHEN iGroupType = 1 THEN fStopQuantity/ CU_G.iChangRate END) END AS iStopNum,
-- (fInQuantity) AS fInQtty, 
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fInNum,0) WHEN iGroupType = 1 THEN fInQuantity/ CU_G.iChangRate END) AS fInNum,
-- (fTransInQuantity) AS fTransInQtty,
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fTransInNum,0) WHEN iGroupType = 1 THEN fTransInQuantity/ CU_G.iChangRate END) AS fTransInNum,
-- (ISNULL(fInQuantity,0) + ISNULL(fTransInQuantity,0)) AS fInQttySum,
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fInNum,0) + ISNULL(fTransInNum,0) WHEN iGroupType = 1 THEN (ISNULL(fInQuantity,0) + ISNULL(fTransInQuantity,0))/ CU_G.iChangRate END) AS fInNumSum,
-- (fOutQuantity) AS fOutQtty, 
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fOutNum,0) WHEN iGroupType = 1 THEN fOutQuantity/ CU_G.iChangRate END) AS fOutNum,
-- CS.cBatchProperty1,CS.cBatchProperty2,CS.cBatchProperty3,CS.cBatchProperty4,CS.cBatchProperty5,CS.cBatchProperty6,CS.cBatchProperty7,CS.cBatchProperty8,CS.cBatchProperty9,CS.cBatchProperty10,
--  (fTransOutQuantity) AS fTransOutQtty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fTransOutNum,0) WHEN iGroupType = 1 THEN fTransOutQuantity/ CU_G.iChangRate END) AS fTransOutNum,
--  (ISNULL(fOutQuantity,0) + ISNULL(fTransOutQuantity,0)) AS fOutQttySum , 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fOutNum,0) + ISNULL(fTransOutNum,0) WHEN iGroupType = 1 THEN (ISNULL(fOutQuantity,0) + ISNULL(fTransOutQuantity,0))/ CU_G.iChangRate END) AS fOutNumSum,
--  (fDisableQuantity) AS fDisableQtty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fDisableNum,0) WHEN iGroupType = 1 THEN fDisableQuantity/ CU_G.iChangRate END) AS fDisableNum,
--  (ipeqty) AS fpeqty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(ipenum,0) WHEN iGroupType = 1 THEN ipeqty/ CU_G.iChangRate END) AS fpenum,
-- (CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END) AS fAvailQtty,dLastCheckDate, 
-- (CASE WHEN iGroupType = 0 THEN 0  WHEN iGroupType = 2 THEN  CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iNum,0)- IsNull(fStopNum,0) END  - ISNULL(fOutNum,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iNum,0)- IsNull(fStopNum,0) END  - ISNULL(fOutNum,0) END WHEN iGroupType = 1 THEN  (CASE WHEN bInvBatch=1 THEN  
-- CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END)/CU_G.iChangRate ELSE NULL END) AS fAvailNum
-- FROM v_ST_currentstockForReport  CS inner join dbo.Inventory I ON I.cInvCode = CS.cInvCode   
-- left join dbo.InventoryClass IC ON IC.cInvCCode = I.cInvCCode LEFT OUTER JOIN dbo.ComputationUnit CU_G ON I.cSTComUnitCode =CU_G.cComUnitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_A ON I.cAssComUnitCode = CU_A.cComunitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_M ON I.cComUnitCode = CU_M.cComunitCode 
-- LEFT OUTER JOIN dbo.Warehouse W ON CS.cWhCode = W.cWhCode 
-- left join vendor v1 on v1.cvencode = cs.cvmivencode 
-- left join v_aa_enum E1 on E1.enumcode = ISNULL(cs.iExpiratDateCalcu,0) and E1.enumtype=N'SCM.ExpiratDateCalcu' 
-- LEFT OUTER JOIN dbo.v_aa_enum E with (nolock) on E.enumcode=convert(nchar,CS.cMassUnit) and E.enumType=N'ST.MassUnit' 
-- WHERE  1=1  AND 1=1  
-- ) as HH
-- group by cInvCode,cInvCName

--20151230,新可用量算法,生成U8订单后,即扣减订单数量
/*20151226,可用量查询,生成U8订单后,扣减该数量*/
  ----组合查询可用量
  --订单中的物料
  select bb.cinvcode into #tempcinvcode from Dl_opOrder aa
  inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId where aa.strBillNo=@strBillNo
--declare @qty1 decimal(20,6),@qty decimal(20,6),@ddate varchar(20)
declare @ddate varchar(20)
select @ddate=convert(varchar(10),getdate(),120)
/*V2.0:2015-01-06新算法---BEGIN*/
 Select   cInvCode 'cinvcode',SUM(ISNULL(iQuantity,0))  'qty' into #temp11 from SA_CurrentStock 
 inner join warehouse on warehouse.cwhcode=SA_CurrentStock.cwhcode  
 where cInvCode in (select cinvcode from #tempcinvcode ) and binavailcalcu=1 and  (isotype=0 or (isotype=1 and isodid=N'')) AND isnull(bStopFlag,0)=0
 group by cInvCode

 SELECT  SO_SODetails.cInvCode 'cinvcode',Sum(Case When (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) < 0 Then 0 Else (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) End)  'qty1'  into #temp22
FROM SO_SOMain 
INNER JOIN SO_SODetails ON SO_SOMain.ID = SO_SODetails.ID   INNER JOIN Inventory ON SO_SODetails.cInvCode = Inventory.cInvCode  left join ST_PELockedSum c with (nolock) on SO_SODetails.cInvCode = c.cinvcode and c.isotype =1 
and c.isodid = convert(nvarchar(40),ISNULL(SO_SODetails.isosid,N''))  WHERE  IsNull(SO_SODetails.csCloser ,N'') =N'' AND   IsNull(SO_SOMain.cBustype,N'') <> N'直运销售' AND   (Case When IsNull(SO_SODetails.dPreDate,N'') = N'' 
Then SO_SOMain.dDate Else SO_SODetails.dPreDate End ) <=@ddate And   SO_SODetails.cInvCode  in (select cinvcode from #tempcinvcode )   AND Isnull(SO_SODetails.cparentcode,'')= '' 
group by SO_SODetails.cInvCode
/*V2.0:2015-01-06新算法---END*/
	--网上下单系统已存在的未生成U8订单和驳回修改的订单数量,待顾客确认的订单,这部分数量需要用U8数量减去,(不排除当前订单数量)
	select dod.cinvcode,isnull(sum(isnull(iquantity,0)),0) 'opqty' into #temp33  from Dl_opOrderDetail dod
inner join Dl_opOrder do on do.lngopOrderId=dod.lngopOrderId
where dod.cinvcode  in (select cinvcode from #tempcinvcode ) and do.bytStatus in (1,2,3) 
--and do.strBillNo!=@strBillNo
group by  dod.cinvcode

select aa.cinvcode,(sum(isnull(aa.qty,0))-sum(isnull(bb.qty1,0)))*0.7-sum(isnull(cc.opqty,0)) 'fAvailQtty' into #temp from #temp11 aa
left join #temp22 bb on aa.cinvcode=bb.cinvcode
left join #temp33 cc on aa.cinvcode=cc.cinvcode
group by aa.cinvcode

--读取数据	/*20151221,修改,库存数据,计算逻辑:如果商品当前库存数量小于0,则为当前订单数量,否则为库存量+订单量*/
select 
dod.cinvcode 'cInvCode',
dod.cinvname 'cInvName',
iv.cInvStd 'cInvStd',
dod.cComUnitName 'cComUnitName',
dod.cInvDefine1 'cInvDefine1',
dod.cInvDefine2 'cInvDefine2',
dod.cInvDefine13 'cInvDefine13',
dod.cInvDefine14 'cInvDefine14',
dod.UnitGroup 'UnitGroup',
dod.cComUnitQTY 'cComUnitQTY',
dod.cInvDefine1QTY 'cInvDefine1QTY',
dod.cInvDefine2QTY 'cInvDefine2QTY',
dod.iquantity 'cInvDefineQTY',
dod.iquotedprice 'cComUnitPrice',
dod.isum+dod.idiscount 'cComUnitAmount',
dod.cdefine22 'pack',
dod.itaxunitprice 'ExercisePrice',
case  when t1.fAvailQtty<=0 then dod.iquantity
else  t1.fAvailQtty+dod.iquantity
end  'Stock',
dod.kl 'kl',
dod.cunitid 'cComUnitCode',
dod.iTaxRate 'iTaxRate',
dod.cn1cComUnitName 'cn1cComUnitName',
dod.irowno 'irowno'
 from Dl_opOrder do
inner join Dl_opOrderDetail dod on do.lngopOrderId=dod.lngopOrderId 
inner join Inventory iv on iv.cInvCode=dod.cinvcode
left join #temp t1 on t1.cInvCode=dod.cinvcode
where do.strBillNo =@strBillNo
order by irowno

END



GO
/****** Object:  StoredProcedure [dbo].[DLproc_OrderDetailModifyStockQtyCompareBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:echo
-- Create date:2015-12-21
-- Description:	用于用户修改订单时,比较库存是否可用
--参数:商品编码,单据编号
-- =============================================


CREATE PROCEDURE [dbo].[DLproc_OrderDetailModifyStockQtyCompareBySel]
@cinvcode varchar(50) ,
@strBillNo varchar(50) 
AS

BEGIN

	SET NOCOUNT ON;
declare @qty decimal(20,6),@fAvailQtty decimal(20,6)
	--插入库存数据,20151230禁用,启用新的可用量算法
--	select cInvCode,cInvCName,sum(isnull(fAvailQtty,0)*0.7) 'fAvailQtty' into #temp from (
--SELECT  CS.cExpirationdate as 有效期至,E1.enumname as 有效期推算方式,W.cWhCode, W.cWhName, I.cInvCode, I.cInvAddCode,  I.cInvName, I.cInvStd, I.cInvCCode , IC.cInvCName, 
-- CU_M.cComUnitName AS cInvM_Unit, CASE WHEN I.iGroupType = 0 THEN NULL  WHEN I.iGrouptype = 2 THEN CU_A.cComUnitName  WHEN I.iGrouptype = 1 THEN CU_G.cComUnitName END  AS cInvA_Unit,convert(nvarchar(38),
--  convert(decimal(38,2),CASE WHEN I.iGroupType = 0 THEN NULL      WHEN I.iGroupType = 2 THEN (CASE WHEN CS.iQuantity = 0.0 OR CS.iNum = 0.0 THEN NULL ELSE CS.iQuantity/CS.iNum END)      
-- WHEN I.iGroupType = 1 THEN CU_G.iChangRate END)) AS iExchRate,
--  Null as cInvDefine1, Null as cInvDefine2, Null as cInvDefine3, Null as cFree1, Null as cFree2, Null as cFree3, Null as cFree4, Null as cFree5, Null as cFree6, Null as cFree7, Null as cFree8, Null as cFree9, 
-- Null as cFree10, Null as cInvDefine4, Null as cInvDefine5, Null as cInvDefine6, Null as cInvDefine7, Null as cInvDefine8, Null as cInvDefine9, Null as cInvDefine10, Null as cInvDefine11, Null as cInvDefine12, 
--  Null as cInvDefine13, Null as cInvDefine14, Null as cInvDefine15, Null as cInvDefine16,cs.cBatch, cs.EnumName As iSoTypeName, cs.csocode as SOCode, cs.cdemandmemo,convert(nvarchar,cs.isoseq) as iRowNo,
--cs.cvmivencode,v1.cvenabbname as cvmivenname , isnull(E.enumname,N'') as cMassUnitName,CS.dVDate, CS.dMdate,convert(varchar(20),CS.iMassDate) as iMassDate,
-- (iQuantity) AS iQtty,( CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(iNum,0) WHEN iGroupType = 1 THEN iQuantity/ CU_G.iChangRate END) AS iNum,
--  CASE WHEN CS.bStopFlag = 1 OR CS.bGspStop = 1 THEN iQuantity ELSE IsNull(fStopQuantity,0) END AS iStopQtty,
--  CASE WHEN CS.bStopFlag = 1 OR CS.bGspStop = 1 THEN (CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(iNum,0) WHEN iGroupType = 1 THEN iQuantity/ CU_G.iChangRate END) 
-- ELSE (CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(fStopNum,0) WHEN iGroupType = 1 THEN fStopQuantity/ CU_G.iChangRate END) END AS iStopNum,
-- (fInQuantity) AS fInQtty, 
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fInNum,0) WHEN iGroupType = 1 THEN fInQuantity/ CU_G.iChangRate END) AS fInNum,
-- (fTransInQuantity) AS fTransInQtty,
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fTransInNum,0) WHEN iGroupType = 1 THEN fTransInQuantity/ CU_G.iChangRate END) AS fTransInNum,
-- (ISNULL(fInQuantity,0) + ISNULL(fTransInQuantity,0)) AS fInQttySum,
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fInNum,0) + ISNULL(fTransInNum,0) WHEN iGroupType = 1 THEN (ISNULL(fInQuantity,0) + ISNULL(fTransInQuantity,0))/ CU_G.iChangRate END) AS fInNumSum,
-- (fOutQuantity) AS fOutQtty, 
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fOutNum,0) WHEN iGroupType = 1 THEN fOutQuantity/ CU_G.iChangRate END) AS fOutNum,
-- CS.cBatchProperty1,CS.cBatchProperty2,CS.cBatchProperty3,CS.cBatchProperty4,CS.cBatchProperty5,CS.cBatchProperty6,CS.cBatchProperty7,CS.cBatchProperty8,CS.cBatchProperty9,CS.cBatchProperty10,
--  (fTransOutQuantity) AS fTransOutQtty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fTransOutNum,0) WHEN iGroupType = 1 THEN fTransOutQuantity/ CU_G.iChangRate END) AS fTransOutNum,
--  (ISNULL(fOutQuantity,0) + ISNULL(fTransOutQuantity,0)) AS fOutQttySum , 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fOutNum,0) + ISNULL(fTransOutNum,0) WHEN iGroupType = 1 THEN (ISNULL(fOutQuantity,0) + ISNULL(fTransOutQuantity,0))/ CU_G.iChangRate END) AS fOutNumSum,
--  (fDisableQuantity) AS fDisableQtty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fDisableNum,0) WHEN iGroupType = 1 THEN fDisableQuantity/ CU_G.iChangRate END) AS fDisableNum,
--  (ipeqty) AS fpeqty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(ipenum,0) WHEN iGroupType = 1 THEN ipeqty/ CU_G.iChangRate END) AS fpenum,
-- (CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END) AS fAvailQtty,dLastCheckDate, 
-- (CASE WHEN iGroupType = 0 THEN 0  WHEN iGroupType = 2 THEN  CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iNum,0)- IsNull(fStopNum,0) END  - ISNULL(fOutNum,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iNum,0)- IsNull(fStopNum,0) END  - ISNULL(fOutNum,0) END WHEN iGroupType = 1 THEN  (CASE WHEN bInvBatch=1 THEN  
-- CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END)/CU_G.iChangRate ELSE NULL END) AS fAvailNum
-- FROM v_ST_currentstockForReport  CS inner join dbo.Inventory I ON I.cInvCode = CS.cInvCode   
-- left join dbo.InventoryClass IC ON IC.cInvCCode = I.cInvCCode LEFT OUTER JOIN dbo.ComputationUnit CU_G ON I.cSTComUnitCode =CU_G.cComUnitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_A ON I.cAssComUnitCode = CU_A.cComunitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_M ON I.cComUnitCode = CU_M.cComunitCode 
-- LEFT OUTER JOIN dbo.Warehouse W ON CS.cWhCode = W.cWhCode 
-- left join vendor v1 on v1.cvencode = cs.cvmivencode 
-- left join v_aa_enum E1 on E1.enumcode = ISNULL(cs.iExpiratDateCalcu,0) and E1.enumtype=N'SCM.ExpiratDateCalcu' 
-- LEFT OUTER JOIN dbo.v_aa_enum E with (nolock) on E.enumcode=convert(nchar,CS.cMassUnit) and E.enumType=N'ST.MassUnit' 
-- WHERE  1=1  AND 1=1  
-- ) as HH
-- group by cInvCode,cInvCName
 --select @fAvailQtty=isnull(fAvailQtty,0) from #temp where cInvCode=@cinvcode

   ----组合查询可用量,20151230启用,20150106mod
declare @qty1 decimal(20,6),@ddate varchar(20),@opqty decimal(20,6)
select @ddate=convert(varchar(10),getdate(),120)
 Select   @qty=SUM(ISNULL(iQuantity,0)) from SA_CurrentStock 
 inner join warehouse on warehouse.cwhcode=SA_CurrentStock.cwhcode  
 where cInvCode=@cinvcode and binavailcalcu=1 and  (isotype=0 or (isotype=1 and isodid=N'')) AND isnull(bStopFlag,0)=0

 SELECT @qty1=Sum(Case When (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) < 0 Then 0 Else (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) End)  
FROM SO_SOMain 
INNER JOIN SO_SODetails ON SO_SOMain.ID = SO_SODetails.ID   INNER JOIN Inventory ON SO_SODetails.cInvCode = Inventory.cInvCode  left join ST_PELockedSum c with (nolock) on SO_SODetails.cInvCode = c.cinvcode and c.isotype =1 
and c.isodid = convert(nvarchar(40),ISNULL(SO_SODetails.isosid,N''))  WHERE  IsNull(SO_SODetails.csCloser ,N'') =N'' AND   IsNull(SO_SOMain.cBustype,N'') <> N'直运销售' AND   (Case When IsNull(SO_SODetails.dPreDate,N'') = N'' 
Then SO_SOMain.dDate Else SO_SODetails.dPreDate End ) <=@ddate And   SO_SODetails.cInvCode = @cinvcode  AND Isnull(SO_SODetails.cparentcode,'')= '' 

select @fAvailQtty=(isnull(@qty,0)-isnull(@qty1,0))*0.7

	--网上下单系统已存在的未生成U8订单和驳回修改的订单数量,待顾客确认的订单,这部分数量需要用U8数量减去
	select @fAvailQtty=@fAvailQtty-isnull(sum(iquantity),0)  from Dl_opOrderDetail dod
inner join Dl_opOrder do on do.lngopOrderId=dod.lngopOrderId
where dod.cinvcode=@cinvcode and do.bytStatus in (1,2,3)


--读取数据	/*20151221,修改,库存数据,计算逻辑:如果商品当前库存数量小于0,则为当前订单数量,否则为可用量+订单量*/
select @opqty=isnull(iquantity,0) --订单数量
 from Dl_opOrder do
inner join Dl_opOrderDetail dod on do.lngopOrderId=dod.lngopOrderId 
where do.strBillNo =@strBillNo and dod.cinvcode=@cinvcode

select @opqty=isnull(@opqty,0)

if @fAvailQtty<=0 
begin
select @opqty as 'qty'
end
else
begin
select @opqty+@fAvailQtty as 'qty'
end

END



GO
/****** Object:  StoredProcedure [dbo].[DLproc_OrderGetNewBillNoBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:echo
-- Create date:2015-10-23
-- Description:	用于查询,并创建最大订单流水号[DLproc_OrderGetNewBillNoBySel]
--订单号:WSXD+两位年+两位月+五位流水号
-- =============================================


CREATE PROCEDURE [dbo].[DLproc_OrderGetNewBillNoBySel]

AS

BEGIN

	SET NOCOUNT ON;

declare @maxbillno int,@return varchar(50)
--获取两位年,两位月,组合,如1510,1509
--select  'WSXD'+convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+'00001'
--获取最大单号
select @maxbillno=isnull(max(substring(strBillNo,5,9)),0)+1 from Dl_opOrder where strBillNo like 'DLOP%' 
IF @maxbillno=100000
BEGIN
select 'error'  'BillNo'
END
ELSE
BEGIN
--创建新的订单编号
select 'DLOP'+convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+right('0000'+convert(varchar(5),@maxbillno),5) 'BillNo'
END
return @return

END


GO
/****** Object:  StoredProcedure [dbo].[DLproc_OrderGetNewBillNoBySelU8]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





-- =============================================

-- Author:echo

-- Create date:2015-10-23

-- Description:	用于查询,并创建最大订单流水号[DLproc_OrderGetNewBillNoBySelU8]

--订单号:WSXD+两位年+两位月+五位流水号

-- =============================================





CREATE PROCEDURE [dbo].[DLproc_OrderGetNewBillNoBySelU8]

@BillNo varchar(20) OUTPUT

AS



BEGIN



	SET NOCOUNT ON;



declare @maxbillno int

--获取两位年,两位月,组合,如1510,1509

--select  'WSXD'+convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+'00001'

--获取最大单号

select @maxbillno=isnull(max(substring(csocode,5,9)),0)+1 from SO_SOMain where csocode like 'wsxd%' 

IF @maxbillno=100000

BEGIN

--select 'error'  'BillNo'

set @BillNo='error'

END

ELSE

BEGIN

--创建新的订单编号

--select 'WSXD'+convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+right('0000'+convert(varchar(5),@maxbillno),5) 'BillNo'

select @BillNo='WSXD'+convert(varchar(2),right(Year(GetDate()),2))+convert(varchar(2),Right(100+Month(GetDate()),2))+right('0000'+convert(varchar(5),@maxbillno),5)

END





END


GO
/****** Object:  StoredProcedure [dbo].[DLproc_OrderYDetailModifyBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:echo
-- Create date:2016-01-14
-- Description:	用于查询用户指定的被驳回需要修改的参照订单[DLproc_OrderYDetailModifyBySel]
--参数:单据编号
-- =============================================


CREATE PROCEDURE [dbo].[DLproc_OrderYDetailModifyBySel]
@strBillNo varchar(50) --被修改订单编码(这部分数量要被扣除,2016-01-15,增加,不光此订单要被扣除,整个状态为3的该顾客的被驳回订单数量都需要扣除,)
--@OrderBillNo varchar(30) --预订单编号
--@lngBillType int	--单据类型
AS

BEGIN

	SET NOCOUNT ON;
	
--20151230,新可用量算法,生成U8订单后,即扣减订单数量
/*20151226,可用量查询,生成U8订单后,扣减该数量*/
  ----组合查询可用量
  --订单中的物料
  select bb.cinvcode into #tempcinvcode from Dl_opOrder aa
  inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId where aa.strBillNo=@strBillNo
--declare @qty1 decimal(20,6),@qty decimal(20,6),@ddate varchar(20)
declare @ddate varchar(20)
select @ddate=convert(varchar(10),getdate(),120)
/*V2.0:2015-01-06新算法---BEGIN*/
 Select   cInvCode 'cinvcode',SUM(ISNULL(iQuantity,0))  'qty' into #temp11 from SA_CurrentStock 
 inner join warehouse on warehouse.cwhcode=SA_CurrentStock.cwhcode  
 where cInvCode in (select cinvcode from #tempcinvcode ) and binavailcalcu=1 and  (isotype=0 or (isotype=1 and isodid=N'')) AND isnull(bStopFlag,0)=0
 group by cInvCode

 SELECT  SO_SODetails.cInvCode 'cinvcode',Sum(Case When (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) < 0 Then 0 Else (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) End)  'qty1'  into #temp22
FROM SO_SOMain 
INNER JOIN SO_SODetails ON SO_SOMain.ID = SO_SODetails.ID   INNER JOIN Inventory ON SO_SODetails.cInvCode = Inventory.cInvCode  left join ST_PELockedSum c with (nolock) on SO_SODetails.cInvCode = c.cinvcode and c.isotype =1 
and c.isodid = convert(nvarchar(40),ISNULL(SO_SODetails.isosid,N''))  WHERE  IsNull(SO_SODetails.csCloser ,N'') =N'' AND   IsNull(SO_SOMain.cBustype,N'') <> N'直运销售' AND   (Case When IsNull(SO_SODetails.dPreDate,N'') = N'' 
Then SO_SOMain.dDate Else SO_SODetails.dPreDate End ) <=@ddate And   SO_SODetails.cInvCode  in (select cinvcode from #tempcinvcode )   AND Isnull(SO_SODetails.cparentcode,'')= '' 
group by SO_SODetails.cInvCode
/*V2.0:2015-01-06新算法---END*/
	--网上下单系统已存在的未生成U8订单和驳回修改的订单数量,待顾客确认的订单,这部分数量需要用U8数量减去,(不排除当前订单数量)
	select dod.cinvcode,isnull(sum(isnull(iquantity,0)),0) 'opqty' into #temp33  from Dl_opOrderDetail dod
inner join Dl_opOrder do on do.lngopOrderId=dod.lngopOrderId
where dod.cinvcode  in (select cinvcode from #tempcinvcode ) and do.bytStatus in (1,2,3) 
--and do.strBillNo!=@strBillNo
group by  dod.cinvcode

select aa.cinvcode,(sum(isnull(aa.qty,0))-sum(isnull(bb.qty1,0)))*0.7-sum(isnull(cc.opqty,0)) 'fAvailQtty' into #temp from #temp11 aa
left join #temp22 bb on aa.cinvcode=bb.cinvcode
left join #temp33 cc on aa.cinvcode=cc.cinvcode
group by aa.cinvcode

--2016-01-14,预订单号,预订单号,可用量,预订单autoid
	--cSCloser,行关闭人,null
--获取对应的U8的订单编号,数量,可用量,
--根据网单号,查询被修改订单的对应的U8预订单的商品的可用量(总数量-累计订单数量)
select bb.cinvcode,bb.cpreordercode,bb.cinvcode+bb.cpreordercode 'itemid',isnull(spod.iQuantity,0)-isnull(spod.fdhquantity,0) 'realqty',bb.iaoids into #temp111 from Dl_opOrder aa inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId 
left join SA_PreOrderDetails spod on spod.cInvCode=bb.cinvcode and bb.iaoids=spod.autoid
where aa.strBillNo=@strBillNo and   spod.cSCloser is null 
--获取要修改的订单类型,便于后续查询该类型订单数据
--网上订单系统中,待审核/待确认/待修改状态的订单数量(扣除当前被修改订单的单据数量),2016-01-15,排除订单状态为3的单据数量,并且状态为1,2的数量为该顾客的订单数量
select bb.cinvcode,bb.iaoids,sum(bb.iquantity) 'iquantity' into #temp222 from  Dl_opOrder aa inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId
where aa.lngBillType=(select lngBillType from Dl_opOrder where strBillNo=@strBillNo) and bytStatus in (1,2) and aa.strBillNo!=@strBillNo and aa.ccuscode=(select top(1) ccuscode from Dl_opOrder where strBillNo=@strBillNo)
group by bb.cinvcode,bb.iaoids
--汇总数据,cinvcode	cpreordercode	itemid	realqty	iaoids
--01010100102	Y151200028	01010100102Y151200028	36	1000000233
select aa.cinvcode,aa.cpreordercode,aa.itemid,case when (aa.realqty-isnull(bb.iquantity,0)) >0 then (aa.realqty-isnull(bb.iquantity,0))  else 0 end realqty,aa.iaoids into #temp333 from #temp111 aa left join #temp222 bb on aa.cinvcode=bb.cinvcode and aa.iaoids=bb.iaoids  
	-----------------------------------------------------------------------------
--2016-01-15:获取物料对应的库存可用量数据


--读取数据	/*20151221,修改,库存数据,计算逻辑:如果商品当前库存数量小于0,则为当前订单数量,否则为库存量+订单量*/
select 
dod.cinvcode 'cInvCode',
dod.cinvname 'cInvName',
iv.cInvStd 'cInvStd',
dod.cComUnitName 'cComUnitName',
dod.cInvDefine1 'cInvDefine1',
dod.cInvDefine2 'cInvDefine2',
dod.cInvDefine13 'cInvDefine13',
dod.cInvDefine14 'cInvDefine14',
dod.UnitGroup 'UnitGroup',
dod.cComUnitQTY 'cComUnitQTY',
dod.cInvDefine1QTY 'cInvDefine1QTY',
dod.cInvDefine2QTY 'cInvDefine2QTY',
dod.iquantity 'cInvDefineQTY',
dod.iquotedprice 'cComUnitPrice',
dod.isum+dod.idiscount 'cComUnitAmount',
dod.cdefine22 'pack',
dod.itaxunitprice 'ExercisePrice',
case  when t1.fAvailQtty<=0 then dod.iquantity
else  t1.fAvailQtty+dod.iquantity
end  'Stock',
dod.kl 'kl',
dod.cunitid 'cComUnitCode',
dod.iTaxRate 'iTaxRate',
dod.cn1cComUnitName 'cn1cComUnitName',
--dod.irowno 'irowno'
dod.cpreordercode 'ccode',
t3.itemid,
t3.realqty,
t3.iaoids
 from Dl_opOrder do
inner join Dl_opOrderDetail dod on do.lngopOrderId=dod.lngopOrderId 
inner join Inventory iv on iv.cInvCode=dod.cinvcode
left join #temp t1 on t1.cInvCode=dod.cinvcode
left join #temp333 t3 on dod.cinvcode=t3.cinvcode and dod.iaoids=t3.iaoids
where do.strBillNo =@strBillNo
order by irowno





END



GO
/****** Object:  StoredProcedure [dbo].[DLproc_QuasiOrderDetailBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec DLproc_QuasiOrderDetailBySel '01010100101','010101'
-- =============================================
-- Author:echo
-- Create date:2015-10-25
-- Description:	用于查询左边菜单传过来的物料详情
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_QuasiOrderDetailBySel]

@cinvcode varchar(50),
@cCusCode varchar(50)

AS

BEGIN
SET NOCOUNT ON;
	/*start10-31添加,获取物料的价格*/
		/*11-03修改:添加*/
declare @p17 float
set @p17=0
declare @p18 float
set @p18=0
declare @p19 float
set @p19=NULL
declare @p20 float
set @p20=100
declare @p21 float
set @p21=NULL
declare @p22 nvarchar(200)
set @p22=NULL
declare @p23 bit
set @p23=1
declare @bOnsale  int
set  @bOnsale=0
declare @nOriginalPrice float
set @nOriginalPrice=NULL
declare @date varchar(10)
select @date=convert(varchar(10),getdate(),120)
exec SA_FetchPrice1 @cCusCode,@cinvcode,N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',@date,N'人民币',0,0,
@p17 output,@p18 output,@p19 output,@p20 output,@p21 output,@p22 output,@p23 output,@bOnsale output,@nOriginalPrice output,N''
--select @p17, @p18, @p19, @p20, @p21, @p22, @p23
	/*end10-31添加,获取物料的价格*/
	/*Start,11-01,添加获取库存可用量查询*/
	declare @fAvailQtty decimal(18,3)
	--U8可用量查询(老的查询方式,可用量不会扣减保存的销售订单数量,20151226采用新的可用量计算方式,如下)
--select @fAvailQtty=(sum(fAvailQtty)*0.7)  from (
--SELECT  CS.cExpirationdate as 有效期至,E1.enumname as 有效期推算方式,W.cWhCode, W.cWhName, I.cInvCode, I.cInvAddCode,  I.cInvName, I.cInvStd, I.cInvCCode , IC.cInvCName, 
-- CU_M.cComUnitName AS cInvM_Unit, CASE WHEN I.iGroupType = 0 THEN NULL  WHEN I.iGrouptype = 2 THEN CU_A.cComUnitName  WHEN I.iGrouptype = 1 THEN CU_G.cComUnitName END  AS cInvA_Unit,convert(nvarchar(38),
--  convert(decimal(38,2),CASE WHEN I.iGroupType = 0 THEN NULL      WHEN I.iGroupType = 2 THEN (CASE WHEN CS.iQuantity = 0.0 OR CS.iNum = 0.0 THEN NULL ELSE CS.iQuantity/CS.iNum END)      
-- WHEN I.iGroupType = 1 THEN CU_G.iChangRate END)) AS iExchRate,
--  Null as cInvDefine1, Null as cInvDefine2, Null as cInvDefine3, Null as cFree1, Null as cFree2, Null as cFree3, Null as cFree4, Null as cFree5, Null as cFree6, Null as cFree7, Null as cFree8, Null as cFree9, 
-- Null as cFree10, Null as cInvDefine4, Null as cInvDefine5, Null as cInvDefine6, Null as cInvDefine7, Null as cInvDefine8, Null as cInvDefine9, Null as cInvDefine10, Null as cInvDefine11, Null as cInvDefine12, 
--  Null as cInvDefine13, Null as cInvDefine14, Null as cInvDefine15, Null as cInvDefine16,cs.cBatch, cs.EnumName As iSoTypeName, cs.csocode as SOCode, cs.cdemandmemo,convert(nvarchar,cs.isoseq) as iRowNo,
--cs.cvmivencode,v1.cvenabbname as cvmivenname , isnull(E.enumname,N'') as cMassUnitName,CS.dVDate, CS.dMdate,convert(varchar(20),CS.iMassDate) as iMassDate,
-- (iQuantity) AS iQtty,( CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(iNum,0) WHEN iGroupType = 1 THEN iQuantity/ CU_G.iChangRate END) AS iNum,
--  CASE WHEN CS.bStopFlag = 1 OR CS.bGspStop = 1 THEN iQuantity ELSE IsNull(fStopQuantity,0) END AS iStopQtty,
--  CASE WHEN CS.bStopFlag = 1 OR CS.bGspStop = 1 THEN (CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(iNum,0) WHEN iGroupType = 1 THEN iQuantity/ CU_G.iChangRate END) 
-- ELSE (CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(fStopNum,0) WHEN iGroupType = 1 THEN fStopQuantity/ CU_G.iChangRate END) END AS iStopNum,
-- (fInQuantity) AS fInQtty, 
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fInNum,0) WHEN iGroupType = 1 THEN fInQuantity/ CU_G.iChangRate END) AS fInNum,
-- (fTransInQuantity) AS fTransInQtty,
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fTransInNum,0) WHEN iGroupType = 1 THEN fTransInQuantity/ CU_G.iChangRate END) AS fTransInNum,
-- (ISNULL(fInQuantity,0) + ISNULL(fTransInQuantity,0)) AS fInQttySum,
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fInNum,0) + ISNULL(fTransInNum,0) WHEN iGroupType = 1 THEN (ISNULL(fInQuantity,0) + ISNULL(fTransInQuantity,0))/ CU_G.iChangRate END) AS fInNumSum,
-- (fOutQuantity) AS fOutQtty, 
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fOutNum,0) WHEN iGroupType = 1 THEN fOutQuantity/ CU_G.iChangRate END) AS fOutNum,
-- CS.cBatchProperty1,CS.cBatchProperty2,CS.cBatchProperty3,CS.cBatchProperty4,CS.cBatchProperty5,CS.cBatchProperty6,CS.cBatchProperty7,CS.cBatchProperty8,CS.cBatchProperty9,CS.cBatchProperty10,
--  (fTransOutQuantity) AS fTransOutQtty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fTransOutNum,0) WHEN iGroupType = 1 THEN fTransOutQuantity/ CU_G.iChangRate END) AS fTransOutNum,
--  (ISNULL(fOutQuantity,0) + ISNULL(fTransOutQuantity,0)) AS fOutQttySum , 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fOutNum,0) + ISNULL(fTransOutNum,0) WHEN iGroupType = 1 THEN (ISNULL(fOutQuantity,0) + ISNULL(fTransOutQuantity,0))/ CU_G.iChangRate END) AS fOutNumSum,
--  (fDisableQuantity) AS fDisableQtty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fDisableNum,0) WHEN iGroupType = 1 THEN fDisableQuantity/ CU_G.iChangRate END) AS fDisableNum,
--  (ipeqty) AS fpeqty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(ipenum,0) WHEN iGroupType = 1 THEN ipeqty/ CU_G.iChangRate END) AS fpenum,
-- (CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END) AS fAvailQtty,dLastCheckDate, 
-- (CASE WHEN iGroupType = 0 THEN 0  WHEN iGroupType = 2 THEN  CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iNum,0)- IsNull(fStopNum,0) END  - ISNULL(fOutNum,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iNum,0)- IsNull(fStopNum,0) END  - ISNULL(fOutNum,0) END WHEN iGroupType = 1 THEN  (CASE WHEN bInvBatch=1 THEN  
-- CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END)/CU_G.iChangRate ELSE NULL END) AS fAvailNum
-- FROM v_ST_currentstockForReport  CS inner join dbo.Inventory I ON I.cInvCode = CS.cInvCode   
-- left join dbo.InventoryClass IC ON IC.cInvCCode = I.cInvCCode LEFT OUTER JOIN dbo.ComputationUnit CU_G ON I.cSTComUnitCode =CU_G.cComUnitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_A ON I.cAssComUnitCode = CU_A.cComunitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_M ON I.cComUnitCode = CU_M.cComunitCode 
-- LEFT OUTER JOIN dbo.Warehouse W ON CS.cWhCode = W.cWhCode 
-- left join vendor v1 on v1.cvencode = cs.cvmivencode 
-- left join v_aa_enum E1 on E1.enumcode = ISNULL(cs.iExpiratDateCalcu,0) and E1.enumtype=N'SCM.ExpiratDateCalcu' 
-- LEFT OUTER JOIN dbo.v_aa_enum E with (nolock) on E.enumcode=convert(nchar,CS.cMassUnit) and E.enumType=N'ST.MassUnit' 
-- WHERE  1=1  AND 1=1   And I.cInvCode = @cinvcode
-- ) as HH

/*20151226,可用量查询,生成U8订单后,扣减该数量*/
  ----组合查询可用量
declare @qty1 decimal(20,6),@qty decimal(20,6),@ddate varchar(20)
select @ddate=convert(varchar(10),getdate(),120)

/*V1.0:算法修正成V2.0*/
-- Select   @qty=SUM(ISNULL(iQuantity,0)-isnull(fStopQuantity,0) +ISNULL(fInQuantity,0) - ISNULL(fOutQuantity,0)) from SA_CurrentStock 
-- inner join warehouse on warehouse.cwhcode=SA_CurrentStock.cwhcode  
-- where cInvCode=@cinvcode and binavailcalcu=1 and  (isotype=0 or (isotype=1 and isodid=N'')) AND isnull(bStopFlag,0)=0
-- SELECT @qty1=Sum(Case When (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) < 0 Then 0 Else (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) End)  
--FROM SO_SOMain 
--INNER JOIN SO_SODetails ON SO_SOMain.ID = SO_SODetails.ID   INNER JOIN Inventory ON SO_SODetails.cInvCode = Inventory.cInvCode  left join ST_PELockedSum c with (nolock) on SO_SODetails.cInvCode = c.cinvcode and c.isotype =1 
--and c.isodid = convert(nvarchar(40),ISNULL(SO_SODetails.isosid,N''))  WHERE  IsNull(SO_SODetails.csCloser ,N'') =N'' AND   IsNull(SO_SOMain.cBustype,N'') <> N'直运销售' AND   (Case When IsNull(SO_SODetails.dPreDate,N'') = N'' 
--Then SO_SOMain.dDate Else SO_SODetails.dPreDate End ) <=@ddate And   SO_SODetails.cInvCode = @cinvcode  AND Isnull(SO_SODetails.cparentcode,'')= '' 

/*V2.0:2015-01-06新算法---BEGIN*/

 Select   @qty=sum(iQuantity) from SA_CurrentStock inner join warehouse on warehouse.cwhcode=SA_CurrentStock.cwhcode  
 where cInvCode=@cinvcode and binavailcalcu=1 and  (isotype=0 or (isotype=1 and isodid=N'')) AND isnull(bStopFlag,0)=0

 SELECT @qty1=Sum(Case When (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) < 0 Then 0 Else (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) End) FROM SO_SOMain 
INNER JOIN SO_SODetails ON SO_SOMain.ID = SO_SODetails.ID   INNER JOIN Inventory ON SO_SODetails.cInvCode = Inventory.cInvCode  left join ST_PELockedSum c with (nolock) on SO_SODetails.cInvCode = c.cinvcode and c.isotype =1 
and c.isodid = convert(nvarchar(40),ISNULL(SO_SODetails.isosid,N''))  WHERE  IsNull(SO_SODetails.csCloser ,N'') =N'' AND   IsNull(SO_SOMain.cBustype,N'') <> N'直运销售' AND   (Case When IsNull(SO_SODetails.dPreDate,N'') = N'' 
Then SO_SOMain.dDate Else SO_SODetails.dPreDate End ) <=@ddate And   SO_SODetails.cInvCode = @cinvcode  AND Isnull(SO_SODetails.cparentcode,'')= '' 
/*V2.0:2015-01-06新算法---END*/

select @fAvailQtty=(isnull(@qty,0)-isnull(@qty1,0))*0.7

	--网上下单系统已存在的未生成U8订单和驳回修改的订单数量,待顾客确认的订单,这部分数量需要用U8数量减去
select @fAvailQtty=@fAvailQtty-isnull(sum(iquantity),0)  from Dl_opOrderDetail dod
inner join Dl_opOrder do on do.lngopOrderId=dod.lngopOrderId
where dod.cinvcode=@cinvcode and do.bytStatus in (1,2,3)
if @fAvailQtty<0 
begin
set @fAvailQtty=0
end
 	/*END,11-01,添加获取库存可用量查询*/

if exists (select * from Inventory where cInvCode=@cinvcode and iGroupType=0)	--iGroupType:0,无换算率,1固定换算率,2浮动换算率
begin 
select it.cInvName,isnull(it.cInvStd,' ') 'cInvStd',cn.cComUnitName,it.cInvCode ,isnull(cn.iChangRate,1) 'iChangRate',isnull(cn1.cComUnitName,' ') 'cSAComUnit'
,@p17 'nOriginalPrice',isnull(@p19,0) 'BeforeExercisePrice', @p20 'Rate',isnull(@fAvailQtty,0) 'fAvailQtty',@bOnsale 'bOnsale',it.cComUnitCode,it.iTaxRate,isnull(cn1.cComUnitName,' ') 'cn1cComUnitName',
@p17*@p20/100 'ExercisePrice',@nOriginalPrice 'Quote'
from Inventory it
--inner join ComputationGroup cg on it.cGroupCode=cg.cGroupCode
inner join computationunit cn on it.cGroupCode=cn.cGroupCode
left join computationunit cn1 on cn1.cComunitCode=it.cSAComUnitCode
where it.cInvCode=@cinvcode   and cn.cComunitCode=it.cComUnitCode --添加条件,过滤,只取主计量单位
order by cn.bMainUnit desc,cn.iChangRate
end
else	--iGroupType:0,无换算率,1固定换算率,2浮动换算率
begin
select it.cInvName,isnull(it.cInvStd,' ') 'cInvStd',cn.cComUnitName,it.cInvCode ,isnull(cn.iChangRate,1) 'iChangRate',isnull(cn1.cComUnitName,' ') 'cSAComUnit'
,@p17 'nOriginalPrice',isnull(@p19,0) 'BeforeExercisePrice', @p20 'Rate',isnull(@fAvailQtty,0) 'fAvailQtty',@bOnsale 'bOnsale',it.cComUnitCode,it.iTaxRate,isnull(cn1.cComUnitName,' ') 'cn1cComUnitName',
@p17*@p20/100 'ExercisePrice',@nOriginalPrice 'Quote'
from Inventory it
--inner join ComputationGroup cg on it.cGroupCode=cg.cGroupCode
inner join computationunit cn on it.cGroupCode=cn.cGroupCode
left join computationunit cn1 on cn1.cComunitCode=it.cSAComUnitCode
where it.cInvCode=@cinvcode
order by cn.bMainUnit desc,cn.iChangRate
end


--exec DLproc_QuasiOrderDetailBySel '01010100101','010101'
--exec DLproc_QuasiOrderDetailBySel '32020103','010101'
--exec DLproc_QuasiOrderDetailBySel '020101001','010101'
--exec DLproc_QuasiOrderDetailBySel '01080100101','010113'
--exec DLproc_QuasiOrderDetailBySel '01060200105','010113'

END


GO
/****** Object:  StoredProcedure [dbo].[DLproc_QuasiOrderDetailModifyBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [DLproc_QuasiOrderDetailModifyBySel] '01010100101','010101'
-- =============================================
-- Author:echo
-- Create date:2015-10-28
-- Description:	用于查询左边菜单传过来的物料详情(操作员修改订单,查询可用量)
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_QuasiOrderDetailModifyBySel]

@cinvcode varchar(50),
@cCusCode varchar(50),
@strBillNo varchar(50)

AS

BEGIN
SET NOCOUNT ON;
	/*start10-31添加,获取物料的价格*/
		/*11-03修改:添加*/
declare @p17 float
set @p17=0
declare @p18 float
set @p18=0
declare @p19 float
set @p19=NULL
declare @p20 float
set @p20=100
declare @p21 float
set @p21=NULL
declare @p22 nvarchar(200)
set @p22=NULL
declare @p23 bit
set @p23=1
declare @bOnsale  int
set  @bOnsale=0
declare @nOriginalPrice float
set @nOriginalPrice=NULL
declare @date varchar(10)
select @date=convert(varchar(10),getdate(),120)
exec SA_FetchPrice1 @cCusCode,@cinvcode,N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',@date,N'人民币',0,0,
@p17 output,@p18 output,@p19 output,@p20 output,@p21 output,@p22 output,@p23 output,@bOnsale output,@nOriginalPrice output,N''
--select @p17, @p18, @p19, @p20, @p21, @p22, @p23
	/*end10-31添加,获取物料的价格*/
	/*Start,11-01,添加获取库存可用量查询*/
	declare @fAvailQtty decimal(18,3)
	--U8可用量查询(老的查询方式,可用量不会扣减保存的销售订单数量,20151226采用新的可用量计算方式,如下)
--select @fAvailQtty=(sum(fAvailQtty)*0.7)  from (
--SELECT  CS.cExpirationdate as 有效期至,E1.enumname as 有效期推算方式,W.cWhCode, W.cWhName, I.cInvCode, I.cInvAddCode,  I.cInvName, I.cInvStd, I.cInvCCode , IC.cInvCName, 
-- CU_M.cComUnitName AS cInvM_Unit, CASE WHEN I.iGroupType = 0 THEN NULL  WHEN I.iGrouptype = 2 THEN CU_A.cComUnitName  WHEN I.iGrouptype = 1 THEN CU_G.cComUnitName END  AS cInvA_Unit,convert(nvarchar(38),
--  convert(decimal(38,2),CASE WHEN I.iGroupType = 0 THEN NULL      WHEN I.iGroupType = 2 THEN (CASE WHEN CS.iQuantity = 0.0 OR CS.iNum = 0.0 THEN NULL ELSE CS.iQuantity/CS.iNum END)      
-- WHEN I.iGroupType = 1 THEN CU_G.iChangRate END)) AS iExchRate,
--  Null as cInvDefine1, Null as cInvDefine2, Null as cInvDefine3, Null as cFree1, Null as cFree2, Null as cFree3, Null as cFree4, Null as cFree5, Null as cFree6, Null as cFree7, Null as cFree8, Null as cFree9, 
-- Null as cFree10, Null as cInvDefine4, Null as cInvDefine5, Null as cInvDefine6, Null as cInvDefine7, Null as cInvDefine8, Null as cInvDefine9, Null as cInvDefine10, Null as cInvDefine11, Null as cInvDefine12, 
--  Null as cInvDefine13, Null as cInvDefine14, Null as cInvDefine15, Null as cInvDefine16,cs.cBatch, cs.EnumName As iSoTypeName, cs.csocode as SOCode, cs.cdemandmemo,convert(nvarchar,cs.isoseq) as iRowNo,
--cs.cvmivencode,v1.cvenabbname as cvmivenname , isnull(E.enumname,N'') as cMassUnitName,CS.dVDate, CS.dMdate,convert(varchar(20),CS.iMassDate) as iMassDate,
-- (iQuantity) AS iQtty,( CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(iNum,0) WHEN iGroupType = 1 THEN iQuantity/ CU_G.iChangRate END) AS iNum,
--  CASE WHEN CS.bStopFlag = 1 OR CS.bGspStop = 1 THEN iQuantity ELSE IsNull(fStopQuantity,0) END AS iStopQtty,
--  CASE WHEN CS.bStopFlag = 1 OR CS.bGspStop = 1 THEN (CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(iNum,0) WHEN iGroupType = 1 THEN iQuantity/ CU_G.iChangRate END) 
-- ELSE (CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(fStopNum,0) WHEN iGroupType = 1 THEN fStopQuantity/ CU_G.iChangRate END) END AS iStopNum,
-- (fInQuantity) AS fInQtty, 
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fInNum,0) WHEN iGroupType = 1 THEN fInQuantity/ CU_G.iChangRate END) AS fInNum,
-- (fTransInQuantity) AS fTransInQtty,
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fTransInNum,0) WHEN iGroupType = 1 THEN fTransInQuantity/ CU_G.iChangRate END) AS fTransInNum,
-- (ISNULL(fInQuantity,0) + ISNULL(fTransInQuantity,0)) AS fInQttySum,
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fInNum,0) + ISNULL(fTransInNum,0) WHEN iGroupType = 1 THEN (ISNULL(fInQuantity,0) + ISNULL(fTransInQuantity,0))/ CU_G.iChangRate END) AS fInNumSum,
-- (fOutQuantity) AS fOutQtty, 
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fOutNum,0) WHEN iGroupType = 1 THEN fOutQuantity/ CU_G.iChangRate END) AS fOutNum,
-- CS.cBatchProperty1,CS.cBatchProperty2,CS.cBatchProperty3,CS.cBatchProperty4,CS.cBatchProperty5,CS.cBatchProperty6,CS.cBatchProperty7,CS.cBatchProperty8,CS.cBatchProperty9,CS.cBatchProperty10,
--  (fTransOutQuantity) AS fTransOutQtty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fTransOutNum,0) WHEN iGroupType = 1 THEN fTransOutQuantity/ CU_G.iChangRate END) AS fTransOutNum,
--  (ISNULL(fOutQuantity,0) + ISNULL(fTransOutQuantity,0)) AS fOutQttySum , 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fOutNum,0) + ISNULL(fTransOutNum,0) WHEN iGroupType = 1 THEN (ISNULL(fOutQuantity,0) + ISNULL(fTransOutQuantity,0))/ CU_G.iChangRate END) AS fOutNumSum,
--  (fDisableQuantity) AS fDisableQtty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fDisableNum,0) WHEN iGroupType = 1 THEN fDisableQuantity/ CU_G.iChangRate END) AS fDisableNum,
--  (ipeqty) AS fpeqty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(ipenum,0) WHEN iGroupType = 1 THEN ipeqty/ CU_G.iChangRate END) AS fpenum,
-- (CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END) AS fAvailQtty,dLastCheckDate, 
-- (CASE WHEN iGroupType = 0 THEN 0  WHEN iGroupType = 2 THEN  CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iNum,0)- IsNull(fStopNum,0) END  - ISNULL(fOutNum,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iNum,0)- IsNull(fStopNum,0) END  - ISNULL(fOutNum,0) END WHEN iGroupType = 1 THEN  (CASE WHEN bInvBatch=1 THEN  
-- CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END)/CU_G.iChangRate ELSE NULL END) AS fAvailNum
-- FROM v_ST_currentstockForReport  CS inner join dbo.Inventory I ON I.cInvCode = CS.cInvCode   
-- left join dbo.InventoryClass IC ON IC.cInvCCode = I.cInvCCode LEFT OUTER JOIN dbo.ComputationUnit CU_G ON I.cSTComUnitCode =CU_G.cComUnitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_A ON I.cAssComUnitCode = CU_A.cComunitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_M ON I.cComUnitCode = CU_M.cComunitCode 
-- LEFT OUTER JOIN dbo.Warehouse W ON CS.cWhCode = W.cWhCode 
-- left join vendor v1 on v1.cvencode = cs.cvmivencode 
-- left join v_aa_enum E1 on E1.enumcode = ISNULL(cs.iExpiratDateCalcu,0) and E1.enumtype=N'SCM.ExpiratDateCalcu' 
-- LEFT OUTER JOIN dbo.v_aa_enum E with (nolock) on E.enumcode=convert(nchar,CS.cMassUnit) and E.enumType=N'ST.MassUnit' 
-- WHERE  1=1  AND 1=1   And I.cInvCode = @cinvcode
-- ) as HH

/*20151226,可用量查询,生成U8订单后,扣减该数量*/
--  ----组合查询可用量
--declare @qty1 decimal(20,6),@qty decimal(20,6),@ddate varchar(20)
--select @ddate=convert(varchar(10),getdate(),120)
-- Select   @qty=SUM(ISNULL(iQuantity,0)) from SA_CurrentStock 
-- inner join warehouse on warehouse.cwhcode=SA_CurrentStock.cwhcode  
-- where cInvCode=@cinvcode and binavailcalcu=1 and  (isotype=0 or (isotype=1 and isodid=N'')) AND isnull(bStopFlag,0)=0
-- SELECT @qty1=Sum(Case When (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) < 0 Then 0 Else (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) End)  
--FROM SO_SOMain 
--INNER JOIN SO_SODetails ON SO_SOMain.ID = SO_SODetails.ID   INNER JOIN Inventory ON SO_SODetails.cInvCode = Inventory.cInvCode  left join ST_PELockedSum c with (nolock) on SO_SODetails.cInvCode = c.cinvcode and c.isotype =1 
--and c.isodid = convert(nvarchar(40),ISNULL(SO_SODetails.isosid,N''))  WHERE  IsNull(SO_SODetails.csCloser ,N'') =N'' AND   IsNull(SO_SOMain.cBustype,N'') <> N'直运销售' AND   (Case When IsNull(SO_SODetails.dPreDate,N'') = N'' 
--Then SO_SOMain.dDate Else SO_SODetails.dPreDate End ) <=@ddate And   SO_SODetails.cInvCode = @cinvcode  AND Isnull(SO_SODetails.cparentcode,'')= '' 
--select @fAvailQtty=(isnull(@qty,0)-isnull(@qty1,0))*0.7

/*V2.0:2015-01-06新算法---BEGIN*/
declare @ddate varchar(20)
select @ddate=convert(varchar(10),getdate(),120)
 Select   cInvCode 'cinvcode',SUM(ISNULL(iQuantity,0))  'qty' into #temp11 from SA_CurrentStock 
 inner join warehouse on warehouse.cwhcode=SA_CurrentStock.cwhcode  
 where cInvCode =@cinvcode and binavailcalcu=1 and  (isotype=0 or (isotype=1 and isodid=N'')) AND isnull(bStopFlag,0)=0
 group by cInvCode

 SELECT  SO_SODetails.cInvCode 'cinvcode',Sum(Case When (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) < 0 Then 0 Else (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) End)  'qty1'  into #temp22
FROM SO_SOMain 
INNER JOIN SO_SODetails ON SO_SOMain.ID = SO_SODetails.ID   INNER JOIN Inventory ON SO_SODetails.cInvCode = Inventory.cInvCode  left join ST_PELockedSum c with (nolock) on SO_SODetails.cInvCode = c.cinvcode and c.isotype =1 
and c.isodid = convert(nvarchar(40),ISNULL(SO_SODetails.isosid,N''))  WHERE  IsNull(SO_SODetails.csCloser ,N'') =N'' AND   IsNull(SO_SOMain.cBustype,N'') <> N'直运销售' AND   (Case When IsNull(SO_SODetails.dPreDate,N'') = N'' 
Then SO_SOMain.dDate Else SO_SODetails.dPreDate End ) <=@ddate And   SO_SODetails.cInvCode =@cinvcode  AND Isnull(SO_SODetails.cparentcode,'')= '' 
group by SO_SODetails.cInvCode
/*V2.0:2015-01-06新算法---END*/

--	--网上下单系统已存在的未生成U8订单和驳回修改的订单数量,待顾客确认的订单,这部分数量需要用U8数量减去,2016-01-21注视
--select @fAvailQtty=@fAvailQtty-isnull(sum(iquantity),0)  from Dl_opOrderDetail dod
--inner join Dl_opOrder do on do.lngopOrderId=dod.lngopOrderId
--where dod.cinvcode=@cinvcode and do.bytStatus in (1,2,3)
--if @fAvailQtty<0 
--begin
--set @fAvailQtty=0
--end

	--网上下单系统已存在的未生成U8订单和驳回修改的订单数量,待顾客确认的订单,这部分数量需要用U8数量减去,(不排除当前订单数量).2016-01-21add
	select dod.cinvcode,isnull(sum(isnull(iquantity,0)),0) 'opqty' into #temp33  from Dl_opOrderDetail dod
inner join Dl_opOrder do on do.lngopOrderId=dod.lngopOrderId
where dod.cinvcode=@cinvcode and do.bytStatus in (1,2,3) 
--and do.strBillNo!=@strBillNo
group by  dod.cinvcode

select @fAvailQtty=(sum(isnull(aa.qty,0))-sum(isnull(bb.qty1,0)))*0.7-sum(isnull(cc.opqty,0)) from #temp11 aa
left join #temp22 bb on aa.cinvcode=bb.cinvcode
left join #temp33 cc on aa.cinvcode=cc.cinvcode
group by aa.cinvcode

--2015,12,28,加上当前订单的数量
select @fAvailQtty=@fAvailQtty+isnull(bb.iquantity,0) from Dl_opOrder aa
inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId where aa.strBillNo=@strBillNo and bb.cinvcode=@cinvcode 
 	/*END,11-01,添加获取库存可用量查询*/

--2016-01-27,变更,增加单位组的判断
if exists (select 1 from Inventory where cInvCode=@cinvcode and iGroupType=0)	--iGroupType:0,无换算率,1固定换算率,2浮动换算率
begin
select it.cInvName,isnull(it.cInvStd,' ') 'cInvStd',cn.cComUnitName,it.cInvCode ,isnull(cn.iChangRate,1) 'iChangRate',isnull(cn1.cComUnitName,' ') 'cSAComUnit'
,@p17 'nOriginalPrice',isnull(@p19,0) 'BeforeExercisePrice', @p20 'Rate',isnull(@fAvailQtty,0) 'fAvailQtty',@bOnsale 'bOnsale',it.cComUnitCode,it.iTaxRate,isnull(cn1.cComUnitName,' ') 'cn1cComUnitName',
@p17*@p20/100 'ExercisePrice',@nOriginalPrice 'Quote'
from Inventory it
--inner join ComputationGroup cg on it.cGroupCode=cg.cGroupCode
inner join computationunit cn on it.cGroupCode=cn.cGroupCode
left join computationunit cn1 on cn1.cComunitCode=it.cSAComUnitCode
where it.cInvCode=@cinvcode   and cn.cComunitCode=it.cComUnitCode --添加条件,过滤,只取主计量单位
order by cn.iChangRate
end 
else	--iGroupType:0,无换算率,1固定换算率,2浮动换算率
begin
select it.cInvName,isnull(it.cInvStd,' ') 'cInvStd',cn.cComUnitName,it.cInvCode ,isnull(cn.iChangRate,1) 'iChangRate',isnull(cn1.cComUnitName,' ') 'cSAComUnit'
,@p17 'nOriginalPrice',isnull(@p19,0) 'BeforeExercisePrice', @p20 'Rate',isnull(@fAvailQtty,0) 'fAvailQtty',@bOnsale 'bOnsale',it.cComUnitCode,it.iTaxRate,isnull(cn1.cComUnitName,' ') 'cn1cComUnitName',
@p17*@p20/100 'ExercisePrice',@nOriginalPrice 'Quote'
from Inventory it
--inner join ComputationGroup cg on it.cGroupCode=cg.cGroupCode
inner join computationunit cn on it.cGroupCode=cn.cGroupCode
left join computationunit cn1 on cn1.cComunitCode=it.cSAComUnitCode
where it.cInvCode=@cinvcode
order by cn.iChangRate
end


--exec [DLproc_QuasiOrderDetailModifyBySel] '01010100101','010101','DLOP15120856'
--exec [DLproc_QuasiOrderDetailModifyBySel] '32020103','010101'
--exec [DLproc_QuasiOrderDetailModifyBySel] '020101001','010101'
--exec [DLproc_QuasiOrderDetailModifyBySel] '010201001','010101'
--exec [DLproc_QuasiOrderDetailModifyBySel] '01030601802','010801'

END


GO
/****** Object:  StoredProcedure [dbo].[DLproc_QuasiYOrderDetailBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec DLproc_QuasiYOrderDetailBySel '01010100101','010101'
-- =============================================
-- Author:echo
-- Create date:2015-12-11
-- Description:	用于查询左边菜单传过来的物料详情
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_QuasiYOrderDetailBySel]

@cinvcode varchar(50),
@cpreordercode varchar(50) --预订单号

AS

BEGIN
SET NOCOUNT ON;
	
	/*Start,11-01,添加获取库存可用量查询*/
	declare @fAvailQtty decimal(18,3)
	--U8可用量查询(老的查询方式,可用量不会扣减保存的销售订单数量,20151226采用新的可用量计算方式,如下)
--select @fAvailQtty=(sum(fAvailQtty)*0.7)  from (
--SELECT  CS.cExpirationdate as 有效期至,E1.enumname as 有效期推算方式,W.cWhCode, W.cWhName, I.cInvCode, I.cInvAddCode,  I.cInvName, I.cInvStd, I.cInvCCode , IC.cInvCName, 
-- CU_M.cComUnitName AS cInvM_Unit, CASE WHEN I.iGroupType = 0 THEN NULL  WHEN I.iGrouptype = 2 THEN CU_A.cComUnitName  WHEN I.iGrouptype = 1 THEN CU_G.cComUnitName END  AS cInvA_Unit,convert(nvarchar(38),
--  convert(decimal(38,2),CASE WHEN I.iGroupType = 0 THEN NULL      WHEN I.iGroupType = 2 THEN (CASE WHEN CS.iQuantity = 0.0 OR CS.iNum = 0.0 THEN NULL ELSE CS.iQuantity/CS.iNum END)      
-- WHEN I.iGroupType = 1 THEN CU_G.iChangRate END)) AS iExchRate,
--  Null as cInvDefine1, Null as cInvDefine2, Null as cInvDefine3, Null as cFree1, Null as cFree2, Null as cFree3, Null as cFree4, Null as cFree5, Null as cFree6, Null as cFree7, Null as cFree8, Null as cFree9, 
-- Null as cFree10, Null as cInvDefine4, Null as cInvDefine5, Null as cInvDefine6, Null as cInvDefine7, Null as cInvDefine8, Null as cInvDefine9, Null as cInvDefine10, Null as cInvDefine11, Null as cInvDefine12, 
--  Null as cInvDefine13, Null as cInvDefine14, Null as cInvDefine15, Null as cInvDefine16,cs.cBatch, cs.EnumName As iSoTypeName, cs.csocode as SOCode, cs.cdemandmemo,convert(nvarchar,cs.isoseq) as iRowNo,
--cs.cvmivencode,v1.cvenabbname as cvmivenname , isnull(E.enumname,N'') as cMassUnitName,CS.dVDate, CS.dMdate,convert(varchar(20),CS.iMassDate) as iMassDate,
-- (iQuantity) AS iQtty,( CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(iNum,0) WHEN iGroupType = 1 THEN iQuantity/ CU_G.iChangRate END) AS iNum,
--  CASE WHEN CS.bStopFlag = 1 OR CS.bGspStop = 1 THEN iQuantity ELSE IsNull(fStopQuantity,0) END AS iStopQtty,
--  CASE WHEN CS.bStopFlag = 1 OR CS.bGspStop = 1 THEN (CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(iNum,0) WHEN iGroupType = 1 THEN iQuantity/ CU_G.iChangRate END) 
-- ELSE (CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(fStopNum,0) WHEN iGroupType = 1 THEN fStopQuantity/ CU_G.iChangRate END) END AS iStopNum,
-- (fInQuantity) AS fInQtty, 
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fInNum,0) WHEN iGroupType = 1 THEN fInQuantity/ CU_G.iChangRate END) AS fInNum,
-- (fTransInQuantity) AS fTransInQtty,
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fTransInNum,0) WHEN iGroupType = 1 THEN fTransInQuantity/ CU_G.iChangRate END) AS fTransInNum,
-- (ISNULL(fInQuantity,0) + ISNULL(fTransInQuantity,0)) AS fInQttySum,
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fInNum,0) + ISNULL(fTransInNum,0) WHEN iGroupType = 1 THEN (ISNULL(fInQuantity,0) + ISNULL(fTransInQuantity,0))/ CU_G.iChangRate END) AS fInNumSum,
-- (fOutQuantity) AS fOutQtty, 
--  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fOutNum,0) WHEN iGroupType = 1 THEN fOutQuantity/ CU_G.iChangRate END) AS fOutNum,
-- CS.cBatchProperty1,CS.cBatchProperty2,CS.cBatchProperty3,CS.cBatchProperty4,CS.cBatchProperty5,CS.cBatchProperty6,CS.cBatchProperty7,CS.cBatchProperty8,CS.cBatchProperty9,CS.cBatchProperty10,
--  (fTransOutQuantity) AS fTransOutQtty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fTransOutNum,0) WHEN iGroupType = 1 THEN fTransOutQuantity/ CU_G.iChangRate END) AS fTransOutNum,
--  (ISNULL(fOutQuantity,0) + ISNULL(fTransOutQuantity,0)) AS fOutQttySum , 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fOutNum,0) + ISNULL(fTransOutNum,0) WHEN iGroupType = 1 THEN (ISNULL(fOutQuantity,0) + ISNULL(fTransOutQuantity,0))/ CU_G.iChangRate END) AS fOutNumSum,
--  (fDisableQuantity) AS fDisableQtty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fDisableNum,0) WHEN iGroupType = 1 THEN fDisableQuantity/ CU_G.iChangRate END) AS fDisableNum,
--  (ipeqty) AS fpeqty, 
-- (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(ipenum,0) WHEN iGroupType = 1 THEN ipeqty/ CU_G.iChangRate END) AS fpenum,
-- (CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END) AS fAvailQtty,dLastCheckDate, 
-- (CASE WHEN iGroupType = 0 THEN 0  WHEN iGroupType = 2 THEN  CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iNum,0)- IsNull(fStopNum,0) END  - ISNULL(fOutNum,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iNum,0)- IsNull(fStopNum,0) END  - ISNULL(fOutNum,0) END WHEN iGroupType = 1 THEN  (CASE WHEN bInvBatch=1 THEN  
-- CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END)/CU_G.iChangRate ELSE NULL END) AS fAvailNum
-- FROM v_ST_currentstockForReport  CS inner join dbo.Inventory I ON I.cInvCode = CS.cInvCode   
-- left join dbo.InventoryClass IC ON IC.cInvCCode = I.cInvCCode LEFT OUTER JOIN dbo.ComputationUnit CU_G ON I.cSTComUnitCode =CU_G.cComUnitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_A ON I.cAssComUnitCode = CU_A.cComunitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_M ON I.cComUnitCode = CU_M.cComunitCode 
-- LEFT OUTER JOIN dbo.Warehouse W ON CS.cWhCode = W.cWhCode 
-- left join vendor v1 on v1.cvencode = cs.cvmivencode 
-- left join v_aa_enum E1 on E1.enumcode = ISNULL(cs.iExpiratDateCalcu,0) and E1.enumtype=N'SCM.ExpiratDateCalcu' 
-- LEFT OUTER JOIN dbo.v_aa_enum E with (nolock) on E.enumcode=convert(nchar,CS.cMassUnit) and E.enumType=N'ST.MassUnit' 
-- WHERE  1=1  AND 1=1   And I.cInvCode = @cinvcode
-- ) as HH
/**/
--/*20151226,可用量查询,生成U8订单后,扣减该数量*/
--  ----组合查询可用量
--declare @qty1 decimal(20,6),@qty decimal(20,6),@ddate varchar(20)
--select @ddate=convert(varchar(10),getdate(),120)
-- Select   @qty=SUM(ISNULL(iQuantity,0)) from SA_CurrentStock 
-- inner join warehouse on warehouse.cwhcode=SA_CurrentStock.cwhcode  
-- where cInvCode=@cinvcode and binavailcalcu=1 and  (isotype=0 or (isotype=1 and isodid=N'')) AND isnull(bStopFlag,0)=0
-- SELECT @qty1=Sum(Case When (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) < 0 Then 0 Else (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) End)  
--FROM SO_SOMain 
--INNER JOIN SO_SODetails ON SO_SOMain.ID = SO_SODetails.ID   INNER JOIN Inventory ON SO_SODetails.cInvCode = Inventory.cInvCode  left join ST_PELockedSum c with (nolock) on SO_SODetails.cInvCode = c.cinvcode and c.isotype =1 
--and c.isodid = convert(nvarchar(40),ISNULL(SO_SODetails.isosid,N''))  WHERE  IsNull(SO_SODetails.csCloser ,N'') =N'' AND   IsNull(SO_SOMain.cBustype,N'') <> N'直运销售' AND   (Case When IsNull(SO_SODetails.dPreDate,N'') = N'' 
--Then SO_SOMain.dDate Else SO_SODetails.dPreDate End ) <=@ddate And   SO_SODetails.cInvCode = @cinvcode  AND Isnull(SO_SODetails.cparentcode,'')= '' 
--select @fAvailQtty=(isnull(@qty,0)-isnull(@qty1,0))*0.7
/*V2.0:2015-01-06新算法---BEGIN*/
declare @ddate varchar(20)
select @ddate=convert(varchar(10),getdate(),120)
 Select   cInvCode 'cinvcode',SUM(ISNULL(iQuantity,0))  'qty' into #temp11 from SA_CurrentStock 
 inner join warehouse on warehouse.cwhcode=SA_CurrentStock.cwhcode  
 where cInvCode =@cinvcode and binavailcalcu=1 and  (isotype=0 or (isotype=1 and isodid=N'')) AND isnull(bStopFlag,0)=0
 group by cInvCode

 SELECT  SO_SODetails.cInvCode 'cinvcode',Sum(Case When (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) < 0 Then 0 Else (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) End)  'qty1'  into #temp22
FROM SO_SOMain 
INNER JOIN SO_SODetails ON SO_SOMain.ID = SO_SODetails.ID   INNER JOIN Inventory ON SO_SODetails.cInvCode = Inventory.cInvCode  left join ST_PELockedSum c with (nolock) on SO_SODetails.cInvCode = c.cinvcode and c.isotype =1 
and c.isodid = convert(nvarchar(40),ISNULL(SO_SODetails.isosid,N''))  WHERE  IsNull(SO_SODetails.csCloser ,N'') =N'' AND   IsNull(SO_SOMain.cBustype,N'') <> N'直运销售' AND   (Case When IsNull(SO_SODetails.dPreDate,N'') = N'' 
Then SO_SOMain.dDate Else SO_SODetails.dPreDate End ) <=@ddate And   SO_SODetails.cInvCode =@cinvcode  AND Isnull(SO_SODetails.cparentcode,'')= '' 
group by SO_SODetails.cInvCode
/*V2.0:2015-01-06新算法---END*/

--	--网上下单系统已存在的未生成U8订单和驳回修改的订单数量,待顾客确认的订单,这部分数量需要用U8数量减去,,2016-01-21注视
--	select @fAvailQtty=@fAvailQtty-isnull(sum(iquantity),0)  from Dl_opOrderDetail dod
--inner join Dl_opOrder do on do.lngopOrderId=dod.lngopOrderId
--where dod.cinvcode=@cinvcode and do.bytStatus in (1,2,3)
--if @fAvailQtty<0 or @fAvailQtty is null
--begin
--set @fAvailQtty=0
--end

--网上下单系统已存在的未生成U8订单和驳回修改的订单数量,待顾客确认的订单,这部分数量需要用U8数量减去,(不排除当前订单数量).2016-01-21add
	select dod.cinvcode,isnull(sum(isnull(iquantity,0)),0) 'opqty' into #temp33  from Dl_opOrderDetail dod
inner join Dl_opOrder do on do.lngopOrderId=dod.lngopOrderId
where dod.cinvcode=@cinvcode and do.bytStatus in (1,2,3) 
--and do.strBillNo!=@strBillNo
group by  dod.cinvcode

select @fAvailQtty=(sum(isnull(aa.qty,0))-sum(isnull(bb.qty1,0)))*0.7-sum(isnull(cc.opqty,0)) from #temp11 aa
left join #temp22 bb on aa.cinvcode=bb.cinvcode
left join #temp33 cc on aa.cinvcode=cc.cinvcode
group by aa.cinvcode

 	/*END,11-01,添加获取库存可用量查询*/

select aa.*,bb.*,cc.cInvStd,cc.cComUnitCode,@fAvailQtty 'fAvailQtty',bb.cinvcode+aa.ccode 'itemid',ee.autoid 'iaoids' from Dl_opPreOrder aa 
inner join Dl_opPreOrderDetail bb on aa.lngPreOrderId=bb.lngPreOrderId
left join Inventory cc on bb.cinvcode=cc.cInvCode
left join SA_PreOrderMain dd on dd.cCode=aa.ccode
left join SA_PreOrderDetails ee on dd.ID=ee.ID and ee.cInvCode=@cinvcode
--inner join computationunit cn on cc.cGroupCode=cn.cGroupCode
where bb.cinvcode=@cinvcode and aa.ccode=@cpreordercode

--exec DLproc_QuasiYOrderDetailBySel '01010100101','Y151200002'
--exec DLproc_QuasiYOrderDetailBySel '01010100101','Y20151200021'
--exec DLproc_QuasiYOrderDetailBySel '020101001','010101'
--exec DLproc_QuasiYOrderDetailBySel '010201001','010101'

END

--            dt.Columns.Add("cInvCode"); //编码    0
--            dt.Columns.Add("cInvName"); //名称    1
--            dt.Columns.Add("cInvStd");  //规格    2    
--            dt.Columns.Add("cComUnitName"); //基本单位  3
--            dt.Columns.Add("cInvDefine1"); //大包装单位  4
--            dt.Columns.Add("cInvDefine2"); //小包装单位  5
--            dt.Columns.Add("cInvDefine13");  //大包装换算率   6  
--            dt.Columns.Add("cInvDefine14"); //小包装换算率    7
--            dt.Columns.Add("UnitGroup"); //单位换算率组   8     
--            dt.Columns.Add("cComUnitQTY"); //基本单位数量 9
--            dt.Columns.Add("cInvDefine1QTY"); //大包装单位数量 10
--            dt.Columns.Add("cInvDefine2QTY"); //小包装单位数量 11
--            dt.Columns.Add("cInvDefineQTY"); //包装量数量汇总,包装量  12
--            dt.Columns.Add("cComUnitPrice"); //基本单位单价(报价)   13
--            dt.Columns.Add("cComUnitAmount"); //基本单位金额  14
--            dt.Columns.Add("pack"); //包装量换算结果  15
--            dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16
--            dt.Columns.Add("Stock"); //可用库存量   17
--            dt.Columns.Add("kl"); //扣率   18
--            dt.Columns.Add("cComUnitCode"); //基本单位编码   19
--            dt.Columns.Add("iTaxRate"); //销项税率   20
--            dt.Columns.Add("cn1cComUnitName"); //销售单位名称   21

--cinvcode
-- cinvname
-- cInvStd
-- cComUnitName
-- cInvDefine1
-- cInvDefine2
-- cInvDefine13
-- cInvDefine14
-- UnitGroup
-- cComUnitQTY
-- cInvDefine1QTY
-- cInvDefine2QTY
-- cInvDefineQTY
--iquotedprice
--isum
--cDefine22
--itaxunitprice
--@fAvailQtty
--kl
--cComUnitCode
--iTaxRate
--cn1cComUnitName



GO
/****** Object:  StoredProcedure [dbo].[DLproc_QuasiYOrderDetailModifyBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec DLproc_QuasiYOrderDetailBySel '01010100101','010101'
-- =============================================
-- Author:echo
-- Create date:2016-01-15
-- Description:	用于查询左边菜单传过来的物料详情(用于酬宾订单和特殊订单的修改)
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_QuasiYOrderDetailModifyBySel]

@cinvcode varchar(50),
@cpreordercode varchar(50) --预订单号

AS

BEGIN
SET NOCOUNT ON;
	
	/*Start,11-01,添加获取库存可用量查询*/
	declare @fAvailQtty decimal(18,3)
	--U8可用量查询(老的查询方式,可用量不会扣减保存的销售订单数量,20151226采用新的可用量计算方式,如下)
/*20151226,可用量查询,生成U8订单后,扣减该数量*/
  ----组合查询可用量
declare @qty1 decimal(20,6),@qty decimal(20,6),@ddate varchar(20)
select @ddate=convert(varchar(10),getdate(),120)
 Select   @qty=SUM(ISNULL(iQuantity,0)) from SA_CurrentStock 
 inner join warehouse on warehouse.cwhcode=SA_CurrentStock.cwhcode  
 where cInvCode=@cinvcode and binavailcalcu=1 and  (isotype=0 or (isotype=1 and isodid=N'')) AND isnull(bStopFlag,0)=0
 SELECT @qty1=Sum(Case When (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) < 0 Then 0 Else (SO_SODetails.iQuantity - IsNull(iFHQuantity ,0) - isnull(c.iquantity,0)) End)  
FROM SO_SOMain 
INNER JOIN SO_SODetails ON SO_SOMain.ID = SO_SODetails.ID   INNER JOIN Inventory ON SO_SODetails.cInvCode = Inventory.cInvCode  left join ST_PELockedSum c with (nolock) on SO_SODetails.cInvCode = c.cinvcode and c.isotype =1 
and c.isodid = convert(nvarchar(40),ISNULL(SO_SODetails.isosid,N''))  WHERE  IsNull(SO_SODetails.csCloser ,N'') =N'' AND   IsNull(SO_SOMain.cBustype,N'') <> N'直运销售' AND   (Case When IsNull(SO_SODetails.dPreDate,N'') = N'' 
Then SO_SOMain.dDate Else SO_SODetails.dPreDate End ) <=@ddate And   SO_SODetails.cInvCode = @cinvcode  AND Isnull(SO_SODetails.cparentcode,'')= '' 
select @fAvailQtty=(isnull(@qty,0)-isnull(@qty1,0))*0.7
	--网上下单系统已存在的未生成U8订单和驳回修改的订单数量,待顾客确认的订单,这部分数量需要用U8数量减去
	--2016-01-16,重要更新,酬宾订单和特殊订单为的数量为私有数量,需要根据顾客编码取出,并扣减,并且除去订单状态为3的订单(被驳回,待修改的订单数量)
	select @fAvailQtty=@fAvailQtty-isnull(sum(iquantity),0)  from Dl_opOrderDetail dod
inner join Dl_opOrder do on do.lngopOrderId=dod.lngopOrderId
where dod.cinvcode=@cinvcode and do.bytStatus in (1,2)
if @fAvailQtty<0 or @fAvailQtty is null
begin
set @fAvailQtty=0
end
 	/*END,11-01,添加获取库存可用量查询*/
select aa.*,bb.*,cc.cInvStd,cc.cComUnitCode,@fAvailQtty 'fAvailQtty',bb.cinvcode+aa.ccode 'itemid',ee.autoid 'iaoids' from Dl_opPreOrder aa 
inner join Dl_opPreOrderDetail bb on aa.lngPreOrderId=bb.lngPreOrderId
left join Inventory cc on bb.cinvcode=cc.cInvCode
left join SA_PreOrderMain dd on dd.cCode=aa.ccode
left join SA_PreOrderDetails ee on dd.ID=ee.ID and ee.cInvCode=@cinvcode
--inner join computationunit cn on cc.cGroupCode=cn.cGroupCode
where bb.cinvcode=@cinvcode and aa.ccode=@cpreordercode

--exec [DLproc_QuasiYOrderDetailModifyBySel] '01010100101','Y151200002'
--exec [DLproc_QuasiYOrderDetailModifyBySel] '01010100101','Y20151200021'
--exec [DLproc_QuasiYOrderDetailModifyBySel] '020101001','010101'
--exec [DLproc_QuasiYOrderDetailModifyBySel] '010201001','010101'

END

--            dt.Columns.Add("cInvCode"); //编码    0
--            dt.Columns.Add("cInvName"); //名称    1
--            dt.Columns.Add("cInvStd");  //规格    2    
--            dt.Columns.Add("cComUnitName"); //基本单位  3
--            dt.Columns.Add("cInvDefine1"); //大包装单位  4
--            dt.Columns.Add("cInvDefine2"); //小包装单位  5
--            dt.Columns.Add("cInvDefine13");  //大包装换算率   6  
--            dt.Columns.Add("cInvDefine14"); //小包装换算率    7
--            dt.Columns.Add("UnitGroup"); //单位换算率组   8     
--            dt.Columns.Add("cComUnitQTY"); //基本单位数量 9
--            dt.Columns.Add("cInvDefine1QTY"); //大包装单位数量 10
--            dt.Columns.Add("cInvDefine2QTY"); //小包装单位数量 11
--            dt.Columns.Add("cInvDefineQTY"); //包装量数量汇总,包装量  12
--            dt.Columns.Add("cComUnitPrice"); //基本单位单价(报价)   13
--            dt.Columns.Add("cComUnitAmount"); //基本单位金额  14
--            dt.Columns.Add("pack"); //包装量换算结果  15
--            dt.Columns.Add("ExercisePrice"); //基本单位单价(执行价格)   16
--            dt.Columns.Add("Stock"); //可用库存量   17
--            dt.Columns.Add("kl"); //扣率   18
--            dt.Columns.Add("cComUnitCode"); //基本单位编码   19
--            dt.Columns.Add("iTaxRate"); //销项税率   20
--            dt.Columns.Add("cn1cComUnitName"); //销售单位名称   21

--cinvcode
-- cinvname
-- cInvStd
-- cComUnitName
-- cInvDefine1
-- cInvDefine2
-- cInvDefine13
-- cInvDefine14
-- UnitGroup
-- cComUnitQTY
-- cInvDefine1QTY
-- cInvDefine2QTY
-- cInvDefineQTY
--iquotedprice
--isum
--cDefine22
--itaxunitprice
--@fAvailQtty
--kl
--cComUnitCode
--iTaxRate
--cn1cComUnitName



GO
/****** Object:  StoredProcedure [dbo].[DLproc_SampleOrderByUpd]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec DLproc_SampleOrderByUpd

-- =============================================
-- Author:echo
-- Create date:2016-01-11
-- Description:	修改(更新)订单表头插入数据(样品资料)[DLproc_SampleOrderByUpd]
-- 参数:
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_SampleOrderByUpd]
@strBillNo varchar (50), --DL订单编号
@strRemarks varchar(500),--备注
@strLoadingWays varchar(50),--装车方式
@bytStatus varchar(10)	--订单状态


AS

BEGIN

	SET NOCOUNT ON;
declare @maxbillno int
--更新数据
update Dl_opOrder set strRemarks=@strRemarks,strLoadingWays=@strLoadingWays,bytStatus=@bytStatus where strBillNo=@strBillNo
--返回订单表头id
--select top(1) lngopOrderId from Dl_opOrder where strBillNo=@strBillNo
select max(lngopOrderId) 'lngopOrderId' from Dl_opOrder where strBillNo=@strBillNo

END





GO
/****** Object:  StoredProcedure [dbo].[DLproc_SOADetailforCustomerBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [DLproc_NewSampleOrderByIns]

-- =============================================
-- Author:echo
-- Create date:2015-12-25
-- Description:	顾客输入起止日期和选择顾客,进行查询账单明细
-- 参数:
-- =============================================
--exec DLproc_SOADetailforCustomerBySel '2015-12-01','2015-12-15','01010101'
CREATE PROCEDURE [dbo].[DLproc_SOADetailforCustomerBySel]
@datebegin varchar(30),--开始日期
@dateend varchar(30),--截至日期
@ccuscode varchar(20)--顾客编号
AS

BEGIN

	SET NOCOUNT ON;
/*================================================================================================================================*/
----
declare @LocaleID varchar(32) ,@maxidlsid INT,@minidlsid INT,@maxddate datetime,@minddate datetime
----
Create Table #TempNo1(iYear nvarchar(10),iMonth nvarchar(10),iDay nvarchar(10),cDwCode nvarchar(20),cDwName nvarchar(120),cDwAbbName nvarchar(120),cDeptCode nvarchar(20),cDepName nvarchar(255),
cPerson nvarchar(20), cPersonName nvarchar(120), cInvCode nvarchar(60), cInvName nvarchar(255),cInvStd nvarchar(255),cDwCCode nvarchar(20),cDWCName nvarchar(120),cDwDCode nvarchar(20),cDCName nvarchar(120),cHDwCode nvarchar(20),
cHDwName nvarchar(120),cHDptCode nvarchar(20),cHDepName nvarchar(120),cHPsnCode nvarchar(20),cHPersonName nvarchar(120),cInvCCode nvarchar(20),cInvCName nvarchar(120),cCode nvarchar(60),cCode_Name nvarchar(120),cItem_Class nvarchar(2),
cItem_Name nvarchar(60),cItemCode nvarchar(60),cItemName nvarchar(255),cPzNum nvarchar(15),cContractType nvarchar(12),cContractTypeName nvarchar(40),cContractID nvarchar(64),cContractName nvarchar(400),cCusCreditCompany nvarchar(20),
cCusCreditName nvarchar(120),cOrderNo nvarchar(30),cDLCode nvarchar(30),cSaleOut nvarchar(30),dgst nvarchar(255),vtype nvarchar(50),vid nvarchar(30),cCancelNo nvarchar(40),cpzid nvarchar(30),exchname nvarchar(8),price float,Rate Float,jf_f money,jf_s float,
jf money,df_f money,df_s float,df money,jf_f2 money,jf_s2 float,jf2 money,df_f2 money,df_s2 float,df2 money,ye_f money,ye_s float,ye money,csysid nvarchar(2),Auto_Id int,dRDate datetime,dExpireDate datetime,dVouchDate datetime,cDwInvCode nvarchar(60),
cDwInvName nvarchar(255),iCreLine float,dDispatchDate datetime,[cDefine1] nvarchar(20))
----
--if not exists(select * from tempdb..sysindexes where name='IXByGroup' and id=object_id('#TempNo1')) 
Create Index IXByGroup on #TempNo1(cdwcode,[dRDate] ,[Auto_Id])
----
--declare @p1 int
--set @p1=180150015
--declare @p3 int
--set @p3=2
--declare @p4 int
--set @p4=1
--declare @p5 int
--set @p5=-1
--exec sp_cursoropen @p1 output,N'select * from userdef where citemname<>'''' and cclass in (N''单据头'',N''单据体'') and cDicDbName like ''cdefine%'' ',@p3 output,@p4 output,@p5 output
--select @p1, @p3, @p4, @p5
----
Create Table #TempNo2 (iYear nvarchar(10),iMonth nvarchar(10),iDay nvarchar(10),cDwCode nvarchar(20),cDwName nvarchar(120),cDwAbbName nvarchar(120),cDeptCode nvarchar(20),cDepName nvarchar(255),cPerson nvarchar(20), 
cPersonName nvarchar(120), cInvCode nvarchar(60), cInvName nvarchar(255),cInvStd nvarchar(255),cDwCCode nvarchar(20),cDWCName nvarchar(120),cDwDCode nvarchar(20),cDCName nvarchar(120),cHDwCode nvarchar(20),cHDwName nvarchar(120),
cHDptCode nvarchar(20),cHDepName nvarchar(120),cHPsnCode nvarchar(20),cHPersonName nvarchar(120),cInvCCode nvarchar(20),cInvCName nvarchar(120),cCode nvarchar(60),cCode_Name nvarchar(120),cItem_Class nvarchar(2),cItem_Name nvarchar(60),
cItemCode nvarchar(60),cItemName nvarchar(255),cPzNum nvarchar(15),cContractType nvarchar(12),cContractTypeName nvarchar(40),cContractID nvarchar(64),cContractName nvarchar(400),cCusCreditCompany nvarchar(20),cCusCreditName nvarchar(120),
cOrderNo nvarchar(30),cDLCode nvarchar(30),cSaleOut nvarchar(30),dgst nvarchar(255),vtype nvarchar(50),vid nvarchar(30),cCancelNo nvarchar(40),cpzid nvarchar(30),exchname nvarchar(8),price float,Rate Float,jf_f money,jf_s float,jf money,df_f money,
df_s float,df money,jf_f2 money,jf_s2 float,jf2 money,df_f2 money,df_s2 float,df2 money,ye_f money,ye_s float,ye money,csysid nvarchar(2),Auto_Id int,dRDate datetime,dExpireDate datetime,dVouchDate datetime,cDwInvCode nvarchar(60),cDwInvName nvarchar(255),
iCreLine float,dDispatchDate datetime,[cDefine1] nvarchar(20))
----
create table #TempNo3 (ctypecode  nvarchar(10),ctypename nvarchar(60),ctypeclass nvarchar(1),cflag nvarchar(2),csign nvarchar(8)) 
Create Index idx_ctypecode on #TempNo3(ctypecode) 
select @LocaleID=LocaleID from U8LangDefine with(nolock) where LangID=@@LANGID 
INSERT INTO #TempNo3(ctypecode,ctypename,ctypeclass,cflag,csign) 
/*ap_vouchtype_base:单据类型表(AP 应付款管理) 
ctypecode:单据类型 ,ctypename:单据类型名称,ctypeclass:类型分类,cflag:应收应付标志,csign:凭证类别字 
*/
select ctypecode,ctypename,ctypeclass,cflag,csign from ap_vouchtype_base Where LocaleID = @LocaleID
----
insert into #TempNo2(iMonth,iDay,cdwcode,cdwname,cdwabbname,dgst,jf_f,jf_s,jf,df_f,df_s,df,csysid) 
/*	Ar_DetailCust_s: 视图,Ar_Detail:应收明细账表(AR 应收款管理)与Customer以及Inventory的关联查询结果.Ar_Detail表无字段说明
PayCondition:付款条件档案(AS 公用目录设置) ;cPayCode:付款条件编码
(此语句应该为查询该顾客的期初数据)
cdwcode:单位编码;cexch_name:币种名称;idamount_f:借方原币金额;idamount_s:借方数量;idamount:借方金额;icamount_f:贷方原币金额;icamount_s:贷方数量;icamount:贷方金额
*/
select null as imonth,null as iDay,max(cdwcode),max(cdwname) as cdwname,max(cdwabbname) as cdwabbname,N'期初余额' as dgst,sum(case when a.cexch_name=N'人民币' then 0 else idamount_f end) as jf_f,sum(idamount_s) as jf_s,sum(idamount) as jf,
sum(case when a.cexch_name=N'人民币' then 0 else icamount_f end) as df_f,sum(icamount_s) as df_s,sum(icamount) as df,N'AR' 
From Ar_DetailCust_s  a with (nolock) 
LEFT JOIN PayCondition with (nolock) ON a.cPayCode = PayCondition.cPayCode 
left join dispatchlists on a.idlsid=dispatchlists.idlsid  where  (isnull(dcreditstart,dregdate)<@datebegin  and (cProcStyle not in (N'26',N'27',N'28',N'29',N'EX26',N'EX27') or (iperiod=0 and dispatchlists.idlsid is null)) and (cprocstyle<>N'XJ' or ccovouchtype in (N'60',N'SR45'))) 
And (a.cFlag=N'AR' or a.cBusType=N'代理进口') and a.iflag<=2  And (a.cDwCode >= @ccuscode ) and (a.cDwCode <= @ccuscode ) Group by cdwcode Having sum(idamount - icamount) <> 0 Or sum(idamount_f - icamount_f) <> 0
----
SELECT  @maxidlsid = MAX(idlsid), @minidlsid = MIN(idlsid) From Ar_DetailCust_s a with(nolock)  where  (isnull(dCreditStart,dregdate)>=@datebegin  and isnull(dCreditStart,dregdate)<=@dateend  and cProcStyle not in (N'26',N'27',N'28',N'29',N'EX26',N'EX27') 
and (cprocstyle<>N'XJ' or ccovouchtype in (N'60',N'SR45')))  And (a.cFlag=N'AR' or a.cBusType=N'代理进口') and (a.iflag<=2) And (a.cDwCode >= @ccuscode ) and (a.cDwCode <= @ccuscode )
select @maxddate = MAX(ddate), @minddate = MIN(Ddate) from rdrecords32 rdrecords with(nolock) inner join rdrecord32 rdrecord with(nolock) on rdrecords.id=rdrecord.id where idlsid>= @minidlsid AND idlsid<= @maxidlsid
create table #TempNo4 (idlsid int,cSource nvarchar(2),ccode nvarchar(30))
Create Index idlsid_idx on #TempNo4(idlsid)
insert into #TempNo4 
select idlsid,max(case when csource like N'出口%' then N'EX' else N'2' end) as cSource ,max(ccode) as ccode from rdrecords32 rdrecords with(nolock) inner join rdrecord32 rdrecord with(nolock) on rdrecords.id=rdrecord.id
where ddate >= @minddate and ddate <= @maxddate group by idlsid
----
--if  object_id('tempdb..#TempNo5') is not null drop table #TempNo5
----
 select isnull(dCreditStart,dregdate) as dRegDate,dvouchdate,cdwcode,cdwname,cdwabbname,cdeptcode,cperson,a.cinvcode,cdwccode,cdwdcode,chdwcode,chdptcode,chpsncode,a.ccode as ccode,a.citem_class as citem_class,a.citemcode as citemcode,
 a.citemname as citemname,ccontracttype,ccontractid,corderno,a.cdlcode as cdlcode,a.ccuscreditcompany as ccuscreditcompany,a.icreline,(cGlsign+N'-'+isnull(REPLICATE(N'0',4-len(iGLno_id)),N'')+convert(nvarchar,iGLno_id)) as cPzNum,
 (case when ccovouchtype=cprocstyle Or a.iFlag=1 Or cProcStyle=N'XJ' then cdigest else (case when cdigest is null then Ap_VouchType_2.ctypename else cdigest end) end) as dgst,(case when ccovouchtype=cprocstyle or a.iFlag=1 Or cProcStyle=N'XJ' 
 then Ap_VouchType.cTypeName else Ap_VouchType_2.cTypeName end) as vtype,(case when ccovouchtype=cprocstyle or a.iFlag=1 Or cProcStyle=N'XJ' then cCoVouchId else cCancelNo end) as vid,ccancelno, cpzid,(case when a.cexch_name=N'人民币' 
 then 0 else idamount_f end) as jf_f,(idamount_s) as jf_s,idamount as jf,(case when a.cexch_name=N'人民币' then 0 else icamount_f end) as df_f,(icamount_s) as df_s,(icamount) as df,(a.cexch_name) as exchname,(a.iprice) as price,(a.iexchrate) As Rate,
 a.cflag as csysid,Auto_Id,ibvid,isnull(dCreditStart,dregdate) as dRDate,dCreditStart ,(isnull(dgatheringdate,isnull(a.dtZbjEndDate,dVouchDate+isnull(PayCondition.iPayCreDays,0)))) as dExpireDate,[cDefine1],idlsid,cprocstyle,ccovouchtype,ccovouchid,cOperator 
 INTO #TempNo5 
 From Ar_DetailCust_s  a with (nolock) LEFT JOIN #TempNo3 Ap_VouchType with (nolock) ON a.cVouchType = Ap_VouchType.cTypeCode  LEFT JOIN #TempNo3 Ap_VouchType_2 with (nolock) ON a.cProcstyle = Ap_VouchType_2.cTypeCode 
 LEFT JOIN PayCondition with (nolock) ON a.cPayCode = PayCondition.cPayCode where  (isnull(dCreditStart,dregdate)>=@datebegin  and isnull(dCreditStart,dregdate)<=@dateend  and cProcStyle not in (N'26',N'27',N'28',N'29',N'EX26',N'EX27') 
 and (cprocstyle<>N'XJ' or ccovouchtype in (N'60',N'SR45'))) And (a.cFlag=N'AR' or a.cBusType=N'代理进口') and a.iflag<=2  And (a.cDwCode >= @ccuscode ) and (a.cDwCode <= @ccuscode )
----
insert into #TempNo1(iYear,iMonth,iDay,cdwcode,cdwname,cdwabbname,corderno,cdlcode,csaleout,cPzNum,dgst,vtype,vid,cCancelNo,cpzid,jf_f,jf_s,jf,df_f,df_s,df,exchname,price,rate,csysid,Auto_Id,dRdate,dExpireDate,[cDefine1]) 
select year(min(dregdate)) as iyear,month(min(dregdate)) as imonth,Day(min(dregdate)) as iDay,max(cdwcode),max(cdwname) as cdwname,max(cdwabbname) as cdwabbname,max(corderno),max(a.cdlcode),max(#TempNo4.ccode),Max(cPzNum) as cPzNum,
max(dgst) as dgst,max(vtype) as vtype,max(vid) as vid,max(cCancelNo) as ccancelno, max(cpzid) as cpzid,sum(jf_f) as jf_f,sum(jf_s) as jf_s,sum(jf) as jf,sum(df_f) as df_f,sum(df_s) as df_s,sum(df) as df,max(a.exchname) as exchname,max(a.price) as price,
max(a.rate) As Rate,max(a.csysid) as csysid,Max(Auto_Id),min(dRdate),max(dexpiredate),max(a.cDefine1) From #TempNo5 a left join #TempNo4 on a.idlsid=#TempNo4.idlsid  and ((a.ccovouchtype like 'EX%' AND #TempNo4.cSource = 'EX') OR (a.ccovouchtype like '2%' 
AND #TempNo4.cSource = '2') ) Group by cdwcode,ccancelno,a.exchname,dCreditStart having sum(jf)<>0 or sum(df) <> 0 Or sum(jf_f)<>0 or sum(df_f) <> 0
----
insert into #TempNo2(iMonth,iDay,cdwcode,cdwname,cdwabbname,dgst,jf_f,jf_s,jf,df_f,df_s,df,csysid) 
select null as imonth,null as iDay,max(cdwcode),max(cdwname) as cdwname,max(cdwabbname) as cdwabbname,N'期初余额' as dgst,sum(case when a.cexch_name=N'人民币' then 0 else iamount_f end) as jf_f,sum(iamount_s) as jf_s,sum(iamount) as jf,0 as df_f,
0 as df_s,0 as df,N'AR' From V_OutNotBalance_A a with (nolock) LEFT JOIN PayCondition with (nolock) ON a.cPayCode = PayCondition.cPayCode where  (isnull(dCreditStart,dvouchdate)<@datebegin ) And (a.cDwCode >= @ccuscode ) and (a.cDwCode <= @ccuscode ) 
and cCheckMan<>'' Group By cdwcode
----
--if exists(select * from sysobjects where name =N'#MyRdrecords32idlsid' and xtype=N'U')  drop table #MyRdrecords32idlsid
----

SELECT  @maxidlsid = MAX(idlsid), @minidlsid = MIN(idlsid)  From v_outnotbalance_a a with(nolock)  where  (isnull(dCreditStart,dvouchdate)>=@datebegin  and isnull(dCreditStart,dvouchdate)<=@dateend ) And (a.cDwCode >= @ccuscode ) and (a.cDwCode <= @ccuscode )
select @maxddate = MAX(ddate), @minddate = MIN(Ddate) from rdrecords32 rdrecords with(nolock) inner join rdrecord32 rdrecord with(nolock) on rdrecords.id=rdrecord.id where idlsid>= @minidlsid AND idlsid<= @maxidlsid
select idlsid,rdrecord.csource,rdrecord.ccode,ddate into #MyRdrecords32idlsid from rdrecords32 rdrecords with(nolock) inner join rdrecord32 rdrecord with(nolock) on rdrecords.id=rdrecord.id
where rdrecords.idlsid>= @minidlsid AND rdrecords.idlsid<= @maxidlsid  AND ddate >= @minddate
and ddate <= @maxddate
insert into #TempNo4 
select Rd32.idlsid,max(case when Rd32.csource like N'出口%' then N'EX' else N'2' end)
as cSource ,max(Rd32.ccode) as ccode
from #MyRdrecords32idlsid Rd32 
 left join #TempNo4 on Rd32.idlsid=#TempNo4.idlsid
 where #TempNo4.idlsid is null
 group by Rd32.idlsid
----
insert into #TempNo1(iYear,iMonth,iDay,cdwcode,cdwname,cdwabbname,corderno,cdlcode,csaleout,cPzNum,dgst,vtype,vid,cCancelNo,cpzid,jf_f,jf_s,jf,df_f,df_s,df,exchname,price,rate,csysid,Auto_Id,dRdate,dExpireDate,[cDefine1]) 
select min(case when a.bcredit=1 then year(dCreditStart) else year(dvouchdate) end) as iYear,min(case when a.bcredit=1 then month(dCreditStart) else month(dvouchdate) end) as iMonth,min(case when a.bcredit=1 then day(dCreditStart) else day(dvouchdate) end) as iDay,
max(cdwcode),max(cdwname) as cdwname,max(cdwabbname) as cdwabbname,max(corderno),max(a.cVouchID),max(#TempNo4.ccode),'' as cPzNum,max(isnull(a.cDigest,'')) as dgst,max(VouchType_enum.EnumName) as vtype,max(a.cVouchID) as vid,null as cCancelNo,
null as cpzid,sum(case when a.cexch_name=N'人民币' then 0 else iamount_f end) as jf_f,sum(iamount_s) as jf_s,sum(iamount) as jf,0 as df_f,0 as df_s,0 as df,max(a.cexch_name) as exchname,max(a.iPrice) as price,max(a.iexchrate) As Rate,N'AR',null as auto_id,
min(case when a.bcredit=1 then dCreditStart else dvouchdate end),max((case when a.bcredit=1 then dgatheringdate else dVouchDate+IsNull(PayCondition.iPayCreDays,0) end)),max(a.cDefine1) From V_OutNotBalance_A a with (nolock) 
LEFT JOIN VouchType with (nolock) on a.cVouchType=VouchType.cVouchType  Left Join V_AA_enum VouchType_enum with (nolock) on VouchType_enum.EnumType='SA.cVouchTypeName' and VouchType_enum.EnumCode=VouchType.cVouchName  
LEFT JOIN PayCondition with (nolock) ON a.cPayCode = PayCondition.cPayCode left join #TempNo4 on a.idlsid=#TempNo4.idlsid  and #TempNo4.cSource = N'2' where  (isnull(dCreditStart,dvouchdate)>=@datebegin  and isnull(dCreditStart,dvouchdate)<=@dateend )  
And (a.cDwCode >= @ccuscode ) and (a.cDwCode <= @ccuscode ) and cCheckMan<>'' Group By cdwcode,a.cVouchType,a.cVouchID
----
Insert into #TempNo1(cdwcode,cdwname,cdwabbname,dgst,jf_f,jf_s,jf,df_f,df_s,df,jf_f2,jf_s2,jf2,df_f2,df_s2,df2) 
Select cdwcode,cdwname,cdwabbname,max(dgst),sum(jf_f),sum(jf_s),sum(jf),sum(df_f),sum(df_s),sum(df),sum(jf_f2),sum(jf_s2),sum(jf2),sum(df_f2),sum(df_s2),sum(df2) From #TempNo2 Group by cdwcode,cdwname,cdwabbname
----
update #TempNo1 set jf_f=(case when jf_f is null then 0 else jf_f end),jf_s=(case when jf_s is null then 0 else jf_s end),jf=(case when jf is null then 0 else jf end),df_f=(case when df_f is null then 0 else df_f end),df_s=(case when df_s is null then 0 else df_s end),
df=(case when df is null then 0 else df end),jf_f2=(case when jf_f2 is null then 0 else jf_f2 end),jf_s2=(case when jf_s2 is null then 0 else jf_s2 end),jf2=(case when jf2 is null then 0 else jf2 end),df_f2=(case when df_f2 is null then 0 else df_f2 end),
df_s2=(case when df_s2 is null then 0 else df_s2 end),df2=(case when df2 is null then 0 else df2 end)
----
Update #TempNo1 set ye_f=jf_f-df_f-jf_f2+df_f2,ye_s=jf_s-df_s-jf_s2+df_s2,ye=jf-df-jf2+df2
----
delete #TempNo1 Where jf_f=0 and jf=0 and df_f=0 and df=0 and jf_f2=0 and jf2=0 and df_f2=0 and df2=0
----
Update #TempNo1 set jf_f=null,jf_s=null,jf=null,df_f=null,df_s=null,df=null,jf_f2=null,jf_s2=null,jf2=null,df_f2=null,df_s2=null,df2=null,csysid=Null where iMonth is null
----
Update #TempNo1 set dgst=replace(dgst,char(13)+char(10),'') 
----
--Select top 1 * from #TempNo1
----
--Drop table  #TempNo6
----
SELECT [iYear] as [iYear],[iMonth] as [iMonth],[iDay] as [iDay],[cPzNum] as [cPzNum],[cDwCode],[cDwAbbName] as [cDwAbbName],[cDwName] as [cDwName],[cDeptCode] as [cDeptCode],[cDepName] as [cDepName],[cPerson] as [cPerson],
[cPersonName] as [cPersonName],[cInvCode] as [cInvCode],[cInvName] as [cInvName],[cInvStd] as [cInvStd],[cDwCCode] as [cDwCCode],[cDWCName] as [cDWCName],[cDwDCode] as [cDwDCode],[cDCName] as [cDCName],[cHDwCode] as [cHDwCode],
[cHDwName] as [cHDwName],[cHDptCode] as [cHDptCode],[cHDepName] as [cHDepName],[cHPsnCode] as [cHPsnCode],[cHPersonName] as [cHPersonName],[cInvCCode] as [cInvCCode],[cInvCName] as [cInvCName],[cCode] as [cCode],
[cCode_Name] as [cCode_Name],[cItem_Class] as [cItem_Class],[cItem_Name] as [cItem_Name],[cItemCode] as [cItemCode],[cItemName] as [cItemName],[cContractType] as [cContractType],[cContractTypeName] as [cContractTypeName],
[cContractID] as [cContractID],[cContractName] as [cContractName],[cCusCreditCompany] as [cCusCreditCompany],[cCusCreditName] as [cCusCreditName],[dgst] as [dgst],[cOrderNo] as [cOrderNo],[cDLCode] as [cDLCode],[cSaleOut] as [cSaleOut],[vtype],
[vid],[cCancelNo],[cpzid] as [cpzid],[exchname],round([price],2) as [price],round([Rate],2) as [Rate],round([jf_s],2) as [jf_s],round([jf_f],2) as [jf_f],round([jf],2) as [jf],round([df_s],2) as [df_s],round([df_f],2) as [df_f],round([df],2) as [df],round([jf_s2],2) as [jf_s2],
round([jf_f2],2) as [jf_f2],round([jf2],2) as [jf2],round([df_s2],2) as [df_s2],round([df_f2],2) as [df_f2],round([df2],2) as [df2],round([ye_s],2) as [ye_s],round([ye_f],2) as [ye_f],round([ye],2) as [ye],[csysid],round([Auto_Id],2) as [Auto_Id],[dRDate],[dExpireDate] as [dExpireDate],
[dVouchDate] as [dVouchDate],[cDwInvCode] as [cDwInvCode],[cDwInvName] as [cDwInvName],round([iCreLine],2) as [iCreLine],[dDispatchDate] as [dDispatchDate],[cDefine1] as [cDefine1] 
Into #TempNo6 
from #TempNo1
----
--Drop table  #TempNo7
----
SELECT max([iYear]) as [iYear],max([iMonth]) as [iMonth],max([iDay]) as [iDay],max([cPzNum]) as [cPzNum],[cDwCode],max([cDwAbbName]) as [cDwAbbName],max([cDwName]) as [cDwName],max([cDeptCode]) as [cDeptCode],max([cDepName]) as [cDepName],
max([cPerson]) as [cPerson],max([cPersonName]) as [cPersonName],max([cInvCode]) as [cInvCode],max([cInvName]) as [cInvName],max([cInvStd]) as [cInvStd],max([cDwCCode]) as [cDwCCode],max([cDWCName]) as [cDWCName],max([cDwDCode]) as [cDwDCode],
max([cDCName]) as [cDCName],max([cHDwCode]) as [cHDwCode],max([cHDwName]) as [cHDwName],max([cHDptCode]) as [cHDptCode],max([cHDepName]) as [cHDepName],max([cHPsnCode]) as [cHPsnCode],max([cHPersonName]) as [cHPersonName],
max([cInvCCode]) as [cInvCCode],max([cInvCName]) as [cInvCName],max([cCode]) as [cCode],max([cCode_Name]) as [cCode_Name],max([cItem_Class]) as [cItem_Class],max([cItem_Name]) as [cItem_Name],max([cItemCode]) as [cItemCode],max([cItemName]) as [cItemName],
max([cContractType]) as [cContractType],max([cContractTypeName]) as [cContractTypeName],max([cContractID]) as [cContractID],max([cContractName]) as [cContractName],max([cCusCreditCompany]) as [cCusCreditCompany],max([cCusCreditName]) as [cCusCreditName],
max([dgst]) as [dgst],max([cOrderNo]) as [cOrderNo],max([cDLCode]) as [cDLCode],max([cSaleOut]) as [cSaleOut],[vtype],[vid],[cCancelNo],max([cpzid]) as [cpzid],[exchname],sum(round([price],2)) as [price],max([Rate]) as [Rate],sum(round([jf_s],2)) as [jf_s],
sum(round([jf_f],2)) as [jf_f],sum(round([jf],2)) as [jf],sum(round([df_s],2)) as [df_s],sum(round([df_f],2)) as [df_f],sum(round([df],2)) as [df],sum(round([jf_s2],2)) as [jf_s2],sum(round([jf_f2],2)) as [jf_f2],sum(round([jf2],2)) as [jf2],sum(round([df_s2],2)) as [df_s2],
sum(round([df_f2],2)) as [df_f2],sum(round([df2],2)) as [df2],sum(round([ye_s],2)) as [ye_s],sum(round([ye_f],2)) as [ye_f],sum(round([ye],2)) as [ye],[csysid],sum(round([Auto_Id],2)) as [Auto_Id],[dRDate],max([dExpireDate]) as [dExpireDate],max([dVouchDate]) as [dVouchDate],
max([cDwInvCode]) as [cDwInvCode],max([cDwInvName]) as [cDwInvName],sum(round([iCreLine],2)) as [iCreLine],max([dDispatchDate]) as [dDispatchDate],max([cDefine1]) as [cDefine1] 
Into #TempNo7 
FROM #TempNo6 GROUP BY [cDwCode], [vtype], [vid], [cCancelNo], [exchname], [csysid], [dRDate]
----
--select * from #TempNo7
update #TempNo7 set  cOrderNo='期初余额'  where dgst='期初余额'
SELECT t1.*
,t2.cMemo,t3.cDefine1 'skdBZ',t3.cSSName
FROM #TempNo7 t1
left join DispatchList t2 on t1.cDLCode=t2.cDLCode
left join (select aa.iID,aa.cVouchID,bb.chdefine3,cc.cSSName,aa.cDefine1 from Ap_CloseBill aa
left join Ap_CloseBill_extradefine bb on aa.iID=bb.iID
left join SettleStyle cc on aa.cSSCode=cc.cSSCode) t3 on t3.cVouchID=t1.vid
  ORDER BY [cDwCode] ,[dRDate] ,[Auto_Id]
----
----
----
----
END
--exec DLproc_SOADetailforCustomerBySel '2015-12-01','2099-01-01','010108'
--exec DLproc_SOADetailforCustomerBySel '2015-12-01','2099-01-01','01010204'
--exec DLproc_SOADetailforCustomerBySel '2015-12-01','2015-12-05','01010201'
--exec DLproc_SOADetailforCustomerBySel '2015-12-01','2015-12-31','01010101'

--cDwName
--成都二片区

--iYear	iMonth	iDay
--2015	12	1
--cOrderNo	cDLCode
--NULL	NULL
--cDLCode
--提货清单号
--dgst
--期初余额

--df
--NULL
--jf
--25510.58cDwName
--成都二片区

--vid	cCancelNo	cpzid
--0000000023	AR480000000023	AR0000000000035
--cn_id (U861)  票据号  nvarchar 30  True  
--csettle (U861)  结算方式编码  nvarchar 3  True 
--ccode (U861)  科目编码  nvarchar 15  False  
--ccus_id (U861)  客户编码  nvarchar 20  True  
--ccode_equal (U861)  对方科目编码  nvarchar 50  True  
--coutno_id (U861)  外部凭证业务号  nvarchar 50  True  
--coutid (U861)  外部凭证单据号  nvarchar 50  True  
--select cn_id,csettle ,md,mc,ino_id ,* from GL_accvouch where ino_id ='0057'
--收款单cDefine1=统计,去掉备注

GO
/****** Object:  StoredProcedure [dbo].[DLproc_SOADetailforSearchBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [DLproc_SOADetail] '01010100101','010101','','',0
-- =============================================
-- Author:echo
-- Create date:2015-12-21
-- Description:	用于查询账单明细
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_SOADetailforSearchBySel]
@ccuscode varchar(50),
@beginDate varchar(20), 
@endDate  varchar(20)

AS

BEGIN
SET NOCOUNT ON;
declare @sqlstrBillNo varchar(100),@sqlbeginDate varchar(100),@sqlendDate varchar(100)

--查询发货数量，顾客，单据号，金额
select aa.cCusCode,aa.cCusName,aa.dDate,bb.cSoCode,iQuantity,iSum into #temp1  from DispatchList aa
inner join DispatchLists bb on aa.DLID=bb.DLID where (aa.dDate between  @beginDate and @endDate) 
 and cVouchType='05' and aa.cCusCode=@ccuscode and bb.cSoCode is not null


 select * from #temp1
 union all
select '发货单合计','','','',sum(isnull(iQuantity,0)),sum(isnull(iSum,0)) from #temp1




END

--exec [DLproc_SOADetailforSearchBySel] '010106','2015-12-01','2015-12-05'

--DispatchList
--bReturnFlag (U861)  退货标志  bit 1  False
--cVouchType (U861)  单据类型编码  nvarchar 2  False  
--csvouchtype (U871)  来源单据类型  nvarchar 10  True   
--dcreatesystime (U871)  制单时间  datetime 8  True  
--iverifystate (U870)  审核状态  int 4  True  
--DLID (U861)  发货退货单主表标识  int 4  False  
--cDLCode (U861)  发货退货单号  nvarchar 30  True  
--dDate (U861)  单据日期  datetime 8  False 
--bReturnFlag (U861)  退货标志  bit 1  False  
--cSOCode (U861)  销售订单号  nvarchar 30  True  
--cCusCode (U861)  客户编码  nvarchar 20  True  
--cCusName (U861)  客户名称  nvarchar 120  True  


--select cordercode,cSoCode,* from DispatchLists
--cordercode (U870)  订单号  nvarchar 30  True 
--bmpforderclosed (U8110)  订单关闭标识  bit 1  True  
--cInvCode (U861)  存货编码  nvarchar 20  False 
--iQuantity (U861)  数量  decimal 17  True  
--iSum (U861)  原币价税合计  money 8  True  
--iSettleQuantity (U861)  开票数量  decimal 17  True  
--cMemo (U861)  备注  nvarchar 60  True  
--iSOsID (U861)  销售订单子表标识  int 4  True  
--iRetQuantity (U861)  退货数量  userdecimal 13  True 
--cSoCode (U861)  销售订单号  nvarchar 30  True 

----查询发货数量，顾客，单据号，金额
--select aa.cCusCode,aa.cCusName,aa.dDate,bb.cSoCode,iQuantity,iSum,iRetQuantity  from DispatchList aa
--inner join DispatchLists bb on aa.DLID=bb.DLID where aa.dDate between '2015-01-01' and '2015-12-05'
-- and cVouchType='05'

--cCusCode	cCusName	dDate	cSoCode	iQuantity	iSum
--010106	成都六片区	2015-12-05 00:00:00.000	W151200350	60.0000000000	69.48




  

GO
/****** Object:  StoredProcedure [dbo].[DLproc_sp_GetID_test]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[DLproc_sp_GetID_test]  (  

 @RemoteId nvarchar (2) =N'00',  

 @cAcc_Id nvarchar(3) ,  

 @cVouchType nvarchar(50) =N'' ,  

 @iAmount int =0 ,  

 @iFatherId int OUTPUT,  

 @iChildId int OUTPUT,

 @bEnableNewRule bit=1)

AS  

	IF @bEnableNewRule IS NULL SET @bEnableNewRule=1

	IF LTRIM(@RemoteId)=N'' OR @RemoteId IS NULL SET @RemoteId=N'00'

	IF @RemoteId=N'00' AND @bEnableNewRule=1

	BEGIN

		DECLARE @iReturn int	

		EXECUTE @iReturn =sp_GetIDWithoutRemote @cAcc_Id=@cAcc_Id,@cVouchType=@cVouchType,@iAmount=@iAmount,@iFatherId=@iFatherId OUTPUT,@iChildId=@iChildId OUTPUT

		RETURN @iReturn

	END

   set lock_timeout 30000  

   BEGIN TRAN T1   

   if exists(select * from UFSystem..UA_Identity with (updlock,rowlock) where cAcc_Id=@cAcc_Id and cVouchType=@cVouchType)  

      begin  

         select @iFatherId=iFatherId ,@iChildId=iChildId  

               from UFSystem..UA_Identity   

        where cAcc_Id=@cAcc_Id and cVouchType=@cVouchType  

         if @@error <>0   

            begin  

               goto ErrHandle   

            end  

         if (len(@iFatherId+1)>7)  

            set @iFatherId=1  

         else  

            set @iFatherId=@iFatherId+1  

  

         if (len(@iChildId+@iAmount)>7)  

            set @iChildId=1  

         else  

            set @iChildId=@iChildId+@iAmount  

  

         update UFSystem..UA_Identity set iFatherId=@iFatherId,iChildId=@iChildId  

                where cAcc_Id=@cAcc_Id and cVouchType=@cVouchType  

         if @@error <>0   

            begin  

               goto ErrHandle   

            end  

         set @iFatherId=convert(int,@RemoteId+right('0000000'+convert(nvarchar(7),@iFatherId),7))  

         set @iChildId=convert(int,@RemoteId+right('0000000'+convert(nvarchar(7),@iChildId),7))         

      end  

   else  

      begin  

         if @@error <>0   

            begin  

               goto ErrHandle   

            end      

         if (@iAmount>0)  

            begin  

               insert into UFSystem..UA_Identity (cAcc_Id,cVouchType,iFatherId,iChildId)   

    values (@cAcc_Id,@cVouchType,1,@iAmount)  

               if @@error <>0   

                  begin  

                     goto ErrHandle   

                  end                    

               set @iFatherId=convert(int,@RemoteId+right('0000000'+convert(nvarchar(7),1),7))  

               set @iChildId=convert(int,@RemoteId+right('0000000'+convert(nvarchar(7),@iAmount),7))  

            end  

         else  

            begin  

               insert into UFSystem..UA_Identity (cAcc_Id,cVouchType,iFatherId,iChildId)   

    values (@cAcc_Id,@cVouchType,1,1)  

               if @@error <>0   

                  begin  

                     goto ErrHandle   

                  end                    

               set @iFatherId=convert(int,@RemoteId+right('0000000'+convert(nvarchar(7),1),7))  

               set @iChildId=convert(int,@RemoteId+right('0000000'+convert(nvarchar(7),1),7))  

            end  

      end  

   if @@error <>0   

      begin  

         goto ErrHandle   

      end                    

   COMMIT TRAN T1    

   return 0  

   ErrHandle:  

      rollback tran T1   

      return -1  


GO
/****** Object:  StoredProcedure [dbo].[DLproc_sp_GetID_test_1]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  PROCEDURE [dbo].[DLproc_sp_GetID_test_1]


as
declare @p1 varchar(50),@p2 varchar(50)
set @p1=''
set @p2=''
exec DLproc_sp_GetID_test '00','002','Somain',1,@p1 output,@p2 output
select @p1,@p2


GO
/****** Object:  StoredProcedure [dbo].[DLproc_TreeListDetailsAllBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--drop proc [DLproc_TreeListDetailsAllBySel]
-- =============================================
-- Author:echo
-- Create date:2015-12-14
-- Description:	用于查询选中的物料大类下的明细数据
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_TreeListDetailsAllBySel]

@cInvCCode varchar(20),
@cCusCode varchar(20)
AS

BEGIN

	SET NOCOUNT ON;
	set @cInvCCode=@cInvCCode+'%'
declare @strsql varchar(5000)
--declare @cCusCode varchar(20),@cInvCCode varchar(20)
--set @cCusCode='010101'
--set @cInvCCode='01%'
--v2.00 2015-12-02
--v3.00 /*2015.12.28:增加dEDate is null,如果禁用不为空则是被禁用商品,*/
--set @strsql=
/*2015.01.08禁用,启用新的查询,不计算可用量Begin*/
--'select cInvName,cInvStd,cComUnitName,cInvCode,(fAvailQtty-iquantity)*0.7 '+''''+'fAvailQtty'+''''+' from (
--select aa.cInvName,aa.cInvStd,aa.cComUnitName,aa.cInvCode,isnull(bb.fAvailQtty,0) '+''''+'fAvailQtty'+''''+',isnull(cc.iquantity,0) '+''''+'iquantity'+''''+',dEDate from (
--select cInvName,isnull(cInvStd,'+''''+'  '+''''+') '+''''+'cInvStd'+''''+',cn.cComUnitName,cInvCode,dEDate 
--from 
--(select I.cInvName,I.cInvStd,I.cInvCode,I.cComUnitCode,I.cInvCCode,I.dEDate from Inventory I where  I.bSale=1 
--and (I.cInvCode LIKE '+''''+'01%'+''''+' or I.cInvCode LIKE '+''''+'02%'+''''+' or I.cInvCode LIKE '+''''+'32%'+''''+'  and I.dEDate is null) and I.cInvCCode not in (select left(cinvCode,9) from  SA_CusInvLimited where cCusCode='+''''+@cCusCode+''''+' group by left(cinvCode,9) ) ) ic 
-- left join computationunit cn on ic.cComUnitCode=cn.cComunitCode 
--WHERE (ic.cInvCode like '+''''+@cInvCCode+''''+') ) as aa
--left join (
--SELECT  I.cInvCode,I.cInvCCode , sum(
-- (CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END)) AS fAvailQtty
-- FROM v_ST_currentstockForReport  CS inner join dbo.Inventory I ON I.cInvCode = CS.cInvCode   
-- left join dbo.InventoryClass IC ON IC.cInvCCode = I.cInvCCode LEFT OUTER JOIN dbo.ComputationUnit CU_G ON I.cSTComUnitCode =CU_G.cComUnitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_A ON I.cAssComUnitCode = CU_A.cComunitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_M ON I.cComUnitCode = CU_M.cComunitCode 
-- LEFT OUTER JOIN dbo.Warehouse W ON CS.cWhCode = W.cWhCode 
-- left join vendor v1 on v1.cvencode = cs.cvmivencode 
-- left join v_aa_enum E1 on E1.enumcode = ISNULL(cs.iExpiratDateCalcu,0) and E1.enumtype=N'+''''+'SCM.ExpiratDateCalcu'+''''+' 
-- LEFT OUTER JOIN dbo.v_aa_enum E with (nolock) on E.enumcode=convert(nchar,CS.cMassUnit) and E.enumType=N'+''''+'ST.MassUnit'+''''+' 
-- where I.cInvCode like '+''''+@cInvCCode+''''+'
-- group by I.cInvCode,I.cInvCCode ) as bb on aa.cInvCode=bb.cInvCode
--	left join (
--select  dod.cinvcode,isnull(sum(iquantity),0) '+''''+'iquantity'+''''+' from Dl_opOrderDetail dod
--inner join Dl_opOrder do on do.lngopOrderId=dod.lngopOrderId
--inner join Inventory ii on ii.cInvCode=dod.cinvcode
--where ii.cInvCode like '+''''+@cInvCCode+''''+' and do.bytStatus in (1,2,3)
--group by dod.cinvcode ) as cc on cc.cinvcode=aa.cInvCode
--) as hh where hh.dEDate is null 
--order by hh.cInvCode,hh.cInvName,hh.cInvStd'

--new code,2015.01.08;
--2016-02-22禁用
set @strsql=
'select cInvName,isnull(cInvStd,'+''''+' '+''''+') cInvStd,cn.cComUnitName,cInvCode  from 
(select I.cInvName,I.cInvStd,I.cInvCode,I.cComUnitCode,I.cInvCCode,I.dEDate from Inventory I where  I.bSale=1 and isnull(dEDate,'+''''+'9999-12-31'+''''+')  > GETDATE()
and (I.cInvCode like '+''''+@cInvCCode+'%' +''''+') and I.cInvCCode not in (select left(cinvCode,9) from  SA_CusInvLimited where cCusCode='+''''+@cCusCode+''''+' group by left(cinvCode,9) ) ) ic 
 left join computationunit cn on ic.cComUnitCode=cn.cComunitCode order by cInvCode,cInvName,cInvStd'
/*2015.01.08禁用,启用新的查询,不计算可用量End*/


--2016-02-22启用新的查询方式
--declare @bLimit	 bit	
--select TOP 1  @bLimit=L.bLimited  from SA_CusInvLimited L where L.cCusCode=@cCusCode;
--set @strsql='declare @bLimit	 bit
--select TOP 1  @bLimit=L.bLimited  from SA_CusInvLimited L where L.cCusCode=@cCusCode
--if @bLimit=0 BEGIN select cInvName,isnull(cInvStd,'+''''+''''+') cInvStd,cn.cComUnitName,cInvCode  from 
--	(select I.cInvName,I.cInvStd,I.cInvCode,I.cComUnitCode,I.cInvCCode,I.dEDate from Inventory I where  I.bSale=1 and isnull(dEDate,'+''''+'9999-12-31'+''''+')  > GETDATE()
--	and (I.cInvCode like '+''''+@cInvCCode+'%' +''''+') and I.cInvCode not in (select cinvCode from  SA_CusInvLimited where cCusCode='+''''+@cCusCode+''''+' and  bLimited=0 ) ) ic 
--	left join computationunit cn on ic.cComUnitCode=cn.cComunitCode
--end
--if @bLimit=1 
--BEGIN
--	select cInvName,isnull(cInvStd,'+''''+''''+') cInvStd,cn.cComUnitName,cInvCode  from 
--	(select I.cInvName,I.cInvStd,I.cInvCode,I.cComUnitCode,I.cInvCCode,I.dEDate from Inventory I where  I.bSale=1 and isnull(dEDate,'+''''+'9999-12-31'+''''+')  > GETDATE()
--	and (I.cInvCode like '+''''+@cInvCCode+'%' +''''+') and I.cInvCode in (select cinvCode from  SA_CusInvLimited where cCusCode='+''''+@cCusCode+''''+' and  bLimited=1 ) ) ic 
--	left join computationunit cn on ic.cComUnitCode=cn.cComunitCode 
--end'

--select @strsql
exec(@strsql)
 	--网上下单系统已存在的未生成U8订单和驳回修改的订单数量,待顾客确认的订单,这部分数量需要用U8数量减去

END

--SELECT * from Inventory I  left JOIN SA_CusInvLimited L  on I.cinvcode=l.cInvCode 
--and  L.ccuscode='010102' and L.blimited=0 where l.cInvCode is null and (I.cInvCode LIKE '01%' or I.cInvCode LIKE '02%' or I.cInvCode LIKE '32%') 

--exec DLproc_TreeListDetailsAllBySel '010201002','010101'
--exec DLproc_TreeListDetailsAllBySel '0101','010101'
--exec DLproc_TreeListDetailsAllBySel '0102','010101'
--exec DLproc_TreeListDetailsAllBySel '0103','010101'
--exec DLproc_TreeListDetailsAllBySel '0104','010101'
--exec DLproc_TreeListDetailsAllBySel '320401','010101'
--exec DLproc_TreeListDetailsAllBySel '01','010101'


--select  dod.cinvcode,isnull(sum(iquantity),0) 'iquantity' from Dl_opOrderDetail dod
--inner join Dl_opOrder do on do.lngopOrderId=dod.lngopOrderId
--inner join Inventory ii on ii.cInvCode=dod.cinvcode
--where ii.cInvCode like '01%' and do.bytStatus in (1,2,3)
--group by dod.cinvcode ) as cc on cc.cinvcode=aa.cInvCode


--select cInvName,cInvStd,cComUnitName,cInvCode,(fAvailQtty-iquantity)*0.7 'fAvailQtty' from (
--select aa.cInvName,aa.cInvStd,aa.cComUnitName,aa.cInvCode,isnull(bb.fAvailQtty,0) 'fAvailQtty'
--,isnull(cc.iquantity,0) 'iquantity' 
--from (
--select cInvName,isnull(cInvStd,' ') 'cInvStd',cn.cComUnitName,cInvCode 
--from 
--(SELECT I.cInvName,I.cInvStd,I.cInvCode,I.cComUnitCode,I.cInvCCode from Inventory I  left JOIN SA_CusInvLimited L  on I.cinvcode=l.cInvCode 
--and  L.ccuscode='010101' and L.blimited=0 where l.cInvCode is null and (I.cInvCode LIKE '01%' or I.cInvCode LIKE '02%' or I.cInvCode LIKE '32%') ) ic 
-- left join computationunit cn on ic.cComUnitCode=cn.cComunitCode 
--WHERE (ic.cInvCode like '01%') ) as aa
--left join (
--SELECT  I.cInvCode,I.cInvCCode , sum(
-- (CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END)) AS fAvailQtty
-- FROM v_ST_currentstockForReport  CS inner join dbo.Inventory I ON I.cInvCode = CS.cInvCode   
-- left join dbo.InventoryClass IC ON IC.cInvCCode = I.cInvCCode LEFT OUTER JOIN dbo.ComputationUnit CU_G ON I.cSTComUnitCode =CU_G.cComUnitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_A ON I.cAssComUnitCode = CU_A.cComunitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_M ON I.cComUnitCode = CU_M.cComunitCode 
-- LEFT OUTER JOIN dbo.Warehouse W ON CS.cWhCode = W.cWhCode 
-- left join vendor v1 on v1.cvencode = cs.cvmivencode 
-- left join v_aa_enum E1 on E1.enumcode = ISNULL(cs.iExpiratDateCalcu,0) and E1.enumtype=N'SCM.ExpiratDateCalcu' 
-- LEFT OUTER JOIN dbo.v_aa_enum E with (nolock) on E.enumcode=convert(nchar,CS.cMassUnit) and E.enumType=N'ST.MassUnit' 
-- where I.cInvCode like '01%'
-- group by I.cInvCode,I.cInvCCode ) as bb on aa.cInvCode=bb.cInvCode
-- 	left join (
--select  dod.cinvcode,isnull(sum(iquantity),0) 'iquantity' from Dl_opOrderDetail dod
--inner join Dl_opOrder do on do.lngopOrderId=dod.lngopOrderId
--inner join Inventory ii on ii.cInvCode=dod.cinvcode
--where ii.cInvCode like '01%' and do.bytStatus in (1,2,3)
--group by dod.cinvcode ) as cc on cc.cinvcode=aa.cInvCode
--) as hh order by hh.cInvCode,hh.cInvName,hh.cInvStd
GO
/****** Object:  StoredProcedure [dbo].[DLproc_TreeListDetailsBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:echo
-- Create date:2015-10-10
-- Description:	用于查询选中的物料大类下的明细数据
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_TreeListDetailsBySel]

@cInvCCode nvarchar(20),
@cCusCode nvarchar(20)
AS

BEGIN

	SET NOCOUNT ON;

--select cInvName,isnull(cInvStd,' ') 'cInvStd',cn.cComUnitName,cInvCode,
--ic.cInvCode+'|'+ic.cInvName+'|'+isnull(ic.cInvStd,' ')+'|'+cn.cComUnitName+'|'+ic.cInvDefine1+'|'+ic.cInvDefine2+'|'+cast(ic.cInvDefine13 as nvarchar(50))+'|'+
--cast(ic.cInvDefine14 as nvarchar(50))+'|'+'1'+ic.cInvDefine1+'='+cast(ic.cInvDefine13 as nvarchar(10))+ic.cInvDefine2+'='
--+cast(cast(ic.cInvDefine13 as decimal(18,0))*cast(ic.cInvDefine14 as decimal(18,0)) as nvarchar(50))+cn.cComUnitName 'data'
--from Inventory ic left join computationunit cn on ic.cComUnitCode=cn.cComunitCode 
--WHERE ([cInvCCode] = @cInvCCode)

--v1.00
--select cInvName,isnull(cInvStd,' ') 'cInvStd',cn.cComUnitName,cInvCode 
--from 
--(SELECT I.cInvName,I.cInvStd,I.cInvCode,I.cComUnitCode,I.cInvCCode from Inventory I  left JOIN SA_CusInvLimited L  on I.cinvcode=l.cInvCode 
--and  L.ccuscode=@cCusCode and L.blimited=0 where l.cInvCode is null and (I.cInvCode LIKE '01%' or I.cInvCode LIKE '02%' or I.cInvCode LIKE '32%')) ic 
-- left join computationunit cn on ic.cComUnitCode=cn.cComunitCode 
--WHERE (ic.cInvCCode = @cInvCCode)
--v2.00 2015-12-02
--v3.00 /*2015.12.28:增加dEDate is null,如果禁用不为空则是被禁用商品,*/
/*2015.01.08禁用,启用新的查询,不计算可用量Begin*/
--select cInvName,cInvStd,cComUnitName,cInvCode,(fAvailQtty-iquantity)*0.7 'fAvailQtty' from (
--select aa.cInvName,aa.cInvStd,aa.cComUnitName,aa.cInvCode,isnull(bb.fAvailQtty,0) 'fAvailQtty',isnull(cc.iquantity,0) 'iquantity',dEDate from (
--select cInvName,isnull(cInvStd,' ') 'cInvStd',cn.cComUnitName,cInvCode,dEDate 
--from 
--(SELECT I.cInvName,I.cInvStd,I.cInvCode,I.cComUnitCode,I.cInvCCode,I.dEDate from Inventory I  left JOIN SA_CusInvLimited L  on I.cinvcode=l.cInvCode 
--and  L.ccuscode=@cCusCode and L.blimited=0 where l.cInvCode is null and (I.cInvCode LIKE '01%' or I.cInvCode LIKE '02%' or I.cInvCode LIKE '32%')) ic 
-- left join computationunit cn on ic.cComUnitCode=cn.cComunitCode 
--WHERE (ic.cInvCCode = @cInvCCode) ) as aa
--left join (
--SELECT  I.cInvCode,I.cInvCCode , sum(
-- (CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
-- ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END)) AS fAvailQtty
-- FROM v_ST_currentstockForReport  CS inner join dbo.Inventory I ON I.cInvCode = CS.cInvCode   
-- left join dbo.InventoryClass IC ON IC.cInvCCode = I.cInvCCode LEFT OUTER JOIN dbo.ComputationUnit CU_G ON I.cSTComUnitCode =CU_G.cComUnitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_A ON I.cAssComUnitCode = CU_A.cComunitCode 
-- LEFT OUTER JOIN dbo.ComputationUnit CU_M ON I.cComUnitCode = CU_M.cComunitCode 
-- LEFT OUTER JOIN dbo.Warehouse W ON CS.cWhCode = W.cWhCode 
-- left join vendor v1 on v1.cvencode = cs.cvmivencode 
-- left join v_aa_enum E1 on E1.enumcode = ISNULL(cs.iExpiratDateCalcu,0) and E1.enumtype=N'SCM.ExpiratDateCalcu' 
-- LEFT OUTER JOIN dbo.v_aa_enum E with (nolock) on E.enumcode=convert(nchar,CS.cMassUnit) and E.enumType=N'ST.MassUnit' 
-- where I.cInvCCode=@cInvCCode
-- group by I.cInvCode,I.cInvCCode ) as bb on aa.cInvCode=bb.cInvCode
-- 	--网上下单系统已存在的未生成U8订单和驳回修改的订单数量,待顾客确认的订单,这部分数量需要用U8数量减去
--	left join (
--select  dod.cinvcode,isnull(sum(iquantity),0) 'iquantity' from Dl_opOrderDetail dod
--inner join Dl_opOrder do on do.lngopOrderId=dod.lngopOrderId
--inner join Inventory ii on ii.cInvCode=dod.cinvcode
--where ii.cInvCCode=@cInvCCode and do.bytStatus in (1,2,3)
--group by dod.cinvcode ) as cc on cc.cinvcode=aa.cInvCode
--) as hh  where hh.dEDate is null 
--order by hh.cInvCode,hh.cInvName,hh.cInvStd
--new code,2015.01.08
declare @strsql varchar(5000)
set @strsql=
'select cInvName,isnull(cInvStd,'+''''+' '+''''+') cInvStd,cn.cComUnitName,cInvCode  from 
(select I.cInvName,I.cInvStd,I.cInvCode,I.cComUnitCode,I.cInvCCode,I.dEDate from Inventory I where  I.bSale=1 and isnull(dEDate,'+''''+'9999-12-31'+''''+')  > GETDATE()
and (I.cInvCode like '+''''+@cInvCCode+'%' +''''+') and I.cInvCCode not in (select left(cinvCode,9) from  SA_CusInvLimited where cCusCode='+''''+@cCusCode+''''+' group by left(cinvCode,9) ) ) ic 
 left join computationunit cn on ic.cComUnitCode=cn.cComunitCode order by cInvCode,cInvName,cInvStd'
 exec (@strsql)
 --select @strsql
/*2015.01.08禁用,启用新的查询,不计算可用量End*/


END

--SELECT * from Inventory I  left JOIN SA_CusInvLimited L  on I.cinvcode=l.cInvCode 
--and  L.ccuscode='010102' and L.blimited=0 where l.cInvCode is null and (I.cInvCode LIKE '01%' or I.cInvCode LIKE '02%' or I.cInvCode LIKE '32%') 

--exec DLproc_TreeListDetailsBySel '010201001','010101'
--exec DLproc_TreeListDetailsBySel '010201001','010101'
--exec DLproc_TreeListDetailsBySel '010201001','010101'
--exec DLproc_TreeListDetailsBySel '010201001','010101'
--exec DLproc_TreeListDetailsBySel '3202','010108'





GO
/****** Object:  StoredProcedure [dbo].[DLproc_TreeListDetailsNoQTYBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:echo
-- Create date:2015-12-07
-- Description:	用于查询选中的物料大类下的明细数据,完善,显示大类数据
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_TreeListDetailsNoQTYBySel]

@cInvCCode nvarchar(20),
@cCusCode nvarchar(20)
AS

BEGIN

	SET NOCOUNT ON;
	set @cInvCCode=@cInvCCode+'%'
--if @cInvCCode='' or @cInvCCode is null or @cInvCCode=' ' or  @cInvCCode='01'
--begin
--select  1 cInvName,2 cInvStd,3 cComUnitName,4 cInvCode where 1=2
--end
--else
begin
--v3.00 /*2015.12.28:增加dEDate is null,如果禁用不为空则是被禁用商品,*/
select aa.cInvName,aa.cInvStd,aa.cComUnitName,aa.cInvCode from (
select cInvName,isnull(cInvStd,' ') 'cInvStd',cn.cComUnitName,cInvCode,dEDate 
from 
(SELECT I.cInvName,I.cInvStd,I.cInvCode,I.cComUnitCode,I.cInvCCode,I.dEDate from Inventory I  left JOIN SA_CusInvLimited L  on I.cinvcode=l.cInvCode 
and  L.ccuscode=@cCusCode and L.blimited=0 where l.cInvCode is null and (I.cInvCode LIKE '01%' or I.cInvCode LIKE '02%' or I.cInvCode LIKE '32%')) ic 
 left join computationunit cn on ic.cComUnitCode=cn.cComunitCode 
WHERE (ic.cInvCCode like @cInvCCode) ) as aa where aa.dEDate is null
order by aa.cInvCode
end


 

END

--SELECT * from Inventory I  left JOIN SA_CusInvLimited L  on I.cinvcode=l.cInvCode 
--and  L.ccuscode='010102' and L.blimited=0 where l.cInvCode is null and (I.cInvCode LIKE '01%' or I.cInvCode LIKE '02%' or I.cInvCode LIKE '32%') 

--exec DLproc_TreeListDetailsNoQTYBySel '010101','010101'





GO
/****** Object:  StoredProcedure [dbo].[DLproc_TreeListPreDetailsBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:echo
-- Create date:2015-12-11
-- Description:	用于查询选中的预订单下的物料明细及可用量
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_TreeListPreDetailsBySel]
@strBillNo nvarchar(20),
@lngBillType int
AS

BEGIN

	SET NOCOUNT ON;
	--cSCloser,行关闭人,null
--获取对应的U8的订单编号,数量,可用量,

select aa.cCode,bb.cInvCode,cc.cInvName,cc.cInvStd,isnull(iQuantity,0) 'iQuantity',isnull(iQuantity,0)-isnull(fdhquantity,0) 'qty',bb.autoid 'iaoids'  into #temp1 from SA_PreOrderDetails bb
inner join SA_PreOrderMain aa on aa.ID=bb.ID
inner join Inventory cc on bb.cInvCode=cc.cInvCode
inner join Dl_opPreOrder dd on dd.ccode=aa.cCode
where  bb.cSCloser is null and (isnull(iQuantity,0)-isnull(fdhquantity,0))>0 and dd.strBillNo=@strBillNo

 	--网上下单系统已存在的未生成U8订单和驳回修改的订单数量,待顾客确认的订单,这部分数量需要用U8数量减去
	select bb.cpreordercode,bb.cinvcode,isnull(sum(isnull(bb.iquantity,0)),0) 'iquantity' into #temp2 from Dl_opOrder aa
	inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId 
	where aa.lngBillType =@lngBillType  and aa.bytStatus in (1,2,3)
	group by bb.cpreordercode,bb.cinvcode

	--获取最终可用量,减去网上下单,未审核通过的订单
	select aa.*,(isnull(aa.qty,0)-isnull(bb.iquantity,0)) 'realqty',aa.cInvCode+aa.cCode 'itemid',iaoids from #temp1 aa 
	left  join #temp2 bb on aa.cInvCode=bb.cinvcode and aa.cCode=bb.cpreordercode 
	where (isnull(aa.qty,0)-isnull(bb.iquantity,0)) >0

END

--exec [DLproc_TreeListPreDetailsBySel] 'Y20151200020',1
--exec [DLproc_TreeListPreDetailsBySel] 'Y20151200019',1
--cpreordercode  预订单号

--select aa.cCode,bb.cInvCode,cc.cInvName,cc.cInvStd,isnull(iQuantity,0) 'iQuantity',isnull(iQuantity,0)-isnull(fdhquantity,0) 'qty' from SA_PreOrderDetails bb
--inner join SA_PreOrderMain aa on aa.ID=bb.ID
--inner join Inventory cc on bb.cInvCode=cc.cInvCode
--inner join Dl_opPreOrder dd on dd.ccode=aa.cCode
--where  bb.cSCloser is null and (isnull(iQuantity,0)-isnull(fdhquantity,0))>0 and dd.strBillNo='Y20151200020'

--	select bb.cpreordercode,bb.cinvcode,isnull(sum(isnull(bb.iquantity,0)),0) 'iquantity'  from Dl_opOrder aa
--	inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId 
--	where aa.lngBillType =1  and aa.bytStatus in (1,2,3)
--	group by bb.cpreordercode,bb.cinvcode

--	select cpreordercode,* from Dl_opOrderDetail






GO
/****** Object:  StoredProcedure [dbo].[DLproc_TreeListPreDetailsModifyBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:echo
-- Create date:2016-01-14
-- Description:	用于查询修改参照酬宾订单或者特殊订单的商品详情,可用量
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_TreeListPreDetailsModifyBySel]
@strBillNo varchar(30),	--预订单编号,treelist选中的dl预订单编号
@OrderBillNo varchar(30),	--被修改订单编码(这部分数量要被扣除)aa.strBillNo!=@OrderBillNo
@lngBillType int
AS

BEGIN

	SET NOCOUNT ON;
	--cSCloser,行关闭人,null
--获取对应的U8的订单编号,数量,可用量,(数量为订单总量,可用量为订单总量-累计订单数量,从U8预订单表中获取该数据)
select aa.cCode,bb.cInvCode,cc.cInvName,cc.cInvStd,isnull(iQuantity,0) 'iQuantity',isnull(iQuantity,0)-isnull(fdhquantity,0) 'qty',bb.autoid 'iaoids'  into #temp1 from SA_PreOrderDetails bb
inner join SA_PreOrderMain aa on aa.ID=bb.ID
inner join Inventory cc on bb.cInvCode=cc.cInvCode
inner join Dl_opPreOrder dd on dd.ccode=aa.cCode
where  bb.cSCloser is null and (isnull(iQuantity,0)-isnull(fdhquantity,0))>0 and dd.strBillNo=@strBillNo

 	--网上下单系统已存在的未生成U8订单和驳回修改的订单数量,待顾客确认的订单,这部分数量需要用U8数量减去,并且减去被修改订单的这部分数据,当选择的订单明细中有该数据时,则在cellperpare中删除
	--2016-01-15:排除所有状态为3,被驳回,待修改的订单,^^^重要更新,网上订单中对应的订单应该为私有数量,这部分数量需要单独取出,根据订单,查询到顾客编号,然后查询该顾客编号对应的所有的参照预订单商品数量
	--获取顾客编码,select ccuscode from Dl_opOrder where strBillNo=@strBillNo
	select bb.cpreordercode,bb.cinvcode,isnull(sum(isnull(bb.iquantity,0)),0) 'iquantity' into #temp2 from Dl_opOrder aa
	inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId 
	where aa.lngBillType =@lngBillType  and aa.bytStatus in (1,2) and aa.strBillNo!=@OrderBillNo and aa.ccuscode =(select top (1) ccuscode from Dl_opOrder where strBillNo=@strBillNo)
	group by bb.cpreordercode,bb.cinvcode

	--获取最终可用量,减去网上下单,未审核通过的订单
	select aa.*,(isnull(aa.qty,0)-isnull(bb.iquantity,0)) 'realqty',aa.cInvCode+aa.cCode 'itemid',iaoids from #temp1 aa 
	left  join #temp2 bb on aa.cInvCode=bb.cinvcode and aa.cCode=bb.cpreordercode 
	where (isnull(aa.qty,0)-isnull(bb.iquantity,0)) >0

END

--exec [DLproc_TreeListPreDetailsModifyBySel] 'Y20151200057','DLOP15120885',1
--exec [DLproc_TreeListPreDetailsModifyBySel] 'Y20151200052','DLOP15120885',1
--exec [DLproc_TreeListPreDetailsModifyBySel] 'Y20151200018','DLOP15120885',1
--cpreordercode  预订单号

--select aa.cCode,bb.cInvCode,cc.cInvName,cc.cInvStd,isnull(iQuantity,0) 'iQuantity',isnull(iQuantity,0)-isnull(fdhquantity,0) 'qty' from SA_PreOrderDetails bb
--inner join SA_PreOrderMain aa on aa.ID=bb.ID
--inner join Inventory cc on bb.cInvCode=cc.cInvCode
--inner join Dl_opPreOrder dd on dd.ccode=aa.cCode
--where  bb.cSCloser is null and (isnull(iQuantity,0)-isnull(fdhquantity,0))>0 and dd.strBillNo='Y20151200020'

--	select bb.cpreordercode,bb.cinvcode,isnull(sum(isnull(bb.iquantity,0)),0) 'iquantity'  from Dl_opOrder aa
--	inner join Dl_opOrderDetail bb on aa.lngopOrderId=bb.lngopOrderId 
--	where aa.lngBillType =1  and aa.bytStatus in (1,2,3)
--	group by bb.cpreordercode,bb.cinvcode

--	select cpreordercode,* from Dl_opOrderDetail



  


GO
/****** Object:  StoredProcedure [dbo].[DLproc_U8SOAAutoSendBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:echo
-- Create date:2016-01-20
-- Description:	用于定期发送 客户的欠款分析（对账单）
-- =============================================
CREATE PROCEDURE [dbo].[DLproc_U8SOAAutoSendBySel]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
--@ccuscode varchar(50),
--@ddate varchar(20)
AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from

	-- interfering with SELECT statements.

	SET NOCOUNT ON;
	declare @ddate varchar(20)
select @ddate=convert(varchar(10),getdate(),120)
select Ar_DetailCust.cDwCode as FL,Ar_DetailCust.cDwName as FL1,Ar_DetailCust.cDwCode as MX,Ar_DetailCust.cDwName as MX1,
sum(case when cCoVouchType like N'2%' or cCoVouchType like N'EX%' then iDAmount-iCAmount else 0 end)+sum(case when cCoVouchType like N'R%' or cCoVouchType like N'60%' or cCoVouchType 
like N'SR%' or cCoVouchType like N'IM92%' or cCoVouchType = N'IM40' or (cCoVouchType like N'4%' and cFlag=N'AP' and cBusType = '代理进口') then iDAmount-iCAmount else 0 end)-sum(case when cCoVouchType 
like N'4%' and cFlag=N'AR' then iCAmount-iDAmount else 0 end) as QK,sum(case when cCoVouchType like N'2%' or cCoVouchType like N'EX%' then iDAmount-iCAmount else 0 end) as HK,sum(case when cCoVouchType 
like N'R%' or cCoVouchType like N'60%' or cCoVouchType like N'SR%' or cCoVouchType like N'IM92%' or cCoVouchType = N'IM40' or (cCoVouchType like N'4%' and cFlag=N'AP' and cBusType = '代理进口') 
then iDAmount-iCAmount else 0 end) as YSK,sum(case when cCoVouchType like N'4%' and cFlag=N'AR' then iCAmount-iDAmount else 0 end) as SKD,max(Ar_DetailCust.iCreLine) as 
iCreLine Into #temp1 From Ar_DetailCust 
Where  ((isnull(dCreditStart,dregdate)<=@ddate)) And iFlag<3 and Ar_DetailCust.cDwCode is not null and Ar_DetailCust.cDwCode is not null And cProcStyle<> '9H'  
--And Ar_DetailCust.cDwCode =@ccuscode
Group By Ar_DetailCust.cDwCode,Ar_DetailCust.cDwName,Ar_DetailCust.cDwCode,Ar_DetailCust.cDwName

Insert Into #temp1 Select V_outnotbalance.cDwCode as FL,V_outnotbalance.cDwName as FL1,V_outnotbalance.cDwCode as MX,V_outnotbalance.cDwName as MX1,
sum(iAmount) as QK,sum(iAmount) as HK,0 as YSK,0 as SKD,max(V_outnotbalance.iCreLine) as iCreLine From V_outnotbalance Where  ((bcredit=1 and dCreditStart<=@ddate)) And cCheckMan<>''  and V_outnotbalance.cDwCode is not null 
and V_outnotbalance.cDwCode is not null  
--And V_outnotbalance.cDwCode = @ccuscode  
Group By V_outnotbalance.cDwCode,V_outnotbalance.cDwName,V_outnotbalance.cDwCode,V_outnotbalance.cDwName

Insert Into #temp1 Select AR_V_ExecuteBill.cDwCode as FL,AR_V_ExecuteBill.cDwName as FL1,AR_V_ExecuteBill.cDwCode as MX,AR_V_ExecuteBill.cDwName as MX1,
sum(iRAmount) as QK,sum(iRAmount) as HK,0 as YSK,0 as SKD,max(AR_V_ExecuteBill.iCreLine) as iCreLine From AR_V_ExecuteBill Where  ((bcredit=1 and dCreditStart<=@ddate)) and AR_V_ExecuteBill.cDwCode is not null 
and AR_V_ExecuteBill.cDwCode is not null  
--And AR_V_ExecuteBill.cDwCode = @ccuscode  
Group By AR_V_ExecuteBill.cDwCode,AR_V_ExecuteBill.cDwName,AR_V_ExecuteBill.cDwCode,AR_V_ExecuteBill.cDwName

Select max(FL) as FL,MAX(FL1) as FL1,MAX(MX) as MX,MAX(MX1) as MX1,sum(QK) as QK,sum(HK) as HK,sum(YSK) as YSK,sum(SKD) as SKD,max(iCreLine) as iCreLine into #tempCreLine from #temp1 
Group by FL,MX Having sum(QK)<>0 or sum(HK)<>0 or sum(YSK)<>0 or sum(SKD)<>0 Order By sum(QK) DESC
--货款金额:QK
--ccusname:MX1
--ccuscode:FL
 --选择出当前需要发送账单的顾客,(根据当前设置的账单日期中的day天,对比当前月的天数,是否满足,如账单日期为31号,而当前月只有30,则30号发送账单,类推)
select hh.* into #tempCus from (
select aa.strLoginName,bb.cCusCode,bb.cCusName,32-DAY(getdate()+32-DAY(getdate())) 'CurMonthDays',
case when (32-DAY(getdate()+32-DAY(getdate()))-isnull(cc.ccdefine1,0))>=0 then isnull(cc.ccdefine1,0)
else 32-DAY(getdate()+32-DAY(getdate())) end SOASendTime,
convert(varchar(10),day(GETDATE()))  'CurDay' from Dl_opUser aa 
left join Customer bb on aa.cCusCode=left(bb.cCusCode,6) left join Customer_extradefine cc on bb.cCusCode=cc.cCusCode  
where aa.strUserLevel=3 and strStatus=1 
) as hh where hh.CurDay=hh.SOASendTime

--关联当前的顾客,发送账单(插入数据)
insert into Dl_opU8SOA 
select aa.strLoginName ccuscode,aa.cCusName ccusname,GETDATE(),GETDATE(),isnull(bb.QK,0) dblAmount,
case when isnull(bb.QK,0)>=0 then dbo.L2U(isnull(bb.QK,0),1)
else '负'+dbo.L2U(isnull(bb.QK,0)*-1,1) end 'strUper',
'1' 'strOper','admin' 'strOperName',0,null,0,month(GETDATE()),year(GETDATE())
from #tempCus aa left join #tempCreLine bb on aa.cCusCode=bb.FL

--select * from Dl_opU8SOA
--插入soa账单
--insert into Dl_opU8SOA 
--select ccuscode,ccusname, strEndDate,datSendTime, dblAmount, strUper, strOper,strOperName,bytCheck,datCheckTime,intperiodid,intPeriod,intPeriodYear
--当月 天数 :32-DAY(getdate()+32-DAY(getdate()))


drop table #temp1
drop table #tempCus
drop table #tempCreLine

END
--exec DLproc_U8SOAAutoSendBySel  
--select * from Dl_opUser
--update Dl_opUser set SOATime='2016-01-22' where strUserLevel=3 and SOATime='2016-01-21'
--顾客编号
--顾客姓名
--欠款总计
--信用额度
--信用余额
--货款
--应收款
--预收款
--【业务规则】
--系统默认的分析条件为：分析客户+所有币种+截止当前日期+显示信用额度+所有款项。
--欠款总计=货款+其他应收款-预收款。其中货款=到截止日期仍未结算完的发票（正向-负向）之和；
--其他应收款=到截止日期仍未结算完的应收单（正向-负向）之和；
--预收款=到截止日期预收款余额。


--顾客编号,ccuscode
--顾客名称,ccusname
--账单截至日期,strEndDate
--账单发送时间,datSendTime
--账单金额,dblAmount
--大写,strUper
--操作员,strOper
--是否确认,bytCheck
--确认时间,datCheckTime
--账单期间,intPeriod


GO
/****** Object:  StoredProcedure [dbo].[DLproc_U8SOABySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:echo
-- Create date:2015-12-18
-- Description:	用于查询客户的欠款分析（对账单）
-- =============================================
CREATE PROCEDURE [dbo].[DLproc_U8SOABySel]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
@ccuscode varchar(50),
@ddate varchar(20)
AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from

	-- interfering with SELECT statements.

	SET NOCOUNT ON;
--select @ddate=convert(varchar(10),@ddate,120)
select Ar_DetailCust.cDwCode as FL,Ar_DetailCust.cDwName as FL1,Ar_DetailCust.cDwCode as MX,Ar_DetailCust.cDwName as MX1,
sum(case when cCoVouchType like N'2%' or cCoVouchType like N'EX%' then iDAmount-iCAmount else 0 end)+sum(case when cCoVouchType like N'R%' or cCoVouchType like N'60%' or cCoVouchType 
like N'SR%' or cCoVouchType like N'IM92%' or cCoVouchType = N'IM40' or (cCoVouchType like N'4%' and cFlag=N'AP' and cBusType = '代理进口') then iDAmount-iCAmount else 0 end)-sum(case when cCoVouchType 
like N'4%' and cFlag=N'AR' then iCAmount-iDAmount else 0 end) as QK,sum(case when cCoVouchType like N'2%' or cCoVouchType like N'EX%' then iDAmount-iCAmount else 0 end) as HK,sum(case when cCoVouchType 
like N'R%' or cCoVouchType like N'60%' or cCoVouchType like N'SR%' or cCoVouchType like N'IM92%' or cCoVouchType = N'IM40' or (cCoVouchType like N'4%' and cFlag=N'AP' and cBusType = '代理进口') 
then iDAmount-iCAmount else 0 end) as YSK,sum(case when cCoVouchType like N'4%' and cFlag=N'AR' then iCAmount-iDAmount else 0 end) as SKD,max(Ar_DetailCust.iCreLine) as 
iCreLine Into #temp1 From Ar_DetailCust 
Where  ((isnull(dCreditStart,dregdate)<=@ddate)) And iFlag<3 and Ar_DetailCust.cDwCode is not null and Ar_DetailCust.cDwCode is not null And cProcStyle<> '9H'  And Ar_DetailCust.cDwCode =@ccuscode
Group By Ar_DetailCust.cDwCode,Ar_DetailCust.cDwName,Ar_DetailCust.cDwCode,Ar_DetailCust.cDwName

Insert Into #temp1 Select V_outnotbalance.cDwCode as FL,V_outnotbalance.cDwName as FL1,V_outnotbalance.cDwCode as MX,V_outnotbalance.cDwName as MX1,
sum(iAmount) as QK,sum(iAmount) as HK,0 as YSK,0 as SKD,max(V_outnotbalance.iCreLine) as iCreLine From V_outnotbalance Where  ((bcredit=1 and dCreditStart<=@ddate)) And cCheckMan<>''  and V_outnotbalance.cDwCode is not null 
and V_outnotbalance.cDwCode is not null  And V_outnotbalance.cDwCode = @ccuscode  Group By V_outnotbalance.cDwCode,V_outnotbalance.cDwName,V_outnotbalance.cDwCode,V_outnotbalance.cDwName

Insert Into #temp1 Select AR_V_ExecuteBill.cDwCode as FL,AR_V_ExecuteBill.cDwName as FL1,AR_V_ExecuteBill.cDwCode as MX,AR_V_ExecuteBill.cDwName as MX1,
sum(iRAmount) as QK,sum(iRAmount) as HK,0 as YSK,0 as SKD,max(AR_V_ExecuteBill.iCreLine) as iCreLine From AR_V_ExecuteBill Where  ((bcredit=1 and dCreditStart<=@ddate)) and AR_V_ExecuteBill.cDwCode is not null 
and AR_V_ExecuteBill.cDwCode is not null  And AR_V_ExecuteBill.cDwCode = @ccuscode  Group By AR_V_ExecuteBill.cDwCode,AR_V_ExecuteBill.cDwName,AR_V_ExecuteBill.cDwCode,AR_V_ExecuteBill.cDwName

Select max(FL) as FL,MAX(FL1) as FL1,MAX(MX) as MX,MAX(MX1) as MX1,sum(QK) as QK,sum(HK) as HK,sum(YSK) as YSK,sum(SKD) as SKD,max(iCreLine) as iCreLine from #temp1 
Group by FL,MX Having sum(QK)<>0 or sum(HK)<>0 or sum(YSK)<>0 or sum(SKD)<>0 Order By sum(QK) DESC

drop table #temp1

END
--exec DLproc_U8SOABySel '01010101','2015-12-19'
--顾客编号
--顾客姓名
--欠款总计
--信用额度
--信用余额
--货款
--应收款
--预收款
--【业务规则】
--系统默认的分析条件为：分析客户+所有币种+截止当前日期+显示信用额度+所有款项。
--欠款总计=货款+其他应收款-预收款。其中货款=到截止日期仍未结算完的发票（正向-负向）之和；
--其他应收款=到截止日期仍未结算完的应收单（正向-负向）之和；
--预收款=到截止日期预收款余额。


--顾客编号,ccuscode
--顾客名称,ccusname
--账单截至日期,strEndDate
--账单发送时间,datSendTime
--账单金额,dblAmount
--大写,strUper
--操作员,strOper
--是否确认,bytCheck
--确认时间,datCheckTime
--账单期间,intPeriod


GO
/****** Object:  StoredProcedure [dbo].[DLproc_U8SOASearchBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:echo
-- Create date:2015-12-19
-- Description:	用于顾客查询客户的欠款分析（对账单）,顾客自定义条件查询
-- =============================================
CREATE PROCEDURE [dbo].[DLproc_U8SOASearchBySel]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
@strComboPeriodYear varchar(10),
@ccuscode varchar(50),
@intPeriod varchar(50),
@intCheck varchar(50),
@strconccuscode varchar(50)
AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from

	-- interfering with SELECT statements.

	SET NOCOUNT ON;
--select @ddate=convert(varchar(10),@ddate,120)
declare @strsql varchar(5000),@ccuscode1 varchar(200),@intPeriod1 varchar(200),@intCheck1 varchar(200),@strComboPeriodYears varchar(200)
set @strComboPeriodYears=' and intPeriodYear='+@strComboPeriodYear 

if @ccuscode='0'
begin
set @ccuscode1=' ccuscode like '+''''+@strconccuscode+'%'+''''+' and '
end
else
begin
set @ccuscode1=' ccuscode='+''''+@ccuscode+''''+' and '
end

if @intPeriod='0'
begin
set @intPeriod1=' 1=1 and '
end
else
begin
set @intPeriod1=' intPeriod='+@intPeriod+' and '
end

if @intCheck='-1'
begin
set @intCheck1=' 1=1'
end
else
begin
set @intCheck1=' bytCheck='+@intCheck
end

set @strsql='select *,case datCheckTime
 when '+''''+'1900-01-01 00:00:00.000'+''''+' then '+''''+' '+''''+' end datCheckTime1 from Dl_opU8SOA where '+@ccuscode1+@intPeriod1+@intCheck1 +@strComboPeriodYears+' order by intPeriod,ccuscode desc'
exec(@strsql)
--select @strsql
END
--exec [DLproc_U8SOASearchBySel] '2015','0',0,-1,'010101'
--exec [DLproc_U8SOASearchBySel] '0',12,0,'010101'
--exec [DLproc_U8SOASearchBySel] '0',0,1,'010101'
--顾客编号
--顾客姓名
--欠款总计
--信用额度
--信用余额
--货款
--应收款
--预收款
--【业务规则】
--系统默认的分析条件为：分析客户+所有币种+截止当前日期+显示信用额度+所有款项。
--欠款总计=货款+其他应收款-预收款。其中货款=到截止日期仍未结算完的发票（正向-负向）之和；
--其他应收款=到截止日期仍未结算完的应收单（正向-负向）之和；
--预收款=到截止日期预收款余额。


--顾客编号,ccuscode
--顾客名称,ccusname
--账单截至日期,strEndDate
--账单发送时间,datSendTime
--账单金额,dblAmount
--大写,strUper
--操作员,strOper
--是否确认,bytCheck
--确认时间,datCheckTime
--账单期间,intPeriod
 

GO
/****** Object:  StoredProcedure [dbo].[DLproc_U8SOASearchOfOperBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:echo
-- Create date:2015-12-21
-- Description:	用于顾客查询客户的欠款分析（对账单）,操作员自定义条件查询
-- =============================================
CREATE PROCEDURE [dbo].[DLproc_U8SOASearchOfOperBySel]
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
@ccuscode varchar(50),--顾客编码
@intPeriod varchar(50),--账单期间
@intCheck varchar(50)--是否发送
AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from

	-- interfering with SELECT statements.

	SET NOCOUNT ON;
--select @ddate=convert(varchar(10),@ddate,120)
declare @strsql varchar(5000),@ccuscode1 varchar(200),@intPeriod1 varchar(200),@intCheck1 varchar(200),@s int 

--获取所有顾客，编号，名称
select cCusCode,cCusName into #tempcus from Customer
 
--if @ccuscode=' '
--begin
--set @ccuscode1=' 1=1  '
--end
--else
--begin
--set @ccuscode1=' ccuscode='+''''+@ccuscode+'''' 
--end

create table #tempkk(
ccuscode	varchar(50),
ccusname	varchar(50),
lngSOAid	varchar(50),
strEndDate	varchar(50),
datSendTime	varchar(50),
dblAmount	varchar(50),
strUper	varchar(550),
strOper	varchar(50),
strOperName	varchar(50),
bytCheck	varchar(50),
datCheckTime	varchar(50),
intperiodid	varchar(50),
intPeriod	varchar(50),
datCheckTime1 varchar(50)
)

--set @strsql='select t1.cCusCode,t1.cCusName,t2.lngSOAid,t2.strEndDate,t2.datSendTime,t2.dblAmount,t2.strUper,t2.strOper,t2.strOperName,t2.bytCheck,t2.datCheckTime,
--t2.intperiodid,t2.intPeriod,case t2.datCheckTime when '+''''+'1900-01-01 00:00:00.000'+''''+' then '+''''+' '+''''+' end datCheckTime1 into #tempkk 
--from #tempcus t1 left join Dl_opU8SOA t2 on t1.cCusCode=t2.ccuscode where '
-- +@ccuscode1+' order by t2.intPeriod,t2.ccuscode desc'
--exec(@strsql)
if @ccuscode=' '
begin
insert into #tempkk
select t1.cCusCode ccuscode,t1.cCusName ccusname,t2.lngSOAid,t2.strEndDate,t2.datSendTime,t2.dblAmount,t2.strUper,t2.strOper,t2.strOperName,t2.bytCheck,t2.datCheckTime,
t2.intperiodid,t2.intPeriod,case t2.datCheckTime when '1900-01-01 00:00:00.000' then ' ' end datCheckTime1 
from #tempcus t1 left join Dl_opU8SOA t2 on t1.cCusCode=t2.ccuscode
end
else
begin
insert into #tempkk
select t1.cCusCode ccuscode,t1.cCusName ccusname,t2.lngSOAid,t2.strEndDate,t2.datSendTime,t2.dblAmount,t2.strUper,t2.strOper,t2.strOperName,t2.bytCheck,t2.datCheckTime,
t2.intperiodid,t2.intPeriod,case t2.datCheckTime when '1900-01-01 00:00:00.000' then ' ' end datCheckTime1 
from #tempcus t1 left join Dl_opU8SOA t2 on t1.cCusCode=t2.ccuscode where t2.ccuscode=@ccuscode
end


--select @strsql

--exec [DLproc_U8SOASearchOfOperBySel] ' ','0','-1'
	--创建12期间表
	create table #tempPeriod(
	intPeriod  varchar(10)
	)
	set @s=1
	while @s<13
	begin
	insert into #tempPeriod select @s
	set @s=@s+1
	end 
--选择顾客（全部顾客or单一顾客）+所有月份+已发送账单
select t1.intPeriod 'p',t2.* into #temp2 from #tempkk t2 cross join  #tempPeriod t1  order by t1.intPeriod desc,t2.cCusCode 
--on t1.intPeriod=t2.intPeriod 
--select * from #temp2 where lngSOAid is not null
--exec [DLproc_U8SOASearchOfOperBySel] ' ','0','-1'
--条件筛选
if @intPeriod='0'
begin
set @intPeriod1=' 1=1 and '
end
else
begin
set @intPeriod1=' p='+@intPeriod+' and '
end
 if @intCheck='-1'
begin
set @intCheck1=' 1=1'
end
if @intCheck='0'
begin
set @intCheck1=' strOper is not null or strOper !='+''''+' '+''''
end
if @intCheck='1'
begin
set @intCheck1=' strOper is null or strOper =='+''''+' '+''''
end
set @strsql='select *,p+ccuscode+isnull(lngSOAid,0) keyid from #temp2 where '+@intPeriod1+@intCheck1 +' order by intPeriod,ccuscode desc'
--select @strsql
exec (@strsql)

--drop table #tempcus
--drop table #tempPeriod
--drop table #temp1
--drop table #temp2
drop table #tempkk
END
--exec [DLproc_U8SOASearchOfOperBySel] ' ','0','-1'
--exec [DLproc_U8SOASearchOfOperBySel] ' ','12','-1'
--exec [DLproc_U8SOASearchOfOperBySel] '0',0,1,'010101'
--顾客编号
--顾客姓名
--欠款总计
--信用额度
--信用余额
--货款
--应收款
--预收款
--【业务规则】
--系统默认的分析条件为：分析客户+所有币种+截止当前日期+显示信用额度+所有款项。
--欠款总计=货款+其他应收款-预收款。其中货款=到截止日期仍未结算完的发票（正向-负向）之和；
--其他应收款=到截止日期仍未结算完的应收单（正向-负向）之和；
--预收款=到截止日期预收款余额。


--顾客编号,ccuscode
--顾客名称,ccusname
--账单截至日期,strEndDate
--账单发送时间,datSendTime
--账单金额,dblAmount
--大写,strUper
--操作员,strOper
--是否确认,bytCheck
--确认时间,datCheckTime
--账单期间,intPeriod
 

GO
/****** Object:  StoredProcedure [dbo].[DLproc_UnauditedOrderBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:echo
-- Create date:2015-10-24
-- Description:	用于查询订单[[DLproc_UnauditedOrderBySel]]
--参数:订单状态:未审核,审核
-- =============================================


CREATE PROCEDURE [dbo].[DLproc_UnauditedOrderBySel]
@bytStatus int 
AS

BEGIN

	SET NOCOUNT ON;
	
	select do.strBillNo,du.strUserName,do.ccusname,cdefine11,strRemarks,convert(varchar(10),do.datCreateTime,120) 'datCreateTime' from Dl_opOrder do
	inner join Dl_opUser  du on do.lngopUserId=du.lngopUserId
	where do.bytStatus=@bytStatus and  do.strManagers = ' ' and cSTCode='00'
	order by do.strBillNo desc

END


GO
/****** Object:  StoredProcedure [dbo].[DLproc_UnauditedOrderManagersBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:echo
-- Create date:2015-10-24
-- Description:	用于查询订单[[DLproc_UnauditedOrderManagersBySel]]
--参数:订单状态:未审核,审核
-- =============================================


CREATE PROCEDURE [dbo].[DLproc_UnauditedOrderManagersBySel]
@strManagers varchar(100),
@lngBillType int
AS

BEGIN

	SET NOCOUNT ON;
	
	--select do.strBillNo,du.strUserName,do.ccusname,cdefine11,strRemarks,convert(varchar(10),do.datCreateTime,120) 'datCreateTime' from Dl_opOrder do
	--inner join Dl_opUser  du on do.lngopUserId=du.lngopUserId
	--where do.bytStatus='1' and do.strManagers = @strManagers and lngBillType=@lngBillType
	--order by do.strBillNo desc
	/*2015-12-14修改,lngBillType 显示*/
    select do.strBillNo,du.strUserName,do.ccusname,cdefine11,strRemarks,convert(varchar(10),do.datCreateTime,120) 'datCreateTime',do.strRejectRemarks,
	  
	 case when lngBillType='0' and cSTCode='00'  then '普通订单'
	 when lngBillType='0' and cSTCode='01'  then '样品资料'
	when lngBillType='1' then '酬宾订单'
		when lngBillType='2' then '特殊订单' end lngBillType
	from Dl_opOrder do
	inner join Dl_opUser  du on do.lngopUserId=du.lngopUserId
	where do.bytStatus='1' and do.strManagers = @strManagers 
	order by do.strBillNo desc

END


GO
/****** Object:  StoredProcedure [dbo].[DLproc_UnauditedPreOrderBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:echo
-- Create date:2015-12-10
-- Description:	用于查询订单[[DLproc_UnauditedPreOrderBySel]]
--参数:订单状态:未审核,审核
-- =============================================


CREATE PROCEDURE [dbo].[DLproc_UnauditedPreOrderBySel]
@bytStatus int,
@lngBillType int
AS

BEGIN

	SET NOCOUNT ON;
	
	select do.strBillNo,du.strUserName,do.ccusname,convert(varchar(10),do.ddate,120) 'datCreateTime' from Dl_opPreOrder do
	inner join Dl_opUser  du on do.lngopUserId=du.lngopUserId
	where do.bytStatus=@bytStatus and  do.strManagers = '  '  and lngBillType=@lngBillType
	order by do.strBillNo desc

END



GO
/****** Object:  StoredProcedure [dbo].[DLproc_UnauditedPreOrderManagersBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:echo
-- Create date:2015-10-24
-- Description:	用于查询订单[[DLproc_UnauditedPreOrderManagersBySel]]
--参数:订单状态:未审核,审核
-- =============================================


CREATE PROCEDURE [dbo].[DLproc_UnauditedPreOrderManagersBySel]
@strManagers varchar(100) ,
@lngBillType int 
AS

BEGIN

	SET NOCOUNT ON;
	
	select do.strBillNo,du.strUserName,do.ccusname,do.ddate  from Dl_opPreOrder do
	inner join Dl_opUser  du on do.lngopUserId=du.lngopUserId
	where do.bytStatus='1' and do.strManagers = @strManagers and lngBillType=@lngBillType 
	order by do.strBillNo desc

END



GO
/****** Object:  StoredProcedure [dbo].[DLproc_UserAddressPSBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================

-- Author:echo

-- Create date:2015-10-12

-- Description:	用于查询获取配送方式(配送)[DLproc_UserAddressPSBySel]

-- =============================================

CREATE PROCEDURE [dbo].[DLproc_UserAddressPSBySel]

@lngopUserId int

AS

BEGIN

	SET NOCOUNT ON;

SELECT [lngopUseraddressId], [lngopUserId], [strDistributionType], [strConsigneeName], [strConsigneeTel], 
[strReceivingAddress],[datCreatetime], [lngCreater],[bitSYSDefault],[bitDistributionTypeDefault],strProvince,strCity,strDistrict FROM [Dl_opUserAddress]
WHERE ([lngopUserId] = @lngopUserId) and [strDistributionType]='配送' and bytStatus=0

END


GO
/****** Object:  StoredProcedure [dbo].[DLproc_UserAddressPSBySelGroup]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:echo
-- Create date:2015-10-20
-- Description:	用于查询获取配送方式(配送)[[DLproc_UserAddressPSBySelGroup]]
-- =============================================

CREATE PROCEDURE [dbo].[DLproc_UserAddressPSBySelGroup]

@lngopUserId int

AS

BEGIN

	SET NOCOUNT ON;

SELECT [lngopUseraddressId], [strDistributionType]+','+[strConsigneeName]+','+[strConsigneeTel]+','+ strDistrict+[strReceivingAddress] 'ShippingInformation',
strConsigneeName,strConsigneeTel,strReceivingAddress,strCarplateNumber,strDriverName,strDriverTel,strIdCard,strDistrict FROM [Dl_opUserAddress]
WHERE ([lngopUserId] = @lngopUserId) and [strDistributionType]='配送' and bytStatus=0

END


GO
/****** Object:  StoredProcedure [dbo].[DLproc_UserAddressZTBySel]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================



-- Author:echo



-- Create date:2015-10-12



-- Description:	用于查询获取配送方式(自提)[DLproc_UserAddressZTBySel]



-- =============================================



CREATE PROCEDURE [dbo].[DLproc_UserAddressZTBySel]



@lngopUserId int



AS



BEGIN



	SET NOCOUNT ON;



SELECT [lngopUseraddressId], [lngopUserId], [strDistributionType], [strCarplateNumber],[strDriverName],[strDriverTel],
[strIdCard],[datCreatetime], [lngCreater],[bitSYSDefault],[bitDistributionTypeDefault] FROM [Dl_opUserAddress]
WHERE ([lngopUserId] = @lngopUserId) and [strDistributionType]='自提' and bytStatus=0



END


GO
/****** Object:  StoredProcedure [dbo].[DLproc_UserAddressZTBySelGroup]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:echo
-- Create date:2015-10-20
-- Description:	用于查询获取配送方式(自提)[[DLproc_UserAddressZTBySelGroup]]
-- =============================================


CREATE PROCEDURE [dbo].[DLproc_UserAddressZTBySelGroup]

@lngopUserId int

AS

BEGIN

	SET NOCOUNT ON;

SELECT [lngopUseraddressId], [strDistributionType]+',车牌号:'+[strCarplateNumber]+',司机姓名:'+[strDriverName]+',司机电话:'+[strDriverTel]+',司机身份证:'+[strIdCard] 'ShippingInformation',
strConsigneeName,strConsigneeTel,strReceivingAddress,strCarplateNumber,strDriverName,strDriverTel,strIdCard,strDistrict FROM [Dl_opUserAddress]
WHERE ([lngopUserId] = @lngopUserId) and [strDistributionType]='自提' and bytStatus=0

END


GO
/****** Object:  UserDefinedFunction [dbo].[L2U]    Script Date: 2016-02-27 8:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[L2U](@n_LowerMoney numeric(15,2),@v_TransType int) 

RETURNS VARCHAR(200) AS 

BEGIN 

Declare @v_LowerStr VARCHAR(200) -- 小写金额 

Declare @v_UpperPart VARCHAR(200) 

Declare @v_UpperStr VARCHAR(200) -- 大写金额

Declare @i_I int



set @v_LowerStr = LTRIM(RTRIM(ROUND(@n_LowerMoney,2))) --四舍五入为指定的精度并删除数据左右空格

set @i_I = 1

set @v_UpperStr = ''



while ( @i_I <= len(@v_LowerStr))

begin

select @v_UpperPart = case substring(@v_LowerStr,len(@v_LowerStr) - @i_I + 1,1)

WHEN '.' THEN '元'

WHEN '0' THEN '零'

WHEN '1' THEN '壹'

WHEN '2' THEN '贰'

WHEN '3' THEN '叁'

WHEN '4' THEN '肆'

WHEN '5' THEN '伍'

WHEN '6' THEN '陆'

WHEN '7' THEN '柒'

WHEN '8' THEN '捌'

WHEN '9' THEN '玖'

END

+ 

case @i_I

WHEN 1 THEN '分'

WHEN 2 THEN '角'

WHEN 3 THEN ''

WHEN 4 THEN ''

WHEN 5 THEN '拾'

WHEN 6 THEN '佰'

WHEN 7 THEN '仟'

WHEN 8 THEN '万'

WHEN 9 THEN '拾'

WHEN 10 THEN '佰'

WHEN 11 THEN '仟'

WHEN 12 THEN '亿'

WHEN 13 THEN '拾'

WHEN 14 THEN '佰'

WHEN 15 THEN '仟'

WHEN 16 THEN '万'

ELSE ''

END

set @v_UpperStr = @v_UpperPart + @v_UpperStr

set @i_I = @i_I + 1

end



if ( 0 = @v_TransType)

begin

set @v_UpperStr = REPLACE(@v_UpperStr,'零拾','零') 

set @v_UpperStr = REPLACE(@v_UpperStr,'零佰','零') 

set @v_UpperStr = REPLACE(@v_UpperStr,'零仟','零') 

set @v_UpperStr = REPLACE(@v_UpperStr,'零零零','零')

set @v_UpperStr = REPLACE(@v_UpperStr,'零零','零')

set @v_UpperStr = REPLACE(@v_UpperStr,'零角零分','整')

set @v_UpperStr = REPLACE(@v_UpperStr,'零分','整')

set @v_UpperStr = REPLACE(@v_UpperStr,'零角','零')

set @v_UpperStr = REPLACE(@v_UpperStr,'零亿零万零元','亿元')

set @v_UpperStr = REPLACE(@v_UpperStr,'亿零万零元','亿元')

set @v_UpperStr = REPLACE(@v_UpperStr,'零亿零万','亿')

set @v_UpperStr = REPLACE(@v_UpperStr,'零万零元','万元')

set @v_UpperStr = REPLACE(@v_UpperStr,'万零元','万元')

set @v_UpperStr = REPLACE(@v_UpperStr,'零亿','亿')

set @v_UpperStr = REPLACE(@v_UpperStr,'零万','万')

set @v_UpperStr = REPLACE(@v_UpperStr,'零元','元')

set @v_UpperStr = REPLACE(@v_UpperStr,'零零','零')

end



-- 对壹元以下的金额的处理 

if ( '元' = substring(@v_UpperStr,1,1))

begin

set @v_UpperStr = substring(@v_UpperStr,2,(len(@v_UpperStr) - 1))

end



if ( '零' = substring(@v_UpperStr,1,1))

begin

set @v_UpperStr = substring(@v_UpperStr,2,(len(@v_UpperStr) - 1))

end



if ( '角' = substring(@v_UpperStr,1,1))

begin

set @v_UpperStr = substring(@v_UpperStr,2,(len(@v_UpperStr) - 1))

end



if ( '分' = substring(@v_UpperStr,1,1))

begin

set @v_UpperStr = substring(@v_UpperStr,2,(len(@v_UpperStr) - 1))

end



if ('整' = substring(@v_UpperStr,1,1))

begin

set @v_UpperStr = '零元整'

end

return @v_UpperStr

END

GO
