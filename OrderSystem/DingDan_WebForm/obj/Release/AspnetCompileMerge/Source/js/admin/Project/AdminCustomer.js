   window.onload = function() {
       axios.interceptors.request.use(function(config) {
           config.headers['Content-Type'] = 'application/x-www-form-urlencoded; charset=UTF-8';
           var p = new URLSearchParams();
           for (var key in config.data) {
               p.append(key, config.data[key])
           }
           config.data = p;
           return config;
       }, function(err) {
           return Promise.reject(error);
       });

       axios.defaults.baseURL = '/handler/AdminHandler.ashx';
       axios.interceptors.response.use(function(res) {

           if (res.data.flag != 1) {
               vm.$message({ type: 'error', message: res.data.message });
               return false;
           }
           return res;
       })

       var vm = new Vue({
           el: '#app',
           data: {
               customers: [], //页面加载时获取所有用户
               customersMap: {},
               selectCustomers: [], //选择右侧Table里的用户
               groups: [],
               treeCheckKeys: [],
               message: '',
               addFormVisible: false,
               addLoading: false,
               addFormRules: {
                   customerName: [
                       { required: true, message: '请输入客户姓名', trigger: 'blur' }
                   ],
                   customerPhone: [
                       { required: true, message: '请输入客户电话', trigger: 'blur' },
                       { max:11,min:11,message: '电话号码不正确', trigger: 'blur' }
                   ],
                   customerGroupName: [
                       { required: true, message: '请选择所属分组', trigger: 'blur' }
                   ],

               },
               addForm: {
                   customerName: '',
                   customerPhone: '',
                   customerGroupName: '',
                   customerGroupId: ''

               },
               editFormVisible: false,
               editForm: {
                   customerId: '',
                   customerName: '',
                   customerPhone: '',
                   customerGroupName: '',
                   customerGroupId: ''

               },
           },
           components: {

           },
           methods: {
               show: function() {
                   console.log(this.selectCustomers);
               },
               //显示新增界面
               handleAdd: function() {
                   this.addFormVisible = true;
                   this.addForm = {
                       customerName: '',
                       customerPhone: '',
                       customerGroupName: '',
                       customerGroupId: ''
                   };

               },
               //新增
               addSubmit: function() {
                   this.$refs.addForm.validate((valid) => {
                       if (valid) {

                           axios.post('', {
                                   "Action": "AddAdminCustomer",
                                   "customerName": this.addForm.customerName,
                                   "customerPhone": this.addForm.customerPhone,
                                   "customerGroupId": this.addForm.customerGroupId

                               })
                               .then(res => {
                                   if (res) {
                                       this.$message({
                                           message: '新增成功',
                                           type: 'success'
                                       });
                                       this.addFormVisible = false;
                                       this.getAllCustomers();
                                   }

                               })
                               .catch(error => {
                                   console.log(error);
                               });


                       }
                   });
               },
               //编辑
               editSubmit: function() {
                   this.$refs.editForm.validate((valid) => {
                       if (valid) {
                           axios.post('', {
                                   "Action": "EditAdminCustomer",
                                   "customerId": this.editForm.customerId,
                                   "customerName": this.editForm.customerName,
                                   "customerPhone": this.editForm.customerPhone,
                                   "customerGroupId": this.editForm.customerGroupId
                               })
                               .then(res => {
                                   if (res) {
                                       this.$message({
                                           message: '修改成功',
                                           type: 'success'
                                       });
                                       this.$refs['editForm'].resetFields();
                                       this.editFormVisible = false;
                                       this.getAllCustomers();
                                   }

                               })
                               .catch(error => {
                                   console.log(error);
                               });
                       }
                   });
               },
               //获取该用户所创建的所有客户组
               getAllCustomers: function() {
                   axios.post('', {
                           'Action': 'GetAllAdminCustomerByCreateId',
                       })
                       .then(res => {
                           if (res) {
                               this.customers = res.data.customers;
                               if (this.customers.length > 0) {
                                   this.customers.forEach(item => {
                                       this.customersMap[item.GroupId] = item;
                                   })
                               }
                           } else {}
                       })

               },
               //显示编辑界面
               handleEdit: function(row) {
                   this.editFormVisible = true;
                   this.editForm = {
                       customerId: row.id,
                       customerName: row.customerName,
                       customerPhone: row.customerPhone,
                       customerGroupName: row.groupName,
                       customerGroupId: row.GroupId
                   };
               },
               handleDelete: function(row) {
                   this.$confirm('此操作将删除该客户, 是否继续?', '提示', {
                       confirmButtonText: '确定',
                       cancelButtonText: '取消',
                       type: 'warning',
                   }).then(() => {
                       axios.post('', {
                               'Action': 'DelAdminCustomerById',
                               'customerId': row.id
                           })
                           .then(res => {
                               if (res) {
                                   this.$message({
                                       message: '删除成功',
                                       type: 'success'
                                   });
                                   this.getAllCustomers();
                               }
                           })
                   }).catch(() => {

                   })
               },

               //获取该用户所创建的所有客户组
               getAllGroup: function() {
                   axios.post('', {
                           'Action': 'GetAllAdminCustomerGroupByCreateId',
                       })
                       .then(res => {
                           if (res) {
                               this.groups = res.data.group;
                           } else {}
                       })

               },
               fn: function(data, pid) {
                   var result = [],
                       temp;
                   for (var i = 0; i < data.length; i++) {
                       if (data[i].pid == pid) {
                           var obj = { "label": data[i].groupName, "id": data[i].id };
                           temp = this.fn(data, data[i].id);
                           if (temp.length > 0) {
                               obj.children = temp;
                           }
                           result.push(obj);
                       }
                   }
                   return result;
               },
               handleAddNodeClick: function(a, b, c) {

                   this.addForm.customerGroupName = b.data.label;
                   this.addForm.customerGroupId = b.data.id;
               },
               handleEditNodeClick: function(a, b, c) {
                   this.editForm.customerGroupName = b.data.label;
                   this.editForm.customerGroupId = b.data.id;
               },
               handleAddNodeCheck: function(a, b, c) {
                   this.treeCheckKeys = this.$refs.tree.getCheckedKeys();
               },
               empty: function() {
                   this.$refs.tree.setCheckedKeys([]);
               },
               showKeys: function() {
                   console.log(this.$refs.tree.getCheckedKeys());
               },
               handleSelectCustomers: function(val) {
                   this.selectCustomers = val;
               },
               sendSMS: function() {
                   if (this.selectCustomers.length == 0) {
                       this.$message({
                           message: '你还没有选择客户！',
                           type: 'error'
                       });
                       return false;
                   }
                   if (this.message.trim() == '') {
                       this.$message({
                           message: '你还没有输入短信内容！',
                           type: 'error'
                       });
                       return false;
                   }
                   var phoneArr = [],
                       phones = '';
                   this.selectCustomers.forEach(item => {
                       phoneArr.push(item.customerPhone);
                   });
                   phones = phoneArr.join();
                   axios.post('', {
                       "Action": "AdminSendSMS",
                       "phones": phones,
                       "content": this.message
                   }).then(res => {
                       if (res) {
                           this.$notify({
                               title: '成功',
                               message: '已成功发送客户短信',
                               type: 'success',
                               duration: 0
                           });
                          this.message='';
                          this.$refs.table.clearSelection();
                       }

                   })
               }
           },
           computed: {
               treeData: function() {
                   var arr = [];
                   var o = {};
                   o.id = 0;
                   o.label = '根目录';
                   o.children = [];
                   o.children = this.fn(this.groups, 0);
                   arr[0] = o;
                   return arr;
               },
               addTreeData: function() {
                   return this.fn(this.groups, 0);
               },
               treeCheckCustomers: function() {
                   var o = [];
                   var a = this.treeCheckKeys;
                   if (a.length > 0) {
                       this.customers.forEach(item => {
                           if (a.indexOf(item.GroupId) != -1) {
                               o.push(item)
                           }
                       })
                   }

                   return o;
               },


           },
           watch: {
               addFormVisible: function() {
                   if (!this.addFormVisible) {
                       this.$refs['addForm'].resetFields();
                   }
               },
               editFormVisible: function() {
                   if (!this.editFormVisible) {
                       this.$refs['editForm'].resetFields();
                   }
               }
           },

           created: function() {
               this.getAllCustomers();
               this.getAllGroup();
           }
       })


   }