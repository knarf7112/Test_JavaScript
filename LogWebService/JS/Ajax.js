﻿//method='get or post', url=ajax's page, send object,async=true or false, callback=response do method[
var Ajax = function(method, url, sendCmd, async, callback) {
    var xhr;
    if (window.XMLHttpRequest){
        xhr = new XMLHttpRequest();
        if (xhr.overrideMimeType) {
            //部分版本的 Mozilla 瀏覽器，在伺服器送回的資料未含 XML mime-type 檔頭（header）時會出錯
            xhr.overrideMimeType("text/xml");
        }
    }
    else if (window.ActiveXObject) {
        try{
            xhr.ActiveXObject("Msxm12.XMLHTTP");
        }
        catch (e) {
            try{
                xhr = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e) { console.log(e);}
        }
    }

    xhr.open(method.toUpperCase(), url, async);
    xhr.addEventListener("readystatechange", callback, false);
    //xhr.onload = function (data) {
    //    console.log(data);
    //}
    var data = JSON.stringify(sendCmd);//"cmd=" + sendCmd ;

    if (method.toUpperCase() == "POST") {
        xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");//"application/x-www-form-urlencoded");
        xhr.send(data);
    }
    else {
        //xhr.setRequestHeader("Access-Control-Allow-Origin", "*");//sever有寫才AJAX的到資料
        xhr.send(sendCmd);
    }
    
    
}

