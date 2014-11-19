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
        //self.resolver.register('michael-face', 'face');

        // dali
        //self.resolver.register('dali-face', 'face');

        // colin
        //self.resolver.register('colin-face', 'face');

        // varnon
        //self.resolver.register('varnon-face', 'face');

        // mario
        //self.resolver.register('mario-face', 'face');
        //self.resolver.register('mario-hat', 'outsideRing');

        // luigi
        //self.resolver.register('luigi-face', 'face');
        //self.resolver.register('luigi-hat', 'outsideRing');

        // chaplin
        //self.resolver.register('chaplin-face', 'face');
        //self.resolver.register('chaplin-hat', 'outsideRing');
        //self.resolver.register('chaplin-cane', 'outsideRing');

        // che guevara
        //self.resolver.register('che-guevara-face', 'face');
        //self.resolver.register('che-guevara-hat', 'outsideRing');

        // zorro
        self.resolver.register('zorro-face', 'face');
        self.resolver.register('zorro-hat', 'outsideRing');

        // richie
        self.resolver.register('richie-face', 'face');
    };

    return {
        viewModel: Tap,
        template: htmlString
    };
});