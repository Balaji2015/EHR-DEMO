$(document).ready(function () {
    var e = 0;
    jQuery.fn.rotate = function (e) {
        $("#_imgBig").css({
            "-webkit-transform": "rotate(" + e + "deg)"
        })
    }, $("#leftrotate").click(function () {
        e -= 90, $("#leftrotate").rotate(e)
    }), $("#zoomin").click(function () {
        var e = 10,
            t = parseInt($("#_imgBig").width());
        $("#_imgBig").width(t + e + "px");
        var n = parseInt($("#_imgBig").height());
        $("#_imgBig").height(n + e + "px")
    }), $("#zoomout").click(function () {
        var e = 10,
            t = parseInt($("#_imgBig").width());
        $("#_imgBig").width(t - e + "px");
        var n = parseInt($("#_imgBig").height());
        $("#_imgBig").height(n - e + "px")
    }), $("#revert").click(function () {
        $("#_imgBig").css("width", ""), $("#_imgBig").css("height", "")
    }), $("#rotateright").click(function () {
        e += 90, $("#rotateright").rotate(e)
    }), $("#_imgBig").css("opacity", "1");

    OnLoadGrid("ASC");
});

function viewImage(e) {
    var t = document.getElementById("grdIndexing"),
        n = t.rows[e.parentNode.parentNode.rowIndex].cells[9].innerText;
    $(top.window.document).find("#txtViewImageIndexingInformation")[0].value = "";
    $(top.window.document).find("#TabViewImageIndexing").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabModalViewImageIndexingTitle")[0].textContent = "Image Viewer -Indexing";
    $(top.window.document).find("#TabmdldlgViewImageIndexing")[0].style.width = "975px";
    $(top.window.document).find("#TabmdldlgViewImageIndexing")[0].style.height = "860px";
    $(top.window.document).find("#TabViewImageIndexingFrame")[0].style.height = "895px";
    $(top.window.document).find("#TabViewImageIndexingFrame")[0].contentDocument.location.href = "frmImageViewer.aspx?FileName=" + encodeURIComponent(n) + "&Source=INDEX";
    return false;
}

function btnClose_Clicked() {
    self.close();
    return false;
}

function GetRadWindow() {
    var e = null;
    return window.radWindow ? e = window.radWindow : window.frameElement.radWindow && (e = window.frameElement.radWindow), e
}

function Close() {
    var e = GetRadWindow();
    return e.argument = null, e.close(), !1
}

