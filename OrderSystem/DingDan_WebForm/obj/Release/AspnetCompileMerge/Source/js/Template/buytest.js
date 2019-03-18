axios.defaults.baseURL = '/handler/productHandler.ashx';
var Bus = new Vue();
var vm = new Vue({
    el: '#vm',
    data: {
        postData: 'aaaaaaab',
        msg: 'this is msg',
        productClass: [],
        selectTableVisible:false
    },
    methods: {

        bb: () => {
            var _this = this;
            axios({
                method: 'post',
                data: {
                    "Action": "Get_Product_List",
                    "cInvCCode": id,
                    "kpdw": "999999",
                    "iShowType": 1,
                    
                }
            }).then(function(res) {
                console.log(res);
                _this.proTableData = res.dt;
            })
        },
        transDate: function() {
            var result = [],
                temp = {},
                list = arr,
                idstr = "id",
                pidstr = "pid";
            for (i = 0; i < list.length; i++) {
                temp[list[i][idstr]] = list[i]; //将nodes数组转成对象类型
            }
            for (j = 0; j < list.length; j++) {
                tempVp = temp[list[j][pidstr]]; //获取每一个子对象的父对象
                if (tempVp) { //判断父对象是否存在，如果不存在直接将对象放到第一层
                    if (!tempVp["children"]) tempVp["children"] = []; //如果父元素的nodes对象不存在，则创建数组
                    tempVp["children"].push(list[j]); //将本对象压入父对象的nodes数组
                } else {
                    result.push(list[j]); //将不存在父对象的对象直接放入一级目录
                }
            }
            this.productClass = result;
        },
        getSelectedProData(val){
            console.log(val);
        }
    },
    mounted(){
        Bus.$on('sendSelectedProData',function(val){
            console.log(val);
        })
    },
    created() {
        this.transDate();
    },
    computed:{
        SelectTableButtonText(){
            if (this.selectTableVisible) {
                return '隐藏商品选择';
            }else{
                return '显示商品选择'
            }
        }
    }
})