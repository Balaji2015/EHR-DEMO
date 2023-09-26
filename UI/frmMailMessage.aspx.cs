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
using System.Text;
using System.Text.RegularExpressions;
using OpenPop.Pop3;
using OpenPop.Mime;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;

namespace Acurus.Capella.UI
{
    public partial class frmMailMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Session["controlID"] != null)
                //{
                //    Session["Content"] = Session["controlID"];
                //    txtMessage.Text = Session["Content"].ToString();
                //    Session.Remove("controlID");
                //}
                hdnRole.Value = Request["Role"].ToString();
                string strContent1 = "";
                if (Request["BodyMessage"] != null)
                {

                    //CAP-833 The input is not a valid Base-64 string
                    byte[] btArrayASCII = ValidateAndDecodeBase64String(Request["BodyMessage"]?.ToString());
              
                    //txtMessage.Text=  ASCIIEncoding.ASCII.GetString(btArrayASCII).Replace("&amp;","&");
                    if (btArrayASCII != null)
                    {
                        strContent1 = ASCIIEncoding.ASCII.GetString(btArrayASCII).Replace("&amp;", "&");
                        //string[] tokens = strContent1.Split(new string[] { "Body : " }, 2, 0);

                        string[] tokens = strContent1.Split(new string[] { "Body : " }, Regex.Matches(strContent1, "Body : ").Count + 1, 0);


                        string strContent = tokens[tokens.Count() - 1].ToString();

                        //string strContent = strContent1.Substring(strContent1.LastIndexOf("Body"), strContent1.Length - strContent1.LastIndexOf("Body"));
                        string[] sOutput = strContent1.Split('\n');
                        string sValue = string.Empty;
                        string sFinalvalue = string.Empty;
                        for (int i = 0; i < sOutput.Count(); i++)
                        {
                            sValue = string.Empty;
                            string[] sSplit = sOutput[i].ToString().Split(new string[] { "Body : " }, Regex.Matches(sOutput[i].ToString(), "Body : ").Count + 1, 0);
                            if (sSplit.Count() > 0)
                            {
                                for (int j = 0; j < sSplit.Count(); j++)
                                {
                                    sValue = string.Empty;
                                    if (sSplit[j].ToString().TrimEnd() != "Body :")
                                    {
                                        var vv = (from p in sSplit[j].Split(' ') where p.ToString().TrimStart().StartsWith("www.") || p.ToString().TrimStart().StartsWith("https:") || p.ToString().TrimStart().StartsWith("http:") || p.ToString().TrimStart().StartsWith("ftp:") select p).ToArray();
                                        if (vv.Length > 0)
                                        {
                                            string sURL = string.Empty;
                                            sValue = sSplit[j].ToString();
                                            for (int k = 0; k < vv.Length; k++)
                                            {
                                                if (vv[k].ToString() != "")
                                                {
                                                    sURL = getUrl(vv[k].ToString());
                                                    sValue = sValue.Replace(vv[k].ToString(), sURL);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            sValue = getUrl(sSplit[j].ToString());
                                            if (sValue == "")
                                            {
                                                if (sSplit[j] == "" && j == 0 && sSplit.Count() > 1)
                                                {
                                                    sValue = "Body : ";
                                                }
                                                else
                                                    sValue = sSplit[j].ToString();
                                            }
                                        }


                                        sFinalvalue += sValue + "\n";
                                    }
                                    else
                                    {
                                        sFinalvalue += sSplit[j].ToString();
                                    }
                                }

                            }
                            else
                            {
                                sFinalvalue += sOutput[i].ToString() + "\n";
                            }


                        }

                        //string[] sSplit = strContent.Split('\n');
                        //string sValue = string.Empty;
                        //string sFinalvalue = string.Empty;
                        //foreach(string sval in sSplit)
                        //{
                        //    sValue = string.Empty;
                        //    if(sval.TrimStart().StartsWith("www."))
                        //    {
                        //        Regex urlregex = new Regex(@"(www.([\w.]+\/?)\S*)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
                        //        sValue = urlregex.Replace(sval, "<a href=\"//$1\" target=\"_blank\">$1</a>");
                        //    }
                        //    else if (sval.TrimStart().StartsWith("https:"))
                        //    {
                        //        Regex urlregex = new Regex(@"(https:\/\/([\w.]+\/?)\S*)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
                        //        sValue = urlregex.Replace(sval, "<a href=\"$1\" target=\"_blank\">$1</a>");
                        //    }
                        //    else if (sval.TrimStart().StartsWith("http:"))
                        //    {
                        //        Regex urlregex = new Regex(@"(http:\/\/([\w.]+\/?)\S*)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
                        //        sValue = urlregex.Replace(sval, "<a href=\"$1\" target=\"_blank\">$1</a>");
                        //    }
                        //    else if (sval.TrimStart().StartsWith("ftp:"))
                        //    {
                        //        Regex urlregex = new Regex(@"(ftp:\/\/([\w.]+\/?)\S*)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
                        //        sValue = urlregex.Replace(sval, "<a href=\"$1\" target=\"_blank\">$1</a>");
                        //    }
                        //    else
                        //    {
                        //        sValue = sval;
                        //    }
                        //    sFinalvalue += sValue+"\n";
                        //}
                        //string svalfirst = string.Empty;
                        //foreach (string sval in tokens.Take(tokens.Length-1))
                        //{
                        //    svalfirst += sval + "Body : ";
                        //}

                        //lblcontent.Text = tokens[0].ToString().Replace("\n", "<br>") + "Body : " + sFinalvalue.Replace("\n", "<br>");//strContent.ToString().Replace("\n", "<br>");

                        //lblcontent.Text = svalfirst.Replace("\n", "<br>") + sFinalvalue.Replace("\n", "<br>");

                        lblcontent.Text = sFinalvalue.Replace("\n", "<br>");

                        Session["Content"] = strContent1;
                    }
                }
                if (Request["PatientID"] != null && Request["EmailID"] != "" && Request["EncounterID"] != "0")
                {
                    hdnPatientID.Value = Request["PatientID"].ToString();
                    hdnEmailID.Value = Request["EmailID"].ToString();
                    hdnEncounterID.Value = Request["EncounterID"].ToString();

                }
                if (Request["FileName"] != null && Request["FileName"].Trim() != string.Empty && Request["FileName"].Trim() != "&nbsp;")
                {

                    hdnmailPath.Value = Request["FileName"].ToString();
                    string[] filename = Request["FileName"].Split('|');
                    for (int i = 0; i < filename.Length; i++)
                    {
                        Label lbl = new Label();
                        lbl.Text = Path.GetFileName(filename[i]) + "<br/>";
                        lbl.Attributes.Add("class", "Labellink");
                        lbl.Attributes.Add("onclick", "DownloadFile(this)");
                        lbl.Attributes.Add("path", filename[i]);
                        dvattachment.Controls.Add(lbl);

                    }
                }
                else if (Request["EmailID"] != null)
                {
                    hdnEncounterID.Value = Request["EncounterID"].ToString();
                    hdnPatientID.Value = "0";
                    hdnEmailID.Value = Request["EmailID"].ToString();
                }
                else
                {
                    hdnPatientID.Value = "0";
                    hdnEmailID.Value = string.Empty;
                    hdnEncounterID.Value = "0";
                }
                if (Request["IS_Patient_Portal"] != null)
                {
                    hdnIsPatientPortal.Value = Request["IS_Patient_Portal"].ToString();
                }
                if (Request["MsgId"] != null)
                {
                    string MsgId = Request["MsgId"].ToString();
                    if (MsgId != "0")
                    {
                        PhysicianLibrary lstphysician = new PhysicianLibrary();
                        PhysicianManager obj = new PhysicianManager();
                        lstphysician = obj.GetById(Convert.ToUInt64(ClientSession.PhysicianId));

                        if (lstphysician != null)
                        {
                            
                            ArrayList mesagecontent = new ArrayList();

                            Pop3Client pop3Client;

                            if (Session["Pop3Client"] == null)
                            {
                                pop3Client = new Pop3Client();
                                if (lstphysician.Mail_Server_Address != "")
                                {
                                    pop3Client.Connect(lstphysician.Mail_Server_Address.Split('|')[0], Convert.ToInt32(lstphysician.Mail_Server_Address.Split('|')[1]), true);//"pop.mail.yahoo.com", 995, true);
                                    if (lstphysician.PhyEMail.Trim() != "" && lstphysician.Physician_EMail_Password.Trim() != "")
                                    {
                                        pop3Client.Authenticate(lstphysician.PhyEMail, lstphysician.Physician_EMail_Password);//"opsnbjvjqwiltctu");
                                        Session["Pop3Client"] = pop3Client;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                else
                                {
                                    return;
                                }

                            }
                            else
                            {
                                pop3Client = (Pop3Client)Session["Pop3Client"];
                            }
                            OpenPop.Mime.Message message = pop3Client.GetMessage(Convert.ToInt32(Request["MsgId"].ToString()), null);
                            string from = message.Headers.From.ToString();
                            string toaddress = "vallikannu.a@acurussolutions.com";

                            string subject = message.Headers.Subject;
                            string datesent = message.Headers.DateSent.ToString();

                            strContent1 = "\r\n";
                            //txtMessage +="\r\n";
                            strContent1 += "\r\n---------------------------------------------------------";


                            strContent1 += "\r\n" + "From :  " + from;
                            strContent1 += "\r\n" + "To :  " + toaddress;




                            strContent1 += "\r\n" + "Message Date&Time :  " + datesent;
                            strContent1 += "\r\n" + "Subject :  " + subject.Replace("&nbsp;", "\r\n");

                            MessagePart body = message.FindFirstHtmlVersion();

                            MessagePart plainTextPart = message.FindFirstPlainTextVersion();
                            string Bodymessage = "";
                            if (plainTextPart != null)
                            {
                                // The message had a text/plain version - show that one
                                Bodymessage = plainTextPart.GetBodyAsText();
                                // mesagecontent.Add(message.Headers.MessageId.ToString() + "|" + plainTextPart.GetBodyAsText() + "|" + attacmentlist);
                            }
                            else
                            {
                                // Try to find a body to show in some of the other text versions
                                List<MessagePart> textVersions = message.FindAllTextVersions();
                                if (textVersions.Count >= 1)
                                {
                                    Bodymessage = textVersions[0].GetBodyAsText();
                                    // mesagecontent.Add(message.Headers.MessageId.ToString() + "|" + textVersions[0].GetBodyAsText() + "|" + attacmentlist);
                                }
                                else
                                    Bodymessage = "<<OpenPop>> Cannot find a text version body in this message to show <<OpenPop>>";
                            }
                            List<MessagePart> lstattachment = message.FindAllAttachments();


                            if (lstattachment != null && lstattachment.Count > 0)
                            {

                                try
                                {
                                    DirectoryInfo virdir = new DirectoryInfo(Server.MapPath("atala-capture-download//" + Session.SessionID + "//PROVIDERMAILINBOX"));
                                    if (!virdir.Exists)
                                    {
                                        virdir.Create();
                                    }
                                    foreach (var ado in lstattachment)
                                    {


                                        ado.Save(new System.IO.FileInfo(System.IO.Path.Combine(Server.MapPath("atala-capture-download//" + Session.SessionID + "//PROVIDERMAILINBOX"), ado.FileName)));

                                        string filepath = Path.Combine(Server.MapPath("atala-capture-download//" + Session.SessionID + "//PROVIDERMAILINBOX"), ado.FileName);
                                        if (hdnmailPath.Value == "")
                                            hdnmailPath.Value = filepath;
                                        else
                                            hdnmailPath.Value = hdnmailPath.Value + "|" + filepath;

                                        Label lbl = new Label();
                                        lbl.Text = Path.GetFileName(ado.FileName) + "<br/>";
                                        lbl.Attributes.Add("class", "Labellink");
                                        lbl.Attributes.Add("onclick", "DownloadFile(this)");
                                        lbl.Attributes.Add("path", filepath);
                                        dvattachment.Controls.Add(lbl);




                                    }

                                }




                                catch (Exception ex)
                                {
                                    throw ex;
                                }


                            }


                            string bodymessage = strContent1 + "\r\n" + "Body :  " + Bodymessage.ToString().ToString().Replace("&nbsp;", "\r\n");

                            string[] tokens = bodymessage.Split(new string[] { "Body : " }, Regex.Matches(Bodymessage, "Body : ").Count + 1, 0);


                            string strContent = tokens[tokens.Count() - 1].ToString();

                            //string strContent = strContent1.Substring(strContent1.LastIndexOf("Body"), strContent1.Length - strContent1.LastIndexOf("Body"));
                            string[] sOutput = bodymessage.Split('\n');
                            string sValue = string.Empty;
                            string sFinalvalue = string.Empty;
                            for (int i = 0; i < sOutput.Count(); i++)
                            {
                                sValue = string.Empty;
                                string[] sSplit = sOutput[i].ToString().Split(new string[] { "Body : " }, Regex.Matches(sOutput[i].ToString(), "Body : ").Count + 1, 0);
                                if (sSplit.Count() > 0)
                                {
                                    for (int j = 0; j < sSplit.Count(); j++)
                                    {
                                        sValue = string.Empty;
                                        if (sSplit[j].ToString().TrimEnd() != "Body :")
                                        {
                                            var vv = (from p in sSplit[j].Split(' ') where p.ToString().TrimStart().StartsWith("www.") || p.ToString().TrimStart().StartsWith("https:") || p.ToString().TrimStart().StartsWith("http:") || p.ToString().TrimStart().StartsWith("ftp:") select p).ToArray();
                                            if (vv.Length > 0)
                                            {
                                                string sURL = string.Empty;
                                                sValue = sSplit[j].ToString();
                                                for (int k = 0; k < vv.Length; k++)
                                                {
                                                    if (vv[k].ToString() != "")
                                                    {
                                                        sURL = getUrl(vv[k].ToString());
                                                        sValue = sValue.Replace(vv[k].ToString(), sURL);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                sValue = getUrl(sSplit[j].ToString());
                                                if (sValue == "")
                                                {
                                                    if (sSplit[j] == "" && j == 0 && sSplit.Count() > 1)
                                                    {
                                                        sValue = "Body : ";
                                                    }
                                                    else
                                                        sValue = sSplit[j].ToString();
                                                }
                                            }


                                            sFinalvalue += sValue + "\n";
                                        }
                                        else
                                        {
                                            sFinalvalue += sSplit[j].ToString();
                                        }
                                    }

                                }
                                else
                                {
                                    sFinalvalue += sOutput[i].ToString() + "\n";
                                }


                            }
                            lblcontent.Text = sFinalvalue.Replace("\n", "<br>");

                            Session["Content"] = bodymessage;


                            // }

                            // }

                        }
                    }
                   
                }
            }

        }
        public static byte[] ValidateAndDecodeBase64String(string base64String)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(base64String))
                {
                    return null;
                }

                // Remove spaces from the input string
                string cleanedBase64 = base64String.Replace(" ", "");

                // Define a regular expression pattern for valid Base64 strings
                string base64Pattern = @"^[A-Za-z0-9+/]*={0,2}$";

                if (Regex.IsMatch(cleanedBase64, base64Pattern))
                {
                    return Convert.FromBase64String(cleanedBase64);
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected void btndownload_Click(object sender, EventArgs e)
        {
            string localPath = string.Empty;
            string ftpServerIP = string.Empty;
            string ftpUserName = string.Empty;
            string ftpPassword = string.Empty;
            string simagePathname = string.Empty;
            string source = string.Empty;
            string file_path = string.Empty;
            string _fileName = string.Empty;




            localPath = System.Configuration.ConfigurationSettings.AppSettings["LocalPath"];
            ftpServerIP = System.Configuration.ConfigurationSettings.AppSettings["ftpServerIP"].ToString();
            //+"//" + System.Configuration.ConfigurationSettings.AppSettings["ftpMailpath"].ToString();

            ftpUserName = System.Configuration.ConfigurationSettings.AppSettings["ftpUserID"];
            ftpPassword = System.Configuration.ConfigurationSettings.AppSettings["ftpPassword"];


            string localpath = Server.MapPath("atala-capture-download//" + Session.SessionID + "//MAILINBOX");
            DirectoryInfo virdir = new DirectoryInfo(Server.MapPath("atala-capture-download//" + Session.SessionID + "//MAILINBOX"));
            if (!virdir.Exists)
            {
                virdir.Create();
            }
            FileInfo[] file = virdir.GetFiles();
            for (int i = 0; i < file.Length; i++)
            {
                File.Delete(file[i].FullName);
            }

            FTPImageProcess ftpImage = new FTPImageProcess();


            // DirectoryInfo childDir = new DirectoryInfo(new FileInfo(files[i]).DirectoryName);
            string[] sDirName = hdnpath.Value.Split('/');
            string ftpip = Path.Combine(ftpServerIP, sDirName[sDirName.Length - 2]);
            ftpImage.DownloadFromImageServer("0", ftpip, ftpUserName, ftpPassword, Path.GetFileName(hdnpath.Value), localpath, out string sCheckFileNotFoundException);
            if (sCheckFileNotFoundException != "" && sCheckFileNotFoundException.Contains("CheckFileNotFoundException"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert(\"" + sCheckFileNotFoundException.Split('~')[1] + "\");", true);
                return;
            }
            string orig_image = localpath + "\\" + Path.GetFileName(hdnpath.Value);




            // Append cookie
            HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
            cookie.Value = "Flag";
            cookie.Expires = DateTime.Now.AddDays(1);
            Response.AppendCookie(cookie);
            // end
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;

            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(hdnpath.Value));
            byte[] data = req.DownloadData(orig_image);
            response.BinaryWrite(data);
            response.End();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Downloaded", "{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }", true);
            Response.End();


        }
        public string getUrl(string sURL)
        {
            string sValue = string.Empty;
            if (sURL.ToString().TrimStart().StartsWith("www."))
            {
                Regex urlregex = new Regex(@"(www.([\w.]+\/?)\S*)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
                sValue = urlregex.Replace(sURL.ToString(), "<a href=\"//$1\" target=\"_blank\">$1</a>");
            }
            else if (sURL.ToString().TrimStart().StartsWith("https:"))
            {
                Regex urlregex = new Regex(@"(https:\/\/([\w.]+\/?)\S*)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
                sValue = urlregex.Replace(sURL.ToString(), "<a href=\"$1\" target=\"_blank\">$1</a>");
            }
            else if (sURL.ToString().TrimStart().StartsWith("http:"))
            {
                Regex urlregex = new Regex(@"(http:\/\/([\w.]+\/?)\S*)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
                sValue = urlregex.Replace(sURL.ToString(), "<a href=\"$1\" target=\"_blank\">$1</a>");
            }
            else if (sURL.ToString().TrimStart().StartsWith("ftp:"))
            {
                Regex urlregex = new Regex(@"(ftp:\/\/([\w.]+\/?)\S*)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
                sValue = urlregex.Replace(sURL.ToString(), "<a href=\"$1\" target=\"_blank\">$1</a>");
            }
            return sValue;
        }
    }
}
