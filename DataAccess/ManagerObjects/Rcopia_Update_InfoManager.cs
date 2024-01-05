using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acurus.Capella.Core.DomainObjects;
using NHibernate;
using NHibernate.Criterion;
using System.Xml;
using System.Globalization;
using System.Collections;
using System.IO;

namespace Acurus.Capella.DataAccess.ManagerObjects
{

    public partial interface IRcopia_Update_InfoManager : IManagerBase<Rcopia_Update_info, ulong>
    {
        IList<Rcopia_Update_info> GetRcopiaUpdateInfo();
        void InsertinToRcopia_Update_info(string sCommand, DateTime dtLastUpdateTime, string sValue, string sMacAddress, string sLegalOrg);
        //void DownloadRCopiaInfo(string sDownloadAddress, string sUserName, string sMACAddress, DateTime dtClientDate, string sFacilityName, ulong EncID);
        //Jira CAP-1366
        //void DownloadRCopiaInfo(string sUploadAddress, string sUserName, string sMACAddress, DateTime dtClientDate, string sFacilityName, ulong EncID, ulong ulHumanID, string sLegalOrg);
        string DownloadRCopiaInfo(string sUploadAddress, string sUserName, string sMACAddress, DateTime dtClientDate, string sFacilityName, ulong EncID, ulong ulHumanID, string sLegalOrg);
        string GetRcopiaUpdateInfoCommandName(string sCommandName);
    }

    public partial class Rcopia_Update_InfoManager : ManagerBase<Rcopia_Update_info, ulong>, IRcopia_Update_InfoManager
    {


        #region Constructors

        public Rcopia_Update_InfoManager()
            : base()
        {

        }
        public Rcopia_Update_InfoManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion



        #region Methods
        CultureInfo culture = new CultureInfo("en-US");

        public IList<Rcopia_Update_info> GetRcopiaUpdateInfo()
        {
            IList<Rcopia_Update_info> ilstRcopiaUpdateInfo = GetAll();
            return ilstRcopiaUpdateInfo;
        }

        public string GetRcopiaUpdateInfoCommandName(string sCommandName)
        {

            IList<Rcopia_Update_info> RcopiaUpdateList = new List<Rcopia_Update_info>();
            ArrayList aryRcopiaUpdateList;
            string ResultDate = "";
            ISession mySession = NHibernateSessionManager.Instance.CreateISession();
            IQuery query1 = mySession.GetNamedQuery("GetRcopiaLastUpdateDateTime.UsingCommand");
            query1.SetString(0, sCommandName);


            aryRcopiaUpdateList = new ArrayList(query1.List());
            mySession.Close();
            foreach (string oj in aryRcopiaUpdateList)
            {
                ResultDate = oj.ToString();
            }
            return ResultDate;
        }

        public string GetRcopiaUpdateInfoCommandNameAndLegalOrg(string sCommandName, string sLegalOrg)
        {

            IList<Rcopia_Update_info> RcopiaUpdateList = new List<Rcopia_Update_info>();
            ArrayList aryRcopiaUpdateList;
            string ResultDate = "";
            ISession mySession = NHibernateSessionManager.Instance.CreateISession();
            IQuery query1 = mySession.GetNamedQuery("GetRcopiaLastUpdateDateTimeByLegalOrg.UsingCommand");
            query1.SetString(0, sCommandName);
            query1.SetString(1, sLegalOrg);


            aryRcopiaUpdateList = new ArrayList(query1.List());
            mySession.Close();
            foreach (string oj in aryRcopiaUpdateList)
            {
                ResultDate = oj.ToString();
            }
            return ResultDate;
        }

