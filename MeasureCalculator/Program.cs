using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Acurus.Capella.Core.DomainObjects;
using Acurus.Capella.DataAccess.ManagerObjects;

namespace MeasureCalculator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            PQRI_MeasureManager pqriMeasureMngr = new PQRI_MeasureManager();
            IList<PQRI_Measure> ilstPQRIMeasure = new List<PQRI_Measure>();
            //PQRIMeasureCalculator pqriCalc = new PQRIMeasureCalculator();
            //DateTime dtFromDate = Convert.ToDateTime(DateTime.Now.Year + "-01-01");
            DateTime dtFromDate = Convert.ToDateTime(System.Configuration.ConfigurationSettings.AppSettings["FromDate"]);
            DateTime dtToDate = Convert.ToDateTime(System.Configuration.ConfigurationSettings.AppSettings["ToDate"]);
            string sLegalOrg = System.Configuration.ConfigurationSettings.AppSettings["LegalOrg"];

            string GetDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(GetDirectory);

            string sPhysicianXmlPath = GetDirectory + "\\ConfigXML\\PhysicianList.xml";
            XmlDocument itemPhysiciandoc = new XmlDocument();
            XmlTextReader XmlPhysicianText = new XmlTextReader(sPhysicianXmlPath);
            itemPhysiciandoc.Load(XmlPhysicianText);

            Console.WriteLine("Getting CQM Measures List");

            ilstPQRIMeasure = pqriMeasureMngr.GetPQRIMeasureDetails();

            Console.WriteLine("Deleting existing CQM data");

            CQMSummaryManager cqmSummaryMngr = new CQMSummaryManager();
            cqmSummaryMngr.DeleteCQMData();

            ulong ulPhyID = 0;
            XmlNodeList xmlphy = itemPhysiciandoc.GetElementsByTagName("Physician");
            Console.WriteLine("Total Physician Count: " + xmlphy.Count.ToString());
            for (int iCount=0;iCount< xmlphy.Count;iCount++)
            {
                Console.Write("Filling the CQM Data for Physician ID : " + xmlphy[iCount].Attributes[0].Value.ToString());
                ulPhyID = Convert.ToUInt64(xmlphy[iCount].Attributes[0].Value);
                pqriMeasureMngr.FillPQRIMeasureCalculator(sLegalOrg, ulPhyID, dtFromDate, dtToDate, ilstPQRIMeasure);
            }
        }
    }
}
