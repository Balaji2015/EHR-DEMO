var sIsEnableLogRocket = (document?.cookie?.slice(document?.cookie?.indexOf("CIsEnableLogRocket"))?.split(";")[0]?.split("=")[1]) ?? "";
if (sIsEnableLogRocket == "Y") {
    var IsSanitizedAll = false;
    var IsWithoutMergeIframe = false;
    if (document?.currentScript?.src?.indexOf("JsLogRocket") != undefined &&
        document?.currentScript?.src?.indexOf("JsLogRocket") > -1) {

        var Requestparam = new URLSearchParams(document.currentScript.src.split("?")[1]);

        if (Requestparam != null && Requestparam != undefined
            && Requestparam?.get("IsWithoutMergeIframe") != null && Requestparam?.get("IsWithoutMergeIframe") == "Y") {
            IsWithoutMergeIframe = true;
        }

        if (Requestparam != null && Requestparam != undefined
            && Requestparam?.get("IsSanitizedAll") != null && Requestparam?.get("IsSanitizedAll") == "Y") {
            IsSanitizedAll = true;
        }

    }
    const script = document.createElement('script');
    script.src = 'https://cdn.lgrckt-in.com/LogRocket.min.js';
    script.onload = () => {

        var sUsername = (document?.cookie?.slice(document?.cookie?.indexOf("CUserName"))?.split(";")[0]?.split("=")[1]) ?? "";
        var sPersonname = (document?.cookie?.slice(document?.cookie?.indexOf("CPersonName"))?.split(";")[0]?.split("=")[1]) ?? "";
        var sEMailAddress = (document?.cookie?.slice(document?.cookie?.indexOf("CEMailAddress"))?.split(";")[0]?.split("=")[1]) ?? "";
        var sLogRocketClientID = (document?.cookie?.slice(document?.cookie?.indexOf("CLogRocketClientID"))?.split(";")[0]?.split("=")[1]) ?? "";
        var sVersion = (document?.cookie?.slice(document?.cookie?.indexOf("CVersion"))?.split(";")[0]?.split("=")[1]) ?? "";
        var BlockListClassNames = ['class-hide-this'];

        if (IsWithoutMergeIframe == true) {
            LogRocket.init(sLogRocketClientID,
                {
                    release: sVersion,
                    dom: {
                        privateClassNameBlocklist: BlockListClassNames,
                    },
                });
        }
        else if (IsSanitizedAll == true) {
            LogRocket.init(sLogRocketClientID,
                {
                    release: sVersion,
                    dom: {
                        inputSanitizer: true,
                        textSanitizer: true,
                        privateClassNameBlocklist: BlockListClassNames,
                    },
                });
        }
        else {
            LogRocket.init(sLogRocketClientID,
                {
                    mergeIframes: true,
                    parentDomain: "https://" + window.location.host,
                    release: sVersion,
                    dom: {
                        privateClassNameBlocklist: BlockListClassNames,
                    },
                });
        }

        LogRocket.identify(sUsername ?? "", {
            name: sPersonname ?? "",
            email: sEMailAddress ?? "",
        });

    };
    document.head.appendChild(script);
}