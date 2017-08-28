$(document).ready(function () {
    // collapse button change when hide panel1
    $('#collapse1').on('hide.bs.collapse',
        function() {
            //$('#min').attr('src', '/Images/Maximiza.gif');
            $('#min').removeClass('fa-window-minimize');
            $('#min').addClass('fa-window-maximize');
            $('#min').attr('title', 'Mostrar');

        });
    $('#collapse1').on('show.bs.collapse',
        function () {
            $('#min').removeClass('fa-window-maximize');
            $('#min').addClass('fa-window-minimize');
            $('#min').attr('title', 'Esconder');

        });
    // collapse button change when hide panel2
    $('#collapse2').on('hide.bs.collapse',
        function (e) {
            // if adicionado, uma vez que esta funcao era activada quando era feito o toggle de outros eventos
            if ($(this).is(e.target)) {
                $('#min2').removeClass('fa-window-minimize');
                $('#min2').addClass('fa-window-maximize');               
                $('#min2').attr('title', 'Mostrar');
            }       

        });
    $('#collapse2').on('show.bs.collapse',
        function () {
            $('#min2').removeClass('fa-window-maximize');
            $('#min2').addClass('fa-window-minimize');
            $('#min2').attr('title', 'Esconder');

        });

    // collapse button change when hide panel3
    $('#collapse3').on('hide.bs.collapse',
        function () {
            $('#min3').removeClass('fa-window-minimize');
            $('#min3').addClass('fa-window-maximize');
            $('#min3').attr('title', 'Mostrar');

        });
    $('#collapse3').on('show.bs.collapse',
        function () {
            $('#min3').removeClass('fa-window-maximize');
            $('#min3').addClass('fa-window-minimize');
            $('#min3').attr('title', 'Esconder');

        });

    //$(function () {
    //    //Session[""].ToString();
    //    $('#Data').datetimepicker({        
    //        format: 'DD/MM/YYYY'
    //    });
    //});


    // for each select option(interveniente) add the select2 plugin
    $('select').each(function(index) {
        var teste = ".selectInterveniente" + (index+1);       
        $(teste).select2();
    });
    // submit the form when click on button

    //$('#gravar').on('click', function (event) {
    //    event.preventDefault();       
    //    $('#myForm').submit();
    //});

    //$('#gravar1').on('click', function (event) {
    //    if (confirm("Deseja realmente alterar o estado da reunião para APROVADA?"))
    //    {
    //        event.preventDefault();
    //        $('.estado').val('A');
    //        $('.estadoFlag').val(true);
    //        $('#myForm').submit();
    //    }
       
    //});

    //$('#gravar2').on('click', function (event) {
    //    event.preventDefault();
    //    $('.estado').val('E');
    //    $('.estadoFlag').val(true);
    //    $('#myForm').submit();
    //});

    //$('#gravar3').on('click', function (event) {
    //    event.preventDefault();
    //    $('.estado').val('X');
    //    $('.estadoFlag').val(true);
    //    $('#myForm').submit();
    //});

    // mudar a imagem ou icon para botao de cada tema
    $('.mostraGestInov').each(function (index) {
        $('#Foo' + (index)).on('show.bs.collapse',
            function() {
                $('#imagem' + (index)).attr('src', '/Images/menos.gif');

            });
    });

    $('.mostraGestInov').each(function (index) {
        $('#Foo' + (index)).on('hide.bs.collapse',
            function () {
                $('#imagem' + (index)).attr('src', '/Images/mais.gif');

            });
    });



    /*MODAL */

    $("#wf5").click(function() {
        $("#myModal").modal('show');
    })

    $("#wf").click(function () {
        //alert("entrei");
        $("#modalWorkflow").modal('show');
    })

    $("#anexos").click(function () {
        //alert("entrei");
        $("#upload").modal('show');
    })

   

    //$('#tableIdeias').on('click', "span.comentarios", function () {
    //    var id = $(this).attr("data-id");
    //    $("#modal").html("");
    //    $("#modal").load("Comentar?id=" + id, function () {
    //        $("#modal").show();
    //    })
    //});

});

//console.log("entr");


/*CHAT*/
//$(document).on('click', '.panel-heading span.icon_minim', function (e) {
//    //alert("entrei");
//    var $this = $(this);
//    if (!$this.hasClass('panel-collapsed')) {
//        $this.parents('.panel').find('.panel-body').slideUp();
//        $this.addClass('panel-collapsed');
//        $this.removeClass('glyphicon-minus').addClass('glyphicon-plus');
//    } else {
//        $this.parents('.panel').find('.panel-body').slideDown();
//        $this.removeClass('panel-collapsed');
//        $this.removeClass('glyphicon-plus').addClass('glyphicon-minus');
//    }
//});
//$(document).on('focus', '.panel-footer input.chat_input', function (e) {
//    var $this = $(this);
//    if ($('#minim_chat_window').hasClass('panel-collapsed')) {
//        $this.parents('.panel').find('.panel-body').slideDown();
//        $('#minim_chat_window').removeClass('panel-collapsed');
//        $('#minim_chat_window').removeClass('glyphicon-plus').addClass('glyphicon-minus');
//    }
//});
//$(document).on('click', '#new_chat', function (e) {
//    //alert("entrei");
//    var size = $(".chat-window:last-child").css("margin-left");
//    size_total = parseInt(size) + 400;
//    alert(size_total);
//    var clone = $("#chat_window_1").clone().appendTo(".container");
//    clone.css("margin-left", size_total);
//});
//$(document).on('click', '.icon_close', function (e) {
//    //$(this).parent().parent().parent().parent().remove();
//    $("#chat_window_1").remove();
//});

