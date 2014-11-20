define(['ko', 'text!app/components/monopoly-face/template.html'], function (ko, htmlString) {

    function MonopolyFace(model) {
        var self = this;
    };

    return {
        viewModel: MonopolyFace,
        template: htmlString
    };
});