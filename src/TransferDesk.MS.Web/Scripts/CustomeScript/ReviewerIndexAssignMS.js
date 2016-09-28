var IsEnable = false;
var txtOldMsID;
$(document).ready(function () {

    jQuery('#loading').hide();
    $('#ddlJournalTitle').empty();
    $('#ddlJournalTitle').append($('<option>').text("-- Select Journal --").attr('value', -1));
    ClearPopUpControls();
    LoadJournalList();
    LoadTask();

    $("#btnAssignMSScript").click(function () {
        var a = [0];
        $('#template_tr input:checked').each(function () {
            a.push($(this).attr('value'));
        });
        var reviewerIds = a.join(', ');
        DisplayManuscriptDetails("LoadSelectedReviewers", "Null", reviewerIds);

    });

    $('#txtManuscriptID').focus(function () {
        txtOldMsID = $('#txtManuscriptID').val();
    });

    //loads records on manuscript text change.
    $('#txtManuscriptID').focusout(function () {
        if ($('#txtManuscriptID').val().trim() != txtOldMsID.trim()) {

            var a = [0];
            $('#template_tr input:checked').each(function () {
                a.push($(this).attr('value'));
            });
            var reviewerIds = a.join(', ');

            var manuScriptId = $('#txtManuscriptID').val().trim();
            DisplayManuscriptDetails("LoadOntxtbox", manuScriptId, reviewerIds);

            var cnt = 0;
            var totRows = 0;
            $('#tableTemplateMSPopup_tr tr').each(function (i, rows) {
                totRows++;
                var row = $(this);
                if (row.find('input[type="checkbox"]').is(':checked')) {
                    cnt++;
                }
            });
            if (totRows == cnt) {
                $("#btnAssignMS").attr('disabled', false);
                $("#btnSubmitFinal").attr('disabled', false);
            } else {
                $("#btnAssignMS").attr('disabled', true);
                $("#btnSubmitFinal").attr('disabled', true);
            }
            $('#chkIsRevision').attr('checked', false);

        }
    });


    $("#btnAssignMS").click(function () {
        var isTrue = ManuscriptAllocationValidations(0);//0 : for save.
        if (isTrue) {
            saveSubmitReviewers(0);//0 : for save records.
        }
    });
    $("#btnSubmitFinal").click(function () {
        var isTrue = ManuscriptAllocationValidations(1);//1 : for submit.
        if (isTrue) {
            saveSubmitReviewers(1);//1 : for submit records.
        }
    });
});

