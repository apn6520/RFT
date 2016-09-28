var counter = 0;
var JournalList = [];
var JournalsList = [];
var EmailAddressList = [];
var referenceLinkList = [];
var areaOfExpertiseList = [];
var ManuscriptTitleList = [];
$(document).ready(function () {
    jQuery('#loading').hide();
    $("#txtLastName").focus();
    $("#lblheading").text("Reviewer Profile");
    $("#divPopup").dialog({
        show: "slide", modal: true,
        autoOpen: false,
        width: '500px',
        height: 'auto',
    });
    $("#saveDialog").dialog({
        modal: true,
        autoOpen: false,
        width: '300px',
        height: 'auto',
        closeOnEscape: false,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
        }
    });
    var reviewerId = Number($('#ReviewerViewBagData').text());
    if (reviewerId > 0) {
        LoadProfileData(reviewerId);
    } else {
        $("#otherExpertiesDiv").remove();
        //Plug-in function for the bootstrap version of the multiple email
        $(function () {
            //To render the input device to multiple email input using a simple hyperlink text
            $('#txtEmailAddress').multiple_emails({ theme: "Basic" });
            //Shows the value of the input device, which is in JSON format
            $('#current_emails').text($('#txtEmailAddress').val());
            $('#txtEmailAddress').change(function () {
                $('#current_emails').text($(this).val());
            });
        });
    }
    $("#referenceLinkDiv").focusin(function (e) {
        $("#referenceLinkDiv").css('border', '2px solid #A6C8FF');
    });
    $("#referenceLinkDiv").focusout(function (e) {
        $("#referenceLinkDiv").css('border', '1px solid #ccc');
    });
 
    
    $("#RefLink").keydown(function (e) {        
        var keynum;
        if (window.event) { // IE					
            keynum = e.keyCode;
        }
        else if (e.which) { // Netscape/Firefox/Opera					
            keynum = e.which;
        }
        // Supported key press is tab, enter, space or comma, there is no support for semi-colon since the keyCode differs in various browsers
        if (keynum == 13 || keynum == 9) {
            counter++;
            var refText = $("#RefLink").val();
            if (refText != "") {
                var manuScriptData = { ID: counter, ReferenceLink: refText };
                $("#referenceUL").append($("#ReferenceGridTemplate").render(manuScriptData));
            }           
            $("#RefLink").val("");
        }
    });

    jQuery('#txtNumberofrelevantpublications').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });

    $("#txtJournalSearch").keyup(function (e) {      
        $.ajax({
            url: AppPath + "/ReviewerIndex/GetJournal",
            type: "POST",
            dataType: "json",
            data: { term: e.target.value },
            success: function (result) {
                $('#ddlJournalName').empty();
                $.each(result, function (i, value) {
                    $('#ddlJournalName').append($('<option>').text(value.JournalTitle).attr('value', value.ID));
                });
            }
        })
    });
    

    $("#ddlJournalName").dblclick(function (e) {      
        var flag = true;
        $('#JournalNameUL .email_name').each(function (j, li) {        
            if (li.textContent == e.target.text) {
                flag = false;
            } 
        });
        if (flag) {
            var manuScriptData = { JournalID: e.target.value, JournalTitle: e.target.text };
            $("#JournalNameUL").append($("#JournalTemplate").render(manuScriptData));
        } else {        
            ShowFailureResponseMessage("The Journal name '" + e.target.text + "' has already been added.");
        }
    });

    // here we can  add Journal name by entering from keyboard. its new enhancement in existing functionality
    $("#ddlJournalName").bind("keyup", function(e) {       
        var keynum;
        if (window.event) { // IE					
            keynum = e.keyCode;
        }
        else if (e.which) { // Netscape/Firefox/Opera					
            keynum = e.which;
        }
        if (keynum == 13) {
            var flag = true;
            var name = $(this.selectedOptions).text().trim();
            $('#JournalNameUL .email_name').each(function (j, li) {
                if (li.textContent == name) {
                    flag = false;
                }
            });         
            if (flag) {
                var manuScriptData = { JournalID: this.value, JournalTitle: name };
                if (name.trim() == "") {
                    ShowFailureResponseMessage("Please select Journal from the Journal list.");
                } else {
                    $("#JournalNameUL").append($("#JournalTemplate").render(manuScriptData));
                }               
               
            } else {
                ShowFailureResponseMessage("The Journal name '" + name + "' has already been added.");
            }
        }
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
    GetAutoCompleteData();  
});

