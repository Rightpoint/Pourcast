define('app/components/keg-body/viewModel', ['ko', 'app/events', 'app/dataService'], function (ko, events, dataService) {
    
    function KegBody(model) {
        var self = this;
    };

    return {
        viewModel: KegBody,
        template: { element: 'keg-body-template' }
    };
});