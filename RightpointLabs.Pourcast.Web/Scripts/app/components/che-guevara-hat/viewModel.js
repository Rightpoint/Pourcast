define(['ko', 'text!app/components/che-guevara-hat/template.html'], function (ko, htmlString) {

    function CheGuevaraHat(model) {
        var self = this;
    };

    return {
        viewModel: CheGuevaraHat,
        template: htmlString
    };
});