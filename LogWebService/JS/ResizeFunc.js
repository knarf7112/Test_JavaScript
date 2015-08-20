var ResizeFunc = function (element) {
    this.element = element;//HTML DOM Element
    this.element.mainObj = this;//bind the new object and element
    this.mousePressed;//mouse down flag
    this.startX;// start X axis(document position : absolute)
    this.startWidth;//element default width
    this.className;//add/remove class in element

    this.Init = function () {
        console.log(this);
        this.element.onmousedown = this._mousedown;
        this.element.onmousemove = this._mousemove;
        this.element.onmouseup = this._mouseup;
        this.element.onselectstart = this._selectstart;
        this.element.onmouseenter = function (event) {
            this.style.cursor = 'col-resize';
        };
        this.element.onmouseleave = function (event) {
            this.style.cursor = '';
        };
    };
    //mouse down event
    this._mousedown = function (event) {
        //check defined link object property is exist
        if (!!this.mainObj) {
            var e = event || window.event;
            this.mainObj.mousePressed = true;
            this.mainObj.startX = e.pageX;
            this.mainObj.startWidth = this.offsetWidth;
            this.style.cursor = "col-resize";
            console.log(this.mainObj.mousePressed,this.mainObj.startX,this.mainObj.startWidth,this.style.cursor);
        }
    };
    //mouse move event
    this._mousemove = function (event) {
        //if (!!event.target.mainObj && event.target.mainObj.mousePressed) {
        //    //預設的元素寬度 + (移動時的文件絕對位置 - 起始的文件絕對位置)
        //    event.target.style.width = (event.target.mainObj.startWidth + event.pageX - event.target.mainObj.startX) + 'px';
        //    console.log('width:' + event.target.style.width);
        //}

        if (!!this.mainObj && this.mainObj.mousePressed) {
            //預設的元素寬度 + (移動時的文件絕對位置 - 起始的文件絕對位置)
            this.style.width = (this.mainObj.startWidth + event.pageX - this.mainObj.startX) + 'px';
            console.log('width:' + this.style.width);

        }
    };
    //mouse up event
    this._mouseup = function (event) {
        //if (!!event.target.mainObj && event.target.mainObj.mousePressed) {
        //    event.target.mousePressed = false;
        //    event.target.style.cursor = '';
        //}
        //*****************************************************
        if (!!this.mainObj && this.mainObj.mousePressed) {
            this.mainObj.mousePressed = false;
            this.style.cursor = "";
        }
    };
    //disabled element select
    this._selectstart = function(){
        return false;
    };
}