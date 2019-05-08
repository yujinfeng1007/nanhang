// 文件上传
jQuery(function () {
    /**
	* @author Cytusian
	* @version 1.0.1
	* @name initUploader
	* @param {String} container 上传容器ID
	* @param {String} fileInput 文件传值的Input ID
	* @param {String} thumb 缩略图或文件名预览DOM ID
	* @param {Number} maxFile 最多上传的文件个数
    * @param {String} maxSize 上传文件最大大小, 单位MB
    * @param {String} isCover 是否简图
    * @param {Object} server 路径serverurl
	* @param {Object} accept 接收文件的格式
	*   title {String} 文字描述
    *   extensions {String} 允许的文件后缀，不带点，多个用逗号分割。
    *   mimeTypes {String} 多个用逗号分割。
	*/
    var $ = jQuery
    $.extend({
        initUploader: function (options) {
            var uploader = new Uploader(options)
            uploader.init()
        }
    });
    function Uploader(options) {
        this.options = options
        this.uploader = null
        this.state = 'pending'
        this.ratio = window.devicePixelRatio || 1,
            this.thumbnailWidth = 1
        this.thumbnailHeight = 1
        this.$list = null
        this.$startBtn = null
        this.$selectBtn = null
        this.default = {
            isCover: true,
            maxFile: 1,
            maxSize: 5,
            accept: {
                title: '图片',
                extensions: '*',
                mimeTypes: '*/*'
            },
            server: '/File/FileUpload'
        }
    }
    Uploader.prototype = {
        init: function () {
            this.options = $.extend({}, this.default, this.options)
            var options = this.options
            $(options.container).html(this._getBaseLayout())
            this._initWebuploader()
        },
        _initWebuploader: function () {
            var options = this.options
            this.$list = $(options.container).find('.upload-list')
            this.$startBtn = $(options.container).find('.start-upload')
            this.$selectBtn = $(options.container).find('.file-picker')
            this.uploader = WebUploader.create({
                swf: 'webuploader/Uploader.swf',
                accept: options.accept,
                pick: {
                    id: this.$startBtn[0],
                    label: '选择文件'
                },
                server: options.server,
                fileSingleSizeLimit: options.maxSize * 1024 * 1024
            })
            this._bindEvent()
        },
        _getFileListLayOut: function (file) {
            var $li = $('<li id="' + file.id + '" class="file-item">' +
                '<div class="file-status">' +
                '<h4 class="info">' + '</h4>' +
                '<p class="state">等待上传...</p>' +
                '</div>' +
                '<div id="' + file.id + '" class="item">' +
                '<img class="thumbnail" alt="thumb" />' + '<div class="file-info">' + file.name + '</div>' + '<input type="hidden" class="fileitem' + file.id + '" name="fileitem" data-IsCover=' + this.options.isCover + '/>' +
                '</div>' +
                '<div class="btn-group operate-btns" role="group" aria-label="...">' +
                '<button type="button" class="btn btn-primary upload-start">开始上传</button>' +
                '<button type="button" class="btn btn-danger upload-delete">删除文件</button>' +
                '</div>' +
                '</li>'
            )
            var $img = $li.find('img')
            return {
                $img: $img,
                $li: $li
            }
        },
        _displayListener: function () {
            var options = this.options
            var fileDom = this.$list.find('li.file-item')
            if (fileDom.length) {
                $(options.thumb).hide()
                if (fileDom.length === options.maxFile) {
                    this.$startBtn.hide()
                } else {
                    this.$startBtn.show()
                }
            } else {
                this.$list.hide()
                this.$startBtn.show()
                $(options.thumb).show()
            }
        },
        _bindEvent: function () {
            var _this = this
            var options = this.options

            $(options.container).find('.close-upload').on('click', function () {
                _this.$list.hide()
                _this.$startBtn.show()
                $(_this.options.thumb).show()
                _this.$list.find('li').remove()
                if (_this.uploader.getFiles().length) {
                    $.each(_this.uploader.getFiles(), function (i, item) {
                        _this.uploader.removeFile(item.id)
                    })
                }
            })
            this.uploader.on("error", function (type) {
                if (type == "F_EXCEED_SIZE") {
                    top.layer.alert("文件大小不能超过" + _this.options.maxSize + "！MB", { icon: "fa-times-circle", title: "系统提示", btn: ["确认"], btnclass: ["btn btn-primary"] });
                }
            });
            this.uploader.on('fileQueued', function (file) {
                var listItem = _this._getFileListLayOut(file)
                _this.$list.show()
                _this.$list.append(listItem['$li'])
                $('img.thumbnail').zoomify()
                _this._displayListener()
                listItem['$li'].find('.operate-btns .upload-start').on('click', function (e) {
                    e.preventDefault()
                    var fileId = $(this).parent().parent().attr('id')
                    _this.uploader.upload(fileId)
                })
                listItem['$li'].find('.operate-btns .upload-delete').on('click', function (e, file) {
                    e.preventDefault()
                    var fileId = $(this).parents().find('.file-item').attr('id')
                    $('#' + fileId).remove()
                    _this.uploader.removeFile(fileId)
                    _this._displayListener()
                    $('.fileitem' + file.id).val("")
                })
                _this.uploader.makeThumb(file, function (error, src) {
                    if (error) {
                        listItem['$img'].replaceWith('<span>不能预览</span>');
                        return;
                    }
                    listItem['$img'].attr('src', src);
                }, _this.thumbnailWidth, _this.thumbnailHeight);
            });
            this.uploader.on('uploadProgress', function (file, percentage) {
                var $li = $('#' + file.id),
                    $percent = $li.find('.progress .progress-bar');
                $li.find('.operate-btns button').addClass('disabled')
                // 避免重复创建
                if (!$percent.length) {
                    $percent = $('<div class="progress progress-striped active">' +
                        '<div class="progress-bar" role="progressbar" style="width: 0%">' +
                        '</div>' +
                        '</div>').appendTo($li).find('.progress-bar');
                } else {
                    $percent.show()
                }

                $li.find('p.state').text('上传中');
                $percent.css('width', percentage * 100 + '%');
            });
            this.uploader.on('uploadSuccess', function (file, response) {
                debugger
                var $li = $('#' + file.id)
                $(options.fileInput).val(response.filpath);
                $('.fileitem' + file.id).val(response.filpath);
                $(options.thumb).attr('src', response.filpath)
                $li.find('p.state').text(response.message);
                $li.find('.progress').hide()
                $li.find('.operate-btns button').removeClass('disabled')
                alt = response.message;
                var icon = "";
                if (response.state == "success") {
                    icon = "fa-check-circle";
                    $li.find('.upload-start').remove()
                }

                if (response.state == "error") {
                    icon = "fa-times-circle";
                }
                top.layer.alert(alt, { icon: icon, title: "系统提示", btn: ["确认"], btnclass: ["btn btn-primary"] });
            });
        },
        _getBaseLayout: function () {
            var template = []
            template.push('<div class="uploader">')
            template.push('<ul class="upload-list">')
            template.push('<div class="close-upload"><i class="fa fa-close" /></div>')
            template.push('</ul>')
            template.push('<div class="btns">')
            template.push('<div class="file-picker"></div>')
            template.push('<div class="start-upload">开始上传</div>')
            template.push('</div>')
            template.push('</div>')
            return template.join('')
        }
    }
})
/**jQuery(function () {
    var $ = jQuery,
        $list = $('#thelist'),
        $btn = $('#ctlBtn'),
        state = 'pending',
        uploader;

    // 优化retina, 在retina下这个值是2
    ratio = window.devicePixelRatio || 1,

    // 缩略图大小
        thumbnailWidth = 100 * ratio,
        thumbnailHeight = 100 * ratio,

    uploader = WebUploader.create({
        // 自动上传。
        //auto: true,

        // 不压缩image
        resize: false,

        // swf文件路径
        swf: 'webuploader/Uploader.swf',

        // 文件接收服务端。
        server: '/File/FileUpload',

        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#picker',
        fileSingleSizeLimit: 20 * 1024 * 1024
    });

    uploader.on("error", function (type) {
        debugger;
        if (type == "F_EXCEED_SIZE") {
            top.layer.alert("文件大小不能超过20M！", { icon: "fa-times-circle", title: "系统提示", btn: ["确认"], btnclass: ["btn btn-primary"] });
            //alert("文件大小不能超过20M");
        }
    });
    // 当有文件添加进来的时候
    uploader.on('fileQueued', function (file) {
        var $li = $(
                '<div id="' + file.id + '" class="file-item thumbnail">' +
                    '<img>' + '<div class="info">' + file.name + '</div>' +
                '</div>'
                ),
            $img = $li.find('img');

        $list.append('<div id="' + file.id + '" class="item">' +
            '<h4 class="info">' + '</h4>' +
            '<p class="state">等待上传...</p>' +
        '</div>');

        $list.append($li);

        // 创建缩略图
        uploader.makeThumb(file, function (error, src) {
            if (error) {
                $img.replaceWith('<span>不能预览</span>');
                return;
            }

            $img.attr('src', src);
        }, thumbnailWidth, thumbnailHeight);
    });

    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {
        var $li = $('#' + file.id),
            $percent = $li.find('.progress .progress-bar');

        // 避免重复创建
        if (!$percent.length) {
            $percent = $('<div class="progress progress-striped active">' +
              '<div class="progress-bar" role="progressbar" style="width: 0%">' +
              '</div>' +
            '</div>').appendTo($li).find('.progress-bar');
        }

        $li.find('p.state').text('上传中');

        $percent.css('width', percentage * 100 + '%');
    });
    var alt = "";
    uploader.on('uploadSuccess', function (file, requeson) {
        debugger;
        $("#Files").val(requeson.filpath);
        $('#' + file.id).find('p.state').text(requeson.message);
        alt = requeson.message;
        var icon = "";
        if (alt == "上传成功！")
            icon = "fa-check-circle";
        if (alt == "上传文件格式有误！" || alt == "上传的文件不能为空！" || alt == "上传错误！")
            icon = "fa-times-circle";
        top.layer.alert(alt, { icon: icon, title: "系统提示", btn: ["确认"], btnclass: ["btn btn-primary"] });
    });

    uploader.on('uploadError', function (file) {
        $('#' + file.id).find('p.state').text('上传出错');
    });

    uploader.on('uploadComplete', function (file) {
        debugger;
        $('#' + file.id).find('.progress').fadeOut();
    });
    //uploader.on('uploadFinished', function (requeson) {
    //    alert(requeson);
    //});
    uploader.on('all', function (type) {
        if (type === 'startUpload') {
            state = 'uploading';
        } else if (type === 'stopUpload') {
            state = 'paused';
        } else if (type === 'uploadFinished') {
            state = 'done';
        }

        if (state === 'uploading') {
            $btn.text('暂停上传');
        } else {
            $btn.text('开始上传');
        }
    });

    $btn.on('click', function () {
        if (state === 'uploading') {
            uploader.stop();
            //buttom按钮阻止提交
            return false;
        } else {
            uploader.upload();
            //buttom按钮阻止提交
            return false;
        }
    });
});**/