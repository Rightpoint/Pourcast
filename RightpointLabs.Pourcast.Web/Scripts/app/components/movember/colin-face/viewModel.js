define(['ko', 'text!app/components/colin-face/template.html'], function (ko, htmlString) {

    function ColinFace(model) {
        var self = this;
    };

    return {
        viewModel: ColinFace,
        template: htmlString
    };
});