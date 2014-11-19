define(['ko', 'text!app/components/zorro-hat/template.html'], function (ko, htmlString) {

    function ZorroHat(model) {
        var self = this;
    };

    return {
        viewModel: ZorroHat,
        template: htmlString
    };
});