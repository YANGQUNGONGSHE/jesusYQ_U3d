Vue.http.headers.common['Content-Type'] = 'application/json';
Vue.http.headers.common['Accept'] = 'application/json';
var app = new Vue({
    el: "#app",
    data: {
        commentsId: request['commentsId'],
        postId: request['postId'],
        repliesList: [],
        parentReplyObj: {},
        authorInfo: {},
        ssid: ""
    },
    mounted: function () {
        this.fetchReplies(); // 帖子回复的回复
        this.fetchComments(); // 帖子回复
    },
    filters: {
        repliesDate: function (date) {
            var timestamp = date;
            var newDate = new Date();
            newDate.setTime(timestamp * 1000);
            var getLastDate = newDate.format('yyyy-MM-dd hh:mm:ss');
            return getLastDate.substring(5, 10)
        }
    },
    methods: {
        fetchReplies: function () {
            console.info(this.commentsId)
            this.$http.get('http://apiv2.yangqungongshe.com/replies/query/byparent', {
                params: {
                    "parentid": this.commentsId,
                },
                headers: {
                    "X-ss-opt": "perm",
                    "X-ss-pid": this.ssid
                }
            }).then(function (res) {
                console.info('fetchReplies=>', res);
                this.repliesList = res.body.Replies;
            })
        },
        fetchComments: function () {
            var self = this;
            this.$http.get('http://apiv2.yangqungongshe.com/comments/query/byparent', {
                params: {
                    "parentid": this.postId,
                },
                headers: {
                    "X-ss-opt": "perm",
                    "X-ss-pid": this.ssid
                }
            }
            ).then(function (res) {
                var self = this;
                console.info('fetchComments=>', res);
                var commentsList = res.body.Comments;
                $.each(commentsList, function (i, comment) {
                    if (commentsList[i].Id == self.commentsId) {
                        self.parentReplyObj = comment;
                        self.authorInfo = self.parentReplyObj.User;
                    }
                })
                console.info('parentReplyObj=>', self.parentReplyObj);
            })
        },
        clickLike: function (id, yesVoted) {
            var self = this;
            console.info(id)
            console.info(yesVoted)
            if (!yesVoted) {
                this.$http.post( // 增加
                    "http://apiv2.yangqungongshe.com/votes",
                    { "ParentType": "回复", "ParentId": id, "Value": !yesVoted },
                    {
                        emulateJSON: true,
                        headers: {
                            "X-ss-opt": "perm",
                            "X-ss-pid": this.ssid
                        }
                    }
                ).then(function (res) {
                    console.info('clickLike=>', res);
                    this.fetchReplies();
                }, function (res) {
                    console.info(res);
                })
            } else {
                this.$http.delete( // 删除
                    "http://apiv2.yangqungongshe.com/votes", {
                        params: {
                            "ParentType": "回复",
                            "ParentId": id
                        },
                        headers: {
                            "X-ss-opt": "perm",
                            "X-ss-pid": this.ssid
                        }
                    }
                ).then(function (res) {
                    this.fetchReplies();
                }, function (res) {
                    console.info(res);
                })
            }
        },
        clickParentLike: function (id, yesVoted) {
            console.info(id);
            console.info(yesVoted);
            var self = this;
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
                    this.fetchComments()
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
                    this.fetchComments();
                }, function (res) {
                    console.info(res);
                })
            }
        }
    }
})

function getUnityData(ssid) {
    app.ssid = ssid;
}
