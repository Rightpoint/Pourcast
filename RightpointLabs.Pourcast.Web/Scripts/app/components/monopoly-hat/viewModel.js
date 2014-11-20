define(['ko', 'text!app/components/monopoly-hat/template.html'], function (ko, htmlString) {

    function MonopolyHat(model) {
        var self = this;
    };

    return {
        viewModel: MonopolyHat,
        template: htmlString
    };
});