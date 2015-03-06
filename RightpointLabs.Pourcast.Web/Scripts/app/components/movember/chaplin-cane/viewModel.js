define(['ko', 'text!app/components/chaplin-cane/template.html'], function (ko, htmlString) {

    function ChaplinCane(model) {
        var self = this;
    };

    return {
        viewModel: ChaplinCane,
        template: htmlString
    };
});