var flag_entrada = [];
var listaAtividadesAtivas = []; //lista com os ids das atividades que têm o chat aberto
var nrChatsAbertos = 0;
var dataGlobal;

function minimizarChat(chatid) {
    //alert("entrei");
    $("#minMax" + chatid).slideUp();
    document.getElementById("minimizarChat" + chatid).setAttribute("id", "maximizarChat" + chatid);
    $("#maximizarChat" + chatid).removeClass("glyphicon glyphicon-minus icon_minim").addClass("glyphicon glyphicon-plus");
    document.getElementById("maximizarChat" + chatid).setAttribute("onclick", "maximizarChat(" + chatid + ");return false;");
}

function maximizarChat(chatid) {
    try {
        $("#minMax" + chatid).slideDown();
        document.getElementById("maximizarChat" + chatid).setAttribute("id", "minimizarChat" + chatid);
        $("#minimizarChat" + chatid).removeClass("glyphicon glyphicon-plus").addClass("glyphicon glyphicon-minus icon_minim");
        document.getElementById("minimizarChat" + chatid).setAttribute("onclick", "minimizarChat(" + chatid + ");return false;");
    }
    catch (ex)
    { }
}

function fecharChat(chatid) {
    document.getElementById("chatTema" + chatid).style.display = "none";

    try {
        if (document.getElementById("maximizarChat" + chatid)) {//caso a janela tenha sido fechada estando o chat minimizado -> é maximizado depois de ser fechado para da próxima vez que for aberto aparecer maximizado.
            maximizarChat(chatid);
        }
    }
    catch (ex)
    { }
    var pos = listaAtividadesAtivas.indexOf(chatid);
    listaAtividadesAtivas.splice(listaAtividadesAtivas.indexOf(chatid), 1);//Apaga o id da lista de atividades ativas
    nrChatsAbertos--;
    updatePosicoesJanelasChat(pos); // Para reposicionar as janelas dos chats
}

/*
Método que é chamado após uma janela de chat ter sido fechada de modo a reposicionar as restantes que ficaram abertas
*/
function updatePosicoesJanelasChat(pos) {
    try {
        for (var j = pos; j < listaAtividadesAtivas.length ; j++) {
            var stringEspacamentoDireita = document.getElementById("linha" + listaAtividadesAtivas[j]).style.right.valueOf(); //Valor atual do espacamento á direita da página
            var valorEspacamentoDireita = stringEspacamentoDireita.replace("px", ""); //Para ficar apenas com o valor
            var novoEspacamentoDireita = parseInt(valorEspacamentoDireita) - 225;
            document.getElementById("linha" + listaAtividadesAtivas[j]).style.right = novoEspacamentoDireita + "px";
        }
    }
    catch (ex) { }
}

function abrirChatTema(nrTema) {
    var nrFilhosAtivos = 0;
    var nrFilhos = $("#ChatTemas > div").length;

    for (var i = 0; i < nrFilhos; i++) {

        if (document.getElementById("chatTema" + i).style.display == "inline-block") {
            nrFilhosAtivos++;
        }
        $('#mensagemAtividade' + i).keyup(function (event) {
            if (event.keyCode == 13 && event.shiftKey) {



            } else if (event.keyCode == 13) {
                event.stopPropagation();
                var content = this.value;
                var caret = getCaret(this);
                this.value = content.substring(0, caret - 1) + content.substring(caret, content.length - 1);
                var tg = this.id.slice(('mensagemAtividade').length, this.id.length);
                $('#enviarAtividade' + tg).trigger("click");

            }
        });
    }


    if (document.getElementById("chatTema" + nrTema).style.display != "inline-block" && nrFilhosAtivos < 6) {
        var nrChatsPossiveis = parseInt(window.innerWidth / 240);
        if (nrFilhosAtivos < nrChatsPossiveis) {

            var espacoDireita = 40 + (225 * nrFilhosAtivos);

            document.getElementById("linha" + nrTema).style.right = espacoDireita + "px";
            document.getElementById("chatTema" + nrTema).style.display = "inline-block";
            listaAtividadesAtivas.push(nrTema);
            nrChatsAbertos++;
        }
    }
}


function inicial(formato_data) {
    if (formato_data == "DDMMYYYY") {

        $('#Data').datetimepicker({
            format: 'DD/MM/YYYY'
        });

    } else if (formato_data == "YYYYMMDD") {
        $('#Data').datetimepicker({
            format: 'YYYY/MM/DD'
        });
      
    }
    
}
