//目標功能
//1.建立table框架(即空殼DOM in memory)
//2.使用框架(DOM插入或替換DOM內的資料)
//var TableManager = {
//    'Version': '00'
//}
//**********************************************
//建立 table 框架 rows=>tr count, columns=>td count
var TableManager = function () {
    
}

TableManager.prototype = {
    //建立並回傳新table DOM元素,使用DOMParser
    //rows=表格列數量,column=表格欄數量,tableId=設定table的id屬性名稱,tbClassName=所有欄位的class屬性名稱,flexibleBar=表示是否加入可拉縮的元素
    createTable: function (rowCount, columnCount, tableAttributeString, cellAttributeString, hasFlexibleBar) {
        
        //elements
        var tableAttr = !!tableAttributeString ? (" " + tableAttributeString.trim() + " ") : "";
        var cellAttr = !!cellAttributeString ? (" " + cellAttributeString.trim() + " ") : "";
        var rows = rowCount || 0;
        var columns = columnCount || 0;
        var td = "";
        var th = "";
        var tr = "";
        var thead = "";
        var tbody = "";
        var table = "";
        var docHtml;
        var element;
        var parser = new DOMParser();

        //td collection
        for (var j = 0; j < columns; j++) {
            th += '<th' + cellAttr + '>' + (!!hasFlexibleBar ? '<span style="margin-right: 0px;' +
                                                             'padding:0px 0px;' +
                                                             'height: 30px;' +
                                                             'border:1px solid rgba(0,0,0,0);' +
                                                             'float:right;' +
                                                             'cursor:col-resize;" class="vertical' + j + '"></span>'
                                                             :
                                                             '') + '</th>';
            td += '<td' + cellAttr + '>' + (!!hasFlexibleBar ? '<span style="margin-right: -5px;' +
                                                             'padding:0px 0px;' +
                                                             'height: 30px;' +
                                                             'border:3px solid red;' +
                                                             'float:right;' +
                                                             'cursor:col-resize;" class="vertical' + j + '"></span>' : '') + '</td>';
        }

        //tr + td collection
        for (var i = 0; i < rows; i++) {
            tr += "<tr>" + td + "</tr>" +
            (!!hasFlexibleBar ? '<tr><td colspan="' + columns + '" style="margin:0px 0px;' +
                                                                'width:0px;' +
                                                                'height:0px;' +
                                                                'padding:0px 0px;' + 
                                                                'border:1px solid rgba(0,0,0,0);' +
                                                                'cursor:row-resize;' +
                                                                'class="horizontal' + (i + 1) + '"></td></tr>'
                           : '');
        }

        thead = "<thead><tr>" + th + "</tr>" + (!!hasFlexibleBar ? '<tr><td colspan="' + columns + '" style="margin:0px 0px;' +
                                                                'width:0px;' +
                                                                'height:0px;' +
                                                                'padding:0px 0px;' +
                                                                'border:1px solid rgba(0,0,0,0);' +
                                                                'cursor:row-resize;' +
                                                                'class="horizontal0"></td></tr>'
                                                : '') + "</thead>";
        tbody = '<tbody>' + tr + '</tbody>';
        table = '<table' + tableAttr + '>' + thead + tbody + '</table>';
        console.log('table字串=>',table);
        docHtml = parser.parseFromString(table, 'text/html');
        element = docHtml.getElementsByTagName('table')[0];

        //release
        parser = null;
        docHtml = null;

        return element;
    },

    //元素增加某個class
    addClass: function(element,className){
        if (!(element instanceof HTMLElement)) {
            throw new Error("注入元素非HTML物件");
        }

        if (this.isClassExist(className) && !element.classList.contains(className)) {
            element.classList.add(className);
        }
    },
    //若class存在於document內則回傳true,否則回傳false
    isClassExist: function(className){
        var allStyleSheets = window.document.styleSheets;//get all style sheet include extent or intern at document load
        //search all styleSheet
        for (var styleSheetIndex = 0; styleSheetIndex < allStyleSheets.length; styleSheetIndex++) {
            var rules = allStyleSheets[styleSheetIndex].cssRules || allStyleSheets[styleSheetIndex].rules;
            //search all rules or cssRules
            for (var ruleIndex = 0; ruleIndex < rules.length; ruleIndex++) {
                if (className === rules[ruleIndex].selectorText) {
                    return true;
                }
            }
        }
        return false;
    },
    //元素移除某個class
    removeClass: function(element,className){
        if (!(element instanceof HTMLElement)) {
            throw new Error("注入元素非HTML物件");
        }
        if (element.classList.contains(className)) {
            element.classList.remove(className);
        }
    },
    //插入資料到表格元素
    //dataObj=資料陣列(裡面元素為{xxx:'xx'}),dataStartIndex=設定讀取的資料起始位置,tableElement=要改變資料的table元素
    insertData: function (dataAry, dataStartIndex, tableElement) {
        var startIndex = dataStartIndex || 0;
        var keys;
        var trElements;

        if (!(dataAry instanceof Array)) {
            console.error('插入資料非陣列元素!');
            return;
        }

        keys = Object.getOwnPropertyNames(dataAry[0]);
        //tableElement.querySelectorAll('td[class^=vertical]');
        trElements = tableElement.querySelectorAll('tr');//td:not([class^=horizontal])');//不包含列縮放的元素
        console.dir(trElements);
        //clear table cell value
        for (var i = 0; i < trElements.length; i++) { //tr
            for (var j = 0; j < trElements[i].children.length; j++) { //td or th
                //th
                if (trElements[i].children[j].nodeName.toUpperCase() == 'TH') {
                    trElements[i].children[j].textContent = keys[j];//setting th[i]'s value
                }
                    //tr
                else {
                    //check data length if not overflow 
                    if ((i + startIndex - 1) < dataAry.length) {
                        console.log(('dataObj[' + (i + startIndex - 1) + '][' + keys[j] + ']'), dataAry[i + startIndex - 1][keys[j]]);
                        trElements[i].children[j].textContent = dataAry[i + startIndex - 1][keys[j]];
                    }
                    else {
                        trElements[i].children[j].textContent = '';
                    }
                }
            }
        }
    },
    //展現資料
    //element=要插入的DOM元素, parentElement=要展示資料的父元素, insertBefore=插入在父元素裡面的前面(true)或後面(false)
    display: function (element, parentElement, insertBefore) {
        if (!(parentElement instanceof HTMLElement) || !(element instanceof HTMLElement)) {
            console.log('parameter is not a HTMLElement!!!', element, parentElement);
            return;
        };
        if (!insertBefore) {
                parentElement.appendChild(element);
        }
        else {
            parentElement.insertBefore(element, parentElement.firstChild); 
        }
    },
};
var divs;
//**********************************************
var Grid = function (obj) {
    //紀錄寬度(含border,padding,不含scrollBar)
    this.width = obj.width || obj.displayNode.offsetWidth;
    //紀錄高度(含border,padding,不含scrollBar)
    this.height = obj.height || obj.displayNode.offsetHeight;
    //紀錄欄數
    this.column = obj.column || 5;
    this.row = obj.row || 20;
    //文字大小
    this.font_size = obj.font_size || Math.sqrt(20);
    this.pading_size = obj.pading_size || 2;
    this.border_size = obj.border_size || 2;
    this.scroll_x_size = obj.scroll_x_size || 20;
    this.min_column_width = obj.min_column_width || 30;
    this.resize_Sensitivity = obj.resize_Sensitivity || 3;
    this.buffer_column_width = obj.buffer_column_width || 1.1;
    //json data
    this.data = obj.data || [];
    this.column_width = obj.column_width || [];
    this.column_order = obj.column_order || [];
    //total cell
    this.amount = this.column * (this.row + 1);
    this.node_height = (this.height - this.border_size * (this.row + 2) - this.pading_size * 2 - this.scroll_x_size) / (this.row + 1);
    this.displayNode = obj.displayNode;
    this.gridNode;
    this.contentNode;
    this.$control;
    this.package_num = 0;
    this.row_st = 0;
    this.page_package = {
        page: 0,
        limit_page: 0
    }
    //展現資料的元素陣列
    this.display_nodes = [];
    this.control_nodes = [];
    this.control_package = {
        re_size_mousedown: false,
        re_size_target: "",
        scroll_display_move: 0
    };
    this.Initial = function () {
        if (this.displayNode) {
            this.gridNode = document.createElement("div");
            this.contentNode = document.createElement("div");
            this.gridNode.classList.add("grid");
            this.contentNode.classList.add("content");
            this.gridNode.appendChild(this.contentNode);
            this.displayNode.appendChild(this.gridNode);
            this._refine_data();
            this._create_TableFrame('s1');
            this.header_css_setting('title');
        }
        else {
            console.error("Initial failed !!!");
        }
    }

    this._refine_data = function () {
        // refresh width
        if (!this.buffer_column_width.length) {
            //default width
            var tmp = (this.width - this.border_size * (this.column + 1) - this.pading_size * 2) / this.column;
            if (tmp < this.min_column_width) {
                tmp = this.min_column_width;
            }
            //set default width
            for (var count = 0; count < this.column; count++) {
                this.column_width.push(tmp);
            }
        }
        if (!this.column_order.length) {
            //set default order
            for (var count = 0; count < this.column; count++) {
                this.column_order.push(count);
            }
        }
    };
    //create table frame and set  
    this._create_TableFrame = function (cellClassName) {
        if (!!this.displayNode) {
            var tmp = "";
            var domparser = new DOMParser();
            var main = this;
            var classAttr = !!cellClassName ? (' class="' + cellClassName + '"') : "";

            for (var columnCount = 0; columnCount < this.amount; columnCount++) {
                tmp += "<div" + classAttr + "></div>";
            }
            tmp = '<div class="grid">' + tmp + '</div>';
            var htmlDom = domparser.parseFromString(tmp, 'text/html');
            var gridDOM = htmlDom.getElementsByClassName('grid')[0];
            this.displayNode.appendChild(gridDOM);
            //domparser.parseFromString(tmp,'text/html');
            //select all cell and set attributes in main object
            var divs = Array.prototype.slice.call( this.displayNode.querySelectorAll('.' + cellClassName));//node List convert Array

            divs.forEach(function (element, index) {
                var headerIndex = index % main.column;
                //header
                if (index < main.column) {
                    //push header
                    main.display_nodes.push([{
                        node: element,
                        data: {
                            width: main.column_width[headerIndex],  //header cell width
                            height: main.node_height,                       //header cell height
                            top: main.border_size + main.pading_size,       //header cell Y axis
                            left: main._compute_nodeLeft(headerIndex),
                            type: 'header',
                            value: ''
                        }
                    }]);
                }
                else {
                    //body
                    main.display_nodes[headerIndex].push({
                        node: element,
                        data: {
                            width: main.display_nodes[headerIndex][0].data.width,
                            height: main.node_height,
                            top: (main.node_height + main.border_size) * (Math.floor(index / main.column)) + main.border_size + main.pading_size,
                            left: main.display_nodes[headerIndex][0].data.left,
                            type: 'node',
                            value: ''
                        }
                    });
                }
                //set node attribute
                main.display_nodes[headerIndex][Math.floor(index / main.column)].node.setAttribute('data-column', headerIndex);
                main.display_nodes[headerIndex][Math.floor(index / main.column)].node.setAttribute('data-row', Math.floor(index / main.column));
                main.display_nodes[headerIndex][Math.floor(index / main.column)].node.setAttribute('data-type', main.display_nodes[headerIndex][Math.floor(index / main.column)].data.type);
            });
        } else {
            console.error('Init Error');
        }
    };
    //header css setting
    this.header_css_setting = function (className) {
        for (var columnCount = 0; columnCount < this.column; columnCount++) {
            this.display_nodes[columnCount][0].node.classList.add(className);
        }
    }
    //setting position
    this._location_refresh = function () {
        var main = this;
        for (var headerIndex = 0; headerIndex < this.display_nodes.length; headerIndex++) {
            for (var rowIndex = 0; rowIndex < this.display_nodes[headerIndex].length; rowIndex++) {
                this.display_nodes[headerIndex][rowIndex].node.style.width = this.display_nodes[headerIndex][rowIndex].data.width + 'px';
                this.display_nodes[headerIndex][rowIndex].node.style.height = this.display_nodes[headerIndex][rowIndex].data.height + 'px';
                this.display_nodes[headerIndex][rowIndex].node.style.top = this.display_nodes[headerIndex][rowIndex].data.top + 'px';
                this.display_nodes[headerIndex][rowIndex].node.style.left = this.display_nodes[headerIndex][rowIndex].data.left + 'px';
            }
        }
    };
    this._control_bind = function () {
        var main = this;

    };
    //calculate the node left 
    this._compute_nodeLeft = function (nodeColumnIndex) {
        //one unit 
        var nodeLeft = this.pading_size + this.border_size;// 1 cell/per
        //add all node width
        for (var count = 0; count < nodeColumnIndex; count++) {
            nodeLeft += this.column_width[this.column_order[count]];
        }
        return nodeLeft;
    };
}


