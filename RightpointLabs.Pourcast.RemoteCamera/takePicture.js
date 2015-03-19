// vim: ts=4 sw=4 noexpandtab ai
var v4l = require('v4l2camera');
var fs = require('fs');
var pngjs = require('pngjs');
var yuyv2rgb = require('./yuyv2rgb.js');

module.exports = function(outPipe, delay, callback) {
	delay = delay || 0;
	var cam = new v4l.Camera('/dev/video1');
	var size = cam.formats.reduce(function(a, v) {
		if(v.width * v.height > a.width * a.height) {
			return { width: v.width, height: v.height };
		}
		return a;
	}, { width: 0, height: 0});
//	size = { width: 640, height: 480 };
//	size = { width: 160, height: 120 };
	cam.configSet(size);
	console.log(cam.configGet());
	cam.start();
	cam.capture(function(success) {
		setTimeout(function() {
			var yuyv = cam.toYUYV();
			console.log('got yuyv, converting');
			var rgb = yuyv2rgb(yuyv, cam.width, cam.height);
			console.log('got rgb, saving');
			var img = new pngjs.PNG({width: cam.width, height: cam.height});
			var size = img.width * img.height;
			for(var i=0; i < size; i++) {
				img.data[i * 4 + 0] = rgb[i * 3 + 0];
				img.data[i * 4 + 1] = rgb[i * 3 + 1];
				img.data[i * 4 + 2] = rgb[i * 3 + 2];
				img.data[i * 4 + 3] = 255;;
			}
			console.log('encoding');
			img.pack().pipe(outPipe);
			console.log('stopping');
			cam.stop();
			console.log('wrote to outPipe');
			if(callback) {
				callback();
			}
		}, delay);
	});
};

