USE [UFDATA_001_2015]
GO
/****** Object:  StoredProcedure [dbo].[DL_cashUser_money]    Script Date: 2015-11-30 18:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



create proc [dbo].[DL_cashUser_money](@CusCode VARCHAR(20))
as
BEGIN
        DECLARE @CusCreLine decimal
				select @CusCreLine=iCusCreLine from Customer where cCusCode= @CusCode

        if 0 =@CusCreLine
        BEGIN

            create table #TmpTable  
            (
            cCusCode    varchar(50),  
            cCusAddress    varchar(50),
            cContact    varchar(50),  
            cManagerEmp    varchar(50),  
            CLevl    varchar(50),  
            dTotalBuyAmt    decimal,  
            deCreditBal    decimal,  
            dEArbal    decimal  
            );

            insert into #TmpTable  exec usp_sa_getCusInfoForVouchHelp @cCusCode=@CusCode  ;

            SELECT @CusCreLine= deARBal  from  #TmpTable

            DROP table #TmpTable 

            DECLARE @OrderID VARCHAR(20)
            DECLARE @OrderMoney decimal

					declare SO_MainCursor cursor                                --声明一个游标，查询满足条件的数据
          for SELECT M.cSOCode, sum(D.iNatMoney) from SO_SOMain M,SO_SODetails D 
                    where D.csocode=M.csocode 
                    and  M.cCusCode=@CusCode and (M.iStatus=0 or M.iStatus is null ) 
                    GROUP BY M.cSOCode  ORDER BY M.cSOCode desc

          open SO_MainCursor;

          fetch next from SO_MainCursor into @OrderID ,@OrderMoney
          while @@FETCH_STATUS =0 
          begin
                    --SELECT @OrderID ,@OrderMoney
										if (@CusCreLine>=@OrderMoney)	
											BEGIN
												SELECT @OrderID ,@OrderMoney,@CusCreLine
												update SO_SOMain set cDefine16= @OrderMoney where cSOCode=@OrderID
												set @CusCreLine=@CusCreLine-@OrderMoney
											END
										fetch next from SO_MainCursor into @OrderID ,@OrderMoney
          end
					
          close SO_MainCursor    --关闭
          deallocate SO_MainCursor    --删除

			END
END


GO
/****** Object:  StoredProcedure [dbo].[DL_FetchCusInvLimited]    Script Date: 2015-11-30 18:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[DL_FetchCusInvLimited]  ---ycc 添加
	(@sCusCode nvarchar(20)=N'')
 AS
	declare @bLimitSale bit       --客户是否控制允限销
	declare @bLimit	 bit	        --客户是 允销 限销
	
	SELECT @bLimitSale=IsNull(C.bLimitSale,0) from Customer C where C.cCusCode=@sCusCode

	if @bLimitSale=0 
	begin
		select I.cInvCode,I.cInvName,I.cInvStd,I.cInvCCode from Inventory I where I.bSale=1
	end
	ELSE
	BEGIN
		select @bLimit=COUNT(l.bLimited)  from SA_CusInvLimited l where l.cCusCode=@sCusCode;
		if @bLimit=0 

		select 1 ---I.cInvCode,I.cInvName,I.cInvStd,I.cInvCCode from Inventory I where I.bSale=1

		ELSE
	  begin
				select TOP 1  @bLimit=l.bLimited  from SA_CusInvLimited l where l.cCusCode=@sCusCode;
				
				if @bLimit=1 
				BEGIN
					select I.cInvCode,I.cInvName,I.cInvStd,I.cInvCCode from Inventory I ,SA_CusInvLimited C where C.cInvCode=I.cInvCode and I.bSale=1 and C.cCusCode=@sCusCode
				end
				if @bLimit=0 
				BEGIN
					select I.cInvCode,I.cInvName,I.cInvStd,I.cInvCCode from Inventory I where I.bSale=1 and I.cInvCode not in (select  cInvCode from SA_CusInvLimited where cCusCode=@sCusCode) 
				end
	  end
end


GO
/****** Object:  StoredProcedure [dbo].[DL_getCusCreditInfo]    Script Date: 2015-11-30 18:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[DL_getCusCreditInfo]     -- 原函数名称为 usp_sa_getCusInfoForVouchHelp  ycc 添加函数取客户实际信用额
 (     
   @cCusCode as nvarchar(60)=N''    
 )    
as    
 declare @iTotalMoney decimal(20,6)  --累计购货金额    
 declare @iArBalance decimal(20,6)  --应收余额    
 declare @iCreBalance decimal(20,6)  --信用余额    
    
 declare @chrWhereCus nvarchar(200) --客户条件    
  
   
 set @chrWhereCus=(select cCusCreditCompany from customer where ccuscode=@cCusCode)  
 --set @chrWhereCus = N'and cCusCreditCompany = N''' + @chrWhereCus + N''' '    
    
 --删除临时表    
 --if exists(select name from tempdb..sysobjects where id = object_id(N'tempdb..UFTmpTable_sa_credit999'))    
 -- drop table tempdb..UFTmpTable_sa_credit999    
     
 --调用信用余额表报表取客户应收余额、信用余额  
  declare @dblDepMoney as float
 exec SA_DepCusCreditSum @cCusCode, N'', @iCreBalance output,  @dblDepMoney output
 select  @iArBalance =  cast(isnull(sum(farsum),0) as decimal(26,2))     
 from SA_CreditSum inner join Customer on SA_CreditSum.cCusCode =customer.cCusCode where customer.cInvoiceCompany =@chrWhereCus
    
 --计算累计购货金额    
 select @iTotalMoney = cast(isnull(sum(inatmoney),0) as decimal(26,2))     
 from salebillvouchs b inner join salebillvouch h on b.sbvid = h.sbvid    
 where h.ccuscode = @ccuscode    
    
 --返回数据    
 select  cCusCode,  
  @iCreBalance as CusCredit,   ------ 自己计算的信用余额
  ccusoaddress as cCusAddress, --客户地址    
  ccusperson as cContact,  --联系人    
  ccuspperson as cManageEmp, --主管业务员    
  customerkcode as CLevel, --客户等级    
  @iTotalMoney as dTotalBuyAmt, --累计购货金额    
  @iArBalance as deCreditBal, --应收余额    
  @iCreBalance as deARBal  --信用余额    
 from customer where cCusCode=@cCusCode    
  
  


GO
/****** Object:  StoredProcedure [dbo].[DLproc_InventoryBySel]    Script Date: 2015-11-30 18:00:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================

-- Author:echo

-- Create date:2015-09-28

-- Description:	用于查询物料大类结构,生成物料树结构

-- =============================================

CREATE PROCEDURE [dbo].[DLproc_InventoryBySel]

	-- Add the parameters for the stored procedure here

	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 

	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
@cSTCode varchar(10)
AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from

	-- interfering with SELECT statements.

	SET NOCOUNT ON;



    -- Insert statements for procedure here
if @cSTCode='0' or @cSTCode='00'
begin
		select cInvCCode as 'KeyFieldName',case iInvCGrade
		when 1 then 'null'
		when 2 then SUBSTRING(cInvCCode,1,2)
		when 3 then SUBSTRING(cInvCCode,1,4)
		when 4 then SUBSTRING(cInvCCode,1,6)
		when 5 then SUBSTRING(cInvCCode,1,8)
		when 6 then SUBSTRING(cInvCCode,1,10)
		end 'ParentFieldName' ,
		--case iInvCGrade
		--when 1 then cInvCName
		--else SUBSTRING(cInvCName,len(cInvCName)-charindex('-',reverse(cInvCName),0)+2,charindex('-',reverse(cInvCName),0))
		--end 'NodeName' 
		cInvCName 'NodeName'
		from InventoryClass where SUBSTRING(cInvCCode,1,2)='01' or SUBSTRING(cInvCCode,1,2)='02'
		order by cInvCCode,iInvCGrade,bInvCEnd
end
Else
begin
		select cInvCCode as 'KeyFieldName',case iInvCGrade
		when 1 then 'null'
		when 2 then SUBSTRING(cInvCCode,1,2)
		when 3 then SUBSTRING(cInvCCode,1,4)
		when 4 then SUBSTRING(cInvCCode,1,6)
		when 5 then SUBSTRING(cInvCCode,1,8)
		when 6 then SUBSTRING(cInvCCode,1,10)
		end 'ParentFieldName' ,
		--case iInvCGrade
		--when 1 then cInvCName
		--else SUBSTRING(cInvCName,len(cInvCName)-charindex('-',reverse(cInvCName),0)+2,charindex('-',reverse(cInvCName),0))
		--end 'NodeName' 
		cInvCName 'NodeName'
		from InventoryClass where SUBSTRING(cInvCCode,1,2)='32'
		order by cInvCCode,iInvCGrade,bInvCEnd
end



END

GO
/****** Object:  StoredProcedure [dbo].[DLproc_MyWorkBySel]    Script Date: 2015-11-30 18:00:33 ******/
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
@strBillNo varchar(50),
@beginDate varchar(20), 
@endDate  varchar(20),
@cSTCode  varchar(10),
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
else
begin
set @sqlcSTCode='cSTCode='+''''+@cSTCode+''''+' and '
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

