var IsEFax = localStorage.getItem("IsEFax");
var sCategoryPhy = null;

$(document).ready(function () {

    var IsEnableGrid = localStorage.getItem("IsEnableGrid");

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    //CAP-803

    //CAP-1450
    setTimeout(function () {
        $("#txtFax")?.mask("(999)999-9999", { placeholder: "(___)___-____" });
        $("#txtPhone")?.mask("(999)999-9999", { placeholder: "(___)___-____" });
        $('#txtZip')?.mask("99999-9999", { placeholder: "_____-____" });
    }, 2000);

    if (IsEFax == "true") {
        if (lblFax.innerText == "Fax#") {
            lblFax.innerText += "*";
            lblFax.innerHTML = lblFax.innerText;
        }
        $('#lblFax').addClass('MandLabelstyle');
    }
    $.ajax({
        type: 'POST',
        url: "WebServices/PhysicianLibraryServices.asmx/LoadCategoryAndSpecialites",
        dataType: 'json',
        data: '',
        contentType: "application/json;charset=utf-8",
        async: false,
        success: function (data) {
            var SpecialtyList = $.parseJSON(data.d).SpecialtyList;
            var CategoryList = $.parseJSON(data.d).CategoryList;
            var FacilityNameList = $.parseJSON(data.d).FacilityNameList;
            if (SpecialtyList.length > 0) {
                var specialty = '<table><tbody style=" height: 100px; display: inline-block; width: 100%; overflow: auto;">';
                for (var i = 0; i < SpecialtyList.length; i++) {
                    specialty += '<tr><td><label><input type="checkbox" id="' + SpecialtyList[i].Specialty.replace('&', '').replace('.', '').replace(',', '').replace(/\s/g, '') + '"/>' + SpecialtyList[i].Specialty + '</label></td></tr>';
                    
                }
                specialty += '</tbody></table>';
                $("#divSpecialities").append(specialty);
            }

            if (FacilityNameList.length > 0) {
                var Facility = '<table><tbody style=" height: 100px; display: inline-block; width: 100%; overflow: auto;">';
                for (var i = 0; i < FacilityNameList.length; i++) {
                    Facility += '<tr><td><label><input type="checkbox" id="' + FacilityNameList[i].FacilityName.replace('&', '').replace('.', '').replace(',', '').replace('#', '').replace('-', '').replace(/\s/g, '') + '" />' + FacilityNameList[i].FacilityName + '</label></td></tr>';
                }
                Facility += '</tbody></table>';
                $("#divFacility").append(Facility);
            }
            if (CategoryList.length > 0) {
                $("#ddlCategory option").remove();
                var select = document.getElementById("ddlCategory");
                for (var i = 0; i < CategoryList.length; i++) {
                    var option = document.createElement('option');
                    option.text = option.value = CategoryList[i].Category;
                    select.add(option);
                    if (sCategoryPhy == null)
                        sCategoryPhy =  CategoryList[i].Category.trim() ;
                    else
                        sCategoryPhy = sCategoryPhy + "," + CategoryList[i].Category.trim() ;

                }
                var sCategory = "NON CAPELLA USER(Physician)";
                if (IsEFax == "true") {
                    $("#ddlPhysicianType")[0].disabled = true;
                    DisableorEnableFacility(true);
                }
                else {
                    $("#ddlPhysicianType")[0].disabled = false;
                    DisableorEnableFacility(false);
                }
                for (var x = 0; x < select.length - 1 ; x++) {
                    if (select.options[x].text.toUpperCase().split(" (PHYSICIAN / PA / NP)")[0] == sCategory)
                        select.selectedIndex = x;
                }
            }
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
            }
        }
    });
    $('#tblPhysicianDetails thead tr th').click(function () {
        //grab all header rows
        $('#tblPhysicianDetails thead tr th').each(function (column) {
            $(this).addClass('sortable').click(function () {
                var findSortKey = function ($cell) {
                    return $cell.find('.sort-key').text().toUpperCase() + ' ' + $cell.text().toUpperCase();

                };
                var sortDirection = $(this).is('.sorted-asc') ? -1 : 1;
                var $rows = $(this).parent().parent().parent().find('tbody tr').get();
                var bob = 0;
                //loop through all the rows and find
                $.each($rows, function (index, row) {
                    row.sortKey = findSortKey($(row).children('td').eq(column));
                });

                //compare and sort the rows alphabetically or numerically
                $rows.sort(function (a, b) {
                    if (a.sortKey.indexOf('-') == -1 && (!isNaN(a.sortKey) && !isNaN(a.sortKey))) {
                        //Rough Numeracy check                          

                        if (parseInt(a.sortKey) < parseInt(b.sortKey)) {
                            return -sortDirection;
                        }
                        if (parseInt(a.sortKey) > parseInt(b.sortKey)) {
                            return sortDirection;
                        }

                    } else {
                        if (a.sortKey < b.sortKey) {
                            return -sortDirection;
                        }
                        if (a.sortKey > b.sortKey) {
                            return sortDirection;
                        }
                    }
                    return 0;
                });

                //add the rows in the correct order to the bottom of the table
                $.each($rows, function (index, row) {
                    $('#tblPhysicianDetails tbody').append(row);
                    row.sortKey = null;
                });

                //identify the collumn sort order
                $('#tblPhysicianDetails thead tr th').removeClass('sorted-asc sorted-desc');
                var $sortHead = $('#tblPhysicianDetails thead tr th').filter(':nth-child(' + (column + 1) + ')');
                sortDirection == 1 ? $sortHead.addClass('sorted-asc') : $sortHead.addClass('sorted-desc');

                //identify the collum to be sorted by
                $('td').removeClass('sorted').filter(':nth-child(' + (column + 1) + ')').addClass('sorted');
            });
        });
    });
    if (IsEnableGrid != null && IsEnableGrid != undefined) {
        if (IsEnableGrid == "false") {
            $("#divgrd").hide();
            $("#divMain").css("height", "275px")
        }
        else {
            $("#divgrd").show();
            $("#divMain").css("height", "100%")
            LoadGrid();
        }
    }
    ddlCategory_Change();
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //Input Mask for landline phone number

});

