var count = 0;
//*******************************************************
var data = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
var operator = ["+", "-", ""];
var allResult = [];
var result = "";
PermutationAndCombine(0, result);
//len=維度, result=結果
function PermutationAndCombine(len, result) {
    if (len == data.length - 1) {
        //記錄結果
        allResult.push(result + data[len]);
        return;//離開點
    }
    for (var i = 0; i < operator.length; i++) {
        var tmp = data[len] + operator[i];
        PermutationAndCombine(len + 1, result + tmp);
    }
}
console.log("" + allResult);
//*******************************************************
//測試迴圈的recursive

//var count = callback(a, a.length, 0);
function callback(arr, len, count) {
    if (len == 0) {
        console.log("len=" + len + " count:" + count);
        return;
    }
    var i;
    for (i = 0; i < arr.length; i++) {
        console.log("i=" + i + "  len=" + len);
        //console.log(arr);
        count++;
        callback(arr, len - 1, count);
    }
    
}
console.log(count);
//********************************************************
