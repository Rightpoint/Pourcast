define(['ko', 'text!app/components/hat/template.html'], function (ko, htmlString) {

    function Hat(params) {
        var self = this;

        self.params = params;
    };

    return {
        viewModel: Hat,
        template: htmlString
    };
});