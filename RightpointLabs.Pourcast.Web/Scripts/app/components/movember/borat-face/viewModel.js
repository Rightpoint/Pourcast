define(['ko', 'text!app/components/borat-face/template.html'], function (ko, htmlString) {

    function BoratFace(model) {
        var self = this;
    };

    return {
        viewModel: BoratFace,
        template: htmlString
    };
});