axios.defaults.baseURL = '/handler/productHandler.ashx';
var vm = new Vue({
    el: "#vm",
    data: {
        XOrderList: [],
        addressList: [],
        carTypeList: [],
        changeRow: {},
        selectValue: "31",
        updateModelVisible: false,
        cancelModelVisible: false,
        formItem: {
            addressValue: "",
            carTypeValue: "",
            strLoadingWays: "",
            strRemarks: "",
        },
        tableColumn: [

            {
                title: '序号',
                width: 70,
                align: "center",
                fixed: 'left',
                render: (h, params) => {
                    return h('span', params.index + 1);
                }
            },
            {
                title: '操作',
                width: 180,
                align: "center",
                fixed: 'left',
                render: (h, params) => {
                    return h('div', [
                        h('Button', {
                            props: {
                                type: 'info',
                                size: 'small'
                            },
                            style: {
                                marginRight: '5px'
                            },
                            on: {
                                click: () => {
                                    vm.showUpdateModel(params)
                                }
                            }
                        }, '编辑货运信息'),
                        h('Button', {
                            props: {
                                type: 'error',
                                size: 'small'
                            },
                            style: {
                                marginRight: '5px'
                            },
                            on: {
                                click: () => {
                                    vm.showCancelModel(params)
                                }
                            }
                        }, '关闭')

                    ])
                    if (params.row.xOrderStatus == 31) {
                        return h('div', [
                            h('Button', {
                                props: {
                                    type: 'info',
                                    size: 'small'
                                },
                                style: {
                                    marginRight: '5px'
                                },
                                on: {
                                    click: () => {
                                        vm.showUpdateModel(params)
                                    }
                                }
                            }, '编辑货运信息'),

                        ])
                    } else if (params.row.xOrderStatus == 11) {
                        return h('div', [
                            h('Button', {
                                props: {
                                    type: 'error',
                                    size: 'small'
                                },
                                style: {
                                    marginRight: '5px'
                                },
                                on: {
                                    click: () => {
                                        vm.showCancelModel(params)
                                    }
                                }
                            }, '关闭')
                        ])
                    }
                }
            },
            {
                title: '订单编号',
                key: 'strBillNo',
                width: 130,
                align: "center"
            },
            {
                title: '订单状态',
                key: 'xOrderStatus',
                width: 100,
                align: "center",
                render: (h, params) => {
                    var s = params.row.xOrderStatus;
                    switch (s) {
                        case 1:
                            return h('Tag', {
                                props: {
                                    color: "pink"
                                }
                            }, "待审核");
                            break;
                        case 11:
                            return h('Tag', {
                                props: {
                                    color: "#006699"
                                }
                            }, "审核中");
                            break;
                        case 21:
                            return h('Tag', {
                                props: {
                                    color: "#006699"
                                }
                            }, "生产中");
                            break;
                        case 31:
                            return h('Tag', {
                                props: {
                                    color: "green"
                                }
                            }, "可提货");
                            break;
                        case 41:
                            return h('Tag', {
                                props: {
                                    color: "blue"
                                }
                            }, "已完成");
                            break;
                        case 99:
                            return h('Tag', {
                                props: {
                                    color: "red"
                                }
                            }, "已关闭");
                            break;
                        default:
                            return h('Tag', {
                                props: {
                                    color: "red"
                                }
                            }, params.row.xOrderStatus);
                    }
                }
            },
            {
                title: '开票单位',
                key: 'ccusname',
                width: 180,
                align: "center"
            },
            {
                title: '下单账号',
                key: 'strAllAcount',
                width: 100,
                align: "center"
            },
            {
                title: '产品名称',
                key: 'cInvName',
                width: 180,
                align: "center"
            },
            {
                title: '产品规格',
                key: 'cInvStd',
                width: 100,
                align: "center"
            },
            {
                title: '包装数量',
                key: 'cdefine22',
                width: 90,
                align: "center"
            },
            {
                title: '产品数量',
                key: 'iQuantity',
                width: 90,
                align: "center"
            },
            {
                title: '产品单价',
                key: 'iTaxUnitPrice',
                width: 90,
                align: "center"
            },
            {
                title: '金额',
                key: 'iSum',
                width: 90,
                align: "center"
            },
            {
                title: '货运信息',
                key: 'cdefine11',
                width: 240,
                align: "center",
                render: (h, params) => {
                    if (params.row.cdefine11.length > 20) {
                        return h('div', {
                            attrs: {
                                title: params.row.cdefine11
                            }
                        }, params.row.cdefine11.substring(0, 20) + "....")
                    } else {
                        return h('div', params.row.cdefine11)
                    }
                }
            },
            {
                title: '备注',
                key: 'strRemarks',
                width: 240,
                align: "center",
                render: (h, params) => {
                    if (params.row.strRemarks.length > 20) {
                        return h('div', {
                            attrs: {
                                title: params.row.strRemarks
                            }
                        }, params.row.strRemarks.substring(0, 20) + "......")
                    } else {
                        return h('div', params.row.strRemarks)
                    }
                }
            },
                        {
                title: '关闭原因',
                key: 'strRejectRemarks',
                width: 240,
                align: "center",
                render: (h, params) => {
                    if (params.row.strRejectRemarks.length > 20) {
                        return h('div', {
                            attrs: {
                                title: params.row.strRejectRemarks
                            }
                        }, params.row.strRejectRemarks.substring(0, 20) + "......")
                    } else {
                        return h('div', params.row.strRejectRemarks)
                    }
                }
            }
        ]
    },
    methods: {
        GetXOrderList() {
            var _this = this;
            axios({
                method: 'post',
                data: {
                    "Action": "GetXOrderList",
                    "status": _this.selectValue
                }
            }).then(function(res) {
                if (res) {
                    _this.XOrderList = res.data.XOrderList;
                    _this.addressList = res.data.AddressList;
                    _this.carTypeList = res.data.CarTypeList;
                }

            })
        },
        GetAddressByType() {
            var _this = this;
            axios({
                method: 'post',
                data: {
                    "Action": "GetAddressByType",
                    "AddressType": "1"
                }
            }).then(function(res) {
                console.log(res);
                _this.addressData = res.DataSet.address_dt;
            })
        },
        showUpdateModel(val) {
            this.changeRow = val.row;
            this.formItem.addressValue = Number(val.row.lngopUseraddressId);
            this.formItem.carTypeValue = val.row.cdefine3;
            this.formItem.strLoadingWays = val.row.strLoadingWays;
            this.formItem.strRemarks = val.row.strRemarks;
            this.updateModelVisible = true;

        },
        showCancelModel(val) {
            this.changeRow = val.row;
            this.cancelModelVisible = true;
        },
        closeModel() {
            this.updateModelVisible = false;
            this.cancelModelVisible = false;
        },
        updateInfo() {
            if (this.formItem.strLoadingWays.length > 100) {
                this.$Message.error({
                    content: "装车方式不能超过100个字",
                    duration: "10",
                    closable: true
                });
                return false;
            }
            if (this.formItem.strRemarks.length > 200) {
                this.$Message.error({
                    content: "备注不能超过200个字",
                    duration: "10",
                    closable: true
                });
                return false;
            }
            var _this = this;
            axios({
                method: 'post',
                data: {
                    "Action": "UpdateXOrderAddress",
                    "strBillNo": _this.changeRow.strBillNo,
                    "addressId": _this.formItem.addressValue,
                    "carTypeValue": _this.formItem.carTypeValue,
                    "strLoadingWays": _this.formItem.strLoadingWays,
                    "strRemarks": _this.formItem.strRemarks,

                }
            }).then(function(res) {
                if (!res) {
                    return false;
                }
                _this.updateModelVisible = false;
                _this.$Message.success({
                    content: '需求订单:' + _this.changeRow.strBillNo + "车辆信息更改成功",
                    closable: true,
                    duration: 10
                })
                _this.changeRow.lngopUseraddressId = _this.formItem.addressValue;
                _this.changeRow.cdefine3 = _this.formItem.carTypeValue;
                _this.changeRow.strLoadingWays = _this.formItem.strLoadingWays;
                _this.changeRow.strRemarks = _this.formItem.strRemarks;
                _this.addressList.forEach((item) => {
                    if (item.lngopUseraddressId == _this.formItem.addressValue) {
                        _this.changeRow.cdefine11 = '自提' + ',' + item.strCarplateNumber + ',' + item.strDriverName + ',' + item.strDriverTel + ',' + item.strIdCard;
                        return;
                    }
                })

            })
        },
        cancelXOrder() {

            var _this = this;
            axios({
                method: 'post',
                data: {
                    "Action": "CancelXOrder",
                    "strBillNo": _this.changeRow.strBillNo
                }
            }).then(function(res) {
                _this.cancelModelVisible = false;
                if (!res) {
                    return false;
                }

                _this.$Message.success({
                    content: '需求订单:' + _this.changeRow.strBillNo + "关闭成功",
                    closable: true,
                    duration: 10
                })
                _this.changeRow.xOrderStatus = 99;

            })
        }
    },
    created() {
        this.GetXOrderList();
    }
})