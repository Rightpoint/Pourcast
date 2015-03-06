define(['ko', 'text!app/components/varnon-face/template.html'], function (ko, htmlString) {
    
    function VarnonFace(model) {
        var self = this;
    };

    return {
        viewModel: VarnonFace,
        template: htmlString
    };
});