function VerificationPopupOnSave()
{
    var institute = $("#txtAffiliation").val();
    var departmentName = $("#txtDepartmentName").val();
    var streetName = $("#txtStreetName").val();
    var countryName = $("#txtCountry").val();
    var stateName = $("#txtState").val();
    var cityName = $("#txtCity").val();

    $("#tdInstitute").text(institute);
    $("#tdInstitute").attr("title", institute); 
    $("#tdDepartment").text(departmentName);
    $("#tdDepartment").attr("title", departmentName);
    $("#tdStreet").text(streetName);
    $("#tdStreet").attr("title", streetName);
    $("#tdCountry").text(countryName);
    $("#tdCountry").attr("title", countryName);
    $("#tdState").text(stateName);
    $("#tdState").attr("title", stateName);
    $("#tdCity").text(cityName);
    $("#tdCity").attr("title", cityName);
    var isValid = ReviewerProfileValidation();
    if (isValid) {
        $("#divPopup").dialog('open');
    }
}

function CloseSavePopup()
{
    $("#divPopup").dialog('close');
}

function ReviewerSaveSuccess() {
    var id = $("#newReviewerId").val();   
    //location.reload();
    window.open(AppPath + "/ReviewerIndex/ReviewerProfile?reviewerId=" + id, "_self")
}

