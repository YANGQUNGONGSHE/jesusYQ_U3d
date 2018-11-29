Vue.http.headers.common['Content-Type'] = 'application/json';
Vue.http.headers.common['Accept'] = 'application/json';
var basePostUrl = 'http://apiv2.yangqungongshe.com/posts/';

var app = new Vue({
    el: '#app',
    data: {
        _ssid: '',
        _postId: '',
        isActive: false,
        postData: [], //帖子
        authorInfo: {}, //帖子作者
        likesList: [], //帖子点赞列表
        commentsList: [], //帖子评论列表
    },
    mounted: function () {
        setTimeout(() => {
            this.fetchPost(); //查询帖子
            this.fetchLikes(); //查询点赞
            this.fetchComments(); //查询评论
        }, 90);
    },
    watch: {
        isActive: function () {
            this.scroll();
        }
    },
    filters: {
        publishedDate: function (date) { // 帖子发布时间
            console.info('date=>', date);
            var timestamp = date;
            var newDate = new Date();
            newDate.setTime(timestamp * 1000);
            var getLastDate = newDate.format('yyyy-MM-dd hh:mm:ss');
            // console.info(getLastDate);
            console.info(getLastDate.substring(0, 10));
            return getLastDate.substring(0, 10)
        },
        commentDate: function (date) {
            var timestamp = date;
            var newDate = new Date();
            newDate.setTime(timestamp * 1000);
            var getLastDate = newDate.format('yyyy-MM-dd hh:mm:ss');
            return getLastDate.substring(5, 10)
        }
    },
    methods: {
        fetchPost: function () {
            var url = basePostUrl + this._postId;
            this.$http.get(url, {
            }).then(function (res) {
                console.info(res);
                this.postData = res.body.Post;
                this.postData.PictureUrl = this.postData.PictureUrl;
                this.authorInfo = this.postData.Author;
                console.info(this.postData);
                console.info(this.authorInfo);
            })
        },
        fetchLikes: function () {
            this.$http.get('http://apiv2.yangqungongshe.com/likes/query/byparent', {
                params: {
                    "parentid": this._postId,
                    "descending": true
                }
            }).then(function (res) {
                console.info('fetchLikes=>', res);
                this.likesList = res.body.Likes;
            })
        },
        fetchComments: function () {
            this.$http.get('http://apiv2.yangqungongshe.com/comments/query/byparent', {
                params: {
                    "parentid": this._postId,
                    "descending": true
                },
                headers: {
                    "X-ss-opt": "perm",
                    "X-ss-pid": this._ssid
                }
            }
            ).then(function (res) {
                console.info('fetchComments=>', res);
                this.commentsList = res.body.Comments;
            })
        },
        clickLike: function (id, yesVoted) { // 评论里的点赞
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
                            "X-ss-pid": this._ssid
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
                            "X-ss-pid": this._ssid
                        }
                    }
                ).then(function (res) {
                    this.fetchComments();
                }, function (res) {
                    console.info(res);
                })
            }
        },
        //查看回复
        goReply: function (commentsId, postId) {
            console.info(commentsId);
            window.location.href = 'uniwebview://preach_changePage?pageEnum=1';
            window.location.href = 'reply.html?commentsId=' + commentsId + '&postId=' + postId;
        },
        //进行回复
        publishReply: function (commentId, commentName) {
            window.location.href = 'uniwebview://preach_changePage?pageEnum=1&commentId=' + commentId + '&commentName=' + commentName;
        },
        //滚动元素
        scroll: function () {
            var xuyang = document.getElementById("xuyang");
            var scrollH = xuyang.offsetHeight;
            console.info('？？？？？？？？？？？？？？？scrollH=>', scrollH);
            $("#app").scrollTo( {toT : scrollH} );
        }
    },
})

function getUnityData(ssid, postId) {
    console.log("收到PostId:", postId);
    console.log("收到Ssid:", ssid);
    app._ssid = ssid;
    app._postId = postId;
    window.location.href = 'uniwebview://preach_debug_getUnityData?ssid=' + app._ssid + '&postId=' + app._postId;
}

function scrollToComment() {
    app.isActive = true;
    app.scroll();
}