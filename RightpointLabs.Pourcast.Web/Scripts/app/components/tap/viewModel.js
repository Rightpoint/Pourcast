define(['ko', 'text!app/components/tap/template.html'], function (ko, htmlString) {
    
    function Tap(model) {
        var self = this;
    };

    return {
        viewModel: Tap,
        template: htmlString
    };
});