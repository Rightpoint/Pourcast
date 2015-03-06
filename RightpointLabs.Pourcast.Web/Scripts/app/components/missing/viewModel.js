define('app/components/missing/viewModel', ['ko', 'jquery'], function (ko, $) {

    function Missing() { };

    return {
        viewModel: Missing,
        template: { element: 'missing-template' }
    };
});