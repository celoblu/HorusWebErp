/*
 * Rotina JS para critica de dados
 * @param {any} e
 * 
 */

function MascaraNum(o, f) {
    v_obj = o;
    v_fun = f;
    setTimeout("execmascara()", 1);
}
function execmascara() {
    v_obj.value = v_fun(v_obj.value);
}

function mcep(v) {
    v = v.replace(/\D/g, "");                   //Remove tudo o que não é dígito
    v = v.replace(/^(\d{5})(\d)/, "$1-$2");     //Esse é tão fácil que não merece explicações
    return v;
}
function mvalor(v) {
    v = v.replace(/\D/g, "");//Remove tudo o que não é dígito
    v = v.replace(/(\d)(\d{8})$/, "$1.$2");//coloca o ponto dos milhões
    v = v.replace(/(\d)(\d{5})$/, "$1.$2");//coloca o ponto dos milhares

    v = v.replace(/(\d)(\d{2})$/, "$1,$2");//coloca a virgula antes dos 2 últimos dígitos
    return v;
}

// retorna o radio button selecionado
function Radio(objRadio) {

    for (i = 0; i < objRadio.length; i++) {
        if (objRadio[i].checked == true) {
            return objRadio[i].value;
        }
    }
    return "-1";
}

function Mascara(tipo, campo, teclaPress) {
    if (window.event) {
        var tecla = teclaPress.keyCode;
    }
    else {
        tecla = teclaPress.which;
    }
    var s = new String(campo.value);
    // Remove todos os caracteres à seguir: ( ) / - . e espaço, para tratar a string denovo.
    s = s.replace(/(\.|\(|\)|\/|\-| )+/g, '');
    tam = s.length + 1;

    if (tecla != 9 && tecla != 8) {
        switch (tipo) {

            case 'cpf':
                if (tam > 3 && tam < 7) {
                    campo.value = s.substr(0, 3) + '.' + s.substr(3, tam);
                }
                if (tam >= 7 && tam < 10) {
                    campo.value = s.substr(0, 3) + '.' + s.substr(3, 3) + '.' + s.substr(6, tam - 6);
                }
                if (tam >= 10 && tam < 12) {
                    campo.value = s.substr(0, 3) + '.' + s.substr(3, 3) + '.' + s.substr(6, 3) + '-' + s.substr(9, tam - 9);
                }
                break;


            case 'cnpj':

                if (tam > 2 && tam < 4) {
                    campo.value = s.substr(0, 2) + '.' + s.substr(3, tam);
                }
                if (tam >= 6 && tam < 7) {
                    campo.value = s.substr(0, 2) + '.' + s.substr(2, 3) + '.';
                }

                if (tam >= 7 && tam == 9) {
                    campo.value = s.substr(0, 2) + '.' + s.substr(2, 3) + '.' + s.substr(5, 3) + '/';

                }

                if (tam >= 9 && tam == 13) {
                    campo.value = s.substr(0, 2) + '.' + s.substr(2, 3) + '.' + s.substr(5, 3) + '/' + s.substr(8, 4) + '-';

                }

                break;

            case 'telefone':
                if (tam > 2 && tam < 4) {
                    campo.value = '(' + s.substr(0, 2) + ') ' + s.substr(2, tam);
                }
                if (tam >= 7 && tam < 11) {
                    campo.value = '(' + s.substr(0, 2) + ') ' + s.substr(2, 4) + '-' + s.substr(6, tam - 6);
                }
                break;

            case 'data':
                if (tam > 2 && tam < 4) {
                    campo.value = s.substr(0, 2) + '/' + s.substr(2, tam);
                }
                if (tam > 4 && tam < 11) {
                    campo.value = s.substr(0, 2) + '/' + s.substr(2, 2) + '/' + s.substr(4, tam - 4);
                }
                break;

            case 'cep':
                if (tam > 5 && tam < 7) {
                    campo.value = s.substr(0, 5) + '-' + s.substr(5, tam);
                }
                break;

            case 'time':
                if (tam > 2 && tam < 5) {
                    campo.value = s.substr(0, 2) + ':' + s.substr(2, tam);
                }
                break;

            case 'Estoque':
                break;
        }
    }
}

