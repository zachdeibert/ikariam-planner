const serializer = new XMLSerializer();
const parser = new DOMParser();

const removeTags = [
    "SCRIPT",
    "STYLE"
];
const keepAttributes = [
    "class",
    "id",
    "max",
    "value"
];

function safeForEach(collection, callback) {
    const list = [];
    for (let i = 0; i < collection.length; ++i) {
        list.push(collection.item(i));
    }
    list.forEach(callback);
}

function strip(node) {
    safeForEach(node.childNodes, child => {
        switch (child.nodeType) {
            case Node.ELEMENT_NODE:
                if (removeTags.indexOf(child.nodeName) >= 0) {
                    child.parentNode.removeChild(child);
                } else {
                    safeForEach(child.attributes, attr => {
                        if (keepAttributes.indexOf(attr.name) < 0) {
                            child.removeAttribute(attr.name);
                        }
                    });
                    strip(child);
                }
                break;
            case Node.TEXT_NODE:
                child.nodeValue = child.nodeValue.trim();
                break;
            case Node.COMMENT_NODE:
                child.parentNode.removeChild(child);
                break;
            default:
                break;
        }
    });
}

function uploadDOM() {
    const doc = parser.parseFromString(document.body.outerHTML, "text/html");
    strip(doc.getRootNode());
    fetch(`http://localhost:5357/ikariam-planner/${location.host.split(".")[0]}/${location.search}`, {
        "method": "POST",
        "body": serializer.serializeToString(doc)
    });
}

window.addEventListener("load", () => {
    uploadDOM();
});

browser.runtime.onMessage.addListener(req => {
    if (req === "ikariam-planner-update") {
        uploadDOM();
    }
});