set @sqlstrManagers='strManagers='+''''+@strManagers+''''

set @sql='select strBillNo,isnull(cSOCode,'+''''+' '+''''+') cSOCode,convert(varchar(10),datCreateTime,120) datCreateTime,ccusname,cdefine11,strRemarks,strLoadingWays,
case bytStatus
when '+''''+'1'+''''+' then '+''''+'未审核'+''''+'
when '+''''+'2'+''''+' then '+''''+'待确认'+''''+'
when '+''''+'3'+''''+' then '+''''+'被驳回'+''''+'
when '+''''+'4'+''''+' then '+''''+'已审核'+''''+'
 end bytStatus from Dl_opOrder where '+@sqlstrBillNo+@sqlbeginDate+@sqlendDate+@sqlcSTCode+@sqlbytStatus+@sqlstrManagers+' order by strBillNo desc'

--select @sql
exec (@sql)
END

--exec DLproc_MyWorkBySel '','2015-11-21','2015-11-24','-1',4,2



GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewOrderByIns]    Script Date: 2015-11-30 18:00:33 ******/
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
@strU8BillNo varchar(30)	--U8单据号,样品资料写入

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
--获取最大单号
select @maxbillno=isnull(max(convert(int,substring(strBillNo,5,9))),0)+1 from Dl_opOrder where strBillNo like 'DLOP%' 
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
select @strBillNo='DLOP'+convert(varchar(9),@maxbillno)
--select @strBillNo
--插入表头数据
insert into Dl_opOrder (
lngopUserId,strBillNo,datCreateTime,bytStatus,strRemarks,ccuscode,cdefine1,cdefine2,cdefine3,cdefine9,cdefine10,cdefine11,cdefine12,cdefine13,ccusname,cpersoncode,cSCCode,datDeliveryDate, strLoadingWays,cSTCode,lngopUseraddressId,RelateU8NO
) 
values (
@lngopUserId,@strBillNo,@datCreateTime,@bytStatus,@strRemarks,@ccuscode,@cdefine1,@cdefine2,@cdefine3,@cdefine9,@cdefine10,@cdefine11,@cdefine12,@cdefine13,@ccusname,@cpersoncode,@cSCCode,@datDeliveryDate, @strLoadingWays, @cSTCode,@lngopUseraddressId,@strU8BillNo
)
--20151129添加,样品资料直接给关联的U8订单专员负责,如果订单员已经接单,则直接关联转接
if @cSTCode='01'
begin
declare @strManagers varchar(10)
select @strManagers=isnull(strManagers,' ') from Dl_opOrder where cSOCode=@strU8BillNo
update Dl_opOrder set strManagers=@strManagers where strBillNo=@strBillNo
end
--20151129添加,清除临时授权,
update Customer set cCusPostCode=null where cCusCode=@ccuscode --客户编码,开票单位编码

