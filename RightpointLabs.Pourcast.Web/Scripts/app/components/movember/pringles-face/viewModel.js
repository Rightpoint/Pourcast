define(['ko', 'text!app/components/pringles-face/template.html'], function (ko, htmlString) {

    function PringlesFace(model) {
        var self = this;
    };

    return {
        viewModel: PringlesFace,
        template: htmlString
    };
});