function handleNumber(event, mask) {
    with (event) {
        stopPropagation()
        preventDefault()
        if (!charCode) return
        var c = String.fromCharCode(charCode)
        if (c.match(/[^-\d,]/)) return
        with (target) {
            var txt = value.substring(0, selectionStart) + c + value.substr(selectionEnd)
            var pos = selectionStart + 1
        }
    }
    var dot = count(txt, /\./, pos)
    txt = txt.replace(/[^-\d,]/g, '')

    var mask = mask.match(/^(\D*)\{(-)?(\d*|null)?(?:,(\d+|null))?\}(\D*)$/); if (!mask) return // meglio exception?
    var sign = !!mask[2], decimals = +mask[4], integers = Math.max(0, +mask[3] - (decimals || 0))
    if (!txt.match('^' + (!sign ? '' : '-?') + '\\d*' + (!decimals ? '' : '(,\\d*)?') + '$')) return

    txt = txt.split(',')
    if (integers && txt[0] && count(txt[0], /\d/) > integers) return
    if (decimals && txt[1] && txt[1].length > decimals) return
    txt[0] = txt[0].replace(/\B(?=(\d{3})+(?!\d))/g, '.')

    with (event.target) {
        value = mask[1] + txt.join(',') + mask[5]
        selectionStart = selectionEnd = pos + (pos == 1 ? mask[1].length : count(value, /\./, pos) - dot)
    }

    function count(str, c, e) {
        e = e || str.length
        for (var n = 0, i = 0; i < e; i += 1) if (str.charAt(i).match(c)) n += 1
        return n
    }
}

function MascaraMoeda(objTextBox, SeparadorMilesimo, SeparadorDecimal, e, Decimais) {
    var decimais = Decimais;
    var sep = 0;
    var key = '';
    var i = j = 0;
    var len = len2 = 0;
    var strCheck = '0123456789';
    var aux = aux2 = '';
    var whichCode = (window.Event) ? e.which : e.keyCode;
    if (whichCode == 13) return true;
    key = String.fromCharCode(whichCode); // Valor para o código da Chave
    if (strCheck.indexOf(key) == -1) return false; // Chave inválida
    len = objTextBox.value.length;
    for (i = 0; i < len; i++)
        if ((objTextBox.value.charAt(i) != '0') && (objTextBox.value.charAt(i) != SeparadorDecimal)) break;
    aux = '';
    for (; i < len; i++)
        if (strCheck.indexOf(objTextBox.value.charAt(i)) != -1) aux += objTextBox.value.charAt(i);
    aux += key;
    len = aux.length;
    if (len == 0) objTextBox.value = '';
    if (len == 1) objTextBox.value = '0' + SeparadorDecimal + '0' + aux;
    if (len == 2) objTextBox.value = '0' + SeparadorDecimal + aux;
    if (len > 2) {
        aux2 = '';
        for (j = 0, i = len - 3; i >= 0; i--) {
            if (j == 3) {
                aux2 += SeparadorMilesimo;
                j = 0;
            }
            aux2 += aux.charAt(i);
            j++;
        }
        objTextBox.value = '';
        len2 = aux2.length;
        for (i = len2 - 1; i >= 0; i--)
            objTextBox.value += aux2.charAt(i);
        objTextBox.value += SeparadorDecimal + aux.substr(len - decimais, len);
    }
    return false;
}

//--->Função para verificar se o valor digitado é número...<---
function Numeros(Numero) {

    var numero = parseFloat(Numero).toFixed(2);
    alert(numero);

    return numero.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}

function digitos(event) {
    if (window.event) {
        // IE
        key = event.keyCode;
    }
    else if (event.which) {
        // netscape
        key = event.which;
    }
    if (key != 8 || key != 13 || key < 48 || key > 57) {
        return (((key > 47) && (key < 58)) || (key == 8) || (key == 13));
        return true;
    }
}


function SomenteNumero(e) {

    var tecla = (window.event) ? event.keyCode : e.which;

    if (tecla > 47 && tecla < 59) {
        return true;
    }
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}