--返回订单表头id
--select top(1) lngopOrderId from Dl_opOrder where strBillNo=@strBillNo
select max(lngopOrderId) 'lngopOrderId',@strBillNo 'strBillNo' from Dl_opOrder where strBillNo=@strBillNo

END




GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewOrderByUpd]    Script Date: 2015-11-30 18:00:33 ******/
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
@cdefine9 varchar(50),--自定义项8
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
/****** Object:  StoredProcedure [dbo].[DLproc_NewOrderDetailByIns]    Script Date: 2015-11-30 18:00:33 ******/
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
/****** Object:  StoredProcedure [dbo].[DLproc_NewOrderDetailByUpd]    Script Date: 2015-11-30 18:00:33 ******/
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
/****** Object:  StoredProcedure [dbo].[DLproc_NewOrderDetailU8ByIns]    Script Date: 2015-11-30 18:00:33 ******/
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


AS

BEGIN

	SET NOCOUNT ON;

--插入表头数据

--insert into Dl_opOrderDetail(
--lngopOrderId,cinvcode,iquantity,inum,iquotedprice,iunitprice,itaxunitprice,imoney,itax,isum,inatunitprice,inatmoney,inattax,inatsum,kl,itaxrate,cdefine22,iinvexchrate,cunitid,irowno
--) 
--values (
--@lngopOrderId,@cinvcode,@iquantity,@inum,@iquotedprice,@iunitprice,@itaxunitprice,@imoney,@itax,@isum,@inatunitprice,@inatmoney,@inattax,@inatsum,@kl,@itaxrate,@cdefine22,@iinvexchrate,@cunitid,@irowno
--)

END





