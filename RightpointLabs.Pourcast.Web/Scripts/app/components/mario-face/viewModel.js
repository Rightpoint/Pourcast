define(['ko', 'text!app/components/mario-face/template.html'], function (ko, htmlString) {

    function MarioFace(model) {
        var self = this;
    };

    return {
        viewModel: MarioFace,
        template: htmlString
    };
});