        public void InsertinToRcopia_Update_info(string sCommand, DateTime dtLastUpdateTime, string sValue, string sMacAddress, string sLegalOrg)
        {
            IList<Rcopia_Update_info> ilstUpdateInfo = new List<Rcopia_Update_info>();
            IList<Rcopia_Update_info> ilstinsert = null;
            Rcopia_Update_info rcopiaupdate = new Rcopia_Update_info();
            //ISession mySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(Rcopia_Update_info)).Add(Expression.Eq("Command", sCommand)).Add(Expression.Eq("Legal_Org", sLegalOrg));
                if (crit.List<Rcopia_Update_info>().Count > 0)
                {
                    rcopiaupdate = crit.List<Rcopia_Update_info>()[0];
                    rcopiaupdate.Last_Updated_Date_Time = dtLastUpdateTime;
                    rcopiaupdate.Value = sValue;
                    rcopiaupdate.Legal_Org = sLegalOrg;
                    ilstUpdateInfo.Add(rcopiaupdate);
                }
                mySession.Close();
            }
            //SaveUpdateDeleteWithTransaction(ref ilstinsert, ilstUpdateInfo, null, sMacAddress);
            SaveUpdateDelete_DBAndXML_WithTransaction(ref ilstinsert, ref ilstUpdateInfo, null, sMacAddress, false, false, 0, string.Empty);

        }
        //Old DownloadRCopiaInfo
        //public void DownloadRCopiaInfo(string sDownloadAddress, string sUserName, string sMACAddress, DateTime dtClientDate, string sFacilityName, ulong EncID)
        //{
        //    RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager();

        //    RCopiaGenerateXML rcopiaXML = new RCopiaGenerateXML();
        //    RCopiaTransactionManager tranMngr = new RCopiaTransactionManager();
        //    RCopiaXMLResponseProcess rcopiaResponseXML = new RCopiaXMLResponseProcess();
        //    string sInputXML = string.Empty;
        //    string sOutputXML = string.Empty;
        //    //As per the meeting with Krishnan on 15-Mar-2012
        //    //The user need not change any demographics details through RCopia - Demograchics module
        //    //Even if they change, we can not let the data to be updated from external(rcopia) to our human table
        //    //Because, this will affect our existing human details. If we do that, there will be no control.

        //    //We should actually compare the LastName+FirstName+DOB+Gender - before we update our human table.
        //    //But, because of time constraint, temporarily the updatehumantable call is disabled.

        //    //sInputXML =  rcopiaXML.CreateUpdatePatientXML();

        //    //if (sUploadAddress != null)
        //    //{
        //    //    sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.UploadAddress + sInputXML, 1);
        //    //    rcopiaResponseXML.ReadXMLResponse(sOutputXML, sMACAddress);

        //    //    XmlDocument XMLDoc = new XmlDocument();
        //    //    XMLDoc.LoadXml(sOutputXML);
        //    //    XmlNodeList xmlReqNode = XMLDoc.GetElementsByTagName("LastUpdateDate");
        //    //    string CmdElementText1 = ((XmlElement)xmlReqNode[1]).InnerText;
        //    //    xmlReqNode = XMLDoc.GetElementsByTagName("Command");
        //    //    string CmdElementText2 = ((XmlElement)xmlReqNode[0]).InnerText;

        //    //    InsertinToRcopia_Update_info(CmdElementText2 ,Convert.ToDateTime(CmdElementText1,culture),string.Empty, sMACAddress);
        //    //}
        //    //BugID:51252  Changed uploadAddress to downloadAddress
        //    WorkFlowManager obj = new WorkFlowManager();
        //    sInputXML = rcopiaXML.CreateUpdateAllergyXML();
        //    if (rcopiaSessionMngr.DownloadAddress != null)
        //    {
        //        sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.DownloadAddress + sInputXML, 1);

        //        rcopiaResponseXML.ReadXMLResponse(sOutputXML, sUserName, sMACAddress, dtClientDate, sFacilityName, EncID);

        //        XmlDocument XMLDoc = new XmlDocument();
        //        if (sOutputXML != null)
        //        {
        //            XMLDoc.LoadXml(sOutputXML);