function Verifica_Hora(e) {

    if (event.keyCode < 48 || event.keyCode > 57) {

        event.returnValue = false;

    }

    if (e.value.length == 2 || e.value.length == 6) {
        e.value += ":";
    }
}


//adiciona mascara de data
function MascaraData(data) {
    if (mascaraInteiro(data) === false) {
        event.returnValue = false;
    }
    return formataCampo(data, '00/00/0000', event);
}

//valida numero inteiro 
function mascaraInteiro() {
    var tecla = (window.event) ? event.keyCode : e.which;

    if (tecla < 48 || tecla > 58) {
        event.returnValue = false;
        return false;
    }

    event.returnValue = true;
    return true;

    /*
    if (event.keyCode < 48 || event.keyCode > 57) {
        event.returnValue = false;
        return false;
    }
    return true;
    */
}

function CheckDateNew(pObj) {

    var expReg = /^((0[1-9]|[12]\d)\/(0[1-9]|1[0-2])|30\/(0[13-9]|1[0-2])|31\/(0[13578]|1[02]))\/(19|20)?\d{2}$/;
    var aRet = true;
    if ((pObj) && (pObj.value.match(expReg)) && (pObj.value != '')) {
        var dia = pObj.value.substring(0, 2);
        var mes = pObj.value.substring(3, 5);
        var ano = pObj.value.substring(6, 10);
        if ((mes == 4 || mes == 6 || mes == 9 || mes == 11) && dia > 30)
            aRet = false;
        else
            if ((ano % 4) != 0 && mes == 2 && dia > 28)
                aRet = false;
            else
                if ((ano % 4) == 0 && mes == 2 && dia > 29)
                    aRet = false;
    } else
        aRet = false;

    if (!aRet) {
        alert("Data Inválida!");
    }
    return aRet;
}

function CheckDate(pObj) {

    var expReg = /^((0[1-9]|[12]\d)\/(0[1-9]|1[0-2])|30\/(0[13-9]|1[0-2])|31\/(0[13578]|1[02]))\/(19|20)?\d{2}$/;
    var aRet = true;

    if ((pObj) && (pObj.value.match(expReg)) && (pObj.value != '')) {
        var dia = pObj.value.substring(0, 2);
        var mes = pObj.value.substring(3, 5);
        var ano = pObj.value.substring(6, 10);
        if (mes == 4 || mes == 6 || mes == 9 || mes == 11 && dia > 30)
            aRet = false;
        else
            if ((ano % 4) != 0 && mes == 2 && dia > 28)
                aRet = false;
            else
                if ((ano % 4) == 0 && mes == 2 && dia > 29)
                    aRet = false;
    } else
        aRet = false;
    return aRet;
}

