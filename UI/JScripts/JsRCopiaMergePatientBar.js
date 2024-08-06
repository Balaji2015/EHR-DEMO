function OnTabClick(e)
{
    if (event?.currentTarget?.id != undefined && event?.currentTarget?.id != null) {
        if (event?.currentTarget?.id == "btnMedication") {
            $("#btnAllergies").removeClass("btncolorMyQ");
            $("#btnMedication").addClass("btncolorMyQ");
            $("#lblScreenDis")[0].innerText = "List of Duplicate Medications in Keep Account";
            $("#ifrmRcopiaDuplicateScreen")[0].src = "";
            $("#ifrmRcopiaDuplicateScreen")[0].src = "frmRCopiaDuplicateMediations.aspx";
        }
        else if (event?.currentTarget?.id == "btnAllergies") {
            $("#btnMedication").removeClass("btncolorMyQ");
            $("#btnAllergies").addClass("btncolorMyQ");
            $("#lblScreenDis")[0].innerText = "List of Duplicate Allergies in Keep Account";
            $("#ifrmRcopiaDuplicateScreen")[0].src = ""
            $("#ifrmRcopiaDuplicateScreen")[0].src = "";
        }
    }
}
