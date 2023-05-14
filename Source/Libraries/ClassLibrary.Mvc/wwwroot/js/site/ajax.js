/*!
  * Licensed under MIT (https://github.com/cdcavell/cdcavell.dev/blob/main/LICENSE)
  *
  *  Revisions:
  *  ----------------------------------------------------------------------------------------------------
  * | Contributor           | Build   | Revison Date | Description
  * |-----------------------|---------|--------------|-----------------------------------------------------
  * | Christopher D. Cavell | 1.0.4.0 | 12/30/2022   | User Role Claims Development
  * | Christopher D. Cavell | 1.0.0.0 | 08/21/2022   | Initial Development
  *
  */

/* Example Post Call:
 *
 * ajaxPost(url, token, model)
 *     .then(function (data) {
 *         console.debug(data);
 *     })
 *     .catch((error) => {
 *         console.log(error)
 *     });
 */

function ajaxPost(url, token, model) {
    return new Promise((resolve, reject) => {
        console.debug('-- AJax POST: ' + url + ' token: ' + token);
        console.debug(model);
            
        $.ajax({
            url: url,
            method: "POST",
            async: true,
            cache: false,
            dataType: "json",
            data: {
                __RequestVerificationToken: token,
                model: model
            },
            success: function (data) {
                console.debug(data);
                resolve(data);
            },
            complete: function () {
                console.debug('-- AJax Complete');
            },
            error: function (error) {
                console.error(error);
                reject(error)
            }
        });       
    })
}


/* Example Get Call:
 *
 * ajaxGet(url)
 *     .then(function (data) {
 *         console.debug(data);
 *     })
 *     .catch((error) => {
 *         console.log(error)
 *     });
 */
function ajaxGet(url) {
    return new Promise((resolve, reject) => {
        console.debug('-- AJax GET: ' + url + ' token: ' + token);
        $.ajax({
            url: url,
            method: "GET",
            async: true,
            cache: false,
            dataType: "json",
            success: function (data) {
                console.debug(data);
                resolve(data);
            },
            complete: function () {
                console.debug('-- AJax Complete');
            },
            error: function (error) {
                console.error(error);
                reject(error)
            }
        });
    })
}

function ajaxError(error) {

    console.error('Status Text : ' + error.statusText);
    console.error('Status Code : ' + error.status);
    console.error('Status Text : ' + error.statusText);
    console.error('Response    : ' + error.responseText);

    alert('Status: ' + error.statusText + '\r\nCode: ' + error.status + '\r\nResponse: ' + error.responseText);

}

