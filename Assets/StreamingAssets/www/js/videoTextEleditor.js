var contentEditor = new Eleditor({
    el: '#contentEditor',
    upload:{
        // server: '/upload.json',
        server: 'http://apiv2.yangqungongshe.com/files/image.json',
        compress: false,
        fileSizeLimit: 2
    },
    toolbars: [
      'insertImage',
      'deleteThis',
      'cancel'
    ],
    placeHolder: '点击添加一张视频封面'
});

// var  reg = /<[^>]+>/g;
// return reg.test(htmlStr);

function getEditorHtml () {
    var videoImg = $("#contentEditor").find('img:first').attr('src'); //封面
    videoImg = videoImg ? videoImg : '';
    var videoSrc = $('#videoSrc').val().replace(/\"/g, "'"); //视频地址如：<iframe width="640" height="498" src="http://v.qq.com/iframe/player.html?vid=q0167swo7g2&tiny=0&auto=0"></iframe>
    console.info('videoSrc', videoSrc);
    console.info(videoSrc.indexOf('<iframe'));
    
    var titleTxt = $("#titleTxt").val();  //标题
    var contTxt = $("#contTxt").val();  //正文
    var summary = contTxt; //摘要
    var summaryLength = summary.length; // 摘要截取50
    if (summaryLength < 50) {
        summary = summary.substring(0, summaryLength) 
    } else {
        summary = summary.substring(0, 50) + '...'
    }
    console.info('videoImg', videoImg);
    console.info('titleTxt', titleTxt);
    console.info('summary', summary);
    console.info('contTxt', contTxt);

    if (videoSrc.indexOf('<iframe') != -1) { //判断是否含iframe标签
        console.info(true)
        $.ajax({ // 发布请求
            type: 'post',
            url: 'http://apiv2.yangqungongshe.com/posts',
            headers:{"Content-Type":"application/json", "Accept":"application/json", "X-ss-opt":"perm", "X-ss-pid":this.ssid},
            data: '{"SourcePictureUrl": "'+ videoImg+'", "Title": "'+titleTxt+'", "Summary": "'+summary+'", "ContentType": "视频", "Content": "'+contTxt+'", "AutoPublish":true, "ContentUrl":"'+videoSrc+'"}',
            dataType: "json",
            success: function (res) {
                console.info('res=>', res);
                window.location.href = 'uniwebview://preach_publish?succ=1';
            },
            error: function (res) {
                console.info('error=>', res.responseText.Message);
                window.location.href = 'uniwebview://preach_publish?succ=0&msg='+res.responseJSON.ResponseStatus.Message;
            }
        }) 
    } else {    
        console.info(false)
        layer.open({
            content: '请输入正确的视频地址！'
            ,skin: 'msg'
            ,time: 2 //2秒后自动关闭
        });
    }
}

function getUnityData(ssid) {
    this.ssid = ssid;
}