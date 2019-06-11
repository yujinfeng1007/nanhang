(function(){
    window.$ = function(selector){
        return document.querySelector(selector)
    }
    // success写法1
    // Object.prototype.method = function(name,func){
    //     return Object.prototype[name] = func
    // }
    // Object.method('addClass',function(cls){
    //     if(this.className.indexOf(cls)>0)return
    //     this.className += ' '+cls
    // })
    //success写法2
    Object.prototype.addClass = function(cls){
        if(this.className.indexOf(cls)>0)return
        this.className += ' '+cls
    }
    Object.prototype.removeClass = function(cls){
        if(this.className.indexOf(cls)>0){
            this.className.replace(' '+cls,'')
        }
    }
    Object.prototype.toggle = function(cls){
        
    }
   
    
    
    
    
    
    
    console.log(Object.addClass)
   
})()