        //            XmlNodeList xmlReqNode = XMLDoc.GetElementsByTagName("LastUpdateDate");
        //            string CmdElementText1 = ((XmlElement)xmlReqNode[1]).InnerText;
        //            xmlReqNode = XMLDoc.GetElementsByTagName("Command");
        //            string CmdElementText2 = ((XmlElement)xmlReqNode[0]).InnerText;

        //            InsertinToRcopia_Update_info(CmdElementText2, Convert.ToDateTime(CmdElementText1, culture), string.Empty, sMACAddress);

        //        }
        //    }

        //    sInputXML = rcopiaXML.CreateUpdateMedicationXML();

        //    if (rcopiaSessionMngr.DownloadAddress != null)
        //    {

        //        sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.DownloadAddress + sInputXML, 1);

        //        rcopiaResponseXML.ReadXMLResponse(sOutputXML, sUserName, sMACAddress, dtClientDate, sFacilityName, EncID);

        //        XmlDocument XMLDoc = new XmlDocument();
        //        if (sOutputXML != null)
        //        {
        //            XMLDoc.LoadXml(sOutputXML);
        //            XmlNodeList xmlReqNode = XMLDoc.GetElementsByTagName("LastUpdateDate");
        //            string CmdElementText1 = ((XmlElement)xmlReqNode[1]).InnerText;
        //            xmlReqNode = XMLDoc.GetElementsByTagName("Command");
        //            string CmdElementText2 = ((XmlElement)xmlReqNode[0]).InnerText;
        //            InsertinToRcopia_Update_info(CmdElementText2, Convert.ToDateTime(CmdElementText1, culture), string.Empty, sMACAddress);

        //        }
        //    }
        //    //sInputXML = rcopiaXML.CreateUpdateProblemXML();
        //    //if (RCopiaSessionManager.UploadAddress != null)
        //    //{
        //    //    sOutputXML = rcopiaSessionMngr.HttpPost(RCopiaSessionManager.UploadAddress + sInputXML, 1);
        //    //    rcopiaResponseXML.ReadXMLResponse(sOutputXML, sMACAddress);
        //    //}

        //    sInputXML = rcopiaXML.CreateUpdatePrescriptionXML();
        //    if (rcopiaSessionMngr.DownloadAddress != null)
        //    {

        //        sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.DownloadAddress + sInputXML, 1);

        //        rcopiaResponseXML.ReadXMLResponse(sOutputXML, sUserName, sMACAddress, dtClientDate, sFacilityName, EncID);
        //        XmlDocument XMLDoc = new XmlDocument();
        //        if (sOutputXML != null)
        //        {
        //            XMLDoc.LoadXml(sOutputXML);
        //            XmlNodeList xmlReqNode = XMLDoc.GetElementsByTagName("LastUpdateDate");
        //            string CmdElementText1 = ((XmlElement)xmlReqNode[1]).InnerText;
        //            xmlReqNode = XMLDoc.GetElementsByTagName("Command");
        //            string CmdElementText2 = ((XmlElement)xmlReqNode[0]).InnerText;
        //            InsertinToRcopia_Update_info(CmdElementText2, Convert.ToDateTime(CmdElementText1, culture), string.Empty, sMACAddress);

        //        }
        //    }


        //}



