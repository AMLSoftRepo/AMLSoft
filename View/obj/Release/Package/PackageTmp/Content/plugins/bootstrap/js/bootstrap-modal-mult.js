﻿    /*╔╦═══╗
      ║║╔═╗║
      ║║╚══╗
    ╔╗║╠══╗║
    ║╚╝║╚═╝║
    ╚══╩═══╝*/
//<!--=============================================================================================-->
//Permite llamar o mostrar multiples ventanas modal-->
(function ($, window) {

    $(document).on('show.bs.modal', '.modal', function () {
        var zIndex = 1050 + (20 * $('.modal:visible').length);
        $(this).css('z-index', zIndex);
        setTimeout(function () {
            $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
        }, 0);
    });

    $(document).on('hidden.bs.modal', '.modal', function () {
        $('.modal:visible').length && $(document.body).addClass('modal-open');
    });


}(jQuery, window));