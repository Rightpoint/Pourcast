define(['ko', 'jquery', 'text!app/components/missing/template.html'], function (ko, $, htmlString) {

    function Missing() { };

    return {
        viewModel: Missing,
        template: htmlString
    };
});