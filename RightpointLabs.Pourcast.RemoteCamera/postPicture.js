// vim: ts=4 sw=4 noexpandtab ai
var querystring = require('querystring');
var http = require('http');
var fs = require('fs');

module.exports = function(host, port, path, data) {
	var postData = querystring.stringify({ '': data });
	var options = {
		host: host,
		port: port || '80',
		path: path,
		method: 'POST',
		headers: {
			'Content-Type': 'application/x-www-form-urlencoded',
			'Content-Length': postData.length
		}
	};

	var post = http.request(options, function(res) {
		res.setEncoding('utf8');
		res.on('data', function(chunk) {
			console.log('Response: ' + chunk);
		});
	});
	post.write(postData);
	post.end();
};
