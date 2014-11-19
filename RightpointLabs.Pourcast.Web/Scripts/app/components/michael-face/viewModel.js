define(['ko', 'text!app/components/michael-face/template.html'], function (ko, htmlString) {
    
    function MichaelFace(model) {
        var self = this;
    };

    return {
        viewModel: MichaelFace,
        template: htmlString
    };
});