define(['app/events', 'jquery'], function (events, $) {

    if (window.location.href.indexOf("camera=true") < 0)
        return;

    function getVideoStream() {
        var d = $.Deferred();
        navigator.webkitGetUserMedia({ video: true }, function (s) {
            d.resolve(s);
        }, function() {
            d.reject("Failed to initialize camera");
        });
        return d.promise();
    }
    function delay(ms, value) {
        var d = $.Deferred();
        setTimeout(function() {
            d.resolve(value);
        }, ms);
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
    function takeShot(v) {
        var c = document.createElement("canvas");
        console.log("Taking shot...");
        c.width = v.clientWidth;
        c.height = v.clientHeight;

        var ctx = c.getContext("2d");
        ctx.drawImage(v, 0, 0);

        return c.toDataURL('image/jpeg');
    }

    var secure = window.location.protocol === "https:";
    var acquirePicture = (secure ? function () {
        // this is SSL, so the allow will be remembered, which means we can do cleanup after each shot

        // get it once first so that it'll prompt the user right away if we don't have the rights yet
        getVideoStream().then(function(s) {
            s.stop();
        });

        return function () {
            return getVideoStream().then(function (s) {
                var v = setupVideo(s);
                return delay(500, { v: v, s: s });
            }).then(function (obj) {
                var v = obj.v;
                var s = obj.s;

                var data = takeShot(v);

                v.pause();
                v.src = null;
                $(v).remove();
                s.stop();
                return data;
            });
        };
    } : function () {
        // this is *not* SSL, so the allow won't be remembered - we can't do cleanup after each shot
        var stream = getVideoStream();

        return function() {
            return stream.then(function (s) {
                var v = setupVideo(s);
                return delay(500, v);
            }).then(function (v) {
                console.log(v);
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
                $.post("/api/picture/Taken?tapId=" + tId, { '': data });
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
});