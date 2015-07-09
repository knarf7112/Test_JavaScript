//啟動器
//this寫法
var Timer = function (doMethod, timeout, methodParameters) {
    this.startId = 0;
    this.callMethod = doMethod;
    this.methodParameters = methodParameters;
    this.timeout = timeout;
    this.start = function () {
        this.startId = setInterval(this.callMethod, this.timeout, this.methodParameters);
    };
    this.stop = function () {
        clearInterval(this.startId);
    };
}

//Three.js寫法

//啟動器
//doMethod=要執行的方法, methodParameters=方法要帶入的參數, timeout=執行的間隔時間
var Timer = function (doMethod, methodParameters, timeout) {
    if (!!doMethod)
        this.callMethod = doMethod;
    if (!!methodParameters)
        this.methodParameters = methodParameters;
    if (!!timeout)
        this.timeout = timeout;
};

Timer.prototype = {
    startId: -1,//因為消失就無法停止,故給予原型預設值確保屬性存在
    timeout: 1000,//同上

    //
    start: function () {
        var callFunction = this.callMethod || function () { console.log('沒有指定執行方法,所以跑預設的 ...'); };
        this.startId = setInterval(callFunction, this.timeout, this.methodParameters);
    },

    stop: function () {
        clearInterval(this.startId);
    }
};
