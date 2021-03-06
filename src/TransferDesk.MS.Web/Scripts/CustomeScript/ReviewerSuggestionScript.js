﻿$(document).ready(function () {
    jQuery.validator.addMethod(
              'date',
              function (value, element, params) {
                  if (this.optional(element)) {
                      return true;
                  };
                  var result = false;
                  try {
                      $.datepicker.parseDate('dd/mm/yy', value);
                      result = true;
                  } catch (err) {
                      result = false;
                  }
                  return result;
              },
              ''
          );


    jQuery("#scroll-up").hide();
    jQuery(function () {
        jQuery(window).scroll(function () {
            if (jQuery(this).scrollTop() > 150) {
                jQuery('#scroll-up').fadeIn();
            } else {
                jQuery('#scroll-up').fadeOut();
            }
        });
        jQuery('#scroll-up').click(function () {
            jQuery('body,html').animate({
                scrollTop: 0
            }, 400);
            return false;
        });
    });

    $("#btnReport").click(function () {
        window.open(this.href + "?MSID=" + $("#MSID").val(), '_target');
        return false;
    });

    $('#btnOk').click(function () {
        var rowindex = $("input[name='rbtnCount']:checked").closest("tr").index();
        var ID = $("#ID_" + (rowindex - 1)).val();
        $("#dbID").val(ID);
        $('#myModal').modal('hide');
        if (ID == 0 || ID == '' || ID == null) {
            var url = AppPath + 'ReviewerSuggestion/ReviewersSuggestions';
        } else {
            var url = AppPath + 'ReviewerSuggestion/ReviewersSuggestions/' + ID;
        }
        window.location.href = url;

    });
    $("#ValidationSummaryClose").click(function () {
        $("#dvValidationSummary").css({ "display": "none" });
    });
    $('#txtSearch').keypress(function (e) {
        if (e.which == 13) {
            $('#btnSubmit').click();
        }
    });

    $('#btnNewAdd').click(ResetAllControls)
    $('#myModal').on('hidden.bs.modal', function (e) {
        $('#txtSearch').val('');
        $('#SelectedValue').find('option:first').attr('selected', 'selected');
    });

    $('#btnSubmit').click(function () {
        var selectedValue = $("#SelectedValue option:selected").val();
        var searchText = $('#txtSearch').val();
        if (selectedValue == 0 || selectedValue == null || selectedValue == "") {
            alert('Please, select search-by');
            return false;
        }
        if (searchText == "" || searchText == null) {
            alert('Please,Enter search-text');
            return false;
        }
        var URL = AppPath + "ReviewerSuggestion/GetReviewerSearchResult";
        loadMSDetailsModal(URL, selectedValue, searchText);
    });//submit action end

   
    $('.datepicker').datepicker({ dateFormat: 'dd/mm/yy' });
    if ($("#MSID").val() == '') {
        document.getElementById("DisplayStartDate").value = new Date($.now()).getDate() + "/" + ("0" + (new Date($.now()).getMonth() + 1)).slice(-2) + "/" + new Date($.now()).getFullYear();
    }
    $("#ddlRole").change(function () {
        IsQualityRole();
        IsQualityCheck();
		 IsViewEditable();
    });

    //when first time page load
    IsQualityRole();

    //when ddl QualityCheck change
    $("#QualityCheck").change(function () {
        IsQualityCheck();
    });

    //when ddlAccurate change
    $("#ddlAccurate").change(function () {
        IsAccurate();
    });

    $("#btnIsQualityFinalSubmit").click(function () {
        if (confirm('Are you sure you want to submit ?')) {
            return true;
        }
        else {
            return false;
        }
    });


    //when first time page load
    IsAccurate();

    //when first time page load
    IsQualityCheck();

    IsViewEditable();

    $("#btnRevertToAssociate").click(function () {
        var newTasks = [];
        $(".task_checked:checked").each(function () {
            var reviewerInfoID = $(this).attr("id");
            var msReviewersSuggestionID = $(this).val();
            newTasks[msReviewersSuggestionID] = reviewerInfoID;
        });
        if (newTasks.length > 0) {
            var role = $("#ddlRole option:selected").text();
            if (role == "Quality Analyst") {
                var QualityCheck = $("#QualityCheck option:selected").text();
                if (QualityCheck.toLocaleLowerCase() == "yes") {
                    var accurate = $("#ddlAccurate option:selected").text();
                    if (accurate.toLocaleLowerCase() == "no") {
                        if (!$(".chkQualityCheck:checked").length > 0) {
                            alert("Please, Select Error Categories");
                            return false;
                        }
                        if (!(jQuery.trim($("#ErrorDescription").val()).length > 0)) {
                            alert("Please, Enter Error Description");
                            return false;
                        }
                    }
                }
            }
        } else {
            alert('Please, select Reviewer to un-Assign');
            return false;
        }
    });

    $("#btnIsQualityFinalSubmit").click(function () {
        IsErrorCatAndDescriptionFilled();
    });


});//Ready function ends

