$.fn.extend({
    /**
     * 文件上传模块
     * RenYongQiang 2019/01/10
     * var obj = {
     *  type: '',                 加载模块,image/files
     *  id: 'filePicker',         id
     *  maxNum: 0,                最大文件数
     *  action: '',               服务路径
     *  form: {},                 参数
     *  callback: function () {}, 回调函数
     *  singleSize: 2,            单文件的最大容量,M
     *  showfile: true,           是否显示文件提醒
     *  delay: 0                  延迟移除提示,ms
     * }
     */
    webUploader(obj) {
        $(this).empty();

        obj._this = $(this);
        switch (obj.type) {
            case 'image': return image(obj);
            case 'files': return files(obj);
            default: return false;
        }
        //上传图片
        function image(obj) {
            //主窗口
            var $wrap = $(
                '<div id="uploader">' +
                '<div class="queueList">' +
                '<div id="dndArea" class="placeholder">' +
                '<div id="filePicker"></div>' +
                '<p>或将照片拖到这里</p>' +
                '</div>' +
                '</div>' +
                '<div class="statusBar" style="display:none;">' +
                '<div class="info"></div>' +
                '<div class="btns">' +
                '<div id="filePicker2"></div>' +
                '<div class="recheckBtn">重新选择</div>' +
                '<div class="uploadBtn">开始上传</div>' +
                '</div>' +
                '</div>' +
                '</div >').appendTo(obj._this);
            // 图片容器
            var $queue = $('<ul class="filelist"></ul>').appendTo($wrap.find('.queueList'));
            // 状态栏，包括进度和控制按钮
            var $statusBar = $wrap.find('.statusBar');
            // 文件总体选择信息。
            var $info = $statusBar.find('.info');
            // 上传按钮
            var $upload = $wrap.find('.uploadBtn');
            //重新选择
            var $recheck = $wrap.find('.recheckBtn');
            // 没选择文件之前的内容。
            var $placeHolder = $wrap.find('.placeholder');
            // 添加的文件数量
            var fileCount = 0;
            // 添加的文件总大小
            var fileSize = 0;
            // 优化retina, 在retina下这个值是2
            var ratio = window.devicePixelRatio || 1;
            // 缩略图大小
            var thumbnailWidth = 110 * ratio;
            var thumbnailHeight = 110 * ratio;
            // 可能有pedding, ready, uploading, confirm, done.
            var state = 'pedding';
            // 所有文件的进度信息，key为file id
            var percentages = {};
            // 判断浏览器是否支持图片的base64
            var isSupportBase64 = (function () {
                var data = new Image();
                var support = true;
                data.onload = data.onerror = function () {
                    if (this.width != 1 || this.height != 1) {
                        support = false;
                    }
                }
                data.src = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==";
                return support;
            })();
            // 检测是否已经安装flash，检测flash的版本
            var flashVersion = (function () {
                var version;
                try {
                    version = navigator.plugins['Shockwave Flash'];
                    version = version.description;
                } catch (ex) {
                    try {
                        version = new ActiveXObject('ShockwaveFlash.ShockwaveFlash').GetVariable('$version');
                    } catch (ex2) {
                        version = '0.0';
                    }
                }
                version = version.match(/\d+/g);
                return parseFloat(version[0] + '.' + version[1], 10);
            })();
            var supportTransition = (function () {
                var s = document.createElement('p').style,
                    r = 'transition' in s ||
                        'WebkitTransition' in s ||
                        'MozTransition' in s ||
                        'msTransition' in s ||
                        'OTransition' in s;
                s = null;
                return r;
            })();

            if (!WebUploader.Uploader.support('flash') && WebUploader.browser.ie) {
                // flash 安装了但是版本过低。
                if (flashVersion) {
                    (function (container) {
                        window['expressinstallcallback'] = function (state) {
                            switch (state) {
                                case 'Download.Cancelled':
                                    alert('您取消了更新！');
                                    break;
                                case 'Download.Failed':
                                    alert('安装失败');
                                    break;
                                default:
                                    alert('安装已成功，请刷新！');
                                    break;
                            }
                            delete window['expressinstallcallback'];
                        };

                        var swf = '~/scripts/libs/plugins/webuploader/expressInstall.swf';
                        // insert flash object
                        var html = '<object type="application/' + 'x-shockwave-flash" data="' + swf + '" ';

                        if (WebUploader.browser.ie) {
                            html += 'classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" ';
                        }

                        html += 'width="100%" height="100%" style="outline:0">' +
                            '<param name="movie" value="' + swf + '" />' +
                            '<param name="wmode" value="transparent" />' +
                            '<param name="allowscriptaccess" value="always" />' +
                            '</object>';

                        container.html(html);

                    })($wrap);

                    // 压根就没有安转。
                } else {
                    $wrap.html('<a href="http://www.adobe.com/go/getflashplayer" target="_blank" border="0"><img alt="get flash player" src="http://www.adobe.com/macromedia/style_guide/images/160x41_Get_Flash_Player.jpg" /></a>');
                }

                return;
            } else if (!WebUploader.Uploader.support()) {
                alert('您的浏览器不支持图片上传！');
                return;
            }
            if (!isSupportBase64) {
                alert('您的浏览器不支持查看缩略图！');
                return;
            }

            // WebUploader实例
            var uploader = WebUploader.create({
                pick: {
                    id: '#filePicker',
                    label: '点击选择图片'
                },
                formData: obj.form,
                dnd: '#dndArea',
                paste: '#uploader',
                swf: '~/scripts/libs/plugins/webuploader/Uploader.swf',
                chunked: false,
                chunkSize: 512 * 1024,
                server: obj.action,
                // runtimeOrder: 'flash',

                accept: {
                    title: 'Images',
                    extensions: 'gif,jpg,jpeg,bmp,png',
                    mimeTypes: 'image/*'
                },

                // 禁掉全局的拖拽功能。这样不会出现图片拖进页面的时候，把图片打开。
                disableGlobalDnd: true,
                fileNumLimit: obj.maxNum,
                fileSizeLimit: 200 * 1024 * 1024, // 200 M
                fileSingleSizeLimit: obj.singleSize * 1024 * 1024 // {0}M
            });
            // 拖拽时不接受 js, txt 文件。
            uploader.on('dndAccept', function (items) {
                var denied = false,
                    len = items.length,
                    i = 0,
                    // 修改js类型
                    unAllowed = 'text/plain;application/javascript ';

                for (; i < len; i++) {
                    // 如果在列表里面
                    if (~unAllowed.indexOf(items[i].type)) {
                        denied = true;
                        break;
                    }
                }

                return !denied;
            });
            //上传返回事件
            uploader.on('uploadSuccess', function (file, response) {
                if (obj.callback && typeof obj.callback == "function") {
                    obj.callback(file, response);
                }
            });
            // “继续添加”按钮，
            uploader.addButton({
                id: '#filePicker2',
                label: '继续添加'
            });
            //上传进度
            uploader.onUploadProgress = function (file, percentage) {
                var $li = $('#' + file.id),
                    $percent = $li.find('.progress span');

                $percent.css('width', percentage * 100 + '%');
                percentages[file.id][1] = percentage;
                updateStatus();
            };
            //排队等待
            uploader.onFileQueued = function (file) {
                fileCount++;
                fileSize += file.size;

                if (fileCount === 1) {
                    $placeHolder.addClass('element-invisible');
                    $statusBar.show();
                }

                addFile(file);
                setState('ready');
                updateStatus();
            };
            //移除附件
            uploader.onFileDequeued = function (file) {
                fileCount--;
                fileSize -= file.size;

                if (!fileCount) {
                    setState('pedding');
                }

                removeFile(file);
                updateStatus();
            };
            //上传状态
            uploader.on('all', function (type) {
                switch (type) {
                    case 'uploadFinished':
                        setState('confirm');
                        break;
                    case 'startUpload':
                        setState('uploading');
                        break;
                    case 'stopUpload':
                        setState('paused');
                        break;
                }
            });
            //错误提示
            uploader.onError = function (code) {
                alert(code);
            };
            //上传
            $upload.on('click', function () {
                if ($(this).hasClass('disabled')) {
                    return;
                }

                if (state === 'ready') {
                    uploader.upload();
                } else if (state === 'paused') {
                    uploader.upload();
                } else if (state === 'uploading') {
                    uploader.stop();
                }
            });
            //重新上传
            $info.on('click', '.retry', function () {
                uploader.retry();
            });

            $upload.addClass('state-' + state);
            updateStatus();
            //重新选择
            $recheck.on('click', function () {
                uploader.destroy();
                obj._this.webUploader(obj);
            });
            // 添加文件
            function addFile(file) {
                var $li = $(
                    '<li id="' + file.id + '">' +
                    '<p class="title">' + file.name + '</p>' +
                    '<p class="imgWrap"></p>' +
                    '<p class="progress"><span></span></p>' +
                    '</li>');
                var $btns = $(
                    '<div class="file-panel">' +
                    '<span class="cancel">删除</span>' +
                    '<span class="rotateRight">向右旋转</span>' +
                    '<span class="rotateLeft">向左旋转</span>' +
                    '</div>').appendTo($li);
                var $prgress = $li.find('p.progress span');
                var $wrap = $li.find('p.imgWrap');
                var $info = $('<p class="error"></p>');
                var showError = function (code) {
                    var text;
                    switch (code) {
                        case 'exceed_size':
                            text = '文件大小超出';
                            break;
                        case 'interrupt':
                            text = '上传暂停';
                            break;
                        default:
                            text = code == 'invalid' || !code
                                ? '上传失败'
                                : code;
                            break;
                    }
                    $li.find('span.success').remove();
                    $info.text(text.slice(0, 7) + (text.length > 7 ? '...' : '')).attr('title', text).appendTo($li);
                };

                if (file.getStatus() === 'invalid') {
                    showError(file.statusText);
                } else {
                    $wrap.text('预览中');
                    uploader.makeThumb(file, function (error, src) {
                        var img;
                        if (error) {
                            $wrap.text('不能预览');
                            return;
                        }
                        if (isSupportBase64) {
                            img = $('<img src="' + src + '">');
                            $wrap.empty().append(img);
                        }
                    }, thumbnailWidth, thumbnailHeight);

                    percentages[file.id] = [file.size, 0];
                    file.rotation = 0;
                }

                file.on('statuschange', function (cur, prev) {
                    if (prev === 'progress') {
                        $prgress.parent().hide();
                        $prgress.hide().width(0);
                    } else if (prev === 'queued') {
                        $li.off('mouseenter mouseleave');
                        $btns.remove();
                    }

                    // 成功
                    if (cur === 'error' || cur === 'invalid') {
                        console.log(file.statusText);
                        showError(file.statusText);
                        percentages[file.id][1] = 1;
                    } else if (cur === 'interrupt') {
                        showError('interrupt');
                    } else if (cur === 'queued') {
                        $info.remove();
                        $prgress.parent().show();
                        $prgress.css('display', 'block');
                        percentages[file.id][1] = 0;
                    } else if (cur === 'progress') {
                        $info.remove();
                        $prgress.parent().show();
                        $prgress.css('display', 'block');
                    } else if (cur === 'complete') {
                        $prgress.parent().hide();
                        $prgress.hide().width(0);
                        $li.append('<span class="success"></span>');
                    }

                    $li.removeClass('state-' + prev).addClass('state-' + cur);
                });

                $li.on('mouseenter', function () {
                    $btns.stop().animate({ height: 30 });
                });

                $li.on('mouseleave', function () {
                    $btns.stop().animate({ height: 0 });
                });

                $btns.on('click', 'span', function () {
                    var index = $(this).index(),
                        deg;

                    switch (index) {
                        case 0:
                            uploader.removeFile(file);
                            return;
                        case 1:
                            file.rotation += 90;
                            break;
                        case 2:
                            file.rotation -= 90;
                            break;
                    }

                    if (supportTransition) {
                        deg = 'rotate(' + file.rotation + 'deg)';
                        $wrap.css({
                            '-webkit-transform': deg,
                            '-mos-transform': deg,
                            '-o-transform': deg,
                            'transform': deg
                        });
                    } else {
                        $wrap.css('filter', 'progid:DXImageTransform.Microsoft.BasicImage(rotation=' + (~~((file.rotation / 90) % 4 + 4) % 4) + ')');
                    }
                });

                $li.appendTo($queue);
            }

            // 删除文件
            function removeFile(file) {
                var $li = $('#' + file.id);

                delete percentages[file.id];
                updateStatus();
                $li.off().find('.file-panel').off().end().remove();
            }
            // 反馈结果
            function updateStatus() {
                var text = '', stats;
                if (state === 'ready') {
                    text = '选中' + fileCount + '张图片，共' + WebUploader.formatSize(fileSize) + '。';
                } else if (state === 'confirm') {
                    stats = uploader.getStats();
                    if (stats.uploadFailNum) {
                        text = `已成功上传${stats.successNum}张照片，${stats.uploadFailNum}张照片上传失败，<a class="retry" href="#">重新上传</a>失败图片`;
                    }
                } else {
                    stats = uploader.getStats();
                    text = `共${fileCount}张（${WebUploader.formatSize(fileSize)}），已上传${stats.successNum}张`;
                    if (stats.uploadFailNum) {
                        text += '，失败' + stats.uploadFailNum + '张';
                    }
                }
                if (typeof stats != "undefined" && stats.successNum > 0) {
                    $recheck.hide();
                }

                $info.html(text);
            }
            //更新状态
            function setState(val) {
                var stats;
                if (val === state) {
                    return;
                }

                $upload.removeClass('state-' + state);
                $upload.addClass('state-' + val);
                state = val;

                switch (state) {
                    case 'pedding':
                        $placeHolder.removeClass('element-invisible');
                        $queue.hide();
                        $statusBar.addClass('element-invisible');
                        uploader.refresh();
                        break;
                    case 'ready':
                        $placeHolder.addClass('element-invisible');
                        $('#filePicker2').removeClass('element-invisible');
                        $queue.show();
                        $statusBar.removeClass('element-invisible');
                        uploader.refresh();
                        break;
                    case 'uploading':
                        $('#filePicker2').addClass('element-invisible');
                        $upload.text('暂停上传');
                        break;
                    case 'paused':
                        $upload.text('继续上传');
                        break;
                    case 'confirm':
                        $('#filePicker2').removeClass('element-invisible');
                        $upload.text('开始上传');
                        stats = uploader.getStats();
                        if (stats.successNum && !stats.uploadFailNum) {
                            setState('finish');
                            return;
                        }
                        break;
                    case 'finish':
                        stats = uploader.getStats();
                        if (stats.successNum) {
                            //alert('上传成功');
                        } else {
                            // 没有成功的图片，重设
                            state = 'done';
                            location.reload();
                        }
                        break;
                }
                updateStatus();
            }
        }
        //上传文件
        function files(obj) {
            var $wrap = $(
                '<div>\
                    <div id="' + (obj.id || 'filePicker') + '">选择文件</div>\
                    <ul class="fileList"></ul>\
                <div>'
            ).appendTo(obj._this);
            var $queue = $wrap.find('ul.fileList');

            // WebUploader实例
            var uploader = WebUploader.create({
                auto: true,
                pick: '#' + (obj.id || 'filePicker'),
                formData: obj.form,
                server: obj.action,
                fileNumLimit: obj.maxNum,
                fileSizeLimit: 200 * 1024 * 1024, // 200 M
                fileSingleSizeLimit: obj.singleSize * 1024 * 1024, // {0}M
                swf: '~/scripts/libs/plugins/webuploader/Uploader.swf'
            });

            //错误提示
            uploader.onError = function (code) {
                alert(code);
            };
            //排队等待
            uploader.on('fileQueued', function (file) {
                addFile(file);
            });
            //上传进度
            uploader.onUploadProgress = function (file, percentage) {
                var $li = $queue.find('#' + file.id),
                    $bar = $li.find('.progret .bar');

                $bar.css('width', percentage * 100 + '%');
            };
            //上传返回事件
            uploader.on('uploadSuccess', function (file, response) {
                if (obj.callback && typeof obj.callback == "function") {
                    obj.callback(file, response);
                }
            });
            // 添加文件
            function addFile(file) {
                var $li = $(
                    '<li id="' + file.id + '">\
                        <div class="cancel"></div>\
                        <span class="fileName">' + file.name + ' (' + WebUploader.formatSize(file.size) + ')</span>\
                        <span class="data"></span>\
                        <div class="progret"></div>\
                    </li>');
                var $data = $li.find('.data');
                var $cancel = $('<a href="javascript:">X</a>').appendTo($li.find('.cancel'));
                var $bar = $('<div class="bar"></div>').appendTo($li.find('.progret'));

                //跟进文件状态
                file.on('statuschange', function (cur, prev) {
                    var text, state;
                    switch (cur) {
                        case 'inited':
                            text = "等待上传";
                            break;
                        case 'queued':
                            text = "开始上传";
                            break;
                        case 'progress':
                            text = "正在上传";
                            break;
                        case 'complete':
                            text = "上传成功";
                            state = 'success';
                            break;
                        case 'error':
                            text = "上传失败";
                            state = 'error';
                            break;
                        case 'invalid':
                            text = "上传失败";
                            state = 'error';
                            break;
                        case 'interrupt':
                            text = "上传暂停";
                            break;
                        case 'cancelled':
                            text = "已被移除";
                            break;
                        default:
                            text = file.statusText;
                            state = 'error';
                            break
                    }

                    if (text) $data.text(' - ' + text.slice(0, 13) + (text.length > 13 ? '...' : '')).attr('title', text);
                    if (state) $li.addClass(state);
                });

                //删除
                $cancel.on('click', function() { delFile(file) });

                if (typeof obj.showfile == "undefined" || obj.showfile)
                    $li.appendTo($queue);
                if (obj.delay && obj.delay > 0)
                    setTimeout(function() { delFile(file) }, obj.delay);
            }
            //删除文件
            function delFile(file) {
                var $li = $queue.find('#' + file.id);
                $li.animate({ opacity: 0 }, 'slow', function() {
                    uploader.removeFile(file);
                    $li.remove();
                });
            }
        }
    }
});