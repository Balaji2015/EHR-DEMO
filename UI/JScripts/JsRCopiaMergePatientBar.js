function OnTabClick(e)
{
    var human_id = document.URL.slice(document.URL.indexOf("HumanID")).split("&")[0].split("=")[1];
    if (event?.currentTarget?.id != undefined && event?.currentTarget?.id != null) {

        if (top.window.document.getElementById("TabPatientMergeFrame").contentWindow.document.getElementById("ifrmRcopiaDuplicateScreen").contentWindow.document.getElementById("btnDelete").disabled == false) {
            var bcheck = confirm("Are you sure you want to change tab?");

            if (bcheck == true) {
                if (event?.currentTarget?.id == "btnMedication") {
                    $("#btnAllergies").removeClass("btncolorMyQ");
                    $("#btnMedication").addClass("btncolorMyQ");
                    $("#lblScreenDis")[0].innerText = "List of Duplicate Medications";
                    $("#ifrmRcopiaDuplicateScreen")[0].src = "";
                    $("#ifrmRcopiaDuplicateScreen")[0].src = "frmRCopiaDuplicateMediations.aspx?HumanID=" + human_id;
                }
                else if (event?.currentTarget?.id == "btnAllergies") {
                    $("#btnMedication").removeClass("btncolorMyQ");
                    $("#btnAllergies").addClass("btncolorMyQ");
                    $("#lblScreenDis")[0].innerText = "List of Duplicate Allergies";
                    $("#ifrmRcopiaDuplicateScreen")[0].src = ""
                    $("#ifrmRcopiaDuplicateScreen")[0].src = "frmRCopiaDuplicateAllergy.aspx?HumanID=" + human_id;
                }
            }
            else {
                return false;
            }


        }
        else {

            if (event?.currentTarget?.id == "btnMedication") {
                $("#btnAllergies").removeClass("btncolorMyQ");
                $("#btnMedication").addClass("btncolorMyQ");
                $("#lblScreenDis")[0].innerText = "List of Duplicate Medications";
                $("#ifrmRcopiaDuplicateScreen")[0].src = "";
                $("#ifrmRcopiaDuplicateScreen")[0].src = "frmRCopiaDuplicateMediations.aspx?HumanID=" + human_id;
            }
            else if (event?.currentTarget?.id == "btnAllergies") {
                $("#btnMedication").removeClass("btncolorMyQ");
                $("#btnAllergies").addClass("btncolorMyQ");
                $("#lblScreenDis")[0].innerText = "List of Duplicate Allergies";
                $("#ifrmRcopiaDuplicateScreen")[0].src = ""
                $("#ifrmRcopiaDuplicateScreen")[0].src = "frmRCopiaDuplicateAllergy.aspx?HumanID=" + human_id;
            }
        }
        
    }
}

function GetColors() {
    ColorList = ["#0000FF", "#008B8B", "#B8860B", "#8B008B", "#FF8C00", "#483D8B", "#FF1493", "#800000", "#FF00FF", "#CD5C5C", "#4B0082", "#006400", "#8A2BE2", "#A52A2A", "#5F9EA0", "#D2691E", "#FF7F50", "#6495ED", "#DC143C", "#00008B", "#00FFFF", "#3F00FF", "#097969", "#228B22", "#808000", "#CC5500", "#FF4433", "#DE3163", "#800080", "#5D3FD3", "#FF3131", "#93C572", "#F4BB44", "#B4C424", "#702963", "#7F00FF", "#F88379", "#DA70D6", "#E30B5C", "#FFBF00"];
    return ColorList;
}

function ConvertDate(utcDate,WantTime) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    //var now = new Date(utcDate + ' UTC');
    var now = new Date(utcDate);
    var then = '';
    if (utcDate == '0001-01-01 00:00:00')
        if (WantTime == "WithTime") {
            then = utcDate;
        }
        else {
            then = "0001-01-01";
        }
    else
        then = ('0' + now.getDate()).slice(-2) + '-' + monthNames[now.getMonth()] + '-' + now.getFullYear();
    var hours = now.getHours();
    var minutes = now.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = ('0' + hours).slice(-2) + ':' + minutes + ' ' + ampm;
    //Add time in date
    if (WantTime == "WithTime") {
        if (utcDate != '0001-01-01 00:00:00')
        then += ' ' + strTime;
    }
    return then;
}