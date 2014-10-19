define(['jquery', 'ko'], function ($, ko) {

    function Keg(params) {
        console.log("Creating keg component");
        var self = this;
        self.model = params.model;
    };

    return Keg;
});