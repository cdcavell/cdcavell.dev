/*!
  * Licensed under MIT (https://github.com/cdcavell/Jackdaw/blob/main/LICENSE)
  *
  *  Revisions:
  *  ----------------------------------------------------------------------------------------------------
  * | Contributor           | Build   | Revison Date | Description
  * |-----------------------|---------|--------------|-----------------------------------------------------
  * | Christopher D. Cavell | 1.0.0.0 | 08/21/2022   | Initial Development
  *
  */

function wait() {
    $('.preloader').delay(500).fadeIn('slow');
    $('.preloader-icon').fadeIn(400);
}

function noWait() {
    $('.preloader-icon').fadeOut(400);
    $('.preloader').delay(500).fadeOut('slow');
}
