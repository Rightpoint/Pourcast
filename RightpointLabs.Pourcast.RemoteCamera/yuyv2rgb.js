var minmax = function (min, v, max) {
    return (v < min) ? min : (max < v) ? max : v;
};
var yuv2r = function (y, u, v) {
    return minmax(0, (y + 359 * v) >> 8, 255);
};
var yuv2g = function (y, u, v) {
    return minmax(0, (y + 88 * v - 183 * u) >> 8, 255);
};
var yuv2b = function (y, u, v) {
    return minmax(0, (y + 454 * u) >> 8, 255);
};
var yuyv2rgb = function (yuyv, width, height) {
    var rgb = new Array(width * height * 3);
    for (var i = 0; i < height; i++) {
        for (var j = 0; j < width; j += 2) {
            var index = i * width + j;
            var y0 = yuyv[index * 2 + 0] << 8;
            var u = yuyv[index * 2 + 1] - 128;
            var y1 = yuyv[index * 2 + 2] << 8;
            var v = yuyv[index * 2 + 3] - 128;
            rgb[index * 3 + 0] = yuv2r(y0, u, v);
            rgb[index * 3 + 1] = yuv2g(y0, u, v);
            rgb[index * 3 + 2] = yuv2b(y0, u, v);
            rgb[index * 3 + 3] = yuv2r(y1, u, v);
            rgb[index * 3 + 4] = yuv2g(y1, u, v);
            rgb[index * 3 + 5] = yuv2b(y1, u, v);
        }
    }
    return rgb;
};

module.exports = yuyv2rgb;
