Vue.http.headers.common['Content-Type'] = 'application/json';
Vue.http.headers.common['Accept'] = 'application/json';
var app = new Vue({
    el: '#app',
    data: {
        bookId: request['BookId'],
        volumeNumber: parseInt(request['VolumeNumber']),
        volumeLength: parseInt(request['VolumeLength']), //xuyang
        chapterNumber: parseInt(request['ChapterNumber']),
        chapterLength: parseInt(request['ChapterLength']), //xuyang
        index: request['index'],
        parentId: request['parentId'],
        textVal: '',
        ssid: "",
        isShowLoading: false,
    },
    mounted: function () {
        // $('.comment').focus();
        setTimeout(function () {
            console.info('键盘弹起啊')
            $('.comment').trigger("click").focus()
        }, 500)
    },
    filters: {

    },
    methods: {
        send: function () {
            this.isShowLoading = true;
            console.info(this.textVal);
            console.log("aaa", this.ssid);
            this.$http.post(
                'http://apiv2.yangqungongshe.com/comments',
                { "ParentType": "节", "ParentId": this.parentId, "Content": this.textVal },
                {
                    emulateJSON: true,
                    headers: {
                        "X-ss-opt": "perm",
                        "X-ss-pid": this.ssid
                    }
                }
            ).then(function (res) {
                this.isShowLoading = false;
                console.info('send=>', res);
                if (this.textVal) {
                    window.location.href = "texts.html?BookId=" + this.bookId + "&VolumeNumber=" + this.volumeNumber +"&VolumeLength=" + this.volumeLength + "&ChapterNumber=" + this.chapterNumber+ "&ChapterLength=" + this.chapterLength + "&index=" + this.index;
                }
                // if (res.ok) {
                //     window.location.href = 'uniwebview://publish_node?succ=1';
                // } else {
                //     window.location.href = 'uniwebview://publish_node?succ=0&msg=' + res.responseJSON.ResponseStatus.Message;
                // }
            })
        }
    },
})

function send() {
    app.send();
}

function getUnityData(ssid) {
    app.ssid = ssid;
}
