//CAP-413 Restrict the Capella EHR access in multiple tabs

const channel = new BroadcastChannel('tab');
let isOriginal = true;

channel.postMessage('another-tab');
// note that listener is added after posting the message

channel.addEventListener('message', (msg) => {

    var currentURL = window.location.href;

    if (currentURL.match(/[?&]allowmultipletab=([^&]+)/) != undefined && currentURL.match(/[?&]allowmultipletab=([^&]+)/) != null) {
        return;
    }

    if (msg.data === 'another-tab' && isOriginal) {
        // message received from 2nd tab
        // reply to all new tabs that the website is already open
        channel.postMessage('already-open');
    }
    if (msg.data === 'already-open') {
        isOriginal = false;
        // message received from original tab
        // replace this with whatever logic you need
        window.location.replace("frmRestrictMultipleTabs.aspx");
    }
});