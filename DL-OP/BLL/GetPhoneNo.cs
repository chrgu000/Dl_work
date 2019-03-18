using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL;
using Model;


namespace BLL
{
    public class GetPhoneNo
    {
        /// <summary>
        /// 获取顾客手机号码(Id,PhoneNo)
        /// </summary>
        /// <param name="cCusCode">顾客登录编码</param>
        /// <returns>返回顾客电话号码(Id,PhoneNo)</returns>
        public DataTable GetCustomerPhoneNo(string cCusCode)
        {
            //建立手机号码datatable
            DataTable phone = new DataTable();
            //创建table的第一列
            DataColumn Id = new DataColumn();
            //该列的数据类型
            Id.DataType = System.Type.GetType("System.Int32");
            //该列得名称
            Id.ColumnName = "Id";
            //该列得默认值
            Id.DefaultValue = 999;
            //创建table的第二列
            DataColumn PhoneNo = new DataColumn();
            //该列的数据类型
            PhoneNo.DataType = System.Type.GetType("System.String");
            //该列得名称
            PhoneNo.ColumnName = "PhoneNo";
            //该列得默认值
            PhoneNo.DefaultValue = "";
            // 将所有的列添加到table上
            phone.Columns.Add(Id);
            phone.Columns.Add(PhoneNo);

            //获取顾客手机号码
            //string username = cCusCode;
            DataTable pp = new LoginManager().GetPhoneNo(cCusCode);
            if (pp.Rows.Count>0 )    //如果有手机号码存在,分割
            {
                if (pp.Rows[0]["cCusPhone"].ToString() != " ")
                {
                    string s = pp.Rows[0]["cCusPhone"].ToString();
                    string[] sArray = s.Split(';');
                    //foreach (string i in sArray)
                    //Console.WriteLine(i.ToString());
                    for (int i = 0; i < sArray.Length; i++)
                    {
                        //创建一行
                        DataRow row = phone.NewRow();
                        //将此行添加到table中
                        phone.Rows.Add(i + 1, sArray[i].ToString());
                        //if (i==9) //限制电话号码个数
                        //{
                        //    i = 999;
                        //}
                    }
                }             
            }

            return phone;
        }


        //public bool Update

        public DataTable GetSubCustomerPhoneNo(string lngopUserId, string lngopUserExId)
        {
            //建立手机号码datatable
            DataTable phone = new DataTable();
            //创建table的第一列
            DataColumn Id = new DataColumn();
            //该列的数据类型
            Id.DataType = System.Type.GetType("System.Int32");
            //该列得名称
            Id.ColumnName = "Id";
            //该列得默认值
            Id.DefaultValue = 999;
            //创建table的第二列
            DataColumn PhoneNo = new DataColumn();
            //该列的数据类型
            PhoneNo.DataType = System.Type.GetType("System.String");
            //该列得名称
            PhoneNo.ColumnName = "PhoneNo";
            //该列得默认值
            PhoneNo.DefaultValue = "";
            // 将所有的列添加到table上
            phone.Columns.Add(Id);
            phone.Columns.Add(PhoneNo);
            DataTable pp = new LoginManager().GetOrderSubPhoneNo(lngopUserId, lngopUserExId);           
            if (pp.Rows.Count > 0)    //如果有手机号码存在,分割
            {
                for (int k = 0; k < pp.Rows.Count; k++)
                {
                    if (pp.Rows[k]["cCusPhone"].ToString() != " ")
                    {
                        string s = pp.Rows[k]["cCusPhone"].ToString();
                        string[] sArray = s.Split(';');
                        //foreach (string i in sArray)
                        //Console.WriteLine(i.ToString());
                        for (int i = 0; i < sArray.Length; i++)
                        {
                            int hh = phone.Rows.Count;
                            //创建一行
                            DataRow row = phone.NewRow();
                            //将此行添加到table中
                            phone.Rows.Add(hh + 1, sArray[i].ToString());
                            //if (i==9) //限制电话号码个数
                            //{
                            //    i = 999;
                            //}
                        }
                    } 
                }
                
            }

            return phone;
        }



    }
}
