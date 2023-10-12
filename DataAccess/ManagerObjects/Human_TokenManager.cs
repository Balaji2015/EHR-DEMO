using System;
using System.Collections;
using System.Collections.Generic;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.Core.DTO;
using NHibernate;
using NHibernate.Criterion;
using System.IO;
using System.Runtime.Serialization;
using System.Web.Mail;
using System.Data;
using System.Linq;

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public partial interface IHuman_TokenManager : IManagerBase<Human_Token, ulong>
    {
        int SaveHuman_tokenWithoutTransaction(IList<Human> ListToInsertHuman, IList<Human> ListToUpdateHuman, ISession MySession, string MACAddress, string sPriCarrier);
    }
    public partial class Human_TokenManager : ManagerBase<Human_Token, ulong>, IHuman_TokenManager
    {
        #region Constructors

        public Human_TokenManager()
            : base()
        {

        }
        public Human_TokenManager
            (INHibernateSession session)
            : base(session)
        {

        }
        #endregion

        #region Methods

        public int SaveHuman_tokenWithoutTransaction(IList<Human> ListToInsertHuman, IList<Human> ListToUpdateHuman, ISession MySession, string MACAddress, string sPriCarrier)
        {
            int iResult = 0;
            bool bCheck = false;
            if ((ListToInsertHuman != null && ListToInsertHuman.Count > 0))
            {
                IList<Human_Token> ListToUpdate = null;
                IList<Human_Token> humanTokenList = new List<Human_Token>();
                Human_Token objhumanToken = new Human_Token();
                string Patient_Account_External = ListToInsertHuman[0].Patient_Account_External != string.Empty && ListToInsertHuman[0].Patient_Account_External != " " ? " | EX.ACC#: " + ListToInsertHuman[0].Patient_Account_External : "";
                string medical_record_number = ListToInsertHuman[0].Medical_Record_Number != string.Empty && ListToInsertHuman[0].Medical_Record_Number != " " ? " | MR#: " + ListToInsertHuman[0].Medical_Record_Number : "";
                string Address = ListToInsertHuman[0].Street_Address1 != string.Empty && ListToInsertHuman[0].City != string.Empty ? " | ADDR: " + ListToInsertHuman[0].Street_Address1 + " , " + ListToInsertHuman[0].City + " " + ListToInsertHuman[0].ZipCode : " | ZipCode: " + ListToInsertHuman[0].ZipCode;
                string phone = ListToInsertHuman[0].Home_Phone_No != string.Empty ? " | Ph: " + ListToInsertHuman[0].Home_Phone_No : "";
                string fax = ListToInsertHuman[0].Fax_Number != string.Empty ? " | Fax: " + ListToInsertHuman[0].Fax_Number : "";
                string Result = string.Empty;

                if (sPriCarrier == string.Empty)
                {
                    //Jira CAP-1151
                    //Result = ListToInsertHuman[0].Last_Name + "," + ListToInsertHuman[0].First_Name + " " + ListToInsertHuman[0].MI
                    Result = ListToInsertHuman[0].Last_Name + ", " + ListToInsertHuman[0].First_Name + " " + ListToInsertHuman[0].MI
                              + " | DOB: " + ListToInsertHuman[0].Birth_Date.ToString("dd-MMM-yyyy")
                              + " | " + ListToInsertHuman[0].Sex
                              + " | ACC#: " + ListToInsertHuman[0].Id.ToString()
                              + Patient_Account_External
                              + medical_record_number
                             + Address
                              + phone
                              + fax
                              + " | PATIENT TYPE: " + ListToInsertHuman[0].Human_Type;
                }
                else
                {
                    //Jira CAP-1151
                    //Result = ListToInsertHuman[0].Last_Name + "," + ListToInsertHuman[0].First_Name + " " + ListToInsertHuman[0].MI
                    Result = ListToInsertHuman[0].Last_Name + ", " + ListToInsertHuman[0].First_Name + " " + ListToInsertHuman[0].MI
                              + " | DOB: " + ListToInsertHuman[0].Birth_Date.ToString("dd-MMM-yyyy")
                              + " | " + ListToInsertHuman[0].Sex
                              + " | ACC#: " + ListToInsertHuman[0].Id.ToString()
                              + " | PRI.CAR: " + sPriCarrier
                              + Patient_Account_External
                              + medical_record_number
                             + Address
                              + phone
                              + fax
                              + " | PATIENT TYPE: " + ListToInsertHuman[0].Human_Type;
                }

                if (ListToInsertHuman[0].ACO_Is_Eligible_Patient != string.Empty && ListToInsertHuman[0].ACO_Is_Eligible_Patient != "N")
                    Result += " | " + ListToInsertHuman[0].ACO_Is_Eligible_Patient;

               
                string[] containfield = new string[8];
                containfield[0] = ListToInsertHuman[0].Last_Name != string.Empty ? "L" : "";
                containfield[1] = ListToInsertHuman[0].First_Name != string.Empty ? "F" : "";
                containfield[2] = ListToInsertHuman[0].MI != string.Empty ? ListToInsertHuman[0].MI.Length > 1 ? "M" : "" : "";
                containfield[3] = ListToInsertHuman[0].Medical_Record_Number != string.Empty ? "R" : "";
                containfield[4] = ListToInsertHuman[0].Id != 0 ? "I" : "";
                containfield[5] = ListToInsertHuman[0].Patient_Account_External != string.Empty ? "E" : "";
                // containfield[6] = ListToInsertHuman[0].SSN != string.Empty ? "S" : "";
                if (ListToInsertHuman[0].Last_Name != string.Empty && ListToInsertHuman[0].Birth_Date != null)
                    containfield[6] = "BL";
                else
                    containfield[6] = "";
                containfield[7] = ListToInsertHuman[0].Birth_Date != null ? "B" : "";

                for (int k = 0; k < containfield.Length; k++)
                {
                    string option = containfield[k] != "" ? containfield[k] : "skip";
                    if (option != "skip")
                    {
                        objhumanToken = new Human_Token();
                        objhumanToken.Human_ID = ListToInsertHuman[0].Id;
                        objhumanToken.Result = Result;
                        objhumanToken.Legal_Org = ListToInsertHuman[0].Legal_Org;
                        objhumanToken.Primary_Carrier_ID = ListToInsertHuman[0].Primary_Carrier_ID;
                        objhumanToken.Patient_Status = ListToInsertHuman[0].Patient_Status;
                        objhumanToken.Account_Status = ListToInsertHuman[0].Account_Status;
                        objhumanToken.Human_Type = ListToInsertHuman[0].Human_Type;

                        switch (option)
                        {

                            case "L":
                                objhumanToken.Token = ListToInsertHuman[0].Last_Name;
                                humanTokenList.Add(objhumanToken);
                                break;
                            case "F":
                                objhumanToken.Token = ListToInsertHuman[0].First_Name;
                                humanTokenList.Add(objhumanToken);
                                break;
                            case "M":
                                objhumanToken.Token = ListToInsertHuman[0].MI;
                                humanTokenList.Add(objhumanToken);
                                break;
                            case "R":
                                objhumanToken.Token = ListToInsertHuman[0].Medical_Record_Number;
                                humanTokenList.Add(objhumanToken);
                                break;
                            case "I":
                                objhumanToken.Token = ListToInsertHuman[0].Id.ToString();
                                humanTokenList.Add(objhumanToken);
                                break;
                            case "E":
                                objhumanToken.Token = ListToInsertHuman[0].Patient_Account_External;
                                humanTokenList.Add(objhumanToken);
                                break;
                            //case "S":
                            //objhumanToken.Token = ListToInsertHuman[0].SSN;// For Bug Id : 74044 
                            //humanTokenList.Add(objhumanToken);
                            //break;
                            case "BL":
                                objhumanToken.Token = ListToInsertHuman[0].Birth_Date.ToString("yyyy") + ListToInsertHuman[0].Last_Name;
                                humanTokenList.Add(objhumanToken);
                                break;
                            case "B":
                                objhumanToken.Token = ListToInsertHuman[0].Birth_Date.ToString("dd-MMM-yyyy");
                                humanTokenList.Add(objhumanToken);
                                break;
                            default:
                                break;
                        }
                    }
                }

                GenerateXml XMLObj = null;
                iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref humanTokenList, ref ListToUpdate, null, MySession, MACAddress, false, false, 0, string.Empty, ref XMLObj);
            }
            else if (ListToUpdateHuman != null && ListToUpdateHuman.Count > 0)
            {
                //if (ListToUpdateHuman[0].Created_By != "gurantor")
                //{
                IList<Human_Token> ListToUpdate = null;
                IList<Human_Token> humanTokenList = new List<Human_Token>();
                IList<Human_Token> humanTokenDeleteList = new List<Human_Token>();
                Human_Token objhumanToken = new Human_Token();
                Human_TokenManager HumTokMgn = new Human_TokenManager();
                ICriteria criteria = MySession.CreateCriteria(typeof(Human_Token)).Add(Expression.Eq("Human_ID", ListToUpdateHuman[0].Id)).AddOrder(Order.Asc("Id"));
                humanTokenDeleteList = criteria.List<Human_Token>();

                string Result = string.Empty;
                string Updatename = string.Empty;
                string Oldname = string.Empty;
                if (ListToUpdateHuman[0].Last_Name != "")
                    Updatename = ListToUpdateHuman[0].Last_Name;
                if (ListToUpdateHuman[0].First_Name != "")
                    Updatename += "|" + ListToUpdateHuman[0].First_Name;
                if (ListToUpdateHuman[0].MI != "" && ListToUpdateHuman[0].MI.Length > 1)
                    Updatename += "|" + ListToUpdateHuman[0].MI;
                if (ListToUpdateHuman[0].Medical_Record_Number != "")
                    Updatename += "|" + ListToUpdateHuman[0].Medical_Record_Number;
                if (Convert.ToString(ListToUpdateHuman[0].Id) != "")
                    Updatename += "|" + Convert.ToString(ListToUpdateHuman[0].Id);
                if (Convert.ToString(ListToUpdateHuman[0].Patient_Account_External) != "")
                    Updatename += "|" + Convert.ToString(ListToUpdateHuman[0].Patient_Account_External);
                //if (ListToUpdateHuman[0].SSN != "")
                //    Updatename += "|" + ListToUpdateHuman[0].SSN;
                if (ListToUpdateHuman[0].Last_Name != string.Empty && ListToUpdateHuman[0].Birth_Date != null)
                    Updatename += "|" + ListToUpdateHuman[0].Birth_Date.ToString("yyyy") + ListToUpdateHuman[0].Last_Name;
                if (Convert.ToString(ListToUpdateHuman[0].Birth_Date) != "")
                    Updatename += "|" + ListToUpdateHuman[0].Birth_Date.ToString("dd-MMM-yyyy");
                for (int i = 0; i < humanTokenDeleteList.Count; i++)
                {
                    Oldname += i == 0 ? humanTokenDeleteList[i].Token : "|" + humanTokenDeleteList[i].Token;
                }

                if (Updatename.ToUpper().Split('|').Length == Oldname.ToUpper().Split('|').Length)
                {
                    if ((Oldname != Updatename) || (ListToUpdateHuman[0].Patient_Status != humanTokenDeleteList[0].Patient_Status) || (ListToUpdateHuman[0].Account_Status != humanTokenDeleteList[0].Account_Status) || (ListToUpdateHuman[0].Primary_Carrier_ID != humanTokenDeleteList[0].Primary_Carrier_ID)) //For Bug Id : 61568 
                        bCheck = true;
                    else
                        bCheck = false;

                }
                else
                {
                    bCheck = true;
                }
                if (bCheck == false)
                {
                    string Patient_Account_External = ListToUpdateHuman[0].Patient_Account_External != string.Empty && ListToUpdateHuman[0].Patient_Account_External != " " ? " | EX.ACC#: " + ListToUpdateHuman[0].Patient_Account_External : "";
                    string medical_record_number = ListToUpdateHuman[0].Medical_Record_Number != string.Empty && ListToUpdateHuman[0].Medical_Record_Number != " " ? " | MR#: " + ListToUpdateHuman[0].Medical_Record_Number : "";
                    string Address = ListToUpdateHuman[0].Street_Address1 != string.Empty && ListToUpdateHuman[0].City != string.Empty ? " | ADDR: " + ListToUpdateHuman[0].Street_Address1 + " , " + ListToUpdateHuman[0].City + " " + ListToUpdateHuman[0].ZipCode : " | ZipCode: " + ListToUpdateHuman[0].ZipCode;
                    string phone = ListToUpdateHuman[0].Home_Phone_No != string.Empty ? " | Ph: " + ListToUpdateHuman[0].Home_Phone_No : "";
                    string fax = ListToUpdateHuman[0].Fax_Number != string.Empty ? " | Fax: " + ListToUpdateHuman[0].Fax_Number : "";

                    if (sPriCarrier == string.Empty)
                    {
                        //Jira CAP-1151
                        //Result = ListToUpdateHuman[0].Last_Name + "," + ListToUpdateHuman[0].First_Name + " " + ListToUpdateHuman[0].MI
                        Result = ListToUpdateHuman[0].Last_Name + ", " + ListToUpdateHuman[0].First_Name + " " + ListToUpdateHuman[0].MI
                                      + " | DOB: " + ListToUpdateHuman[0].Birth_Date.ToString("dd-MMM-yyyy")
                                      + " | " + ListToUpdateHuman[0].Sex
                                      + " | ACC#: " + ListToUpdateHuman[0].Id.ToString()
                                      + Patient_Account_External
                                      + medical_record_number
                                      + Address
                                      + phone
                                      + fax
                                      + " | PATIENT TYPE: " + ListToUpdateHuman[0].Human_Type;
                    }
                    else
                    {
                        //Jira CAP-1151
                        //Result = ListToUpdateHuman[0].Last_Name + "," + ListToUpdateHuman[0].First_Name + " " + ListToUpdateHuman[0].MI
                        Result = ListToUpdateHuman[0].Last_Name + ", " + ListToUpdateHuman[0].First_Name + " " + ListToUpdateHuman[0].MI
                                      + " | DOB: " + ListToUpdateHuman[0].Birth_Date.ToString("dd-MMM-yyyy")
                                      + " | " + ListToUpdateHuman[0].Sex
                                      + " | ACC#: " + ListToUpdateHuman[0].Id.ToString()
                                      + " | PRI.CAR: " + sPriCarrier
                                      + Patient_Account_External
                                      + medical_record_number
                                      + Address
                                      + phone
                                      + fax
                                      + " | PATIENT TYPE: " + ListToUpdateHuman[0].Human_Type;
                    }

                    if (ListToUpdateHuman[0].ACO_Is_Eligible_Patient != string.Empty && ListToUpdateHuman[0].ACO_Is_Eligible_Patient != "N")
                        Result += " | " + ListToUpdateHuman[0].ACO_Is_Eligible_Patient;

                    if (Result != humanTokenDeleteList[0].Result)
                        bCheck = true;
                }
                if (bCheck)
                {

                    if (Result == string.Empty)
                    {
                        string Patient_Account_External = ListToUpdateHuman[0].Patient_Account_External != string.Empty && ListToUpdateHuman[0].Patient_Account_External != " " ? " | EX.ACC#: " + ListToUpdateHuman[0].Patient_Account_External : "";
                        string medical_record_number = ListToUpdateHuman[0].Medical_Record_Number != string.Empty && ListToUpdateHuman[0].Medical_Record_Number != " " ? " | MR#: " + ListToUpdateHuman[0].Medical_Record_Number : "";
                        string Address = ListToUpdateHuman[0].Street_Address1 != string.Empty && ListToUpdateHuman[0].City != string.Empty ? " | ADDR: " + ListToUpdateHuman[0].Street_Address1 + " , " + ListToUpdateHuman[0].City + " " + ListToUpdateHuman[0].ZipCode : " | ZipCode: " + ListToUpdateHuman[0].ZipCode;
                        string phone = ListToUpdateHuman[0].Home_Phone_No != string.Empty ? " | Ph: " + ListToUpdateHuman[0].Home_Phone_No : "";
                        string fax = ListToUpdateHuman[0].Fax_Number != string.Empty ? " | Fax: " + ListToUpdateHuman[0].Fax_Number : "";

                        if (sPriCarrier == string.Empty)
                        {
                            //Jira CAP-1151
                            //Result = ListToUpdateHuman[0].Last_Name + "," + ListToUpdateHuman[0].First_Name + " " + ListToUpdateHuman[0].MI
                            Result = ListToUpdateHuman[0].Last_Name + ", " + ListToUpdateHuman[0].First_Name + " " + ListToUpdateHuman[0].MI
                                          + " | DOB: " + ListToUpdateHuman[0].Birth_Date.ToString("dd-MMM-yyyy")
                                          + " | " + ListToUpdateHuman[0].Sex
                                          + " | ACC#: " + ListToUpdateHuman[0].Id.ToString()
                                          + Patient_Account_External
                                          + medical_record_number
                                          + Address
                                          + phone
                                          + fax
                                          + " | PATIENT TYPE: " + ListToUpdateHuman[0].Human_Type;
                        }
                        else
                        {
                            //Jira CAP-1151
                            //Result = ListToUpdateHuman[0].Last_Name + "," + ListToUpdateHuman[0].First_Name + " " + ListToUpdateHuman[0].MI
                            Result = ListToUpdateHuman[0].Last_Name + ", " + ListToUpdateHuman[0].First_Name + " " + ListToUpdateHuman[0].MI
                                          + " | DOB: " + ListToUpdateHuman[0].Birth_Date.ToString("dd-MMM-yyyy")
                                          + " | " + ListToUpdateHuman[0].Sex
                                          + " | ACC#: " + ListToUpdateHuman[0].Id.ToString()
                                          + " | PRI.CAR: " + sPriCarrier
                                          + Patient_Account_External
                                          + medical_record_number
                                          + Address
                                          + phone
                                          + fax
                                          + " | PATIENT TYPE: " + ListToUpdateHuman[0].Human_Type;
                        }

                        if (ListToUpdateHuman[0].ACO_Is_Eligible_Patient != string.Empty && ListToUpdateHuman[0].ACO_Is_Eligible_Patient != "N")
                            Result += " | " + ListToUpdateHuman[0].ACO_Is_Eligible_Patient;
                    }


                    string[] containfield = new string[8];
                    containfield[0] = ListToUpdateHuman[0].Last_Name != string.Empty ? "L" : "";
                    containfield[1] = ListToUpdateHuman[0].First_Name != string.Empty ? "F" : "";
                    containfield[2] = ListToUpdateHuman[0].MI != string.Empty ? ListToUpdateHuman[0].MI.Length > 1 ? "M" : "" : "";
                    containfield[3] = ListToUpdateHuman[0].Medical_Record_Number != string.Empty ? "R" : "";
                    containfield[4] = ListToUpdateHuman[0].Id != 0 ? "I" : "";
                    containfield[5] = ListToUpdateHuman[0].Patient_Account_External != string.Empty ? "E" : "";
                   // containfield[6] = ListToUpdateHuman[0].SSN != string.Empty ? "S" : "";
                    if (ListToUpdateHuman[0].Last_Name != string.Empty && ListToUpdateHuman[0].Birth_Date != null)
                        containfield[6] = "BL";
                    else
                        containfield[6] = "";


                    containfield[7] = ListToUpdateHuman[0].Birth_Date != null ? "B" : "";

                    for (int k = 0; k < containfield.Length; k++)
                    {
                        string option = containfield[k] != "" ? containfield[k] : "skip";
                        if (option != "skip")
                        {
                            objhumanToken = new Human_Token();
                            objhumanToken.Human_ID = ListToUpdateHuman[0].Id;
                            objhumanToken.Result = Result;
                            objhumanToken.Legal_Org = ListToUpdateHuman[0].Legal_Org;
                            objhumanToken.Primary_Carrier_ID = ListToUpdateHuman[0].Primary_Carrier_ID;
                            objhumanToken.Patient_Status = ListToUpdateHuman[0].Patient_Status;
                            objhumanToken.Account_Status = ListToUpdateHuman[0].Account_Status;
                            objhumanToken.Human_Type = ListToUpdateHuman[0].Human_Type;

                            switch (option)
                            {

                                case "L":
                                    objhumanToken.Token = ListToUpdateHuman[0].Last_Name;
                                    humanTokenList.Add(objhumanToken);
                                    break;
                                case "F":
                                    objhumanToken.Token = ListToUpdateHuman[0].First_Name;
                                    humanTokenList.Add(objhumanToken);
                                    break;
                                case "M":
                                    objhumanToken.Token = ListToUpdateHuman[0].MI;
                                    humanTokenList.Add(objhumanToken);
                                    break;
                                case "R":
                                    objhumanToken.Token = ListToUpdateHuman[0].Medical_Record_Number;
                                    humanTokenList.Add(objhumanToken);
                                    break;
                                case "I":
                                    objhumanToken.Token = ListToUpdateHuman[0].Id.ToString();
                                    humanTokenList.Add(objhumanToken);
                                    break;
                                case "E":
                                    objhumanToken.Token = ListToUpdateHuman[0].Patient_Account_External;
                                    humanTokenList.Add(objhumanToken);
                                    break;
                                //case "S":
                                //    objhumanToken.Token = ListToUpdateHuman[0].SSN;// For Bug Id : 74044 
                                //    humanTokenList.Add(objhumanToken);
                                //    break;
                                case "BL":
                                    objhumanToken.Token = ListToUpdateHuman[0].Birth_Date.ToString("yyyy") + ListToUpdateHuman[0].Last_Name;
                                    humanTokenList.Add(objhumanToken);
                                    break;
                                case "B":
                                    objhumanToken.Token = ListToUpdateHuman[0].Birth_Date.ToString("dd-MMM-yyyy");
                                    humanTokenList.Add(objhumanToken);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    GenerateXml XMLObj = null;
                    iResult = SaveUpdateDelete_DBAndXML_WithoutTransaction(ref humanTokenList, ref ListToUpdate, humanTokenDeleteList, MySession, MACAddress, false, false, 0, string.Empty, ref XMLObj);
                }
                // }
            }
            return iResult;
        }
        public Human_Token GetHumanTokenbyhumanid(ulong human_id)
        {
            Human_Token objHuman_Token = new Human_Token();
            using (ISession iMySession = NHibernateSessionManager.Instance.CreateISession())
            {
                ICriteria crit = iMySession.CreateCriteria(typeof(Human_Token)).Add(Expression.Eq("Human_ID", human_id));
                if (crit.List<Human_Token>().Count != 0)
                    objHuman_Token = crit.List<Human_Token>()[0];
                iMySession.Close();
            }

            return objHuman_Token;
        }
        #endregion
    }
}