function SaveProfile() {
    $("#divPopup").dialog('close');
    $("body").append('<div class="modalOverlay">');
    var isValid = ReviewerProfileValidation();
    if (isValid) {
        jQuery('#loading').show();
        var initials = $("#ddlInitials").val();
        var lastName = $("#txtLastName").val();
        var firstName = $("#txtFirstName").val();
        var middleName = $("#txtMiddleName").val();
        //var email = $("#txtEmailAddress").val();
        var affiliation = $("#txtAffiliation").val();
        var departmentName = $("#txtDepartmentName").val();
        var streetName = $("#txtStreetName").val();
        var countryName = $("#txtCountry").val();
        var stateName = $("#txtState").val();
        var cityName = $("#txtCity").val();
        var numOfPublications = $("#txtNumberofrelevantpublications").val();
        var reviewerId = Number($('#ReviewerViewBagData').text());
        var instituteID = $("#txtAffiliationId").val();
        var deptId = $("#txtDepartmentId").val();
        var cityID = $("#txtCityId").val();
        var userID = 12;
        var stateId = $("#txtStateId").val();
        var countryID = $("#txtCountryId").val();
        var updatedEmailArray = [];
        var expertiseList = [];
        //updatedEmailArray = email.replace(/\"/g, "").replace("[", "").replace("]", "").split(',');
        $('#emailCollection li').each(function (j, li) {            
            updatedEmailArray.push(li.id);
        });
        
        $.each(JournalsList, function (i, item) {
            item.IsActive = false;
        });
        $('#JournalNameUL .email_name').each(function (j, li) {
            var result = $.grep(JournalsList, function (e) { return e.JournalID == li.id; });

            if (result.length == 0) {
                // not found
                JournalsList.push({ JournalID: li.id, JournalTitle: li.textContent, ReviewerMasterID: reviewerId, IsActive: true });
            }
            else {
                for (var i = 0; i < JournalsList.length; i++) {
                    if (JournalsList[i].JournalID == li.id) {
                        JournalsList[i].IsActive = true;
                    }
                }
            }

        });

        $.each(referenceLinkList, function (i, item) {
            item.IsActive = false;
        });
        $('#referenceUL .email_name').each(function (j, li) {
            var result = $.grep(referenceLinkList, function (e) { return e.ID == li.id; });
            if (result.length == 0) {
                // if not found then push object into the list
                referenceLinkList.push({ ID: 0, ReferenceLink: li.textContent, ReviewerMasterID: reviewerId, IsActive: true });
            }
            else {
                for (var i = 0; i < referenceLinkList.length; i++) {
                    if (referenceLinkList[i].ReferenceLink == li.textContent) {
                        referenceLinkList[i].IsActive = true;
                    }
                }
            }
        });


        $.each(areaOfExpertiseList, function (i, item) {
            item.IsActive = false;
        });
        var updatedExperties = [];
        $('#ExpertiseTable_tr tr').each(function (j, li) {
            var areaOfExpertiseMasterID = [];
            $(li).find('td').each(function () {
                areaOfExpertiseMasterID.push($(this).attr('id'));
            });
            updatedExperties.push({ PID: areaOfExpertiseMasterID[0], SID: areaOfExpertiseMasterID[1], TID: areaOfExpertiseMasterID[2], IsActive: true, ReviewerMasterID: reviewerId });
        });

        $.each(updatedExperties, function (i, item) {
            var result = $.grep(areaOfExpertiseList, function (e) { return e.PID == item.PID && e.SID == item.SID && e.TID == item.TID; });
            if (result.length == 0) {
                // if not found then push object into the list
                areaOfExpertiseList.push({ PID: item.PID, SID: item.SID, TID: item.TID, IsActive: true, ReviewerMasterID: reviewerId });
            }
            else {
                for (var i = 0; i < areaOfExpertiseList.length; i++) {
                    if (areaOfExpertiseList[i].PID == item.PID && areaOfExpertiseList[i].SID == item.SID && areaOfExpertiseList[i].TID == item.TID) {
                        areaOfExpertiseList[i].IsActive = true;
                    }
                }
            }
        });

        $.each(EmailAddressList, function (i, item) {
            item.IsActive = false;
        });
        $.each(updatedEmailArray, function (i, item) {
            var result = $.grep(EmailAddressList, function (e) { return e.Email == item; });
            if (result.length == 0) {
                // not found
                EmailAddressList.push({ ID: 0, Email: item, ReviewerMasterID: reviewerId, IsActive: true });
            }
            else {
                for (var i = 0; i < EmailAddressList.length; i++) {
                    if (EmailAddressList[i].Email == item) {
                        EmailAddressList[i].IsActive = true;
                    }
                }
            }
        }); 
        // EmailAddressList - we need to remove those items from list by id == 0 and isactive == false
        $.each(ManuscriptTitleList, function (i, item) {
            item.IsActive = false;
        });
        $('#ManuscriptDetails td').each(function (j, li) {
            var msScriptID = li.children[0].textContent;
            if (msScriptID.trim() != "") {
                var name = li.children[1].textContent;
                var titleID = li.children[1].id;
                var result = $.grep(ManuscriptTitleList, function (e) { return e.ID == titleID; });

                if (result.length == 0) {
                    // not found
                    ManuscriptTitleList.push({ ID: titleID, Name: name, MScriptID: msScriptID, ReviewerMasterID: reviewerId, IsActive: true });
                }
                else {
                    for (var i = 0; i < ManuscriptTitleList.length; i++) {
                        if (ManuscriptTitleList[i].ID == titleID) {
                            ManuscriptTitleList[i].IsActive = true;
                        }
                    }
                }
            }


        });


        var profileDetails = {
            ReviewerId: reviewerId,
            Initials: initials,
            LastName: lastName.trim(),
            FirstName: firstName.trim(),
            MiddleName: middleName.trim(),
            Name: initials + " " + firstName.trim() + " " + middleName.trim() + " " + lastName.trim(),
            InstituteID: instituteID,
            InstituteName: affiliation.trim(),
            DepartmentName: departmentName.trim(),
            DeptId: deptId,
            StreetName: streetName.trim(),
            Country: countryName.trim(),
            State: stateName.trim(),
            City: cityName.trim(),
            CityID: cityID,
            NoOfPublication: numOfPublications,
            UserID: userID,
            StateId: stateId,
            CountryID: countryID,
            ReviewerEmails: EmailAddressList,
            ReferenceLink: referenceLinkList,
            Journal: JournalsList,
            AreaOfExpReviewerlink: areaOfExpertiseList,
            TitleMaster: ManuscriptTitleList,
        }


        $.ajax(
         {
             type: "POST",
             url: AppPath + "/ReviewerIndex/SaveReviewerProfile",
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             data: JSON.stringify(profileDetails),
             success: function (result) {
                 jQuery('#loading').hide();
                 $('.modalOverlay').remove();               
                 $("#newReviewerId").val(result);
                 $("#saveDialog").dialog('open');                             
             },
             error: function (x, y, z) {
                 location.reload();
             }
         });
    }
}

