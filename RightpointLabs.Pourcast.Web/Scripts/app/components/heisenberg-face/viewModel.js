define(['ko', 'text!app/components/heisenberg-face/template.html'], function (ko, htmlString) {

    function HeisenbergFace(model) {
        var self = this;
    };

    return {
        viewModel: HeisenbergFace,
        template: htmlString
    };
});