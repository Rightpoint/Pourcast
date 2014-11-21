define(['ko', 'text!app/components/hogan-keg-body/template.html'], function (ko, htmlString) {

    function HoganKegBody(model) {
        var self = this;
    };

    return {
        viewModel: HoganKegBody,
        template: htmlString
    };
});