var Public = Public || {}
var Business = Business || {}
Public.isIE6 = !window.XMLHttpRequest //ie6
var pageConfigInfo = localStorage.getItem('tableConfig') || {}
if (!localStorage.getItem('tableConfig')) {
  localStorage.setItem('tableConfig', JSON.stringify({
  }))
  pageConfigInfo = {}
} else {
  pageConfigInfo = JSON.parse(localStorage.getItem('tableConfig'))
}


Public.getDefaultPage = function () {
  var win = window.self
  do {
    if (win.CONFIG) {
      return win
    }
    win = win.parent
  } while (true)
}

/**
    * 页面配置模块
    * @param  {string}  id               页面ID ，标识
    * @param {object} pageConfig        页面配置 {id: '初始化DOM id', config: '页面配置定义'}
    * @return {[type]}                   [description]
    */
Public.mod_PageConfig = (function (mod) {
    var inited = false, //模块启动开关
      conf //当前页配置
    mod.register = function (id, pageConfig) {
      inited = true
      mod.pageId = id
      mod.pageConfig = pageConfig
      mod.location = location
      mod.gridConfig = pageConfig.config.grid
      mod.btnConfig = pageConfig.config.gridButton
      mod.gridId = id + 'Grid'
      mod.pagerId = id + 'Pager'
      pageConfigInfo = pageConfigInfo || {}
      pageConfigInfo['' + id] = pageConfigInfo['' + id] || {}
      conf = pageConfigInfo['' + id]
       //页面事件注册
      mod.gridReg = _gridReg
      mod.genGridBtn = _genGridBtn
      mod.genPageHtml = _genPageHtml
      mod.conf = conf
      mod.config = _config
      mod.updatePageConfig = _updatePageConfig
      mod.setGridWidthByIndex = _setGridWidthByIndex
      mod.getSearchParam = _getSearchParam
      mod.getSelectRow = _getSelectRow
      mod.baseGridConfig = {
        datatype: "json",
        rownumbers: true,
        cellEdit: !1,
        altRows: !0,
        gridview: !0,
        viewrecords: !0,
        rowNum: 10,
        rowList: [10, 20, 50, 100, 200, 500],
        shrinkToFit: !1,
        autowidth: !0,
        onselectrow: !1,
        multiselect: !0,
        styleUI: 'Bootstrap',
        resizeStop: function (r, i) {
          mod.setGridWidthByIndex(r, i, mod.gridId)
        },
      }
      return mod;
    };
    mod.init = function () {
      var config = mod.pageConfig
      var gridConfig = mod.gridConfig
      var btnConfig = mod.btnConfig
      mod.genPageHtml(config)
      mod.genGridBtn(btnConfig)
      mod.gridReg(mod.gridId, gridConfig.colModel)
      _eventReg()
      _setAuth()
    }
    function _config() {
      var content = [
        '<div class="content">',
        '<ul style="list-style:none;padding-left: 0" class="c_wrapper" id="c_wrapper">',
        '</ul>',
        '</div>'
      ];
      $.modalOpen({
        title: '页面配置',
        type: 1,
        width: '500px',
        btn: ['确认', '恢复默认设置'],
        height: '300px',
        url: content.join(''),
        lock: true,
        success: function (layer) {
          var $_tab = $('<ul class="ui-tab" id="c_tab" style="border-bottom: 1px solid #EBEBEB;  position: relative;  top: -10px;  left: -10px;  margin-right: -20px;"></ul>')
          var $_wrapper = layer.find('#c_wrapper')
          for (var gridId in conf.grids) { 
            var conf_grid = conf.grids[gridId] || {}
            if (typeof conf.grids[gridId] !== 'function' && conf_grid.isReg) { 
              var $grid = $('#' + mod.gridId)
              var configGridId = 'c_' + mod.gridId
              var _caption = conf_grid.caption ? '（' + conf_grid.caption + '）' : ''
              _caption != "" && $_tab.append('<li style="border-top: none;  border-bottom: none;  border-color: #EBEBEB;">表格' + _caption + '</li>')
              $_wrapper.append([
                '<li class="grid-wrap dn">',
                '<table class="table-bordered config-table" id="' + configGridId + '">',
                '</table>',
                '</li>'
              ].join(''))
              var dataArr = conf_grid.colModel //缓存里的配置信息
              var _dataArr = []
              for (var i = 0; i < dataArr.length; i++) {
                var col = dataArr[i];
                if (!col) continue;
                //取消排除强制隐藏字段
                //if ($.trim(col['label']) && !col.defhidden && !col.disConfigured) {
                if ($.trim(col['label']) && !col.disConfigured) {
                    _dataArr.push(col)
                }
              }
              var $configGrid = layer.find('#' + configGridId)
              $configGrid.jqGrid({
                data: _dataArr,
                  datatype: "clientSide",
                  width: 480,
                  height: 290,
                  rownumbers: true,
                  gridview: true,
                  onselectrow: false,
                  idPrefix: 'c_gridId_', //表格id前缀
                  styleUI: 'Bootstrap',
                  colModel: [
                    { name: 'name', label: '列名', hidden: true },
                    { name: 'defLabel', label: '列名称', width: 100 },
                    { name: 'label', label: '列名称', width: 100, hidden: true },
                    {
                        name: 'aname', label: '别名', width: 100, editable: true, formatter: function (val, opts, row) {
                            if (!val) {
                                val = row['label'];
                            }
                            return val;
                        }
                    },
                    {
                        name: 'hidden', label: '是否显示', width: 50, align: 'center', formatter: function (val, opts, row) {
                            var text = val == true ? '已隐藏' : '已显示';
                            var cls = val == true ? 'ui-label-default' : 'ui-label-success';
                            if (row.defhidden || row.frozen) return '<span class="ui-label ui-label-default">不可控</span>';
                            return '<span class="set-status ui-label ' + cls + '" data-delete="' + val + '" data-id="' + row.id + '">' + text + '</span>';
                        }
                    }
                    ,
                    {
                        name: 'hidden', label: '上下移动', width: 50, align: 'center', formatter: function (val, opts, row) {
                            //var text = opts.rowId == 1 ? '' : '上移';
                            //var cls = opts.rowId == 1 ? '' : 'ui-label-success';
                            var text = "上", text2 = "下";
                            var cls = "ui-label-success";
                            return '<span class="set-sort ui-label ' + cls + '" >' + text + '</span> <span class="set-sort-down ui-label ' + cls + '" >' + text2 + '</span>';
                        }
                    }
                 ],
                 shrinkToFit: true,
                 forceFit: true,
                 cellEdit: true,
                 rowNum: 100,
                 cmTemplate: {
                   sortable: !1
                 },
                 cellsubmit: 'clientArray',
                 afterSaveCell: function (rowid, name, val, iRow, iCol) {
                    switch (name) {
                      case 'aname':
                        if (!$.trim(val)) {
                          $.modalMsg('请输入别名', 'warning')
                          return;
                        } else {
                          var rowData = $configGrid.jqGrid('getRowData', rowid)
                          $grid.jqGrid('setColProp', rowData['name'], { label: rowData.aname })
                          $configGrid.jqGrid('setRowData', rowid, { label: rowData.aname })
                          var $th = $('#jqgh_' + $grid[0].id + '_' + rowData['name'])
                          $th.html($th.html().replace(rowData['label'], rowData.aname))
                          _updatePageConfig($grid[0].id, ['label', rowData.name, rowData['aname']])
                          return val
                        }
                      break
                    }
                }
              })
              function bindEvent ($grid, $configGrid) {
                $configGrid.on('click', '.set-status', function(event) {
                  event.preventDefault()
                  var $this = $(this)
                    var id = $this.closest('tr')[0].id
                    var rowData = $configGrid.jqGrid('getRowData', id)
                    if ($this.hasClass('ui-label-success')) {
                      _updatePageConfig($grid[0].id, ['hidden', rowData.name, true])
                      $configGrid.jqGrid('setCell', id, 'hidden', true)
                      $grid.jqGrid('hideCol', rowData['name'])
                    } else {
                      _updatePageConfig($grid[0].id, ['hidden', rowData.name, false])
                      $configGrid.jqGrid('setCell', id, 'hidden', false)
                      $grid.jqGrid('showCol', rowData['name'])
                    }
                })
                $configGrid.on('click', '.set-sort', function (event) {
                  event.preventDefault()
                  var $this = $(this)
                  var id = $this.parent().parent().attr("id")
                  var index = parseInt(id.replace("c_gridId_", "")) - 1
                  if (index > 0) {
                    _sortPageConfig($grid[0].id, index)
                    $this.parent().parent().attr("id", "c_gridId_" + index)
                    $this.parent().parent().prev("tr").attr("id", "c_gridId_" + (index + 1))
                    $this.parent().parent().prev("tr").before($this.parent().parent())
                  } else {
                    $.modalMsg('已经是第一行', 'warning')
                  }
                })
                $configGrid.on('click', '.set-sort-down', function (event) {
                  event.preventDefault()
                  var $this = $(this)
                  var id = $this.parent().parent().attr("id")
                  var index = parseInt(id.replace("c_gridId_", "")) + 1
                  if ($this.parent().parent().next("tr").attr("id")) {
                    _sortPageConfigDown($grid[0].id, index)
                    $this.parent().parent().attr("id", "c_gridId_" + index)
                    $this.parent().parent().next("tr").attr("id", "c_gridId_" + (index - 1))
                    $this.parent().parent().next("tr").after($this.parent().parent())
                  } else {
                    $.modalMsg('已经是最后一行', 'warning')
                  }
                })
              }
              bindEvent($grid, $configGrid)
            }
          }
        },
        yes: function (index, layero) {
            conf.modifyTime = conf.curTime = new Date()
            _updateConfig(_formatPostData())
            _cancelGridEdit(layero)
            $(window).unbind('unload')
            mod.location.reload()
        },
        btn2: function (index, layero) {
          $.modalConfirm('该操作会刷新当前页签，是否继续?', function (result) {
            if (result) {
              _cancelGridEdit(layero)
              pageConfigInfo['' + mod.pageId] = null
              $(window).unbind('unload')
              mod.location.reload()
            }
          })
        }
      })
    }
    function _gridReg(gridId, defColModel, caption) {
      if (!defColModel) {
        return
      }
      conf.grids = conf.grids || {}
      conf.grids[gridId] = conf.grids[gridId] || {}
      var g = conf.grids[gridId]
      var s = g.colModel
      
      g.caption = caption
      g.defColModel = defColModel
      g.colModel = g.colModel || defColModel
      for (var i = 0; i < defColModel.length; i++) {
        if (!defColModel[i]) continue
        defColModel[i].defLabel = defColModel[i]['label']
        defColModel[i].defhidden = undefined == defColModel[i].defhidden ? defColModel[i].hidden : defColModel[i].defhidden
        c1 = defColModel[i]
        if (g.colModel) {
            
            for (var j = 0; j < g.colModel.length; j++) {
              var c = g.colModel[j]
              var c2 = {
                name: c[0] || c['name']
                , label: c[1] || c['label']
                , hidden: c.defhidden ? c1.hidden : (c[2] || c['hidden']) //c有defhidden 就是从缓存取的，因为数据库没有存def配置，处理因为缓存导致刷新子页面而把新放开/权限的字段给默认隐藏了
                , width: c[3] || c['width']
              }
              if (c1['name'] === c2['name']) {
                _modelClone(c1, c2)
              }
            }
        }
      }
      var defColModelSort = {}
      $.extend(true, defColModelSort, defColModel)
      if (s) {
        if (defColModel.length == s.length) {
          for (var i = 0; i < defColModel.length; i++) {
            for (var j = 0; j < defColModel.length; j++) {
              var c3 = {
                name: s[i][0] || s[i]['name']
              }
              if (c3['name'] == defColModelSort[j]['name']) {
                defColModel[i] = defColModelSort[j]
              }
            }
          }
        }
      }
      g.colModel = defColModel //用户列配置扩展
      g.isReg = true
      var gridConfig = $.extend({}, mod.baseGridConfig, mod.gridConfig, { colModel:  conf.grids[mod.gridId].colModel, pager: '#' + mod.pagerId})
      gridConfig.onSelectRow = _rowSelEvent
      gridConfig.onSelectAll = _rowSelEvent
      gridConfig.loadComplete = _setAuth
      $('#' + mod.gridId).jqGrid(gridConfig).jqGrid('setFrozenColumns')
      $(window).resize(function () {
        Public.resizeGrid(120, 0, $('#' + mod.gridId))
      })
      $(window).resize()
    }
    function _rowSelEvent () {
      var data = mod.getSelectRow()
      var $searchEl = $('#' + mod.pageConfig.id).find('.gridBtn')
      if (data.length !== 1) {
        $searchEl.find('a[multiaction=false]').addClass('disabled').find('a[multiaction=true]').removeClass('disabled')
      } else {
        $searchEl.find('a[multiaction=false]').removeClass('disabled').find('a[multiaction=true]').addClass('disabled')
      }
    }
    function _getSelectRow() {
      var output = []
      var selRow = $('#' + mod.gridId).jqGrid('getGridParam', 'selarrrow')
      $.each(selRow, function (i, item) {
        output.push($('#' + mod.gridId).jqGrid('getRowData', item))
      })
      return output
    }
    function _sortPageConfig(gridId, index) {
        if (!conf.grids || !conf.grids[gridId] || !conf.grids[gridId].isReg || !index > 0) {
            return;
        }
        var g = conf.grids[gridId];
        var c1 = g.colModel[index];
        var c2 = g.colModel[index - 1];
        g.colModel[index] = c2;
        g.colModel[index - 1] = c1;
        conf.curTime = Date.parse(new Date())
    };
    function _sortPageConfigDown(gridId, index) {
        if (!conf.grids || !conf.grids[gridId] || !conf.grids[gridId].isReg) {
            return
        }
        var g = conf.grids[gridId]
        var c1 = g.colModel[index - 1]
        var c2 = g.colModel[index - 2]
        g.colModel[index - 1] = c2
        g.colModel[index - 2] = c1
        conf.curTime = Date.parse(new Date())
    }
    function _cancelGridEdit($content) {
      for (var gridId in conf.grids) {
        var conf_grid = conf.grids[mod.gridId] || {}
        if (typeof conf.grids[mod.gridId] !== 'function' && conf_grid.isReg) {
          var $grid = $('#' + mod.gridId)
          var configGridId = 'c_' + mod.gridId
          
          var $confGrid = $content.find('#' + configGridId)
          if ($confGrid[0].p.savedRow.length != 0) {
            $confGrid.jqGrid("saveCell", $confGrid[0].p.savedRow[0].id, $confGrid[0].p.savedRow[0].ic)
          }
        }
      }
    }
    function _modelClone(c1, c2, propName) {
      if (propName) {
        c1[propName] = c2[propName];
      } else {
        $.extend(true, c1, {//开放修改的列属性
          label: c2['label']//列名
          ,hidden: c2['hidden']//显示与隐藏
          ,width: c2['width']//宽度
        });
        $.extend(true, c1, c2);
      }
      return c1;
    }
    function _genPageHtml(config) {
      var $content = $('<div class="gridTop"><div id="search-'+ mod.pageId +'" class="search"></div><div class="gridBtn"></div></div>' +
        '<div class="gridPanel">' +
          '<table id='+ mod.gridId +' class="gridList"></table>' +
          '<div id='+ mod.pagerId +' class="gridPager"></div>' +
        '</div>'
    )
      $('#' + mod.pageConfig.id).html($content)
      var searchHtml = _genSearchHtml(config.config.searchParams)
      $('#' + mod.pageConfig.id).find('.search').html(searchHtml)
    }
    function _gridBtnClick ($target, callback) {
      
    }
    function _genGridBtn (config) {
      var $content = $('<div class="btn-group"><a class="btn btn-primary" id="btn-config-'+ mod.pageId +'" href="javascript:">页面配置</a></div>')
      var $buttonGroup = $('<div class="btn-groups"></div>')
      $('#' + mod.pageConfig.id).find('.gridBtn').html($content)
	  if (config && config.data) {
        $.each(config.data, function (i, item) {
          var $btn
          if (i + 1 > config.lim ) {
            $btn = $('<li><a class="operate-btn '+ (item.class || '') +'" multiaction='+ item.multiAction +' authorize="yes" href="javascript:">'+ item.name +'</a></li>')
            if ($buttonGroup.find('.btn-group.more-btn').length) {
              $buttonGroup.find('.btn-group.more-btn .dropdown-menu').append($btn)
            } else {
              $buttonGroup.append('<div class="btn-group more-btn"></div>')
              $buttonGroup.find('.btn-group.more-btn').append('<a class="btn btn-primary dropdown-toggle" data-toggle="dropdown" href="javascript:">更多操作<span class="caret"></span></a><ul class="dropdown-menu"></ul>')
              $buttonGroup.find('.btn-group.more-btn .dropdown-menu').append($btn)
            }
            $btn.click(function (e) {
              var selRow = mod.getSelectRow()
              item.callBack(e, selRow)
            })
          } else {
            $btn = $('<div class="btn-group"><a multiaction='+ item.multiAction +' class="btn btn-primary '+ (item.class || '') +' operate-btn" href="javascript:">'+ item.name +'</a></div>')
            $buttonGroup.append($btn)
            $btn.find('a').click(function (e) {
              var selRow = mod.getSelectRow()
              item.callBack(e, selRow)
            })
          }
        })
	    $('#' + mod.pageConfig.id).find('.gridBtn').append($buttonGroup)
	  }
        $('#' + mod.pageConfig.id).find('.gridBtn').append($content)
      
    }
    function _genSearchHtml (config) {
      if (!config) {
        return
      }
      var $content = $('<ul><li class="formTitle">搜索：</li></ul>')
      $.each(config, function (i, item) {
        switch (item.type)
        {
          case 'input':
            $content.append('<li style="margin-left:9px">' +
              '<div class="input-group" style="margin-top:1px;width: '+ (item.width || 150) +'px">' +
                '<input id="'+ i +'" type="text" name='+ i +' class="form-control" placeholder="请输入'+ item.name +'" style="width: '+ (item.width || 150) +'px;">' +
              '</div>' +
            '</li>')
            break;
          case 'select':
            $content.append('<li style="margin-left:9px">' +
              '<div class="input-group" style="margin-top:1px;width: '+ (item.width || 200) +'px">' +
                '<select style="width:'+ (item.width || 200) +'px;" name='+ i +' id='+ i +' class="form-control" ></select>' +
              '</div>' +
            '</li>')
            $content.find('#' + i).bindSelect($.extend({}, item.config, {displayBlank: true}))
            break;
          case 'datetime': 
          $content.append('<li style="margin-left:9px">' +
              '<div class="input-group" style="margin-top:1px;width: '+ (item.width || 150) +'px">' +
              '<input id="'+ i +'" onfocus="WdatePicker()" placeholder="请选择'+ item.name +'" type="text" class="form-control input-wdatepicker" name='+ i +'  style="width: '+ (item.width || 150) +'px;">' +
              '</div>' +
            '</li>')
            break;
        }
      })
      var $lastEl = $content.find('li:last-child .input-group')
      $lastEl.append('<span class="input-group-btn">' +
        '<button id="btn-search-'+ mod.pageId +'" type="button" class="btn  btn-primary"><i class="fa fa-search"></i></button>' +
      '</span>').css('width', $lastEl.width() + 30)
      return $content
    }
    function _eventReg() {
        //列配置
        $('#btn-config-'+ mod.pageId).click(function () {
          mod.config()
        })
        $('#btn-search-' + mod.pageId).click(function () {
          var param = mod.getSearchParam()
          $('#' + mod.gridId).jqGrid('setGridParam', {
            postData: param,
          }).trigger('reloadGrid')
        })
        $(window).on('unload', function () {
          if (conf && conf.curTime && conf.modifyTime !== conf.curTime) {
            conf.modifyTime = conf.curTime
            _updateConfig(_formatPostData())
          }
        })
    }
    function _setAuth() {
      var moduleId = top.$(".NFine_iframe:visible").attr("id").substr(6)
      var dataJson = top.clients.authorizeButton[moduleId]
      $('#' + mod.pageConfig.id).find('.operate-btn').attr('authorize', 'no')
      if (dataJson != undefined) {
        $.each(dataJson, function (i, item) {
          if (item.F_EnabledMark) {
            $('#' + mod.pageConfig.id).find("." + item.F_EnCode).attr('authorize', 'yes')
          } else {
            $('#' + mod.pageConfig.id).find("." + item.F_EnCode).attr('authorize', 'no')
          }
        })
      }
      $('#' + mod.pageConfig.id).find('[authorize=no]').remove()
      $('#' + mod.gridId).jqGrid('resizeGrid')
    }
    function _getSearchParam() {
      var param = {}
      $('#' + mod.pageConfig.id).find('.search input,.search select').each(function () {
        var id = $(this).attr('id')
        param[id] = $(this).val()
      })
      return param
    }
    function _updateConfig(value) {
      var storageConfig = JSON.parse(localStorage.getItem('tableConfig'))
      storageConfig[mod.pageId] = {}
      storageConfig[mod.pageId] = value
      localStorage.setItem('tableConfig', JSON.stringify(storageConfig))
    }
    function _formatPostData() {
        //表格的列配置转成数组类型减少数据量
        var _conf = $.extend(true, {}, conf)
        //克隆conf
        for (var gridId in _conf.grids) {
          var g = _conf.grids[gridId]
          if (typeof g != 'function' && g.isReg) {
            var colModel = g.colModel
            var tmpArr = []
            var col2 = []
            var isdbl = false
            for (var i = 0; i < colModel.length; i++) {
              isdbl = false
              var col = colModel[i]
              if (!col)
                continue;
              for (var c = 0; c < col2.length; c++) {
                if (col2[c]['name'] == col['name'])
                  isdbl = true
              }
              if (isdbl)
                continue;
              col2.push(colModel[i])
                tmpArr.push([
                  col['name']//列名,唯一标识
                  , col['label']//列名
                  , col.defhidden ? null : col['hidden']//显示与隐藏//默认是隐藏的就不受列配置的显影控制
                  , col['width']
                ])
            }
              g.colModel = tmpArr
          }
        }
        return _conf
    }
    function _updatePageConfig(gridId, prop) {
        //为了区分用户修改，必须精确到每个字段的每个属性 prop = [propName, colName, propValue];
      if (!conf.grids || !conf.grids[gridId] || !conf.grids[gridId].isReg || !prop || prop.length != 3) {
        return
      }
      prop = {
        propName: prop[0],
        colName: prop[1],
        propValue: prop[2]
      }
      var g = conf.grids[gridId]
      for (var i = 0; i < g.colModel.length; i++) {
        var c1 = g.colModel[i]
          if (c1.name == prop.colName) {
            c1[prop.propName] = prop.propValue
          }
      }
      conf.curTime = Date.parse(new Date())
    }
    function _setGridWidthByIndex(newwidth, index, gridId) {
      _updatePageConfig(gridId, ['width', conf.grids[gridId].defColModel[index - 1]['name'], newwidth])
    }
    return mod
})(Public.mod_PageConfig || {})

//设置表格宽高
Public.setGrid = function (adjustH, adjustW) {
    var adjustW = adjustW || 20;
    var gridW = $(window).width() - adjustW, gridH = $(window).height() - $(".gridPanel").offset().top - adjustH;
    return {
        w: gridW,
        h: gridH
    }
}

Public.resizeGrid = function (adjustH, adjustW, grid) {
  var gridWH = Public.setGrid(adjustH, adjustW)
  grid.jqGrid('setGridHeight', gridWH.h)
  grid.jqGrid('setGridWidth', gridWH.w)
}