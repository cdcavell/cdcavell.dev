function onCaptchaSubmit(captchaToken) {

    wait();

    var url = '/Home/ValidateCaptchaToken';
    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    var model = {
        CaptchaToken: captchaToken
    };
    
    ajaxPost(url, token, model)
        .then(function (data) {
            console.debug('reCAPTCHA: ' + data);
            $('#IsSubmit').val('True');
            $('#Form').submit();
        })
        .catch((error) => {
            noWait();
            ajaxError(error)
        });

};
