$(document).ready(function() {
    
    $("tr.grid-row").dblclick(function () {
        var crestId = ($(this).closest("tr").find("td").eq(1).text()).trim();
        $.ajax(
        {
            method: "GET",
            url: AppPath + 'QualityAnalystDashBoard/OpenManuscript',
            data: {
                crestID: crestId
            },
            contentType: "application/json; charset=utf-8",
            success: function (returnValue) {
                debugger;
                if (returnValue.returnValue.match("true")) {
                    if (returnValue.ManuscriptID == 0) {
                        alert('Manuscript not found.');
                    }
                    else {
                        window.open(AppPath + '/Manuscript/HomePage/' + returnValue.ManuscriptID, "_blank");
                    }
                    window.location.reload(true);
                }
                else {
                    alert(returnValue.message);
                }
            },
            error: function (xhr, exception) {
            }
        });
    });

    $("#btnFetchId").click(function () {
        var manuscriptsID = $("#manuscriptsID").val();
        $.ajax(
        {
            method: "GET",
            url: AppPath + 'QualityAnalystDashBoard/FetchJob',
            contentType: "application/json; charset=utf-8",
            success: function (returnValue) {
                debugger;
                if (returnValue.returnValue.match("true")) {
                    alert(returnValue.message);
                    window.location.reload(true);
                }
                else {
                    alert(returnValue.message);
                }
            },
            error: function (xhr, exception) {
            }
        });
    });

});