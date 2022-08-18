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
            XmlTextReader XmlText = null;
            XmlDocument itemDoc = new XmlDocument();
             XmlNodeList xmlTagName = null;

            if (ClientSession.HumanId != 0)
            {
            ln:
                string FileName = "Human" + "_" + ClientSession.HumanId + ".xml";
                string strXmlFilePath = Path.Combine(System.Configuration.ConfigurationSettings.AppSettings["XMLPath"], FileName);
                try
                {
                    if (File.Exists(strXmlFilePath) == true)
                    {
                        XmlText = new XmlTextReader(strXmlFilePath);

                        using (FileStream fs = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            itemDoc.Load(fs);
                            XmlText.Close();

                            if (itemDoc.GetElementsByTagName("HumanList") != null && itemDoc.GetElementsByTagName("HumanList").Count > 0)
                            {
                                xmlTagName = itemDoc.GetElementsByTagName("HumanList")[0].ChildNodes;

                                if (xmlTagName != null)
                                {
                                    for (int j = 0; j < xmlTagName.Count; j++)
                                    {
                                        if (xmlTagName[j].Attributes["Id"].Value == ClientSession.HumanId.ToString())
                                        {
                                            objFillHuman.Birth_Date = Convert.ToDateTime(xmlTagName[j].Attributes["Birth_Date"].Value);
                                            objFillHuman.Last_Name = xmlTagName[j].Attributes["Last_Name"].Value;
                                            objFillHuman.First_Name = xmlTagName[j].Attributes["First_Name"].Value;
                                            objFillHuman.EMail = xmlTagName[j].Attributes["EMail"].Value;
                                        }
                                    }
                                }
                            }
                            fs.Close();
                            fs.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    XmlText.Close();
                    //Thread.Sleep(5000);
                    UtilityManager.GenerateXML(ClientSession.HumanId.ToString(), "Human");

                    goto ln;
                }
            }

            //To Get Physician Details
            XmlDocument xmldoc1 = new XmlDocument();
            string sPhyFirstName = string.Empty;
            string sPhyLastName = string.Empty;
            string strXmlFilePath1 = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "ConfigXML\\PhysicianAddressDetails.xml");
            if (File.Exists(strXmlFilePath1) == true)
            {
                xmldoc1.Load(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "ConfigXML\\" + "PhysicianAddressDetails" + ".xml");
                XmlNode nodeMatchingPhysicianAddress = xmldoc1.SelectSingleNode("/PhysicianAddress/p" + ClientSession.PhysicianId);
                if (nodeMatchingPhysicianAddress != null)
                {
                    sPhyFirstName = nodeMatchingPhysicianAddress.Attributes["Physician_First_Name"].Value.ToString();
                    sPhyLastName = nodeMatchingPhysicianAddress.Attributes["Physician_Last_Name"].Value.ToString();
                }
            }

            var lstPatient = new
            {
                id = ClientSession.HumanId,
                dob = objFillHuman.Birth_Date,
                firstName = objFillHuman.First_Name,
                lastName = objFillHuman.Last_Name,
                emailAddress = objFillHuman.EMail,
            };

            var lstPhysician = new
            {
                id = ClientSession.PhysicianId,
                firstName = sPhyFirstName,
                lastName = sPhyLastName,
            };

            var lstEncounter = new
            {
                id = ClientSession.EncounterId,
                dateOfService = UtilityManager.ConvertToLocal(ClientSession.FillEncounterandWFObject.EncRecord.Date_of_Service).ToString("MM/dd/yyyy"),
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

        }
    }
}