function LoadGrid() {
    var WSData = JSON.stringify({
        Category: sCategoryPhy
    });
    $.ajax({
        type: 'POST',
        url: "WebServices/PhysicianLibraryServices.asmx/LoadPhysicianLibrary",
        data: WSData,
        contentType: "application/json;charset=utf-8",
        async: false,
        success: function (data) {

            var objdata = $.parseJSON(data.d);
            var tabContents;
            if (objdata.length > 0) {
                for (var i = 0; i < objdata.length; i++) {
                    $('#tblPhysicianDetails tbody tr').remove();
                    var vName = '';
                    var vOrganization = '';
                    if (objdata[i].Category.toUpperCase() == "ORGANIZATION")
                    {
                        vName = objdata[i].Company;
                        vOrganization = objdata[i].Name;
                    }
                    else
                    {
                        vOrganization = objdata[i].Company;
                        vName = objdata[i].Name;
                    }
                    tabContents += "<tr class='Gridbodystyle'>"
                       + "<td style='width:5%;'><img src=" + "Resources/edit.gif" + " onclick='Update(this);'" + "/></td>"
                       + "<td style='width:15%;'>" + objdata[i].Category + "</td>"
                       + "<td style='width:20%;'>" + vName + "</td>"
                       + "<td style='width:28%;display:none;'>" + objdata[i].Specialty + "</td>"
                       + "<td style='width:10%;'>" + objdata[i].NPI + "</td>"
                       + "<td style='width:25%;display:none;'>" + objdata[i].Facility + "</td>"
                       + "<td style='display:none;'>" + objdata[i].Physician_Library_ID + "</td>"
                       + "<td style='display:none;'>" + objdata[i].Physician_Type + "</td>"
                       + "<td style='display:none;'>" + vOrganization + "</td>"
                       + "<td style='display:none;'>" + objdata[i].Physician_Address1 + "</td>"
                       + "<td style='display:none;'>" + objdata[i].Physician_Address2 + "</td>"
                       + "<td style='display:none;'>" + objdata[i].Physician_City + "</td>"
                       + "<td style='display:none;'>" + objdata[i].Physician_State + "</td>"
                       + "<td style='display:none;'>" + objdata[i].Physician_Zip + "</td>"
                       + "<td style='display:none;'>" + objdata[i].Physician_Telephone + "</td>"
                       + "<td style='display:none;'>" + objdata[i].Physician_Fax + "</td>"
                       + "<td style='display:none;'>" + objdata[i].Physician_EMail + "</td></tr>";
                }
                $("#tblPhysicianDetails").find('tbody').append(tabContents);
            }
            scrolify($('#tblPhysicianDetails'), 132);
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
            }
        }
    });
}

