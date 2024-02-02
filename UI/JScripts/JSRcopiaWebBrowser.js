//function RcopiaDownload() {
//    StartRcopiaStrip();
//    $.ajax({
//        type: "POST",
//        url: "frmEncounter.aspx/DownloadRcoipa",
//        contentType: "application/json;charset=utf-8",
//        dataType: "json",
//        async: true,
//        success: function (data) {
//            //Jira CAP-1366
//            StopRcopiaStrip();
//            RcopiaErrorAlert(data.d);
//        },
//        error: function (result) {
//            //Jira CAP-1366
//            StopRcopiaStrip();
//            //alert(result.d);
//        }
//    });
//}
//top.window.document.addEventListener("visibilitychange", function () {
//    if (top.window.document?.getElementById('ctl00_C5POBody_EncounterContainer')?.contentWindow?.document?.querySelector('#myTabs .active')?.id != undefined
//        && top.window.document?.getElementById('ctl00_C5POBody_EncounterContainer')?.contentWindow?.document?.querySelector('#myTabs .active')?.id == "tabStripEncounter_tbEPrescription") {
//        alert("visibilitychange event called");
//    }
//    //RcopiaDownload();
//});
////$(top.window).on("blur",function () {
////    alert("blur event called");
////});
////$('iframe').each(function () {
////    $(this.contentWindow).bind({
////        focus: function () {},
////        blur: function () {
            
////        }
////    });
////});
////$($($(top.window.document?.getElementById('ctl00_C5POBody_EncounterContainer')?.contentWindow?.document?.querySelector('#myTabs .active'))[0].parentElement.parentElement.children[1]).find(".active")[0].children[0]).each(function () {
////    $(this.contentWindow).bind({
////        focus: function () { },
////        blur: function () {
////            RcopiaDownload();
////        }
////    });
////});

//$($($($(top.window.document?.getElementById('ctl00_C5POBody_EncounterContainer')?.contentWindow?.document?.querySelector('#myTabs .active'))[0].parentElement.parentElement.children[1]).find(".active")[0].children[0])[0].contentWindow?.document.getElementById("radBrowser")).each(function () {
//    $(this.contentWindow).bind({
//        focus: function () { },
//        blur: function () {
//            RcopiaDownload();
//        }
//    });
//});
