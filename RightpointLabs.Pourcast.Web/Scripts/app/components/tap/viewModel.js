define(['ko', 'app/componentResolver', 'text!app/components/tap/template.html'], function (ko, ComponentResolver, htmlString) {
    
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

        // heisenberg
        //self.resolver.register('heisenberg-face', 'face');
        //self.resolver.register('heisenberg-hat', 'onKegBody');

        // borat
        //self.resolver.register('borat-face', 'face');
        //self.resolver.register('borat-flag', 'outsideRing');

        // yosemite
        //self.resolver.register('yosemite-face', 'face');

        // waters
        //self.resolver.register('waters-face', 'face');

        // gable
        //self.resolver.register('gable-face', 'face');

        // pringes
        //self.resolver.register('pringles-face', 'face');

        // michael
        self.resolver.register('michael-face', 'face');

        // dali
        self.resolver.register('dali-face', 'face');
    };

    return {
        viewModel: Tap,
        template: htmlString
    };
});