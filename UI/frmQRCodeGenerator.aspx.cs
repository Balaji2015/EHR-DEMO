using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Acurus.Capella.Core.DomainObjects;
using Newtonsoft.Json;
using QRCoder;

namespace Acurus.Capella.UI
{
    public partial class frmQRCodeGenerator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //To Get Patient Details
            Human objFillHuman = new Human();
            //XmlTextReader XmlText = null;
            XmlDocument itemDoc = new XmlDocument();
            //XmlNodeList xmlTagName = null;

            //Jira CAP-953 - start
            ulong ulHumanID = 0;
            ulong ulEncounterID = 0;
            ulong ulPhysicanID = 0;
            DateTime Date_of_Service = DateTime.MinValue;
            if (Request["HumanID"] != null)
            {
                ulHumanID = Convert.ToUInt64(Request["HumanID"]);
            }
            else if (ClientSession.HumanId != 0)
            {
                ulHumanID = ClientSession.HumanId;
            }

            if (Request["EncounterID"] != null)
            {
                ulEncounterID = Convert.ToUInt64(Request["EncounterID"]);
            }
            else if (ClientSession.EncounterId != 0)
            {
                ulEncounterID = ClientSession.EncounterId;
            }


            if (Request["PhysicianID"] != null)
            {
                ulPhysicanID = Convert.ToUInt64(Request["PhysicianID"]);
            }
            else if (ClientSession.PhysicianId != 0)
            {
                ulPhysicanID = ClientSession.PhysicianId;
            }

            if (Request["DOS"] != null)
            {
                Date_of_Service = Convert.ToDateTime(Request["DOS"]);
            }
            else if (ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service != DateTime.MinValue)
            {
                Date_of_Service = ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service;
            }
            //Jira CAP-953 - end

            //Jira CAP-953
            //if (ClientSession.HumanId != 0)
            if (ulHumanID != 0)
            {
                IList<string> ilstHumanTag = new List<string>();
                ilstHumanTag.Add("HumanList");
            ln:
                try
                {
                    IList<object> ilstHumanBlobList = new List<object>();
                    //Jira CAP-953
                    //ilstHumanBlobList = UtilityManager.ReadBlob(ClientSession.HumanId, ilstHumanTag);
                    ilstHumanBlobList = UtilityManager.ReadBlob(ulHumanID, ilstHumanTag);

                    if (ilstHumanBlobList != null && ilstHumanBlobList.Count > 0)
                    {
                        if (ilstHumanBlobList[0] != null)
                        {
                            for (int iCount = 0; iCount < ((IList<object>)ilstHumanBlobList[0]).Count; iCount++)
                            {
                                objFillHuman = ((Human)((IList<object>)ilstHumanBlobList[0])[iCount]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //XmlText.Close();
                    //Thread.Sleep(5000);

                    //Jira CAP-953
                    //UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");
                    UtilityManager.GenerateXML(ulHumanID.ToString(), "Human");

                    goto ln;
                }

                //ln:
                //    string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
                //    string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                //    try
                //    {
                //        if (File.Exists(strXmlFilePath) == true)
                //        {
                //            XmlText = new XmlTextReader(strXmlFilePath);

                //            using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //            {
                //                itemDoc.Load(fs);
                //                XmlText.Close();

                //                if (itemDoc.GetElementsByTagName("HumanList") != null && itemDoc.GetElementsByTagName("HumanList").Count > 0)
                //                {
                //                    xmlTagName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes;

                //                    if (xmlTagName != null)
                //                    {
                //                        for (int j = 0; j < xmlTagName.Count; j++)
                //                        {
                //                            if (xmlTagName[j].Attributes["Id"].Value == ClientSession.HumanId.ToString())
                //                            {
                //                                objFillHuman.Birth_Date = Convert.ToDateTime(xmlTagName[j].Attributes["Birth_Date"].Value);
                //                                objFillHuman.Last_Name = xmlTagName[j].Attributes["Last_Name"].Value;
                //                                objFillHuman.First_Name = xmlTagName[j].Attributes["First_Name"].Value;
                //                                objFillHuman.EMail = xmlTagName[j].Attributes["EMail"].Value;
                //                            }
                //                        }
                //                    }
                //                }
                //                fs.Close();
                //                fs.Dispose();
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        XmlText.Close();
                //        //Thread.Sleep(5000);
                //        UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");

                //        goto ln;
                //    }
            }

            //To Get Physician Details
            XmlDocument xmldoc1 = new XmlDocument();
            string sPhyFirstName = string.Empty;
            string sPhyLastName = string.Empty;
            string strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
            if (File.Exists(strXmlFilePath1) == true)
            {
                xmldoc1.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                //Jira CAP-953
                //XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/PhysicianAddress/p" + ClientSession.PhysicianId);
                XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/PhysicianAddress/p" + ulPhysicanID);
                if (nodeMatchingPhysicianAddress != null)
                {
                    sPhyFirstName = nodeMatchingPhysicianAddress.Attributes["Physician_First_Name"].Value.ToString();
                    sPhyLastName = nodeMatchingPhysicianAddress.Attributes["Physician_Last_Name"].Value.ToString();
                }
            }

            var lstPatient = new
            {
                //Jira CAP-953
                //id = "" + ClientSession.HumanId.ToString() + "",
                id = "" + ulHumanID.ToString() + "",
                dob = objFillHuman.Birth_Date,
                firstName = objFillHuman.First_Name,
                lastName = objFillHuman.Last_Name,
                emailAddress = objFillHuman.EMail,
            };

            var lstPhysician = new
            {
                //Jira CAP-953
                //id = "" + ClientSession.PhysicianId + "",
                id = "" + ulPhysicanID + "",
                firstName = sPhyFirstName,
                lastName = sPhyLastName,
            };

            var lstEncounter = new
            {
                //Jira CAP-953
                //id = "" + ClientSession.EncounterId + "",
                id = "" + ulEncounterID + "",
                //Jira CAP-953
                //dateOfService = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).ToString("MM/dd/yyyy"),
                dateOfService = UtilityManager.ConvertToLocal(Date_of_Service).ToString("MM/dd/yyyy"),
            };

            var lstFinalResult = new
            {
                flow = "CAPELLA_ENCOUNTER",
                patient = lstPatient,
                physician = lstPhysician,
                encounter = lstEncounter,
            };

            string code = JsonConvert.SerializeObject(lstFinalResult, Newtonsoft.Json.Formatting.Indented);

            QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = null;

            qrCodeData = qrGenerator.CreateQrCode(code, QRCoder.QRCodeGenerator.ECCLevel.Q);

            QRCode qrCode = new QRCode(qrCodeData);
            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            imgBarCode.Height = 250;
            imgBarCode.Width = 250;
            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
                plBarCode.Controls.Add(imgBarCode);
            }
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "QRCodeGenerator", "{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}", true);
        }
    }
}