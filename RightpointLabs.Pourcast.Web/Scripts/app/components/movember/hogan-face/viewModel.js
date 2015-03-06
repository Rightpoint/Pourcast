define(['ko', 'text!app/components/hogan-face/template.html'], function (ko, htmlString) {

    function HoganFace(model) {
        var self = this;
    };

    return {
        viewModel: HoganFace,
        template: htmlString
    };
});