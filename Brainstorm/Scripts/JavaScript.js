$(document).ready(function () {
    // collapse button change when hide panel1
    $('#collapse1').on('hide.bs.collapse',
        function() {
            $('#min').attr('src', '/Images/Maximiza.gif');
            $('#min').attr('title', 'Mostrar');

        });
    $('#collapse1').on('show.bs.collapse',
        function () {
            $('#min').attr('src', '/Images/Min.gif');
            $('#min').attr('title', 'Esconder');

        });
    // collapse button change when hide panel2
    $('#collapse2').on('hide.bs.collapse',
        function () {
            $('#min2').attr('src', '/Images/Maximiza.gif');
            $('#min2').attr('title', 'Mostrar');

        });
    $('#collapse2').on('show.bs.collapse',
        function () {
            $('#min2').attr('src', '/Images/Min.gif');
            $('#min2').attr('title', 'Esconder');

        });

    // collapse button change when hide panel3
    $('#collapse3').on('hide.bs.collapse',
        function () {
            $('#min3').attr('src', '/Images/Maximiza.gif');
            $('#min3').attr('title', 'Mostrar');

        });
    $('#collapse3').on('show.bs.collapse',
        function () {
            $('#min3').attr('src', '/Images/Min.gif');
            $('#min3').attr('title', 'Esconder');

        });

    $(function() {
        $('#Data').datetimepicker({        
            format: 'DD/MM/YYYY'
        });
    });

    
    $(".select2").select2({
        //width: 'resolve'
        //placeholder: "Selecionar interveniente",
        //allowClear: true
    });



    //$('#Interveniente1').change(function () {
    //    //console.log($(this).text().split(/\s+/));
    //    //var thisvalue = $(this).find("option:selected").text();
    //    //var splitted = thisvalue.split(/\s+/);
    //    //var result = splitted[0].substring(1, splitted[0].length - 1);
    //    //$(this).find("option:selected").attr("selected", null);
    //    //$(this).find("option:selected").val(result);
    //    $('#campoleitura1').val($(this).val());
    //});

    //$('#Interveniente2').change(function () {
    //    $('#campoleitura2').val($(this).val());
    //});

    //$('#Interveniente3').change(function () {
    //    $('#campoleitura3').val($(this).val());
    //});

    //$('#Interveniente4').change(function () {
    //    $('#campoleitura4').val($(this).val());
    //});

    //$('#Interveniente5').change(function () {
    //    $('#campoleitura5').val($(this).val());
    //});

});

//console.log("entr");
