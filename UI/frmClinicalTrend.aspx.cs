using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using Acurus.Capella.Core.DTOJson;

namespace Acurus.Capella.UI
{
    public partial class frmClinicalTrend : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //string sLookupReasonnotPerformedXmlFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\staticlookup.xml");
                //if (File.Exists(sLookupReasonnotPerformedXmlFilePath) == true)
                //{
                //    XmlDocument itemDoc = new XmlDocument();
                //    XmlTextReader XmlText = new XmlTextReader(sLookupReasonnotPerformedXmlFilePath);
                //    itemDoc.Load(XmlText);
                //    XmlText.Close();

                //    XmlNodeList xmlNodeList = itemDoc.GetElementsByTagName("FlowSheetList");
                //    if (xmlNodeList != null && xmlNodeList.Count > 0)
                //    {
                //        Dictionary<string, string> dictFlowSheet = new Dictionary<string, string>();
                //        for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                //        {
                //            //cboFlowSheetType.Items.Add(xmlNodeList[0].ChildNodes[j].Attributes[1].Value);
                //            dictFlowSheet.Add(xmlNodeList[0].ChildNodes[j].Attributes[1].Value, xmlNodeList[0].ChildNodes[j].Attributes[0].Value);
                //        }

                //        cboFlowSheetType.DataSource = dictFlowSheet;
                //        cboFlowSheetType.DataTextField = "Key";
                //        cboFlowSheetType.DataValueField = "Value";
                //        cboFlowSheetType.DataBind();
                //    }
                //    //Get Flow sheet period list from ststiclookup XML
                //    XmlNodeList xmlNodePeriodList = itemDoc.GetElementsByTagName("FlowSheetPeriodList");
                //    if (xmlNodePeriodList != null && xmlNodePeriodList.Count > 0)
                //    {
                //        for (int j = 0; j < xmlNodePeriodList[0].ChildNodes.Count; j++)
                //        {
                //            cboFlowSheetPeriod.Items.Add(xmlNodePeriodList[0].ChildNodes[j].Attributes[0].Value);
                //        }
                //    }
                //}
                //CAP-2787
                StaticLookupList staticLookupList = ConfigureBase<StaticLookupList>.ReadJson("staticlookup.json");
                if (staticLookupList != null)
                {
                    var flowSheetList = staticLookupList.FlowSheetList.ToList();
                    if (flowSheetList != null)
                    {
                        Dictionary<string, string> dictFlowSheet = new Dictionary<string, string>();
                        foreach (var flowSheet in flowSheetList)
                        {
                              dictFlowSheet.Add(flowSheet.value,flowSheet.Field_Name);
                        }
                        //CAP-2787 - Reopen
                        //cboFlowSheetType.DataSource = flowSheetList;
                        cboFlowSheetType.DataSource = dictFlowSheet;
                        cboFlowSheetType.DataTextField = "Key";
                        cboFlowSheetType.DataValueField = "Value";
                        cboFlowSheetType.DataBind();
                    }
                    var flowSheetPeriodList = staticLookupList.FlowSheetPeriodList.ToList();
                    if (flowSheetPeriodList != null)
                    {
                        foreach (var flowSheetPeriod in flowSheetPeriodList)
                        {
                            cboFlowSheetPeriod.Items.Add(flowSheetPeriod.value);
                        }
                    }
                }

            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Patient Communication StopLoad", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);//added for BugID:45808
        }
    }
}