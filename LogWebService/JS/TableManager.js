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
            th += '<th' + cellAttr + '>' + (!!hasFlexibleBar ? '<span style="margin:0px 0px;' +
                                                             'padding:0px 0px;' + 
                                                             'border:1px solid rgba(0,0,0,0);' +
                                                             'cursor:row-resize;" class="vertical' + j + '"></span>'
                                                             :
                                                             '') + '</th>';
            td += '<td' + cellAttr + '>' + (!!hasFlexibleBar ? '<span style="margin:0px 0px;' +
                                                             'padding:0px 0px;' +
                                                             'border:1px solid rgba(0,0,0,0);' +
                                                             'cursor:row-resize;" class="vertical' + j + '"></span>' : '') + '</td>';
        }

        //tr + td collection
        for (var i = 0; i < rows; i++) {
            tr += "<tr>" + td + "</tr>" +
            (!!hasFlexibleBar ? '<tr><td conspan="' + columns + '" style="margin-right: -5px;' +
                                                                'border:1px solid rgba(0,0,0,0);' +
                                                                'cursor:col-resize;' +
                                                                'float:right;" ' +
                                                         'class="horizontal' + (i + 1) + '"></td></tr>'
                           : '');
        }

        thead = "<thead><tr>" + th + "</tr>" + (!!hasFlexibleBar ? '<tr><td conspan="' + columns + '" style="margin-right: -5px;' +
                                                                'border:1px solid rgba(0,0,0,0);' +
                                                                'cursor:col-resize;' +
                                                                'float:right;" ' +
                                                                'class="horizontal0"></></tr>'
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
        tableElement.querySelectorAll('td[class^=]')
        trElements = tableElement.querySelectorAll('tr');
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

//**********************************************



