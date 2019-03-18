   window.onload = function() {
       axios.interceptors.request.use(function(config) {
           config.headers['Content-Type'] = 'application/x-www-form-urlencoded; charset=UTF-8';
           var p = new URLSearchParams();
           for (var key in config.data) {
               p.append(key, config.data[key])
           }
           config.data = p;
           vm.$Spin.show({
               render: (h) => {
                   return h('div', [
                       h('Icon', {
                           'class': 'demo-spin-icon-load',
                           props: {
                               type: 'load-c',
                               size: 18
                           }
                       }),
                       h('div', 'Loading')
                   ])
               }
           });
           return config;
       }, function(err) {
           return Promise.reject(error);
       });

       axios.interceptors.response.use(function(res) {
           vm.$Spin.hide();
           console.log(res);
           if (res.data.flag != 1) {
               vm.$Modal.error({ content: res.data.message });
               return false;
           }
           return res;
       })

       var vm = new Vue({
           el: '#vm',
           data: {
               showAddGroupModal: false,

               addGroupRule: {
                   groupName: [
                       { required: true, message: '必须输入分组名', trigger: 'blur' }
                   ],
                   groupParent: [
                       { required: true, message: '必须选择所属分组', trigger: 'blur' },
                   ]
               },
               groups: [],
               columns: [{
                       title: '操作',
                       key: 'action',
                       width: 150,
                       align: 'center',
                       render: (h, params) => {
                           return h('div', [
                               h('Button', {
                                   props: {
                                       type: 'primary',
                                       size: 'small'
                                   },
                                   style: {
                                       marginRight: '5px'
                                   },
                                   on: {
                                       click: () => {
                                           vm.show(params.index)
                                       }
                                   }
                               }, '编辑'),
                               h('Button', {
                                   props: {
                                       type: 'error',
                                       size: 'small'
                                   },
                                   on: {
                                       click: () => {
                                           vm.delGroup(params)
                                       }
                                   }
                               }, '删除')
                           ]);
                       }
                   },
                   {
                       title: '分组名',
                       key: 'groupName'
                   },
                   {
                       title: '所属分组',
                       key: 'parentName'
                   },
                   {
                       title: '创建日期',
                       key: 'createTime'
                   }
               ],
               select1: '3221a',
               modal1: false,
               tableSelet: [],
               columns1: columns1,
               data1: data1,
               cityList: cityList,
               model10: []
           },
           components: {

           },
           methods: {
               addGroup(name) {
                   console.log(this.addGroupModel.groupParent);
                   // this.$refs[name].validate((valid) => {
                   //     if (valid) {
                   //         axios.post('/handler/AdminHandler.ashx', {
                   //                 'Action': 'AddAdminCustomerGroup',
                   //                 'groupName': this.addGroupModel.groupName,
                   //                 'groupParent': this.addGroupModel.groupParent
                   //             })
                   //             .then(res => {
                   //                 if (res) {
                   //                     this.$Message.success('新增成功!');
                   //                     this.showAddGroupModal = false;
                   //                     this.addGroupModel.groupName = '';
                   //                     this.addGroupModel.groupParent = '';
                   //                     this.$refs[name].resetFields();
                   //                     this.getAllGroup();
                   //                 } else {
                   //                     this.showAddGroupModal = false;
                   //                 }
                   //             })
                   //     } else {
                   //         this.$Message.error('必埴信息不完整!');
                   //     }
                   // })

                   if (this.addGroupModel.groupName != '' && this.addGroupModel.groupParent != '') {
                       axios.post('/handler/AdminHandler.ashx', {
                               'Action': 'AddAdminCustomerGroup',
                               'groupName': this.addGroupModel.groupName,
                               'groupParent': this.addGroupModel.groupParent
                           })
                           .then(res => {
                               if (res) {
                                   this.$Message.success('新增成功!');
                                   this.showAddGroupModal = false;
                                   this.addGroupModel.groupName = '';
                                   this.addGroupModel.groupParent = '';
                                   this.$refs[name].resetFields();
                                   //this.getAllGroup();
                               } else {
                                   this.showAddGroupModal = false;
                               }
                           })
                   } else {
                       this.$Message.error('必埴信息不完整!');
                   }
               },
               changeGroupParent: function(v) {
                   this.addGroupModel.groupParent = v.toString();
                   console.log(this.addGroupModel.groupParent);

               },
               //获取该用户所创建的所有客户组
               getAllGroup: function() {
                   axios.post('/handler/AdminHandler.ashx', {
                           'Action': 'GetAllAdminCustomerGroupByCreateId',
                       })
                       .then(res => {
                           if (res) {
                               this.groups = res.data.group;
                           } else {}
                       })

               },
               delGroup: function(data) {
                   this.$Modal.confirm({
                       content: '确认要删除该分组及该分组下所有子分组?',
                       onOk: function() {
                           console.log(data.row.id);
                           axios.post('/handler/AdminHandler.ashx', {
                                   'Action': 'DelAdminCustomerGroupById',
                                   'id': data.row.id
                               })
                               .then(res => {
                                   if (res) {
                                       this.$Message.success({ content: '删除成功!' });
                                       vm.getAllGroup();
                                   }
                               })
                       }
                   })
               },
               show: function() {
                   console.log(this.addGroupModel.groupName);
                   this.showAddGroupModal = !this.showAddGroupModal;
               },
               modal: function() {
                   this.modal1 = !this.modal1;
               },
               axios: function() {
                   axios.post('/handler/AdminHandler.ashx', { 'Action': 'GetAllRole', 'codes': '09351' }).then(response => {
                       console.log(response.data);
                   })

               }
           },
           computed: {
               showAddGroupModal1: function() {


                   return this;
               },
               addGroupModel: function() {
                   var o = {};
                   o.groupName = '';
                   o.groupParent = ''
                   o.groups = [{ id: "0", groupName: "根目录" }];
                   o.groups = o.groups.concat(this.groups);
                   return o;
               }

           },
           created: function() {
               this.getAllGroup();
           }
       })

   }