define(['ko', 'text!app/components/yosemite-face/template.html'], function (ko, htmlString) {

    function YosemiteFace(model) {
        var self = this;
    };

    return {
        viewModel: YosemiteFace,
        template: htmlString
    };
});