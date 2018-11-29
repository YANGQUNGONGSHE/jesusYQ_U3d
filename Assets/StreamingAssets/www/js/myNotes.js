Vue.http.headers.common['Content-Type'] = 'application/json';
Vue.http.headers.common['Accept'] = 'application/json';
var app = new Vue({
    el: '#app',
    data: {
        parentId: request['parentId'],
        // isActive: 0,
        // postData: [], //帖子
        // authorInfo: {}, //帖子作者
        // likesList: [], //帖子点赞列表
        myNoteList: [],
        ssid: ""
    },
    mounted: function () {
        this.fetchMyNoteList();
    },
    filters: {
        noteDate: function (date) {
            var timestamp = date;
            var newDate = new Date();
            newDate.setTime(timestamp * 1000);
            var getLastDate = newDate.format('yyyy/MM/dd hh:mm:ss');
            return getLastDate.substring(0, 10)
        }
    },
    methods: {
        //请求我的笔记数据
        fetchMyNoteList: function () {
            this.$http.get('http://apiv2.yangqungongshe.com/comments/query/byparent', {
                params: {
                    "parentid": this.parentId,
                    "ismine": true,
                },
                headers: {
                    "X-ss-opt": "perm",
                    "X-ss-pid": this.ssid
                }
            }).then(function (res) {

                console.info('res=>', res);
                this.myNoteList = res.body.Comments;
            })
        },
        //点赞
        clickLike: function (id, yesVoted) {
            var self = this;
            console.info(id)
            console.info(yesVoted)
            if (!yesVoted) {
                this.$http.post( // 增加
                    "http://apiv2.yangqungongshe.com/votes",
                    { "ParentType": "评论", "ParentId": id, "Value": !yesVoted },
                    {
                        emulateJSON: true,
                        headers: {
                            "X-ss-opt": "perm",
                            "X-ss-pid": this.ssid
                        }
                    }
                ).then(function (res) {
                    console.info('clickLike=>', res);
                    this.fetchMyNoteList();
                }, function (res) {
                    console.info(res);
                })
            } else {
                this.$http.delete( // 删除
                    "http://apiv2.yangqungongshe.com/votes", {
                        params: {
                            "ParentType": "评论",
                            "ParentId": id
                        },
                        headers: {
                            "X-ss-opt": "perm",
                            "X-ss-pid": this.ssid
                        }
                    }
                ).then(function (res) {
                    this.fetchMyNoteList();
                }, function (res) {
                    console.info(res);
                })
            }
        },
        delNote: function (noteId) {
            console.info(noteId);
            this.$http.delete( // 删除
                "http://apiv2.yangqungongshe.com/comments/{CommentId}", {
                    params: {
                        "CommentId": noteId
                    },
                    headers: {
                        "X-ss-opt": "perm",
                        "X-ss-pid": this.ssid
                    }
                }
            ).then(function (res) {
                console.info(res);
                this.fetchMyNoteList();
            }, function (res) {
                console.info(res);
            })
        }
    },
})

function getUnityData(ssid) {
    app.ssid = ssid;
}