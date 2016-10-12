$(document).ready(function () {
    jQuery('#loading').hide();

    jQuery('#txtGreaterthan, #txtLessThan').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });

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

    $('#txtTitleSrch,#txtTitleSrch1, #txtGreaterthan,#txtLessThan').keyup(function (e) {
        if (e.keyCode == 13) {
            btnSearch_onclick();
        }
    });
});
var firstrow = 1;
var lastrow = 5;
var currentpagenum = 1;
var totalcount;

function resetSearch() {
    $("#txtTitleSrch").val("");
    $("#txtTitleSrch1").val("");
    $("#txtGreaterthan").val("");
    $("#txtLessThan").val("");
    $("#ddlManuscript").val("tm.Name");
    $("#ddlConditions").val("AND");
    $("#ddlSearchTwo").val("vwe.AreaOfExpertise");
    $("#SearchResultGridDiv").css("display", "none");
    $("#PaginatationDiv").css("display", "none");
    $("#NoReviewerFound").css("display", "none");
    $("#lblTimeTaken").text("");
}

function btnSearch_onclick() {
    jQuery('#loading').show();
    var pageCount = Number($("#ddlDisplayPages").val());
    firstrow = 1;
    lastrow = 20;
    currentpagenum = 1;

    var ddlManuscript = $("#ddlManuscript").val();
    var ddlConditions = $("#ddlConditions").val();
    var ddlSearchTwo = $("#ddlSearchTwo").val();
    var txtTitleSrch = $("#txtTitleSrch").val().trim();
    var txtTitleSrch1 = $("#txtTitleSrch1").val().trim();
    var minValue = $("#txtGreaterthan").val();
    var maxValue = $("#txtLessThan").val();
    var start_time = new Date().getTime();
    if (minValue == "") {
        minValue = 0;
    } if (maxValue == "") {
        maxValue = 999;
    }
     
    $.ajax({
        url: AppPath + "/ReviewerIndex/GetReviewerIndexData", 
        type: 'GET',
        dataType: 'json',
        data: { fromrow: firstrow, torow: lastrow, pagesize: pageCount, searchOne: txtTitleSrch, searchTwo: txtTitleSrch1, minValue: minValue, maxValue: maxValue, SearchOneVal: ddlManuscript, ConditionVal: ddlConditions, SearchTwoVal: ddlSearchTwo, NewSearch: true },
        success: function (response) {
            var request_time = (new Date().getTime() - start_time) / 1000 ;
            $("#lblTimeTaken").text("" + response.totalcount + " results (" + request_time + " seconds)");
            $("#SearchResultGridDiv").css("display", "block");
            $("#PaginatationDiv").css("display", "block");
            $("#template_tr").append($("#tableTemplate").render());
            $("#NoReviewerFound").css("display", "none");
            if (response.records == 0) {
                $("#SearchResultGridDiv").css("display", "none");
                $("#PaginatationDiv").css("display", "none");
                jQuery('#loading').hide();
                $("#NoReviewerFound").css("display", "block");                
                return false;
            }
            firstrow = response.firstrownumber;
            lastrow = response.lastrownumber;
            totalcount = response.totalcount;
            jQuery('#loading').hide();
            $("#template_tr").empty();
            $("#template_tr").append($("#ScopusTemplate").render(response.ScopusData));
            $("#template_tr").append($("#tableTemplate").render(response.records));
            document.getElementById("start_span").innerHTML = Math.ceil(1);
            document.getElementById("last_span").innerHTML = Math.ceil(totalcount / pageCount);

            document.getElementById("start_span_header").innerHTML = Math.ceil(1);
            document.getElementById("last_span_header").innerHTML = lastrow;
            document.getElementById("TotalCount_span_header").innerHTML = totalcount;            
        },
        error: function (xhr) {
            jQuery('#loading').hide();
        }
    });
}

function HighlightSelectedTr(id) {
    $("#" + id + "_tr").css("background-color", "#ffffff");
    if ($("#" + id).prop("checked")) {
        $("#" + id + "_tr").css("background-color", "#F5F6CE");
    }
}

