let Account = {

    // BEGIN - Login View Functions
    Login: function () {

        $(document).ready(function () {

            $('#navbar-ul').hide();
            $('#headerRow').hide();
            pageContentTransparentBackground();
            noWait();

        });

    },
    // END - Login View Functions

    // BEGIN - Logout View Functions
    Logout: function () {

        $(document).ready(function () {

            pageContentTransparentBackground();
            noWait();

        });

    },
    // END - Logout View Functions

    // BEGIN - LoggedOut View Functions
    LoggedOut: function () {

        $(document).ready(function () {

            pageContentTransparentBackground();

            var a = document.querySelector("a.PostLogoutRedirectUri");
            if (a) {
                window.location = a.href;
            } 

            noWait();

        });

    },
    // END - LoggedOut View Functions

};
