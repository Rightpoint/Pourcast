(function (app, $, ko, toast, moment) {
    app.Beer = app.Beer || function (name, abv, logo, style, glass) {
        this.name = ko.observable(name);
        this.abv = ko.observable(abv);
        this.logo = ko.observable(logo);
        this.glass = ko.observable(glass);
        this.style = ko.observable(style);
    };

}(window.pourcast = window.pourcast || {}, jQuery, ko, toastr, moment));