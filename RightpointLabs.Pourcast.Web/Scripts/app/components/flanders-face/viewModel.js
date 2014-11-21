define(['ko', 'text!app/components/flanders-face/template.html'], function (ko, htmlString) {

    function FlandersFace(model) {
        var self = this;
    };

    return {
        viewModel: FlandersFace,
        template: htmlString
    };
});