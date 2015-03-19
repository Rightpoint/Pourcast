
// vim: ts=4 sw=4 noexpandtab ai

var fs = require('fs');
require('./takePicture.js')(fs.createWriteStream('result.png'), 1000);