function ReviewerProfileValidation()
{
    var flag = true;
    var journalList = [];
    var referenceLinkList = [];
    var expertiseList = [];
    var ManuscriptDetailsList = [];
       
    var lastName = $("#txtLastName").val();
    if (lastName.trim() == "") {
        flag = false;
        ShowFailureResponseMessage("Last name can not be blank.");
        $("#txtLastName").focus();
        return false;
    }
    var firstName = $("#txtFirstName").val();
    if (firstName.trim() == "") {
        flag = false;
        ShowFailureResponseMessage("First name can not be blank.");
        $("#txtFirstName").focus();
        return false;
    }
    var affiliation = $("#txtAffiliation").val();
    if (affiliation.trim() == "") {
        flag = false;
        ShowFailureResponseMessage("Institute name can not be blank.");
        $("#txtAffiliation").focus();
        return false;
    }
    var departmentName = $("#txtDepartmentName").val();
    if (departmentName.trim() == "") {
        flag = false;
        ShowFailureResponseMessage("Department name can not be blank.");
        $("#txtDepartmentName").focus();
        return false;
    }    
    var countryName = $("#txtCountry").val();
    if (countryName.trim() == "") {
        flag = false;
        ShowFailureResponseMessage("Country name can not be blank.");
        $("#txtCountry").focus();
        return false;
    }
    var stateName = $("#txtState").val();
    if (stateName.trim() == "") {
        flag = false;
        ShowFailureResponseMessage("State name can not be blank.");
        $("#txtState").focus();
        return false;
    }
    var cityName = $("#txtCity").val();
    if (cityName.trim() == "") {
        flag = false;
        ShowFailureResponseMessage("City name can not be blank.");
        $("#txtCity").focus();
        return false;
    }
    var email = $("#txtEmailAddress").val();   
    if (email.trim() == "" || email.trim() == "[]") {
        flag = false;
        ShowFailureResponseMessage("Email address can not be blank.");
        $("#txtEmailAddress").focus();
        return false;
    }
    $('#JournalNameUL .email_name').each(function (j, li) {
        journalList.push(li);
    });
    if (journalList.length == 0) {
        flag = false;
        ShowFailureResponseMessage("Journal Name can not be blank.");
        $("#txtJournalSearch").focus();
        return false;
    }
    $('#referenceUL .email_name').each(function (j, li) {
        referenceLinkList.push(li);
    });
    if (referenceLinkList.length == 0) {
        flag = false;
        ShowFailureResponseMessage("Reference link can not be blank.");
        $("#RefLink").focus();
        return false;
    }
    var numOfPublications = $("#txtNumberofrelevantpublications").val();
    if (numOfPublications.trim() == "") {
        flag = false;
        ShowFailureResponseMessage("Number of relevant publications can not be blank.");
        $("#txtNumberofrelevantpublications").focus();
        return false;
    }    
    $('#ExpertiseTable_tr td').each(function (j, li) {
        if (li.id != "") {
            expertiseList.push(li);
        }
    });
    if (expertiseList.length == 0) {
        flag = false;
        ShowFailureResponseMessage("Please add primary, secondary, tertiary area of expertise.");
        $("#ddlPrimary").focus();
        return false;
    }
    $('#ManuscriptDetails td').each(function (j, li) {
        if (li.children[0].textContent != "") {
            ManuscriptDetailsList.push(li);
        }
    });
    if (ManuscriptDetailsList.length == 0) {
        flag = false;
        ShowFailureResponseMessage("Manuscript Details can not be blank.");
        $("#txttitleTitle").focus();
        return false;
    }
    return flag;
}


