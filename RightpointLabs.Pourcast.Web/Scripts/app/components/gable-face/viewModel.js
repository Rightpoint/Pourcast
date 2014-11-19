define(['ko', 'text!app/components/gable-face/template.html'], function (ko, htmlString) {

    function GableFace(model) {
        var self = this;
    };

    return {
        viewModel: GableFace,
        template: htmlString
    };
});