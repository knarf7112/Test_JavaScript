//目標功能
//1.建立table框架(即空殼DOM in memory)
//2.使用框架(DOM插入或替換資料)
var TableManager = {
    'Version': '00'
}
//**********************************************
//建立 table 框架 rows=>tr count, columns=>td count
TableManager.Create = function () {
    
}

TableManager.Create.prototype = {
    createTable: function (rows, columns, tableId, tdClassName) {
        //elements
        var className = !!tdClassName ? ( ' class="' + tdClassName + '"') : "";
        var tableIdAttr = !!tableId ? (' id="' + tableId + '"') : "";
        var td = "";
        var th = "";
        var tr = "";
        var thead = "";
        var tbody = "";
        var table = "";
        var parser = new DOMParser();
        var docHtml;
        var element;

        //td collection
        for (var j = 0; j < columns; j++) {
            th += '<th' + className + '></th>';
            td += '<td' + className + '></td>';
        }

        //tr + td collection
        for (var i = 0; i < rows; i++) {
            tr += "<tr>" + td + "</tr>";
        }

        thead = "<thead><tr>" + th + "</tr></thead>";
        tbody = '<tbody>' + tr + '</tbody>';
        table = '<table' + tableIdAttr + '>' + thead + tbody + '</table>';
        docHtml = parser.parseFromString(table, 'text/html');
        element = docHtml.getElementsByTagName('table')[0];

        //release
        parser = null;
        docHtml = null;

        return element;
    },
    insertData: function (dataObj, dataStartIndex, tableElement) {
        var keys = Object.getOwnPropertyNames(dataObj[0]);
        var trElements = tableElement.querySelectorAll('tr');
        //clear table cell value
        for (var i = 0; i < trElements.length; i++) { //tr
            for (var j = 0; j < trElements[i].children.length; j++) { //td or th
                //th
                if (trElements[i].children[j].nodeName.toUpperCase() == 'TH') {
                    trElements[i].children[j].textContent = keys[j];//setting th[i]'s value
                }
                    //tr
                else {
                    //-------TODO...................................
                    
                    if ((i + dataStartIndex) < dataObj.length) {
                        console.log(('dataObj[' + (i + dataStartIndex) + '][' + keys[j] + ']'), dataObj[i + dataStartIndex][keys[j]]);
                        trElements[i].children[j].textContent = dataObj[i + dataStartIndex][keys[j]];
                    }
                    else {
                        trElements[i].children[j].textContent = '';
                    }
                }
            }
        }
        

    },
};

//**********************************************
TableManager.Insert = function () {

}


