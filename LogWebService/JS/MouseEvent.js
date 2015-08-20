// fail js(not finish)
var MouseDrag = function (element) {
    this.element = element;
    this.element.mouseObj = this;//DOM link this object
    this.ElementX = parseInt(element.offsetLeft);
    this.ElementY = parseInt(element.offsetTop);
    this.StartX = 0;//點擊的起始X軸位置(畫面上的絕對位置)
    this.StartY = 0;//點擊的起始Y軸位置(畫面上的絕對位置)
    this.BindEvent = function () {
        console.log(this);
        this.element.onmousedown = this.mousedown;
        this.element.onmousemove = this.mousemove;
        this.element.onmouseup = this.mouseup;
    };
    //element mousedoun event 
    this.mousedown = function (event) {
        var e = event || window.event;
        this.element.mouseObj.StartX = e.pageX;
        this.element.mouseObj.StartY = e.pageY;
        
    };
    //element mousemove event
    this.mousemove = function (event) {
        //change 
        this.element.style.position = 'relative';
        //this.element.style.width = 
    };
    this.mouseup = function (event) {
        //clean object
        this.element.onmousemove = this.element._mouseup = null;
    };
    
}

MouseDrag.prototype = {
    element: "None",
    //物件最初原始位置
    originPosition: { X: this.offsetLeft, Y: this.offsetTop },
    EventList: [],
    //BinddingEvent : function () {
    //    //console.dir(this);
    //    //console.dir(this.element);
    //    console.dir(this);
    //    //var test = MouseDrag.bind(this.element);
    //    //this.EventList.push(test);
    //    this.element.onmousedown = this._mousedown;
    //    //this.element.onmousemove = this._mousemove;
    //    //this.element.onmouseup = this._mouseup;
    //},
    //計算滑鼠位置用的事件方法
    _event : function (event) {
        //相容IE
        if (!event) {
            event = window.event;
            event.target = event.srcElement;
            event.layerX = event.offsetX;
            event.layerY = event.offsetY;
        }
        //計算當前滑鼠指標的X軸距離
        event.totalX = event.pageX || event.clientX + document.body.srcollLeft;//畫面上的絕對位置X || (相對於當下視窗的位置X + 控制左右的scrollBar起始位置)
        //計算當前滑鼠指標的Y軸距離
        event.totalY = event.pageY || event.clientY + document.body.scrollTop; //畫面上的絕對位置Y || (相對於當下視窗的位置Y + 控制上下的scrollBar起始位置)
        return event;
    },
    //記錄滑鼠按下的位置
    _mousedown : function (event) {
        //console.dir(this);//this指向Tag元素
        console.log(MouseDrag.element);//因定義方法在此,故執行方法指標時會回到此位置,內部雖然沒有定義element,但像外搜尋有,所以可以知道
        //console.dir(event);
        //console.dir(MouseDrag);
        //event = MouseDrag.prototype._event.call(this, event);
        //console.dir(MouseDrag.EventList);
        event = MouseDrag.prototype._event.call(this, event);
        console.log("可以繼續下去了...");
        MouseDrag.ElementX = parseInt(MouseDrag.element.offsetLeft);//拖放元素的X軸座標
        MouseDrag.ElementY = parseInt(MouseDrag.element.offsetTop);//拖放元素的Y軸座標
        MouseDrag.StartX = event.totalX;
        MouseDrag.StartY = event.totalY;
        //this._mousemove.bind(element);
        MouseDrag.element.onmousemove = MouseDrag._mousemove;//無法從外來的event再綁回此物件的方法,MouseDrag._mousemove => undefined
        MouseDrag.element.onmouseup = MouseDrag._mouseup;
    },
    //記錄滑鼠每次移動的距離差
    _mousemove : function (event) {
        console.log('start mouse move event...');
        //取得event資訊
        event = MouseDrag._event(event);
        //change position style
        MouseDrag.element.style.position = 'absolute';
        //define drag element x axis distance
        MouseDrag.element.style.left = MouseDrag.ElementX + event.totalX - MouseDrag.StartX + 'px';
        //define drag element y axis distance
        MouseDrag.element.style.top = MouseDrag.ElementY + event.totalY - MouseDrag.StartY + 'px';
    },
    //記錄滑鼠鍵放開後最後的位置
    _mouseup: function (event) {
        console.log('start mouse up event...');
        //取得event資訊
        event = MouseDrag._event(event);
        MouseDrag.ElementX = parseInt(MouseDrag.element.offsetLeft);//記錄拖放元素的X軸座標
        MouseDrag.ElementY = parseInt(MouseDrag.element.offsetTop); //記錄拖放元素的Y軸座標
        MouseDrag.StartX = event.totalX;//記錄滑鼠拖放元素的X軸座標
        MouseDrag.StartY = event.totalY;//記錄滑鼠拖放元素的Y軸座標
        //釋放操作物件
        MouseDrag.element.onmousemove = MouseDrag.element.onmouseup = null;
    },

    _pos: function (element, event) {
        var posX = 0, posY = 0;
        var e = event || window.event;

        if (e.pageX || e.pageY) {
            posX = e.pageX;
            posY = e.pageY;
            console.log(e.pageX, e.pageY);
        }
        else if (e.clientX || e.clientY) {
            posX = e.clientX + document.documentElement.scrollLeft + document.body.scrollLeft;
            posY = e.clientY + document.documentElement.scrollTop + document.body.scrollTop;
        }
        element.style.position = 'absolute';//定義目前物件為絕對定位
        element.style.top = (posY) + 'px';
        element.style.left = (posX) + 'px';
    }
}



