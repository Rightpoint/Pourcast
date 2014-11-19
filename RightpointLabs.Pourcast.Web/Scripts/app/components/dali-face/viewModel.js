define(['ko', 'text!app/components/dali-face/template.html'], function (ko, htmlString) {

    function DaliFace(model) {
        var self = this;
    };

    return {
        viewModel: DaliFace,
        template: htmlString
    };
});