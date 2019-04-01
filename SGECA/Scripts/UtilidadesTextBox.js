
//ej: onkeypress="return verificarNumeroEntero(event);"
function verificarNumeroEntero(evento) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

//ej: onkeypress="return verificarNumeroReal(this,event);"
function verificarNumeroReal(objeto, evento) {
    var keyPressed;
    if (evento.keyCode)
        keyPressed = evento.keyCode;
    else keyPressed = evento.charCode;
    var keyASCII = String.fromCharCode(keyPressed);

    var valorActual = objeto.value + keyASCII;
    return !isNaN(parseFloat(valorActual)) && isFinite(valorActual);
}

function numerodecimal(objeto, evento) {
    var key = window.event.keyCode;

    if (key < 46 || key > 57) {
        window.event.keyCode = 0;
        return false;
    }
}

function verificarNumeroRealySubmit(objeto, evento) {
   
    var keyPressed;
    if (evento.keyCode)
        keyPressed = evento.keyCode;
    else keyPressed = evento.charCode;
    var keyASCII = String.fromCharCode(keyPressed);

    var valorActual = objeto.value;
    if (event.keyCode != 13)
        valorActual = objeto.value + keyASCII;
    

    if (event.keyCode == 13 && !isNaN(parseFloat(valorActual)) && isFinite(valorActual))
    {
        document.getElementById("ctl00_ctl00_cphContenido_cphContenido_btnAgregar").focus();
       //form.submit();
    }else
        return false;
    
 }




//function validateEnter(e, objeto) {
//    var key = (window.event) ? event.keyCode : e.keyCode;

//    if (key == 13) {

//        var txt = document.getElementById(objeto);
//        if (txt != null) {
//            txt.focus();
//        }
//    }
//}

//function hacerEnter(e) {
//    tecla = (document.all) ? e.keyCode : e.which;
//    if (tecla == 13) {
//        var btn = document.getElementById('ctl00_ctl00_cphContenido_cphContenido_txtSubTotal');
//        if (btn) {
//            btn.click();
//        }
//    }
//}

//function ChangeOnEnter() {
//    //if (event.keyCode === 13) {
//    //    document.getElementById("a").focus();
//    //    return false;
//    //}
//    alert("OKs");
//}

function espere() {

    $('.loading').css('display', 'block');

};

//function hacerfocus(control) {
//    alert(control);
//    tecla = (document.all) ? e.keyCode : e.which;
//    if (tecla == 13) {
//        var ctrl = document.getElementById(control);
//        if (ctrl) {
//            ctrl.focus();
//        }
//    }
//}
