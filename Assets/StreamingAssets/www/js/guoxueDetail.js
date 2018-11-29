
var volumeNumber = parseInt(request['VolumeNumber']);
// var volumeLength = parseInt(request['VolumeLength']);
var chapterNumber = parseInt(request['ChapterNumber']);
var chapterLength = parseInt(request['ChapterLength']);
volumeNumber = volumeNumber ? volumeNumber : 1;
chapterNumber = chapterNumber ? chapterNumber : 1;
chapterLength = chapterLength ? chapterLength : 1;
// console.info(volumeNumber);

var app = new Vue({
    el: '#app',
    data: {
        isShowAc: true,
        // isShowSelect: true,
        bookId: '',
        volumeList: [],
        volumeIdx: volumeNumber ? volumeNumber - 1 : 0, //下标
        volumeNum: volumeNumber ? volumeNumber : 1, //下标+1
        chapterList: [],
        chapterIdx: chapterNumber ? chapterNumber - 1 : 0,
        chapterNum: chapterNumber ? chapterNumber : 1,
        isShowSelect: true, //新约旧约
        val: '',
        lastRead: '', //上次读到
        lastReadVolumeTitle: '',
        lastReadChapterTitle: '',
        ssid: '',
    },
    mounted: function () {
        setTimeout(() => {
            console.info(this.volumeNum);
            console.info(this.volumeIdx)
            // alert('22')
            this.fetchVolume();
        }, 250);
    },
    filters: {

    },
    methods: {
        fetchVolume: function () { // 卷
            this.$http.get('http://apiv2.yangqungongshe.com/books/{BookId}/volumes/query', {
                params: {
                    "BookId": this.bookId,
                }
            }).then(function (res) {
                console.info(res);
                this.volumeList = res.body.Volumes;
                this.fetchChapter(this.volumeNum); // 初始化章
                this.srcoll(this.volumeIdx);
                this.getLastRead();// 上次阅读查询
            })
        },
        goChapter: function (index, title) { // 点击卷至章
            this.chapterIdx = 0; // 卷变化时，章下标重新初始化
            this.isShowAc = false;
            this.volumeIdx = index; // active
            this.volumeNum = index + 1;
            if (index < 39) {
                this.isShowSelect = true;
            } else {
                this.isShowSelect = false;
            }
            this.fetchChapter(this.volumeNum);
            this.srcoll(this.chapterIdx);
            window.location.href = 'uniwebview://selectedVolume?msg='+title;
        },
        fetchChapter: function (volumeNum) { // 查询章（根据下标查询）
            this.$http.get("http://apiv2.yangqungongshe.com/books/{BookId}/volumes/{VolumeNumber}/chapters/query", {
                params: {
                    "BookId": this.bookId,
                    "VolumeNumber": volumeNum
                }
            }).then(function (res) {
                console.info(res);
                this.chapterList = res.body.Chapters;
                // this.lastRead = this.chapterList[chapterNumber - 1].Title; //上次读到
            })
        },
        //旧约
        goOld: function () {
            this.isShowSelect = true;
            this.srcoll(0);
        },
        //新约
        goNew: function () {
            this.isShowSelect = false;
            this.srcoll(39);
        },
        srcoll: function (idx) { //滚动
            // $(".content-detail").scrollTop(80)
            $(".content-detail").scrollTo({ toT: idx * 50.5 }); //zepto插件
        },
        clickVolume: function () {
            this.isShowAc = true;
            this.srcoll(this.volumeNum);
        },
        // clickChapter: function () { //点击章
        //     this.isShowAc = false;
        //     this.srcoll(this.chapterIdx);
        // },
        getLastRead: function () {
            console.info(this.bookId);
            this.$http.get('http://apiv2.yangqungongshe.com/chapterreads/last', {
                params: {
                    "bookid": this.bookId,
                },
                headers: {
                    "X-ss-opt": "perm",
                    "X-ss-pid": this.ssid,
                }
            }).then(function (res) {
                console.info('rreess=>', res);
                var resData = res.body.ChapterRead;
                this.lastReadVolumeTitle = resData.Volume.Title;
                this.lastReadChapterTitle = resData.Chapter.Title;
                this.lastRead = resData.Volume.Title + resData.Chapter.Title;
                window.location.href = 'uniwebview://selectedLastRead?msg='+this.lastReadVolumeTitle;
            })
        },
        goOnRead: function () { //点击继续阅读
           // window.location.href = 'uniwebview://selectedLastRead?msg='+this.lastReadVolumeTitle;
            console.info(volumeNumber, chapterNumber);
            if (this.lastRead) {
                window.location.href = "texts.html?BookId=" + this.bookId + "&VolumeNumber=" + volumeNumber + "&VolumeLength=" + this.volumeList.length + "&ChapterNumber=" + chapterNumber + "&ChapterLength=" + chapterLength;
                // setTimeout(()=>{ 
                //     window.location.href = "texts.html?BookId=" + this.bookId + "&VolumeNumber=" + volumeNumber + "&VolumeLength=" + this.volumeList.length + "&ChapterNumber=" + chapterNumber + "&ChapterLength=" + chapterLength;
                // }, 100)
                console.log(123,this.lastReadVolumeTitle);
            } else {
                return ;
            }
            
            //上次阅读时，书id不变，卷长度不变。
            // window.location.href = "texts.html?BookId=" + this.bookId + "&VolumeNumber=" + volumeNumber + "&VolumeLength=" + this.volumeList.length + "&ChapterNumber=" + chapterNumber + "&ChapterLength=" + chapterLength;
            
        },
        fetchParagraphList: function () { //关键字查询及跳转
            console.info(this.val);
            var obj = {};
            obj.BookId = this.bookId;
            obj.Contentfilter = this.val;

            if (window.localStorage) {
                window.localStorage.setItem("item", JSON.stringify(obj));
                window.location.href = "textsFilter.html";
            } else {
                alter("请使用高级浏览器！")
            }
        },
        goParagraph: function (index) { // 跳转到节
            this.chapterIdx = index; // active
            this.chapterNum = index + 1;
            window.location.href= "texts.html?BookId=" + this.bookId + "&VolumeNumber=" + this.volumeNum + "&VolumeLength=" + this.volumeList.length + "&ChapterNumber=" + this.chapterNum + "&ChapterLength=" + this.chapterList.length;
            // window.location.href = "texts.html?BookId=" + this.bookId + "&VolumeNumber=" + this.volumeNum + "&ChapterNumber=" + this.chapterNum;
        }
    },
})

function getUnityData(ssid, bookId) {
    // alert('11')
    app.ssid = ssid;
    app.bookId =  bookId;
}