GO
/****** Object:  StoredProcedure [dbo].[DLproc_NewOrderU8ByIns]    Script Date: 2015-11-30 18:00:33 ******/
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

	SET NOCOUNT ON;

declare @csocode varchar(50),@csysbarcode varchar(50),@maxbillno int,@id varchar(50),@cbsysbarcodeDetail varchar(50),@ddate varchar(50),@lngopOrderId varchar(50),@isosid varchar(50),@bytStatus varchar(2),@cmaker varchar(10),
@lngopUseraddressId varchar(10)--useraddress的ID
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

--获取表头最大id
select @id=MAX(id)+1 from SO_SOMain
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
@csysbarcode,convert(varchar(10),datCreateTime,120),@csocode,ccuscode,cdefine1,cdefine2,cdefine3,cdefine8,cdefine9,cdefine10,right(cdefine11,charindex(',',reverse(cdefine11))-1),cdefine12,cdefine13,convert(varchar(10),datCreateTime,120),
convert(varchar(10),datCreateTime,120),ccusname,ccuscode,strRemarks,cpersoncode,@id,cSCCode,datDeliveryDate, strLoadingWays, cSTCode,datBillTime,@cmaker,
--被动数据
N'0601',N'人民币',1,17,N'普通销售',0,0,0,1,0,0,131395,
--null
Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,
Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null,Null 
from Dl_opOrder where strBillNo=@strBillNo 
--插入扩展字段:(地址id对应的省市区信息)/*2015-11-21 增加*/;11-26修改,添加新的扩展字段2,样品资料关联U8的订单号
--insert into SO_SOMain_ExtraDefine (ID,chdefine1) select @id,strDistrict from Dl_opUserAddress where lngopUseraddressId=@lngopUseraddressId
insert into SO_SOMain_ExtraDefine (ID,chdefine1,chdefine2) select @id,dou.strDistrict,do.RelateU8NO from Dl_opUserAddress dou,Dl_opOrder do where dou.lngopUseraddressId=@lngopUseraddressId and do.strBillNo=@strBillNo

--插入U8表体数据V2.0
	--完成表头数据插入之后,获取表头的:销售订单主表标识
set @cbsysbarcodeDetail=@csysbarcode+'|'
select @ddate=datCreateTime,@lngopOrderId=lngopOrderId from Dl_opOrder where strBillNo=@strBillNo
select @isosid=max(isosid) from SO_SODetails
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
/****** Object:  StoredProcedure [dbo].[DLproc_OrderDetailModifyBySel]    Script Date: 2015-11-30 18:00:33 ******/
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

	--插入库存数据
	select cInvCode,cInvCName,sum(isnull(fAvailQtty,0)*0.7) 'fAvailQtty' into #temp from (
SELECT  CS.cExpirationdate as 有效期至,E1.enumname as 有效期推算方式,W.cWhCode, W.cWhName, I.cInvCode, I.cInvAddCode,  I.cInvName, I.cInvStd, I.cInvCCode , IC.cInvCName, 
 CU_M.cComUnitName AS cInvM_Unit, CASE WHEN I.iGroupType = 0 THEN NULL  WHEN I.iGrouptype = 2 THEN CU_A.cComUnitName  WHEN I.iGrouptype = 1 THEN CU_G.cComUnitName END  AS cInvA_Unit,convert(nvarchar(38),
  convert(decimal(38,2),CASE WHEN I.iGroupType = 0 THEN NULL      WHEN I.iGroupType = 2 THEN (CASE WHEN CS.iQuantity = 0.0 OR CS.iNum = 0.0 THEN NULL ELSE CS.iQuantity/CS.iNum END)      
 WHEN I.iGroupType = 1 THEN CU_G.iChangRate END)) AS iExchRate,
  Null as cInvDefine1, Null as cInvDefine2, Null as cInvDefine3, Null as cFree1, Null as cFree2, Null as cFree3, Null as cFree4, Null as cFree5, Null as cFree6, Null as cFree7, Null as cFree8, Null as cFree9, 
 Null as cFree10, Null as cInvDefine4, Null as cInvDefine5, Null as cInvDefine6, Null as cInvDefine7, Null as cInvDefine8, Null as cInvDefine9, Null as cInvDefine10, Null as cInvDefine11, Null as cInvDefine12, 
  Null as cInvDefine13, Null as cInvDefine14, Null as cInvDefine15, Null as cInvDefine16,cs.cBatch, cs.EnumName As iSoTypeName, cs.csocode as SOCode, cs.cdemandmemo,convert(nvarchar,cs.isoseq) as iRowNo,
