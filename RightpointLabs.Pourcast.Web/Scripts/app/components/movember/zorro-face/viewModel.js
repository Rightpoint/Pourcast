define(['ko', 'text!app/components/zorro-face/template.html'], function (ko, htmlString) {

    function ZorroFace(model) {
        var self = this;
    };

    return {
        viewModel: ZorroFace,
        template: htmlString
    };
});