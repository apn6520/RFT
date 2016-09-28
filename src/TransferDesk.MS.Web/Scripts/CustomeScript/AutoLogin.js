$(document).ready(function () {

   

    $("#dialogEmail").dialog({
        show: "slide",
        modal: true,
        autoOpen: false,
        width: '500',
        height: '400',
        top: '100px',
        resizable: false,
        draggable: true,
        closeOnEscape: true,
        title: "Email Details - ",
        closeText: ""
    });

    $("#dialogErrorDescription").dialog({
        show: "slide",
        modal: true,
        autoOpen: false,
        width: '400',
        height: '400',
        top: '100px',
        resizable: false,
        draggable: true,
        closeOnEscape: true,
        title: "Error Description - ",
        closeText: ""
    });

    $('#btnSearch').click(function () {
        var txtfromDate = $("#datepicker").val();

        $.ajax({
            url: AppPath + "/AdminDashboard/AutoLoginJournals",
            type: 'GET',
            dataType: 'json',
            data: { createdDateTime: txtfromDate },
            success: function (response) { 
                $("#template_tr").append($("#tableTemplate").render());
                $("#NoRecordsFound").css("display", "none");
                if (response.Data == 0) {
                    $("#SearchResultGridDiv").css("display", "none");
                    //$("#NoRecordsFound").css("display", "block");
                    //return false;
                }
                $("#SearchResultGridDiv").css("display", "block");
                $("#template_tr").empty();
                $("#template_tr").append($("#tableTemplate").render(response.Data));

                $('#pTotal').html(response.Data.length);
            },
            error: function (xhr) {
            }
        });
    });



});

function getEmail(id) {

    $('#eMailDisplay').html("<pre>" + id + "</pre>");
    $("#dialogEmail").dialog("open");
    //    $('.ui-button-icon ui-icon ui-icon-closethick').remove();
    //   $('.ui-button-icon-space').remove();
}

function getErrorDescription(id) {
    $('#eErrorDescription').html("<pre>" + id + "</pre>");
    $('#dialogErrorDescription').dialog("open");
}
 

