using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace DAL
{
    public class DbDao
    {
        #region 根据数据库字段类型获取SqlDbType类型
        public   SqlDbType Get_SqlDbType(string sqlTypeString)
        {
            SqlDbType dbType = SqlDbType.Variant;//默认为Object

            switch (sqlTypeString)
            {
                case "int":
                    dbType = SqlDbType.Int;
                    break;
                case "varchar":
                    dbType = SqlDbType.VarChar;
                    break;
                case "bit":
                    dbType = SqlDbType.Bit;
                    break;
                case "datetime":
                    dbType = SqlDbType.DateTime;
                    break;
                case "decimal":
                    dbType = SqlDbType.Decimal;
                    break;
                case "float":
                    dbType = SqlDbType.Float;
                    break;
                case "image":
                    dbType = SqlDbType.Image;
                    break;
                case "money":
                    dbType = SqlDbType.Money;
                    break;
                case "ntext":
                    dbType = SqlDbType.NText;
                    break;
                case "nvarchar":
                    dbType = SqlDbType.NVarChar;
                    break;
                case "smalldatetime":
                    dbType = SqlDbType.SmallDateTime;
                    break;
                case "smallint":
                    dbType = SqlDbType.SmallInt;
                    break;
                case "text":
                    dbType = SqlDbType.Text;
                    break;
                case "bigint":
                    dbType = SqlDbType.BigInt;
                    break;
                case "binary":
                    dbType = SqlDbType.Binary;
                    break;
                case "char":
                    dbType = SqlDbType.Char;
                    break;
                case "nchar":
                    dbType = SqlDbType.NChar;
                    break;
                case "numeric":
                    dbType = SqlDbType.Decimal;
                    break;
                case "real":
                    dbType = SqlDbType.Real;
                    break;
                case "smallmoney":
                    dbType = SqlDbType.SmallMoney;
                    break;
                case "sql_variant":
                    dbType = SqlDbType.Variant;
                    break;
                case "timestamp":
                    dbType = SqlDbType.Timestamp;
                    break;
                case "tinyint":
                    dbType = SqlDbType.TinyInt;
                    break;
                case "uniqueidentifier":
                    dbType = SqlDbType.UniqueIdentifier;
                    break;
                case "varbinary":
                    dbType = SqlDbType.VarBinary;
                    break;
                case "xml":
                    dbType = SqlDbType.Xml;
                    break;
                case "time":
                    dbType = SqlDbType.Time;
                    break;
                case "date":
                    dbType = SqlDbType.Date;
                    break;
            }
            return dbType;
        }
        #endregion

        #region 判断SqlType是否为数字类型，如果是的话需要将空值设置为0
        public bool IsNumber(string sqlType) {
            bool b = false;
            string[] SqlTypeNames = new string[] { "int", "decimal","float", "money","smallint" ,"bigint" ,"numeric","real","smallmoney",  "tinyint"};
              int id = Array.IndexOf(SqlTypeNames,sqlType);
            if (id!=-1)
            {
                b = true;
            }
            return b;
        }
        #endregion


        #region 将SQLServer数据类型（如：varchar）转换为.Net类型（如：String）
        /// 将SQLServer数据类型（如：varchar）转换为.Net类型（如：String） 
        /// </summary> 
        /// <param name="sqlTypeString">Sql server的数据类型</param> 
        /// <returns>相对应的C#数据类型</returns> 
        public   string SqltoCsharpT(string sqlType)
        {
            string[] SqlTypeNames = new string[] { "int", "varchar","bit" ,"datetime","decimal","float","image","money",
"ntext","nvarchar","smalldatetime","smallint","text","bigint","binary","char","nchar","numeric",
"real","smallmoney", "sql_variant","timestamp","tinyint","uniqueidentifier","varbinary"};

            string[] CSharpTypes = new string[] {"int", "string","bool" ,"DateTime","Decimal","Double","Byte[]","Single",
"string","string","DateTime","Int16","string","Int64","Byte[]","string","string","Decimal",
"Single","Single", "Object","Byte[]","Byte","Guid","Byte[]"};

            int i = Array.IndexOf(SqlTypeNames, sqlType.ToLower());

            return CSharpTypes[i];
        }
        #endregion
    }
}
