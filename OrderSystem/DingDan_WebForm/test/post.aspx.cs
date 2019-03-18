using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DingDan_WebForm.test
{
    public partial class post : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
         
          
          string a =@"

































 













































<script>window.alert('投票成功！');self.location.href=document.referrer;</script>






<!doctype html>

<html>

<head>

<meta charset='gbk'>

<meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no'>

<title>请为“四川多联实业有限公司”点赞！2017第九届中国塑料管道招标采购评价推介活动</title>

<meta name='description' content='' />

<script src='jquery-3.1.1.min.js' type='text/javascript'></script>

<link href='gb.css?1.12' type='text/css' rel='stylesheet'/>

<script type='text/javascript'>

$(document).ready(function(e) {

	var video='{VIDEO}';

	var video1=video.substring(video.length-3);

	if('mp4'!=video1){

		$('.comvideo').remove();

	}

	$('.share').click(function(){

		$('.showshare').show();

		$('.share_bg').show();

		})

	$('.showvideo i,share_close,.showshare,.black_overlay').click(function(){

		$('.showvideo').hide().find('video').attr('src','');

		$('.showshare').hide();

		$('.black_overlay').hide();

		})

	$('.hdtab li').click(function(){

		var Thisind = $(this).index();

		$('.hdtab li').removeClass('acttab').eq(Thisind).addClass('acttab');

		$('.list ul').hide().eq(Thisind).show();

		

		})



});

</script>

<script type='text/javascript'>

	

	//招标机构投票

	function check_dai(record_id){

		document.form_main.action = 'info.jsp?record_id='+record_id;

		document.form_main.op.value='op_submit';

		document.form_main.submit();

	}

	

</script>

<script src='http://res.wx.qq.com/open/js/jweixin-1.2.0.js'></script>

<script>

$(document).ready(function(){

var appId;

var signature;

var nonceStr;

var timestamp;

var geturl=location.href.split('#')[0];

var shareImage='http://www.paihang360.com/mzt/slgd2017/images/weixin.jpg';//图片 

var shareTitle='请为“四川多联实业有限公司”点赞！';//标题 

var shareDesc='2017第九届中国塑料管道招标采购评价推介活动'; //简介 

var shareUrl=geturl; //链接 

$.ajax({

url:'http://www.paihang360.com/weixin/geturl.jsp',

type:'get',

data:{'geturl':geturl},

dateType:'json',

async:false,

success:function(date){

}

})







$.ajax({

url:'http://www.paihang360.com/weixin/wx.jsp',

type:'get',

dateType:'json',

async:false,

success:function(date){

//  alert(date.replace(/[\r\n]/g,''));

var sdata=date.replace(/[\r\n]/g,'').split(' ');

appId=sdata[1];

timestamp=sdata[2];

nonceStr=sdata[3];

signature=sdata[4];

}

})

wx.config({

    debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端



//打开，参数信息会通过log打出，仅在pc端时才会打印。

    appId: appId, // 必填，公众号的唯一标识

    timestamp:timestamp, // 必填，生成签名的时间戳

    nonceStr: nonceStr, // 必填，生成签名的随机串

    signature:signature,// 必填，签名，见附录1

    jsApiList: 



['onMenuShareTimeline','onMenuShareAppMessage','onMenuShareQQ','onMenuShareWeibo','onMenuShareQZone'] // 



//必填，需要使用的JS接口列表，所有JS接口列表见附录2

});









wx.ready(function(){

    //分享到朋友圈

        wx.onMenuShareTimeline({

        title: shareTitle, // 分享标题

        link: shareUrl, // 分享链接

        imgUrl:shareImage, // 分享图标

        success: function () { 

       //     alert('已分享');// 用户确认分享后执行的回调函数

        },

        cancel: function () { 

        //  alert('已取消');// 用户取消分享后执行的回调函数

        }

    });

    //分享到朋友

    wx.onMenuShareAppMessage({

        title: shareTitle, // 分享标题

        desc: shareDesc, // 分享描述

        link: shareUrl, // 分享链接

        imgUrl:shareImage, // 分享图标

        type: '', // 分享类型,music、video或link，不填默认为link

        dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空

        success: function () { 

        //    alert('已分享');// 用户确认分享后执行的回调函数

        },

        cancel: function () { 

        //  alert('已取消');// 用户取消分享后执行的回调函数

        }

    });

    //分享到QQ

    wx.onMenuShareQQ({

        title: shareTitle, // 分享标题

        desc: shareDesc, // 分享描述

        link: shareUrl, // 分享链接

        imgUrl: shareImage, // 分享图标

        success: function () { 

           // alert('已分享');// 用户确认分享后执行的回调函数

        },

        cancel: function () { 

            //alert('已取消');// 用户取消分享后执行的回调函数

        }

    });

    //分享到腾讯微博

    wx.onMenuShareWeibo({

        title: shareTitle, // 分享标题

        desc: shareDesc, // 分享描述

        link: shareUrl, // 分享链接

        imgUrl: shareImage, // 分享图标

        success: function () { 

          //  alert('已分享');// 用户确认分享后执行的回调函数

        },

        cancel: function () { 

        //  alert('已取消');// 用户取消分享后执行的回调函数

        }

    });

    //分享到QQ空间

    wx.onMenuShareQZone({

        title: shareTitle, // 分享标题

        desc: shareDesc, // 分享描述

        link: shareUrl, // 分享链接

        imgUrl: shareImage, // 分享图标

        success: function () { 

         //   alert('已分享');// 用户确认分享后执行的回调函数

        },

        cancel: function () { 

        //  alert('已取消');// 用户取消分享后执行的回调函数

        }

    });

    });



})</script>

</head>



<body>

<div class='viewport'>

    <div class='back'><a href='index.jsp'><img src='images/back.png'></a></div>

    <nav>

    	<ul>

            <li class='act'><a href='index.jsp'><img src='images/vote-h.png' alt=''></a></li>

            <li><a href='about.jsp'><img src='images/ab.png' alt=''><p>活动介绍</p></a></li>

            <li><a href='bd/index.jsp'><img src='images/list.png' alt=''><p>获奖榜单</p></a></li>

        </ul>

    </nav>

    <div class='banner'>

		<img src='images/banner.jpg'  />

	</div>

    <div class='info'>



        <form name='form_main' method='post'  action='' >

		<input type='hidden'  name='op' value='init'>



    	<div class='name'>四川多联实业有限公司</div>

        <!-- <div class='video'>

            <video controls='' preload='none' webkit-playsinline='true' source='' src='http://www.paihang360.com/paihang_images/bid_vote_video/147614/{VIDEO}' type='video/mp4'></video>

        </div> -->

		<div class='video'><video controls='' preload='none' webkit-playsinline='true' source='' src='http://www.paihang360.com/paihang_images/bid_vote_video/147614/duolian.mp4' type='video/mp4'></video> </div>

        <div class='abcont'>

            <div class='vote clearfix'>

                <div class='logo'>

                    <img src='http://www.paihang360.com/paihang_images/bid_vote/147614/多联.jpg'/>

                </div> 

                <div class='vote-num'>

                    <p>票数：<i>14402</i></p>                

          <img onclick='check_dai(147614)' src='images/dianzan.png'>

                </div>	

            </div>

            <div class='itrbox'>

                <div class='itr'>

                    <p>&nbsp;&nbsp;&nbsp;四川多联实业有限公司（包含以下子、分公司：成都多联建材有限责任公司、成都市多联塑胶实业公司）是一家专业从事新型塑胶管道等建材领域系列产品的研制、生产和销售的国家高新技术企业。公司自1988年成立以来，一直坚持以市场为导向、用户至上、质量求生存，走自身发展之路，是目前国内同行业中成立早、规模大、品种齐、质量优、开发能力强的知名企业。公司拥有“多联”驰名品牌，市场占有率雄踞西部前三位，名列全国同类企业前茅。<br>
&nbsp;&nbsp;&nbsp;1992年，多联公司以敢为人先的魄力，率先在西南地区开发“难燃PVC电线套管”填补了市场空白，为内地推广使用新型建材做出了卓越贡献。如今，公司拥有一支技术实力雄厚、管理精明强干的队伍和国内领先的生产线和检测设备，现已形成十大系列产品（包括：难燃PVC电线套管、阻燃PVC电线精装管、PVC-U建筑环保排水管、PP-R环保冷热给水管、PP-R环保冷热水精装管、PE环保给水管、PE燃气管、PE-RT环保节能采暖管、埋地环保排水用PVC-U双壁波纹管、&nbsp;地下通信管道用PVC-U多孔管及配套产品），年产十余万吨的生产能力。<br>
&nbsp;&nbsp;&nbsp;公司坚持以质量为本，走品牌兴业之路，企业不断发展壮大。公司拥有稳定的质量管理体系，通过了ISO9001-2008质量管理体系、ISO14001-2004环境管理体系和OHSAS18001-2011职业健康安全管理体系认证，公司每种产品均严格按照标准生产，每批产品均认真按质量标准检测，本着对社会负责，对用户负责的态度，向市场提供质优价廉的产品，得到了社会的广泛认同。<br>
&nbsp;&nbsp;&nbsp;本公司产品经国家多次市场监督抽检均质量合格，并被授予“质量合格好产品”的荣誉。1997年“多联”&nbsp;品牌被四川省政府授予“四川名牌”称号后，又相继获得“国家免检产品”、“中国环境标志产品”&nbsp;、“中国驰名商标”&nbsp;、“中国著名品牌”&nbsp;、“国家高新技术企业”&nbsp;、“新华节水认证”&nbsp;、“绿色建筑选用产品”&nbsp;、“中国优质产品”、“中国著名品牌”、“质量信用AAA等级企业”、“中国AAA级信用企业”、“中国人民银行AAA级信用企业”、“商务部AAA级信用企业”、“中国建材首选品牌”、“全国公认十佳畅销品牌”和“地方名优产品”等荣誉。产品畅销全国二十余省、市，深受用户好评和信赖。</p>

                </div>

            </div>

        </div>

		</form>

    </div>

</div>

</body>

</html>




";

          int b = a.IndexOf(@"<div class='vote-num'>

                    <p>票数：<i>");
     string c=   a.Substring(b+55,5);
            
        Response.Write(b+"<br/>");
        Response.Write(c + "<br/>");

        Regex regex = new Regex(@"\d{5}", RegexOptions.IgnoreCase);

      Match  m=  regex.Match(a);
        //显示每一个满足正则表达式的子字符串  
      Response.Write(m.Value+ "<br/>");
        }


          [System.Web.Services.WebMethod]
        public static string vote(){
               string url = "http://www.paihang360.com/mzt/slgd2017/info.jsp?record_id=147614";
            string postData = "op=op_submit";

           string res=HttpPost(url, postData);
           if (res.Contains("投票成功"))
           {
               return DateTime.Now.ToString()+"   投票成功";
           }
           else
           {
               return DateTime.Now.ToString() + "   投票失败";
           }
        }

        //HttpPost方法
        //body是要传递的参数,格式"roleId=1&uid=2"
        //post的cotentType填写:
        //"application/x-www-form-urlencoded"
        //soap填写:"text/xml; charset=utf-8"
        //http登录post
        private static  string HttpPost(string Url, string Body)
        {
            string ResponseContent = "";
            //  var uri = new Uri(Url, true);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
            httpWebRequest.Headers["X-Forwarded-For"] = getRandomIp();
            httpWebRequest.ContentType = "application/x-www-form-urlencoded"; ;
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 20000; //setInstanceFollowRedirects
            // httpWebRequest.MediaType = "json";

            byte[] btBodys = Encoding.UTF8.GetBytes(Body);
            // httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("GB2312"));
            try
            {



                ResponseContent = streamReader.ReadToEnd();


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                httpWebResponse.Close();
                streamReader.Close();
                httpWebRequest.Abort();
                httpWebResponse.Close();
            }
            return ResponseContent;
        }


        public static string getRandomIp()
        {     /*
     int[][]
     这个叫交错数组,白话文就是数组的数组.
    初始化的方法:
     int[][] numbers = new int[][] { new int[] {2,3,4}, new int[] {5,6,7,8,9} };
当然也可以使用{}初始化器初始化
             int[][] numbers = { new int[] {2,3,4}, 
                            new int[] {5,6,7,8,9} 
                          };
     */
            //            int[][] range = {
            //new int[]{607649792,608174079},//36.56.0.0-36.63.255.255
            //new int[]{1038614528,1039007743},//61.232.0.0-61.237.255.255
            //new int[]{1783627776,1784676351},//106.80.0.0-106.95.255.255
            //new int[]{2035023872,2035154943},//121.76.0.0-121.77.255.255
            //new int[]{2078801920,2079064063},//123.232.0.0-123.235.255.255
            //new int[]{-1950089216,-1948778497},//139.196.0.0-139.215.255.255
            //new int[]{-1425539072,-1425014785},//171.8.0.0-171.15.255.255
            //new int[]{-1236271104,-1235419137},//182.80.0.0-182.92.255.255
            //new int[]{-770113536,-768606209},//210.25.0.0-210.47.255.255
            // new int[]{-569376768,-564133889}, //222.16.0.0-222.95.255.255
            //};

  

            int[][] range = {
                        new int[]{975044608,977272831},////58.30.0.0-58.63.255.255
                        new int[]{607649792,608174079},//36.56.0.0-36.63.255.255
                        new int[]{999751680,999784447},//59.151.0.0-59.151.127.255
                        new int[]{1019346944,1019478015}, //60.194.0.0-60.195.255.255
                        new int[]{1038614528,1039007743},//61.232.0.0-61.237.255.255
                        new int[]{1783627776,1784676351},//106.80.0.0-106.95.255.255
                        new int[]{1947009024,1947074559},//116.13.0.0-116.13.255.255
                        new int[]{1987051520,1988034559},//118.112.0.0-118.126.255.255
                        new int[]{2035023872,2035154943},//121.76.0.0-121.77.255.255
                        new int[]{2078801920,2079064063},//123.232.0.0-123.235.255.255
                        new int[]{-1950089216,-1948778497},//139.196.0.0-139.215.255.255
                        new int[]{-1425539072,-1425014785},//171.8.0.0-171.15.255.255
                        new int[]{-1236271104,-1235419137},//182.80.0.0-182.92.255.255
                        new int[]{-770113536,-768606209},//210.25.0.0-210.47.255.255
                        new int[]{-569376768,-564133889},//222.16.0.0-222.95.255.255
                  
                        };

            Random rdint = new Random();
            int index = rdint.Next(10);
            string ip = num2ip(range[index][0] + new Random().Next(range[index][1] - range[index][0]));
            return ip;
        }

        /*
         * 将十进制转换成ip地址
        */
        public static string num2ip(int ip)
        {
            int[] b = new int[4];
            string x = "";
            //位移然后与255 做高低位转换
            b[0] = (int)((ip >> 24) & 0xff);
            b[1] = (int)((ip >> 16) & 0xff);
            b[2] = (int)((ip >> 8) & 0xff);
            b[3] = (int)(ip & 0xff);
            x = (b[0]).ToString() + "." + (b[1]).ToString() + "." + (b[2]).ToString() + "." + (b[3]).ToString();

            return x;
        }
    }
}