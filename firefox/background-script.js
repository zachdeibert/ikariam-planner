browser.webRequest.onCompleted.addListener(ev => {
    browser.tabs.sendMessage(ev.tabId, "ikariam-planner-update");
}, {
    "urls": [
        "https://*.ikariam.gameforge.com/?view=*"
    ],
    "types": [
        "main_frame",
        "xmlhttprequest"
    ]
});