function AddExpertiseInToTable() {
    counter++;
    var PrimaryExp = $("#ddlPrimary option:selected").text();
    var SecondaryExp = $("#ddlSecondary option:selected").text();
    var TertiaryExp = $("#ddlTertiary option:selected").text();

    var PID = $("#ddlPrimary option:selected").val();
    var SID = $("#ddlSecondary option:selected").val();
    var TID = $("#ddlTertiary option:selected").val();

    if (TertiaryExp == "--Select Tertiary--" || SecondaryExp == "--Select Secondary--" || PrimaryExp == "--Select Primary--") {       
        ShowFailureResponseMessage("Please select primary, secondary and tertiary area of expertise.");
    } else {
        var expertiseData = { PrimaryExp: PrimaryExp, SecondaryExp: SecondaryExp, TertiaryExp: TertiaryExp, PID: PID, SID: SID, TID: TID };

        var ExitsExperties = [];
        $('#ExpertiseTable_tr tr').each(function (j, li) {
            var areaOfExpertiseMasterID = [];
            $(li).find('td').each(function () {
                areaOfExpertiseMasterID.push($(this).attr('id'));
            });
            ExitsExperties.push({ PID: areaOfExpertiseMasterID[0], SID: areaOfExpertiseMasterID[1], TID: areaOfExpertiseMasterID[2]});
        });

        var result = $.grep(ExitsExperties, function (e) { return e.PID == PID && e.SID == SID && e.TID == TID; });
        if (result.length > 0) {
            ShowFailureResponseMessage("This combination of primary, secondary and tertiary area of expertise is already exists.");
            return false;
            // if result found then show alert message
            
        }

        $("#ExpertiseTable_tr").append($("#ExpertGridTemplate").render(expertiseData));
        $("#errorExpertise").text("");
    }

  
}
function AddManuscriptToTable() {
    counter--;
    var mscriptID = $("#txttitleId").val();
    var title = $("#txttitleTitle").val();
    if (mscriptID.trim() == "" || title.trim() == "") {
        ShowFailureResponseMessage("Manuscript Title & Manuscript Id are mandatory.");
        return false;
    } else {
        var data = [];
        $('#ManuscriptDetails td').each(function (j, li) {
            var msScriptID = li.children[0].textContent;
            if (msScriptID.trim() != "") {
                var name = li.children[1].textContent;
                var titleID = li.children[1].id;
                data.push({ ID: titleID, Name: name.trim(), MScriptID: msScriptID.trim() });
            }
        });
        var result = $.grep(data, function (e) { return e.MScriptID == mscriptID.trim() && e.Name == title.trim(); });
        if (result.length > 0) {
            ShowFailureResponseMessage("Manuscript Id & Title are already exists.");
            return false;
            // if result found then show alert message
        } else {
            var Id = $("#txttitleTitleId").val();
            if (Id == "") {
                Id = counter;
            }
            var manuScriptData = { ID: Id, MScriptID: mscriptID, Name: title };
            $("#ManuscriptDetails").append($("#ManuScriptDetailsTemplate").render(manuScriptData));
            $("#txttitleTitle").val("");
            $("#txttitleId").val("");
        }
        
    }

}
function removeManuScriptTableRow(id) {
    $("#" + id + "_ManuScript_tr").remove();   
}

function removeTableRow(id) {
    $("#" + id + "_tr").remove();
}

function removeRefTableRow(id) {
    $("#" + id + "_li").remove();   
}

function EditManuScriptDetails(li)
{
    //$('#txttitleId').attr('readonly', true);
    $("#updatePanel").css("display", "block");
    $("#addPanel").css("display", "none");
    var data = $(li).find('td span')
    $("#txttitleId").val(data[0].textContent);
    $("#txttitleTitle").val(data[1].textContent);
    $("#editTitleId").val(data[1].id);
}

function CancelManuscriptDetails()
{
    $('#txttitleId').attr('readonly', false);
    $("#addPanel").css("display", "block");
    $("#updatePanel").css("display", "none");
    $("#txttitleId").val("");
    $("#txttitleTitle").val("");
    $("#editTitleId").val("");
    
}

