//CAP-413 Restrict the Capella EHR access in multiple tabs
if (document.cookie.indexOf("_instance=true") === -1) {
    document.cookie = "_instance=true";
    window.onunload = function () {
        document.cookie = "_instance=true;expires=Thu, 01-Jan-1970 00:00:01 GMT";
    };
}
else {
    var win = window.open("frmRestrictMultipleTabs.aspx", "_self"); win.close();
}

//Below code block is to completly restrict the incognito window as part of CAP-413, it is decided to not restrict
//the incognito window. So, commenting the below code block.
//function isIncognitoMode() {
//    if (navigator.storage && navigator.storage.estimate && performance && performance.memory) {
//        return navigator.storage.estimate().then(function (data) {
//            return data.quota < performance.memory.jsHeapSizeLimit;
//        });
//    } else {
//        return Promise.resolve(false); // Not supported or unable to detect
//    }
//}
//isIncognitoMode().then(function (result) {
//    if (result) {
//        var win = window.open("frmRestrictMultipleTab.aspx", "_self"); win.close();
//    }
//});