function saveSubmitReviewers(isAssociateFinalSubmit) {
    var a = [0];
    $('#template_tr input:checked').each(function () {
        a.push($(this).attr('value'));
    });
    var reviewerIds = a.join(', ');

    var msid = $('#txtManuscriptID').val().trim();
    var ddlTask = $('#ddlTask option:selected').val();
    var jobType = $('input[type="radio"]:checked').val();
    var articleTitle = $('#txtArticleTitle').val().trim();
    var ddlJournalTitle = $('#ddlJournalTitle option:selected').val();
    var user = "";
    var rowCount = 0;
    var totRows = 0;
    $('#tableTemplateMSPopup_tr tr').each(function (i, rows) {
        var row = $(this);
        if (row.find('input[type="checkbox"]').is(':checked') && row.find('input[type="checkbox"]').is(':enabled')) {
            totRows++;
        }
    });
    $('#tableTemplateMSPopup_tr tr').each(function (i, rows) {
        var row = $(this);
        //loop for save reviewers who's checkbox is checked and enabled is true only. 
        
        if (row.find('input[type="checkbox"]').is(':enabled')) {

            var reviewerMasterId = row.find('td input')[0].id.replace('N', "");
            var msReviewerSuggestionId = row.find('td input')[1].id;
            var chk = row.find('td input')[0].checked ? true : false;
            $.ajax({
                url: AppPath + "/ReviewerIndex/SaveReviewersSuggestion",
                type: "GET",
                dataType: "json",
                data: {
                    key: "INSERT",
                    msid: msid,
                    ddlTask: ddlTask,
                    rollId: 1,
                    jobType: jobType,
                    articleTitle: articleTitle,
                    ddlJournalId: ddlJournalTitle,
                    user: user,
                    reviewerMasterId: reviewerMasterId,
                    isAssociateFinalSubmit: isAssociateFinalSubmit,
                    msReviewerSuggestionId: msReviewerSuggestionId
                },
                success: function (result) {
                    var rowNum = result;
                    if (rowNum != 0 || rowNum != null) {
                        $.ajax({
                            url: AppPath + "/ReviewerIndex/SaveReviewersSuggestionInfo",
                            type: "POST",
                            dataType: "json",
                            data: {
                                key: "INSERT",
                                rowNum: rowNum,
                                reviewerMasterId: reviewerMasterId,
                                user: user,
                                chk: chk,
                                isAssociateFinalSubmit: isAssociateFinalSubmit,
                                articleTitle: articleTitle,
                                msid: msid
                            },
                            success: function (result) {
                                rowCount++;
                                if (rowCount == totRows) {
                                    //loads data after save.
                                    DisplayManuscriptDetails("LoadOnSaveButton", msid, reviewerIds);
                                    if (isAssociateFinalSubmit == 0) {
                                        $('#template_tr tr input:checked').each(function () {
                                            $(this).attr('checked', false);
                                        });
                                        alert("Manuscript saved successfuly...!.");
                                    }
                                    else if (isAssociateFinalSubmit == 1) {
                                        alert("Manuscript submited successfuly...!.");
                                        ClearPopUpControls();
                                        $('#myModal2').modal('toggle');
                                        //opens new tab(Reviewer Suggestion Page) after final submit.
                                        //var url = '@System.Configuration.ConfigurationManager.AppSettings["ReviewerSuggestion"]' + rowNum;  // UAT
                                        var url = 'http://192.168.84.68/ManuscriptScreening/ReviewerSuggestion/ReviewersSuggestions/' + rowNum; // Testing
                                        //var url = 'http://192.168.84.6/TransferDeskService/ReviewerSuggestion/ReviewersSuggestions/' + rowNum; // Live
                                        window.open(url);
                                    }
                                }
                            },
                            error: function (ex) { }
                        });
                    }
                },
                error: function (xhr) {
                }
            });
        }
    });
}

function EnableDesablePopupControls() {
    var cnt = 0;
    $('#tableTemplateMSPopup_tr tr').each(function (i, row) {
        var row = $(this);
        if (row.find('input[type="checkbox"]').is(':checked') && row.find('input[type="checkbox"]').is(':disabled')) {
            cnt++;
        }
    });
    if (cnt > 0) {
        $('input[type="radio"]').prop('disabled', true);
        $('#ddlJournalTitle').prop('disabled', true);
        $('#txtArticleTitle').prop('disabled', true);
        $('#ddlTask').prop('disabled', true);

        IsEnable = false;
    } else {
        $('input[type="radio"]').prop('disabled', false);
        $('#ddlJournalTitle').prop('disabled', false);
        $('#txtArticleTitle').prop('disabled', false);
        $('#ddlTask').prop('disabled', false);

        IsEnable = true;
    }
}

function SaveReviewersSuggestion(key, msid, ddlTask, rollId, jobType, articleTitle, ddlJournalId, user, reviewerMasterId, isAssociateFinalSubmit, msReviewerSuggestionId) {

    $.ajax({
        url: AppPath + "/ReviewerIndex/SaveReviewersSuggestion",
        type: "POST",
        dataType: "json",
        data: {
            key: key,
            msid: msid,
            ddlTask: ddlTask,
            rollId: rollId,
            jobType: jobType,
            articleTitle: articleTitle,
            ddlJournalId: ddlJournalId,
            user: user,
            reviewerMasterId: reviewerMasterId,
            isAssociateFinalSubmit: isAssociateFinalSubmit,
            msReviewerSuggestionId: msReviewerSuggestionId
        },
        success: function (result) {
            alert(result);
            return result;
            return false;
        },
        error: function (xhr) {

        }
    });

}

