

--更新U8中的顾客电话号码
select * from UFDATA_003_2015.dbo.Customer
select * from Customer
--update Customer set cCusPhone=bb.cCusPhone from Customer aa left join UFDATA_003_2015.dbo.Customer bb on aa.cCusCode=bb.cCusCode 
select cCusCode,cCusName,cCusPhone from Customer

--更新用户信息Dl_opUser
--更新Dl_opOrderBillNoSetting
--更新Dl_opPreOrderControl
--更新Dl_opQQService
--更新Dl_opSOASetting
--更新Dl_opSOASettingDetail
--更新Dl_opSystemConfiguration
--更新

select * from Dl_opUser
select * from Dl_opOrder