$.mailUploader = function ($container, options) {
    var defaults = {
        maxFile: 1,
        accept: {
            title: 'Images',
            extensions: 'gif,jpg,jpeg,bmp,png',
            mimeTypes: 'image/*'
        },
        url: '/File/FileUpload',
        maxSize: '100',
        picker: '#picker',
        onInsert: function (src) { console.log(src) }
    }
    var status = 'pending'
    var options = $.extend(defaults, options)
    function getFileList(id, src, exist, name) {
        var type = checkType(src)
        var $list = $('<div fileSrc="' + src + '" class="G0 file-item ' + (exist ? 'upload-success' : '') + '" id=' + (id || '') + '>' +
            (type === 'image' ? '<img class="thumbnail" data-action="zoom" src=' + src + ' />' : '<img class="thumbnail" src="/Content/img/file.svg" />') +
            (name ? '<div class="o0" title=' + name + '>' + name + '</div>' : '') +
            '<div class="j0">' +
            '<div class="nui-progress"><span class="nui-progress-bar"><span class="nui-progress-bar-inner" style="width:0%;"></span></span><span class="nui-progress-text">0%</span></div>' +
            '<span class="nui-txt status">' + (exist ? '上传完成' : '等待上传') + '</span>' +
            '</div>' +
            '<div class="m0">' +
            (exist ? '' : ('<a href="javascript:void(0)" class="nui-txt-link upload-btn">开始上传</a>')) +
            (type === 'image' ? '<a href="javascript:void(0)" class="nui-txt-link insert">插入正文</a>' : '') +
            '<a href="javascript:void(0)" class="nui-txt-link delete">删除</a>' +
            '</div>' +
            '</div>')
        $list.data('fileInfo', {
            url: src,
            size: null,
            type: null,
            success: exist
        })
        return $list
    }
    function checkType(value) {
        if (value) {
            var seat = value.lastIndexOf(".")
            if (seat !== -1) {
                var extension = value.substring(seat).toLowerCase()
                if (extension === '.jpg' || extension === '.gif' || extension === '.png' || extension === '.jpeg') {
                    return 'image'
                }
            } else {
                if (value.indexOf('data:image/') !== -1) {
                    return 'image'
                } else {
                    return 'file'
                }
            }
        } else {
            return 'file'
        }
        return 'file'
    }
    function displayCheck($listContainer, $button) {
        var fileDom = $listContainer.find('div.file-item')
        if (fileDom.length) {
            if (fileDom.length >= options.maxFile) {
                $button.hide()
            } else {
                $button.show()
            }
        } else {
            $button.show()
        }
        $button.find('.webuploader-pick').html('选择文件(' + fileDom.length + '/' + options.maxFile + ')')
    }
    $container.each(function () {
        var $button = $('<div class="btn btn-primary btn-small">选择文件</div>')
        var id = $(this).attr('id')
        var $fileTrigger = $(this).prev('input.file')
        var $input = $('<input type="uploader" class="uploader form-control  hidden" name=' + id + ' id=' + id + ' />')
        var $listContainer = $('<div id="attachContent" class="D0"></div>')
        if ($(this).hasClass('required')) {
            $input.addClass('required')
        } else {
            $input.removeClass('required')
        }
        $(this).append($input).append($listContainer)
        if (!options.picker) {
            $(this).append($button)
        }
        $(this).removeAttr('id')
        $(this).attr('id', id + '-file')
        var uploader = WebUploader.create({
            swf: 'webuploader/Uploader.swf',
            accept: options.accept,
            auto: false,
            pick: options.picker || {
                id: $button[0],
                label: '选择文件'
            },
            fileSingleSizeLimit: Number(options.maxSize) * 1024 * 1024,
            server: options.url,
            duplicate: true
        })
        bindEvent(uploader, $listContainer, $button, $input)

        $(this)[0].setFiles = function (fileList) {
            if (fileList.length) {
                $.each(fileList, function (i, item) {
                    var $dom = getFileList(null, item.Path, true, item.Name)
                    $listContainer.append($dom)
                })
                displayCheck($listContainer, $button)
            }
        }
        $(this)[0].getFiles = function (url) {
            output = []
            $listContainer.find('div.file-item').each(function () {
                output.push($(this).data('fileInfo').url || '')
            })

            return output.join(',')
        }
    })
    function getFileValue($container) {
        var imgArr = []
        $container.find('.file-item.upload-success').each(function () {
            imgArr.push($(this).data('fileInfo')['url'])
        })
        return imgArr.join(',')
    }
    function bindEvent(uploader, $listContainer, $button, $input) {
        displayCheck($listContainer, $button)
        uploader.on('fileQueued', function (file) {
            uploader.makeThumb(file, function (error, src) {
                console.log(file)
                $fileItem = getFileList(file.id, error ? null : src, false, file.name)
                $listContainer.append($fileItem)
                $('img.thumbnail').off().on('click', function () {
                    top.layer.open({
                        type: 1,
                        title: '图片预览',
                        shadeClose: true,
                        area: ['80%', '600px'],
                        content: '<div class="image-preview-layer"><img src="' + $(this).attr('src') + '" alt="preview" /></div>'
                    })
                })
                displayCheck($listContainer, $button)
                $fileItem.find('.m0 .upload-btn').click(function (e) {
                    e.preventDefault();
                    $(this).html('上传中')
                    $('#' + file.id).find('.nui-txt.status').html('上传中')
                    uploader.upload(file.id)
                })
            }, 1, 1)
        })
        uploader.on('uploadProgress', function (file, percentage) {
            $('#' + file.id).find('.nui-progress .nui-progress-bar-inner').css('width', percentage * 100 + '%')
            $('#' + file.id).find('.nui-progress .nui-progress-text').html(percentage * 100 + '%')
        })
        $listContainer.on('click', 'a.delete', function (e) {
            e.preventDefault();
            e.stopPropagation()
            var id = $(this).parents('div.file-item').attr('id')
            $(this).parents('div.file-item').remove()
            if (id) {
                $('#' + id).remove()
                uploader.removeFile(id)
            }
            var fileValue = getFileValue($listContainer)
            displayCheck($listContainer, $button)
            $input.val(fileValue)
        })
        $listContainer.on('click', 'a.insert', function (e) {
            if ($(this).parents('.file-item').hasClass('upload-success')) {
                options.onInsert($(this).parents('.file-item').attr('fileSrc'))
            }
        })
        uploader.on('uploadStart', function (file) {
            $('#' + file.id).find('.nui-progress').show()
            $('#' + file.id).find('.status').html('上传中')
        })
        uploader.on('uploadSuccess', function (file, response) {
            if (response.state === 'success') {
                $('#' + file.id).addClass('upload-success').attr('fileSrc', response.url)
                $('#' + file.id).find('.nui-progress').hide()
                $('#' + file.id).find('.m0 .upload-btn').remove()
                $('#' + file.id).find('.m0 a.insert').show()
                $('#' + file.id).find('.nui-txt.status').html('上传成功')
                $('#' + file.id).data('fileInfo', {
                    url: response.url,
                    size: response.size,
                    type: response.type,
                    success: true
                })
            } else if (response.state === 'error') {
                $('#' + file.id).addClass('upload-error')
                $('#' + file.id).find('.m0 .upload-btn').html('重新上传')
                $('#' + file.id).find('.nui-txt.status').html('上传失败')
                $('#' + file.id).data('fileInfo', {
                    url: null,
                    size: null,
                    type: null,
                    success: false
                })
                $.modalMsg(response.message, 'warning')
            }
            var fileValue = getFileValue($listContainer)
            $input.val(fileValue)
        })
        uploader.on('error', function (error) {
            console.log(error)
            if (error === 'F_EXCEED_SIZE') {
                $.modalMsg('文件不能大于' + options.maxSize + 'MB', 'warning')
            }
            if (error === 'Q_TYPE_DENIED') {
                $.modalMsg('不能上传该文件格式', 'warning')
            }
        })
    }
}