function DisplayManuscriptDetails(key, msid, reviewerIds) {



    $.ajax({
        url: AppPath + "/ReviewerIndex/DisplayManuscriptDetails",
        type: "GET",
        dataType: "json",
        data: { key: key, msid: msid, reviewersId: reviewerIds },
        success: function (response) {
            for (var i = 0; i < response.length; i++) {
                if (response[i].ID > 0) {
                    response[i].ReviewerMasterID = response[i].ReviewerMasterID + "N";
                }
            }
            $("#tableTemplateMSPopup_tr").append($("#tableTemplateMSPopup").render());
            $("#tableTemplateMSPopup_tr").empty();
            $("#tableTemplateMSPopup_tr").append($("#tableTemplateMSPopup").render(response));

            if (response.length > 0) {

                var msid = response[response.length - 1].msid == "" ? $('#txtManuscriptID').val() : $('#txtManuscriptID').val();
                $('#txtManuscriptID').val(msid);

                var rdbtn = (response[response.length - 1].JobType == "" || response[response.length - 1].JobType == "J") ? "J" : "B";
                if (rdbtn == "B") {
                    $('#BookRadiobtn').prop('checked', true);
                } else {
                    $('#journalRadiobtn').attr('checked', true);
                }
                var selectedJournal = response[response.length - 1].JournalID == "" ? "-1" : response[response.length - 1].JournalID;
                $("#ddlJournalTitle option[value=" + selectedJournal + "]").prop("selected", true);

                var articleTitle = response[response.length - 1].ArticleTitle.toString();
                $('#txtArticleTitle').val(articleTitle);

                var selectedTask = response[response.length - 1].SMTaskID == "" ? "-1" : response[response.length - 1].SMTaskID;
                $("#ddlTask option[value=" + selectedTask + "]").prop("selected", true);

                EnableDesablePopupControls();
                if (response.length >= 2 && (response[response.length - 1].flag) == "1") {
                    EnableDesablePopupControls();
                }

            } else {
                LoadJournalList();
                LoadTask();
                $('#txtArticleTitle').val("");
            }

        },
        error: function (xhr) {

        }
    });

}

function onjournalClick() {
    LoadJournalList();
}

function onBookClick() {
    $('#ddlJournalTitle').empty();
    $('#ddlJournalTitle').append($('<option>').text("-- Select Book --").attr('value', -1));

}

/* Clear's all controls from popup and uncheck all selected checkbob on search pane. */
function ClearPopUpControls() {
    $("#txtManuscriptID").val("");
    $("#txtArticleTitle").val("");
    $('#template_tr tr input:checked').each(function () {
        $(this).attr('checked', false);
    });
    $('#template_tr tr').each(function (i, row) {
        row = $(this);
        row.css('background-color', '#ffffff');
    });
    $('#chkIsRevision').attr('checked', false);
    LoadJournalList();
    $('#journalRadiobtn').prop('checked', true);
    LoadTask();
}

function LoadJournalList() {
    $.ajax({
        url: AppPath + "/ReviewerIndex/GetJournal",
        type: "POST",
        dataType: "json",
        data: { term: null },
        success: function (result) {
            $('#ddlJournalTitle').empty();
            $('#ddlJournalTitle').append($('<option>').text("-- Select Journal --").attr('value', -1));
            $.each(result, function (i, value) {
                $('#ddlJournalTitle').append($('<option>').text(value.JournalTitle).attr('value', value.ID));
            });
        }
    });
}

function LoadTask() {

    $.ajax({
        url: AppPath + "/ReviewerIndex/GetTask",
        type: "POST",
        datatype: "json",
        success: function (result) {
            $("#ddlTask").empty();
            $('#ddlTask').append($('<option>').text("-- Select Task --").attr('value', -1));
            $.each(result, function (i, value) {
                $('#ddlTask').append($('<option>').text(value.Description).attr('value', value.ID));
            });
        }

    });
}