        //Patient Level RCopia Download - Commented
        public string DownloadRCopiaInfo(string sDownloadAddress, string sUserName, string sMACAddress, DateTime dtClientDate, string sFacilityName, ulong EncID, ulong ulHumanID, string sLegalOrg)
        {
            RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
            RCopiaGenerateXML rcopiaXML = new RCopiaGenerateXML();
            RCopiaTransactionManager tranMngr = new RCopiaTransactionManager();
            RCopiaXMLResponseProcess rcopiaResponseXML = new RCopiaXMLResponseProcess();
            string sInputXML = string.Empty;
            string sOutputXML = string.Empty;
            //As per the meeting with Krishnan on 15-Mar-2012
            //The user need not change any demographics details through RCopia - Demograchics module
            //Even if they change, we can not let the data to be updated from external(rcopia) to our human table
            //Because, this will affect our existing human details. If we do that, there will be no control.

            //We should actually compare the LastName+FirstName+DOB+Gender - before we update our human table.
            //But, because of time constraint, temporarily the updatehumantable call is disabled.

            //sInputXML =  rcopiaXML.CreateUpdatePatientXML();

            //if (sUploadAddress != null)
            //{
            //    sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.UploadAddress + sInputXML, 1);
            //    rcopiaResponseXML.ReadXMLResponse(sOutputXML, sMACAddress);

            //    XmlDocument XMLDoc = new XmlDocument();
            //    XMLDoc.LoadXml(sOutputXML);
            //    XmlNodeList xmlReqNode = XMLDoc.GetElementsByTagName("LastUpdateDate");
            //    string CmdElementText1 = ((XmlElement)xmlReqNode[1]).InnerText;
            //    xmlReqNode = XMLDoc.GetElementsByTagName("Command");
            //    string CmdElementText2 = ((XmlElement)xmlReqNode[0]).InnerText;

            //    InsertinToRcopia_Update_info(CmdElementText2 ,Convert.ToDateTime(CmdElementText1,culture),string.Empty, sMACAddress);
            //}
            //BugID:51252  Changed uploadAddress to downloadAddress

            DateTime dtRCopia_Allergy_Last_Updated_Date_Time = DateTime.MinValue;
            DateTime dtRCopia_Medication_Last_Updated_Date_Time = DateTime.MinValue;
            DateTime dtRCopia_Prescription_Last_Updated_Date_Time = DateTime.MinValue;

            HumanManager humanMngr = new HumanManager();
            Human objHuman = null;
            DateTime dtHumanCreatedDateTime = DateTime.MinValue;
            if (ulHumanID != 0)
            {
                objHuman = humanMngr.GetHumanFromHumanID(ulHumanID);
                if (objHuman != null)
                {
                    dtRCopia_Allergy_Last_Updated_Date_Time = objHuman.RCopia_Allergy_Last_Updated_Date_Time;
                    dtRCopia_Medication_Last_Updated_Date_Time = objHuman.RCopia_Medication_Last_Updated_Date_Time;
                    dtRCopia_Prescription_Last_Updated_Date_Time = objHuman.RCopia_Prescription_Last_Updated_Date_Time;
                    dtHumanCreatedDateTime = objHuman.Created_Date_And_Time;
                }
            }

            WorkFlowManager obj = new WorkFlowManager();

            sInputXML = rcopiaXML.CreateUpdateAllergyXML(ulHumanID, dtRCopia_Allergy_Last_Updated_Date_Time, sLegalOrg, dtHumanCreatedDateTime);
            if (rcopiaSessionMngr.DownloadAddress != null && sInputXML != string.Empty)
            {
                sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.DownloadAddress + sInputXML, 1, sUserName);
                //Jira CAP-1366
                if (sOutputXML != null && sOutputXML.StartsWith("HttpPostError") == true)
                {
                    return sOutputXML;
                }
                rcopiaResponseXML.ReadXMLResponse(sOutputXML, sUserName, sMACAddress, dtClientDate, sFacilityName, EncID, sLegalOrg);
                XmlDocument XMLDoc = new XmlDocument();
                if (sOutputXML != null)
                {
                    XMLDoc.LoadXml(sOutputXML);

                    XmlNodeList xmlReqNode = XMLDoc.GetElementsByTagName("LastUpdateDate");
                    string CmdElementText1 = ((XmlElement)xmlReqNode[1]).InnerText;
                    //xmlReqNode = XMLDoc.GetElementsByTagName("Command");
                    //string CmdElementText2 = ((XmlElement)xmlReqNode[0]).InnerText;

                    //InsertinToRcopia_Update_info(CmdElementText2, Convert.ToDateTime(CmdElementText1, culture), string.Empty, sMACAddress);
                    if (CmdElementText1.ToString() != string.Empty)
                    {
                        objHuman.RCopia_Allergy_Last_Updated_Date_Time = Convert.ToDateTime(CmdElementText1.ToString());
                    }
                }
            }

