define(['ko', 'text!app/components/flanders-keg-body/template.html'], function (ko, htmlString) {

    function FlandersKegBody(model) {
        var self = this;
    };

    return {
        viewModel: FlandersKegBody,
        template: htmlString
    };
});