function IsErrorCatAndDescriptionFilled() {
    var role = $("#ddlRole option:selected").text();
    if (role == "Quality Analyst") {
        var QualityCheck = $("#QualityCheck option:selected").text();
        if (QualityCheck.toLocaleLowerCase() == "yes") {
            var accurate = $("#ddlAccurate option:selected").text();
            if (accurate.toLocaleLowerCase() == "no") {
                if (!$(".chkQualityCheck:checked").length > 0) {
                    alert("Please, Select Error Categories");
                    return false;
                }
                if (!(jQuery.trim($("#ErrorDescription").val()).length > 0)) {
                    alert("Please, Enter Error Description");
                    return false;
                }
            }
        }
    }
}

function IsQualityRole() {
    var role = $("#ddlRole option:selected").text();
    if (role == "Quality Analyst") {
        $("#divQualityAnalyst").css({ "display": "normal" });
        $("#hrUpQualityAnalyst").css({ "display": "normal" });
        $("#btnNewAdd").prop('disabled', true);
        $("#btnQualitySave,#btnIsQualityFinalSubmit").prop('disabled', false);
        $('#ddlJournalTitle,#ddlTaskID,#ArticleTitle,#MSID').prop("disabled", false);
        $(".task_checked").prop('disabled', false);
        IsQualityCheck();
    }
    else if (role.toLocaleLowerCase() == "associate") {
        $("#divQualityAnalyst").css({ "display": "none" });
        $("#hrUpQualityAnalyst").css({ "display": "none" });
        $("#btnNewAdd").prop('disabled', false);
        $("#btnQualitySave,#btnIsQualityFinalSubmit").prop('disabled', true);
        $('#ddlJournalTitle,#ddlTaskID,#ArticleTitle,#MSID').prop("disabled", true);
        $(".task_checked").prop('disabled', true);
    }
}

function ResetAllControls() {
    var url = AppPath + 'ReviewerSuggestion/ReviewersSuggestions';
    window.location.href = url;
}

