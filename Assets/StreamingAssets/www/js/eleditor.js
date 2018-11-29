//标题头图
var titleImg = new Eleditor({
  el: '#titleImg',
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
  placeHolder: '点击上传一张封面图'
});

//标题
// var title = new Eleditor({
//   el: '#title',
//   // upload:{
//   //     // server: '/upload.json',
//   //     server: 'http://apiv2.yangqungongshe.com/files/image.json',
//   //     compress: false,
//   //     fileSizeLimit: 2
//   // },
//   toolbars: [
//     'insertText',
//     'editText',
//     'deleteThis',
//     'cancel'
//   ],
//   placeHolder: '点击编辑标题'
// });


//标题正文
var contentEditor = new Eleditor({ 
    el: '#contentEditor',
    upload:{
        // server: '/upload.json',
        server: 'http://apiv2.yangqungongshe.com/files/image.json',
        compress: false,
        fileSizeLimit: 2
    },
    toolbars: [
      'insertText',
      'editText',
      'insertImage',
      // 'insertLink',
      // 'deleteBefore',
      // 'deleteAfter',
      // 'insertHr',
      'deleteThis',

      //自定义一个按钮
      // {
      //     id: 'changeIndent',
      //     tag: 'p,img', //指定P标签操作，可不填
      //     name: '缩进',
      //     handle: function(select, controll){//回调返回选择的dom对象和控制按钮对象
      //       var _$ele = $(select),
      //         _$controll = $(controll);
      //       _$controll.html(_$ele.css('text-indent') != '0px' ? '缩进' : '还原缩进');
      //       _$ele.css('text-indent', _$ele.css('text-indent') == '0px' ? '5em' : '0px');
      //     }
      // },

      //自定义按钮，该按钮只有在编辑IMG标签时才会显示
      {
          id: 'rotateImage',
          tag: 'img', //指定IMG标签操作，可不填
          name: '反转图片',
          handle: function(select, controll){
            var _$ele = $(select),
              _$controll = $(controll);
            if( _$ele.attr('transform-rotate') != '180deg' ){
              _$controll.html('还原图片');
                _$ele.attr('transform-rotate', '180deg').css('transform', 'rotate(180deg)');
            }else{
              _$controll.html('反转图片');
              _$ele.removeAttr('transform-rotate').css('transform', 'rotate(0)');
            }
          }
        },
        'cancel'
    ],
    placeHolder: '点击编辑正文内容'
});

