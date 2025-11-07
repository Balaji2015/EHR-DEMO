var dataTable;
var Id = 0;
$(document).ready(function () {
    OnLoadGrid();
});

$('#btnRefreshMyScan').click(function () {
    OnLoadGrid();
});

$('#btnProcessScan').click(function () {
    if ($('#divErrorIndexing').children().find('.highlight').length == 0) {
        alert("Please select one file to process");
        return false;
    }
    OpenIndexing();
});

function OnLoadGrid() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    $('#divErrorIndexing').empty();
    $("#divErrorIndexing").append(`<table id="tblErrorIndexing" class="table table-bordered Gridbodystyle" style="table-layout: fixed;width:99%;">
                            <thead class="header" style="border: 0px;">
                                <tr class="header">
                                    <th style="border: 1px solid #909090; text-align: center;">File Name</th>
                                    <th style="border: 1px solid #909090; text-align: center;">No. of Pages</th>
                                    <th style="border: 1px solid #909090; text-align: center;">Exception</th>
                                    <th style="display:none;">Id</th>
                                </tr>
                            </thead>
                        </table>`);

    dataTable = new DataTable('#tblErrorIndexing', {
        serverSide: false,
        lengthChange: false,
        searching: true,
        processing: false,
        scrollCollapse: true,
        scrollY: '420px',
        ordering: true,
        autoWidth: false,
        order: [],
        pageLength: 15,
        language: {
            search: "",
            searchPlaceholder: "Search by File Name or Exception",
            infoFiltered: ""
        },
        dom: '<"top"ipf>rt<"bottom"l><"clear">',
        ajax: {
            url: '/frmErrorIndexing.aspx/LoadGrid',
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            deferRender: true,
            //data: function (d) {
            //    d.extra_search = extra_search;
            //    return d;
            //},
            dataSrc: function (json) {
                var objdata = json.d;
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                json.data = JSON.parse(objdata.data);
                return json.data;
            },
            error: function (xhr, error, code) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                }
            }
        },
        columns: [
            { data: 'File_Name', sWidth: '40%' },
            { data: 'No_of_Pages', sWidth: '10%', searchable: false },
            { data: 'Reason_Description', sWidth: '50%' },
            { data: 'Id', sWidth: '0%', sClass: "hide_column", searchable: false },
        ],
        initComplete: function (settings, json) {
            $("#tblErrorIndexing_filter input")[0].classList.add('searchicon');
        }
    });

    $('#tblErrorIndexing_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
    });

    $('#tblErrorIndexing_info').css({
        'min-width': '180px'
    });

    $('#tblErrorIndexing tbody').on('click', 'tr', function () {
        $('#tblErrorIndexing tr').removeClass("odd");
        $('#tblErrorIndexing tr').removeClass("even");
        $('#tblErrorIndexing tbody tr').removeClass("highlight");
        $(this).addClass('highlight');
        var rowData = [];
        $(this).find('td').each(function () {
            rowData.push($(this).text().trim());
        });
        Id = rowData[3];
    });

    $('#tblErrorIndexing tbody').on('dblclick', 'tr', function () {
        OpenIndexing();
    });
}

function OpenIndexing() {
    //var Mode = "UploadDocuments";
    var obj = new Array();
    //var screen = "OnlineDocuments";
    //obj.push("Screen=" + screen);
    //obj.push("ScreenMode=" + Mode);
    obj.push("IndexingExceptionLogId=" + Id);
    //localStorage.setItem("IndexingScreenMode", Mode);
    var dateonclient = new Date;
    var Tz = (dateonclient.getTimezoneOffset());
    document.cookie = "Tz=" + Tz;
    //var result = openModal("frmIndexing.aspx", 710, 1200, obj, "RadWindow1");

    var Argument = ""; var PageName = "frmIndexing.aspx"; if (obj != undefined) {
        for (var i = 0; i < obj.length; i++) { if (i != 0) { Argument = Argument + "&" + obj[i]; } else { Argument = obj[i]; } }
        if (obj.length != 0) { PageName = PageName + "?"; }
    }

    var oWnd = GetRadWindow();
    var childWindow = oWnd.BrowserWindow.radopen(PageName + Argument, "IndexWindow");
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(1200, 710);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();

    childWindow.add_close(function (oWindow, args) {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        OnLoadGrid();
    });
}