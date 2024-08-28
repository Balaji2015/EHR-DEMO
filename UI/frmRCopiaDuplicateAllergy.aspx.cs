using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Text.RegularExpressions;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Drawing;
using Acurus.Capella.UI;
using Acurus.Capella.UI.UserControls;
using System.Xml;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Web.Services;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Acurus.Capella.UI
{
    public partial class frmRCopiaDuplicateAllergie : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


        }


        [WebMethod(EnableSession = true)]
        public static string GetAllergyWithPartialDuplicates(string HumanId, string status)
        {
            if (ClientSession.UserName == string.Empty)
            {
                HttpContext.Current.Response.StatusCode = 999;
                HttpContext.Current.Response.Status = "999 Session Expired";
                HttpContext.Current.Response.StatusDescription = "frmSessionExpired.aspx";
                return "Session Expired";
            }


            Rcopia_AllergyManager Rcopiamanager = new Rcopia_AllergyManager();
            IList<Rcopia_Allergy> Groupbylist = new List<Rcopia_Allergy>();
            List<Rcopia_Allergy> ExactList = new List<Rcopia_Allergy>();
            List<Rcopia_Allergy> PartialList = new List<Rcopia_Allergy>();
            List<Rcopia_Allergy> MergList = new List<Rcopia_Allergy>();
            List<Rcopia_Allergy> PartialFinallist = new List<Rcopia_Allergy>();
            List<Rcopia_Allergy> NewExactList = new List<Rcopia_Allergy>();
            List<Rcopia_Allergy> NewColorList = new List<Rcopia_Allergy>();
            List<Rcopia_Allergy> PartialListNew = new List<Rcopia_Allergy>();
            List<Rcopia_Allergy> NewPartialList = new List<Rcopia_Allergy>();

            IList<Rcopia_Allergy> AllergyList = Rcopiamanager.GetAllergyListByIdStatus(Convert.ToUInt64(HumanId), status);

            Groupbylist = (AllergyList.GroupBy(x => new { x.Allergy_Name, x.Reaction, x.OnsetDate }).Where(g => g.Count() > 1).Select(x => x.FirstOrDefault())).ToList<Rcopia_Allergy>();

            if (Groupbylist.Count > 0)
            {
                ExactList = Groupbylist.ToList<Rcopia_Allergy>();
                PartialList = AllergyList.GroupBy(x => x.Allergy_Name).Where(g => g.Count() > 1).Select(x => x.FirstOrDefault()).ToList<Rcopia_Allergy>();

                for (int i = 0; i < PartialList.Count; i++)
                {
                    PartialListNew = AllergyList.Where(x => x.Allergy_Name == PartialList[i].Allergy_Name).ToList<Rcopia_Allergy>();
                    if (PartialListNew.Count > 0)
                    {
                        NewExactList.AddRange(PartialListNew);
                    }
                }


                for (int i = 0; i < ExactList.Count; i++)
                {
                    MergList = NewExactList.Where(x => x.Allergy_Name == ExactList[i].Allergy_Name && x.Reaction == ExactList[i].Reaction && x.OnsetDate == ExactList[i].OnsetDate).ToList<Rcopia_Allergy>();
                    if (MergList.Count > 0)
                    {
                        NewPartialList = NewExactList.Except(MergList).ToList();
                        NewExactList = NewPartialList;
                    }
                }

                NewExactList.AddRange(ExactList);



                if (NewExactList.Count > 0)
                {
                    PartialFinallist = NewExactList.OrderBy(x => x.Allergy_Name).ToList();

                }

            }

            else
            {
                PartialList = AllergyList.GroupBy(x => x.Allergy_Name).Where(g => g.Count() > 1).Select(x => x.FirstOrDefault()).OrderBy(y => y.Allergy_Name).ToList<Rcopia_Allergy>();

                for (int i = 0; i < PartialList.Count; i++)
                {
                    NewExactList = AllergyList.Where(x => x.Allergy_Name == PartialList[i].Allergy_Name).ToList<Rcopia_Allergy>();
                    if (NewExactList.Count > 0)
                    {
                        PartialFinallist.AddRange(NewExactList);
                    }
                }



            }

            NewColorList = PartialFinallist.GroupBy(x => x.Allergy_Name).Select(x => x.FirstOrDefault()).ToList<Rcopia_Allergy>();

            string[] ColorList = new string[] { "#0000FF", "#008B8B", "#B8860B", "#8B008B", "#FF8C00", "#483D8B", "#FF1493", "#800000", "#FF00FF", "#CD5C5C", "#4B0082", "#006400", "#8A2BE2", "#A52A2A", "#5F9EA0", "#D2691E", "#FF7F50", "#6495ED", "#DC143C", "#00008B", "#00FFFF", "#3F00FF", "#097969", "#228B22", "#808000", "#CC5500", "#FF4433", "#DE3163", "#800080", "#5D3FD3", "#FF3131", "#93C572", "#F4BB44", "#B4C424", "#702963", "#7F00FF", "#F88379", "#DA70D6", "#E30B5C", "#FFBF00" };

            for (int i = 0; i < NewColorList.Count; i++)
            {
                for (int j = 0; j < PartialFinallist.Count; j++)
                {
                    if (NewColorList[i].Allergy_Name == PartialFinallist[j].Allergy_Name)
                    {
                        PartialFinallist[j].Status = PartialFinallist[j].Status + "~" + ColorList[i];
                    }
                }

            }



            if (PartialFinallist.Count > 0)
            {
                var AllergieList = PartialFinallist.Select(a => new
                {
                    AlergyName = a.Allergy_Name,
                    Reaction = a.Reaction,
                    OnsetDate = a.OnsetDate != DateTime.MinValue ? a.OnsetDate.ToString("dd-MMM-yyyy") : "",
                    Createdby = a.Created_By,
                    CreatedDate = a.Created_Date_And_Time.ToString("dd-MMM-yyyy HH:mm:ss tt"),
                    RcopiaId = a.Id,
                    Status = a.Status.Split('~')[0],
                    Colour = a.Status.Split('~')[1]
                });


                return JsonConvert.SerializeObject(AllergieList);
            }

            return JsonConvert.SerializeObject("");
        }


        [WebMethod(EnableSession = true)]
        public static string DeleteRCopiaAllergy(string RCopiaId)
        {

            Rcopia_AllergyManager rcopiaMngr = new Rcopia_AllergyManager();

            IList<ulong> ilstRcopiaID = new List<ulong>();
            string[] SplitId = RCopiaId.Split(',');
            foreach (string id in SplitId)
            {
                ilstRcopiaID.Add(Convert.ToUInt64(id));
            }

            var Updateval = rcopiaMngr.UpdateRcopiaAllergy(ilstRcopiaID, ClientSession.HumanId, ClientSession.FacilityName, ClientSession.LegalOrg, ClientSession.UserName);

            return Updateval;
        }



    }
}