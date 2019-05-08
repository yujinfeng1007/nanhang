$(function() {
    document.body.className = localStorage.getItem('config-skin')
    $("[data-toggle='tooltip']").tooltip()
})
var classTable = function() {
    this.timeSpanCn = {
        'MN': '早晨',
        'AM': '上午',
        'CM': '中午',
        'PM': '下午',
        'EN': '晚上'
    }
    this.classHour = {
        AM: [{
            ClassHour: "1",
            ClassHourCh: "第1节",
            ClassTime: "8:00~9:00",
        }, {
            ClassHour: "2",
            ClassHourCh: "第2节",
            ClassTime: "9:00~10:00",
        }, {
            ClassHour: "3",
            ClassHourCh: "第3节",
            ClassTime: "10:00~11:00",
        }, {
            ClassHour: "4",
            ClassHourCh: "第4节",
            ClassTime: "11:00~12:00",
        }],
        PM: [{
            ClassHour: "5",
            ClassHourCh: "第5节",
            ClassTime: "13:00~14:00",
        }, {
            ClassHour: "6",
            ClassHourCh: "第6节",
            ClassTime: "14:00~15:00",
        }, {
            ClassHour: "7",
            ClassHourCh: "第7节",
            ClassTime: "15:00~16:00",
        }, {
            ClassHour: "8",
            ClassHourCh: "第8节",
            ClassTime: "16:00~17:00",
        }],
        EN: [{
            ClassHour: "9",
            ClassHourCh: "第9节",
            ClassTime: "17:00~18:00",
        }, {
            ClassHour: "10",
            ClassHourCh: "第10节",
            ClassTime: "18:00~19:00",
        }, {
            ClassHour: "11",
            ClassHourCh: "第11节",
            ClassTime: "19:00~20:00",
        }, {
            ClassHour: "12",
            ClassHourCh: "第12节",
            ClassTime: "20:00~21:00",
        }]
    }
    this.$container = null
}

classTable.prototype = {
    _getBaseLayout: function() {
        var template = '<table class="timetable">'  + '<thead>' + '<tr>' + '<th>时段</th>' + '<th>内容</th>' + '<th class="class-time">时间</th>' + '<th data-index="1">周一</th>' + '<th data-index="2">周二</th>' + '<th data-index="3">周三</th>' + '<th data-index="4">周四</th>' + '<th data-index="5">周五</th>' + '<th data-index="6">周六</th>' + '<th data-index="7">周日</th>' + '</tr>' + '<tr class="th-date">' + '<th></th>' + '<th></th>' + '<th class="class-time"></th>' + '<th class="date-col"></th>' + '<th class="date-col"></th>' + '<th class="date-col"></th>' + '<th class="date-col"></th>' + '<th class="date-col"></th>' + '<th class="date-col"></th>' + '<th class="date-col"></th>' + '</tr>' + '</thead>' + '<tbody>' + '</tbody>' + '</table>';
        this.$container.html(template)
    },
    loadClassHour: function(data, showTime, callback) {
        showTime = showTime || false
        var _html = "";
        for (var t in data) {
            for (var dt in data[t]) {
                _html += "<tr>";
                if (dt === "0")
                    _html += "<td rowspan='" + data[t].length + "'>" + this.timeSpanCn[t] + "</td>";
                _html += "<td>" + data[t][dt].ClassHourCh + "</td>" + "<td class='class-time'>" + data[t][dt].ClassTime + "</td>"
                var i = 0;
                while (i < 7) {
                    var _id;
                    i++;
                    if (i === 7)
                        _id = "t_7_" + data[t][dt].ClassHour;
                    else
                        _id = "t_" + i + "_" + data[t][dt].ClassHour;
                    _html += "<td id='" + _id + "'></td>";
                }
                _html += "</tr>";
            }
        }
        this.$container.find('tbody').html(_html);
        this.toggleClassHour(showTime)
        if (callback)
            callback();
    },
        toggleClassHour: function(show) {
            var $classCol = this.$container.find('.class-time')
            if (show) {
                $classCol.show()
            } else {
                $classCol.hide()
            }
        },
        setClasses: function(data) {
            this.clearClasses()
            for (var dateIndex in data) {
                var htm = ''
                var date = data[dateIndex]
                var $div = $('<div>').attr({
                    courseId: date.courseId,
                    teacherId: date.teacherId,
                    roomId: date.roomId
                }).addClass('item')
                if (date['courseName']) {
                    $div.html(date['courseName'] + (date['teacherName'] ? '(' + date['teacherName'] + ')' : ''))
                }
                $('#t_' + date['dateIndex'] + '_' + date['lessonIndex']).attr({
                    courseId: date.courseId,
                    teacherId: date.teacherId,
                    roomId: date.roomId
                }).append($div)
            }
        },
        clearClasses: function() {
            this.$container.find('td').each(function() {
                if ($(this).attr('id')) {
                    $(this).find('.item').remove()
                    $(this).removeAttr('courseid').removeAttr('teacherid').removeAttr('roomid')
                }
            })
        },
        setToday: function() {
            var today = new Date().getDay()
            this.$container.find('thead th[data-index=' + today + ']').addClass('thisday')
        },
        _enableFrameSelection: function(options) {
            var $selectBoxDashed = $('<div class="select-box-dashed"></div>')
            var that = this
            function clearBubble(e) {
                if (e.stopPropagation) {
                    e.stopPropagation()
                } else {
                    e.cancelBubble = true
                }
                if (e.preventDefault) {
                    e.preventDefault()
                } else {
                    e.returnValue = false
                }
            }
            this.$container.find('tbody').off('mousedown').on('mousedown', function(eventDown) {
                if (eventDown.button !== 0) {
                    return
                }
                that.$container.find('tbody td').removeClass('selected')
                that.$container.append($selectBoxDashed)
                var startX = eventDown.x || eventDown.clientX
                var startY = eventDown.y || eventDown.clientY - (options.offsetTop || 0)
                $selectBoxDashed.css({
                    left: startX,
                    top: startY
                })
                //  根据鼠标移动，设置选框宽高
                var _x = null
                var _y = null
                //  清除事件冒泡、捕获
                clearBubble(eventDown)
                that.$container.find('tbody').off('mousemove').on('mousemove', function(eventMove) {
                    //  设置选框可见
                    $selectBoxDashed.css('display', 'block');
                    //  根据鼠标移动，设置选框的位置、宽高
                    _x = eventMove.x || eventMove.clientX;
                    _y = (eventMove.y || eventMove.clientY) - (options.offsetTop || 0)
                    //  暂存选框的位置及宽高，用于将 select-item 选中
                    var _left = Math.min(_x, startX);
                    var _top = Math.min(_y, startY);
                    var _width = Math.abs(_x - startX);
                    var _height = Math.abs(_y - startY);
                    $selectBoxDashed.css({
                        left: _left,
                        top: _top,
                        width: _width,
                        height: _height
                    });
                    //  遍历容器中的选项，进行选中操作
                    that.$container.find('td[id]').each(function() {
                        var $item = $(this)
                        var itemX_pos = $item.prop('offsetWidth') + $item.prop('offsetLeft')
                        var itemY_pos = $item.prop('offsetHeight') + $item.prop('offsetTop')
                        //  判断 select-item 是否与选框有交集，添加选中的效果（ temp-selected ，在事件 mouseup 之后将 temp-selected 替换为 selected）
                        var condition1 = itemX_pos > _left
                        var condition2 = itemY_pos > _top
                        var condition3 = $item.prop('offsetLeft') < (_left + _width)
                        var condition4 = $item.prop('offsetTop') < (_top + _height)
                        if (condition1 && condition2 && condition3 && condition4) {
                            $item.addClass('temp-selected')
                        } else {
                            $item.removeClass('temp-selected')
                        }
                    });
                    //  清除事件冒泡、捕获
                    clearBubble(eventMove)
                })
                $(document).off('mouseup.selectTable').on('mouseup.selectTable', function(e) {
                    if (e.button !== 0) {
                        return
                    }
                    that.$container.find('tbody').off('mousemove')
                    that.$container.find('tbody').find('td.temp-selected').removeClass('temp-selected').addClass('selected')
                    $selectBoxDashed.remove()
                    if (options.selectCallback) {
                        options.selectCallback(that.$container.find('tbody td.selected'))
                        $(document).off('mouseup.selectTable')
                    }
                })
            })
        },
        init: function(options) {
            this.$container = options.$container
            if (options.classHour)
                this.classHour = options.classHour
            this._getBaseLayout()
            this.setToday()
            this.loadClassHour(this.classHour)
            if (options.frameSelection) {
                this._enableFrameSelection(options)
            }
        },
        setHeaderDate: function(date) {
            var $dateTh = this.$container.find('thead .th-date .date-col')
            $.each(date, function(i, item) {
                $dateTh.eq(i).html(item)
            })
        }
    }
