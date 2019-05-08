// 文件上传
/**jQuery(function () {
    var $ = jQuery,
        $list = $('#filethelist'),
        $btn = $('#filectlBtn'),
        state = 'pending',
        fileuploader;

    // 优化retina, 在retina下这个值是2
    ratio = window.devicePixelRatio || 1,

    // 缩略图大小
        thumbnailWidth = 100 * ratio,
        thumbnailHeight = 100 * ratio,

    fileuploader = WebUploader.create({
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
        pick: {
		  id: '#filepicker',
		  label: '选择图片'
		},
        accept: {
            title: 'Images',
            extensions: 'gif,jpg,jpeg,bmp,png',
            mimeTypes: 'image/*'
        },
        fileSingleSizeLimit: 20 * 1024 * 1024,
        fileNumLimit: 1
    });

    fileuploader.on("error", function (type) {
        debugger;
        if (type == "F_EXCEED_SIZE") {
            top.layer.alert("文件大小不能超过20M！", { icon: "fa-times-circle", title: "系统提示", btn: ["确认"], btnclass: ["btn btn-primary"] });
            //alert("文件大小不能超过20M");
        }
    });
    // 当有文件添加进来的时候
    fileuploader.on('fileQueued', function (file) {
        var $li = $(
                '<div id="' + file.id + '" class="file-item thumbnail">' +
                    '<img>' + '<div class="info">' + file.name + '</div>' +
                '</div>'
                ),
        $btns = $('<div class="file-panel">' +
                    '<span class="cancel" style="color:red;">删除</span></div>').appendTo($li),
            $img = $li.find('img');

        $list.append('<div id="' + file.id + '" class="item">' +
            '<h4 class="info">' + '</h4>' +
            '<p class="state">等待上传...</p>' +
        '</div>');

        $list.append($li);

        // 创建缩略图
        fileuploader.makeThumb(file, function (error, src) {
            if (error) {
                $img.replaceWith('<span>不能预览</span>');
                return;
            }

            $img.attr('src', src);
        }, thumbnailWidth, thumbnailHeight);

        $btns.on('click', 'span', function () {
            fileuploader.removeFile(file);
            $list.empty();
        });
    });

    // 文件上传过程中创建进度条实时显示。
    fileuploader.on('uploadProgress', function (file, percentage) {
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
    fileuploader.on('uploadSuccess', function (file, requeson) {
        debugger;
        $("#Banner").val(requeson.uploadname);
        $("#banner_img").attr("style", "display:none");
        $('#' + file.id).find('p.state').text(requeson.message);
        alt = requeson.message;
        //var icon="";
        //if (requeson == "上传成功！")
        //    icon = "fa-check-circle";
        //if (requeson == "上传文件格式有误！" || requeson == "上传的文件不能为空！" || requeson == "上传错误！")
        //    icon = "fa-times-circle";
        //top.layer.alert(requeson, { icon: icon, title: "系统提示", btn: ["确认"], btnclass: ["btn btn-primary"] });
    });

    fileuploader.on('uploadError', function (file) {
        $('#' + file.id).find('p.state').text('上传出错');
    });

    fileuploader.on('uploadComplete', function (file) {
        debugger;
        $('#' + file.id).find('.progress').fadeOut();
        var icon = "";
        if (alt == "上传成功！")
            icon = "fa-check-circle";
        if (alt == "上传文件格式有误！" || alt == "上传的文件不能为空！" || alt == "上传错误！")
            icon = "fa-times-circle";
        top.layer.alert(alt, { icon: icon, title: "系统提示", btn: ["确认"], btnclass: ["btn btn-primary"] });
    });
    //fileuploader.on('uploadFinished', function (requeson) {
    //    alert(requeson);
    //});
    fileuploader.on('all', function (type) {
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
            fileuploader.stop();
            //buttom按钮阻止提交
            return false;
        } else {
            fileuploader.upload();
            //buttom按钮阻止提交
            return false;
        }
    });
});**/