function UpdateManuscriptToTable()
{    
    var mScriptId = $("#txttitleId").val();
    var titleName = $("#txttitleTitle").val();
    var titleId = Number($("#editTitleId").val());
    if (mScriptId == "" || titleName == "") {
        ShowFailureResponseMessage("Manuscript Title & Manuscript Id are mandatory.");
        return false;
    } else {
        var manuScriptData = [];
        $('#ManuscriptDetails td').each(function (j, li) {
            var msScriptID = li.children[0].textContent;
            if (msScriptID.trim() != "") {
                var name = li.children[1].textContent;
                var titleID = li.children[1].id;
                manuScriptData.push({ ID: titleID, Name: name, MScriptID: msScriptID, IsActive: true });
            }
        });
        for (var i = 0; i < manuScriptData.length; i++) {       
            if (manuScriptData[i].ID == titleId) {
                manuScriptData[i].MScriptID = mScriptId;
                manuScriptData[i].Name = titleName;           
            }
        }
        $("#ManuscriptDetails").empty();
        $("#ManuscriptDetails").append($("#ManuScriptDetailsTemplate").render(manuScriptData));
        if (titleId > 0) {
            $.ajax({
                url: AppPath + "/ReviewerIndex/EditManuscriptTitle",
                type: 'GET',
                dataType: 'json',
                data: { titleId: titleId, mScriptID: mScriptId, name: titleName },
                success: function (response) {                 
                    var Id = Number($("#editTitleId").val());
                    for (var i = 0; i < ManuscriptTitleList.length; i++) {
                        if (ManuscriptTitleList[i].ID == Id) {
                            ManuscriptTitleList[i].MScriptID = mScriptId;
                            ManuscriptTitleList[i].Name = titleName;
                        }
                    }
                    CancelManuscriptDetails();
                },
                error: function (xhr) {
                }
            });
        } else {
            CancelManuscriptDetails();
        }
       
    }
}
function LoadProfileData(reviewerId) {   
    $.ajax({
        url: AppPath + "/ReviewerIndex/LoadProfileData",
        type: 'GET',
        dataType: 'json',
        data: { reviewerId: reviewerId },
        success: function (response) {
            if (response.records != null) {
                BindDataToControl(response.records);                
            }         
        },
        error: function (xhr) {
        }
    }); 
}

function BindDataToControl(response) {
    var emailList = '["emailString"]';
    var emailString = [];
    EmailAddressList = [];
    if (response.ReviewerEmails.length > 0) {
        for (var i = 0; i < response.ReviewerEmails.length; i++) {
            emailString.push(response.ReviewerEmails[i].Email);
            EmailAddressList.push({ ID: response.ReviewerEmails[i].ID, Email: response.ReviewerEmails[i].Email, ReviewerMasterID: response.ReviewerDetails_Result.ReviewerID, IsActive: false });
        }
    }    
    emailList = emailList.replace("emailString", emailString.join('","'));

    $("#txtEmailAddress").val(emailList);
    //Plug-in function for the bootstrap version of the multiple email
    $(function () {
        //To render the input device to multiple email input using a simple hyperlink text
        $('#txtEmailAddress').multiple_emails({ theme: "Basic" });
        //Shows the value of the input device, which is in JSON format
        $('#current_emails').text($('#txtEmailAddress').val());
        $('#txtEmailAddress').change(function () {
            $('#current_emails').text($(this).val());
        });
    });    
    if (response.AreaOfExpertise == 0) {
        $('#otherExperties').text("Not Available");
    }
    $.each(response.AreaOfExpertise, function (i, value) {
        $('#otherExperties').text(value.Name);
    });    
    $("#ddlInitials").val(response.ReviewerDetails_Result.Initials)
    $("#txtLastName").val(response.ReviewerDetails_Result.LastName);
    $("#txtFirstName").val(response.ReviewerDetails_Result.FirstName);
    $("#txtMiddleName").val(response.ReviewerDetails_Result.MiddleName);     
    $("#txtAffiliation").val(response.ReviewerDetails_Result.InstituteName);
    $("#txtDepartmentName").val(response.ReviewerDetails_Result.DepartmentName);
    $("#txtStreetName").val(response.ReviewerDetails_Result.StreetName);
    $("#txtCountry").val(response.ReviewerDetails_Result.Country);
    $("#txtState").val(response.ReviewerDetails_Result.State);
    $("#txtCity").val(response.ReviewerDetails_Result.City);
    $("#txtNumberofrelevantpublications").val(response.ReviewerDetails_Result.NoOfPublication);
    $("#JournalNameUL").empty();
    $("#JournalNameUL").append($("#JournalTemplate").render(response.Journal));
    $("#referenceUL").empty();
    $("#referenceUL").append($("#ReferenceGridTemplate").render(response.ReferenceLink));
    $("#ExpertiseTable_tr").empty();
    $("#ExpertiseTable_tr").append($("#ExpertGridTemplate").render(response.AreaOfExpReviewerlink));
    $("#ManuscriptDetails").empty();
    $("#ManuscriptDetails").append($("#ManuScriptDetailsTemplate").render(response.TitleMaster));
    $("#txtAffiliationId").val(response.ReviewerDetails_Result.InstituteID);
    $("#txtDepartmentId").val(response.ReviewerDetails_Result.DeptID);
    $("#txtCityId").val(response.ReviewerDetails_Result.CityId);  
    $("#txtCountryId").val(response.ReviewerDetails_Result.CountryID);
    $("#txtStateId").val(response.ReviewerDetails_Result.StateId);
    JournalsList = response.Journal; 
    referenceLinkList = response.ReferenceLink;
    areaOfExpertiseList = response.AreaOfExpReviewerlink;
    ManuscriptTitleList = response.TitleMaster; 
}

