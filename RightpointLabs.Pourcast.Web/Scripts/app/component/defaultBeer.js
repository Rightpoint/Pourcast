define(['jquery', 'ko'], function ($, ko) {

    function Beer(params) {
        console.log("Creating Beer component");
        var self = this;
        self.model = params.model;
    };

    return Beer;
});