$.reload = function() {
    location.reload();
    return false;
}
$.guid = function guid() {
    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}
$.loading = function(bool, text, topPage) {
    if (topPage) {} else {
        topPage = top
    }
    var $loadingpage = topPage.$("#loadingPage");
    var $loadingtext = $loadingpage.find('.loading-content');
    if (bool) {
        $loadingpage.show();
    } else {
        if ($loadingtext.attr('istableloading') == undefined) {
            $loadingpage.hide();
        }
    }
    if (!!text) {
        $loadingtext.html(text);
    } else {
        $loadingtext.html("数据加载中，请稍后…");
    }
    $loadingtext.css("left", (topPage.$('body').width() - $loadingtext.width()) / 2 - 50);
    $loadingtext.css("top", (topPage.$('body').height() - $loadingtext.height()) / 2);
}
$.request = function(name) {
    var search = location.search.slice(1);
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] == name) {
            if (unescape(ar[1]) == 'undefined') {
                return "";
            } else {
                return unescape(ar[1]);
            }
        }
    }
    return "";
}
$.currentWindow = function () {
    var iframeId = top.$(".winIframe:visible").attr("id");
    var iframeContext = top.frames[iframeId].contentWindow || top.frames[iframeId]
    var subiframe = iframeContext.$(".step-pane.active")
    if (subiframe !== null && subiframe.find("iframe").length > 0)
        return subiframe.find("iframe").get(0).contentWindow || subiframe.find("iframe").get(0)
    return iframeContext
}
$.browser = function() {
    var userAgent = navigator.userAgent;
    var isOpera = userAgent.indexOf("Opera") > -1;
    if (isOpera) {
        return "Opera"
    }
    ;if (userAgent.indexOf("Firefox") > -1) {
        return "FF";
    }
    if (userAgent.indexOf("Chrome") > -1) {
        if (window.navigator.webkitPersistentStorage.toString().indexOf('DeprecatedStorageQuota') > -1) {
            return "Chrome";
        } else {
            return "360";
        }
    }
    if (userAgent.indexOf("Safari") > -1) {
        return "Safari";
    }
    if (userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1 && !isOpera) {
        return "IE";
    }
    ;
}
$.download = function(url, data, method) {
    if (url && data) {
        data = typeof data == 'string' ? data : jQuery.param(data);
        var inputs = '';
        $.each(data.split('&'), function() {
            var pair = this.split('=');
            inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
        });
        $('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>').appendTo('body').submit().remove();
    }
    ;
}
;
$.addRequired = function(formId) {
    $(formId).find('table.form tr').find('input, select, textarea').each(function(i, item) {
        var $this = $(item)
        if ($this.is('[required]') || $this.hasClass('required')) {
            console.log($this.parents('td'))
            $this.parents('td').prev().addClass('required')
        }
    })
}
$.gridAuthBtn = function($el) {
    var moduleId = top.$(".NFine_iframe:visible").attr("id").substr(6)
    var dataJson = top.clients.authorizeButton[moduleId]
    if (!dataJson) {
        return $el.html()
    } else {
        $.each(dataJson, function(i, item) {
            if (item.F_EnabledMark) {
                $el.find('.' + item.F_EnCode).addClass('enabled')
            } else {
                $el.find('.' + item.F_EnCode).addClass('disabled')
            }
        })
        $el.find('.operate-btn.disabled').remove()

        return $el.html()
    }
}
$.convertFileToDataURLviaFileReader = function(url, callback) {
    var xhr = new XMLHttpRequest()
    xhr.responseType = 'blob'
    xhr.onload = function() {
        var reader = new FileReader()
        reader.onloadend = function() {
            callback(reader.result)
        }
        reader.readAsDataURL(xhr.response)
    }
    xhr.open('GET', url)
    xhr.send()
}

