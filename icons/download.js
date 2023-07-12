#!/usr/bin/env node
const crypto = require("crypto");
const fs = require("fs");
const https = require("https");
const path = require("path");
const sharp = require("sharp");

const icons = {
    "https://static.wikia.nocookie.net/ikariam/images/c/c0/Town_hall_l.png": [
        [
            "firefox/icon-48.png",
            {
                "type": "resize",
                "opts": {
                    "width": 48,
                    "height": 48,
                    "fit": "cover",
                    "position": "left"
                }
            }
        ],
        [
            "firefox/icon-96.png",
            {
                "type": "resize",
                "opts": {
                    "width": 96,
                    "height": 96,
                    "fit": "cover",
                    "position": "left"
                }
            }
        ]
    ]
};

Object.getOwnPropertyNames(icons).forEach(url => {
    const cacheFile = path.join("cache", crypto.createHash("sha256").update(url).digest("hex") + path.extname(url));
    fs.access(cacheFile, fs.constants.R_OK, err => {
        const processFile = () => {
            fs.stat(__filename, (err, selfStats) => {
                if (err) {
                    console.error(err);
                } else {
                    for (let i = 0; i < icons[url].length; ++i) {
                        const proc = icons[url][i];
                        const outputFile = path.join("..", proc[0]);
                        fs.stat(outputFile, (err, stats) => {
                            if (err || stats.mtime.getTime() < selfStats.mtime.getTime()) {
                                const img = sharp(cacheFile);
                                for (let j = 1; j < proc.length; ++j) {
                                    const step = proc[j];
                                    switch (step.type) {
                                        case "resize":
                                            img.resize(step.opts);
                                            break;
                                        default:
                                            console.error(`Unknown processing step '${step.type}'`);
                                            break;
                                    }
                                }
                                img.toFile(outputFile, err => {
                                    if (err) {
                                        console.error(err);
                                    } else {
                                        console.log(`Generated ${outputFile}`);
                                    }
                                });
                            }
                        });
                    }
                }
            });
        };
        if (err) {
            const file = fs.createWriteStream(cacheFile);
            https.get(url, resp => {
                resp.pipe(file);
                file.on("finish", () => {
                    file.close(() => {
                        console.log(`Downloaded ${url}`);
                        processFile();
                    });
                });
            }).on("error", err => {
                fs.unlink(cacheFile);
                console.error(err);
            });
        } else {
            processFile();
        }
    });
});