function deletefiles(filePath) {
    if (confirm("Are you sure you want to delete the file Permanently?")) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $.ajax({
            type: "POST",
            async: true,
            url: "frmErroredSignedReports.aspx/SourceImageDelete",
            data: '{filePath: "' + filePath + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (data.d != "Success") {
                    alert(data.d);
                    return false;
                }
                else {
                    alert("The selected file has been successfully deleted.");
                    OnLoadGrid("ASC");
                    //CAP-3066
                    $('#imgControls').css("display", "block");
                    $('#imgholder,#bigImagePDF,#PDFholder').css("display", "none");
                }
            },
            error: function OnError(xhr) {
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
    return false;
}

$("#divCreatedDateAndTime").click(function () {
    var sortOrder = $(this).attr('data-sort-order');
    if (sortOrder == "ASC") {
        $(this).attr('data-sort-order', 'DESC');
    } else {
        $(this).attr('data-sort-order', 'ASC');
    }
    OnLoadGrid(sortOrder);
});

function OnLoadGrid(sortOrder) {
    $.ajax({
        type: "POST",
        async: true,
        url: `frmErroredSignedReports.aspx/LoadGrid`,
        data: '{sortOrder: "' + sortOrder + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            StopLoadOnUploadFile();
            $('#tblFiles tr').removeClass("highlight");
            var tabContents = "";
            var objdata = $.parseJSON(data.d);
            var vdelete = "<td></td>";
            var createdDate = "<td></td>";
            var firstFileName = "";
            var firstFilePath = "";
            if (objdata?.length ?? 0 > 0) {
                $('#lblFileCount').text(objdata.length);
                for (var i = 0; i < objdata.length; i++) {
                    firstFileName = objdata[i].Item1.split('\\').pop();
                    firstFilePath = objdata[i].Item1;
                    firstFilePath = firstFilePath.split("\\").join("/");

                    vdelete = `<td title='Delete' style='width: 41px;'><img style='width: 12px;' src='Resources/Delete-Blue.png' onclick="deletefiles('${firstFilePath}');" /></td>`;
                    createdDate = `<td title='Created Date and Time' style='width: 160px;'>${objdata[i].Item2}</td>`;
                    tabContents = tabContents + `<tr style='cursor: pointer;'>${vdelete}<td onclick="GridOpenFile('${firstFileName}','${firstFilePath}',this);">${firstFileName}</td><td style='display:none'>${firstFilePath}</td>${createdDate}</tr>`;
                }
            }
            if (tabContents == "") {
                tabContents = "<tr style='color:red'><td colspan='2'></td></tr>";
                $("#tbFilesBody").html(tabContents);
            }
            else {
                $("#tbFilesBody").html(tabContents);
                //GridOpenFile(firstFileName, firstFilePath, "");
            }
        },
        error: function OnError(xhr) {
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

function GridOpenFile(SelectedFileName, sFilePath, isClick) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (isClick) {
        select(isClick.parentNode);
    }
    sFilePath = sFilePath.split("\\").join("/");
    var data = '{filename: "' + SelectedFileName + '",filepath: "' + sFilePath + '"}';
    $.ajax({
        type: "POST",
        async: true,
        url: "frmErroredSignedReports.aspx/OpenGridFile",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var Pagecount = data.d;
            $("#hdnPagecount")[0].value = Pagecount;
            if (SelectedFileName.split('.').pop().toUpperCase() != "PDF") {
                HidePdf();
                $("#PageBox")[0].value = "1";
                $("#PageLabel")[0].innerText = "/" + Pagecount;
                document.getElementById("next").setAttribute('onClick', 'ChangePg(this)');
                document.getElementById("prev").setAttribute('onClick', 'ChangePg(this)');
                document.getElementById('deletethumbnail').style.display = "none";
                StopLoadOnUploadFile();
                sFilePath = sFilePath.replaceAll("%", "%25");
                sFilePath = sFilePath.replaceAll("#", "%23");
                sFilePath = sFilePath.replaceAll('"', "%22");
                sFilePath = sFilePath.replaceAll("<", "%3C");
                sFilePath = sFilePath.replaceAll(">", "%3E");
                sFilePath = sFilePath.replaceAll("|", "%7C");
                document.getElementById("_imgBig").src = "ViewImg.aspx?View=1&FilePath=" + sFilePath + "&Pg=1&Height=650&Width=550";
            }
            else {
                ViewPDF();
                StopLoadOnUploadFile();
                var erroredFilePath = document.getElementById("hdnErroredFilePath").value;
                var fileName = sFilePath.split('\\').pop().split('/').pop();
                sFilePath = erroredFilePath + fileName;

                if (sFilePath.indexOf("////") != -1) {
                    sFilePath = sFilePath.replaceAll(/\/\//g, "/");
                }
                sFilePath = sFilePath.replaceAll("%", "%25");
                sFilePath = sFilePath.replaceAll("#", "%23");
                sFilePath = sFilePath.replaceAll('"', "%22");
                sFilePath = sFilePath.replaceAll("<", "%3C");
                sFilePath = sFilePath.replaceAll(">", "%3E");
                sFilePath = sFilePath.replaceAll("|", "%7C");
                document.getElementById("bigImagePDF").src = sFilePath;
            }
        },
        error: function OnError(xhr) {
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

function select(e) {
    var existingSelectedItem = $("#tbFilesBody tr.highlight");
    if (existingSelectedItem.length > 0) { existingSelectedItem.removeClass("highlight"); }
    $(e).toggleClass("highlight");
}

function ChangePg(e) {
    //next Image
    if (parseInt(document.getElementById("PageBox").value) <= parseInt($("#PageLabel")[0].innerText.replace("/", "").trim()) && e.title.toUpperCase() == "NEXT IMAGE") {
        var ePageBox = $("#PageLabel")[0].innerText,
            tvalue = $("#PageBox"),
            n = tvalue.val();
        if ($("#PageLabel")[0].innerText.replace("/", "").trim() != "1" && parseInt(document.getElementById("PageBox").value) < $("#PageLabel")[0].innerText.replace("/", "").trim()) {
            n++;
            $("#PageBox")[0].value = n;
        }
    }//previous Image
    else if (parseInt(document.getElementById("PageBox").value) <= parseInt($("#PageLabel")[0].innerText.replace("/", "").trim()) && e.title.toUpperCase() == "PREVIOUS IMAGE") {
        var ePageBox = $("#PageBox"),
            tvalue = ePageBox.val();
        if ($("#PageLabel")[0].innerText.replace("/", "").trim() != "1" && tvalue != "1") {
            tvalue--;
            $("#PageBox")[0].value = tvalue;
        }
    }
    Src = "ViewImg.aspx?View=1&FilePath=" + GetBigSrc("FilePath") + "&Pg=" + $("#PageBox").val() + "&Height=" + GetBigSrc("Height") + "&Width=" + GetBigSrc("Width");
    SrcBig = "ViewImg.aspx?View=1&FilePath=" + GetBigSrc("FilePath") + "&Pg=" + $("#PageBox").val() + "&Height=1000&Width=1000";
    SrcRevert = "ViewImg.aspx?View=1&FilePath=" + GetBigSrc("FilePath") + "&Pg=" + $("#PageBox").val() + "&Height=600&Width=600";
    SrcNavigate = "ViewImg.aspx?View=1&FilePath=" + GetBigSrc("FilePath") + "&Height=600&Width=600";
    document.getElementById("_imgBig").style.opacity = 0;
    document.getElementById("_imgBig").style.transition = "opacity 4s";
    document.getElementById("_imgBig").src = Src;
    document.getElementById("_imgBig").style.opacity = 1;
    document.getElementById("zoomin").onclick = function () {
        ChangePg(this), document.getElementById("_imgBig").src = SrcBig + "&Rotate" + GetBigSrc("Rotate")
    };
    document.getElementById("zoomout").onclick = function () {
        ChangePg(this), document.getElementById("_imgBig").src = SrcRevert
    }
}

function HidePdf() {
    document.getElementById('imgControls').style.display = "block";
    document.getElementById('PDFholder').style.display = "none";
    document.getElementById('bigImagePDF').style.display = "none";
    document.getElementById('imgholder').style.display = "block";
}

function StopLoadOnUploadFile() {
    sessionStorage.setItem('StartLoading', 'false');
    StopLoadFromPatChart();
}

function ViewPDF() {
    document.getElementById('imgControls').style.display = "none";
    document.getElementById('PDFholder').style.display = "block";
    document.getElementById('bigImagePDF').style.display = "block";
    document.getElementById('imgholder').style.display = "none";
    $('#bigImagePDF').show();
}