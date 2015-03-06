define('app/components/hat/viewModel', ['ko', 'text!app/components/hat/template.html'], function (ko, htmlString) {

    function Hat(model) {
        var self = this;
    };

    return {
        viewModel: Hat,
        template: htmlString
    };
});