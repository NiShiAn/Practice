require.config({
    // baseUrl: "/scripts/plus",//
    //deps: ['require-css'],
    paths: {
        //"require-css": "css.min",
        jquery: "//libs.baidu.com/jquery/2.0.3/jquery",
        pinyin_dict_firstletter: "pinyin/dict/pinyin_dict_firstletter",
        pinyin_dict_withtone: "pinyin/dict/pinyin_dict_withtone",
        pinyin: "pinyin/pinyinUtil",
        ngy: "test"
        //select2: "select2/select2.min"
    },
    //map: {
    //    '*': {
    //        css: '/scripts/plus/css.min.js'
    //    }
    //},
    shim: {
        ngy: {
            exports: "NGY"
        },
        pinyin: {
            deps: ["pinyin_dict_firstletter", "pinyin_dict_withtone"],
            exports: "pinyin"
        },
        //select2: ["/scripts/plus/select2/select2.min.css"]
    },

});