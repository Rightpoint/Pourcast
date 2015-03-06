define(['ko', 'text!app/components/richie-face/template.html'], function (ko, htmlString) {

    function RichieFace(model) {
        var self = this;
    };

    return {
        viewModel: RichieFace,
        template: htmlString
    };
});