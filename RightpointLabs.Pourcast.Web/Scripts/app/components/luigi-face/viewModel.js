define(['ko', 'text!app/components/luigi-face/template.html'], function (ko, htmlString) {

    function LuigiFace(model) {
        var self = this;
    };

    return {
        viewModel: LuigiFace,
        template: htmlString
    };
});