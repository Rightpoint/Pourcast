define(['ko', 'text!app/components/monopoly-cane/template.html'], function (ko, htmlString) {

    function MonopolyCane(model) {
        var self = this;
    };

    return {
        viewModel: MonopolyCane,
        template: htmlString
    };
});