cs.cvmivencode,v1.cvenabbname as cvmivenname , isnull(E.enumname,N'') as cMassUnitName,CS.dVDate, CS.dMdate,convert(varchar(20),CS.iMassDate) as iMassDate,
 (iQuantity) AS iQtty,( CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(iNum,0) WHEN iGroupType = 1 THEN iQuantity/ CU_G.iChangRate END) AS iNum,
  CASE WHEN CS.bStopFlag = 1 OR CS.bGspStop = 1 THEN iQuantity ELSE IsNull(fStopQuantity,0) END AS iStopQtty,
  CASE WHEN CS.bStopFlag = 1 OR CS.bGspStop = 1 THEN (CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(iNum,0) WHEN iGroupType = 1 THEN iQuantity/ CU_G.iChangRate END) 
 ELSE (CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(fStopNum,0) WHEN iGroupType = 1 THEN fStopQuantity/ CU_G.iChangRate END) END AS iStopNum,
 (fInQuantity) AS fInQtty, 
  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fInNum,0) WHEN iGroupType = 1 THEN fInQuantity/ CU_G.iChangRate END) AS fInNum,
 (fTransInQuantity) AS fTransInQtty,
  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fTransInNum,0) WHEN iGroupType = 1 THEN fTransInQuantity/ CU_G.iChangRate END) AS fTransInNum,
 (ISNULL(fInQuantity,0) + ISNULL(fTransInQuantity,0)) AS fInQttySum,
  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fInNum,0) + ISNULL(fTransInNum,0) WHEN iGroupType = 1 THEN (ISNULL(fInQuantity,0) + ISNULL(fTransInQuantity,0))/ CU_G.iChangRate END) AS fInNumSum,
 (fOutQuantity) AS fOutQtty, 
  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fOutNum,0) WHEN iGroupType = 1 THEN fOutQuantity/ CU_G.iChangRate END) AS fOutNum,
 CS.cBatchProperty1,CS.cBatchProperty2,CS.cBatchProperty3,CS.cBatchProperty4,CS.cBatchProperty5,CS.cBatchProperty6,CS.cBatchProperty7,CS.cBatchProperty8,CS.cBatchProperty9,CS.cBatchProperty10,
  (fTransOutQuantity) AS fTransOutQtty, 
 (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fTransOutNum,0) WHEN iGroupType = 1 THEN fTransOutQuantity/ CU_G.iChangRate END) AS fTransOutNum,
  (ISNULL(fOutQuantity,0) + ISNULL(fTransOutQuantity,0)) AS fOutQttySum , 
 (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fOutNum,0) + ISNULL(fTransOutNum,0) WHEN iGroupType = 1 THEN (ISNULL(fOutQuantity,0) + ISNULL(fTransOutQuantity,0))/ CU_G.iChangRate END) AS fOutNumSum,
  (fDisableQuantity) AS fDisableQtty, 
 (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fDisableNum,0) WHEN iGroupType = 1 THEN fDisableQuantity/ CU_G.iChangRate END) AS fDisableNum,
  (ipeqty) AS fpeqty, 
 (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(ipenum,0) WHEN iGroupType = 1 THEN ipeqty/ CU_G.iChangRate END) AS fpenum,
 (CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
 ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END) AS fAvailQtty,dLastCheckDate, 
 (CASE WHEN iGroupType = 0 THEN 0  WHEN iGroupType = 2 THEN  CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iNum,0)- IsNull(fStopNum,0) END  - ISNULL(fOutNum,0) 
 ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iNum,0)- IsNull(fStopNum,0) END  - ISNULL(fOutNum,0) END WHEN iGroupType = 1 THEN  (CASE WHEN bInvBatch=1 THEN  
 CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
 ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END)/CU_G.iChangRate ELSE NULL END) AS fAvailNum
 FROM v_ST_currentstockForReport  CS inner join dbo.Inventory I ON I.cInvCode = CS.cInvCode   
 left join dbo.InventoryClass IC ON IC.cInvCCode = I.cInvCCode LEFT OUTER JOIN dbo.ComputationUnit CU_G ON I.cSTComUnitCode =CU_G.cComUnitCode 
 LEFT OUTER JOIN dbo.ComputationUnit CU_A ON I.cAssComUnitCode = CU_A.cComunitCode 
 LEFT OUTER JOIN dbo.ComputationUnit CU_M ON I.cComUnitCode = CU_M.cComunitCode 
 LEFT OUTER JOIN dbo.Warehouse W ON CS.cWhCode = W.cWhCode 
 left join vendor v1 on v1.cvencode = cs.cvmivencode 
 left join v_aa_enum E1 on E1.enumcode = ISNULL(cs.iExpiratDateCalcu,0) and E1.enumtype=N'SCM.ExpiratDateCalcu' 
 LEFT OUTER JOIN dbo.v_aa_enum E with (nolock) on E.enumcode=convert(nchar,CS.cMassUnit) and E.enumType=N'ST.MassUnit' 
 WHERE  1=1  AND 1=1  
 ) as HH
 group by cInvCode,cInvCName