/* This function handle the pagination , its commmon function for Next and Previous button
its default page size is 5
Written  By Amar */
function ReviewerIndexPagination(PageValue) {
    $("html, body").animate({ scrollTop: $(document).height() }, 500);

    //jQuery('#base_div').showLoading();
    var pageCount = Number($("#ddlDisplayPages").val());
    if (currentpagenum >= (Math.ceil(totalcount / pageCount)) && PageValue == 1) {
        return false;
    }

    if (currentpagenum <= 1 && PageValue == 0) {
        return false;
    }

    /* For Previous */
    if (PageValue == 0) {
        lastrow = firstrow - 1;
        firstrow = firstrow - pageCount;
        currentpagenum = currentpagenum - 1;

    } /* For Next */
    else if (PageValue == 1) {
        firstrow = lastrow + 1;
        lastrow = lastrow + pageCount;
        currentpagenum = currentpagenum + 1;
    }
    jQuery('#loading').show();
    var ddlManuscript = $("#ddlManuscript").val();
    var ddlConditions = $("#ddlConditions").val();
    var ddlSearchTwo = $("#ddlSearchTwo").val();
    var txtTitleSrch = $("#txtTitleSrch").val().trim();
    var txtTitleSrch1 = $("#txtTitleSrch1").val().trim();
    var minValue = $("#txtGreaterthan").val();
    var maxValue = $("#txtLessThan").val();
    var start_time = new Date().getTime();

    $.ajax({
        url: AppPath + "/ReviewerIndex/GetReviewerIndexData",
        type: 'GET',
        dataType: 'json',
        data: { fromrow: firstrow, torow: lastrow, pagesize: pageCount, searchOne: txtTitleSrch, searchTwo: txtTitleSrch1, minValue: minValue, maxValue: maxValue, SearchOneVal: ddlManuscript, ConditionVal: ddlConditions, SearchTwoVal: ddlSearchTwo, NewSearch: false },
        success: function (response) {
            var request_time = (new Date().getTime() - start_time) / 1000;
            $("#lblTimeTaken").text("" + response.totalcount + " results (" + request_time + " seconds)");
            firstrow = response.firstrownumber;
            lastrow = response.lastrownumber;
            jQuery('#loading').hide();
            $("#template_tr").empty();
            $("#template_tr").append($("#tableTemplate").render(response.records));
            //jQuery('#base_div').hideLoading();                  
            //pagiButtonStatus()
            document.getElementById("start_span").innerHTML = Math.ceil(currentpagenum);
            document.getElementById("last_span").innerHTML = Math.ceil(totalcount / pageCount);

            document.getElementById("start_span_header").innerHTML = Math.ceil(firstrow);
            document.getElementById("last_span_header").innerHTML = Math.ceil(lastrow);
            document.getElementById("TotalCount_span_header").innerHTML = totalcount;
        },
        error: function (xhr) {
        }
    });
}

/* Following logic controlling the next previous button disabled property
Written  By Amar */
function pagiButtonStatus() {

    if (totalcount <= lastrow) {
        $("#btn_Next").attr("disabled", "disabled");
        //$("#btn_Next").prop("onclick", false);
    }
    if (totalcount > lastrow) {
        $("#btn_Next").attr("disabled", false);
    }
    if (totalcount <= 5) {
        $("#btn_Next").attr("disabled", "disabled");
    };
    if (firstrow <= 1) {
        $("#btn_Previous").attr("disabled", "disabled");
    }
    if (firstrow > 1) {
        $("#btn_Previous").attr("disabled", false);
    }
}

function FirstAndLastPagination(PageValue) {
    $("html, body").animate({ scrollTop: $(document).height() }, 500);
    var pageCount = Number($("#ddlDisplayPages").val());
    var ddlManuscript = $("#ddlManuscript").val();
    var ddlConditions = $("#ddlConditions").val();
    var ddlSearchTwo = $("#ddlSearchTwo").val();
    var txtTitleSrch = $("#txtTitleSrch").val().trim();
    var txtTitleSrch1 = $("#txtTitleSrch1").val().trim();
    var minValue = $("#txtGreaterthan").val();
    var maxValue = $("#txtLessThan").val();
    var start_time = new Date().getTime();
    /* For Previous */
    if (PageValue == 0) {
        lastrow = pageCount;
        firstrow = 1;
        currentpagenum = 1;

    } /* For Next */
    else if (PageValue == 1) {
        currentpagenum = Math.ceil(totalcount / pageCount);
        firstrow = ((currentpagenum - 1) * pageCount) + 1;
        lastrow = firstrow + pageCount;
    }

    $.ajax({
        url: AppPath + "/ReviewerIndex/GetReviewerIndexData",
        type: 'GET',
        dataType: 'json',
        data: { fromrow: firstrow, torow: lastrow, pagesize: pageCount, searchOne: txtTitleSrch, searchTwo: txtTitleSrch1, minValue: minValue, maxValue: maxValue, SearchOneVal: ddlManuscript, ConditionVal: ddlConditions, SearchTwoVal: ddlSearchTwo, NewSearch: false },
        success: function (response) {
            var request_time = (new Date().getTime() - start_time) / 1000;
            $("#lblTimeTaken").text("" + response.totalcount + " results (" + request_time + " seconds)");
            firstrow = response.firstrownumber;
            lastrow = response.lastrownumber;
            $("#template_tr").empty();
            $("#template_tr").append($("#tableTemplate").render(response.records));
            //jQuery('#base_div').hideLoading();                  
            //pagiButtonStatus()
            document.getElementById("start_span").innerHTML = Math.ceil(currentpagenum);
            document.getElementById("last_span").innerHTML = Math.ceil(totalcount / pageCount);

            document.getElementById("start_span_header").innerHTML = Math.ceil(firstrow);
            document.getElementById("last_span_header").innerHTML = Math.ceil(lastrow);
            document.getElementById("TotalCount_span_header").innerHTML = totalcount;
        },
        error: function (xhr) {
        }
    });

}

function getCheckedValues()
{
    var selectedIds = [];
    $('#template_tr input:checked').each(function () {
        selectedIds.push($(this).attr('value'));
    });   
}