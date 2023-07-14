browser.webRequest.onCompleted.addListener(ev => {
    browser.tabs.query({
        "active": true
    }).then(tabs => {
        if (tabs.findIndex(tab => tab.id == ev.tabId) >= 0) {
            browser.tabs.sendMessage(ev.tabId, "ikariam-planner-update");
        }
    });
}, {
    "urls": [
        "https://*.ikariam.gameforge.com/?view=*"
    ],
    "types": [
        "main_frame",
        "xmlhttprequest"
    ]
});

browser.tabs.onActivated.addListener(ev => {
    browser.tabs.query({
        "active": true
    }).then(tabs => {
        if (tabs.findIndex(tab => tab.id == ev.tabId) >= 0) {
            browser.tabs.sendMessage(ev.tabId, "ikariam-planner-update");
        }
    });
});