--读取数据	
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
t1.fAvailQtty 'Stock',
dod.kl 'kl',
dod.cunitid 'cComUnitCode',
dod.iTaxRate 'iTaxRate',
dod.cn1cComUnitName 'cn1cComUnitName'
 from Dl_opOrder do
inner join Dl_opOrderDetail dod on do.lngopOrderId=dod.lngopOrderId 
inner join Inventory iv on iv.cInvCode=dod.cinvcode
left join #temp t1 on t1.cInvCode=dod.cinvcode
where do.strBillNo =@strBillNo
order by irowno

END


GO
/****** Object:  StoredProcedure [dbo].[DLproc_OrderGetNewBillNoBySel]    Script Date: 2015-11-30 18:00:33 ******/
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
/****** Object:  StoredProcedure [dbo].[DLproc_OrderGetNewBillNoBySelU8]    Script Date: 2015-11-30 18:00:33 ******/
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
/****** Object:  StoredProcedure [dbo].[DLproc_QuasiOrderDetailBySel]    Script Date: 2015-11-30 18:00:33 ******/
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
	--U8可用量查询
select @fAvailQtty=(sum(fAvailQtty)*0.7)  from (
SELECT  CS.cExpirationdate as 有效期至,E1.enumname as 有效期推算方式,W.cWhCode, W.cWhName, I.cInvCode, I.cInvAddCode,  I.cInvName, I.cInvStd, I.cInvCCode , IC.cInvCName, 
 CU_M.cComUnitName AS cInvM_Unit, CASE WHEN I.iGroupType = 0 THEN NULL  WHEN I.iGrouptype = 2 THEN CU_A.cComUnitName  WHEN I.iGrouptype = 1 THEN CU_G.cComUnitName END  AS cInvA_Unit,convert(nvarchar(38),
  convert(decimal(38,2),CASE WHEN I.iGroupType = 0 THEN NULL      WHEN I.iGroupType = 2 THEN (CASE WHEN CS.iQuantity = 0.0 OR CS.iNum = 0.0 THEN NULL ELSE CS.iQuantity/CS.iNum END)      
 WHEN I.iGroupType = 1 THEN CU_G.iChangRate END)) AS iExchRate,
  Null as cInvDefine1, Null as cInvDefine2, Null as cInvDefine3, Null as cFree1, Null as cFree2, Null as cFree3, Null as cFree4, Null as cFree5, Null as cFree6, Null as cFree7, Null as cFree8, Null as cFree9, 
 Null as cFree10, Null as cInvDefine4, Null as cInvDefine5, Null as cInvDefine6, Null as cInvDefine7, Null as cInvDefine8, Null as cInvDefine9, Null as cInvDefine10, Null as cInvDefine11, Null as cInvDefine12, 
  Null as cInvDefine13, Null as cInvDefine14, Null as cInvDefine15, Null as cInvDefine16,cs.cBatch, cs.EnumName As iSoTypeName, cs.csocode as SOCode, cs.cdemandmemo,convert(nvarchar,cs.isoseq) as iRowNo,
cs.cvmivencode,v1.cvenabbname as cvmivenname , isnull(E.enumname,N'') as cMassUnitName,CS.dVDate, CS.dMdate,convert(varchar(20),CS.iMassDate) as iMassDate,
 (iQuantity) AS iQtty,( CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(iNum,0) WHEN iGroupType = 1 THEN iQuantity/ CU_G.iChangRate END) AS iNum,
  CASE WHEN CS.bStopFlag = 1 OR CS.bGspStop = 1 THEN iQuantity ELSE IsNull(fStopQuantity,0) END AS iStopQtty,
  CASE WHEN CS.bStopFlag = 1 OR CS.bGspStop = 1 THEN (CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(iNum,0) WHEN iGroupType = 1 THEN iQuantity/ CU_G.iChangRate END) 
 ELSE (CASE WHEN iGroupType = 0 THEN 0 WHEN iGroupType = 2 THEN ISNULL(fStopNum,0) WHEN iGroupType = 1 THEN fStopQuantity/ CU_G.iChangRate END) END AS iStopNum,
 (fInQuantity) AS fInQtty, 
  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fInNum,0) WHEN iGroupType = 1 THEN fInQuantity/ CU_G.iChangRate END) AS fInNum,
 (fTransInQuantity) AS fTransInQtty,
  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fTransInNum,0) WHEN iGroupType = 1 THEN fTransInQuantity/ CU_G.iChangRate END) AS fTransInNum,
 (ISNULL(fInQuantity,0) + ISNULL(fTransInQuantity,0)) AS fInQttySum,
  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fInNum,0) + ISNULL(fTransInNum,0) WHEN iGroupType = 1 THEN (ISNULL(fInQuantity,0) + ISNULL(fTransInQuantity,0))/ CU_G.iChangRate END) AS fInNumSum,
 (fOutQuantity) AS fOutQtty, 
  (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fOutNum,0) WHEN iGroupType = 1 THEN fOutQuantity/ CU_G.iChangRate END) AS fOutNum,
 CS.cBatchProperty1,CS.cBatchProperty2,CS.cBatchProperty3,CS.cBatchProperty4,CS.cBatchProperty5,CS.cBatchProperty6,CS.cBatchProperty7,CS.cBatchProperty8,CS.cBatchProperty9,CS.cBatchProperty10,
  (fTransOutQuantity) AS fTransOutQtty, 
 (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fTransOutNum,0) WHEN iGroupType = 1 THEN fTransOutQuantity/ CU_G.iChangRate END) AS fTransOutNum,
  (ISNULL(fOutQuantity,0) + ISNULL(fTransOutQuantity,0)) AS fOutQttySum , 
 (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fOutNum,0) + ISNULL(fTransOutNum,0) WHEN iGroupType = 1 THEN (ISNULL(fOutQuantity,0) + ISNULL(fTransOutQuantity,0))/ CU_G.iChangRate END) AS fOutNumSum,
  (fDisableQuantity) AS fDisableQtty, 
 (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(fDisableNum,0) WHEN iGroupType = 1 THEN fDisableQuantity/ CU_G.iChangRate END) AS fDisableNum,
  (ipeqty) AS fpeqty, 
 (CASE WHEN iGroupType = 0 THEN NULL WHEN iGroupType=2 THEN ISNULL(ipenum,0) WHEN iGroupType = 1 THEN ipeqty/ CU_G.iChangRate END) AS fpenum,
 (CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
 ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END) AS fAvailQtty,dLastCheckDate, 
 (CASE WHEN iGroupType = 0 THEN 0  WHEN iGroupType = 2 THEN  CASE WHEN bInvBatch=1 THEN  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iNum,0)- IsNull(fStopNum,0) END  - ISNULL(fOutNum,0) 
 ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iNum,0)- IsNull(fStopNum,0) END  - ISNULL(fOutNum,0) END WHEN iGroupType = 1 THEN  (CASE WHEN bInvBatch=1 THEN  
 CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) 
 ELSE  CASE WHEN bStopFlag =1 OR bGSPStop= 1 THEN 0 ELSE ISNULL(iQuantity,0)- IsNull(fStopQuantity,0) END  - ISNULL(fOutQuantity,0) END)/CU_G.iChangRate ELSE NULL END) AS fAvailNum
 FROM v_ST_currentstockForReport  CS inner join dbo.Inventory I ON I.cInvCode = CS.cInvCode   
 left join dbo.InventoryClass IC ON IC.cInvCCode = I.cInvCCode LEFT OUTER JOIN dbo.ComputationUnit CU_G ON I.cSTComUnitCode =CU_G.cComUnitCode 
 LEFT OUTER JOIN dbo.ComputationUnit CU_A ON I.cAssComUnitCode = CU_A.cComunitCode 
 LEFT OUTER JOIN dbo.ComputationUnit CU_M ON I.cComUnitCode = CU_M.cComunitCode 
 LEFT OUTER JOIN dbo.Warehouse W ON CS.cWhCode = W.cWhCode 
 left join vendor v1 on v1.cvencode = cs.cvmivencode 
 left join v_aa_enum E1 on E1.enumcode = ISNULL(cs.iExpiratDateCalcu,0) and E1.enumtype=N'SCM.ExpiratDateCalcu' 
 LEFT OUTER JOIN dbo.v_aa_enum E with (nolock) on E.enumcode=convert(nchar,CS.cMassUnit) and E.enumType=N'ST.MassUnit' 
 WHERE  1=1  AND 1=1   And I.cInvCode = @cinvcode
 ) as HH
	--网上下单系统已存在的未生成U8订单和未驳回修改的订单数量,这部分数量需要用U8数量减去
	select @fAvailQtty=@fAvailQtty-isnull(sum(iquantity),0)  from Dl_opOrderDetail dod
