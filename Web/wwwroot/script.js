var cn = new signalR.HubConnection('events');

cn.on('RecieveEvent', function (data) {
    console.log('test', data);
    var div = document.createElement("div");
    var pre = document.createElement("pre");
    var code = document.createElement("code");
    code.innerText = JSON.stringify(data);

    pre.appendChild(code);
    div.appendChild(pre);
    var output = document.getElementById("output");
    output.insertBefore(div, output.firstChild);
});

cn.start();