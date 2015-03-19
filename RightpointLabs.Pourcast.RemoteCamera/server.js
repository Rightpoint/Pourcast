// vim: ts=4 sw=4 noexpandtab ai

// modules
var express = require('express')
  , http = require('http')
  , fs = require('fs')
  , signalR = require('signalr-client')
  , MemoryStream = require('memory-stream')
  , takePicture = require('./takePicture.js')
  , stream = require('stream');

var client = new signalR.client('http://192.168.25.141:23456/signalR', ['eventsHub']);
client.on('eventsHub', 'PictureRequested', function() {
	console.log('got request for a picture');

	var pic2 = fs.createWriteStream('temp.png');
	pic2.on('finish', function() {
		var dataUrl = require('dataurl').convert({
			data: fs.readFileSync('temp.png'),
			mimetype: 'image/png'
		});

		console.log(dataUrl);
		console.log('uploading - url length: ' + dataUrl.length);
		require('./postPicture.js')('192.168.25.141', '23456', '/api/picture/Taken?tapId=', dataUrl);
		console.log('uploaded');
	});
	takePicture(pic2, 1000);
});

// app parameters
var app = express();
app.set('port', 8080);
app.use(express.static('.'));

app.get('/', function(req, res) {
	res.send('<img src="newPicture.png"/>');
});
app.get('/newPicture.png', function(req, res) {
	var takePicture = require('./takePicture.js');
	takePicture(res,1000);
});

// HTTP server
http.createServer(app).listen(app.get('port'), function () {
  console.log('HTTP server listening on port ' + app.get('port'));
});

module.exports.app = app;
