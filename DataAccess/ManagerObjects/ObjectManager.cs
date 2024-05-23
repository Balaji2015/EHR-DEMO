using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DTO;
using NHibernate;
using System.Linq;
using Acurus.Capella.Core.DomainObjects;
using System.Web;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
//Added By Selvaraman
namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial class ObjectManager
    {
        public IList<MyQ> resultmyqList = new List<MyQ>();


        public IList<MyQ> FillMyObjects(string FacName, string[] ObjType, string ProcessType, string UserName, Boolean bShowAll, int DefaultNoofDays)
        {

            ArrayList FavoriteList = null;
            IQuery query1 = null;
            MyQ myq = new MyQ();
            // ISession Mysession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession Mysession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {
                    if (ObjType.Contains("ENCOUNTER") || ObjType.Contains("DOCUMENTATION") || ObjType.Contains("DOCUMENT REVIEW") || ObjType.Contains("CHECK OUT") || ObjType.Contains("PHONE ENCOUNTER"))
                    {
                        //Srividhya added the below query on 30-May-2015==START
                        if (bShowAll == true)
                        {
                            //if (FacName=="ALL")
                            //{
                            //    query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithoutFacility.WithAllAppointments");
                            //    query1.SetString(0, UserName);
                            //    if (ProcessType == "UNASSIGNED")
                            //    {
                            //        query1.SetString(1, "UNKNOWN");
                            //    }
                            //    else
                            //    {
                            //        query1.SetString(1, UserName);
                            //    }
                            //}
                            //else
                            //{
                            //    query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithAllAppointments");
                            //    query1.SetString(0, UserName);
                            //    if (ProcessType == "UNASSIGNED")
                            //    {
                            //        query1.SetString(1, "UNKNOWN");
                            //    }
                            //    else
                            //    {
                            //        query1.SetString(1, UserName);
                            //    }
                            //    query1.SetString(2, FacName);
                            //}

                            query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithoutFacility.WithFacility.WithAllAppointments");
                            query1.SetString(0, UserName);
                            //if (ProcessType == "UNASSIGNED")
                            //{
                            //    query1.SetString(1, "UNKNOWN");
                            //}
                            //else
                            //{
                            //    query1.SetString(1, UserName);
                            //}
                            //    query1.SetString(2, FacName);
                        }
                        else
                        {
                            query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithoutCurrentOwnerWithoutFacility.WithoutAllAppointments");
                            //query1.SetInt32(0, DefaultNoofDays);
                            query1.SetString(0, UserName);
                            //if (ProcessType == "UNASSIGNED")
                            //{
                            //    query1.SetString(2, "UNKNOWN");
                            //}
                            //else
                            //{
                            //    query1.SetString(2, UserName);
                            //}
                            //query1.SetString(3, FacName);
                        }
                        //Srividhya added the below query on 30-May-2015==END

                        #region Commented by Srividhya on 30-May-2015
                        //if (FacName == "ALL")
                        //{
                        //    if (bShowAll == true)
                        //    {
                        //        query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithoutFacility.WithAllAppointments");
                        //        query1.SetString(0, UserName);
                        //        if (ProcessType == "UNASSIGNED")
                        //        {
                        //            query1.SetString(1, "UNKNOWN");
                        //        }
                        //        else
                        //        {
                        //            query1.SetString(1, UserName);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithoutFacility.WithoutAllAppointments");
                        //        query1.SetInt32(0, DefaultNoofDays);
                        //        query1.SetString(1, UserName);
                        //        if (ProcessType == "UNASSIGNED")
                        //        {
                        //            query1.SetString(2, "UNKNOWN");
                        //        }
                        //        else
                        //        {
                        //            query1.SetString(2, UserName);
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    if (bShowAll == true)
                        //    {
                        //        query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithAllAppointments");
                        //        query1.SetString(0, UserName);
                        //        if (ProcessType == "UNASSIGNED")
                        //        {
                        //            query1.SetString(1, "UNKNOWN");
                        //        }
                        //        else
                        //        {
                        //            query1.SetString(1, UserName);
                        //        }
                        //        query1.SetString(2, FacName);
                        //    }
                        //    else
                        //    {
                        //        query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithoutAllAppointments");
                        //        query1.SetInt32(0, DefaultNoofDays);
                        //        query1.SetString(1, UserName);
                        //        if (ProcessType == "UNASSIGNED")
                        //        {
                        //            query1.SetString(2, "UNKNOWN");
                        //        }
                        //        else
                        //        {
                        //            query1.SetString(2, UserName);
                        //        }
                        //        query1.SetString(3, FacName);
                        //    }

                        //}
                        #endregion

                        query1.SetParameterList("ObjList", ObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, ObjType);
                    }
                    else if (ObjType.Contains("TASK"))
                    {
                        if (bShowAll == true)
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyTaskObjectDetails.WithoutFacility");
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyTaskObjectDetails.WithFacility");
                                query1.SetString(2, FacName);
                            }
                        }
                        else
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyTaskObjectDetails.WithoutFacility.ShowAllFalse");
                                // query1.SetInt32(2, DefaultNoofDays);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyTaskObjectDetails.WithFacility.ShowAllFalse");
                                query1.SetString(2, FacName);
                                //query1.SetInt32(2, DefaultNoofDays);
                            }
                        }
                        query1.SetString(0, UserName);
                        if (ProcessType == "UNASSIGNED")
                        {
                            query1.SetString(1, "UNKNOWN");
                        }
                        else
                        {
                            query1.SetString(1, UserName);
                        }
                        query1.SetParameterList("ObjList", ObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, ObjType);
                    }
                    else if (ObjType.Contains("ADDENDUM"))
                    {
                        if (FacName == "ALL")
                        {
                            if (bShowAll)
                                query1 = Mysession.GetNamedQuery("FillMyAddendumObjectDetails.WithoutFacility.WithAllAppointments");
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyAddendumObjectDetails.WithoutFacility.WithoutAllAppointments");
                                // query1.SetInt32(3, DefaultNoofDays);
                            }
                        }
                        else
                        {
                            if (bShowAll)
                            {
                                query1 = Mysession.GetNamedQuery("FillMyAddendumObjectDetails.WithFacility.WithAllAppointments");
                                query1.SetString(3, FacName);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyAddendumObjectDetails.WithFacility.WithoutAllAppointments");
                                query1.SetString(3, FacName);
                                //query1.SetInt32(4, DefaultNoofDays);
                            }
                        }

                        query1.SetDateTime(0, DateTime.MinValue);
                        query1.SetString(1, UserName);
                        if (ProcessType == "UNASSIGNED")
                        {
                            query1.SetString(2, "UNKNOWN");
                        }
                        else
                        {
                            query1.SetString(2, UserName);
                        }
                        query1.SetParameterList("ObjList", ObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, ObjType);
                    }
                    else if (ObjType.Contains("DICTATION_RESULT"))
                    {

                        if (FacName == "ALL")
                        {
                            if (bShowAll == true)
                            {
                                query1 = Mysession.GetNamedQuery("FillMyDictationObjectDetails.WithoutFacility.WithAllAppointments");
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyDictationObjectDetails.WithoutFacility.WithoutAllAppointments");
                                //query1.SetInt32("Days", DefaultNoofDays);
                            }
                        }
                        else
                        {
                            if (bShowAll == true)
                            {
                                query1 = Mysession.GetNamedQuery("FillMyDictationObjectDetails.WithFacility.WithAllAppointments");
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyDictationObjectDetails.WithFacility.WithoutAllAppointments");
                                //query1.SetInt32("Days", DefaultNoofDays);
                            }
                            query1.SetString("FacName", FacName);
                        }
                        query1.SetString("UserName", UserName);
                        if (ProcessType == "UNASSIGNED")
                        {
                            query1.SetString("CurOwner", "UNKNOWN");
                        }
                        else
                        {
                            query1.SetString("CurOwner", UserName);
                        }
                        query1.SetParameterList("ObjList", ObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, ObjType);
                    }
                    //Added By Srividhya For ACO on 2-Sep-2014
                    else if (ObjType.Contains("CARE_COORDINATE"))
                    {
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("Fill.MyCareCoordinate_ObjectDetails.All");
                        }
                        else if (FacName == "MyQ")//Added for ACO on 06-oct-2014
                        {
                            query1 = Mysession.GetNamedQuery("Fill.MyCareCoordinate_ObjectDetails.MyQ");
                        }
                        else//Added for ACO on 06-oct-2014
                        {
                            query1 = Mysession.GetNamedQuery("Fill.MyCareCoordinate_ObjectDetails.GeneralQ");
                        }

                        query1.SetString(0, UserName);
                        if (ProcessType == "UNASSIGNED")
                        {
                            query1.SetString(1, "UNKNOWN");
                        }
                        else
                        {
                            query1.SetString(1, UserName);
                        }
                        query1.SetParameterList("ObjList", ObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, ObjType);
                    }
                    //Added By Srividhya For ACO on 2-Sep-2014
                    if (ObjType.Contains("DIAGNOSTIC ORDER") || ObjType.Contains("IMAGE ORDER"))
                    {
                        string[] myObjType = new string[2];
                        myObjType[0] = "DIAGNOSTIC ORDER";
                        myObjType[1] = "IMAGE ORDER";
                        StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
                        if (bShowAll == true)
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithoutFacility");
                            }
                            else
                            {
                                //if (objStaticLookupMgr.getStaticLookupByFieldName("CMG FACILITY NAME")[0].Value.Trim().ToUpper() != FacName)
                                // if (sAncillary.Trim().ToUpper() != FacName)//Bug ID:33975
                                var facAncillary = from f in NHibernateSessionUtility.Instance.MyAncillaryFacilityList where f.Fac_Name == FacName select f;
                                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                                if (ilstFacAncillary.Count == 0)
                                {
                                    query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility");
                                    query1.SetString(2, FacName);
                                }
                                else
                                {
                                    query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility.CMG");
                                }
                            }
                        }
                        else
                        {

                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithoutFacility.ShowAllFalse");
                                //query1.SetInt32(2, DefaultNoofDays);
                            }
                            else
                            {
                                // if (sAncillary.ToUpper() != FacName)//Bug ID:33975
                                var facAncillary = from f in NHibernateSessionUtility.Instance.MyAncillaryFacilityList where f.Fac_Name == FacName select f;
                                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                                if (ilstFacAncillary.Count == 0)
                                {
                                    query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility.ShowAllFalse");
                                    query1.SetString(2, FacName);
                                    //query1.SetInt32(3, DefaultNoofDays);
                                }
                                else
                                {
                                    query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility.ShowAllFalse.CMG");
                                    //query1.SetInt32(2, DefaultNoofDays);
                                }
                            }
                        }
                        query1.SetString(0, UserName);
                        if (ProcessType == "UNASSIGNED")
                        {
                            query1.SetString(1, "UNKNOWN");
                        }
                        else
                        {
                            query1.SetString(1, UserName);
                        }
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, myObjType);

                        //
                        //StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
                        //IList<StaticLookup> stFieldLook = objStaticLookupMgr.getStaticLookupByFieldName("CMG LAB NAME");
                        //  IList<StaticLookup> stFieldLookFacilityName = objStaticLookupMgr.getStaticLookupByFieldName("CMG FACILITY NAME");
                        // if (FacName.ToString().ToUpper().Trim() == sAncillary.ToString().ToUpper().Trim())//Bug ID:33975
                        var vfacAncillary = from f in NHibernateSessionUtility.Instance.MyAncillaryFacilityList where f.Fac_Name == FacName select f;
                        IList<FacilityLibrary> lstFacAncillary = vfacAncillary.ToList<FacilityLibrary>();
                        if (lstFacAncillary.Count > 0)
                        {
                            if (resultmyqList != null && resultmyqList.Count > 0)
                            {
                                IList<MyQ> tempList = resultmyqList.Where(a => ((a.Current_Process == "RESULT_PROCESS" && a.EHR_Obj_Type == "DIAGNOSTIC ORDER" && a.CMG_Encounter_ID == 0) || (a.Current_Process == "MA_RESULTS")) && a.Lab_Name.Trim().ToUpper() == "CMG Anc.-1866 #101".Trim().ToUpper()).ToList<MyQ>();
                                resultmyqList = tempList;
                                //for (int iCount = 0; iCount < tempList.Count; iCount++)
                                //{
                                //    resultmyqList.RemoveAt(resultmyqList.IndexOf(tempList[iCount]));
                                //}
                            }
                        }
                        //else
                        //{
                        //    if (resultmyqList != null && resultmyqList.Count > 0)
                        //    {
                        //        IList<MyQ> tempList = resultmyqList.Where(a => a.Current_Process == "RESULT_PROCESS").ToList<MyQ>();
                        //        resultmyqList = resultmyqList.Except(tempList).ToList<MyQ>();//.RemoveAt(resultmyqList.IndexOf(tempList[iCount]));
                        //    }
                        //}

                        //
                        //StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
                        //IList<StaticLookup> stFieldLook = objStaticLookupMgr.getStaticLookupByFieldName("CMG LAB NAME");
                        //if (resultmyqList != null && resultmyqList.Count > 0)
                        //{
                        //    IList<MyQ> tempList = resultmyqList.Where(a => a.Current_Process == "RESULT_PROCESS" && a.EHR_Obj_Type == "DIAGNOSTIC ORDER" && a.CMG_Encounter_ID == 0 && a.Lab_Name.Trim().ToUpper() == stFieldLook[0].Value.Trim().ToUpper()).ToList<MyQ>();
                        //    resultmyqList = tempList;
                        //    //resultmyqList= resultmyqList.Except(tempList).ToList<MyQ>();//.RemoveAt(resultmyqList.IndexOf(tempList[iCount]));
                        //    //for (int iCount = 0; iCount < tempList.Count; iCount++)
                        //    //{
                        //    //    resultmyqList.RemoveAt(resultmyqList.IndexOf(tempList[iCount]));
                        //    //}
                        //}
                    }
                    //if (ObjType.Contains("INTERNAL DIAGNOSTIC ORDER") || ObjType.Contains("INTERNAL IMAGE ORDER"))
                    //{
                    //    string[] myObjType = new string[2];
                    //    myObjType[0] = "INTERNAL DIAGNOSTIC ORDER";
                    //    myObjType[1] = "INTERNAL IMAGE ORDER";
                    //    if (FacName == "ALL")
                    //    {
                    //        query1 = Mysession.GetNamedQuery("FillMyInternalLabImageOrderObjectDetails.WithoutFacility");
                    //    }
                    //    else
                    //    {
                    //        query1 = Mysession.GetNamedQuery("FillMyInternalLabImageOrderObjectDetails.WithFacility");
                    //        query1.SetString(2, FacName);
                    //    }
                    //    query1.SetString(0, UserName);
                    //    if (ProcessType == "UNASSIGNED")
                    //    {
                    //        query1.SetString(1, "UNKNOWN");
                    //    }
                    //    else
                    //    {
                    //        query1.SetString(1, UserName);
                    //    }
                    //    query1.SetParameterList("ObjList", myObjType);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, myObjType);
                    //}
                    //if (ObjType.Contains("INTERNAL ORDER"))
                    //{
                    //    string[] myObjType = new string[1];
                    //    myObjType[0] = "INTERNAL ORDER";
                    //    if (bShowAll == true)
                    //    {
                    //        if (FacName == "ALL")
                    //        {
                    //            query1 = Mysession.GetNamedQuery("FillMyInternalOrderObjectDetails.WithoutFacility");
                    //        }
                    //        else
                    //        {
                    //            query1 = Mysession.GetNamedQuery("FillMyInternalOrderObjectDetails.WithFacility");
                    //            query1.SetString(2, FacName);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (FacName == "ALL")
                    //        {
                    //            query1 = Mysession.GetNamedQuery("FillMyInternalOrderObjectDetails.WithoutFacility.ShowAllFalse");
                    //            //query1.SetInt32(2, DefaultNoofDays);
                    //        }
                    //        else
                    //        {
                    //            query1 = Mysession.GetNamedQuery("FillMyInternalOrderObjectDetails.WithFacility.ShowAllFalse");
                    //            query1.SetString(2, FacName);
                    //            //query1.SetInt32(3, DefaultNoofDays);
                    //        }
                    //    }
                    //    query1.SetString(0, UserName);
                    //    if (ProcessType == "UNASSIGNED")
                    //    {
                    //        query1.SetString(1, "UNKNOWN");
                    //    }
                    //    else
                    //    {
                    //        query1.SetString(1, UserName);
                    //    }
                    //    query1.SetParameterList("ObjList", myObjType);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, myObjType);
                    //}
                    if (ObjType.Contains("IMMUNIZATION ORDER"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "IMMUNIZATION ORDER";
                        if (bShowAll == true)
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyImmunizationOrderObjectDetails.WithoutFacility");
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyImmunizationOrderObjectDetails.WithFacility");
                                query1.SetString(2, FacName);
                            }
                        }
                        else
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyImmunizationOrderObjectDetails.WithoutFacility.ShowAllFalse");
                                //query1.SetInt32(2, DefaultNoofDays);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyImmunizationOrderObjectDetails.WithFacility.ShowAllFalse");
                                query1.SetString(2, FacName);
                                //query1.SetInt32(3, DefaultNoofDays);
                            }
                        }
                        query1.SetString(0, UserName);
                        if (ProcessType == "UNASSIGNED")
                        {
                            query1.SetString(1, "UNKNOWN");
                        }
                        else
                        {
                            query1.SetString(1, UserName);
                        }
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, myObjType);
                    }
                    if (ObjType.Contains("REFERRAL ORDER"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "REFERRAL ORDER";
                        if (bShowAll == true)
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyReferralOrderObjectDetails.WithoutFacility");
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyReferralOrderObjectDetails.WithFacility");
                                query1.SetString(2, FacName);
                            }
                        }
                        else
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyReferralOrderObjectDetails.WithoutFacility.ShowAllFalse");
                                //query1.SetInt32(2, DefaultNoofDays);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyReferralOrderObjectDetails.WithFacility.ShowAllFalse");
                                query1.SetString(2, FacName);
                                //query1.SetInt32(3, DefaultNoofDays);
                            }
                        }
                        query1.SetString(0, UserName);
                        if (ProcessType == "UNASSIGNED")
                        {
                            query1.SetString(1, "UNKNOWN");
                        }
                        else
                        {
                            query1.SetString(1, UserName);
                        }
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, myObjType);
                    }
                    if (ObjType.Contains("E-PRESCRIBE"))
                    {
                        if (bShowAll == true)
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyE-PrescriptionObjectDetails.WithoutFacility");

                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyE-PrescriptionObjectDetails.WithFacility");
                                query1.SetString(2, FacName);
                            }
                        }
                        else
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyE-PrescriptionObjectDetails.WithoutFacility.ShowAllFalse");
                                //query1.SetInt32(2, DefaultNoofDays);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyE-PrescriptionObjectDetails.WithFacility.ShowAllFalse");
                                query1.SetString(2, FacName);
                                //query1.SetInt32(3, DefaultNoofDays);
                            }
                        }
                        query1.SetString(0, UserName);
                        if (ProcessType == "UNASSIGNED")
                        {
                            query1.SetString(1, "UNKNOWN");
                        }
                        else
                        {
                            query1.SetString(1, UserName);
                        }
                        query1.SetParameterList("ObjList", ObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, ObjType);
                    }
                    if (ObjType.Contains("SCAN") || ObjType.Contains("SCAN RESULT"))
                    {
                        if (bShowAll)
                        {

                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyScanObjectDetails.WithoutFacility");
                                query1.SetString(0, UserName);
                                if (ProcessType == "UNASSIGNED")
                                    query1.SetString(1, "UNKNOWN");
                                else
                                    query1.SetString(1, UserName);
                                query1.SetString(2, ObjType[0].ToString());
                                query1.SetString(3, UserName);
                                if (ProcessType == "UNASSIGNED")
                                    query1.SetString(4, "UNKNOWN");
                                else
                                    query1.SetString(4, UserName);
                                query1.SetString(5, ObjType[1].ToString());
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyScanObjectDetails.WithFacility");
                                query1.SetString(0, UserName);
                                if (ProcessType == "UNASSIGNED")
                                    query1.SetString(1, "UNKNOWN");
                                else
                                    query1.SetString(1, UserName);

                                query1.SetString(2, FacName);
                                query1.SetString(3, ObjType[0].ToString());
                                query1.SetString(4, UserName);
                                if (ProcessType == "UNASSIGNED")
                                    query1.SetString(5, "UNKNOWN");
                                else
                                    query1.SetString(5, UserName);
                                query1.SetString(6, FacName);
                                query1.SetString(7, ObjType[1].ToString());

                            }
                        }
                        else
                        {
                            //<<<<<<< ObjectManager.cs




                            //                        if (FacName == "ALL")
                            //                        {
                            //                            query1 = Mysession.GetNamedQuery("FillMyScanObjectDetails.WithoutFacility.No_ofdays");
                            //                            query1.SetString(0, UserName);
                            //                            if (ProcessType == "UNASSIGNED")
                            //                                query1.SetString(1, "UNKNOWN");
                            //                            else
                            //                                query1.SetString(1, UserName);
                            //                            query1.SetString(2, ObjType[0].ToString());
                            //                            query1.SetInt32(3, DefaultNoofDays);
                            //                            query1.SetString(4, UserName);
                            //                            if (ProcessType == "UNASSIGNED")
                            //                                query1.SetString(5, "UNKNOWN");
                            //                            else
                            //                                query1.SetString(5, UserName);
                            //                            query1.SetString(6, ObjType[1].ToString());
                            //                            query1.SetInt32(7, DefaultNoofDays);
                            //                        }
                            //                        else
                            //                        {
                            //                            query1 = Mysession.GetNamedQuery("FillMyScanObjectDetails.WithFacility.No_ofdays");
                            //                            query1.SetString(0, UserName);
                            //                            if (ProcessType == "UNASSIGNED")
                            //                                query1.SetString(1, "UNKNOWN");
                            //                            else
                            //                                query1.SetString(1, UserName);

                            //                            query1.SetString(2, FacName);
                            //                            query1.SetString(3, ObjType[0].ToString());
                            //                            query1.SetInt32(4, DefaultNoofDays);
                            //                            query1.SetString(5, UserName);
                            //                            if (ProcessType == "UNASSIGNED")
                            //                                query1.SetString(6, "UNKNOWN");
                            //                            else
                            //                                query1.SetString(6, UserName);
                            //                            query1.SetString(7, FacName);
                            //                            query1.SetString(8, ObjType[1].ToString());
                            //                            query1.SetInt32(9, DefaultNoofDays);

                            //                        }



                            //                    }

                            //||||||| 1.48
                            //                        query1 = Mysession.GetNamedQuery("FillMyScanObjectDetails.WithFacility");
                            //                        query1.SetString(2, FacName);
                            //                    }
                            //                    query1.SetString(0, UserName);
                            //                    if (ProcessType == "UNASSIGNED")
                            //                    {
                            //                        query1.SetString(1, "UNKNOWN");
                            //                    }
                            //                    else
                            //                    {
                            //                        query1.SetString(1, UserName);
                            //                    }
                            //                    query1.SetParameterList("ObjList", ObjType);
                            //=======




                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyScanObjectDetails.WithoutFacility.No_ofdays");
                                query1.SetString(0, UserName);
                                if (ProcessType == "UNASSIGNED")
                                    query1.SetString(1, "UNKNOWN");
                                else
                                    query1.SetString(1, UserName);
                                query1.SetString(2, ObjType[0].ToString());
                                //query1.SetInt32(3, DefaultNoofDays);
                                query1.SetString(3, UserName);
                                if (ProcessType == "UNASSIGNED")
                                    query1.SetString(4, "UNKNOWN");
                                else
                                    query1.SetString(4, UserName);
                                query1.SetString(5, ObjType[1].ToString());
                                //query1.SetInt32(6, DefaultNoofDays);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyScanObjectDetails.WithFacility.No_ofdays");
                                query1.SetString(0, UserName);
                                if (ProcessType == "UNASSIGNED")
                                    query1.SetString(1, "UNKNOWN");
                                else
                                    query1.SetString(1, UserName);

                                query1.SetString(2, FacName);
                                query1.SetString(3, ObjType[0].ToString());
                                //query1.SetInt32(4, DefaultNoofDays);
                                query1.SetString(4, UserName);
                                if (ProcessType == "UNASSIGNED")
                                    query1.SetString(5, "UNKNOWN");
                                else
                                    query1.SetString(5, UserName);
                                query1.SetString(6, FacName);
                                query1.SetString(7, ObjType[1].ToString());
                                //query1.SetInt32(8, DefaultNoofDays);

                            }



                        }

                        //>>>>>>> 1.48.2.7.2.1
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, ObjType);
                    }

                    //var fill1 = from f in fillMyQList where (f.ObjTypeList == "ENCOUNTER" || f.ObjTypeList == "DOCUMENTATION" || f.ObjTypeList == "DOCUMENT REVIEW" || f.ObjTypeList == "CHECK OUT" || f.ObjTypeList == "PHONE ENCOUNTER") orderby f.ObjSysId select f.ObjSysId;//|| f.ObjTypeList=="E-PRESCRIBE"
                    //var fill2 = from f in fillMyQList where (f.ObjTypeList == "ENCOUNTER" || f.ObjTypeList == "DOCUMENTATION" || f.ObjTypeList == "DOCUMENT REVIEW" || f.ObjTypeList == "CHECK OUT" || f.ObjTypeList == "PHONE ENCOUNTER") orderby f.ObjSysId select f;
                    //templist = fill1.ToArray<ulong>();
                    //tempMyqlist = fill2.ToList<FillMyQ>();
                    //if (templist.Count() != 0)
                    //{
                    //    query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails");
                    //    query1.SetParameterList("List", templist);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, tempMyqlist);
                    //}

                    //fill1 = from f in fillMyQList where (f.ObjTypeList == "TASK") orderby f.ObjSysId select f.ObjSysId;
                    //fill2 = from f in fillMyQList where (f.ObjTypeList == "TASK") orderby f.ObjSysId select f;
                    //templist = fill1.ToArray<ulong>();
                    //tempMyqlist = fill2.ToList<FillMyQ>();
                    //if (templist.Count() != 0)
                    //{
                    //    query1 = Mysession.GetNamedQuery("FillMyTaskObjectDetails");
                    //    query1.SetParameterList("List", templist);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, tempMyqlist);
                    //}

                    //fill1 = from f in fillMyQList where ((f.ObjTypeList == "DIAGNOSTIC ORDER" || f.ObjTypeList == "IMAGE ORDER")) orderby f.ObjSysId select f.ObjSysId;
                    //fill2 = from f in fillMyQList where ((f.ObjTypeList == "DIAGNOSTIC ORDER" || f.ObjTypeList == "IMAGE ORDER")) orderby f.ObjSysId select f;
                    //templist = fill1.ToArray<ulong>();
                    //tempMyqlist = fill2.ToList<FillMyQ>();
                    //if (templist.Count() != 0)
                    //{
                    //    query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails");
                    //    query1.SetParameterList("List", templist);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, tempMyqlist);
                    //}

                    //fill1 = from f in fillMyQList where (f.ObjTypeList == "SCAN") orderby f.ObjSysId select f.ObjSysId;
                    //fill2 = from f in fillMyQList where (f.ObjTypeList == "SCAN") orderby f.ObjSysId select f;
                    //templist = fill1.ToArray<ulong>();
                    //tempMyqlist = fill2.ToList<FillMyQ>();
                    //if (templist.Count() != 0)
                    //{
                    //    query1 = Mysession.GetNamedQuery("FillMyScanObjectDetails");
                    //    query1.SetParameterList("List", templist);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, tempMyqlist);
                    //}

                    # region Commented by Srividhya on 30-May-2015
                    ////added by srividhya                
                    //if (ObjType.Contains("WORKSET"))
                    //{
                    //    string[] myObjType = new string[1];
                    //    myObjType[0] = "WORKSET";
                    //    if (FacName == "ALL")
                    //    {
                    //        query1 = Mysession.GetNamedQuery("FillMyWorksetObjectDetails.WithoutFacility");
                    //    }
                    //    else
                    //    {
                    //        query1 = Mysession.GetNamedQuery("FillMyWorksetObjectDetails.WithFacility");
                    //        query1.SetString(2, FacName);
                    //    }
                    //    query1.SetString(0, UserName);
                    //    if (ProcessType == "UNASSIGNED")
                    //    {
                    //        query1.SetString(1, "UNKNOWN");
                    //    }
                    //    else
                    //    {
                    //        query1.SetString(1, UserName);
                    //    }
                    //    query1.SetParameterList("ObjList", myObjType);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, myObjType);
                    //}
                    //if (ObjType.Contains("EXCEPTION"))
                    //{
                    //    string[] myObjType = new string[1];
                    //    myObjType[0] = "EXCEPTION";
                    //    if (FacName == "ALL")
                    //    {
                    //        query1 = Mysession.GetNamedQuery("FillMyExceptionObjectDetails.WithoutFacility");
                    //    }
                    //    else
                    //    {
                    //        query1 = Mysession.GetNamedQuery("FillMyExceptionObjectDetails.WithFacility");
                    //        query1.SetString(2, FacName);
                    //    }
                    //    query1.SetString(0, UserName);
                    //    if (ProcessType == "UNASSIGNED")
                    //    {
                    //        query1.SetString(1, "UNKNOWN");
                    //    }
                    //    else
                    //    {
                    //        query1.SetString(1, UserName);
                    //    }
                    //    query1.SetParameterList("ObjList", myObjType);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, myObjType);
                    //}
                    //if (ObjType.Contains("CALL"))
                    //{
                    //    string[] myObjType = new string[1];
                    //    myObjType[0] = "CALL";
                    //    if (FacName == "ALL")
                    //    {
                    //        query1 = Mysession.GetNamedQuery("FillMyCallObjectDetails.WithoutFacility");
                    //    }
                    //    else
                    //    {
                    //        query1 = Mysession.GetNamedQuery("FillMyCallObjectDetails.WithFacility");
                    //        query1.SetString(2, FacName);
                    //    }
                    //    query1.SetString(0, UserName);
                    //    if (ProcessType == "UNASSIGNED")
                    //    {
                    //        query1.SetString(1, "UNKNOWN");
                    //    }
                    //    else
                    //    {
                    //        query1.SetString(1, UserName);
                    //    }
                    //    query1.SetParameterList("ObjList", myObjType);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, myObjType);
                    //}
                    //if (ObjType.Contains("QC_ERROR"))
                    //{
                    //    string[] myObjType = new string[1];
                    //    myObjType[0] = "QC_ERROR";
                    //    if (FacName == "ALL")
                    //    {
                    //        query1 = Mysession.GetNamedQuery("FillMyQCERRORObjectDetails.WithoutFacility");
                    //    }
                    //    else
                    //    {
                    //        query1 = Mysession.GetNamedQuery("FillMyQCERRORObjectDetails.WithFacility");
                    //        query1.SetString(2, FacName);
                    //    }
                    //    query1.SetString(0, UserName);
                    //    if (ProcessType == "UNASSIGNED")
                    //    {
                    //        query1.SetString(1, "UNKNOWN");
                    //    }
                    //    else
                    //    {
                    //        query1.SetString(1, UserName);
                    //    }
                    //    query1.SetParameterList("ObjList", myObjType);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, myObjType);
                    //}
                    #endregion

                    if (ObjType.Contains("DIAGNOSTIC_RESULT"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "DIAGNOSTIC_RESULT";
                        query1 = Mysession.GetNamedQuery("FillMyResultsObjectDetails");
                        query1.SetString(0, "DIAGNOSTIC_RESULT");
                        //query1.SetString(1, "RESULT_REVIEW");
                        query1.SetString(1, UserName);
                        //query1.SetInt32(2, DefaultNoofDays);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, myObjType);

                    }

                    //fill1 = from f in fillMyQList where (f.ObjTypeList == "WORKSET") orderby f.ObjSysId select f.ObjSysId;
                    //fill2 = from f in fillMyQList where (f.ObjTypeList == "WORKSET") orderby f.ObjSysId select f;
                    //templist = fill1.ToArray<ulong>();
                    //tempMyqlist = fill2.ToList<FillMyQ>();
                    //if (templist.Count() != 0)
                    //{
                    //    query1 = Mysession.GetNamedQuery("FillMyBillingDetails.Workset");
                    //    query1.SetParameterList("List", templist);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, tempMyqlist);
                    //}

                    //fill1 = from f in fillMyQList where (f.ObjTypeList == "CALL") orderby f.ObjSysId select f.ObjSysId;
                    //fill2 = from f in fillMyQList where (f.ObjTypeList == "CALL") orderby f.ObjSysId select f;
                    //templist = fill1.ToArray<ulong>();
                    //tempMyqlist = fill2.ToList<FillMyQ>();
                    //if (templist.Count() != 0)
                    //{
                    //    query1 = Mysession.GetNamedQuery("FillMyBillingDetails.Call");
                    //    query1.SetParameterList("List", templist);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, tempMyqlist);
                    //}

                    //fill1 = from f in fillMyQList where (f.ObjTypeList == "QC_ERROR") orderby f.ObjSysId select f.ObjSysId;
                    //fill2 = from f in fillMyQList where (f.ObjTypeList == "QC_ERROR") orderby f.ObjSysId select f;
                    //templist = fill1.ToArray<ulong>();
                    //tempMyqlist = fill2.ToList<FillMyQ>();
                    //if (templist.Count() != 0)
                    //{
                    //    query1 = Mysession.GetNamedQuery("FillMyBillingDetails.Qc.Error");
                    //    query1.SetParameterList("List", templist);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, tempMyqlist);
                    //}

                    //fill1 = from f in fillMyQList where (f.ObjTypeList == "EXCEPTION") orderby f.ObjSysId select f.ObjSysId;
                    //fill2 = from f in fillMyQList where (f.ObjTypeList == "EXCEPTION") orderby f.ObjSysId select f;
                    //templist = fill1.ToArray<ulong>();
                    //tempMyqlist = fill2.ToList<FillMyQ>();
                    //if (templist.Count() != 0)
                    //{
                    //    query1 = Mysession.GetNamedQuery("FillMyBillingDetails.Exception");
                    //    query1.SetParameterList("List", templist);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, tempMyqlist);
                    //}

                    //fill1 = from f in fillMyQList where (f.ObjTypeList == "RESCAN") orderby f.ObjSysId select f.ObjSysId;
                    //fill2 = from f in fillMyQList where (f.ObjTypeList == "RESCAN") orderby f.ObjSysId select f;
                    //templist = fill1.ToArray<ulong>();
                    //tempMyqlist = fill2.ToList<FillMyQ>();
                    //if (templist.Count() != 0)
                    //{
                    //    query1 = Mysession.GetNamedQuery("FillMyReScanObjectDetails");
                    //    query1.SetParameterList("List", templist);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, tempMyqlist);
                    //}

                    //fill1 = from f in fillMyQList where (f.ObjTypeList == "INTERNAL ORDER") orderby f.ObjSysId select f.ObjSysId;
                    //fill2 = from f in fillMyQList where (f.ObjTypeList == "INTERNAL ORDER") orderby f.ObjSysId select f;
                    //templist = fill1.ToArray<ulong>();
                    //tempMyqlist = fill2.ToList<FillMyQ>();
                    //if (templist.Count() != 0)
                    //{
                    //    query1 = Mysession.GetNamedQuery("FillMyInternalOrderObjectDetails");
                    //    query1.SetParameterList("List", templist);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, tempMyqlist);
                    //}

                    //fill1 = from f in fillMyQList where (f.ObjTypeList == "IMMUNIZATION ORDER") orderby f.ObjSysId select f.ObjSysId;
                    //fill2 = from f in fillMyQList where (f.ObjTypeList == "IMMUNIZATION ORDER") orderby f.ObjSysId select f;
                    //templist = fill1.ToArray<ulong>();
                    //tempMyqlist = fill2.ToList<FillMyQ>();
                    //if (templist.Count() != 0)
                    //{
                    //    query1 = Mysession.GetNamedQuery("FillMyImmunizationOrderObjectDetails");
                    //    query1.SetParameterList("List", templist);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, tempMyqlist);
                    //}

                    //fill1 = from f in fillMyQList where (f.ObjTypeList == "INTERNAL DIAGNOSTIC ORDER" || f.ObjTypeList == "INTERNAL IMAGE ORDER") orderby f.ObjSysId select f.ObjSysId;
                    //fill2 = from f in fillMyQList where (f.ObjTypeList == "INTERNAL DIAGNOSTIC ORDER" || f.ObjTypeList == "INTERNAL IMAGE ORDER") orderby f.ObjSysId select f;
                    //templist = fill1.ToArray<ulong>();
                    //tempMyqlist = fill2.ToList<FillMyQ>();
                    //if (templist.Count() != 0)
                    //{
                    //    query1 = Mysession.GetNamedQuery("FillMyInternalLabImageOrderObjectDetails");
                    //    query1.SetParameterList("List", templist);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, tempMyqlist);
                    //}
                    //fill1 = from f in fillMyQList where (f.ObjTypeList == "E-PRESCRIBE" || f.ObjTypeList == "E-PRESCRIBE") orderby f.ObjSysId select f.ObjSysId;
                    //fill2 = from f in fillMyQList where (f.ObjTypeList == "E-PRESCRIBE" || f.ObjTypeList == "E-PRESCRIBE") orderby f.ObjSysId select f;
                    //templist = fill1.ToArray<ulong>();
                    //tempMyqlist = fill2.ToList<FillMyQ>();
                    //if (templist.Count() != 0)
                    //{
                    //    query1 = Mysession.GetNamedQuery("FillMyE-PrescriptionObjectDetails");
                    //    query1.SetParameterList("List", templist);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, tempMyqlist);
                    //}
                }
                catch (Exception ex)
                {
                    String sException = string.Empty;
                    if (ex.StackTrace != null)
                        sException = ex.StackTrace.ToString();
                    if (ex.Message != null)
                        sException += Environment.NewLine + ex.Message.ToString();
                    if (ex.InnerException != null && ex.InnerException.Message != null)
                        sException += Environment.NewLine + ex.InnerException.Message.ToString();
                    System.IO.StreamWriter sw = null;
                    try
                    {
                        string path = "C:/Exception_Logs/Exception_Log_File.txt";
                        sw = System.IO.File.AppendText(path);
                        string logLine = System.String.Format(
                            "{0:G}: {1}.", System.DateTime.Now, sException);
                        sw.WriteLine(logLine);
                    }
                    finally
                    {
                        sw.Close();
                    }
                    throw new Exception(sException);
                }
                finally
                {
                    Mysession.Close();
                }
            }
            return resultmyqList;
        }

        private void FillDTO(ArrayList FavoriteList, string[] ObjType)
        {
            for (int i = 0; i < FavoriteList.Count; i++)
            {

                object[] oj = (object[])FavoriteList[i];

                MyQ myq = new MyQ();
                if (ObjType.Contains("ENCOUNTER") || ObjType.Contains("DOCUMENTATION") || ObjType.Contains("DOCUMENT REVIEW") || ObjType.Contains("CHECK OUT") || ObjType.Contains("PHONE ENCOUNTER"))
                {
                    //if (ObjType.Contains(oj[15].ToString()) == false)
                    //{
                    //    continue;
                    //}
                    if (oj[12] == null)
                    {
                        continue;
                    }
                    if (oj[6] != null)
                    {
                        myq.AssignedMedicalAsst = oj[6].ToString();
                    }
                    //if (oj[6] != null)
                    //{
                    //    myq.AssignedPhysician = oj[6].ToString();
                    //}
                    //if (oj[13] != null)
                    //{
                    //    myq.Current_Owner = oj[13].ToString();
                    //}
                    if (oj[10] != null)
                    {
                        myq.Current_Process = oj[10].ToString();
                    }
                    if (oj[0] != null)
                    {
                        myq.Date_of_Service = Convert.ToDateTime(oj[0].ToString());
                    }
                    //if (oj[16] != null)
                    //{
                    //    myq.EHR_Obj_Sub_Type = oj[16].ToString();
                    //}
                    if (oj[14] != null)
                    {
                        myq.EHR_Obj_Type = oj[14].ToString();
                    }
                    if (oj[2] != null)
                    {
                        myq.First_Name = oj[2].ToString();
                    }
                    if (oj[1] != null)
                    {
                        myq.Last_Name = oj[1].ToString();
                    }
                    //if (oj[1] != null)
                    //{
                    //    myq.Medical_Record_Number = oj[1].ToString();
                    //}
                    if (oj[3] != null)
                    {
                        myq.MI = oj[3].ToString();
                    }
                    if (oj[12] != null)
                    {
                        myq.Human_ID = Convert.ToUInt64(oj[12].ToString());
                    }
                    if (oj[5] != null)
                    {
                        myq.Encounter_ID = Convert.ToUInt64(oj[5].ToString());
                    }
                    if (oj[4] != null)
                    {
                        myq.PhyName = oj[4].ToString();
                    }
                    if (oj[13] != null)
                    {
                        myq.Physician_ID = Convert.ToUInt64(oj[13].ToString());
                    }
                    //if (oj[9] != null)
                    //{
                    //    myq.Type_Of_Visit = oj[9].ToString();
                    //}
                    if (oj[7] != null)
                    {
                        myq.DOB = Convert.ToDateTime(oj[7].ToString());
                    }
                    if (oj[8] != null)
                    {
                        myq.Appt_Date_Time = Convert.ToDateTime(oj[8].ToString());
                    }
                    if (oj[9] != null)
                    {
                        myq.External_Account_Number = oj[9].ToString();
                    }
                    //if (oj[14] != null)
                    //{
                    //    myq.Exam_Room = oj[14].ToString();
                    //}
                    //if (oj[13] != null)
                    //{
                    //    myq.Facility_Name = oj[13].ToString();
                    //}
                    //if (oj[17] != null)
                    //{
                    //    myq.Patient_Status = oj[17].ToString();
                    //}
                    if (oj[11] != null)
                    {
                        myq.Is_EandM_Submitted = oj[11].ToString();
                    }
                    //if (oj[19] != null)
                    //{
                    //    myq.Order_Submit_ID = Convert.ToUInt32(oj[19]);
                    //}
                    if (oj.Count() == 20 && (oj[18].ToString() == "GeneralQ" || oj[18].ToString() == "MyQueue"))
                    {
                        if (oj[15] != null && oj[15].ToString() != "")
                        {
                            myq.Type_Of_Visit = oj[15].ToString();
                        }
                        if (oj[16] != null && oj[16].ToString() != "")
                        {
                            myq.Facility_Name = oj[16].ToString();
                        }
                        if (oj[17] != null && oj[17].ToString() != "")
                        {
                            myq.Insurance_Plan_Name = oj[17].ToString();
                        }
                        if (oj[19] != null && oj[19].ToString() != "")
                        {
                            myq.Carrier_Name = oj[19].ToString();
                        }
                    }
                    else if (oj.Count() > 17)
                    {
                        if (oj[15] != null && oj[15].ToString() != "")
                        {
                            myq.Ordering_Physician = oj[15].ToString();
                        }
                        else if (oj[16] != null && oj[16].ToString() != "")
                        {
                            myq.Ordering_Physician = oj[16].ToString();
                        }

                        if (oj[17] != null && oj[17].ToString() != "")
                        {
                            myq.Test_Details = oj[17].ToString();
                        }
                    }

                }

                else if (ObjType.Contains("TASK"))
                {
                    if (ObjType.Contains(oj[12].ToString()) == false)
                    {
                        continue;
                    }
                    myq.Message_ID = Convert.ToUInt64(oj[0]);
                    myq.Human_ID = Convert.ToUInt64(oj[1]);
                    myq.Message_Description = Convert.ToString(oj[2]);
                    myq.Assigned_To = Convert.ToString(oj[3]);
                    myq.Modified_By = Convert.ToString(oj[4]);
                    myq.Modified_Date_Time = Convert.ToDateTime(oj[5]);
                    //myq.Facility_Name = Convert.ToString(oj[6]);
                    myq.Msg_Date_And_Time = Convert.ToDateTime(oj[6]);
                    myq.Version = Convert.ToInt32(oj[7]);
                    myq.Last_Name = Convert.ToString(oj[8]);
                    myq.First_Name = Convert.ToString(oj[9]);
                    myq.MI = Convert.ToString(oj[10]);
                    //myq.DOB = Convert.ToDateTime(oj[11]);
                    myq.External_Account_Number = Convert.ToString(oj[11]);
                    myq.EHR_Obj_Type = Convert.ToString(oj[12]);
                    if (oj[13] != null && oj[13] != string.Empty)
                        myq.Priority = Convert.ToString(oj[13]);
                    if (oj[14] != null && oj[14] != string.Empty)
                        myq.Message_Notes = Convert.ToString(oj[14]);
                    if (oj[15] != null && oj[15] != string.Empty)
                        myq.Created_By = Convert.ToString(oj[15]);
                    if (oj[16] != null && oj[16] != string.Empty)
                        myq.Facility_Name = Convert.ToString(oj[16]);
                }
                //else if (ObjType.Contains("DIAGNOSTIC ORDER") || ObjType.Contains("IMAGE ORDER") || ObjType.Contains("INTERNAL DIAGNOSTIC ORDER") || ObjType.Contains("INTERNAL IMAGE ORDER"))
                else if (ObjType.Contains("DIAGNOSTIC ORDER") || ObjType.Contains("IMAGE ORDER"))
                {
                    if (ObjType.Contains(oj[18].ToString()) == false)
                    {
                        continue;
                    }
                    if (Convert.ToString(oj[0]).EndsWith("-") == true)
                    {
                        myq.Procedure_Ordered = Convert.ToString(oj[0]).Remove(Convert.ToString(oj[0]).Length - 1);
                    }
                    else
                    {
                        myq.Procedure_Ordered = Convert.ToString(oj[0]);
                    }

                    if (oj[2] != null)
                    {
                        myq.Medical_Record_Number = oj[1].ToString();
                    }
                    myq.Human_ID = Convert.ToUInt64(oj[2]);
                    myq.Last_Name = Convert.ToString(oj[3]);
                    myq.First_Name = Convert.ToString(oj[4]);
                    myq.MI = Convert.ToString(oj[5]);
                    myq.PhyName = Convert.ToString(oj[6]);
                    myq.Lab_Name = Convert.ToString(oj[7]);
                    myq.Lab_Loc_Name = Convert.ToString(oj[8]);
                    myq.External_Account_Number = Convert.ToString(oj[9]);
                    myq.DOB = Convert.ToDateTime(oj[10]);
                    myq.Encounter_ID = Convert.ToUInt64(oj[11]);
                    myq.Physician_ID = Convert.ToUInt64(oj[12]);
                    myq.Lab_ID = Convert.ToUInt64(oj[13]);
                    myq.Lab_Location_ID = Convert.ToUInt64(oj[14]);
                    //myq.Facility_Name = Convert.ToString(oj[15]);
                    myq.Order_ID = Convert.ToUInt64(oj[15]);
                    myq.Current_Owner = Convert.ToString(oj[16]);
                    myq.Current_Process = Convert.ToString(oj[17]);
                    myq.EHR_Obj_Type = Convert.ToString(oj[18]);
                    myq.EHR_Obj_Sub_Type = Convert.ToString(oj[19]);
                    myq.Order_Submit_ID = Convert.ToUInt64(oj[20]);
                    myq.Created_Date_And_Time = Convert.ToDateTime(oj[21]);
                    myq.CMG_Encounter_ID = Convert.ToUInt64(oj[22]);
                    //myq.Is_Narrative = Convert.ToString(oj[23]);
                    myq.File_Reference_No = Convert.ToString(oj[23]);//BugID:43099
                    myq.Is_Abnormal = Convert.ToString(oj[24]);
                    if (oj[25] != null && oj[25] != "")
                        myq.Test_Date = Convert.ToDateTime(oj[25]);
                    if (oj[26] != null && oj[26] != "")
                        myq.Is_Narrative = Convert.ToString(oj[26]);


                }
                else if (ObjType.Contains("SCAN"))
                {
                    if (ObjType.Contains(oj[7].ToString()) == false)
                    {
                        continue;
                    }

                    if (oj[0] != null)
                    {
                        myq.Scanned_File_Name = oj[0].ToString();
                        myq.No_of_Pages = Convert.ToInt32(oj[1].ToString());
                        myq.Scanned_Date = Convert.ToDateTime(oj[2].ToString());
                        myq.Facility_Name = oj[3].ToString();
                        //myq.Scan_Type = oj[4].ToString();
                        myq.Scan_ID = Convert.ToUInt64(oj[4]);
                        myq.Current_Owner = oj[5].ToString();
                        myq.Current_Process = oj[6].ToString();
                        myq.EHR_Obj_Type = oj[7].ToString();
                        myq.Obj_System_Id = Convert.ToUInt64(oj[4]);
                        myq.Human_ID = Convert.ToUInt64(oj[8]);
                    }

                }
                //else if (ObjType.Contains("INTERNAL ORDER") || ObjType.Contains("IMMUNIZATION ORDER"))
                else if (ObjType.Contains("IMMUNIZATION ORDER"))
                {
                    if (oj[2] != null)//Humanid
                    {

                        //myq.Order_ID = Convert.ToUInt64(tempMyQ[i].ObjSysId);
                        //myq.Current_Owner = tempMyQ[i].CurrOwner;
                        //myq.Current_Process = tempMyQ[i].CurrProcessList;
                        //myq.EHR_Obj_Sub_Type = tempMyQ[i].CurrObjSubType;
                        //myq.EHR_Obj_Type = tempMyQ[i].ObjTypeList;
                        myq.Procedure_Ordered = oj[0].ToString();
                        //Added by Janani on 21-Jul-2011
                        if (oj[2] != null)
                        {
                            myq.Medical_Record_Number = oj[1].ToString();
                        }
                        myq.Human_ID = Convert.ToUInt64(oj[2]);
                        myq.Last_Name = Convert.ToString(oj[3]);
                        myq.First_Name = Convert.ToString(oj[4]);
                        myq.MI = Convert.ToString(oj[5]);
                        myq.PhyName = Convert.ToString(oj[6]);
                        myq.External_Account_Number = oj[7].ToString();
                        myq.DOB = Convert.ToDateTime(oj[8]);
                        myq.Encounter_ID = Convert.ToUInt64(oj[9]);
                        myq.Physician_ID = Convert.ToUInt64(oj[10]);
                        myq.Facility_Name = Convert.ToString(oj[11]);
                        myq.Date_of_Service = Convert.ToDateTime(oj[12]);
                        myq.Order_ID = Convert.ToUInt64(oj[13]);
                        myq.Current_Owner = Convert.ToString(oj[14]);
                        myq.Current_Process = Convert.ToString(oj[15]);
                        myq.EHR_Obj_Type = Convert.ToString(oj[16]);
                        myq.EHR_Obj_Sub_Type = Convert.ToString(oj[17]);
                        myq.Created_Date_And_Time = Convert.ToDateTime(oj[18]);
                    }
                }
                else if (ObjType.Contains("REFERRAL ORDER"))
                {
                    if (ObjType.Contains(oj[15].ToString()) == false)
                    {
                        continue;
                    }
                    //myq.Order_ID = Convert.ToUInt64(tempMyQ[i].ObjSysId);
                    //myq.Current_Owner = tempMyQ[i].CurrOwner;
                    //myq.Current_Process = tempMyQ[i].CurrProcessList;
                    //myq.EHR_Obj_Sub_Type = tempMyQ[i].CurrObjSubType;
                    //myq.EHR_Obj_Type = tempMyQ[i].ObjTypeList;
                    if (oj[0] != null)
                    {
                        myq.Procedure_Ordered = Convert.ToString(oj[0]);
                        //Added by Janani on 21-Jul-2011
                        if (oj[2] != null)
                        {
                            myq.Medical_Record_Number = Convert.ToString(oj[0]);
                        }
                        myq.Human_ID = Convert.ToUInt64(oj[1]);
                        myq.Last_Name = Convert.ToString(oj[2]);
                        myq.First_Name = Convert.ToString(oj[3]);
                        myq.MI = Convert.ToString(oj[4]);
                        myq.PhyName = Convert.ToString(oj[5]);
                        myq.External_Account_Number = oj[6].ToString();
                        myq.DOB = Convert.ToDateTime(oj[7]);
                        myq.Encounter_ID = Convert.ToUInt64(oj[8]);
                        myq.Physician_ID = Convert.ToUInt64(oj[9]);
                        myq.Facility_Name = Convert.ToString(oj[10]);
                        myq.Date_of_Service = Convert.ToDateTime(oj[11]);
                        myq.Order_ID = Convert.ToUInt64(oj[12]);
                        myq.Current_Owner = Convert.ToString(oj[13]);
                        myq.Current_Process = Convert.ToString(oj[14]);
                        myq.EHR_Obj_Type = Convert.ToString(oj[15]);
                        myq.EHR_Obj_Sub_Type = Convert.ToString(oj[16]);
                        myq.Reason_For_Referral = Convert.ToString(oj[17]);
                        myq.Referred_to = Convert.ToString(oj[18]);
                        myq.Referred_to_Facility = Convert.ToString(oj[19]);
                        myq.Created_Date_And_Time = Convert.ToDateTime(oj[20]);
                    }
                }
                else if (ObjType.Contains("E-PRESCRIBE"))
                {
                    if (oj[0] != null)
                    {
                        if (ObjType.Contains(oj[8].ToString()) == false)
                        {
                            continue;
                        }
                        myq.Prescription_Date = Convert.ToDateTime(oj[0].ToString());
                        //myq.Medical_Record_Number = Convert.ToString(oj[1]);
                        myq.Human_ID = Convert.ToUInt64(oj[1]);
                        myq.External_Account_Number = Convert.ToString(oj[2]);
                        myq.Last_Name = Convert.ToString(oj[3]);
                        myq.First_Name = Convert.ToString(oj[4]);
                        myq.MI = Convert.ToString(oj[5]);
                        myq.DOB = Convert.ToDateTime(oj[6]);
                        myq.Current_Process = Convert.ToString(oj[10]);
                        myq.Prescription_Id = Convert.ToInt32(oj[7]);
                        myq.EHR_Obj_Type = Convert.ToString(oj[8]);
                        myq.Current_Owner = Convert.ToString(oj[9]);
                    }
                }
                else if (ObjType.Contains("ADDENDUM"))
                {
                    if (oj[4] != null)
                        myq.Appt_Date_Time = Convert.ToDateTime(oj[4].ToString());
                    if (oj[7] != null)
                    {
                        myq.Addendum_Signed_Date_Time = Convert.ToDateTime(oj[7].ToString());
                    }
                    else
                    {
                        myq.Addendum_Signed_Date_Time = DateTime.MinValue;
                    }
                    //myq.Medical_Record_Number = Convert.ToString(oj[0]);
                    myq.Human_ID = Convert.ToUInt64(oj[0]);
                    myq.External_Account_Number = Convert.ToString(oj[5]);
                    myq.Last_Name = Convert.ToString(oj[1]);
                    myq.First_Name = Convert.ToString(oj[2]);
                    myq.MI = Convert.ToString(oj[3]);
                    //myq.DOB = Convert.ToDateTime(oj[4]);
                    //myq.Facility_Name = Convert.ToString(oj[6]);
                    myq.Current_Process = Convert.ToString(oj[6]);
                    if (oj[9] != null)
                    {
                        myq.Addendum_Created_Date_Time = Convert.ToDateTime(oj[9].ToString());
                    }
                    else
                    {
                        myq.Addendum_Created_Date_Time = DateTime.MinValue;
                    }
                    if (oj[8] != null)
                    {
                        myq.Addendum_Created_By = Convert.ToString(oj[8]);
                    }
                    else
                    {
                        myq.Addendum_Created_By = " ";
                    }
                    myq.Addendum_Signed_By = Convert.ToString(oj[10]);
                    myq.Encounter_ID = Convert.ToUInt64(oj[11]);
                    myq.Physician_ID = Convert.ToUInt64(oj[12]);
                    myq.EHR_Obj_Type = Convert.ToString(oj[13]);
                    myq.Addendum_ID = Convert.ToUInt64(oj[14]);
                    myq.Current_Owner = Convert.ToString(oj[15]);
                }
                else if (ObjType.Contains("DICTATION_RESULT"))
                {
                    myq.Exception_ID = Convert.ToUInt64(oj[0]);
                    myq.Exception_Encounter_ID = Convert.ToUInt64(oj[1]);
                    myq.Dictation_File_Path = Convert.ToString(oj[2]);
                    myq.Obj_System_Id = Convert.ToUInt64(oj[3]);
                    myq.Current_Process = Convert.ToString(oj[4]);
                    myq.Facility_Name = Convert.ToString(oj[5]);
                    myq.Object_Type = Convert.ToString(oj[6]);
                    myq.Current_Owner = Convert.ToString(oj[7]);
                    myq.Reason = Convert.ToString(oj[8]);
                    myq.Header = Convert.ToString(oj[9]);
                    myq.SubHeader = Convert.ToString(oj[10]);


                }
                //Added By Srividhya For ACO on 2-Sep-2014
                else if (ObjType.Contains("CARE_COORDINATE"))
                {
                    myq.Message_ID = Convert.ToUInt64(oj[0]);
                    myq.Human_ID = Convert.ToUInt64(oj[1]);
                    myq.Message_Description = Convert.ToString(oj[2]);
                    myq.Current_Process = Convert.ToString(oj[3]);
                    myq.Current_Owner = Convert.ToString(oj[4]);
                    myq.Modified_By = Convert.ToString(oj[5]);
                    myq.Modified_Date_Time = Convert.ToDateTime(oj[6]);
                    myq.Facility_Name = Convert.ToString(oj[7]);
                    myq.Msg_Date_And_Time = Convert.ToDateTime(oj[8]);
                    myq.Version = Convert.ToInt32(oj[9]);
                    myq.Last_Name = Convert.ToString(oj[10]);
                    myq.First_Name = Convert.ToString(oj[11]);
                    myq.MI = Convert.ToString(oj[12]);
                    myq.DOB = Convert.ToDateTime(oj[13]);
                    myq.Medical_Record_Number = Convert.ToString(oj[14]);
                    myq.EHR_Obj_Type = Convert.ToString(oj[15]);
                    myq.Message_Notes = Convert.ToString(oj[16]);
                    myq.Priority = Convert.ToString(oj[17]);
                    myq.LACE_Score = Convert.ToString(oj[18]);
                }
                //Added By Srividhya For ACO on 2-Sep-2014

                //Muthusamy for fill the results object MyOrders Queue - 17-12-2014
                else if (ObjType.Contains("DIAGNOSTIC_RESULT"))
                {
                    if (oj[0] != null)
                    {
                        myq.Created_Date_And_Time = Convert.ToDateTime(oj[0]);

                        myq.Human_ID = Convert.ToUInt64(oj[1]);
                        myq.External_Account_Number = Convert.ToString(oj[2]);
                        myq.Last_Name = Convert.ToString(oj[3]);
                        myq.First_Name = Convert.ToString(oj[4]);
                        myq.MI = Convert.ToString(oj[5]);
                        myq.DOB = Convert.ToDateTime(oj[6]);
                        myq.Current_Process = Convert.ToString(oj[7]);
                        myq.Lab_ID = Convert.ToUInt64(oj[8]);
                        if (oj[9] != null)
                            myq.PhyName = Convert.ToString(oj[9]);
                        myq.Lab_Name = Convert.ToString(oj[10]);
                        myq.ResultMasterID = Convert.ToUInt64(oj[11]);
                        myq.EHR_Obj_Type = Convert.ToString(oj[12]);
                        if (oj[13] != null)
                            myq.Physician_ID = Convert.ToUInt64(oj[13]);
                        myq.Is_Abnormal = Convert.ToString(oj[14]);
                    }

                }
                //$Muthusamy

                resultmyqList.Add(myq);
            }
        }

        public IList<MyQueueCountDTO> FillCountObjects(string FacName, string[] ObjType, string UserName, int DefaultNoofDays)
        {
            //string sAncillary = string.Empty;
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            ArrayList FavoriteList = null;
            IQuery query1 = null;
            MyQueueCountDTO myq = new MyQueueCountDTO();
            IList<MyQueueCountDTO> countlist = new List<MyQueueCountDTO>();


            using (ISession Mysession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {
                    if (ObjType.Contains("TASK"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "TASK";
                        query1 = Mysession.GetNamedQuery("Count.Tasks.Queues.ShowAllFalse");
                        query1.SetString(0, UserName);
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        IList<Queue_List> lstQueue = new List<Queue_List>();
                        foreach (object obj in FavoriteList)
                        {
                            Queue_List objQueue = new Queue_List();
                            object[] item = (object[])obj;
                            objQueue.Fac_Name = Convert.ToString(item[7]);
                            objQueue.Current_Owner = Convert.ToString(item[3]);
                            objQueue.Obj_Type = Convert.ToString(item[15]);
                            lstQueue.Add(objQueue);
                        }
                        if (lstQueue.Count > 0)
                        {
                            var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == "UNKNOWN" && ob.Fac_Name.ToUpper() == FacName.ToUpper() select ob).ToList<Queue_List>();
                            myq.Task_Count = resultSet.Count;
                            resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == UserName.ToUpper() select ob).ToList<Queue_List>();
                            myq.My_Task_Count = resultSet.Count;
                        }

                    }
                    else if (ObjType.Contains("ADDENDUM"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "ADDENDUM";
                        query1 = Mysession.GetNamedQuery("Count.Addendum.Queues.WithoutAllAppointments");
                        query1.SetString(0, UserName);
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        IList<Queue_List> lstQueue = new List<Queue_List>();
                        foreach (object obj in FavoriteList)
                        {
                            Queue_List objQueue = new Queue_List();
                            object[] item = (object[])obj;
                            objQueue.Fac_Name = Convert.ToString(item[8]);
                            objQueue.Current_Owner = Convert.ToString(item[18]);
                            objQueue.Obj_Type = Convert.ToString(item[16]);
                            lstQueue.Add(objQueue);
                        }
                        if (lstQueue.Count > 0)
                        {
                            var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == "UNKNOWN" && ob.Fac_Name.ToUpper() == FacName.ToUpper() select ob).ToList<Queue_List>();
                            myq.Amendmnt_Count = resultSet.Count;
                            resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == UserName.ToUpper() select ob).ToList<Queue_List>();
                            myq.My_Amendmnt_Count = resultSet.Count;
                        }


                    }
                    else if (ObjType.Contains("DICTATION_RESULT"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "DICTATION_RESULT";
                        query1 = Mysession.GetNamedQuery("Count.Dictation.Queues.WithoutAllAppointments");
                        query1.SetString("UserName", UserName);
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        IList<Queue_List> lstQueue = new List<Queue_List>();
                        foreach (object obj in FavoriteList)
                        {
                            Queue_List objQueue = new Queue_List();
                            object[] item = (object[])obj;
                            objQueue.Fac_Name = Convert.ToString(item[5]);
                            objQueue.Current_Owner = Convert.ToString(item[6]);
                            objQueue.Obj_Type = Convert.ToString(item[7]);
                            lstQueue.Add(objQueue);
                        }
                        if (lstQueue.Count > 0)
                        {
                            var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == "UNKNOWN" && ob.Fac_Name.ToUpper() == FacName.ToUpper() select ob).ToList<Queue_List>();
                            myq.Dict_Count = resultSet.Count;
                            resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == UserName.ToUpper() select ob).ToList<Queue_List>();
                            myq.My_Dict_Count = resultSet.Count;
                        }

                    }


                    if (ObjType.Contains("DIAGNOSTIC ORDER")) //|| ObjType.Contains("IMAGE ORDER"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "DIAGNOSTIC ORDER";
                        //myObjType[1] = "IMAGE ORDER";
                        StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
                        query1 = Mysession.GetNamedQuery("Count.DiagnosticOrder.Queues.ShowAllFalse");
                        query1.SetString(0, UserName);
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        IList<Queue_List> lstQueue = new List<Queue_List>();
                        foreach (object obj in FavoriteList)
                        {
                            Queue_List objQueue = new Queue_List();
                            object[] item = (object[])obj;
                            objQueue.Fac_Name = Convert.ToString(item[15]);
                            objQueue.Current_Owner = Convert.ToString(item[17]);
                            objQueue.Obj_Type = Convert.ToString(item[19]);
                            lstQueue.Add(objQueue);
                        }
                        if (lstQueue.Count > 0)
                        {
                            var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == UserName.ToUpper() select ob).ToList<Queue_List>();
                            myq.My_Diag_Order_Count = resultSet.Count;
                        }
                        /* Commented For Bug ID: 33976
                        query1 = Mysession.GetNamedQuery("CountFillMyOrderObjectDetails.WithoutFacility.ShowAllFalse");
                        query1.SetString(0, UserName);
                        query1.SetString(1, UserName);
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        myq.My_Diag_Order_Count = Convert.ToInt16(FavoriteList[0]);
                        */

                        //if (objStaticLookupMgr.getStaticLookupByFieldName("CMG FACILITY NAME")[0].Value.Trim().ToUpper() != FacName)
                        var facAncillary = from f in NHibernateSessionUtility.Instance.MyAncillaryFacilityList where f.Fac_Name == FacName select f;
                        IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                        if (ilstFacAncillary.Count == 0)
                        {
                            //if (sAncillary.Trim().ToUpper() != FacName)//Bug ID:33975
                            //{
                            if (lstQueue.Count > 0)
                            {
                                var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == "UNKNOWN" && ob.Fac_Name.ToUpper() == FacName.ToUpper() select ob).ToList<Queue_List>();
                                myq.Diag_Order_Count = resultSet.Count;
                            }
                            /* Commented For Bug ID: 33976
                            query1 = Mysession.GetNamedQuery("CountFillMyOrderObjectDetails.WithFacility.ShowAllFalse");
                            query1.SetString(2, FacName);
                            query1.SetString(0, UserName);
                            query1.SetString(1, "UNKNOWN");
                            query1.SetParameterList("ObjList", myObjType);
                            FavoriteList = new ArrayList(query1.List());
                            myq.Diag_Order_Count = Convert.ToInt16(FavoriteList[0]);
                             */
                        }
                        else
                        {
                            if (lstQueue.Count > 0)
                            {
                                var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == "UNKNOWN" select ob).ToList<Queue_List>();
                                myq.Diag_Order_Count = resultSet.Count;
                            }
                            /* Commented For Bug ID: 33976
                           query1 = Mysession.GetNamedQuery("CountFillMyOrderObjectDetails.WithFacility.ShowAllFalse.CMG");
                           query1.SetString(0, UserName);
                           query1.SetString(1, "UNKNOWN");
                           query1.SetParameterList("ObjList", myObjType);
                           FavoriteList = new ArrayList(query1.List());
                           myq.Diag_Order_Count = Convert.ToInt16(FavoriteList[0]);
                           */
                        }
                        //commented for bugId: 34087 
                        //IList<StaticLookup> stFieldLook = objStaticLookupMgr.getStaticLookupByFieldName("CMG LAB NAME");
                        //string stFieldLook = "CMG Anc.-1866 #101";

                        ////IList<StaticLookup> stFieldLookFacilityName = objStaticLookupMgr.getStaticLookupByFieldName("CMG FACILITY NAME");
                        ////if (FacName == stFieldLookFacilityName[0].Value)
                        //if (FacName.ToString().ToUpper().Trim() == sAncillary.ToString().ToUpper().Trim())//Bug ID:33975
                        //{
                        //    if (resultmyqList != null && resultmyqList.Count > 0)
                        //    {
                        //        IList<MyQ> tempList = resultmyqList.Where(a => ((a.Current_Process == "RESULT_PROCESS" && a.EHR_Obj_Type == "DIAGNOSTIC ORDER" && a.CMG_Encounter_ID == 0) || (a.Current_Process == "MA_RESULTS")) && a.Lab_Name.Trim().ToUpper() == stFieldLook.Trim().ToUpper()).ToList<MyQ>();
                        //        resultmyqList = tempList;

                        //    }
                        //}

                    }
                    //if (ObjType.Contains("INTERNAL DIAGNOSTIC ORDER") || ObjType.Contains("INTERNAL IMAGE ORDER"))
                    //{
                    //    string[] myObjType = new string[2];
                    //    myObjType[0] = "INTERNAL DIAGNOSTIC ORDER";
                    //    myObjType[1] = "INTERNAL IMAGE ORDER";

                    //    query1 = Mysession.GetNamedQuery("CountFillMyInternalLabImageOrderObjectDetails.WithoutFacility");
                    //    query1.SetString(0, UserName);
                    //    query1.SetString(1, UserName);
                    //    query1.SetParameterList("ObjList", myObjType);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    myq.My_inter_Order_Count = Convert.ToInt16(FavoriteList[0]);

                    //    query1 = Mysession.GetNamedQuery("CountFillMyInternalLabImageOrderObjectDetails.WithFacility");
                    //    query1.SetString(2, FacName);
                    //    query1.SetString(0, UserName);
                    //    query1.SetString(1, "UNKNOWN");
                    //    query1.SetParameterList("ObjList", myObjType);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    myq.Inter_Order_Count = Convert.ToInt16(FavoriteList[0]);
                    //}
                    //if (ObjType.Contains("INTERNAL ORDER"))
                    //{
                    //    string[] myObjType = new string[1];
                    //    myObjType[0] = "INTERNAL ORDER";
                    //    query1 = Mysession.GetNamedQuery("Count.InternalOrder.Queues.ShowAllFalse");
                    //    query1.SetString(0, UserName);
                    //    query1.SetParameterList("ObjList", myObjType);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    IList<Queue_List> lstQueue = new List<Queue_List>();
                    //    foreach (object obj in FavoriteList)
                    //    {
                    //        Queue_List objQueue = new Queue_List();
                    //        object[] item = (object[])obj;
                    //        objQueue.Fac_Name = Convert.ToString(item[11]);
                    //        objQueue.Current_Owner = Convert.ToString(item[14]);
                    //        objQueue.Obj_Type = Convert.ToString(item[16]);
                    //        lstQueue.Add(objQueue);
                    //    }
                    //    if (lstQueue.Count > 0)
                    //    {
                    //        var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == "UNKNOWN" && ob.Fac_Name.ToUpper() == FacName.ToUpper() select ob).ToList<Queue_List>();
                    //        myq.Inter_Order_Count = resultSet.Count;
                    //        resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == UserName.ToUpper() select ob).ToList<Queue_List>();
                    //        myq.My_inter_Order_Count = resultSet.Count;
                    //    }
                    //    /* Commented for Bug ID: 33976
                    //    query1 = Mysession.GetNamedQuery("CountFillMyInternalOrderObjectDetails.WithoutFacility.ShowAllFalse");
                    //    query1.SetString(0, UserName);
                    //    query1.SetString(1, UserName);
                    //    query1.SetParameterList("ObjList", myObjType);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    myq.My_inter_Order_Count = Convert.ToInt16(FavoriteList[0]);

                    //    query1 = Mysession.GetNamedQuery("CountFillMyInternalOrderObjectDetails.WithFacility.ShowAllFalse");
                    //    query1.SetString(2, FacName);
                    //    query1.SetString(0, UserName);
                    //    query1.SetString(1, "UNKNOWN");
                    //    query1.SetParameterList("ObjList", myObjType);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    myq.Inter_Order_Count = Convert.ToInt16(FavoriteList[0]);
                    //    */
                    //}
                    if (ObjType.Contains("IMMUNIZATION ORDER"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "IMMUNIZATION ORDER";

                        query1 = Mysession.GetNamedQuery("Count.Immunization.Order.Queues");
                        query1.SetString(0, UserName);
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        IList<Queue_List> lstQueue = new List<Queue_List>();
                        foreach (object obj in FavoriteList)
                        {
                            Queue_List objQueue = new Queue_List();
                            object[] item = (object[])obj;
                            objQueue.Fac_Name = Convert.ToString(item[11]);
                            objQueue.Current_Owner = Convert.ToString(item[14]);
                            objQueue.Obj_Type = Convert.ToString(item[15]);
                            lstQueue.Add(objQueue);
                        }
                        if (lstQueue.Count > 0)
                        {
                            var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == "UNKNOWN" && ob.Fac_Name.ToUpper() == FacName.ToUpper() select ob).ToList<Queue_List>();
                            myq.Immun_Order_Count = resultSet.Count;
                            resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == UserName.ToUpper() select ob).ToList<Queue_List>();
                            myq.My_Immun_Order_Count = resultSet.Count;
                        }
                        /* Commented For Bug ID:33976
                        query1 = Mysession.GetNamedQuery("CountFillMyImmunizationOrderObjectDetails.WithoutFacility.ShowAllFalse");
                        query1.SetString(0, UserName);
                        query1.SetString(1, UserName);
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        myq.My_Immun_Order_Count = Convert.ToInt16(FavoriteList[0]);

                        query1 = Mysession.GetNamedQuery("CountFillMyImmunizationOrderObjectDetails.WithFacility.ShowAllFalse");
                        query1.SetString(2, FacName);
                        query1.SetString(0, UserName);
                        query1.SetString(1, "UNKNOWN");
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        myq.Immun_Order_Count = Convert.ToInt16(FavoriteList[0]);
                        */
                    }
                    if (ObjType.Contains("REFERRAL ORDER"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "REFERRAL ORDER";
                        query1 = Mysession.GetNamedQuery("Count.ReferralOrder.Queues.ShowAllFalse");
                        query1.SetString(0, UserName);
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        IList<Queue_List> lstQueue = new List<Queue_List>();
                        foreach (object obj in FavoriteList)
                        {
                            Queue_List objQueue = new Queue_List();
                            object[] item = (object[])obj;
                            objQueue.Fac_Name = Convert.ToString(item[10]);
                            objQueue.Current_Owner = Convert.ToString(item[13]);
                            objQueue.Obj_Type = Convert.ToString(item[15]);
                            lstQueue.Add(objQueue);
                        }
                        if (lstQueue.Count > 0)
                        {
                            var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == "UNKNOWN" && ob.Fac_Name.ToUpper() == FacName.ToUpper() select ob).ToList<Queue_List>();
                            myq.Refer_Order_Count = resultSet.Count;
                            resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == UserName.ToUpper() select ob).ToList<Queue_List>();
                            myq.My_Refer_Order_Count = resultSet.Count;
                        }
                        /* Commented For Bug ID:33976
                        query1 = Mysession.GetNamedQuery("CountFillMyReferralOrderObjectDetails.WithoutFacility.ShowAllFalse");
                        query1.SetString(0, UserName);
                        query1.SetString(1, UserName);
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        myq.My_Refer_Order_Count = Convert.ToInt16(FavoriteList[0]);

                        query1 = Mysession.GetNamedQuery("CountFillMyReferralOrderObjectDetails.WithFacility.ShowAllFalse");
                        query1.SetString(2, FacName);
                        query1.SetString(0, UserName);
                        query1.SetString(1, "UNKNOWN");
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        myq.Refer_Order_Count = Convert.ToInt16(FavoriteList[0]);
                        */
                    }
                    if (ObjType.Contains("E-PRESCRIBE"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "E-PRESCRIBE";
                        query1 = Mysession.GetNamedQuery("Count.E-Prescription.Queues.ShowAllFalse");
                        query1.SetString(0, UserName);
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        IList<Queue_List> lstQueue = new List<Queue_List>();
                        foreach (object obj in FavoriteList)
                        {
                            Queue_List objQueue = new Queue_List();
                            object[] item = (object[])obj;
                            objQueue.Current_Owner = Convert.ToString(item[10]);
                            objQueue.Obj_Type = Convert.ToString(item[9]);
                            lstQueue.Add(objQueue);
                        }
                        if (lstQueue.Count > 0)
                        {
                            var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == "UNKNOWN" select ob).ToList<Queue_List>();
                            myq.Presc_Count = resultSet.Count;
                            resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == UserName.ToUpper() select ob).ToList<Queue_List>();
                            myq.My_Presc_Count = resultSet.Count;
                        }
                        /* Commented For Bug ID:33976
                        query1 = Mysession.GetNamedQuery("CountFillMyE-PrescriptionObjectDetails.WithoutFacility.ShowAllFalse");
                        query1.SetString(0, UserName);
                        query1.SetString(1, UserName);
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        myq.My_Presc_Count = Convert.ToInt16(FavoriteList[0]);

                        query1 = Mysession.GetNamedQuery("CountFillMyE-PrescriptionObjectDetails.WithFacility.ShowAllFalse");
                        query1.SetString(2, FacName);
                        query1.SetString(0, UserName);
                        query1.SetString(1, "UNKNOWN");
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        myq.Presc_Count = Convert.ToInt16(FavoriteList[0]);
                        */
                    }

                    if (ObjType.Contains("SCAN") || ObjType.Contains("SCAN RESULT"))
                    {
                        string[] myObjType = new string[2];
                        myObjType[0] = "SCAN";
                        myObjType[1] = "SCAN RESULT";


                        query1 = Mysession.GetNamedQuery("CountFillMyScanObjectDetails.WithoutFacility.No_ofdays");
                        query1.SetString(0, UserName);
                        query1.SetString(1, UserName);
                        query1.SetString(2, myObjType[0].ToString());
                        query1.SetString(3, UserName);
                        query1.SetString(4, UserName);
                        query1.SetString(5, myObjType[1].ToString());
                        FavoriteList = new ArrayList(query1.List());
                        myq.My_Scan_Count = Convert.ToInt16(FavoriteList[0]);

                        query1 = Mysession.GetNamedQuery("CountFillMyScanObjectDetails.WithFacility.No_ofdays");
                        query1.SetString(0, UserName);
                        query1.SetString(1, "UNKNOWN");
                        query1.SetString(2, FacName);
                        query1.SetString(3, myObjType[0].ToString());
                        query1.SetString(4, UserName);
                        query1.SetString(5, "UNKNOWN");
                        query1.SetString(6, FacName);
                        query1.SetString(7, myObjType[1].ToString());
                        FavoriteList = new ArrayList(query1.List());
                        myq.Scan_Count = Convert.ToInt16(FavoriteList[0]);
                    }

                    if (ObjType.Contains("DIAGNOSTIC_RESULT") && (FacName == "ALL"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "DIAGNOSTIC_RESULT";
                        query1 = Mysession.GetNamedQuery("CountFillMyResultsObjectDetails");
                        query1.SetString(0, UserName);
                        //query1.SetString(1, "RESULT_REVIEW");
                        query1.SetString(1, "DIAGNOSTIC_RESULT");
                        //query1.SetInt32(2, DefaultNoofDays);
                        FavoriteList = new ArrayList(query1.List());
                        myq.My_DiagRslt_Order_Count = Convert.ToInt16(FavoriteList[0]);

                    }

                }
                catch (Exception ex)
                {
                    String sException = string.Empty;
                    if (ex.StackTrace != null)
                        sException = ex.StackTrace.ToString();
                    if (ex.Message != null)
                        sException += Environment.NewLine + ex.Message.ToString();
                    if (ex.InnerException != null && ex.InnerException.Message != null)
                        sException += Environment.NewLine + ex.InnerException.Message.ToString();
                    throw new Exception(sException);
                }
                finally
                {
                    Mysession.Close();
                }
            }

            myq.Order_Count = myq.Diag_Order_Count + myq.Immun_Order_Count + myq.Inter_Order_Count + myq.Refer_Order_Count;

            myq.My_Order_Count = myq.My_Diag_Order_Count + myq.My_Immun_Order_Count + myq.My_inter_Order_Count + myq.My_Refer_Order_Count + myq.My_DiagRslt_Order_Count;

            countlist.Add(myq);
            return countlist;
        }

        public ulong GetEncountershowallcount(string ProcessType, string UserName, string[] ObjType, string FacName)
        {
            ulong EncounterCount = 0;
            using (ISession Mysession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {
                    if (FacName.Contains("ViewAllFacilities"))
                    {

                        ArrayList encounterList = null;

                        IQuery query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithoutFacility.WithAllAppointments.Count");

                        if (ProcessType == "UNASSIGNED")
                        {
                            query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithAllAppointments.chkViewAllFacilities.Count");
                            // query1.SetInt32(0, DefaultNoofDays);
                            query1.SetString(0, UserName);
                            if (ProcessType == "UNASSIGNED")
                            {
                                query1.SetString(1, "UNKNOWN");
                            }
                            else
                            {
                                query1.SetString(1, UserName);
                            }
                            query1.SetString(2, UserName);
                            //query1.SetString(2, FacName);



                        }
                        else
                        {

                            query1.SetString(0, UserName);

                        }

                        query1.SetParameterList("ObjList", ObjType);
                        encounterList = new ArrayList(query1.List());
                        EncounterCount = Convert.ToUInt32(encounterList[0]);
                    }
                    else
                    {
                        ArrayList encounterList = null;

                        IQuery query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithoutFacility.WithAllAppointments.Count");

                        if (ProcessType == "UNASSIGNED")
                        {
                            query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithAllAppointments.Count");
                            // query1.SetInt32(0, DefaultNoofDays);
                            query1.SetString(0, UserName);
                            if (ProcessType == "UNASSIGNED")
                            {
                                query1.SetString(1, "UNKNOWN");
                            }
                            else
                            {
                                query1.SetString(1, UserName);
                            }
                            query1.SetString(2, FacName);



                        }
                        else
                        {

                            query1.SetString(0, UserName);

                        }

                        query1.SetParameterList("ObjList", ObjType);
                        encounterList = new ArrayList(query1.List());
                        EncounterCount = Convert.ToUInt32(encounterList[0]);
                    }
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
            }
            return EncounterCount;
        }
        public IList<MyQueueCountDTO> ObjectCount(string FacName, string[] ObjType, string UserName, int DefaultNoofDays)
        {


            if (FacName.Contains("ViewAllFacilities"))
            {
                FacName = FacName.Split('~')[1];
            }

            //string sAncillary = string.Empty;
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}
            ArrayList FavoriteList = null;
            IQuery query1 = null;
            MyQueueCountDTO myq = new MyQueueCountDTO();
            IList<MyQueueCountDTO> countlist = new List<MyQueueCountDTO>();


            using (ISession Mysession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {

                    if (ObjType.Contains("TASK"))
                    {
                        #region changed for new change
                        string[] myObjType = new string[1];
                        myObjType[0] = "TASK";

                        #endregion
                        // Commented for Bug ID: 33976
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("CountFillMyTaskObjectDetails.WithoutFacility.ShowAllFalse");
                            query1.SetString(0, UserName);
                            //Jira #CAP-939 -start
                            //query1.SetString(1, UserName);
                            string[] ObjTaskType = new string[1];
                            ObjTaskType[0] = "TASK";
                            query1.SetParameterList("ObjList", ObjTaskType);
                            //Jira #CAP-939 - end
                            // query1.SetParameterList("ObjList", myObjType);
                            FavoriteList = new ArrayList(query1.List());
                            myq.My_Task_Count = Convert.ToInt16(FavoriteList[0]);
                        }
                        else
                        {

                            query1 = Mysession.GetNamedQuery("CountFillMyTaskObjectDetails.WithFacility.ShowAllFalse");
                            //query1.SetString(2, FacName);
                            //query1.SetString(0, UserName);
                            //query1.SetString(1, "UNKNOWN");
                            ////query1.SetParameterList("ObjList", myObjType);
                            query1.SetString("userName", UserName);
                            query1.SetString("currentOwner", "UNKNOWN");
                            query1.SetString("facilityName", FacName);
                            string[] ObjTaskType = new string[1];
                            ObjTaskType[0] = "TASK";
                            query1.SetParameterList("ObjList", ObjTaskType);
                            FavoriteList = new ArrayList(query1.List());
                            myq.Task_Count = Convert.ToInt16(FavoriteList[0]);
                        }

                    }
                    if (ObjType.Contains("ADDENDUM"))
                    {
                        #region commented for new change
                        //string[] myObjType = new string[1];
                        //myObjType[0] = "ADDENDUM";
                        //query1 = Mysession.GetNamedQuery("Count.Addendum.Queues.WithoutAllAppointments");
                        //query1.SetString(0, UserName);
                        //query1.SetParameterList("ObjList", myObjType);
                        //FavoriteList = new ArrayList(query1.List());
                        //IList<Queue_List> lstQueue = new List<Queue_List>();
                        //foreach (object obj in FavoriteList)
                        //{
                        //    Queue_List objQueue = new Queue_List();
                        //    object[] item = (object[])obj;
                        //    objQueue.Fac_Name = Convert.ToString(item[8]);
                        //    objQueue.Current_Owner = Convert.ToString(item[18]);
                        //    objQueue.Obj_Type = Convert.ToString(item[16]);
                        //    lstQueue.Add(objQueue);
                        //}
                        //if (lstQueue.Count > 0)
                        //{
                        //    var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == "UNKNOWN" && ob.Fac_Name.ToUpper() == FacName.ToUpper() select ob).ToList<Queue_List>();
                        //    myq.Amendmnt_Count = resultSet.Count;
                        //    resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == UserName.ToUpper() select ob).ToList<Queue_List>();
                        //    myq.My_Amendmnt_Count = resultSet.Count;
                        //}
                        #endregion
                        // Commented for Bug ID: 33976
                        string[] myObjType = new string[1];
                        myObjType[0] = "ADDENDUM";
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("CountFillMyAddendumObjectDetails.WithoutFacility.WithoutAllAppointments");
                            query1.SetString(0, UserName);
                            //query1.SetString(1, UserName);
                            // query1.SetParameterList("ObjList", myObjType);
                            FavoriteList = new ArrayList(query1.List());
                            myq.My_Amendmnt_Count = Convert.ToInt16(FavoriteList[0]);
                        }
                        else
                        {
                            query1 = Mysession.GetNamedQuery("CountFillMyAddendumObjectDetails.WithFacility.WithoutAllAppointments");
                            query1.SetString(2, FacName);
                            query1.SetString(0, UserName);
                            query1.SetString(1, "UNKNOWN");
                            //  query1.SetParameterList("ObjList", myObjType);
                            FavoriteList = new ArrayList(query1.List());
                            myq.Amendmnt_Count = Convert.ToInt16(FavoriteList[0]);


                        }
                    }
                    if (ObjType.Contains("DICTATION_RESULT"))
                    {
                        #region commented for new change
                        //string[] myObjType = new string[1];
                        //myObjType[0] = "DICTATION_RESULT";
                        //query1 = Mysession.GetNamedQuery("Count.Dictation.Queues.WithoutAllAppointments");
                        //query1.SetString("UserName", UserName);
                        //query1.SetParameterList("ObjList", myObjType);
                        //FavoriteList = new ArrayList(query1.List());
                        //IList<Queue_List> lstQueue = new List<Queue_List>();
                        //foreach (object obj in FavoriteList)
                        //{
                        //    Queue_List objQueue = new Queue_List();
                        //    object[] item = (object[])obj;
                        //    objQueue.Fac_Name = Convert.ToString(item[5]);
                        //    objQueue.Current_Owner = Convert.ToString(item[6]);
                        //    objQueue.Obj_Type = Convert.ToString(item[7]);
                        //    lstQueue.Add(objQueue);
                        //}
                        //if (lstQueue.Count > 0)
                        //{
                        //    var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == "UNKNOWN" && ob.Fac_Name.ToUpper() == FacName.ToUpper() select ob).ToList<Queue_List>();
                        //    myq.Dict_Count = resultSet.Count;
                        //    resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == UserName.ToUpper() select ob).ToList<Queue_List>();
                        //    myq.My_Dict_Count = resultSet.Count;
                        //}
                        #endregion
                        // Commented for Bug ID: 33976
                        string[] myObjType = new string[1];
                        myObjType[0] = "DICTATION_RESULT";
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("CountFillMyDictationObjectDetails.WithoutFacility.WithoutAllAppointments");
                            query1.SetString("UserName", UserName);
                            query1.SetString("CurOwner", UserName);
                            // query1.SetParameterList("ObjList", myObjType);
                            FavoriteList = new ArrayList(query1.List());
                            myq.My_Dict_Count = Convert.ToInt16(FavoriteList[0]);

                        }
                        else
                        {
                            query1 = Mysession.GetNamedQuery("CountFillMyDictationObjectDetails.WithFacility.WithoutAllAppointments");
                            query1.SetString("FacName", FacName);
                            query1.SetString("UserName", UserName);
                            query1.SetString("CurOwner", "UNKNOWN");
                            // query1.SetParameterList("ObjList", myObjType);
                            FavoriteList = new ArrayList(query1.List());
                            myq.Dict_Count = Convert.ToInt16(FavoriteList[0]);
                        }

                    }
                    //if (ObjType.Contains("DIAGNOSTIC ORDER") && ObjType.Contains("INTERNAL ORDER") && ObjType.Contains("IMMUNIZATION ORDER") && ObjType.Contains("REFERRAL ORDER"))
                    if (ObjType.Contains("DIAGNOSTIC ORDER") && ObjType.Contains("IMMUNIZATION ORDER") && ObjType.Contains("REFERRAL ORDER") && ObjType.Contains("DME ORDER"))
                    {
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("Count.MyOrders");
                            query1.SetString(0, UserName);
                            query1.SetString(1, UserName);
                            // query1.SetParameterList("ObjList", myObjType);
                            FavoriteList = new ArrayList(query1.List());
                            myq.My_Order_Count = Convert.ToInt16(FavoriteList[0]);
                        }
                        var facAncillary = from f in NHibernateSessionUtility.Instance.MyAncillaryFacilityList where f.Fac_Name == FacName select f;
                        IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                        if (ilstFacAncillary.Count == 0 && FacName != "ALL")
                        {
                            //if (sAncillary.Trim().ToUpper() != FacName && FacName != "ALL")
                            //{
                            query1 = Mysession.GetNamedQuery("Count.Orders");
                            query1.SetString(2, FacName);
                            query1.SetString(0, UserName);
                            query1.SetString(1, "UNKNOWN");
                            // query1.SetParameterList("ObjList", myObjType);
                            FavoriteList = new ArrayList(query1.List());
                            myq.Order_Count = Convert.ToInt16(FavoriteList[0]);
                        }
                        else if (FacName != "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("CountFillMyOrderObjectDetails.WithFacility.ShowAllFalse.CMG");
                            query1.SetString(0, UserName);
                            query1.SetString(1, "UNKNOWN");
                            query1.SetString(2, FacName);
                            //query1.SetParameterList("ObjList", myObjType);
                            FavoriteList = new ArrayList(query1.List());
                            myq.Diag_Order_Count = Convert.ToInt16(FavoriteList[0]);
                        }
                        if (myq.Diag_Order_Count > 0)
                        {
                            myq.Order_Count = myq.Order_Count + myq.Diag_Order_Count;
                        }
                    }
                    //#region differentorders
                    //if (ObjType.Contains("DIAGNOSTIC ORDER")) 
                    //{
                    //    string[] myObjType = new string[1];
                    //    myObjType[0] = "DIAGNOSTIC ORDER";
                    //    //myObjType[1] = "IMAGE ORDER";
                    //    #region commented for new change
                    //    //StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
                    //    //query1 = Mysession.GetNamedQuery("Count.DiagnosticOrder.Queues.ShowAllFalse");
                    //    //query1.SetString(0, UserName);
                    //    //query1.SetParameterList("ObjList", myObjType);
                    //    //FavoriteList = new ArrayList(query1.List());
                    //    //IList<Queue_List> lstQueue = new List<Queue_List>();
                    //    //foreach (object obj in FavoriteList)
                    //    //{
                    //    //    Queue_List objQueue = new Queue_List();
                    //    //    object[] item = (object[])obj;
                    //    //    objQueue.Fac_Name = Convert.ToString(item[15]);
                    //    //    objQueue.Current_Owner = Convert.ToString(item[17]);
                    //    //    objQueue.Obj_Type = Convert.ToString(item[19]);
                    //    //    lstQueue.Add(objQueue);
                    //    //}
                    //    //if(lstQueue.Count>0)
                    //    //{
                    //    //    var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == UserName.ToUpper() select ob).ToList<Queue_List>();
                    //    //    myq.My_Diag_Order_Count = resultSet.Count;
                    //    //}
                    //    #endregion
                    //    // Commented For Bug ID: 33976
                    //    if (FacName == "ALL")
                    //    {
                    //        query1 = Mysession.GetNamedQuery("CountFillMyOrderObjectDetails.WithoutFacility.ShowAllFalse");
                    //        query1.SetString(0, UserName);                           
                    //        query1.SetParameterList("ObjList", myObjType);
                    //        FavoriteList = new ArrayList(query1.List());
                    //        myq.My_Diag_Order_Count = Convert.ToInt16(FavoriteList[0]);


                    //    }

                    //    if ("sAncillary".Trim().ToUpper() != FacName && FacName != "ALL")//Bug ID:33975
                    //    {

                    //        query1 = Mysession.GetNamedQuery("CountFillMyOrderObjectDetails.WithFacility.ShowAllFalse");
                    //        query1.SetString(2, FacName);
                    //        query1.SetString(0, UserName);
                    //        query1.SetString(1, "UNKNOWN");
                    //        query1.SetParameterList("ObjList", myObjType);
                    //        FavoriteList = new ArrayList(query1.List());
                    //        myq.Diag_Order_Count = Convert.ToInt16(FavoriteList[0]);

                    //    }
                    //    else
                    //    {

                    //        if (FacName != "ALL")
                    //        {
                    //            query1 = Mysession.GetNamedQuery("CountFillMyOrderObjectDetails.WithFacility.ShowAllFalse.CMG");
                    //            query1.SetString(0, "UNKNOWN");
                    //            query1.SetParameterList("ObjList", myObjType);
                    //            FavoriteList = new ArrayList(query1.List());
                    //            myq.Diag_Order_Count = Convert.ToInt16(FavoriteList[0]);
                    //        }

                    //    }


                    //}

                    //if (ObjType.Contains("INTERNAL ORDER"))
                    //{
                    //    string[] myObjType = new string[1];
                    //    myObjType[0] = "INTERNAL ORDER";
                    //    #region commented for new change
                    //    //query1 = Mysession.GetNamedQuery("Count.InternalOrder.Queues.ShowAllFalse");
                    //    //query1.SetString(0, UserName);
                    //    //query1.SetParameterList("ObjList", myObjType);
                    //    //FavoriteList = new ArrayList(query1.List());
                    //    //IList<Queue_List> lstQueue = new List<Queue_List>();
                    //    //foreach (object obj in FavoriteList)
                    //    //{
                    //    //    Queue_List objQueue = new Queue_List();
                    //    //    object[] item = (object[])obj;
                    //    //    objQueue.Fac_Name = Convert.ToString(item[11]);
                    //    //    objQueue.Current_Owner = Convert.ToString(item[14]);
                    //    //    objQueue.Obj_Type = Convert.ToString(item[16]);
                    //    //    lstQueue.Add(objQueue);
                    //    //}
                    //    //if (lstQueue.Count > 0)
                    //    //{
                    //    //    var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == "UNKNOWN" && ob.Fac_Name.ToUpper() == FacName.ToUpper() select ob).ToList<Queue_List>();
                    //    //    myq.Inter_Order_Count = resultSet.Count;
                    //    //    resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == UserName.ToUpper() select ob).ToList<Queue_List>();
                    //    //    myq.My_inter_Order_Count = resultSet.Count;
                    //    //}
                    //    #endregion
                    //    //Commented for Bug ID: 33976
                    //    if (FacName == "ALL")
                    //    {
                    //        query1 = Mysession.GetNamedQuery("CountFillMyInternalOrderObjectDetails.WithoutFacility.ShowAllFalse");
                    //        query1.SetString(0, UserName);                            
                    //        query1.SetParameterList("ObjList", myObjType);
                    //        FavoriteList = new ArrayList(query1.List());
                    //        myq.My_inter_Order_Count = Convert.ToInt16(FavoriteList[0]);
                    //    }
                    //    else
                    //    {
                    //        query1 = Mysession.GetNamedQuery("CountFillMyInternalOrderObjectDetails.WithFacility.ShowAllFalse");
                    //        query1.SetString(2, FacName);
                    //        query1.SetString(0, UserName);
                    //        query1.SetString(1, "UNKNOWN");
                    //        query1.SetParameterList("ObjList", myObjType);
                    //        FavoriteList = new ArrayList(query1.List());
                    //        myq.Inter_Order_Count = Convert.ToInt16(FavoriteList[0]);
                    //    }

                    //}
                    //if (ObjType.Contains("IMMUNIZATION ORDER"))
                    //{
                    //    string[] myObjType = new string[1];
                    //    myObjType[0] = "IMMUNIZATION ORDER";
                    //    #region commented for new change
                    //    //query1 = Mysession.GetNamedQuery("Count.Immunization.Order.Queues");
                    //    //query1.SetString(0, UserName);
                    //    //query1.SetParameterList("ObjList",myObjType);
                    //    //FavoriteList = new ArrayList(query1.List());
                    //    //IList<Queue_List> lstQueue=new List<Queue_List>();
                    //    //foreach(object obj in FavoriteList)
                    //    //{
                    //    //    Queue_List objQueue=new Queue_List();
                    //    //    object[] item = (object[])obj;
                    //    //    objQueue.Fac_Name=Convert.ToString(item[11]);
                    //    //    objQueue.Current_Owner=Convert.ToString(item[14]);
                    //    //    objQueue.Obj_Type=Convert.ToString(item[15]);
                    //    //    lstQueue.Add(objQueue);
                    //    //}
                    //    //if(lstQueue.Count>0)
                    //    //{
                    //    //    var resultSet=(from ob in lstQueue where ob.Current_Owner.ToUpper()=="UNKNOWN" && ob.Fac_Name.ToUpper()==FacName.ToUpper() select ob).ToList<Queue_List>();
                    //    //    myq.Immun_Order_Count = resultSet.Count;
                    //    //    resultSet=(from ob in lstQueue where ob.Current_Owner.ToUpper()==UserName.ToUpper() select ob).ToList<Queue_List>();
                    //    //    myq.My_Immun_Order_Count=resultSet.Count;
                    //    //}
                    //    #endregion
                    //    // Commented For Bug ID:33976
                    //    if (FacName == "ALL")
                    //    {
                    //        query1 = Mysession.GetNamedQuery("CountFillMyImmunizationOrderObjectDetails.WithoutFacility.ShowAllFalse");
                    //        query1.SetString(0, UserName);                           
                    //        query1.SetParameterList("ObjList", myObjType);
                    //        FavoriteList = new ArrayList(query1.List());
                    //        myq.My_Immun_Order_Count = Convert.ToInt16(FavoriteList[0]);
                    //    }
                    //    else
                    //    {
                    //        query1 = Mysession.GetNamedQuery("CountFillMyImmunizationOrderObjectDetails.WithFacility.ShowAllFalse");
                    //        query1.SetString(2, FacName);
                    //        query1.SetString(0, UserName);
                    //        query1.SetString(1, "UNKNOWN");
                    //        query1.SetParameterList("ObjList", myObjType);
                    //        FavoriteList = new ArrayList(query1.List());
                    //        myq.Immun_Order_Count = Convert.ToInt16(FavoriteList[0]);
                    //    }
                    //}
                    //if (ObjType.Contains("REFERRAL ORDER"))
                    //{
                    //    string[] myObjType = new string[1];
                    //    myObjType[0] = "REFERRAL ORDER";
                    //    #region commented for new change
                    //    //query1 = Mysession.GetNamedQuery("Count.ReferralOrder.Queues.ShowAllFalse");
                    //    //query1.SetString(0, UserName);
                    //    //query1.SetParameterList("ObjList", myObjType);
                    //    //FavoriteList = new ArrayList(query1.List());
                    //    //IList<Queue_List> lstQueue = new List<Queue_List>();
                    //    //foreach (object obj in FavoriteList)
                    //    //{
                    //    //    Queue_List objQueue = new Queue_List();
                    //    //    object[] item = (object[])obj;
                    //    //    objQueue.Fac_Name = Convert.ToString(item[10]);
                    //    //    objQueue.Current_Owner = Convert.ToString(item[13]);
                    //    //    objQueue.Obj_Type = Convert.ToString(item[15]);
                    //    //    lstQueue.Add(objQueue);
                    //    //}
                    //    //if (lstQueue.Count > 0)
                    //    //{
                    //    //    var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == "UNKNOWN" && ob.Fac_Name.ToUpper() == FacName.ToUpper() select ob).ToList<Queue_List>();
                    //    //    myq.Refer_Order_Count = resultSet.Count;
                    //    //    resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == UserName.ToUpper() select ob).ToList<Queue_List>();
                    //    //    myq.My_Refer_Order_Count = resultSet.Count;
                    //    //}
                    //    #endregion
                    //    // Commented For Bug ID:33976
                    //    if (FacName == "ALL")
                    //    {
                    //        query1 = Mysession.GetNamedQuery("CountFillMyReferralOrderObjectDetails.WithoutFacility.ShowAllFalse");
                    //        query1.SetString(0, UserName);
                    //        query1.SetString(1, UserName);  
                    //        query1.SetParameterList("ObjList", myObjType);
                    //        FavoriteList = new ArrayList(query1.List());
                    //        myq.My_Refer_Order_Count = Convert.ToInt16(FavoriteList[0]);
                    //    }
                    //    else
                    //    {
                    //        query1 = Mysession.GetNamedQuery("CountFillMyReferralOrderObjectDetails.WithFacility.ShowAllFalse");
                    //        query1.SetString(2, FacName);
                    //        query1.SetString(0, UserName);
                    //        query1.SetString(1, "UNKNOWN");
                    //        query1.SetParameterList("ObjList", myObjType);
                    //        FavoriteList = new ArrayList(query1.List());
                    //        myq.Refer_Order_Count = Convert.ToInt16(FavoriteList[0]);

                    //    }
                    //}
                    //#endregion
                    if (ObjType.Contains("E-PRESCRIBE"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "E-PRESCRIBE";
                        #region commented for new change
                        //query1 = Mysession.GetNamedQuery("Count.E-Prescription.Queues.ShowAllFalse");
                        //query1.SetString(0, UserName);
                        //query1.SetParameterList("ObjList", myObjType);
                        //FavoriteList = new ArrayList(query1.List());
                        //IList<Queue_List> lstQueue = new List<Queue_List>();
                        //foreach (object obj in FavoriteList)
                        //{
                        //    Queue_List objQueue = new Queue_List();
                        //    object[] item = (object[])obj;
                        //    objQueue.Current_Owner = Convert.ToString(item[10]);
                        //    objQueue.Obj_Type = Convert.ToString(item[9]);
                        //    lstQueue.Add(objQueue);
                        //}
                        //if (lstQueue.Count > 0)
                        //{
                        //    var resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == "UNKNOWN" select ob).ToList<Queue_List>();
                        //    myq.Presc_Count = resultSet.Count;
                        //    resultSet = (from ob in lstQueue where ob.Current_Owner.ToUpper() == UserName.ToUpper() select ob).ToList<Queue_List>();
                        //    myq.My_Presc_Count = resultSet.Count;
                        //}
                        #endregion
                        // Commented For Bug ID:33976
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("CountFillMyE-PrescriptionObjectDetails.WithoutFacility.ShowAllFalse");
                            query1.SetString(0, UserName);
                            // query1.SetParameterList("ObjList", myObjType);
                            FavoriteList = new ArrayList(query1.List());
                            myq.My_Presc_Count = Convert.ToInt16(FavoriteList[0]);
                        }
                        else
                        {
                            query1 = Mysession.GetNamedQuery("CountFillMyE-PrescriptionObjectDetails.WithFacility.ShowAllFalse");
                            query1.SetString(2, FacName);
                            query1.SetString(0, UserName);
                            query1.SetString(1, "UNKNOWN");
                            //  query1.SetParameterList("ObjList", myObjType);
                            FavoriteList = new ArrayList(query1.List());
                            myq.Presc_Count = Convert.ToInt16(FavoriteList[0]);
                        }
                    }

                    if (ObjType.Contains("SCAN") || ObjType.Contains("SCAN RESULT"))
                    {
                        string[] myObjType = new string[2];
                        myObjType[0] = "SCAN";
                        myObjType[1] = "SCAN RESULT";
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("CountFillMyScanObjectDetails.WithoutFacility.No_ofdays");
                            query1.SetString(0, UserName);
                            query1.SetString(1, myObjType[0].ToString());
                            //query1.SetString(2, UserName);
                            //query1.SetString(3, myObjType[1].ToString());
                            FavoriteList = new ArrayList(query1.List());
                            myq.My_Scan_Count = Convert.ToInt16(FavoriteList[0]);

                        }
                        else
                        {
                            query1 = Mysession.GetNamedQuery("CountFillMyScanObjectDetails.WithFacility.No_ofdays");
                            //query1.SetString(0, UserName);
                            //query1.SetString(1, "UNKNOWN");
                            //query1.SetString(2, FacName);
                            //query1.SetString(3, myObjType[0].ToString());
                            //query1.SetString(4, UserName);
                            //query1.SetString(5, "UNKNOWN");
                            //query1.SetString(6, FacName);
                            //query1.SetString(7, myObjType[1].ToString());

                            query1.SetString(0, "UNKNOWN");
                            query1.SetString(1, myObjType[0].ToString());
                            query1.SetString(2, FacName);
                            query1.SetString(3, "UNKNOWN");
                            query1.SetString(4, myObjType[0].ToString());
                            query1.SetString(5, FacName);
                            FavoriteList = new ArrayList(query1.List());
                            myq.Scan_Count = Convert.ToInt16(FavoriteList[0]);
                        }
                    }

                    if (ObjType.Contains("DIAGNOSTIC_RESULT") && (FacName == "ALL"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "DIAGNOSTIC_RESULT";
                        query1 = Mysession.GetNamedQuery("CountFillMyResultsObjectDetails");
                        query1.SetString(0, UserName);
                        query1.SetString(1, "DIAGNOSTIC_RESULT");
                        FavoriteList = new ArrayList(query1.List());
                        myq.My_DiagRslt_Order_Count = Convert.ToInt16(FavoriteList[0]);

                    }

                }
                catch (Exception ex)
                {
                    String sException = string.Empty;
                    if (ex.StackTrace != null)
                        sException = ex.StackTrace.ToString();
                    if (ex.Message != null)
                        sException += Environment.NewLine + ex.Message.ToString();
                    if (ex.InnerException != null && ex.InnerException.Message != null)
                        sException += Environment.NewLine + ex.InnerException.Message.ToString();
                    throw new Exception(sException);
                }
                finally
                {
                    Mysession.Close();
                }
            }
            countlist.Add(myq);
            return countlist;
        }
        public IList<MyQ> FillObjectsCompleted(string FacName, string[] ObjType, string ProcessType, string UserName, Boolean bShowAll, int DefaultNoofDays, string FacilityName)
        {
            //string sAncillary = string.Empty;
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}

            ArrayList FavoriteList = null;
            IQuery query1 = null;
            ArrayList FavoriteListTask = null;
            IQuery query = null;
            MyQ myq = new MyQ();
            using (ISession Mysession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {
                    query = Mysession.GetNamedQuery("FillMyTaskObjectDetailsCompleted");
                    query.SetString(0, UserName);
                    query.SetParameterList("ObjList", ObjType);
                    FavoriteListTask = new ArrayList(query.List());
                    query1 = Mysession.GetNamedQuery("FillMyTaskObjectDetailsCompletedCreatedBy");
                    query1.SetString(0, UserName);
                    query1.SetParameterList("ObjList", ObjType);
                    FavoriteList = new ArrayList(query1.List());

                    ArrayList FavoriteListFinaltask = new ArrayList();
                    if (FavoriteListTask != null && FavoriteListTask.Count > 0)
                    {
                        Boolean bIsNoMatch = false;

                        foreach (object[] val in FavoriteListTask)
                        {
                            foreach (object[] val1 in FavoriteList)
                            {
                                if (val[0].ToString() != (val1[0].ToString()))
                                {
                                    bIsNoMatch = true;
                                }
                                else
                                {
                                    bIsNoMatch = false;
                                    break;
                                }
                            }

                            if (bIsNoMatch == true || FavoriteList.Count == 0)
                                FavoriteListFinaltask.Add(val);
                        }
                    }
                    FavoriteList.AddRange(FavoriteListFinaltask);
                    FillDTO(FavoriteList, ObjType);
                }
                catch (Exception ex)
                {
                    String sException = string.Empty;
                    if (ex.StackTrace != null)
                        sException = ex.StackTrace.ToString();
                    if (ex.Message != null)
                        sException += Environment.NewLine + ex.Message.ToString();
                    if (ex.InnerException != null && ex.InnerException.Message != null)
                        sException += Environment.NewLine + ex.InnerException.Message.ToString();
                    System.IO.StreamWriter sw = null;
                    try
                    {
                        string path = "C:/Exception_Logs/Exception_Log_File.txt";
                        sw = System.IO.File.AppendText(path);
                        string logLine = System.String.Format(
                            "{0:G}: {1}.", System.DateTime.Now, sException);
                        sw.WriteLine(logLine);
                    }
                    finally
                    {
                        sw.Close();
                    }

                    throw new Exception(sException);
                }
                finally
                {
                    Mysession.Close();
                }
            }
            return resultmyqList;
        }

        public IList<MyQ> FillObjectsOpenTaskCreatedByMe(string FacNae, string[] ObjType, string ProcessType, string UserName, Boolean bShowAll, int DefaultNoofDays, string FacilityName)
        {
            IQuery query1 = null;
            MyQ myq = new MyQ();
            using (ISession Mysession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {
                    if (bShowAll)
                    {
                        query1 = Mysession.GetNamedQuery("FillMyTaskObjectDetails.WithoutFacilityCreatedby");
                    }
                    else
                    {
                        query1 = Mysession.GetNamedQuery("FillMyTaskObjectDetails.WithoutFacility.ShowAllFalseCreatedby");
                    }

                    query1.SetString(0, UserName);
                    query1.SetParameterList("ObjList", ObjType);
                    var FavoriteList = new ArrayList(query1.List());
                    FillDTO(FavoriteList, ObjType);
                }
                catch (Exception ex)
                {
                    String sException = string.Empty;
                    if (ex.StackTrace != null)
                        sException = ex.StackTrace.ToString();
                    if (ex.Message != null)
                        sException += Environment.NewLine + ex.Message.ToString();
                    if (ex.InnerException != null && ex.InnerException.Message != null)
                        sException += Environment.NewLine + ex.InnerException.Message.ToString();
                    System.IO.StreamWriter sw = null;
                    try
                    {
                        string path = "C:/Exception_Logs/Exception_Log_File.txt";
                        sw = System.IO.File.AppendText(path);
                        string logLine = System.String.Format(
                            "{0:G}: {1}.", System.DateTime.Now, sException);
                        sw.WriteLine(logLine);
                    }
                    finally
                    {
                        sw.Close();
                    }

                    throw new Exception(sException);
                }
                finally
                {
                    Mysession.Close();
                }
            }
            return resultmyqList;
        }

        public IList<MyQ> FillObjects(string FacName, string[] ObjType, string ProcessType, string UserName, Boolean bShowAll, int DefaultNoofDays, string FacilityName)
        {

            //string sAncillary = string.Empty;
            //if (System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"] != null)
            //{
            //    sAncillary = System.Configuration.ConfigurationManager.AppSettings["AncillaryTestClinic"].ToString();
            //}

            ArrayList FavoriteList = null;
            IQuery query1 = null;
            ArrayList FavoriteListTask = null;
            IQuery queryTask = null;
            MyQ myq = new MyQ();
            // ISession Mysession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession Mysession = NHibernateSessionManager.Instance.CreateISession())
            {
                try
                {
                    if (ObjType.Contains("ENCOUNTER") || ObjType.Contains("DOCUMENTATION") || ObjType.Contains("DOCUMENT REVIEW") || ObjType.Contains("CHECK OUT") || ObjType.Contains("PHONE ENCOUNTER"))
                    {

                        if (FacName == "ALL")
                        {
                            if (bShowAll == true)
                            {
                                query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithoutFacility.WithAllAppointments");
                                // query1.SetString(0, UserName);

                                var facAncillary = from f in NHibernateSessionUtility.Instance.MyAncillaryFacilityList where f.Fac_Name == FacilityName select f;
                                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                                if (ilstFacAncillary.Count > 0)
                                {
                                    //if (sAncillary.ToUpper() == FacilityName.ToUpper())
                                    //{

                                    query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithoutFacility.WithAllAppointments.Ancillary");

                                }


                                if (ProcessType == "UNASSIGNED")
                                {
                                    query1.SetString(0, "UNKNOWN");
                                }
                                else
                                {
                                    query1.SetString(0, UserName);
                                }
                                // query1.SetString(1, UserName);//BugID:48936

                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithoutFacility.WithoutAllAppointments");
                                //  query1.SetInt32(0, DefaultNoofDays);
                                // query1.SetString(1, UserName);

                                var facAncillary = from f in NHibernateSessionUtility.Instance.MyAncillaryFacilityList where f.Fac_Name.ToUpper() == FacilityName.ToUpper() select f;
                                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                                if (ilstFacAncillary.Count > 0)
                                {
                                    //if (sAncillary.ToUpper() == FacilityName.ToUpper())
                                    //{
                                    query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithoutFacility.WithoutAllAppointments.Ancillary");
                                }
                                if (ProcessType == "UNASSIGNED")
                                {
                                    query1.SetString(0, "UNKNOWN");
                                }
                                else
                                {
                                    query1.SetString(0, UserName);
                                }
                                //query1.SetString(1, UserName);//BugID:48936
                            }
                        }
                        else
                        {
                            if (FacName.Contains("ViewAllFacilities"))
                            {
                                if (bShowAll == true)
                                {
                                    query1 = Mysession.CreateSQLQuery("select Carrier_ID,Facility_Name from map_user_carrier_facility where user_name='" + UserName + "'");
                                    ArrayList ilistObj = new ArrayList(query1.List());
                                    if (ilistObj != null)
                                    {
                                        if (ilistObj.Count == 0)
                                        {
                                            query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithAllAppointments.chkViewAllFacilities");
                                            //query1.SetString(0, FacName);
                                            if (ProcessType == "UNASSIGNED")
                                            {
                                                query1.SetString(0, "UNKNOWN");
                                            }
                                            else
                                            {
                                                query1.SetString(0, UserName);
                                            }
                                            query1.SetString(1, UserName);
                                            query1.SetString(2, UserName);
                                        }
                                        else
                                        {
                                            string[] CarrierList = new string[ilistObj.Count];
                                            //string[] FacilityList = new string[ilistObj.Count];
                                            int iCount = 0;
                                            foreach (object item in ilistObj)
                                            {
                                                object[] obj = (object[])item;

                                                CarrierList[iCount] = obj[0].ToString();
                                                //if (obj[1].ToString() == "ALL")
                                                //    FacilityList[iCount] = FacName;
                                                //else
                                                //    FacilityList[iCount] = obj[1].ToString();
                                                iCount++;
                                            }

                                            if (CarrierList.Length == 1 && CarrierList[0] == "0" )//&& FacilityList.Length == 1)
                                            {
                                                query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithAllAppointments.chkViewAllFacilities");
                                                //query1.SetString(0, FacName);

                                                if (ProcessType == "UNASSIGNED")
                                                {
                                                    query1.SetString(0, "UNKNOWN");
                                                }
                                                else
                                                {
                                                    query1.SetString(0, UserName);
                                                }
                                                query1.SetString(1, UserName);
                                                query1.SetString(2, UserName);
                                            }
                                            else
                                            {
                                                query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithAllAppointments.WithCarrier.chkViewAllFacilities");

                                                if (ProcessType == "UNASSIGNED")
                                                {
                                                    query1.SetString(0, "UNKNOWN");
                                                }
                                                else
                                                {
                                                    query1.SetString(0, UserName);
                                                }
                                                query1.SetString(1, UserName);
                                                query1.SetString(2, UserName);

                                                //query1.SetParameterList("FacList", FacilityList);
                                                query1.SetParameterList("CarrierList", CarrierList);
                                            }
                                        }
                                    }

                                    //if (Carrier == "0")
                                    //{
                                    //    query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithAllAppointments");
                                    //}
                                    //else
                                    //{
                                    //    query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithAllAppointments.WithCarrier");
                                    //    query1.SetString(3, Carrier);
                                    //}
                                    //query1.SetString(0, FacName);

                                    //if (ProcessType == "UNASSIGNED")
                                    //{
                                    //    query1.SetString(1, "UNKNOWN");
                                    //}
                                    //else
                                    //{
                                    //    query1.SetString(1, UserName);
                                    //}
                                    //query1.SetString(2, UserName);

                                }
                                else
                                {
                                    query1 = Mysession.CreateSQLQuery("select Carrier_ID,Facility_Name from map_user_carrier_facility where user_name='" + UserName + "'");
                                    ArrayList ilistObj = new ArrayList(query1.List());
                                    if (ilistObj != null)
                                    {
                                        if (ilistObj.Count == 0)
                                        {
                                            query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithoutAllAppointments.chkViewAllFacilities");
                                            //query1.SetString(0, FacName);

                                            if (ProcessType == "UNASSIGNED")
                                            {
                                                query1.SetString(0, "UNKNOWN");
                                            }
                                            else
                                            {
                                                query1.SetString(0, UserName);
                                            }
                                            query1.SetString(1, UserName);
                                            query1.SetString(2, UserName);
                                        }
                                        else
                                        {
                                            string[] CarrierList = new string[ilistObj.Count];
                                            //string[] FacilityList = new string[ilistObj.Count];
                                            int iCount = 0;
                                            foreach (object item in ilistObj)
                                            {
                                                object[] obj = (object[])item;

                                                CarrierList[iCount] = obj[0].ToString();
                                                //if (obj[1].ToString() == "ALL")
                                                //    FacilityList[iCount] = FacName;
                                                //else
                                                //    FacilityList[iCount] = obj[1].ToString();
                                                iCount++;
                                            }

                                            if (CarrierList.Length == 1 && CarrierList[0] == "0" )//&& FacilityList.Length == 1)
                                            {
                                                query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithoutAllAppointments.chkViewAllFacilities");
                                                //query1.SetString(0, FacName);

                                                if (ProcessType == "UNASSIGNED")
                                                {
                                                    query1.SetString(0, "UNKNOWN");
                                                }
                                                else
                                                {
                                                    query1.SetString(0, UserName);
                                                }
                                                query1.SetString(1, UserName);
                                                query1.SetString(2, UserName);
                                            }
                                            else
                                            {
                                                query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithoutAllAppointments.WithCarrier.chkViewAllFacilities");
                                                // query1.SetString(0, FacName);

                                                if (ProcessType == "UNASSIGNED")
                                                {
                                                    query1.SetString(0, "UNKNOWN");
                                                }
                                                else
                                                {
                                                    query1.SetString(0, UserName);
                                                }
                                                query1.SetString(1, UserName);
                                                query1.SetString(2, UserName);

                                                //query1.SetParameterList("FacList", FacilityList);
                                                query1.SetParameterList("CarrierList", CarrierList);
                                            }
                                        }
                                    }

                                    //if (Carrier == "0")
                                    //{
                                    //    query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithoutAllAppointments");
                                    //}
                                    //else
                                    //{
                                    //    query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithoutAllAppointments.WithCarrier");
                                    //    query1.SetString(3, Carrier);
                                    //}
                                    //// query1.SetInt32(0, DefaultNoofDays);
                                    //query1.SetString(0, FacName);

                                    //if (ProcessType == "UNASSIGNED")
                                    //{
                                    //    query1.SetString(1, "UNKNOWN");
                                    //}
                                    //else
                                    //{
                                    //    query1.SetString(1, UserName);
                                    //}
                                    //query1.SetString(2, UserName);
                                }
                            }
                            else
                            {
                                if (bShowAll == true)
                                {
                                    query1 = Mysession.CreateSQLQuery("select Carrier_ID,Facility_Name from map_user_carrier_facility where user_name='" + UserName + "'");
                                    ArrayList ilistObj = new ArrayList(query1.List());
                                    if (ilistObj != null)
                                    {
                                        if (ilistObj.Count == 0)
                                        {
                                            query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithAllAppointments");
                                            query1.SetString(0, FacName);
                                            if (ProcessType == "UNASSIGNED")
                                            {
                                                query1.SetString(1, "UNKNOWN");
                                            }
                                            else
                                            {
                                                query1.SetString(1, UserName);
                                            }
                                            query1.SetString(2, UserName);
                                        }
                                        else
                                        {
                                            string[] CarrierList = new string[ilistObj.Count];
                                            string[] FacilityList = new string[ilistObj.Count];
                                            int iCount = 0;
                                            foreach (object item in ilistObj)
                                            {
                                                object[] obj = (object[])item;

                                                CarrierList[iCount] = obj[0].ToString();
                                                if (obj[1].ToString() == "ALL")
                                                    FacilityList[iCount] = FacName;
                                                else
                                                    FacilityList[iCount] = obj[1].ToString();
                                                iCount++;
                                            }

                                            if (CarrierList.Length == 1 && CarrierList[0] == "0" && FacilityList.Length == 1)
                                            {
                                                query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithAllAppointments");
                                                query1.SetString(0, FacName);

                                                if (ProcessType == "UNASSIGNED")
                                                {
                                                    query1.SetString(1, "UNKNOWN");
                                                }
                                                else
                                                {
                                                    query1.SetString(1, UserName);
                                                }
                                                query1.SetString(2, UserName);
                                            }
                                            else
                                            {
                                                query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithAllAppointments.WithCarrier");

                                                if (ProcessType == "UNASSIGNED")
                                                {
                                                    query1.SetString(0, "UNKNOWN");
                                                }
                                                else
                                                {
                                                    query1.SetString(0, UserName);
                                                }
                                                query1.SetString(1, UserName);

                                                query1.SetParameterList("FacList", FacilityList);
                                                query1.SetParameterList("CarrierList", CarrierList);
                                            }
                                        }
                                    }

                                    //if (Carrier == "0")
                                    //{
                                    //    query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithAllAppointments");
                                    //}
                                    //else
                                    //{
                                    //    query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithAllAppointments.WithCarrier");
                                    //    query1.SetString(3, Carrier);
                                    //}
                                    //query1.SetString(0, FacName);

                                    //if (ProcessType == "UNASSIGNED")
                                    //{
                                    //    query1.SetString(1, "UNKNOWN");
                                    //}
                                    //else
                                    //{
                                    //    query1.SetString(1, UserName);
                                    //}
                                    //query1.SetString(2, UserName);

                                }
                                else
                                {
                                    query1 = Mysession.CreateSQLQuery("select Carrier_ID,Facility_Name from map_user_carrier_facility where user_name='" + UserName + "'");
                                    ArrayList ilistObj = new ArrayList(query1.List());
                                    if (ilistObj != null)
                                    {
                                        if (ilistObj.Count == 0)
                                        {
                                            query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithoutAllAppointments");
                                            query1.SetString(0, FacName);

                                            if (ProcessType == "UNASSIGNED")
                                            {
                                                query1.SetString(1, "UNKNOWN");
                                            }
                                            else
                                            {
                                                query1.SetString(1, UserName);
                                            }
                                            query1.SetString(2, UserName);
                                        }
                                        else
                                        {
                                            string[] CarrierList = new string[ilistObj.Count];
                                            string[] FacilityList = new string[ilistObj.Count];
                                            int iCount = 0;
                                            foreach (object item in ilistObj)
                                            {
                                                object[] obj = (object[])item;

                                                CarrierList[iCount] = obj[0].ToString();
                                                if (obj[1].ToString() == "ALL")
                                                    FacilityList[iCount] = FacName;
                                                else
                                                    FacilityList[iCount] = obj[1].ToString();
                                                iCount++;
                                            }

                                            if (CarrierList.Length == 1 && CarrierList[0] == "0" && FacilityList.Length == 1)
                                            {
                                                query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithoutAllAppointments");
                                                query1.SetString(0, FacName);

                                                if (ProcessType == "UNASSIGNED")
                                                {
                                                    query1.SetString(1, "UNKNOWN");
                                                }
                                                else
                                                {
                                                    query1.SetString(1, UserName);
                                                }
                                                query1.SetString(2, UserName);
                                            }
                                            else
                                            {
                                                query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithoutAllAppointments.WithCarrier");
                                                // query1.SetString(0, FacName);

                                                if (ProcessType == "UNASSIGNED")
                                                {
                                                    query1.SetString(0, "UNKNOWN");
                                                }
                                                else
                                                {
                                                    query1.SetString(0, UserName);
                                                }
                                                query1.SetString(1, UserName);

                                                query1.SetParameterList("FacList", FacilityList);
                                                query1.SetParameterList("CarrierList", CarrierList);
                                            }
                                        }
                                    }

                                    //if (Carrier == "0")
                                    //{
                                    //    query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithoutAllAppointments");
                                    //}
                                    //else
                                    //{
                                    //    query1 = Mysession.GetNamedQuery("FillMyEncounterObjectDetails.WithFacility.WithoutAllAppointments.WithCarrier");
                                    //    query1.SetString(3, Carrier);
                                    //}
                                    //// query1.SetInt32(0, DefaultNoofDays);
                                    //query1.SetString(0, FacName);

                                    //if (ProcessType == "UNASSIGNED")
                                    //{
                                    //    query1.SetString(1, "UNKNOWN");
                                    //}
                                    //else
                                    //{
                                    //    query1.SetString(1, UserName);
                                    //}
                                    //query1.SetString(2, UserName);
                                }
                            }

                        }

                        query1.SetParameterList("ObjList", ObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, ObjType);
                    }
                    else if (ObjType.Contains("TASK"))
                    {
                        //if (bShowAll == true)
                        //{

                        //    if (FacName == "ALL")
                        //    {
                        //        query1 = Mysession.GetNamedQuery("FillMyTaskObjectDetails.WithoutFacility");
                        //        query1.SetString(0, UserName);
                        //    }
                        //}
                        //else
                        //{
                        //    if (FacName == "ALL")
                        //    {

                        //        //queryTask = Mysession.GetNamedQuery("FillMyTaskObjectDetails.WithoutFacility.ShowAllFalseCreatedby");
                        //        //queryTask.SetString(0, UserName);
                        //        //queryTask.SetParameterList("ObjList", ObjType);
                        //        //FavoriteListTask = new ArrayList(queryTask.List());
                        //        query1 = Mysession.GetNamedQuery("FillMyTaskObjectDetails.WithoutFacility.ShowAllFalse");
                        //        query1.SetString(0, UserName);
                        //        // query1.SetInt32(2, DefaultNoofDays);
                        //    }
                        //    else
                        //    {
                        //        query1 = Mysession.GetNamedQuery("FillMyTaskObjectDetails.WithFacility.ShowAllFalse");
                        //        query1.SetString(0, UserName);
                        //        query1.SetString(1, "UNKNOWN");
                        //        query1.SetString(2, FacName);
                        //        //query1.SetInt32(2, DefaultNoofDays);
                        //    }
                        //}
                        ////query1.SetString(0, UserName);
                        ////if (ProcessType == "UNASSIGNED")
                        ////{
                        ////    query1.SetString(1, "UNKNOWN");
                        ////}
                        ////else
                        ////{
                        ////    query1.SetString(1, UserName);
                        ////}

                        if (FacName == "ALL")
                        {
                            if (bShowAll)
                            {
                                query1 = Mysession.GetNamedQuery("FillMyTaskObjectDetails.WithoutFacility");
                                query1.SetString(0, UserName);
                            }
                            else
                            {
                                //queryTask = Mysession.GetNamedQuery("FillMyTaskObjectDetails.WithoutFacility.ShowAllFalseCreatedby");
                                //queryTask.SetString(0, UserName);
                                //queryTask.SetParameterList("ObjList", ObjType);
                                //FavoriteListTask = new ArrayList(queryTask.List());
                                query1 = Mysession.GetNamedQuery("FillMyTaskObjectDetails.WithoutFacility.ShowAllFalse");
                                query1.SetString(0, UserName);
                                // query1.SetInt32(2, DefaultNoofDays);
                            }
                        }
                        else
                        {
                            if (bShowAll)
                            {
                                query1 = Mysession.GetNamedQuery("FillGenTaskObjectDetails.WithFacility.WithAllAppointments");
                                //query1.SetDateTime(0, DateTime.MinValue);
                                query1.SetString("userName", UserName);
                                query1.SetString("currentOwner", "UNKNOWN");
                                query1.SetString("facilityName", FacName);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillGenTaskObjectDetails.WithFacility.ShowAllFalse");
                                query1.SetString("userName", UserName);
                                query1.SetString("currentOwner", "UNKNOWN");
                                query1.SetString("facilityName", FacName);
                                //query1.SetInt32(2, DefaultNoofDays);
                            }
                        }

                        query1.SetParameterList("ObjList", ObjType);
                        FavoriteList = new ArrayList(query1.List());
                        //ArrayList FavoriteListFinaltask = new ArrayList();
                        //if (FavoriteListTask != null && FavoriteListTask.Count > 0)
                        //{
                        //    Boolean bIsNoMatch = false;

                        //    foreach (object[] val in FavoriteListTask)
                        //    {
                        //        foreach (object[] val1 in FavoriteList)
                        //        {
                        //            if (val[0].ToString() != (val1[0].ToString()))
                        //            {
                        //                bIsNoMatch = true;
                        //            }
                        //            else
                        //            {
                        //                bIsNoMatch = false;
                        //                break;
                        //            }
                        //        }

                        //        if (bIsNoMatch == true || FavoriteList.Count == 0)
                        //            FavoriteListFinaltask.Add(val);
                        //    }
                        //}
                        //FavoriteList.AddRange(FavoriteListFinaltask);
                        FillDTO(FavoriteList, ObjType);
                    }
                    else if (ObjType.Contains("ADDENDUM"))
                    {
                        if (FacName == "ALL")
                        {
                            if (bShowAll)
                            {
                                query1 = Mysession.GetNamedQuery("FillMyAddendumObjectDetails.WithoutFacility.WithAllAppointments");
                                //query1.SetDateTime(0, DateTime.MinValue);
                                query1.SetString("userName", UserName);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyAddendumObjectDetails.WithoutFacility.WithoutAllAppointments");
                                //query1.SetDateTime(0, DateTime.MinValue);
                                query1.SetString("userName", UserName);
                            }
                        }
                        else
                        {
                            if (bShowAll)
                            {
                                query1 = Mysession.GetNamedQuery("FillMyAddendumObjectDetails.WithFacility.WithAllAppointments");
                                //query1.SetDateTime(0, DateTime.MinValue);
                                query1.SetString("userName", UserName);
                                query1.SetString("currentOwner", "UNKNOWN");
                                query1.SetString("facilityName", FacName);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyAddendumObjectDetails.WithFacility.WithoutAllAppointments");
                                //query1.SetDateTime(0, DateTime.MinValue);
                                query1.SetString("userName", UserName);
                                query1.SetString("currentOwner", "UNKNOWN");
                                query1.SetString("facilityName", FacName);
                            }
                        }
                        query1.SetParameterList("ObjList", ObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, ObjType);
                    }
                    else if (ObjType.Contains("DICTATION_RESULT"))
                    {

                        if (FacName == "ALL")
                        {
                            if (bShowAll == true)
                            {
                                query1 = Mysession.GetNamedQuery("FillMyDictationObjectDetails.WithoutFacility.WithAllAppointments");
                                query1.SetString("CurOwner", UserName);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyDictationObjectDetails.WithoutFacility.WithoutAllAppointments");
                                query1.SetString("CurOwner", UserName);
                            }
                        }
                        else
                        {
                            if (bShowAll == true)
                            {
                                query1 = Mysession.GetNamedQuery("FillMyDictationObjectDetails.WithFacility.WithAllAppointments");
                                query1.SetString("UserName", UserName);
                                query1.SetString("CurOwner", "UNKNOWN");
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyDictationObjectDetails.WithFacility.WithoutAllAppointments");
                                query1.SetString("UserName", UserName);
                                query1.SetString("CurOwner", "UNKNOWN");
                            }
                            query1.SetString("FacName", FacName);
                        }

                        query1.SetParameterList("ObjList", ObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, ObjType);
                    }
                    //Added By Srividhya For ACO on 2-Sep-2014
                    else if (ObjType.Contains("CARE_COORDINATE"))
                    {
                        if (FacName == "ALL")
                        {
                            query1 = Mysession.GetNamedQuery("Fill.MyCareCoordinate_ObjectDetails.All");
                        }
                        else if (FacName == "MyQ")//Added for ACO on 06-oct-2014
                        {
                            query1 = Mysession.GetNamedQuery("Fill.MyCareCoordinate_ObjectDetails.MyQ");
                        }
                        else//Added for ACO on 06-oct-2014
                        {
                            query1 = Mysession.GetNamedQuery("Fill.MyCareCoordinate_ObjectDetails.GeneralQ");
                        }

                        query1.SetString(0, UserName);
                        if (ProcessType == "UNASSIGNED")
                        {
                            query1.SetString(1, "UNKNOWN");
                        }
                        else
                        {
                            query1.SetString(1, UserName);
                        }
                        query1.SetParameterList("ObjList", ObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, ObjType);
                    }
                    //Added By Srividhya For ACO on 2-Sep-2014
                    if (ObjType.Contains("DIAGNOSTIC ORDER") || ObjType.Contains("IMAGE ORDER") || ObjType.Contains("DME ORDER"))
                    {
                        string[] myObjType = new string[3];
                        myObjType[0] = "DIAGNOSTIC ORDER";
                        myObjType[1] = "IMAGE ORDER";
                        myObjType[2] = "DME ORDER";
                        StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
                        if (bShowAll == true)
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithoutFacility");
                                query1.SetString(0, UserName);
                                query1.SetString(1, UserName);
                            }
                            else
                            {
                                //if (objStaticLookupMgr.getStaticLookupByFieldName("CMG FACILITY NAME")[0].Value.Trim().ToUpper() != FacName)
                                var facAncillary = from f in NHibernateSessionUtility.Instance.MyAncillaryFacilityList where f.Fac_Name == FacName select f;
                                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                                if (ilstFacAncillary.Count == 0)
                                {
                                    //if (sAncillary.Trim().ToUpper() != FacName)//Bug ID:33975
                                    //{
                                    query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility");
                                    query1.SetString(0, UserName);
                                    query1.SetString(1, "UNKNOWN");
                                    query1.SetString(2, FacName);
                                }
                                else
                                {
                                    query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility.CMG");
                                    query1.SetString(0, UserName);
                                    query1.SetString(1, "UNKNOWN");
                                    query1.SetString(2, FacName);
                                }
                            }
                        }
                        else
                        {

                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithoutFacility.ShowAllFalse");
                                query1.SetString(0, UserName);
                                query1.SetString(1, UserName);

                            }
                            else
                            {
                                var facAncillary = from f in NHibernateSessionUtility.Instance.MyAncillaryFacilityList where f.Fac_Name == FacName select f;
                                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                                if (ilstFacAncillary.Count == 0)
                                {
                                    //if (sAncillary.ToUpper() != FacName)//Bug ID:33975
                                    //{
                                    query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility.ShowAllFalse");
                                    query1.SetString(0, UserName);
                                    query1.SetString(1, "UNKNOWN");
                                    query1.SetString(2, FacName);
                                }
                                else
                                {
                                    query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility.ShowAllFalse.CMG");
                                    query1.SetString(0, UserName);
                                    query1.SetString(1, "UNKNOWN");
                                    query1.SetString(2, FacName);
                                }
                            }
                        }
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, myObjType);

                        //
                        //StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
                        //IList<StaticLookup> stFieldLook = objStaticLookupMgr.getStaticLookupByFieldName("CMG LAB NAME");
                        //  IList<StaticLookup> stFieldLookFacilityName = objStaticLookupMgr.getStaticLookupByFieldName("CMG FACILITY NAME");
                        //if (FacName.ToString().ToUpper().Trim() == sAncillary.ToString().ToUpper().Trim())//Bug ID:33975
                        //{
                        //    if (resultmyqList != null && resultmyqList.Count > 0)
                        //    {
                        //        IList<MyQ> tempList = resultmyqList.Where(a => ((a.Current_Process == "RESULT_PROCESS" && a.EHR_Obj_Type == "DIAGNOSTIC ORDER" && a.CMG_Encounter_ID == 0) || (a.Current_Process == "MA_RESULTS")) && a.Lab_Name.Trim().ToUpper() == "CMG Anc.-1866 #101".Trim().ToUpper()).ToList<MyQ>();
                        //        resultmyqList = tempList;
                        //        //for (int iCount = 0; iCount < tempList.Count; iCount++)
                        //        //{
                        //        //    resultmyqList.RemoveAt(resultmyqList.IndexOf(tempList[iCount]));
                        //        //}
                        //    }
                        //}

                    }
                    //if (ObjType.Contains("INTERNAL DIAGNOSTIC ORDER") || ObjType.Contains("INTERNAL IMAGE ORDER"))
                    //{
                    //    string[] myObjType = new string[2];
                    //    myObjType[0] = "INTERNAL DIAGNOSTIC ORDER";
                    //    myObjType[1] = "INTERNAL IMAGE ORDER";
                    //    if (FacName == "ALL")
                    //    {
                    //        query1 = Mysession.GetNamedQuery("FillMyInternalLabImageOrderObjectDetails.WithoutFacility");
                    //        query1.SetString(0, UserName);
                    //        query1.SetString(1, UserName);
                    //    }
                    //    else
                    //    {
                    //        query1 = Mysession.GetNamedQuery("FillMyInternalLabImageOrderObjectDetails.WithFacility");
                    //        query1.SetString(0, UserName);
                    //        query1.SetString(1, "UNKNOWN");
                    //        query1.SetString(2, FacName);
                    //    }

                    //    query1.SetParameterList("ObjList", myObjType);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, myObjType);
                    //}
                    //if (ObjType.Contains("INTERNAL ORDER"))
                    //{
                    //    string[] myObjType = new string[1];
                    //    myObjType[0] = "INTERNAL ORDER";
                    //    if (bShowAll == true)
                    //    {
                    //        if (FacName == "ALL")
                    //        {
                    //            query1 = Mysession.GetNamedQuery("FillMyInternalOrderObjectDetails.WithoutFacility");
                    //            query1.SetString(0, UserName);
                    //            query1.SetString(1, UserName);
                    //        }
                    //        else
                    //        {
                    //            query1 = Mysession.GetNamedQuery("FillMyInternalOrderObjectDetails.WithFacility");
                    //            query1.SetString(0, UserName);
                    //            query1.SetString(1, "UNKNOWN");
                    //            query1.SetString(2, FacName);

                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (FacName == "ALL")
                    //        {
                    //            query1 = Mysession.GetNamedQuery("FillMyInternalOrderObjectDetails.WithoutFacility.ShowAllFalse");
                    //            query1.SetString(0, UserName);
                    //            query1.SetString(1, UserName);

                    //        }
                    //        else
                    //        {
                    //            query1 = Mysession.GetNamedQuery("FillMyInternalOrderObjectDetails.WithFacility.ShowAllFalse");
                    //            query1.SetString(0, UserName);
                    //            query1.SetString(1, "UNKNOWN");
                    //            query1.SetString(2, FacName);

                    //        }
                    //    }

                    //    query1.SetParameterList("ObjList", myObjType);
                    //    FavoriteList = new ArrayList(query1.List());
                    //    FillDTO(FavoriteList, myObjType);
                    //}

                    if (ObjType.Contains("DME ORDER1"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "DME ORDER";
                        StaticLookupManager objStaticLookupMgr = new StaticLookupManager();
                        if (bShowAll == true)
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithoutFacility");
                                query1.SetString(0, UserName);
                                query1.SetString(1, UserName);
                            }
                            else
                            {
                                //if (objStaticLookupMgr.getStaticLookupByFieldName("CMG FACILITY NAME")[0].Value.Trim().ToUpper() != FacName)
                                var facAncillary = from f in NHibernateSessionUtility.Instance.MyAncillaryFacilityList where f.Fac_Name == FacName select f;
                                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                                if (ilstFacAncillary.Count == 0)
                                {
                                    //if (sAncillary.Trim().ToUpper() != FacName)//Bug ID:33975
                                    //{
                                    query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility");
                                    query1.SetString(0, UserName);
                                    query1.SetString(1, "UNKNOWN");
                                    query1.SetString(2, FacName);
                                }
                                else
                                {
                                    query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility.CMG");
                                    query1.SetString(0, UserName);
                                    query1.SetString(1, "UNKNOWN");
                                    query1.SetString(2, FacName);
                                }
                            }
                        }
                        else
                        {

                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithoutFacility.ShowAllFalse");
                                query1.SetString(0, UserName);
                                query1.SetString(1, UserName);

                            }
                            else
                            {
                                var facAncillary = from f in NHibernateSessionUtility.Instance.MyAncillaryFacilityList where f.Fac_Name == FacName select f;
                                IList<FacilityLibrary> ilstFacAncillary = facAncillary.ToList<FacilityLibrary>();
                                if (ilstFacAncillary.Count == 0)
                                {
                                    //if (sAncillary.ToUpper() != FacName)//Bug ID:33975
                                    //{
                                    query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility.ShowAllFalse");
                                    query1.SetString(0, UserName);
                                    query1.SetString(1, "UNKNOWN");
                                    query1.SetString(2, FacName);
                                }
                                else
                                {
                                    query1 = Mysession.GetNamedQuery("FillMyOrderObjectDetails.WithFacility.ShowAllFalse.CMG");
                                    query1.SetString(0, UserName);
                                    query1.SetString(1, "UNKNOWN");
                                    query1.SetString(2, FacName);
                                }
                            }
                        }
                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, myObjType);
                    }

                    if (ObjType.Contains("IMMUNIZATION ORDER"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "IMMUNIZATION ORDER";
                        if (bShowAll == true)
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyImmunizationOrderObjectDetails.WithoutFacility");
                                query1.SetString(0, UserName);
                                query1.SetString(1, UserName);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyImmunizationOrderObjectDetails.WithFacility");
                                query1.SetString(0, UserName);
                                query1.SetString(1, "UNKNOWN");
                                query1.SetString(2, FacName);
                            }
                        }
                        else
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyImmunizationOrderObjectDetails.WithoutFacility.ShowAllFalse");
                                query1.SetString(0, UserName);
                                query1.SetString(1, UserName);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyImmunizationOrderObjectDetails.WithFacility.ShowAllFalse");
                                query1.SetString(0, UserName);
                                query1.SetString(1, "UNKNOWN");
                                query1.SetString(2, FacName);

                            }
                        }

                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, myObjType);
                    }
                    if (ObjType.Contains("REFERRAL ORDER"))
                    {
                        string[] myObjType = new string[1];
                        myObjType[0] = "REFERRAL ORDER";
                        if (bShowAll == true)
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyReferralOrderObjectDetails.WithoutFacility");
                                query1.SetString(0, UserName);
                                query1.SetString(1, UserName);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyReferralOrderObjectDetails.WithFacility");
                                query1.SetString(0, UserName);
                                query1.SetString(1, "UNKNOWN");
                                query1.SetString(2, FacName);
                            }
                        }
                        else
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyReferralOrderObjectDetails.WithoutFacility.ShowAllFalse");
                                query1.SetString(0, UserName);
                                query1.SetString(1, UserName);

                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyReferralOrderObjectDetails.WithFacility.ShowAllFalse");
                                query1.SetString(0, UserName);
                                query1.SetString(1, "UNKNOWN");
                                query1.SetString(2, FacName);
                            }
                        }

                        query1.SetParameterList("ObjList", myObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, myObjType);
                    }
                    if (ObjType.Contains("E-PRESCRIBE"))
                    {
                        if (bShowAll == true)
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyE-PrescriptionObjectDetails.WithoutFacility");
                                query1.SetString(0, UserName);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyE-PrescriptionObjectDetails.WithFacility");
                                query1.SetString(0, UserName);
                                query1.SetString(1, "UNKNOWN");
                                query1.SetString(2, FacName);
                            }
                        }
                        else
                        {
                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyE-PrescriptionObjectDetails.WithoutFacility.ShowAllFalse");
                                query1.SetString(0, UserName);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyE-PrescriptionObjectDetails.WithFacility.ShowAllFalse");
                                query1.SetString(0, UserName);
                                query1.SetString(1, "UNKNOWN");
                                query1.SetString(2, FacName);

                            }
                        }

                        query1.SetParameterList("ObjList", ObjType);
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, ObjType);
                    }
                    if (ObjType.Contains("SCAN"))// || ObjType.Contains("SCAN RESULT"))
                    {
                        if (bShowAll)
                        {

                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyScanObjectDetails.WithoutFacility");
                                query1.SetString(0, UserName);
                                query1.SetString(1, ObjType[0].ToString());
                                //query1.SetString(2, UserName);
                                //query1.SetString(3, ObjType[1].ToString());
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyScanObjectDetails.WithFacility");
                                //query1.SetString(0, UserName);
                                //if (ProcessType == "UNASSIGNED")
                                //    query1.SetString(1, "UNKNOWN");
                                //else
                                //    query1.SetString(1, UserName);

                                //query1.SetString(2, FacName);
                                //query1.SetString(3, ObjType[0].ToString());
                                //// query1.SetString(4, UserName);
                                ////if (ProcessType == "UNASSIGNED")
                                ////    query1.SetString(5, "UNKNOWN");
                                ////else
                                ////    query1.SetString(5, UserName);
                                ////query1.SetString(6, FacName);
                                ////query1.SetString(7, ObjType[1].ToString());
                                
                                if (ProcessType == "UNASSIGNED")
                                    query1.SetString(0, "UNKNOWN");
                                else
                                    query1.SetString(0, UserName);
                                query1.SetString(1, ObjType[0].ToString());
                                query1.SetString(2, FacName);
                                query1.SetString(3, UserName);
                                // query1.SetString(4, UserName);
                                //if (ProcessType == "UNASSIGNED")
                                //    query1.SetString(5, "UNKNOWN");
                                //else
                                //    query1.SetString(5, UserName);
                                //query1.SetString(6, FacName);
                                //query1.SetString(7, ObjType[1].ToString());

                            }
                        }
                        else
                        {

                            if (FacName == "ALL")
                            {
                                query1 = Mysession.GetNamedQuery("FillMyScanObjectDetails.WithoutFacility.No_ofdays");
                                query1.SetString(0, UserName);
                                query1.SetString(1, ObjType[0].ToString());
                                // query1.SetString(2, UserName);
                                //  query1.SetString(3, ObjType[1].ToString());
                                //query1.SetInt32(6, DefaultNoofDays);
                            }
                            else
                            {
                                query1 = Mysession.GetNamedQuery("FillMyScanObjectDetails.WithFacility.No_ofdays");
                                query1.SetString(0, UserName);
                                if (ProcessType == "UNASSIGNED")
                                    query1.SetString(1, "UNKNOWN");
                                else
                                    query1.SetString(1, UserName);

                                query1.SetString(2, FacName);
                                query1.SetString(3, ObjType[0].ToString());
                                //query1.SetInt32(4, DefaultNoofDays);
                                //query1.SetString(4, UserName);
                                //if (ProcessType == "UNASSIGNED")
                                //    query1.SetString(5, "UNKNOWN");
                                //else
                                //    query1.SetString(5, UserName);
                                //query1.SetString(6, FacName);
                                //query1.SetString(7, ObjType[1].ToString());
                                //query1.SetInt32(8, DefaultNoofDays);

                            }



                        }

                        //>>>>>>> 1.48.2.7.2.1
                        FavoriteList = new ArrayList(query1.List());
                        FillDTO(FavoriteList, ObjType);
                    }


                    if (ObjType.Contains("DIAGNOSTIC_RESULT"))
                    {
                        if (bShowAll)
                        {
                            string[] myObjType = new string[1];
                            myObjType[0] = "DIAGNOSTIC_RESULT";
                            query1 = Mysession.GetNamedQuery("FillMyResultsObjectDetails.ShowAllFalse");
                            query1.SetString(0, "DIAGNOSTIC_RESULT");
                            query1.SetString(1, UserName);
                            FavoriteList = new ArrayList(query1.List());
                            FillDTO(FavoriteList, myObjType);
                        }
                        else
                        {
                            string[] myObjType = new string[1];
                            myObjType[0] = "DIAGNOSTIC_RESULT";
                            query1 = Mysession.GetNamedQuery("FillMyResultsObjectDetails");
                            query1.SetString(0, "DIAGNOSTIC_RESULT");
                            query1.SetString(1, UserName);
                            FavoriteList = new ArrayList(query1.List());
                            FillDTO(FavoriteList, myObjType);
                        }
                    }
                }
                catch (Exception ex)
                {
                    String sException = string.Empty;
                    if (ex.StackTrace != null)
                        sException = ex.StackTrace.ToString();
                    if (ex.Message != null)
                        sException += Environment.NewLine + ex.Message.ToString();
                    if (ex.InnerException != null && ex.InnerException.Message != null)
                        sException += Environment.NewLine + ex.InnerException.Message.ToString();
                    System.IO.StreamWriter sw = null;
                    try
                    {
                        string path = "C:/Exception_Logs/Exception_Log_File.txt";
                        sw = System.IO.File.AppendText(path);
                        string logLine = System.String.Format(
                            "{0:G}: {1}.", System.DateTime.Now, sException);
                        sw.WriteLine(logLine);
                    }
                    finally
                    {
                        sw.Close();
                    }

                    throw new Exception(sException);
                }
                finally
                {
                    Mysession.Close();
                }
            }
            return resultmyqList;
        }
    }

    [DataContract]
    public partial class Queue_List
    {
        #region Declaration
        private string _Fac_Name = string.Empty;
        private string _Current_Owner = string.Empty;
        private string _Obj_Type = string.Empty;
        #endregion

        #region GetHashValue

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(this.GetType().FullName);
            sb.Append(_Fac_Name);
            sb.Append(_Current_Owner);
            sb.Append(_Obj_Type);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties
        [DataMember]
        public virtual string Fac_Name
        {
            get { return _Fac_Name; }
            set { _Fac_Name = value; }
        }
        [DataMember]
        public virtual string Current_Owner
        {
            get { return _Current_Owner; }
            set { _Current_Owner = value; }
        }
        [DataMember]
        public virtual string Obj_Type
        {
            get { return _Obj_Type; }
            set { _Obj_Type = value; }
        }
        #endregion
    }
}
