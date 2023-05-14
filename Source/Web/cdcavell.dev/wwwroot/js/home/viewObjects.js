let Home = {

    // BEGIN - Public Index View Functions
    Index: function () {

        $(document).ready(function () {

            // break out video tag to body tag
            $("#BackgroundVideo").appendTo("body")
                .hide()
                .fadeIn(8500)
                .delay(800)
                .fadeOut(7000);

            $("#header1").hide();
            $("#fader h2").each(function () {
                $(this).hide();
            });

            pageContentTransparentBackground();
            noWait();

            $("#fader h2").delay(500).seqfx();

        });

        (function ($) {
            $.fn.seqfx = function () {
                let elements = this,
                    l = elements.length,
                    i = 0;

                switch ($('#cultureId').val()) {
                    case 'nl':
                        if (l > 1) { i = 1; }
                        break;
                    case 'fr':
                        if (l > 2) { i = 2; }
                        break;
                    case 'es':
                        if (l > 3) { i = 3; }
                        break;
                    case 'ja':
                        if (l > 4) { i = 4; }
                        break;
                    case 'ar':
                        if (l > 5) { i = 5; }
                        break;
                    case 'uk':
                        if (l > 6) { i = 6; }
                        break;
                }

                function execute() {
                    let current = $(elements[i]);
                    i = (i + 1) % l;

                    current
                        .fadeIn(2000)
                        .delay(500)
                        .fadeOut(3000, execute);
                }
                execute();
                return this;
            };
        }(jQuery));

    },
    // END - Public Index View Functions

    // BEGIN - Public License View Functions
    License: function () {

        $(document).ready(function () {

            noWait();

        });

    },
    // END - Public License View Functions

    // BEGIN - Public Culture View Functions
    Culture: function () {

        $(document).ready(function () {

            noWait();

        });

    },
    // END - Public Culture View Functions

    // BEGIN - Public About View Functions
    About: function () {

        $(document).ready(function () {

            noWait();

        });

    },
    // END - Public About View Functions

    // BEGIN - Public About View Functions
    SignedOff: function () {

        $(document).ready(function () {

            pageContentTransparentBackground();
            noWait();

        });

    }
    // END - Public About View Functions
};
