//啟動器
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