function ManuscriptAllocationValidations(isSubmit) {
    var flag = true;

    var msid = $('#txtManuscriptID').val();
    if (msid.trim() == "" || msid.trim() == null) {
        flag = false;
        ShowFailureResponseMessage("Manuscript number can not be blank.");
        $('#txtManuscriptID').focus();
        return false;
    }

    var jobType = $('#ddlJournalTitle option:selected').text();
    if (jobType.trim() == "-- Select Journal --") {
        flag = false;
        ShowFailureResponseMessage("Journal/Book name can not be blank.");
        $('#ddlJournalTitle').focus();
        return false;
    }

    var articleTitle = $('#txtArticleTitle').val();
    if (articleTitle.trim() == "" || articleTitle.trim() == null) {
        flag = false;
        ShowFailureResponseMessage("Article title can not be blank.");
        $('#txtArticleTitle').focus();
        return false;
    }

    var task = $('#ddlTask option:selected').text();
    if (task.trim() == "-- Select Task --") {
        flag = false;
        ShowFailureResponseMessage("Task can not be blank.");
        $('#ddlTask').focus();
        return false;
    }

    var cnt = 0;
    $('#tableTemplateMSPopup_tr tr').each(function (i, rows) {
        var row = $(this);
        if (row.find('input[type="checkbox"]').is(':checked') && row.find('input[type="checkbox"]').is(':enabled')) {
            cnt++;
        }
    });
    if (cnt == 0) {
        flag = false;
        ShowFailureResponseMessage("Please select at least one check box.");
        return false;
    }
    if (isSubmit == 1) {
        return confirm('Are you sure you want to submit?.Press Ok to proceed!.');
    }

    return flag;

}

function CheckDuplicatesReviewers(id) {
    $("#" + id + "_tr").css("background-color", "#FFFFFF");
    if ($("#" + id).prop("checked")) {
        $("#" + id + "_tr").css("background-color", "#F5F6CE");

        var manuScriptId = $('#txtManuscriptID').val().trim();
        var journalId = $('#ddlJournalTitle option:selected').val();
        if (journalId == "") {
            journalId = 0;
        }
        $.ajax({
            url: AppPath + "/ReviewerIndex/CheckDuplicates",
            type: "GET",
            dataType: "json",
            data: {
                journalID: journalId,
                key: "CheckDuplicates",
                msid: manuScriptId,
                reviewersId: id.replace('N', "")
            },
            success: function (response) {
                if (response > 0) {
                    $("#" + id + "_tr").css("color", "#DB1E31");
                    $("#" + id + "_tr").css("background-color", "#FFFFFF");
                    $('#' + id).attr('checked', false);
                    $('#' + id).attr('disabled', true);
                    $('#' + id).attr('title', "Same reviewer is already saved, you can not modify it.");
                    $("#" + id + "_tr").attr('title', "Selected reviewer is already submited for same manuscript number and " + $('#ddlJournalTitle option:selected').text() + " journal.");
                    ShowFailureResponseMessage("Selected reviewer is already submited for same manuscript number and " + $('#ddlJournalTitle option:selected').text() + " journal.");
                }

            },
            error: function (xhr) {

            }
        });
        $("#btnAssignMS").attr('disabled', false);
        $("#btnSubmitFinal").attr('disabled', false);
    }
}

function IsRevision() {
    if (($('#chkIsRevision').is(":checked")) && IsEnable == false) {
        $('input[type="radio"]').prop('disabled', false);
        $('#ddlJournalTitle').prop('disabled', false);
        $('#txtArticleTitle').prop('disabled', false);
        $('#ddlTask').prop('disabled', false);
    }
    if (($('#chkIsRevision').is(":checked") == false) && IsEnable == false) {
        $('input[type="radio"]').prop('disabled', true);
        $('#ddlJournalTitle').prop('disabled', true);
        $('#txtArticleTitle').prop('disabled', true);
        $('#ddlTask').prop('disabled', true);
    }

}