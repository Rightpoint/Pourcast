define('app/components/tap/viewModel', ['ko', 'app/componentResolver'], function (ko, ComponentResolver) {
    
    function Tap(model) {
        var self = this;

        self.hasKeg = ko.computed(function() {
            return ko.unwrap(model.hasKeg);
        });
        self.keg = ko.computed(function() {
            return ko.unwrap(model.keg);
        });

        self.resolver = new ComponentResolver();
        self.resolver.register('keg', 'keg');
        self.resolver.register('keg-body', 'kegBody');
        self.resolver.register('face', 'face');
        self.resolver.register('beer', 'beer');
        self.resolver.register('percent', 'bits');
        self.resolver.register('the-dream', 'onKegBody');
    };

    return {
        viewModel: Tap,
        template: { element: 'tap-template' }
    };
});