function CreateNewProfile() {
    window.location = AppPath + "/ReviewerIndex/ReviewerProfile?reviewerId=0";
}

function CancelProfile() {
    window.location = AppPath + "/ReviewerIndex/ReviewerIndexSearch";
}
function ddlPrimaryChange(e)
{
    $.ajax({
        url: AppPath + "/ReviewerIndex/GetSecondaryExpertise",
        type: "POST",
        dataType: "json",
        data: { primaryParentID: e.value },
        success: function (result) {
            $('#ddlSecondary').empty();
            $('#ddlSecondary').append($('<option>').text("--Select Secondary--").attr('value', -1));
            $.each(result, function (i, value) {
                $('#ddlSecondary').append($('<option>').text(value.Name).attr('value', value.ID));
            });
            $('#ddlTertiary').empty();
            $('#ddlTertiary').append($('<option>').text("--Select Tertiary--").attr('value', -1));
        }
    })

}

function ddlSecondaryChange(e) {
    $.ajax({
        url: AppPath + "/ReviewerIndex/GetTertiaryExpertise",
        type: "POST",
        dataType: "json",
        data: { secondaryParentID: e.value },
        success: function (result) {
            $('#ddlTertiary').empty();
            $('#ddlTertiary').append($('<option>').text("--Select Tertiary--").attr('value', -1));
            $.each(result, function (i, value) {
                $('#ddlTertiary').append($('<option>').text(value.Name).attr('value', value.ID));
            });
        }
    })
}