            sInputXML = rcopiaXML.CreateUpdateMedicationXML(ulHumanID, dtRCopia_Medication_Last_Updated_Date_Time, sLegalOrg, dtHumanCreatedDateTime);

            if (rcopiaSessionMngr.DownloadAddress != null && sInputXML != string.Empty)
            {
                sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.DownloadAddress + sInputXML, 1, sUserName);
                //Jira CAP-1366
                if (sOutputXML != null && sOutputXML.StartsWith("HttpPostError") == true)
                {
                    return sOutputXML;
                }
                rcopiaResponseXML.ReadXMLResponse(sOutputXML, sUserName, sMACAddress, dtClientDate, sFacilityName, EncID, sLegalOrg);
                XmlDocument XMLDoc = new XmlDocument();
                if (sOutputXML != null)
                {
                    XMLDoc.LoadXml(sOutputXML);
                    XmlNodeList xmlReqNode = XMLDoc.GetElementsByTagName("LastUpdateDate");
                    string CmdElementText1 = ((XmlElement)xmlReqNode[1]).InnerText;
                    //xmlReqNode = XMLDoc.GetElementsByTagName("Command");
                    //string CmdElementText2 = ((XmlElement)xmlReqNode[0]).InnerText;

                    //InsertinToRcopia_Update_info(CmdElementText2, Convert.ToDateTime(CmdElementText1, culture), string.Empty, sMACAddress);

                    if (CmdElementText1.ToString() != string.Empty)
                    {
                        objHuman.RCopia_Medication_Last_Updated_Date_Time = Convert.ToDateTime(CmdElementText1.ToString());
                    }
                }
            }
            //sInputXML = rcopiaXML.CreateUpdateProblemXML();
            //if (RCopiaSessionManager.UploadAddress != null)
            //{
            //    sOutputXML = rcopiaSessionMngr.HttpPost(RCopiaSessionManager.UploadAddress + sInputXML, 1);
            //    rcopiaResponseXML.ReadXMLResponse(sOutputXML, sMACAddress);
            //}


            sInputXML = rcopiaXML.CreateUpdatePrescriptionXML(ulHumanID, dtRCopia_Prescription_Last_Updated_Date_Time, sLegalOrg, dtHumanCreatedDateTime);

            if (rcopiaSessionMngr.DownloadAddress != null && sInputXML != string.Empty)
            {
                sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.DownloadAddress + sInputXML, 1, sUserName);
                //Jira CAP-1366
                if (sOutputXML != null && sOutputXML.StartsWith("HttpPostError") == true)
                {
                    return sOutputXML;
                }
                rcopiaResponseXML.ReadXMLResponse(sOutputXML, sUserName, sMACAddress, dtClientDate, sFacilityName, EncID, sLegalOrg);
                XmlDocument XMLDoc = new XmlDocument();
                if (sOutputXML != null)
                {
                    XMLDoc.LoadXml(sOutputXML);
                    XmlNodeList xmlReqNode = XMLDoc.GetElementsByTagName("LastUpdateDate");
                    string CmdElementText1 = ((XmlElement)xmlReqNode[1]).InnerText;
                    //xmlReqNode = XMLDoc.GetElementsByTagName("Command");
                    //string CmdElementText2 = ((XmlElement)xmlReqNode[0]).InnerText;

                    //InsertinToRcopia_Update_info(CmdElementText2, Convert.ToDateTime(CmdElementText1, culture), string.Empty, sMACAddress);
                    if (CmdElementText1.ToString() != string.Empty)
                    {
                        objHuman.RCopia_Prescription_Last_Updated_Date_Time = Convert.ToDateTime(CmdElementText1.ToString());
                    }
                }
            }

            if (objHuman != null)
            {
                humanMngr.UpdateToHuman(objHuman, string.Empty);
            }

            return string.Empty;

        }
        #endregion
    }
}
