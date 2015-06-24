var callback = function (xhr) {
    console.log("callback:");
    console.log(xhr);
    if (xhr.readyState == 4) {
        if (xhr.status == 200) {
            console.log("回傳成功:");
            console.log(xhr.responseText);
        }
        else {
            alert('There was a problem with the request.');
        }
    }
}

function Ajax(method, url, sendCmd, async) {
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
    xhr.onload = function (data) {
        console.log(data);
    }
    if (method.toUpperCase() == "POST") {
        xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    }
    var data = { cmd: sendCmd };
    xhr.send(data);
}

