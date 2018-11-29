
Vue.http.headers.common['Content-Type'] = 'application/json';
Vue.http.headers.common['Accept'] = 'application/json';

// var readData = {};
// var readData = JSON.parse(localStorage.getItem("readData")) ? JSON.parse(localStorage.getItem("readData")) : {};
var obj = {};
console.info('obj=>', obj);
var paragraphIdx = parseInt(request['index']); // addNote.html跳转过来index
paragraphIdx = paragraphIdx ? paragraphIdx : -1;
var app = new Vue({
    el: '#app',
    data: {
        bookId: request['BookId'],
        volumeNumber: parseInt(request['VolumeNumber']),
        volumeLength: parseInt(request['VolumeLength']), //xuyang
        chapterNumber: parseInt(request['ChapterNumber']),
        chapterLength: parseInt(request['ChapterLength']), //xuyang
        index: paragraphIdx, //xuyang
        // noteSwitch: true, //是否从addNote.html跳转过来（控制开关）

        isShowAc: true, //切换注释和笔记
        isStartEnd: true, //开始和结束
        isBeginRead: false, //是否已经开始阅读打卡
        textIdx : 0, // 点击注释和笔记，把当前textList的index赋给它
        isShowNoData: false, //暂无数据
        isShowNoteNoData: false, //笔记暂无数据
        showLoading: true, //加载gif
        isShowLoading: false, //切换时，加载gif显示
        // showAnnotationAndNote: false, //是否显示注释和笔记，false为否
        noteListIdx: 0,
        entitiesState: [], // xuyang
        acIconIdx: 0, //icon
        // childState: [], // xuyang
        contList:[{content:'注释'}, {content:'笔记'}],
        number: 0,
        isShowShare: false,

        // acChapterParagraphs: [], //active章所有节
        shareArr: [], //分享选中的节id数组
        textList: [],

        // startVolumeNum: '', //开始卷
        // startChapterNum: '', //开始章
        // startParagraph: '', //开始节
        // // endVolumeNum: '', //结束卷
        // endChapterNum: '', //结束章
        // endParagraph: '', //结束节
        endIdStr: '', //返回目录请求需要的id
        isShowRange: false, //滑块部分
        isFinishRead: false, //是否结束打卡
        isShowFontSize: true, //控制字体大小
        ssid: ""
    },
    watch: {
        volumeNumber: function(val) {
            this.$http.get('http://apiv2.yangqungongshe.com/books/{BookId}/volumes/{VolumeNumber}', {
                params: {
                    "BookId": this.bookId,
                    "VolumeNumber": this.volumeNumber,
                }
            }).then(function(res) {
                var title = res.body.Volume.Title;
                window.location.href = 'uniwebview://selectedVolume?msg='+title;
            });
        }
    },
    mounted: function () {
        // this.fetchNoteList();
        this.fetchData();
    },
    filters: {
        noteDate: function (date) {
            var timestamp = date;
            var newDate = new Date();
            newDate.setTime(timestamp * 1000);
            var getLastDate = newDate.format('yyyy-MM-dd hh:mm:ss');
            return getLastDate.substring(5, 10)
        }
    },
    methods: {
        isShowRead: function () {
            if (!this.isFinishRead) { //如果已经打卡结束，不显示"结束"打卡icon
                if (obj != null || obj != {}) {
                    if (parseInt(obj.startVolumeNum) < this.volumeNumber) {
                        this.isStartEnd = false;
                    } else if (parseInt(obj.startVolumeNum) == this.volumeNumber) {
                        if (parseInt(obj.startChapterNum) <= this.chapterNumber) {
                            this.isStartEnd = false;
                        } else {
                            this.isStartEnd = true;
                        }
                    } else {
                        this.isStartEnd = true;
                    }
                }
            }
        },
        fetchData: function () {
            // layer.open({ // 弹出层
            //     type: 2
            //     // ,content: '加载中'
            // });
            this.isShowLoading = true; //打开加载gif
            console.info(this.volumeNumber , this.volumeLength , this.chapterNumber , this.chapterLength)
            var self = this;
            this.$http.get('http://apiv2.yangqungongshe.com/books/{BookId}/volumes/{VolumeNumber}/chapters/{ChapterNumber}/paragraphs/query', {
                params: {
                    "BookId": this.bookId,
                    "VolumeNumber": this.volumeNumber,
                    "ChapterNumber": this.chapterNumber,
                    // "loadannotations": false
                }
            }).then(function (res) {
                console.info(res);
                this.isShowLoading = false; //关闭加载gif

                // document.documentElement.scrollTop = 0;
                // var scrollTop = document.documentElement.scrollTop || window.pageYOffset || document.body.scrollTop;
                // scrollTop = 0;
                if (this.index == -1) { // 不是从addNote.html跳转过来
                    $("#container").scrollTo({ toT: 0 }); //zepto插件
                }

                var textArrList = res.body.Paragraphs;


                this.isShowRange = true; //滑块部分在加载完后显示
                this.isShowShare = false; //收藏分享加载完后隐藏
                if (this.isBeginRead) { //小于打卡节所在章，不显示结束打卡icon
                    this.isShowRead();
                }
                //注释icon不显示
                if (this.entitiesState[this.acIconIdx]) {
                    console.info('关闭注释')
                    this.entitiesState[this.acIconIdx] = false;
                }
                //字体大小icon
                if (this.isShowFontSize) {
                    this.clickFontIcon();
                }
                console.info('textList=>', this.textList);
                // window.location.href = 'uniwebview://selectedChapter?msg=' + that.allChapterList[slider.activeIndex].Title
                // window.location.href = 'uniwebview://loadComplete?chapterTitle=' + this.allChapterList[this.chapterNumber-1].Title;
                // layer.closeAll(); // 关闭弹出层
                var allParagraphIdArr = []; //所有节id数组  （-----所有节的icon显示我的笔记功能-----）
                var allParagraphIdStr = ""; //所有节字符串
                this.classMap = ['aaa','bbb','ccc'];
                $.each(textArrList, function (i, item) { // 向数据里新增箭头或笔记icon
                    item.isShowArrow = false; // 新增一个isShowArrow属性并赋值 是否箭头(默认隐藏箭头)
                    allParagraphIdArr.push(item.Id); //所有节id数组
                    allParagraphIdStr = allParagraphIdArr.join(','); //所有节字符串
                    item.textColor = false; // 新增一个textColor属性
                    item.noteList = [];

                    // if (!item.Commented) {
                        item.initClassname = self.classMap[0]; // 新增一个initClassname属性并赋值 笔记icon
                    // } else {
                    //     item.initClassname = self.classMap[1];
                    // }

                })
                console.info('allParagraphIdArr=>', allParagraphIdArr);
                console.info('allParagraphIdStr=>', allParagraphIdStr);
                this.$http.get('http://apiv2.yangqungongshe.com/comments/count/byparents', {
                    params: {
                        "parentids": allParagraphIdStr,
                        "ismine": true
                    },
                    headers: {
                        "X-ss-opt": "perm",
                        "X-ss-pid": this.ssid
                    }
                }
                ).then(function (res) {
                    console.info('res=>', res);
                    var commentsCountList = res.body.ParentsCounts; //所有笔记(评论)
                    var commentsCountArr = [];
                    $.each(commentsCountList, function (i, item) {
                        if (commentsCountList[i].CommentsCount > 0) {
                            console.info(i);
                            commentsCountArr.push(i);
                        }
                    })
                    console.info('commentsCountArr=>', commentsCountArr);
                    $.each(textArrList, function (i, item) {
                        $.each(commentsCountArr, function (i2, item2) {
                            if (textArrList[i].Id == item2) {
                                textArrList[i].initClassname = 'bbb';
                            }
                        })
                    })
                    this.textList = textArrList;
                    console.info('textList=>', this.textList);
                    //增加笔记后模拟两个点击事件
                    if (this.index >= 0) { //是否从addNote.html跳转过来
                        this.clickShowNoteCont(this.index, this.textList[this.index].Number);
                        setTimeout(function() {
                            self.clickNote(self.textList[self.index], self.index, 1); 
                        }, 200)
                     }
                })
            })

            this.$http.get('http://apiv2.yangqungongshe.com/books/{BookId}/volumes/{VolumeNumber}/chapters/{ChapterNumber}', {
                params: {
                    "BookId": this.bookId,
                    "VolumeNumber": this.volumeNumber,
                    "ChapterNumber": this.chapterNumber,
                }
            }).then(function(res) {
                var title = res.body.Chapter.Title;
                this.endIdStr =  this.bookId + '-' + this.volumeNumber + '-' + this.chapterNumber;
                window.location.href = 'uniwebview://loadComplete?chapterTitle='+title + '&endIdStr=' + this.endIdStr;
            });
        },

        // 阅读开始
        goStart: function (id) {
            console.info('开始id=>', id);
            this.isStartEnd = false;
            this.isBeginRead = true;
            var arr = id.split("-");
            console.info(arr);

            obj.startVolumeNum = arr[1];
            obj.startChapterNum = arr[2];
            obj.startParagraph = arr[3];
            obj.chapterLength = this.chapterLength;
            // window.localStorage.setItem("readData", JSON.stringify(readData));
            // console.info('readData=>', readData);
        },
        // 阅读结束
        goEnd: function (id) {
            var self = this;
            console.info('结束id=>', id);

            layer.open({
                // style: 'border:none; background-color:skyblue; color:#1f1f1f;',
                // title: [
                //     '我是标题',
                //     'background-color:#8DCE16; color:#fff;'
                // ],
                content:'是否需要结束？',
                btn: ['确定结束', '再阅读一遍'],
                // shadeClose: false,
                yes: function (index) {//结束回调
                    console.info('yes');
                    // window.localStorage.removeItem("readData");
                    var arr = id.split("-");
                    console.info(arr);
                    console.info('obj=>', obj);
                    self.$http.post(
                        "http://apiv2.yangqungongshe.com/views/paragraphs",
                        {"BookId": self.bookId, "VolumeNumber": obj.startVolumeNum, "BeginChapterNumber": obj.startChapterNum, "BeginParagraphNumber": obj.startParagraph, "EndChapterNumber": arr[2], "EndParagraphNumber": arr[3]},
                        {
                            emulateJSON: true,
                            headers: {
                                "X-ss-opt":"perm",
                                "X-ss-pid": self.ssid
                            }
                        }
                    ).then(function (res) {

                        console.info( res);

                        self.isStartEnd = true; //打卡icon显示情况
                        self.isFinishRead = true; //打卡已经结束
                        layer.close(index);
                        layer.open({
                            content: '打卡成功！'
                            ,skin: 'msg'
                            ,time: 2 //2秒后自动关闭
                        });
                        // window.location.href= "classics.html"; //待做

                    }, function (error) {

                        console.info(error);
                    })
                },
                no: function (index) {//再阅读一遍回调

                    console.info('no');
                    layer.close(index);
                    console.info(obj);
                    self.isStartEnd = true;
                    self.isBeginRead = false;
                    self.bookId = self.bookId;
                    self.volumeNumber = parseInt(obj.startVolumeNum);
                    self.volumeLength = self.volumeLength;
                    self.chapterNumber = parseInt(obj.startChapterNum);
                    self.chapterLength = obj.chapterLength;
                    self.fetchData();
                    // window.location.href= "texts.html?BookId=" + self.bookId + "&VolumeNumber=" + obj.startVolumeNum + "&VolumeLength=" + self.volumeLength + "&ChapterNumber=" + obj.startChapterNum + "&ChapterLength=" + obj.chapterLength;
                    // window.location.href= "texts.html?BookId=" + self.bookId + "&VolumeNumber=" + obj.startVolumeNum + "&ChapterNumber=" + obj.startChapterNum;
                }
            })
        },

        //icon 显示
        clickShowNoteCont: function (index, itemNum) {
            var state = this.entitiesState;
            this.$set(state, index, !state[index]);
            for (var a in this.entitiesState) {
                if (a != index) {
                    this.$set(state, a, false);
                }
            }//增加判断，改变同级的展开状态
            this.acIconIdx = index; //节index
            // this.acChapter = chapter; //章
            console.info(this.entitiesState);
            console.info('this.textIdx', this.textIdx);
            console.info('index', index);
            if (this.textIdx == index) {
                // this.showOnlyOne(index);
                if (this.textList[index].isShowArrow === false) {
                    this.textList[index].isShowArrow = true;
                    this.textList[index].oldclass = this.textList[index].initClassname; // 新增一个属性oldclass赋于初始值
                    this.textList[index].initClassname = this.classMap[2];
                } else {
                    this.textList[index].isShowArrow = false;
                    if (this.textList[index].oldclass) {
                        this.textList[index].initClassname = this.textList[index].oldclass;
                    }
                }
            } else if (this.textIdx != index ) {
                this.number = 0;
                this.showLoading = true;
                this.isShowNoData = false;
                this.textList[this.textIdx].isShowArrow = false;
                if (this.textList[this.textIdx].oldclass) {
                    this.textList[this.textIdx].initClassname = this.textList[this.textIdx].oldclass;
                }

                this.textIdx = index;
                this.textList[index].isShowArrow = true;
                this.textList[index].oldclass = this.textList[index].initClassname; // 新增一个属性
                this.textList[index].initClassname = this.classMap[2];
            }
            console.info(this.entitiesState[index])
            if (this.entitiesState[index]) {
                if (this.textList[index].Annotations.length == 0) { //如果注释不存在，请求
                    this.$http.get('http://apiv2.yangqungongshe.com/books/{BookId}/volumes/{VolumeNumber}/chapters/{ChapterNumber}/paragraphs/{ParagraphNumber}/annotations/query', {
                        params: {
                            "BookId": this.bookId,
                            "VolumeNumber": this.volumeNumber,
                            "ChapterNumber": this.chapterNumber,
                            "ParagraphNumber": itemNum,
                        }
                    }
                    ).then(function (res) {
                        this.showLoading = false;
                        console.info('res=>', res);
                        this.textList[index].Annotations = res.body.ParagraphAnnotations;

                        if (this.textList[index].Annotations.length == 0) {
                            this.isShowNoData = true
                        } else {
                            this.isShowNoData = false
                        }
                    })
                } else { // 如果注释存在，不再请求了
                    this.showLoading = false;
                    this.isShowNoData = false;
                }
            }
        },

        clickLike: function (id, yesVoted, index, item) { //点赞笔记
            console.info(id); //笔记id
            console.info(yesVoted);
            console.info(index); //节index
            console.info(item.Id); //节id
            var self = this;
            if (!yesVoted) {
                this.$http.post( // 增加
                    "http://apiv2.yangqungongshe.com/votes",
                    {"ParentType": "评论","ParentId": id, "Value": !yesVoted},
                    {
                        emulateJSON: true,
                        headers: {
                            "X-ss-opt":"perm",
                            "X-ss-pid": this.ssid
                        }
                    }
                ).then(function (res) {
                    console.info('clickLike=>', res);
                    this.fetchNoteList(item, index);
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
                            "X-ss-opt":"perm",
                            "X-ss-pid": this.ssid
                        }
                    }
                ).then(function (res) {
                    console.info(res)
                    this.fetchNoteList(item, index);
                }, function (res) {
                    console.info(res);
                })
            }
        },
        // 创建笔记
        addNote: function (itemId, index) {

            // window.location.href= "addNote.html?parentId=" + itemId;
            window.location.href = "addNote.html?BookId=" + this.bookId + "&VolumeNumber=" + this.volumeNumber +"&VolumeLength=" + this.volumeLength + "&ChapterNumber=" + this.chapterNumber+ "&ChapterLength=" + this.chapterLength + "&parentId=" + itemId + "&index=" + index;
        },
        // 我的笔记
        myNotes: function (itemId) {

            window.location.href= "myNotes.html?parentId=" + itemId;
        },

        //滑动加载更多数据(点击笔记)
        clickNote: function (item, index, idx) {
            var self = this;
            console.info(item);
            console.info(index);
            this.number = idx;
            if (idx == 1) { //当idx等于1时，初始化gif和noData

                if (item.noteList.length == 0) { //如果noteList
                    this.showLoading = true;
                    this.isShowNoteNoData = false;
                    this.fetchNoteList(item, index);
                } else { //如果noteList存在，不再请求了
                    this.showLoading = false;
                    this.isShowNoteNoData = false;
                }
                if (self.index >= 0) { //是否从addNote.html跳转过来
                    setTimeout(function(){ // 需要延迟才能获取到高度
                        var paragraphList = document.getElementsByClassName('yang');//可能有兼容问题
                        var leng = paragraphList.length;
                        var paragraphListHeight = 0;
                        console.info(paragraphList);
                        for (var i=0; i<leng; i++) { // 获取小于当前笔记的所有节高度
                            if (i<self.index) {
                                console.info(paragraphList[i].offsetHeight)
                                paragraphListHeight += paragraphList[i].offsetHeight + 4;
                            }
                        }
                        console.info(paragraphListHeight);
                        $("#container").scrollTo({ toT: paragraphListHeight - 4 });
                        self.index = -1; // 关闭笔记开关（节下标）
                    }, 300)
                }
            }
        },
        //查询笔记
        fetchNoteList: function (item, index) {
            var self = this;
            this.$http.get('http://apiv2.yangqungongshe.com/comments/query/byparent', {
                params: {
                    "parentid": item.Id,
                    "ismine": true
                },
                headers: {
                    "X-ss-opt":"perm",
                    "X-ss-pid": this.ssid
                }
            }
            ).then(function (res) {

                console.info('fetchNoteList=>', res);
                // console.info('noteList=>', self.textList[index].noteList);
                // self.textList[index].noteList = res.body.Comments;
                // console.info('noteList=>', self.textList[index].noteList);
                item.noteList = res.body.Comments;
                this.showLoading = false;
                if (item.noteList.length == 0) {
                    this.textList[index].oldclass = 'aaa';
                    this.isShowNoteNoData = true;
                } else {
                    this.isShowNoteNoData = false;
                }
                layer.closeAll(); // 关闭弹出框
            })
        },
        //删除我的笔记
        delMyNote: function (id, item, paragraphIdx) {
            var self = this;
            console.info(id, paragraphIdx);
            layer.open({
                content: '是否删除该评论？',
                btn: ['确定', '再想想'],
                // shadeClose: false,
                yes: function (index) {
                    self.$http.delete("http://apiv2.yangqungongshe.com/comments/{CommentId}", {
                            params: {
                                // "ParentType": "评论",
                                "CommentId": id
                            },
                            headers: {
                                "X-ss-opt": "perm",
                                "X-ss-pid": self.ssid
                            }
                        }
                    ).then(function (res) {
                        console.info(res)
                        self.fetchNoteList(item, paragraphIdx);

                    }, function (res) {
                        console.info(res);
                    })
                },
                no: function (index) {

                    console.info('no');
                }
            })
        },
        //分享
        clickTxt: function(itemId, index) {

            // console.info(txt.substring(1));
            this.textList[index].textColor = !this.textList[index].textColor;
            this.showShare();
            if (this.textList[index].textColor) {
                this.shareArr.push(itemId);
            } else {
                this.shareArr.removeByValue(itemId); //removeByValue为数组原型上新增的方法
            }
            console.info('分享节数组shareArr=>', this.shareArr);
        },
        showShare: function () {
            var self = this;
            var leng = this.textList.length;
            for (var i=0; i<leng; i++) {
                if (this.textList[i].textColor == true) {
                    console.info(i);
                    this.isShowShare = true
                    break;
                } else {
                    this.isShowShare = false;
                }
            }
            console.info(this.isShowShare)
        },
        //收藏句子
        bookmark: function () {
            // console.info(this.shareArr)
            this.$http.post(
                "http://apiv2.yangqungongshe.com/bookmarks/paragraphs",
                {"ParagraphIds": JSON.stringify(this.shareArr)},
                {
                    emulateJSON: true,
                    headers: {
                        "X-ss-opt":"perm",
                        "X-ss-pid": this.ssid
                    }
                }
            ).then(function (res) {

                console.info('res=>', res);
                layer.open({
                    content: '收藏成功！'
                    ,skin: 'msg'
                    ,time: 2 //2秒后自动关闭
                });

            }, function (error) {

                console.info(error);
                layer.open({
                    content: '收藏失败！'
                    ,skin: 'msg'
                    ,time: 2 //2秒后自动关闭
                });
            })
            $.each(this.textList, function (i, item) {
                item.textColor = false;
            })
            this.isShowShare = false;
        },
        //分享
        myShare: function () {
            //----------------------------未完成-----------------------------
            console.info('分享')
        },
        //返回目录
        goBack: function () {
            console.info('ggggooooback')
            this.endIdStr =  this.bookId + '-' + this.volumeNumber + '-' + this.chapterNumber
            console.info(this.endIdStr)
            this.$http.post( // 增加
                "http://apiv2.yangqungongshe.com/chapterreads",
                { "ChapterId": this.endIdStr},
                {
                    emulateJSON: true,
                    headers: {
                        "X-ss-opt": "perm",
                        "X-ss-pid": this.ssid
                    }
                }
            ).then(function (res) {
                // console.info(endIdStr)
                console.info('ssss=>', res);
                window.location.href = "guoxueDetail.html?BookId=" + this.bookId + "&VolumeNumber=" + this.volumeNumber +"&VolumeLength=" + this.volumeLength + "&ChapterNumber=" + this.chapterNumber+ "&ChapterLength=" + this.chapterLength;
            }, function (res) {
                console.info(res);
            })
            // window.location.href= "guoxueDetail.html?BookId=" + this.bookId + "&VolumeNumber=" + this.volumeNumber + "&VolumeLength=" + this.volumeLength + "&ChapterNumber=" + this.chapterNumber + "&ChapterLength=" + this.chapterLength;
        },
        //隐藏字体大小与否
        clickFontIcon: function () {
            this.isShowFontSize = !this.isShowFontSize;
        },
        //上一章
        prevChapter: function (){
            if (this.volumeNumber >= 1) {

                if (this.chapterNumber > 1) {
                    this.chapterNumber--;
                    // turnRight();
                    this.fetchData();
                } else if (this.chapterNumber == 1) {

                    console.info('已经到了该卷首章了')
                    if (this.volumeNumber > 1) {
                        this.volumeNumber--;
                        this.getCapterList(this.bookId, this.volumeNumber);
                    } else if (this.volumeNumber == 1) {

                        layer.open({
                            content: '已经在首卷首章了！'
                            ,skin: 'msg'
                            ,time: 2 //2秒后自动关闭
                        });
                    }
                }
            }
            this.endIdStr =  this.bookId + '-' + this.volumeNumber + '-' + this.chapterNumber;
            window.location.href = 'uniwebview://endIdStr?msg='+this.endIdStr;
        },

        //下一章
        nextChapter: function (){
            console.info('caonima');
            if (this.volumeNumber <= this.volumeLength) {
                if (this.chapterNumber < this.chapterLength) {
                    this.chapterNumber++;
                    this.fetchData();
                } else {
                    if (this.volumeNumber < this.volumeLength) {

                        console.info('已经到了末章了');
                        this.volumeNumber++;
                        this.getCapterNum(this.bookId, this.volumeNumber);
                    } else if (this.volumeNumber == this.volumeLength) {
                        // this.className = "";   //左滑不展开(已阅读完毕)
                        layer.open({
                            content: '本书已阅读完毕！'
                            ,skin: 'msg'
                            ,time: 2 //2秒后自动关闭
                        });
                    }
                }
            }
            this.endIdStr =  this.bookId + '-' + this.volumeNumber + '-' + this.chapterNumber;
            window.location.href = 'uniwebview://endIdStr?msg='+this.endIdStr;
        },

        //得到卷的章数(右滑) 上一章
        getCapterList: function (bookId, volumeNumber){
            console.info(bookId, volumeNumber)
            this.$http.get('http://apiv2.yangqungongshe.com/books/{BookId}/volumes/{VolumeNumber}/chapters/query', {
                params: {
                    "BookId": bookId,
                    "VolumeNumber": volumeNumber,
                }
            }
            ).then(function (res) {

                console.info(res);
                this.chapterNumber = res.body.Chapters.length;
                this.chapterLength = res.body.Chapters.length;
                this.fetchData(); //查数据
            })
        },
        // 得到卷的章数(左滑) 下一章
        getCapterNum: function (bookId, volumeNumber){

            this.$http.get('http://apiv2.yangqungongshe.com/books/{BookId}/volumes/{VolumeNumber}/chapters/query', {
                params: {
                    "BookId": bookId,
                    "VolumeNumber": volumeNumber,
                }
            }
            ).then(function (res) {

                console.info(res);
                this.chapterLength = res.body.Chapters.length;
                this.chapterNumber = 1;
                this.fetchData(); //查数据
            })
        }
    },
})

function goBack(){
  app.goBack();
}

function getUnityData(ssid) {
    app.ssid = ssid;
}
