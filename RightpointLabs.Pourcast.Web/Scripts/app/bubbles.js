define(['jquery'], function () {
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

function bubbles($container, params) {

    
}
