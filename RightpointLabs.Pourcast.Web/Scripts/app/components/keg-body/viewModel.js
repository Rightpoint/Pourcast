define(['ko', 'app/events', 'app/dataService', 'text!app/components/keg-body/template.html'], function (ko, events, dataService, htmlString) {
    
    function KegBody(model) {
        var self = this;
    };

    return {
        viewModel: KegBody,
        template: htmlString
    };
});