// vim: ts=4 sw=4 noexpandtab ai

var fs = require('fs');

var data = fs.readFileSync('result.png');
var dataUrl = require('dataurl').convert({
    data: data,
    mimetype: 'image/png'
});
console.log(dataUrl);
console.log('uploading - url length: ' + dataUrl.length);

require('./postPicture.js')('192.168.25.141', '23456', '/api/picture/Taken?tapId=', dataUrl);
