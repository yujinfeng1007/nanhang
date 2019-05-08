var fyAjaxLoading = $('<div id="fyAjaxLoading"><div id="fyAjaxLoadingText">数据传送中，请稍候...</div></div>').appendTo("body");
var sys = {
    saveSysInfoToCookie: function (e) {
        var t = {
            userName: e.userName,
            userId: e.userId,
            realName: e.realName,
            roleId: e.roleId,
            schoolOpen: e.schoolOpen,
            schoolClose: e.schoolClose,
            schoolName: e.xxmc,
            date: parseInt(e.date, 10),
            xq: e.xq,
            xqIx: sys.transXQM(e.xqm),
            zc: e.zc,
            stuJb: e.StuJb,
            bjDisplayName: e.BjDisplayName,
            termAlias :e.termAlias,
            XqhId :e.XqhId,
            uid: e.uid,
            xjh: e.StuXjh,
            bynf:e.bynf,
            token: e.token
        };
        if (e.roleId === "t") { delete t.stuJb; delete t.xjh; delete t.bynf; }
        var o = $.extend({}, t);
        fy.cookie("sysInfo", JSON.stringify(o), {expires: 1, path: "/"});
        return t
    },
    transXQM: function (e) {
        var t = parseInt(e, 10).toString() + (e.indexOf("一") > -1 ? 1 : 2);
        return parseInt(t, 10)
    },
    shortGradeName: function (e) {
        if (e) {
            var t = e.replace(/年级/g, "").replace(/([高|初|小])([中|学])(.*)/g, "$1$3");
            if (t.indexOf("小") === -1) return t;
            return t.replace(/^小/, "")
        }
    },
    shortClassName: function (e, t) {
        return sys.shortGradeName(e) + " (" + (t || "").toString().replace(/班/, "") + ")班"
    },
    parseJsonDate: fy.parseJsonDate,
    getToday: function () {
        if (sys.sysInfo.date) {
            return new Date(sys.sysInfo.date)
        }
        return new Date
    },
    getDatesByWeek: function (e, t, o, n) {
        var i = n || "MM/dd";
        var s = e.getDay();
        var r = o > 1 ? fy.addDays(e, (o - 1) * 7 - s + 1) : e;
        var a = fy.addDays(r, 7 - r.getDay());
        if (a > t)a = t;
        return {from: fy.formatDate(r, i), to: fy.formatDate(a, i)}
    },
    caculWeeks: function (e, t) {
        var o = fy.parseDate(e.StartTime), n = fy.parseDate(e.StopTime), i = fy.weekSpan(n, o);
        var s = {length: i};
        for (var r = 1; r <= i; r++)s[r] = sys.getDatesByWeek(o, n, r, t);
        return s
    },
    calculWeekNum: function (e) {
        if (typeof e === "string")e = fy.parseDate(e);
        return fy.weekSpan(e, sys.sysInfo.schoolOpenDate)
    },
    safeHTML: function (e) {
        return e.replace(/</g, "&lt").replace(/>/g, "&gt").replace(/"/g, "''")
    },
    ico: {
        ok: "img/icon_16_3/ok_check_yes_tick_accept_success-16.png",
        error: "img/icon_16_3/error_warning_alert_attention-16.png",
        warn: "img/icon_16_3/danger_hanger_triangle_traffic_cone-16.png",
        info: "img/icon_16_3/info_information_user_about-16.png",
        confirm: "img/icon_16_3/confirmation_verification-16.png"
    },
    error: function (e, t, o) {
        fy.alert(sys.ico.error + '<div style="display: inline-block">' + e + "</div>", t, o)
    },
    warn: function (e, t, o) {
        fy.alert(this.ico.warn + '<div style="display: inline-block">' + e + "</div>", t, o)
    },
    ok: function (e, t, o) {
        fy.alert(this.ico.ok + '<div style="display: inline-block">' + e + "</div>", t, o)
    },
    alert: function (e, t, o) {
        fy.alert(this.ico.info + '<div style="display: inline-block">' + e + "</div>", t, o)
    },
    confirm: function (e, t, o) {
        fy.confirm(this.ico.confirm + '<div style="display: inline-block">' + e + "</div>", t || fy.EMPTY_FN, o)
    },
    wrapHandler: function (e, t) {
        return {actionCode: e, actionParam: JSON.stringify(t)}
    },
    refreshWidget: function (e) {
        var t = $("#wdg_" + e).find("iframe");
        if (t.length) {
            var o = t[0].contentWindow;
            if (o)o.location.reload(true)
        }
    },
    noty: function (e, t) {
        var o;
        if (typeof e === "string")o = {type: "success", timeout: 1e3, layout: "bottomCenter"}; else o = e;
        o.text = fy.cutChars(o.text || "", 88);
        var n = $.extend({
            theme: "relax",
            maxVisible: 1,
            killer: false,
            timeout: sys.inSiteMessageRefresh - 2e3,
            type: "alert",
            layout: "bottomRight"
        }, o);
        var i = top.window.noty(n), s;
        if (typeof t === "string") {
            if (!top.window.sys.noty[t])top.window.sys.noty[t] = top.window.searchItemByUrl(t);
            if (top.window.sys.noty[t]) {
                s = function () {
                    top.window.sys.noty[t].trigger("click")
                }
            }
        } else if (typeof t === "function") {
            s = t
        }
        if (s)i.$message.click(s);
        return i
    },
    getMyBoxy: function (e) {
        var t = e || window, o = null;
        if (t === top) {
            o = t.fy.popupManager.get(t.frameElement)
        } else if (t.frameElement && t.parent.window.fy) {
            o = t.parent.window.fy.popupManager.get(t.frameElement)
        }
        return o
    },
    openedWins: fy.getPopuppedIFrames(),
    openModule: function (e) {
        if (!sys.noty[e])sys.noty[e] = window.searchItemByUrl(e);
        if (sys.noty[e])sys.noty[e].trigger("click")
    },
    openIFrame: function (e, t, o, n) {
        var i = e.url.toString();
        if (i.indexOf("../../") === 0) {
            e.url = fy.serverRootPath + i.substr(6)
        } else if (i.indexOf("../") === 0) {
            e.url = fy.serverRootPath + "page/" + i.substr(3)
        } else if (i.indexOf("/page/") === 0) {
            e.url = fy.serverRootPath + i.substr(1)
        } else if (i.indexOf(fy.serverRootPath) === 0) {
        } else e.url = fy.serverRootPath + i;
        var ii = e.url.toString();
        if (ii.indexOf("/page/") === -1) {
            throw new Error('链接必须使用 "/page/module/pageUrl" 这样的形式！')
        }
        e.mode = o;
        e.oUrl = i;
        return fy.popIFrame(e, t, n)
    },
    openToTop: function (e, t, o, n) {
        return top.sys.openIFrame(e, t, o, n)
    },
    getPopIframe: function (e) {
        var t = e.jq.find("iframe")[0];
        return t.contentWindow ? t.contentWindow : t.contentDocument.defaultView
    },
    title: {
        docTitle: function () {
            var e = self === top ? document : top.window.document;
            return e.title || "智慧校园"
        }(), taskId: "blinkDocumentTitle", blink: function (e) {
            var t = self === top ? document : top.window.document, o = 0;
            var n = function () {
                o = ++o % 2;
                if (o)t.title = window.sys.title.docTitle + "　【" + e + "】"; else t.title = window.sys.title.docTitle
            };
            if (fy.timerManager.has(window.sys.title.taskId))fy.timerManager.remove(window.sys.title.taskId);
            fy.timerManager.add({id: window.sys.title.taskId, timeOut: 200, fn: n})
        }, stopBlink: function () {
            fy.timerManager.remove(window.sys.title.taskId);
            var e = self === top ? document : top.window.document;
            e.title = window.sys.title.docTitle
        }
    },
    plugins: {},
    ajaxHandlers: 0,
    ajaxLoadingTimer: 0,
    ajaxLoadingShow: function () {
        window.fyAjaxLoading.stop(true, true).fadeIn()
    },
    ajaxLoadingHide: function () {
        window.fyAjaxLoading.stop(true, true).fadeOut()
    },
    init: function () {
        log(this.debug);
        if (!this.loginPage)this.loginPage = this.rootPath + "iPadFrame.html";
        fy.onAjaxError = this.error;
        fy.serverRootPath = sys.rootPath;
        for (var e in this.ico)this.ico[e] = '<img src="' + this.rootPath + this.ico[e].replace(/^['/']/, "") + '" class="tipIco" />';
        if (self === top) {
            fyAjaxLoading.css("height", "auto");
            var t = fy.cookie("sysInfo");
            this.sysInfo = t ? JSON.parse(t) : {};
            if (this.sysInfo && this.sysInfo.date) {
                this.sysInfo.date = parseInt(this.sysInfo.date, 10)
            }
            if (window.opener && window.opener.document)document.title = window.opener.document.title; else if (this.sysInfo.schoolName)document.title = this.sysInfo.schoolName
        } else {
            if (top.window.sys && top.window.sys.sysInfo)this.sysInfo = top.window.sys.sysInfo; else {
                var o = fy.cookie("sysInfo");
                this.sysInfo = o ? JSON.parse(o) : {}
            }
        }
        if (this.sysInfo && this.sysInfo.xqIx) {
            var n = this.sysInfo.xqIx + "";
            this.sysInfo.xqm = n.substr(0, 4) + "学年度第" + (n.substr(4, 1) == "2" ? "二" : "一") + "学期";
            this.sysInfo.schoolOpenDate = fy.parseDate(this.sysInfo.schoolOpen)
        }
    }

};