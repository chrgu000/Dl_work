using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Model
{
    public class Buy_list
    {

        public int irowno { get; set; }
        public string cinvcode { get; set; }
        public string cinvname { get; set; }
        public double iquantity { get; set; }
        public double iquotedprice { get; set; }
        public double kl { get; set; }

        public double MinOrderQTY { get; set; }

        public string cComUnitName { get; set; }
        public string cInvDefine1 { get; set; }
        public string cInvDefine2 { get; set; }
        public double cInvDefine13 { get; set; }
        public double cInvDefine14 { get; set; }
        public string unitGroup { get; set; }
        public double cComUnitQTY { get; set; }
        public double cInvDefine2QTY { get; set; }
        public double cInvDefine1QTY { get; set; }
        public double itaxunitprice { get; set; }
        public string cunitid { get; set; }
        public string cDefine22 { get; set; }

        public double itaxrate { get; set; }

        public string temp_name { get; set; }

 
        public string itemid { get; set; }


        public Buy_list()
        {
            this.cComUnitName = cComUnitName;
            this.cComUnitQTY = cComUnitQTY;
            this.cDefine22 = cDefine22;
            this.cinvcode = cinvcode;
            this.cInvDefine1 = cInvDefine1;
            this.cInvDefine13 = cInvDefine13;
            this.cInvDefine14 = cInvDefine14;
            this.cInvDefine1QTY = cInvDefine1QTY;
            this.cInvDefine2 = cInvDefine2;
            this.cInvDefine2QTY = cInvDefine2QTY;
            this.cinvname = cinvname;
            this.cunitid = cunitid;
            this.itaxrate = itaxrate;
            this.temp_name = temp_name;
            this.MinOrderQTY = MinOrderQTY;
            this.itemid = itemid;
      
        }



    }

    public class ReInfo
    {
        public string message { get; set; }

        public string flag { get; set; }

        public List<string[]> messages { get; set; }

        public string[] msg { get; set; }

        public DataTable datatable { get; set; }

        public DataTable dt { get; set; }

        public List<DataTable> list_dt { get; set; }

        public List<string> list_msg { get; set; }

        public DataTable kpdw_dt { get; set; }
        public DataTable CusCredit_dt { get; set; }
        public DataTable CarType_dt { get; set; }
        public DataTable WareHouse_dt { get; set; }

        public List<string> limit_code { get; set; } //限销商品编码

        public List<string> limit_name { get; set; } //限销商品名称

        public string cWhCode { get; set; } //仓库名称
        public ReInfo()
        {
            this.flag = flag;

            this.messages = messages;

            this.message = message;

            this.msg = msg;

            this.datatable = datatable;

            this.dt = dt;

            this.list_dt = list_dt;

            this.list_msg = list_msg;

            this.CarType_dt = CarType_dt;

            this.CusCredit_dt = CusCredit_dt;

            this.kpdw_dt = kpdw_dt;

            this.limit_code = limit_code;

            this.limit_name = limit_name;

            this.WareHouse_dt = WareHouse_dt;

            this.cWhCode = cWhCode;
        }
    }

#region 新增特殊订单表头（用于从前台传过来的表头数据实例化）
    public class form_Data  
    {
        public string ccuscode { get; set; }  //开票单位ID

        public string csccode { get; set; }  //送货方式
        public string lngopUseraddressId { get; set; } //送货地址ID

        public string txtArea { get; set; }   //行政区ID

        public string strRemarks { get; set; } //备注

        public string datDeliveryDate { get; set; }  //提货时间

        public string strLoadingWays { get; set; } //装车方式

        public string carType { get; set; } //车型

        public string ccuspperson { get; set; } //业务员

        public string txtAddress { get; set; } //地址信息

        public string ccusname { get; set; } //客户名称


        public string areaid { get; set; } //行政区编码

        public string iaddresstype { get; set; } //地址类型，1为自提，2为配送，3为自提需配送

        public string chdefine21 { get; set; }

        public string cWhCode { get; set; } //仓库编号
        public form_Data()
        {


            this.ccuscode = ccuscode;

            this.csccode = csccode;

            this.lngopUseraddressId = lngopUseraddressId;

            this.txtArea = txtArea;

            this.strRemarks = strRemarks;

            this.datDeliveryDate = datDeliveryDate;

            this.strLoadingWays = strLoadingWays;

            this.carType = carType;

            this.ccuspperson = ccuspperson;

            this.txtAddress = txtAddress;

            this.ccusname = ccusname;

            this.areaid = areaid;

            this.iaddresstype = iaddresstype;

            this.chdefine21 = chdefine21;

            this.cWhCode = cWhCode;
        }
    }
#endregion
}

