///#source 1 1 /Scripts/app/events.js
define('app/events', ['jquery', 'toastr', 'signalr.hubs'], function ($, toastr) {

    var subscribers = {};

    // in order to get the client to actually subscribe for events on a hub, we have to register at least one client-side call for that hub...
    // logging (the next line) will help show that if you need further details
    $.connection.eventsHub.client.dummyClientCallback = function () { };
    //$.connection.hub.logging = true;
    $.connection.hub.start({ waitForPageLoad: false });

    var pub = {
        on: function(event, callback) {
            $.connection.eventsHub.on(event, callback);

            if (!subscribers[event]) {
                subscribers[event] = [];
            }

            subscribers[event].push(callback);
        },
        off: function(event, callback) {
            $.connection.eventsHub.off(event, callback);

            var eventSubscribers = subscribers[event];
            var index = eventSubscribers.indexOf(callback);

            if (index > -1) {
                eventSubscribers.splice(index, 1);
            }
        },
        raise: function(event, args) {
            subscribers[event].forEach(function(callback) {
                callback(args);
            });
        },
        send: function (event, args) {
            $.connection.eventsHub.server[event].apply($.connection.eventsHub.server, args);
        }
    };


    // reconnect
    var retryCount = 0;
    var wasDisconnected = false;
    var disconnectedToast;

    $.connection.hub.stateChanged(function(e) {
        var isConnected = e.newState === $.connection.connectionState.connected;
        var isDisconnected = e.newState === $.connection.connectionState.disconnected;

        if (isConnected) {
            if (wasDisconnected) {
                toastr.clear(disconnectedToast);
                toastr.success("Reconnected");
                pub.raise("Reconnected");

                wasDisconnected = false;
            }
        } else if (isDisconnected) {
            if (!wasDisconnected) {
                disconnectedToast = toastr.error("Disconnected", "", {
                    timeOut: 0,
                    extendedTimeOut: 0
                });

                retryCount = 0;
                wasDisconnected = true;
            }
        }
    });

    $.connection.hub.disconnected(function() {
        setTimeout(function() {
            $.connection.hub.start();
        }, Math.pow(2, retryCount) * 1000);

        retryCount++;
    });


    // admin refresh
    pub.on("Refresh", function() {
        window.location.reload();
    });

    return pub;
});
///#source 1 1 /Scripts/app/bindings.js
define('app/bindings', ['ko', 'jquery', 'app/bubbles'], function (ko, $, bubbles) {
    var bindings = {};

    bindings.init = function () {
        ko.bindingHandlers.bubbles = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                bubbles($(element), valueAccessor());
            },
            update: function (element, valueAccessor, allBindings, viewModel, bindingContext) { }
        };

        (function () {
            var getModel = function (params, bindingContext) {
                if (typeof params === 'string' || !params.model) {
                    return bindingContext.$data;
                } else {
                    // is this unwrapping bad?
                    return ko.unwrap(params.model);
                }
            };

            var getResolver = function(params, bindingContext) {
                if (typeof params === 'string' || !params.resolver) {
                    return getClosestResolver(bindingContext);
                } else {
                    return params.resolver;
                }
            };

            var getKey = function(params) {
                if (typeof params === 'string' || !params.key) {
                    return params;
                } else {
                    return params.key;
                }
            };

            var getClosestResolver = function(bindingContext) {
                if (bindingContext.$data.resolver) {
                    return bindingContext.$data.resolver;
                }

                var i = 0;
                var resolver = null;
                while (!resolver) {
                    var $parent = bindingContext.$parents[i];
                    if ($parent.resolver) {
                        resolver = $parent.resolver;
                    }

                    i++;
                }

                return resolver;
            };

            ko.bindingHandlers.oneComponent = {
                init: function(element, valueAccessor, allBindings, viewModel, bindingContext) { },
                update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var params = valueAccessor();
                    var key = getKey(params);
                    var resolver = getResolver(params, bindingContext);
                    var model = getModel(params, bindingContext);
                    var components = resolver.resolve(key, model);

                    var bestComponent = components.reduce(function(best, component) {
                        var config = component.config;

                        if (best == null) {
                            if (config.isActive()) {
                                return {
                                    name: component.name,
                                    config: config
                                };
                            } else {
                                return null;
                            }
                        } else {
                            if (config.isActive() && config.rank() > best.config.rank()) {
                                return {
                                    name: component.name,
                                    config: config
                                };
                            } else {
                                return best;
                            }
                        }
                    }, null);

                    ko.virtualElements.emptyNode(element);

                    var componentValueAccessor = function () {
                        if (bestComponent) {
                            return {
                                name: bestComponent.name,
                                params: model,
                            };
                        } else {
                            return {
                                name: 'missing',
                                params: model,
                            };
                        }
                    };

                    ko.bindingHandlers.component.init(element, componentValueAccessor, allBindings, viewModel, bindingContext);
                }
            };

            ko.bindingHandlers.manyComponents = {
                init: function(element, valueAccessor, allBindings, viewModel, bindingContext) { },
                update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
                    var params = valueAccessor();
                    var key = getKey(params);
                    var resolver = getResolver(params, bindingContext);
                    var model = getModel(params, bindingContext);
                    var components = resolver.resolve(key, model);

                    ko.virtualElements.emptyNode(element);

                    var elements = [];

                    components.forEach(function(component) {
                        var config = component.config;

                        if (config.isActive()) {

                            var innerElement = document.createElement('div');
                            elements.push(innerElement);

                            var componentValueAccessor = function() {
                                return {
                                    name: component.name,
                                    params: model,
                                };
                            };
                            ko.bindingHandlers.component.init(innerElement, componentValueAccessor, allBindings, viewModel, bindingContext);
                        }
                    });

                    ko.virtualElements.setDomNodeChildren(element, elements);
                }
            };

            ko.virtualElements.allowedBindings.oneComponent = true;
            ko.virtualElements.allowedBindings.manyComponents = true;
        }());
    };

    return bindings;
});
///#source 1 1 /Scripts/app/bubbles.js
define('app/bubbles', ['jquery'], function () {
    var bubbles = function($container, p) {
        p = $.extend({
            minBubbleCount: 5, // Minimum number of bubbles
            maxBubbleCount: 20, // Maximum number of bubbles
            minBubbleSize: 2, // Smallest possible bubble diameter (px)
            maxBubbleSize: 8, // Largest possible bubble diameter (px)
            minRiseTime: 5,
            maxRiseTime: 12,
            maxWaitTime: 15 //
        }, p || {});

        // Calculate a random number of bubbles based on our min/max
        var bubbleCount = p.minBubbleCount + Math.floor(Math.random() * (p.maxBubbleCount + 1));

        // Create the bubbles
        for (var i = 0; i < bubbleCount; i++) {
            $container.append('<div class="bubble-container"><div class="bubble"></div></div>');
        }

        // Now randomise the various bubble elements
        $container.find('.bubble-container').each(function() {

            // Randomise the bubble positions (0 - 100%)
            var pos_rand = Math.floor(Math.random() * 101);

            // Randomise their size
            var size_rand = p.minBubbleSize + Math.floor(Math.random() * (p.maxBubbleSize - p.minBubbleSize + 1));

            // Randomise the time they start rising (0-15s)
            var delay_rand = Math.random() * (p.maxWaitTime + 1);

            // Randomise their speed (5-12s)
            var speed_rand = p.minRiseTime + Math.random() * (p.maxRiseTime - p.minRiseTime);

            // Cache the this selector
            var $this = $(this);

            // Apply the new styles
            $this.css({
                'left': pos_rand + '%',

                '-webkit-animation-duration': speed_rand + 's',
                '-moz-animation-duration': speed_rand + 's',
                '-ms-animation-duration': speed_rand + 's',
                'animation-duration': speed_rand + 's',

                '-webkit-animation-delay': delay_rand + 's',
                '-moz-animation-delay': delay_rand + 's',
                '-ms-animation-delay': delay_rand + 's',
                'animation-delay': delay_rand + 's'
            });

            $this.children('.bubble').css({
                'width': size_rand + 'px',
                'height': size_rand + 'px'
            });

        });
    };
    return bubbles;
});
///#source 1 1 /Scripts/app/camera.js
define('app/camera', ['app/events', 'jquery'], function (events, $) {

    if (window.location.href.indexOf("camera=true") < 0)
        return;

    function getVideoStream() {
        var d = $.Deferred();

        // try a series of options to get us the best picture we can
        var options = [
            { video: { mandatory: { minWidth: 2304, minHeight: 1296 } } },
            { video: { mandatory: { minWidth: 2304, minHeight: 1536 } } },
            { video: { mandatory: { minWidth: 1920, minHeight: 1080 } } },
            { video: { mandatory: { minWidth: 1280, minHeight: 720 } } },
            { video: { mandatory: { minWidth: 2048, minHeight: 1536 } } },
            { video: { mandatory: { minWidth: 1024, minHeight: 768 } } },
            { video: { mandatory: { minWidth: 640, minHeight: 480 } } },
            { video: true }
        ];

        function tryOne() {
            var opt = options.shift();
            if (opt) {
                console.log(opt);
                navigator.webkitGetUserMedia(opt, function (s) {
                    console.log("Opened camera with args", opt);
                    d.resolve(s);
                }, function () {
                    tryOne();
                });
            } else {
                d.reject("Failed to initialize camera");
            }
        }

        tryOne();

        return d.promise();
    }
    function setupVideo(stream) {
        var v = document.createElement("video");
        $(v).css({ 'z-index': -9999, opacity: 0, position: 'absolute', top: '-9999px', left: '-9999px' });
        $("body").append(v);
        v.src = URL.createObjectURL(stream);
        v.play();
        return v;
    }
    function waitForVideoReady(v, retVal, ms, iter) {
        if (!ms) ms = 100;
        if (!iter) iter = 20;

        var d = $.Deferred();

        if (v.clientHeight > 150) {
            setTimeout(function () {
                d.resolve(retVal);
            }, 500);
        } else {
            var interval = setInterval(function () {
                if (v.clientHeight > 150) {
                    console.log("Found with " + iter + " left");
                    clearInterval(interval);
                    setTimeout(function() {
                        d.resolve(retVal);
                    }, 500);
                } else {
                    iter--;
                    if (iter <= 0) {
                        clearInterval(interval);
                        d.reject("failed to initialize camera");
                    }
                }
            }, ms);
        }

        return d.promise();
    }
    function takeShot(v) {
        var c = document.createElement("canvas");
        console.log("Taking shot...");
        c.width = v.clientWidth;
        c.height = v.clientHeight;

        var ctx = c.getContext("2d");
        ctx.drawImage(v, 0, 0);

        return c.toDataURL('image/png');
    }

    var secure = window.location.protocol === "https:";
    var acquirePicture = (secure ? function () {
        // this is SSL, so the allow will be remembered, which means we can do cleanup after each shot

        // so long as we're taking a picture at startup, that will trigger the prompt
        //getVideoStream().then(function(s) {
        //    s.stop();
        //});

        return function () {
            return getVideoStream().then(function (s) {
                var v = setupVideo(s);
                return waitForVideoReady(v, { v: v, s: s }, 100, 30);
            }).then(function (obj) {
                var v = obj.v;
                var s = obj.s;

                var data = takeShot(v);

                v.pause();
                v.src = '';
                $(v).remove();
                s.stop();
                return data;
            });
        };
    } : function () {
        // this is *not* SSL, so the allow won't be remembered - we can't do cleanup after each shot
        var stream = getVideoStream();

        return function() {
            return stream.then(function (s) {
                var v = setupVideo(s);
                return waitForVideoReady(v, v, 100, 30);
            }).then(function (v) {
                console.log(v);
                var data = takeShot(v);

                $(v).remove();

                return data;
            });
        };
    })();


    var pendingPics = null;
    function takePicture(tapId) {
        if (pendingPics) {
            pendingPics.push(tapId);
            return;
        }

        pendingPics = [tapId];
        acquirePicture().then(function(data) {
            for (var i in pendingPics) {
                var tId = pendingPics[i];
                var url = "/api/picture/Taken?tapId=" + (tId || "");
                console.log(url);
                $.post(url, { '': data });
            }
        }).done(function () {
            pendingPics = null;
        });
    };

    // test out the camera when launching the app
    takePicture(null);
    events.on("PourStarted", function (e) {
        takePicture(e.TapId);
    });
    events.on("PictureRequested", function (e) {
        takePicture(null);
    });
});
///#source 1 1 /Scripts/app/componentResolver.js
define('app/componentResolver', ['ko'], function (ko) {

    function ComponentResolver() {
        this.components = ko.observableArray([]);
        this.configs = {};
    };

    ko.components.register('missing', { require: 'app/components/missing/viewModel' });

    var ensureConfig = function (component, model) {
        var self = this;
        var name = component.name;

        if (!self.configs[name]) {
            self.configs[name] = new component.Config(model);
        }

        return self.configs[name];
    };

    ComponentResolver.prototype = {
        register: function (name, key) {
            var self = this;

            require(['app/components/' + name + '/config'], function (Config) {
                if (!ko.components.isRegistered(name)) {
                    ko.components.register(name, { require: 'app/components/' + name + '/viewModel' });
                }

                self.components.push({
                    key: key,
                    name: name,
                    Config: Config
                });
            });
        },

        resolve: function (key, model) {
            var self = this;

            var matchingComponents = self.components().filter(function (component) {
                return component.key === key;
            });

            var configs = matchingComponents.map(function(component) {
                return {
                    name: component.name,
                    config: ensureConfig.call(self, component, model)
                };
            });

            return configs;
        },
    };

    return ComponentResolver;
});
///#source 1 1 /Scripts/app/dataService.js
define('app/dataService', ['jquery', 'app/model/tap', 'app/model/keg', 'app/model/beer', 'app/model/brewery', 'app/model/style', 'exports'], function($, Tap, Keg, Beer, Brewery, Style, exports) {

    exports.getTaps = function() {
        return $.get("/api/beerOnTap").then(function(beerOnTapJson) {
            var taps = [];

            beerOnTapJson.forEach(function(data) {
                var tap, brewery, style, beer, keg;

                if (data.keg != null) {
                    brewery = new Brewery(data.brewery);
                    style = new Style(data.style);
                    beer = new Beer(data.beer, brewery, style);
                    keg = new Keg(data.keg, beer);
                }

                tap = new Tap(data.tap, keg);
                taps.push(tap);
            });

            return taps;
        });
    };

    exports.getKegFromTapId = function(tapId) {
        return $.get('/api/beerOnTap/' + tapId).then(function(data) {
            var brewery, style, beer, keg;

            if (data.keg != null) {
                brewery = new Brewery(data.brewery);
                style = new Style(data.style);
                beer = new Beer(data.beer, brewery, style);
                keg = new Keg(data.keg, beer);
            }

            return keg;
        });
    };
});
