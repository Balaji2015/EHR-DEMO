using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Acurus.Capella.Core.DomainObjects;
using System.Diagnostics;
using System.Net;
using System.IO;
using Acurus.Capella.Core.DTO;
using System.Collections;
using System.Runtime.Serialization;
using System.Data;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.Text;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using DocumentFormat.OpenXml.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Telerik.Web.UI.Chat;
using Telerik.Web.UI.Skins;
using Acurus.Capella.UI.Extensions;
using User = Acurus.Capella.Core.DomainObjects.User;
using System.Collections.Specialized;

namespace Acurus.Capella.UI
{
    public partial class frmSelectLegalOrg : System.Web.UI.Page
    {
        UserManager UserMngr = new UserManager();
        DirectURLUtility directURLUtility = new DirectURLUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillFacilityComboBox(ClientSession.LegalOrg);
            }
        }

        private void FillFacilityComboBox(string legal_org)
        {
            cboFacilityName.Items.Clear();

            XmlDocument xmldoc = new XmlDocument();

            string strXmlFilePath = System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\Facility_Library.xml");
            if (File.Exists(strXmlFilePath) == true)
            {
                xmldoc.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "Facility_Library" + ".xml");
                XmlNodeList xmlFacilityList = xmldoc.GetElementsByTagName("Facility");

                if (xmlFacilityList.Count > 0)
                {
                    foreach (XmlNode item in xmlFacilityList)
                    {
                        if (item != null && item.Attributes.GetNamedItem("Legal_Org").Value == legal_org)
                            cboFacilityName.Items.Add(item.Attributes[0].Value);
                    }
                    cboFacilityName.Value = ClientSession.FacilityName;
                }
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            ClientSession.FacilityName = cboFacilityName.Value;
            //CAP-1911
            Response.SetCookie(new HttpCookie("CFacilityName") { Value = ClientSession.FacilityName.ToString(), HttpOnly = false });            
            ClientScript.RegisterStartupScript(this.GetType(), "SelectLegalOrg", "{RadWindowClose();changeReload();}", true);
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "SelectLegalOrg", "RadWindowClose();", true);
        }
    }
}