function GetAutoCompleteData()
{
    $("#txtAffiliation").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: AppPath + "/ReviewerIndex/AutoCompleteInstitute",
                type: "POST",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    $("#txtAffiliation").removeClass("ui-autocomplete-loading");
                    response($.map(data, function (item) {
                        return { label: item.Name, value: item.ID };
                    }))
                    $("#txtAffiliationId").val(null);
                }
            })
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#txtAffiliation").val(ui.item.label);
        },
        select: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.label);
            $("#txtAffiliationId").val(ui.item.value);
        },
        messages: {
            noResults: "", results: ""
        }
    });

    $("#txtDepartmentName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: AppPath + "/ReviewerIndex/AutoCompleteDepartment",
                type: "POST",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    $("#txtDepartmentName").removeClass("ui-autocomplete-loading");
                    response($.map(data, function (item) {
                        return { label: item.Name, value: item.ID };
                    }))
                    $("#txtDepartmentId").val(null);
                }
            })
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#txtDepartmentName").val(ui.item.label);
        },
        select: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.label);
            $("#txtDepartmentId").val(ui.item.value);
        },
        messages: {
            noResults: "", results: ""
        }
    });

    $("#txtCountry").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: AppPath + "/ReviewerIndex/AutoCompleteCountry",
                type: "POST",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    $("#txtCountry").removeClass("ui-autocomplete-loading");
                    response($.map(data, function (item) {
                        return { label: item.Name, value: item.ID };
                    }))
                    $("#txtCountryId").val(null);
                }
            })
        },
        focus: function(event, ui){
            event.preventDefault();
            $("#txtCountry").val(ui.item.label);
        },
        select: function (event, ui) {         
            event.preventDefault();   
            $(this).val(ui.item.label);
            $("#txtCountryId").val(ui.item.value);
            $("#txtState").val("");
            $("#txtStateId").val("");
            $("#txtCity").val("");
            $("#txtCityId").val("");
        },
        messages: {
            noResults: "", results: ""
        }
        
    });

    $("#txtState").autocomplete({       
        source: function (request, response) {
            var countryId = $("#txtCountryId").val();           
            if (countryId == "") {
                countryId = 0;
            }
            $.ajax({
                url: AppPath + "/ReviewerIndex/AutoCompleteState",
                type: "POST",
                dataType: "json",
                data: { term: request.term,parentPrefix: countryId },
                success: function (data) {
                    $("#txtState").removeClass("ui-autocomplete-loading");
                    response($.map(data, function (item) {
                        return { label: item.Name, value: item.ID };
                    }))
                    $("#txtStateId").val(null);
                }
            })
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#txtState").val(ui.item.label);
        },
        select: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.label);
            $("#txtStateId").val(ui.item.value);
            $("#txtCity").val("");
            $("#txtCityId").val("");
        },
        messages: {
            noResults: "", results: ""
        }
    });

    $("#txtCity").autocomplete({
        source: function (request, response) {
            var stateId = $("#txtStateId").val();
            if (stateId == "") {
                stateId = 0;
            }
            $.ajax({
                url: AppPath + "/ReviewerIndex/AutoCompleteCity",
                type: "POST",
                dataType: "json",
                data: { term: request.term, parentPrefix: stateId },
                success: function (data) {
                    $("#txtCity").removeClass("ui-autocomplete-loading");
                    response($.map(data, function (item) {
                        return { label: item.Name, value: item.ID };
                    }))
                    $("#txtCityId").val(null);
                }
            })
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#txtCity").val(ui.item.label);
        },
         select: function (event, ui) {
             event.preventDefault();
             $(this).val(ui.item.label);
             $("#txtCityId").val(ui.item.value);
         },
        messages: {
            noResults: "", results: ""
        }
    });
    $("#txttitleTitle").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: AppPath + "/ReviewerIndex/AutoCompleteManuscriptTitle",
                type: "POST",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    $("#txttitleTitle").removeClass("ui-autocomplete-loading");
                    response($.map(data, function (item) {
                        return { label: item.Name, value: item.TitleID };
                    }))
                    //$("#txttitleTitleId").val(null);
                }
            })
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#txttitleTitle").val(ui.item.label);
        },
        select: function (event, ui) {
            event.preventDefault();
            $(this).val(ui.item.label);
            $("#txttitleTitleId").val(ui.item.value);
        },
        messages: {
            noResults: "", results: ""
        }
    });

    // Load JournalName dropdown on page load -> following code will loda all journals on page load
    var journalSearch = $("#txtJournalSearch").val();
    $.ajax({
        url: AppPath + "/ReviewerIndex/GetJournal",
        type: "POST",
        dataType: "json",
        data: { term: journalSearch },
        success: function (result) {           
            $('#ddlJournalName').empty();
            $.each(result, function (i, value) {
                $('#ddlJournalName').append($('<option>').text(value.JournalTitle).attr('value', value.ID));
            });
        }
    })

    $.ajax({
        url: AppPath + "/ReviewerIndex/GetPrimaryExpertise",
        type: "POST",
        dataType: "json",     
        success: function (result) {
            $('#ddlPrimary').empty();
            $('#ddlPrimary').append($('<option>').text("--Select Primary--").attr('value', -1));
            $.each(result, function (i, value) {
                $('#ddlPrimary').append($('<option>').text(value.Name).attr('value', value.ID));
            });
        }
    })    
    
}