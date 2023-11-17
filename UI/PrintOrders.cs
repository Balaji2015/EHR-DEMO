using System;
using System.Linq;
using Acurus.Capella.Core.DomainObjects;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Acurus.Capella.Core.DTO;
using Acurus.Capella.DataAccess.ManagerObjects;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using System.Net;
using System.Text;
using iTextSharp.text.pdf.draw;
using Acurus.Capella.DataAccess.QuestWebServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Configuration;

namespace Acurus.Capella.UI
{
    public class PrintOrders
    {
        IList<FacilityLibrary> facilityList = new List<FacilityLibrary>();
        LabLocationManager objLabLocProxy = new LabLocationManager();
        static iTextSharp.text.Font normalFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        static iTextSharp.text.Font normalFont7 = iTextSharp.text.FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        static iTextSharp.text.Font reducedFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
        static iTextSharp.text.Font onlyBoldFont = iTextSharp.text.FontFactory.GetFont("Arial", 13, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
        static iTextSharp.text.Font redBoldFont = iTextSharp.text.FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
        static iTextSharp.text.Font QuestHeading = iTextSharp.text.FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
        static iTextSharp.text.Font QuestBigE = iTextSharp.text.FontFactory.GetFont("Arial", 50, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
        static iTextSharp.text.Font WhiteFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.WHITE);
        static iTextSharp.text.Font InnerHeading = iTextSharp.text.FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
        static iTextSharp.text.Font GrayFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.LIGHT_GRAY);
        static iTextSharp.text.Font GrayFont3 = iTextSharp.text.FontFactory.GetFont("Arial", 5, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.LIGHT_GRAY);
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        // FacilityLibrary facilityObj = null;
        string address = string.Empty;
        // float X = 50f, Y = 60f;
        public class CreateTableQuest
        {
            private string _FieldName = string.Empty;
            private string _FieldValue = string.Empty;
            private iTextSharp.text.Font _FontName;
            private int _ColoumSpan;
            public CreateTableQuest(string fieldName, string fieldValue, int coloumSpan, iTextSharp.text.Font fontName)
            {
                FieldName = fieldName;
                FieldValue = fieldValue;
                FontName = fontName;
                ColoumSpan = coloumSpan;
            }
            public string FieldName
            {
                get
                {
                    return _FieldName;
                }
                set
                {
                    _FieldName = value;

                }
            }
            public string FieldValue
            {
                get
                {
                    return _FieldValue;
                }
                set
                {
                    _FieldValue = value;

                }
            }
            public iTextSharp.text.Font FontName
            {
                get
                {
                    return _FontName;
                }
                set
                {
                    _FontName = value;

                }
            }
            public int ColoumSpan
            {
                get
                {
                    return _ColoumSpan;
                }
                set
                {
                    _ColoumSpan = value;

                }
            }
        }
        public static int CalculateAge(DateTime birthDate)
        {
            // cache the current time
            DateTime now = DateTime.Today; // today is fine, don't need the timestamp from now
            // get the difference in years
            int years = now.Year - birthDate.Year;
            // subtract another year if we're before the
            // birth day in the current year
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                --years;

            return years;
        }

        public string CallPrintLabAndImageOrders(string Path, ulong ulEncounter_ID, OrdersDTO objOrderDTOPrint, FillHumanDTO objHuman, string OrderType)
        {
            IList<OrderLabDetailsDTO> ilstOrderLabDetailsDTO = new List<OrderLabDetailsDTO>();
            IList<OrdersAssessment> ilstOrdersAssessment = new List<OrdersAssessment>();
            var serializer = new NetDataContractSerializer();
            OrdersManager objOrdersManager = new OrdersManager();
            OrdersDTO objOrderDTO = null;
            object objDTO;
            if (objOrderDTOPrint != null)
                objOrderDTO = objOrderDTOPrint;
            else
            {
                objDTO = (object)serializer.ReadObject(objOrdersManager.LoadOrders(ulEncounter_ID, ClientSession.PhysicianId, ClientSession.HumanId, OrderType, string.Empty, UtilityManager.ConvertToUniversal(), false));
                objOrderDTO = (OrdersDTO)objDTO;
            }
            string sPrintPathName = string.Empty;
            if (ulEncounter_ID == 0)
            {
                sPrintPathName = PrintLabAndImageOrdersMenuLevel(objOrderDTO, ClientSession.PhysicianId, objHuman, OrderType, Path);

            }
            else
                sPrintPathName = PrintLabAndImageOrders(objOrderDTO, ClientSession.PhysicianId, objHuman, OrderType, Path);

            return sPrintPathName;
        }
        public string PrintLabAndImageOrdersMenuLevel(OrdersDTO orderDetDTO, ulong Physician_ID, FillHumanDTO humanRecord, string OrderType, string Pathname)
        {
            PhysicianManager objPhysicianManager = new PhysicianManager();
            PhysicianLibrary objPhysician = new PhysicianLibrary();
            if (Physician_ID != 0)
                objPhysician = objPhysicianManager.GetphysiciannameByPhyID(Physician_ID)[0];

            IList<string> CustomPrintLabNames = new List<string>();
            CustomPrintLabNames.Add("QUEST DIAGNOSTICS");
            CustomPrintLabNames.Add("LABCORP");
            CustomPrintLabNames.Add("OUTPATIENT ORDER");
            var distinctLab = (from obj1 in orderDetDTO.ilstOrderLabDetailsDTO
                               where !CustomPrintLabNames.Contains(obj1.LabName.ToUpper())
                               group obj1 by obj1.OrdersSubmit.Lab_ID into g
                               select new
                               {
                                   g.Key,
                                   LocationID = from obj in orderDetDTO.ilstOrderLabDetailsDTO where obj.OrdersSubmit.Lab_ID == g.Key group obj by obj.OrdersSubmit.Lab_Location_ID into g1 select new { LocID = g1.Key }
                               });
            int i = 0;
            //  OrdersManager objOrdersManager = new OrdersManager();
            string sPrintPathName = string.Empty;
            foreach (var item in distinctLab)
            {
                foreach (var Loc in item.LocationID)
                {
                    IList<OrderLabDetailsDTO> ordList = (from obj in orderDetDTO.ilstOrderLabDetailsDTO where obj.OrdersSubmit.Lab_ID == Convert.ToUInt64(item.Key) && obj.OrdersSubmit.Lab_Location_ID == Convert.ToUInt64(Loc.LocID) select obj).ToList<OrderLabDetailsDTO>();
                    if (ordList != null)
                    {

                        //var groupedOrders3 = from u in ordList
                        //                     group u by (u.ObjOrder.Modified_Date_And_Time.ToString("yyyy-MM-dd") != "0001-01-01" ? u.ObjOrder.Modified_Date_And_Time : u.ObjOrder.Created_Date_And_Time) into g
                        //                     select new
                        //                     {
                        //                         OrderDetails = g.First<OrderLabDetailsDTO>(),
                        //                         OrderList = g.ToList<OrderLabDetailsDTO>(),
                        //                         Procedures = g.Select(u => (u.procedureCodeDesc))
                        //                     };

                        var groupedOrders = (from u in ordList
                                             group u by (u.ObjOrder.Created_Date_And_Time.ToString("yyyy-MM-dd")) into g
                                             select g).ToArray();


                        foreach (var order in groupedOrders)
                        {
                            IList<OrderLabDetailsDTO> orderList = (from o in order select o).ToList<OrderLabDetailsDTO>();

                            //if (i == 0)
                            //    humanRecord = objOrdersManager.GetHumanById(ordList[0].OrdersSubmit.Human_ID);
                            // for (int j = 0; j < ordList.Count;j++ )

                            if (Physician_ID == 0)
                            {
                                EncounterManager objEncMngr = new EncounterManager();
                                IList<Encounter> objEnc = new List<Encounter>();
                                objEnc = objEncMngr.GetEncounterByOrderSubmitID(Convert.ToInt32(orderDetDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Id));
                                if (objEnc.Count > 0)
                                {
                                    objPhysician.PhyFirstName = objEnc[0].Referring_Physician;
                                    objPhysician.PhyAddress1 = objEnc[0].Referring_Address;
                                    objPhysician.PhyTelephone = objEnc[0].Referring_Phone_No;
                                    objPhysician.PhyFax = objEnc[0].Referring_Fax_No;
                                    objPhysician.PhyNPI = objEnc[0].Referring_Provider_NPI;
                                }
                            }
                            if (OrderType == "DME ORDER")
                            {
                                var ordDMEList = (from obj in ordList
                                                  group obj by obj.OrdersSubmit.Id into g
                                                  select new
                                                  {
                                                      T1 = g.ToList<OrderLabDetailsDTO>()
                                                  }).ToList();

                                //int j = 0;

                                for (int cnt = 0; cnt < ordDMEList.Count; cnt++)
                                {
                                    sPrintPathName += PrintDMEOrdersForSelectedLab(ordDMEList[cnt].T1[0].OrdersSubmit, objPhysician, humanRecord, OrderType, Pathname);
                                }
                            }
                            else
                            {
                                sPrintPathName += PrintOrdersForSelectedLab(orderList, objPhysician, humanRecord, OrderType, orderDetDTO, Pathname);
                            }

                            i++;
                        }
                    }
                }
            }

            //Outpatient Order
            IList<OrderLabDetailsDTO> ordListOutpatient = (from obj in orderDetDTO.ilstOrderLabDetailsDTO where obj.LabName.ToUpper() == "OUTPATIENT ORDER" select obj).ToList<OrderLabDetailsDTO>();
            IList<ulong> ulOrderSubmitIDList = new List<ulong>();

            if (ordListOutpatient != null)
            {
                //if (ordListOutpatient.Count > 0)
                for (int iCount = 0; iCount < ordListOutpatient.Count; iCount++)
                {
                    IList<OrderLabDetailsDTO> ordListTempOutpatient = new List<OrderLabDetailsDTO>();
                    ordListTempOutpatient.Add(ordListOutpatient[iCount]);
                    ulOrderSubmitIDList.Add(ordListOutpatient[iCount].OrdersSubmit.Id);
                    sPrintPathName += PrintOrdersForOutpatientOrder(ordListTempOutpatient, objPhysician, humanRecord, orderDetDTO, Pathname, OrderType, ordListOutpatient);
                }
            }
            return sPrintPathName;
        }

        public string PrintLabAndImageOrders(OrdersDTO orderDetDTO, ulong Physician_ID, FillHumanDTO humanRecord, string OrderType, string Pathname)
        {
            PhysicianManager objPhysicianManager = new PhysicianManager();
            PhysicianLibrary objPhysician = objPhysicianManager.GetphysiciannameByPhyID(Physician_ID)[0];
            IList<string> CustomPrintLabNames = new List<string>();
            CustomPrintLabNames.Add("QUEST DIAGNOSTICS");
            CustomPrintLabNames.Add("LABCORP");
            CustomPrintLabNames.Add("OUTPATIENT ORDER");
            //FacilityManager FacilityList = new FacilityManager();
            //facilityList = FacilityList.GetFacilityList();
            var distinctLab = (from obj1 in orderDetDTO.ilstOrderLabDetailsDTO
                               where !CustomPrintLabNames.Contains(obj1.LabName.ToUpper())
                               group obj1 by obj1.OrdersSubmit.Lab_ID into g
                               select new
                               {
                                   g.Key,
                                   LocationID = from obj in orderDetDTO.ilstOrderLabDetailsDTO where obj.OrdersSubmit.Lab_ID == g.Key group obj by obj.OrdersSubmit.Lab_Location_ID into g1 select new { LocID = g1.Key }
                               });
            int i = 0;
            //  OrdersManager objOrdersManager = new OrdersManager();
            string sPrintPathName = string.Empty;
            foreach (var item in distinctLab)
            {
                foreach (var Loc in item.LocationID)
                {
                    IList<OrderLabDetailsDTO> ordList = (from obj in orderDetDTO.ilstOrderLabDetailsDTO where obj.OrdersSubmit.Lab_ID == Convert.ToUInt64(item.Key) && obj.OrdersSubmit.Lab_Location_ID == Convert.ToUInt64(Loc.LocID) select obj).ToList<OrderLabDetailsDTO>();
                    if (ordList != null)
                    {
                        if (ordList.Count > 0)
                        {
                            //if (i == 0)
                            //    humanRecord = objOrdersManager.GetHumanById(ordList[0].OrdersSubmit.Human_ID);
                            // for (int j = 0; j < ordList.Count;j++ )
                            if (OrderType == "DME ORDER")
                            {
                                var ordDMEList = (from obj in ordList
                                                  group obj by obj.OrdersSubmit.Id into g
                                                  select new
                                                  {
                                                      T1 = g.ToList<OrderLabDetailsDTO>()
                                                  }).ToList();

                                //int j = 0;

                                for (int cnt = 0; cnt < ordDMEList.Count; cnt++)
                                {
                                    sPrintPathName += PrintDMEOrdersForSelectedLab(ordDMEList[cnt].T1[0].OrdersSubmit, objPhysician, humanRecord, OrderType, Pathname);
                                }
                            }
                            else
                            {


                                sPrintPathName += PrintOrdersForSelectedLab(ordList, objPhysician, humanRecord, OrderType, orderDetDTO, Pathname);
                            }

                            i++;
                        }
                    }
                }
            }

            //Outpatient Order
            IList<OrderLabDetailsDTO> ordListOutpatient = (from obj in orderDetDTO.ilstOrderLabDetailsDTO where obj.LabName.ToUpper() == "OUTPATIENT ORDER" select obj).ToList<OrderLabDetailsDTO>();
            IList<ulong> ulOrderSubmitIDList = new List<ulong>();
            if (ordListOutpatient != null)
            {
                if (ordListOutpatient != null)
                {
                    //if (ordListOutpatient.Count > 0)
                    for (int iCount = 0; iCount < ordListOutpatient.Count; iCount++)
                    {
                        if (ulOrderSubmitIDList.Contains(ordListOutpatient[iCount].OrdersSubmit.Id) == false)
                        {
                            IList<OrderLabDetailsDTO> ordListTempOutpatient = new List<OrderLabDetailsDTO>();
                            ordListTempOutpatient.Add(ordListOutpatient[iCount]);
                            ulOrderSubmitIDList.Add(ordListOutpatient[iCount].OrdersSubmit.Id);
                            sPrintPathName += PrintOrdersForOutpatientOrder(ordListTempOutpatient, objPhysician, humanRecord, orderDetDTO, Pathname, OrderType, ordListOutpatient);
                        }
                    }
                }
            }

            return sPrintPathName;
        }
        public Human GetHumanByHumanID(ulong HumanID)
        {
            IList<Human> Humanlist = null;
            HumanManager objHumanManager = new HumanManager();
            Humanlist = objHumanManager.GetPatientDetailsUsingPatientInformattion(HumanID);
            if (Humanlist != null)
            {
                if (Humanlist.Count > 0)
                {
                    return Humanlist[0];
                }
            }
            return new Human();
        }

        //Added for PrintLabel
        public string PrintLabelfromOrders(string sAccountNumberForQuest, ulong uOrderSubmitID, string sPatientName, string Path, string sLab)
        {
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
            string sPathName = string.Empty;
            string folderPath = Path;
            Directory.CreateDirectory(folderPath);
            sPathName = folderPath + "\\" + sPatientName + "-" + uOrderSubmitID.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd hhmmss tt") + ".pdf";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPathName, FileMode.Create));
            iTextSharp.text.Rectangle pageSize = doc.PageSize;
            HeaderEventGenerate headerEvent = new HeaderEventGenerate();
            doc.Open();
            wr.PageEvent = headerEvent;
            headerEvent.OnStartPage(wr, doc);
            headerEvent.OnEndPage(wr, doc);
            PdfPTable patTable = new PdfPTable(new float[] { 100 });
            patTable.WidthPercentage = 100;
            PdfPCell cell = new PdfPCell(new Phrase(sLab, reducedFont));
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //cell.Colspan = 2;
            cell.Border = 0;
            patTable.AddCell(cell);
            cell = CreateCell("", "");
            //cell.Colspan = 2;
            cell.Border = 0;
            patTable.AddCell(cell);
            cell = CreateCell("Client #           : " + sAccountNumberForQuest, "");
            //cell.Colspan = 2;
            cell.Border = 0;
            patTable.AddCell(cell);
            //cell = CreateCell("", sAccountNumberForQuest);
            //cell.Border = 0;
            //patTable.AddCell(cell);
            cell = CreateCell("Lab Ref #        : " + "ACUR" + uOrderSubmitID.ToString(), "");
            //cell.Colspan = 2;
            cell.Border = 0;
            patTable.AddCell(cell);
            //cell = CreateCell("", "ACUR" + uOrderSubmitID.ToString());
            //cell.Border = 0;
            //patTable.AddCell(cell);
            cell = CreateCell("Patient Name : " + sPatientName, "");
            //cell.Colspan = 2;
            cell.Border = 0;
            patTable.AddCell(cell);
            //cell = CreateCell("", sPatientName);
            //cell.Border = 0;
            //patTable.AddCell(cell);
            doc.Add(patTable);
            doc.Close();
            return sPathName;
        }
        //End

        public string PrintOrdersForSelectedLab(IList<OrderLabDetailsDTO> ordList, PhysicianLibrary objPhysician, FillHumanDTO humanRecord, string OrderType, OrdersDTO orderDetDTO, string Path)
        {
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
            FacilityManager FacilityList = new FacilityManager();
            facilityList = FacilityList.GetFacilityList();
            string sPrintPathName = string.Empty;
            //string folderPath = System.Configuration.ConfigurationSettings.AppSettings["CapellaConfigurationSetttings"] + "\\" + System.Configuration.ConfigurationSettings.AppSettings["OrdersPrintPath"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
            //string folderPath = Path + "\\" + DateTime.Now.ToString("yyyyMMdd");
            string folderPath = Path;
            Directory.CreateDirectory(folderPath);

            var Createdate = (from o in ordList select o.ObjOrder.Created_Date_And_Time).OrderByDescending(c => c).ToArray();

            //For Bug Id :74659 
            DateTime dtESD = DateTime.MinValue;
            if (ordList != null && ordList.Count() > 0 && ordList[0].OrdersSubmit != null)
            {
                var SignatureDate = (from os in ordList select os.OrdersSubmit.Modified_Date_And_Time.ToString("dd-MM-yyyy hh:mm:ss") != "01-01-0001 12:00:00" ? os.OrdersSubmit.Modified_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") : os.OrdersSubmit.Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt")).OrderByDescending(a => a).ToArray();
                if (SignatureDate != null && SignatureDate.Count() > 0)
                    dtESD = Convert.ToDateTime(SignatureDate[0].ToString());

            }

            string strLabName = Regex.Replace(ordList[0].LabName, @"[^0-9a-zA-Z]+", " ");
            if (objPhysician != null)
            {
                //CAP-1143
                //sPrintPathName = folderPath + "\\" + humanRecord.Human_ID.ToString() + "_" + humanRecord.Last_Name + " " + humanRecord.MI + " " + humanRecord.First_Name + "_" + objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix + "_" + OrderType.Replace(' ', '_') + "_" + strLabName + "_" + ordList[0].LabLocName + "_" + UtilityManager.ConvertToLocal(Createdate[0]).ToString("yyyyMMdd hhmmss tt") + ".pdf";//ordList[0].ObjOrder.Created_Date_And_Time.ToString("yyyyMMdd hhmmss tt") + ".pdf";
                sPrintPathName = folderPath + "\\" + Guid.NewGuid().ToString() + ".pdf";
            }
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            iTextSharp.text.Rectangle pageSize = doc.PageSize;
            HeaderEventGenerate headerEvent = new HeaderEventGenerate();
            doc.Open();
            wr.PageEvent = headerEvent;
            headerEvent.OnStartPage(wr, doc);
            headerEvent.OnEndPage(wr, doc);

            Paragraph par = new Paragraph(DateTime.Now.ToString("dd-MMM-yyyy"), normalFont);
            par.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
            doc.Add(par);

            if (OrderType.ToUpper() == "DIAGNOSTIC ORDER")
            {
                doc.Add(new Paragraph("Lab Details:", reducedFont));
            }
            else if (OrderType.ToUpper() == "IMAGE ORDER")
            {
                doc.Add(new Paragraph("Imaging Center Details:", reducedFont));
            }
            doc.Add(new Paragraph(ordList[0].LabName, normalFont));



            LabLocationManager objLabLocationManager = new LabLocationManager();
            IList<LabLocation> locList = objLabLocationManager.GetLabLocationUsinglabID(ordList[0].OrdersSubmit.Lab_ID);

            //System.Xml.Linq.XDocument xmlLabLocation = System.Xml.Linq.XDocument.Load(Server.MapPath(@"ConfigXML\LabLocationList.xml"));
            if (locList != null)
            {
                if (locList.Count > 0)
                {
                    string labAddress = string.Empty, labCity = string.Empty;
                    LabLocation objLabLoc = null;
                    try
                    {
                        objLabLoc = (from obj in locList where obj.Id == ordList[0].OrdersSubmit.Lab_Location_ID select obj).ToList<LabLocation>()[0];
                    }
                    catch
                    {
                        objLabLoc = null;
                    }
                    if (objLabLoc != null)
                    {
                        if (objLabLoc.Street_Address1 != string.Empty && objLabLoc.Street_Address2 != string.Empty)
                        {
                            labAddress = objLabLoc.Street_Address1 + "\n" + objLabLoc.Street_Address2;
                        }
                        else
                            labAddress = objLabLoc.Street_Address1;
                        if (labAddress != string.Empty)
                        {
                            doc.Add(new Paragraph(labAddress, normalFont));
                        }

                        if (objLabLoc.City != string.Empty && objLabLoc.State != string.Empty)
                        {
                            labCity = objLabLoc.City + ", " + objLabLoc.State + " " + objLabLoc.ZipCode;
                        }
                        else
                            labCity = ordList[0].LabLocName;
                        if (labCity != string.Empty)
                        {
                            doc.Add(new Paragraph(labCity, normalFont));
                        }
                        if (objLabLoc.Phone_No != string.Empty)
                        {
                            doc.Add(new Paragraph("Phone Number: " + objLabLoc.Phone_No, normalFont));
                        }
                        if (objLabLoc.Fax_No != string.Empty)
                        {
                            doc.Add(new Paragraph("Fax Number: " + objLabLoc.Fax_No, normalFont));
                        }
                    }
                }
            }

            doc.Add(new Paragraph("\n"));

            doc.Add(new Paragraph("Client:", reducedFont));
            doc.Add(new Paragraph(System.Configuration.ConfigurationSettings.AppSettings["ClientName"], normalFont));

