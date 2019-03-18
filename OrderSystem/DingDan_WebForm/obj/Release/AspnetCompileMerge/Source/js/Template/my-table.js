Vue.component('select-product-table', {
    template: ` <div>
	<Table border :columns="tableColumn" :data="selectProductData" ></Table>
	</div>`,
    props:['selectProductData'],
    data() {
        return {
             tableColumn: [{
                    title: '序号',
                    width: 80,
                    align: "center",
                    render: (h, params) => {
                        return h('span', params.index + (vm.currentPage - 1) * vm.pageSize + 1);
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
    methods: {
        bb() {
            var o = {};
            o["name"] = "张地方官";
            o["age"] = 213;
            //	this.$emit('search',o)
            this.$Message.error({ "content": "未查询到该订单！", "duration": 5 });
        }
    }
})