//push array的方式
function recursive(arr, len, result) {

    var rotated = function (ary, index) {
        var tmpAry = [];
        tmpAry.push(ary[index]);
        return tmpAry.concat(ary.slice(0, index)).concat(ary.slice(index + 1, ary.length));
    };
    var rcursiveAry = "";
    var allPermutation = [];
    function backTracking(arr, result) {
        if (arr.length == 0) {
            console.log(result);
            allPermutation.push(result);
            return;//result;
        }
        for (var i = 0; i < arr.length; i++) {
            //console.log('目前i='+i+'  :arr:' + arr);
            var rotatedAry = rotated(arr, i);
            //console.log('目前i=' + i + '   arr:' + rotatedAry);

            //result += rotatedAry.shift();
            var header = rotatedAry.shift();
            backTracking(rotatedAry, result + header);
            
            //console.log(result);
            //console.log("i=" + i + " arr:" + tmp);

        }
    }
    backTracking(arr, rcursiveAry);
    //console.log(rcursiveAry)
    return allPermutation;
}