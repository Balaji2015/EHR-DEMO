using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.Linq;
using System.Xml;
using System.Reflection;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IRcopia_MedicationManager : IManagerBase<Rcopia_Medication, ulong>
    {
        ulong AppendToRcopia_Medication(Rcopia_Medication objMedication, string MACAddress);
        Rcopia_Medication UpdateToRcopia_Medication(Rcopia_Medication objMedication, string MACAddress);
        IList<Rcopia_Medication> GetRcopiaMedicationRecords(IList<ulong> ulRcopia_ID);
        IList<Rcopia_Medication> GetRCopiaMedByHumanID(ulong ulHumanID);
        void InsertOrUpdateMedication(IList<Rcopia_Medication> ilstMedication, string sUserName, string MACAddress, DateTime dtClientDate, string sFacilityName, ulong EncID);
        IList<Rcopia_Medication> GetRxNormByHumanID(ulong ulHumanID);
        IList<Rcopia_Medication> GetRmedicationByHumanID(string ulHumanID);
        IList<Rcopia_Medication> GetMedicationByHumanID(ulong ulHumanID);
        IList<Rcopia_Medication> GetMedicationWithExactDuplicates(ulong ulHumanID, string sShowall);
        IList<Rcopia_Medication> GetMedicationWithPartialDuplicates(ulong ulHumanID, string sStatus);
    }

    public partial class Rcopia_MedicationManager : ManagerBase<Rcopia_Medication, ulong>, IRcopia_MedicationManager
    {
        #region Constructors

        public Rcopia_MedicationManager()
            : base()
        {

        }
        public Rcopia_MedicationManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods

        //public void InsertOrUpdateMedication(IList<Rcopia_Medication> ilstMedication, string sUserName, string MACAddress, DateTime dtClientDate, string sFacilityName, ulong EncID)
        //{
        //    if (ilstMedication.Count > 0)
        //    {
        //        IList<Rcopia_Medication> Rcopia_MedicationList = new List<Rcopia_Medication>();
        //        IList<Rcopia_Medication> Rcopia_MedicationUpdateList = new List<Rcopia_Medication>();

        //        for (int i = 0; i < ilstMedication.Count; i++)
        //        {
        //            if (ilstMedication[i].Human_ID != 0)
        //            {
        //                Rcopia_Medication objMedication = GetRcopiaMedicationRecords(ilstMedication[i].Id);
        //                if (objMedication == null)
        //                {
        //                    if (Rcopia_MedicationList.Count > 0)
        //                    {
        //                        if (!Rcopia_MedicationList.Any(item => item.Id == ilstMedication[i].Id))
        //                        {
        //                            if (Rcopia_MedicationUpdateList.Count > 0)
        //                            {
        //                                if (!Rcopia_MedicationUpdateList.Any(item => item.Id == ilstMedication[i].Id))
        //                                {
        //                                    //ilstMedication[i].Created_Date_And_Time = DateTime.Now;
        //                                    //ilstMedication[i].Created_Date_And_Time = DateTime.UtcNow;
        //                                    ilstMedication[i].Created_Date_And_Time = dtClientDate;
        //                                    ilstMedication[i].Created_By = sUserName;
        //                                    ilstMedication[i].Facility_Name = sFacilityName;
        //                                    ilstMedication[i].Encounter_ID = EncID;
        //                                    ilstMedication[i].Status = "ACTIVE";
        //                                    Rcopia_MedicationList.Add(ilstMedication[i]);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                //ilstMedication[i].Created_Date_And_Time = DateTime.Now;
        //                                //ilstMedication[i].Created_Date_And_Time = DateTime.UtcNow;
        //                                ilstMedication[i].Created_Date_And_Time = dtClientDate;
        //                                ilstMedication[i].Created_By = sUserName;
        //                                ilstMedication[i].Facility_Name = sFacilityName;
        //                                ilstMedication[i].Encounter_ID = EncID;
        //                                ilstMedication[i].Status = "ACTIVE";

        //                                Rcopia_MedicationList.Add(ilstMedication[i]);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //ilstMedication[i].Created_Date_And_Time = DateTime.Now;
        //                        //ilstMedication[i].Created_Date_And_Time = DateTime.UtcNow;
        //                        ilstMedication[i].Created_Date_And_Time = dtClientDate;
        //                        ilstMedication[i].Created_By = sUserName;
        //                        ilstMedication[i].Facility_Name = sFacilityName;
        //                        ilstMedication[i].Encounter_ID = EncID;
        //                        ilstMedication[i].Status = "ACTIVE";
        //                        Rcopia_MedicationList.Add(ilstMedication[i]);
        //                    }
        //                }
        //                else
        //                {
        //                    //objMedication.Modified_Date_And_Time = DateTime.Now;
        //                    //objMedication.Modified_Date_And_Time = DateTime.UtcNow;
        //                    objMedication.Modified_Date_And_Time = dtClientDate;
        //                    objMedication.Modified_By = sUserName;
        //                    objMedication.Facility_Name = sFacilityName;
        //                    objMedication.Encounter_ID = EncID;
        //                    ilstMedication[i].Status = "ACTIVE";
        //                    Rcopia_Medication objUpdateMedication = UpdateMedicationObject(objMedication, ilstMedication[i]);
        //                    Rcopia_MedicationUpdateList.Add(objUpdateMedication);
        //                }
        //            }
        //        }
        //        ulong EncounterORHumanId = 0;
        //        if (Rcopia_MedicationList.Count > 0)
        //            EncounterORHumanId = Rcopia_MedicationList[0].Human_ID;
        //        else
        //            EncounterORHumanId = Rcopia_MedicationUpdateList[0].Human_ID;
        //        //SaveUpdateDelete_DBAndXML_WithTransaction(ref Rcopia_MedicationList, ref Rcopia_MedicationUpdateList, null, MACAddress, true, true, EncounterORHumanId, string.Empty);
        //        SaveUpdateDeleteWithTransaction(ref Rcopia_MedicationList, Rcopia_MedicationUpdateList, null, MACAddress);

        //            var newList = Rcopia_MedicationList.Concat(Rcopia_MedicationUpdateList);
        //            IList<Rcopia_Medication> combinedList = newList.ToList<Rcopia_Medication>();

        //            var query = combinedList.GroupBy(p => p.Human_ID).Select(g => g.First()).ToList();
        //            IList<Rcopia_Medication> MyRCopiaList = query.ToList<Rcopia_Medication>();

        //            for (int iCount = 0; iCount <= MyRCopiaList.Count - 1; iCount++)
        //            {
        //                IList<Rcopia_Medication> rcopiaMedList = GetMedicationByHumanID(MyRCopiaList[iCount].Human_ID);
        //                GenerateXml XMLObj = new GenerateXml();

        //                ulong uHuman_id = MyRCopiaList[iCount].Human_ID;
        //                List<object> lstObj = new List<object>();

        //                for (int i = 0; i < rcopiaMedList.Count; i++)
        //                {
        //                    lstObj.Add(rcopiaMedList[i]);
        //                    lstObj = lstObj.Cast<object>().ToList();
        //                }
        //                if (rcopiaMedList.Count>0)
        //                    XMLObj.GenerateXmlSaveRCopia(lstObj, uHuman_id, string.Empty, true, false, false, true);
        //            }


        //        //if (Rcopia_MedicationList.Count > 0)
        //        //{
        //        //    for (int i = 0; i < Rcopia_MedicationList.Count; i++)
        //        //    {
        //        //        ulong uHuman_id = Rcopia_MedicationList[i].Human_ID;
        //        //        List<object> lstObj = new List<object>();
        //        //        lstObj.Add(Rcopia_MedicationList[i]);
        //        //        lstObj = lstObj.Cast<object>().ToList();
        //        //        XMLObj.GenerateXmlSaveStatic(lstObj, uHuman_id, string.Empty);
        //        //    }
        //        //}
        //        //if (Rcopia_MedicationUpdateList.Count > 0)
        //        //{
        //        //    for (int i = 0; i < Rcopia_MedicationUpdateList.Count; i++)
        //        //    {
        //        //        ulong uHuman_id = Rcopia_MedicationUpdateList[i].Human_ID;
        //        //        List<object> lstObj = new List<object>();
        //        //        lstObj.Add(Rcopia_MedicationUpdateList[i]);
        //        //        lstObj = lstObj.Cast<object>().ToList();
        //        //        XMLObj.GenerateXmlUpdate(lstObj, uHuman_id, string.Empty);
        //        //    }
        //        //}
        //    }
        //}

        public void InsertOrUpdateMedication(IList<Rcopia_Medication> ilstMedication, string sUserName, string MACAddress, DateTime dtClientDate, string sFacilityName, ulong EncID)
        {
            if (ilstMedication.Count > 0)
            {
                IList<Rcopia_Medication> Rcopia_MedicationList = new List<Rcopia_Medication>();
                IList<Rcopia_Medication> Rcopia_MedicationUpdateList = new List<Rcopia_Medication>();

                List<ulong> RcopiaID = ilstMedication.Select(x => x.Id).ToList<ulong>();

                IList<Rcopia_Medication> lstMedication = GetRcopiaMedicationRecords(RcopiaID);

                Rcopia_Medication objMedication = null;

                for (int i = 0; i < ilstMedication.Count; i++)
                {
                    if (ilstMedication[i].Human_ID != 0)
                    {
                        IList<Rcopia_Medication> med = (from m in lstMedication where m.Id == ilstMedication[i].Id select m).ToList<Rcopia_Medication>();
                        if (med != null && med.Count > 0)
                            objMedication = (Rcopia_Medication)med[0];
                        else
                            objMedication = null;
                        if (objMedication == null)
                        {
                            if (Rcopia_MedicationList.Count > 0)
                            {
                                if (!Rcopia_MedicationList.Any(item => item.Id == ilstMedication[i].Id))
                                {
                                    if (Rcopia_MedicationUpdateList.Count > 0)
                                    {
                                        if (!Rcopia_MedicationUpdateList.Any(item => item.Id == ilstMedication[i].Id))
                                        {
                                            //ilstMedication[i].Created_Date_And_Time = DateTime.Now;
                                            //ilstMedication[i].Created_Date_And_Time = DateTime.UtcNow;
                                            ilstMedication[i].Created_Date_And_Time = dtClientDate;
                                            ilstMedication[i].Created_By = sUserName;
                                            ilstMedication[i].Facility_Name = sFacilityName;
                                            ilstMedication[i].Encounter_ID = EncID;
                                            ilstMedication[i].Status = "ACTIVE";
                                            Rcopia_MedicationList.Add(ilstMedication[i]);
                                        }
                                    }
                                    else
                                    {
                                        //ilstMedication[i].Created_Date_And_Time = DateTime.Now;
                                        //ilstMedication[i].Created_Date_And_Time = DateTime.UtcNow;
                                        ilstMedication[i].Created_Date_And_Time = dtClientDate;
                                        ilstMedication[i].Created_By = sUserName;
                                        ilstMedication[i].Facility_Name = sFacilityName;
                                        ilstMedication[i].Encounter_ID = EncID;
                                        ilstMedication[i].Status = "ACTIVE";

                                        Rcopia_MedicationList.Add(ilstMedication[i]);
                                    }
                                }
                            }
                            else
                            {
                                //ilstMedication[i].Created_Date_And_Time = DateTime.Now;
                                //ilstMedication[i].Created_Date_And_Time = DateTime.UtcNow;
                                ilstMedication[i].Created_Date_And_Time = dtClientDate;
                                ilstMedication[i].Created_By = sUserName;
                                ilstMedication[i].Facility_Name = sFacilityName;
                                ilstMedication[i].Encounter_ID = EncID;
                                ilstMedication[i].Status = "ACTIVE";
                                Rcopia_MedicationList.Add(ilstMedication[i]);
                            }
                        }
                        else
                        {
                            //objMedication.Modified_Date_And_Time = DateTime.Now;
                            //objMedication.Modified_Date_And_Time = DateTime.UtcNow;
                            objMedication.Modified_Date_And_Time = dtClientDate;
                            objMedication.Modified_By = sUserName;
                            objMedication.Facility_Name = sFacilityName;
                            objMedication.Encounter_ID = EncID;
                            ilstMedication[i].Status = "ACTIVE";
                            Rcopia_Medication objUpdateMedication = UpdateMedicationObject(objMedication, ilstMedication[i]);
                            Rcopia_MedicationUpdateList.Add(objUpdateMedication);
                        }
                    }
                }
                ulong EncounterORHumanId = 0;
                if (Rcopia_MedicationList.Count > 0)
                    EncounterORHumanId = Rcopia_MedicationList[0].Human_ID;
                else if (Rcopia_MedicationUpdateList.Count > 0)
                    EncounterORHumanId = Rcopia_MedicationUpdateList[0].Human_ID;
                if (EncounterORHumanId != 0)
                    SaveUpdateDelete_DBAndXML_WithTransaction(ref Rcopia_MedicationList, ref Rcopia_MedicationUpdateList, null, MACAddress, true, true, EncounterORHumanId, string.Empty);
                //SaveUpdateDeleteWithTransaction(ref Rcopia_MedicationList, Rcopia_MedicationUpdateList, null, MACAddress);

                //var newList = Rcopia_MedicationList.Concat(Rcopia_MedicationUpdateList);
                //IList<Rcopia_Medication> combinedList = newList.ToList<Rcopia_Medication>();

                //var query = combinedList.GroupBy(p => p.Human_ID).Select(g => g.First()).ToList();
                //IList<Rcopia_Medication> MyRCopiaList = query.ToList<Rcopia_Medication>();

                //for (int iCount = 0; iCount <= MyRCopiaList.Count - 1; iCount++)
                //{
                //    IList<Rcopia_Medication> rcopiaMedList = GetMedicationByHumanID(MyRCopiaList[iCount].Human_ID);
                //    GenerateXml XMLObj = new GenerateXml();

                //    ulong uHuman_id = MyRCopiaList[iCount].Human_ID;
                //    List<object> lstObj = new List<object>();

                //    for (int i = 0; i < rcopiaMedList.Count; i++)
                //    {
                //        lstObj.Add(rcopiaMedList[i]);
                //        lstObj = lstObj.Cast<object>().ToList();
                //    }
                //    if (rcopiaMedList.Count > 0)
                //        XMLObj.GenerateXmlSaveRCopia(lstObj, uHuman_id, string.Empty, true, false, false, true);
                //}


                //if (Rcopia_MedicationList.Count > 0)
                //{
                //    for (int i = 0; i < Rcopia_MedicationList.Count; i++)
                //    {
                //        ulong uHuman_id = Rcopia_MedicationList[i].Human_ID;
                //        List<object> lstObj = new List<object>();
                //        lstObj.Add(Rcopia_MedicationList[i]);
                //        lstObj = lstObj.Cast<object>().ToList();
                //        XMLObj.GenerateXmlSaveStatic(lstObj, uHuman_id, string.Empty);
                //    }
                //}
                //if (Rcopia_MedicationUpdateList.Count > 0)
                //{
                //    for (int i = 0; i < Rcopia_MedicationUpdateList.Count; i++)
                //    {
                //        ulong uHuman_id = Rcopia_MedicationUpdateList[i].Human_ID;
                //        List<object> lstObj = new List<object>();
                //        lstObj.Add(Rcopia_MedicationUpdateList[i]);
                //        lstObj = lstObj.Cast<object>().ToList();
                //        XMLObj.GenerateXmlUpdate(lstObj, uHuman_id, string.Empty);
                //    }
                //}
            }
        }



        public ulong AppendToRcopia_Medication(Rcopia_Medication objMedication, string MACAddress)
        {
            IList<Rcopia_Medication> Rcopia_MedicationList = new List<Rcopia_Medication>();
            Rcopia_MedicationList.Add(objMedication);
            // SaveUpdateDeleteWithTransaction(ref Rcopia_MedicationList, null, null, MACAddress);
            return Rcopia_MedicationList[0].Id;
        }

        public Rcopia_Medication UpdateToRcopia_Medication(Rcopia_Medication objMedication, string MACAddress)
        {
            IList<Rcopia_Medication> Rcopia_MedicationList = new List<Rcopia_Medication>();
            Rcopia_MedicationList.Add(objMedication);
            IList<Rcopia_Medication> Rcopia_MedicationListadd = null;
            SaveUpdateDeleteWithTransaction(ref Rcopia_MedicationListadd, Rcopia_MedicationList, null, MACAddress);
            //Rcopia_Medication ReturnMedication = GetRcopiaMedicationRecords(objMedication.Id);
            return Rcopia_MedicationList[0];
        }

        public IList<Rcopia_Medication> GetRcopiaMedicationRecords(IList<ulong> ulRcopia_ID)
        {
            IList<Rcopia_Medication> objMedication = new List<Rcopia_Medication>();
            //ISession mySession = NHibernateSessionManager.Instance.CreateISession();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(Rcopia_Medication)).Add(Expression.In("Id", ulRcopia_ID.ToArray()));
                if (crit.List<Rcopia_Medication>().Count > 0)
                {
                    objMedication = crit.List<Rcopia_Medication>();
                }
                mySession.Close();
            }
            return objMedication;
        }

        public Rcopia_Medication UpdateMedicationObject(Rcopia_Medication objMedication, Rcopia_Medication objUpdateMedication)
        {
            //Jira CAP-2281 - Add Human_id
            objMedication.Human_ID = objUpdateMedication.Human_ID;
            objMedication.Action = objUpdateMedication.Action;
            objMedication.Brand_Name = objUpdateMedication.Brand_Name;
            objMedication.Comments = objUpdateMedication.Comments;
            objMedication.Dose = objUpdateMedication.Dose;
            objMedication.Dose_Other = objUpdateMedication.Dose_Other;
            objMedication.Dose_Timing = objUpdateMedication.Dose_Timing;
            objMedication.Dose_Unit = objUpdateMedication.Dose_Unit;
            objMedication.Drug = objUpdateMedication.Drug;
            objMedication.Duration = objUpdateMedication.Duration;
            objMedication.Fill_Date = objUpdateMedication.Fill_Date;
            objMedication.First_DataBank_Med_ID = objUpdateMedication.First_DataBank_Med_ID;
            objMedication.Form = objUpdateMedication.Form;
            objMedication.Generic_Name = objUpdateMedication.Generic_Name;
            objMedication.Height = objUpdateMedication.Height;
            objMedication.Intended_Use = objUpdateMedication.Intended_Use;
            objMedication.Last_Modified_By = objUpdateMedication.Last_Modified_By;
            objMedication.Last_Modified_Date = objUpdateMedication.Last_Modified_Date;
            objMedication.NDC_ID = objUpdateMedication.NDC_ID;
            objMedication.Other_Notes = objUpdateMedication.Other_Notes;
            objMedication.Patient_Notes = objUpdateMedication.Patient_Notes;
            objMedication.Quantity = objUpdateMedication.Quantity;
            objMedication.Quantity_Unit = objUpdateMedication.Quantity_Unit;
            objMedication.Refills = objUpdateMedication.Refills;
            objMedication.Route = objUpdateMedication.Route;
            objMedication.Sig_Changed_Date = objUpdateMedication.Sig_Changed_Date;
            objMedication.Start_Date = objUpdateMedication.Start_Date;
            objMedication.Stop_Date = objUpdateMedication.Stop_Date;
            objMedication.Stop_Reason = objUpdateMedication.Stop_Reason;
            objMedication.Strength = objUpdateMedication.Strength;
            objMedication.Substitution_Permitted = objUpdateMedication.Substitution_Permitted;
            objMedication.Weight = objUpdateMedication.Weight;
            objMedication.Medication_Order = objUpdateMedication.Medication_Order;
            objMedication.ICD_Code = objUpdateMedication.ICD_Code;
            objMedication.ICD_Code_Description = objUpdateMedication.ICD_Code_Description;
            objMedication.Deleted = objUpdateMedication.Deleted;

            objMedication.Pharmacy_Address1 = objUpdateMedication.Pharmacy_Address1;
            objMedication.Pharmacy_Address2 = objUpdateMedication.Pharmacy_Address2;
            objMedication.Pharmacy_City = objUpdateMedication.Pharmacy_City;
            objMedication.Pharmacy_CrossStreet = objUpdateMedication.Pharmacy_CrossStreet;
            objMedication.Pharmacy_Electronic = objUpdateMedication.Pharmacy_Electronic;
            objMedication.Pharmacy_Fax = objUpdateMedication.Pharmacy_Fax;
            objMedication.Pharmacy_Is24Hour = objUpdateMedication.Pharmacy_Is24Hour;
            objMedication.Pharmacy_Level3 = objUpdateMedication.Pharmacy_Level3;
            objMedication.Pharmacy_Name = objUpdateMedication.Pharmacy_Name;
            objMedication.Pharmacy_Phone = objUpdateMedication.Pharmacy_Phone;
            objMedication.Pharmacy_State = objUpdateMedication.Pharmacy_State;
            objMedication.Pharmacy_Zip = objUpdateMedication.Pharmacy_Zip;

            return objMedication;
        }

        public IList<Rcopia_Medication> GetRCopiaMedByHumanID(ulong ulHumanID)
        {
            IList<Rcopia_Medication> rcopiaMedicationList = new List<Rcopia_Medication>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession mySession = NHibernateSessionManager.Instance.CreateISession();
                //  ISQLQuery sql = session.GetISession().CreateSQLQuery("select m.* from rcopia_medication m where m.Human_ID='"+ulHumanID+"' group by m.last_modified_date").AddEntity("m",typeof(Rcopia_Medication));
                ICriteria crit = mySession.CreateCriteria(typeof(Rcopia_Medication)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Deleted", "N"));
                rcopiaMedicationList = crit.List<Rcopia_Medication>();
                mySession.Close();
            }
            return rcopiaMedicationList;
        }

        public IList<Rcopia_Medication> GetRxNormByHumanID(ulong ulHumanID)
        {
            IList<Rcopia_Medication> rcopiaMedicationList = new List<Rcopia_Medication>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession mySession = NHibernateSessionManager.Instance.CreateISession();
                ISQLQuery sql = session.GetISession().CreateSQLQuery("select m.* from rcopia_medication m where m.Human_ID='" + ulHumanID + "' and Deleted='N' group by m.Rxnorm_ID").AddEntity("m", typeof(Rcopia_Medication));
                // ICriteria crit = mySession.CreateCriteria(typeof(Rcopia_Medication)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Deleted", "N"));
                rcopiaMedicationList = sql.List<Rcopia_Medication>();
                mySession.Close();
            }
            return rcopiaMedicationList;
        }

        public IList<Rcopia_Medication> GetRmedicationByHumanID(string ulHumanID)
        {
            IList<Rcopia_Medication> rcopiaMedicationList = new List<Rcopia_Medication>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession mySession = NHibernateSessionManager.Instance.CreateISession();
                ISQLQuery sql = session.GetISession().CreateSQLQuery("select m.* from rcopia_medication m where m.Human_ID in ('" + ulHumanID + "')").AddEntity("m", typeof(Rcopia_Medication));
                // ICriteria crit = mySession.CreateCriteria(typeof(Rcopia_Medication)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Deleted", "N"));
                rcopiaMedicationList = sql.List<Rcopia_Medication>();
                mySession.Close();
            }
            return rcopiaMedicationList;
        }
        public IList<Rcopia_Medication> GetMedicationByHumanID(ulong ulHumanID)
        {
            IList<Rcopia_Medication> rcopiaMedicationList = new List<Rcopia_Medication>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = mySession.CreateCriteria(typeof(Rcopia_Medication)).Add(Expression.Eq("Human_ID", ulHumanID));
                rcopiaMedicationList = crit.List<Rcopia_Medication>();
                mySession.Close();
            }
            return rcopiaMedicationList;
        }
        public IList<Rcopia_Medication> GetMaxID()
        {
            IList<Rcopia_Medication> rcopiaMedicationList = new List<Rcopia_Medication>();
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISession mySession = NHibernateSessionManager.Instance.CreateISession();
                ISQLQuery sql = session.GetISession().CreateSQLQuery("select m.* from rcopia_medication m order by rcopia_id desc limit 1 ").AddEntity("m", typeof(Rcopia_Medication));
                // ICriteria crit = mySession.CreateCriteria(typeof(Rcopia_Medication)).Add(Expression.Eq("Human_ID", ulHumanID)).Add(Expression.Eq("Deleted", "N"));
                rcopiaMedicationList = sql.List<Rcopia_Medication>();
                mySession.Close();
            }
            return rcopiaMedicationList;
        }
        #endregion

        public IList<Rcopia_Medication> GetMedicationWithExactDuplicates(ulong ulHumanID, string sShowall)
        {
            IList<Rcopia_Medication> rcopiaMedicationList = new List<Rcopia_Medication>();
            IList<Rcopia_Medication> ilstrcopiaMedicationReturn = new List<Rcopia_Medication>();
            DateTime sTodayDate = DateTime.Now.Date;
            string sMedicationStatus = "";
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ISQLQuery sql = session.GetISession().CreateSQLQuery("select * from rcopia_medication as a where Human_ID=" + ulHumanID + " and Deleted <> 'Y' order by Brand_Name,Strength,Form,Dose,Dose_Unit,Route,Dose_Timing,Start_Date,Stop_Date,Patient_Notes,Other_Notes;").AddEntity("m", typeof(Rcopia_Medication));

                rcopiaMedicationList = sql.List<Rcopia_Medication>();
                mySession.Close();
            }
            for (int iCount = 0; iCount < rcopiaMedicationList.Count; iCount++)
            {
                //Get duplicate count
                IList<Rcopia_Medication> ilstMedicationDuplicateSet = ilstrcopiaMedicationReturn.Where(x => x.Brand_Name == rcopiaMedicationList[iCount].Brand_Name
                && x.Strength == rcopiaMedicationList[iCount].Strength
                && x.Form == rcopiaMedicationList[iCount].Form
                && x.Dose == rcopiaMedicationList[iCount].Dose
                && x.Dose_Unit == rcopiaMedicationList[iCount].Dose_Unit
                && x.Route == rcopiaMedicationList[iCount].Route
                && x.Dose_Timing == rcopiaMedicationList[iCount].Dose_Timing
                && x.Start_Date == rcopiaMedicationList[iCount].Start_Date
                && x.Stop_Date == rcopiaMedicationList[iCount].Stop_Date
                && x.Patient_Notes == rcopiaMedicationList[iCount].Patient_Notes
                && x.Other_Notes == rcopiaMedicationList[iCount].Other_Notes).ToList<Rcopia_Medication>();

                int iDuplicateSetCount = ilstMedicationDuplicateSet.Count();

                //get duplicate medications
                if (iDuplicateSetCount == 0)
                {

                    IList<Rcopia_Medication> ilstCountOfrcopiaMedication = rcopiaMedicationList.Where(x => x.Brand_Name == rcopiaMedicationList[iCount].Brand_Name
                    && x.Strength == rcopiaMedicationList[iCount].Strength
                    && x.Form == rcopiaMedicationList[iCount].Form
                    && x.Dose == rcopiaMedicationList[iCount].Dose
                    && x.Dose_Unit == rcopiaMedicationList[iCount].Dose_Unit
                    && x.Route == rcopiaMedicationList[iCount].Route
                    && x.Dose_Timing == rcopiaMedicationList[iCount].Dose_Timing
                    && x.Start_Date == rcopiaMedicationList[iCount].Start_Date
                    && x.Stop_Date == rcopiaMedicationList[iCount].Stop_Date
                    && x.Patient_Notes == rcopiaMedicationList[iCount].Patient_Notes
                    && x.Other_Notes == rcopiaMedicationList[iCount].Other_Notes).ToList<Rcopia_Medication>();

                    ilstCountOfrcopiaMedication = SeparateMedicationByStatus(ilstCountOfrcopiaMedication, sShowall.ToUpper());
                    
                    //Sorting
                    ilstCountOfrcopiaMedication = ilstCountOfrcopiaMedication.OrderBy(x => x.Status).ThenByDescending(y => y.Id).ToList<Rcopia_Medication>();

                    if (ilstCountOfrcopiaMedication.Count > 1)
                    {
                        ilstCountOfrcopiaMedication.RemoveAt(0);
                        ilstrcopiaMedicationReturn = ilstrcopiaMedicationReturn.Concat(ilstCountOfrcopiaMedication).ToList<Rcopia_Medication>();
                    }
                }

            }

            return ilstrcopiaMedicationReturn;
        }

        public IList<Rcopia_Medication> GetMedicationWithPartialDuplicates(ulong ulHumanID, string sStatus)
        {
            IList<Rcopia_Medication> rcopiaMedicationList = new List<Rcopia_Medication>();
            IList<Rcopia_Medication> ilstrcopiaMedicationReturn = new List<Rcopia_Medication>();
            Rcopia_Medication rcopia_Medication = new Rcopia_Medication();
            DateTime sTodayDate = DateTime.Now.Date;
            string sMedicationStatus = "";
            using (ISession mySession = NHibernateSessionManager.Instance.CreateISession())
            {
                //ISQLQuery sql = session.GetISession().CreateSQLQuery("select * from rcopia_medication as a where Human_ID=" + ulHumanID + " and Deleted <> 'Y' group by Brand_Name,Strength,Form,Dose,Dose_Unit,Route,Dose_Timing,Start_Date,Stop_Date,Patient_Notes,Other_Notes;").AddEntity("m", typeof(Rcopia_Medication));
                ISQLQuery sql = session.GetISession().CreateSQLQuery("select * from rcopia_medication as a where Human_ID=" + ulHumanID + " and Deleted <> 'Y';").AddEntity("m", typeof(Rcopia_Medication));

                rcopiaMedicationList = sql.List<Rcopia_Medication>();
                mySession.Close();
            }

            IList<Rcopia_Medication> ilstExactDuplicatesOfMedication = GetMedicationWithExactDuplicates(ulHumanID, sStatus);
            if (ilstExactDuplicatesOfMedication.Count > 0)
            {
                rcopiaMedicationList = rcopiaMedicationList.Except(ilstExactDuplicatesOfMedication).ToList<Rcopia_Medication>();
            }

            for (int iCount = 0; iCount < rcopiaMedicationList.Count; iCount++)
            {
                rcopia_Medication = new Rcopia_Medication();
                //Get partial duplicates of medication
                IList<Rcopia_Medication> ilstCountOfrcopiaMedication = rcopiaMedicationList.Where(x => x.Brand_Name == rcopiaMedicationList[iCount].Brand_Name
                && x.Strength == rcopiaMedicationList[iCount].Strength
                && x.Form == rcopiaMedicationList[iCount].Form
                && x.Dose == rcopiaMedicationList[iCount].Dose
                && x.Dose_Unit == rcopiaMedicationList[iCount].Dose_Unit
                && x.Route == rcopiaMedicationList[iCount].Route
                && x.Dose_Timing == rcopiaMedicationList[iCount].Dose_Timing).ToList<Rcopia_Medication>();

                ilstCountOfrcopiaMedication = SeparateMedicationByStatus(ilstCountOfrcopiaMedication, sStatus.ToUpper());

                if (ilstCountOfrcopiaMedication.Count > 1)
                {
                    ilstCountOfrcopiaMedication = ilstCountOfrcopiaMedication.Where(x=> x.Id == rcopiaMedicationList[iCount].Id).ToList<Rcopia_Medication>();
                    if (ilstCountOfrcopiaMedication.Count > 0)
                    {
                        ilstrcopiaMedicationReturn.Add(ilstCountOfrcopiaMedication[0]);
                    }
                }


            }

            //Sorting
            ilstrcopiaMedicationReturn = SortingMedicationListForPartialDuplicate(ilstrcopiaMedicationReturn);

            return ilstrcopiaMedicationReturn;
        }

        public IList<Rcopia_Medication> SortingMedicationListForPartialDuplicate(IList<Rcopia_Medication> ilstParticalDuplicateSort)
        {
            ilstParticalDuplicateSort = ilstParticalDuplicateSort.OrderBy(rcopia => rcopia.Brand_Name).
                ThenBy(rcopia => rcopia.Strength).ThenBy(rcopia => rcopia.Form).ThenBy(rcopia => rcopia.Dose).
                ThenBy(rcopia => rcopia.Dose_Unit).ThenBy(rcopia => rcopia.Route).ThenBy(rcopia => rcopia.Dose_Timing).
                ThenBy(rcopia => rcopia.Start_Date).ThenBy(rcopia => rcopia.Stop_Date).ToList<Rcopia_Medication>() ;
            return ilstParticalDuplicateSort;
        }

        public IList<Rcopia_Medication> SeparateMedicationByStatus(IList<Rcopia_Medication> rcopiaMedicationList, string sStatus)
        {
            DateTime sTodayDate = DateTime.Now.Date;
            for (int iCount = 0; iCount < rcopiaMedicationList.Count; iCount++)
            {
                if ((rcopiaMedicationList[iCount].Start_Date.Date == DateTime.MinValue.Date && rcopiaMedicationList[iCount].Stop_Date.Date == DateTime.MinValue.Date) ||
                       (rcopiaMedicationList[iCount].Start_Date.Date <= sTodayDate.Date && rcopiaMedicationList[iCount].Stop_Date.Date == DateTime.MinValue.Date) ||
                       (rcopiaMedicationList[iCount].Start_Date.Date <= sTodayDate.Date && rcopiaMedicationList[iCount].Stop_Date.Date > sTodayDate.Date) ||
                       (rcopiaMedicationList[iCount].Start_Date.Date == DateTime.MinValue.Date && rcopiaMedicationList[iCount].Stop_Date.Date > sTodayDate.Date))
                {
                    rcopiaMedicationList[iCount].Status = "ACTIVE";
                }
                else
                {
                    rcopiaMedicationList[iCount].Status = "INACTIVE";
                }
            }

            if (sStatus == "ACTIVE")
            {
                rcopiaMedicationList = rcopiaMedicationList.Where(x => x.Status == sStatus).ToList<Rcopia_Medication>();
            }
            return rcopiaMedicationList;
        }

        public string UpdateRcopiaMedication(IList<ulong> ilstRcopiaID, ulong ulHumanID, string sFacilityName, string sLegalOrg, string sUserName)
        {
            if (ilstRcopiaID.Count == 0)
            {
                return "Success";
            }
            RCopiaGenerateXML rcopiaXML = new RCopiaGenerateXML();
            string sInputXML = string.Empty;
            string sOutputXML = string.Empty;
            string sRequestXML = string.Empty;
            IList<ulong> ilstMedicationId = new List<ulong>();
            DateTime dtRCopia_Medication_Last_Updated_Date_Time = new DateTime(2001, 01, 01);
            RCopiaSessionManager rcopiaSessionMngr = new RCopiaSessionManager(sLegalOrg);
            RCopiaTransactionManager rCopiaTransactionManager = new RCopiaTransactionManager();
            IList<RCopiaDeduplicateLog> ilstrcopia_deduplicate_log = new List<RCopiaDeduplicateLog>();
            //To get the Medication list from DrFirst
            sInputXML = rcopiaXML.CreateUpdateMedicationXML(ulHumanID, dtRCopia_Medication_Last_Updated_Date_Time, sLegalOrg, DateTime.MinValue);

            if (rcopiaSessionMngr.DownloadAddress != null && sInputXML != string.Empty)
            {
                sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.DownloadAddress + sInputXML, 1, sUserName);
                //Jira CAP-1366
                if (sOutputXML != null && sOutputXML.StartsWith("HttpPostError") == true)
                {
                    return "Error during getting the medication records for the patient - " + sOutputXML;
                }

                XmlDocument XMLDoc = new XmlDocument();
                if (sOutputXML != null)
                {
                    XMLDoc.LoadXml(sOutputXML);
                    int iMedicationListCount = XMLDoc.SelectNodes("RCExtResponse/Response/MedicationList/Medication").Count;
                    int iResopnseMedicationCount = 0;
                    for (int iCont = 1; iCont <= iMedicationListCount; iCont++)
                    {

                        ilstMedicationId = ilstRcopiaID.Where(x => x.ToString() == XMLDoc.SelectSingleNode("RCExtResponse/Response/MedicationList/Medication[" + iCont + "]/RcopiaID").InnerText).ToList<ulong>();

                        if (ilstMedicationId.Count > 0)
                        {
                            sRequestXML = sRequestXML + XMLDoc.SelectSingleNode("RCExtResponse/Response/MedicationList/Medication[" + iCont + "]").OuterXml;
                            iResopnseMedicationCount++;
                        }
                    }
                    if (ilstRcopiaID.Count != iResopnseMedicationCount)
                    {
                        return "Medication record is not found in DrFirst for RCopia ID : " + string.Join(",", ilstRcopiaID);
                    }
                    sRequestXML = "<MedicationList>" + sRequestXML + "</MedicationList>";

                    sRequestXML = rCopiaTransactionManager.createSendMedicationXml(ulHumanID, sLegalOrg, sRequestXML);

                    // To update the medication data in DrFirst
                    if (rcopiaSessionMngr.UploadAddress != null && sRequestXML != string.Empty && sRequestXML.ToUpper().Contains("<DELETED>N</DELETED>") == true)
                    {
                        XmlDocument UpdateXMLDoc = new XmlDocument();
                        UpdateXMLDoc.LoadXml(sRequestXML);

                        int iUpdateMedicationCount = UpdateXMLDoc.SelectNodes("RCExtRequest/Request/MedicationList/Medication").Count;
                        for (int i = 1; i <= iUpdateMedicationCount; i++)
                        {
                            //Delete Tag
                            if (UpdateXMLDoc?.SelectSingleNode("RCExtRequest/Request/MedicationList/Medication[" + i + "]/Deleted")?.InnerText != null)
                            {
                                UpdateXMLDoc.SelectSingleNode("RCExtRequest/Request/MedicationList/Medication[" + i + "]/Deleted").InnerText = "y";
                            }
                            else
                            {
                                XmlNode UpdateDeleteTag = UpdateXMLDoc.CreateElement("Deleted");
                                UpdateDeleteTag.InnerText = "y";
                                UpdateXMLDoc.SelectSingleNode("RCExtRequest/Request/MedicationList/Medication[" + i + "]").AppendChild(UpdateDeleteTag);
                            }


                        }
                        sRequestXML = UpdateXMLDoc.OuterXml;
                        sOutputXML = rcopiaSessionMngr.HttpPost(rcopiaSessionMngr.UploadAddress + sRequestXML, 1, sUserName);
                        //Jira CAP-1366
                        if (sOutputXML != null && sOutputXML.StartsWith("HttpPostError") == true)
                        {
                            return "Error during updating the medication record - " + sOutputXML;
                        }
                        XmlDocument XMLResponse = new XmlDocument();
                        XMLResponse.LoadXml(sOutputXML);
                        int iResponseMedicationListCount = XMLResponse.SelectNodes("RCExtResponse/Response/MedicationList/Medication").Count;
                        string sStatus = string.Empty;
                        string sTemp = string.Empty;
                        string sErrorRcopiaID = string.Empty;

                        RCopiaDeduplicateLog rcopiaDuplicatelog = new RCopiaDeduplicateLog();
                        for (int iCont = 1; iCont <= iResponseMedicationListCount; iCont++)
                        {

                            sStatus = XMLResponse.SelectSingleNode("RCExtResponse/Response/MedicationList/Medication[" + iCont + "]/Status")?.InnerText;
                            sTemp = XMLResponse.SelectSingleNode("RCExtResponse/Response/MedicationList/Medication[" + iCont + "]/RcopiaID")?.InnerText;

                            if (sTemp != null && sStatus != null && sStatus == "deleted")
                            {
                                rcopiaDuplicatelog = new RCopiaDeduplicateLog();
                                rcopiaDuplicatelog.RCopia_ID = Convert.ToUInt64(sTemp);
                                rcopiaDuplicatelog.Human_ID = ulHumanID;
                                rcopiaDuplicatelog.Entity_Name = "Medication";
                                rcopiaDuplicatelog.Duplicate_Type = "Partial Duplicate";
                                rcopiaDuplicatelog.Created_Date_and_Time = DateTime.Now.ToUniversalTime();
                                rcopiaDuplicatelog.Created_By = sUserName;
                                ilstrcopia_deduplicate_log.Add(rcopiaDuplicatelog);
                            }
                            else if (sTemp != null && sStatus != null && sStatus != "deleted")
                            {
                                sErrorRcopiaID = sErrorRcopiaID + ((sErrorRcopiaID != string.Empty) ? "," : "RCopia ID : ") + sTemp + " : " + sStatus;
                            }
                            else if (sStatus == null || sTemp == null)
                            {
                                sErrorRcopiaID = sErrorRcopiaID + ((sErrorRcopiaID != string.Empty) ? "," : "")
                                    + ((sStatus == null && sTemp != null) ? "RCopia ID : " + sTemp + " Status is null" :
                                    (sStatus != null && sTemp == null) ? "RCopia ID is null " + sStatus : "RCopia ID and Status are null");
                            }
                        }
                        if (sErrorRcopiaID != string.Empty)
                        {
                            return "Medication is not updated in DrFirst. \n For refernce : " + sErrorRcopiaID;
                        }

                    }
                    else
                    {
                        return (rcopiaSessionMngr.UploadAddress == null) ? "rcopiaSessionMngr.UploadAddress is null" : (sRequestXML == string.Empty) ? "sRequestXML is Empty" : "There is no DELETED tag in Response or DELETED is in 'Y'";
                    }
                }


                //Download Rcopia Data from the DrFirst
                string sErrorMessage = string.Empty;
                Rcopia_Update_InfoManager objUpdateInfoMngr = new Rcopia_Update_InfoManager();
                DateTime dtClientDate = DateTime.UtcNow;
                if (sUserName != null && sFacilityName != null)
                    //Commented the Patient Level RCopia Download
                    sErrorMessage = objUpdateInfoMngr.DownloadRCopiaInfo(rcopiaSessionMngr.DownloadAddress, sUserName, string.Empty, dtClientDate, sFacilityName, 0, ulHumanID, sLegalOrg);
                if (sErrorMessage != string.Empty)
                {
                    return "DownloadRCopiaInfo-" + sErrorMessage;
                }

                //Loging the deleted iteams in ilstRcopia_deduplicate_log
                if (ilstrcopia_deduplicate_log.Count > 0)
                {
                    RCopiaDeduplicateLogManager mngrRCopiaDeduplicateLog = new RCopiaDeduplicateLogManager();
                    mngrRCopiaDeduplicateLog.SaveRcopia_deduplicate_logWithTransaction(ilstrcopia_deduplicate_log, null, string.Empty);
                }


            }

            return "Success";
        }

        public string UpdateMedicationInHumanBlob(IList<ulong> ilstHumanID)
        {
            HumanBlobManager HumanBlobMngr = new HumanBlobManager();
            Rcopia_MedicationManager rcopiaMngr = new Rcopia_MedicationManager();
            IList<Human_Blob> ilstHumanBlob = new List<Human_Blob>();
            Human_Blob objHumanblobKeepAndMerge = new Human_Blob();
            byte[] bytesKeep = null;
            XmlDocument xmlHumanDocKeepAndMerge = new XmlDocument();
            IList<Rcopia_Medication> ilstKeepAndMergeMedication = new List<Rcopia_Medication>();
            XmlNode newnode = null;
            Rcopia_Medication Properties = new Rcopia_Medication();
            try
            {
                for (int iCount = 0; iCount < ilstHumanID.Count; iCount++)
                {

                    ilstKeepAndMergeMedication = rcopiaMngr.GetMedicationByHumanID(ilstHumanID[iCount]);


                    objHumanblobKeepAndMerge = HumanBlobMngr.GetHumanBlob(ilstHumanID[iCount])[0];

                    xmlHumanDocKeepAndMerge.LoadXml(System.Text.Encoding.UTF8.GetString(objHumanblobKeepAndMerge.Human_XML));

                    string sKeepMedications = GenerateTag(ilstKeepAndMergeMedication);

                    if (sKeepMedications != string.Empty)
                    {
                        if (xmlHumanDocKeepAndMerge.GetElementsByTagName(Properties.GetType().Name + "List").Count > 0)
                        {
                            xmlHumanDocKeepAndMerge.GetElementsByTagName(Properties.GetType().Name + "List")[0].InnerXml = sKeepMedications;
                        }
                        else
                        {
                            newnode = xmlHumanDocKeepAndMerge.CreateNode(XmlNodeType.Element, Properties.GetType().Name + "List", "");
                            newnode.InnerXml = sKeepMedications;
                            xmlHumanDocKeepAndMerge.GetElementsByTagName("Modules")[0].AppendChild(newnode);
                        }
                    }
                    else if (xmlHumanDocKeepAndMerge.GetElementsByTagName(Properties.GetType().Name + "List").Count > 0)
                    {
                        xmlHumanDocKeepAndMerge.GetElementsByTagName(Properties.GetType().Name + "List")[0].RemoveAll();
                    }


                    try
                    {
                        bytesKeep = System.Text.Encoding.Default.GetBytes(xmlHumanDocKeepAndMerge.OuterXml);
                    }
                    catch (Exception ex)
                    {
                        return "Error : " + ex?.Message;
                    }
                    objHumanblobKeepAndMerge.Human_XML = bytesKeep;
                    ilstHumanBlob.Add(objHumanblobKeepAndMerge);

                }
                HumanBlobMngr.SaveHumanBlobWithTransaction(ilstHumanBlob, string.Empty);
            }
            catch (Exception ex)
            {
                return "Error : " + ex?.Message;
            }
            return "Success";
        }

        public string GenerateTag(IList<Rcopia_Medication> ilstKeepAndMergeMedication)
        {
            string sObjectTags = string.Empty;
            IEnumerable<PropertyInfo> propInfo = null;
            XmlDocument itemDoc = new XmlDocument();
            XmlAttribute attlabel = null;
            XmlNode Newnode = null;
            Rcopia_Medication Properties = new Rcopia_Medication();
            
                propInfo = from obji in (Properties).GetType().GetProperties() select obji;


                foreach (Rcopia_Medication rcopiamedication in ilstKeepAndMergeMedication)
                {
                    Newnode = null;
                    Newnode = itemDoc.CreateNode(XmlNodeType.Element, rcopiamedication.GetType().Name, "");
                    foreach (PropertyInfo property in propInfo)
                    {
                        attlabel = null;
                        attlabel = itemDoc.CreateAttribute(property.Name);
                        if (property.PropertyType.Name.ToUpper() == "DATETIME")
                        {
                            attlabel.Value = Convert.ToDateTime(property.GetValue(rcopiamedication, null)).ToString("yyyy-MM-dd hh:mm:ss tt");
                        }
                        else
                        {
                            attlabel.Value = property.GetValue(rcopiamedication, null).ToString();
                        }
                        Newnode.Attributes.Append(attlabel);
                    }
                    sObjectTags = sObjectTags + Newnode.OuterXml;
                }
            
            return sObjectTags;
        }
    }
}