inner join Dl_opOrder do on do.lngopOrderId=dod.lngopOrderId
where dod.cinvcode=@cinvcode and do.bytStatus in (1,4)
if @fAvailQtty<0 
begin
set @fAvailQtty=0
end
 	/*END,11-01,添加获取库存可用量查询*/
select it.cInvName,it.cInvStd,cn.cComUnitName,it.cInvCode ,isnull(cn.iChangRate,1) 'iChangRate',isnull(cn1.cComUnitName,' ') 'cSAComUnit'
,@p17 'nOriginalPrice',isnull(@p19,0) 'BeforeExercisePrice', @p20 'Rate',isnull(@fAvailQtty,0) 'fAvailQtty',@bOnsale 'bOnsale',it.cComUnitCode,it.iTaxRate,cn1.cComUnitName 'cn1cComUnitName',
@p17*@p20/100 'ExercisePrice',@nOriginalPrice 'Quote'
from Inventory it
--inner join ComputationGroup cg on it.cGroupCode=cg.cGroupCode
inner join computationunit cn on it.cGroupCode=cn.cGroupCode
left join computationunit cn1 on cn1.cComunitCode=it.cSAComUnitCode
where it.cInvCode=@cinvcode
order by cn.iChangRate

--exec DLproc_QuasiOrderDetailBySel '01010100101','010101'
--exec DLproc_QuasiOrderDetailBySel '32020103','010101'
--exec DLproc_QuasiOrderDetailBySel '020101001','010101'