$.dataURLtoBlob = function(dataurl) {
    var arr = dataurl.split(',')
      , mime = arr[0].match(/:(.*?);/)[1]
      , bstr = atob(arr[1])
      , n = bstr.length
      , u8arr = new Uint8Array(n);
    while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
    }
    return new Blob([u8arr],{
        type: mime
    });
}
$.simpleUploader = function($container, options) {
    var defaults = {
        maxFile: 1,
        accept: {
            title: 'Images',
            extensions: 'gif,jpg,jpeg,bmp,png',
            mimeTypes: 'image/*'
        },
        url: '/File/FileUpload',
        maxSize: '100'
    }
    var status = 'pending'
    var options = $.extend(defaults, options)
    function getFileList(id, src, exist) {
        var type = checkType(src)
        var $list = $('<li class="file-item ' + (exist ? 'upload-success' : '') + '" id=' + (id || '') + '>' + '<a target="_blank" href="' + src + '">' + '<div>' + (type === 'image' ? '<img class="thumbnail" data-action="zoom" src=' + src + ' />' : '<img class="thumbnail" src="/Content/img/file.svg" />') + '<div class="start-upload">' + (exist ? '' : ('<button class="btn btn-primary">开始上传</button>')) + '<div class="progress" style="margin-top: 10px;margin-bottom: 10px"><div role="progressbar" class="progress-bar progress-bar-success"></div></div>' + '</div><i class="fa fa-close"></i></div>' + '</a>' + '</li>')
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
        var fileDom = $listContainer.find('li.file-item')
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
    $container.each(function() {
        var $button = $('<div class="btn btn-primary btn-small">选择文件</div>')
        var id = $(this).attr('id')
        var $fileTrigger = $(this).prev('input.file')
        var $input = $('<input type="uploader" class="uploader form-control  hidden" name=' + id + ' id=' + id + ' />')
        var $listContainer = $('<ul class="preview-list"></ul>')
        if ($(this).hasClass('required')) {
            $input.addClass('required')
        } else {
            $input.removeClass('required')
        }
        $(this).append($input).append($listContainer).append($button)
        $(this).removeAttr('id')
        $(this).attr('id', id + '-file')
        var uploader = WebUploader.create({
            swf: 'webuploader/Uploader.swf',
            accept: options.accept,
            auto: false,
            pick: {
                id: $button[0],
                label: '选择文件',
                multiple: false
            },
            fileSingleSizeLimit: Number(options.maxSize) * 1024 * 1024,
            server: options.url,
            duplicate: true
        })
        bindEvent(uploader, $listContainer, $button, $input)
        $(this)[0].setFiles = function(url) {
            var fileList = url.split(',')
            if (fileList.length) {
                $.each(fileList, function(i, item) {
                    var $dom = getFileList(null, item, true)
                    $listContainer.append($dom)
                })
                displayCheck($listContainer, $button)
                $input.val(url)
            }
        }
        $(this)[0].getFiles = function(url) {
            var output = $listContainer.find('li.file-item').map(function() {
                return $(this).data('fileInfo') || {}
            })
        }
    })
    function getFileValue($container) {
        var imgArr = []
        $container.find('.file-item.upload-success').each(function() {
            imgArr.push($(this).data('fileInfo')['url'])
        })
        return imgArr.join(',')
    }
    function bindEvent(uploader, $listContainer, $button, $input) {
        displayCheck($listContainer, $button)
        uploader.on('fileQueued', function(file) {
            uploader.makeThumb(file, function(error, src) {
                $fileItem = getFileList(file.id, error ? null : src, false)
                $listContainer.append($fileItem)
                $('img.thumbnail').off().on('click', function() {
                    top.layer.open({
                        type: 1,
                        title: '图片预览',
                        shadeClose: true,
                        area: ['80%', '600px'],
                        content: '<div class="image-preview-layer"><img src="' + $(this).attr('src') + '" alt="preview" /></div>'
                    })
                })
                displayCheck($listContainer, $button)
                $fileItem.find('.start-upload button').click(function(e) {
                    e.preventDefault();
                    $(this).html('上传中')
                    uploader.upload(file.id)
                })
            }, 1, 1)
        })
        uploader.on('uploadProgress', function(file, percentage) {
            $('#' + file.id).find('.progress .progress-bar').css('width', percentage * 100)
        })
        $listContainer.on('click', '.fa-close', function(e) {
            e.preventDefault();
            e.stopPropagation()
            var id = $(this).parents('li.file-item').attr('id')
            $(this).parents('li.file-item').remove()
            if (id) {
                $('#' + id).remove()
                uploader.removeFile(id)
            }
            var fileValue = getFileValue($listContainer)
            displayCheck($listContainer, $button)
            $input.val(fileValue)
        })
        uploader.on('uploadSuccess', function(file, response) {
            if (response.state === 'success') {
                $('#' + file.id).addClass('upload-success').attr('imgSrc', response.url)
                $('#' + file.id).find('.start-upload').remove()
                $('#' + file.id).data('fileInfo', {
                    url: response.url,
                    size: response.size,
                    type: response.type,
                    success: true
                })
            } else if (response.state === 'error') {
                $('#' + file.id).addClass('upload-error')
                $('#' + file.id).find('.start-upload button').html('重新上传')
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
        uploader.on('error', function(error) {
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
$.modalOpen = function(options) {
    var defaults = {
        id: null,
        type: 2,
        title: '系统窗口',
        width: "100px",
        height: "100px",
        fix: false,
        url: '',
        shade: 0.3,
        btn: ['确认', '关闭'],
        btnclass: ['btn btn-primary', 'btn btn-danger'],
        callBack: null,
        closeBtn: 1
    };
    var _options = $.extend({}, defaults, options);
    var _width = top.$(window).width() > parseInt(_options.width.replace('px', '')) ? _options.width : top.$(window).width() + 'px';
    var _height = top.$(window).height() > parseInt(_options.height.replace('px', '')) ? _options.height : top.$(window).height() + 'px';
    _options.success = function(layro, index) {
        layro.find('iframe').addClass('winIframe')
        options.success && options.success(layro, index)
    }
    _options.yes = _options.yes || function(index) {
        _options.callBack(_options.id, index)
    }
    _options.cancel = function() {
        return true;
    }
    _options.area = [_width, _height]
    _options.content = _options.url
    top.layer.open(_options);
}
$.modalConfirm = function(content, callBack) {
    top.layer.confirm(content, {
        icon: "fa-exclamation-circle",
        title: "系统提示",
        btn: ['确认', '取消'],
        btnclass: ['btn btn-primary', 'btn btn-danger'],
        closeBtn: 1
    }, function() {
        callBack(true);
    }, function() {
        callBack(false)
    });
}
$.modalAlert = function(content, type) {
    var icon = "";
    if (type == 'success') {
        icon = "fa-check-circle";
    }
    if (type == 'error') {
        icon = "fa-times-circle";
    }
    if (type == 'warning') {
        icon = "fa-exclamation-circle";
    }
    var area = "";
    if (content.indexOf("导入错误如下") > -1) {
        area = '500px';
    }
    top.layer.alert(content, {
        icon: icon,
        area: area,
        title: "系统提示",
        btn: ['确认'],
        btnclass: ['btn btn-primary'],
    });
}
$.modalMsg = function(content, type) {
    if (type != undefined) {
        var icon = "";
        if (type == 'success') {
            icon = "fa-check-circle";
        }
        if (type == 'error') {
            icon = "fa-times-circle";
        }
        if (type == 'warning') {
            icon = "fa-exclamation-circle";
        }
        top.layer.msg(content, {
            icon: icon,
            time: 4000,
            //time: 40000,
            shift: 5
        });
        top.$(".layui-layer-msg").find('i.' + icon).parents('.layui-layer-msg').addClass('layui-layer-msg-' + type);
    } else {
        top.layer.msg(content);
    }
}
$.modalClose = function() {
    var index = top.layer.getFrameIndex(window.name);
    //先得到当前iframe层的索引
    var $IsdialogClose = top.$("#layui-layer" + index).find('.layui-layer-btn').find("#IsdialogClose");
    var IsClose = $IsdialogClose.is(":checked");
    if ($IsdialogClose.length == 0) {
        IsClose = true;
    }
    if (IsClose) {
        top.layer.close(index);
    } else {
        location.reload();
    }
}
$.submitForm = function(options) {
    var defaults = {
        url: "",
        param: [],
        loading: "正在提交数据...",
        success: null,
        close: true
    };

    var options = $.extend(defaults, options);
    $.loading(true, options.loading);
    window.setTimeout(function() {
        if ($('[name=__RequestVerificationToken]').length > 0) {
            options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
        }
        $.ajax({
            url: options.url,
            data: options.param,
            type: "post",
            dataType: "json",
            success: function(data) {
                $.loading(false);
                if (data.state == "success") {
                    options.success(data);
                    $.modalMsg(data.message, data.state);
                    if (options.close == true) {
                        $.modalClose();
                    }
                } else {
                    $.modalAlert(data.message, data.state);
                }
            },
            error: function(XMLHttpRequest, textStatus, errorThrown) {
                $.loading(false);
                $.modalMsg(errorThrown, "error");
            },
            beforeSend: function() {
                $.loading(true, options.loading);
            },
            complete: function() {
                $.loading(false);
            }
        });
    }, 500);
}
$.deleteForm = function(options) {
    var defaults = {
        prompt: "注：您确定要删除该项数据吗？",
        url: "",
        param: [],
        loading: "正在删除数据...",
        success: null,
        close: true
    };
    var options = $.extend(defaults, options);
    if ($('[name=__RequestVerificationToken]').length > 0) {
        options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    $.modalConfirm(options.prompt, function(r) {
        if (r) {
            $.loading(true, options.loading);
            window.setTimeout(function() {
                $.ajax({
                    url: options.url,
                    data: options.param,
                    //contentType: "application/json; charset=utf-8",
                    type: "post",
                    dataType: "json",
                    success: function(data) {
                        if (data.state == "success") {
                            options.success(data);
                            $.modalMsg(data.message, data.state);
                        } else {
                            $.modalAlert(data.message, data.state);
                        }
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        $.loading(false);
                        $.modalMsg(errorThrown, "error");
                    },
                    beforeSend: function() {
                        $.loading(true, options.loading);
                    },
                    complete: function() {
                        $.loading(false);
                    }
                });
            }, 500);
        }
    });
}
$.ConfirmForm = function(options) {
    var defaults = {
        prompt: "注：您确定要进行该项操作吗？",
        url: "",
        param: [],
        loading: "正在操作数据...",
        success: null,
        close: true,
        topPage: top
    };
    var options = $.extend(defaults, options);
    if ($('[name=__RequestVerificationToken]').length > 0) {
        options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    $.modalConfirm(options.prompt, function(r) {
        if (r) {
            $.loading(true, options.loading, options.topPage);
            window.setTimeout(function() {
                $.ajax({
                    url: options.url,
                    data: options.param,
                    //contentType: "application/json; charset=utf-8",
                    type: "post",
                    dataType: "json",
                    success: function(data) {
                        if (data.state == "success") {
                            options.success(data);
                            $.modalMsg(data.message, data.state);
                        } else {
                            $.modalAlert(data.message, data.state);
                        }
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        $.loading(false, '', options.topPage);
                        $.modalMsg(errorThrown, "error");
                    },
                    beforeSend: function() {
                        $.loading(true, options.loading, options.topPage);
                    },
                    complete: function() {
                        $.loading(false, '', options.topPage);
                    }
                });
            }, 500);
        }
    });
}
$.jsonWhere = function(data, action) {
    if (action == null)
        return;
    var reval = new Array();
    $(data).each(function(i, v) {
        if (action(v)) {
            reval.push(v);
        }
    })
    return reval;
}
$.keyLight = function(id, key, backgroundColor) {
    let oDiv = document.getElementById(id)
      , sText = oDiv.innerHTML
      , bgColor = backgroundColor || "orange"
      , sKey = "<span style='background-color: " + bgColor + ";'>" + key + "</span>"
      , num = -1
      , rStr = new RegExp(key,"g")
      , rHtml = new RegExp("\<.*?\>","ig")
      , //匹配html元素
    aHtml = sText.match(rHtml);
    //存放html元素的数组
    sText = sText.replace(rHtml, '{~}');
    //替换html标签
    sText = sText.replace(rStr, sKey);
    //替换key
    sText = sText.replace(/{~}/g, function() {
        //恢复html标签
        num++;
        return aHtml[num];
    });
    oDiv.innerHTML = sText;
}
$.fn.jqGridRowValue = function(key) {
    var $grid = $(this);
    var selectedRowIds = $grid.jqGrid("getGridParam", "selarrrow");
    if (selectedRowIds.length !== 0) {
        var json = [];
        var len = selectedRowIds.length;
        for (var i = 0; i < len; i++) {
            var rowData;
            if (key) {
                rowData = ($grid.jqGrid('getRowData', selectedRowIds[i]))[key];
            } else {
                rowData = $grid.jqGrid('getRowData', selectedRowIds[i])
            }
            if (!$.isEmptyObject(rowData)) {
                json.push(rowData)
            }
        }
        return json;
    } else {
        if ($grid.jqGrid('getGridParam', 'selrow') === null) {
            return []
        } else {
            return $grid.jqGrid('getRowData', $grid.jqGrid('getGridParam', 'selrow'));
        }
    }
}
$.checkEmail = function(value) {
    var reg = new RegExp(/^([a-zA-Z0-9._-])+@([a-zA-Z0-9_-])+(\.[a-zA-Z0-9_-])+/)
    return reg.test(value)
}
$.fn.formValid = function() {
    return $(this).valid();
}
$.fn.formSerialize = function(formdate, ignore) {
    var ignore = ignore === undefined ? false : ignore
    //console.log(ignore)
    var element = $(this);
    if (!!formdate) {
        for (var key in formdate) {
            var $id = element.find("[name=" + key + "]");
            var value = $.trim(formdate[key]).replace(/&nbsp;/g, '');
            var value = $.trim(formdate[key]);
            var type = $id.attr('type');
            if ($id.hasClass("select2-hidden-accessible")) {
                type = "select";
            }
            if ($id.hasClass('uploader')) {
                type = 'uploader'
            }
            switch (type) {
                case "checkbox":
                    if (value == "true") {
                        $id.prop("checked", true);
                    } else if (value == "false") {
                        $id.prop("checked", false);
                    } else {
                        var array = value.split(',')
                        $.each(array, function(i, item) {
                            $('#' + key + '-' + item).prop('checked', true)
                        })
                    }
                    break;
                case "select":
                    $id.val(value).trigger("change");
                    break;
                case "radio":
                    if (value) {
                        //console.log(value)
                        $id.parent().find("[value=" + value + "]").prop('checked', true)
                    }
                    break;
                case "uploader":
                    if (value) {
                        $('#' + $id.attr('name') + '-file')[0].setFiles(value)
                    }
                    break;
                default:
                    $id.val(value);
                    break;
            }
        }
        ;return false;
    }
    var postdata = {};
    element.find('input,select,textarea').each(function(r) {
        var $this = $(this);
        var name = $this.attr('name');
        var type = $this.attr('type');
        var id = $this.attr('id')
        if (!ignore || (ignore && !$(this).is('.ignore, .disabled'))) {
            switch (type) {
                case "checkbox":
                    if ($this.attr('multiselect')) {
                        if ($this.is(":checked")) {
                            if (postdata[name]) {
                                postdata[name] += $(this).val() + ','
                            } else {
                                postdata[name] = $(this).val() + ','
                            }
                        }
                    } else {
                        postdata[name] = $this.is(":checked");
                    }
                    break;
                case "radio":
                    if ($this.is(":checked")) {
                        postdata[name] = $(this).val();
                    }
                    break;
                default:
                    var value = null;
                    //value = $this.val();
                    if ($this.val() == "") {
                        value = "&nbsp;"
                    } else if ($this.val() == null) {
                        value = "&nbsp;"
                    } else
                        value = $this.val();
                    //value = $this.val() == null ? "&nbsp;" : $this.val();
                    //alert(value)
                    if (!$.request("keyValue")) {
                        value = value.toString().replace(/&nbsp;/g, '');
                        //value = value.toString();
                    }
                    postdata[name] = value;
                    break;
            }
        }
    });
    if ($('[name=__RequestVerificationToken]').length > 0) {
        postdata["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    return postdata;
}
$.fn.teacherSelect = function(selectData, singleSelect) {
    var singleSelect = singleSelect || false
    var selectedData = []
    var $container = $('<div id="choose-teacher"></div>')
    var $searchBar = $('<div class="choose-teacher-top"><div style="float:left;margin-left: 10px">学部:</div><div class="teacher-filter"><select id="F_Divis_ID" placeholder="请选择学部"></select></div><div style="float:left;margin-left: 10px">姓名:</div><div class="teacher-filter"><input placeholder="搜索教师" class="teacher-seek" /><i class="fa fa-search"></i></div></div>')
    var $teacherContent = $('<p style="margin-left: 35px;font-size: 16px;margin-top: 15px;">待选择</p><div class="choose-teacher-content"><ul class="clearfix"></ul></div>')
    var $feedBackContent = $('<p style="margin-left: 35px;font-size: 16px;margin-top: 15px;">已选择</p><div class="choose-teacher-content feed-back"><ul class="clearfix"></ul></div>')
    if (selectData.length) {
        if (selectData.length) {
            selectedData = _.map(selectData, function(item, i) {
                return {
                    F_Id: item.School_Teachers_Entity ? item.School_Teachers_Entity.F_Id : '',
                    F_FacePhoto: item.School_Teachers_Entity ? item.School_Teachers_Entity.F_FacePhoto : '',
                    F_Name: item.School_Teachers_Entity ? item.School_Teachers_Entity.F_Name : ''
                }
            })
        }
    }
    $container.append($searchBar).append($teacherContent).append($feedBackContent)
    $searchBar.find('#F_Divis_ID').bindSelect({
        url: '/SystemManage/Organize/GetSelectJsonByCategoryId?keyword=Division'
    })
    $(this)[0].getTeacherList = function() {
        var $li = $feedBackContent.find('ul li')
        var output = []
        if ($li.length) {
            $li.each(function() {
                var id = $(this).attr('data-id')
                output.push(id)
            })
        }
        return output.join(',')
    }
    $(this)[0].getTeacherListOriginal = function() {
        var $li = $feedBackContent.find('ul li')
        var output = []
        if ($li.length) {
            $li.each(function() {
                var id = $(this).attr('data-id')
                var img = $(this).attr('data-imgsrc')
                var name = $(this).attr('data-name')
                output.push({
                    F_Id: id,
                    F_FacePhoto: img,
                    F_Name: name
                })
            })
            return output
        }
    }
    $(this).html($container)
    function getTeacherList(name, id) {
        var deferred = $.Deferred()
        $.ajax({
            url: '/SchoolManage/School_Teachers/GetGridSelect',
            type: 'GET',
            dataType: 'json',
            data: {
                F_Name: name || '',
                F_Divis_ID: id || ''
            },
            success: function(data) {
                if (data.state === "error") {
                    $.modalAlert(data.message, 'error')
                    return
                }
                if (data.length === 0) {
                    $teacherContent.find('ul').html('<p>未找到相关教师</p>')
                    return
                }
                var list = renderList(data, true)
                $teacherContent.find('ul').html(list)
                deferred.resolve(data)
            }
        })
        return deferred.promise()
    }
    function renderList(data, select) {
        var list = ''
        $.each(data, function(i, item) {
            if (select) {
                var selected = _.find(selectedData, function(n) {
                    return n.F_Id === item.F_Id
                })
            }
            list += '<li data-imgsrc=' + item.F_FacePhoto + ' data-name=' + item.F_Name + ' data-id=' + item.F_Id + '>' + '<div class="teacher-icon"><img src="' + item.F_FacePhoto + '" /></div>' + '<div class="teacher-name">' + item.F_Name + '</div>' + (select ? '<div class="teacher-checkbox ' + (selected ? 'active' : '') + '"></div>' : '<div class="cancel">x</div>') + '</li>'
        })
        return list
    }
    function getSelectList() {
        var $selectList = renderList(selectedData)
        $feedBackContent.find('ul').html($selectList)
        $feedBackContent.find('ul li .cancel').off('click').on('click', function() {
            var $li = $(this).parents('li')
            var id = $li.attr('data-id')
            $li.remove()
            $teacherContent.find('li[data-id=' + id + ']').find('.teacher-checkbox').removeClass('active')
            _.remove(selectedData, function(n) {
                return n.F_Id === id
            })
        })
    }
    if (selectedData.length) {
        getSelectList()
    }
    $searchBar.find('i').click(function() {
        getTeacherList($searchBar.find('.teacher-seek').val(), $searchBar.find('#F_Divis_ID').val())
        $teacherContent.off('click').on('click', 'ul li', function() {
            var param = {
                F_Id: $(this).attr('data-id'),
                F_Name: $(this).attr('data-name'),
                F_FacePhoto: $(this).attr('data-imgsrc')
            }
            if (singleSelect && $feedBackContent.find('ul li').length >= 1) {
                $.modalMsg('只能选择一个', 'warning')
                return
            }
            if (!$(this).find('.teacher-checkbox').hasClass('active')) {
                $(this).find('.teacher-checkbox').addClass('active')
                selectedData.push(param)
                getSelectList()
            }
        })
    })
}
$.fn.bindSelect = function(options) {
    var defaults = {
        id: "id",
        text: "text",
        search: true,
        url: "",
        param: [],
        change: null,
        dictionary: "",
        dictionaryOrg: "",
        OrgId: "",
        //显示请选择
        displayBlank: true
    };
    var options = $.extend(defaults, options);
    if (options.search) {
        options.matcher = function(params, data) {
            if (params.term == null || $.trim(params.term) === '') {
                return data
            }
            if (data.keyword.toUpperCase().indexOf(params.term.toUpperCase()) >= 0) {
                return data
            }
            return false
        }
    }
    var $element = $(this);
    if (options.dictionary !== "") {
        var dictionary = top.clients.dataItems
        var selectOptions = $.map(dictionary, function(item, i) {
            if (i === options.dictionary) {
                var selectDom = $.map(item, function(_item, _i) {
                    return '<option value=' + _i + '>' + _item + '</option>'
                })
                return selectDom
            }
        })
        if (options.displayBlank) {
            $element.empty()
            $element.append('<option value="">--请选择--</option>')
        }

        $element.append(selectOptions.join(''))
    }
    if (options.OrgId !== "") {
        if (options.dictionaryOrg !== "") {
            var dictionary = top.clients.orgDic
            var selectOptions = $.map(dictionary, function(item, i) {
                if (i === options.OrgId) {
                    var selectDom = $.map(item, function(_item, _i) {
                        if (_i === options.dictionaryOrg) {
                            var selectOrg = $.map(_item, function(item1, i1) {
                                return '<option value=' + item1.text + '>' + item1.text + '</option>'
                            })
                            return selectOrg
                        }
                    })
                    return selectDom
                }
            })
            if (options.displayBlank) {
                $element.empty()
                $element.append('<option value="">--请选择--</option>')
            }
            $element.append(selectOptions.join(''))
        }
    }

    if (options.url != "") {
        $.ajax({
            url: options.url,
            data: options.param,
            dataType: "json",
            async: false,
            success: function(data) {
                if (options.displayBlank) {
                    $element.empty()
                    $element.append('<option value="">--请选择--</option>')
                }
                $.each(data, function(i) {
                    $element.append($("<option></option>").val(data[i][options.id]).html(data[i][options.text]));
                });
                $element.select2({
                    minimumResultsForSearch: options.search == true ? 0 : -1,
                    width: 150
                });
                $element.on("change", function(e) {
                    $(this).trigger('blur');
                    if (options.change != null) {
                        options.change(data[$(this).find("option:selected").index()]);
                    }
                    $("#select2-" + $element.attr('id') + "-container").html($(this).find("option:selected").text().replace(/　　/g, ''));
                });
            }
        });
    } else {
        $element.select2({
            minimumResultsForSearch: options.search == true ? 0 : -1,
            width: 150
        });
    }
}
$.fn.bindCheckBox = function(options) {
    var defaults = {
        id: "id",
        text: "text",
        search: true,
        url: "",
        param: [],
        dictionary: "",
        name: "",
        required: true
    };
    var options = $.extend(defaults, options);
    var $element = $(this);
    if (options.url != "") {
        $.ajax({
            url: options.url,
            data: options.param,
            dataType: "json",
            async: false,
            success: function(data) {
                $.each(data, function(i) {
                    var $checkbox = $("<div class='ckbox'><input multiselect='true' type='checkbox' value='" + data[i][options.id] + "' name=" + options.name + " id='" + data[i][options.id] + "' /><label for='" + data[i][options.id] + "'>" + data[i][options.text] + "</label></div>")
                    if (data[i]['ifChecked']) {
                        $checkbox.find('input').attr('checked', 'checked')
                    }
                    $element.append($checkbox)
                });
                //$element.find('.ckbox:first-child input').attr('required', 'required')
            }
        });
    }
    if (options.dictionary) {
        var data = top.clients.dataItems[options.dictionary]
        var checkboxes = $.each(data, function(i, item) {
            $element.append('<div class="ckbox"><input multiselect="true" type="checkbox" id="' + options.name + '-' + i + '" value="' + i + '" name="' + options.name + '" /><label for="' + options.name + '-' + i + '">' + item + '</label></div>')
        })
    }
    if (options.required) {
        $element.find('.ckbox:first-child input').attr('required', 'required')
    }
}
$.fn.authorizeButton = function() {
    var moduleId = top.$(".NFine_iframe:visible").attr("id").substr(6);
    var dataJson = top.clients.authorizeButton[moduleId];
    var $element = $(this);
    $element.find('a[authorize=yes]').attr('authorize', 'no');
    if (dataJson != undefined) {
        $.each(dataJson, function(i) {
            $element.find("#" + dataJson[i].F_EnCode).attr('authorize', 'yes');
        });
    }
    $element.find("[authorize=no]").parents('li').prev('.split').remove();
    $element.find("[authorize=no]").parents('li').remove();
    $element.find('[authorize=no]').remove();
}
$.fn.initTabs = function(data) {
    var $this = $(this)
    var template = []
    template.push('<ul class="nav nav-tabs">')
    $.each(data, function(i, item) {
        template.push('<li class="' + (i === 0 ? 'active' : '') + '">')
        template.push('<a href="#' + item.id + '" data-toggle="tab">' + item.name + '</a>')
        template.push('</li>')
    })
    template.push('</ul>')
    template.push('<div class="tab-content">')
    $.each(data, function(i, item) {
        template.push('<div class="tab-pane ' + (i === 0 ? 'active' : '') + '" id="' + item.id + '">')
        template.push('<iframe src="' + item.src + '"></iframe>')
        template.push('</div>')
    })
    template.push('</div>')
    $this.html(template.join(''))
}
$.fn.dataGrid = function(options) {
    var defaults = {
        datatype: "json",
        rownumbers: true,
        gridview: true,
        altRows: true,
        viewrecords: true,
        autoGridWidth: true,
        multiselect: true,
        //开启多选,
        rowNum: 15,
        autoGridWidthFix: 0,
        autoGridHeightFix: 0,
        shrinkToFit: true,
        autoGridHeight: true,
        styleUI: 'Bootstrap'
    };
    var options = $.extend(defaults, options);
    var $element = $(this);
    options["onSelectRow"] = function(rowid) {
        var grid = new Array();
        if ($(this).jqGrid("getGridParam", "selarrrow").length !== 0) {
            grid = $(this).jqGrid("getGridParam", "selarrrow");
        } else {
            if ($(this).jqGrid("getGridParam", "selrow") !== null) {
                grid.push($(this).jqGrid("getGridParam", "selrow"));
            } else {
                grid = [];
            }
        }
        var $operate = $element.parents('.gridPanel').parent().find('.operate');
        //by ben
        //var $operate = $(".operate");
        var length = grid.length;
        $('.operate .first span').html(length)
        if (length > 0) {
            $operate.animate({
                "left": 0
            }, 200);
            if (length !== 1) {
                $operate.find('#NF-edit, #NF-Details, #NF-revisepassword').addClass('disabled');
                $operate.find('.mutil-edit, .muilt-edit').addClass('disabled');
                $operate.find('#NF-approve,#NF-discard,#NF-submit').addClass('disabled');
            } else {
                $operate.find('#NF-edit, #NF-Details, #NF-revisepassword').removeClass('disabled');
                $operate.find('.mutil-edit, .muilt-edit').removeClass('disabled');
                $operate.find('#NF-approve,#NF-discard,#NF-submit').removeClass('disabled');
            }
        } else {
            $operate.animate({
                "left": '-100.1%'
            }, 200);
        }
        $operate.find('.close').click(function() {
            $operate.animate({
                "left": '-100.1%'
            }, 200);
        });
        if (options['onCustomSelectRow'] != null) {
            options['onCustomSelectRow'](rowid);
        }
    }
    ;
    options['onSelectAll'] = function(rows) {
        var length = $(this).jqGrid("getGridParam", "selarrrow").length;
        $('.operate .first span').html(length)
        var $operate = $element.parents('.gridPanel').parent().find('.operate');
        //by ben
        //var $operate = $(".operate");
        if (length > 0) {
            if (length !== 1) {
                $operate.find('#NF-edit, #NF-Details, #NF-revisepassword').addClass('disabled');
                $operate.find('#NF-approve,#NF-discard,#NF-submit').addClass('disabled');
            } else {
                $operate.find('#NF-edit, #NF-Details, #NF-revisepassword').removeClass('disabled');
                $operate.find('#NF-approve,#NF-discard,#NF-submit').removeClass('disabled');
            }
            $operate.animate({
                "left": 0
            }, 200);
        } else {
            $operate.animate({
                "left": '-100.1%'
            }, 200);
        }
        $operate.find('.close').click(function() {
            $operate.animate({
                "left": '-100.1%'
            }, 200);
        })
    }
    $element.jqGrid(options);
    function setGridHeight() {
        var gridHeight = 0
        if (!options.autoGridHeight) {
            return
        }
        if (typeof options.autoGridHeight == "function") {
            gridHeight = options.autoGridHeight()
        } else {
            var tabContent = $element.parents(".tab-content")
            if (tabContent.length > 0) {} else {
                var gridHeight = $element.height()
                var gridParent = $element.parent()
                if (gridParent.length != 0) {
                    console.log(gridParent.height())
                    gridHeight = gridParent.height()
                }
                console.log($element, gridParent)
                gridHeight = ($(window).height() - $("body").height() + gridHeight - options.autoGridHeightFix)
                if (gridHeight < 150) {
                    gridHeight = 150
                }
                gridParent.height(gridHeight - 20)
            }
        }
        if (gridHeight != 0) {
            $element.jqGrid("setGridHeight", gridHeight)
        }
    }
    function setGridWidth() {
        if (!options.autoGridWidth) {
            return
        }
        var gridWidth = 0
        if (typeof options.autoGridWidth == "function") {
            gridWidth = options.autoGridWidth()
        } else {
            var jqGridParent = $element.parents(".ui-jqgrid").parent()
            if (jqGridParent.is(":visible")) {
                gridWidth = jqGridParent.width() - 2
            }
        }
        if (gridWidth != 0) {
            $element.jqGrid("setGridWidth", gridWidth - options.autoGridWidthFix, (options.shrinkToFit && $(window).width() > 500))
        }
    }
    function resizeDataGrid() {
        setGridHeight()
        setGridWidth()
        setTimeout(function() {
            setGridHeight()
            setGridWidth()
        }, (!!navigator.userAgent.match(/MSIE 8.0/)) ? 200 : 100)
    }
    resizeDataGrid()
    $(window).resize(function() {
        resizeDataGrid()
    })
    $(window).resize()
    return $element
}
