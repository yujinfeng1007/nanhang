(function($t){

	//top.fy.server('shortcuts' , 'saveShortcuts.foyo?actionParam=1&multiParam=1');
	//fy.server('shortcuts' , 'saveShortcuts.foyo?actionParam=1&multiParam=1');

	$t.empty().css({
		backgroundColor : 'transprent'
	});

	var pane = $('<ul id="shortcutUl" class="screenUl" style=" white-space: normal ; display: block ; margin: 0;min-height: 138px;"></ul>').appendTo($t) ;

	/*var schBar = $('<input type="text" style="background-color: transparent;border-radius: 50px;background-color: rgba(0,0,0,0.1);border-color: rgba(0,0,0,0.3)">').appendTo($t) ;

	schBar.keypress(function(){
		log('dad' , searchSubMenuByName()) ;
	}) ;*/

	var widgetPanel = pane.parents('.widget') ,
		titleBar = $t.siblings('.widgetTitle') ;

	titleBar.css({position : 'absolute' ,zIndex : 10}) ;

	widgetPanel.mouseover(function(){
		titleBar.width($t.width());
	});



	window.refreshMenuShortcut = function(){
		//有可能widget开启后被关闭
		if(window.document.getElementById('shortcutUl')){
			fy.server.shortcuts.getJSON({actionCode : 'getShortcuts'} , function(json){
//log(sys.rootPath + sys.menuIconPath);
				pane.bindList({
					list : json.data ,
					template : '<li id="sct-{SysMenuId}" class="navItem" data-name="{text}" data-menu="{SysMenuId}" data-link="{link}"><img class="navItemIco" src="' + sys.menuIconPath+'{ico}"><span class="navItemLabel">{text}</span></li>' ,
					onBound : function(){
						$.contextMenu({
							selector: '#shortcutUl>.navItem',
							callback: function(key, misc) {
								var menuId = misc.$trigger.data('menu');
								fy.server.shortcuts.getJSON({menuId : menuId , actionCode : 'deleteShortcuts'} , function(){
									misc.$trigger.remove() ;
								});
							},
							items: {
								"shortcut": {name: "从快捷工具栏移除", icon: "delete"}
							}
						});
					}
				}).delegate('li' , 'click' , function(){
						top.sys.openModule( $(this).data("link") ) ;
					});
			});
		}
	};

	refreshMenuShortcut();
});