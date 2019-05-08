//init global parameters
sys.debug = (fy.server.isLocal === true); //是否打开调试模式，正式部署时亦可明确设为false
sys.rootPath = "/";//网站虚拟路径根，务必确认必须以斜杠开头和结尾
//sys.offline = false; //是否运行在无数据库模式下
sys.logoPath = "UpLoad/logo/";  //logo文件路径，必须以斜杠结尾
sys.menuIconPath = "UpLoad/menu/";  //菜单图标路径，必须以斜杠结尾
sys.wallpaperPath = "UpLoad/wallpaper/";//桌面背景存放位置
sys.album = "uploadfiles/album/"; //相册背景
sys.wallpaper = "01.jpg"; //桌面默认背景
sys.maxWins = 5;//现代风格下，最大开启的窗口数
sys.inSiteMessageRefresh = 5000; //站内消息的刷新间隔(毫秒), 不宜小于10000 ，即10秒
sys.setLog = true ; //是否记录打开模块的操作日志
sys.viewemail = 0; //是否显示未读邮件
sys.emailRefresh = 10000; //未读邮件刷新间隔
//----> 以下不用修改
sys.sysInfo = null ; //不用设，将通过系统自动取得
sys.loginPage = fy.cookie("loginPage"); //登录页面来源
//sys.encryptData = false ;  //是否加密数据
//sys.encryptKey = null ; //不用设，将通过系统自动取得
sys.timeZone = (new Date).getTimezoneOffset();  //时区偏移,自动值,中国是+8,不宜手工修改
sys.showErrCode = true;//是否展示错误信息的方法名
sys.showJumpImg = true;//是否展示首页右上角图片

sys.defaultWinMax = true; //窗口是否默认最大化

sys.electiveCourseScore = "0";  // 选修课成绩输入是否显示输成绩分数 0:显示 1:不显示

sys.homeShowPhoto = "1"; //首页是否显示照片 1:显示，0:不显示

sys.teacherPhoto = "0"; //保存教师信息时是否必须上传照片 0:否 1:是
sys.studentPhoto = "0"; //保存学生信息时是否必须上传照片 0:否 1:是
sys.studentphotoisedit = "0"; //控制学生是否可以上传照片 0:否 1：是
sys.issueWebsite = false; //信息发布发布到网站按钮是否显示
sys.isSyncExam = false;//市三女中考试列表同步阅卷系统考试数据按钮是否显示
sys.isSyncStudent = false;//市三女中考试科目设置是否显示同步学生成绩按钮
sys.isSyncCourse = false;//市三女中课程管理是否显示同步学科按钮

sys.isShowFillDetail = false; //问卷调查是否显示填写详情
//系统功能初始化
sys.init();


// disable effects under IE8
// if($.browser.msie && $.browser.version < 9) jQuery.fx.off = true ;

//jQuery ajax global setting
$.ajaxSetup({
	beforeSend: function (jqXHR, settings) {
		if (!sys.ajaxHandlers) {
			if (self != top) {
				if (sys.ajaxLoadingTimer) clearTimeout(sys.ajaxLoadingTimer);
				sys.ajaxLoadingTimer = 0;
				sys.ajaxLoadingShow();
			}
		}
		sys.ajaxHandlers++;
		/*
		 if(sys.encryptData){
		 log(jqXHR, settings);
		 }
		 */
	},
	complete: function (jqXHR, textStatus) {
		sys.ajaxHandlers--;
		if (!sys.ajaxHandlers) {
			if (self != top) sys.ajaxLoadingTimer = setTimeout(sys.ajaxLoadingHide, 50);
		}
	},
	cache: false,
	timeout: 30000,
	dataType: 'json',
	error: function (jqXHR, textStatus, errorThrown) {
		console.log(sys.loginPage);
		var code = jqXHR.status;
		if (sys.debug) {
			if (code === 0 || code === 200) return;
			if (code === 401 || code === 403) {
				sys.confirm("权限不足，您需要重新登录吗？", function () {
					//top.window.location.replace(sys.loginPage);
				});
				//if(top.welcomeScreen) top.welcomeScreen.hide();
				/*setTimeout(function () {
					//top.window.location.replace(sys.loginPage);
				}, 5000);*/
			}
			else sys.error("系统错误：" + code + "，<br/>" + errorThrown);
		}
		else {
			//直接踢出系统
			if (code !== 0 && code !== 200) {
				//top.window.location.replace(sys.loginPage);
			}
		}
	}
});



