
////////////////////////////////////////////////////////////////////////////////
//VALIDACIONES  Digitos //onkeypress = "return Digitos(event, this);"
////////////////////////////////////////////////////////////////////////////////
function Digitos(e, field) {
    var teclaPulsada = window.event ? window.event.keyCode : e.which;
    var valor = field.value;

    if (teclaPulsada == 08 || (teclaPulsada == 46 && valor.indexOf(".") == -1)) return true;

    return /\d/.test(String.fromCharCode(teclaPulsada));
}


////////////////////////////////////////////////////////////////////////////////
//VALIDACIONES  Digitos //onkeypress = "return Digitos(event, this);"
////////////////////////////////////////////////////////////////////////////////
function DigitosNegativos(e, field) {
    var teclaPulsada = window.event ? window.event.keyCode : e.which;
    var valor = field.value;

    if (teclaPulsada == 08 || (teclaPulsada == 46 && valor.indexOf(".") == -1) || (teclaPulsada == 45 && valor.indexOf("-") == -1)) return true;

    return /^-?\d/.test(String.fromCharCode(teclaPulsada));
}


////////////////////////////////////////////////////////////////////////////////
//VALIDACIONES  Digitos //onkeypress = "return Numeros(event, this);"
////////////////////////////////////////////////////////////////////////////////
function Numeros(e, field) {
    var teclaPulsada = window.event ? window.event.keyCode : e.which;
    var valor = field.value;

    if (teclaPulsada == 08) return true;

    return /\d/.test(String.fromCharCode(teclaPulsada));
}



////////////////////////////////////////////////////////////////////////////////
//VALIDACIONES  Digitos //onkeypress = "return Letras(event, this);"
////////////////////////////////////////////////////////////////////////////////
function Letras(e, field) {
    var teclaPulsada = window.event ? window.event.keyCode : e.which;
    var valor = field.value;

    if (teclaPulsada == 08) return true;

    return /[A-Za-zÑÁÉÍÓÚñáéíóú\s]/.test(String.fromCharCode(teclaPulsada));
}


////////////////////////////////////////////////////////////////////////////////
//PERMITE DAR FORMATO A FECHA //formatDate(e.data.record.Fecha, 'dd/mm/yyyy HH:MM:ss TT')
////////////////////////////////////////////////////////////////////////////////
function formatDate(text, format) {
    var dt;

    if (typeof (text) === 'undefined' || text === null)
        text = '';
    else {
        dt = new Date(parseInt(text.substr(6), 10));
        text = dt.format(format); //using 3rd party plugin "Date Format 1.2.3 by (c) 2007-2009 Steven Levithan <stevenlevithan.com>"   
    }
    return text;
}


////////////////////////////////////////////////////////////////////////////////
//PERMITE ENVIAR MENSAJES A LA VISTA SEGUN
//EL MENSAJE RETORNADO POR EL CONTROLLER
////////////////////////////////////////////////////////////////////////////////
function Mensajes(mensaje) {
    if (mensaje == 'Transaccion realizada correctamente')
        $().toastmessage('showSuccessToast', mensaje);
    else
        $().toastmessage('showErrorToast', mensaje);
};


//////////////////////////////////////////////////////////////////////////////////
//PERMITE ENVIAR MENSAJES A LA VISTA SEGUN EL MENSAJE RETORNADO POR EL CONTROLLER
//////////////////////////////////////////////////////////////////////////////////
//$(document).ajaxStart(function () {
//    Pace.restart();
//});

$.ajaxSetup({
    timeout: (1000*60)*5
});

Pace.on("start", function () {
    $("div.locked").show();
});
Pace.on("done", function () {
    $("div.locked").hide();
});



////////////////////////////////////////////////////////////////////////////////
//CARGA DE UNA SERIE DE FUNCIONES QUE SERAN USADAS EN DIFERENTES PAGINAS 
////////////////////////////////////////////////////////////////////////////////
$(document).ready(function () {

    //Mantener posicion de la opcion seleccionada dentro del menu principal
    function MantenerOpcionSeleccionada() {
        var Inicio = true;
        var urldest = window.location.pathname;
        urldest = urldest.split('/')[1] + urldest.split('/')[2];

        //Mantener posicion para listas
        if ((urldest == "ONUundefined") || (urldest == "SDNundefined"))
            urldest = "ParametroONUSDNundefined";
        if (urldest == "PERSONALIZADAundefined")
            urldest = "Generalundefined";
        if (urldest == "RptListaOficioundefined")
            urldest = "PersonaOficioundefined";

        $('.sidebar-menu li a').each(function (index) {
            var urlorg = $(this).attr('href').trim();

            urlorg = urlorg.split(/\\/)[1] + urlorg.split(/\\/)[2];
            if (urlorg == urldest) {
                $(this).parents("treeview-menu").addClass("menu-open");
                $(this).parents("li").addClass("active");
                Inicio = false;
            }
        });

        if (Inicio) {
            $('.sidebar-menu').find(".treeview").first().addClass("active");
        }
    }

    MantenerOpcionSeleccionada();


    //Deshabilitar pestañas cuando tengan la clase disabled
    $("a[data-toggle=tab]").on("click", function (e) {
        if ($(this).hasClass("disabled")) {
            e.preventDefault();
            return false;
        }
    });


    //Deficion de calendario
    $('.dpicker').datetimepicker({
        timepicker: false,
        format: 'd/m/Y'
    });
    $.datetimepicker.setLocale('es');


    //Declarancion de mascaras 
    $(".currency").inputmask({ clearMaskOnLostFocus: false, removeMaskOnSubmit: true, autoUnmask: true, alias: "currency", prefix: "$ ", groupSeparator: ',', autoGroup: true, digits: 2, digitsOptional: false, placeholder: '0' });
    $(".porcent").inputmask({ clearMaskOnLostFocus: false, removeMaskOnSubmit: true, autoUnmask: true, alias: "percentage", prefix: "% ", groupSeparator: ',', autoGroup: true, digits: 2, digitsOptional: false, placeholder: '0' });
    $(".decimal").inputmask({ clearMaskOnLostFocus: false, removeMaskOnSubmit: true, autoUnmask: true, alias: "decimal", groupSeparator: ',', autoGroup: true, digits: 2, digitsOptional: false, placeholder: '0' });
    $(".numeric").inputmask({ clearMaskOnLostFocus: false, removeMaskOnSubmit: true, autoUnmask: true, alias: "integer", rightAlign: false, autoGroup: true, digitsOptional: false, groupSeparator: ',', placeholder: '0' });
    $('.time').inputmask({ clearMaskOnLostFocus: false, removeMaskOnSubmit: true, autoUnmask: true, alias: "hh:mm:ss", rightAlign: false, autoGroup: true, digitsOptional: false, placeholder: '0' });
    $(".DUI").inputmask("99999999-9", { placeholder: " ", clearMaskOnLostFocus: true });
    $(".NIT").inputmask("9999-999999-999-9", { placeholder: " ", clearMaskOnLostFocus: true });

    //Asignacion de Select2 a una clase CSS
    $(".select2-single").select2({
        language: "es",
        width: '100%'
    });


    //OFF Autocomplete
    $('input').attr('autocomplete', 'off');

    //Anular ENTER en etiquetas form
    ////$('textarea').keypress(function (e) {
    ////    if (e.which == 13) {
    ////        return false;
    ////    }
    ////});


});



