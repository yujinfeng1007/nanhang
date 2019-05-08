fy.server({
    wallPapers: "json/wallpapers.json",
    sysInfo: "/ClientsData/GetClientsDataJson",
    saveWidget: "/SystemManage/User/SubmitSetUp",
    getWidget: "/SystemManage/CurrentUser/GetCurrentUser",
    getOrgDic: "/SystemManage/Organize/GetOrgDics",
    getWeather: "http://tj.nineton.cn/Heart/index/all?city=CHSH000000&language=zh-chs&unit=c&aqi=city&alarm=1&key=78928e706123c1a8f1766f062bc8676b "
});
//fy.server.sysMsg.unlimited = fy.server.setLog.unlimited = true;
if (fy.request["isout"] == "1") {
    $("#acSignOut").hide();
    var tgl = $("#acToggle"), olink = tgl.attr("href");
    tgl.attr("href", olink + location.search)
}
var setTopBar = function (e) {
    var t = e.role == "教师" ? "老师" : e.role == "学生" ? "同学" : "同学家长";
    var sp = $("#spTopBar").html(e.schoolWeek + '　　<a href="#" onclick="viewUserProfile(\'' + e.userId + "' , '" + e.roleId + '\')" class="aSnow">' + e.realName + "</a>" + t + "，欢迎您");
    var _mail = $('<span id="mailinfo" style="padding-left: 10px;"></span>').appendTo(sp);
    showunreadmail(e, _mail);
    var s = $("#topNavBar").show();
    (function (e) {
        var t = $("body");
        var s = e.find("a").eq(0), i = {right: t.width() - s.position().left - s.width() + 19, width: s.width()};
        var a = $('<div id="sdUnder"></div>').appendTo(e).css(i);
        e.delegate("a", "mouseenter", function () {
            var e = $(this), s = {right: t.width() - e.position().left - e.width() + 19, width: e.width()};
            if (document.addEventListener)a.stop().animate(s, 200); else a.css(s)
        })
    })(s);
    sys.sysInfo = sys.saveSysInfoToCookie(e);
    if (e.roleId !== "t" && e.hasAd) {
        alert(1);
        var i = $('<div id="adv-top">广告位置</div>').appendTo("body");
        $('<img id="adv-top-close" src="../../img/close_w.jpg">').appendTo(i).click(function () {
            $(this).unbind();
            i.slideUp(function () {
                i.remove()
            })
        })
    }
};

function showunreadmail(e,tar){
    if(sys.viewemail){
        var _even = e.dzxx ? 'onclick="viewemailinfo();"' : '',
            _text = e.dzxx ? ' <cus style="font-weight: bold;">未读【<cus1 style="color:#FF0000;text-decoration: none;">'+ e.unreadmail +'</cus1>】</cus1>' : ' <cus1 style="color: #FF0000;font-weight: bold;">未启用</cus1>';
        tar.html('<a class="aSnow"  href="#" '+_even+' ><img title="'+ e.dzxx +'" src="../../img/icon/chat.png" class="icoTip">'+_text+'</a>');
    }

}

if(sys.viewemail) {
    (function timerMailCount(t) {
        var timer = setTimeout(refreshunreadmailcount, t);
        function refreshunreadmailcount() {
            clearTimeout(timer);
            fy.server.refreshunreadmail.getJSON(function (json) {
                var e = json.data;
                showunreadmail(e,$('#mailinfo'));
                timer = setTimeout(refreshunreadmailcount, t);
            })
        }
    })(sys.emailRefresh);
}



