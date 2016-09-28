
$(document).ready(function () {
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
    if ($("#BookLoginID").val() == null || $("#BookLoginID").val() == 0) {
        $('#ReceivedDate').val('');
    }

    if ($("#dbID").val() == "" || $("#dbID").val() == 0) {
        $("#btnPreviewBookManuscript").hide();
    }else {
        $("#btnPreviewBookManuscript").show();
    }

    $("#BookTitleId").val($("#ddlBookTitle").val());
    $(".ManuscriptDetails").find('input').each(function () {
        $(this).prop('readonly', true);
        $("#ddlBookTitle").prop('disabled', true);
    });

    $('#btnOk').click(function () {
        var rowindex = $("input[name='rbtnCount']:checked").closest("tr").index();
        var ID = $("#ID_" + (rowindex - 1)).val();
        $("#BookLoginID").val(ID);
        $('#myModal').modal('hide');
        if (ID == 0 || ID == '' || ID == null) {
            var url = AppPath + 'Manuscript/BookScreening';
        } else {
            var url = AppPath + 'Manuscript/BookScreening/' + ID;
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

    $('#myModal').on('hidden.bs.modal', function (e) {
        $('#txtSearch').val('');
        $('#SelectedValue').find('option:first').attr('selected', 'selected');
    })

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

        var URL = AppPath + "Manuscript/GetBookSearchResult";
        loadMSDetailsModal(URL, selectedValue, searchText);
    });//submit action end

    $('#btnNewAdd').click(ResetAllControls);

    $('#QualityStartCheckDate').datepicker({ dateFormat: 'dd/mm/yy' });

    if ($("#PageCount").val() == 0) {
        $("#PageCount").val('');
    }

    //if quality role is selected display quality view
    $("#ddlRole").change(function () {
        IsQualityRole();
        IsViewEditable();  
    });

    $('#StartDate').prop("readonly", true);

    

    $("#btnAssociateSave,#btnAssociateIsFinalSubmit, #btnQualitySave,#btnIsQualityFinalSubmit").click(function () {
        var Highest_iThenticateFromSingleSrc =parseInt($('#Highest_iThenticateFromSingleSrc').val());
        var iThenticatePercentage = parseInt($('#iThenticatePercentage').val());
        var iThenticateResult = $('#hdniThenticateResult1').val();
        var EnglishlangQualityId = $('#hdnEnglishlangQuality1').val();
        var EthicsComplienceId = $('#hdnEthicsComplience1').val();
        if (iThenticateResult == 0) {
            alert("Please,Select Cross Check / iThenticate Result");
            return false;
        }
        else if (EnglishlangQualityId == 0) {
            alert("Please, Select English Language Quality");
            return false;
        }
        else if (EthicsComplienceId == 0) {
            alert("Please, Select Ethics Compliance");
            return false;
        }
        if ((Highest_iThenticateFromSingleSrc > iThenticatePercentage))
        {
            alert("Highest i-thenticate % to be lower than i-thenticate %");
            return false;
        }
    });
    //when ddl QualityCheck change
    $("#QualityCheck").change(function () {
        IsQualityCheck();
    });

    //when first time page load
    IsAccurate();
    //when ddlAccurate change
    $("#ddlAccurate").change(function () {
        IsAccurate();
    });

    //when first time page load
    IsQualityRole();

    //when first time page load
    IsQualityCheck();

    $("#btnIsQualityFinalSubmit").click(function () {
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
    });
    $('#PageCount').attr("readonly", false);
    $('#ChapterTitle').attr("readonly", false);
    IsViewEditable();  
    $('span').removeClass("dd-pointer dd-pointer-down");

});//ready function end

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
            dvTable.append(table);
            $("#myModal").modal();
        }
    });

}

function ImageDropdownValidation() {
    var iThenticateResult = $('#hdniThenticateResult1').val();
    var EnglishlangQualityId = $('#hdnEnglishlangQuality1').val();
    var EthicsComplienceId = $('#hdnEthicsComplience1').val();
    if (iThenticateResult == 0) {
        alert("Please,select Cross Check / iThenticate result");
        return false;
    }
    else if (EnglishlangQualityId == 0) {
        alert("Please, select English language Quality");
        return false;
    }
    else if (EthicsComplienceId == 0) {
        alert("Please, select Ethics Complience")
        return false;
    }
}