function ValidaCpf(campo){    
    var strCPF = document.getElementById("Cpf").value;

    strCPF = strCPF.replace(/\D+/g, "");
    exp = /\.|\-/g;
    strCPF = strCPF.toString().replace(exp, "");

    var Soma;
    var Resto;
    var Flag = 0;

    if (strCPF.length != 11 ||
        strCPF == "00000000000" ||
        strCPF == "11111111111" ||
        strCPF == "22222222222" ||
        strCPF == "33333333333" ||
        strCPF == "44444444444" ||
        strCPF == "55555555555" ||
        strCPF == "66666666666" ||
        strCPF == "77777777777" ||
        strCPF == "88888888888" ||
        strCPF == "99999999999")
        Flag = 1;

    Soma = 0;
    if (strCPF == "00000000000") {
        Flag = 1;
    }
    for (i = 1; i <= 9; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
    Resto = (Soma * 10) % 11;

    if ((Resto == 10) || (Resto == 11)) Resto = 0;
    if (Resto != parseInt(strCPF.substring(9, 10))) {
        Flag = 1;
    }

    Soma = 0;
    for (i = 1; i <= 10; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
    Resto = (Soma * 10) % 11;

    if ((Resto == 10) || (Resto == 11)) Resto = 0;

    if (Resto != parseInt(strCPF.substring(10, 11))) {
        Flag = 1;
    }
    if (Flag == 1) {
        var Msg = 'CPF Invalido!!!!, Corrija-ao!';
        alert(Msg);
        campo.value = '';        
    }
    return true;
}



function validarCNPJ(campo) {
    var cnpj = campo.value;
    cnpj = cnpj.replace(/[^\d]+/g, '');

    if (cnpj == '') return false;

    if (cnpj.length != 14) {
        alert("Cnpj inválido...");
        return false;
    }


    // Elimina CNPJs invalidos conhecidos
    if (cnpj == "00000000000000" ||
        cnpj == "11111111111111" ||
        cnpj == "22222222222222" ||
        cnpj == "33333333333333" ||
        cnpj == "44444444444444" ||
        cnpj == "55555555555555" ||
        cnpj == "66666666666666" ||
        cnpj == "77777777777777" ||
        cnpj == "88888888888888" ||
        cnpj == "99999999999999") {

        alert("Cnpj inválido");
        return false;
    }

    // Valida DVs
    tamanho = cnpj.length - 2
    numeros = cnpj.substring(0, tamanho);
    digitos = cnpj.substring(tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(0)) {
        alert("Cnpj inválido...");
        campo.value = '';
        return false;
    }



    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(1)) {
        alert("Cnpj inválido!");
        campo.value = '';
        return false;
    }

    return true;

}

function validarPIS(pis, tipo, campo) {

    var multiplicadorBase = "3298765432";
    var total = 0;
    var resto = 0;
    var multiplicando = 0;
    var multiplicador = 0;
    var digito = 99;

    // Retira a mascara
    pis = pis.value;

    var numeroPIS = pis.replace(/[^\d]+/g, '');

    if (numeroPIS.length !== 11 ||
        numeroPIS === "00000000000" ||
        numeroPIS === "11111111111" ||
        numeroPIS === "22222222222" ||
        numeroPIS === "33333333333" ||
        numeroPIS === "44444444444" ||
        numeroPIS === "55555555555" ||
        numeroPIS === "66666666666" ||
        numeroPIS === "77777777777" ||
        numeroPIS === "88888888888" ||
        numeroPIS === "99999999999") {
        return false;
    } else {
        for (var i = 0; i < 10; i++) {
            multiplicando = parseInt(numeroPIS.substring(i, i + 1));
            multiplicador = parseInt(multiplicadorBase.substring(i, i + 1));
            total += multiplicando * multiplicador;
        }

        resto = 11 - total % 11;
        resto = resto === 10 || resto === 11 ? 0 : resto;

        digito = parseInt("" + numeroPIS.charAt(10));

        if (digito != resto) {

            var Msg = 'Código  Pis/Nis Inválido, Corrija!';
            alert(Msg);
            document.getElementById(campo).value = "";
        }

        return
    }
}

//valida data
function ValidaData(data) {
    exp = /\d{2}\/\d{2}\/\d{4}/
    if (!exp.test(data.value)) {
        alert('Data Invalida! Formato deve Ser DD/MM/AAAA!');

        $('#DT input').each(function () {
            $(this).val('');
        });

    }

}

//formata de forma generica os campos
function formataCampo(campo, Mascara, evento) {
    var boleanoMascara;

    var Digitato = evento.keyCode;
    exp = /\-|\.|\/|\(|\)| /g
    campoSoNumeros = campo.value.toString().replace(exp, "");

    var posicaoCampo = 0;
    var NovoValorCampo = "";
    var TamanhoMascara = campoSoNumeros.length;;

    if (Digitato != 8) { // backspace
        for (i = 0; i <= TamanhoMascara; i++) {
            boleanoMascara = ((Mascara.charAt(i) == "-") || (Mascara.charAt(i) == ".")
                || (Mascara.charAt(i) == "/"))
            boleanoMascara = boleanoMascara || ((Mascara.charAt(i) == "(")
                || (Mascara.charAt(i) == ")") || (Mascara.charAt(i) == " "))
            if (boleanoMascara) {
                NovoValorCampo += Mascara.charAt(i);
                TamanhoMascara++;
            } else {
                NovoValorCampo += campoSoNumeros.charAt(posicaoCampo);
                posicaoCampo++;
            }
        }
        campo.value = NovoValorCampo;
        return true;
    } else {
        return true;
    }
}