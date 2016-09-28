$(document).ready(function () {

    $('#ddlBookTitle').multiselect({
        nonSelectedText: 'Select-Book Title',
        enableFiltering: true,
        includeSelectAllOption: true,
        enableCaseInsensitiveFiltering: true,
        maxHeight: 300,
        numberDisplayed: 0,
        buttonWidth: '245px'
    });


    $('#ddlJournalTitle').multiselect({
        nonSelectedText: 'Select-Journal Title',
        enableFiltering: true,
        includeSelectAllOption: true,
        enableCaseInsensitiveFiltering: true,
        maxHeight: 300,
        numberDisplayed: 0,
        buttonWidth: '245px'
    });



    if ($("#IsJournal").is(":checked")) {   
        $('.readOnly').css({ 'pointer-events': '' });
        // $(".btn-group > button.multiselect.dropdown-toggle.btn.btn-default").css({ "background-color": "#ffffff" });
    } else {
        $('.readOnly').css({ 'pointer-events': 'none' });
        // $("button.multiselect.dropdown-toggle.btn.btn-default").css({ "background-color": "#eeeeee" });
        $("#ddlJournalTitle").multiselect("clearSelection");
    }

    $("#IsJournal").change(function () {
        if ($(this).is(":checked")) {
            $('.readOnly').css({ 'pointer-events': '' });
            //   $(".btn-group > button.multiselect.dropdown-toggle.btn.btn-default").css({ "background-color": "#ffffff" });
        } else {
            $('.readOnly').css({ 'pointer-events': 'none' });
            //    $(".btn-group > button.multiselect.dropdown-toggle.btn.btn-default").css({ "background-color": "#eeeeee" });
            $("#ddlJournalTitle").multiselect("clearSelection");
        }
    });

    if ($("#IsBook").is(":checked")) {
        $('.readOnly1').css({ 'pointer-events': '' });
        //   $(".btn-group > button.multiselect.dropdown-toggle.btn.btn-default").css({ "background-color": "#ffffff" });
    } else {
        $('.readOnly1').css({ 'pointer-events': 'none' });
        //   $("button.multiselect.dropdown-toggle.btn.btn-default").css({ "background-color": "#eeeeee" });
        $("#ddlBookTitle").multiselect("clearSelection");
    }

    $("#IsBook").change(function () {
        if ($(this).is(":checked")) {
            $('.readOnly1').css({ 'pointer-events': '' });
            //     $(".btn-group > button.multiselect.dropdown-toggle.btn.btn-default").css({ "background-color": "#ffffff" });
        } else {
            $('.readOnly1').css({ 'pointer-events': 'none' });
            //       $(".btn-group > button.multiselect.dropdown-toggle.btn.btn-default").css({ "background-color": "#eeeeee" });
            $("#ddlBookTitle").multiselect("clearSelection");
        }
    });

    //if ($("#IsBook").is(":checked")) {   
    //    $('#ddlBookTitle').multiselect({
    //        nonSelectedText: 'Select-Book Title',
    //        enableFiltering: true,
    //        includeSelectAllOption: true,
    //        enableCaseInsensitiveFiltering: true,
    //        maxHeight: 300,
    //        numberDisplayed: 0,
    //        buttonWidth: '223px'
    //    });
    //    $("#ddlBookTitle").multiselect("enable");
    //} else {
    //    $("#ddlBookTitle").multiselect("disable");
    //}


    //$("#IsBook").change(function () {
    //    if ($(this).is(":checked")) {         
    //        $("#ddlBookTitle").multiselect("enable");
    //    } else {
    //        $("#ddlBookTitle").multiselect("disable");
    //        $("#ddlBookTitle").multiselect("clearSelection");
    //    }
    //});

    //$(function () {
    //    $('#ddlJournalTitle').multiselect({
    //        nonSelectedText: 'Select-Journal Title',
    //        enableFiltering: true,
    //        includeSelectAllOption: true,
    //        enableCaseInsensitiveFiltering: true,
    //        maxHeight: 300,
    //        numberDisplayed: 0,
    //        buttonWidth: '223px'
    //    });
    //});




    $("#ddlServiceType").on("change", loadslipdingscale);

    function loadslipdingscale() {
        var selectedServiceType = $("#ddlServiceType option:selected").val();
        var slidingscale = $("#ddlSlidingScale");
        if (selectedServiceType == 0 || selectedServiceType == null || selectedServiceType == "") {
            slidingscale.empty();
            slidingscale.append(new Option("Select Sliding Scale", ""));
            return false;
        }
        $.ajax(
        {
            method: "GET",
            url: AppPath + "UserRole/GetSlidingScaleList",
            contentType: "application/json; charset=utf-8",
            data: { ServiceTypeId: selectedServiceType },
            success: function (data) {
                var drpCntnt = "";
                if (data != "") {
                    var array = data.split('~');
                    var po;
                    if (array.length == 0) {
                        po = "No data found";
                    } else {
                        slidingscale.empty();
                        slidingscale.append(new Option("Select Sliding Scale", ""));
                        //articleDDL.append(new Option(" ", 0));
                        for (var i = 0; i < array.length - 1; i++) {
                            var temp = array[i].split('---');
                            slidingscale.append(new Option(temp[1], temp[0]));
                        }
                    }
                } else {
                    slidingscale.empty();
                    slidingscale.append(new Option("Select Sliding Scale", ""));
                    $("#ddlSlidingScale option[value='']").attr('selected', true)
                }
            },
            error: function (xhr, exception) {
                alert("Error occured while fetching Sliding Scale details.");
            }
        });

    };

    $("#btnReset").click(function () {
        var url = AppPath + 'UserRole/UserMaster';
        window.location.href = url;
    });


    $("#UserID").focusout(function () {
        $("#IsActive").prop("checked", true);
        $.get(AppPath + "UserRole/EmployeeName", { "userID": $("#UserID").val() }, function (data) {
            $("#Name").val(data);
        });
    });

    $("#ddlRole").focusout(function () {
        var selectedrole = $("#ddlRole option:selected").text();
        if (selectedrole == "Quality Analyst") {
            $("#ddlSlidingScale").val("");
            $("#ddlSlidingScale").prop("disabled", true);

        } else
            $("#ddlSlidingScale").prop("disabled", false);
    });
    if ($("#ID").val() != 0) {
        $('#btnSub').val("Update");
        var selectedrole = $("#ddlRole option:selected").text();
        if (selectedrole == "Quality Analyst") {
            $("#ddlSlidingScale").val("");
            $("#ddlSlidingScale").prop("disabled", true);

        }
    } else {
        $('#btnSub').val("Submit");
    }

    //if ($("#ID").val() != 0) {
    //    if ($("#IsJournal").is(":checked")) {
    //        $("#ddlJournalTitle").val("");
    //        alert("Please select Journals");
    //    }
    //}
    

    $(".editButton").click(function () {    
        var id = ($(this).closest("tr").find("td").eq(0).text()).trim();
        var servicetype = ($(this).closest("tr").find("td").eq(3).text()).trim();

        if (id == 0 || id == '' || id == null) {
            var url = AppPath + 'UserRole/UserMaster';
        } else {
           
            var url = AppPath + 'UserRole/UserMaster?id=' + id;
        }
        window.location.href = url;
   
    });


});
