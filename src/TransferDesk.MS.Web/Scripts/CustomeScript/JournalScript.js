$(document).ready(function () {
    $('#dvIsActive').css({ "display": "none" });
    $('.editButton').click(function () {
        $('.field-validation-error').text("")
        $('#ID').val($(this).closest("tr").find("td").eq(0).text());
        $('#JournalTitle').val($(this).closest("tr").find("td").eq(1).text().trim());
        $('#Link').val($(this).closest("tr").find("td").eq(2).text().trim());
        $('#AliasName').val($(this).closest("tr").find("td").eq(3).text().trim());
        //$('#IsActive').val($(this).closest("tr").find("td").eq(5).text());
        var chkvalue = $(this).closest("tr").children("td").find("input[name='IsActive']").attr("checked");
        $('#dvIsActive').css({ "display": "block" });
        if (chkvalue == "checked") {
            $("#IsActive").prop("checked", true);
            $("#IsActive").val("true");
        }
        else {
            $("#IsActive").prop("checked", false);
            $("#IsActive").val("true");
        }
        $('#IsAliasActive').val($(this).closest("tr").find("td").eq(4).text());
        var chkAliasvalue = $(this).closest("tr").children("td").find("input[name='IsAliasActive']").attr("checked");
        $('#dvIsAliasActive').css({ "display": "block" });
        if (chkAliasvalue == "checked") {
            $('#IsAliasActive').prop("checked", true);
            $('#IsAliasActive').val("true");
        }
        else {
            $('#IsAliasActive').prop("checked", false);
            $('#IsAliasActive').val("true");
        }
        

        $('#btnJournalAdd').val("Update");
    });

    $('#dvIsActive').css({ "display": "none" });

    $('#btnReset').click(function () {
        //window.location.href = "/ManuscriptScreening/Manuscript/Journal/"; 
        window.location.href = AppPath + "Admin/JournalMaster";
    });


    $('#IsActive').change(function () { 
        if ($(this).is(":checked")) {
            $(this).val("true");
        } else {
            $(this).val("false");
        }
    });


    $('#AliasName').keyup(function () {
        if ($('#AliasName').val().trim() == "") {
            $("#IsAliasActive").prop("checked", false);
            $('#IsAliasActive').val("false");
        } else {
            $("#IsAliasActive").prop("checked", true);
            $('#IsAliasActive').val("true");
        }
    });

    $('#AliasName').focusout(function () {
        $.getJSON(AppPath + "Admin/IsAliasAvailable", { "aliasName": $('#AliasName').val().trim(), "Id": $('#ID').val() }, function (data) {
            if (data == true) {
                alert('Alias Name: ' + $('#AliasName').val().trim() + ' is already present.');
                $('#AliasName').val('')
                $('#AliasName').focus();
                return false;
            }
        });
    });

    
});//ready function end