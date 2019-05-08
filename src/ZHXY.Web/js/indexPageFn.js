
//IE < 10 don't support full screen api
if ($.browser.msie) {
	if ($.browser.version < 11) $('#liFullScr').hide();
	if ($.browser.version < 8) alert('您使用的IE版本过低，请使用IE8以上的浏览器，\n\n如果您打开了浏览器的兼容模式，亦请切换关闭之。\n\n如果您使用的是第三方浏览器，诸如：\n搜狗、360、QQ、百度、金山、世界之窗、遨游等，\n请使用这些浏览器的“高速模式”');
}
//mobile sets
if (fy.browser.touchable) $('#liFullScr').hide();
//search menu item by Url
var searchItemByUrl = function () {
	var wholeLv2Icons = $('#menu-lv1').find(".menu-lv2");
	window.searchItemByUrl = function (url) {
		var _url  , _search ;
		if(url.indexOf('?')>-1) {
			_url = url.substring(0, url.indexOf('?')) ;
			_search = url.substring(url.indexOf('?')) ;
		}
		else {
			_url = url ;
			_search = '' ;
		}
		if(_url.indexOf('../../page/')===0)
			_url = _url.replace('../../page/' , '../') ;

		var x = wholeLv2Icons.length , tar , notFound = true;
		while (x--) {
			tar = wholeLv2Icons.eq(x);
			var link = tar.data("link").replace(/[?].*/gi , '') ;
			if (link === _url) {
				if(_search) tar.data("link" , url);
				notFound = false;
				break;
			}
		}
		return (notFound ? null : tar);
	};
	return false;
};
var extraJson = {
    "userId": 1,
    "userName": "admin",
    "realName": "嘿嘿",
    "date": 1530581237020,
    "schoolWeek": "2018年度第一学期 第20教学周",
    "wallPaper": "Gaoqingzhiwu30.jpg",
    "widgetLayout": [["WS01", "w09", "05", "wh02", "kcb01"], ["wh03", "w04", "07", "MSG01", "F01（自定义单需确保系统唯一性）"], ["wh01", "MSG02", "w03", "W101", "tqqk"]],
    "role": "教师",
    "roleId": "s",
    "schoolOpen": "2018-02-20",
    "schoolClose": "2018-06-29",
    "xqm": "2018学年度第一学期",
    "xq": 25,
    "zc": 20,
    "xxmc": "绿橄榄智慧校园",
    "logoImage": "(131607984127152354)logo.png",
    "dzxx": "",
    "unreadmail": "0",
    "uid": "admin",
    "token": "",
    "termAlias": "2018年度第一学期",
    "XqhId": "1",
    "BjDisplayName": "BH",
    "error": ""
  };
