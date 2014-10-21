define(['ko', 'text!app/components/tap/template.html'], function (ko, htmlString) {
    
    function Tap(model) {
        var self = this;

        self.resolver = model.resolver;
    };

    return {
        viewModel: Tap,
        template: htmlString
    };
});