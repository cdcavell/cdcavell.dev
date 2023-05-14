/*!
  * Licensed under MIT (https://github.com/cdcavell/cdcavell.dev/blob/main/LICENSE)
  *
  *  Revisions:
  *  ----------------------------------------------------------------------------------------------------
  * | Contributor           | Build   | Revison Date | Description
  * |-----------------------|---------|--------------|---------------------------------------------------
  * | Christopher D. Cavell | 1.0.4.0 | 12/30/2022   | User Role Claims Development
  * | Christopher D. Cavell | 1.0.3.0 | 12/10/2022   | User Registration Development
  * | Christopher D. Cavell | 1.0.2.0 | 09/13/2022   | Duende IdentityServer Development
  * | Christopher D. Cavell | 1.0.0.0 | 08/21/2022   | Initial Development
  *
  */

$(document).ready(function () {

    $('.culture-link a').click(function (e) {

        e.preventDefault();
        wait();
        
        var url = '/Home/SetCulture';
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        var model = {
            Culture: $(this).data('culture'),
            ReturnUrl: window.location.pathname + location.search
        };

        ajaxPost(url, token, model)
            .then(function (data) {
                window.location.replace(data.returnUrl);
            })
            .catch((error) => {
                ajaxError(error)
            });

    });

});


