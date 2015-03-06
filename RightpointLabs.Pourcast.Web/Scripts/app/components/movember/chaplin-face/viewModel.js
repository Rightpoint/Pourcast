define(['ko', 'text!app/components/chaplin-face/template.html'], function (ko, htmlString) {

    function ChaplinFace(model) {
        var self = this;
    };

    return {
        viewModel: ChaplinFace,
        template: htmlString
    };
});