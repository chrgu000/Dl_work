using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using BLL;
using System.Text;

/// <summary>
/// Check 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class Check : System.Web.Services.WebService {
    BLL.Check ck = new BLL.Check();
    public Check () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
      
     

    }

    /// <summary>
    /// 检测网上订单与U8销售订单产品数量及总金额是否相同
    /// </summary>
    /// <param name="cSOCode">U8订单号</param>
    /// <returns></returns>
    [WebMethod(Description = "验证网单产品数量及购买量及各项金额与生成的U8订单是否相符")]
   
    public string Check_OpOrder_Num(string cSOCode) {
     
       return  ck.Check_OpOrder_Num(cSOCode);
      
    }
    
}
