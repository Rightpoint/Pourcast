define(['ko', 'text!app/components/pourcast/template.html'], function (ko, htmlString) {

    function Pourcast(model) {
        var self = this;

        self.taps = ko.computed(function() {
            return model.taps();
        });
    };

    return {
        viewModel: Pourcast,
        template: htmlString
    };
});