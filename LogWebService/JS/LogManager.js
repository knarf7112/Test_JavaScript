//var LogManager = { REVISION: '00' };
var LogManager = function () {

};
LogManager.prototype = {
    //default command
    commandObj: {
        Cmd: 'GetLog',
        TableName: 'alcenterlog',
        lastId: -1,
        searchDate: "20150626"
    },
    tableManager: !!TableManager ? new TableManager() : undefined,
    //資料模組
    Data: {
        originData: [],//temp origin JSON data
        groupData: undefined,
        currentIndex: 0,//set the last get origin JSON data
        Category: ['Id', 'Date', 'Thread', 'Level', 'Logger', 'Message', 'Exception', 'HostId'],

        //新增JSON資料到originData列表
        insert: function (data) {
            //原始資料太多,清一下
            if (this.originData.length > 100) {
                this.originData.shift();
            }
            var jsonData = JSON.parse(data);
            this.originData.push(jsonData);
        },
        getListGroupBy: function (category, listObj) {
            var resultObj = {};
            //var category = category
            var list = !!listObj ? listObj : this.originData[this.currentIndex];
            for (var i = 0; i < list.length; i++) {
                var keyName = (category + list[i][category]);//category:"Thread" + Id:123 => "Thread123"
                //不存在則在Object新增key and []
                if (!resultObj[keyName]) {
                    resultObj[keyName] = [];
                }
                resultObj[keyName].push(list[i]);
            }
            return resultObj;
        },
    },


};
//**********************************************************
//request command and response command
//LogManager.TransCommandObj = function (cmdObj) {
//    //default is get log
//    if (arguments.length == 0) {
//        return {
//            Cmd: 'GetLog',
//            TableName: 'alcenterlog',
//            lastId: -1,
//            searchDate: "20150626"
//        };
//    }
//    else {
//        //just {}
//        if (!(cmdObj instanceof Array) && ((typeof cmdObj) === 'object'))
//            return cmdObj;
//        else
//            throw new Error("parameter isn't pure object");
//    };
//};

//LogManager.TransCommandObj.prototype = {
    
//};
//**********************************************************
//LogManager.Data = function (jsonData) {
//    try {
//        //this.josnObject not exists!
//        if (!this.OriginJsonObject)
//            this.OriginJsonObject = JSON.parse(jsonData);//ex:[{id:1,...},{id:2,...},{id:3,...},...]
//            //if jsonObject and data is instance of Array Object
//        else if ((this.OriginJsonObject instanceof Array) && (jsonData instanceof Array))
//            this.OriginJsonObject = this.OriginJsonObject.concat(jsonData);
//        else
//            throw new Error('parameter is incorrectly formatting:' + data);
//    }
//    catch (e) {
//        console.log('JSON parse failed: ' + e.toString());
//    }
//};

//LogManager.Data.prototype = {
//    originData: [],
//    constructor: LogManager.Data,

//    Category: [ 'Id', 'Date', 'Thread', 'Level', 'Logger', 'Message', 'Exception', 'HostId'],

//    getListGroupBy: function (category,listObj) {
//        var resultObj = {};
//        var list = !!listObj ?  listObj : this.OriginJsonObject;
//        for (var i = 0; i < list.length; i++) {
//            var keyName = (category + list[i][category]);//category:"Thread" + Id:123 => "Thread123"
//            //不存在則在Object新增key and []
//            if (!resultObj[keyName]) {
//                resultObj[keyName] = [];
//            }
//            resultObj[keyName].push(list[i]);
//        }
//        return resultObj;
//    },
    
//    //display
//};
//**********************************************************