//Check with fax
function Add(sCheckDuplicatefax) {
    var Specialtylist = '';
    var FacilityList = '';
    $('#divSpecialities input:checked').each(function () {
        Specialtylist += ($(this))[0].labels[0].innerText + ",";
    });

    $('#divFacility input:checked').each(function () {
        FacilityList += ($(this))[0].labels[0].innerText + "|";
    });
    if ($("#txtLastName")[0].value != "") {
        var Returnval = AvoidSpecailCharacteronsave($("#txtLastName")[0].value, $("#txtLastName"));
        if (!Returnval) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return Returnval;
        }
    }
    if ($("#txtFirstName")[0].value != "") {
        var Returnval = AvoidSpecailCharacteronsave($("#txtFirstName")[0].value, $("#txtFirstName"));
        if (!Returnval) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return Returnval;
        }
    }
    if ($("#txtSuffix")[0].value != "") {
        var Returnval = AvoidSpecailCharacteronsave($("#txtSuffix")[0].value, $("#txtSuffix"));
        if (!Returnval) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return Returnval;
        }
    }
    if ($("#txtNPI")[0].value != "") {
        var Returnval = isNumberKeyonsave($("#txtNPI")[0].value, $("#txtNPI"));
        if (!Returnval) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return Returnval;
        }
    }
    if ($("#txtCompany")[0].value != "") {
        var Returnval = AvoidSpecailCharacteronsave($("#txtCompany")[0].value, $("#txtCompany"));
        if (!Returnval) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return Returnval;
        }
    }
    if ($("#txtCity")[0].value != "") {
        var Returnval = AvoidSpecailCharacteronsave($("#txtCity")[0].value, $("#txtCity"));
        if (!Returnval) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return Returnval;
        }
    }
   
    if ($("#txtState")[0].value != "") {
        var Returnval = AllowCharacteronsave($("#txtState")[0].value, $("#txtState"));
        if (!Returnval) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return Returnval;
        }
    }
    if ($("#txtEmail")[0].value != "") {
        var Returnval = AllowMailCharacteronsave($("#txtEmail")[0].value, $("#txtEmail"));
        if (!Returnval) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return Returnval;
        }
    }


    if ($("#ddlCategory")[0].value.toUpperCase().split(" (PHYSICIAN / PA / NP)")[0] == "PROVIDER") {

        if ($("#txtLastName")[0].value == "") {
            DisplayErrorMessage('1011167');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }

        if ($("#txtFirstName")[0].value == "") {
            DisplayErrorMessage('1011165');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        //Gitlab #2862
        //if ($("#txtNPI")[0].value == "") {
        //    DisplayErrorMessage('1011179');
        //    document.getElementById(GetClientId("txtNPI")).focus();
        //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //    return false;
        //}
        if (Specialtylist == "") {
            DisplayErrorMessage('1011168');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (FacilityList == "") {
            DisplayErrorMessage('1011169');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    if ($("#ddlCategory")[0].value.toUpperCase() == "NON CAPELLA USER(PHYSICIAN)") {
        if ($("#txtLastName")[0].value == "") {
            DisplayErrorMessage('1011167');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if ($("#txtFirstName")[0].value == "") {
            DisplayErrorMessage('1011165');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        //if ($("#txtNPI")[0].value == "") {
        //    DisplayErrorMessage('1011179');
        //    document.getElementById(GetClientId("txtNPI")).focus();
        //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //    return false;
        //}
    }
    if ($("#ddlCategory")[0].value.toUpperCase() == "ORGANIZATION") {
        if ($("#txtCompany")[0].value == "") {
            DisplayErrorMessage('1011166');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    if ($("#ddlCategory")[0].value.toUpperCase() != "PROVIDER" && $("#ddlCategory")[0].value.toUpperCase() != "ORGANIZATION") {
        if ($("#txtLastName")[0].value == "") {
            DisplayErrorMessage('1011167');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if ($("#txtFirstName")[0].value == "") {
            DisplayErrorMessage('1011165');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    if (document.getElementById(GetClientId("txtEmail")).value.length != 0 && IsEmail(document.getElementById(GetClientId("txtEmail")).value) == false) {

        DisplayErrorMessage('320010');
        document.getElementById(GetClientId("txtEmail")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if ($("#txtNPI")[0].value.length > 0 && $("#txtNPI")[0].value.length < 10) {
        DisplayErrorMessage('1011175');
        document.getElementById(GetClientId("txtNPI")).focus();
        return false;
    }
    if (document.getElementById("txtZip").value.length != 0 && document.getElementById("txtZip").value != "_____-____") {
        var str = document.getElementById("txtZip").value;
        if (str.replace(/_/gi, "").length != 5 && str.replace(/_/gi, "").length != 10) {
            DisplayErrorMessage('420050');
            document.getElementById(GetClientId("txtZip")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var sCategory = '';
    if ($("#ddlCategory")[0].value.toUpperCase().split(" (PHYSICIAN / PA / NP)")[0] == "PROVIDER") {
        sCategory = $("#ddlCategory")[0].value.toUpperCase().split(" (PHYSICIAN / PA / NP)")[0];
    }
    else {
        sCategory = $("#ddlCategory")[0].value;
    }
    if (IsEFax == "true") {
        if ($("#txtFax")[0].value == "") {
            DisplayErrorMessage('1011185');
            document.getElementById(GetClientId("txtFax")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
   
            var vgrd = $('#divgrd').css('display');
            var WSData = JSON.stringify({
        Category: sCategory,
        Prefix: $("#ddlPrefix")[0].value,
        Specialty: Specialtylist,//$("#ddlSpecialty")[0].value,
        Facility: FacilityList,//$("#ddlFacility")[0].value,
        PhyLastName: $("#txtLastName")[0].value,
        PhyFirstName: $("#txtFirstName")[0].value,
        Suffix: $("#txtSuffix")[0].value,
        PhyNPI: $("#txtNPI")[0].value,
        Company: $("#txtCompany")[0].value,
        PhyAddress1: $("#txtAddressLine1")[0].value,
        PhyAddress2: $("#txtAddressLine2")[0].value,
        PhyCity: $("#txtCity")[0].value,
        PhyState: $("#txtState")[0].value,
        Zip: $("#txtZip")[0].value,
        Phone: $("#txtPhone")[0].value,
        Fax: $("#txtFax")[0].value,
        Email: $("#txtEmail")[0].value,
        ButtonType: document.getElementById('btnSave').innerText,
        PhysicianId: document.getElementById("hdnPhysicianId").value,
        PhysicianType: $("#ddlPhysicianType")[0].value,
        PhyMI: document.getElementById("txtMI").value,
        grddisplay: vgrd,
        CheckDuplicatefax: sCheckDuplicatefax,
        sCategory: sCategoryPhy
    });
    $.ajax({
        type: 'POST',
        url: "WebServices/PhysicianLibraryServices.asmx/AddProvider",
        data: WSData,
        contentType: "application/json;charset=utf-8",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            var tabContents;

            if (vgrd == "none" && objdata == "None") {
                DisplayErrorMessage('110020');
                Aftersave();
                var IsEnableGrid = localStorage.getItem("IsEnableGrid");
                if (IsEnableGrid != null && IsEnableGrid != undefined) {
                    if (IsEnableGrid == "false") {
                        $("#btnClose").click();
                    }
                }
            }
            else if (objdata != "" && objdata.length > 0) {
                if (objdata[0].ExistNPI != null) {
                    DisplayErrorMessage('1011170');
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return;
                }
                if (vgrd != "none") {
                    for (var i = 0; i < objdata.length; i++) {
                        $('#tblPhysicianDetails tbody tr').remove();
                        var vName = '';
                        var vOrganization = '';
                        if (objdata[i].Category.toUpperCase() == "ORGANIZATION") {
                            vName = objdata[i].Company;
                            vOrganization = objdata[i].Name;
                        }
                        else {
                            vOrganization = objdata[i].Company;
                            vName = objdata[i].Name;
                        }
                        tabContents += "<tr class='Gridbodystyle'>"
                           + "<td style='width:5%;'><img src=" + "Resources/edit.gif" + " onclick='Update(this);'" + "/></td>"
                           + "<td style='width:15%;'>" + objdata[i].Category + "</td>"
                           + "<td style='width:20%;'>" + vName + "</td>"
                           + "<td style='width:28%;display:none;'>" + objdata[i].Specialty + "</td>"
                           + "<td style='width:10%;'>" + objdata[i].NPI + "</td>"
                           + "<td style='width:25%;display:none;'>" + objdata[i].Facility + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_Library_ID + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_Type + "</td>"
                           + "<td style='display:none;'>" + vOrganization + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_Address1 + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_Address2 + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_City + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_State + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_Zip + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_Telephone + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_Fax + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_EMail + "</td></tr>";

                    }
                    $("#tblPhysicianDetails").find('tbody').append(tabContents);
                }
                scrolify($('#tblPhysicianDetails'), 132);
                DisplayErrorMessage('110020');
                Aftersave();
            }

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
            }
        }
    });
}


function AddProvider() {
    var Specialtylist = '';
    var FacilityList = '';
    $('#divSpecialities input:checked').each(function () {
        Specialtylist += ($(this))[0].labels[0].innerText + ",";
    });

    $('#divFacility input:checked').each(function () {
        FacilityList += ($(this))[0].labels[0].innerText + "|";
    });
    if ($("#txtLastName")[0].value != "") {
        var Returnval = AvoidSpecailCharacteronsave($("#txtLastName")[0].value, $("#txtLastName"));
        if (!Returnval)
            return Returnval;
    }
    if ($("#txtFirstName")[0].value != "") {
        var Returnval = AvoidSpecailCharacteronsave($("#txtFirstName")[0].value, $("#txtFirstName"));
        if (!Returnval)
            return Returnval;
    }
    if ($("#txtSuffix")[0].value != "") {
        var Returnval = AvoidSpecailCharacteronsave($("#txtSuffix")[0].value, $("#txtSuffix"));
        if (!Returnval)
            return Returnval;
    }
    if ($("#txtNPI")[0].value != "") {
        var Returnval = isNumberKeyonsave($("#txtNPI")[0].value, $("#txtNPI"));
        if (!Returnval)
            return Returnval;
    }
    if ($("#txtCompany")[0].value != "") {
        var Returnval = AvoidSpecailCharacteronsave($("#txtCompany")[0].value, $("#txtCompany"));
        if (!Returnval)
            return Returnval;
    }
    if ($("#txtCity")[0].value != "") {
        var Returnval = AvoidSpecailCharacteronsave($("#txtCity")[0].value, $("#txtCity"));
        if (!Returnval)
            return Returnval;
    }
   
    if ($("#txtState")[0].value != "") {
        var Returnval = AllowCharacteronsave($("#txtState")[0].value, $("#txtState"));
        if (!Returnval)
            return Returnval;
    }
    if ($("#txtEmail")[0].value != "") {
        var Returnval = AllowMailCharacteronsave($("#txtEmail")[0].value, $("#txtEmail"));
        if (!Returnval)
            return Returnval;
    }


    if ($("#ddlCategory")[0].value.toUpperCase().split(" (PHYSICIAN / PA / NP)")[0] == "PROVIDER") {

        if ($("#txtLastName")[0].value == "") {
            DisplayErrorMessage('1011167');
            return false;
        }

        if ($("#txtFirstName")[0].value == "") {
            DisplayErrorMessage('1011165');
            return false;
        }
        //if ($("#txtNPI")[0].value == "") {
        //    DisplayErrorMessage('1011179');
        //    document.getElementById(GetClientId("txtNPI")).focus();
        //    return false;
        //}
        if (Specialtylist == "") {
            DisplayErrorMessage('1011168');
            return false;
        }
        if (FacilityList == "") {
            DisplayErrorMessage('1011169');
            return false;
        }
    }
    if ($("#ddlCategory")[0].value.toUpperCase() == "NON CAPELLA USER(PHYSICIAN)") {
        if ($("#txtLastName")[0].value == "") {
            DisplayErrorMessage('1011167');
            return false;
        }
        if ($("#txtFirstName")[0].value == "") {
            DisplayErrorMessage('1011165');
            return false;
        }
        //if ($("#txtNPI")[0].value == "") {
        //    DisplayErrorMessage('1011179');
        //    document.getElementById(GetClientId("txtNPI")).focus();
        //    return false;
        //}
    }
    if ($("#ddlCategory")[0].value.toUpperCase() == "ORGANIZATION") {
        if ($("#txtCompany")[0].value == "") {
            DisplayErrorMessage('1011166');
            return false;
        }
    }
    if ($("#ddlCategory")[0].value.toUpperCase() != "PROVIDER" && $("#ddlCategory")[0].value.toUpperCase() != "ORGANIZATION") {
        if ($("#txtLastName")[0].value == "") {
            DisplayErrorMessage('1011167');
            return false;
        }
        if ($("#txtFirstName")[0].value == "") {
            DisplayErrorMessage('1011165');
            return false;
        }
    }
    if (document.getElementById(GetClientId("txtEmail")).value.length != 0 && IsEmail(document.getElementById(GetClientId("txtEmail")).value) == false) {

        DisplayErrorMessage('320010');
        document.getElementById(GetClientId("txtEmail")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if ($("#txtNPI")[0].value.length > 0 && $("#txtNPI")[0].value.length < 10) {
        DisplayErrorMessage('1011175');
        document.getElementById(GetClientId("txtNPI")).focus();
        return false;
    }
    if (document.getElementById("txtZip").value.length != 0 && document.getElementById("txtZip").value != "_____-____") {
        var str = document.getElementById("txtZip").value;
        if (str.replace(/_/gi, "").length != 5 && str.replace(/_/gi, "").length != 10) {
            DisplayErrorMessage('420050');
            document.getElementById(GetClientId("txtZip")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var sCategory = '';
    if ($("#ddlCategory")[0].value.toUpperCase().split(" (PHYSICIAN / PA / NP)")[0] == "PROVIDER") {
        sCategory = $("#ddlCategory")[0].value.toUpperCase().split(" (PHYSICIAN / PA / NP)")[0];
    }
    else {
        sCategory = $("#ddlCategory")[0].value;
    }
    if (IsEFax == "true") {
        if ($("#txtFax")[0].value == "") {
            DisplayErrorMessage('1011185');
            document.getElementById(GetClientId("txtFax")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
   
            var vgrd = $('#divgrd').css('display');
        
    var vFax = '';
    var WSData = JSON.stringify({
        Category: sCategory,
        Prefix: $("#ddlPrefix")[0].value,
        Specialty: Specialtylist,//$("#ddlSpecialty")[0].value,
        Facility: FacilityList,//$("#ddlFacility")[0].value,
        PhyLastName: $("#txtLastName")[0].value,
        PhyFirstName: $("#txtFirstName")[0].value,
        Suffix: $("#txtSuffix")[0].value,
        PhyNPI: $("#txtNPI")[0].value,
        Company: $("#txtCompany")[0].value,
        PhyAddress1: $("#txtAddressLine1")[0].value,
        PhyAddress2: $("#txtAddressLine2")[0].value,
        PhyCity: $("#txtCity")[0].value,
        PhyState: $("#txtState")[0].value,
        Zip: $("#txtZip")[0].value,
        Phone: $("#txtPhone")[0].value,
        Fax: $("#txtFax")[0].value,
        Email: $("#txtEmail")[0].value,
        ButtonType: document.getElementById('btnSave').innerText,
        PhysicianId: document.getElementById("hdnPhysicianId").value,
        PhysicianType: $("#ddlPhysicianType")[0].value,
        PhyMI: document.getElementById("txtMI").value,
        grddisplay: vgrd,
        CheckDuplicatefax: 'N',
        sCategory: sCategoryPhy
    });
    $.ajax({
        type: 'POST',
        url: "WebServices/PhysicianLibraryServices.asmx/AddProvider",
        data: WSData,
        contentType: "application/json;charset=utf-8",
        async: true,
        success: function (data) {
            var objdata = '';
            if (data == undefined && vFax!='')
            {
                objdata = vFax;
            }
            else
                objdata = $.parseJSON(data.d);
            var tabContents;
            if (objdata.includes("|")) {
                var Getobjval = objdata.split("|")[0]
            }
            else {
                var Getobjval = objdata;
            }
            if (vgrd == "none" && Getobjval == "None") {
                DisplayErrorMessage('110020');
                var IsEnableGrid = localStorage.getItem("IsEnableGrid");
                if (IsEnableGrid != null && IsEnableGrid != undefined) {
                    if (IsEnableGrid == "false") {
                        $("#btnClose").click();
                        var vProviderName = $("#ddlPrefix")[0].value + ". " + $("#txtFirstName")[0].value + " " + document.getElementById("txtMI").value + " " + $("#txtLastName")[0].value;
                        var vPhyNmae = $("#ddlPrefix")[0].value + $("#txtFirstName")[0].value + " " + document.getElementById("txtMI").value + " " + $("#txtLastName")[0].value + $("#txtSuffix")[0].value;
                        var vFullName = $("#ddlPrefix")[0].value + $("#txtFirstName")[0].value + " " + document.getElementById("txtMI").value + " " + $("#txtLastName")[0].value + "(" + $("#txtSuffix")[0].value + ")";
                        var PCP_PhyDetails = objdata.split("|")[1] + "&" + vProviderName + "&" + vPhyNmae + "&" + $("#txtNPI")[0].value +"&" +vFullName + "|" + "NPI:" + $("#txtNPI")[0].value + "|" + Specialtylist + "|" + "FACILITY:" + FacilityList + "|"
                            + "ADDR: " +$("#txtAddressLine1")[0].value + "," + $("#txtAddressLine2")[0].value + "," + $("#txtCity")[0].value + ","
                            + $("#txtState")[0].value + "," + $("#txtZip")[0].value
                            + "|" + "PH:" + $("#txtPhone")[0].value+ "FAX:" + $("#txtFax")[0].value ;

                        localStorage.setItem("PhyDetails", PCP_PhyDetails);
                    }
                }
                Aftersave();
            }
            else if (objdata != "" && objdata.length > 0) {
                if (objdata[0].ExistNPI != null) {
                    DisplayErrorMessage('1011170');
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return;
                }
                if (objdata[0].ExistFax != null) {
                    vFax = objdata;
                    if (DisplayErrorMessage('1011186') == true) {
                        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                        Add('Y');
                        //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return;
                    }
                    else
                    {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return;
                    }

                    var PCP_PhyDetails = objdata[i].Physician_Library_ID + "|" + vName + "|" + objdata[i].NPI + "|" + objdata[i].specialty + "|" + objdata[i].Facility + "|"
                        + objdata[i].Physician_Address1 + "," + objdata[i].Physician_Address2 + "," + objdata[i].Physician_City + "," + objdata[i].Physician_State + "," + objdata[i].Physician_Zip + "|"
                            + objdata[i].Physician_Fax + "|" + objdata[i].Physician_Telephone ;

                    localStorage.setItem("PhyDetails", PCP_PhyDetails);
                   
                }
                if (vgrd != "none") {
                    for (var i = 0; i < objdata.length; i++) {
                        $('#tblPhysicianDetails tbody tr').remove();

                        var vName = '';
                        var vOrganization = '';
                        if (objdata[i].Category.toUpperCase() == "ORGANIZATION")
                        {
                            vName = objdata[i].Company;
                            vOrganization = objdata[i].Name;
                        }
                        else
                        {
                            vOrganization = objdata[i].Company;
                            vName = objdata[i].Name;
                        }
                        tabContents += "<tr class='Gridbodystyle'>"
                           + "<td style='width:5%;'><img src=" + "Resources/edit.gif" + " onclick='Update(this);'" + "/></td>"
                           + "<td style='width:15%;'>" + objdata[i].Category + "</td>"
                           + "<td style='width:20%;'>" + vName + "</td>"
                           + "<td style='width:28%;display:none;'>" + objdata[i].Specialty + "</td>"
                           + "<td style='width:10%;'>" + objdata[i].NPI + "</td>"
                           + "<td style='width:25%;display:none;'>" + objdata[i].Facility + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_Library_ID + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_Type + "</td>"
                           + "<td style='display:none;'>" + vOrganization + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_Address1 + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_Address2 + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_City + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_State + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_Zip + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_Telephone + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_Fax + "</td>"
                           + "<td style='display:none;'>" + objdata[i].Physician_EMail + "</td></tr>";
                    }
                    $("#tblPhysicianDetails").find('tbody').append(tabContents);
                }
                scrolify($('#tblPhysicianDetails'), 132);
                DisplayErrorMessage('110020');
                Aftersave();
            }

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
            }
        }
    });
}
function closewindow() {
    $(top.window.document).find("#btnPhysicianLibraryClose").click();
}
function IsEmail(email) {
    var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return expr.test(email);

}

function AvoidSpecailCharacteronsave(soutput, id) {
    var regex = new RegExp("^[a-zA-Z0-9' t]+$");
    if (!regex.test(soutput)) {
        var sid = document.getElementById("lbl" + id[0].id.replace('txt', '')).innerText.replace('*', '');
        DisplayErrorMessage('1011180', '', sid);
        document.getElementById(GetClientId(document.getElementById("txt" + id[0].id.replace('lbl', '')))).focus();
        return false;
    }
    return true;
}
function AllowSpecailCharacteronsave(soutput, id) {
    var regex = new RegExp("^[a-zA-Z0-9'-:#., t'/']+$");
    if (!regex.test(soutput)) {
        var sid = document.getElementById("lbl" + id[0].id.replace('txt', '')).innerText.replace('*', '');
        DisplayErrorMessage('1011180', '', sid);
        document.getElementById(GetClientId(document.getElementById("txt" + id[0].id.replace('lbl', '')))).focus();
        return false;
    }
    return true;
}
function AllowCharacteronsave(soutput, id) {
    var regex = new RegExp("^[a-zA-Z]+$");
    if (!regex.test(soutput)) {
        var sid = document.getElementById("lbl" + id[0].id.replace('txt', '')).innerText.replace('*', '');
        DisplayErrorMessage('1011180', '', sid);
        document.getElementById(GetClientId(document.getElementById("txt" + id[0].id.replace('lbl', '')))).focus();
        return false;
    }
    return true;
}
function AllowMailCharacteronsave(soutput, id) {
    var regex = new RegExp("^[a-zA-Z0-9@_. t]+$");
    if (!regex.test(soutput)) {
        var sid = document.getElementById("lbl" + id[0].id.replace('txt', '')).innerText.replace('*', '');
        DisplayErrorMessage('1011180', '', sid);
        document.getElementById(GetClientId(document.getElementById("txt" + id[0].id.replace('lbl', '')))).focus();
        return false;
    }
    return true;
}
function isNumberKeyonsave(soutput, id) {
    var regex = new RegExp("^[0-9]+$");
    if (!regex.test(soutput)) {
        var sid = document.getElementById("lbl" + id[0].id.replace('txt', '')).innerText.replace('*', '');
        DisplayErrorMessage('1011180', '', sid);
        document.getElementById(GetClientId(document.getElementById("txt" + id[0].id.replace('lbl', '')))).focus();
        return false;
    }
    return true;
}
function AvoidSpecailCharacter(e) {
    
    var regex = new RegExp("^[a-zA-Z0-9'., t]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }
    e.preventDefault();
    return false;
}
function AllowSpecailCharacter(e) {
    var regex = new RegExp("^[a-zA-Z0-9'-:#., t'/']+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }
    e.preventDefault();
    return false;
}
function AllowCharacter(e) {
    var regex = new RegExp("^[a-zA-Z]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }
    e.preventDefault();
    return false;
}
function AllowMailCharacter(e) {
    var regex = new RegExp("^[a-zA-Z0-9@_. t]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }
    e.preventDefault();
    return false;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function scrolify(tblAsJQueryObject, height) {
    if (document.getElementById('dvAdd') != undefined)
        $('#dvAdd').remove();
    var oTbl = tblAsJQueryObject;
    var oTblDiv = $("<div id='dvAdd'/>");
    oTblDiv.css('height', height);
    oTblDiv.css('overflow', 'auto');
    oTblDiv.css('margin-top', '-20px');
    oTbl.wrap(oTblDiv);
    oTbl.attr("data-item-original-width", oTbl.width());
    oTbl.find('thead tr td').each(function () {
        $(this).attr("data-item-original-width", $(this).width());
    });
    oTbl.find('tbody tr:eq(0) td').each(function () {
        $(this).attr("data-item-original-width", $(this).width());
    });
    var newTbl = oTbl.clone();
    oTbl.find('thead tr').remove();
    newTbl.find('tbody tr').remove();

    oTbl.parent().parent().prepend(newTbl);
    newTbl.wrap("<div/>");
    newTbl.width(newTbl.attr('data-item-original-width'));
    newTbl.find('thead tr td').each(function () {
        $(this).width($(this).attr("data-item-original-width"));
    });
    oTbl.width(oTbl.attr('data-item-original-width'));
    oTbl.find('tbody tr:eq(0) td').each(function () {
        $(this).width($(this).attr("data-item-original-width"));
    });
    if (tblAsJQueryObject[0] != undefined) {
        if (tblAsJQueryObject[0].parentElement.parentElement.id == "GeneralQTable") {
            $("#ScrollIDGeneral").css('height', '');
            $("#ScrollIDGeneral").css('overflow-y', '');
        }
        else {
            $("#scrollID").css('height', '');
            $("#scrollID").css('overflow-y', '');
        }
    }
}
function ddlCategory_Change() {
    var lblFirstName = document.getElementById('lblFirstName');
    var lblLastName = document.getElementById('lblLastName');
    var lblNPI = document.getElementById('lblNPI');
    var lblCompany = document.getElementById('lblCompany');
    $('#divFacility input:checked').each(function () {
        ($(this))[0].checked = false;
    });
    $('#divSpecialities input:checked').each(function () {
        ($(this))[0].checked = false;
    });
    $("#ddlPhysicianType")[0].selectedIndex = 0;
    if ($("#ddlCategory")[0].value.toUpperCase().split(" (PHYSICIAN / PA / NP)")[0] == "PROVIDER") {
        if (lblFirstName.innerText != "First Name*") {
            lblFirstName.innerText += "*";
            lblFirstName.innerHTML = lblFirstName.innerText;
        }
        $('#lblFirstName').addClass('MandLabelstyle');
        $(lblFirstName).html($(lblFirstName).html().replace("*", "<span class='manredforstar'>*</span>"));
        if (lblLastName.innerText != "Last Name*") {
            lblLastName.innerText += "*";
            lblLastName.innerHTML = lblLastName.innerText;
        }
        $('#lblLastName').addClass('MandLabelstyle');
        $(lblLastName).html($(lblLastName).html().replace("*", "<span class='manredforstar'>*</span>"));

        //if (lblNPI.innerText != "NPI*") {
        //    lblNPI.innerText += "*";
        //    lblNPI.innerHTML = lblNPI.innerText;
        //}
        //$('#lblNPI').addClass('MandLabelstyle');
        //$(lblNPI).html($(lblNPI).html().replace("*", "<span class='manredforstar'>*</span>"));
        lblCompany.innerText = lblCompany.innerText.replace('*', ' ').trim();
        lblCompany.style.color = "black";
        $('#lblCompany').removeClass('MandLabelstyle');
        $('#lgndSpecialities').addClass('legendschedulerborderPhysician');
        $('#lgndSpecialities').removeClass('legendschedulerborderPhysicianNonMand');
        $('#lgndFacilityName').addClass('legendschedulerborderPhysician');
        $('#lgndFacilityName').removeClass('legendschedulerborderPhysicianNonMand');
        $("#ddlPhysicianType")[0].disabled = false;
        DisableorEnableFacility(false);
        DisableorEnableSpecialities(false);
    }
    else if ($("#ddlCategory")[0].value.toUpperCase().split(" (PHYSICIAN / PA / NP)")[0] == "NON CAPELLA USER(PHYSICIAN)") {
        if (lblFirstName.innerText != "First Name*") {
            lblFirstName.innerText += "*";
            lblFirstName.innerHTML = lblFirstName.innerText;
        }
        $('#lblFirstName').addClass('MandLabelstyle');
        $(lblFirstName).html($(lblFirstName).html().replace("*", "<span class='manredforstar'>*</span>"));
        if (lblLastName.innerText != "Last Name*") {
            lblLastName.innerText += "*";
            lblLastName.innerHTML = lblLastName.innerText;
        }
        $('#lblLastName').addClass('MandLabelstyle');
        $(lblLastName).html($(lblLastName).html().replace("*", "<span class='manredforstar'>*</span>"));

        //if (lblNPI.innerText != "NPI*") {
        //    lblNPI.innerText += "*";
        //    lblNPI.innerHTML = lblNPI.innerText;
        //}
        //$('#lblNPI').addClass('MandLabelstyle');
        //$(lblNPI).html($(lblNPI).html().replace("*", "<span class='manredforstar'>*</span>"));
        lblCompany.innerText = lblCompany.innerText.replace('*', ' ').trim();
        lblCompany.style.color = "black";
        $('#lblCompany').removeClass('MandLabelstyle');
        $('#lgndSpecialities').addClass('legendschedulerborderPhysicianNonMand');
        $('#lgndSpecialities').removeClass('legendschedulerborderPhysician');
        $('#lgndFacilityName').addClass('legendschedulerborderPhysicianNonMand');
        $('#lgndFacilityName').removeClass('legendschedulerborderPhysician');
        $("#ddlPhysicianType")[0].disabled = true;
        DisableorEnableFacility(true);
        DisableorEnableSpecialities(false);
    }
    else if ($("#ddlCategory")[0].value == "ORGANIZATION") {
        lblFirstName.innerText = lblFirstName.innerText.replace('*', ' ').trim();
        lblFirstName.style.color = "black";
        $('#lblFirstName').removeClass('MandLabelstyle');
        lblLastName.innerText = lblLastName.innerText.replace('*', ' ').trim();
        lblLastName.style.color = "black";
        $('#lblLastName').removeClass('MandLabelstyle');
        //lblNPI.innerText = lblNPI.innerText.replace('*', ' ').trim();
        //lblNPI.style.color = "black";
        //$('#lblNPI').removeClass('MandLabelstyle');
        if (lblCompany.innerText != "Company*") {
            lblCompany.innerText += "*";
            lblCompany.innerHTML = lblCompany.innerText;
        }
        $('#lblCompany').addClass('MandLabelstyle');
        $(lblCompany).html($(lblCompany).html().replace("*", "<span class='manredforstar'>*</span>"));
        $('#lgndSpecialities').addClass('legendschedulerborderPhysicianNonMand');
        $('#lgndSpecialities').removeClass('legendschedulerborderPhysician');
        $('#lgndFacilityName').addClass('legendschedulerborderPhysicianNonMand');
        $('#lgndFacilityName').removeClass('legendschedulerborderPhysician');
        $("#ddlPhysicianType")[0].disabled = true;
        DisableorEnableFacility(true);
        DisableorEnableSpecialities(true);
    }
    else {
        lblCompany.innerText = lblCompany.innerText.replace('*', ' ').trim();
        lblCompany.style.color = "black";
        $('#lblCompany').removeClass('MandLabelstyle');
        //lblNPI.innerText = lblNPI.innerText.replace('*', ' ').trim();
        //lblNPI.style.color = "black";
        //$('#lblNPI').removeClass('MandLabelstyle');
        if (lblFirstName.innerText != "First Name*") {
            lblFirstName.innerText += "*";
            lblFirstName.innerHTML = lblFirstName.innerText;
        }
        $('#lblFirstName').addClass('MandLabelstyle');
        $(lblFirstName).html($(lblFirstName).html().replace("*", "<span class='manredforstar'>*</span>"));
        if (lblLastName.innerText != "Last Name*") {
            lblLastName.innerText += "*";
            lblLastName.innerHTML = lblLastName.innerText;
        }
        $('#lblLastName').addClass('MandLabelstyle');
        $(lblLastName).html($(lblLastName).html().replace("*", "<span class='manredforstar'>*</span>"));
        $('#lgndSpecialities').addClass('legendschedulerborderPhysicianNonMand');
        $('#lgndSpecialities').removeClass('legendschedulerborderPhysician');
        $('#lgndFacilityName').addClass('legendschedulerborderPhysicianNonMand');
        $('#lgndFacilityName').removeClass('legendschedulerborderPhysician');
        $("#ddlPhysicianType")[0].disabled = true;
        DisableorEnableFacility(true);
        DisableorEnableSpecialities(true);
    }
}

function DisableorEnableFacility(bval) {
    var regExpr = /[^a-zA-Z0-9-s]/g;
    $('#divFacility input:checkbox').each(function () {
        var x = $(this).attr("id").replace(regExpr, "");
        if ($('#divFacility input:checkbox[id^=' + x + ']').length > 0)
            $('#divFacility input:checkbox[id^=' + x + ']')[0].disabled = bval;

    });

}

function DisableorEnableSpecialities(bval) {
    var regExpr = /[^a-zA-Z0-9-s]/g;
    $('#divSpecialities input:checkbox').each(function () {
        var x = $(this).attr("id").replace(regExpr, "");
        if ($('#divSpecialities input:checkbox[id^=' + x + ']').length > 0)
            $('#divSpecialities input:checkbox[id^=' + x + ']')[0].disabled = bval;

    });

}

function Aftersave() {
    $("#ddlPrefix")[0].selectedIndex = 0;
    $("#ddlPhysicianType")[0].selectedIndex = 0;
    $("#txtLastName")[0].value = '';
    $("#txtMI")[0].value = '';
    $("#txtFirstName")[0].value = '';
    $("#txtSuffix")[0].value = '';
    $("#txtNPI")[0].value = '';
    $("#txtCompany")[0].value = '';
    $("#txtAddressLine1")[0].value = '';
    $("#txtAddressLine2")[0].value = '';
    $("#txtCity")[0].value = '';
    $("#txtState")[0].value = '';
    $("#txtZip")[0].value = '';
    $("#txtPhone")[0].value = '';
    $("#txtFax")[0].value = '';
    $("#txtEmail")[0].value = '';

    $('#divSpecialities input:checked').each(function () {
        ($(this))[0].checked = false;
    });

    $('#divFacility input:checked').each(function () {
        ($(this))[0].checked = false;
    });
    var IsEnableGrid = localStorage.getItem("IsEnableGrid");
    var IsEFax = localStorage.getItem("IsEFax");
    var select = document.getElementById("ddlCategory");
    var sCategory = "NON CAPELLA USER(Physician)";
    if (IsEFax == "true") {
        sCategory = "NON CAPELLA USER";
        $("#ddlPhysicianType")[0].disabled = true;
    }
    else {
        sCategory = "NON CAPELLA USER(PHYSICIAN)";
        $("#ddlPhysicianType")[0].disabled = false;
    }
    for (var x = 0; x < select.length - 1 ; x++) {
        if (select.options[x].text.toUpperCase().split(" (PHYSICIAN / PA / NP)")[0] == sCategory)
            select.selectedIndex = x;
    }
    $('#lgndSpecialities').addClass('legendschedulerborderPhysician');
    $('#lgndSpecialities').removeClass('legendschedulerborderPhysicianNonMand');
    $('#lgndFacilityName').addClass('legendschedulerborderPhysician');
    $('#lgndFacilityName').removeClass('legendschedulerborderPhysicianNonMand');
    DisableorEnableFacility(false);
    ddlCategory_Change();
    document.getElementById('btnSave').innerText = "Add";
    document.getElementById('btnClearAll').innerText = "Clear All";
    return false;
}
function ClearAll() {
    var bClearAll = "false";
    if (document.getElementById('btnClearAll').innerText == "Clear All") {
        bClearAll = "true";
    }
    else
        bClearAll = "false";
    if (bClearAll == "true") {
        if (DisplayErrorMessage('1011173') == true) {
            clear();
            return false;
        }
    }
    else {
        if (DisplayErrorMessage('1011174') == true) {
            clear();
            return false;
        }
    }

}
function clear() {
    $("#ddlPrefix")[0].selectedIndex = 0;
    $("#ddlPhysicianType")[0].selectedIndex = 0;
    $("#txtLastName")[0].value = '';
    $("#txtMI")[0].value = '';
    $("#txtFirstName")[0].value = '';
    $("#txtSuffix")[0].value = '';
    $("#txtNPI")[0].value = '';
    $("#txtCompany")[0].value = '';
    $("#txtAddressLine1")[0].value = '';
    $("#txtAddressLine2")[0].value = '';
    $("#txtCity")[0].value = '';
    $("#txtState")[0].value = '';
    $("#txtZip")[0].value = '';
    $("#txtPhone")[0].value = '';
    $("#txtFax")[0].value = '';
    $("#txtEmail")[0].value = '';

    $('#divSpecialities input:checked').each(function () {
        ($(this))[0].checked = false;
    });

    $('#divFacility input:checked').each(function () {
        ($(this))[0].checked = false;
    });
    var IsEnableGrid = localStorage.getItem("IsEnableGrid");
    var IsEFax = localStorage.getItem("IsEFax");
    var select = document.getElementById("ddlCategory");
    
    var sCategory = "NON CAPELLA USER(Physician)";
    if (IsEFax == "true") {
        sCategory = "NON CAPELLA USER";
        $("#ddlPhysicianType")[0].disabled = true;
    }
    else {
        sCategory = "NON CAPELLA USER(Physician)";
        $("#ddlPhysicianType")[0].disabled = false;
    }
    for (var x = 0; x < select.length - 1 ; x++) {
        if (select.options[x].text.toUpperCase().split(" (PHYSICIAN / PA / NP)")[0] == sCategory)
            select.selectedIndex = x;
    }
    $('#lgndSpecialities').addClass('legendschedulerborderPhysician');
    $('#lgndSpecialities').removeClass('legendschedulerborderPhysicianNonMand');
    $('#lgndFacilityName').addClass('legendschedulerborderPhysician');
    $('#lgndFacilityName').removeClass('legendschedulerborderPhysicianNonMand');
    DisableorEnableFacility(false);
    document.getElementById('btnSave').innerText = "Add";
    document.getElementById('btnClearAll').innerText = "Clear All";
    ddlCategory_Change();
}

function Update(item) {
    var Category = $(item).parent().parent()[0].childNodes[1].textContent;
    var Name = '';
    var Organization = '';
    if (Category.toUpperCase() == "ORGANIZATION") {
        Organization= $(item).parent().parent()[0].childNodes[2].textContent;
        Name = $(item).parent().parent()[0].childNodes[8].textContent;
    }
    else
    {
        Organization = $(item).parent().parent()[0].childNodes[8].textContent;
        Name = $(item).parent().parent()[0].childNodes[2].textContent;
    }
    var Specialty = $(item).parent().parent()[0].childNodes[3].textContent;
    var NPI = $(item).parent().parent()[0].childNodes[4].textContent;
    var Facility = $(item).parent().parent()[0].childNodes[5].textContent;
    document.getElementById("hdnPhysicianId").value = $(item).parent().parent()[0].childNodes[6].textContent;
    var ddlCategory = document.getElementById("ddlCategory")
    if (Category.toUpperCase() == "PROVIDER")
        Category = "PROVIDER (PHYSICIAN / PA / NP)";

    for (var x = 0; x < ddlCategory.length ; x++) {
        if (Category.toUpperCase() == ddlCategory.options[x].text.toUpperCase())
            ddlCategory.selectedIndex = x;
    }
    $('#divSpecialities input:checked').each(function () {
        ($(this))[0].checked = false;
    });

    $('#divFacility input:checked').each(function () {
        ($(this))[0].checked = false;
    });
    ddlCategory_Change();
    var specialties = Specialty.split(',');
    $('#divSpecialities input:checkbox').each(function () {
        var x = $(this).attr("id").replace('&', '').replace('.', '').replace(',', '').replace(/\s/g, '');
        for (var i = 0; i < specialties.length; i++) {
            if (specialties[i].replace('&', '').replace('.', '').replace(',', '').replace(/\s/g, '') == x) {
                if ($('#divSpecialities input:checkbox[id^=' + x + ']').length > 0)
                    $('#divSpecialities input:checkbox[id^=' + x + ']')[0].checked = true;
            }
        }
    });

    var Fac = Facility.split('|');
    $('#divFacility input:checkbox').each(function () {
        var x = $(this).attr("id").replace('&', '').replace('.', '').replace(',', '').replace('#', '').replace('-', '').replace(/\s/g, '');
        for (var i = 0; i < Fac.length; i++) {
            if (Fac[i].replace('&', '').replace('.', '').replace(',', '').replace('#', '').replace('-', '').replace(/\s/g, '') == x) {
                if ($('#divFacility input:checkbox[id^=' + x + ']').length > 0)
                    $('#divFacility input:checkbox[id^=' + x + ']')[0].checked = true;
            }
        }
    });

    var Prefix = Name.split(" ")[0]
    var ddlPrefix = document.getElementById("ddlPrefix")

    for (var x = 0; x < ddlPrefix.length - 1 ; x++) {
        if (Prefix.toUpperCase() == ddlPrefix.options[x].text.toUpperCase())
            ddlPrefix.selectedIndex = x;
    }
    var FirstName = '';
    if (Name.split(" ")[1] != "undefined") {
        FirstName = Name.split(" ")[1];
    }
    var MI = '';
    if (Name.split(" ")[2] != "undefined") {
        MI = Name.split(" ")[2];
    }
    var LastName = '';
    if (Name.split(" ")[3] != "undefined") {
        LastName = Name.split(" ")[3];
    }
    var suffix = '';
    if (Name.split(" ")[4] != "undefined") {
        suffix = Name.split(" ")[4];
    }

    $("#txtLastName")[0].value = LastName;
    $("#txtFirstName")[0].value = FirstName;
    $("#txtNPI")[0].value = NPI;

    var PhysicianType = $(item).parent().parent()[0].childNodes[7].textContent; ddlPhysicianType
    var ddlPhysicianType = document.getElementById("ddlPhysicianType")

    for (var x = 0; x < ddlPhysicianType.length - 1 ; x++) {
        if (PhysicianType.toUpperCase() == ddlPhysicianType.options[x].text.toUpperCase())
            ddlPhysicianType.selectedIndex = x;
    }

    $("#txtMI")[0].value = MI;
    $("#txtSuffix")[0].value = suffix;
    $("#txtCompany")[0].value = Organization;
    $("#txtAddressLine1")[0].value = $(item).parent().parent()[0].childNodes[9].textContent;
    $("#txtAddressLine2")[0].value = $(item).parent().parent()[0].childNodes[10].textContent;
    $("#txtCity")[0].value = $(item).parent().parent()[0].childNodes[11].textContent;
    $("#txtState")[0].value = $(item).parent().parent()[0].childNodes[12].textContent;
    $("#txtZip")[0].value = $(item).parent().parent()[0].childNodes[13].textContent;
    $("#txtPhone")[0].value = $(item).parent().parent()[0].childNodes[14].textContent;
    $("#txtFax")[0].value = $(item).parent().parent()[0].childNodes[15].textContent;
    $("#txtEmail")[0].value = $(item).parent().parent()[0].childNodes[16].textContent;

    document.getElementById('btnSave').innerText = "Update";
    document.getElementById('btnClearAll').innerText = "Cancel";
}
