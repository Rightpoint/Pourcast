define(['ko', 'text!app/components/zappa-face/template.html'], function (ko, htmlString) {

    function ZappaFace(model) {
        var self = this;
    };

    return {
        viewModel: ZappaFace,
        template: htmlString
    };
});