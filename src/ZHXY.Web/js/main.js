$(window).load(function () {
    window.taskBar = $("#taskBar");
    top.clients = {
    };
    function getLimYear(data) {
        var output = {};
        var currentYear = new Date().getFullYear();
        var yearRange = [];
        for (i = 0; i < 5; i++) {
            yearRange.push(currentYear + i)
        }
        $.each(data, function (n, t) {
            $.each(yearRange, function (i, item) {
                if (Number(n) === item) {
                    output[n] = t
                }
            })
        });
        return output
    }
    function getOrgDic(data) {
        var output = {};
        $.each(data, function (i, item) {
            output[i] = {};
            $.each(item, function (_i, _item) {
                output[i][_i] = $.map(_item, function (n) {
                    return { id: n, text: n }
                })
            })
        });
        return output
    }
    var leftMenuBar = $("#navUl"),
        screenWrap = $("#screen"),
        scrArr = [];
    var extraJson = {
        "userId": 1,
        "userName": "admin",
        "realName": "嘿嘿",
        "date": 1530581237020,
        "schoolWeek": "2018年度第一学期 第20教学周",
        "wallPaper": "01.jpg",
        "widgetLayout": [["wh02"], [], []],
        "role": "教师",
        "roleId": "s",
        "schoolOpen": "2018-02-20",
        "schoolClose": "2018-06-29",
        "xqm": "2018学年度第一学期",
        "xq": 25,
        "zc": 20,
        "xxmc": "智慧校园管理系统",
        "logoImage": "logo.png",
        "dzxx": "",
        "unreadmail": "0",
        "uid": "admin",
        "token": "",
        "termAlias": "2018年度第一学期",
        "XqhId": "1",
        "BjDisplayName": "BH",
        "error": ""
    };
    fy.server.sysInfo.getJSON(function (json) {
        json.dataItems.limYear = getLimYear(json.dataItems.F_Year);
        fy.server.getWidget.getJSON(function (widget) {
            fy.server.getOrgDic.getJSON(function (org) {
                json.orgDic = getOrgDic(org);
                extraJson.realName = widget.UserName;
                extraJson.userName = widget.UserCode;
                extraJson.widgetLayout = JSON.parse(widget.F_User_SetUp);
                json = $.extend({},
                    json, extraJson);
                top.clients = json;
                if ((sys.homeShowPhoto || '1') == "1") {
                    avastHeight = 0;
                }
                var peopleRole = json.roleId || 't';
                $('#schLogo').attr({
                    src: sys.rootPath + sys.logoPath + json.logoImage,
                    title: json.xxmc
                });
                var navigatorMenus = JSON.parse(json.authorizeMenu);
                navigatorMenus = _.filter(navigatorMenus, n => n.F_BelongSys === '1')
                //widgets
                var wl = json.widgetLayout;
                if (wl) window.myWidgets = wl;
                else window.myWidgets = [[], [], []];
                if (window.widgets) loadWidgets(window.myWidgets);

                $('#liWPS').css('visibility', 'visible').find("a").click(selectWallpaper(json.wallPaper));

                leftMenuBar.bindList({
                    list: navigatorMenus,
                    template: '<li class="navItem" data-menu="{F_Id}"><img class="navItemIco" src="' + window.sys.menuIconPath + '{F_Ico}"><span class="navItemLabel">{F_FullName}</span></li>',
                    itemFilter: function (data, i) {
                        screenWrap.append('<ul class="screenUl"></ul>');
                        scrArr[i] = data.ChildNodes;
                        return data;
                    }
                });
                //bind level2 subMenu
                screenWrap.find(".screenUl").bindLists({
                    lists: scrArr,
                    template: '<li class="{ChildNodes:=navClass}" data-name="{F_FullName}" data-menu="{F_Id}" data-link="{F_UrlAddress:=gLink}"><img class="navItemIco" src="' + window.sys.menuIconPath + '085.png"><span class="navItemLabel">{F_FullName}</span></li>',
                    itemRender: {
                        gLink: function (t) {
                            x = t ? t.indexOf("?") : 1;
                            if (x > -1) {
                                t += '&uid=' + json.uid + '&token=' + json.token;
                            } else {
                                t += '?uid=' + json.uid + '&token=' + json.token;
                            }
                            return t;
                        },
                        navClass: function (t) {
                            if (t.length === 0) {
                                return 'navItem'
                            } else {
                                return 'navItem container'
                            }
                        }
                    },
                    onBound: function (list) {
                        this.find(".navItem").each(function (i, li) {
                            if (list[i].ChildNodes.length === 0) {
                                if (list[i].F_UrlAddress) {
                                    //list[i]['oUrl'] = list[i].link.replace(/[?].*/gi , '') ;
                                    var url = list[i].F_UrlAddress,
                                        x = url.indexOf("?");
                                    if (x > -1) {
                                        list[i]['oUrl'] = url.substr(0, x);
                                        list[i]['search'] = url.substr(x) + '&uid=' + json.uid + '&token=' + json.token;
                                    } else {
                                        list[i]['oUrl'] = url;
                                        list[i]['search'] = '?uid=' + json.uid + '&token=' + json.token;
                                    }
                                }
                            } else {
                                $(li).html('<img class="navItemIco" src="UpLoad/menu/023.png"><ul style="display:none;" class="navItemContainer third"></ul><ul style="display:none;" class="navItemContainer higher"></ul><input class="groupName">');
                                $(li).find('input').val(list[i].F_FullName)
                            }
                            $(li).data("win", list[i]);
                        });
                    },
                    onAllComplete: function () {
                        $('.screenUl .navItem.container').each(function (i, li) {
                            var data = $(li).data('win');
                            $.each(data.ChildNodes, function (i, item) {
                                if (item.ChildNodes.length) {
                                    $li = $('<li class="navItem inner folder" data-name=' + item.F_FullName + ' data-menu=' + item.F_Id + '><img class="navItemIco" src="/UpLoad/menu/023.png"><span class="navItemLabel">' + item.F_FullName + '</span></li>')
                                } else {
                                    $li = $('<li class="navItem inner" data-name=' + item.F_FullName + ' data-menu=' + item.F_Id + '><img class="navItemIco" src="' + (item.F_Ico ? window.sys.menuIconPath + '' + item.F_Ico : window.sys.menuIconPath + '085.png') + '"><span class="navItemLabel">' + item.F_FullName + '</span></li>')
                                }

                                $li.data('win', item);
                                $(li).find('.navItemContainer.third').append($li)
                            })
                        });
                        $.contextMenu({
                            selector: '#screen .navItem',
                            callback: function (key, misc) {
                                //log(key , misc) ;
                                var menuId = misc.$trigger.data('menu'),
                                    liId = 'sct-' + menuId;
                                if (!document.getElementById(liId)) {
                                    /**fy.server.shortcuts.getJSON({
                                                              menuId: menuId,
                                                              actionCode: 'saveShortcuts'
                                                          },
                                                          function () {
                                                              if (window.widgets) {
                                                                  if ('wh01' in window.widgets) {
                                                                      $('#shortcutUl').append(misc.$trigger.clone().attr('id', liId));
                                                                  }
                                                              }
                                                          });*/
                                    $('#shortcutUl').append(misc.$trigger.clone().attr('id', liId));
                                }
                            },
                            items: {
                                "shortcut": {
                                    name: "发送到快捷工具栏",
                                    icon: "edit"
                                }
                            }
                        });
                        window.welcomeScreen.stop(true, true).fadeOut(800,
                            function () {
                                $(this).hide().css({
                                    opacity: 1
                                });
                                var aside = $("#navPad"),
                                    asideCloser = $('#navPadCloser'),
                                    widgetPane = $('#widgets');
                                //student's avatar
                                if ((sys.homeShowPhoto || '1') == "1") {
                                    if (peopleRole == "t") {
                                        json.avatar = '../../upload/teacher_photo/' + json.userName + '.jpg';
                                    } else {
                                        json.avatar = '../../upload/student_photo/' + (json.avatar || json.userName) + '.jpg';
                                    }
                                }

                                function asideOut() {
                                    screenWrap.css('paddingLeft', 160);
                                    widgetPane.css('left', 160);
                                    asideCloser.hide().unbind('click.ipad');
                                    aside.animate({
                                        left: 0,
                                        duration: 300
                                    },
                                        function () {
                                            asideCloser.css({
                                                left: 120,
                                                backgroundPosition: "12px 10px"
                                            }).show().bind('click.ipad', asideIn);
                                        });
                                }

                                function asideIn() {
                                    screenWrap.css('paddingLeft', 40);
                                    widgetPane.css('left', 40);
                                    asideCloser.hide().unbind('click.ipad');
                                    aside.animate({
                                        left: -120,
                                        duration: 200
                                    },
                                        function () {
                                            asideCloser.css({
                                                left: 0,
                                                backgroundPosition: "12px -23px"
                                            }).show().bind('click.ipad', asideOut);
                                        });
                                }
                                asideCloser.hover(function () {
                                    asideCloser.css('opacity', 1);
                                },
                                    function () {
                                        asideCloser.css('opacity', 0.5);
                                    });
                                asideOut();
                                //init function of search menu item by Url
                                searchItemByUrl(screenWrap);
                                console.log(window); //make widgets
                                window.makeWidgets(); //set top bar informations
                                window.setTopBar(json); //fetch runtime system message
                                //web QQ
                                for (var pln in sys.plugins) {
                                    sys.plugins[pln].init();
                                }
                                //start timer
                                fy.timerManager.run();
                            });
                    }
                });
                /**
                 * 左侧滚动
                 */
                var mTop = ((sys.homeShowPhoto || '1') == "1" ? avastHeight + 162 : 162);
                //left menu scroll-bar
                var wrapper = $("#navBoard").css({
                    height: $(document).height() - mTop
                }),
                    wrapHeight = wrapper.innerHeight(),
                    lastTb = leftMenuBar.find("li:last"),
                    itemHeight = lastTb.outerHeight();
                //fix IE outer height
                if (fy.browser.msie) {
                    itemHeight += 4;
                }
                var tbHeight = itemHeight * (navigatorMenus.length + 1),
                    //为了使最下一个被css遮罩虚化的部分完全显示，+20px  itemHeight * navigatorMenus.length + 20
                    sp = tbHeight - wrapHeight;
                var wrapResizeTimer = 0;
                $(window).resize(function () {
                    if (!wrapResizeTimer) {
                        wrapResizeTimer = setTimeout(function () {
                            wrapper.css({
                                height: $(document).height() - mTop
                            });
                            wrapHeight = wrapper.innerHeight();
                            sp = tbHeight - wrapHeight;
                            wrapResizeTimer = 0;
                        },
                            0);
                    }
                });
                if (sp >= 0) {
                    var bUl = $("#navUl"),
                        mouseTimer = 0;
                    //Arrow - up
                    var arrowUp = $("#navPadUp");
                    if (fy.browser.touchable) {
                        arrowUp.click(scrollUp);
                    } else {
                        arrowUp.mousedown(function () {
                            mouseTimer = setTimeout(scrollUp, 20);
                        });
                    }
                    //Arrow - down
                    var arrowDown = $("#navPadDown").css("visibility", 'visible');
                    if (fy.browser.touchable) {
                        arrowDown.click(scrollDown);
                    } else {
                        arrowDown.mousedown(function () {
                            mouseTimer = setTimeout(scrollDown, 20);
                        });
                    }
                    if (fy.browser.touchable) {
                        $("#navPadUp,#navPadDown").show();
                    } else {
                        $("#navPadUp,#navPadDown").show().mouseup(function () {
                            clearTimeout(mouseTimer);
                            mouseTimer = 0;
                        });
                        //listener of mouse wheel event
                        wrapper.mousewheel(function (event, delta, deltaX, deltaY) {
                            //log(delta, deltaX, deltaY);
                            clearTimeout(mouseTimer);
                            mouseTimer = 0;
                            delta < 0 ? scrollDown() : scrollUp();
                        });
                    }

                    function scrollUp() {
                        clearTimeout(mouseTimer);
                        var umt = parseInt(bUl.css("marginTop"));
                        if (umt >= 0) {
                            return;
                        }
                        var mt = umt + itemHeight;
                        if (mt >= 0) {
                            mt = 0;
                            arrowUp.css("visibility", 'hidden');
                            arrowDown.css("visibility", 'visible');
                        } else {
                            arrowUp.css("visibility", 'visible');
                            arrowDown.css("visibility", 'visible');
                        }
                        bUl.stop(true, true).animate({
                            "marginTop": mt
                        },
                            100,
                            function () {
                                if (mouseTimer !== 0) {
                                    setTimeout(scrollUp, 20);
                                }
                            });
                    }

                    function scrollDown() {
                        clearTimeout(mouseTimer);
                        var umt = parseInt(bUl.css("marginTop"));
                        //log(tbHeight , wrapHeight , umt);
                        if (tbHeight + umt <= wrapHeight) {
                            return;
                        }
                        var mt = umt - itemHeight;
                        if (tbHeight + umt - wrapHeight < itemHeight) {
                            mt = -(tbHeight - wrapHeight);
                            arrowUp.css("visibility", 'visible');
                            arrowDown.css("visibility", 'hidden');
                        } else {
                            arrowUp.css("visibility", 'visible');
                            arrowDown.css("visibility", 'visible');
                        }
                        bUl.stop(true, true).animate({
                            "marginTop": mt
                        },
                            100,
                            function () {
                                if (mouseTimer !== 0) {
                                    setTimeout(scrollDown, 20);
                                }
                            });
                    }
                }
                //menu icons click handler , switch screen icons
                var navItems = $(".navItem", "#navUl"),
                    screens = screenWrap.find(".screenUl"),
                    widgetsDiv = $("#widgets");
                window.showDesktop = function () {
                    var current = screens.filter(".currentScreen");
                    if (current.length) {
                        current.removeClass("currentScreen").stop(true, true).fadeOut(200);
                        widgetsDiv.css("display", "block");
                    }
                    taskBar.find(".taskItem").each(function () {
                        var taskItem = $(this),
                            boxy = taskItem.data("window");
                        if (boxy.visible) {
                            boxy["minimum"]();
                        }
                    });
                };
                leftMenuBar.delegate(".navItem", "click.ipad",
                    function (evt) {
                        var i = navItems.index(this),
                            current = screens.filter(".currentScreen");
                        //show widgets
                        if (screens.index(current) === i) {
                            $(this).removeClass('navItem_current');
                            current.removeClass("currentScreen").stop(true, true).fadeOut(200);
                            widgetsDiv.css("display", "block");
                        }
                        //show icons
                        else {
                            $(this).addClass('navItem_current').siblings('.navItem_current').removeClass('navItem_current');
                            widgetsDiv.css("display", "none");
                            if (current.length) {
                                if ($.browser.msie) {
                                    current.removeClass("currentScreen").hide(0,
                                        function () {
                                            screens.eq(i).addClass("currentScreen").show(0);
                                        });
                                } else {
                                    current.removeClass("currentScreen").stop(true, true).fadeOut(200,
                                        function () {
                                            screens.eq(i).stop(true, true).addClass("currentScreen").fadeIn(500);
                                        });
                                }
                            } else {
                                if ($.browser.msie) {
                                    screens.eq(i).addClass("currentScreen").show(0);
                                } else {
                                    screens.eq(i).addClass("currentScreen").fadeIn(500);
                                }
                            }
                            //alert(moduleGroup);
                            var data = moduleGroup.getData();
                            moduleGroup.addDragEvent(i, scrArr);
                            pid = $(this).attr("data-menu");
                        }
                    });
                //the second level icon click
                screenWrap.delegate(".navItem:not(.folder)", "click.ipad",
                    function (evt) {
                        evt.stopPropagation();
                        var src = $(this);
                        console.log(src);
                        //(new Function("", "return " + src.data("win")))();
                        var win = fy.popupManager.get(src.data("iframe"));
                        if (win) {
                            win.toTop().show();
                        } else {
                            var cfg = src.data("win");
                            //log(cfg);
                            var group = src[0].className;

                            if (cfg && cfg.F_UrlAddress && !src.hasClass('containerFixed') && !src.hasClass('container')) {
                                console.log(cfg);
                                if (cfg.F_Target == "iframe" || cfg.F_Target == "") {
                                    top.$.cookie('nfine_currentmoduleid', cfg.F_Id, { path: '/' });
                                    window.createWindow(src, cfg);
                                } else if (cfg.F_Target == "js") {
                                    window.open(cfg.F_UrlAddress + cfg.search);
                                }
                            } else if (group === "navItem container" || (group === "navItem containerFixed")) {
                            } else {
                                sys.alert("该功能菜单尚未设定关联程序，请联系系统管理员。");
                            }
                        }
                    });
                //proxy of the task bar click event
                taskBar.delegate(".taskItem", "click.ipad",
                    function (evt) {
                        var taskItem = $(this),
                            boxy = taskItem.data("window");
                        top.$.cookie('nfine_currentmoduleid', taskItem.data('id'), { path: '/' });
                        taskItem.addClass("taskItemCur").siblings(".taskItemCur").removeClass("taskItemCur");
                        boxy[boxy.visible ? "minimum" : "show"]().toTop();
                    });
            });
        })
    })
});