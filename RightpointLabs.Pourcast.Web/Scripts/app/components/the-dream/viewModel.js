define('app/components/the-dream/viewModel', ['ko', 'app/componentResolver', 'text!app/components/the-dream/template.html'], function (ko, ComponentResolver, template) {
    
    function TheDream(model) {
        var self = this;
    };

    return {
        viewModel: TheDream,
        template: template
    };
});