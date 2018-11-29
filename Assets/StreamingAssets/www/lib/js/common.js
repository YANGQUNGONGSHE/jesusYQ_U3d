// var timestamp3 = 1511352968;
// var newDate = new Date();
// newDate.setTime(timestamp3 * 1000);
Date.prototype.format = function(format) { //在日期原型上增加一个format方法
    var date = {
            "M+": this.getMonth() + 1,
            "d+": this.getDate(),
            "h+": this.getHours(),
            "m+": this.getMinutes(),
            "s+": this.getSeconds(),
            "q+": Math.floor((this.getMonth() + 3) / 3),
            "S+": this.getMilliseconds()
    };
    if (/(y+)/i.test(format)) {
            format = format.replace(RegExp.$1, (this.getFullYear() + '').substr(4 - RegExp.$1.length));
    }
    for (var k in date) {
            if (new RegExp("(" + k + ")").test(format)) {
                    format = format.replace(RegExp.$1, RegExp.$1.length == 1
                            ? date[k] : ("00" + date[k]).substr(("" + date[k]).length));
            }
    }
    return format;
}
// console.log(newDate.format('yyyy-MM-dd hh:mm:ss'));

/*************************************源·传海************************************/

// var getAliImgPath = "https://together-imgs.oss-cn-shanghai.aliyuncs.com/";
// var uploadImage = function (img,success) {
//     var imgPath = img.val();
//     if (imgPath == "") {
//         alert("请选择上传图片！");
//         return;
//     }
//     var formData = new FormData();
//     formData.append('file', img[0].files[0]);
//     $.ajax({
//         type: "POST",
//         url: "https://img.jointogether.cn/upload/image",
//         data:formData,
//         cache: false,
//         async:true,
//         processData: false,
//         contentType: false,
//         success: success,
//         error: function() {
//             alert("上传失败，请检查网络后重试");
//         }
//     });
// };

/**获取页面跳转后的参数**/
function GetRequest() { 
	var url = location.search; //获取url中"?"符后的字串 
	var theRequest = new Object(); 
	if (url.indexOf("?") != -1) { 
		var str = url.substr(1); 
		strs = str.split("&"); 
		for(var i = 0; i < strs.length; i ++) { 
			theRequest[strs[i].split("=")[0]]=unescape(strs[i].split("=")[1]); 
		} 
	} 
	return theRequest; 
}
var request = GetRequest();




/***把2012-12-21转化成2012年12月21日***/

// function dateFormat(str){
// //	var str = "2017-02-16";
//     var reg =/(\d{4})\-(\d{2})\-(\d{2})/;
//     var date = str.replace(reg,"$1年$2月$3日");
// //  alert(date);
// 	return date;
// }


/***根据年月日 时秒分等获取周几***/
// var dateWeek = function (date){
// 	var x = "";
// 	var day = new Date(date).getDay();
// 	switch (day){
// 		case 0:
// 		  x="星期日";
// 		  break;
// 		case 1:
// 		  x="星期一";
// 		  break;
// 		case 2:
// 		  x="星期二";
// 		  break;
// 		case 3:
// 		  x="星期三";
// 		  break;
// 		case 4:
// 		  x="星期四";
// 		  break;
// 		case 5:
// 		  x="星期五";
// 		  break;
// 		case 6:
// 		  x="星期六";
// 		  break;
// 	}
// 	return x;
// }

// var dateWeek2 = function (date){
//     var x = "";
//     var day = new Date(date).getDay();
//     switch (day){
//         case 0:
//             x="周日";
//             break;
//         case 1:
//             x="周一";
//             break;
//         case 2:
//             x="周二";
//             break;
//         case 3:
//             x="周三";
//             break;
//         case 4:
//             x="周四";
//             break;
//         case 5:
//             x="周五";
//             break;
//         case 6:
//             x="周六";
//             break;
//     }
//     return x;
// }
// Date.prototype.pattern=function(fmt) {
//     var o = {
//         "M+" : this.getMonth()+1, //月份
//         "d+" : this.getDate(), //日
//         "h+" : this.getHours()%12 == 0 ? 12 : this.getHours()%12, //小时
//         "H+" : this.getHours(), //小时
//         "m+" : this.getMinutes(), //分
//         "s+" : this.getSeconds(), //秒
//         "q+" : Math.floor((this.getMonth()+3)/3), //季度
//         "S" : this.getMilliseconds() //毫秒
//     };
//     var week = {
//         "0" : "/u65e5",
//         "1" : "/u4e00",
//         "2" : "/u4e8c",
//         "3" : "/u4e09",
//         "4" : "/u56db",
//         "5" : "/u4e94",
//         "6" : "/u516d"
//     };
//     if(/(y+)/.test(fmt)){
//         fmt=fmt.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length));
//     }
//     if(/(E+)/.test(fmt)){
//         fmt=fmt.replace(RegExp.$1, ((RegExp.$1.length>1) ? (RegExp.$1.length>2 ? "/u661f/u671f" : "/u5468") : "")+week[this.getDay()+""]);
//     }
//     for(var k in o){
//         if(new RegExp("("+ k +")").test(fmt)){
//             fmt = fmt.replace(RegExp.$1, (RegExp.$1.length==1) ? (o[k]) : (("00"+ o[k]).substr((""+ o[k]).length)));
//         }
//     }
//     return fmt;
// };

//为数组对象增加相应的的方法，调用就变得更加简单了，直接调用数组的removeByValue方法即可删除指定元素
Array.prototype.removeByValue = function(val) {
    for(var i=0; i<this.length; i++) {
      if(this[i] == val) {
        this.splice(i, 1);
        break;
      }
    }
}