END


GO
/****** Object:  StoredProcedure [dbo].[DLproc_TreeListDetailsBySel]    Script Date: 2015-11-30 18:00:33 ******/
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

select cInvName,isnull(cInvStd,' ') 'cInvStd',cn.cComUnitName,cInvCode 
from 
(SELECT I.cInvName,I.cInvStd,I.cInvCode,I.cComUnitCode,I.cInvCCode from Inventory I  left JOIN SA_CusInvLimited L  on I.cinvcode=l.cInvCode 
and  L.ccuscode=@cCusCode and L.blimited=0 where l.cInvCode is null and (I.cInvCode LIKE '01%' or I.cInvCode LIKE '02%' or I.cInvCode LIKE '32%')) ic 
 left join computationunit cn on ic.cComUnitCode=cn.cComunitCode 
WHERE (ic.cInvCCode = @cInvCCode)


END

--SELECT * from Inventory I  left JOIN SA_CusInvLimited L  on I.cinvcode=l.cInvCode 
--and  L.ccuscode='010102' and L.blimited=0 where l.cInvCode is null and (I.cInvCode LIKE '01%' or I.cInvCode LIKE '02%' or I.cInvCode LIKE '32%') 


GO
/****** Object:  StoredProcedure [dbo].[DLproc_UnauditedOrderBySel]    Script Date: 2015-11-30 18:00:33 ******/
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
/****** Object:  StoredProcedure [dbo].[DLproc_UnauditedOrderManagersBySel]    Script Date: 2015-11-30 18:00:33 ******/
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
@strManagers varchar(100) 
AS

BEGIN

	SET NOCOUNT ON;
	
	select do.strBillNo,du.strUserName,do.ccusname,cdefine11,strRemarks,convert(varchar(10),do.datCreateTime,120) 'datCreateTime' from Dl_opOrder do
	inner join Dl_opUser  du on do.lngopUserId=du.lngopUserId
	where do.bytStatus='1' and do.strManagers = @strManagers 
	order by do.strBillNo desc

END


GO
/****** Object:  StoredProcedure [dbo].[DLproc_UserAddressPSBySel]    Script Date: 2015-11-30 18:00:33 ******/
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
WHERE ([lngopUserId] = @lngopUserId) and [strDistributionType]='配送'

END

GO
/****** Object:  StoredProcedure [dbo].[DLproc_UserAddressPSBySelGroup]    Script Date: 2015-11-30 18:00:33 ******/
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
WHERE ([lngopUserId] = @lngopUserId) and [strDistributionType]='配送'

END

GO
/****** Object:  StoredProcedure [dbo].[DLproc_UserAddressZTBySel]    Script Date: 2015-11-30 18:00:33 ******/
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
WHERE ([lngopUserId] = @lngopUserId) and [strDistributionType]='自提'



END

GO
/****** Object:  StoredProcedure [dbo].[DLproc_UserAddressZTBySelGroup]    Script Date: 2015-11-30 18:00:33 ******/
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
WHERE ([lngopUserId] = @lngopUserId) and [strDistributionType]='自提'

END

GO
