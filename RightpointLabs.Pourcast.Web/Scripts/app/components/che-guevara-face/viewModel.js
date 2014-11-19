define(['ko', 'text!app/components/che-guevara-face/template.html'], function (ko, htmlString) {

    function CheGuevaraFace(model) {
        var self = this;
    };

    return {
        viewModel: CheGuevaraFace,
        template: htmlString
    };
});