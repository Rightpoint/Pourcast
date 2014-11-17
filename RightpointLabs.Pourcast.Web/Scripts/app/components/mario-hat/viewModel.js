define(['ko', 'text!app/components/mario-hat/template.html'], function (ko, htmlString) {

    function MarioHat(model) {
        var self = this;
    };

    return {
        viewModel: MarioHat,
        template: htmlString
    };
});