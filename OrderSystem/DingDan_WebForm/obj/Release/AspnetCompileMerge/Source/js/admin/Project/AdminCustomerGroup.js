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
           console.log(res);
           if (res.data.flag != 1) {
               vm.$message({ type: 'error', message: res.data.message });
               return false;
           }
           return res;
       })

       var vm = new Vue({
           el: '#app',
           data: {
               groups: [],
               // treeData: [],
               addFormVisible: false,
               addLoading: false,
               addFormRules: {
                   groupName: [
                       { required: true, message: '请输入组名', trigger: 'blur' }
                   ],
                   groupParent: [
                       { required: true, message: '请选择所属分组', trigger: 'blur' }
                   ],

               },
               addForm: {
                   groupName: '',
                   groupParent: '',
                   groupParentId: ''

               },
               editFormVisible: false,
               editForm: {
                   groupName: '',
                   groupParent: '',
                   groupParentId: '',
                   groupId:''

               },
           },
           components: {

           },
           methods: {
               show: function() {
                   this.$alert('这是一段内容', '标题名称', {
                       confirmButtonText: '确定',
                       callback: action => {
                           console.log(`${action}`);
                           this.$message({
                               type: 'info',
                               message: `action: ${ action }`
                           });
                       }
                   });
               },
               //显示新增界面
               handleAdd: function() {
                   this.addFormVisible = true;
                   this.addForm = {
                       groupName: '',
                       groupParent: ''
                   };

               },
               //新增
               addSubmit: function() {
                   this.$refs.addForm.validate((valid) => {
                       if (valid) {
                           axios.post('', { "Action": "AddAdminCustomerGroup", "groupName": this.addForm.groupName, "groupParent": this.addForm.groupParentId })
                               .then(res => {
                                   console.log(res);
                                   this.$message({
                                       message: '新增成功',
                                       type: 'success'
                                   });
                                   this.$refs['addForm'].resetFields();
                                   this.addFormVisible = false;
                                   this.getAllGroup();
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
                           axios.post('', 
                            { "Action": "EditAdminCustomerGroup",
                             "groupName": this.editForm.groupName, 
                             "groupParentId": this.editForm.groupParentId,
                             "groupId":this.editForm.groupId 
                           })
                               .then(res => {
                                   console.log(res);
                                   this.$message({
                                       message: '修改成功',
                                       type: 'success'
                                   });
                                   this.$refs['editForm'].resetFields();
                                   this.editFormVisible = false;
                                   this.getAllGroup();
                               })
                               .catch(error => {
                                   console.log(error);
                               });
                       }
                   });
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
               handleEdit: function(row) {
                   this.editFormVisible = true;
                   this.editForm = {
                       groupName: row.groupName,
                       groupParent: row.parentName,
                       groupParentId: row.pid,
                       groupId:row.id
                   };
               },
               handleDelete: function(row) {
                  console.log(row);
                   this.$confirm('此操作将删除该分组及该分组下所有子分组, 是否继续?', '提示', {
                       confirmButtonText: '确定',
                       cancelButtonText: '取消',
                       type: 'warning',
                   }).then(() => {
                       console.log(row.id);
                       axios.post('/handler/AdminHandler.ashx', {
                               'Action': 'DelAdminCustomerGroupById',
                               'id': row.id
                           })
                           .then(res => {
                               if (res) {
                                   this.$message({
                                       message: '删除成功',
                                       type: 'success'
                                   });
                                   this.getAllGroup();
                               }
                           })
                   }).catch(() => {

                   })
               },
               // getTreeData: function() {
               //  this.treeData.length=0;
               //     var o={};
               //     o.id=0;o.label='根目录';o.children=[];
               //     o.children=this.fn(this.groups, 0);
               //     this.treeData[0]=o;
               //     console.log(this.treeData);
               // },
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
                   this.addForm.groupParent = b.data.label;
                   this.addForm.groupParentId = b.data.id;
               },
               handleEditNodeClick: function(a, b, c) {
                   this.editForm.groupParent = b.data.label;
                   this.editForm.groupParentId = b.data.id;
               },
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
               }

           },
           created: function() {
               this.getAllGroup();
           }
       })


   }