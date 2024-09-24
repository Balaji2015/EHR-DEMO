using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Acurus.Capella.Core.DomainObjects;

using System.Reflection;
using System.IO;
using System.Collections;
using Acurus.Capella.DataAccess.ManagerObjects;

namespace Acurus.Capella.LabAgent
{


    public partial class ResultsUtilityManager
    {
        #region Declaration
        static ulong Lab_ID;
        #endregion
        #region ResultSplitProcess
        public void ResultSplitProcess()
        {
            ResultMasterManager resultMasterProxy = new ResultMasterManager();
            IList<ResultLookup> resultLookupList = null;
            ResultLookupManager objResultLookupManager = new ResultLookupManager();
            resultLookupList = objResultLookupManager.GetResultLookup();
            IList<ulong> Order_ID_List = new List<ulong>();

            /*To read the result file*/
            //DirectoryInfo di = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["ResultReceivedPath"]);
            //if (di.Exists == true)
            //{
            //What about Quest - .hl7????
            foreach (FileInfo[] files in ResultsUtilityManager.GetFilesFromDirectory())
            {


                FileInfo[] rgFiles = files;
                //FileInfo[] rgFiles = di.GetFiles("*.dat");
                if (rgFiles.Count() != 0)
                {
                    IList<ResultMaster> resultMasterList = new List<ResultMaster>();
                    IList<ResultORC> resultORCList = new List<ResultORC>();
                    IList<ResultOBR> resultOBRList = new List<ResultOBR>();
                    IList<ResultOBX> resultOBXList = new List<ResultOBX>();
                    IList<ResultNTE> resultNTEList = new List<ResultNTE>();
                    IList<ResultZEF> resultZEFList = new List<ResultZEF>();
                    IList<ResultZPS> resultZPSList = new List<ResultZPS>();
                    int masterItemNumber = 0;
                    int ObrItemNumber = 0;
                    int ObxItemNmber = 0;
                    int OrcItemNumber = 0;
                    bool orcSegementRead = false;
                    bool allergyCommentsFlag = false;
                    foreach (FileInfo fi in rgFiles)
                    {
                        bool bUnimportedFile = false;
                        //Jira CAP-1116
                        try
                        {
                            ArrayList myResults = new ArrayList();
                            TextReader trs = new StreamReader(@fi.DirectoryName + "\\" + fi.Name);
                            string sOutput = trs.ReadToEnd();
                            trs.Close();
                            trs.Dispose();
                            string[] newarray = sOutput.Split('\n');
                            if (newarray != null)
                            {
                                for (int g = 0; g < newarray.Length; g++)
                                {
                                    if (newarray[g].Contains("\r") == false)
                                    {
                                        newarray[g] += "\r";
                                    }
                                }
                            }
                            allergyCommentsFlag = false;
                            orcSegementRead = false;
                            string sCurrentResult = string.Empty;
                            int j = 0;
                            string segmentName = string.Empty;
                            string segmentField = string.Empty;
                            string segmentSubField = string.Empty;
                            Assembly currentAssembly;
                            Type myType;
                            object instance = null;
                            MemberInfo[] memberInfo;
                            string prevDomainName = string.Empty;
                            string propName = string.Empty;
                            string domain = string.Empty;
                            string prevSegmentName = string.Empty;

                            for (int i = 0; i < newarray.Length; i++)
                            {
                                if (newarray[i].StartsWith("MSH") == true)
                                {
                                    if (i != 0)
                                    {
                                        myResults.Add(sCurrentResult);
                                    }
                                    sCurrentResult = newarray[i];
                                }
                                else
                                {
                                    sCurrentResult = sCurrentResult + newarray[i];
                                }
                                if (i == newarray.Length - 1)
                                {
                                    j = i + 1;
                                    if (j == newarray.Length)
                                    {
                                        myResults.Add(sCurrentResult);
                                    }
                                }
                            }

                            for (int k = 0; k < myResults.Count; k++)
                            {
                                orcSegementRead = false;
                                string[] line = myResults[k].ToString().Split('\r');
                                if (line.Length > 0)
                                {
                                    for (int l = 0; l < line.Length; l++)
                                    {
                                        string[] segments = line[l].Split('|');
                                        if (segments[0] != string.Empty)
                                        {
                                            if (char.IsLetter(segments[0], 0) == true)
                                            {
                                                segmentName = segments[0];
                                            }
                                            IList<ResultLookup> resLook = (from dom in resultLookupList
                                                                           where dom.Segment_Name == segmentName && dom.Lab_ID == Lab_ID
                                                                           select dom).ToList();
                                            if (resLook.Count != 0 && (prevDomainName.ToUpper() != resLook[0].Domain_Object.ToUpper() || segmentName.ToUpper() == "OBX" || segmentName.ToUpper() == "NTE" || segmentName.ToUpper() == "ZPS" || segmentName.ToUpper() == "ZEF"))
                                            {
                                                if (resLook[0].Domain_Object.Contains("ORC") == true)
                                                {
                                                    if (orcSegementRead == false)
                                                    {
                                                        orcSegementRead = true;
                                                        domain = resLook[0].Domain_Object;
                                                        currentAssembly = Assembly.Load("Acurus.Capella.Core");
                                                        myType = currentAssembly.GetType(domain);
                                                        instance = Activator.CreateInstance(myType);
                                                        memberInfo = instance.GetType().GetMembers();
                                                    }
                                                    else
                                                    {
                                                        instance = null;
                                                    }
                                                }
                                                else
                                                {
                                                    domain = resLook[0].Domain_Object;
                                                    currentAssembly = Assembly.Load("Acurus.Capella.Core");
                                                    myType = currentAssembly.GetType(domain);
                                                    instance = Activator.CreateInstance(myType);
                                                    memberInfo = instance.GetType().GetMembers();
                                                }
                                            }
                                            else if (resLook.Count == 0)
                                                instance = null;
                                            if (instance != null)
                                            {
                                                for (int i = 0; i < segments.Length; i++)
                                                {
                                                    ResultLookup re = null;
                                                    segmentField = segmentName + "-" + i.ToString();
                                                    /*If inner segment is present then the number of values in result file and 
                                                     * the number of inner segments in lookup should be considered.*/
                                                    if ((!segments[i].Contains(@"^~\&")) && (segments[i].Contains("^")) && segmentField != "OBX-5")
                                                    {
                                                        string[] innerSegments = segments[i].Split('^');
                                                        for (int m = 0; m < innerSegments.Length; m++)
                                                        {
                                                            //Jira CAP-1147
                                                            re = null;
                                                            segmentSubField = segmentField + "." + (m + 1).ToString();
                                                            IList<ResultLookup> reList = (from res in resultLookupList
                                                                                          where res.Segment_Name == segmentName && res.Lab_ID == Lab_ID && res.Segment_Field == segmentField && res.Segment_Sub_Field == segmentSubField
                                                                                          select res).ToList();
                                                            if (reList.Count != 0)
                                                            {
                                                                re = reList[0];
                                                            }
                                                            if (re != null)
                                                            {
                                                                /*With the column name the property can be found and value can be assigned.*/
                                                                propName = re.Column_Name;
                                                                PropertyInfo propInfo = instance.GetType().GetProperty(propName);
                                                                propInfo.SetValue(instance, innerSegments[m], null);
                                                            }
                                                        }
                                                    }
                                                    /*If there is no inner segment with '^' symbol.*/
                                                    else
                                                    {
                                                        IList<ResultLookup> reList = (from res in resultLookupList
                                                                                      where res.Segment_Name == segmentName && res.Lab_ID == Lab_ID && res.Segment_Field == segmentField && res.Segment_Sub_Field.Trim() == string.Empty
                                                                                      select res).ToList();
                                                        if (reList.Count != 0)
                                                        {
                                                            re = reList[0];
                                                        }
                                                        if (re != null)
                                                        {
                                                            /*With the column name the property can be found and value can be assigned.*/
                                                            propName = re.Column_Name;
                                                            PropertyInfo propInfo = instance.GetType().GetProperty(propName);
                                                            propInfo.SetValue(instance, segments[i], null);
                                                        }
                                                    }
                                                }
                                                if (instance != null)
                                                {
                                                    if (segmentName.ToUpper() == "PID")
                                                    {
                                                        allergyCommentsFlag = false;
                                                        ResultMaster resMaster = (ResultMaster)instance;
                                                        string val = resMaster.MSH_Segment_Type_ID;
                                                        if (val != string.Empty)
                                                        {
                                                            resMaster.Result_Received_Date = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                                            resMaster.Created_By = "ACURUS";
                                                            resMaster.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                                            resMaster.File_Name = fi.Name;
                                                            resultMasterList.Add(resMaster);
                                                            if (resultMasterList.Count > 0)
                                                            {
                                                                /*Result Master ID to be assigned for each list so, in each list individually 
                                                                 * it should be assigned. In manager side it can be easily processed using this 
                                                                 * number.*/
                                                                masterItemNumber = resultMasterList.Count - 1;
                                                            }
                                                        }
                                                    }

                                                    else if (segmentName.ToUpper() == "ORC")
                                                    {
                                                        allergyCommentsFlag = false;
                                                        ResultORC resOrc = (ResultORC)instance;
                                                        resOrc.Result_Master_ID = Convert.ToUInt64(masterItemNumber);
                                                        resOrc.Created_By = "ACURUS";
                                                        resOrc.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                                        if (resOrc.ORC_Specimen_ID.Length > 4 && resOrc.ORC_Specimen_ID.Contains("ACUR") == true)
                                                        {
                                                            string orderSubmitId = resOrc.ORC_Specimen_ID.Remove(0, 4);
                                                            if (resultMasterList.Count >= 0)
                                                            {
                                                                try
                                                                {
                                                                    resultMasterList[masterItemNumber].Order_ID = Convert.ToUInt64(orderSubmitId);
                                                                    Order_ID_List.Add(Convert.ToUInt64(orderSubmitId));
                                                                }
                                                                catch (Exception e)
                                                                {

                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            resultMasterList[masterItemNumber].Order_ID = Convert.ToUInt64(0);
                                                            //order_Submit_Id_List.Add(Convert.ToUInt64(0));
                                                        }
                                                        resultORCList.Add(resOrc);
                                                        if (resultORCList.Count > 0)
                                                        {
                                                            OrcItemNumber = resultORCList.Count - 1;
                                                        }
                                                    }
                                                    else if (segmentName.ToUpper() == "OBR")
                                                    {
                                                        allergyCommentsFlag = false;
                                                        ResultOBR resObr = (ResultOBR)instance;
                                                        resObr.Result_Master_ID = Convert.ToUInt64(masterItemNumber);
                                                        resObr.Created_By = "ACURUS";
                                                        resObr.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                                        resultOBRList.Add(resObr);
                                                        if (resultOBRList.Count > 0)
                                                        {
                                                            ObrItemNumber = resultOBRList.Count - 1;
                                                        }
                                                    }
                                                    else if (segmentName.ToUpper() == "OBX")
                                                    {
                                                        allergyCommentsFlag = false;
                                                        ResultOBX resObx = (ResultOBX)instance;
                                                        resObx.Result_Master_ID = Convert.ToUInt64(masterItemNumber);
                                                        resObx.Result_OBR_ID = Convert.ToUInt64(ObrItemNumber);
                                                        resObx.Created_By = "ACURUS";
                                                        resObx.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                                        resultOBXList.Add(resObx);
                                                        if (resultOBXList.Count > 0)
                                                        {
                                                            ObxItemNmber = resultOBXList.Count - 1;
                                                        }
                                                    }
                                                    else if (segmentName.ToUpper() == "NTE")
                                                    {
                                                        if (prevSegmentName.Contains("PID") == true)
                                                        {
                                                            allergyCommentsFlag = true;
                                                            ResultNTE resNte = (ResultNTE)instance;
                                                            resNte.Comment_Type = "Allergy Comments";
                                                            resNte.Result_Master_ID = Convert.ToUInt64(masterItemNumber);
                                                            resNte.Result_OBR_ID = 0;
                                                            resNte.Result_OBX_ID = 0;
                                                            //To identify to which MSh this allergy comments belong. There will not be any null in MSH so with the index the allergy comments can be tied at manager side.
                                                            resNte.Result_Master_ID = Convert.ToUInt64(masterItemNumber);
                                                            resNte.Created_By = "ACURUS";
                                                            resNte.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                                            resultNTEList.Add(resNte);
                                                        }
                                                        else
                                                        {
                                                            if (allergyCommentsFlag == true)
                                                            {
                                                                ResultNTE resNte = (ResultNTE)instance;
                                                                resNte.Comment_Type = "Allergy Comments";
                                                                resNte.Result_Master_ID = Convert.ToUInt64(masterItemNumber);
                                                                resNte.Result_OBR_ID = 0;
                                                                resNte.Result_OBX_ID = 0;
                                                                //To identify to which MSh this allergy comments belong. There will not be any null in MSH so with the index the allergy comments can be tied at manager side.
                                                                resNte.Result_Master_ID = Convert.ToUInt64(masterItemNumber);
                                                                resNte.Created_By = "ACURUS";
                                                                resNte.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                                                resultNTEList.Add(resNte);
                                                            }
                                                            else
                                                            {
                                                                ResultNTE resNte = (ResultNTE)instance;
                                                                resNte.Result_Master_ID = Convert.ToUInt64(masterItemNumber);
                                                                resNte.Result_OBR_ID = Convert.ToUInt64(ObrItemNumber);
                                                                resNte.Result_OBX_ID = Convert.ToUInt64(ObxItemNmber);
                                                                resNte.Comment_Type = "ASR Comments";
                                                                resNte.Created_By = "ACURUS";
                                                                resNte.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                                                resultNTEList.Add(resNte);
                                                            }
                                                        }
                                                    }
                                                    else if (segmentName.ToUpper() == "ZEF")
                                                    {
                                                        allergyCommentsFlag = false;
                                                        ResultZEF resZef = (ResultZEF)instance;
                                                        resZef.Result_Master_ID = Convert.ToUInt64(masterItemNumber);
                                                        resZef.Created_By = "ACURUS";
                                                        resZef.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                                        resultZEFList.Add(resZef);
                                                    }
                                                    else if (segmentName.ToUpper() == "ZPS")
                                                    {
                                                        allergyCommentsFlag = false;
                                                        ResultZPS resZps = (ResultZPS)instance;
                                                        resZps.Result_Master_ID = Convert.ToUInt64(masterItemNumber);
                                                        resZps.Created_By = "ACURUS";
                                                        resZps.Created_Date_And_Time = System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
                                                        resultZPSList.Add(resZps);
                                                    }
                                                }
                                                /*MSH,PID,ORC contains in single table so to avoid creating new instance the
                                                 *previous domain name is maintained.*/
                                                prevDomainName = domain;
                                                prevSegmentName = segmentName;
                                            }
                                        }
                                    }
                                }
                            }
                            if (resultMasterList.Count > 0 || resultORCList.Count > 0 || resultOBRList.Count > 0 || resultNTEList.Count > 0 || resultOBXList.Count > 0 || resultZEFList.Count > 0 || resultZPSList.Count > 0)
                            {
                                if (resultMasterList.Any(a => a.PID_External_Patient_ID.Trim() == string.Empty))
                                {
                                    StringBuilder logmsg = new StringBuilder();
                                    foreach (ResultMaster resMstr in resultMasterList.Where(a => a.PID_External_Patient_ID.Trim() == string.Empty))
                                    {
                                        logmsg.Append("Warning : Result Without Human ID is received for a patient named at " + resMstr.PID_Patient_Last_Name + " " + resMstr.PID_Patient_First_Name + " " + resMstr.PID_Patient_Middle_Name + " " + DateTime.Now.ToString() + Environment.NewLine);

                                    }
                                    using (TextWriter tx = new StreamWriter(Program.LabAgentLog, true))
                                    {
                                        tx.WriteLine(logmsg);
                                    }

                                }
                                /*Save to tables.*/
                                //if (.Name.Contains("Others"))
                                if (resultMasterList.Count > 0)
                                {
                                    foreach (ResultMaster resMaster in resultMasterList)
                                    {
                                        resMaster.Lab_ID = Lab_ID;
                                        resMaster.Is_Electronic_Mode = "Y";
                                    }

                                }
                                if (rgFiles[0].FullName.Contains("Others"))
                                {
                                    resultMasterProxy.ImprotLabResultsForOutsideOrders(resultMasterList.ToArray<ResultMaster>(), resultORCList.ToArray<ResultORC>(), resultOBRList.ToArray<ResultOBR>(), resultOBXList.ToArray<ResultOBX>(), resultNTEList.ToArray<ResultNTE>(), resultZEFList.ToArray<ResultZEF>(), resultZPSList.ToArray<ResultZPS>(), "");
                                }
                                else
                                {
                                    bUnimportedFile = resultMasterProxy.ImprotLabResultsForCapellaOrders(resultMasterList.ToArray<ResultMaster>(), resultORCList.ToArray<ResultORC>(), resultOBRList.ToArray<ResultOBR>(), resultOBXList.ToArray<ResultOBX>(), resultNTEList.ToArray<ResultNTE>(), resultZEFList.ToArray<ResultZEF>(), resultZPSList.ToArray<ResultZPS>(), "");
                                }
                                //                           resultMasterProxy.SaveResultMaster(resultMasterList.ToArray<ResultMaster>(), resultORCList.ToArray<ResultORC>(), resultOBRList.ToArray<ResultOBR>(), resultOBXList.ToArray<ResultOBX>(), resultNTEList.ToArray<ResultNTE>(), resultZEFList.ToArray<ResultZEF>(), resultZPSList.ToArray<ResultZPS>());
                                resultMasterList.Clear();
                                resultORCList.Clear();
                                resultOBRList.Clear();
                                resultOBXList.Clear();
                                resultNTEList.Clear();
                                resultZEFList.Clear();
                                resultZPSList.Clear();

                            }
                            //DirectoryInfo dir = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["ResultProcessedPath"]);
                            //if (dir.Exists == true)
                            //{
                            if (!Directory.Exists(fi.DirectoryName + "\\Imported\\"))
                                Directory.CreateDirectory(fi.DirectoryName + "\\Imported\\");

                            if (bUnimportedFile == true)
                            {
                                if (!Directory.Exists(fi.DirectoryName + "\\UnImported\\"))
                                    Directory.CreateDirectory(fi.DirectoryName + "\\UnImported\\");
                            }
                            try
                            {
                                if (bUnimportedFile != true)
                                {
                                    System.IO.File.Move(fi.FullName, fi.DirectoryName + "\\Imported\\" + fi.Name);
                                }
                                else
                                {
                                    System.IO.File.Move(fi.FullName, fi.DirectoryName + "\\UnImported\\" + fi.Name);
                                    StringBuilder logmsg = new StringBuilder();
                                    logmsg.Append("Warning : File Moved to Unimported Folder " + fi.Name + " " + DateTime.Now.ToString() + Environment.NewLine);
                                    using (TextWriter tx = new StreamWriter(Program.LabAgentLog, true))
                                    {
                                        tx.WriteLine(logmsg);
                                    }
                                }
                            }
                            catch
                            {
                                //if(File.Exists(fi.DirectoryName + "\\Imported\\" + fi.Name))

                                if (bUnimportedFile != true)
                                {
                                    //System.IO.File.Move(fi.FullName, fi.DirectoryName + "\\Imported\\" + fi.Name.Replace(fi.Name, fi.Name.Replace(".dat", "") + "_" + DateTime.Now.ToString("dd-MMM-yyyy hh:mmttss")+fi.Extension));
                                    System.IO.File.Move(fi.FullName, fi.DirectoryName + @"\Imported\" + fi.Name.Replace(fi.Extension, "") + DateTime.Now.ToString("_dd-MM-yyyy_HHmmss") + fi.Extension);
                                }
                                else
                                {
                                    System.IO.File.Move(fi.FullName, fi.DirectoryName + @"\UnImported\" + fi.Name.Replace(fi.Extension, "") + DateTime.Now.ToString("_dd-MM-yyyy_HHmmss") + fi.Extension);
                                    StringBuilder logmsg = new StringBuilder();
                                    logmsg.Append("Warning : File Moved to Unimported Folder " + fi.Name + " " + DateTime.Now.ToString() + Environment.NewLine);
                                    using (TextWriter tx = new StreamWriter(Program.LabAgentLog, true))
                                    {
                                        tx.WriteLine(logmsg);
                                    }
                                }

                            }

                        }
                        catch (Exception e)
                        {

                            if (!Directory.Exists(fi.DirectoryName + "\\UnImported\\"))
                                Directory.CreateDirectory(fi.DirectoryName + "\\UnImported\\");

                            try
                            {

                                System.IO.File.Move(fi.FullName, fi.DirectoryName + "\\UnImported\\" + fi.Name);
                                StringBuilder logmsg = new StringBuilder();
                                logmsg.Append("Unidentified Exception " + Environment.NewLine);

                                logmsg.Append("Warning : File Moved to Unimported Folder " + fi.Name + " " + DateTime.Now.ToString() + Environment.NewLine);

                                logmsg.Append("Error Date and Time : " + DateTime.Now.ToString() + " - ");
                                logmsg.Append("Error Message : " + e.Message.ToString() + " - ");
                                if (e.InnerException != null)
                                    logmsg.Append(e.InnerException.Message != null ? "InnerException Message : " + e.InnerException.Message.ToString() + " - " : "");
                                else
                                    logmsg.Append("Error : " + e.ToString() + Environment.NewLine);

                                logmsg.Append("Stack Trace : " + e.StackTrace.ToString() + Environment.NewLine);
                                using (TextWriter tx = new StreamWriter(Program.LabAgentLog, true))
                                {
                                    tx.WriteLine(logmsg);
                                }

                            }
                            catch
                            {
                                System.IO.File.Move(fi.FullName, fi.DirectoryName + "\\UnImported\\" + fi.Name);
                                StringBuilder logmsg = new StringBuilder();
                                logmsg.Append("Unidentified Exception " + Environment.NewLine);

                                logmsg.Append("Warning : File Moved to Unimported Folder " + fi.Name + " " + DateTime.Now.ToString() + Environment.NewLine);

                                logmsg.Append("Error Date and Time : " + DateTime.Now.ToString() + " - ");
                                logmsg.Append("Error Message : " + e.Message.ToString() + " - ");
                                if (e.InnerException != null)
                                    logmsg.Append(e.InnerException.Message != null ? "InnerException Message : " + e.InnerException.Message.ToString() + " - " : "");
                                else
                                    logmsg.Append("Error : " + e.ToString() + Environment.NewLine);

                                logmsg.Append("Stack Trace : " + e.StackTrace.ToString() + Environment.NewLine);
                                using (TextWriter tx = new StreamWriter(Program.LabAgentLog, true))
                                {
                                    tx.WriteLine(logmsg);
                                }
                            }

                        }
                        //}
                        //else
                        //{
                        //DirectoryInfo d = Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["ResultProcessedPath"]);
                        //System.IO.File.Move(fi.FullName, System.Configuration.ConfigurationSettings.AppSettings["ResultProcessedPath"] + "\\" + fi.Name);
                        //}
                    }
                }

            }
        }
        public static IEnumerable<FileInfo[]> GetFilesFromDirectory()
        {
            IList<string> FolderLocations = new List<string>();

            FolderLocations.Add(System.Configuration.ConfigurationSettings.AppSettings["ResultReceivedPath"]);
            FolderLocations.Add(System.Configuration.ConfigurationSettings.AppSettings["QuestResultPath"]);
            FolderLocations.Add(System.Configuration.ConfigurationSettings.AppSettings["SachReceivedPath"]);
            FolderLocations.Add(System.Configuration.ConfigurationSettings.AppSettings["OrderNotMadeThroughCapellaReceiverFilePathName"]);
            foreach (string str in FolderLocations)
            {
                if (!Directory.Exists(str))
                    Directory.CreateDirectory(str);
                DirectoryInfo di = new DirectoryInfo(str);
                if (di.Exists == true)
                {
                    FileInfo[] rgFiles;
                    //What about Quest - .hl7????
                    // if (!di.Name.Contains("Quest") && !di.Name.Contains("Sach"))//For bug Id 60056
                    if (di.Name.ToUpper().Contains("LABCORP"))
                        ResultsUtilityManager.Lab_ID = 1;
                    //rgFiles = di.GetFiles("*.dat");
                    else if (di.Name.ToUpper().Contains("QUEST"))
                        ResultsUtilityManager.Lab_ID = 2;
                    else if (di.Name.ToUpper().Contains("SACH"))
                        ResultsUtilityManager.Lab_ID = 13;
                    //    rgFiles = di.GetFiles("*.hl7");
                    //else
                    //    rgFiles = di.GetFiles();
                    rgFiles = di.GetFiles("*.dat");
                    yield return rgFiles;
                }

            }
        }
        #endregion
    }
}
