define(['ko', 'text!app/components/luigi-hat/template.html'], function (ko, htmlString) {

    function LuigiHat(model) {
        var self = this;
    };

    return {
        viewModel: LuigiHat,
        template: htmlString
    };
});