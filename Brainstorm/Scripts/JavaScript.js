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

    $(function() {
        $('#Data').datetimepicker({        
            format: 'DD/MM/YYYY'
        });
    });

    //alert("js");
    //$(".select2").select2({
        //width: 'resolve'
        //placeholder: "Selecionar interveniente",
        //allowClear: true
    // });

    // for each select option(interveniente) add the select2 plugin
    $('select').each(function(index) {
        var teste = ".selectInterveniente" + (index+1);       
        $(teste).select2();
    });
    // submit the form when click on button
    $('#gravar1').on('click', function (event) {
        event.preventDefault();
        $('.estado').val('A');
        $('.estadoFlag').val(true);
        $('#myForm').submit();
    });

    $('#gravar2').on('click', function (event) {
        event.preventDefault();
        $('.estado').val('E');
        $('.estadoFlag').val(true);
        $('#myForm').submit();
    });

    $('#gravar3').on('click', function (event) {
        event.preventDefault();
        $('.estado').val('X');
        $('.estadoFlag').val(true);
        $('#myForm').submit();
    });

    //$('#botao').on('click', function (event) {
    //    event.preventDefault();
    //    $('#imagem').attr('src', '/Images/menos.gif');
        
    //});

    $('#Foo').on('show.bs.collapse',
        function () {
            $('#imagem').attr('src', '/Images/menos.gif');
            //$('#min3').attr('title', 'Esconder');

        });

    $('#Foo').on('hide.bs.collapse',
        function () {
            $('#imagem').attr('src', '/Images/mais.gif');
            //$('#min3').attr('title', 'Esconder');

        });

    // mudar a imagem ou icon para botao de cada tema
    $('.teste1').each(function(index) {
        $('#Foo' + (index)).on('show.bs.collapse',
            function() {
                $('#imagem' + (index)).attr('src', '/Images/menos.gif');

            });
    });

    $('.teste1').each(function (index) {
        $('#Foo' + (index)).on('hide.bs.collapse',
            function () {
                $('#imagem' + (index)).attr('src', '/Images/mais.gif');

            });
    });


    //function AltEstadoReg(tipo) {

    //    if (tipo == 1) {
    //        document.all("TBLEstadoReg").style.left = window.event.x - 250;
    //        document.all("TBLEstadoReg").style.display = "inline";
    //    }
    //    else {
    //        document.all("TBLEstadoReg").style.display = "none";
    //    }
    //}


    /*MODAL */

    //$("#AterarR").click(function() {
    //    $("#myModal").modal('show');
    //})

    $(function () {
        //var loading = $('#loadbar').hide();
        //$(document)
        //    .ajaxStart(function () {
        //        loading.show();
        //    }).ajaxStop(function () {
        //        loading.hide();
        //    });

        $("label.btn").on('click', function () {
            var choice = $(this).find('input:radio').val();
            $('#loadbar').show();
            $('#quiz').fadeOut();
            setTimeout(function () {
                $("#answer").html($(this).checking(choice));
                $('#quiz').show();
                $('#loadbar').fadeOut();
                /* something else */
            }, 1500);
        });

        $ans = 3;

        //$.fn.checking = function (ck) {
        //    if (ck != $ans)
        //        return 'INCORRECT';
        //    else
        //        return 'CORRECT';
        //};
    });

});

//console.log("entr");
