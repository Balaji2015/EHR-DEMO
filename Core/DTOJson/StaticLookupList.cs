using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acurus.Capella.Core.DTOJson
{
    public class AssessmentStatusDefaulted
    {
        public string value { get; set; }
    }

    public class AssessmentStatusDefaultList
    {
        public string Name { get; set; }
        public string value { get; set; }
    }

    public class AssessmentStatusList
    {
        public string Name { get; set; }
        public string value { get; set; }
        public string is_required { get; set; }
    }

    public class BpStatusBasedFollowUpValueList
    {
        public string Field_Name { get; set; }
        public string Type { get; set; }
        public string value { get; set; }
    }

    public class BulkExportSchedulerpath
    {
        public string value { get; set; }
        public string Description { get; set; }
    }

    public class CategoryList
    {
        public string Field_Name { get; set; }
        public string value { get; set; }
        public string Sort_Order { get; set; }
    }

    public class CptsnomedList
    {
        public string cpt { get; set; }
        public string snomedcode { get; set; }
    }

    public class DemographicsList
    {
        public string Field_Name { get; set; }
        public string value { get; set; }
        public string Description { get; set; }
        public string Sort_Order { get; set; }
        public string Default_Value { get; set; }
    }

    public class FlowSheetList
    {
        public string Field_Name { get; set; }
        public string value { get; set; }
        public string intervals { get; set; }
        public string Ydisplayformat { get; set; }
        public string YDisplayText { get; set; }
        public string xDisplayFormat { get; set; }
        public string xDisplayText { get; set; }
        public string valueSets { get; set; }
    }

    public class FlowSheetPeriodList
    {
        public string value { get; set; }
    }

    public class FollowupForBPStatusList
    {
        public string FollowupForBPStatus { get; set; }
    }

    public class FollowupList
    {
        public string Field_Name { get; set; }
        public string value { get; set; }
        public string Description { get; set; }
        public string Sort_Order { get; set; }
    }

    public class FoodAllergySnomedList
    {
        public string Field_Name { get; set; }
        public string value { get; set; }
        public string Description { get; set; }
        public string Sort_Order { get; set; }
    }

    public class HelpMenu
    {
        public string Name { get; set; }
        public string ReferenceLink { get; set; }
    }

    public class HelpMenuList
    {
        public HelpMenu HelpMenu { get; set; }
    }

    public class HospitalizationHistoryList
    {
        public string Field_Name { get; set; }
        public string value { get; set; }
    }

    public class MammogramTypeList
    {
        public string Field_Name { get; set; }
        public string value { get; set; }
        public string Description { get; set; }
        public string Sort_Order { get; set; }
    }

    public class MedicationReasonNotPerformedList
    {
        public string Field_Name { get; set; }
        public string value { get; set; }
        public string Description { get; set; }
        public string Sort_Order { get; set; }
    }

    public class Procedurecodelist
    {
        public string Code { get; set; }
        public string snomedcode { get; set; }
    }

    public class ReasonNotPerformedList
    {
        public string Field_Name { get; set; }
        public string value { get; set; }
        public string Description { get; set; }
        public string Sort_Order { get; set; }
    }

    public class StaticLookupList
    {
        public StaticLookupList()
        {
            VitalList = new List<VitalList>();
            ReasonNotPerformedList = new List<ReasonNotPerformedList>();
            FollowupList = new List<FollowupList>();
            MammogramTypeList = new List<MammogramTypeList>();
            FoodAllergySnomedList = new List<FoodAllergySnomedList>();
            BpStatusBasedFollowUpValueList = new List<BpStatusBasedFollowUpValueList>();
            FlowSheetList = new List<FlowSheetList>();
            FlowSheetPeriodList = new List<FlowSheetPeriodList>();
            DemographicsList = new List<DemographicsList>();
            AssessmentStatusList = new List<AssessmentStatusList>();
            AssessmentStatusDefaultList = new List<AssessmentStatusDefaultList>();
            procedurecodelist = new List<Procedurecodelist>();
            HospitalizationHistoryList = new List<HospitalizationHistoryList>();
            cptsnomedList = new List<CptsnomedList>();
            BulkExportSchedulerpath = new List<BulkExportSchedulerpath>();
            ErrorFileNamesList = new List<string>();
            RxHistoryList = new List<RxHistoryList>();
            MedicationReasonNotPerformedList = new List<MedicationReasonNotPerformedList>();
            SpecialtyList = new List<SpecialtyList>();
            CategoryList = new List<CategoryList>();
            ServiceTypeList = new List<ServiceTypeList>();
            ServiceTypeSelectionList = new List<ServiceTypeSelectionList>();

            StaticLookUpList = new StaticLookUplst();
            HelpMenuList = new HelpMenuList();
            FollowupForBPStatusList = new FollowupForBPStatusList();            FaxFolderNameList = new List<FaxFolderNameList>();
        }
        public List<VitalList> VitalList { get; set; }
        public List<ReasonNotPerformedList> ReasonNotPerformedList { get; set; }
        public List<FollowupList> FollowupList { get; set; }
        public List<MammogramTypeList> MammogramTypeList { get; set; }
        public List<FoodAllergySnomedList> FoodAllergySnomedList { get; set; }
        public List<BpStatusBasedFollowUpValueList> BpStatusBasedFollowUpValueList { get; set; }
        public List<FlowSheetList> FlowSheetList { get; set; }
        public List<FlowSheetPeriodList> FlowSheetPeriodList { get; set; }
        public List<DemographicsList> DemographicsList { get; set; }
        public AssessmentStatusDefaulted AssessmentStatusDefaulted { get; set; }
        public List<AssessmentStatusList> AssessmentStatusList { get; set; }
        public List<AssessmentStatusDefaultList> AssessmentStatusDefaultList { get; set; }
        public List<Procedurecodelist> procedurecodelist { get; set; }
        public List<HospitalizationHistoryList> HospitalizationHistoryList { get; set; }
        public List<CptsnomedList> cptsnomedList { get; set; }
        public List<BulkExportSchedulerpath> BulkExportSchedulerpath { get; set; }
        public List<string> ErrorFileNamesList { get; set; }
        public List<RxHistoryList> RxHistoryList { get; set; }
        public StaticLookUplst StaticLookUpList { get; set; }
        public List<MedicationReasonNotPerformedList> MedicationReasonNotPerformedList { get; set; }
        public FollowupForBPStatusList FollowupForBPStatusList { get; set; }
        public List<SpecialtyList> SpecialtyList { get; set; }
        public List<CategoryList> CategoryList { get; set; }
        public List<ServiceTypeList> ServiceTypeList { get; set; }
        public List<ServiceTypeSelectionList> ServiceTypeSelectionList { get; set; }
        public List<FaxFolderNameList> FaxFolderNameList { get; set; }
        public HelpMenuList HelpMenuList { get; set; }
    }

    public class FaxFolderNameList
    {
        public string FolderName { get; set; }
    }

    public class RxHistoryList
    {
        public string Field_Name { get; set; }
        public string value { get; set; }
        public string Description { get; set; }
        public string Sort_Order { get; set; }
    }

    public class ServiceTypeList
    {
        public string Type { get; set; }
        public string Code { get; set; }
    }

    public class ServiceTypeSelectionList
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string ServiceTypeCode { get; set; }
        public string Payer { get; set; }
    }

    public class SpecialtyList
    {
        public string Field_Name { get; set; }
        public string value { get; set; }
        public string Sort_Order { get; set; }
    }

    public class StaticLookUp
    {
        public string Name { get; set; }
        public string value { get; set; }
        public string Description { get; set; }
        public string Sort_Order { get; set; }
    }

    public class StaticLookUplst
    {
        public List<StaticLookUp> StaticLookUp { get; set; }

        //[JsonProperty("#text")]
        public string text { get; set; }
    }

    public class VitalList
    {
        public string Name { get; set; }
        public string value { get; set; }
        public string Description { get; set; }
    }


}
