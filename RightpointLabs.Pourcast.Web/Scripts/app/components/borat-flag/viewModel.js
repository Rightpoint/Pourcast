define(['ko', 'text!app/components/borat-flag/template.html'], function (ko, htmlString) {

    function BoratFlag(model) {
        var self = this;
    };

    return {
        viewModel: BoratFlag,
        template: htmlString
    };
});