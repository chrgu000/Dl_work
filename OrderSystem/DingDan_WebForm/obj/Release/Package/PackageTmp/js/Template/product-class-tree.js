Vue.component('product-class-tree', {
    template: `
    <div>
      <Row :gutter="16">
                <i-col span="6" style='border: 1px solid #eee'>
                   <Tree :data='treeData' @on-select-change="handleNodeClick"></Tree>
                </i-col>
                <i-col span="18">
         
<Table stripe border :data="selectTableData"  :columns='tableColumn' :height="tableHeight"
 @on-row-click="handleTableRowClick" ref="selectTable" @on-select="getSelectedVal" @on-select-cancel="getCancelVal"></Table>
                </i-col>
            </Row>
          <br><br>
            <Row>
 <Col span="8" offset="16"> 
<Button type="primary" @click='sendSelectedProData'>确定</Button>
 </Col>
            </Row>
    </div>
    `,
    props: ['treeData', 'tableHeight'],
    data() {
        return {
            selectTableData: [],
            selectedProData: ['02010100114', '02010100202', '02010100201'],
            tableColumn: [{
                    type: 'selection',
                    width: 60,
                    align: 'center'
                },
                {
                    title: '序号',
                    width: 80,
                    align: "center",
                    render: (h, params) => {
                        return h('span', params.index + 1);
                    }
                },
                {
                    title: '产品名称',
                    key: 'cInvName',
                    align: "center"
                },
                {
                    title: '规格',
                    key: 'cInvStd',
                    width: 180,
                    align: "center"
                },
                {
                    title: '单位',
                    key: 'cComUnitName',
                    width: 180,
                    align: "center"
                }
            ]
        }
    },
    computed: {

    },
    methods: {
        sendSelectedProData() {
            Bus.$emit('sendSelectedProData', this.selectedProData);
        },
        handleNodeClick(d) {
            var _this = this;
            let id = d[0]['id'];
            axios({
                method: 'post',
                data: {
                    "Action": "Get_Product_List",
                    "cInvCCode": id,
                    "kpdw": "999999",
                    "iShowType": "1"
                }
            }).then(function(res) {
                if (res) {
                    console.log(res);
                    _this.selectTableData = res.dt;
                    _this.selectTableData.forEach((item, index) => {
                        if (_this.selectedProData.indexOf(item.cInvCode) != -1) {
                            _this.$nextTick(function() {
                                _this.$refs.selectTable.toggleSelect(index);
                            })

                        }
                    })
                }
            });
        },
        handleTableRowClick(val, index) {
            this.$refs.selectTable.toggleSelect(index);
        },
        getSelectedVal(selection, row) {
            this.setVal(row, "add");
        },
        getCancelVal(selection, row) {
            this.setVal(row, "splice");
        },
        setVal(row, active) {
            if (active == 'add') {
                if (this.selectedProData.indexOf(row.cInvCode) == -1) {
                    this.selectedProData.push(row.cInvCode)
                }
            } else {
                if (this.selectedProData.indexOf(row.cInvCode) != -1) {
                    this.selectedProData.splice(this.selectedProData.indexOf(row.cInvCode), 1)
                }
            }
        }

    },
    created() {

    }


})