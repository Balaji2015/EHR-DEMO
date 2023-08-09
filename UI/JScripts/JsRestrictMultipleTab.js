////CAP-413 Restrict the Capella EHR access in multiple tabs
//if (!localStorage.getItem('_instance') || localStorage.getItem('_instance') && !localStorage.getItem('newtab')) {
//    // Set a key in the local storage to mark this as the first instance
//    localStorage.setItem('_instance', 'true');
//    localStorage.setItem('newtab', 'false');

//    // Add an event listener to detect when the user closes the tab or navigates away
//    window.addEventListener('beforeunload', function () {
//        // Remove the key from the local storage when the user leaves the page
//        localStorage.removeItem('_instance');
//        localStorage.removeItem('newtab');
//    });

//    // Add an event listener to detect when the user closes the tab or navigates away
//    window.addEventListener('unload', function () {
//        // Remove the key from the local storage when the user leaves the page
//        localStorage.removeItem('_instance');
//        localStorage.removeItem('newtab');
//    });
//}
//else {
//    localStorage.setItem('newtab', 'true');
//    // If the key is already present in the local storage, it means another instance of the page is open.
//    // Redirect the current page to a different URL to prevent multiple instances.
//    window.location.replace("frmRestrictMultipleTabs.aspx");
//}


////if (document.cookie.indexOf("_instance=true") === -1) {
////    document.cookie = "_instance=true";
////    window.onunload = function () {
////        document.cookie = "_instance=true;expires=Thu, 01-Jan-1970 00:00:01 GMT";
////    };
////}
////else {
////    var win = window.open("frmRestrictMultipleTabs.aspx", "_self"); win.close();
////}

////if (!sessionStorage.getItem('_instance')) {
////    // Set a key in the session storage to mark this as the first instance
////    sessionStorage.setItem('_instance', 'true');


////CAP-413 Restrict the Capella EHR access in multiple tabs
//// Check if the page is the first instance by looking for a specific key in the local storage
////if (!localStorage.getItem('_instance')) {
////    // Set a key in the local storage to mark this as the first instance
////    localStorage.setItem('_instance', 'true');

////    // Add an event listener to detect when the user closes the tab or navigates away
////    window.addEventListener('beforeunload', function () {
////        // Remove the key from the local storage when the user leaves the page
////        localStorage.removeItem('_instance');
////    });

////    window.addEventListener('unload', function () {
////        // Remove the key from the local storage when the user leaves the page
////        localStorage.removeItem('_instance');
////    });
////}
////else {
////    // If the key is already present in the local storage, it means another instance of the page is open.
////    // Redirect the current page to a different URL to prevent multiple instances.
////    window.location.replace("frmRestrictMultipleTabs.aspx");
////}
////if (!localStorage.getItem('_instance') || localStorage.getItem('_instance') && !localStorage.getItem('newtab')){
////    // Set a key in the local storage to mark this as the first instance
////    localStorage.setItem('_instance', true);
////    localStorage.setItem('newtab', false);
////    localStorage.removeItem('isDuplicate');

////    // Add an event listener to detect when the user closes the tab or navigates away
////    window.addEventListener('beforeunload', function () {
////        // Remove the key from the local storage when the user leaves the page
////        localStorage.removeItem('_instance');
////        localStorage.removeItem('newtab');
////        localStorage.removeItem('isDuplicate');

////    });

////     // Add an event listener to detect when the user closes the tab or navigates away
////    window.addEventListener('unload', function () {
////        // Remove the key from the local storage when the user leaves the page
////        localStorage.removeItem('_instance');
////        localStorage.removeItem('newtab');
////        localStorage.removeItem('isDuplicate');
////    });
////}
////else if (localStorage.getItem('_instance') && localStorage.getItem('newtab') == "true" && !localStorage.getItem('isDuplicate')) {
////    localStorage.removeItem('newtab', false);
////    window.location.replace("frmLogin.aspx");
////}
////else {
////    localStorage.setItem('newtab', true);
////    // If the key is already present in the local storage, it means another instance of the page is open.
////    // Redirect the current page to a different URL to prevent multiple instances.
////    window.location.replace("frmRestrictMultipleTabs.aspx");
////}

////Below code block is to completly restrict the incognito window as part of CAP-413, it is decided to not restrict
////the incognito window. So, commenting the below code block.
////function isIncognitoMode() {
////    if (navigator.storage && navigator.storage.estimate && performance && performance.memory) {
////        return navigator.storage.estimate().then(function (data) {
////            return data.quota < performance.memory.jsHeapSizeLimit;
////        });
////    } else {
////        return Promise.resolve(false); // Not supported or unable to detect
////    }
////}
////isIncognitoMode().then(function (result) {
////    if (result) {
////        var win = window.open("frmRestrictMultipleTab.aspx", "_self"); win.close();
////    }
////});