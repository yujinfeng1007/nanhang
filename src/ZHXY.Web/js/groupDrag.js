var moduleGroup=(function()
{
    var wrapper=document.querySelector("#screen"),itemWidth=136,dataTransfer,
        moduleData=null,containerList=null,dataCopy,container,items,groups,exterLen,
        isDraging,cols,rows,clientX,clientY,dropTag,dragTag,
        currGroupIndex=-1,currGroupSortI,currGroupDom,currGroupInput,currGroupLis,firstLayerTitle,firstLayer;

    //添加事件
    function addDragleEvent()
    {
        //执行这里兼容 IE9及以下浏览器，
        if(typeof container.style.transition==="undefined")
        {
            for(var i=0;i<items.length;i++)
            {
                items[i].addEventListener("mousedown",function()
                {
                    this.dragDrop();
                },false);
            }
        }
        container.addEventListener("dragstart",evtStart,false);
        container.addEventListener("drag",evtDrag,false);
        container.addEventListener("dragend",evtEnd,false);
        container.addEventListener("dragenter",evtEnter,false);
        container.addEventListener("dragleave",evtLeave,false);
        container.addEventListener("dragover",evtOver,false);
        container.addEventListener("drop",evtDrop,false);
        container.addEventListener("click",clickGroup,false);
    }
    function evtStart(evt)
    {
        if(evt.target.nodeName.toLocaleLowerCase()==="li")
        {
            isDraging=true;
            dataTransfer=evt.dataTransfer;
            dataTransfer.setData("text","this is test");	//为了兼容Firefox
            dragTag=evt.target;
        }
    }
    function evtDrag()
    {
        if(isDraging)
        {
            dragTag.style.opacity=0.3;
        }
    }
    function evtEnd(evt)
    {
        if(isDraging)
        {
            dragTag.style.opacity=1;
            isDraging=false;
        }
        //因为智慧校园的首页存在的问题(初步判断为其他控件改写了原生拖拽事件，导致drop事件的event对象有问题，指向了dragTag)
        //导致在 drop 事件中获取到的鼠标坐标不对，不是鼠标松开的的坐标，而是点击 dragTag 是的坐标，dragover，drag事件都是如此
        //这个问题只有在这个页面才会有，我自己单独测试的页面没有,所以在这面取 drop 事件时的鼠标坐标，然后延迟一段时间在执行 drop 事件
        clientX=evt.clientX;
        clientY=evt.clientY;
    }
    function evtEnter(evt)
    {
        evt.preventDefault();
        evt.stopPropagation();
        var evtTag=evt.target;
        if(evtTag===dragTag) { return }
        switch(evtTag.className)
        {
            case "navItem container":
                dropTag=evtTag;
                evtTag.className="navItem container hover";
                break;
            case "navItem":
                dropTag=evtTag;
                evtTag.className="navItem hover";
                break;
            case "navItemContainer":
                evtTag.className="navItemContainer dashedBorder";
                break;
            case "screenUl currentScreen":
                evtTag.className="screenUl currentScreen dashedBorder";
                break;
        }
    }
    function evtLeave(evt)
    {
        evt.preventDefault();
        evt.stopPropagation();
        var evtTag=evt.target;
        if(evtTag===dragTag) { return }
        switch(evtTag.className)
        {
            case "navItem container hover":
                evtTag.className="navItem container";
                break;
            case "navItem hover":
                evtTag.className="navItem";
                break;
            case "navItemContainer dashedBorder":
                evtTag.className="navItemContainer";
                break;
            case "screenUl currentScreen dashedBorder":
                evtTag.className="screenUl currentScreen";
                break;
        }
    }
    function evtOver(evt)
    {
        evt.preventDefault();
        evt.stopPropagation();
    }
    function evtDrop(evt)
    {
        evt.preventDefault();
        evt.stopPropagation();
        var data=dataTransfer.getData("Text"),evtTag=evt.target,evtClassName=evtTag.className;
        if(evtTag===dragTag) { return; }

        //因为 event时间被修改导致的 bug，所以才延迟执行
        setTimeout(function()
        {
            //火狐正常，没有这个bug
            if(clientX===0&&clientY===0)
            {
                clientX=evt.clientX;
                clientY=evt.clientY;
            }
            switch(evtClassName)
            {
                case "navItemContainer dashedBorder":
                    moveItem(evt,"groupIndex",6);
                    evtTag.className="navItemContainer";
                    break;
                case "screenUl currentScreen dashedBorder":
                    moveItem(evt,"sortIndex",cols);
                    evtTag.className="screenUl currentScreen";
                    break;
                case "navItem hover":
                    if(dragTag.className==="navItem container")
                    {
                        evtTag.className="navItem";
                        return;
                    }
                    groupItem(evt.target.className);
                    break;
                case "navItem container hover":
                    if(dragTag.className==="navItem container")
                    {
                        evtTag.className="navItem container";
                        return;
                    }
                    groupItem(evt.target.className);
                    break;
            }
            reSort(moduleData);
            isDraging=false;
            dropTag=null;
            dragTag=null;
        },50);
    }

    function clickGroup(evt)
    {
        var eleTag=evt.target,$target=$(eleTag),eleTagClassName=eleTag.className,wrapperCont=container.parentNode;
        switch(eleTagClassName)
        {
            case "navItem container":

                //获取展开组的 DOM、属性
                if(currGroupIndex===-1)
                {
                    currGroupDom=eleTag;
                    currGroupInput=eleTag.getElementsByTagName("input")[0];
                    currGroupLis=eleTag.getElementsByTagName("li");
                    for(var i=0,len=groups.length;i<len;i++)
                    {
                        if(groups[i].item.groupName===currGroupInput.value)
                        {
                            currGroupSortI=groups[i].item.sortIndex;
                            currGroupIndex=i;
                            break;
                        }
                    }
                }
                firstLayerTitle=currGroupInput.value;
                wrapperCont.style.zIndex=10;     // 样式有bug，fixed 存在覆盖不全面问题
                currGroupDom.className="navItem containerFixed";
                currGroupDom.draggable=false;
                container.style.overflow="visible";
                $(evt.target).find('>.navItemIco').hide();
                $(evt.target).find('ul.third').show().end().find('ul.higher').hide();
                //修改展开的组的 DOM
                for(var i=0,len=currGroupLis.length;i<len;i++)
                {
                    if(currGroupLis[i].className.indexOf('folder')!==-1)
                    {
                        currGroupLis[i].className="navItem del folder";
                    } else
                    {
                        currGroupLis[i].className="navItem del";
                    }
                    var delTag=document.createElement("dfn");
                    delTag.className="removeItem";
                    currGroupLis[i].appendChild(delTag);
                }
                reSort(moduleData);
                break;
            case "closeWindow":
            case "navItem containerFixed":
                currGroupInput.className="groupName";
                var inputValue=currGroupInput.value;
                //判断组名
                for(var i=0,len=groups.length;i<len;i++)
                {
                    if(i===currGroupIndex) { continue }
                    if(inputValue===groups[i].item.groupName) { return alert("组名称不能与已有的组名重复") }
                }
                if(inputValue.length>10)
                {
                    alert("组名长度不能大于10个字符!");
                    return;
                }

                //关闭组操作
                wrapperCont.style.zIndex="";		// 样式有bug，fixed 存在覆盖不全面问题
                currGroupDom.className="navItem container";
                currGroupDom.draggable=true;
                container.style.overflow="";
                //修改组内 li 的DOM
                for(var i=0,len=currGroupLis.length;i<len;i++)
                {
                    if(currGroupLis[i].className.indexOf('folder')!==-1)
                    {
                        currGroupLis[i].className="navItem inner folder";
                    } else
                    {
                        currGroupLis[i].className="navItem inner";
                    }

                    currGroupLis[i].removeChild(currGroupLis[i].lastChild);
                }
                //判断是否需要修改组名
                //if( inputValue !== groups[currGroupIndex].groupName){
                //    moduleData.forEach(function (v) {
                //       v.sortIndex === currGroupSortI && (v.groupName = currGroupInput.value);
                //   });
                //  }
                //重置当前组的变量
                currGroupInput.value=firstLayerTitle;
                currGroupLis=null;
                currGroupInput=null;
                currGroupDom=null;
                currGroupSortI=-1;
                currGroupIndex=-1;
                reSort(moduleData);
                $(evt.target).find('>.navItemIco').show();
                $(evt.target).find('ul').hide();
                console.log(firstLayerTitle);

                break;
            case "groupName":
                eleTag.className="groupName edit";
                eleTag.select();
                break;
            case "removeItem":
                evt.stopPropagation();
                currGroupInput.className="groupName";

                var removeItem=eleTag.parentNode,currGroupSysMenuID,
                    removeData=getItemData(removeItem.getAttribute("data-menu"),moduleData),
                    removeGroupI=removeData.groupIndex;

                //删除item，放置到父容器中
                removeData.sortIndex=exterLen;
                removeData.groupIndex=0;
                removeData.groupName="";
                removeItem.className="navItem";
                removeItem.removeChild(removeItem.lastChild);
                container.appendChild(removeItem);

                moduleData.forEach(function(v)
                {
                    if(v.sortIndex===currGroupSortI)
                    {
                        currGroupSysMenuID===undefined&&(currGroupSysMenuID=v.F_Id);
                        v.groupIndex>removeGroupI&&(v.groupIndex-=1);
                    }
                });
                currGroupDom.setAttribute("data-menu",currGroupSysMenuID);
                groups[currGroupIndex].len-=1;

                //这个是删除组里所有的项后，执行
                if(groups[currGroupIndex].len===0)
                {
                    groups.splice(currGroupIndex,1);
                    moduleData.forEach(function(v)
                    {
                        v.sortIndex>currGroupSortI&&(v.sortIndex-=1);
                    });
                    container.removeChild(currGroupDom);

                    //关闭组操作
                    wrapperCont.style.zIndex="";		// 样式有bug，fixed 存在覆盖不全面问题
                    container.style.overflow="";
                    //重置当前组的变量
                    currGroupLis=null;
                    currGroupInput=null;
                    currGroupDom=null;
                    currGroupSortI=-1;
                    currGroupIndex=-1;
                }
                reSort(moduleData);
                break;
            case "navItemContainer":
                currGroupInput.className="groupName";
                break;
            case "navItem del folder":

                var menuData=$target.data('win');
                var $wrapper=$target.parents('li.navItem.containerFixed');
                console.log(menuData);
                $wrapper.find('ul.higher').empty();
                $wrapper.find('ul.higher').show().end().find('ul.third').hide();
                currGroupInput.value=menuData.F_FullName;
                $.each(menuData.ChildNodes,function(i,item)
                {
                    var $li;
                    if(item.ChildNodes.length)
                    {
                        $li=$('<li class="navItem del folder" data-name='+item.F_FullName+' data-menu='+item.F_Id+'><img class="navItemIco" src="/UpLoad/menu/023.png"><span class="navItemLabel">'+item.F_FullName+'</span></li>')
                    } else
                    {
                        $li=$('<li class="navItem del" data-name='+item.F_FullName+' data-menu='+item.F_Id+'><img class="navItemIco" src="'+(item.F_Ico? window.sys.menuIconPath+''+item.F_Ico:window.sys.menuIconPath+'085.png')+'"><span class="navItemLabel">'+item.F_FullName+'</span></li>')
                    }
                    console.log($li);
                    $wrapper.find('ul.higher').append($li);
                    $li.data('win',item)
                });
                break;
        }
    }

    //删除事件
    function removeGroupDom()
    {
    }

    //排序事件
    function moveItem(evt,indexType,colsLen)
    {
        var dragData=getItemData(dragTag.getAttribute("data-menu"),moduleData),
            dragSortI=dragData[indexType],
            dropX,dropY,x,y,dropSortI,maxLen;

        //计算 drop 事件的clienX，clienY 的值，计算出 dropSortI 的值
        if(indexType==="sortIndex")
        {
            dropX=clientX-160;
            dropY=clientY-100;
            maxLen=exterLen;
        } else
        {
            dropX=clientX-evt.target.offsetLeft;
            dropY=clientY-evt.target.offsetTop;
            maxLen=groups[currGroupIndex].len;
        }
        x=Math.floor((dropX+68)/itemWidth);
        y=Math.floor(dropY/itemWidth);
        x>=colsLen&&(x=colsLen);
        dropSortI=y*cols+x;
        dropSortI>maxLen&&(dropSortI=maxLen);

        console.log("dropSortI==="+dropSortI);

        //根据放置的位置，更改数组中对应的值
        if(dropSortI>dragSortI)
        {
            if(indexType==="sortIndex")
            {
                moduleData.forEach(function(v)
                {
                    if(v.sortIndex===dragSortI)
                    {
                        v.sortIndex=dropSortI-1
                    } else if(v.sortIndex<dropSortI&&v.sortIndex>dragSortI)
                    {
                        v.sortIndex-=1
                    }
                });
            } else
            {
                moduleData.forEach(function(v)
                {
                    if(v.sortIndex===currGroupSortI)
                    {
                        if(v.groupIndex===dragSortI)
                        {
                            v.groupIndex=dropSortI-1;
                        } else if(v.groupIndex<dropSortI&&v.groupIndex>dragSortI)
                        {
                            v.groupIndex-=1;
                        }
                    }
                });
            }
        } else if(dropSortI<dragSortI)
        {
            if(indexType==="sortIndex")
            {
                moduleData.forEach(function(v)
                {
                    if(v.sortIndex===dragSortI)
                    {
                        v.sortIndex=dropSortI;
                    } else if(v.sortIndex>=dropSortI&&v.sortIndex<dragSortI)
                    {
                        v.sortIndex+=1;
                    }
                });
            } else
            {
                moduleData.forEach(function(v)
                {
                    if(v.sortIndex===currGroupSortI)
                    {
                        if(v.groupIndex===dragSortI)
                        {
                            v.groupIndex=dropSortI;
                        } else if(v.groupIndex>=dropSortI&&v.groupIndex<dragSortI)
                        {
                            v.groupIndex+=1;
                        }
                    }
                });
            }
        }
    }

    //分组事件
    function groupItem(className)
    {
        var dragData=getItemData(dragTag.getAttribute("data-menu"),moduleData),
            dropData=getItemData(dropTag.getAttribute("data-menu"),moduleData),
            dragSortI=dragData.sortIndex,dropSortI=dropData.sortIndex;

        switch(className)
        {
            case "navItem container hover":
                dropTag.className="navItem container";
                dragTag.className="navItem inner";

                //遍历groups数组，找到对应的分组，将 dragTag 添加进去
                groups.forEach(function(v)
                {
                    if(v.item.sortIndex===dropSortI)
                    {
                        dragData.sortIndex=v.item.sortIndex;
                        dragData.groupName=v.item.groupName;
                        dragData.groupIndex=v.len++;
                        v.dom.appendChild(dragTag);
                    }
                });
                break;
            case "navItem hover":
                //判断组名有没有重复，组名从最大的开始递加
                var gNameIndex=1,gName="功能组合";
                if(groups.length>0)
                {
                    groups.forEach(function(v)
                    {
                        if(v.item.groupName.indexOf("功能组合")===0)
                        {
                            var str=/.+(\d+)/.exec(v.item.groupName)[1];
                            var num=parseInt(str);
                            num>=gNameIndex&&(gNameIndex=num+1);
                        }
                    });
                }
                gName+=gNameIndex;

                //改变数组，dom
                dropData.groupName=gName;
                dragData.sortIndex=dropSortI;
                dragData.groupIndex=1;
                dragData.groupName=gName;
                dropTag.className="navItem inner";
                dragTag.className="navItem inner";

                var dom=createGroupDom(dropData,dropTag.style.cssText);
                dom.appendChild(dropTag);
                dom.appendChild(dragTag);
                groups.push({ item: dropData,len: 2,dom: dom });
                break;
        }
        //遍历数组，调整排序
        moduleData.forEach(function(v)
        {
            v.sortIndex>dragSortI&&(v.sortIndex-=1);
        });
    }

    //创建组DOM
    function createGroupDom(data,cssText)
    {
        var li=document.createElement("li"),
            ul=document.createElement("ul"),
            input=document.createElement("input");
        li.className="navItem container";
        li.setAttribute("data-menu",data.F_Id);
        li.draggable=true;
        cssText===undefined||(li.style.cssText=cssText);
        ul.className="navItemContainer";
        input.className="groupName";
        input.value=data.groupName;
        li.appendChild(ul);
        li.appendChild(input);
        container.appendChild(li);
        return ul;
    }

    //计算坐标
    function coordinate(index,cols,width)
    {
        var outX=index%cols,outY=Math.floor(index/cols);
        return {
            x: outX*width,
            y: outY*width
        }
    }

    //获取子项的数据
    function getItemData(sysMenuId,data)
    {
        for(var i=0,len=data.length;i<len;i++)
        {
            if(data[i].F_Id===sysMenuId)
            {
                return data[i];
            }
        }
    }

    //重构DOM ，修改
    function reCreateDom(data)
    {
        var item,itemData,len=groups.length,groupsItem;
        //修改 DOM
        for(var i=data.length-1;i>=0;i--)
        {
            item=items[i];
            itemData=data[i];
            item.draggable=true;
            if(itemData.groupName==="") { continue; }
            for(var j=0;j<len;j++)
            {
                groupsItem=groups[j];
                if(groupsItem.item.groupName===itemData.groupName)
                {
                    groupsItem.dom===undefined&&(groupsItem.dom=createGroupDom(itemData));
                    item.className="navItem inner";
                    groupsItem.dom.appendChild(item);
                    break;
                }
            }
        }
    }

    //获取最大长度
    function getMaxLen(data)
    {
        var len=0;
        data.forEach(function(v)
        {
            v.sortIndex>=len&&(len=v.sortIndex+1);
        });
        return len;
    }

    //重新计算宽高位置
    function reSort(data)
    {
        var maxWidth=document.documentElement.clientWidth-200,item,coord,sIndex,gIndex,contWidth,aData;
        //根据item 的个数，重置父容器的宽高
        exterLen=getMaxLen(data);
        cols=Math.floor(maxWidth/itemWidth);
        rows=Math.ceil(exterLen/cols);
        contWidth=data.length<cols? data.length*itemWidth+4:maxWidth;
        container.style.width=contWidth+"px";
        container.style.height=rows*itemWidth+"px";

        //算出每个 item 的坐标位置
        for(var i=0,len=items.length;i<len;i++)
        {
            item=items[i];
            aData=getItemData(item.getAttribute("data-menu"),data);
            sIndex=aData.sortIndex;
            gIndex=aData.groupIndex;
            switch(item.className)
            {
                case "navItem":
                case "navItem container":
                    coord=coordinate(sIndex,cols,itemWidth);
                    break;
                case "navItem inner":
                    coord=coordinate(gIndex,3,32);
                    break;
                case "navItem del":
                    coord=coordinate(gIndex,6,itemWidth);
                    break;
                case "navItem containerFixed":
                    coord={ x: 0,y: 0 };
            }
            item.style.cssText="transform:translate("+coord.x+"px,"+coord.y+"px)";
        }
    }

    //数据格式化（不标准的数据处理）
    function dataParse(data)
    {
        if(data.length===0) { return; }

        //新增加的功能，是没有sortIndex、groupIndex 属性的，添加属性
        exterLen=getMaxLen(data);
        data.forEach(function(v)
        {
            if(v.sortIndex===-1||v.sortIndex===undefined)
            {
                v.sortIndex=exterLen++;
                v.groupIndex=0;
                v.groupName="";
            }
        });

        var dataCopy=JSON.parse(JSON.stringify(data)),len=data.length,si=0,gi=0,i=0,dCopy;

        //数组副本排序
        dataCopy.sort(function(a,b)
        {
            if(a.sortIndex-b.sortIndex===0)
            {
                return a.groupIndex-b.groupIndex
            } else
            {
                return a.sortIndex-b.sortIndex
            }
        });

        //数组sortIndex groupIndex 重新赋值
        do
        {
            dCopy=dataCopy[i];
            dCopy.sortIndex=si;
            if(dCopy.groupName==="")
            {
                si++;
            } else
            {
                dCopy.groupIndex=gi;
                gi++;
                if(i<(len-2)&&dCopy.groupName!==dataCopy[i+1].groupName)
                {
                    si++;
                    gi=0;
                }
            }
            i++;
        } while(i<len);

        //将重新赋值后的数组，映射到原数组中
        data.forEach(function(v)
        {
            for(var j=0;j<len;j++)
            {
                if(dataCopy[j].F_Id===v.F_Id)
                {
                    v.sortIndex=dataCopy[j].sortIndex;
                    v.groupIndex=dataCopy[j].groupIndex;
                    break;
                }
            }
        });
    }

    //数据分组
    function sortingGroups(data)
    {
        var i,isCreate;
        groups=[];
        data.forEach(function(v)
        {
            if(v.groupName!=="")
            {
                if(groups.length>0)
                {
                    isCreate=groups.some(function(val,ind)
                    {
                        i=ind;
                        return val.item.groupName===v.groupName;
                    });
                    isCreate? groups[i].len++:groups.push({ item: v,len: 1 });
                } else
                {
                    groups.push({ item: v,len: 1 })
                }
            }
        });
    }

    return {
        reSort: function()
        {
            if(moduleData===null) { return; }
            reSort(moduleData);
        },
        addDragEvent: function(index,data)
        {
            //dom 查询
            containerList===null&&(containerList=wrapper.querySelectorAll(".screenUl"));
            container=containerList[index];
            items=$(container).find('.navItem:not(.inner)');

            // //数据处理
            moduleData=data[index];
            dataCopy=JSON.parse(JSON.stringify(moduleData));

            //第一次加载数据的时候，格式化数组,修改 DOM
            items[0].getAttribute("draggable")===null&&dataParse(moduleData);
            // console.log(JSON.stringify(moduleData,["text","groupIndex","sortIndex"],4));
            sortingGroups(moduleData);
            console.log(moduleData,'moduleData');
            items[0].getAttribute("draggable")===null&&reCreateDom(moduleData);

            reSort(moduleData);
            addDragleEvent();
        },
        getData: function()
        {
            if(moduleData===null) { return null; }
            var change=moduleData.some(function(v,i)
            {
                return v.sortIndex!==dataCopy[i].sortIndex||v.groupIndex!==dataCopy[i].groupIndex||v.groupName!==dataCopy[i].groupName;
            });
            return change? moduleData:null;
        },
        removeDragEvent: function(index)
        {
        },
        setItemWidth: function(value)
        {
            if(typeof value==="number") { throw new Error("请输入正确的宽度格式,宽度格式为整数"); }
            itemWidth=value;
        }
    }
})();