axios.defaults.baseURL = '/handler/productHandler.ashx';
var vm = new Vue({
    el: "#vm",
    data: {
        dateRange: ["", ""],
        tableData: [],
        tableColumn: [

            {
                title: '序号',
                width: 70,
                align: "center",
                // fixed: 'left',
                render: (h, params) => {
                    return h('span', params.index + 1);
                }
            },

            {
                title: '订单编号',
                key: 'strBillNo',
                width: 150,
                align: "center"
            }, {
                title: '产品名称',
                key: 'cInvName',
                width: 250,
                align: "center"
            }, {
                title: '产品规格',
                key: 'cInvStd',
                width: 150,
                align: "center"
            }, {
                title: '订单数量',
                key: 'iQuantity_order',
                width: 140,
                align: "center",
                render: function(h, params) {
                    if (params.row.iQuantity_order != "") {
                        return h('span', Number(params.row.iQuantity_order).toFixed(2));
                    }
                }
            },
            {
                title: '提货数量',
                key: 'iQuantity_really',
                width: 140,
                align: "center",
                render: function(h, params) {
                    if (params.row.iQuantity_really != "") {
                        return h('span', Number(params.row.iQuantity_really).toFixed(2));
                    }
                }
            }, {
                title: '信用分数',
                key: 'Point',
                width: 130,
                align: "center",
                render: function(h, params) {
                    var color = "";
                    if (params.row.Flag == "+") {
                        color = "red";
                    } else {
                        color = "green";
                    }
                    return h('Tag', {
                            props: {
                                color: color,

                            },
                            style: {
                                width: '50px'
                            }
                        },
                        params.row.Point);
                }
            }, {
                title: '日期',
                key: 'datCreateTime',
                width: 180,
                align: "center"
            },
            {
                title: '备注',
                key: 'cRemark',
                width: 400,
                align: "center",
                render: (h, params) => {
                    return h('span', params.row.cRemark)
                }
            },

        ]
    },
    methods: {
        GetUserBehavior() {
            var _this = this;
            axios({
                method: 'post',
                data: {
                    "Action": "GetUserBehavior",
                    "startDate": _this.dateRange[0],
                    "endDate": _this.dateRange[1]
                }
            }).then(function(res) {
                if (res) {
                    _this.tableData = [];
                    _this.tableData = res.data;

                }

            })
        },
        handleDateRange(val) {
            this.dateRange = val;

        },

    },
    created() {
        this.GetUserBehavior();
    },
    computed: {
        score() {

            if (this.tableData.length == 0) {
                 console.log(this.tableData.length);
                return 0;
            } else {

                var s=0;
                this.tableData.forEach((item) => {
                     s+=item["Point"]
                })

                return s.toFixed(2);
            }

        }
    }
 
})