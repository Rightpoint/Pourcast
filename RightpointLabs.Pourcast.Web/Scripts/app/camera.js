define(['app/events'], function (events) {

    if (window.location.href.indexOf("camera=true") < 0)
        return;

    navigator.webkitGetUserMedia({ video: true }, function (s) {
        events.on("PourStarted", function (e) {
            console.log("Loading video object...");
            var v = document.createElement("video");
            var c = document.createElement("canvas");
            $("body").append(v);
            $(v).css({ opacity: 0, position: 'absolute' });
            v.src = URL.createObjectURL(s);
            v.play();
            setTimeout(function () {
                console.log("Taking shot...");
                c.width = v.clientWidth;
                c.height = v.clientHeight;

                var ctx = c.getContext("2d");
                ctx.drawImage(v, 0, 0);

                var data = c.toDataURL('image/jpeg');

                $.post("/api/picture/Taken", { '': data });

                $(v).remove();
                console.log("Cleaning up...");
            }, 500);
        });
    }, function () { console.log("failed to initialize camera"); });

});