function viewemailinfo(){
    fy.server.viewemail.getJSON(function(json){
        window.open(json.data);
    });
}
function viewUserProfile(e, t) {
    var s = t === "t" ? "../userProfiles/viewTeacherProfile.shtml" : "../userProfiles/viewStudentProfile.shtml";
    if (typeof sys.noty[s] === "undefined")sys.noty[s] = window.searchItemByUrl(s);
    if (sys.noty[s]) {
        sys.noty[s].trigger("click")
    } else {
        sys.openIFrame({url: s}, {title: "个人身份信息", maximizable: true, closable: true}).maximum()
    }
}
function signOut() {
    fy.confirm("<div style='margin: 0 30px 12px 30px;line-height: 22px;'><iframe id='crossoriginFrm' style='height: 0;width:0'><iframe id='sessionFrm' style='height: 0;width:0'></iframe><label><input name='iptSignOut' type='radio' value='0'>退出后自动关闭页面</label><br>" + "<label><input name='iptSignOut' type='radio' value='1' checked>重新登录</label></div>", function () {
        var reLoginPage = fy.cookie('reLoginPage');
        var $crossoriginFrm = $('#crossoriginFrm');
        $crossoriginFrm.attr("src", reLoginPage);
        var $sessionFrm = $('#sessionFrm');
        $sessionFrm.attr("src", "../../page/system/ClearOtherSession.aspx");
         fy.server.UpdateOutTime.post(function (json) {
            fy.server.signOut.getJSON(function () {
               
                if ($(':radio[name="iptSignOut"]:checked').val() == "0") {
                    try {
                        window.opener = null;
                        window.open("", "_self");
                        window.close()
                    } catch (e) {
                        window.location.replace(sys.loginPage);
                    }
                } else {
                        window.location.replace(sys.loginPage);
                }
            })
        });
    }, {title: "<img src='../../img/icon/shutdown.png' align='absmiddle'> 您确定要退出系统吗？", draggable: false, modal: .66})
}
var gotoFullScreen = function () {
    var e = false;
    return function (t) {
        if (e) {
            fy.runPrefixMethod(document, "CancelFullScreen") || fy.runPrefixMethod(document, "ExitFullscreen");
            e = false;
            $(t).html('<img src="../../img/ico_16_2/user-desktop.png" class="icoTip"/>全屏')
        } else {
            fy.runPrefixMethod(document.documentElement, "RequestFullScreen") || fy.runPrefixMethod(document.documentElement, "RequestFullscreen");
            e = true;
            $(t).html('<img src="../../img/ico_16_2/user-desktop.png" class="icoTip"/>还原')
        }
    }
}();
function findIndexOfMinValue(e) {
    var t = Math.min.apply(Math, e);
    for (var s = 0, i = e.length; s < i; s++)if (e[s] === t)return s;
    return-1
}
var widgets = [], myWidgets = null;
sys.htmlWidgets = {loadJS: function (obj, ref) {
    var outer = ref.parents(".widget"), titleBar = ref.siblings(".widgetTitle");
    ref.css("backgroundColor", "transparent");
    titleBar.css("visibility", "hidden");
    outer.hover(function () {
        titleBar.css("visibility", "visible")
    }, function () {
        titleBar.css("visibility", "hidden")
    });
    if (this[obj.id])this[obj.id](ref); else {
        $.get(sys.rootPath + obj.src, function (str) {
            eval('sys.htmlWidgets["' + obj.id + '"]=' + str);
            sys.htmlWidgets[obj.id](ref)
        }, "script")
    }
}, loadHTM: function (e, t) {
    t.load(sys.rootPath + e.src)
}};
function makeWidgets() {
    //fy.server.widgets.getJSON({type: sys.sysInfo.roleId}, function (e) {
        var e = {
            "data": [{
                "id": "wh02",
                "title": "Flash时钟",
                "height": "0",
                "discript": "透明背景Flash",
                "type": "js",
                "src": "json/widget_wh02.js"
            }],
            "error": ""
        };
        var t = e.data.length;
        window.widgets = {};
        while (t--)window.widgets[e.data[t]["id"]] = e.data[t];

        if (window.myWidgets)loadWidgets(window.myWidgets);
        window.chooseApp = function () {
            var t = fy("#ulApps").list({template: '<li class="appList">' + '<label><input type="checkbox" value="{id}"/>{title}</label>' + '<span class="fGray">{discript}</span>' + "</li>", data: e.data, onCreate: function () {
                this.extra({checkBoxes: this.jq.find(":checkbox")})
            }});
            var s = fy.confirm(t.jq.show(), function () {
                var e = {};
                t.checkBoxes.filter(":checked").each(function () {
                    e[this.value] = this.value in window.widgetIds ? "exist" : "add"
                });
                for (var s in window.widgetIds) {
                    if (s in e) {
                    } else {
                        e[s] = "remove"
                    }
                }
                var i = {add: [], remove: []};
                for (var a in e) {
                    if (e[a] === "add")i.add.push(a); else if (e[a] === "remove") {
                        $("#wdg_" + a).remove();
                        if (window.widgetIds)delete window.widgetIds[a];
                        i.remove.push(a)
                    }
                }
                window.saveLayouts(i)
            }, {title: "<img src='../../img/icon/app.png' align='absmiddle'> 选择您的首页应用", show: false, closeable: true, unloadOnHide: false, afterShow: function () {
                t.checkBoxes.each(function () {
                    this.checked = this.value in window.widgetIds
                })
            }});
            return function () {
                s.show()
            }
        }()
    //})
}
function loadWidgets(e) {
    if (window.widgetIds)$(".screenColumn", "#widgets").sortable("destroy");
    var t = [
        [],
        [],
        []
    ];
    for (var s = 0; s < 3; s++) {
        for (var i = 0, a = e[s].length; i < a; i++) {
            t[s][i] = window.widgets[e[s][i]]
        }
    }
    window.widgetCols = $(".screenColumn", "#widgets").bindLists({lists: t, template: '<div class="widget" id="wdg_{id}" data-wid="{id}">' + '<div class="widgetTitle" data-wid="{id}"><div class="widgetCloser"></div>{title}</div><div class="widgetBody">loading...</div></div>', onBound: function (e) {
        this.find(".widgetBody").each(function (t, s) {
            var i = e[t], a = $(s);
            i.height = parseInt(i.height, 10);
            if (i.type === "iframe") {
                a.css("height", i.height || 200).html('<iframe class="widget-iframe" src="' + (sys.rootPath + i.src) + '" style="height:' + ((i.height || 180) - 10) + 'px;" frameBorder="0"></iframe>')
            } else if (i.type === "js") {
                sys.htmlWidgets.loadJS(i, a)
            } else {
                sys.htmlWidgets.loadHTM(i, a)
            }
        })
    }, onAllComplete: function () {
        var e = $("#widgets").find(".widgetTitle");
        e.find(".widgetCloser").mousedown(function (e) {
            e.stopPropagation()
        }).click(function (e) {
            var t = $(this).parents(".widget");
            if (t.length === 0)t = $(this).parents(".widget-js");
            var s = t.data("wid");
            sys.confirm("您确定要从桌面移除本应用吗？<br>关闭后您可以从【应用选择器】中再次加入它。<br>&nbsp;", function () {
                t.remove();
                if (window.widgetIds)delete window.widgetIds[s];
                var e = {add: [], remove: [String(s)]};
                window.saveLayouts(e)
            })
        });
        $(".widget").hover(function () {
            $(this).find(".widgetCloser").toggle()
        });
        if (fy.browser.msie && fy.browser.version < 9)e.css("opacity", .67);
        $(".screenColumn", "#widgets").sortable({handle: ".widgetTitle", helper: function () {
            return $('<div class="widget-helper"></div>')[0]
        }, connectWith: ".screenColumn", placeholder: "widget-highlight", tolerance: "pointer", scroll: false, scrollSpeed: 40, start: function () {
            $("body").disableSelection()
        }, stop: function () {
            $("body").enableSelection();
            window.saveLayouts()
        }});
        window.widgetIds = {};
        e.each(function (e, t) {
            var s = $(t);
            window.widgetIds[s.data("wid")] = "exist"
        })
    }})
}
var saveLayouts = function (e) {
    var t = window.widgetCols;
    var s = [
        [],
        [],
        []
    ];
    t.each(function (e, t) {
        $(".widgetTitle", t).each(function (t, i) {
            var a = $(i);
            s[e][t] = a.data("wid")
        })
    });
    if (e && e.add.length) {
        var i = e.add.length;
        while (i--) {
            var a = [s[0].length, s[1].length, s[2].length], o = window.findIndexOfMinValue(a);
            s[o].push(e.add[i])
        }
        window.myWidgets = s;
        window.loadWidgets(window.myWidgets);
        window.showDesktop()
    } else window.myWidgets = s;
    fy.server.saveWidget.post({ F_User_SetUp: JSON.stringify(s) }, fy.EMPTY_FN, "json")
};
var selectWallpaper = function (e) {
    if (e && e.indexOf("_") > -1) {
        var t = e.indexOf("_");
        e = sys.rootPath + sys.album + e.substr(0, t) + "/" + e.substr(t + 1)
    } else {
        e = sys.rootPath + sys.wallpaperPath + (e || sys.wallpaper)
    }
    var s = document.getElementById("wallpaper"), i = s.src = e || sys.rootPath + sys.wallpaperPath + sys.wallpaper.replace(/^['/']/, ""), a = $('<img id="wpSign" src="' + sys.rootPath + 'img/ico_16_2/icon_tick.png">');
    
    s.onerror = function () {
        s.src = sys.rootPath + 'img/clear.gif';
    };
    var o = fy("#ulWPS").list({template: '<li class="liWPS"><img src="{src:=renderer}" data-src="{src}" class="wpThumb"></li>', renderer: function (e) {
        return sys.rootPath + sys.wallpaperPath + "tmb/" + e
    }, onSelect: function () {
        i = sys.rootPath + sys.wallpaperPath + $(this.getSelectedItem()).prepend(a).find(".wpThumb").data("src")
    }, onCreate: function () {
        n.center();
        var e = this.data.length, t = i.substr(i.lastIndexOf("/") + 1);
        while (e--) {
            if (this.data[e].src === t) {
                this.setSelectedIndex(e);
                break
            }
        }
    }});
    var n = fy.confirm(o.jq.show(), function () {
        console.log(i);
        if (s.src !== i) {
            window.welcomeScreen.fadeIn();
            fy.preloadImage(i, function () {
                s.src = i;
                //fy.server.saveProfile.post({actionCode: "saveWallPapers", wallPapers: i.split(sys.wallpaperPath)[1]}, fy.EMPTY_FN, "json");
                window.welcomeScreen.fadeOut()
            }, function () {
                sys.alert("载入图像失败。");
                window.welcomeScreen.fadeOut()
            })
        }
    }, {title: "<img src='../../img/icon/pixes.png' align='absmiddle'> 系统背景选择", closeable: true, show: false, unloadOnHide: false});
    return function () {
        if (!o.data) {
            o.url = fy.server.wallPapers;
            o.create()
        }
        n.show()
    }
};
function setWallpaper(e) {
    if (e && e.indexOf("_") > -1) {
        var t = e.indexOf("_");
        e = sys.rootPath + sys.album + e.substr(0, t) + "/" + e.substr(t + 1)
    } else {
        e = sys.rootPath + sys.wallpaperPath + (e || sys.wallpaper)
    }
    window.welcomeScreen.fadeIn();
    fy.preloadImage(e, function () {
        var t = document.getElementById("wallpaper");
        t.src = e;
        window.welcomeScreen.fadeOut()
    }, function () {
        sys.alert("载入图像失败。");
        window.welcomeScreen.fadeOut()
    })
}
var fetchMsg = {dict: {error: "error", information: "alert", alert: "alert", warning: "warn", success: "ok"}, show: function () {
    $.noty.clearQueue();
    $.noty.closeAll();
    fy.server.sysMsg.getJSON(function (e) {
        if (e.data.length) {
            sys.title.blink("新消息");
            for (var t = 0, s = e.data.length; t < s; t++) {
                var i = e.data[t], a;
                var o = i.MessageUrl;
                if (o) {
                    if (typeof sys.noty[o] === "undefined")sys.noty[o] = window.searchItemByUrl(o);
                    if (sys.noty[o]) {
                        a = function () {
                            fy.server.setMsgStatus.getJSON({MessageId: arguments.callee.msgId}, fy.EMPTY_FN);
                            arguments.callee.icon.trigger("click")
                        };
                        a.icon = sys.noty[o];
                        a.msgId = i.MessageId
                    } else {
                        a = function () {
                            fy.server.setMsgStatus.getJSON({MessageId: arguments.callee.msgId}, fy.EMPTY_FN);
                            sys.openIFrame({url: arguments.callee.url}, {title: "系统消息", maximizable: true, closable: true}).maximum()
                        };
                        a.msgId = i.MessageId;
                        a.url = o
                    }
                } else {
                    a = function () {
                        var e = arguments.callee.msg;
                        fy.server.setMsgStatus.getJSON({MessageId: e.MessageId}, fy.EMPTY_FN);
                        sys[fetchMsg.dict[e.MessageType]](e.MessageContent)
                    };
                    a.msg = i
                }
                sys.noty({text: i.MessageContent, type: i.MessageType}, a)
            }
        } else sys.title.stopBlink()
    })
}, run: function (e) {
    fy.timerManager.add({id: "refreshSystemMsg", timeOut: sys.inSiteMessageRefresh, fn: fetchMsg.show});
}};
(function () {
    var e = fy.cookie("loginAuth"), t = window.location.pathname, s = t.substr(t.lastIndexOf("/") + 1);
    if (e) {
        e = JSON.parse(e);
        e.ui = s
    } else {
        e = {ui: s}
    }
    fy.cookie("loginAuth", e, {expires: 7, path: window.sys.rootPath})
})();


$(window).on('beforeunload',function(){
    //fy.server.UpdateOutTime.post(function(json){});
});