﻿$(document).ready(function () {

    $('#dvIsActive').css({ "display": "none" });
    $('.editButton').click(function () {
        $('.field-validation-error').text("")
        $('#ID').val($(this).closest("tr").find("td").eq(0).text());
        $('#JrID').val($(this).closest("tr").find("td").eq(1).text());
        var journalTitle = $(this).closest("tr").find("td").eq(2).text().trim();
        $("#ddlJournalTitle").val($("#ddlJournalTitle" + " option").filter(function () { return this.text == journalTitle }).val());
        $('#ddlJournalTitle').attr("disabled", true);
        $('#ArticleTypeName').val($(this).closest("tr").find("td").eq(3).text().trim());
        $('#ArticleTypeID').val($(this).closest("tr").find("td").eq(6).text());
        $('#IsActive').val($(this).closest("tr").find("td").eq(4).text());
        var chkvalue = $(this).closest("tr").children("td").find("input[type='checkbox']").attr("checked");
        $('#dvIsActive').css({ "display": "block" });
        if (chkvalue == "checked") {
            $("#IsActive").prop("checked", true);
            $("#IsActive").val("true");
        }
        else {
            $("#IsActive").prop("checked", false);
            $("#IsActive").val("true");
        }
        $('#btnAdd').val("Update");
    });

    $('#dvIsActive').css({ "display": "none" });


    $('#btnReset').click(function () {
        window.location.href = AppPath + "Admin/JournalArticleTypes";
    });

    $('#btnAdd').click(function () {
        var JournalIDName = $('#ddlJournalTitle :selected').text();
        $('#JournalTitleName').val(JournalIDName);
    });
    $('#IsActive').change(function () {
        if ($(this).is(":checked")) {
            $(this).val("true");
        } else {
            $(this).val("false");
        }
    });


});//ready function end