$(document).ready(function ($) {
    fy.server.sysInfo.getJSON(function (json) {
		window.sys.title.docTitle = json.xxmc;
		window.sys.menuIconPath = window.sys.rootPath + window.sys.menuIconPath.replace(/^['/']/, '');
        json = $.extend({},
          json, extraJson); 
        console.log(json);
        top.clients = json;
		//logo
		$('#schLogo').attr({
			'src' : sys.rootPath + sys.logoPath + json.logoImage ,
			'title' : json.xxmc
		}) ;
		//$('#schName').text(json.xxmc) ;
		pageFn.init(json);
	});
});
var pageFn = {
	menuBar: $("#menu-lv1"),
	currentLv1Menu: null,
	currentLv2Menu: null,
	leftSide: $("#pageLeftSide"),
	rightSide: $("#pageRightSide"),
	mainFrame: $("#pageRightFrame"),
	splitter: $("#splitter"),
	menuHidden: false,
    addEventListeners: function () {
        var that = this  , w = that.splitter.width();
        this.splitter.click(function () {
			if (that.menuHidden) {
				that.rightSide.css({left: 170});
				that.leftSide.width(170);
				that.menuBar.show();
				that.menuHidden = false;

				pageFn.splitter.css({
					backgroundPosition: "-10px center"
				});
			}
			else {
				that.menuBar.hide();
				that.leftSide.width(w);
				that.rightSide.css({left: w});
				that.menuHidden = true;

				pageFn.splitter.css({
					backgroundPosition: "0 center"
				});
			}
		});
        //toggle lv2 subMenu
		pageFn.menuBar.delegate(".menu-lv1", "click", function (evt) {
			evt.stopImmediatePropagation();
			var $li = $(this) , sub = $li.next(".ulMenu-lv2");
			if (sub.length) {
				if (pageFn.currentLv1Menu) {
					if (pageFn.currentLv1Menu[0] === this) {
						pageFn.currentLv1Menu.next(".ulMenu-lv2").slideToggle('fast');
						return;
					}
					pageFn.currentLv1Menu.removeClass("menu-lv1-cur").next(".ulMenu-lv2").slideUp('fast');
				}
				sub.slideToggle('fast');
				pageFn.currentLv1Menu = $li.addClass("menu-lv1-cur");
			}
		});

		//lv2 menu item click
		pageFn.menuBar.delegate(".menu-lv2", "click", function (evt) {
			evt.stopImmediatePropagation();
			var $li = $(this) , link = $li.data("link"),widgetType = $li.data("widgettype");
			if(widgetType == 'js')
                window.open(link);
			else
				pageFn.mainFrame.attr("src", link);
			if(sys.setLog) fy.server.setLog.get({t: $li.text(),u:link} , $.noop);
			if (pageFn.currentLv2Menu) pageFn.currentLv2Menu.removeClass("menu-lv2-cur");
			pageFn.currentLv2Menu = $li.addClass("menu-lv2-cur");
		});
        $.contextMenu({
			selector: '.menu-lv2',
			callback: function(key, misc) {
				var menuId = misc.$trigger.data('menu') ,
					liId = 'sct-' + menuId ;
				//log(pageFn.mainFrame[0].contentWindow);

				var win = pageFn.mainFrame[0].contentWindow ;
				if(!win.document.getElementById(liId)) {
					fy.server.shortcuts.getJSON({menuId : menuId , actionCode : 'saveShortcuts'} , function(){
						if(win.refreshMenuShortcut)win.refreshMenuShortcut() ;
					});
				}
			},
			items: {
				"shortcut": {name: "发送到快捷工具栏", icon: "edit"}
			}
		});
    },
    setTopBar: function(json) {
		var sp = $("#spTopBar").html(json.schoolWeek + '　　' + json.realName + ' ' + json.role + '，欢迎您');
        var _mail = $('<span id="mailinfo" style="padding-left: 10px;"></span>').appendTo(sp);
        showunreadmail(json, _mail);
		//save info data
		top.window.sys.sysInfo = sys.saveSysInfoToCookie(json);

		//adv
		// if(json.roleId!=="t"){
         //    var av=$('<div id="adv-top">展示位置</div>').appendTo('body') ;
         //    $('<img id="adv-top-close" src="../../img/close_w.jpg">').appendTo(av).click(function(){
         //        $(this).unbind();
         //        av.remove();
         //    });
        // }

	},
    bindData: function (json) {
		//set info
		this.setTopBar(json);

		//make widgets
		this.mainFrame.attr('src', '../page/system/widgets.html');

		//是否不是教师身份
		var notTeacher = (json.roleId!=="t") ,
			avastHeight = 130 ;

		if (notTeacher) {
			json.avatar = '../../upload/student_photo/' + (json.avatar||json.userName) + '.jpg' ;
			$('<div style="height: ' + avastHeight + 'px;text-align:center;"><img  id="avast" src="' + json.avatar + '"></div>')
				.prependTo(pageFn.menuBar);

			//pageFn.menuBar.css('top' , "+="+avastHeight) ;
		}
        console.log(JSON.parse(json.authorizeMenu));
		//side Menu
		pageFn.menuBar.bindList({
            list: JSON.parse(json.authorizeMenu),
			template: '<li><div class="menu-lv1"><img class="menu-ico" src="' + sys.menuIconPath + '{F_Ico}"/>{F_FullName}</div></li>',
			mode : 'append' ,
			onBound: function (arr) {
				var l = arr.length , i = 0 , $lists = this.find('li');
				for (; i < l; i++) {
					var data = arr[i].ChildNodes;
					if (data && data.length) {
						$('<ul class="ulMenu-lv2"></ul>').appendTo($lists[i]).bindList({
							list: data,
							template: '<li><div class="menu-lv2" data-widgettype="{F_Target}" data-menu="{F_Id}" data-link="{F_UrlAddress}">{F_FullName}</div></li>'
						});
					}
				}
			}
		});

		//top menu shortcuts
		/*if(json.shortCuts && json.shortCuts.length){
		 $("#topNavMenu").bindList({
		 list : json.shortCuts ,
		 mode: 'append' ,
		 template: '<li class="topNavMenuItem"><a class="topNavMenuLink" href="{link}" target="iframe2"><img src="' + window.sys.menuIconPath + '{ico}" class="icoTip"/>{text}</a></li>'
		 }) ;
		 }*/

	},
    init: function (json) {
		this.bindData(json);
		this.addEventListeners();
		this.splitter.click();
		//init function of search menu item by Url
		window.searchItemByUrl();

		//fetch runtime system message
		//fetchMsg.run();

		//web QQ
		for (var pln in sys.plugins) sys.plugins[pln].init();

		//start timer
		//fy.timerManager.run();

		// close the sideBar on page load
		this.splitter.trigger('click');
	}
};