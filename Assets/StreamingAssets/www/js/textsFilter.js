Vue.http.headers.common['Content-Type'] = 'application/json';
Vue.http.headers.common['Accept'] = 'application/json';

// var bookId = request['BookId']; //书id
// var volumeNumber = request['VolumeNumber']; //卷index
// var volumeLength = request['VolumeLength']; //卷length
// var chapterNumber = request['ChapterNumber']; //章index
// var capterLength = request['ChapterLength']; //章length

// console.info(volumeLength,capterLength);
var obj = JSON.parse(localStorage.getItem("item"));

console.info(obj);
// console.info(localStorage.getItem("item"))
console.info(obj.BookId, obj.Contentfilter);
var app = new Vue({
    el: '#app',
    data: {

        textList: [],
        noData: false,
    },
    mounted: function () {
        this.fetchData();
    },
    filters: {
       
    },
    methods: {
        fetchData: function () {
            var self = this;
            this.$http.get('http://apiv2.yangqungongshe.com/books/{BookId}/paragraphs/query', {
                params: {
                    "BookId": obj.BookId, 
                    "contentfilter": obj.Contentfilter,
                    "limit": 25,
                }
            }).then(function (res) {

                console.info(res);
                this.textList = res.body.Paragraphs;
                if (this.textList.length == 0) {
                    this.noData = true;
                }
            })
        },
        goChapter: function (volumeNum, chapterNum) {
            // localStorage.removeItem("item");
            window.location.href= "texts.html?BookId=" + obj.BookId + "&VolumeNumber=" + volumeNum + "&ChapterNumber=" + chapterNum;
        }
    } 
})

