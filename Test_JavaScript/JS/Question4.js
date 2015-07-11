//排列組合算出陣列內值合併後的最大值 ex:50,1,9,2 => max: 95021
var arr = [53, 50, 51, 2, 1, 9];
function sort(arr) {
    var result = [];
    //每個元素都跑一次
    for (var i = 0; i < arr.length; i++) {
        if (result.length == 0) {
            result.push(arr[i]);
        }
        else {
            compare(result, arr[i], 0);
        }
    }
    console.log(result);
    return result;
}
//上面方法呼叫比較用
function compare(resultArr, data, len, dataDigit) {
    var digit = dataDigit || 0;
    //如果result[len]不存在,離開
    if ((typeof resultArr[len]) === 'undefined')
        return;
    //如果大於等於resultArr[len]就插入他的位置,剩的往後排
    if (data.toString()[digit] >= resultArr[len].toString()[digit]) {
        //如果位數一樣就從左比到右,若不同則位數越少的越大,要比到data的最小位數才算結束
        if (data.toString().length <= resultArr[len].toString().length &&
        digit == (data.toString().length - 1)) {
            //console.log(data, resultArr[len]);
            resultArr.splice(len, 0, data);
        }
        else {
            compare(resultArr, data, len, digit + 1);
        }
    }
        //如果小於resultArr[len]而且resultArr[len]後面還有資料,就繼續比下去
    else if ((typeof resultArr[++len]) !== 'undefined') {//!!resultArr[++len]){
        compare(resultArr, data, len);
    }
        //如果小於resultArr[len]且後面沒東西了,表示為陣列最尾部
    else {
        resultArr.push(data);
    }
}