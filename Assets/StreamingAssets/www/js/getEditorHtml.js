
function getEditorHtml() {
    var titleImgStr = $("#titleImg").html().replace(/\"/g, "'"); //头图str
    console.info(titleImgStr)
    var titleImg = $("#titleImg").find('img:first').attr('src'); //头图img
    titleImg = titleImg ? titleImg : '';
    console.info(titleImg);

    var titleStr = $("#title").val();  //标题
    var summary = $("#contentEditor").text(); //摘要
    var summaryLength = summary.length;
    if (summaryLength < 50) {
        summary = summary.substring(0, summaryLength)
    } else {
        summary = summary.substring(0, 50) + '...'
    }
    var editorHtmlStr = $("#contentEditor").html().replace(/\"/g, "'"); //正文
    var titleImgStrArr = titleImgStr.split("p>");
    if (titleStr == "") {
        titleStr = "";
        console.info('请输入正文和标题！')
    }
    if ($('#contentEditor p').hasClass('Eleditor-placeholder')) {
        summary = '';
        editorHtmlStr = '';
    }
    // console.info('titleStr=>', titleStr, "content=>", content, "summary=>", summary);

    $.ajax({ // 发布请求
        type: 'post',
        url: 'http://apiv2.yangqungongshe.com/posts',
        headers:{"Content-Type":"application/json", "Accept":"application/json", "X-ss-opt":"perm", "X-ss-pid":this.ssid},
        data: '{"SourcePictureUrl": "'+ titleImg+'", "Title": "'+titleStr+'", "Summary": "'+summary+'", "ContentType": "图文", "Content": "'+editorHtmlStr+'", "AutoPublish":true}',
        dataType: "json",
        success: function (res) {
            console.info('res=>', res);
            window.location.href = 'uniwebview://preach_publish?succ=1';
        },
        error: function (res) {
            console.log('error=>', res);
            window.location.href = 'uniwebview://preach_publish?succ=0&msg='+res.responseJSON.ResponseStatus.Message;
        }
    })
}

function getUnityData(ssid) {
    this.ssid = ssid;
}