function IsViewEditable() {
    var role = $("#ddlRole option:selected").text();
    if ($("#hdnIsAssociateFinalSubmit").val().toLocaleLowerCase() == "true") {
        if (role.toLocaleLowerCase() == "associate") {
            $("#btnAssociateSave,#btnAssociateIsFinalSubmit").prop('disabled', true);
            $('#PageCount').attr("readonly", true);
            $('#ChapterTitle').attr("readonly", true);
            $(".tWidth").find('input,textarea').each(function () {
                $(this).prop('readonly', true);
                $('#ddlOverallAnalysis, #ddliThenticateResult, #ddlEnglishlangQuality, #ddlEthicsComplience').css({ "pointer-events": "none" });
            });

        }
        else if ($("#hdnIsQualityFinalSubmit").val().toLocaleLowerCase() != "true") {
            $("#btnAssociateSave,#btnAssociateIsFinalSubmit").prop('disabled', true);
            $(".tWidth").find('input,textarea').each(function () {
                $(this).prop('readonly', false);
                $('#ddlOverallAnalysis, #ddliThenticateResult, #ddlEnglishlangQuality, #ddlEthicsComplience').css({ "pointer-events": "" });
            });
            $(".ManuscriptDetails").find('input').each(function () {
                $(this).prop('readonly', true);
                $("#ddlBookTitle").prop('disabled', true);
            });
            $('#PageCount').attr("readonly", false);
            $('#ChapterTitle').attr("readonly", false);
        }
    }

    if ($("#hdnIsQualityFinalSubmit").val().toLocaleLowerCase() == "true") {
        if (role == "Quality Analyst") {       
            $("#btnAssociateSave,#btnAssociateIsFinalSubmit").prop('disabled', true);
            $(".tWidth").find('input,textarea').each(function () {
                $(this).prop('readonly', true);
                $('#ddlOverallAnalysis, #ddliThenticateResult, #ddlEnglishlangQuality, #ddlEthicsComplience').css({ "pointer-events": "none" });
            });
            $("#btnAssociateSave,#btnAssociateIsFinalSubmit, #btnQualitySave,#btnIsQualityFinalSubmit").prop('disabled', true);
            $("#ErrorDescription,#QualityCheck,#ddlAccurate").prop('disabled', true);
            $("#QualityStartCheckDate").prop("readonly", true);
            $("#QualityStartCheckDate").datepicker("option", "disabled", true);
            $(".chkQualityCheck").prop("disabled", true);
        }
    }
}
function IsQualityRole() {
    var role = $("#ddlRole option:selected").text();
    if (role == "Quality Analyst") {
        $("#divQualityAnalyst").css({ "display": "normal" });
        $("#btnAssociateSave, #btnAssociateIsFinalSubmit").prop('disabled', true);
        $("#hrUpQualityAnalyst").css({ "display": "normal" });
        $("#btnNewAdd").prop('disabled', true);
        $("#btnQualitySave,#btnIsQualityFinalSubmit").prop('disabled', false);
    }
    else {
        $("#divQualityAnalyst").css({ "display": "none" });
        $("#hrUpQualityAnalyst").css({ "display": "none" });
        $("#btnNewAdd").prop('disabled', false);
        $("#btnAssociateSave, #btnAssociateIsFinalSubmit").prop('disabled', false);
        $("#btnQualitySave,#btnIsQualityFinalSubmit").prop('disabled', true);
        $('#PageCount').attr("readonly", true);
        $('#ChapterTitle').attr("readonly", true);
    }
}

function IsQualityCheck() {
    var QualityCheck = $("#QualityCheck option:selected").text();
    if (QualityCheck.toLocaleLowerCase() == "no") {
        $(".chkQualityCheck").prop("disabled", true);
        $("#ErrorDescription").prop('disabled', true);
        $("#QualityStartCheckDate").prop("readonly", true);
        $("#ddlAccurate").prop("disabled", true);
        $("#QualityStartCheckDate").prop("readonly", true);
        $("#QualityStartCheckDate").datepicker("option", "disabled", true);
        $("#QualityStartCheckDate").val('');
    }
    else {
        $("#QualityStartCheckDate").prop("readonly", false);
        $("#ddlAccurate").prop("disabled", false);
        $("#QualityStartCheckDate").prop("readonly", false);
        $("#QualityStartCheckDate").datepicker("option", "disabled", false);
        IsAccurate();
    }
}

function IsAccurate() {
    var accurate = $("#ddlAccurate option:selected").text();
    if (accurate.toLocaleLowerCase() == "yes") {
        $(".chkQualityCheck").prop("disabled", true);
        $("#ErrorDescription").prop('disabled', true);
    }
    else {
        $(".chkQualityCheck").prop("disabled", false);
        $("#ErrorDescription").prop('disabled', false);
    }
}

function ResetAllControls() {
    var url = AppPath + 'Manuscript/BookScreening';
    window.location.href = url;
}






