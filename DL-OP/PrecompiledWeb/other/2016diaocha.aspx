<%@ page language="C#" autoeventwireup="true" inherits="other_2016diaocha, dlopwebdll" enableviewstate="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <script src="../js/jquery-1.11.0.min.js"></script>
    <script src="../css/bootstrap.min.css"></script>
    <style>
        * {
            font-family: "microsoft yahei","Lucida Grande",Helvetica,Arial,Verdana,sans-serif;
        }

        body, input, textarea, select {
            font-family: "microsoft yahei",Helvetica,Arial,Verdana,sans-serif;
            font-size: 14px;
            position: relative;
            background: #efefef;
        }

        .red {
            color: red;
        }

        .row {
            margin: 0 0 0 100px;
            margin-bottom: 10px;
        }

        .title {
            border-bottom: 10px solid #3399ff;
            margin: 40px 0 40px 0;
        }

        .container {
        }
    </style>
</head>

<body>
    <form id="form1" >
    <div>
      <div class="container" style="background: white; margin-top: 50px; padding-top: 50px;">
        <h1 style="text-align: center">2016年度客户满意度调查</h1>
            <div class="row">
                <div class="col-lg-10 title">

                    <h3>各区经销商及合作机构：</h3>
                    <p>&nbsp &nbsp &nbsp &nbsp 感谢多年来对“多联”品牌的支持与厚爱，公司一贯秉承“以市场客户为中心”的核心经营理念服务于广大客户群体，为进一步提升和改善公司产品供应、业务响应及服务效率，故制定《客户满意度调查》对公司整体服务进行评测。 </p>
                    <p>&nbsp &nbsp &nbsp &nbsp 我们殷切希望您对公司整体服务进行客观评价，调查结果是公司各业务板块工作改进的重要依据。</p>

                </div>
            </div>
            <div style="margin-bottom:100px;">
                <div class="form-group">
                    <label for="khbm" class="col-lg-1 col-lg-offset-1 control-label">客户编码:</label>
                    <div class="col-lg-3">
                        <asp:Label ID="khbm" runat="server" Text="Label"></asp:Label>  
                    </div>
                </div>
                <div class="form-group">
                    <label for="khmc" class="col-lg-1  col-lg-offset-1 control-label">客户名称:</label>
                    <div class="col-lg-3">
                        <asp:Label ID="khmc" runat="server" Text="Label"></asp:Label>
                    </div>
                </div>
            </div>
            <hr />
            <div id="content">
            </div>
            <div class="row">
                <h4>34、	合作伙伴意见及建议：</h4>
                <textarea class="form-control col-lg-6" name="advise" style="height:100px;width:90%"></textarea>
            </div>
            <div class="row" style="margin:50px 0 50px 0;">
                <input type="button" name="btn" id="btn" value="提交" class="btn btn-primary col-lg-2 col-lg-offset-4" />
            </div>

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var title = Array(
                "1、	您对公司产品的质量是否满意",
                "2、	您对公司产品的品类供应是否满意？",
                "3、	您对公司产品的包装是否满意？",
                "4、	您对公司产品供应保障能力是否满意？",
                "5、	您对公司新产品研发能力是否满意？",
                "6、	综上所述，您对公司产品与当地市场需求匹配度是否满意？",
                "7、	您对公司区域经理在当地市场的业务开发能力是否满意？",
                "8、	您对公司区域经理在产品及营销政策掌握程度是否满意？",
                "9、	您对公司区域经理在项目开发能力及市场配合度是否满意？",
                "10、	您对公司招投标服务是否满意?",
                "11、	您对公司区域经理的态度及主观能动性是否满意？",
                "12、	您对公司整体营销政策是否满意？",
                "13、	您对公司销售团队在当地市场的支持配合力度是否满意？",
                "14、	您对公司商务服务人员的专业性是否满意?",
                "15、	您对公司商务服务人员的服务态度是否满意？",
                "16、	您对公司400话务受理是否满意？",
                "17、	您对公司合同文件服务是否满意？",
                "18、	您对公司订货接单服务是否满意？",
                "19、	您对公司运输服务是否满意？",
                "20、	您对公司样品资料发放是否满意？",
                "21、	您对公司在渠道广告推广方面是否满意？",
                "22、	您对公司网上订单系统的使用是否满意？",
                "23、	您对公司订单处理过程中信息化支撑方面是否满意？",
                "24、	您对公司信息化在商家整体支撑方面的的服务是否满意？",
                "25、	您对公司财务部门结算方面的对接是否满意？",
                "26、	您对公司财务部门在票据及查账方面的对接是否满意？",
                "27、	您对公司财务部门提供的整体服务是否满意？",
                "28、	您对公司物流配送的时效性及司机服务态度方面是否满意？",
                "29、	您对公司物流部门在货品装卸操作是否满意？",
                "30、	您对物流部门提供的整体服务是否满意？",
                "31、	您对我司售后及投诉处理时效是否满意？",
                "32、	您对我司售后及投诉处理结果是否满意？",
                "33、	您对我司售后及投诉处理的专业性及服务是否满意？");
            var html = "";
            var len = title.length + 1;

            for (var i = 1; i < len; i++) {
                html += " <div class='row' id='question" + i + "'> <h4>" + title[i - 1] + "</h4><div class='col-lg-10'>";
                html += "<div class='col-lg-3'><input type='radio' name='question" + i + "' value='A' class='col-lg-2' id='question" + i + "_A'/><label class='col-lg-10' for='question" + i + "_A'>非常满意 5分</label></div>";
                html += "<div class='col-lg-3'><input type='radio' name='question" + i + "' value='B' class='col-lg-2' id='question" + i + "_B'/><label class='col-lg-10'for='question" + i + "_B'>满意 4分</label></div>";
                html += "<div class='col-lg-3'><input type='radio' name='question" + i + "' value='C' class='col-lg-2' id='question" + i + "_C'/><label class='col-lg-10' for='question" + i + "_C'>一般 3分</label></div>";
                html += "<div class='col-lg-3'><input type='radio' name='question" + i + "' value='D' class='col-lg-2' id='question" + i + "_D'/><label class='col-lg-10' for='question" + i + "_D'>不满意 1分</label></div>";
                html += "</div></div><hr />";
            }

            $("#content").append(html);

            $("#btn").click(function () {
                var answer = "";
                for (var i = 1; i < len; i++) {
                    var val = $("#question" + i + " input:radio:checked").val();
                    if (val == undefined) {
                        // alert("第" + i + "题还未选择！");
                        $("#question" + i).addClass("red");
                        $("html,body").animate({ scrollTop: $("#question" + i).offset().top }, 500);
                        return false;
                    } else {
                        answer += val;

                    }

                }
                $.ajax({                 
                    url: "../Handler/Handler.ashx",
                    data: $("#form1").serialize(),
                    success: function (data) {
                        alert(data);
                    }
                })
            })

            $("input[type=radio]").click(function () {
                $("div").removeClass("red");
            })

        })

    </script>  
    </div>
    </form>
</body>
</html>
