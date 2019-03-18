using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace U8API.Entity
{
    public class API10Hd
    {
        public Nullable<Int32> id { get; set; } //主关键字段，int类型
        public String ccode { get; set; } //入库单号，string类型
        public Nullable<DateTime> ddate { get; set; } //入库日期，DateTime类型
        public String cwhname { get; set; } //仓库，string类型
        public String csysbarcode { get; set; } //单据条码，string类型
        public String chinvsn { get; set; } //序列号，string类型
        public String cmodifyperson { get; set; } //修改人，string类型
        public Nullable<DateTime> dmodifydate { get; set; } //修改日期，DateTime类型
        public Nullable<DateTime> dnmaketime { get; set; } //制单时间，DateTime类型
        public Nullable<DateTime> dnmodifytime { get; set; } //修改时间，DateTime类型
        public Nullable<DateTime> dnverifytime { get; set; } //审核时间，DateTime类型
        public Nullable<DateTime> dchkdate { get; set; } //检验日期，DateTime类型
        public String iavaquantity { get; set; } //可用量，string类型
        public String iavanum { get; set; } //可用件数，string类型
        public String ipresentnum { get; set; } //现存件数，string类型
        public String ufts { get; set; } //时间戳，string类型
        public String cpspcode { get; set; } //产品，string类型
        public String iproorderid { get; set; } //生产订单ID，string类型
        public String cmpocode { get; set; } //生产订单号，string类型
        public String cprobatch { get; set; } //生产批号，string类型
        public String iverifystate { get; set; } //iverifystate，string类型
        public String iswfcontrolled { get; set; } //iswfcontrolled，string类型
        public String ireturncount { get; set; } //ireturncount，string类型
        public String cdepname { get; set; }//部门，string类型
        public String crdname { get; set; }//入库类别，string类型
        public Nullable<DateTime> dveridate { get; set; } //审核日期，DateTime类型
        public String cmemo { get; set; } //备注，string类型
        public String cchkperson { get; set; } //检验员，string类型
        public String cmaker { get; set; } //制单人，string类型
        public String chandler { get; set; } //审核人，string类型
        public String itopsum { get; set; } //最高库存量，string类型
        public String caccounter { get; set; } //记账人，string类型
        public String ilowsum { get; set; } //最低库存量，string类型
        public String ipresent { get; set; } //现存量，string类型
        public String isafesum { get; set; } //安全库存量，string类型
        public String cbustype { get; set; } //业务类型，int类型
        public String cpersonname { get; set; } //业务员，string类型
        public String cdefine1 { get; set; } //表头自定义项1，string类型
        public String cdefine11 { get; set; } //表头自定义项11，string类型
        public String cdefine12 { get; set; } //表头自定义项12，string类型
        public String cdefine13 { get; set; } //表头自定义项13，string类型
        public String cdefine14 { get; set; } //表头自定义项14，string类型
        public String cdefine2 { get; set; } //表头自定义项2，string类型
        public String cdefine3 { get; set; } //表头自定义项3，string类型
        public String csource { get; set; } //单据来源，int类型
        public String cdefine5 { get; set; } //表头自定义项5，int类型
        public String cdefine15 { get; set; } //表头自定义项15，int类型
        public Nullable<DateTime> cdefine6 { get; set; } //表头自定义项6，DateTime类型
        public String brdflag { get; set; } //收发标志，string类型
        public Nullable<Double> cdefine7 { get; set; } //表头自定义项7，double类型
        public Nullable<Double> cdefine16 { get; set; } //表头自定义项16，double类型
        public String cdefine8 { get; set; } //表头自定义项8，string类型
        public String cdefine9 { get; set; } //表头自定义项9，string类型
        public String cdefine10 { get; set; } //表头自定义项10，string类型
        public String cvouchtype { get; set; } //单据类型，string类型
        public String cwhcode { get; set; }//仓库编码，string类型
        public String crdcode { get; set; }// 入库类别编码，string类型
        public String cdepcode { get; set; }//部门编码，string类型
        public String cpersoncode { get; set; } //业务员编码，string类型
        public Nullable<Int32> vt_id { get; set; }//模版号，int类型
        public Nullable<DateTime> cdefine4 { get; set; } //表头自定义项4，DateTime类型
    }
}
