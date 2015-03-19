// vim: ts=4 sw=4 noexpandtab ai
var takePicture = require('./takePicture.js');

takePicture(fs.createWriteStream('result.png'));
