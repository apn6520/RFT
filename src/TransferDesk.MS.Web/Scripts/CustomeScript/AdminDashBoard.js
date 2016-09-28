$(document).ready(function () {
    var height = window.innerHeight - 240;
    var crestId = null, serviceType = null, jobProcessingStatus = null, role = null, jobType = null;
    var oTable;
    $("#table-wrap").hide();
    $.ajax({
        type: "GET",
        url: AppPath + "AdminDashBoard/getAdmininDashboardJson",
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (jsonData) {
            //console.log(JSON.stringify(jsonData.jobsdetails));
            jQuery.support.cors = true;
            oTable = $('#myDataTable').dataTable({
                bColReorder: true,
                sDom: 'Rrt',
                bJQueryUI: true,
                bPaginate: false,
                bStateSave: true,
                "scrollY": height,
                "scrollX": true,
                bAutoWidth: false,
                aaData: jsonData.jobsdetails,
                aOrder: [],
                "aoColumnDefs": [
					{ "sClass": "id", "aTargets": [19] }
                ],
                aoColumns: [
                    {
                        mData: null,
                        mRender: function () { return '<input type="checkbox" id="IsSelected" name="IsSelected" class="editor-active">' },
                        bSortable: false,
                    },
                    { mData: "SrNo", "mRender": function (mdata, type, full) { return '<span id="SrNo">' + mdata + '</span>'; } },
                    { mData: "CrestId", "mRender": function (mdata, type, full) { return '<span id="CrestId">' + mdata + '</span>'; } },
                    { mData: "JobType", "mRender": function (mdata, type, full) { return '<span id="JobType">' + mdata + '</span>'; } },
                    { mData: "ServiceType", "mRender": function (mdata, type, full) { return '<span id="ServiceType">' + mdata + '</span>'; } },
                    { mData: "MSID", "mRender": function (mdata, type, full) { return '<span id="MSID">' + mdata + '</span>'; } },
                    { mData: "JournalBookName", "mRender": function (mdata, type, full) { return '<span id="JournalBookName">' + mdata + '</span>'; } },
                    { mData: "PageCount", "mRender": function (mdata, type, full) { return '<span id="PageCount">' + mdata + '</span>'; } },
                    { mData: "Name", "mRender": function (mdata, type, full) {
                        if (mdata == null) {
                            mdata = '';
                        };
                        return '<span id="Name">' + mdata + '</span>';} },
                    { mData: "Role", "mRender": function (mdata, type, full) { return '<span id="Role">' + mdata + '</span>'; } },
                    { mData: "Status", "mRender": function (mdata, type, full) { return '<span id="Status">' + mdata + '</span>'; } },
                    {
                        mData: "Task", "mRender": function (mdata, type, full) {
                            if (mdata == null) {
                                mdata = '';
                            };
                            return '<span id="Task">' + mdata + '</span>';
                        }
                    },
                    {
                        mData: "Revision", "mRender": function (mdata, type, full) {
                            if (mdata == null) {
                                mdata = '';
                            };
                            return '<span id="Revision">' + mdata + '</span>';
                        }
                    },
                    {
                        mData: "GroupNo", "mRender": function (mdata, type, full) {
                            if (mdata == null) {
                                mdata = '';
                            };
                            return '<span id="GroupNo">' + mdata + '</span>';
                        }
                    },
                    {
                        mData: "ReceivedDate", "mRender": function (mdata, type, full) {
                            if (mdata == null) {
                                mdata = '';
                            };
                            return '<span id="CreatedDate">' + dtConvFromJSON(mdata) + '</span>';
                        }
                    },
                    { mData: "CreatedDate", "mRender": function (mdata, type, full) { return '<span id="CreatedDate">' + timeDtConvFromJSON(mdata) + '</span>'; } },
                    {
                        mData: "FetchedDate", "mRender": function (mdata, type, full) {
                            if (mdata == null) {
                                mdata = '';
                            };
                            return '<span id="FetchedDate">' + mdata + '</span>';
                        }
                    },
                    { mData: "Age", "mRender": function (mdata, type, full) { return '<span id="Age">' + mdata + '</span>'; } },
                    {
                        mData: "HandlingTime", "mRender": function (mdata, type, full) {
                            if (mdata == null) {
                                mdata = '';
                            };
                            return '<span id="HandlingTime">' + mdata + '</span>';
                        }
                    },
                    { mData: "Id", "mRender": function (mdata, type, full) { return '<span id="Id">' + mdata + '</span>'; } },
                ],
                fnStateSaveCallback: function (oSettings, oData) {
                    localStorage.setItem($('#userName').text(), JSON.stringify(oData));
                    $("#table-wrap").show();
                },
                fnStateLoadCallback: function (oSettings) {
                    $("#Id").closest('td').attr('id', 'id');
                    return JSON.parse(localStorage.getItem($('#userName').text()));
                }
            });
            $("table tbody tr").click(function (e) {
                if ($(this).hasClass('selected')) {
                    $(this).removeClass('selected');
                    $('td>input', this).removeAttr('checked');
                    $('td>input', this).prop('checked', false);
                } else {
                    $(this).addClass('selected');
                    $('td>input', this).attr('checked', 'checked');
                    $('td>input', this).prop('checked', true);
                }
                crestId = ($(this).closest("tr").find("#Id").text()).trim();
                serviceType = ($(this).closest("tr").find("#ServiceType").text()).trim();
                jobProcessingStatus = ($(this).closest("tr").find("#Status").text()).trim();
                role = ($(this).closest("tr").find("#Role").text()).trim();
                status = ($(this).closest("tr").find("#Status").text()).trim();
                jobType = ($(this).closest("tr").find("#JobType").text()).trim();
                $("#CrestIdVM").val(crestId);
                $("#ServiceTypeVM").val(serviceType);
                $("#JobProcessingStatusVM").val(jobProcessingStatus);
                $("#RoleVM").val(role);
                $("#StatusVm").val(status);
                $("#JobType").val(jobType);
                $("#AssociateName").val(($(this).closest("tr").find("#PageCount").text()).trim());
            });
        }
    });


    /*
        function fnGetSelected(oTableLocal) {
            return oTableLocal.$('tr.row_selected');
        }
    */

    $("#resetTable").click(function () {
        localStorage.removeItem($('#userName').text());
        window.location.reload();
    });
    function dtConvFromJSON(data) {
        if (data == null) return '1/1/1950';
        var r = /\/Date\(([0-9]+)\)\//gi;
        var matches = data.match(r);
        if (matches == null) return '1/1/1950';
        var result = matches.toString().substring(6, 19);
        var epochMilliseconds = result.replace(/^\/Date\(([0-9]+)([+-][0-9]{4})?\)\/$/, '$1');
        var b = new Date(parseInt(epochMilliseconds));
        var c = new Date(b.toString());
        var curr_date = c.getDate();
        var curr_month = c.getMonth() + 1;
        var curr_year = c.getFullYear();
        var d = curr_date + '/' + curr_month.toString() + '/' + curr_year;
        return d;
    }

    function timeDtConvFromJSON(data) {
        if (data == null) return '1/1/1950';
        var r = /\/Date\(([0-9]+)\)\//gi;
        var matches = data.match(r);
        if (matches == null) return '1/1/1950';
        var result = matches.toString().substring(6, 19);
        var epochMilliseconds = result.replace(/^\/Date\(([0-9]+)([+-][0-9]{4})?\)\/$/, '$1');
        var b = new Date(parseInt(epochMilliseconds));
        var c = new Date(b.toString());
        var curr_date = c.getDate();
        var curr_month = c.getMonth() + 1;
        var curr_year = c.getFullYear();
        var curr_h = c.getHours();
        var curr_m = c.getMinutes();
        var curr_s = c.getSeconds();
        var d = curr_date + '/' + curr_month.toString() + '/' + curr_year + ' ' + curr_h + ':' + curr_m + ':' + curr_s;
        return d;
    }







    var userdata = {
        adminFunctionalityEnum: []
    }

    $("#btnAllocateID").click(function () {
        if ($("#IsSelected:checked").length == 0) {
            alert("Please, Select Job.");
        }

        else if (status == "On Hold") {
            var crestid = $("#CrestIdVM").val();
            var serviceType = $("#ServiceTypeVM").val();
            $("#AssociateNamesModal").modal();
            $.ajax(
               {
                   method: "GET",
                   url: AppPath + 'AdminDashBoard/GetLastAssociateName',
                   data: { crestid: crestid, servicetype: serviceType, JobType: $("#JobType").val() },
                   success: function (data) {
                       $("#AssociateName").val(data);
                   },
                   error: function (xhr, exception) {
                       alert('No associate name found');
                   }
               });
            $(menu[0].children[0]).trigger("click");
        }
        else {
            $("#AssociateName").val('');
            $("#AssociateNamesModal").modal();
        }


    });



    $("#AssociateName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: AppPath + 'AdminDashBoard/GetAssociateName',
                data: {
                    searchAssociate: request.term,
                    RoleName: role
                },
                dataType: "json",
                type: "GET",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.empname,
                            val: item.UserID
                        }
                    }))
                },
                error: function (response) {
                },
                failure: function (response) {
                }
            });
        },
        appendTo: $('.modal-body'),
        minLength: 1
    });


    $("#btnOk").click(function () {
        if ($("#AssociateName").val() == "") {
            alert("Select associate.");
            return false;
        } else {
            $("#AssociateNamesModal").modal('hide');
            $("#IsSelected:checked").each(function () {
                userdata.adminFunctionalityEnum.push({
                    CrestIdVM: ($(this).closest("tr").find("#Id").text()).trim(),
                    ServiceTypeVM: ($(this).closest("tr").find("#ServiceType").text()).trim(),
                    JobProcessingStatusVM: ($(this).closest("tr").find("#Status").text()).trim(),
                    RoleVM: ($(this).closest("tr").find("#Role").text()).trim(),
                    JobType: ($(this).closest("tr").find("#JobType").text()).trim(),
                    MSID: ($(this).closest("tr").find("#MSID").text()).trim(),
                    JournalTitle: ($(this).closest("tr").find("#JournalBookName").text()).trim(),
                    ReceivedDate: ($(this).closest("tr").find("#CreatedDate").text()).trim(),
                    AssociateNameVM: $("#AssociateName").val()
                });

            });

            $.ajax(
            {
                method: "POST",
                url: AppPath + 'AdminDashBoard/AllocateManuscriptToUser',
                async: true,
                cache: false,
                traditional: true,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(userdata.adminFunctionalityEnum),
                success: function (returnValue) {
                    if (returnValue.toLowerCase() == "true") {
                        alert("Job's are allocated successfully");
                        location.reload(true);
                    } else {
                        alert("Job's are already allocated to associate.");
                        location.reload(true);
                    }
                },
                error: function (xhr, exception) {
                    alert("Failed to allocate jobs");
                }
            });
        }
    });

    $("#btnExporttoExcel").click(function () {
        $("#myModal").modal();
        $('.datepicker').datepicker({ dateFormat: 'dd/mm/yy', maxDate: '0' }).datepicker("setDate", new Date());
    });

    $('.datepicker').datepicker({ dateFormat: 'dd/mm/yy', maxDate: '0' }).datepicker("setDate", new Date());

    $('#myModal').on('hidden.bs.modal', function () {
        $(this).find("input,textarea,select").val('').end();

    });

    $('#myModal').on('shown.bs.modal', function () {
        $("#btnExportOk").click(function () {
            var FromDateValue = $.datepicker.parseDate("dd/mm/yy", $("#FromDate").val());
            var ToDateValue = $.datepicker.parseDate("dd/mm/yy", $("#ToDate").val());

            if (FromDateValue == null && ToDateValue == null) {
                alert('Please select Dates');
                return false;
            }
            if (FromDateValue == null) {
                alert('Please select From Date');
                return false;
            }
            else if (ToDateValue == null) {
                alert('Please select To Date');
                return false;
            }

            if (FromDateValue > ToDateValue) {
                alert("From date cannot be greater then To Date.");
                return false;
            }

            window.location.href = AppPath + "AdminDashboard/AdminDashBoardExportToExcelData?FromDate=" + $("#FromDate").val() + "&ToDate=" + $("#ToDate").val();
            $("#myModal").modal('hide');
        });
    });

    $('#AssociateNamesModal').on('shown.bs.modal', function () {
        $('#AssociateName').focus();
    });

    $("#btnUnAllocateID").click(function () {
        if ($("#IsSelected:checked").length==0) {
            alert("Please, Select Job");
            return false;
        } else {
            if (!confirm("Are you sure to unallocate the Job")) {
                return false;
            }
            $("#IsSelected:checked").each(function () {
                userdata.adminFunctionalityEnum.push({
                    CrestIdVM: ($(this).closest("tr").find("#Id").text()).trim(),
                    ServiceTypeVM: ($(this).closest("tr").find("#ServiceType").text()).trim(),
                    JobProcessingStatusVM: ($(this).closest("tr").find("#Status").text()).trim(),
                    RoleVM: ($(this).closest("tr").find("#Role").text()).trim(),
                    JobType: ($(this).closest("tr").find("#JobType").text()).trim(),
                    AssociateNameVM: ($(this).closest("tr").find("#Name").text()).trim()
                });

            });


            $.ajax(
                  {
                      method: "POST",
                      url: AppPath + 'AdminDashBoard/UnallocateManuscriptFromUser',
                      async: false,
                      cache: false,
                      traditional: true,
                      contentType: "application/json; charset=utf-8",
                      data: JSON.stringify(userdata.adminFunctionalityEnum),
                      success: function (returnValue) {
                          if (returnValue.toLowerCase() == "true") {
                              alert("Job's are un-allocated successfully");
                              window.location.reload(true);
                          } else {
                              alert("Job's is not allocated to any associate.");
                              location.reload(true);
                          }
                      },
                      error: function (xhr, exception) {
                         // alert("Failed to un-allocate Job's ");
                      }
                  });
        }
    });


    $("#btnHoldId").click(function () {
        var associateName1 = $("#AssociateNameVM").val();
        if ($("#IsSelected:checked").length == 0) {
            alert("Please, Select Job");
            return false;
        }
        else {
            if (!confirm("Do you want to keep the respective job ids on Hold?")) {
                return false;
            }
            $("#IsSelected:checked").each(function () {
                userdata.adminFunctionalityEnum.push({
                    CrestIdVM: ($(this).closest("tr").find("#Id").text()).trim(),
                    ServiceTypeVM: ($(this).closest("tr").find("#ServiceType").text()).trim(),
                    JobProcessingStatusVM: ($(this).closest("tr").find("#Status").text()).trim(),
                    RoleVM: ($(this).closest("tr").find("#Role").text()).trim(),
                    JobType: ($(this).closest("tr").find("#JobType").text()).trim(),
                    AssociateNameVM: ($(this).closest("tr").find("#Name").text()).trim()
                });

            });

            $.ajax(
                  {
                      method: "POST",
                      url: AppPath + 'AdminDashBoard/OnHoldManuscript',
                      async: false,
                      cache: false,
                      traditional: true,
                      contentType: "application/json; charset=utf-8",
                      data: JSON.stringify(userdata.adminFunctionalityEnum),
                      success: function (returnValue) {
                          if (returnValue.toLowerCase() == "true") {
                              alert("Job's are puted on-hold successfully");
                              window.location.reload(true);
                          } else {
                              alert("Job's is already on hold.");
                              location.reload(true);
                          }
                      },
                      error: function (xhr, exception) {
                          alert('Faild to put job on-hold');
                      }
                  });
        }
    });
    $(':checkbox:checked').removeAttr('checked');

});