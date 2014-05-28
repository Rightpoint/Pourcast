var pourcast = pourcast || {};

function bubbles($container, params) {

    params = params || {};
    var minBubbleCount = params.minBubbleCount || 5; // Minimum number of bubbles
    var maxBubbleCount = params.maxBubbleCount || 20; // Maximum number of bubbles
    var minBubbleSize = params.minBubbleSize || 2; // Smallest possible bubble diameter (px)
    var maxBubbleSize = params.maxBubbleSize || 6; // Largest possible bubble diameter (px)

    // Calculate a random number of bubbles based on our min/max
    var bubbleCount = minBubbleCount + Math.floor(Math.random() * (maxBubbleCount + 1));

    // Create the bubbles
    for (var i = 0; i < bubbleCount; i++) {
        $container.append('<div class="bubble-container"><div class="bubble"></div></div>');
    }

    // Now randomise the various bubble elements
    $container.find('.bubble-container').each(function () {

        // Randomise the bubble positions (0 - 100%)
        var pos_rand = Math.floor(Math.random() * 101);

        // Randomise their size
        var size_rand = minBubbleSize + Math.floor(Math.random() * (maxBubbleSize + 1));

        // Randomise the time they start rising (0-15s)
        var delay_rand = Math.floor(Math.random() * 16);

        // Randomise their speed (3-8s)
        var speed_rand = 3 + Math.floor(Math.random() * 7);

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
}
