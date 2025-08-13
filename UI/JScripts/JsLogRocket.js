var sIsEnableLogRocket = (document?.cookie?.slice(document?.cookie?.indexOf("CIsEnableLogRocket"))?.split(";")[0]?.split("=")[1]) ?? "";
if (sIsEnableLogRocket == "Y") {
    const script = document.createElement('script');
    script.src = 'https://cdn.lgrckt-in.com/LogRocket.min.js';
    script.onload = () => {
        document.head.appendChild(script);

        var sUsername = (document?.cookie?.slice(document?.cookie?.indexOf("CUserName"))?.split(";")[0]?.split("=")[1]) ?? "";
        var sPersonname = (document?.cookie?.slice(document?.cookie?.indexOf("CPersonName"))?.split(";")[0]?.split("=")[1]) ?? "";
        var sEMailAddress = (document?.cookie?.slice(document?.cookie?.indexOf("CEMailAddress"))?.split(";")[0]?.split("=")[1]) ?? "";
        var sLogRocketClientID = (document?.cookie?.slice(document?.cookie?.indexOf("CLogRocketClientID"))?.split(";")[0]?.split("=")[1]) ?? "";
        var sVersion = (document?.cookie?.slice(document?.cookie?.indexOf("CVersion"))?.split(";")[0]?.split("=")[1]) ?? "";

        LogRocket.init(sLogRocketClientID,
            {
                mergeIframes: true,
                parentDomain: window.location.host,
                release: sVersion,
            });

        LogRocket.identify(sUsername ?? "", {
            name: sPersonname ?? "",
            email: sEMailAddress ?? "",
        });

    };
    document.head.appendChild(script);
}