function loadMSDetailsModal(URL, selectedValue, searchText) {

    $.get(URL, { "SelectedValue": selectedValue, "SearchBy": searchText }, function (data) {
        var array;
        if (data == '' || data == null) {
            alert("No,record found");
            $('#myModal').modal('hide');
            return false;
        }
        else {
            $.each(data, function (key, val) {
                $.each(val, function (k, v) {
                    array += k + "--" + v + "##";
                });
                array += "---";
            });

            //Create a HTML Table element.
            var table = $("<table />");
            table[0].id = "trdMSDetails";
            table[0].className = "table table-striped table-hover";
            
            //Add headers
            var row = $(table[0].insertRow(-1));
            $.each(data, function (key, val) {
                var headerCell = $("<th style='padding-right: 15px;text-align: left;'  />");
                headerCell.html("Action");
                row.append(headerCell);
                $.each(val, function (k, v) {
                    if (k != "ID") {
                        if (k == "MSID") {
                            k = "Manuscript Number";
                        }
                        var headerCell = $("<th style='padding-right: 15px;text-align: left;' />");
                        headerCell.html(k);
                        row.append(headerCell);
                    }
                    else {
                        var headerCell = $("<th style='display:none;text-align: left;' />");
                        headerCell.html(k);
                        row.append(headerCell);
                    }
                });
                if (data.length > 0)
                    return false;
            });
            var tBody = $(table[0]).append("<tbody/>");
            rows = array.split('---');
            //add row data
            for (var i = 0; i < rows.length - 1; i++) {
                
                var row = $(tBody[0].insertRow(-1));
                var temp = rows[i].split('##');
                //add temp array data to td's
                for (var j = 0; j < temp.length - 1; j++) {
                    tdData = temp[j].split('--');
                    if (j == 0) {
                        var cell = $("<td align='left' valign='middle' style='padding-right: 15px'  />");
                        cell.html('<input type="radio" name="rbtnCount" class="rdbChecked1" id="rdbSelectMS"/>');
                        row.append(cell);
                        var cell = $("<td style='display:none;' />");
                        cell.html("<input type='hidden' id=ID_" + i + " name='ID[" + i + "]' value=" + tdData[1] + " />");
                        row.append(cell);
                    }
                    else {
                        var cell = $("<td align='left' valign='middle' style='padding-right: 15px'  />");
                        if (tdData[1] == "null") {
                            tdData[1] = '';
                        }
                        cell.html(tdData[1]);
                        row.append(cell);
                    }
                }
            }
            var dvTable = $("#myPartialViewDiv");
            dvTable.html("");
            $('#circle').hide();
            dvTable.append(table);
            $("#myModal").modal();
        }
    });

}

// Enable/disable fields depend on the final submit value of associate and quality
function IsViewEditable() {
    var role = $("#ddlRole option:selected").text();
    if ($("#hdnIsQualityFinalSubmit").val().toLocaleLowerCase() == "true") {
        if (role == "Quality Analyst") {
            $('input[type="text"], textarea, select').prop("disabled", true);
            $("#SelectedValue, #txtSearch").prop('disabled', false);
            $("#ddlRole").prop('disabled', false);
            $("#btnQualitySave,#btnIsQualityFinalSubmit").prop('disabled', true);
            $(".chkQualityCheck").prop("disabled", true);
        }
    }
}


function IsQualityCheck() {
    var QualityCheck = $("#QualityCheck option:selected").text();
    if (QualityCheck.toLocaleLowerCase() == "no") {
        $("#ddlAccurate").prop("disabled", true);
        $(".chkQualityCheck").prop("disabled", true);
        $("#ErrorDescription").prop('disabled', true);
        $("#btnIsQualityFinalSubmit").prop('disabled', false);
        $("#btnRevertToAssociate").prop('disabled', true);
    }
    else {
        $("#ddlAccurate").prop("disabled", false);
        $("#btnIsQualityFinalSubmit").prop('disabled', true);
        $("#btnRevertToAssociate").prop('disabled', false);
        IsAccurate();
    }
}

function IsAccurate() {
    var accurate = $("#ddlAccurate option:selected").text();
    if (accurate.toLocaleLowerCase() == "yes") {
        $(".chkQualityCheck").prop("disabled", true);
        $("#ErrorDescription").prop('disabled', true);
        $("#btnIsQualityFinalSubmit").prop('disabled', false);
        $("#btnRevertToAssociate").prop('disabled', true);
    }
    else {
        $(".chkQualityCheck").prop("disabled", false);
        $("#ErrorDescription").prop('disabled', false);
        $("#btnIsQualityFinalSubmit").prop('disabled', true);
        $("#btnRevertToAssociate").prop('disabled', false);
    }

}