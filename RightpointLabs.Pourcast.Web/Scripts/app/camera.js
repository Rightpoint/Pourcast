define('app/camera', ['app/events', 'jquery'], function (events, $) {

    if (window.location.href.indexOf("camera=true") < 0)
        return;

    function getVideoStream() {
        var d = $.Deferred();

        // try a series of options to get us the best picture we can
        var options = [
            { video: { mandatory: { minWidth: 2304, minHeight: 1296 } } },
            { video: { mandatory: { minWidth: 2304, minHeight: 1536 } } },
            { video: { mandatory: { minWidth: 1920, minHeight: 1080 } } },
            { video: { mandatory: { minWidth: 1280, minHeight: 720 } } },
            { video: { mandatory: { minWidth: 2048, minHeight: 1536 } } },
            { video: { mandatory: { minWidth: 1024, minHeight: 768 } } },
            { video: { mandatory: { minWidth: 640, minHeight: 480 } } },
            { video: true }
        ];

        function tryOne() {
            var opt = options.shift();
            if (opt) {
                console.log(opt);
                navigator.webkitGetUserMedia(opt, function (s) {
                    console.log("Opened camera with args", opt);
                    d.resolve(s);
                }, function () {
                    tryOne();
                });
            } else {
                d.reject("Failed to initialize camera");
            }
        }

        tryOne();

        return d.promise();
    }
    function setupVideo(stream) {
        var v = document.createElement("video");
        $(v).css({ 'z-index': -9999, opacity: 0, position: 'absolute', top: '-9999px', left: '-9999px' });
        $("body").append(v);
        v.src = URL.createObjectURL(stream);
        v.play();
        return v;
    }
    function waitForVideoReady(v, retVal, ms, iter) {
        if (!ms) ms = 100;
        if (!iter) iter = 20;

        var d = $.Deferred();

        if (v.clientHeight > 150) {
            setTimeout(function () {
                d.resolve(retVal);
            }, 500);
        } else {
            var interval = setInterval(function () {
                if (v.clientHeight > 150) {
                    console.log("Found with " + iter + " left");
                    clearInterval(interval);
                    setTimeout(function() {
                        d.resolve(retVal);
                    }, 500);
                } else {
                    iter--;
                    if (iter <= 0) {
                        clearInterval(interval);
                        d.reject("failed to initialize camera");
                    }
                }
            }, ms);
        }

        return d.promise();
    }
    function takeShot(v) {
        var c = document.createElement("canvas");
        console.log("Taking shot...");
        c.width = v.clientWidth;
        c.height = v.clientHeight;

        var ctx = c.getContext("2d");
        ctx.drawImage(v, 0, 0);

        return c.toDataURL('image/png');
    }

    var secure = window.location.protocol === "https:";
    var acquirePicture = (secure ? function () {
        // this is SSL, so the allow will be remembered, which means we can do cleanup after each shot

        // so long as we're taking a picture at startup, that will trigger the prompt
        //getVideoStream().then(function(s) {
        //    s.stop();
        //});

        return function () {
            return getVideoStream().then(function (s) {
                var v = setupVideo(s);
                return waitForVideoReady(v, { v: v, s: s }, 100, 30);
            }).then(function (obj) {
                var v = obj.v;
                var s = obj.s;

                var data = takeShot(v);

                v.pause();
                v.src = '';
                $(v).remove();
                if (s.stop) { // .stop() was deprecated in Chrome 45, removed in Chrome 47
                    s.stop();
                } else {
                    s.getTracks()[0].stop();
                }
                return data;
            });
        };
    } : function () {
        // this is *not* SSL, so the allow won't be remembered - we can't do cleanup after each shot
        var stream = getVideoStream();

        return function() {
            return stream.then(function (s) {
                var v = setupVideo(s);
                return waitForVideoReady(v, v, 100, 30);
            }).then(function (v) {
                var data = takeShot(v);

                $(v).remove();

                return data;
            });
        };
    })();


    var pendingPics = null;
    function takePicture(tapId) {
        if (pendingPics) {
            pendingPics.push(tapId);
            return;
        }

        pendingPics = [tapId];
        acquirePicture().then(function(data) {
            for (var i in pendingPics) {
                var tId = pendingPics[i];
                var url = "/api/picture/Taken?tapId=" + (tId || "");
                console.log(url);
                $.post(url, { '': data });
            }
        }).done(function () {
            pendingPics = null;
        });
    };

    // test out the camera when launching the app
    takePicture(null);
    events.on("PourStarted", function (e) {
        takePicture(e.TapId);
    });
    events.on("PictureRequested", function (e) {
        takePicture(null);
    });
});