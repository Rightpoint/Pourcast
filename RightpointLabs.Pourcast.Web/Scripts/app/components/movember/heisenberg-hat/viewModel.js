define(['ko', 'text!app/components/heisenberg-hat/template.html'], function (ko, htmlString) {

    function HeisenbergHat(model) {
        var self = this;
    };

    return {
        viewModel: HeisenbergHat,
        template: htmlString
    };
});