            doc.Add(new Paragraph("\n"));
            string City = string.Empty;
            if (humanRecord.City != string.Empty)
            {
                City = humanRecord.City;
                if (humanRecord.State != string.Empty)
                    City += ", " + humanRecord.State + " " + humanRecord.ZipCode;
            }
            PdfPTable patTable = new PdfPTable(new float[] { 40, 20, 20, 20 });
            patTable.WidthPercentage = 100;
            PdfPCell cell = new PdfPCell(new Phrase("Patient Details", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 4;
            patTable.AddCell(cell);
            cell = CreateCell("Patient Name: \n", humanRecord.Last_Name + "," + humanRecord.First_Name);//(new Phrase("Patient Name: \n" + humanRecord.Last_Name + "," + humanRecord.First_Name, normalFont));
            patTable.AddCell(cell);
            cell = CreateCell("Patient ID: \n", humanRecord.Human_ID.ToString());
            patTable.AddCell(cell);
            cell = CreateCell("Patient DOB: \n", humanRecord.Birth_Date.ToString("dd-MMM-yyyy"));
            patTable.AddCell(cell);
            cell = CreateCell("Patient Sex: \n", humanRecord.Sex);
            patTable.AddCell(cell);
            //cell = CreateCell("Patient SSN: \n", humanRecord.SSN);
            string sPhoneNumbers = (humanRecord.Home_Phone_No != "" ? "Home Phone Number : " + humanRecord.Home_Phone_No + "\n" : string.Empty) + (humanRecord.cell_phone_no != "" ? "Cell Phone Number : " + humanRecord.cell_phone_no + "\n" : string.Empty) + (humanRecord.cell_phone_no != "" ? "Work Phone Number : " + humanRecord.Work_Phone_No : string.Empty);
            cell = CreateCell("Patient Phone Numbers : \n", sPhoneNumbers);
            patTable.AddCell(cell);
            cell = CreateCell("Patient Address: \n", humanRecord.Street_Address1 + "\n" + City);
            cell.Colspan = 3;
            patTable.AddCell(cell);
            doc.Add(patTable);
            doc.Add(new Paragraph("\n"));
            PdfPTable phyTable = new PdfPTable(new float[] { 40, 60 });
            phyTable.WidthPercentage = 100;
            cell = new PdfPCell(new Phrase("Insurance Details", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 2;
            phyTable.AddCell(cell);
            if (humanRecord.PatientInsuredBag != null)
            {
                if (humanRecord.PatientInsuredBag.Count > 0)
                {
                    if (humanRecord.PatientInsuredBag[0].Insured_Human_ID != humanRecord.Human_ID)
                    {
                        Human insuredHuman = GetHumanByHumanID(humanRecord.PatientInsuredBag[0].Insured_Human_ID);
                        if (insuredHuman != null)
                        {
                            cell = CreateCell("Name of Policy Holder: \n", insuredHuman.Last_Name + "," + insuredHuman.First_Name);// +"\nRelationship to Patient: "+humanRecord.PatientInsuredBag[0].Relationship+ "\nAddress: " + insuredHuman.Street_Address1 + "\nCity: ", normalFont);
                            cell.AddElement(new Paragraph("Relationship to Patient: \n", reducedFont));
                            cell.AddElement(new Paragraph(humanRecord.PatientInsuredBag[0].Relationship, normalFont));
                            phyTable.AddCell(cell);
                            string City1 = string.Empty;
                            if (insuredHuman.City != string.Empty)
                            {
                                if (insuredHuman.Street_Address1 != string.Empty && insuredHuman.Street_Address2 != string.Empty)
                                    City1 = insuredHuman.Street_Address1 + "\n" + insuredHuman.Street_Address2;
                                else City1 = insuredHuman.Street_Address1;
                                if (insuredHuman.State != string.Empty && insuredHuman.City != string.Empty)
                                {
                                    City1 += "\n" + insuredHuman.City + ", " + insuredHuman.State + " " + insuredHuman.ZipCode;
                                }
                                else
                                    City1 = "\n" + insuredHuman.City;
                            }
                            cell = CreateCell("Insured Human Address: \n", City1);
                            phyTable.AddCell(cell);
                        }

                    }
                    InsurancePlanManager objInsurancePlanManager = new InsurancePlanManager();
                    InsurancePlan objInsPlan = objInsurancePlanManager.GetInsurancebyID(humanRecord.PatientInsuredBag[0].Insurance_Plan_ID)[0];
                    if (objInsPlan != null)
                    {
                        cell = CreateCell("Insurance Plan Name: \n", objInsPlan.Ins_Plan_Name);
                        phyTable.AddCell(cell);
                        cell = CreateCell("Insurance Address: \n", objInsPlan.Payer_Addrress1 + "\n" + objInsPlan.Payer_City);
                        phyTable.AddCell(cell);
                    }

                }
            }
            doc.Add(phyTable);
            doc.Add(new Paragraph("\n"));
            phyTable = new PdfPTable(new float[] { 40, 60 });
            phyTable.WidthPercentage = 100;
            cell = new PdfPCell(new Phrase("Ordering Facility & Physician Details", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 2;
            phyTable.AddCell(cell);
            if (facilityList != null && ordList != null && ordList.Count() > 0)
            {
                FacilityLibrary objFacility = (from obj in facilityList where obj.Fac_Name == ordList[0].OrdersSubmit.Facility_Name select obj).ToList<FacilityLibrary>()[0];
                if (objFacility != null)
                {
                    cell = CreateCell("Facility Name: \n", objFacility.Fac_Name);
                    phyTable.AddCell(cell);
                    //cell = CreateCell("Facility Address: \n", objFacility.Fac_Address1 + "\n" + objFacility.Fac_City);
                    //phyTable.AddCell(cell);
                    string sFacCity = string.Empty;
                    if (objFacility.Fac_City != string.Empty)
                    {
                        sFacCity = objFacility.Fac_City;
                        if (objFacility.Fac_State != string.Empty)
                            sFacCity += ", " + objFacility.Fac_State + " - " + objFacility.Fac_Zip;
                        if (objFacility.Fac_Telephone != string.Empty)
                            sFacCity += "\n" + "Phone: "+ objFacility.Fac_Telephone;
                        if (objFacility.Fac_Fax != string.Empty)
                            sFacCity += "\n" + "Fax: " + objFacility.Fac_Fax;

                        cell = CreateCell("Facility Address: \n", objFacility.Fac_Address1 + "\n" + sFacCity);
                        phyTable.AddCell(cell);
                    }
                    else
                    {
                        cell = CreateCell("Facility Address: \n", objFacility.Fac_Address1);
                        phyTable.AddCell(cell);
                    }
                }
            }
            if (objPhysician != null)
            {
                cell = CreateCell("Physician Name: \n", objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix);
                phyTable.AddCell(cell);
                cell = CreateCell("Physician NPI: \n", objPhysician.PhyNPI);
                phyTable.AddCell(cell);

                LabManager labMngr = new LabManager();
                IList<Lab> ilstLab = labMngr.GetLabList();
                Lab tempLab = new Lab();

                var orderlab = from l in ilstLab where l.Lab_Name == ordList[0].LabName select l;
                tempLab = orderlab.ToList<Lab>()[0];

                if (tempLab.Category != string.Empty)
                {
                    cell = CreateCell("Category: \n", tempLab.Category);
                    phyTable.AddCell(cell);
                    cell = CreateCell("", "");
                    phyTable.AddCell(cell);
                }
            }

            doc.Add(phyTable);

            doc.Add(new Paragraph("\n"));
            PdfPTable table = new PdfPTable(new float[] { 7, 50, 43 });
            table.WidthPercentage = 100;
            cell = new PdfPCell(new Phrase("Procedure(s) Ordered", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 4;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("S.No.", reducedFont));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Procedure", reducedFont));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Diagnosis", reducedFont));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            //var groupedOrders = from u in ordList
            //                    group u by u.ObjOrder.Modified_Date_And_Time into g
            //                    select new
            //                    {
            //                        OrderDetails = g.First<OrderLabDetailsDTO>(),
            //                        OrderList = g.ToList<OrderLabDetailsDTO>(),
            //                        Procedures = g.Select(u => (u.procedureCodeDesc))
            //                    };

            var groupedOrders = from u in ordList
                                group u by u.OrdersSubmit.Id into g
                                select new
                                {
                                    OrderDetails = g.First<OrderLabDetailsDTO>(),
                                    OrderList = g.ToList<OrderLabDetailsDTO>(),
                                    Procedures = g.Select(u => (u.procedureCodeDesc))
                                };

            int sno = 0;
            foreach (var objOrdLab in groupedOrders)
            {
                string sAssessment = string.Empty, sProcedures = string.Empty;
                Orders obj = objOrdLab.OrderDetails.ObjOrder;
                OrdersSubmit objSub = objOrdLab.OrderDetails.OrdersSubmit;
                //IList<OrdersAssessment> ordAssessment = (from obj1 in orderDetDTO.OrderAssList where obj1.Order_ID == obj.Id select obj1).ToList<OrdersAssessment>();
                IList<OrdersAssessment> ordAssessment = (from obj1 in orderDetDTO.OrderAssList where obj1.Order_Submit_ID == obj.Order_Submit_ID select obj1).ToList<OrdersAssessment>();
                cell = new PdfPCell(new Phrase((++sno).ToString(), normalFont));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
                for (int i = 0; i < ordAssessment.Count; i++)
                {
                    if (i != 0)
                    {
                        sAssessment += "; " + ordAssessment[i].ICD + "-" + ordAssessment[i].ICD_Description;
                    }
                    else
                        sAssessment = ordAssessment[i].ICD + "-" + ordAssessment[i].ICD_Description;

                }
                for (int k = 0; k < objOrdLab.Procedures.ToList<string>().Count; k++)
                {
                    if (k != 0)
                    {
                        sProcedures += "; " + objOrdLab.Procedures.ToList<string>()[k];
                    }
                    else
                        sProcedures = objOrdLab.Procedures.ToList<string>()[k];

                }
                //if (obj.Patient_Instructions != string.Empty)
                //{
                //    sAssessment += Environment.NewLine + Environment.NewLine + "Preparations/Diagnosis: " + obj.Patient_Instructions;
                //}

                if (objSub.Order_Notes != string.Empty)
                {
                    sAssessment += Environment.NewLine + Environment.NewLine + "Notes: " + objSub.Order_Notes;
                }
                table.AddCell(new Phrase(sProcedures, normalFont));
                table.AddCell(new Phrase(sAssessment, normalFont));
            }
            doc.Add(new Paragraph("\n"));
            doc.Add(table);
            doc.Add(new Paragraph("\n"));
            //For Bug Id :74659 
            //string sDate = SignatureDate[0].ToString(); //ordList[0].OrdersSubmit.Modified_Date_And_Time.ToString("dd-MM-yyyy hh:mm:ss") != "01-01-0001 12:00:00" ? ordList[0].OrdersSubmit.Modified_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") : ordList[0].OrdersSubmit.Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt");
            //doc.Add(new Paragraph("Electronically Signed by " + objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix + " On " + UtilityManager.ConvertToLocal(Convert.ToDateTime(sDate)).ToString("dd-MMM-yyyy hh:mm:ss tt"), reducedFont));
            string spara = string.Empty;

            if (dtESD == DateTime.MinValue)
                spara = "Electronically Signed by " + objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix + " On ";
            else
                spara = "Electronically Signed by " + objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix + " On " + UtilityManager.ConvertToLocal(dtESD).ToString("dd-MMM-yyyy hh:mm:ss tt");


            doc.Add(new Paragraph(spara, reducedFont));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("SOME INSURANCES OR MEDICARE MAY NOT PAY FOR VITAMIN D LEVEL, PLEASE VERIFY WITH THE LAB AS YOU MAY GET BILLED FOR IT", reducedFont));
            doc.Add(new Paragraph("\n"));
            doc.Close();
            return sPrintPathName;
            //System.Diagnostics.Process.Start(sPrintPathName);
        }

        public string PrintOrdersForOutpatientOrder(IList<OrderLabDetailsDTO> ordList, PhysicianLibrary objPhysician, FillHumanDTO humanRecord, OrdersDTO orderDetDTO, string Path, string OrderType, IList<OrderLabDetailsDTO> OverallOrdList)
        {
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
            FacilityManager FacilityList = new FacilityManager();
            facilityList = FacilityList.GetFacilityList();
            string sPrintPathName = string.Empty;
            string folderPath = Path;
            Directory.CreateDirectory(folderPath);

            var Createdate = (from o in ordList select o.ObjOrder.Created_Date_And_Time).OrderByDescending(c => c).ToArray();

            var SignatureDate = (from os in ordList select os.OrdersSubmit.Modified_Date_And_Time.ToString("dd-MM-yyyy hh:mm:ss") != "01-01-0001 12:00:00" ? os.OrdersSubmit.Modified_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") : os.OrdersSubmit.Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt")).OrderByDescending(a => a).ToArray();

            string strLabName = Regex.Replace(ordList[0].LabName, @"[^0-9a-zA-Z]+", " ");
            if (objPhysician != null)
            {

                //sPrintPathName = folderPath + "\\" + humanRecord.Human_ID.ToString() + "_" + humanRecord.Last_Name + " " + humanRecord.MI + " " + humanRecord.First_Name + "_" + objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix + "_" + OrderType.Replace(' ', '_') + "_" + strLabName + "_" + ordList[0].LabLocName + "_" + UtilityManager.ConvertToLocal(Createdate[0]).ToString("yyyyMMdd hhmmss tt") + ".pdf";//ordList[0].ObjOrder.Created_Date_And_Time.ToString("yyyyMMdd hhmmss tt") + ".pdf";
                sPrintPathName = folderPath + "\\ACUR_" + ordList[0].OrdersSubmit.Id.ToString() + "_" + humanRecord.Last_Name + " " + humanRecord.MI + " " + humanRecord.First_Name + "_" + objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix + "_" + OrderType.Replace(' ', '_') + "_" + strLabName + "_" + ordList[0].LabLocName + "_" + UtilityManager.ConvertToLocal(Createdate[0]).ToString("yyyyMMdd hhmmss tt") + ".pdf";//ordList[0].ObjOrder.Created_Date_And_Time.ToString("yyyyMMdd hhmmss tt") + ".pdf";
            }
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            iTextSharp.text.Rectangle pageSize = doc.PageSize;

            string sFooter = string.Empty;
            sFooter = "Electronically Signed by " + objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + Environment.NewLine;

            //FacilityLibrary objFacility = (from obj in facilityList where obj.Fac_Name == ordList[0].OrdersSubmit.Facility_Name select obj).ToList<FacilityLibrary>()[0];
            //if (objFacility != null)
            //{
            //    sFooter += objFacility.Fac_Name + ", " + objFacility.Fac_Address1 + ", " + objFacility.Fac_City + ", " + objFacility.Fac_State + " " + objFacility.Fac_Zip+Environment.NewLine;
            //}

            //sFooter+="Phone No: " + objFacility.Fac_Telephone+ "       "+"Fax No: " + objFacility.Fac_Fax;

            OutpatientHeaderEventGenerate headerEvent = new OutpatientHeaderEventGenerate(sFooter + "at " + UtilityManager.ConvertToLocal(Createdate[0]).ToString("dd-MMM-yyyy hh:mm:ss tt"));
            doc.Open();
            wr.PageEvent = headerEvent;
            headerEvent.OnStartPage(wr, doc);
            headerEvent.OnEndPage(wr, doc);
            IList<string> HeaderList = new List<string>();
            IList<string> ValueList = new List<string>();

            LabLocationManager objLabLocationManager = new LabLocationManager();

            Paragraph par = new Paragraph(System.Configuration.ConfigurationSettings.AppSettings["ClientName"], onlyBoldFont);
            par.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            doc.Add(par);

            doc.Add(new Paragraph("\n"));

            if (objPhysician != null)
            {
                par = new Paragraph(objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix, normalFont);
                par.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                doc.Add(par);
            }

            doc.Add(new Paragraph("\n"));

            par = new Paragraph("OUTPATIENT ORDERS", onlyBoldFont);
            par.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            doc.Add(par);

            doc.Add(new Paragraph("\n"));

            PdfPTable patTable = new PdfPTable(new float[] { 25, 35, 20, 20 });
            patTable.WidthPercentage = 100;

            PdfPCell cell;

            EncounterManager encMngr = new EncounterManager();
            IList<Encounter> encList = encMngr.GetEncounterByEncounterID(ordList[0].OrdersSubmit.Encounter_ID);

            ReferralOrderManager refMngr = new ReferralOrderManager();
            ReferralOrderDTO refOrdList = refMngr.GetReferralOrderUsingEncounterID(ordList[0].OrdersSubmit.Encounter_ID);

            cell = new PdfPCell(new Phrase("Date: ", reducedFont));
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);

            if (encList.Count > 0)
            {
                cell = new PdfPCell(new Phrase(UtilityManager.ConvertToLocal(encList[0].Date_of_Service).ToString("dd-MMM-yyyy"), normalFont));
                cell.Border = Rectangle.NO_BORDER;
                patTable.AddCell(cell);
            }
            else
            {
                cell = new PdfPCell(new Phrase(DateTime.Now.ToString("dd-MMM-yyyy"), normalFont));
                cell.Border = Rectangle.NO_BORDER;
                patTable.AddCell(cell);
            }
            cell = new PdfPCell(new Phrase(string.Empty, reducedFont));
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);

            if (ordList[0].OrdersSubmit.Stat == "Y" && ordList[0].OrdersSubmit.Is_Urgent == "N")
            {
                cell = new PdfPCell(new Phrase("Stat", normalFont));
                cell.Border = Rectangle.NO_BORDER;
                patTable.AddCell(cell);
            }
            else if (ordList[0].OrdersSubmit.Is_Urgent == "Y" && ordList[0].OrdersSubmit.Stat == "N")
            {
                cell = new PdfPCell(new Phrase("Urgent", normalFont));
                cell.Border = Rectangle.NO_BORDER;
                patTable.AddCell(cell);
            }
            else if (ordList[0].OrdersSubmit.Is_Urgent == "Y" && ordList[0].OrdersSubmit.Stat == "Y")
            {
                cell = new PdfPCell(new Phrase("Stat / Urgent", normalFont));
                cell.Border = Rectangle.NO_BORDER;
                patTable.AddCell(cell);
            }
            else
            {
                cell = new PdfPCell(new Phrase("Routine", normalFont));
                cell.Border = Rectangle.NO_BORDER;
                patTable.AddCell(cell);
            }

            cell = new PdfPCell(new Phrase("Patient Name: ", reducedFont));
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);
            cell = new PdfPCell(new Phrase(humanRecord.Last_Name + "," + humanRecord.First_Name + " " + humanRecord.MI, normalFont));
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("DOB: ", reducedFont));
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);
            cell = new PdfPCell(new Phrase(humanRecord.Birth_Date.ToString("dd-MMM-yyyy"), normalFont));
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Insurance: ", reducedFont));
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);
            string sInsName = string.Empty;
            if (humanRecord.PatientInsuredBag != null)
            {
                if (humanRecord.PatientInsuredBag.Count > 0)
                {
                    for (int iCount = 0; iCount <= humanRecord.PatientInsuredBag.Count - 1; iCount++)
                    {
                        if (humanRecord.PatientInsuredBag[iCount].Insurance_Type.ToUpper() == "PRIMARY")
                        {
                            InsurancePlanManager objInsurancePlanManager = new InsurancePlanManager();
                            InsurancePlan objInsPlan = objInsurancePlanManager.GetInsurancebyID(humanRecord.PatientInsuredBag[iCount].Insurance_Plan_ID)[0];

                            if (objInsPlan != null)
                            {
                                CarrierManager carrierMngr = new CarrierManager();
                                sInsName = carrierMngr.GetCarrierUsingId(Convert.ToUInt64(objInsPlan.Carrier_ID)).Carrier_Name + " - " + objInsPlan.Ins_Plan_Name;
                            }
                        }
                    }
                }
            }
            cell = new PdfPCell(new Phrase(sInsName, normalFont));
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(string.Empty, reducedFont));
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);
            cell = new PdfPCell(new Phrase(string.Empty, reducedFont));
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Indications for study/Dx: ", reducedFont));
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);
            cell = new PdfPCell(new Phrase(ordList[0].OrdersSubmit.Order_Notes, normalFont));
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(string.Empty, reducedFont));
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);
            cell = new PdfPCell(new Phrase(string.Empty, reducedFont));
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);

            doc.Add(patTable);

            PdfPTable table = new PdfPTable(new float[] { 50, 43 });
            table.WidthPercentage = 100;
            cell = new PdfPCell(new Phrase("Procedure(s) Ordered", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Procedure", reducedFont));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Diagnosis", reducedFont));
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            //var groupedOrders = from u in ordList
            //                    group u by u.OrdersSubmit.Id into g
            //                    select new
            //                    {
            //                        OrderDetails = g.First<OrderLabDetailsDTO>(),
            //                        OrderList = g.ToList<OrderLabDetailsDTO>(),
            //                        Procedures = g.Select(u => (u.procedureCodeDesc))
            //                    };

            var groupedOrders = from u in OverallOrdList
                                where u.OrdersSubmit.Id == ordList[0].OrdersSubmit.Id
                                group u by u.OrdersSubmit.Id into g
                                select new
                                {
                                    OrderDetails = g.First<OrderLabDetailsDTO>(),
                                    OrderList = g.ToList<OrderLabDetailsDTO>(),
                                    Procedures = g.Select(u => (u.procedureCodeDesc))
                                };

            // int sno = 0;
            string sICD = string.Empty;
            foreach (var objOrdLab in groupedOrders)
            {
                string sAssessment = string.Empty, sProcedures = string.Empty;
                Orders obj = objOrdLab.OrderDetails.ObjOrder;
                OrdersSubmit objSub = objOrdLab.OrderDetails.OrdersSubmit;
                IList<OrdersAssessment> ordAssessment = (from obj1 in orderDetDTO.OrderAssList where obj1.Order_Submit_ID == obj.Order_Submit_ID select obj1).ToList<OrdersAssessment>();
                for (int i = 0; i < ordAssessment.Count; i++)
                {
                    if (i != 0)
                    {
                        sAssessment += Environment.NewLine + ordAssessment[i].ICD + "-" + ordAssessment[i].ICD_Description;
                        if (sICD == string.Empty)
                            sICD = ordAssessment[i].ICD;
                        else
                            sICD += "," + ordAssessment[i].ICD;
                    }
                    else
                    {
                        sAssessment = ordAssessment[i].ICD + "-" + ordAssessment[i].ICD_Description;
                        if (sICD == string.Empty)
                            sICD = ordAssessment[i].ICD;
                        else
                            sICD += "," + ordAssessment[i].ICD;
                    }
                }
                for (int k = 0; k < objOrdLab.Procedures.ToList<string>().Count; k++)
                {
                    if (k != 0)
                    {
                        sProcedures += Environment.NewLine + objOrdLab.Procedures.ToList<string>()[k];
                    }
                    else
                        sProcedures = objOrdLab.Procedures.ToList<string>()[k];

                }

                table.AddCell(new Phrase(sProcedures, normalFont));
                table.AddCell(new Phrase(sAssessment, normalFont));
            }

            doc.Add(new Paragraph("\n"));
            doc.Add(table);
            doc.Add(new Paragraph("\n"));

            patTable = new PdfPTable(new float[] { 100 });
            patTable.WidthPercentage = 100;
            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("Follow up");
            string sFollowup = string.Empty;
            if (encList.Count > 0 && encList[0].Return_In_Months != 0)
                sFollowup += encList[0].Return_In_Months.ToString() + " Month(s) ";
            if (encList.Count > 0 && encList[0].Return_In_Weeks != 0)
                sFollowup += encList[0].Return_In_Weeks.ToString() + " Week(s) ";
            if (encList.Count > 0 && encList[0].Return_In_Days != 0)
                sFollowup += encList[0].Return_In_Days.ToString() + " Day(s) ";
            if (encList.Count > 0 && encList[0].Is_PRN == "Y")
            {
                if (sFollowup == string.Empty)
                    sFollowup += "PRN ";
                else
                    sFollowup += "or PRN ";
            }
            if (encList.Count > 0 && encList[0].Is_After_Studies == "Y")
            {
                if (sFollowup == string.Empty)
                    sFollowup += "After Studies";
                else
                    sFollowup += "or After Studies";
            }
            ValueList.Add(sFollowup);
            cell = CreateCellForDataFromDBWithLeftAlignment(HeaderList, ValueList, 2, false);
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);

            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("Referral");
            var reff = from rf in refOrdList.RefOrdAssList where sICD.Contains(rf.ICD) select rf;
            IList<ReferralOrdersAssessment> refOrderAssessList = reff.ToList<ReferralOrdersAssessment>();
            string sReferral = string.Empty;
            IList<string> refNameList = new List<string>();

            for (int iCount = 0; iCount <= refOrderAssessList.Count - 1; iCount++)
            {
                var refOrd = from rOrd in refOrdList.RefOrdList where rOrd.Id == refOrderAssessList[iCount].Referral_Order_ID select rOrd;
                IList<ReferralOrder> refOrderList = refOrd.ToList<ReferralOrder>();

                if (refNameList.Contains(refOrderList[0].To_Physician_Name) == false)
                {
                    if (sReferral == string.Empty)
                        sReferral = refOrderList[0].To_Physician_Name + Environment.NewLine;
                    else
                        sReferral += "  " + refOrderList[0].To_Physician_Name + Environment.NewLine;
                    refNameList.Add(refOrderList[0].To_Physician_Name);
                }
            }

            ValueList.Add(sReferral);
            cell = CreateCellForDataFromDBWithLeftAlignment(HeaderList, ValueList, 2, false);
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);

            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("Surgery Coordinator involved");
            if (encList.Count > 0 && encList[0].Proceed_with_Surgery_Planned == "Y")
            {
                ValueList.Add("Yes");
            }
            else
            {
                ValueList.Add("No");
            }
            cell = CreateCellForDataFromDBWithLeftAlignment(HeaderList, ValueList, 2, false);
            cell.Border = Rectangle.NO_BORDER;
            patTable.AddCell(cell);

            doc.Add(patTable);

            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("\n"));

            FacilityLibrary objFacility = (from obj in facilityList where obj.Fac_Name == ordList[0].OrdersSubmit.Facility_Name select obj).ToList<FacilityLibrary>()[0];
            if (objFacility != null)
            {
                if (objFacility.Fac_Name.ToUpper() == objFacility.Fac_Address1.ToUpper())
                    sFooter += objFacility.Fac_Name + ", " + objFacility.Fac_City + ", " + objFacility.Fac_State + " " + objFacility.Fac_Zip + Environment.NewLine;
                else
                    sFooter += objFacility.Fac_Name + ", " + objFacility.Fac_Address1 + ", " + objFacility.Fac_City + ", " + objFacility.Fac_State + " " + objFacility.Fac_Zip + Environment.NewLine;
            }

            sFooter += "Phone No: " + objFacility.Fac_Telephone + "       " + "Fax No: " + objFacility.Fac_Fax;

            doc.Add(new Paragraph(new Phrase(sFooter, normalFont)));

            doc.Add(new Paragraph("\n"));
            doc.Close();
            return sPrintPathName;
            //System.Diagnostics.Process.Start(sPrintPathName);
        }

        private PdfPCell CreateCell(string HeaderText, string ValueText)
        {
            PdfPCell cell = new PdfPCell();
            Paragraph par = new Paragraph(HeaderText, reducedFont);
            cell.AddElement(par);
            par = new Paragraph(ValueText, normalFont);
            cell.AddElement(par);
            return cell;
        }

        private PdfPCell CreateCellforPrintReceipt(string sText, string sType)
        {
            iTextSharp.text.Font normalFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font reducedFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            PdfPCell cell = new PdfPCell();

            if (sType == "Header")
            {
                Paragraph par = new Paragraph(sText, reducedFont);

                cell.AddElement(par);
            }
            else if (sType == "Text")
            {
                Paragraph par = new Paragraph(sText, normalFont);
                cell.AddElement(par);
            }

            //par = new Paragraph(ValueText, normalFont);
            //cell.AddElement(par);
            return cell;
        }

        public class HeaderEventGenerate : PdfPageEventHelper
        {
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);
                PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.Rectangle pageSize = document.PageSize;
                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                cb.SetTextMatrix(pageSize.GetRight(220), pageSize.GetTop(40));
                //cb.ShowText(frmRxOrder.patientInfo);
                cb.EndText();
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);
                PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.Rectangle pageSize = document.PageSize;
                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                cb.SetTextMatrix(pageSize.GetRight(70), pageSize.GetBottom(40));
                cb.ShowText("Page " + writer.PageNumber.ToString());
                cb.EndText();
            }
        }

        public class OutpatientHeaderEventGenerate : PdfPageEventHelper
        {
            string FooterContent = string.Empty;
            public OutpatientHeaderEventGenerate(string _FooterContent)
            {
                FooterContent = _FooterContent;
            }

            public override void OnStartPage(PdfWriter writer, Document document)
            {
                //base.OnStartPage(writer, document);
                //PdfContentByte cb = writer.DirectContent;
                //iTextSharp.text.Rectangle pageSize = document.PageSize;
                //cb.BeginText();
                //cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9);
                //cb.SetTextMatrix(pageSize.GetRight(220), pageSize.GetTop(40));
                ////cb.ShowText(FooterContent);
                //cb.EndText();
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);
                PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.Rectangle pageSize = document.PageSize;
                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 7);
                cb.SetTextMatrix(pageSize.GetLeft(50), pageSize.GetBottom(40));
                cb.ShowText(FooterContent);
                cb.EndText();
            }
        }

        public class InterpretationNoteHeaderEventGenerate : PdfPageEventHelper
        {
            string HeaderContent = string.Empty;
            string FooterContent = string.Empty;
            public InterpretationNoteHeaderEventGenerate(string _HeaderContent, string _FooterContent)
            {
                HeaderContent = _HeaderContent;
                FooterContent = _FooterContent;
            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);
                PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.Rectangle pageSize = document.PageSize;
                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                cb.SetTextMatrix(pageSize.GetRight(130), pageSize.GetTop(40));
                cb.ShowText(HeaderContent); //frmRxOrder.patientInfo);
                cb.EndText();
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);
                PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.Rectangle pageSize = document.PageSize;
                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                cb.SetTextMatrix(pageSize.GetLeft(20), pageSize.GetBottom(40));
                cb.ShowText(FooterContent); //"Page " + writer.PageNumber.ToString());
                cb.EndText();
            }
        }

        public class FaceSheetHeaderEventGenerate : PdfPageEventHelper
        {
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);
                PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.Rectangle pageSize = document.PageSize;
                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                int i = 380;
                cb.SetTextMatrix(pageSize.GetRight(i), pageSize.GetTop(40));
                
                string sFaceSheet = string.Empty;
                var client = from c in ApplicationObject.ClientList where c.Legal_Org == ClientSession.LegalOrg select c;
                IList<Client> currentClientList = client.ToList<Client>();

                if (currentClientList.Count > 0)
                {
                    sFaceSheet = currentClientList[0].Client_Full_Name;
                }

                //cb.ShowText(ConfigurationManager.AppSettings["ClientName"].ToString());
                 cb.ShowText(sFaceSheet);
                cb.EndText();
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);
                PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.Rectangle pageSize = document.PageSize;
                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                cb.SetTextMatrix(pageSize.GetRight(70), pageSize.GetBottom(40));
                cb.ShowText("Page " + writer.PageNumber.ToString());
                cb.EndText();
            }
        }

        ////public void PrintRxOrders(RxOrderDetailsDTO objRxDetailsDTO, Human humanRecord, PhysicianLibrary objPhysician)
        //{
        //    IList<string> patInfo = new List<string>();
        //    IList<string> facInfo = new List<string>();
        //    IList<string> rxOrdInfo = new List<string>();
        //    IList<object> rxOrderList = new List<object>();
        //    IList<FacilityLibrary> facilityList = AllLibraries.Instance.GetFacilityList();
        //    if (facilityList.Count > 0)
        //    {
        //        facilityObj = (from obj in facilityList where obj.Fac_Name == ClientSession.sFacilityName select obj).ToList<FacilityLibrary>()[0];
        //    }
        //    if (humanRecord != null)
        //    {
        //        patInfo.Add("Patient Name: " + humanRecord.Last_Name + "," + humanRecord.First_Name);
        //        if (humanRecord.Street_Address2 != string.Empty)
        //            address = humanRecord.Street_Address1 + "," + humanRecord.Street_Address2;
        //        else
        //            address = humanRecord.Street_Address1;
        //        if (address != string.Empty)
        //        {
        //            patInfo.Add("Patient Address:" + address);
        //        }
        //        patInfo.Add("Patient DOB: " + humanRecord.Birth_Date.ToString("dd-MMM-yyyy"));
        //        patInfo.Add("Patient Account #: " + humanRecord.Id.ToString());
        //        //patientInfo = humanRecord.Last_Name + "," + humanRecord.First_Name + " " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");//+ humanRecord.Id.ToString();
        //    }

        //    if (facilityObj != null)
        //    {
        //        facInfo.Add("Facility Name: " + facilityObj.Fac_Name);
        //        if (facilityObj.Fac_Address2 != string.Empty)
        //            address = facilityObj.Fac_Address1 + "," + facilityObj.Fac_Address2;
        //        else
        //            address = facilityObj.Fac_Address1;
        //        facInfo.Add("Facility Address: " + address);
        //        facInfo.Add("Facility City: " + facilityObj.Fac_City + "," + facilityObj.Fac_State + " " + facilityObj.Fac_Zip);
        //        facInfo.Add("Facility Phone No.: " + facilityObj.Fac_Telephone);
        //        //facilityInfo = facilityObj.Fac_Name + "\n" + facilityObj.Fac_Address1 + " " + facilityObj.Fac_Address2 + "\n"
        //        //    + facilityObj.Fac_City + ", " + facilityObj.Fac_State + " " + facilityObj.Fac_Zip + "\n" + facilityObj.Fac_Telephone;
        //    }
        //    //frmRxOrder.patientInfo = humanRecord.Last_Name + "," + humanRecord.First_Name + " " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");

        //    foreach (RxOrderPharmacyDetailsDTO obj in objRxDetailsDTO.RxOrderPharmList)
        //    {
        //        rxOrdInfo = new List<string>();
        //        string Sig = string.Empty, Dispense = string.Empty, Refill = string.Empty;
        //        rxOrdInfo.Add("Date: " + obj.objRxOrder.Created_Date_And_Time.ToString("dd-MMM-yyyy"));
        //        rxOrdInfo.Add(obj.objRxOrder.Drug_Name + " " + obj.objRxOrder.Strength);
        //        if (obj.objRxOrder.Dose != 0)
        //        {
        //            Sig = "Take" + " " + obj.objRxOrder.Dose.ToString() + " " + obj.objRxOrder.Unit + " " + obj.objRxOrder.Frequency + " " + "time(s) a day for" + " " + obj.objRxOrder.Days_supply;
        //        }
        //        rxOrdInfo.Add("Sig: " + Sig);
        //        if (obj.objRxOrder.Dispense != 0)
        //        {
        //            Dispense = obj.objRxOrder.Dispense.ToString();
        //        }
        //        rxOrdInfo.Add("Dispense: " + Dispense);
        //        if (obj.objRxOrder.Number_Of_Refills != 0)
        //        {
        //            Refill = obj.objRxOrder.Number_Of_Refills.ToString();
        //        }
        //        rxOrdInfo.Add("Refills: " + Refill);
        //        if (obj.objRxOrder.Notes != string.Empty)
        //        {
        //            rxOrdInfo.Add(obj.objRxOrder.Notes);
        //        }
        //        rxOrderList.Add(rxOrdInfo);


        //    }
        //    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        //    iTextSharp.text.Image addImage = iTextSharp.text.Image.GetInstance(global::Acurus.Capella.FrontOffice.Properties.Resources.Rx, BaseColor.BLACK);
        //    addImage.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
        //    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
        //    string sPrintPathName = string.Empty;
        //    string pathFromConfig = System.Configuration.ConfigurationSettings.AppSettings["RXPrintPath"];
        //    string folderPath = pathFromConfig + "\\" + DateTime.Now.ToString("yyyyMMdd");
        //    Directory.CreateDirectory(folderPath);
        //    sPrintPathName = folderPath + "\\" + humanRecord.Id.ToString() + "_" + humanRecord.Last_Name + " " + humanRecord.MI + " " + humanRecord.First_Name + "_" + objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix + "_RxOrder_" + DateTime.Now.ToString("yyyyMMdd hhmmss tt") + ".pdf";
        //    PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
        //    iTextSharp.text.Rectangle pageSize = doc.PageSize;
        //    HeaderEventGenerate headerEvent = new HeaderEventGenerate();
        //    doc.Open();
        //    wr.PageEvent = headerEvent;
        //    headerEvent.OnStartPage(wr, doc);
        //    headerEvent.OnEndPage(wr, doc);
        //    X = 50f;
        //    Y = 60f;
        //    PdfContentByte cb = wr.DirectContent;
        //    cb.BeginText();

        //    cb.SetFontAndSize(bfTimes, 10);
        //    for (int i = 0; i < patInfo.Count; i++)
        //    {

        //        cb.SetTextMatrix(pageSize.GetLeft(X), pageSize.GetTop(Y));
        //        cb.ShowText(patInfo[i]);
        //        Y += 13;
        //    }
        //    for (int i = 0; i < facInfo.Count; i++)
        //    {
        //        cb.SetTextMatrix(pageSize.GetLeft(X), pageSize.GetTop(Y));
        //        cb.ShowText(facInfo[i]);
        //        Y += 13;

        //    }


        //    foreach (IList<string> rxOrd in rxOrderList)
        //    {
        //        if (Y >= pageSize.Height - 80)
        //        {
        //            cb.EndText();
        //            Y = 60f;
        //            doc.NewPage();
        //            cb.SetFontAndSize(bfTimes, 11);
        //            cb.BeginText();
        //        }
        //        cb.MoveTo(pageSize.GetLeft(X), pageSize.GetTop(Y));
        //        cb.LineTo(pageSize.GetRight(X), pageSize.GetTop(Y));
        //        cb.Stroke();
        //        Y += 13;
        //        addImage.SetAbsolutePosition(pageSize.GetLeft(X), pageSize.GetTop(Y + 15f));
        //        addImage.ScalePercent(35f);
        //        doc.Add(addImage);
        //        for (int i = 0; i < rxOrd.Count; i++)
        //        {
        //            if (Y >= pageSize.Height - 50)
        //            {
        //                cb.EndText();
        //                Y = 60f;
        //                doc.NewPage();
        //                cb.SetFontAndSize(bfTimes, 11);
        //                cb.BeginText();
        //            }
        //            if (i == 0)
        //            {
        //                cb.SetTextMatrix(pageSize.GetRight(X + 85), pageSize.GetTop(Y));
        //                cb.ShowText(rxOrd[i]);
        //                Y += 13;

        //            }
        //            else
        //            {
        //                cb.SetTextMatrix(pageSize.GetLeft(X + 50f), pageSize.GetTop(Y));
        //                cb.ShowText(rxOrd[i]);
        //                Y += 13;
        //            }
        //        }


        //    }
        //    //cb.SetTextMatrix(pageSize.GetRight(X + 50f), pageSize.GetBottom(Y-pageSize.Height-50f));
        //    //cb.ShowText("Physician Signature");
        //    cb.EndText();
        //    doc.Close();
        //    System.Diagnostics.Process.Start(sPrintPathName);
        //}

        public string PrintImmunizationOrders(ImmunizationDTO objImmunizationFill, PhysicianLibrary objPhysician, Human humanRecord, string Path)
        {

            FacilityManager objFacilityManager = new FacilityManager();
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
            facilityList = objFacilityManager.GetFacilityList();
            string sPrintPathName = string.Empty;
            //string folderPath = System.Configuration.ConfigurationSettings.AppSettings["CapellaConfigurationSetttings"] + "\\" + System.Configuration.ConfigurationSettings.AppSettings["ImmunizationPrintPath"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
            string folderPath = Path;
            Directory.CreateDirectory(folderPath);
            if (objPhysician != null && humanRecord != null)
            {
                sPrintPathName = folderPath + "\\" + humanRecord.Id.ToString() + "_" + humanRecord.Last_Name + " " + humanRecord.MI + " " + humanRecord.First_Name + "_" + objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix + "_Immunization_" + DateTime.Now.ToString("yyyyMMdd hhmmss tt") + ".pdf";
            }
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            iTextSharp.text.Rectangle pageSize = doc.PageSize;
            HeaderEventGenerate headerEvent = new HeaderEventGenerate();
            doc.Open();
            wr.PageEvent = headerEvent;
            headerEvent.OnStartPage(wr, doc);
            headerEvent.OnEndPage(wr, doc);
            Paragraph par;
            par = new Paragraph(DateTime.Now.ToString("dd-MMM-yyyy"), normalFont);
            par.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
            doc.Add(par);
            doc.Add(new Paragraph("\n"));
            PdfPTable patTable = new PdfPTable(new float[] { 40, 20, 20, 20 });
            patTable.WidthPercentage = 100;
            PdfPCell cell = new PdfPCell(new Phrase("Patient Details", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 4;
            patTable.AddCell(cell);
            cell = CreateCell("Patient Name: \n", humanRecord.Last_Name + "," + humanRecord.First_Name);//(new Phrase("Patient Name: \n" + humanRecord.Last_Name + "," + humanRecord.First_Name, normalFont));
            patTable.AddCell(cell);
            cell = CreateCell("Patient ID: \n", humanRecord.Id.ToString());
            patTable.AddCell(cell);
            cell = CreateCell("Patient DOB: \n", humanRecord.Birth_Date.ToString("dd-MMM-yyyy"));
            patTable.AddCell(cell);
            cell = CreateCell("Patient Sex: \n", humanRecord.Sex);
            patTable.AddCell(cell);
            cell = CreateCell("Patient SSN: \n", humanRecord.SSN);
            patTable.AddCell(cell);
            string City = string.Empty;
            if (humanRecord.Street_Address1 != string.Empty && humanRecord.Street_Address2 != string.Empty)
                City = humanRecord.Street_Address1 + "\n" + humanRecord.Street_Address2;
            else City = humanRecord.Street_Address1;
            if (humanRecord.State != string.Empty && humanRecord.City != string.Empty)
            {
                City += "\n" + humanRecord.City + ", " + humanRecord.State + " " + humanRecord.ZipCode;
            }
            else
                City = "\n" + humanRecord.City;
            cell = CreateCell("Patient Address: \n", City);
            cell.Colspan = 3;
            patTable.AddCell(cell);
            doc.Add(patTable);
            doc.Add(new Paragraph("\n"));
            PdfPTable phyTable = new PdfPTable(new float[] { 40, 60 });
            phyTable.WidthPercentage = 100;
            cell = new PdfPCell(new Phrase("Insurance Details", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 2;
            phyTable.AddCell(cell);
            if (humanRecord.PatientInsuredBag != null)
            {
                if (humanRecord.PatientInsuredBag.Count > 0)
                {
                    if (humanRecord.PatientInsuredBag[0].Insured_Human_ID != humanRecord.Id)
                    {
                        Human insuredHuman = GetHumanByHumanID(humanRecord.PatientInsuredBag[0].Insured_Human_ID);
                        if (insuredHuman != null)
                        {
                            cell = CreateCell("Name of Policy Holder: \n", insuredHuman.Last_Name + "," + insuredHuman.First_Name);// +"\nRelationship to Patient: "+humanRecord.PatientInsuredBag[0].Relationship+ "\nAddress: " + insuredHuman.Street_Address1 + "\nCity: ", normalFont);
                            cell.AddElement(new Paragraph("Relationship to Patient: \n", reducedFont));
                            cell.AddElement(new Paragraph(humanRecord.PatientInsuredBag[0].Relationship, normalFont));
                            phyTable.AddCell(cell);
                            string City1 = string.Empty;
                            if (insuredHuman.City != string.Empty)
                            {
                                if (insuredHuman.Street_Address1 != string.Empty && insuredHuman.Street_Address2 != string.Empty)
                                    City1 = insuredHuman.Street_Address1 + "\n" + insuredHuman.Street_Address2;
                                else City1 = insuredHuman.Street_Address1;
                                if (insuredHuman.State != string.Empty && insuredHuman.City != string.Empty)
                                {
                                    City1 += "\n" + insuredHuman.City + ", " + insuredHuman.State + " " + insuredHuman.ZipCode;
                                }
                                else
                                    City1 = "\n" + insuredHuman.City;
                            }
                            cell = CreateCell("Insured Human Address: \n", City1);
                            phyTable.AddCell(cell);
                        }

                    }
                    InsurancePlanManager objInsurancePlanManager = new InsurancePlanManager();
                    InsurancePlan objInsPlan = objInsurancePlanManager.GetInsurancebyID(humanRecord.PatientInsuredBag[0].Insurance_Plan_ID)[0];
                    if (objInsPlan != null)
                    {
                        cell = CreateCell("Insurance Plan Name: \n", objInsPlan.Ins_Plan_Name);
                        phyTable.AddCell(cell);
                        cell = CreateCell("Insurance Address: \n", objInsPlan.Payer_Addrress1 + "\n" + objInsPlan.Payer_City);
                        phyTable.AddCell(cell);
                    }

                }
            }
            doc.Add(phyTable);
            doc.Add(new Paragraph("\n"));
            phyTable = new PdfPTable(new float[] { 40, 60 });
            phyTable.WidthPercentage = 100;
            cell = new PdfPCell(new Phrase("Facility & Physician Details", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 2;
            phyTable.AddCell(cell);
            FacilityLibrary objFacility = (from obj in facilityList where obj.Fac_Name == ClientSession.FacilityName select obj).ToList<FacilityLibrary>()[0];
            if (objFacility != null)
            {
                cell = CreateCell("Facility Name: \n", objFacility.Fac_Name);
                phyTable.AddCell(cell);
                cell = CreateCell("Facility Address: \n", objFacility.Fac_Address1 + "\n" + objFacility.Fac_City);
                phyTable.AddCell(cell);
            }
            if (objPhysician != null)
            {
                cell = CreateCell("Physician Name: \n", objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix);
                phyTable.AddCell(cell);
                cell = CreateCell("Physician NPI: \n", objPhysician.PhyNPI);
                phyTable.AddCell(cell);
            }
            doc.Add(phyTable);
            doc.Add(new Paragraph("\n"));
            PdfPTable proTable = new PdfPTable(new float[] { 7, 43, 50 });
            proTable.WidthPercentage = 100;
            cell = new PdfPCell(new Phrase("Procedure(s) Ordered", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 3;
            proTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("S.No.", reducedFont));
            proTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Immunization Procedure", reducedFont));
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
            proTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Immunization Details", reducedFont));
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
            proTable.AddCell(cell);
            for (int i = 0; i < objImmunizationFill.Immunization.Count; i++)
            {
                cell = CreateCell(string.Empty, (i + 1).ToString());
                proTable.AddCell(cell);
                cell = CreateCell(string.Empty, objImmunizationFill.Immunization[i].Procedure_Code + "-" + objImmunizationFill.Immunization[i].Immunization_Description);
                proTable.AddCell(cell);
                cell = new PdfPCell();
                Phrase phr = new Phrase("Route of Administration:  " + objImmunizationFill.Immunization[i].Route_of_Administration, normalFont);
                cell.AddElement(phr);
                if (objImmunizationFill.Immunization[i].Visit_Date != DateTime.MinValue)
                {
                    phr = new Phrase("Order Date:  " + objImmunizationFill.Immunization[i].Visit_Date.ToString("dd-MMM-yyyy"), normalFont);
                }
                else
                {
                    phr = new Phrase("Order Date:  ", normalFont);
                }

                //phr = new Phrase("Order Date:  " + objImmunizationFill.Immunization[i].Created_Date.ToString("dd-MMM-yyyy"), normalFont);
                cell.AddElement(phr);
                phr = new Phrase("Auth. Required:  " + objImmunizationFill.Immunization[i].Authorization_Required, normalFont);
                cell.AddElement(phr);
                phr = new Phrase("Lot #:  " + objImmunizationFill.Immunization[i].Lot_Number, normalFont);
                cell.AddElement(phr);
                phr = new Phrase("Manufacturer:  " + objImmunizationFill.Immunization[i].Manufacturer, normalFont);
                cell.AddElement(phr);
                if (objImmunizationFill.Immunization[i].Expiry_Date != DateTime.MinValue)
                {
                    phr = new Phrase("Expiry Date:  " + objImmunizationFill.Immunization[i].Expiry_Date.ToString("dd-MMM-yyyy"), normalFont);
                }
                else
                {
                    phr = new Phrase("Expiry Date:  ", normalFont);
                }
                cell.AddElement(phr);
                phr = new Phrase("Immunization source:  " + objImmunizationFill.Immunization[i].Immunization_Source, normalFont);
                cell.AddElement(phr);
                phr = new Phrase("Administered By:  " + objImmunizationFill.Immunization[i].Given_By, normalFont);
                cell.AddElement(phr);
                phr = new Phrase("Location: " + objImmunizationFill.Immunization[i].Location, normalFont);
                cell.AddElement(phr);
                string Dose = string.Empty;
                if (objImmunizationFill.Immunization[i].Dose != 0 && objImmunizationFill.Immunization[i].Dose_No != 0)
                {
                    Dose = "Dose to Total Dose:  " + objImmunizationFill.Immunization[i].Dose.ToString() + "/" + objImmunizationFill.Immunization[i].Dose_No.ToString();
                }
                else
                {
                    Dose = "Dose to Total Dose:  ";
                }
                phr = new Phrase(Dose, normalFont);
                cell.AddElement(phr);
                phr = new Phrase("NDC: " + objImmunizationFill.Immunization[i].NDC.ToString(), normalFont);
                cell.AddElement(phr);
                proTable.AddCell(cell);

            }
            doc.Add(proTable);
            doc.Close();
            return sPrintPathName;
            //System.Diagnostics.Process.Start(sPrintPathName);
        }

        public string PrintReferralOrders(IList<ReferralOrder> objList, ulong refFromPhyID, ReferralOrderDTO objRefOrdDTO, FillHumanDTO humanRecord, string Specialty, string TargetDirectoryPath)
        {
            if (objList != null)
            {
                FacilityManager FacilityList = new FacilityManager();
                facilityList = FacilityList.GetFacilityList();
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
                PhysicianManager objPhysicianManager = new PhysicianManager();
                PhysicianLibrary objRefFromPhy = objPhysicianManager.GetphysiciannameByPhyID(refFromPhyID)[0];
                //PhysicianLibrary objRefToPhy = AllLibraries.Instance.GetphysiciannameByPhyID(objList[0].To_Physician_ID, false);
                string sPrintPathName = string.Empty;
                //string folderPath = System.Configuration.ConfigurationSettings.AppSettings["ReferralPrintPath"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                string folderPath = TargetDirectoryPath + System.Configuration.ConfigurationSettings.AppSettings["ReferralPrintPath"];
                Directory.CreateDirectory(folderPath);
                if (objRefFromPhy != null)
                {
                    {
                        sPrintPathName = folderPath + "\\" + humanRecord.Human_ID.ToString() + "_" + humanRecord.Last_Name + "_" + humanRecord.First_Name
                            + "_" + objRefFromPhy.PhyPrefix + " " + objRefFromPhy.PhyFirstName +
                            " " + objRefFromPhy.PhyLastName + " " + objRefFromPhy.PhySuffix + "_ReferralOrder_"
                            + DateTime.Now.ToString("yyyyMMdd hhmmss tt") + objList[0].Id.ToString() + ".pdf";
                        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                        iTextSharp.text.Rectangle pageSize = doc.PageSize;
                        HeaderEventGenerate headerEvent = new HeaderEventGenerate();
                        doc.Open();
                        wr.PageEvent = headerEvent;
                        headerEvent.OnStartPage(wr, doc);
                        headerEvent.OnEndPage(wr, doc);
                        Paragraph par = null;
                        par = new Paragraph(objRefFromPhy.PhyPrefix + " " + objRefFromPhy.PhyFirstName +
                            " " + objRefFromPhy.PhyLastName + " " + objRefFromPhy.PhySuffix, normalFont);
                        par.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                        doc.Add(par);
                        FacilityLibrary objRefFromFac = (from obj in facilityList where obj.Fac_Name == ClientSession.FacilityName select obj).ToList<FacilityLibrary>()[0];
                        //string facilityText = ClientSession.sFacilityName + "\n" + objRefFromFac.Fac_Address1 + "\n" + objRefFromFac.Fac_City + "," + objRefFromFac.Fac_State + " " + objRefFromFac.Fac_Zip + "\n Phone #:" + objRefFromFac.Fac_Telephone + "  Fax:" + objRefFromFac.Fac_Fax;
                        string facilityText;
                        if (objRefFromFac.Fac_Address1.EndsWith(".,"))
                        {
                            facilityText = objRefFromFac.Fac_Address1.Remove(objRefFromFac.Fac_Address1.Length - 2).ToString() + "\n" + objRefFromFac.Fac_City + "," + objRefFromFac.Fac_State + " " + objRefFromFac.Fac_Zip + "\n Phone #:" + objRefFromFac.Fac_Telephone + "  Fax:" + objRefFromFac.Fac_Fax;
                        }
                        else
                        {
                            facilityText = objRefFromFac.Fac_Address1 + "\n" + objRefFromFac.Fac_City + "," + objRefFromFac.Fac_State + " " + objRefFromFac.Fac_Zip + "\n Phone #:" + objRefFromFac.Fac_Telephone + "  Fax:" + objRefFromFac.Fac_Fax;
                        }
                        par = new Paragraph(facilityText, normalFont);
                        par.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                        doc.Add(par);
                        par = new Paragraph("Referral Date: " + DateTime.Now.ToString("dd-MMM-yyyy"), normalFont);
                        par.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        doc.Add(par);
                        doc.Add(new Paragraph("\n"));
                        PdfPTable patTable = new PdfPTable(new float[] { 40, 20, 20, 20 });
                        patTable.WidthPercentage = 100;
                        PdfPCell cell = new PdfPCell(new Phrase("Patient Details", reducedFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        cell.Colspan = 4;
                        patTable.AddCell(cell);
                        cell = CreateCell("Patient Name: \n", humanRecord.Last_Name + "," + humanRecord.First_Name);//(new Phrase("Patient Name: \n" + humanRecord.Last_Name + "," + humanRecord.First_Name, normalFont));
                        patTable.AddCell(cell);
                        cell = CreateCell("Patient ID: \n", humanRecord.Human_ID.ToString());
                        patTable.AddCell(cell);
                        cell = CreateCell("Patient DOB: \n", humanRecord.Birth_Date.ToString("dd-MMM-yyyy"));
                        patTable.AddCell(cell);
                        cell = CreateCell("Patient Sex: \n", humanRecord.Sex);
                        patTable.AddCell(cell);
                        doc.Add(patTable);
                        doc.Add(new Paragraph("\n"));
                        //cell = CreateCell("Patient SSN: \n", humanRecord.SSN);
                        //patTable.AddCell(cell);
                        //cell = CreateCell("Patient Address: \n", humanRecord.Street_Address1 + "\n" + City);
                        //cell.Colspan = 3;
                        //patTable.AddCell(cell);            
                        patTable = new PdfPTable(new float[] { 40, 60 });
                        patTable.WidthPercentage = 100;
                        cell = new PdfPCell(new Phrase("Referred to Physician & Facility Details", reducedFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        cell.Colspan = 2;
                        patTable.AddCell(cell);
                        cell = CreateCell("Referred to Provider: \n", objList[0].To_Physician_Name);
                        patTable.AddCell(cell);
                        cell = CreateCell("Referred to Provider Specialty: \n", Specialty);
                        patTable.AddCell(cell);
                        cell = CreateCell("Referred to Facility: \n", objList[0].To_Facility_Name);
                        patTable.AddCell(cell);
                        cell = CreateCell("Referred to Facility Address: \n", objList[0].To_Facility_Street_Address + "\n" + objList[0].To_Facility_City + "\n" + objList[0].To_Facility_State + "\n" + objList[0].To_Facility_Zip);
                        //cell.Colspan = 3;
                        patTable.AddCell(cell);
                        cell = CreateCell("Referred to Facility Phone #: \n", "\n" + objList[0].To_Facility_Phone_Number);
                        patTable.AddCell(cell);
                        cell = CreateCell("Referred to Facility Fax: \n", "\n" + objList[0].To_Facility_Fax_Number);
                        //cell.Colspan = 3;
                        patTable.AddCell(cell);
                        string Address = string.Empty;
                        FacilityLibrary objRefFacilty;
                        try
                        {
                            objRefFacilty = (from obj in facilityList where obj.Fac_Name == objList[0].To_Facility_Name select obj).ToList<FacilityLibrary>()[0];
                        }
                        catch
                        {
                            objRefFacilty = new FacilityLibrary();
                        }

                        //if (objRefFacilty != null)
                        //{

                        //    if (objRefFacilty.Fac_Address1 != string.Empty && objRefFacilty.Fac_Address2 != string.Empty)
                        //    {
                        //        Address = objRefFacilty.Fac_Address1 + "\n" + objRefFacilty.Fac_Address2;
                        //    }
                        //    else
                        //        Address = objRefFacilty.Fac_Address1;

                        //    if (objRefFacilty.Fac_City != string.Empty && objRefFacilty.Fac_State != string.Empty)
                        //    {
                        //        Address += "\n" + objRefFacilty.Fac_City + ", " + objRefFacilty.Fac_State + " " + objRefFacilty.Fac_Zip;
                        //    }

                        //    cell = CreateCell("Referred to Facility Address: \n", Address);
                        //    //cell.Colspan = 3;
                        //    patTable.AddCell(cell);
                        //    cell = CreateCell("Referred to Facility Phone #: \n", "\n" + objRefFacilty.Fac_Telephone);
                        //    patTable.AddCell(cell);
                        //    cell = CreateCell("Referred to Facility Fax: \n", "\n" + objRefFacilty.Fac_Fax);
                        //    //cell.Colspan = 3;
                        //    patTable.AddCell(cell);
                        //}


                        doc.Add(patTable);
                        PdfPTable refTable = new PdfPTable(new float[] { 100 });
                        refTable.WidthPercentage = 100;
                        cell = new PdfPCell(new Phrase("Referral Details", reducedFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        refTable.AddCell(cell);

                        for (int i = 0; i < objList.Count; i++)
                        {


                            ReferralOrder obj = objList[i];
                            IList<ReferralOrdersAssessment> assList = (from objAss in objRefOrdDTO.RefOrdAssList where objAss.Referral_Order_ID == obj.Id select objAss).ToList<ReferralOrdersAssessment>();
                            //  IList<ReferralOrdersProblemList> proList = (from objPro in objRefOrdDTO.RefOrdProbList where objPro.Referral_Order_ID == obj.Id select objPro).ToList<ReferralOrdersProblemList>();
                            string Diagnosis = string.Empty;
                            foreach (ReferralOrdersAssessment objAss in assList)
                            {
                                if (Diagnosis != string.Empty)
                                {
                                    Diagnosis += "\n                 " + objAss.ICD + "-" + objAss.Assessment_Description;
                                }
                                else
                                {
                                    Diagnosis += objAss.ICD + "-" + objAss.Assessment_Description;
                                }
                            }
                            //foreach (ReferralOrdersProblemList objPro in proList)
                            //{
                            //    if (Diagnosis != string.Empty)
                            //    {
                            //        Diagnosis += "\n                 " + objPro.ICD + "-" + objPro.ICD_Description;
                            //    }
                            //    else
                            //    {
                            //        Diagnosis += objPro.ICD + "-" + objPro.ICD_Description;
                            //    }
                            //}
                            cell = new PdfPCell();
                            par = new Paragraph("Number of Visits: " + obj.Number_of_Visit, normalFont);
                            par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                            cell.AddElement(par);
                            par = new Paragraph("Diagnosis: " + Diagnosis, normalFont);
                            par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                            cell.AddElement(par);
                            par = new Paragraph("Reason For Referral: " + obj.Reason_For_Referral, normalFont);
                            par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                            cell.AddElement(par);

                            if (obj.Service_Requested != string.Empty)
                            {
                                par = new Paragraph("Service Requested: " + obj.Service_Requested, normalFont);
                                par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                                cell.AddElement(par);
                            }
                            if (obj.Service_Requested != string.Empty)
                            {
                                par = new Paragraph("Special Needs : " + obj.Special_Needs, normalFont);
                                par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                                cell.AddElement(par);
                            }

                            if (obj.Referral_Notes != string.Empty)
                            {
                                par = new Paragraph("Referral Notes : " + obj.Referral_Notes, normalFont);
                                par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                                cell.AddElement(par);
                            }

                            refTable.AddCell(cell);
                        }
                        doc.Add(new Paragraph("\n"));
                        doc.Add(refTable);
                        par = new Paragraph("Regards", normalFont);
                        par.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        doc.Add(par);
                        par = new Paragraph("\n(" + objRefFromPhy.PhyPrefix + " " + objRefFromPhy.PhyFirstName +
                            " " + objRefFromPhy.PhyLastName + " " + objRefFromPhy.PhySuffix + ")", normalFont);
                        par.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        doc.Add(par);

                        doc.Close();
                    }
                    return sPrintPathName;

                    //System.Diagnostics.Process.Start(sPrintPathName);
                }
            }
            return string.Empty;
        }

        #region GenerateRequisition

        public static PdfPTable CreateHeaderTable(string Acc, string humanName, string ControlNo, string PatId, string collDate, string humanID)
        {
            PdfPTable header = new PdfPTable(new float[] { 15, 25, 23, 12, 25 });
            header.WidthPercentage = 100f;
            PdfPCell cell = new PdfPCell(new Phrase("Account Number", reducedFont));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
            header.AddCell(cell);
            cell = new PdfPCell(new Phrase(": " + Acc, normalFont));
            cell.Border = Rectangle.NO_BORDER;
            header.AddCell(cell);
            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            header.AddCell(cell);
            cell = new PdfPCell(new Phrase("Patient Name", reducedFont));
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
            cell.Border = Rectangle.NO_BORDER;
            header.AddCell(cell);
            cell = new PdfPCell(new Phrase(": " + humanName, normalFont));
            cell.Border = Rectangle.NO_BORDER;
            header.AddCell(cell);
            cell = new PdfPCell(new Phrase("Req/Control#", reducedFont));
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
            cell.Border = Rectangle.NO_BORDER;
            header.AddCell(cell);
            cell = new PdfPCell(new Phrase(": " + ControlNo, normalFont));
            cell.Border = Rectangle.NO_BORDER;
            header.AddCell(cell);
            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            header.AddCell(cell);
            cell = new PdfPCell(new Phrase("Patient ID", reducedFont));
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
            cell.Border = Rectangle.NO_BORDER;
            header.AddCell(cell);
            cell = new PdfPCell(new Phrase(": " + PatId, normalFont));
            cell.Border = Rectangle.NO_BORDER;
            header.AddCell(cell);
            cell = new PdfPCell(new Phrase("Specimen Date", reducedFont));
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
            cell.Border = Rectangle.NO_BORDER;
            header.AddCell(cell);
            cell = new PdfPCell(new Phrase(": " + collDate, normalFont));
            cell.Border = Rectangle.NO_BORDER;
            header.AddCell(cell);
            cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            header.AddCell(cell);
            cell = new PdfPCell(new Phrase("Alt Pat ID", reducedFont));
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
            cell.Border = Rectangle.NO_BORDER;
            header.AddCell(cell);
            cell = new PdfPCell(new Phrase(": " + humanID, normalFont));
            cell.Border = Rectangle.NO_BORDER;
            header.AddCell(cell);

            return header;

        }

        public static PdfPTable CreateSpecimenTable(string humanName, string specDate, string Acc, string orderSubmitId)
        {
            PdfPTable header = new PdfPTable(new float[] { 20, 20, 20, 20 });
            header.TotalWidth = 100;
            for (int i = 0; i < 2; i++)
            {
                for (int k = 0; k < header.NumberOfColumns; k++)
                {
                    PdfPCell cell = new PdfPCell();
                    PdfPTable tbl = new PdfPTable(new float[] { 40, 60 });
                    PdfPCell c = new PdfPCell(new Phrase(humanName, normalFont7));
                    c.Border = Rectangle.NO_BORDER;
                    // c.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    tbl.AddCell(c);
                    c = new PdfPCell(new Phrase(specDate, normalFont7));
                    c.Border = Rectangle.NO_BORDER;
                    tbl.AddCell(c);
                    c = new PdfPCell(new Phrase(Acc, normalFont7));
                    c.Border = Rectangle.NO_BORDER;
                    tbl.AddCell(c);
                    c = new PdfPCell(new Phrase(orderSubmitId, normalFont7));
                    c.Border = Rectangle.NO_BORDER;
                    tbl.AddCell(c);
                    cell = new PdfPCell(tbl);
                    cell.BorderColor = BaseColor.GRAY;
                    //cell.AddElement(new Phrase(GetValueFromHashTable("PID-5.1, PID-5.2", segTable) + "   " + GetValueFromHashTable("OBR-7", segTable), normalFont7));
                    //cell.AddElement(new Phrase(GetValueFromHashTable("PID-18.1", segTable) + "   " + GetValueFromHashTable("OBR-2", segTable), normalFont7));
                    header.AddCell(cell);
                }
            }
            return header;

        }

        private PdfPCell CreateCellForDataFromDB(IList<string> Header, IList<string> Value, int count, bool IsAfp)
        {
            PdfPTable tbl;
            if (count == 1)
            {
                tbl = new PdfPTable(new float[] { 20, 90 });
            }
            else if (count == 2 && IsAfp == true)
            {
                tbl = new PdfPTable(new float[] { 60, 40 });
            }
            else
            {
                tbl = new PdfPTable(new float[] { 40, 65 });
            }
            tbl.WidthPercentage = 100;
            PdfPCell cell = new PdfPCell();
            if (Header != null && Value != null)
            {
                for (int i = 0; i < Header.Count && i < Value.Count; i++)
                {
                    PdfPCell c = new PdfPCell(new Phrase(Header[i], reducedFont));
                    c.Border = Rectangle.NO_BORDER;
                    c.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    tbl.AddCell(c);
                    c = new PdfPCell(new Phrase(": " + Value[i], normalFont));
                    c.Border = Rectangle.NO_BORDER;
                    tbl.AddCell(c);
                }
            }
            else if (Header == null && Value != null)
            {
                for (int i = 0; i < Value.Count; i++)
                {
                    PdfPCell c;
                    c = new PdfPCell(new Phrase(Value[i], normalFont));
                    c.Colspan = 2;
                    c.Border = Rectangle.NO_BORDER;
                    tbl.AddCell(c);
                }
            }
            cell = new PdfPCell(tbl);
            return cell;
        }

        private PdfPCell CreateCellForDataFromDBWithLeftAlignment(IList<string> Header, IList<string> Value, int count, bool IsAfp)
        {
            PdfPTable tbl;
            if (count == 1)
            {
                tbl = new PdfPTable(new float[] { 20, 90 });
            }
            else if (count == 2 && IsAfp == true)
            {
                tbl = new PdfPTable(new float[] { 60, 40 });
            }
            else
            {
                tbl = new PdfPTable(new float[] { 25, 75 });
            }
            tbl.WidthPercentage = 100;
            PdfPCell cell = new PdfPCell();
            if (Header != null && Value != null)
            {
                for (int i = 0; i < Header.Count && i < Value.Count; i++)
                {
                    PdfPCell c = new PdfPCell(new Phrase(Header[i], reducedFont));
                    c.Border = Rectangle.NO_BORDER;
                    c.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tbl.AddCell(c);
                    c = new PdfPCell(new Phrase(": " + Value[i], normalFont));
                    c.Border = Rectangle.NO_BORDER;
                    tbl.AddCell(c);
                }
            }
            else if (Header == null && Value != null)
            {
                for (int i = 0; i < Value.Count; i++)
                {
                    PdfPCell c;
                    c = new PdfPCell(new Phrase(Value[i], normalFont));
                    c.Colspan = 2;
                    c.Border = Rectangle.NO_BORDER;
                    tbl.AddCell(c);
                }
            }
            cell = new PdfPCell(tbl);
            return cell;
        }

        public string PrintRequisitionUsingDatafromDB(ulong OrderId, string appConfigKey, String CallingFrom, ulong Encounter_id, string OrderType)
        {
            #region DataInitialization


            string Interface_identifier = string.Empty;
            string AccountNumberForLabCorp = string.Empty;
            ulong HumanId = 0;
            ulong PhysicianID = 0;
            OrdersManager objOrdersManager = new OrdersManager();
            //OrdersProxy ordersProxy = new OrdersProxy();
            //SpecimenProxy objSpecimenproxy = new SpecimenProxy();
            LabcorpSettingsManager objLabcorpsettingsManager = new LabcorpSettingsManager();
            OrderSpecimenDTO objOrdDTO = objOrdersManager.LoadSpecimen(OrderId, OrderType);
            IList<LabSettings> ilstLabSettingsLibrary = objLabcorpsettingsManager.GetLabcorpSettings();
            IList<LabSettings> ilstLabcorpSettings = (from lst in ilstLabSettingsLibrary where lst.Lab_ID == objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Lab_ID select lst).ToList<LabSettings>();
            string CliAdd = string.Empty;
            if (ilstLabcorpSettings != null && ilstLabcorpSettings.Count > 0)
            {
                CliAdd = ilstLabcorpSettings[0].Client_Information;
            }
            if (objOrdDTO == null || (objOrdDTO != null && objOrdDTO.ilstOrderLabDetailsDTO.Count == 0) || objOrdDTO.objHuman == null)
                return string.Empty;
            //IList<Specimen> specList = objOrdDTO.ilstOrderLabDetailsDTO.Select(a => a.objSpecimen).ToList<Specimen>() ;
            string raceMaster = "White,Caucasian,Black,American Indian,Asian";
            string specDate = string.Empty;
            string specTime = string.Empty;
            string humanName = string.Empty;
            string humanAddress = string.Empty;
            string humanCity = string.Empty;
            string SSN = string.Empty;
            string orderCode = string.Empty;
            string testOrdered = string.Empty;
            string testcount = string.Empty;
            string totVol = string.Empty;
            string workerComp = string.Empty;
            string collDate = string.Empty;
            Orders objOrder = new Orders();
            string controlReqNumber = "ACUR" + OrderId.ToString();
            string ethnicity = string.Empty;
            OrdersQuestionSetAfp objAFp = null;
            OrdersQuestionSetBloodLead objBloodLead = null;
            OrdersQuestionSetCytology objCytology = null;
            OrdersSubmit objOrdersSubmit = new OrdersSubmit();
            //Specimen objspecimen=new Specimen();
            //if (specList != null)
            //{
            //    if (specList.Count > 0)
            //    {

            //        if (specList[0].Quantity != 0)
            //        {

            //        }
            //    }
            //}
            testcount = objOrdDTO.ilstOrderLabDetailsDTO.Count.ToString();
            string Fasting = string.Empty;
            string Speciment_Type = string.Empty;
            if (objOrdDTO.ilstOrderLabDetailsDTO.Count > 0)
            {
                totVol = objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Quantity.ToString() + " " + objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Specimen_Unit.ToString();
                Fasting = objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Fasting.ToString();
                Speciment_Type = objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Specimen_Type.ToString();
                //objspecimen = objOrdDTO.ilstOrderLabDetailsDTO[0].objSpecimen;
                objOrdersSubmit = objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit;
                objOrder = objOrdDTO.ilstOrderLabDetailsDTO[0].ObjOrder;
                objAFp = objOrdDTO.ilstOrderLabDetailsDTO[0].objAFP;
                objCytology = objOrdDTO.ilstOrderLabDetailsDTO[0].objCytology;
                objBloodLead = objOrdDTO.ilstOrderLabDetailsDTO[0].objBloodLead;
                collDate = UtilityManager.ConvertToLocal(objOrdDTO.ilstOrderLabDetailsDTO[0].ObjOrder.Created_Date_And_Time).ToString("MM/dd/yyyy hh:mm tt");
                //specDate = UtilityManager.ConvertToLocal(DateTime.MinValue != objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Specimen_Collection_Date_And_Time ? objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Specimen_Collection_Date_And_Time : objOrdDTO.ilstOrderLabDetailsDTO[0].ObjOrder.Created_Date_And_Time).ToString("MM/dd/yyyy");
                //specTime = UtilityManager.ConvertToLocal(DateTime.MinValue != objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Specimen_Collection_Date_And_Time ? objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Specimen_Collection_Date_And_Time : objOrdDTO.ilstOrderLabDetailsDTO[0].ObjOrder.Created_Date_And_Time).ToString("hh:mm tt");

                specDate = DateTime.MinValue != objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Specimen_Collection_Date_And_Time ? UtilityManager.ConvertToLocal(objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Specimen_Collection_Date_And_Time).ToString("MM/dd/yyyy") : string.Empty;
                specTime = DateTime.MinValue != objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Specimen_Collection_Date_And_Time ? UtilityManager.ConvertToLocal(objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Specimen_Collection_Date_And_Time).ToString("hh:mm tt") : string.Empty;

                for (int i = 0; i < objOrdDTO.ilstOrderLabDetailsDTO.Count; i++)
                {
                    Orders obj = objOrdDTO.ilstOrderLabDetailsDTO[i].ObjOrder;
                    PhysicianID = obj.Physician_ID;
                    HumanId = obj.Human_ID;
                    if (i == 0)
                    {
                        testOrdered = obj.Lab_Procedure_Description;
                        orderCode = obj.Order_Code_Type + "  " + obj.Lab_Procedure;
                    }
                    else
                    {
                        testOrdered += "\n" + obj.Lab_Procedure_Description;
                        orderCode += "\n" + obj.Order_Code_Type + "  " + obj.Lab_Procedure;
                    }
                }
            }
            PhysicianManager objPhysicianMgr = new PhysicianManager();

            IList<PhysicianLibrary> tempList = new List<PhysicianLibrary>();
            tempList = objPhysicianMgr.GetphysiciannameByPhyID(objOrdDTO.ilstOrderLabDetailsDTO.Select(a => a.ObjOrder.Physician_ID).ToList<ulong>()[0]);
            PhysicianLibrary objPhy = tempList != null && tempList.Count > 0 ? tempList[0] : new PhysicianLibrary();
            FillHumanDTO objHuman = objOrdDTO.objHuman;
            //objPhy = AllLibraries.Instance.GetphysiciannameByPhyID(PhysicianID, false);
            //objHuman = EncounterManager.Instance.GetHumanByHumanID(HumanId);

            string sHeight = string.Empty;
            string sWeight = string.Empty;

            IList<string> ilstHeightandWeight = new List<string>();
            VitalsManager objVitalMngr = new VitalsManager();
            DateTime dtvital = objOrdersSubmit.Modified_Date_And_Time != Convert.ToDateTime("0001-01-01 00:00:00") ? objOrdersSubmit.Modified_Date_And_Time : objOrdersSubmit.Created_Date_And_Time;
            ilstHeightandWeight = objVitalMngr.GetHeightandWeightbyEncounterID(Encounter_id, dtvital, objHuman.Human_ID);
            if (ilstHeightandWeight.Count > 0)
            {
                UIManager objUIMngr = new UIManager();

                var Height = (from h in ilstHeightandWeight where h.Split('-')[0].ToUpper() == "HEIGHT" select h).ToArray()[0].Split('-')[1].ToString();
                sWeight = (from h in ilstHeightandWeight where h.Split('-')[0].ToUpper() == "WEIGHT" select h).ToArray()[0].Split('-')[1].ToString();
                var feetandinch = objUIMngr.ConvertInchtoFeetInch(Height.ToString());
                string[] Splitter = { "'", "''" };
                string[] feetInch = feetandinch.Split(Splitter, StringSplitOptions.RemoveEmptyEntries);
                if (feetInch.Length > 0)
                {
                    sHeight = feetandinch[0] + " Feet " + feetInch[1] + " Inches";
                }
                sWeight = sWeight + " Lbs";
            }

            humanName = objHuman.Last_Name + "," + objHuman.First_Name + " " + objHuman.MI;
            if (objHuman.Street_Address1 != string.Empty && objHuman.Street_Address2 != string.Empty)
            {
                humanAddress = objHuman.Street_Address1 + "\n" + objHuman.Street_Address2;
            }
            else
                humanAddress = objHuman.Street_Address1;

            if (objHuman.City != string.Empty && objHuman.State != string.Empty)
            {
                humanCity = objHuman.City + ", " + objHuman.State + " " + objHuman.ZipCode;
            }
            if (objHuman.SSN != string.Empty && objHuman.SSN.Length == 11)
            {
                SSN = objHuman.SSN.Replace(objHuman.SSN.Substring(0, 6), "XXX-XX");
            }

            string raceToPrint = string.Empty;
            if (objHuman.Race == string.Empty)
            {
                raceToPrint = "Unknown";
            }
            else
            {
                if (raceMaster.Contains(objHuman.Race))
                    raceToPrint = objHuman.Race;
                else
                    raceToPrint = "Other";
            }

            switch (objHuman.Ethnicity_No)
            {
                case 1: ethnicity = "Yes";
                    break;
                case 2: ethnicity = "No";
                    break;
                default:
                    ethnicity = "Unknown";
                    break;
            }

            InsurancePlan objPriPlan = new InsurancePlan();
            InsurancePlan objSecPlan = new InsurancePlan();
            PatientInsuredPlan objPatInsPri = new PatientInsuredPlan();
            PatientInsuredPlan objPatInsSec = new PatientInsuredPlan();
            LabCarrierLookUp objlabCarrierPri = new LabCarrierLookUp();
            LabCarrierLookUp objLabCarrierSec = new LabCarrierLookUp();

            if (objHuman.PatientInsuredBag.Count > 0)
            {
                LabCarrierLookUpManager objLabCarrierLookUpManager = new LabCarrierLookUpManager();
                InsurancePlanManager objInsurancePlanManager = new InsurancePlanManager();
                foreach (PatientInsuredPlan obj in objHuman.PatientInsuredBag)
                {
                    if (obj.Insurance_Type.ToUpper() == "PRIMARY")
                    {
                        objPatInsPri = obj;
                        objPriPlan = objInsurancePlanManager.GetInsurancebyID(obj.Insurance_Plan_ID)[0];
                        objlabCarrierPri = objLabCarrierLookUpManager.GetLabCarrierDetailsForInsPlanID(obj.Insurance_Plan_ID, objOrdersSubmit.Lab_ID);

                    }
                    else if (obj.Insurance_Type.ToUpper() == "SECONDARY")
                    {
                        objPatInsSec = obj;
                        objSecPlan = objInsurancePlanManager.GetInsurancebyID(obj.Insurance_Plan_ID)[0];
                        objLabCarrierSec = objLabCarrierLookUpManager.GetLabCarrierDetailsForInsPlanID(obj.Insurance_Plan_ID, objOrdersSubmit.Lab_ID);
                    }

                }
            }
            if (objPriPlan.Is_Workers_Comp.ToUpper() == "Y" || objSecPlan.Is_Workers_Comp.ToUpper() == "Y")
            {
                workerComp = "Y";
            }
            else
            {
                workerComp = "N";
            }
            FacilityManager objFacilityMgr = new FacilityManager();
            //FacilityProxy objfacilityProxy = new FacilityProxy();
            IList<FacilityLibrary> ilstFacility = objFacilityMgr.GetFacilityByFacilityname(objOrdersSubmit.Facility_Name);
            string VendorNameForLabCorpRequisition = string.Empty;
            if (ilstLabcorpSettings.Count > 0)
            {
                Interface_identifier = ilstLabcorpSettings[0].Interface_Identifier;
                AccountNumberForLabCorp = ilstFacility[0].LabCorp_Account_Number;
                VendorNameForLabCorpRequisition = ilstLabcorpSettings[0].Lab_Practice_Application;
            }
            #endregion

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
            string sPrintPathName = string.Empty;
            string folderPath = appConfigKey;
            //string folderPath = @"E:\Altova LabCorp Output";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            sPrintPathName = folderPath + "\\Order_Requisition_" + controlReqNumber + ".pdf";

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            iTextSharp.text.Rectangle pageSize = doc.PageSize;
            doc.Open();
            //HeaderEventGenerateForLabCorp headerEvent = new HeaderEventGenerateForLabCorp("04992680", objHuman.Last_Name + "," + objHuman.First_Name, controlReqNumber, "", collDate, objHuman.Id.ToString());
            HeaderEventGenerateForLabCorp headerEvent = new HeaderEventGenerateForLabCorp(AccountNumberForLabCorp, objHuman.Last_Name + "," + objHuman.First_Name, controlReqNumber, "", collDate, objHuman.Human_ID.ToString());
            wr.PageEvent = headerEvent;
            headerEvent.OnStartPage(wr, doc);
            headerEvent.OnEndPage(wr, doc);
            PdfPTable table;
            PdfPCell cell = new PdfPCell();
            //Paragraph par = new Paragraph("LabCorp", onlyBoldFont);
            Paragraph par = new Paragraph(System.Configuration.ConfigurationSettings.AppSettings["LabNameForLabCorpRequisition"], onlyBoldFont);
            par.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            doc.Add(par);
            //par = new Paragraph("EREQ", onlyBoldFont);
            par = new Paragraph(Interface_identifier, onlyBoldFont);
            par.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            doc.Add(par);
            //par = new Paragraph("ACUR", onlyBoldFont);
            par = new Paragraph(VendorNameForLabCorpRequisition, onlyBoldFont);
            par.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            doc.Add(par);

            float xPos = doc.LeftMargin, yPos = 150, availableSpace = 0;
            table = new PdfPTable(new float[] { 20, 60, 20 });
            table.WidthPercentage = 100f;
            //cell = new PdfPCell(new Phrase("LabCorp™", redBoldFont));
            cell = new PdfPCell(new Phrase(System.Configuration.ConfigurationSettings.AppSettings["LabNameForLabCorpRequisition"] + "™", redBoldFont));
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            //cell = new PdfPCell(new Phrase("EREQ ACUR", redBoldFont));
            cell = new PdfPCell(new Phrase(Interface_identifier + " " + VendorNameForLabCorpRequisition, redBoldFont));
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Page 1 of 2", redBoldFont));
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
            cell.Border = Rectangle.NO_BORDER;
            doc.Add(new Paragraph("\n"));
            table.AddCell(cell);
            doc.Add(table);
            float location = 100;
            IList<string> HeaderList = new List<string>();
            IList<string> ValueList = new List<string>();
            //table = CreateSpecimenTable(objHuman.Last_Name + "," + objHuman.First_Name, collDate, "04992680", controlReqNumber);
            //table = CreateSpecimenTable(objHuman.Last_Name + "," + objHuman.First_Name, collDate, System.Configuration.ConfigurationSettings.AppSettings["AccountNumberForLabCorpRequisition"], controlReqNumber);
            table = CreateSpecimenTable(objHuman.Last_Name + "," + objHuman.First_Name, collDate, AccountNumberForLabCorp, controlReqNumber);
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(pageSize.Height - (50 + table.TotalHeight)), wr.DirectContent);
            availableSpace = pageSize.Height - (50 + table.TotalHeight + 10);
            table = new PdfPTable(new float[] { 40, 25, 35 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            HeaderList.Add("Account");
            //ValueList.Add("04992680");
            //ValueList.Add( System.Configuration.ConfigurationSettings.AppSettings["AccountNumberForLabCorpRequisition"]);
            ValueList.Add(AccountNumberForLabCorp);
            HeaderList.Add("Req/Control #");
            ValueList.Add(controlReqNumber);
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
            cell.BorderWidthRight = 0;
            table.AddCell(cell);
            cell = new PdfPCell();
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            table.AddCell(cell);
            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("Collection Date");
            ValueList.Add(specDate);
            HeaderList.Add("Collection Time");
            ValueList.Add(specTime);
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, true);
            cell.BorderWidthLeft = 0;
            table.AddCell(cell);
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            yPos += table.TotalHeight + 10;
            location = yPos;

            table = new PdfPTable(new float[] { 50, 50 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            table.AddCell(new PdfPCell(new Phrase("Client / Ordering Site Information", reducedFont)));
            table.AddCell(new PdfPCell(new Phrase("Physician Information", reducedFont)));
            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("Account Name");
            if (CliAdd != null)
                ValueList.Add(CliAdd);
            else
                ValueList.Add("");

            HeaderList.Add("Address");
            if (ilstFacility != null && ilstFacility.Count > 0)
                ValueList.Add(ilstFacility[0].Fac_Address1);
            else
                ValueList.Add("");
            HeaderList.Add("City, State Zip");
            if (ilstFacility != null && ilstFacility.Count > 0)
                ValueList.Add(ilstFacility[0].Fac_City + "," + ilstFacility[0].Fac_State + "," + ilstFacility[0].Fac_Zip);
            else
                ValueList.Add("");

            HeaderList.Add("Phone");
            if (ilstFacility != null && ilstFacility.Count > 0)
                ValueList.Add(ilstFacility[0].Fac_Telephone);
            else
                ValueList.Add("");
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
            table.AddCell(cell);
            HeaderList.Clear();
            ValueList.Clear();
            string phyName = string.Empty;
            if (objPhy.PhyLastName != string.Empty && objPhy.PhyFirstName != string.Empty)
            {
                phyName = objPhy.PhyLastName + ", " + objPhy.PhyFirstName;
            }
            HeaderList.Add("Ordering Physician");
            ValueList.Add(phyName);
            HeaderList.Add("Physician Degree");
            ValueList.Add(objPhy.PhySuffix);
            HeaderList.Add("Physician NPI");
            ValueList.Add(objPhy.PhyNPI);
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
            table.AddCell(cell);
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            //doc.Add(table);
            yPos += table.TotalHeight + 10;
            location += table.TotalHeight;

            table = new PdfPTable(new float[] { 50, 50 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            cell = new PdfPCell(new Phrase("Patient Information", reducedFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("Patient Name");
            ValueList.Add(humanName);
            HeaderList.Add("Gender");
            string sSex = string.Empty;
            if (objHuman.Sex != null && objHuman.Sex != "")
                sSex = objHuman.Sex.Substring(0, 1);
            ValueList.Add(sSex);
            HeaderList.Add("Date of Birth");
            ValueList.Add(objHuman.Birth_Date.ToString("dd-MMM-yyyy"));
            HeaderList.Add("Age");
            ValueList.Add((PrintOrders.CalculateAge(objHuman.Birth_Date)).ToString());




            HeaderList.Add("Patient Address");
            ValueList.Add(humanAddress);
            HeaderList.Add("City, State Zip");
            ValueList.Add(humanCity);
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
            table.AddCell(cell);
            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("Patient SSN");
            ValueList.Add(SSN);
            HeaderList.Add("Patient ID");
            ValueList.Add("");
            HeaderList.Add("Phone");
            ValueList.Add(objHuman.Home_Phone_No);
            HeaderList.Add("Alt Ctrl #");
            ValueList.Add("");
            HeaderList.Add("Alt Patient ID");
            ValueList.Add(objHuman.Human_ID.ToString());
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
            table.AddCell(cell);
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            yPos += table.TotalHeight + 10;
            location += table.TotalHeight;


            table = new PdfPTable(new float[] { 20, 50 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            table.AddCell(new PdfPCell(new Phrase("Order Code", reducedFont)));
            table.AddCell(new PdfPCell(new Phrase("Tests Ordered  (Total: " + testcount.ToString() + ")", reducedFont)));
            HeaderList.Clear();
            ValueList.Clear();
            ValueList.Add(orderCode);
            cell = CreateCellForDataFromDB(null, ValueList, 2, false);
            table.AddCell(cell);
            ValueList.Clear();
            ValueList.Add(testOrdered);
            cell = CreateCellForDataFromDB(null, ValueList, 2, false);
            table.AddCell(cell);
            if (table.TotalHeight > availableSpace - yPos)
            {
                doc.NewPage();
                availableSpace = pageSize.Height - 50;
                yPos = 130;
                location = 0;
            }
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            yPos += table.TotalHeight + 10;
            location += table.TotalHeight;

            if (objCytology != null)
            {
                #region Cytology
                table = new PdfPTable(new float[] { 100 });
                table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                cell = new PdfPCell(new Phrase("Clinical Information:", reducedFont));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Cytology Information", reducedFont));
                table.AddCell(cell);
                HeaderList.Clear();
                ValueList.Clear();
                HeaderList.Add("GYN Source");
                string value = string.Empty;
                if (objCytology.Gyn_Source_Cervical != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Cervical: " + objCytology.Gyn_Source_Cervical;
                    else
                        value += ", " + "Cervical: " + objCytology.Gyn_Source_Cervical;
                }
                if (objCytology.Gyn_Source_Endocervical != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Endocervical: " + objCytology.Gyn_Source_Endocervical;
                    else
                        value += ", " + "Endocervical: " + objCytology.Gyn_Source_Endocervical;
                }
                if (objCytology.Gyn_Source_Endometrial != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Endometrial: " + objCytology.Gyn_Source_Endometrial;
                    else
                        value += ", " + "Endometrial: " + objCytology.Gyn_Source_Endometrial;
                }
                if (objCytology.Gyn_Source_Hysterectomy_Supracervical != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Hysterectomy,Supracervical: " + objCytology.Gyn_Source_Hysterectomy_Supracervical;
                    else
                        value += ", " + "Hysterectomy,Supracervical: " + objCytology.Gyn_Source_Hysterectomy_Supracervical;
                }
                if (objCytology.Gyn_Source_Labia_Vulva != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Labia Vulva: " + objCytology.Gyn_Source_Labia_Vulva;
                    else
                        value += ", " + "Labia Vulva: " + objCytology.Gyn_Source_Labia_Vulva;
                }
                if (objCytology.Gyn_Source_Vaginal != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Vaginal: " + objCytology.Gyn_Source_Vaginal;
                    else
                        value += ", " + "Vaginal: " + objCytology.Gyn_Source_Vaginal;
                }

                ValueList.Add(value);
                HeaderList.Add("Coll Tech");
                value = string.Empty;
                if (objCytology.Collection_Technique_Broom_Alone != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Broom Alone: " + objCytology.Collection_Technique_Broom_Alone;
                    else
                        value += ", " + "Broom Alone: " + objCytology.Collection_Technique_Broom_Alone;
                }
                if (objCytology.Collection_Technique_Brush_Alone != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Brush Alone: " + objCytology.Collection_Technique_Brush_Alone;
                    else
                        value += ", " + "Brush Alone: " + objCytology.Collection_Technique_Brush_Alone;
                }
                if (objCytology.Collection_Technique_Brush_Spatula != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Brush Spatula: " + objCytology.Gyn_Source_Endometrial;
                    else
                        value += ", " + "Brush Spatula: " + objCytology.Gyn_Source_Endometrial;
                }
                if (objCytology.Collection_Technique_Other != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Other: " + objCytology.Collection_Technique_Other;
                    else
                        value += ", " + "Other: " + objCytology.Collection_Technique_Other;
                }
                if (objCytology.Collection_Technique_Spatula_Alone != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Spatula Alone: " + objCytology.Collection_Technique_Spatula_Alone;
                    else
                        value += ", " + "Spatula Alone: " + objCytology.Collection_Technique_Spatula_Alone;
                }
                if (objCytology.Collection_Technique_Swab_Spatula != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Swab Spatula: " + objCytology.Collection_Technique_Swab_Spatula;
                    else
                        value += ", " + "Swab Spatula: " + objCytology.Collection_Technique_Swab_Spatula;
                }
                ValueList.Add(value);
                HeaderList.Add("LMP/Meno Date");
                if (objCytology.LMP_Meno_Date.ToString("yyyyMMdd") != "00010101")
                {
                    ValueList.Add(objCytology.LMP_Meno_Date.ToString("MM/dd/yyyy"));
                }
                else
                {
                    ValueList.Add("");
                }
                HeaderList.Add("Previous Treatment");
                value = string.Empty;
                if (objCytology.Previous_Treatment_Colp_BX != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Colp-BX: " + objCytology.Previous_Treatment_Colp_BX;
                    else
                        value += ", " + "Colp-BX: " + objCytology.Previous_Treatment_Colp_BX;
                }
                if (objCytology.Previous_Treatment_Coniza != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Coniza: " + objCytology.Previous_Treatment_Coniza;
                    else
                        value += ", " + "Coniza: " + objCytology.Previous_Treatment_Coniza;
                }
                if (objCytology.Previous_Treatment_Cyro != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Cyro: " + objCytology.Previous_Treatment_Cyro;
                    else
                        value += ", " + "Cyro: " + objCytology.Previous_Treatment_Cyro;
                }
                if (objCytology.Previous_Treatment_Hyst != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Hyst: " + objCytology.Previous_Treatment_Hyst;
                    else
                        value += ", " + "Hyst: " + objCytology.Previous_Treatment_Hyst;
                }
                if (objCytology.Previous_Treatment_Laser_Vap != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Laser-Vap: " + objCytology.Previous_Treatment_Laser_Vap;
                    else
                        value += ", " + "Laser-Vap: " + objCytology.Previous_Treatment_Laser_Vap;
                }
                if (objCytology.Previous_Treatment_None != string.Empty)
                {
                    if (value == string.Empty)
                        value = "None: " + objCytology.Previous_Treatment_None;
                    else
                        value += ", " + "None: " + objCytology.Previous_Treatment_None;
                }
                if (objCytology.Previous_Treatment_Radiation != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Radiation: " + objCytology.Previous_Treatment_Radiation;
                    else
                        value += ", " + "Radiation: " + objCytology.Previous_Treatment_Radiation;
                }
                ValueList.Add(value);
                HeaderList.Add("Previous Cytology");
                value = string.Empty;
                if (objCytology.Previous_Cytology_Info_Dates_Results != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Dates-Results: " + objCytology.Previous_Cytology_Info_Dates_Results;
                    else
                        value += ", " + "Dates-Results: " + objCytology.Previous_Cytology_Info_Dates_Results;
                }
                if (objCytology.Previous_Cytology_Info_Atypical != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Atypical: " + objCytology.Previous_Cytology_Info_Atypical;
                    else
                        value += ", " + "Atypical: " + objCytology.Previous_Cytology_Info_Atypical;
                }
                if (objCytology.Previous_Cytology_Info_Ca_In_Situ != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Ca-In-Situ: " + objCytology.Previous_Cytology_Info_Ca_In_Situ;
                    else
                        value += ", " + "Ca-In-Situ: " + objCytology.Previous_Cytology_Info_Ca_In_Situ;
                }
                if (objCytology.Previous_Cytology_Info_Dysplasia != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Dysplasia: " + objCytology.Previous_Cytology_Info_Dysplasia;
                    else
                        value += ", " + "Dysplasia: " + objCytology.Previous_Cytology_Info_Dysplasia;
                }
                if (objCytology.Previous_Cytology_Info_Invasive != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Invasive: " + objCytology.Previous_Cytology_Info_Invasive;
                    else
                        value += ", " + "Invasive: " + objCytology.Previous_Cytology_Info_Invasive;
                }
                if (objCytology.Previous_Cytology_Info_Negative != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Neagtive: " + objCytology.Previous_Cytology_Info_Negative;
                    else
                        value += ", " + "Neagtive: " + objCytology.Previous_Cytology_Info_Negative;
                }
                if (objCytology.Previous_Cytology_Info_Other != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Other: " + objCytology.Previous_Cytology_Info_Other;
                    else
                        value += ", " + "Other: " + objCytology.Previous_Cytology_Info_Other;
                }
                ValueList.Add(value);
                HeaderList.Add("Other Patient Info");
                value = string.Empty;
                if (objCytology.Other_Patient_Info_Pregnant != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Pregnant: " + objCytology.Other_Patient_Info_Pregnant;
                    else
                        value += ", " + "Pregnant: " + objCytology.Other_Patient_Info_Pregnant;
                }
                if (objCytology.Other_Patient_Info_Lactating != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Lactating: " + objCytology.Other_Patient_Info_Lactating;
                    else
                        value += ", " + "Lactating: " + objCytology.Other_Patient_Info_Lactating;
                }
                if (objCytology.Other_Patient_Info_Menopausal != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Menopausal: " + objCytology.Other_Patient_Info_Menopausal;
                    else
                        value += ", " + "Menopausal: " + objCytology.Other_Patient_Info_Menopausal;
                }
                if (objCytology.Other_Patient_Info_Oral_Contraceptives != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Oral-Contraceptives: " + objCytology.Other_Patient_Info_Oral_Contraceptives;
                    else
                        value += ", " + "Oral-Contraceptives: " + objCytology.Other_Patient_Info_Oral_Contraceptives;
                }
                if (objCytology.Other_Patient_Info_PMP_Bleeding != string.Empty)
                {
                    if (value == string.Empty)
                        value = "PMP-Bleeding: " + objCytology.Other_Patient_Info_PMP_Bleeding;
                    else
                        value += ", " + "PMP-Bleeding: " + objCytology.Other_Patient_Info_PMP_Bleeding;
                }
                if (objCytology.Other_Patient_Info_Post_Part != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Post-Part: " + objCytology.Other_Patient_Info_Post_Part;
                    else
                        value += ", " + "Post-Part: " + objCytology.Other_Patient_Info_Post_Part;
                }
                if (objCytology.Other_Patient_Info_Estro_RX != string.Empty)
                {
                    if (value == string.Empty)
                        value = "Estro-RX: " + objCytology.Other_Patient_Info_Estro_RX;
                    else
                        value += ", " + "Estro-RX: " + objCytology.Other_Patient_Info_Estro_RX;
                }
                if (objCytology.Other_Patient_Info_IUD != string.Empty)
                {
                    if (value == string.Empty)
                        value = "IUD: " + objCytology.Other_Patient_Info_IUD;
                    else
                        value += ", " + "IUD: " + objCytology.Other_Patient_Info_IUD;
                }
                if (objCytology.Other_Patient_Info_All_Other_Pat != string.Empty)
                {
                    if (value == string.Empty)
                        value = "All-Other-Pat: " + objCytology.Other_Patient_Info_All_Other_Pat;
                    else
                        value += ", " + "All-Other-Pat: " + objCytology.Other_Patient_Info_All_Other_Pat;
                }
                ValueList.Add(value);
                cell = CreateCellForDataFromDB(HeaderList, ValueList, 1, false);
                table.AddCell(cell);
                if (table.TotalHeight > availableSpace - yPos)
                {
                    doc.NewPage();
                    availableSpace = pageSize.Height - 50;
                    yPos = 130;
                    location = 0;
                }
                table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
                yPos += table.TotalHeight + 10;
                location += table.TotalHeight;
                #endregion
            }
            else if (objAFp != null)
            {
                #region AFP
                table = new PdfPTable(new float[] { 33, 33, 33 });
                table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                cell = new PdfPCell(new Phrase("AFP Information", reducedFont));
                cell.Colspan = 3;
                table.AddCell(cell);
                HeaderList.Clear();
                ValueList.Clear();
                HeaderList.Add("Gestational Age (GA)");
                ValueList.Add("");
                HeaderList.Add("GA Days");
                //if (objAFp.Gestational_Age_Days != string.Empty)
                ValueList.Add(objAFp.Gestational_Age_Days.ToString());
                //else
                //    ValueList.Add("");
                HeaderList.Add("GA Weeks");
                //if (objAFp.Gestational_Age_Weeks != 0)
                ValueList.Add(objAFp.Gestational_Age_Weeks.ToString());
                //else
                //    ValueList.Add("");
                HeaderList.Add("GA Decimal");
                //if (objAFp.Gestational_Age_Decimal_Form != 0m)
                ValueList.Add(objAFp.Gestational_Age_Decimal_Form.ToString());
                //else
                //    ValueList.Add("");
                HeaderList.Add("GA Calc Date");
                if (objAFp.Gestational_Age_Date_Of_Calculation != string.Empty)
                {
                    try
                    {
                        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                        DateTime dt = DateTime.ParseExact(objAFp.Gestational_Age_Date_Of_Calculation, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                        ValueList.Add(dt.ToString("MM/dd/yyyy"));
                    }
                    catch
                    {
                        ValueList.Add("");
                    }
                }
                else
                {
                    ValueList.Add(objAFp.Gestational_Age_Date_Of_Calculation.ToString());
                }
                HeaderList.Add("GA Calc Method");
                if (objAFp.GA_Calculation_Method_EDD_EDC == "Y")
                    ValueList.Add(@"EDD\EDC");
                else if (objAFp.GA_Calculation_Method_LMP == "Y")
                    ValueList.Add("LMP Date");
                else if (objAFp.GA_Calculation_Method_Ultrasound == "Y")
                    ValueList.Add("Ultrasound Date");
                HeaderList.Add("LMP Date");
                if (objAFp.LMP_Date != string.Empty)
                {
                    try
                    {
                        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                        DateTime dt = DateTime.ParseExact(objAFp.LMP_Date, "yyyyMMdd", theCultureInfo);
                        ValueList.Add(dt.ToString("MM/dd/yyyy"));
                    }
                    catch (Exception e)
                    {
                        ValueList.Add("");
                    }
                }
                else
                {
                    ValueList.Add(objAFp.LMP_Date.ToString());
                }

                HeaderList.Add("EDD/EDC Date");
                ValueList.Add("");
                cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, true);
                table.AddCell(cell);
                HeaderList.Clear();
                ValueList.Clear();
                HeaderList.Add("Patient Weight");
                //ValueList.Add(objOrdersSubmit.Weight);
                ValueList.Add(sWeight);
                string value = string.Empty;
                HeaderList.Add("Insulin Dependent");
                ValueList.Add(objAFp.Insulin_Dependent);
                HeaderList.Add("# Fetuses");
                //if (objAFp.Number_Of_Fetuses != 0)
                ValueList.Add(objAFp.Number_Of_Fetuses.ToString());
                //else
                //    ValueList.Add("");
                HeaderList.Add("Patient Race");
                ValueList.Add(objHuman.Race);
                //HeaderList.Add("Routine Screening");
                //ValueList.Add(objAFp.Routine_Screening);
                //HeaderList.Add("Prev NTD");
                //ValueList.Add(objAFp.Previous_Neural_Tube_Defects);
                //HeaderList.Add("Adv Mat Age");
                //ValueList.Add(objAFp.Advanced_Maternal_Age);
                //HeaderList.Add("Hist Cyst Fib");
                //ValueList.Add(objAFp.History_Of_Cystic_Fibrosis);
                cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, true);
                table.AddCell(cell);
                HeaderList.Clear();
                ValueList.Clear();
                HeaderList.Add("Other Indications");
                ValueList.Add(objAFp.Other_Indications);
                HeaderList.Add("Prev Elevated AFP");
                ValueList.Add(objAFp.Previously_Elevated_AFP);
                //HeaderList.Add("Early GA");
                //ValueList.Add(objAFp.Reason_For_Repeat_Early_GA);
                //HeaderList.Add("Hemolyzed");
                //ValueList.Add(objAFp.Reason_For_Repeat_Hemolyzed);
                HeaderList.Add("Donor Egg");
                ValueList.Add(objAFp.Donor_Egg);
                HeaderList.Add("Donor Age");
                //if (objAFp.Age_Of_Egg_Donor != 0)
                ValueList.Add(objAFp.Age_Of_Egg_Donor.ToString());
                //else
                //    ValueList.Add("");
                HeaderList.Add("Additional Info");
                ValueList.Add(objAFp.Additional_Information);
                cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, true);
                table.AddCell(cell);
                HeaderList.Clear();
                ValueList.Clear();
                HeaderList.Add("CRL (mm)");
                //if (objAFp.Ultrasound_Measurement_Crown_Rump_Length != 0m)
                ValueList.Add(objAFp.Ultrasound_Measurement_Crown_Rump_Length.ToString());
                //else
                //    ValueList.Add("");
                HeaderList.Add("Nuchal Trans");
                //if (objAFp.Nuchal_Translucency != 0)
                ValueList.Add(objAFp.Nuchal_Translucency.ToString());
                //else
                //    ValueList.Add("");
                HeaderList.Add("CRL Twin B");
                //if (objAFp.Ultrasound_Measurement_Crown_Rump_Length_For_Twin_B != 0m)
                ValueList.Add(objAFp.Ultrasound_Measurement_Crown_Rump_Length_For_Twin_B.ToString());
                //else
                //    ValueList.Add("");
                HeaderList.Add("NT Twin B");
                //if (objAFp.Nuchal_Translucency_For_Twin_B != 0)
                ValueList.Add(objAFp.Nuchal_Translucency_For_Twin_B.ToString());
                //else
                //    ValueList.Add("");
                HeaderList.Add("CRL Date");
                if (objAFp.Ultrasound_Measurement_Crown_Rump_Length_Date != string.Empty)
                {
                    try
                    {
                        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                        DateTime dt = DateTime.ParseExact(objAFp.Ultrasound_Measurement_Crown_Rump_Length_Date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                        ValueList.Add(dt.ToString("MM/dd/yyyy"));
                    }
                    catch
                    {
                        ValueList.Add("");
                    }
                }
                else
                {
                    ValueList.Add(objAFp.Ultrasound_Measurement_Crown_Rump_Length_Date.ToString());
                }
                cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, true);
                table.AddCell(cell);
                HeaderList.Clear();
                ValueList.Clear();
                HeaderList.Add("Chorionicity");
                value = string.Empty;
                value = "Mono: " + objAFp.Chorionicity_Monochorionic;
                value += "\n" + "  Di: " + objAFp.Chorionicity_Dichorionic;
                value += "\n" + "  Unkown: " + objAFp.Chorionicity_Unknown;
                ValueList.Add(value);
                HeaderList.Add("Prior Down Syn/ONTD Screening");
                ValueList.Add(objAFp.Prior_Down_Syndrome_ONTD_Screening_On_Current_Pregnancy);
                HeaderList.Add("Prior Testing in First Trimester");
                ValueList.Add(objAFp.Prior_First_Trimester_Testing);
                HeaderList.Add("Prior Testing in Second Trimester");
                ValueList.Add(objAFp.Prior_Second_Trimester_Testing);
                HeaderList.Add("Family History of NTD");
                ValueList.Add(objAFp.FHX_NTD);
                HeaderList.Add("Prior Preg with Down Syn");
                ValueList.Add(objAFp.Prior_Pregnancy_With_Down_Syndrome);
                cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, true);
                table.AddCell(cell);
                HeaderList.Clear();
                ValueList.Clear();
                HeaderList.Add("Sono L Name");
                ValueList.Add(objAFp.Sonographer_Last_Name);
                HeaderList.Add("Sono F Name");
                ValueList.Add(objAFp.Sonographer_First_Name);
                HeaderList.Add("Sono ID");
                ValueList.Add(objAFp.Sonographer_ID_Number);
                HeaderList.Add("Credentialed By");
                value = string.Empty;
                if (objAFp.Credentialed_By_FMF == "Y")
                {
                    value = "FMF";
                }
                else if (objAFp.Credentialed_By_NTQR == "Y")
                {
                    value = "NTQR";
                }
                else if (objAFp.Credentialed_By_Other_Organization == "Y")
                {
                    value = "Other Organization";
                }
                ValueList.Add(value);
                HeaderList.Add("Site ID");
                ValueList.Add(objAFp.Site_Number);
                HeaderList.Add("Read MD ID");
                ValueList.Add(objAFp.Reading_Physician_ID);
                cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, true);
                table.AddCell(cell);
                if (table.TotalHeight > availableSpace - yPos)
                {
                    doc.NewPage();
                    availableSpace = pageSize.Height - 50;
                    yPos = 130;
                    location = 0;
                }
                table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
                yPos += table.TotalHeight + 10;
                location += table.TotalHeight;
                #endregion
            }
            else
            {


                table = new PdfPTable(new float[] { 50, 50 });
                table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                table.AddCell(new PdfPCell(new Phrase("Clinical Information", reducedFont)));
                table.AddCell(new PdfPCell(new Phrase("Additional Information", reducedFont)));
                //cell = new PdfPCell(new Phrase(objOrdersSubmit.Order_Notes, normalFont));
                HeaderList.Clear();
                ValueList.Clear();
                ValueList.Add(objOrdersSubmit.Order_Notes);
                cell = CreateCellForDataFromDB(null, ValueList, 2, false);
                table.AddCell(cell);
                HeaderList.Clear();
                ValueList.Clear();
                HeaderList.Add("Fasting");
                ValueList.Add(Fasting);
                HeaderList.Add("Height");
                //ValueList.Add(objOrdersSubmit.Height);
                ValueList.Add(sHeight);
                HeaderList.Add("Weight");
                //ValueList.Add(objOrdersSubmit.Weight);
                ValueList.Add(sWeight);
                HeaderList.Add("Total Volume");
                ValueList.Add(totVol);
                cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
                table.AddCell(cell);
                if (table.TotalHeight > availableSpace - yPos)
                {
                    doc.NewPage();
                    availableSpace = pageSize.Height - 50;
                    yPos = 130;
                    location = 0;
                }
                table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
                yPos += table.TotalHeight + 10;
                location += table.TotalHeight;

                table = new PdfPTable(new float[] { 100 });
                table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                table.AddCell(new PdfPCell(new Phrase("Micro/Histo Information", reducedFont)));
                HeaderList.Clear();
                ValueList.Clear();
                HeaderList.Add("Source");
                ValueList.Add(Speciment_Type);
                cell = CreateCellForDataFromDB(HeaderList, ValueList, 1, false);
                table.AddCell(cell);
                table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
                yPos += table.TotalHeight + 10;
                location += table.TotalHeight;

                if (objBloodLead != null)
                {
                    table = new PdfPTable(new float[] { 50, 50 });
                    table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
                    cell = new PdfPCell(new Phrase("Blood Lead Information", reducedFont));
                    cell.Colspan = 2;
                    table.AddCell(cell);
                    HeaderList.Clear();
                    ValueList.Clear();
                    HeaderList.Add("Race");
                    ValueList.Add(raceToPrint);
                    HeaderList.Add("Hispanic");
                    ValueList.Add(ethnicity);
                    cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
                    table.AddCell(cell);
                    HeaderList.Clear();
                    ValueList.Clear();
                    HeaderList.Add("Blood Lead Type");
                    ValueList.Add(objBloodLead.Blood_Lead_Type);
                    HeaderList.Add("Blood Lead Purpose");
                    ValueList.Add(objBloodLead.Blood_Lead_Type_Purpose);
                    cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
                    table.AddCell(cell);
                    table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
                    yPos += table.TotalHeight + 10;
                    location += table.TotalHeight;
                }
            }
            table = new PdfPTable(new float[] { 50, 50 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            string ICdCodes = string.Empty;
            for (int i = 0; i < objOrdDTO.OrderAssList.Count; i++)
            {
                if (ICdCodes.Contains(objOrdDTO.OrderAssList[i].ICD) == false)
                {
                    if (ICdCodes == string.Empty)
                    {
                        ICdCodes = objOrdDTO.OrderAssList[i].ICD;
                    }
                    else
                    {
                        ICdCodes += ", " + objOrdDTO.OrderAssList[i].ICD;
                    }
                }
            }
            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("Diagnosis Codes");
            ValueList.Add(ICdCodes);
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 1, false);
            cell.Colspan = 2;
            table.AddCell(cell);
            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("Bill Type");
            ValueList.Add(objOrdersSubmit.Bill_Type);
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
            table.AddCell(cell);
            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("LCA Ins Code");
            ValueList.Add(objlabCarrierPri.LCA_Carrier_Code);
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
            table.AddCell(cell);
            if (table.TotalHeight > availableSpace - yPos)
            {
                doc.NewPage();
                availableSpace = pageSize.Height - 50;
                yPos = 130;
                location = 0;
            }
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            yPos += table.TotalHeight + 10;
            location += table.TotalHeight;


            table = new PdfPTable(new float[] { 100 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            table.AddCell(new PdfPCell(new Phrase("Responsible Party / Guarantor Information", reducedFont)));
            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("RP Name");
            if (objHuman.Guarantor_Last_Name != string.Empty && objHuman.Guarantor_First_Name != string.Empty)
                ValueList.Add(objHuman.Guarantor_Last_Name + "," + objHuman.Guarantor_First_Name);
            else
                ValueList.Add(objHuman.Guarantor_Last_Name);
            HeaderList.Add("RP Address");
            if (objHuman.Guarantor_Street_Address1 != string.Empty && objHuman.Guarantor_Street_Address2 != string.Empty)
                ValueList.Add(objHuman.Guarantor_Street_Address1 + ", " + objHuman.Guarantor_Street_Address2);
            else
                ValueList.Add(objHuman.Guarantor_Street_Address1);
            HeaderList.Add("RP City, State Zip");
            if (objHuman.Guarantor_City != string.Empty && objHuman.Guarantor_State != string.Empty)
                ValueList.Add(objHuman.Guarantor_City + ", " + objHuman.Guarantor_State + " " + objHuman.Guarantor_Zip_Code);
            else
                ValueList.Add(objHuman.Guarantor_City);
            HeaderList.Add("RP Phone");
            ValueList.Add(objHuman.Guarantor_Home_Phone_Number);
            HeaderList.Add("RP relation to Pt");
            switch (objHuman.Guarantor_Relationship_No)
            {
                case 1:
                    ValueList.Add(objHuman.Guarantor_Relationship);
                    break;
                case 2:
                    ValueList.Add(objHuman.Guarantor_Relationship);
                    break;
                default:
                    ValueList.Add("Other");
                    break;
            }

            cell = CreateCellForDataFromDB(HeaderList, ValueList, 1, false);
            table.AddCell(cell);
            if (table.TotalHeight > availableSpace - yPos)
            {
                doc.NewPage();
                availableSpace = pageSize.Height - 50;
                yPos = 130;
                location = 0;
            }
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            yPos += table.TotalHeight + 10;
            location += table.TotalHeight;

            table = new PdfPTable(new float[] { 35, 35, 30 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("ABN");
            ValueList.Add(objOrdersSubmit.Is_ABN_Signed);
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
            table.AddCell(cell);
            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("Worker's Comp");
            ValueList.Add(workerComp);
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
            table.AddCell(cell);
            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("Date of Injury");
            ValueList.Add("");
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
            table.AddCell(cell);
            if (table.TotalHeight > availableSpace - yPos)
            {
                doc.NewPage();
                availableSpace = pageSize.Height - 50;
                yPos = 130;
                location = 0;
            }
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            yPos += table.TotalHeight + 10;
            location += table.TotalHeight;

            table = new PdfPTable(new float[] { 50, 50 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            cell = new PdfPCell(new Phrase("Insurance Information", reducedFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            table.AddCell(new PdfPCell(new Phrase("Primary Insurance", reducedFont)));
            table.AddCell(new PdfPCell(new Phrase("Secondary Insurance", reducedFont)));
            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("LCA Ins Code");
            ValueList.Add(objlabCarrierPri.LCA_Carrier_Code);
            HeaderList.Add("Ins Co Name");
            ValueList.Add(objPriPlan.Ins_Plan_Name);
            HeaderList.Add("Ins Address 1");
            ValueList.Add(objPriPlan.Payer_Addrress1);
            HeaderList.Add("Ins Address 2");
            ValueList.Add(objPriPlan.Payer_Addrress2);
            HeaderList.Add("Ins City, State Zip");
            ValueList.Add(objPriPlan.Payer_City + "," + objPriPlan.Payer_State + " " + objPriPlan.Payer_Zip);
            HeaderList.Add("Policy Number");
            ValueList.Add(objPatInsPri.Policy_Holder_ID);
            HeaderList.Add("Group #");
            ValueList.Add(objPatInsPri.Group_Number);
            HeaderList.Add("Emp/Group Name");
            ValueList.Add(objHuman.Employer_Name);
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
            table.AddCell(cell);
            HeaderList.Clear();
            ValueList.Clear();
            HeaderList.Add("LCA Ins Code");
            ValueList.Add(objLabCarrierSec.LCA_Carrier_Code);
            HeaderList.Add("Ins Co Name");
            ValueList.Add(objSecPlan.Ins_Plan_Name);
            HeaderList.Add("Ins Address 1");
            ValueList.Add(objSecPlan.Payer_Addrress1);
            HeaderList.Add("Ins Address 2");
            ValueList.Add(objSecPlan.Payer_Addrress2);
            HeaderList.Add("Ins City, State Zip");
            ValueList.Add(objSecPlan.Payer_City + "," + objSecPlan.Payer_State + " " + objSecPlan.Payer_Zip);
            HeaderList.Add("Policy Number");
            ValueList.Add(objPatInsSec.Policy_Holder_ID);
            HeaderList.Add("Group #");
            ValueList.Add(objPatInsSec.Group_Number);
            HeaderList.Add("Emp/Group Name");
            if (objSecPlan.Ins_Plan_Name != string.Empty)
                ValueList.Add(objHuman.Employer_Name);
            else
                ValueList.Add(string.Empty);
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
            table.AddCell(cell);
            table.AddCell(new PdfPCell(new Phrase("Primary Policy Holder / Insured", reducedFont)));
            table.AddCell(new PdfPCell(new Phrase("Secondary Policy Holder / Insured", reducedFont)));
            HeaderList.Clear();
            ValueList.Clear();
            Human objpatinsuredPrihuman = new Human();

            if (objPatInsPri.Insured_Human_ID != objHuman.Human_ID && objPatInsPri.Insured_Human_ID != 0)
            {
                objpatinsuredPrihuman = GetHumanByHumanID(objPatInsPri.Insured_Human_ID);
            }
            HeaderList.Add("Insured Name");
            ValueList.Add(objpatinsuredPrihuman.Last_Name + "," + objpatinsuredPrihuman.First_Name);
            HeaderList.Add("Insured Address");
            ValueList.Add(objpatinsuredPrihuman.Street_Address1);
            HeaderList.Add("Insured Relation to Pt");
            ValueList.Add(objPatInsPri.Relationship);
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
            table.AddCell(cell);
            HeaderList.Clear();
            ValueList.Clear();
            Human objpatinsuredSechuman = new Human();

            if (objPatInsSec.Insured_Human_ID != objHuman.Human_ID && objPatInsSec.Insured_Human_ID != 0)
            {
                objpatinsuredSechuman = GetHumanByHumanID(objPatInsSec.Insured_Human_ID);
            }
            HeaderList.Add("Insured Name");
            ValueList.Add(objpatinsuredSechuman.Last_Name + "," + objpatinsuredSechuman.First_Name);
            HeaderList.Add("Insured Address");
            ValueList.Add(objpatinsuredSechuman.Street_Address1);
            HeaderList.Add("Insured Relation to Pt");
            ValueList.Add(objPatInsSec.Relationship);
            cell = CreateCellForDataFromDB(HeaderList, ValueList, 2, false);
            table.AddCell(cell);
            if (table.TotalHeight > availableSpace - yPos)
            {
                doc.NewPage();
                availableSpace = pageSize.Height - 50;
                yPos = 130;
                location = 0;
            }
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            yPos += table.TotalHeight + 10;

            table = new PdfPTable(new float[] { 100 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            cell = new PdfPCell(new Phrase("Authorization - Please sign and Date", normalFont));
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cell);
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            yPos += table.TotalHeight + 5;

            table = new PdfPTable(new float[] { 100 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            cell = new PdfPCell(new Phrase("I hereby authorize the release of medical information related to the services described hereon and authorize payment directly to Laboratory Corporation of America. I agree to assume responsibility for payment of charges for laboratory services that are not covered by my healthcare insurer.", normalFont));
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cell);
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            yPos += table.TotalHeight + 15;

            table = new PdfPTable(new float[] { 100 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            cell = new PdfPCell(new Phrase("_________________________________________                               _________________________", normalFont));
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cell);
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            yPos += table.TotalHeight + 5;

            table = new PdfPTable(new float[] { 100 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            cell = new PdfPCell(new Phrase("Patient Signature                                                                                      Date", normalFont));
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cell);
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            yPos += table.TotalHeight + 15;

            table = new PdfPTable(new float[] { 100 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            cell = new PdfPCell(new Phrase("_________________________________________                               _________________________", normalFont));
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cell);
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            yPos += table.TotalHeight + 5;

            table = new PdfPTable(new float[] { 100 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            cell = new PdfPCell(new Phrase("Physician Signature                                                                                  Date", normalFont));
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cell);
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            yPos += table.TotalHeight + 5;

            table = new PdfPTable(new float[] { 100 });
            table.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            cell = new PdfPCell(new Phrase(Environment.NewLine + Environment.NewLine + "SOME INSURANCES OR MEDICARE MAY NOT PAY FOR VITAMIN D LEVEL, PLEASE VERIFY WITH THE LAB AS YOU MAY GET BILLED FOR IT", reducedFont));
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table.AddCell(cell);
            table.WriteSelectedRows(0, -1, pageSize.GetLeft(xPos), pageSize.GetTop(yPos), wr.DirectContent);
            yPos += table.TotalHeight + 5;

            //par = new Paragraph("Authorization - Please sign and Date", normalFont);
            //doc.Add(par);
            //par = new Paragraph("I hereby authorize the release of medical information related to the services described hereon and authorize payment directly to Laboratory Corporation of America. I agree to assume responsibility for payment of charges for laboratory services that are not covered by my healthcare insurer.", normalFont);
            //doc.Add(par);
            //doc.Add(new Paragraph("\n"));
            //par = new Paragraph("_________________________________________                               _________________________", normalFont);
            //doc.Add(par);
            //par = new Paragraph("Patient Signature                                                                                      Date", normalFont);
            //doc.Add(par);
            //doc.Add(new Paragraph("\n"));
            //par = new Paragraph("_________________________________________                               _________________________", normalFont);
            //doc.Add(par);
            //par = new Paragraph("Physician Signature                                                                                  Date", normalFont);
            //doc.Add(par);

            doc.Close();
            //if (CallingFrom.ToUpper() != "LABAGENT")
            //    System.Diagnostics.Process.Start(sPrintPathName);
            return sPrintPathName;
        }

        public class HeaderEventGenerateForLabCorp : PdfPageEventHelper
        {
            string Acc;
            string humanName;
            string ControlNo;
            string PatId;
            string collDate;
            string humanID;
            public HeaderEventGenerateForLabCorp(string _Acc, string _humanName, string _ControlNo, string _PatId, string _collDate, string _humanID)
            {
                Acc = _Acc;
                humanName = _humanName;
                ControlNo = _ControlNo;
                PatId = _PatId;
                collDate = _collDate;
                humanID = _humanID;
            }
            iTextSharp.text.Font redBoldFont = iTextSharp.text.FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            public override void OnStartPage(PdfWriter writer, Document document)
            {
                if (document.PageNumber != 0)
                {
                    Paragraph par = new Paragraph("Page " + document.PageNumber + " of 2", redBoldFont);
                    par.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    document.Add(par);
                    document.Add(PrintOrders.CreateHeaderTable(Acc, humanName, ControlNo, PatId, collDate, humanID));
                }
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                //if (document.PageNumber == 1)
                //{
                //    document.Add(frmLabOrderSend.CreateSpecimenTable(headerEvntsegmentTable));
                //}
            }

        }
        #region Quest_Simple_Order
        public string PrintRequisitionUsingDatafromDBQuest(ulong OrderId, string appConfigKey, String CallingFrom)
        {
            iTextSharp.text.Font QuestEOrPSC = new iTextSharp.text.Font();
            string clientAddress = string.Empty;
            string Interface_identifier = string.Empty;
            string AccountNumberForQuest = string.Empty;
            ulong HumanId = 0;
            ulong PhysicianID = 0;
            OrdersManager objOrdersManager = new OrdersManager();
            //SpecimenProxy objSpecimenproxy = new SpecimenProxy();
            OrderSpecimenDTO objOrdDTO = objOrdersManager.LoadSpecimen(OrderId, "DIAGNOSTIC ORDER");
            LabcorpSettingsManager objLabcorpSettingsManager = new LabcorpSettingsManager();
            IList<LabSettings> ilstLabSettingsLibrary = objLabcorpSettingsManager.GetLabcorpSettings();
            IList<LabSettings> ilstLabcorpSettings = (from lst in ilstLabSettingsLibrary where lst.Lab_ID == objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Lab_ID select lst).ToList<LabSettings>();
            if (objOrdDTO == null || (objOrdDTO != null && objOrdDTO.ilstOrderLabDetailsDTO.Count == 0) || objOrdDTO.objHuman == null)
                return string.Empty;
            //IList<Specimen> specList = objOrdDTO.ilstOrderLabDetailsDTO.Select(a=>a.objSpecimen).ToList<Specimen>();
            if (ilstLabcorpSettings != null && ilstLabcorpSettings.Count > 0)
                clientAddress = ilstLabcorpSettings[0].Client_Information;

            string specDate = string.Empty;
            string specTime = string.Empty;
            string humanName = string.Empty;
            string humanAddress = string.Empty;
            string humanCity = string.Empty;
            string SSN = string.Empty;
            string orderCode = string.Empty;
            string testOrdered = string.Empty;
            string testcount = string.Empty;
            string totVol = string.Empty;
            string collDate = string.Empty;
            Orders objOrder = new Orders();
            string controlReqNumber = "ACUR" + OrderId.ToString();
            string Sex = string.Empty;
            string BirthDate = string.Empty;
            string RequestionNo = string.Empty;
            string PSC_Or_E = string.Empty;
            OrdersSubmit objOrdersSubmit = new OrdersSubmit();
            string Fasting = string.Empty;
            string notes = string.Empty;
            testcount = objOrdDTO.ilstOrderLabDetailsDTO.Count.ToString();
            if (objOrdDTO.ilstOrderLabDetailsDTO.Count > 0)
            {
                totVol = objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Quantity.ToString();
                Fasting = objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Fasting.ToString();
                //objSpecimen = objOrdDTO.ilstOrderLabDetailsDTO[0].objSpecimen;
                objOrdersSubmit = objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit;
                objOrder = objOrdDTO.ilstOrderLabDetailsDTO[0].ObjOrder;
                collDate = objOrdDTO.ilstOrderLabDetailsDTO[0].ObjOrder.Created_Date_And_Time.ToString("MM/dd/yyyy hh:mm tt");
                specDate = objOrdDTO.ilstOrderLabDetailsDTO[0].ObjOrder.Created_Date_And_Time.ToString("MM-dd-yyyy");
                specTime = objOrdDTO.ilstOrderLabDetailsDTO[0].ObjOrder.Created_Date_And_Time.ToString("hh:mm tt");
                for (int i = 0; i < objOrdDTO.ilstOrderLabDetailsDTO.Count; i++)
                {
                    Orders obj = objOrdDTO.ilstOrderLabDetailsDTO[i].ObjOrder;
                    PhysicianID = obj.Physician_ID;
                    HumanId = obj.Human_ID;
                    //if (i == 0)
                    //{
                    //    testOrdered = obj.Lab_Procedure_Description;
                    //    orderCode = obj.Order_Code_Type + "  " + obj.Lab_Procedure;
                    //}
                    //else
                    //{
                    testOrdered += "\n" + obj.Lab_Procedure_Description;
                    orderCode += obj.Lab_Procedure + "-" + obj.Lab_Procedure_Description + "\n";
                    notes += " " + objOrdersSubmit.Order_Notes;
                    //}
                }
                //orderCode += "\n";
                RequestionNo = "ACUR" + objOrder.Id.ToString();
                if (objOrdersSubmit.Specimen_In_House == "Y")
                {
                    PSC_Or_E = "e";
                    QuestEOrPSC = QuestBigE;
                }
                else
                {
                    PSC_Or_E = "PSC HOLD ORDER";
                    QuestEOrPSC = QuestHeading;
                }
            }
            PhysicianManager objPhysicianManager = new PhysicianManager();
            PhysicianLibrary objPhy = objPhysicianManager.GetphysiciannameByPhyID(objOrdDTO.ilstOrderLabDetailsDTO.Select(a => a.ObjOrder.Physician_ID).ToList<ulong>()[0])[0];
            FillHumanDTO objHuman = objOrdDTO.objHuman;
            //objPhy = AllLibraries.Instance.GetphysiciannameByPhyID(PhysicianID, false);
            //objHuman = EncounterManager.Instance.GetHumanByHumanID(HumanId);
            humanName = GenerateHumanName(objHuman.First_Name, objHuman.Last_Name, objHuman.MI);
            humanAddress = GenerateStreetAddress(objHuman.Street_Address1, objHuman.Street_Address2);
            humanCity = GenerateCityAndZip(objHuman.City, objHuman.State, objHuman.ZipCode);
            SSN = objHuman.SSN;
            Sex = GenerateSex(objHuman.Sex);
            BirthDate = objHuman.Birth_Date.ToString("dd-MMM-yyyy");
            InsurancePlan objPriPlan = new InsurancePlan();
            InsurancePlan objSecPlan = new InsurancePlan();
            PatientInsuredPlan objPatInsPri = new PatientInsuredPlan();
            PatientInsuredPlan objPatInsSec = new PatientInsuredPlan();
            LabCarrierLookUp objlabCarrierPri = new LabCarrierLookUp();
            LabCarrierLookUp objLabCarrierSec = new LabCarrierLookUp();
            Hashtable InsuredIds = new Hashtable();
            InsurancePlanManager objInsurancePlanManager = new InsurancePlanManager();
            LabCarrierLookUpManager objLabCarrierLookUpManager = new LabCarrierLookUpManager();
            if (objHuman.PatientInsuredBag.Count > 0)
            {
                foreach (PatientInsuredPlan obj in objHuman.PatientInsuredBag)
                {
                    if (obj.Insurance_Type.ToUpper() == "PRIMARY")
                    {
                        objPatInsPri = obj;
                        objPriPlan = objInsurancePlanManager.GetInsurancebyID(obj.Insurance_Plan_ID)[0];
                        objlabCarrierPri = objLabCarrierLookUpManager.GetLabCarrierDetailsForInsPlanID(obj.Insurance_Plan_ID, objOrdersSubmit.Lab_ID);
                        if (!InsuredIds.ContainsKey("Primary"))
                            InsuredIds.Add("Primary", obj.Insured_Human_ID);
                    }
                    else if (obj.Insurance_Type.ToUpper() == "SECONDARY")
                    {
                        objPatInsSec = obj;
                        objSecPlan = objInsurancePlanManager.GetInsurancebyID(obj.Insurance_Plan_ID)[0];
                        objLabCarrierSec = objLabCarrierLookUpManager.GetLabCarrierDetailsForInsPlanID(obj.Insurance_Plan_ID, objOrdersSubmit.Lab_ID);
                        if (!InsuredIds.ContainsKey("Secondary"))
                            InsuredIds.Add("Secondary", obj.Insured_Human_ID);
                    }

                }
            }
            FillHumanDTO ObjHumanPrimary = null;
            FillHumanDTO ObjHumanSecondary = null;
            if (objOrdersSubmit.Bill_Type == "Third Party")
            {
                foreach (DictionaryEntry dct in InsuredIds)
                {
                    if (dct.Key.ToString() == "Primary")
                    {
                        ObjHumanPrimary = new FillHumanDTO();
                        if (Convert.ToUInt32(dct.Value) == objHuman.Human_ID)
                        {
                            ObjHumanPrimary = objHuman;
                        }
                        else
                        {
                            ObjHumanPrimary = (from temphumanobj in objOrdDTO.ilstGuarantor where temphumanobj.Human_ID == Convert.ToUInt32(dct.Value) select temphumanobj).ToList<FillHumanDTO>()[0];
                        }
                    }
                    else if (dct.Key.ToString() == "Secondary")
                    {
                        ObjHumanSecondary = new FillHumanDTO();
                        if (Convert.ToUInt32(dct.Value) == objHuman.Human_ID)
                        {
                            ObjHumanSecondary = objHuman;
                        }
                        else if (Convert.ToUInt32(dct.Value) == 0)
                        {
                            ObjHumanSecondary = null;
                        }
                        else
                        {
                            ObjHumanSecondary = (from temphumanobj in objOrdDTO.ilstGuarantor where temphumanobj.Human_ID == Convert.ToUInt32(dct.Value) select temphumanobj).ToList<FillHumanDTO>()[0];
                        }
                    }

                }
            }
            FacilityManager objFacilityManager = new FacilityManager();
            IList<FacilityLibrary> ilstFacility = objFacilityManager.GetFacilityByFacilityname(ClientSession.FacilityName);
            IList<FacilityLibrary> tempFacilityLibrary = objFacilityManager.GetFacilityByFacilityname(objOrdersSubmit.Facility_Name);
            if (tempFacilityLibrary.Count > 0)
            {
                clientAddress += "\n" + tempFacilityLibrary[0].Fac_Address1 + "\n" + tempFacilityLibrary[0].Fac_City + "," + tempFacilityLibrary[0].Fac_State + "," + tempFacilityLibrary[0].Fac_Zip + "\n" + tempFacilityLibrary[0].Fac_Telephone;
            }
            if (ilstFacility.Count > 0)
            {
                AccountNumberForQuest = ilstFacility[0].Quest_Account_Number;
            }
            string ICdCodes = string.Empty;
            for (int i = 0; i < objOrdDTO.OrderAssList.Count; i++)
            {
                if (ICdCodes.Contains(objOrdDTO.OrderAssList[i].ICD) == false)
                {
                    if (ICdCodes == string.Empty)
                    {
                        ICdCodes = objOrdDTO.OrderAssList[i].ICD;
                    }
                    else
                    {
                        ICdCodes += ", " + objOrdDTO.OrderAssList[i].ICD;
                    }
                }
            }
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
            string sPrintPathName = string.Empty;
            string folderPath = appConfigKey;
            //string folderPath = @"E:\Altova LabCorp Output";
            Directory.CreateDirectory(folderPath);
            sPrintPathName = "D:\\" + folderPath + "\\Order_Requisition_" + controlReqNumber + ".pdf";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            iTextSharp.text.Rectangle pageSize = doc.PageSize;


            doc.Open();

            HeaderEventGenerateForQuest headerEvent = new HeaderEventGenerateForQuest(controlReqNumber, AccountNumberForQuest, PSC_Or_E, QuestEOrPSC, humanName, humanAddress, humanCity, objHuman.Home_Phone_No, specDate, specTime, objHuman.Human_ID.ToString(), BirthDate, Sex, Fasting, controlReqNumber, clientAddress);
            //HeaderEventGenerateForQuest pageeventhandler = new HeaderEventGenerateForQuest(controlReqNumber, AccountNumberForQuest, PSC_Or_E, QuestEOrPSC);
            //wr.PageEvent = pageeventhandler;
            wr.PageEvent = headerEvent;
            headerEvent.OnOpenDocument(wr, doc);
            headerEvent.OnStartPage(wr, doc);
            headerEvent.OnEndPage(wr, doc);
            #region HeaderTable
            //table 1
            IList<CreateTableQuest> ilstPatientInformation = new List<CreateTableQuest>();

            PdfPTable table;
            PdfPCell cell;
            // PdfPTable innertable;
            //table 2
            //table = new PdfPTable(new float[] { 30, 40, 30 });
            //table.WidthPercentage = 100;
            //cell = new PdfPCell(new Phrase("Chaparral Medical Group \nAttn: Administrator \n840 Town Center Drive \nPomona, CA 91767 ",normalFont));
            //cell.Border = Rectangle.NO_BORDER;
            //cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
            //cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            //table.AddCell(cell);
            //cell = new PdfPCell(new Phrase("For Lab Use Only",normalFont));
            //cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            //cell.Border = Rectangle.BOX;
            //cell.BorderColor = BaseColor.LIGHT_GRAY;
            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            //cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
            //table.AddCell(cell);
            //innertable = new PdfPTable(1);
            //table.WidthPercentage = 100;
            //cell = new PdfPCell(new Phrase("Patient Information", reducedFont));
            //cell.Border = Rectangle.BOX;
            //cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            //cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
            //innertable.AddCell(cell);
            //ilstPatientInformation = new List<CreateTableQuest>();
            //ilstPatientInformation.Add(ObjectGenerator(string.Empty, humanName, 0,normalFont));
            //ilstPatientInformation.Add(ObjectGenerator(string.Empty, humanAddress, 0, normalFont));
            //ilstPatientInformation.Add(ObjectGenerator(string.Empty, humanCity, 0, normalFont));
            //ilstPatientInformation.Add(ObjectGenerator(string.Empty, objHuman.Home_Phone_No, 0, normalFont));
            //cell = new PdfPCell(GenerateTable(ilstPatientInformation,1));
            //cell.Border = Rectangle.BOX;
            //innertable.AddCell(cell);
            //cell = new PdfPCell(innertable);
            //table.AddCell(cell);
            //doc.Add(table);
            ////table 3
            //table = new PdfPTable(new float[] { 50, 50 });
            //table.WidthPercentage = 100;
            //ilstPatientInformation = new List<CreateTableQuest>();
            //ilstPatientInformation.Add(ObjectGenerator("Collection Date", specDate, 2, normalFont));
            //ilstPatientInformation.Add(ObjectGenerator("Time",specTime, 0, normalFont));
            ////ilstPatientInformation.Add(ObjectGenerator("Volume", "", 0, normalFont));
            ////ilstPatientInformation.Add(ObjectGenerator("Hours", "", 0, normalFont));
            //ilstPatientInformation.Add(ObjectGenerator("Lab Reference ID", controlReqNumber, 2, reducedFont));
            //ilstPatientInformation.Add(ObjectGenerator("Fasting", objOrder.Fasting, 0, normalFont));
            //cell = new PdfPCell(GenerateTable(ilstPatientInformation, 3));
            //cell.Border = Rectangle.BOX;
            //table.AddCell(cell);
            //ilstPatientInformation = new List<CreateTableQuest>();

            //ilstPatientInformation.Add(ObjectGenerator("Pat ID#", HumanId.ToString(),0, reducedFont));
            ////ilstPatientInformation.Add(ObjectGenerator("SSN", SSN, 0, normalFont));
            //ilstPatientInformation.Add(ObjectGenerator("Sex", Sex, 0, normalFont));
            //ilstPatientInformation.Add(ObjectGenerator("DOB", BirthDate, 2, normalFont));

            ////ilstPatientInformation.Add(ObjectGenerator("Room/Loc",objOrdDTO.Exam_Room, 0, normalFont));
            ////ilstPatientInformation.Add(ObjectGenerator("Result Notification", "Normal", 3, normalFont));
            //cell = new PdfPCell(GenerateTable(ilstPatientInformation, 2));
            //cell.Border = Rectangle.BOX;
            //table.AddCell(cell);
            table = new PdfPTable(2);
            table.WidthPercentage = 100;
            ilstPatientInformation = new List<CreateTableQuest>();
            ilstPatientInformation.Add(new CreateTableQuest("Ordering Physician", objPhy.PhyLastName + ", " + objPhy.PhyFirstName, 2, normalFont));
            //ilstPatientInformation.Add(ObjectGenerator("Physician Degree",objPhy.PhySuffix, 0, normalFont));
            ilstPatientInformation.Add(new CreateTableQuest("Physician NPI", objPhy.PhyNPI, 0, normalFont));
            ilstPatientInformation.Add(new CreateTableQuest("Internal Comments", notes.Trim(), 0, normalFont));
            ilstPatientInformation.Add(new CreateTableQuest("Report Comments", "", 0, normalFont));
            ilstPatientInformation.Add(new CreateTableQuest(string.Empty, "", 2, normalFont));
            if (objOrdersSubmit.Bill_Type == "Third Party")
            {
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, "---", 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Bill Type", objOrdersSubmit.Bill_Type, 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, ":Guarantor", 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Name", GenerateHumanName(objHuman.Guarantor_First_Name, objHuman.Guarantor_Last_Name, objHuman.Guarantor_MI), 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Address", "", 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, GenerateStreetAddress(objHuman.Guarantor_Street_Address1, objHuman.Guarantor_Street_Address2), 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, GenerateCityAndZip(objHuman.Guarantor_City, objHuman.Guarantor_State, objHuman.Guarantor_Zip_Code), 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Relationship", objHuman.Guarantor_Relationship, 0, normalFont));
            }
            else if (objOrdersSubmit.Bill_Type == "Client")
            {
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, "---", 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Bill Type", objOrdersSubmit.Bill_Type, 2, normalFont));
            }
            else if (objOrdersSubmit.Bill_Type == "Patient")
            {
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, "---", 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Bill Type", objOrdersSubmit.Bill_Type, 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, ":Guarantor", 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Name", GenerateHumanName(objHuman.Guarantor_First_Name, objHuman.Guarantor_Last_Name, objHuman.Guarantor_MI), 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Address", "", 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, GenerateStreetAddress(objHuman.Guarantor_Street_Address1, objHuman.Guarantor_Street_Address2), 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, GenerateCityAndZip(objHuman.Guarantor_City, objHuman.Guarantor_State, objHuman.Guarantor_Zip_Code), 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Relationship", objHuman.Guarantor_Relationship, 2, normalFont));
            }
            cell = new PdfPCell(GenerateTable(ilstPatientInformation, 1));
            cell.Border = Rectangle.BOX;
            table.AddCell(cell);
            ilstPatientInformation = new List<CreateTableQuest>();
            //if (objOrder.Bill_Type == "Client")
            //{
            //    ilstPatientInformation.Add(ObjectGenerator("Bill Type", objOrder.Bill_Type, 2, normalFont));
            //}
            //else if (objOrder.Bill_Type == "Patient")
            //{
            //    ilstPatientInformation.Add(ObjectGenerator("Bill Type", objOrder.Bill_Type, 2, normalFont));
            //    ilstPatientInformation.Add(ObjectGenerator(string.Empty,":Guarantor", 2, normalFont));
            //    ilstPatientInformation.Add(ObjectGenerator("Name", GenerateHumanName(objHuman.Guarantor_First_Name, objHuman.Guarantor_Last_Name, objHuman.Guarantor_MI), 2, normalFont));
            //    ilstPatientInformation.Add(ObjectGenerator("Address", "", 2, normalFont));
            //    ilstPatientInformation.Add(ObjectGenerator(string.Empty, GenerateStreetAddress(objHuman.Guarantor_Street_Address1, objHuman.Guarantor_Street_Address2), 2, normalFont));
            //    ilstPatientInformation.Add(ObjectGenerator(string.Empty, GenerateCityAndZip(objHuman.Guarantor_City, objHuman.Guarantor_State, objHuman.Guarantor_Zip_Code), 2, normalFont));
            //    ilstPatientInformation.Add(ObjectGenerator("Relationship", objHuman.Guarantor_Relationship, 2, normalFont));
            //}
            if (objOrdersSubmit.Bill_Type == "Third Party")
            {
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, ":Primary Insurance", 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Quest Bill Code", objlabCarrierPri.LCA_Carrier_Code, 2, normalFont));
                //ilstPatientInformation.Add(ObjectGenerator("Insured Name", GenerateHumanName(ObjHumanPrimary.First_Name,ObjHumanPrimary.Last_Name,ObjHumanPrimary.MI), 2, normalFont));
                //ilstPatientInformation.Add(ObjectGenerator("Insured Address","", 2, normalFont));
                //ilstPatientInformation.Add(ObjectGenerator(string.Empty, GenerateStreetAddress(ObjHumanPrimary.Street_Address1, ObjHumanPrimary.Street_Address2), 2, normalFont));
                //ilstPatientInformation.Add(ObjectGenerator(string.Empty, GenerateCityAndZip(ObjHumanPrimary.City, ObjHumanPrimary.State, objHuman.ZipCode), 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Primary Ins Co Name", objPriPlan.Ins_Plan_Name, 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Primary Ins Co Address", "", 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, objPriPlan.Payer_Addrress1, 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, GenerateCityAndZip(objPriPlan.Payer_City, objPriPlan.Payer_State, objPriPlan.Payer_Zip), 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Policy #", objPatInsPri.Policy_Holder_ID, 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Group #", objPatInsPri.Group_Number, 0, normalFont));
                if (ObjHumanSecondary != null)
                {
                    ilstPatientInformation.Add(new CreateTableQuest(string.Empty, "", 2, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest(string.Empty, ":Secondary Insurance", 2, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest("Quest Bill Code", objLabCarrierSec.LCA_Carrier_Code, 2, normalFont));
                    //ilstPatientInformation.Add(ObjectGenerator("Insured Name", GenerateHumanName(ObjHumanSecondary.First_Name, ObjHumanSecondary.Last_Name, ObjHumanSecondary.MI), 2, normalFont));
                    //ilstPatientInformation.Add(ObjectGenerator("Insured Address", "", 2, normalFont));
                    //ilstPatientInformation.Add(ObjectGenerator(string.Empty, GenerateStreetAddress(ObjHumanSecondary.Street_Address1, ObjHumanSecondary.Street_Address2), 2, normalFont));
                    //ilstPatientInformation.Add(ObjectGenerator(string.Empty, GenerateCityAndZip(ObjHumanSecondary.City, ObjHumanSecondary.State, ObjHumanSecondary.ZipCode), 2, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest("Secondary Ins Co Name", objPriPlan.Ins_Plan_Name, 2, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest("Secondary Ins Co Address", "", 2, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest(string.Empty, objSecPlan.Payer_Addrress1, 2, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest(string.Empty, GenerateCityAndZip(objSecPlan.Payer_City, objSecPlan.Payer_State, objSecPlan.Payer_Zip), 2, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest("Policy #", objPatInsSec.Policy_Holder_ID, 0, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest("Group #", objPatInsSec.Group_Number, 0, normalFont));
                }

            }
            cell = new PdfPCell(GenerateTable(ilstPatientInformation, 2));
            cell.Border = Rectangle.BOX;
            table.AddCell(cell);
            doc.Add(table);
            //table-4
            table = new PdfPTable(1);
            table.WidthPercentage = 100;
            ilstPatientInformation = new List<CreateTableQuest>();
            ilstPatientInformation.Add(new CreateTableQuest("ICD Diagnosis Code(s)", ICdCodes, 0, normalFont));
            cell = new PdfPCell(GenerateTable(ilstPatientInformation, 1));
            table.AddCell(cell);
            doc.Add(table);
            doc.Add(new Phrase("\n"));
            table = new PdfPTable(1);
            table.WidthPercentage = 100;

            //if (objOrder.Bill_Type == "Third Party")
            //doc.Add(new Phrase("\n"));
            //else

            cell = new PdfPCell(new Phrase("Profiles/Tests", WhiteFont));
            cell.MinimumHeight = 10;
            cell.HorizontalAlignment = Rectangle.ALIGN_CENTER;
            cell.BackgroundColor = BaseColor.BLACK;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            doc.Add(table);
            orderCode = "227-ALDOLASE\n2302-BERMUDA GRASS (G2) IGE\n23029-IRON, TOTAL (REFL)\n23086-ACANTHAMOEBA/NAEGLERIA CULTURE\n227-ALDOLASE\n2310-JOHNSON GRASS (G10) IGE\n23230-MESORIDAZINE\n23475-GLUCOSE TOLERANCE TEST, 3 SPECIMENS, (75G)\n23497-DRUG ABUSE PANEL 6-50\n23860-MANGO FRUIT (F91) IGE\n2409-ENGLISH PLANTAIN (W9) IGE\n24989-CHLORAL HYDRATE, BLOOD\n2522-HICKORY/PECAN TREE (T22) IGE\n2556-QUEEN PALM (T72) IGE\n2569-BRUSSEL SPROUTS (RF217) IGE\n2610-CHILI PEPPER (F279) IGE**\n2633-ALLERGEN SPECIFIC IGE CARAWAY SEED*\n26509-PHENOTHIAZINES PANEL (U)\n26517-GLYBURIDE (MICRONASE)\n26524-CBC (INCLUDES DIFF/PLT) WITH PATHOLOGIST REVIEW\n2654-RABBIT EPITHELIA (E82) IGE\n2657-MOUSE EPITHELIA (E71) IGE\n2661-GOOSE FEATHERS (E70) IGE";
            Paragraph par = new Paragraph(new Phrase(orderCode, normalFont));
            //cell.Border = Rectangle.BOX;
            //cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            //table.AddCell(cell);
            par.SetAlignment("Left");
            doc.Add(par);
            //cell = new PdfPCell(new Phrase("End of Requestion :" + controlReqNumber, normalFont7));

            par = new Paragraph("End of Requisition : " + controlReqNumber, normalFont);
            par.SetAlignment("Center");
            doc.Add(par);
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("SOME INSURANCES OR MEDICARE MAY NOT PAY FOR VITAMIN D LEVEL, PLEASE VERIFY WITH THE LAB AS YOU MAY GET BILLED FOR IT", reducedFont));
            doc.Add(new Paragraph("\n"));
            //par = new Paragraph("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n", normalFont);
            //doc.Add(par);
            //cell.Border = Rectangle.NO_BORDER;
            //cell.HorizontalAlignment = Rectangle.ALIGN_CENTER;
            //doc.Add(cell);
            #endregion
            //headerEvent.OnEndPage(wr, doc); 
            doc.Close();
            return sPrintPathName;
        }

        public PdfPTable GenerateTable(IList<CreateTableQuest> ilstCreateTableQuest, int ColoumCount)
        {
            PdfPTable GeneratedTable = new PdfPTable(ColoumCount);
            PdfPCell cell;
            Phrase phr;
            Paragraph par;
            foreach (CreateTableQuest ObjCreateTableQuest in ilstCreateTableQuest)
            {
                cell = new PdfPCell();
                par = new Paragraph();
                string ValueToBePrinted = string.Empty;
                if (ObjCreateTableQuest.FieldName == string.Empty)
                {
                    if (ObjCreateTableQuest.FieldValue.StartsWith(":"))
                    {
                        phr = new Phrase(ObjCreateTableQuest.FieldValue.Substring(1), InnerHeading);
                        par = new Paragraph(phr);
                        cell.AddElement(par);
                    }
                    else if (ObjCreateTableQuest.FieldValue.StartsWith("---"))
                    {

                        Chunk linebreak = new Chunk(new LineSeparator(1f, 100f, BaseColor.BLACK, iTextSharp.text.Element.ALIGN_CENTER, 0));
                        cell.AddElement(linebreak);
                    }
                    else
                    {
                        phr = new Phrase(ObjCreateTableQuest.FieldValue, normalFont);
                        par = new Paragraph(phr);
                        cell.AddElement(par);
                    }
                }
                else
                {
                    //if (ObjCreateTableQuest.FieldValue != ":") 
                    //    phr = new Phrase(ObjCreateTableQuest.FieldName+": ", reducedFont);
                    //else
                    phr = new Phrase(ObjCreateTableQuest.FieldName + " :", reducedFont);
                    par = new Paragraph(phr);
                    if (ObjCreateTableQuest.FieldName == "Pat ID#" || ObjCreateTableQuest.FieldName == "Lab Reference ID")
                        phr = new Phrase(ObjCreateTableQuest.FieldValue, reducedFont);
                    else
                        phr = new Phrase(ObjCreateTableQuest.FieldValue, normalFont);
                    par.AddRange(phr);
                    cell.AddElement(par);
                }
                cell.Border = Rectangle.NO_BORDER;
                if (ObjCreateTableQuest.ColoumSpan > 0)
                    cell.Colspan = ObjCreateTableQuest.ColoumSpan;
                GeneratedTable.AddCell(cell);
            }
            return GeneratedTable;
        }
        //public CreateTableQuest ObjectGenerator(string FieldName, string FieldValue, int ColoumSpan, iTextSharp.text.Font FontName)
        //{
        //    CreateTableQuest ObjCreateTableQuest = new CreateTableQuest();
        //    ObjCreateTableQuest.FieldName = FieldName;
        //    ObjCreateTableQuest.FieldValue = FieldValue;
        //    ObjCreateTableQuest.ColoumSpan = ColoumSpan;
        //    ObjCreateTableQuest.FontName = FontName;
        //    return ObjCreateTableQuest;
        //}
        public class HeaderEventGenerateForQuest : PdfPageEventHelper
        {
            protected PdfTemplate total;
            protected BaseFont helv;
            private bool settingFont = false;

            string ControlNo;
            string ClientAccountNumber;
            string TypeOfSpecimen;
            iTextSharp.text.Font FontName = new iTextSharp.text.Font();
            // int totalPage;
            string humanName = string.Empty;
            string humanAddress = string.Empty;
            string humanCity = string.Empty;
            string Home_Phone_No = string.Empty;
            string specDate = string.Empty;
            string specTime = string.Empty;
            string HumanId = string.Empty;
            string BirthDate = string.Empty;
            string Sex = string.Empty;
            string Fasting = string.Empty;
            string CliAddress = string.Empty;

            public HeaderEventGenerateForQuest(string _ControlNo, string clientAccountNumber, string TypeOfSpecimentCollection, iTextSharp.text.Font EOrPSCFont, string human_Name, string human_Address, string human_City, string home_Phone_No, string spec_Date, string spec_Time, string Human_Id, string Birth_Date, string sex, string fasting, string controlNumber, string ClientAddress)
            {
                ControlNo = _ControlNo;
                ClientAccountNumber = clientAccountNumber;
                TypeOfSpecimen = TypeOfSpecimentCollection;
                FontName = EOrPSCFont;
                humanName = human_Name;
                humanAddress = human_Address;
                humanCity = human_City;
                Home_Phone_No = home_Phone_No;
                specDate = spec_Date;
                specTime = spec_Time;
                Fasting = fasting;
                ControlNo = controlNumber;
                Sex = sex;
                BirthDate = Birth_Date;
                CliAddress = ClientAddress;
                HumanId = Human_Id;
            }
            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                base.OnOpenDocument(writer, document);


                total = writer.DirectContent.CreateTemplate(100, 100);
                total.BoundingBox = new Rectangle(-20, -20, 100, 100);
                helv = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);


                //iTextSharp.text.Font redBoldFont = iTextSharp.text.FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
                //total = writer.DirectContent.CreateTemplate(100, 100);
                //total.BoundingBox = new Rectangle(-20, -20, 100, 100);

            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {

                base.OnStartPage(writer, document);
                PdfPTable table = new PdfPTable(new float[] { 40, 50, 50 });
                table.WidthPercentage = 100;
                PdfPCell cell;
                cell = new PdfPCell(new Phrase("Acurus Solutions", redBoldFont));
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                cell.Border = Rectangle.NO_BORDER;
                table.AddCell(cell);
                Barcode39 code39 = new Barcode39();
                code39.Code = (ClientAccountNumber.ToString() + "-" + ControlNo.ToString());//.PadRight(10, '0'));//.PadRight(16, '0');
                code39.BarHeight = 10;
                code39.ChecksumText = false;
                code39.GenerateChecksum = false;
                code39.StartStopText = true;
                code39.AltText = (ClientAccountNumber.ToString() + "-" + ControlNo.ToString());//.PadRight(10, '0'));//.PadRight(16, '0');
                PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.Image image39 = code39.CreateImageWithBarcode(cb, null, null);
                //image39.Border = Rectangle.NO_BORDER;
                image39.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                cell = new PdfPCell();
                cell.AddElement(image39);
                cell.Border = Rectangle.NO_BORDER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Quest Diagnostics Incorporated", redBoldFont));
                cell.Border = Rectangle.NO_BORDER;
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Client #:" + ClientAccountNumber, redBoldFont));
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell.Colspan = 2;
                cell.Border = Rectangle.NO_BORDER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(TypeOfSpecimen, FontName));
                cell.Border = Rectangle.NO_BORDER;
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                table.AddCell(cell);
                document.Add(table);

                table = new PdfPTable(new float[] { 30, 40, 30 });
                table.WidthPercentage = 100;

                //if(ilstLabSettings!=null && ilstLabSettings.Count>0)
                //{
                // string[] Address=ilstLabSettings.Where
                //cell = new PdfPCell(new Phrase("Chaparral Medical Group \nAttn: Administrator \n840 Town Center Drive \nPomona, CA 91767 ", normalFont));
                //}
                cell = new PdfPCell(new Phrase(CliAddress, normalFont));
                cell.Border = Rectangle.NO_BORDER;
                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("For Lab Use Only", normalFont));
                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                cell.Border = Rectangle.BOX;
                cell.BorderColor = BaseColor.LIGHT_GRAY;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                PdfPTable innertable = new PdfPTable(1);
                table.WidthPercentage = 100;
                cell = new PdfPCell(new Phrase("Patient Information", reducedFont));
                cell.Border = Rectangle.BOX;
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                innertable.AddCell(cell);
                PrintOrders objPrintOrders = new PrintOrders();
                //Acurus.Capella.FrontOffice.PrintOrders.CreateTableQuest ObjectGenerator = new Acurus.Capella.FrontOffice.PrintOrders.CreateTableQuest();
                IList<CreateTableQuest> ilstPatientInformation = new List<CreateTableQuest>();
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, humanName, 0, normalFont)); // ObjectGenerator(string.Empty, humanName, 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, humanAddress, 0, normalFont));//ObjectGenerator(string.Empty, humanAddress, 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, humanCity, 0, normalFont));//ObjectGenerator(string.Empty, humanCity, 0, normalFont));
                if (Home_Phone_No != "(   )    -")
                    ilstPatientInformation.Add(new CreateTableQuest(string.Empty, Home_Phone_No, 0, normalFont));//ObjectGenerator(string.Empty, Home_Phone_No, 0, normalFont));
                cell = new PdfPCell(objPrintOrders.GenerateTable(ilstPatientInformation, 1));
                cell.Border = Rectangle.BOX;


                innertable.AddCell(cell);
                cell = new PdfPCell(innertable);
                table.AddCell(cell);
                document.Add(table);
                //table 3
                table = new PdfPTable(new float[] { 50, 50 });
                table.WidthPercentage = 100;
                ilstPatientInformation = new List<CreateTableQuest>();
                ilstPatientInformation.Add(new CreateTableQuest("Collection Date", specDate, 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Time", specTime, 0, normalFont));
                //ilstPatientInformation.Add(ObjectGenerator("Volume", "", 0, normalFont));
                //ilstPatientInformation.Add(ObjectGenerator("Hours", "", 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Lab Reference ID", ControlNo, 2, reducedFont));
                ilstPatientInformation.Add(new CreateTableQuest("Fasting", Fasting, 0, normalFont));
                cell = new PdfPCell(objPrintOrders.GenerateTable(ilstPatientInformation, 3));
                cell.Border = Rectangle.BOX;
                table.AddCell(cell);
                ilstPatientInformation = new List<CreateTableQuest>();

                ilstPatientInformation.Add(new CreateTableQuest("Pat ID#", HumanId, 0, reducedFont));
                //ilstPatientInformation.Add(ObjectGenerator("SSN", SSN, 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Sex", Sex, 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("DOB", BirthDate, 2, normalFont));

                //ilstPatientInformation.Add(ObjectGenerator("Room/Loc",objOrdDTO.Exam_Room, 0, normalFont));
                //ilstPatientInformation.Add(ObjectGenerator("Result Notification", "Normal", 3, normalFont));
                cell = new PdfPCell(objPrintOrders.GenerateTable(ilstPatientInformation, 2));
                cell.Border = Rectangle.BOX;
                table.AddCell(cell);

                document.Add(table);

                //    document.Add(new Paragraph("\n"));
            }
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);

                PdfContentByte cb = writer.DirectContent;
                //cb.SetCMYKColorFill(0,0,0,35);
                cb.SaveState();
                cb.SetFontAndSize(helv, 12);
                string text = "Page " + writer.PageNumber + " of ";
                float textBase = document.Bottom - 20;
                float textSize = helv.GetWidthPoint(text, 12);
                cb.BeginText();

                if ((writer.PageNumber % 2) == 1)
                {
                    cb.SetTextMatrix(document.Right - 65, textBase);
                    cb.ShowText(text);
                    cb.EndText();
                    cb.AddTemplate(total, document.Right + textSize - 65, textBase);
                }
                else
                {
                    float adjust = helv.GetWidthPoint(text, 12);
                    cb.SetTextMatrix(document.Right - 65, textBase);
                    cb.ShowText(text);
                    cb.EndText();
                    cb.AddTemplate(total, document.Right + textSize - 65, textBase);
                }
                cb.RestoreState();


                //PdfContentByte cb = writer.DirectContent;
                //cb.SaveState();
                //string text = "Page " + writer.PageNumber + " of ";
                //float textBase = document.Bottom;
                //float textSize = 12; //helv.GetWidthPoint(text, 12);
                //cb.BeginText();
                //cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                //cb.SetColorFill(BaseColor.GRAY);
                //if ((writer.PageNumber % 2) == 1)
                //{
                //    cb.SetTextMatrix(document.Right, textBase);
                //    cb.ShowText(text);
                //    cb.EndText();
                //    cb.AddTemplate(total, document.Right + textSize, textBase);
                //}
                //else
                //{
                //    //float adjust = helv.GetWidthPoint("0", 12);
                //    cb.SetTextMatrix(document.Right - textSize , textBase);
                //    cb.ShowText(text);
                //    cb.EndText();
                //    cb.AddTemplate(total, document.Right, textBase);
                //}
                //cb.RestoreState();



                //base.OnEndPage(writer, document);
                //PdfContentByte cb = writer.DirectContent;
                //iTextSharp.text.Rectangle pageSize = document.PageSize;
                //cb.BeginText();
                //cb.SetRGBColorFill(112, 138, 144);
                //cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                //cb.SetTextMatrix(pageSize.GetRight(70), pageSize.GetBottom(40));
                //cb.ShowText("Page " + writer.PageNumber.ToString());
                //cb.EndText();
                ////base.OnEndPage(writer, document);
                ////PdfPCell cell ;
                ////cell = new PdfPCell(new Phrase("Page " + document.PageNumber + " of 2", GrayFont7));
                ////cell.Border = Rectangle.NO_BORDER;
                ////cell.HorizontalAlignment = 1;
                ////document.Add(cell);
            }
            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);

                total.BeginText();
                total.SetFontAndSize(helv, 12);
                total.SetTextMatrix(0, 0);
                int pageNumber = writer.PageNumber - 1;
                total.ShowText(Convert.ToString(pageNumber));
                total.EndText();


                //total.BeginText();
                //total.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                //total.SetTextMatrix(0, 0);
                //int pageNumber = writer.PageNumber - 1;

                // total.ShowText(""+Convert.ToString(pageNumber));
                //total.EndText();
            }


        }
        #endregion
        public string GenerateHumanName(string FirstName, string LastName, string MiddleName)
        {
            string humanName = LastName + "," + FirstName + " " + MiddleName;
            return humanName;
        }
        public string GenerateStreetAddress(string Street_Address1, string Street_Address2)
        {
            string humanAddress = string.Empty;
            if (Street_Address1 != string.Empty && Street_Address2 != string.Empty)
            {
                humanAddress = Street_Address1 + "\n" + Street_Address2;
            }
            else
                humanAddress = Street_Address1;
            return humanAddress;

        }
        public string GenerateCityAndZip(string City, string State, string ZipCode)
        {
            string humanCity = string.Empty;
            if (City != string.Empty && State != string.Empty)
            {
                humanCity = City + ", " + State + " " + ZipCode;
            }
            return humanCity;
        }
        public string GenerateSex(string Sex)
        {
            string sex_char = string.Empty;
            if (Sex == "MALE")
            {
                sex_char = "M";
            }
            else
            {
                sex_char = "F";
            }
            return sex_char;
        }
        public string GenerateNPI(string NPI, string FirstName, string LastName, string MiddleName)
        {
            string FormatedString = string.Empty;
            FormatedString = NPI.ToString() + LastName + "," + FirstName + "" + MiddleName;
            return FormatedString;
        }
        #endregion

        public bool GenerateABN(string AccountNumberForQuest, string ReceivingFacility, string HumanID, string HumanName, string LCACode, string PlanName, string InsAddress, string controlReqNumber, IList<OrdersAssessment> OrdersAssessmentList, string orderCode, string USERNAME, string PASSWORD, string City, string State, string zipcode, bool IsABNshow, string OrderURL, string FilePath, out string returnFilePath)
        {
            #region HL7 Creator And ABN Check
            System.DateTime now = System.DateTime.Now;
            HumanName = HumanName.TrimEnd(new char[1] { ',' });
            string MSH = "MSH|^~\\&|ACUR|{0}||{1}|{2:yyyyMMddHHmm}||ORM^O01|{3}|P|2.3\r";
            string PID = "PID|1|{0}|||{1}|||||||||||||||\r";
            string IN1 = "IN1|1||{0}|{1}|{2}^^{3}^{4}^{5}||||||||||||||||||||||||||||||||||||||||||T|\r";
            string ORCAndOBR = "ORC|NW|{0}|||||||||||||\rOBR|1|{0}||^^^{1}^{2}||||||||||||||\r";
            string DG1 = "DG1|1||{0}^{1}^I9|\r";
            string Code = string.Empty;
            string Des = string.Empty;
            object[] MSHObj = new object[4] { AccountNumberForQuest, ReceivingFacility, now, now.Ticks };
            //object[] PIDObj = new object[2] { objHuman.Human_ID.ToString(), humanName.Replace(',', '^') };
            object[] PIDObj = new object[2] { HumanID, HumanName.Replace(',', '^') };
            //object[] IN1Obj = new object[3] { objlabCarrierPri.LCA_Carrier_Code, objPriPlan.Ins_Plan_Name, objPriPlan.Payer_Addrress1 };
            object[] IN1Obj = new object[6] { LCACode, PlanName, InsAddress, City, State, zipcode };
            object[] ORCAndOBRObj = new object[3] { controlReqNumber, string.Empty, Des };
            object[] DG1Obj = new object[2];
            string tempSegemntHolder = string.Empty;
            string ICDBuilder = string.Empty;
            IList<string> ICDs = new List<string>();
            foreach (OrdersAssessment objOrdersAssessment in OrdersAssessmentList)
            {
                if (!ICDs.Contains(objOrdersAssessment.ICD))
                {
                    ICDs.Add(objOrdersAssessment.ICD);
                    DG1Obj[0] = objOrdersAssessment.ICD;
                    DG1Obj[1] = objOrdersAssessment.ICD_Description;
                    tempSegemntHolder = string.Format(DG1, DG1Obj);
                    //ICDBuilder.Append(tempSegemntHolder + "\r");
                    ICDBuilder += tempSegemntHolder;
                }
            }
            string OrdMsg = string.Empty;
            tempSegemntHolder = string.Format(MSH, MSHObj);
            //OrdMsg.Append(tempSegemntHolder);
            OrdMsg += tempSegemntHolder;
            //OrdMsg.Append("\r");
            tempSegemntHolder = string.Format(PID, PIDObj);
            //OrdMsg.Append(tempSegemntHolder);
            OrdMsg += tempSegemntHolder;
            //OrdMsg.Append("\r");
            tempSegemntHolder = string.Format(IN1, IN1Obj);
            //OrdMsg.Append(tempSegemntHolder);
            OrdMsg += tempSegemntHolder;
            // OrdMsg.Append("\r");
            foreach (string str in orderCode.Split('\n'))
            {
                if (str != string.Empty)
                {
                    string[] CodeBisect = str.Split('\t');
                    if (CodeBisect.Count() > 0)
                    {
                        ORCAndOBRObj[1] = CodeBisect[0];
                        ORCAndOBRObj[2] = CodeBisect[1];

                    }
                    tempSegemntHolder = string.Format(ORCAndOBR, ORCAndOBRObj);
                    //OrdMsg.Append(tempSegemntHolder);
                    OrdMsg += tempSegemntHolder;
                    //OrdMsg.Append("\r");
                    OrdMsg += ICDBuilder;
                    //OrdMsg.Append(ICDBuilder.ToString());
                }
            }
            string openFilename = string.Empty;
            try
            {
                Acurus.Capella.DataAccess.QuestWebServices.OrderService webService = new Acurus.Capella.DataAccess.QuestWebServices.OrderService();
                webService.Url = OrderURL;
                webService.Credentials = new NetworkCredential(USERNAME, PASSWORD);
                webService.PreAuthenticate = true;

                OrderSupportServiceRequest retval = new OrderSupportServiceRequest();
                retval.hl7Order = System.Text.Encoding.ASCII.GetBytes(OrdMsg.ToString());
                OrderSupportServiceRequest supportRequest = retval;
                supportRequest.orderSupportRequests = new string[] { "ABN" };
                OrderSupportServiceResponse supportResponse = new OrderSupportServiceResponse();
                supportResponse = webService.getOrderDocuments(supportRequest);
                OrderSupportDocument[] objOrderSupportDocument = supportResponse.orderSupportDocuments;
                if (supportResponse.status == "SUCCESS")
                {
                    object[] obj = new object[2];

                    if (objOrderSupportDocument[0].documentData != null && IsABNshow)
                    {
                        //obj[0] = objOrderSupportDocument[0].documentType;
                        byte[] barry = objOrderSupportDocument[0].documentData;
                        //obj[1] = ;

                        //File.WriteAllBytes(openFilename = FilePath + "\\ABN_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf", Convert.FromBase64String(((object)(Convert.ToBase64String(barry))).ToString()));
                        File.WriteAllBytes(openFilename = FilePath + "\\ABN_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf", Encoding.ASCII.GetBytes(((object)(Convert.ToBase64String(barry))).ToString()));//BugID:53715
                        returnFilePath = openFilename;
                        //ConvertBase64(TempFile, objPDF[i].ToString());
                        //System.Diagnostics.Process.Start(openFilename);
                        return true;
                    }
                    else if (objOrderSupportDocument[0].documentData != null)
                    {
                        returnFilePath = openFilename;
                        return true;
                    }
                    else
                    {
                        returnFilePath = string.Empty;
                        return false;
                    }
                }
                else
                {
                    returnFilePath = string.Empty;
                    return false;
                    //return new object[] { objOrderSupportDocument != null ? objOrderSupportDocument[0].requestStatus : null };
                }
            }
            catch
            {
                returnFilePath = string.Empty;
                return false;
            }

            #endregion
        }

        public string PrintSplitRequisitionUsingDatafromDBQuest(ulong Order_SubmitID, string appConfigKey, String CallingFrom, bool ABN, string OrderType)
        {
            iTextSharp.text.Font QuestEOrPSC = new iTextSharp.text.Font();
            LabcorpSettingsManager objLabcorpSettingsManager = new LabcorpSettingsManager();
            string Interface_identifier = string.Empty;
            string AccountNumberForQuest = string.Empty;
            ulong HumanId = 0;
            ulong PhysicianID = 0;
            OrdersManager objOrdersManager = new OrdersManager();
            //SpecimenProxy objSpecimenproxy = new SpecimenProxy();
            string clientaddress = string.Empty;
            string UserName = string.Empty;
            string Password = string.Empty;
            string ReceivingFacility = string.Empty;
            string OrderURL = string.Empty;
            OrderSpecimenDTO objOrdDTO = objOrdersManager.LoadSpecimen(Order_SubmitID, OrderType);
            IList<LabSettings> ilstLabSettingsLibrary = objLabcorpSettingsManager.GetLabcorpSettings();
            IList<LabSettings> ilstLabcorpSettings = (from lst in ilstLabSettingsLibrary where lst.Lab_ID == objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Lab_ID select lst).ToList<LabSettings>();
            if (ilstLabcorpSettings != null && ilstLabcorpSettings.Count > 0)
            {
                clientaddress = ilstLabcorpSettings[0].Client_Information;
                ReceivingFacility = ilstLabcorpSettings[0].Receiving_Facility;
                UserName = ilstLabcorpSettings[0].User_Name;
                Password = ilstLabcorpSettings[0].Password;
                OrderURL = ilstLabcorpSettings[0].URL;
            }

            if (objOrdDTO == null || (objOrdDTO != null && objOrdDTO.ilstOrderLabDetailsDTO.Count == 0) || objOrdDTO.objHuman == null)
                return string.Empty;
            //IList<Specimen> specList = objOrdDTO.ilstOrderLabDetailsDTO.Select(a=>a.objSpecimen).ToList<Specimen>();

            string specDate = string.Empty;
            string specTime = string.Empty;
            string humanName = string.Empty;
            string humanAddress = string.Empty;
            string humanCity = string.Empty;
            string SSN = string.Empty;
            string orderCode = string.Empty;
            string testOrdered = string.Empty;
            string testcount = string.Empty;
            string totVol = string.Empty;
            string collDate = string.Empty;
            Orders objOrder = new Orders();
            OrdersSubmit objOrdersSubmit = new OrdersSubmit();

            string controlReqNumber = "ACUR" + Order_SubmitID.ToString();


            string Sex = string.Empty;
            string BirthDate = string.Empty;
            string RequestionNo = string.Empty;
            string PSC_Or_E = string.Empty;

            testcount = objOrdDTO.ilstOrderLabDetailsDTO.Count.ToString();
            string tempState = string.Empty;
            Dictionary<string, IList<OrdersQuestionSetAOE>> AOEAns = new Dictionary<string, IList<OrdersQuestionSetAOE>>();
            Dictionary<string, IList<OrderComponents>> SubComp = new Dictionary<string, IList<OrderComponents>>();
            string notes = string.Empty;
            if (objOrdDTO.ilstOrderLabDetailsDTO.Any(a => !a.OrdersSubmit.Specimen_Collection_Date_And_Time.ToString().StartsWith("01-01-0001")))
            {
                DateTime tSpecimen_Collection_Date_And_Time = objOrdDTO.ilstOrderLabDetailsDTO.Select(a => a.OrdersSubmit.Specimen_Collection_Date_And_Time).ToList<DateTime>()[0];
                specDate = UtilityManager.ConvertToLocal(tSpecimen_Collection_Date_And_Time).ToString("MM-dd-yyyy");
                specTime = UtilityManager.ConvertToLocal(tSpecimen_Collection_Date_And_Time).ToString("hh:mm tt"); // [0].Specimen_Collection_Date_And_Time.ToString("hh:mm tt");
            }
            string Fasting = string.Empty;
            if (objOrdDTO.ilstOrderLabDetailsDTO.Count > 0)
            {
                OrderComponentsManager objOrderComponentsManager = new OrderComponentsManager();
                totVol = objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Quantity.ToString();
                Fasting = objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit.Fasting.ToString();
                IList<OrderComponents> ilstOrderComponents = new List<OrderComponents>();
                ilstOrderComponents = objOrderComponentsManager.GetAll();
                //objSpecimen = objOrdDTO.ilstOrderLabDetailsDTO[0].objSpecimen;
                objOrdersSubmit = objOrdDTO.ilstOrderLabDetailsDTO[0].OrdersSubmit;
                objOrder = objOrdDTO.ilstOrderLabDetailsDTO[0].ObjOrder;
                collDate = UtilityManager.ConvertToLocal(objOrdDTO.ilstOrderLabDetailsDTO[0].ObjOrder.Created_Date_And_Time).ToString("MM/dd/yyyy hh:mm tt");
                if (specDate.Trim() == DateTime.MinValue.ToString("MM-dd-yyyy") && specTime.Trim() == DateTime.MinValue.ToString("hh:mm tt"))
                {
                    specDate = string.Empty;
                    specTime = string.Empty;
                }
                if (specDate.Trim() == string.Empty && specTime.Trim() == string.Empty)
                {
                    specDate = UtilityManager.ConvertToLocal(objOrdDTO.ilstOrderLabDetailsDTO[0].ObjOrder.Created_Date_And_Time).ToString("MM-dd-yyyy");
                    specTime = UtilityManager.ConvertToLocal(objOrdDTO.ilstOrderLabDetailsDTO[0].ObjOrder.Created_Date_And_Time).ToString("hh:mm tt");
                }
                for (int i = 0; i < objOrdDTO.ilstOrderLabDetailsDTO.Count; i++)
                {
                    Orders obj = objOrdDTO.ilstOrderLabDetailsDTO[i].ObjOrder;
                    OrdersSubmit objSub = objOrdDTO.ilstOrderLabDetailsDTO[i].OrdersSubmit;
                    PhysicianID = obj.Physician_ID;
                    HumanId = obj.Human_ID;
                    //if (i == 0)
                    //{
                    //    testOrdered = obj.Lab_Procedure_Description;
                    //    orderCode = obj.Order_Code_Type + "  " + obj.Lab_Procedure;
                    //}
                    //else
                    //{

                    testOrdered += "\n" + obj.Lab_Procedure_Description;
                    AOEAns.Add(obj.Lab_Procedure + "\t" + obj.Lab_Procedure_Description + "|" + obj.Id.ToString(), objOrdDTO.ilstOrderLabDetailsDTO[i].OrderAOEList);
                    orderCode += obj.Lab_Procedure + "\t" + obj.Lab_Procedure_Description + "|" + obj.Id.ToString() + "\n";
                    SubComp.Add(obj.Lab_Procedure + "\t" + obj.Lab_Procedure_Description + "|" + obj.Id.ToString(), (from rec in ilstOrderComponents where rec.Order_Code == obj.Lab_Procedure select rec).ToList<OrderComponents>());
                    tempState = objSub.Temperature;
                    notes += " " + objSub.Order_Notes;
                    //}
                }
                //orderCode += "\n";
                RequestionNo = objOrder.Id.ToString();


                if (objOrdersSubmit.Specimen_In_House == "Y")
                {
                    PSC_Or_E = "e";
                    QuestEOrPSC = QuestBigE;
                }
                else
                {
                    PSC_Or_E = "PSC HOLD ORDER";
                    QuestEOrPSC = QuestHeading;
                }
            }
            PhysicianManager objPhysicianManager = new PhysicianManager();
            PhysicianLibrary objPhy = objPhysicianManager.GetphysiciannameByPhyID(objOrdDTO.ilstOrderLabDetailsDTO.Select(a => a.ObjOrder.Physician_ID).ToList<ulong>()[0])[0];
            FillHumanDTO objHuman = objOrdDTO.objHuman;
            //objPhy = AllLibraries.Instance.GetphysiciannameByPhyID(PhysicianID, false);
            //objHuman = EncounterManager.Instance.GetHumanByHumanID(HumanId);
            humanName = GenerateHumanName(objHuman.First_Name, objHuman.Last_Name, objHuman.MI);
            humanAddress = GenerateStreetAddress(objHuman.Street_Address1, objHuman.Street_Address2);
            humanCity = GenerateCityAndZip(objHuman.City, objHuman.State, objHuman.ZipCode);
            SSN = objHuman.SSN;
            Sex = GenerateSex(objHuman.Sex);
            BirthDate = objHuman.Birth_Date.ToString("dd-MMM-yyyy");
            InsurancePlan objPriPlan = new InsurancePlan();
            InsurancePlan objSecPlan = new InsurancePlan();
            PatientInsuredPlan objPatInsPri = new PatientInsuredPlan();
            PatientInsuredPlan objPatInsSec = new PatientInsuredPlan();
            LabCarrierLookUp objlabCarrierPri = new LabCarrierLookUp();
            LabCarrierLookUp objLabCarrierSec = new LabCarrierLookUp();
            Hashtable InsuredIds = new Hashtable();
            InsurancePlanManager objInsurancePlanManager = new InsurancePlanManager();
            LabCarrierLookUpManager objLabCarrierLookUpManager = new LabCarrierLookUpManager();
            if (objHuman.PatientInsuredBag.Count > 0)
            {
                foreach (PatientInsuredPlan obj in objHuman.PatientInsuredBag)
                {
                    if (obj.Insurance_Type.ToUpper() == "PRIMARY")
                    {
                        objPatInsPri = obj;
                        objPriPlan = objInsurancePlanManager.GetInsurancebyID(obj.Insurance_Plan_ID)[0];
                        objlabCarrierPri = objLabCarrierLookUpManager.GetLabCarrierDetailsForInsPlanID(obj.Insurance_Plan_ID, objOrdersSubmit.Lab_ID);
                        if (!InsuredIds.ContainsKey("Primary"))
                            InsuredIds.Add("Primary", obj.Insured_Human_ID);
                    }
                    else if (obj.Insurance_Type.ToUpper() == "SECONDARY")
                    {
                        objPatInsSec = obj;
                        objSecPlan = objInsurancePlanManager.GetInsurancebyID(obj.Insurance_Plan_ID)[0];
                        objLabCarrierSec = objLabCarrierLookUpManager.GetLabCarrierDetailsForInsPlanID(obj.Insurance_Plan_ID, objOrdersSubmit.Lab_ID);
                        if (!InsuredIds.ContainsKey("Secondary"))
                            InsuredIds.Add("Secondary", obj.Insured_Human_ID);
                    }

                }
            }
            FillHumanDTO ObjHumanPrimary = null;
            FillHumanDTO ObjHumanSecondary = null;
            if (objOrdersSubmit.Bill_Type == "Third Party")
            {
                foreach (DictionaryEntry dct in InsuredIds)
                {
                    if (dct.Key.ToString() == "Primary")
                    {
                        ObjHumanPrimary = new FillHumanDTO();
                        if (Convert.ToUInt32(dct.Value) == objHuman.Human_ID)
                        {
                            ObjHumanPrimary = objHuman;
                        }
                        else
                        {
                            ObjHumanPrimary = (from temphumanobj in objOrdDTO.ilstGuarantor where temphumanobj.Human_ID == Convert.ToUInt32(dct.Value) select temphumanobj).ToList<FillHumanDTO>()[0];
                        }
                    }
                    else if (dct.Key.ToString() == "Secondary")
                    {
                        ObjHumanSecondary = new FillHumanDTO();
                        if (Convert.ToUInt32(dct.Value) == objHuman.Human_ID)
                        {
                            ObjHumanSecondary = objHuman;
                        }
                        else if (Convert.ToUInt32(dct.Value) == 0)
                        {
                            ObjHumanSecondary = null;
                        }
                        else
                        {
                            ObjHumanSecondary = (from temphumanobj in objOrdDTO.ilstGuarantor where temphumanobj.Human_ID == Convert.ToUInt32(dct.Value) select temphumanobj).ToList<FillHumanDTO>()[0];
                        }
                    }

                }
            }
            FacilityManager objFacilityManager = new FacilityManager();
            IList<FacilityLibrary> ilstFacility = objFacilityManager.GetFacilityByFacilityname(ClientSession.FacilityName);
            if (ilstFacility.Count > 0)
            {
                AccountNumberForQuest = ilstFacility[0].Quest_Account_Number;
            }
            string ICdCodes = string.Empty;
            for (int i = 0; i < objOrdDTO.OrderAssList.Count; i++)
            {
                if (ICdCodes.Contains(objOrdDTO.OrderAssList[i].ICD) == false)
                {
                    if (ICdCodes == string.Empty)
                    {
                        ICdCodes = objOrdDTO.OrderAssList[i].ICD;
                    }
                    else
                    {
                        ICdCodes += ", " + objOrdDTO.OrderAssList[i].ICD;
                    }
                }
            }
            string FilePath = string.Empty;
            string ReturnFilePath = string.Empty;
            if (objOrdersSubmit.Bill_Type == "Third Party")
            {

                ABN = GenerateABN(AccountNumberForQuest, ReceivingFacility, objHuman.Human_ID.ToString(), humanName, objlabCarrierPri.LCA_Carrier_Code, objPriPlan.Ins_Plan_Name, objPriPlan.Payer_Addrress1, controlReqNumber, objOrdDTO.OrderAssList, orderCode, UserName, Password, objPriPlan.Payer_City, objPriPlan.Payer_State, objPriPlan.Payer_Zip, false, OrderURL, FilePath, out ReturnFilePath);
            }

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
            string sPrintPathName = string.Empty;
            string folderPath = appConfigKey;
            //string folderPath = @"E:\Altova LabCorp Output";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            sPrintPathName = folderPath + "\\Order_Requisition_" + controlReqNumber + ".pdf";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            iTextSharp.text.Rectangle pageSize = doc.PageSize;
            IList<FacilityLibrary> tempFacilityLibrary = objFacilityManager.GetFacilityByFacilityname(objOrdersSubmit.Facility_Name);
            if (tempFacilityLibrary.Count > 0)
            {
                clientaddress += "\n" + tempFacilityLibrary[0].Fac_Address1 + "\n" + tempFacilityLibrary[0].Fac_City + "," + tempFacilityLibrary[0].Fac_State + "," + tempFacilityLibrary[0].Fac_Zip + "\n" + tempFacilityLibrary[0].Fac_Telephone;
            }
            doc.Open();

            HeaderEventGenerateForQuest headerEvent = new HeaderEventGenerateForQuest(controlReqNumber, AccountNumberForQuest, PSC_Or_E, QuestEOrPSC, humanName, humanAddress, humanCity, objHuman.Home_Phone_No, specDate, specTime, objHuman.Human_ID.ToString(), BirthDate, Sex, Fasting, controlReqNumber, clientaddress);
            wr.PageEvent = headerEvent;
            headerEvent.OnOpenDocument(wr, doc);
            headerEvent.OnStartPage(wr, doc);
            headerEvent.OnEndPage(wr, doc);
            #region HeaderTable
            IList<CreateTableQuest> ilstPatientInformation = new List<CreateTableQuest>();

            PdfPTable table;
            PdfPCell cell;
            //PdfPTable innertable;
            table = new PdfPTable(2);
            table.WidthPercentage = 100;
            ilstPatientInformation = new List<CreateTableQuest>();
            ilstPatientInformation.Add(new CreateTableQuest("Ordering Physician", objPhy.PhyLastName + ", " + objPhy.PhyFirstName + (objPhy.PhyFirstName + objPhy.PhyMiddleName.Trim() != string.Empty ? ", " + objPhy.PhyMiddleName.Trim() : ""), 2, normalFont));
            ilstPatientInformation.Add(new CreateTableQuest("Physician NPI", objPhy.PhyNPI, 0, normalFont));
            ilstPatientInformation.Add(new CreateTableQuest("Internal Comments", notes.Trim(), 0, normalFont));
            ilstPatientInformation.Add(new CreateTableQuest("Report Comments", "", 0, normalFont));
            ilstPatientInformation.Add(new CreateTableQuest(string.Empty, "", 2, normalFont));
            if (objOrdersSubmit.Bill_Type == "Third Party")
            {
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, "---", 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Bill Type", objOrdersSubmit.Bill_Type, 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, ":Guarantor", 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Name", GenerateHumanName(objHuman.Guarantor_First_Name, objHuman.Guarantor_Last_Name, objHuman.Guarantor_MI), 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Address", "", 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, GenerateStreetAddress(objHuman.Guarantor_Street_Address1, objHuman.Guarantor_Street_Address2), 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, GenerateCityAndZip(objHuman.Guarantor_City, objHuman.Guarantor_State, objHuman.Guarantor_Zip_Code), 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Relationship", objPatInsPri.Relationship, 0, normalFont));
            }
            else if (objOrdersSubmit.Bill_Type == "Client")
            {
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, "---", 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Bill Type", objOrdersSubmit.Bill_Type, 2, normalFont));
            }
            else if (objOrdersSubmit.Bill_Type == "Patient")
            {
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, "---", 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Bill Type", objOrdersSubmit.Bill_Type, 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, ":Guarantor", 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Name", GenerateHumanName(objHuman.Guarantor_First_Name, objHuman.Guarantor_Last_Name, objHuman.Guarantor_MI), 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Address", "", 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, GenerateStreetAddress(objHuman.Guarantor_Street_Address1, objHuman.Guarantor_Street_Address2), 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, GenerateCityAndZip(objHuman.Guarantor_City, objHuman.Guarantor_State, objHuman.Guarantor_Zip_Code), 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Relationship", objHuman.Guarantor_Relationship, 2, normalFont));
            }
            cell = new PdfPCell(GenerateTable(ilstPatientInformation, 1));
            cell.Border = Rectangle.BOX;
            table.AddCell(cell);
            ilstPatientInformation = new List<CreateTableQuest>();
            if (objOrdersSubmit.Bill_Type == "Third Party")
            {
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, ":Primary Insurance", 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Quest Bill Code", objlabCarrierPri.LCA_Carrier_Code, 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Primary Ins Co Name", objPriPlan.Ins_Plan_Name, 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Primary Ins Co Address", "", 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, objPriPlan.Payer_Addrress1, 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest(string.Empty, GenerateCityAndZip(objPriPlan.Payer_City, objPriPlan.Payer_State, objPriPlan.Payer_Zip), 2, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Policy #", objPatInsPri.Policy_Holder_ID, 0, normalFont));
                ilstPatientInformation.Add(new CreateTableQuest("Group #", objPatInsPri.Group_Number, 0, normalFont));
                if (ObjHumanSecondary != null)
                {
                    ilstPatientInformation.Add(new CreateTableQuest(string.Empty, "", 2, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest(string.Empty, ":Secondary Insurance", 2, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest("Quest Bill Code", objLabCarrierSec.LCA_Carrier_Code, 2, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest("Secondary Ins Co Name", objSecPlan.Ins_Plan_Name, 2, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest("Secondary Ins Co Address", "", 2, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest(string.Empty, objSecPlan.Payer_Addrress1, 2, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest(string.Empty, GenerateCityAndZip(objSecPlan.Payer_City, objSecPlan.Payer_State, objSecPlan.Payer_Zip), 2, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest("Policy #", objPatInsSec.Policy_Holder_ID, 0, normalFont));
                    ilstPatientInformation.Add(new CreateTableQuest("Group #", objPatInsSec.Group_Number, 0, normalFont));
                }

            }
            cell = new PdfPCell(GenerateTable(ilstPatientInformation, 2));
            cell.Border = Rectangle.BOX;
            table.AddCell(cell);
            doc.Add(table);
            //table-4
            table = new PdfPTable(1);
            table.WidthPercentage = 100;
            ilstPatientInformation = new List<CreateTableQuest>();
            ilstPatientInformation.Add(new CreateTableQuest("ICD Diagnosis Code(s)", objOrdDTO.ICDs.Replace(".", string.Empty), 0, normalFont));
            cell = new PdfPCell(GenerateTable(ilstPatientInformation, 1));
            table.AddCell(cell);
            doc.Add(table);
            doc.Add(new Phrase("\n"));
            table = new PdfPTable(1);
            table.WidthPercentage = 100;
            cell = new PdfPCell(new Phrase("Profiles/Tests", WhiteFont));
            cell.MinimumHeight = 10;
            cell.HorizontalAlignment = Rectangle.ALIGN_CENTER;
            cell.BackgroundColor = BaseColor.BLACK;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            doc.Add(table);

            String[] OrderCodesTobePrinted = orderCode.Split('\n');
            Paragraph par = new Paragraph();
            foreach (string str in OrderCodesTobePrinted)
            {

                if (str != string.Empty)
                {
                    //table = new PdfPTable(new float[] { 10, 90 });
                    //table.WidthPercentage = 50;
                    //cell = new PdfPCell(new Paragraph(((str.Split('|')[0]).Split('\t')[0]),normalFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //cell.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    //table.AddCell(cell);
                    //cell = new PdfPCell(new Paragraph(((str.Split('|')[0]).Split('\t')[1]),normalFont));
                    //cell.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                    //cell.Border = Rectangle.NO_BORDER;
                    //table.AddCell(cell);
                    par = new Paragraph(new Phrase(str.Split('|')[0], normalFont));
                    par.SetAlignment("Left");
                    doc.Add(par);
                    IList<OrderComponents> tempOrderComponents = SubComp[str];

                    foreach (OrderComponents obj in tempOrderComponents)
                    {
                        par = new Paragraph(new Phrase(obj.Order_Code_Name + "- Specimen Transport :" + obj.Temperature, normalFont));
                        par.SetAlignment("Left");
                        par.IndentationLeft = 10f;
                        doc.Add(par);
                    }


                    IList<OrdersQuestionSetAOE> AOEForThisCode = AOEAns[str];


                    foreach (OrdersQuestionSetAOE obj in AOEForThisCode)
                    {
                        //cell = new PdfPCell();
                        //cell.Border = Rectangle.NO_BORDER;
                        //table.AddCell(cell);
                        par = new Paragraph(new Phrase(obj.AOE_Question + ":" + obj.AOE_Value, normalFont));
                        par.SetAlignment("Left");
                        par.IndentationLeft = 10f;
                        doc.Add(par);
                        //cell = new PdfPCell(par);
                        //cell.Indent = 10f;
                        //cell.Border = Rectangle.NO_BORDER;
                        //table.AddCell(cell);
                        //doc.Add(par);
                    }

                }
            }
            if (ABN)
            {
                par = new Paragraph(new Phrase("21799\tABN", normalFont));
                par.SetAlignment("Left");
                doc.Add(par);
            }

            //orderCode += "227-ALDOLASE\n2302-BERMUDA GRASS (G2) IGE\n23029-IRON, TOTAL (REFL)\n23086-ACANTHAMOEBA/NAEGLERIA CULTURE\n227-ALDOLASE\n2310-JOHNSON GRASS (G10) IGE\n23230-MESORIDAZINE\n23475-GLUCOSE TOLERANCE TEST, 3 SPECIMENS, (75G)\n23497-DRUG ABUSE PANEL 6-50\n23860-MANGO FRUIT (F91) IGE\n2409-ENGLISH PLANTAIN (W9) IGE\n24989-CHLORAL HYDRATE, BLOOD\n2522-HICKORY/PECAN TREE (T22) IGE\n2556-QUEEN PALM (T72) IGE\n2569-BRUSSEL SPROUTS (RF217) IGE\n2610-CHILI PEPPER (F279) IGE**\n2633-ALLERGEN SPECIFIC IGE CARAWAY SEED*\n26509-PHENOTHIAZINES PANEL (U)\n26517-GLYBURIDE (MICRONASE)\n26524-CBC (INCLUDES DIFF/PLT) WITH PATHOLOGIST REVIEW\n2654-RABBIT EPITHELIA (E82) IGE\n2657-MOUSE EPITHELIA (E71) IGE\n2661-GOOSE FEATHERS (E70) IGE";
            //Paragraph par = new Paragraph(new Phrase(orderCode, normalFont));
            //par.SetAlignment("Left");
            //doc.Add(par);
            //cell = new PdfPCell(new Phrase("End of Requestion :" + controlReqNumber, normalFont7));
            if (tempState != string.Empty)
            {
                par = new Paragraph("Temperature : " + tempState, normalFont);
                par.SetAlignment("Center");
                doc.Add(par);
            }
            par = new Paragraph("End of Requisition :" + controlReqNumber, normalFont);
            par.SetAlignment("Center");
            doc.Add(par);
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("SOME INSURANCES OR MEDICARE MAY NOT PAY FOR VITAMIN D LEVEL, PLEASE VERIFY WITH THE LAB AS YOU MAY GET BILLED FOR IT", reducedFont));
            doc.Add(new Paragraph("\n"));
            #endregion
            doc.Close();
            return sPrintPathName;
            //System.Diagnostics.Process.Start(sPrintPathName);
        }
        //public string PrintInReferralOrder(IList<ReferralOrder> objList, ulong refFromPhyID, ReferralOrderDTO objRefOrdDTO, Human humanRecord, string Specialty, string TargetDirectoryPath)
        public string PrintInReferralOrder(IList<ReferralOrder> objList, ulong refFromPhyID, ReferralOrderDTO objRefOrdDTO, FillHumanDTO humanRecord, string Specialty, string TargetDirectoryPath)
        {
            if (objList != null)
            {
                var SignatureDate = (from os in objRefOrdDTO.RefOrdList select os.Modified_Date_And_Time.ToString("dd-MM-yyyy hh:mm:ss") != "01-01-0001 12:00:00" ? os.Modified_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt") : os.Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt")).OrderByDescending(a => a).ToArray();

                FacilityManager FacilityList = new FacilityManager();
                facilityList = FacilityList.GetFacilityList();
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
                PhysicianManager objPhysicianManager = new PhysicianManager();
                PhysicianLibrary objRefFromPhy = objPhysicianManager.GetphysiciannameByPhyID(refFromPhyID)[0];
                //PhysicianLibrary objRefToPhy = AllLibraries.Instance.GetphysiciannameByPhyID(objList[0].To_Physician_ID, false);
                string sPrintPathName = string.Empty;
                //string folderPath = System.Configuration.ConfigurationSettings.AppSettings["ReferralPrintPath"] + "\\" + DateTime.Now.ToString("yyyyMMdd");
                string folderPath = TargetDirectoryPath + System.Configuration.ConfigurationSettings.AppSettings["ReferralPrintPath"];
                Directory.CreateDirectory(folderPath);
                if (objRefFromPhy != null)
                {
                    {
                        sPrintPathName = folderPath + "\\" + humanRecord.Human_ID.ToString() + "_" + humanRecord.Last_Name + "_" + humanRecord.First_Name
                            + "_" + objRefFromPhy.PhyPrefix + " " + objRefFromPhy.PhyFirstName +
                            " " + objRefFromPhy.PhyLastName + " " + objRefFromPhy.PhySuffix + "_ReferralOrder_"
                            + DateTime.Now.ToString("yyyyMMdd hhmmss tt") + objList[0].Id.ToString() + ".pdf";
                        PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                        iTextSharp.text.Rectangle pageSize = doc.PageSize;
                        HeaderEventGenerate headerEvent = new HeaderEventGenerate();
                        doc.Open();
                        wr.PageEvent = headerEvent;
                        headerEvent.OnStartPage(wr, doc);
                        headerEvent.OnEndPage(wr, doc);
                        Paragraph par = null;
                        par = new Paragraph(objRefFromPhy.PhyPrefix + " " + objRefFromPhy.PhyFirstName +
                            " " + objRefFromPhy.PhyLastName + " " + objRefFromPhy.PhySuffix, normalFont);
                        par.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                        doc.Add(par);
                        FacilityLibrary objRefFromFac = (from obj in facilityList where obj.Fac_Name == ClientSession.FacilityName select obj).ToList<FacilityLibrary>()[0];
                        //string facilityText = ClientSession.sFacilityName + "\n" + objRefFromFac.Fac_Address1 + "\n" + objRefFromFac.Fac_City + "," + objRefFromFac.Fac_State + " " + objRefFromFac.Fac_Zip + "\n Phone #:" + objRefFromFac.Fac_Telephone + "  Fax:" + objRefFromFac.Fac_Fax;
                        string facilityText;
                        if (objRefFromFac.Fac_Address1.EndsWith(".,"))
                        {
                            facilityText = objRefFromFac.Fac_Address1.Remove(objRefFromFac.Fac_Address1.Length - 2).ToString() + "\n" + objRefFromFac.Fac_City + "," + objRefFromFac.Fac_State + " " + objRefFromFac.Fac_Zip + "\n Phone #:" + objRefFromFac.Fac_Telephone + "  Fax:" + objRefFromFac.Fac_Fax;
                        }
                        else
                        {
                            facilityText = objRefFromFac.Fac_Address1 + "\n" + objRefFromFac.Fac_City + "," + objRefFromFac.Fac_State + " " + objRefFromFac.Fac_Zip + "\n Phone #:" + objRefFromFac.Fac_Telephone + "  Fax:" + objRefFromFac.Fac_Fax;
                        }

                        par = new Paragraph(facilityText, normalFont);
                        par.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                        doc.Add(par);
                        par = new Paragraph("Referral Date: " + DateTime.Now.ToString("dd-MMM-yyyy"), normalFont);
                        par.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        doc.Add(par);
                        //doc.Add(new Paragraph("\n"));
                        //PdfPTable patTable = new PdfPTable(new float[] { 40, 20, 20, 20 });
                        //patTable.WidthPercentage = 100;
                        //PdfPCell cell = new PdfPCell(new Phrase("Patient Details", reducedFont));
                        //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        //cell.Colspan = 4;
                        //patTable.AddCell(cell);
                        //cell = CreateCell("Patient Name: \n", humanRecord.Last_Name + "," + humanRecord.First_Name);//(new Phrase("Patient Name: \n" + humanRecord.Last_Name + "," + humanRecord.First_Name, normalFont));
                        //patTable.AddCell(cell);
                        //cell = CreateCell("Patient ID: \n", humanRecord.Human_ID.ToString());
                        //patTable.AddCell(cell);
                        //cell = CreateCell("Patient DOB: \n", humanRecord.Birth_Date.ToString("dd-MMM-yyyy"));
                        //patTable.AddCell(cell);
                        //cell = CreateCell("Patient Sex: \n", humanRecord.Sex);
                        //patTable.AddCell(cell);
                        //doc.Add(patTable);

                        //string sPhoneNumbers = (humanRecord.Home_Phone_No != "" ? "Home Phone Number : " + humanRecord.Home_Phone_No + "\n" : string.Empty) + (humanRecord.cell_phone_no != "" ? "Cell Phone Number : " + humanRecord.cell_phone_no + "\n" : string.Empty) + (humanRecord.cell_phone_no != "" ? "Work Phone Number : " + humanRecord.Work_Phone_No : string.Empty);
                        //cell = CreateCell("Patient Phone Numbers : \n", sPhoneNumbers);
                        //patTable.AddCell(cell);
                        //cell = CreateCell("Patient Address: \n", humanRecord.Street_Address1 + "\n" + City);
                        //cell.Colspan = 3;
                        //patTable.AddCell(cell);
                        //doc.Add(patTable);
                        //doc.Add(new Paragraph("\n"));
                        //PdfPTable phyTable = new PdfPTable(new float[] { 40, 60 });
                        //phyTable.WidthPercentage = 100;
                        //cell = new PdfPCell(new Phrase("Insurance Details", reducedFont));
                        //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        //cell.Colspan = 2;
                        //phyTable.AddCell(cell);

                        doc.Add(new Paragraph("\n"));
                        string City = string.Empty;
                        if (humanRecord.City != string.Empty)
                        {
                            City = humanRecord.City;
                            if (humanRecord.State != string.Empty)
                                City += ", " + humanRecord.State + " " + humanRecord.ZipCode;
                        }
                        PdfPTable patTable = new PdfPTable(new float[] { 40, 20, 20, 20 });
                        patTable.WidthPercentage = 100;
                        PdfPCell cell = new PdfPCell(new Phrase("Patient Details", reducedFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        cell.Colspan = 4;
                        patTable.AddCell(cell);
                        cell = CreateCell("Patient Name: \n", humanRecord.Last_Name + "," + humanRecord.First_Name);//(new Phrase("Patient Name: \n" + humanRecord.Last_Name + "," + humanRecord.First_Name, normalFont));
                        patTable.AddCell(cell);
                        cell = CreateCell("Patient ID: \n", humanRecord.Human_ID.ToString());
                        patTable.AddCell(cell);
                        cell = CreateCell("Patient DOB: \n", humanRecord.Birth_Date.ToString("dd-MMM-yyyy"));
                        patTable.AddCell(cell);
                        cell = CreateCell("Patient Sex: \n", humanRecord.Sex);
                        patTable.AddCell(cell);
                        //cell = CreateCell("Patient SSN: \n", humanRecord.SSN);
                        string sPhoneNumbers = (humanRecord.Home_Phone_No != "" ? "Home Phone Number : " + humanRecord.Home_Phone_No + "\n" : string.Empty) + (humanRecord.cell_phone_no != "" ? "Cell Phone Number : " + humanRecord.cell_phone_no + "\n" : string.Empty) + (humanRecord.cell_phone_no != "" ? "Work Phone Number : " + humanRecord.Work_Phone_No : string.Empty);
                        cell = CreateCell("Patient Phone Numbers : \n", sPhoneNumbers);
                        patTable.AddCell(cell);
                        cell = CreateCell("Patient Address: \n", humanRecord.Street_Address1 + "\n" + City);
                        cell.Colspan = 3;
                        patTable.AddCell(cell);
                        doc.Add(patTable);
                        doc.Add(new Paragraph("\n"));
                        PdfPTable phyTable = new PdfPTable(new float[] { 40, 60 });
                        phyTable.WidthPercentage = 100;
                        cell = new PdfPCell(new Phrase("Insurance Details", reducedFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        cell.Colspan = 2;
                        phyTable.AddCell(cell);
                        if (humanRecord.PatientInsuredBag != null)
                        {
                            if (humanRecord.PatientInsuredBag.Count > 0)
                            {
                                if (humanRecord.PatientInsuredBag[0].Insured_Human_ID != humanRecord.Human_ID)
                                {
                                    Human insuredHuman = GetHumanByHumanID(humanRecord.PatientInsuredBag[0].Insured_Human_ID);
                                    if (insuredHuman != null)
                                    {
                                        cell = CreateCell("Name of Policy Holder: \n", insuredHuman.Last_Name + "," + insuredHuman.First_Name);// +"\nRelationship to Patient: "+humanRecord.PatientInsuredBag[0].Relationship+ "\nAddress: " + insuredHuman.Street_Address1 + "\nCity: ", normalFont);
                                        cell.AddElement(new Paragraph("Relationship to Patient: \n", reducedFont));
                                        cell.AddElement(new Paragraph(humanRecord.PatientInsuredBag[0].Relationship, normalFont));
                                        phyTable.AddCell(cell);
                                        string City1 = string.Empty;
                                        if (insuredHuman.City != string.Empty)
                                        {
                                            if (insuredHuman.Street_Address1 != string.Empty && insuredHuman.Street_Address2 != string.Empty)
                                                City1 = insuredHuman.Street_Address1 + "\n" + insuredHuman.Street_Address2;
                                            else City1 = insuredHuman.Street_Address1;
                                            if (insuredHuman.State != string.Empty && insuredHuman.City != string.Empty)
                                            {
                                                City1 += "\n" + insuredHuman.City + ", " + insuredHuman.State + " " + insuredHuman.ZipCode;
                                            }
                                            else
                                                City1 = "\n" + insuredHuman.City;
                                        }
                                        cell = CreateCell("Insured Human Address: \n", City1);
                                        phyTable.AddCell(cell);
                                    }

                                }
                                InsurancePlanManager objInsurancePlanManager = new InsurancePlanManager();
                                InsurancePlan objInsPlan = objInsurancePlanManager.GetInsurancebyID(humanRecord.PatientInsuredBag[0].Insurance_Plan_ID)[0];
                                if (objInsPlan != null)
                                {
                                    cell = CreateCell("Insurance Plan Name: \n", objInsPlan.Ins_Plan_Name);
                                    phyTable.AddCell(cell);
                                    cell = CreateCell("Insurance Address: \n", objInsPlan.Payer_Addrress1 + "\n" + objInsPlan.Payer_City);
                                    phyTable.AddCell(cell);
                                }

                            }
                        }
                        doc.Add(phyTable);

                        doc.Add(new Paragraph("\n"));
                        //cell = CreateCell("Patient SSN: \n", humanRecord.SSN);
                        //patTable.AddCell(cell);
                        //cell = CreateCell("Patient Address: \n", humanRecord.Street_Address1 + "\n" + City);
                        //cell.Colspan = 3;
                        //patTable.AddCell(cell);            
                        patTable = new PdfPTable(new float[] { 40, 60 });
                        patTable.WidthPercentage = 100;
                        cell = new PdfPCell(new Phrase("Referred to Physician & Facility Details", reducedFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        cell.Colspan = 2;
                        patTable.AddCell(cell);
                        cell = CreateCell("Referred to Provider: \n", objList[0].To_Physician_Name);
                        patTable.AddCell(cell);
                        cell = CreateCell("Referred to Provider Specialty: \n", Specialty);
                        patTable.AddCell(cell);
                        cell = CreateCell("Referred to Facility: \n", objList[0].To_Facility_Name);
                        patTable.AddCell(cell);
                        cell = CreateCell("Referred to Facility Address: \n", objList[0].To_Facility_Street_Address + "\n" + objList[0].To_Facility_City + "\n" + objList[0].To_Facility_State + "\n" + objList[0].To_Facility_Zip);
                        //cell.Colspan = 3;
                        patTable.AddCell(cell);
                        cell = CreateCell("Referred to Facility Phone #: \n", "\n" + objList[0].To_Facility_Phone_Number);
                        patTable.AddCell(cell);
                        cell = CreateCell("Referred to Facility Fax: \n", "\n" + objList[0].To_Facility_Fax_Number);
                        //cell.Colspan = 3;
                        patTable.AddCell(cell);
                        string Address = string.Empty;
                        FacilityLibrary objRefFacilty;
                        try
                        {
                            objRefFacilty = (from obj in facilityList where obj.Fac_Name == objList[0].To_Facility_Name select obj).ToList<FacilityLibrary>()[0];
                        }
                        catch
                        {
                            objRefFacilty = new FacilityLibrary();
                        }

                        //if (objRefFacilty != null)
                        //{

                        //    if (objRefFacilty.Fac_Address1 != string.Empty && objRefFacilty.Fac_Address2 != string.Empty)
                        //    {
                        //        Address = objRefFacilty.Fac_Address1 + "\n" + objRefFacilty.Fac_Address2;
                        //    }
                        //    else
                        //        Address = objRefFacilty.Fac_Address1;

                        //    if (objRefFacilty.Fac_City != string.Empty && objRefFacilty.Fac_State != string.Empty)
                        //    {
                        //        Address += "\n" + objRefFacilty.Fac_City + ", " + objRefFacilty.Fac_State + " " + objRefFacilty.Fac_Zip;
                        //    }

                        //    cell = CreateCell("Referred to Facility Address: \n", Address);
                        //    //cell.Colspan = 3;
                        //    patTable.AddCell(cell);
                        //    cell = CreateCell("Referred to Facility Phone #: \n", "\n" + objRefFacilty.Fac_Telephone);
                        //    patTable.AddCell(cell);
                        //    cell = CreateCell("Referred to Facility Fax: \n", "\n" + objRefFacilty.Fac_Fax);
                        //    //cell.Colspan = 3;
                        //    patTable.AddCell(cell);
                        //}


                        doc.Add(patTable);
                        PdfPTable refTable = new PdfPTable(new float[] { 100 });
                        refTable.WidthPercentage = 100;
                        cell = new PdfPCell(new Phrase("Referral Details", reducedFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        refTable.AddCell(cell);

                        for (int i = 0; i < objList.Count; i++)
                        {


                            ReferralOrder obj = objList[i];
                            IList<ReferralOrdersAssessment> assList = (from objAss in objRefOrdDTO.RefOrdAssList where objAss.Referral_Order_ID == obj.Id select objAss).ToList<ReferralOrdersAssessment>();
                            //  IList<ReferralOrdersProblemList> proList = (from objPro in objRefOrdDTO.RefOrdProbList where objPro.Referral_Order_ID == obj.Id select objPro).ToList<ReferralOrdersProblemList>();
                            string Diagnosis = string.Empty;
                            foreach (ReferralOrdersAssessment objAss in assList)
                            {
                                if (Diagnosis != string.Empty)
                                {
                                    Diagnosis += "\n                 " + objAss.ICD + "-" + objAss.Assessment_Description;
                                }
                                else
                                {
                                    Diagnosis += objAss.ICD + "-" + objAss.Assessment_Description;
                                }
                            }
                            //foreach (ReferralOrdersProblemList objPro in proList)
                            //{
                            //    if (Diagnosis != string.Empty)
                            //    {
                            //        Diagnosis += "\n                 " + objPro.ICD + "-" + objPro.ICD_Description;
                            //    }
                            //    else
                            //    {
                            //        Diagnosis += objPro.ICD + "-" + objPro.ICD_Description;
                            //    }
                            //}
                            cell = new PdfPCell();
                            par = new Paragraph("Number of Visits: " + obj.Number_of_Visit, normalFont);
                            par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                            cell.AddElement(par);
                            par = new Paragraph("Diagnosis: " + Diagnosis, normalFont);
                            par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                            cell.AddElement(par);
                            par = new Paragraph("Reason For Referral: " + obj.Reason_For_Referral, normalFont);
                            par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                            cell.AddElement(par);

                            if (obj.Service_Requested != string.Empty)
                            {
                                par = new Paragraph("Service Requested: " + obj.Service_Requested, normalFont);
                                par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                                cell.AddElement(par);
                            }
                            if (obj.Service_Requested != string.Empty)
                            {
                                par = new Paragraph("Special Needs : " + obj.Special_Needs, normalFont);
                                par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                                cell.AddElement(par);
                            }

                            if (obj.Referral_Notes != string.Empty)
                            {
                                par = new Paragraph("Referral Notes : " + obj.Referral_Notes, normalFont);
                                par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                                cell.AddElement(par);
                            }

                            refTable.AddCell(cell);
                        }
                        doc.Add(new Paragraph("\n"));
                        doc.Add(refTable);
                        par = new Paragraph("Regards", normalFont);
                        par.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        doc.Add(par);
                        par = new Paragraph("\n(" + objRefFromPhy.PhyPrefix + " " + objRefFromPhy.PhyFirstName +
                          " " + objRefFromPhy.PhyLastName + " " + objRefFromPhy.PhySuffix + ")", normalFont);
                        par.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        doc.Add(par);
                        doc.Add(new Paragraph("\n"));
                        string sDate = SignatureDate[0].ToString();
                        par = new Paragraph("Regards", normalFont);
                        par = new Paragraph("Electronically Signed by " + objRefFromPhy.PhyPrefix + " " + objRefFromPhy.PhyFirstName +
                            " " + objRefFromPhy.PhyLastName + " " + objRefFromPhy.PhySuffix + " On " + UtilityManager.ConvertToLocal(Convert.ToDateTime(sDate)).ToString("dd-MMM-yyyy hh:mm:ss tt"), normalFont);
                        par.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        doc.Add(par);
                        doc.Close();
                    }
                    return sPrintPathName;

                    //System.Diagnostics.Process.Start(sPrintPathName);
                }
            }
            return string.Empty;
        }

        public string PrintDMEOrdersForSelectedLab(OrdersSubmit objOrdersSubmit, PhysicianLibrary objPhysician, FillHumanDTO humanRecord, string OrderType, string Path)
        {
            #region FetchData
            OrdersManager mngrOrders = new OrdersManager();
            Encounter objEncounter = null;
            Lab objLab = null;
            LabLocation objLabLocation = null;
            IList<Orders> objOrders = null;
            IList<OrdersAssessment> objOrdersAssessment = null;
            mngrOrders.GetDataforDMEOrders(objOrdersSubmit, out objEncounter, out  objLab, out  objLabLocation,
            out  objOrders, out  objOrdersAssessment);
            #endregion

            #region Variable Declrtns
            string DME_VendorName = string.Empty;
            string DME_VendorAddress = string.Empty;
            string Lab_Name = string.Empty, Lab_Address = string.Empty, Lab_CSZ = string.Empty, Lab_Phone = string.Empty;
            string Phy_Name = string.Empty, Phy_Degree = string.Empty, Phy_NPI = string.Empty, Phy_TPI = string.Empty;
            string Pat_Name = string.Empty, Pat_Gender = string.Empty, Pat_DOB = string.Empty, Pat_Age = string.Empty,
              Pat_Address = string.Empty, Pat_CSZ = string.Empty, Pat_SSN = string.Empty, Pat_ID = string.Empty, Pat_Phone = string.Empty;
            string OrderCPT = string.Empty, OrderAssICD = string.Empty;
            string DateLastSeen = string.Empty, Duration_DME = string.Empty, Duration_Supplies = string.Empty;
            string declaration = string.Empty, Order_created_date_time = string.Empty;
            #endregion

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
            string sPrintPathName = string.Empty;
            string folderPath = Path;
            Directory.CreateDirectory(folderPath);

            var Createdate = (from o in objOrders select o.Created_Date_And_Time).OrderByDescending(c => c).ToArray();


            string strLabName = Regex.Replace(objLab.Lab_Name, @"[^0-9a-zA-Z]+", " ");
            if (objPhysician != null)
            {
                string labLocName = string.Empty;
                if (objLabLocation != null)
                    labLocName = "_" + objLabLocation.Location_Name;
                //sPrintPathName = folderPath + "\\" + humanRecord.Human_ID.ToString() + "_" + humanRecord.Last_Name + " " + humanRecord.MI + " " + humanRecord.First_Name + "_" + objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix + "_" + OrderType.Replace(' ', '_') + "_" + strLabName + "_" + objLabLocation.Location_Name + "_" + UtilityManager.ConvertToLocal(Createdate[0]).ToString("yyyyMMdd hhmmss tt") + ".pdf";//ordList[0].ObjOrder.Created_Date_And_Time.ToString("yyyyMMdd hhmmss tt") + ".pdf";
                sPrintPathName = folderPath + "\\" + humanRecord.Human_ID.ToString() + "_" + humanRecord.First_Name + " " + humanRecord.MI + " " + humanRecord.Last_Name + "_" + OrderType.Replace(' ', '_') + "_" + strLabName + labLocName + "_" + UtilityManager.ConvertToLocal(Createdate[0]).ToString("yyyyMMdd hhmmss tt") + ".pdf";//ordList[0].ObjOrder.Created_Date_And_Time.ToString("yyyyMMdd hhmmss tt") + ".pdf";
            }
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            iTextSharp.text.Rectangle pageSize = doc.PageSize;
            HeaderEventGenerate headerEvent = new HeaderEventGenerate();
            doc.Open();
            wr.PageEvent = headerEvent;
            headerEvent.OnStartPage(wr, doc);
            headerEvent.OnEndPage(wr, doc);

            #region DME_Vendor_Header
            if (objLab != null && objLab.Id != 0)
            {
                DME_VendorName = objLab.Lab_Name;
            }
            if (objLabLocation != null && objLabLocation.Id != 0)
            {
                DME_VendorAddress = objLabLocation.Street_Address1 + (objLabLocation.Street_Address2.Trim() != string.Empty ? ", " + objLabLocation.Street_Address2 : objLabLocation.Street_Address2) + " Ph:" + objLabLocation.Phone_No;
            }
            Paragraph pr = new Paragraph(DME_VendorName, reducedFont);
            pr.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            doc.Add(pr);
            pr = new Paragraph(DME_VendorAddress, reducedFont);
            pr.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            doc.Add(pr);
            pr = new Paragraph("DME/Medical Supplies Order Form", reducedFont);
            pr.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            doc.Add(pr);
            doc.Add(new Paragraph("\n"));
            #endregion

            #region LabLocation and Physician Details

            if (objLabLocation != null && objLabLocation.Id != 0)
            {
                Lab_Name = objLabLocation.Location_Name;
                Lab_Address = objLabLocation.Street_Address1 + (objLabLocation.Street_Address2.Trim() != string.Empty ?
                    ", " + objLabLocation.Street_Address2 : objLabLocation.Street_Address2);
                Lab_CSZ = objLabLocation.City + ", " + objLabLocation.State + ", " + objLabLocation.ZipCode;
                Lab_Phone = objLabLocation.Phone_No;
            }
            if (objPhysician != null && objPhysician.Id != 0)
            {
                Phy_Name = objPhysician.PhyPrefix + " " + objPhysician.PhyLastName + ", " + objPhysician.PhyFirstName +
                    " " + objPhysician.PhyMiddleName;
                Phy_Degree = objPhysician.PhySuffix;
                Phy_NPI = objPhysician.PhyNPI;

            }
            PdfPTable patTable = new PdfPTable(new float[] { 60, 60 });
            patTable.WidthPercentage = 100;
            PdfPCell cell = new PdfPCell(new Phrase("Client/Ordering Site Information", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            patTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Physician Information", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            patTable.AddCell(cell);
            string OrderingSiteInfo = string.Empty;

            FacilityManager objFacilityManager = new FacilityManager();
            IList<FacilityLibrary> ilstFacility = objFacilityManager.GetFacilityByFacilityname(objOrdersSubmit.Facility_Name);

            //FacilityLibrary objFacility = (from obj in ApplicationObject.facilityLibraryList where obj.Fac_Name == objOrdersSubmit.Facility_Name select obj).ToList<FacilityLibrary>()[0];
            if (ilstFacility.Count > 0)
            {
                //cell = CreateCell("Facility Name: \n", objFacility.Fac_Name);
                //phyTable.AddCell(cell);
                //cell = CreateCell("Facility Address: \n", objFacility.Fac_Address1 + "\n" + objFacility.Fac_City);
                //phyTable.AddCell(cell);

                OrderingSiteInfo = "Name: " + ilstFacility[0].Fac_Name + Environment.NewLine + "Address: " + ilstFacility[0].Fac_Address1 + Environment.NewLine + "City, State, Zip: " +
              ilstFacility[0].Fac_City + ", " + ilstFacility[0].Fac_State + ", " + ilstFacility[0].Fac_Zip + Environment.NewLine + "Phone: " + ilstFacility[0].Fac_Telephone;
                cell = new PdfPCell(new Phrase(OrderingSiteInfo, normalFont));
                patTable.AddCell(cell);
            }


            //OrderingSiteInfo = "Name: " + Lab_Name + Environment.NewLine + "Address: " + Lab_Address + Environment.NewLine + "City, State, Zip: " +
            //   Lab_CSZ + Environment.NewLine + "Phone: " + Lab_Phone;
            //cell = new PdfPCell(new Phrase(OrderingSiteInfo, normalFont));
            //patTable.AddCell(cell);
            string OrderingPhyInfo = string.Empty;
            OrderingPhyInfo = "Ordering Physician: " + Phy_Name + Environment.NewLine + "Physician Degree: " + Phy_Degree + Environment.NewLine
                + "Physician NPI: " + Phy_NPI + Environment.NewLine + "Physician TPI: " + Phy_TPI;
            cell = new PdfPCell(new Phrase(OrderingPhyInfo, normalFont));
            patTable.AddCell(cell);
            doc.Add(patTable);
            doc.Add(new Paragraph("\n"));
            #endregion

            #region Patient Details
            if (humanRecord != null && humanRecord.Human_ID != 0)
            {
                Pat_Name = humanRecord.Last_Name + ", " + humanRecord.First_Name + " " + humanRecord.MI;
                Pat_Gender = humanRecord.Sex;
                Pat_DOB = humanRecord.Birth_Date.ToString("dd-MMM-yyyy");
                Pat_Age = UtilityManager.CalculateAge(humanRecord.Birth_Date).ToString();
                Pat_Address = humanRecord.Street_Address1 + (humanRecord.Street_Address2.Trim() != string.Empty ? ", " + humanRecord.Street_Address2 : humanRecord.Street_Address2);
                Pat_CSZ = humanRecord.City;
                if (humanRecord.State != string.Empty)
                {
                    if (Pat_CSZ == string.Empty)
                    {
                        Pat_CSZ = humanRecord.State;
                    }
                    else
                    {
                        Pat_CSZ = Pat_CSZ + ", " + humanRecord.State;
                    }
                }
                if (humanRecord.ZipCode != string.Empty)
                {
                    if (Pat_CSZ == string.Empty)
                    {
                        Pat_CSZ = humanRecord.ZipCode;
                    }
                    else
                    {
                        Pat_CSZ = Pat_CSZ + ", " + humanRecord.ZipCode;
                    }
                }
                Pat_SSN = humanRecord.SSN;
                Pat_ID = Convert.ToString(humanRecord.Human_ID);
                Pat_Phone = humanRecord.cell_phone_no;
            }
            patTable = new PdfPTable(new float[] { 60, 60 });
            patTable.WidthPercentage = 100;
            cell = new PdfPCell(new Phrase("Patient Information", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 7;
            patTable.AddCell(cell);
            string PatientInfo_1, PatientInfo_2 = string.Empty;
            PatientInfo_1 = "Patient Name: " + Pat_Name + Environment.NewLine + "Gender: " + Pat_Gender + Environment.NewLine
                + "Date of Birth: " + Pat_DOB + Environment.NewLine + "Age: " + Pat_Age + Environment.NewLine + "Address: "
                + Pat_Address + Environment.NewLine + "City, State, Zip:" + Pat_CSZ + Environment.NewLine;
            cell = new PdfPCell(new Phrase(PatientInfo_1, normalFont));
            patTable.AddCell(cell);
            PatientInfo_2 = "Patient SSN: " + Pat_SSN + Environment.NewLine + "Patient ID:" + Pat_ID
                + Environment.NewLine + "Phone: " + Pat_Phone + Environment.NewLine;
            cell = new PdfPCell(new Phrase(PatientInfo_2, normalFont));
            patTable.AddCell(cell);
            doc.Add(patTable);
            doc.Add(new Paragraph("\n"));
            #endregion

            #region Order Details
            patTable = new PdfPTable(new float[] { 20, 10, 10, 10, 10, 20, 20 });
            patTable.WidthPercentage = 100;
            cell = new PdfPCell(new Phrase("HCPCS", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            patTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Quantity", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            patTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Prior authorization required?", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            patTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Beyond Quantity Limit?", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            patTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Custom Item?", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            patTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Justification", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            patTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("ICDs", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            patTable.AddCell(cell);

            if (objOrders != null && objOrders.Count > 0)
            {
                for (int i = 0; i < objOrders.Count; i++)
                {
                    cell = new PdfPCell(new Phrase(objOrders[i].Lab_Procedure + "-" + objOrders[i].Lab_Procedure_Description, normalFont));
                    patTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase(Convert.ToString(objOrders[i].Quantity), normalFont));
                    patTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase(objOrders[i].Prior_Auth_Req, normalFont));
                    patTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase(objOrders[i].Beyond_Qty_Limit, normalFont));
                    patTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase(objOrders[i].Custom_Item, normalFont));
                    patTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase(objOrders[i].Justification, normalFont));
                    patTable.AddCell(cell);
                    string AssICD = string.Empty;
                    //IList<OrdersAssessment> AssICDlst = objOrdersAssessment.Where(a => a.Order_ID == objOrders[i].Id).ToList<OrdersAssessment>();
                    IList<OrdersAssessment> AssICDlst = objOrdersAssessment.Where(a => a.Order_Submit_ID == objOrders[i].Order_Submit_ID).ToList<OrdersAssessment>();

                    if (AssICDlst != null && AssICDlst.Count > 0)
                    {
                        for (int k = 0; k < AssICDlst.Count; k++)
                        {
                            if (k == 0)
                                AssICD = AssICDlst[k].ICD + "-" + AssICDlst[k].ICD_Description;
                            else
                                AssICD = AssICD + ", " + AssICDlst[k].ICD + "-" + AssICDlst[k].ICD_Description;
                        }

                    }
                    cell = new PdfPCell(new Phrase(AssICD, normalFont));
                    patTable.AddCell(cell);
                }
            }
            doc.Add(patTable);
            doc.Add(new Paragraph("\n"));
            #endregion


            #region DME Duration Details

            if (objOrdersSubmit != null && objOrdersSubmit.Id != 0)
            {
                DateLastSeen = objOrdersSubmit.Date_Last_Seen.ToString("dd-MMM-yyyy");
                Duration_DME = Convert.ToString(objOrdersSubmit.Duration_for_DME_Need_in_Months);
                Duration_Supplies = Convert.ToString(objOrdersSubmit.Duration_for_Supplies_Need_in_Months);
            }
            patTable = new PdfPTable(new float[] { 60, 60 });
            patTable.WidthPercentage = 100;
            cell = new PdfPCell(new Phrase("Date Last seen by Physician: " + DateLastSeen, normalFont));
            cell.Colspan = 2;
            patTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Duration of need for DME: " + Duration_DME + " month(s)", normalFont));
            patTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Duration of need for supplies: " + Duration_Supplies + " month(s)", normalFont));
            patTable.AddCell(cell);
            doc.Add(patTable);
            doc.Add(new Paragraph("\n"));
            #endregion

            #region Declaration
            if (objOrdersSubmit != null && objOrdersSubmit.Id != 0)
            {
                Order_created_date_time = objOrdersSubmit.Created_Date_And_Time.ToString("dd-MMM-yyyy hh:mm:ss tt");
            }
            declaration = "By signing this form, I hereby attest the information is consistent with the determination of the " +
            "client's current medical necessity and prescription. By prescribing the identified DME and/or medical supplies, " +
            "I certify the prescribed items are appropriate and can safely be used in the client's home when used as prescribed ";
            doc.Add(new Paragraph(declaration, normalFont));
            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Electronically Signed by " + objPhysician.PhyPrefix + " " + objPhysician.PhyFirstName + " " + objPhysician.PhyMiddleName + " " + objPhysician.PhyLastName + " " + objPhysician.PhySuffix + " On " + UtilityManager.ConvertToLocal(Convert.ToDateTime(Order_created_date_time)).ToString("dd-MMM-yyyy hh:mm:ss tt"), normalFont));
            doc.Add(new Paragraph("\n"));
            doc.Close();
            #endregion

            return sPrintPathName;
        }


        public string PrintFaceSheet(FillQuickPatient objfillList, string sPath)
        {
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
            string sPrintPathName = string.Empty;
            string folderPath = sPath;
            Directory.CreateDirectory(folderPath);
            if (objfillList == null)
                return string.Empty;
            Human humanRecord = objfillList.HumanObj;

            sPrintPathName = folderPath + "\\" + "FaceSheet_" + objfillList.HumanObj.Id.ToString() + DateTime.Now.ToString("yyyyMMdd hhmmss tt") + ".pdf";
            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            iTextSharp.text.Rectangle pageSize = doc.PageSize;
            FaceSheetHeaderEventGenerate headerEvent = new FaceSheetHeaderEventGenerate();
            doc.Open();
            wr.PageEvent = headerEvent;
            headerEvent.OnStartPage(wr, doc);
            headerEvent.OnEndPage(wr, doc);

            Paragraph par = null;
            par = new Paragraph(DateTime.Now.ToString("dd-MMM-yyyy"), reducedFont);
            par.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
            doc.Add(par);

            doc.Add(new Paragraph("\n"));
            PdfPTable patTable = new PdfPTable(new float[] { 25, 25, 25, 25 });
            patTable.WidthPercentage = 100;
            PdfPCell cell = new PdfPCell(new Phrase("Patient Information", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 4;
            patTable.AddCell(cell);
            cell = CreateCell("Patient Name: \n", humanRecord.Last_Name + "," + humanRecord.First_Name);//(new Phrase("Patient Name: \n" + humanRecord.Last_Name + "," + humanRecord.First_Name, normalFont));
            patTable.AddCell(cell);
            cell = CreateCell("Patient Account #: \n", humanRecord.Id.ToString());
            patTable.AddCell(cell);
            cell = CreateCell("Patient DOB: \n", humanRecord.Birth_Date.ToString("dd-MMM-yyyy"));
            patTable.AddCell(cell);
            cell = CreateCell("Patient Sex: \n", humanRecord.Sex);
            patTable.AddCell(cell);
            cell = CreateCell("Patient SSN: \n", humanRecord.SSN);
            patTable.AddCell(cell);
            string City = string.Empty;
            if (humanRecord.Street_Address1 != string.Empty && humanRecord.Street_Address2 != string.Empty)
                City = humanRecord.Street_Address1 + ", " + humanRecord.Street_Address2;
            else City = humanRecord.Street_Address1;
            if (humanRecord.State != string.Empty && humanRecord.City != string.Empty)
            {
                City += "\n" + humanRecord.City + ", " + humanRecord.State + " " + humanRecord.ZipCode;
            }
            else
                City = "\n" + humanRecord.City;
            cell = CreateCell("Patient Address: \n", City);
            cell.Colspan = 2;
            patTable.AddCell(cell);
            cell = CreateCell("Preferred Corres: \n", humanRecord.Preferred_Confidential_Correspodence_Mode);
            patTable.AddCell(cell);
            cell = CreateCell("Employment Status: \n", humanRecord.Employment_Status);
            patTable.AddCell(cell);
            cell = CreateCell("Employer Name: \n", humanRecord.Employer_Name);
            patTable.AddCell(cell);
            cell = CreateCell("Preferred Language: \n", humanRecord.Preferred_Language);
            patTable.AddCell(cell);
            cell = CreateCell("Ethnicity: \n", humanRecord.Ethnicity);
            patTable.AddCell(cell);
            cell = CreateCell("Home Phone: \n", humanRecord.Home_Phone_No);
            patTable.AddCell(cell);
            cell = CreateCell("Cell Phone: \n", humanRecord.Cell_Phone_Number);
            patTable.AddCell(cell);
            string sPrint = string.Empty;
            if (humanRecord.Work_Phone_No != string.Empty)
            {
                sPrint = humanRecord.Work_Phone_No + "  Ext:" + humanRecord.Work_Phone_Ext;
            }
            cell = CreateCell("Work Phone: \n", sPrint);
            patTable.AddCell(cell);
            cell = CreateCell("Fax #: \n", humanRecord.Fax_Number);
            patTable.AddCell(cell);
            cell = CreateCell("E-Mail: \n", humanRecord.EMail);
            patTable.AddCell(cell);
            cell = CreateCell("Representative E-Mail: \n", humanRecord.Representative_Email);
            patTable.AddCell(cell);
            cell = CreateCell("Enroll Online Access: \n", humanRecord.Enrollment_Status);
            patTable.AddCell(cell);
            cell = CreateCell("Previous Name: \n", humanRecord.Previous_Name);
            patTable.AddCell(cell);
            doc.Add(patTable);

            doc.Add(new Paragraph("\n"));
            patTable = new PdfPTable(new float[] { 25, 25, 25, 25 });
            patTable.WidthPercentage = 100;
            cell = new PdfPCell(new Phrase("Care Giver Information", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 4;
            patTable.AddCell(cell);
            if (humanRecord.Care_Giver_Last_Name == string.Empty)
            {
                cell = CreateCell("Name: \n", string.Empty);
                patTable.AddCell(cell);
            }
            else
            {
                cell = CreateCell("Name: \n", humanRecord.Care_Giver_Last_Name + "," + humanRecord.Care_Giver_First_Name);//(new Phrase("Patient Name: \n" + humanRecord.Last_Name + "," + humanRecord.First_Name, normalFont));
                patTable.AddCell(cell);
            }
            cell = CreateCell("Relationship: \n", humanRecord.Care_Giver_Relation);
            patTable.AddCell(cell);
            cell = CreateCell("Phone#: \n", humanRecord.Care_Giver_Phone_Number);
            cell.Colspan = 2;
            patTable.AddCell(cell);
            doc.Add(patTable);

            doc.Add(new Paragraph("\n"));
            patTable = new PdfPTable(new float[] { 25, 25, 25, 25 });
            patTable.WidthPercentage = 100;
            cell = new PdfPCell(new Phrase("Guarantor Information", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 4;
            patTable.AddCell(cell);
            if (humanRecord.Guarantor_Last_Name == string.Empty)
            {
                cell = CreateCell("Name: \n", string.Empty);
                patTable.AddCell(cell);
            }
            else
            {
                cell = CreateCell("Name: \n", humanRecord.Guarantor_Last_Name + "," + humanRecord.Guarantor_First_Name);//(new Phrase("Patient Name: \n" + humanRecord.Last_Name + "," + humanRecord.First_Name, normalFont));
                patTable.AddCell(cell);
            }
            if (humanRecord.Guarantor_Birth_Date.ToString("dd-MMM-yyyy") == "01-Jan-0001")
            {
                cell = CreateCell("Date of Birth: \n", string.Empty);
                patTable.AddCell(cell);
            }
            else
            {
                cell = CreateCell("Date of Birth: \n", humanRecord.Guarantor_Birth_Date.ToString("dd-MMM-yyyy"));
                patTable.AddCell(cell);
            }
            cell = CreateCell("Sex: \n", humanRecord.Guarantor_Sex);
            patTable.AddCell(cell);
            cell = CreateCell("Relationship to Patient: \n", humanRecord.Guarantor_Relationship);
            patTable.AddCell(cell);
            City = string.Empty;
            if (humanRecord.Guarantor_Street_Address1 != string.Empty && humanRecord.Guarantor_Street_Address2 != string.Empty)
                City = humanRecord.Guarantor_Street_Address1 + "\n" + humanRecord.Guarantor_Street_Address2;
            else City = humanRecord.Guarantor_Street_Address1;
            if (humanRecord.Guarantor_State != string.Empty && humanRecord.Guarantor_City != string.Empty)
            {
                City += "\n" + humanRecord.Guarantor_City + ", " + humanRecord.Guarantor_State + " " + humanRecord.Guarantor_Zip_Code;
            }
            else
                City = "\n" + humanRecord.Guarantor_City;
            cell = CreateCell("Address: \n", City);
            cell.Colspan = 2;
            patTable.AddCell(cell);
            cell = CreateCell("Guarantor Email: \n", humanRecord.Gaurantor_Email);
            patTable.AddCell(cell);
            string sPhoneNumber = string.Empty;
            if (humanRecord.Guarantor_Home_Phone_Number != string.Empty)
            {
                sPhoneNumber += humanRecord.Guarantor_Home_Phone_Number + "\n";
            }
            if (humanRecord.Guarantor_CellPhone_Number != string.Empty)
            {
                sPhoneNumber += humanRecord.Guarantor_CellPhone_Number;
            }
            cell = CreateCell("Guarantor Phone Number: \n", sPhoneNumber);
            patTable.AddCell(cell);
            doc.Add(patTable);

            doc.Add(new Paragraph("\n"));
            patTable = new PdfPTable(new float[] { 25, 25, 25, 25 });
            patTable.WidthPercentage = 100;
            cell = new PdfPCell(new Phrase("Emergency Contact Information", reducedFont));
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Colspan = 4;
            patTable.AddCell(cell);
            if (humanRecord.Emergency_Cnt_Last_Name == string.Empty)
            {
                cell = CreateCell("Name: \n", string.Empty);
                patTable.AddCell(cell);
            }
            else
            {
                cell = CreateCell("Name: \n", humanRecord.Emergency_Cnt_Last_Name + "," + humanRecord.Emergency_Cnt_First_Name);//(new Phrase("Patient Name: \n" + humanRecord.Last_Name + "," + humanRecord.First_Name, normalFont));
                patTable.AddCell(cell);
            }
            if (humanRecord.Emergency_BirthDate.ToString("dd-MMM-yyyy") == "01-Jan-0001")
            {
                cell = CreateCell("Date of Birth: \n", string.Empty);
                patTable.AddCell(cell);
            }
            else
            {
                cell = CreateCell("Date of Birth: \n", humanRecord.Emergency_BirthDate.ToString("dd-MMM-yyyy"));
                patTable.AddCell(cell);
            }
            cell = CreateCell("Sex: \n", humanRecord.Emergency_Cnt_Sex);
            patTable.AddCell(cell);
            cell = CreateCell("Relationship to Patient: \n", humanRecord.Emer_Relation);
            patTable.AddCell(cell);
            City = string.Empty;
            if (humanRecord.Emergency_Cnt_StreetAddr1 != string.Empty && humanRecord.Emergency_Cnt_StreetAddr2 != string.Empty)
                City = humanRecord.Emergency_Cnt_StreetAddr1 + "\n" + humanRecord.Emergency_Cnt_StreetAddr2;
            else City = humanRecord.Emergency_Cnt_StreetAddr1;
            if (humanRecord.Emergency_Cnt_State != string.Empty && humanRecord.Emergency_Cnt_City != string.Empty)
            {
                City += "\n" + humanRecord.Emergency_Cnt_City + ", " + humanRecord.Emergency_Cnt_State + " " + humanRecord.Emergency_Cnt_ZipCode;
            }
            else
                City = "\n" + humanRecord.Emergency_Cnt_City;
            cell = CreateCell("Address: \n", City);
            cell.Colspan = 2;
            patTable.AddCell(cell);
            cell = CreateCell("Emergency Home Phone Number: \n", humanRecord.Emergency_Cnt_Home_Phone_Number);
            patTable.AddCell(cell);
            cell = CreateCell("Emergency Cell Phone Number: \n", humanRecord.Emergency_Cnt_CellPhone_Number);
            patTable.AddCell(cell);
            doc.Add(patTable);

            string[] insurancetypes = new[] { "PRIMARY", "SECONDARY", "TERTIARY" };
            IList<PatientInsuredPlan> PatInsOrderedList = new List<PatientInsuredPlan>();
            PatInsOrderedList = humanRecord.PatientInsuredBag.OrderBy(x => x.Sort_Order).Where(s => insurancetypes.Contains(s.Insurance_Type)).ToList<PatientInsuredPlan>();
            humanRecord.PatientInsuredBag = PatInsOrderedList;

            IList<PatientInsuredPlan> tempPatInsList = new List<PatientInsuredPlan>();
            if (humanRecord.PatientInsuredBag != null && humanRecord.PatientInsuredBag.Count > 0)
            {
                for (int iCount = 0; iCount <= humanRecord.PatientInsuredBag.Count - 1; iCount++)
                {
                    tempPatInsList.Clear();
                    tempPatInsList.Add(humanRecord.PatientInsuredBag[iCount]);

                    doc.Add(new Paragraph("\n"));
                    patTable = new PdfPTable(new float[] { 25, 25, 25, 25 });
                    patTable.WidthPercentage = 100;
                    cell = new PdfPCell(new Phrase("Insurance Information (" + tempPatInsList[0].Insurance_Type + ")", reducedFont));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.Colspan = 4;
                    patTable.AddCell(cell);

                    if (tempPatInsList.Count > 0)
                    {
                        cell = CreateCell("Policy Holder ID: \n", tempPatInsList[0].Policy_Holder_ID);
                        patTable.AddCell(cell);

                        Human insuredHuman = GetHumanByHumanID(humanRecord.PatientInsuredBag[0].Insured_Human_ID);
                        if (insuredHuman != null)
                        {
                            cell = CreateCell("Insured Name: \n", insuredHuman.Last_Name + "," + insuredHuman.First_Name);// +"\nRelationship to Patient: "+humanRecord.PatientInsuredBag[0].Relationship+ "\nAddress: " + insuredHuman.Street_Address1 + "\nCity: ", normalFont);
                            patTable.AddCell(cell);
                        }
                        cell = CreateCell("Relationship to Insured: \n", tempPatInsList[0].Relationship);
                        patTable.AddCell(cell);
                        cell = CreateCell("Group #: \n", tempPatInsList[0].Group_Number);
                        patTable.AddCell(cell);

                        InsurancePlanManager InsPlanMngr = new InsurancePlanManager();
                        IList<InsurancePlan> insplanList = InsPlanMngr.GetInsurancebyID(tempPatInsList[0].Insurance_Plan_ID);

                        if (insplanList.Count > 0)
                        {
                            CarrierManager carrierMngr = new CarrierManager();
                            Carrier objCarrier = carrierMngr.GetCarrierUsingId(Convert.ToUInt64(insplanList[0].Carrier_ID));

                            cell = CreateCell("Carrier: \n", objCarrier.Carrier_Name);
                            patTable.AddCell(cell);

                            cell = CreateCell("Insurance Plan: \n", insplanList[0].Ins_Plan_Name);
                            patTable.AddCell(cell);
                        }
                        if (tempPatInsList[0].Effective_Start_Date.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                        {
                            cell = CreateCell("Effective Start Date: \n", tempPatInsList[0].Effective_Start_Date.ToString("dd-MMM-yyyy"));
                            patTable.AddCell(cell);
                        }
                        else
                        {
                            cell = CreateCell("Effective Start Date: \n", string.Empty);
                            patTable.AddCell(cell);
                        }
                        if (tempPatInsList[0].Termination_Date.ToString("dd-MMM-yyyy") != "01-Jan-0001")
                        {
                            cell = CreateCell("Termed Date: \n", tempPatInsList[0].Termination_Date.ToString("dd-MMM-yyyy"));
                            patTable.AddCell(cell);
                        }
                        else
                        {
                            cell = CreateCell("Termed Date: \n", string.Empty);
                            patTable.AddCell(cell);
                        }
                        cell = CreateCell("Copay: \n", tempPatInsList[0].PCP_Copay.ToString());
                        patTable.AddCell(cell);
                        cell = CreateCell("Deductible: \n", tempPatInsList[0].Deductible.ToString());
                        patTable.AddCell(cell);
                        cell = CreateCell("PCP: \n", tempPatInsList[0].PCP_Name);
                        patTable.AddCell(cell);
                        cell = CreateCell("PCP NPI: \n", tempPatInsList[0].PCP_NPI);
                        patTable.AddCell(cell);
                    }
                    doc.Add(patTable);
                }
            }
            else
            {
                doc.Add(new Paragraph("\n"));
                patTable = new PdfPTable(new float[] { 25, 25, 25, 25 });
                patTable.WidthPercentage = 100;
                cell = new PdfPCell(new Phrase("Insurance Information (Primary)", reducedFont));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Colspan = 4;
                patTable.AddCell(cell);
                doc.Add(patTable);

                doc.Add(new Paragraph("\n"));
                patTable = new PdfPTable(new float[] { 25, 25, 25, 25 });
                patTable.WidthPercentage = 100;
                cell = new PdfPCell(new Phrase("Insurance Information (Secondary)", reducedFont));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Colspan = 4;
                patTable.AddCell(cell);
                doc.Add(patTable);

                doc.Add(new Paragraph("\n"));
                patTable = new PdfPTable(new float[] { 25, 25, 25, 25 });
                patTable.WidthPercentage = 100;
                cell = new PdfPCell(new Phrase("Insurance Information (Tertiary)", reducedFont));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Colspan = 4;
                patTable.AddCell(cell);
                doc.Add(patTable);
            }

            Rcopia_AllergyManager rcopiaAllergyMngr = new Rcopia_AllergyManager();
            IList<Rcopia_Allergy> lstrcopiaAllergy = rcopiaAllergyMngr.GetRAllergyByHumanID(humanRecord.Id.ToString());

            if (lstrcopiaAllergy.Count > 0)
            {
                doc.Add(new Paragraph("\n"));
                patTable = new PdfPTable(new float[] { 100 });
                patTable.WidthPercentage = 100;
                cell = new PdfPCell(new Phrase("Allergy Information", reducedFont));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                patTable.AddCell(cell);

                string sAllergy = string.Empty;
                for (int iCount = 0; iCount <= lstrcopiaAllergy.Count - 1; iCount++)
                {
                    sAllergy += lstrcopiaAllergy[iCount].Allergy_Name.ToString() + "\n";
                }

                cell = CreateCell("Allergies: \n", sAllergy);
                patTable.AddCell(cell);

                doc.Add(patTable);
            }

            doc.Close();
            return sPrintPathName;
        }

        public string PrintReceipt(ulong ulEncounterID, ulong ulHumanID, string sPath, Boolean bIsEncounterBased)
        {
            IList<FillPrintRecipt> objFillPrintRecipt = new List<FillPrintRecipt>();
            iTextSharp.text.Font normalFont = iTextSharp.text.FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font boldFont = iTextSharp.text.FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font reducedFont = iTextSharp.text.FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font onlyBoldFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);
            string FaxSubject = string.Empty;
            EncounterManager EncMngr = new EncounterManager();
            objFillPrintRecipt = EncMngr.FillPrintRecipt(ulEncounterID, ulHumanID, bIsEncounterBased);
            if (objFillPrintRecipt.Count > 0)
            {
                //hdnFileName.Value = "Documents\\" + Session.SessionID + "\\" + DateTime.Now.ToString("yyyyMMdd");
                //string sDirPath = Server.MapPath("Documents\\" + Session.SessionID + "\\" + DateTime.Now.ToString("yyyyMMdd"));

                string sPrintPathName = string.Empty;
                if (sPath != string.Empty)
                {
                    if (bIsEncounterBased == false)
                        sPrintPathName = sPath + "\\" + objFillPrintRecipt[0].Human_ID + "_" + objFillPrintRecipt[0].First_Name.Replace("/", "") + " " + objFillPrintRecipt[0].MI.Replace("/", "") + " " + objFillPrintRecipt[0].Last_Name.Replace("/", "") + "_" + objFillPrintRecipt[0].Provider_Name + "_VN" + ulEncounterID.ToString() + "_" + "Print Receipt" + ".pdf";
                    else
                        sPrintPathName = sPath + "\\" + objFillPrintRecipt[0].Human_ID + "_" + objFillPrintRecipt[0].First_Name.Replace("/", "") + " " + objFillPrintRecipt[0].MI.Replace("/", "") + " " + objFillPrintRecipt[0].Last_Name.Replace("/", "") + "_" + objFillPrintRecipt[0].Provider_Name + "_EN" + ulEncounterID.ToString() + "_" + "Print Receipt" + ".pdf";
                }

                //string[] Split = new string[] { Server.MapPath("") };
                //string[] FileName = sPrintPathName.Split(Split, StringSplitOptions.RemoveEmptyEntries);
                //if (hdnFileName.Value == string.Empty)
                //{
                //    hdnFileName.Value = FileName[0].ToString();
                //}
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 20, 20, 50, 50);

                PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
                iTextSharp.text.Rectangle pageSize = doc.PageSize;
                doc.Open();
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                PdfPTable pdfTable = new PdfPTable(new float[] { 20, 20, 1700, 50 });

                string sClientFullName = string.Empty;
                var client = from c in ApplicationObject.ClientList where c.Legal_Org == ClientSession.LegalOrg select c;
                IList<Client> currentClientList = client.ToList<Client>();

                if (currentClientList.Count > 0)
                {
                    sClientFullName = currentClientList[0].Client_Full_Name;
                }


                PdfPCell pdfCell = new PdfPCell(new Phrase(sClientFullName, onlyBoldFont));
                pdfCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.Colspan = 4;
                pdfTable.AddCell(pdfCell);
                doc.Add(pdfTable);

                pdfTable = new PdfPTable(new float[] { 65, 10, 65, 65, 10, 65 });
                pdfCell = CreateCellforPrintReceipt("Prov.Name  ", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);


                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);


                pdfCell = CreateCellforPrintReceipt(objFillPrintRecipt[0].Provider_Name.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt("Appt Date ", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);




                if (UtilityManager.ConvertToLocal(objFillPrintRecipt[0].Appointment_Date).ToString("dd-MMM-yyyy hh:mm tt") != "01-Jan-0001 12:00 AM")
                    pdfCell = CreateCellforPrintReceipt(UtilityManager.ConvertToLocal(objFillPrintRecipt[0].Appointment_Date).ToString("dd-MMM-yyyy hh:mm tt"), "Text");
                else
                    pdfCell = CreateCellforPrintReceipt("", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);

                pdfTable = new PdfPTable(new float[] { 65, 10, 65, 65, 10, 65 });
                pdfCell = CreateCellforPrintReceipt("Fac Name  ", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);



                pdfCell = CreateCellforPrintReceipt(objFillPrintRecipt[0].Facility_Name.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);


                pdfCell = CreateCellforPrintReceipt("Acc#/External Acc# ", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);


                pdfCell = CreateCellforPrintReceipt(objFillPrintRecipt[0].Human_ID.ToString() + " / " + objFillPrintRecipt[0].Patient_Account_External.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);


                pdfTable = new PdfPTable(new float[] { 65, 10, 65, 65, 10, 65 });
                pdfCell = CreateCellforPrintReceipt("Pat. Name  ", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);
                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);



                pdfCell = CreateCellforPrintReceipt(objFillPrintRecipt[0].Last_Name + " " + objFillPrintRecipt[0].Suffix + "," + objFillPrintRecipt[0].First_Name + " " + objFillPrintRecipt[0].MI, "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt("Pri Ins ", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);



                pdfCell = CreateCellforPrintReceipt(objFillPrintRecipt[0].Insurance_Plan_Name.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);


                pdfTable = new PdfPTable(new float[] { 65, 10, 65, 65, 10, 65 });
                pdfCell = CreateCellforPrintReceipt("Pat. DOB  ", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);






                pdfCell = CreateCellforPrintReceipt(objFillPrintRecipt[0].Birth_Date.ToString("dd-MMM-yyyy"), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt("Ins Id ", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);




                pdfCell = CreateCellforPrintReceipt(objFillPrintRecipt[0].Policy_Holder_ID.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);

                pdfTable = new PdfPTable(new float[] { 65, 10, 65, 65, 10, 65 });
                pdfCell = CreateCellforPrintReceipt("Pat. Address  ", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);
                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);






                pdfCell = CreateCellforPrintReceipt(objFillPrintRecipt[0].Street_Address.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt("Voucher# ", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);




                if (bIsEncounterBased == true)
                    pdfCell = CreateCellforPrintReceipt("EN" + objFillPrintRecipt[0].Encounter_ID.ToString(), "Text");
                else
                    pdfCell = CreateCellforPrintReceipt("VN" + objFillPrintRecipt[0].Encounter_ID.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);

                pdfTable = new PdfPTable(new float[] { 65, 10, 65, 65, 10, 65 });
                pdfCell = CreateCellforPrintReceipt(" ", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(" ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);


                pdfCell = CreateCellforPrintReceipt(objFillPrintRecipt[0].City.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(" ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(" ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);



                pdfCell = CreateCellforPrintReceipt(" ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);


                pdfTable = new PdfPTable(new float[] { 65, 10, 65, 65, 10, 65 });
                pdfCell = CreateCellforPrintReceipt(" ", "Text");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(" ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);


                if (objFillPrintRecipt[0].State.ToString() != "")
                    pdfCell = CreateCellforPrintReceipt(objFillPrintRecipt[0].State.ToString() + " " + objFillPrintRecipt[0].Zipcode.ToString(), "Text");
                else
                    pdfCell = CreateCellforPrintReceipt(objFillPrintRecipt[0].Zipcode.ToString(), "Text");

                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(" ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(" ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);



                pdfCell = CreateCellforPrintReceipt(" ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);


                pdfTable = new PdfPTable(new float[] { 65, 10, 65, 65, 10, 65 });
                pdfCell = CreateCellforPrintReceipt("PCP Copay ", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);



                pdfCell = CreateCellforPrintReceipt("$" + " " + objFillPrintRecipt[0].PCP_Copay.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt("SPC Copay ", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);



                pdfCell = CreateCellforPrintReceipt("$" + " " + objFillPrintRecipt[0].SPC_Copay.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);

                pdfTable = new PdfPTable(new float[] { 65, 10, 65, 65, 10, 65 });
                pdfCell = CreateCellforPrintReceipt("Total Ded ", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);



                pdfCell = CreateCellforPrintReceipt("$" + " " + objFillPrintRecipt[0].Total_Deduction.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt("Ded.Met ", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);



                pdfCell = CreateCellforPrintReceipt("$" + " " + objFillPrintRecipt[0].Deduction_Met.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);


                pdfTable = new PdfPTable(new float[] { 65, 10, 65, 65, 10, 65 });
                pdfCell = CreateCellforPrintReceipt("Past Due ", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 3;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;


                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt("$" + " " + objFillPrintRecipt[0].PastDue.ToString(), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 3;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt("Coins ", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(":  ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 1;


                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(objFillPrintRecipt[0].Coinsurance.ToString() + " " + "%", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 3;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);

                pdfTable = new PdfPTable(new float[] { 40, 40, 40, 40, 40, 50, 50, 40, 50 });

                pdfTable.WidthPercentage = 80;
                pdfTable.KeepTogether = true;

                pdfCell = CreateCellDynamic("Payment Method", "", "");
                pdfTable.AddCell(pdfCell);
                pdfCell = CreateCellDynamic("Check/Card#", "", "");
                pdfTable.AddCell(pdfCell);
                pdfCell = CreateCellDynamic("Copay $", "", "");
                pdfTable.AddCell(pdfCell);
                pdfCell = CreateCellDynamic("Rec'd on Acc $", "", "");
                pdfTable.AddCell(pdfCell);
                pdfCell = CreateCellDynamic("Refund Amt $", "", "");
                pdfTable.AddCell(pdfCell);
                pdfCell = CreateCellDynamic("Transaction By", "", "");
                pdfTable.AddCell(pdfCell);
                pdfCell = CreateCellDynamic("Transaction Date & Time", "", "");
                pdfTable.AddCell(pdfCell);
                pdfCell = CreateCellDynamic("Payment Note", "", "");
                pdfTable.AddCell(pdfCell);
                pdfCell = CreateCellDynamic("Amount Paid By", "", "");
                pdfTable.AddCell(pdfCell);
                decimal PaymentAmount = 0;
                decimal RefundAmount = 0;
                decimal RecOnAcc = 0;
                for (int j = 0; j < objFillPrintRecipt.Count; j++)
                {
                    pdfCell = CreateCellDynamic("", objFillPrintRecipt[j].Payment_Method.ToString(), "");
                    pdfTable.AddCell(pdfCell);
                    pdfCell = CreateCellDynamic("", objFillPrintRecipt[j].Check_Card_No.ToString(), "");
                    pdfTable.AddCell(pdfCell);
                    pdfCell = CreateCellDynamic("", objFillPrintRecipt[j].Payment_Amount.ToString(), "");
                    pdfTable.AddCell(pdfCell);
                    pdfCell = CreateCellDynamic("", objFillPrintRecipt[j].Rec_On_Acc.ToString(), "");
                    pdfTable.AddCell(pdfCell);
                    PaymentAmount = PaymentAmount + Convert.ToDecimal(objFillPrintRecipt[j].Payment_Amount);
                    RecOnAcc = RecOnAcc + Convert.ToDecimal(objFillPrintRecipt[j].Rec_On_Acc);
                    pdfCell = CreateCellDynamic("", objFillPrintRecipt[j].Refund_Amount.ToString(), "");
                    pdfTable.AddCell(pdfCell);

                    RefundAmount = RefundAmount + Convert.ToDecimal(objFillPrintRecipt[j].Refund_Amount);

                    pdfCell = CreateCellDynamic("", objFillPrintRecipt[j].Created_By.ToString(), "");
                    pdfTable.AddCell(pdfCell);
                    pdfCell = CreateCellDynamic("", (UtilityManager.ConvertToLocal(objFillPrintRecipt[j].Payment_Date).ToString("dd-MMM-yyyy hh:mm tt")), "");
                    pdfTable.AddCell(pdfCell);
                    pdfCell = CreateCellDynamic("", objFillPrintRecipt[j].Payment_Note.ToString(), "");
                    pdfTable.AddCell(pdfCell);

                    pdfCell = CreateCellDynamic("", objFillPrintRecipt[j].Amount_Paid_By.ToString(), "");
                    pdfTable.AddCell(pdfCell);

                }
                doc.Add(pdfTable);

                pdfTable = new PdfPTable(new float[] { 40, 60, 50, 50 });
                pdfCell = CreateCellforPrintReceipt("Total Payment $:", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 0;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(Convert.ToString((PaymentAmount + RecOnAcc) - RefundAmount), "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 0;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(" ", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 0;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(" ", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                //pdfCell.PaddingTop = 1;
                //pdfCell.PaddingBottom = 0;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);

                pdfTable = new PdfPTable(new float[] { 65, 10, 65, 65, 10, 65 });
                pdfCell = CreateCellforPrintReceipt(" ", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 0;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(" ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 0;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(" ", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                pdfCell.PaddingTop = 1;
                pdfCell.PaddingBottom = 0;
                pdfTable.AddCell(pdfCell);

                //pdfCell = CreateCellforPrintReceipt("-------------------------------", "Text");
                //pdfCell.BorderWidthLeft = 1;
                //pdfCell.BorderWidthRight = 1;
                //pdfCell.BorderWidthTop = 0;
                //pdfCell.BorderWidthBottom = 0;
                //pdfCell.PaddingTop = 1;
                //pdfCell.PaddingBottom = 0;
                //pdfTable.AddCell(pdfCell);


                pdfCell = CreateCellforPrintReceipt(" ", " ");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 0;
                //pdfCell.PaddingTop = 1;
                //pdfCell.PaddingBottom = 0;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);


                pdfTable = new PdfPTable(new float[] { 30, 50, 30, 30 });

                pdfCell = CreateCellforPrintReceipt(" ", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                //pdfCell.PaddingTop = 0;
                //pdfCell.PaddingBottom = 3;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(" ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                //pdfCell.PaddingTop = 0;
                //pdfCell.PaddingBottom = 3;
                pdfTable.AddCell(pdfCell);


                pdfCell = CreateCellforPrintReceipt(" ", "Header");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 0;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                pdfCell.PaddingTop = 0;
                pdfCell.PaddingBottom = 3;
                pdfTable.AddCell(pdfCell);


                pdfCell = CreateCellforPrintReceipt(" ", "Text");
                pdfCell.BorderWidthLeft = 0;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 0;
                pdfCell.BorderWidthBottom = 1;
                //pdfCell.PaddingTop = 0;
                //pdfCell.PaddingBottom = 3;
                pdfTable.AddCell(pdfCell);

                pdfCell = CreateCellforPrintReceipt(" ", "Header");
                pdfCell.BorderWidthLeft = 1;
                pdfCell.BorderWidthRight = 1;
                pdfCell.BorderWidthTop = 1;
                pdfCell.BorderWidthBottom = 1;
                pdfCell.PaddingTop = 0;
                pdfCell.PaddingBottom = 3;
                pdfTable.AddCell(pdfCell);

                doc.Add(pdfTable);

                doc.Close();

                FaxSubject = "Payment Receipt_" + objFillPrintRecipt[0].First_Name + " " + objFillPrintRecipt[0].Last_Name + "_" + (objFillPrintRecipt[0].Date_of_service.ToString("dd-MMM-yyyy") == "01-Jan-0001" ? objFillPrintRecipt[0].Appointment_Date.ToString("dd-MMM-yyyy") : objFillPrintRecipt[0].Date_of_service.ToString("dd-MMM-yyyy"));

                return sPrintPathName + "|" + FaxSubject;
            }
            else
            {
                return "No Receipt";
            }
        }

        public string PrintInterpretationNotes(string sText, string sPhysicianName, string sFacilityAddress, string sPatientInfo, string sHumanID, string sPath, string sInterpretationTitle, string sSignatureDateTime, string sSignedPhysicianName)
        {
            iTextSharp.text.Font normalFont = iTextSharp.text.FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font boldFont = iTextSharp.text.FontFactory.GetFont("Arial", 13, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font reducedFont = iTextSharp.text.FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font onlyBoldFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);
            string FaxSubject = string.Empty;
            EncounterManager EncMngr = new EncounterManager();



            //string sPrintPathName = string.Empty;
            //sPrintPathName = sPath + "\\" + ClientSession.HumanId + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + "Print Interpretation Notes" + ".pdf";
            string[] sTemplate = sText.Split(';');
            string sNotes = string.Empty;
            string sHeader = string.Empty;
            string sTop = string.Empty;
            string sMiddle = string.Empty;
            string sBottom = string.Empty;
            if (sTemplate.Count() > 1)
            {
                string[] sSeperator = new string[] { "Test Reviewed: " };
                string[] sTemplates = sTemplate[1].Split(';');
                sNotes = sTemplate[0].Split(sSeperator, StringSplitOptions.None)[1];
                //CAP-655 remove (M) from suffix, Replace "(M)" with an empty string using regular expression
                string pattern = @"\s?\(M\)$";
                sNotes = Regex.Replace(sNotes, pattern, string.Empty);

                string sReplace = "Interpreting Physician";
                if (sTemplate[1].Split(new string[] { "Interpreting Physician" }, StringSplitOptions.None).Length > 1)
                {
                    string[] sHeaders = sTemplate[1].Split(new string[] { "Interpreting Physician" }, StringSplitOptions.None)[1].Split('\n');
                    sHeaders[1] = "Notes Header";
                    sHeader = sTemplate[1].Split(new string[] { "Interpreting Physician" }, StringSplitOptions.None)[0] + sReplace + string.Join("", sHeaders);

                    sTop = sTemplate[1].Split(new string[] { "Interpreting Physician" }, StringSplitOptions.None)[0] + sReplace + string.Join("", sHeaders).Split(new string[] { "Notes Header" }, StringSplitOptions.None)[0];
                    sMiddle = sNotes;
                    sBottom = string.Join("\n", sHeaders).Split(new string[] { "Notes Header" }, StringSplitOptions.None)[1];
                }
                else
                {
                    sMiddle = sNotes;
                    sBottom = sTemplate[1];
                }
            }
            string sPrintPathName = string.Empty;
            sPrintPathName = sPath + "\\" + sHumanID + "_" + sInterpretationTitle + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 20, 20, 50, 50);

            PdfWriter wr = PdfWriter.GetInstance(doc, new FileStream(sPrintPathName, FileMode.Create));
            iTextSharp.text.Rectangle pageSize = doc.PageSize;

            string sFooter = string.Empty;
            if (sSignatureDateTime != string.Empty)
            {
                //sFooter = "Electronically Signed by " + sPhysicianName + " at " + Convert.ToDateTime(sSignatureDateTime).ToString("dd-MMM-yyyy");
                //Jir Cap - 488
                //sFooter = "Electronically Signed by " + sSignedPhysicianName + " at " + Convert.ToDateTime(sSignatureDateTime).ToString("dd-MMM-yyyy");
                sFooter = "Electronically Signed by " + sSignedPhysicianName + " on " + Convert.ToDateTime(sSignatureDateTime).ToString("dd-MMM-yyyy");
            }
            //else
            //{
            //    sFooter = sPhysicianName;
            //}

            InterpretationNoteHeaderEventGenerate headerEvent = new InterpretationNoteHeaderEventGenerate("RE:" + sPatientInfo.Split('\r')[0].Replace("Patient Name:", "").Trim(), sFooter);
            doc.Open();
            wr.PageEvent = headerEvent;
            headerEvent.OnStartPage(wr, doc);
            headerEvent.OnEndPage(wr, doc);



            //doc.Open();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            //PdfPTable pdfTable = new PdfPTable(new float[] { 100 });

            //CAP-497: Remove the Signed Physician name in the Interpretation notes header
            //Paragraph par = new Paragraph(sPhysicianName, boldFont);
            //par.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            //doc.Add(par);

            //FacilityManager facMngr = new FacilityManager();
            //IList<FacilityLibrary> facList = facMngr.GetFacilityListByFacilityName(sFacilityName);
            //if (facList != null && facList.Count > 0)
            //{
            //string sFacilityAddress = sFacilityName; // facList[0].Fac_Address1 + Environment.NewLine + facList[0].Fac_City + " " + facList[0].Fac_State + " " + facList[0].Fac_Zip + Environment.NewLine + facList[0].Fac_Telephone + Environment.NewLine + Environment.NewLine;

            Paragraph par = new Paragraph(sFacilityAddress, normalFont);
            par.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            doc.Add(par);
            //}

            par = new Paragraph(sPatientInfo + Environment.NewLine, normalFont);
            par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            doc.Add(par);

            //par = new Paragraph(sText + Environment.NewLine + Environment.NewLine + Environment.NewLine, normalFont);
            par = new Paragraph(sTop + Environment.NewLine + Environment.NewLine, normalFont);
            par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            doc.Add(par);

            par = new Paragraph(sMiddle + Environment.NewLine + Environment.NewLine, boldFont);
            par.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            doc.Add(par);

            par = new Paragraph(sBottom + Environment.NewLine + Environment.NewLine + Environment.NewLine, normalFont);
            par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            doc.Add(par);

            //if (sSignatureDateTime != string.Empty)
            //{
            //    par = new Paragraph("Electronically Signed by " + sPhysicianName + " at " + sSignatureDateTime, normalFont);
            //}
            //else
            //{
            //    par = new Paragraph(sPhysicianName, normalFont);
            //}
            //par.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
            //doc.Add(par);

            doc.Close();

            return sPrintPathName;
        }

        private PdfPCell CreateCellDynamic(string HeaderText, string ValueText, string ModuleText)
        {
            iTextSharp.text.Font normalFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font reducedFont = iTextSharp.text.FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);
            iTextSharp.text.Font HeadFont = iTextSharp.text.FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.WHITE);
            PdfPCell cell = new PdfPCell();
            Paragraph par = new Paragraph(HeaderText, reducedFont);
            cell.AddElement(par);
            par = new Paragraph(ValueText, normalFont);
            cell.AddElement(par);
            par = new Paragraph(ModuleText, HeadFont);
            cell.AddElement(par);
            return cell;
        }
    }
}
