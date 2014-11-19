define(['ko', 'text!app/components/chaplin-hat/template.html'], function (ko, htmlString) {

    function MarioHat(model) {
        var self = this;
    };

    return {
        viewModel: MarioHat,
        template: htmlString
    };
});