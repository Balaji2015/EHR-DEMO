

namespace Acurus.Capella.DataAccess.ManagerObjects
{
    public interface IManagerFactory
    {
        // Get Methods
        //IFieldLookupManager GetFieldLookupManager();
        //IFieldLookupManager GetFieldLookupManager(INHibernateSession session);



        IContextManager GetContextManager();
        IContextManager GetContextManager(INHibernateSession session);

        IRegistryLogManager GetRegistryLogManager();
        IRegistryLogManager GetRegistryLogManager(INHibernateSession session);

        IQuest_Lab_Response_LogManager GetQuest_Lab_Response_LogManager();
        IQuest_Lab_Response_LogManager GetQuest_Lab_Response_LogManager(INHibernateSession session);

        IStaticLookupManager GetStaticLookupManager();
        IStaticLookupManager GetStaticLookupManager(INHibernateSession session);

        IUserLookupManager GetUserookupManager();
        IUserLookupManager GetUserookupManager(INHibernateSession session);

        IChiefComplaintManager GetChiefComplaintManager();
        IChiefComplaintManager GetChiefComplaintManager(INHibernateSession session);

        IExaminationManager GetExaminationManager();
        IExaminationManager GetExaminationManager(INHibernateSession session);

        IFacilityManager GetFacilityManager();
        IFacilityManager GetFacilityManager(INHibernateSession session);

        ILoginManager GetLoginManager();
        ILoginManager GetLoginManager(INHibernateSession session);

        IROSManager GetROSManager();
        IROSManager GetROSManager(INHibernateSession session);

        IVitalsManager GetVitalsManager();
        IVitalsManager GetVitalsManager(INHibernateSession session);

        IAssessmentManager GetAssessmentManager();
        IAssessmentManager GetAssessmentManager(INHibernateSession session);

        IHumanMananger GetHumanManager();
        IHumanMananger GetHumanManager(INHibernateSession session);

        IHuman_TokenManager GetHuman_TokenManager();
        IHuman_TokenManager GetHuman_TokenManager(INHibernateSession session);

        IEligibility_VerficationManager GetEligibility_Verification();
        IEligibility_VerficationManager GetEligibility_Verification(INHibernateSession session);

        IEncounterManager GetEncounterManager();
        IEncounterManager GetEncounterManager(INHibernateSession session);

        IPhysicianICD_9Manager GetPhysician_ICD_9Manager();
        IPhysicianICD_9Manager GetPhysician_ICD_9Manager(INHibernateSession session);

        IFamilyHistoryManager GetFamilyHistoryManager();
        IFamilyHistoryManager GetFamilyHistoryManager(INHibernateSession session);
        
        IFamilyHistoryMasterManager GetFamilyHistoryMasterManager();
        IFamilyHistoryMasterManager GetFamilyHistoryMasterManager(INHibernateSession session);

        IFamilyDiseaseManager GetFamilyDiseaseManager();
        IFamilyDiseaseManager GetFamilyDiseaseManager(INHibernateSession session);

        IFamilyDiseaseMasterManager GetFamilyDiseaseMasterManager();
        IFamilyDiseaseMasterManager GetFamilyDiseaseMasterManager(INHibernateSession session);

        ISocialHistoryManager GetSocialHistoryManager();
        ISocialHistoryManager GetSocialHistoryManager(INHibernateSession session);

        ISocialHistoryMasterManager GetSocialHistoryMasterManager();
        ISocialHistoryMasterManager GetSocialHistoryMasterManager(INHibernateSession session);


        IAllICD_9Manager GetAllICD_9Manager();
        IAllICD_9Manager GetAllICD_9Manager(INHibernateSession session);

        IPatientInsuredPlanManager GetPatientInsuredPlanManager();
        IPatientInsuredPlanManager GetPatientInsuredPlanManager(INHibernateSession session);

        IInsurancePlanManager GetInsurancePlanManager();
        IInsurancePlanManager GetInsurancePlanManager(INHibernateSession session);

        IPhysicianManager GetPhysicianManager();
        IPhysicianManager GetPhysicianManager(INHibernateSession session);

        IPastMedicalHistoryManager GetPastMedicalHistoryManager();
        IPastMedicalHistoryManager GetPastMedicalHistoryManager(INHibernateSession session);

        IPastMedicalHistoryMasterManager GetPastMedicalHistoryMasterManager();
        IPastMedicalHistoryMasterManager GetPastMedicalHistoryMasterManager(INHibernateSession session);

        INonDrugAllergyManager GetNonDrugAllergyManager();
        INonDrugAllergyManager GetNonDrugAllergyManager(INHibernateSession session);

        INonDrugAllergyMasterManager GetNonDrugAllergyMasterManager();
        INonDrugAllergyMasterManager GetNonDrugAllergyMasterManager(INHibernateSession session);

        ISurgicalHistoryManager GetSurgicalHistoryManager();
        ISurgicalHistoryManager GetSurgicalHistoryManager(INHibernateSession session);

        ISurgicalHistoryMasterManager GetSurgicalHistoryMasterManager();
        ISurgicalHistoryMasterManager GetSurgicalHistoryMasterManager(INHibernateSession session);

        IHospitalizationHistoryManager GetHospitalizationManager();
        IHospitalizationHistoryManager GetHospitalizationManager(INHibernateSession session);

        IHospitalizationHistoryMasterManager GetHospitalizationMasterManager();
        IHospitalizationHistoryMasterManager GetHospitalizationMasterManager(INHibernateSession session);

        ILabManager GetAllLabManager();
        ILabManager GetAllLabManager(INHibernateSession session);

        ILabLocationManager GetAllLabLocationManager();
        ILabLocationManager GetAllLabLocationManager(INHibernateSession session);

        IBlockdaysManager GetBlockdaysManager();
        IBlockdaysManager GetBlockdaysManager(INHibernateSession session);


        IPhysicianPOVManager GetPhysicianIntermediateManager();
        IPhysicianPOVManager GetPhysicianIntermediateManager(INHibernateSession session);

        IProcessMasterManager GetProcessMasterManager();
        IProcessMasterManager GetProcessMasterManager(INHibernateSession session);

        IWorkFlowManager GetWorkFlowMapManager();
        IWorkFlowManager GetWorkFlowMapManager(INHibernateSession session);

        IWFObjectManager GetWFObjectManager();
        IWFObjectManager GetWFObjectManager(INHibernateSession session);

        IStateManager GetStateManager();
        IStateManager GetStateManager(INHibernateSession session);

        IChargeHeaderManager GetChargeHeaderManager();
        IChargeHeaderManager GetChargeHeaderManager(INHibernateSession session);

        IWorkFlowTypeMasterManager GetWorkFlowTypeMasterManager();
        IWorkFlowTypeMasterManager GetWorkFlowTypeMasterManager(INHibernateSession session);

        IProblemListManager GetProblemListManager();
        IProblemListManager GetProblemListManager(INHibernateSession session);

        IObjectProcessHistoryManager GetObjectProcessHistoryManager();
        IObjectProcessHistoryManager GetObjectProcessHistoryManager(INHibernateSession session);

        IProcUserManager GetProcUserManager();
        IProcUserManager GetProcUserManager(INHibernateSession session);

        IObjectMasterManager GetObjectMasterManager();
        IObjectMasterManager GetObjectMasterManager(INHibernateSession session);

        IScnTabManager GetScnTabManager();
        IScnTabManager GetScnTabManager(INHibernateSession session);

        IElementManager GetElementManager();
        IElementManager GetElementManager(INHibernateSession session);

        IEAndMCodingManager GetEAndCodingManager();
        IEAndMCodingManager GetEAndCodingManager(INHibernateSession session);

        IEAndMCodingICDManager GetEAndCodingICDManager();
        IEAndMCodingICDManager GetEAndCodingICDManager(INHibernateSession session);

        ICreateExceptionManager GetCreateExceptionManager();
        ICreateExceptionManager GetCreateExceptionManager(INHibernateSession session);

        IAssociatedPrimaryICDManager GetAssociatedPrimaryICDManager();
        IAssociatedPrimaryICDManager GetAssociatedPrimaryICDManager(INHibernateSession session);

        IUserSessionManager GetUserSessionManager();
        IUserSessionManager GetUserSessionManager(INHibernateSession session);

        ITreatmentPlanManager GetTreatmentPlanManager();
        ITreatmentPlanManager GetTreatmentPlanManager(INHibernateSession session);

        IAll_DrugManager GetAll_DrugManager();
        IAll_DrugManager GetAll_DrugManager(INHibernateSession session);

        IExamLookupManager GetExamLookupManager();
        IExamLookupManager GetExamLookupManager(INHibernateSession session);

        ICarrierManager GetCarrierManager();
        ICarrierManager GetCarrierManager(INHibernateSession session);

        IProcedureCodeLibraryManager GetProcedureCodeLibraryManager();
        IProcedureCodeLibraryManager GetProcedureCodeLibraryManager(INHibernateSession session);

        IFinancialClassesManager GetFinancialClassesManager();
        IFinancialClassesManager GetFinancialClassesManager(INHibernateSession session);

        ICorrectedDBEntriesManager GetCorrectedDBEntriesManager();
        ICorrectedDBEntriesManager GetCorrectedDBEntriesManager(INHibernateSession session);

        IVisitPaymentManager GetVisitPaymentManager();
        IVisitPaymentManager GetVisitPaymentManager(INHibernateSession session);

        IVisitPaymentArcManager GetVisitPaymentArcManager();
        IVisitPaymentArcManager GetVisitPaymentArcManager(INHibernateSession session);

        IVisitPaymentHistoryManager GetVisitPaymentHistoryManager();
        IVisitPaymentHistoryManager GetVisitPaymentHistoryManager(INHibernateSession session);

        IVisitPaymentHistoryArcManager GetVisitPaymentHistoryArcManager();
        IVisitPaymentHistoryArcManager GetVisitPaymentHistoryArcManager(INHibernateSession session);

        IRoomInLookupManager GetRoomInLookupManager();
        IRoomInLookupManager GetRoomInLookupManager(INHibernateSession session);

        IAccountTransactionManager GetAccountTransactionManager();
        IAccountTransactionManager GetAccountTransactionManager(INHibernateSession session);

        IAccountTransactionArcManager GetAccountTransactionArcManager();
        IAccountTransactionArcManager GetAccountTransactionArcManager(INHibernateSession session);

        IMessageManager GetMessageManager();
        IMessageManager GetMessageManager(INHibernateSession session);

        IOrdersManager GetOrdersManager();
        IOrdersManager GetOrdersManager(INHibernateSession session);

        IReferralOrderManager GetReferralOrderManager();
        IReferralOrderManager GetReferralOrderManager(INHibernateSession session);

        IPhysicianProcedureManager GetPhysicianProcedureManager();
        IPhysicianProcedureManager GetPhysicianProcedureManager(INHibernateSession session);

        IProcedureAuthorizationManager GetProcedureAuthorizationManager();
        IProcedureAuthorizationManager GetProcedureAuthorizationManager(INHibernateSession session);

        IOrdersAssessmentManager GetOrdersAssessmentManager();
        IOrdersAssessmentManager GetOrdersAssessmentManager(INHibernateSession session);

        IPPHeaderManager GetPPHeaderManager();
        IPPHeaderManager GetPPHeaderManager(INHibernateSession session);

        IPPHeaderArcManager GetPPHeaderArcManager();
        IPPHeaderArcManager GetPPHeaderArcManager(INHibernateSession session);

        IPPLineItemManager GetPPLineItemManager();
        IPPLineItemManager GetPPLineItemManager(INHibernateSession session);

        IPPLineItemArcManager GetPPLineItemArcManager();
        IPPLineItemArcManager GetPPLineItemArcManager(INHibernateSession session);

        ICheckManager GetCheckManager();
        ICheckManager GetCheckManager(INHibernateSession session);

        ICheckArcManager GetCheckArcManager();
        ICheckArcManager GetCheckArcManager(INHibernateSession session);

        IScan_IndexManager GetScanIndexManager();
        IScan_IndexManager GetScanIndexManager(INHibernateSession session);

        ILabInsurancePlanManager GetLabInsurancePlanManager();
        ILabInsurancePlanManager GetLabInsurancePlanManager(INHibernateSession session);

        IGrowthChartLookupManager GetGrowthChartLookupManager();
        IGrowthChartLookupManager GetGrowthChartLookupManager(INHibernateSession session);

        IFlowSheetTemplateManager GetFlowSheetTemplateManager();
        IFlowSheetTemplateManager GetFlowSheetTemplateManager(INHibernateSession session);



        IMasterTableListManager GetMasterTableListManager();
        IMasterTableListManager GetMasterTableListManager(INHibernateSession session);


        IScanManager GetScanManager();
        IScanManager GetScanManager(INHibernateSession session);

        IGeneralNotesManager GetGeneralNotesManager();
        IGeneralNotesManager GetGeneralNotesManager(INHibernateSession session);

        IAuditLogManager GetAuditLogManager();
        IAuditLogManager GetAuditLogManager(INHibernateSession session);

        IOrdersProblemListManager GetOrdersProblemListManager();
        IOrdersProblemListManager GetOrdersProblemListManager(INHibernateSession session);

        IReferralOrdersAssessmentManager GetReferralOrdersAssessmentManager();
        IReferralOrdersAssessmentManager GetReferralOrdersAssessmentManager(INHibernateSession session);

        IOrderCodeLibraryManager GetOrderCodeLibraryManager();
        IOrderCodeLibraryManager GetOrderCodeLibraryManager(INHibernateSession session);

        ICarePlanLookupManager GetCarePlanLookupManager();
        ICarePlanLookupManager GetCarePlanLookupManager(INHibernateSession session);

        ICarePlanManager GetCarePlanManager();
        ICarePlanManager GetCarePlanManager(INHibernateSession session);

        IPreventiveScreenLookupManager GetPreventiveScreenLookupManager();
        IPreventiveScreenLookupManager GetPreventiveScreenLookupManager(INHibernateSession session);

        IPreventiveScreenManager GetPreventiveScreenManager();
        IPreventiveScreenManager GetPreventiveScreenManager(INHibernateSession session);

        IWorksetProcAllocManager GetWorksetProcAllocManager();
        IWorksetProcAllocManager GetWorksetProcAllocManager(INHibernateSession session);

        IPhysicianPatientManager GetPhysicianPatientManager();
        IPhysicianPatientManager GetPhysicianPatientManager(INHibernateSession session);

        IPhysicianPatientMasterManager GetPhysicianPatientMasterManager();
        IPhysicianPatientMasterManager GetPhysicianPatientMasterManager(INHibernateSession session);

        IAdvanceDirectiveManager GetAdvanceDirectiveManager();
        IAdvanceDirectiveManager GetAdvanceDirectiveManager(INHibernateSession session);


        IAdvanceDirectiveMasterManager GetAdvanceDirectiveMasterManager();
        IAdvanceDirectiveMasterManager GetAdvanceDirectiveMasterManager(INHibernateSession session);


        IAppointmentLookupManager GetAppointmentLookupManager();
        IAppointmentLookupManager GetAppointmentLookupManager(INHibernateSession session);

        IAssessmentVitalsLookupManager GetAssessmentVitalsLookupManager();
        IAssessmentVitalsLookupManager GetAssessmentVitalsLookupManager(INHibernateSession session);

        IMapPhysicianPhysicianAssitantManager GetMapPhysicianPhysicianAssistantManager();
        IMapPhysicianPhysicianAssitantManager GetMapPhysicianPhysicianAssistantManager(INHibernateSession session);

        IOrdersSubmitManager GetOrdersSubmitManager();
        IOrdersSubmitManager GetOrdersSubmitManager(INHibernateSession session);

        IImmunizationHistoryManager GetImmunizationHistoryManager();
        IImmunizationHistoryManager GetImmunizationHistoryManager(INHibernateSession session);

        IImmunizationMasterHistoryManager GetImmunizationMasterHistoryManager();
        IImmunizationMasterHistoryManager GetImmunizationMasterHistoryManager(INHibernateSession session);

        ITestLookupManager GetTestLookupManager();
        ITestLookupManager GetTestLookupManager(INHibernateSession session);

        IPQRI_MeasureManager GetPQRIMeasureManager();
        IPQRI_MeasureManager GetPQRIMeasureManager(INHibernateSession session);

        IPQRI_DataManager GetPQRIDataManager();
        IPQRI_DataManager GetPQRIDataManager(INHibernateSession session);

        IPQRIManager GetPQRIManager();
        IPQRIManager GetPQRIManager(INHibernateSession session);

        IOrdersQuestionSetAfpManager GetOrdersQuestionSetAfpManager();
        IOrdersQuestionSetAfpManager GetOrdersQuestionSetAfpManager(INHibernateSession session);

        IOrdersQuestionSetBloodLeadManager GetOrdersQuestionSetBloodLeadManager();
        IOrdersQuestionSetBloodLeadManager GetOrdersQuestionSetBloodLeadManager(INHibernateSession session);

        IOrdersQuestionSetCytologyManager GetOrdersQuestionSetCytologyManager();
        IOrdersQuestionSetCytologyManager GetOrdersQuestionSetCytologyManager(INHibernateSession session);

        IFileManagementIndexManager GetFileManagementIndexManager();
        IFileManagementIndexManager GetFileManagementIndexManager(INHibernateSession session);

        IHealthcareQuestionnaireManager GetHealthcareQuestionnaireManager();
        IHealthcareQuestionnaireManager GetHealthcareQuestionnaireManager(INHibernateSession session);

        IQuestionnaireLookupManager GetQuestionnaireLookupManager();
        IQuestionnaireLookupManager GetQuestionnaireLookupManager(INHibernateSession session);

        IResultLookupManager GetResultLookupManager();
        IResultLookupManager GetResultLookupManager(INHibernateSession session);

        IResultMasterManager GetResultMasterManager();
        IResultMasterManager GetResultMasterManager(INHibernateSession session);

        IResultORCManager GetResultORCManager();
        IResultORCManager GetResultORCManager(INHibernateSession session);

        IResultOBRManager GetResultOBRManager();
        IResultOBRManager GetResultOBRManager(INHibernateSession session);

        IResultOBXManager GetResultOBXManager();
        IResultOBXManager GetResultOBXManager(INHibernateSession session);

        IResultNTEManager GetResultNTEManager();
        IResultNTEManager GetResultNTEManager(INHibernateSession session);

        IResultZEFManager GetResultZEFManager();
        IResultZEFManager GetResultZEFManager(INHibernateSession session);

        IResultZPSManager GetResultZPSManager();
        IResultZPSManager GetResultZPSManager(INHibernateSession session);

        ITestManager GetTestManager();
        ITestManager GetTestManager(INHibernateSession session);

        IRcopia_MedicationManager GetRcopia_MedicationManager();
        IRcopia_MedicationManager GetRcopia_MedicationManager(INHibernateSession session);

        IRcopia_AllergyManager GetRcopia_AllergyManager();
        IRcopia_AllergyManager GetRcopia_AllergyManager(INHibernateSession session);

        IRcopia_Prescription_ListManager GetRcopia_Prescription_ListManager();
        IRcopia_Prescription_ListManager GetRcopia_Prescription_ListManager(INHibernateSession session);

        IRcopia_SettingsManager GetRcopia_SettingsManager();
        IRcopia_SettingsManager GetRcopia_SettingsManager(INHibernateSession session);

        ILabcorpSettingsManager GetLabcorpSettingsManager();
        ILabcorpSettingsManager GetLabcorpSettingsManager(INHibernateSession session);

        IRcopia_Update_InfoManager GetRcopia_Update_InfoManager();
        IRcopia_Update_InfoManager GetRcopia_Update_InfoManager(INHibernateSession session);

        IVaccineInfoStatementManager GetVaccineInfoStatementManager();
        IVaccineInfoStatementManager GetVaccineInfoStatementManager(INHibernateSession session);

        IVaccineInfoStatementProcedureManager GetVaccineInfoStatementProcedureManager();
        IVaccineInfoStatementProcedureManager GetVaccineInfoStatementProcedureManager(INHibernateSession session);

        IPhysicianSpecialtyManager GetPhysicianSpecialtyManager();
        IPhysicianSpecialtyManager GetPhysicianSpecialtyManager(INHibernateSession session);

        ILabCarrierLookUpManager GetLabCarrierLookUpManager();
        ILabCarrierLookUpManager GetLabCarrierLookUpManager(INHibernateSession session);

        IDocumentManager GetDocumentManager();
        IDocumentManager GetDocumentManager(INHibernateSession session);

        IVaccineManufacturerCodesManager GetVaccineManufacturerCodesManager();
        IVaccineManufacturerCodesManager GetVaccineManufacturerCodesManager(INHibernateSession session);

        IRuleMedicationManager GetRuleMedicationManager();
        IRuleMedicationManager GetRuleMedicationManager(INHibernateSession session);

        IRuleMasterManager GetRuleMasterManager();
        IRuleMasterManager GetRuleMasterManager(INHibernateSession session);

        IRuleProblemManager GetRuleProblemManager();
        IRuleProblemManager GetRuleProblemManager(INHibernateSession session);

        IRuleMedicationAndAllergyManager GetRuleMedicationAndAllergyManager();
        IRuleMedicationAndAllergyManager GetRuleMedicationAndAllergyManager(INHibernateSession session);

        IRuleLabResultReminderManager GetRuleLabResultReminderManager();
        IRuleLabResultReminderManager GetRuleLabResultReminderManager(INHibernateSession session);

        IPrescriptionManager GetPrescriptionManager();
        IPrescriptionManager GetPrescriptionManager(INHibernateSession session);

        IMasterVitalsManager GetMasterVitalsManager();
        IMasterVitalsManager GetMasterVitalsManager(INHibernateSession session);

        IMapVitalsPhysicianManager GetMapVitalsPhysicianManager();
        IMapVitalsPhysicianManager GetMapVitalsPhysicianManager(INHibernateSession session);

        IPercentileLookUpManager GetPercentileLookUpManager();
        IPercentileLookUpManager GetPercentileLookUpManager(INHibernateSession session);

        IInHouseProcedureManager GetInHouseProcedureManager();
        IInHouseProcedureManager GetInHouseProcedureManager(INHibernateSession session);

        ISpirometryManager GetSpirometryManager();
        ISpirometryManager GetSpirometryManager(INHibernateSession session);



        IABI_ResultsManager GetAbiManager();
        IABI_ResultsManager GetAbiManager(INHibernateSession session);

        IPhysician_DrugManager GetPhysician_DrugManager();
        IPhysician_DrugManager GetPhysician_DrugManager(INHibernateSession session);

        IProcedureCodeRuleMasterManager GetProcedureCodeRuleMasterManager();
        IProcedureCodeRuleMasterManager GetProcedureCodeRuleMasterManager(INHibernateSession session);

        IAOELookUpManager AOELookUpManager();
        IAOELookUpManager AOELookUpManager(INHibernateSession session);

        IOrdersQuestionSetAOEManager OrdersQuestionSetAOEManager();
        IOrdersQuestionSetAOEManager OrdersQuestionSetAOEManager(INHibernateSession session);

        IOrderComponentsManager OrderComponentsManager();
        IOrderComponentsManager OrderComponentsManager(INHibernateSession session);

        IAddendumNotesManager GetAddendumNotesManager();
        IAddendumNotesManager GetAddendumNotesManager(INHibernateSession session);


        IPhysicianResultsManager GetPhysicianResultsManager();
        IPhysicianResultsManager GetPhysicianResultsManager(INHibernateSession session);

        IAcurusResultsMappingManager GetAcurusResultsMappingManager();
        IAcurusResultsMappingManager GetAcurusResultsMappingManager(INHibernateSession session);

        IOrdersRequiredFormsManager GetOrdersRequiredFormsManager();
        IOrdersRequiredFormsManager GetOrdersRequiredFormsManager(INHibernateSession session);

        IDictationExceptionManager GetDictationExceptionManager();
        IDictationExceptionManager GetDictationExceptionManager(INHibernateSession session);

        IUserScnTabManager GetUserScnTabManager();
        IUserScnTabManager GetUserScnTabManager(INHibernateSession session);

        IErrorLogManager GetErrorLogManager();
        IErrorLogManager GetErrorLogManager(INHibernateSession session);

        IMeasuresRuleMasterManager GetMeasuresRuleMasterManager();
        IMeasuresRuleMasterManager GetMeasuresRuleMasterManager(INHibernateSession session);

        IActivityLogManager GetActivityLogManager();
        IActivityLogManager GetActivityLogManager(INHibernateSession session);

        IRegisteredNetworkManager GetRegisteredNetworkManager();
        IRegisteredNetworkManager GetRegisteredNetworkManager(INHibernateSession session);

        ICDSRuleMasterManager GetCDSRuleMasterManager();
        ICDSRuleMasterManager GetCDSRuleMasterManager(INHibernateSession session);

        INotificationManager GetNotificationManager();
        INotificationManager GetNotificationManager(INHibernateSession session);

        IPotentialDiagnosisManager GetPotentialDiagnosisManager();
        IPotentialDiagnosisManager GetPotentialDiagnosisManager(INHibernateSession session);

        IProviderReviewTrackerManager GetProviderReviewTrackerManager();
        IProviderReviewTrackerManager GetProviderReviewTrackerManager(INHibernateSession session);

        IRequestSentLogManager GetRequestSentLogManager();
        IRequestSentLogManager GetRequestSentLogManager(INHibernateSession session);

        IResponseReceivedLogManager GetResponseReceivedLogManager();
        IResponseReceivedLogManager GetResponseReceivedLogManager(INHibernateSession session);

        IVaccineAdminProcedureMappingManager GetVaccineAdminProcedureMappingManager();
        IVaccineAdminProcedureMappingManager GetVaccineAdminProcedureMappingManager(INHibernateSession session);

        IClientManager GetClientManager();
        IClientManager GetClientManager(INHibernateSession session);

        IHumanBlobManager GetHumanBlobManager();
        IHumanBlobManager GetHumanBlobManager(INHibernateSession session);

        IEncounterBlobManager GetEncounterBlobManager();
        IEncounterBlobManager GetEncounterBlobManager(INHibernateSession session);

        IMapXMLBlobManager GetMapXMLBlobManager();
        IMapXMLBlobManager GetMapXMLBlobManager(INHibernateSession session);
        IProcedureModifierLookupManager GetProcedureModifierLookupManager();
        IProcedureModifierLookupManager GetProcedureModifierLookupManager(INHibernateSession session);

        IRCopiaDeduplicateLogManager GetRCopiaDeduplicateLogManager();
        IRCopiaDeduplicateLogManager GetRCopiaDeduplicateLogManager(INHibernateSession session);

        IBlobProgressNoteManager GetBlobProgressNoteManager();
        IBlobProgressNoteManager GetBlobProgressNoteManager(INHibernateSession session);

        ICDCEventTrackerManager GetCDCEventTrackerManager();
        ICDCEventTrackerManager GetCDCEventTrackerManager(INHibernateSession session);
    }

    public class ManagerFactory : IManagerFactory
    {
        #region Constructors

        public ManagerFactory()
        {
        }

        #endregion


        #region Get Methods

        public IVaccineAdminProcedureMappingManager GetVaccineAdminProcedureMappingManager()
        {
            return new VaccineAdminProcedureMappingManager();
        }
        public IVaccineAdminProcedureMappingManager GetVaccineAdminProcedureMappingManager(INHibernateSession session)
        {
            return new VaccineAdminProcedureMappingManager(session);
        }



        public IStaticLookupManager GetStaticLookupManager()
        {
            return new StaticLookupManager();
        }
        public IStaticLookupManager GetStaticLookupManager(INHibernateSession session)
        {
            return new StaticLookupManager(session);
        }
        public IUserLookupManager GetUserookupManager()
        {
            return new UserLookupManager();
        }
        public IUserLookupManager GetUserookupManager(INHibernateSession session)
        {
            return new UserLookupManager(session);
        }

        public IWorksetProcAllocManager GetWorksetProcAllocManager()
        {
            return new WorksetProcAllocManager();
        }
        public IWorksetProcAllocManager GetWorksetProcAllocManager(INHibernateSession session)
        {
            return new WorksetProcAllocManager(session);
        }

        public IChiefComplaintManager GetChiefComplaintManager()
        {
            return new ChiefComplaintsManager();
        }
        public IChiefComplaintManager GetChiefComplaintManager(INHibernateSession session)
        {
            return new ChiefComplaintsManager(session);
        }

        public IExaminationManager GetExaminationManager()
        {
            return new ExaminationManager();
        }
        public IExaminationManager GetExaminationManager(INHibernateSession session)
        {
            return new ExaminationManager(session);
        }

        public IFacilityManager GetFacilityManager()
        {
            return new FacilityManager();
        }
        public IFacilityManager GetFacilityManager(INHibernateSession session)
        {
            return new FacilityManager(session);
        }

        public ILoginManager GetLoginManager()
        {
            return new UserManager();
        }
        public ILoginManager GetLoginManager(INHibernateSession session)
        {
            return new UserManager(session);
        }

        public IROSManager GetROSManager()
        {
            return new ROSManager();
        }
        public IROSManager GetROSManager(INHibernateSession session)
        {
            return new ROSManager(session);
        }

        public IVitalsManager GetVitalsManager()
        {
            return new VitalsManager();
        }
        public IVitalsManager GetVitalsManager(INHibernateSession session)
        {
            return new VitalsManager(session);
        }
        public IAssessmentManager GetAssessmentManager()
        {
            return new AssessmentManager();
        }
        public IAssessmentManager GetAssessmentManager(INHibernateSession session)
        {
            return new AssessmentManager(session);
        }

        public IHumanMananger GetHumanManager()
        {
            return new HumanManager();
        }
        public IHumanMananger GetHumanManager(INHibernateSession session)
        {
            return new HumanManager(session);
        }

        public IHuman_TokenManager GetHuman_TokenManager()
        {
            return new Human_TokenManager();
        }
        public IHuman_TokenManager GetHuman_TokenManager(INHibernateSession session)
        {
            return new Human_TokenManager(session);
        }

        public IEligibility_VerficationManager GetEligibility_Verification()
        {
            return new Eligibility_VerficationManager();
        }
        public IEligibility_VerficationManager GetEligibility_Verification(INHibernateSession session)
        {
            return new Eligibility_VerficationManager(session);
        }


        public IEncounterManager GetEncounterManager()
        {
            return new EncounterManager();
        }
        public IEncounterManager GetEncounterManager(INHibernateSession session)
        {
            return new EncounterManager(session);
        }

        public IPhysicianICD_9Manager GetPhysician_ICD_9Manager()
        {
            return new PhysicianICD_9Manager();
        }

        public IPhysicianICD_9Manager GetPhysician_ICD_9Manager(INHibernateSession session)
        {
            return new PhysicianICD_9Manager(session);
        }

        public IFamilyHistoryManager GetFamilyHistoryManager()
        {
            return new FamilyHistoryManager();
        }

        public IFamilyHistoryManager GetFamilyHistoryManager(INHibernateSession session)
        {
            return new FamilyHistoryManager(session);
        }

        public IFamilyHistoryMasterManager GetFamilyHistoryMasterManager()
        {
            return new FamilyHistoryMasterManager();
        }

        public IFamilyHistoryMasterManager GetFamilyHistoryMasterManager(INHibernateSession session)
        {
            return new FamilyHistoryMasterManager(session);
        }

        public IFamilyDiseaseManager GetFamilyDiseaseManager()
        {
            return new FamilyDiseaseManager();
        }

        public IFamilyDiseaseManager GetFamilyDiseaseManager(INHibernateSession session)
        {
            return new FamilyDiseaseManager(session);
        }

        public IFamilyDiseaseMasterManager GetFamilyDiseaseMasterManager()
        {
            return new FamilyDiseaseMasterManager();
        }

        public IFamilyDiseaseMasterManager GetFamilyDiseaseMasterManager(INHibernateSession session)
        {
            return new FamilyDiseaseMasterManager(session);
        }

        public ISocialHistoryManager GetSocialHistoryManager()
        {
            return new SocialHistoryManager();
        }
        public ISocialHistoryManager GetSocialHistoryManager(INHibernateSession session)
        {
            return new SocialHistoryManager(session);
        }

        public ISocialHistoryMasterManager GetSocialHistoryMasterManager()
        {
            return new SocialHistoryMasterManager();
        }
        public ISocialHistoryMasterManager GetSocialHistoryMasterManager(INHibernateSession session)
        {
            return new SocialHistoryMasterManager(session);
        }
        public IAllICD_9Manager GetAllICD_9Manager()
        {
            return new AllICD_9Manager();
        }
        public IAllICD_9Manager GetAllICD_9Manager(INHibernateSession session)
        {
            return new AllICD_9Manager(session);
        }


        public IInsurancePlanManager GetInsurancePlanManager()
        {
            return new InsurancePlanManager();
        }
        public IInsurancePlanManager GetInsurancePlanManager(INHibernateSession session)
        {
            return new InsurancePlanManager(session);
        }

        public IPatientInsuredPlanManager GetPatientInsuredPlanManager()
        {
            return new PatientInsuredPlanManager();
        }
        public IPatientInsuredPlanManager GetPatientInsuredPlanManager(INHibernateSession session)
        {
            return new PatientInsuredPlanManager(session);
        }

        public IPhysicianManager GetPhysicianManager()
        {
            return new PhysicianManager();
        }

        public IPhysicianManager GetPhysicianManager(INHibernateSession session)
        {
            return new PhysicianManager(session);
        }

        public IImmunizationManager GetimmunizationManager()
        {
            return new ImmunizationManager();
        }
        public IImmunizationManager GetimmunizationManager(INHibernateSession session)
        {
            return new ImmunizationManager(session);
        }

        public IPastMedicalHistoryManager GetPastMedicalHistoryManager()
        {
            return new PastMedicalHistoryManager();
        }
        public IPastMedicalHistoryManager GetPastMedicalHistoryManager(INHibernateSession session)
        {
            return new PastMedicalHistoryManager(session);
        }

        public IPastMedicalHistoryMasterManager GetPastMedicalHistoryMasterManager()
        {
            return new PastMedicalHistoryMasterManager();
        }
        public IPastMedicalHistoryMasterManager GetPastMedicalHistoryMasterManager(INHibernateSession session)
        {
            return new PastMedicalHistoryMasterManager(session);
        }

        public INonDrugAllergyManager GetNonDrugAllergyManager()
        {
            return new NonDrugAllergyManager();
        }
        public INonDrugAllergyManager GetNonDrugAllergyManager(INHibernateSession session)
        {
            return new NonDrugAllergyManager(session);
        }

        public INonDrugAllergyMasterManager GetNonDrugAllergyMasterManager()
        {
            return new NonDrugAllergyMasterManager();
        }
        public INonDrugAllergyMasterManager GetNonDrugAllergyMasterManager(INHibernateSession session)
        {
            return new NonDrugAllergyMasterManager(session);
        }

        public ISurgicalHistoryManager GetSurgicalHistoryManager()
        {
            return new SurgicalHistoryManager();
        }
        public ISurgicalHistoryManager GetSurgicalHistoryManager(INHibernateSession session)
        {
            return new SurgicalHistoryManager(session);
        }

        public ISurgicalHistoryMasterManager GetSurgicalHistoryMasterManager()
        {
            return new SurgicalHistoryMasterManager();
        }
        public ISurgicalHistoryMasterManager GetSurgicalHistoryMasterManager(INHibernateSession session)
        {
            return new SurgicalHistoryMasterManager(session);
        }



        public IHospitalizationHistoryManager GetHospitalizationManager()
        {
            return new HospitalizationHistoryManager();
        }
        public IHospitalizationHistoryManager GetHospitalizationManager(INHibernateSession session)
        {
            return new HospitalizationHistoryManager(session);
        }

        public IHospitalizationHistoryMasterManager GetHospitalizationMasterManager()
        {
            return new HospitalizationHistoryMasterManager();
        }
        public IHospitalizationHistoryMasterManager GetHospitalizationMasterManager(INHibernateSession session)
        {
            return new HospitalizationHistoryMasterManager(session);
        }

        public ILabManager GetAllLabManager()
        {
            return new LabManager();
        }
        public ILabManager GetAllLabManager(INHibernateSession session)
        {
            return new LabManager(session);
        }

        public ILabLocationManager GetAllLabLocationManager()
        {
            return new LabLocationManager();
        }
        public ILabLocationManager GetAllLabLocationManager(INHibernateSession session)
        {
            return new LabLocationManager(session);
        }

        public IBlockdaysManager GetBlockdaysManager()
        {
            return new BlockdaysManager();
        }
        public IBlockdaysManager GetBlockdaysManager(INHibernateSession session)
        {
            return new BlockdaysManager(session);
        }

        public IPhysicianPOVManager GetPhysicianIntermediateManager()
        {
            return new PhysicianPOVManager();
        }
        public IPhysicianPOVManager GetPhysicianIntermediateManager(INHibernateSession session)
        {
            return new PhysicianPOVManager(session);
        }

        public IProcessMasterManager GetProcessMasterManager()
        {
            return new ProcessMasterManager();
        }
        public IProcessMasterManager GetProcessMasterManager(INHibernateSession session)
        {
            return new ProcessMasterManager(session);
        }
        public IWorkFlowManager GetWorkFlowMapManager()
        {
            return new WorkFlowManager();
        }
        public IWorkFlowManager GetWorkFlowMapManager(INHibernateSession session)
        {
            return new WorkFlowManager(session);
        }
        public IWFObjectManager GetWFObjectManager()
        {
            return new WFObjectManager();
        }
        public IWFObjectManager GetWFObjectManager(INHibernateSession session)
        {
            return new WFObjectManager(session);
        }

        public IStateManager GetStateManager()
        {
            return new StateManager();
        }
        public IStateManager GetStateManager(INHibernateSession session)
        {
            return new StateManager(session);
        }

        public IProblemListManager GetProblemListManager()
        {
            return new ProblemListManager();
        }
        public IProblemListManager GetProblemListManager(INHibernateSession session)
        {
            return new ProblemListManager(session);
        }

        public IObjectProcessHistoryManager GetObjectProcessHistoryManager()
        {
            return new ObjectProcessHistoryManager();
        }
        public IObjectProcessHistoryManager GetObjectProcessHistoryManager(INHibernateSession session)
        {
            return new ObjectProcessHistoryManager(session);
        }

        public IProcUserManager GetProcUserManager()
        {
            return new ProcUserManager();
        }
        public IProcUserManager GetProcUserManager(INHibernateSession session)
        {
            return new ProcUserManager(session);
        }

        public IObjectMasterManager GetObjectMasterManager()
        {
            return new ObjectMasterManager();
        }
        public IObjectMasterManager GetObjectMasterManager(INHibernateSession session)
        {
            return new ObjectMasterManager(session);
        }

        public IScnTabManager GetScnTabManager()
        {
            return new ScnTabManager();
        }
        public IScnTabManager GetScnTabManager(INHibernateSession session)
        {
            return new ScnTabManager(session);
        }

        public IElementManager GetElementManager()
        {
            return new ElementManager();
        }
        public IElementManager GetElementManager(INHibernateSession session)
        {
            return new ElementManager(session);
        }

        public ICreateExceptionManager GetCreateExceptionManager()
        {
            return new CreateExceptionManager();
        }

        public ICreateExceptionManager GetCreateExceptionManager(INHibernateSession session)
        {
            return new CreateExceptionManager(session);
        }


        public IEAndMCodingManager GetEAndCodingManager()
        {
            return new EAndMCodingManager();
        }
        public IEAndMCodingManager GetEAndCodingManager(INHibernateSession session)
        {
            return new EAndMCodingManager(session);
        }

        public IEAndMCodingICDManager GetEAndCodingICDManager()
        {
            return new EandMCodingICDManager();
        }
        public IEAndMCodingICDManager GetEAndCodingICDManager(INHibernateSession session)
        {
            return new EandMCodingICDManager(session);
        }

        public IAssociatedPrimaryICDManager GetAssociatedPrimaryICDManager()
        {
            return new AssociatedPrimaryICDManager();
        }

        public IAssociatedPrimaryICDManager GetAssociatedPrimaryICDManager(INHibernateSession session)
        {
            return new AssociatedPrimaryICDManager(session);
        }

        public IUserSessionManager GetUserSessionManager()
        {
            return new UserSessionManager();
        }
        public IUserSessionManager GetUserSessionManager(INHibernateSession session)
        {
            return new UserSessionManager(session);
        }

        public ITreatmentPlanManager GetTreatmentPlanManager()
        {
            return new TreatmentPlanManager();
        }
        public ITreatmentPlanManager GetTreatmentPlanManager(INHibernateSession session)
        {
            return new TreatmentPlanManager(session);
        }

        public IAll_DrugManager GetAll_DrugManager()
        {
            return new All_DrugManager();
        }
        public IAll_DrugManager GetAll_DrugManager(INHibernateSession session)
        {
            return new All_DrugManager(session);
        }

        public IExamLookupManager GetExamLookupManager()
        {
            return new ExamLookupManager();
        }
        public IExamLookupManager GetExamLookupManager(INHibernateSession session)
        {
            return new ExamLookupManager(session);
        }


        public ICarrierManager GetCarrierManager()
        {
            return new CarrierManager();
        }
        public ICarrierManager GetCarrierManager(INHibernateSession session)
        {
            return new CarrierManager(session);
        }

        public IProcedureCodeLibraryManager GetProcedureCodeLibraryManager()
        {
            return new ProcedureCodeLibraryManager();
        }
        public IProcedureCodeLibraryManager GetProcedureCodeLibraryManager(INHibernateSession session)
        {
            return new ProcedureCodeLibraryManager(session);
        }



        public IFinancialClassesManager GetFinancialClassesManager()
        {
            return new FinancialClassesManager();
        }
        public IFinancialClassesManager GetFinancialClassesManager(INHibernateSession session)
        {
            return new FinancialClassesManager(session);
        }

        public ICorrectedDBEntriesManager GetCorrectedDBEntriesManager()
        {
            return new CorrectedDBEntriesManager();
        }
        public ICorrectedDBEntriesManager GetCorrectedDBEntriesManager(INHibernateSession session)
        {
            return new CorrectedDBEntriesManager(session);
        }


        public IVisitPaymentManager GetVisitPaymentManager()
        {
            return new VisitPaymentManager();
        }

        public IVisitPaymentManager GetVisitPaymentManager(INHibernateSession session)
        {
            return new VisitPaymentManager(session);
        }

        public IVisitPaymentArcManager GetVisitPaymentArcManager()
        {
            return new VisitPaymentArcManager();
        }

        public IVisitPaymentArcManager GetVisitPaymentArcManager(INHibernateSession session)
        {
            return new VisitPaymentArcManager(session);
        }

        public IVisitPaymentHistoryManager GetVisitPaymentHistoryManager()
        {
            return new VisitPaymentHistoryManager();
        }

        public IVisitPaymentHistoryManager GetVisitPaymentHistoryManager(INHibernateSession session)
        {
            return new VisitPaymentHistoryManager(session);
        }

        public IVisitPaymentHistoryArcManager GetVisitPaymentHistoryArcManager()
        {
            return new VisitPaymentHistoryArcManager();
        }

        public IVisitPaymentHistoryArcManager GetVisitPaymentHistoryArcManager(INHibernateSession session)
        {
            return new VisitPaymentHistoryArcManager(session);
        }









        public IRoomInLookupManager GetRoomInLookupManager()
        {
            return new RoomInLookupManager();
        }

        public IRoomInLookupManager GetRoomInLookupManager(INHibernateSession session)
        {
            return new RoomInLookupManager(session);
        }

        public IAccountTransactionManager GetAccountTransactionManager()
        {
            return new AccountTransactionManager();
        }
        public IAccountTransactionManager GetAccountTransactionManager(INHibernateSession session)
        {
            return new AccountTransactionManager(session);
        }

        public IAccountTransactionArcManager GetAccountTransactionArcManager()
        {
            return new AccountTransactionArcManager();
        }
        public IAccountTransactionArcManager GetAccountTransactionArcManager(INHibernateSession session)
        {
            return new AccountTransactionArcManager(session);
        }

        public IMessageManager GetMessageManager()
        {
            return new MessageManager();
        }

        public IMessageManager GetMessageManager(INHibernateSession session)
        {
            return new MessageManager(session);
        }

        public IOrdersManager GetOrdersManager()
        {
            return new OrdersManager();
        }

        public IOrdersManager GetOrdersManager(INHibernateSession session)
        {
            return new OrdersManager(session);
        }

        public IReferralOrderManager GetReferralOrderManager()
        {
            return new ReferralOrderManager();
        }
        public IReferralOrderManager GetReferralOrderManager(INHibernateSession session)
        {
            return new ReferralOrderManager(session);
        }

        public IPhysicianProcedureManager GetPhysicianProcedureManager()
        {
            return new PhysicianProcedureManager();
        }

        public IPhysicianProcedureManager GetPhysicianProcedureManager(INHibernateSession session)
        {
            return new PhysicianProcedureManager(session);
        }

        public IProcedureAuthorizationManager GetProcedureAuthorizationManager()
        {
            return new ProcedureAuthorizationManager();
        }

        public IProcedureAuthorizationManager GetProcedureAuthorizationManager(INHibernateSession session)
        {
            return new ProcedureAuthorizationManager(session);
        }

        public IOrdersAssessmentManager GetOrdersAssessmentManager()
        {
            return new OrdersAssessmentManager();
        }

        public IOrdersAssessmentManager GetOrdersAssessmentManager(INHibernateSession session)
        {
            return new OrdersAssessmentManager(session);
        }

        public IPPHeaderManager GetPPHeaderManager()
        {
            return new PPHeaderManager();
        }

        public IPPHeaderManager GetPPHeaderManager(INHibernateSession session)
        {
            return new PPHeaderManager(session);
        }

        public IPPHeaderArcManager GetPPHeaderArcManager()
        {
            return new PPHeaderArcManager();
        }

        public IPPHeaderArcManager GetPPHeaderArcManager(INHibernateSession session)
        {
            return new PPHeaderArcManager(session);
        }

        public IPPLineItemManager GetPPLineItemManager()
        {
            return new PPLineItemManager();
        }

        public IPPLineItemManager GetPPLineItemManager(INHibernateSession session)
        {
            return new PPLineItemManager(session);
        }

        public IPPLineItemArcManager GetPPLineItemArcManager()
        {
            return new PPLineItemArcManager();
        }

        public IPPLineItemArcManager GetPPLineItemArcManager(INHibernateSession session)
        {
            return new PPLineItemArcManager(session);
        }

        public ICheckManager GetCheckManager()
        {
            return new CheckManager();
        }

        public ICheckManager GetCheckManager(INHibernateSession session)
        {
            return new CheckManager(session);
        }

        public ICheckArcManager GetCheckArcManager()
        {
            return new CheckArcManager();
        }

        public ICheckArcManager GetCheckArcManager(INHibernateSession session)
        {
            return new CheckArcManager(session);
        }

        public IChargeHeaderManager GetChargeHeaderManager()
        {
            return new ChargeHeaderManager();
        }

        public IChargeHeaderManager GetChargeHeaderManager(INHibernateSession session)
        {
            return new ChargeHeaderManager(session);
        }

        public IWorkFlowTypeMasterManager GetWorkFlowTypeMasterManager()
        {
            return new WorkFlowTypeMasterManager();
        }

        public IWorkFlowTypeMasterManager GetWorkFlowTypeMasterManager(INHibernateSession session)
        {
            return new WorkFlowTypeMasterManager(session);
        }

        public IScan_IndexManager GetScanIndexManager()
        {
            return new Scan_IndexManager();
        }

        public IScan_IndexManager GetScanIndexManager(INHibernateSession session)
        {
            return new Scan_IndexManager(session);
        }

        public ILabInsurancePlanManager GetLabInsurancePlanManager()
        {
            return new LabInsurancePlanManager();
        }

        public ILabInsurancePlanManager GetLabInsurancePlanManager(INHibernateSession session)
        {
            return new LabInsurancePlanManager(session);
        }

        public IGrowthChartLookupManager GetGrowthChartLookupManager()
        {
            return new GrowthChartLookupManager();
        }

        public IGrowthChartLookupManager GetGrowthChartLookupManager(INHibernateSession session)
        {
            return new GrowthChartLookupManager(session);
        }

        public IFlowSheetTemplateManager GetFlowSheetTemplateManager()
        {
            return new FlowSheetTemplateManager();
        }

        public IFlowSheetTemplateManager GetFlowSheetTemplateManager(INHibernateSession session)
        {
            return new FlowSheetTemplateManager(session);
        }


        public IMasterTableListManager GetMasterTableListManager()
        {
            return new MasterTableListManager();
        }

        public IMasterTableListManager GetMasterTableListManager(INHibernateSession session)
        {
            return new MasterTableListManager(session);
        }

        public IScanManager GetScanManager()
        {
            return new ScanManager();
        }

        public IScanManager GetScanManager(INHibernateSession session)
        {
            return new ScanManager(session);
        }

        public IGeneralNotesManager GetGeneralNotesManager()
        {
            return new GeneralNotesManager();
        }
        public IGeneralNotesManager GetGeneralNotesManager(INHibernateSession session)
        {
            return new GeneralNotesManager(session);
        }

        public IAuditLogManager GetAuditLogManager()
        {
            return new AuditLogManager();
        }
        public IAuditLogManager GetAuditLogManager(INHibernateSession session)
        {
            return new AuditLogManager(session);
        }

        public IOrdersProblemListManager GetOrdersProblemListManager()
        {
            return new OrdersProblemListManager();
        }
        public IOrdersProblemListManager GetOrdersProblemListManager(INHibernateSession session)
        {
            return new OrdersProblemListManager(session);
        }

        public IReferralOrdersAssessmentManager GetReferralOrdersAssessmentManager()
        {
            return new ReferralOrdersAssessmentManager();
        }
        public IReferralOrdersAssessmentManager GetReferralOrdersAssessmentManager(INHibernateSession session)
        {
            return new ReferralOrdersAssessmentManager(session);
        }

        public IOrderCodeLibraryManager GetOrderCodeLibraryManager()
        {
            return new OrderCodeLibraryManager();
        }

        public IOrderCodeLibraryManager GetOrderCodeLibraryManager(INHibernateSession session)
        {
            return new OrderCodeLibraryManager(session);
        }

        public ICarePlanLookupManager GetCarePlanLookupManager()
        {
            return new CarePlanLookupManager();
        }
        public ICarePlanLookupManager GetCarePlanLookupManager(INHibernateSession session)
        {
            return new CarePlanLookupManager(session);
        }

        public ICarePlanManager GetCarePlanManager()
        {
            return new CarePlanManager();
        }
        public ICarePlanManager GetCarePlanManager(INHibernateSession session)
        {
            return new CarePlanManager(session);
        }

        public IPreventiveScreenLookupManager GetPreventiveScreenLookupManager()
        {
            return new PreventiveScreenLookupManager();
        }
        public IPreventiveScreenLookupManager GetPreventiveScreenLookupManager(INHibernateSession session)
        {
            return new PreventiveScreenLookupManager(session);
        }

        public IPreventiveScreenManager GetPreventiveScreenManager()
        {
            return new PreventiveScreenManager();
        }
        public IPreventiveScreenManager GetPreventiveScreenManager(INHibernateSession session)
        {
            return new PreventiveScreenManager(session);
        }

        public IPhysicianPatientManager GetPhysicianPatientManager()
        {
            return new PhysicianPatientManager();
        }

        public IPhysicianPatientManager GetPhysicianPatientManager(INHibernateSession session)
        {
            return new PhysicianPatientManager(session);
        }

        public IPhysicianPatientMasterManager GetPhysicianPatientMasterManager()
        {
            return new PhysicianPatientMasterManager();
        }

        public IPhysicianPatientMasterManager GetPhysicianPatientMasterManager(INHibernateSession session)
        {
            return new PhysicianPatientMasterManager(session);
        }

        public IAdvanceDirectiveManager GetAdvanceDirectiveManager()
        {
            return new AdvanceDirectiveManager();
        }

        public IAdvanceDirectiveManager GetAdvanceDirectiveManager(INHibernateSession session)
        {
            return new AdvanceDirectiveManager(session);
        }


        public IAdvanceDirectiveMasterManager GetAdvanceDirectiveMasterManager()
        {
            return new AdvanceDirectiveMasterManager();
        }

        public IAdvanceDirectiveMasterManager GetAdvanceDirectiveMasterManager(INHibernateSession session)
        {
            return new AdvanceDirectiveMasterManager(session);
        }

        public IAppointmentLookupManager GetAppointmentLookupManager()
        {
            return new AppointmentLookupManager();
        }

        public IAppointmentLookupManager GetAppointmentLookupManager(INHibernateSession session)
        {
            return new AppointmentLookupManager(session);
        }

        public IAssessmentVitalsLookupManager GetAssessmentVitalsLookupManager()
        {
            return new AssessmentVitalsLookupManager();
        }

        public IAssessmentVitalsLookupManager GetAssessmentVitalsLookupManager(INHibernateSession session)
        {
            return new AssessmentVitalsLookupManager(session);
        }


        public IMapPhysicianPhysicianAssitantManager GetMapPhysicianPhysicianAssistantManager()
        {
            return new MapPhysicianPhysicianAssitantManager();
        }

        public IMapPhysicianPhysicianAssitantManager GetMapPhysicianPhysicianAssistantManager(INHibernateSession session)
        {
            return new MapPhysicianPhysicianAssitantManager(session);
        }

        public IOrdersSubmitManager GetOrdersSubmitManager()
        {
            return new OrdersSubmitManager();
        }
        public IOrdersSubmitManager GetOrdersSubmitManager(INHibernateSession session)
        {
            return new OrdersSubmitManager(session);
        }

        public IImmunizationHistoryManager GetImmunizationHistoryManager()
        {
            return new ImmunizationHistoryManager();
        }
        public IImmunizationHistoryManager GetImmunizationHistoryManager(INHibernateSession session)
        {
            return new ImmunizationHistoryManager(session);
        }

        public IImmunizationMasterHistoryManager GetImmunizationMasterHistoryManager()
        {
            return new ImmunizationMasterHistoryManager();
        }
        public IImmunizationMasterHistoryManager GetImmunizationMasterHistoryManager(INHibernateSession session)
        {
            return new ImmunizationMasterHistoryManager(session);
        }

        public IOrdersQuestionSetAfpManager GetOrdersQuestionSetAfpManager()
        {
            return new OrdersQuestionSetAfpManager();
        }

        public IOrdersQuestionSetAfpManager GetOrdersQuestionSetAfpManager(INHibernateSession session)
        {
            return new OrdersQuestionSetAfpManager(session);
        }

        public IOrdersQuestionSetBloodLeadManager GetOrdersQuestionSetBloodLeadManager()
        {
            return new OrdersQuestionSetBloodLeadManager();
        }


        public IOrdersQuestionSetBloodLeadManager GetOrdersQuestionSetBloodLeadManager(INHibernateSession session)
        {
            return new OrdersQuestionSetBloodLeadManager(session);
        }

        public IOrdersQuestionSetCytologyManager GetOrdersQuestionSetCytologyManager()
        {
            return new OrdersQuestionSetCytologyManager();
        }

        public IFileManagementIndexManager GetFileManagementIndexManager()
        {
            return new FileManagementIndexManager();
        }

        public IFileManagementIndexManager GetFileManagementIndexManager(INHibernateSession session)
        {
            return new FileManagementIndexManager(session);
        }


        public ITestLookupManager GetTestLookupManager()
        {
            return new TestLookupManager();
        }
        public ITestLookupManager GetTestLookupManager(INHibernateSession session)
        {
            return new TestLookupManager(session);
        }


        public IHealthcareQuestionnaireManager GetHealthcareQuestionnaireManager()
        {
            return new HealthcareQuestionnaireManager();
        }
        public IHealthcareQuestionnaireManager GetHealthcareQuestionnaireManager(INHibernateSession session)
        {
            return new HealthcareQuestionnaireManager(session);
        }

        public IQuestionnaireLookupManager GetQuestionnaireLookupManager()
        {
            return new QuestionnaireLookupManager();
        }
        public IQuestionnaireLookupManager GetQuestionnaireLookupManager(INHibernateSession session)
        {
            return new QuestionnaireLookupManager(session);
        }

        public ITestManager GetTestManager()
        {
            return new TestManager();
        }
        public ITestManager GetTestManager(INHibernateSession session)
        {
            return new TestManager(session);
        }

        public IOrdersQuestionSetCytologyManager GetOrdersQuestionSetCytologyManager(INHibernateSession session)
        {
            return new OrdersQuestionSetCytologyManager(session);
        }

        public IResultLookupManager GetResultLookupManager()
        {
            return new ResultLookupManager();
        }
        public IResultLookupManager GetResultLookupManager(INHibernateSession session)
        {
            return new ResultLookupManager(session);
        }

        public IResultMasterManager GetResultMasterManager()
        {
            return new ResultMasterManager();
        }
        public IResultMasterManager GetResultMasterManager(INHibernateSession session)
        {
            return new ResultMasterManager(session);
        }

        public IResultORCManager GetResultORCManager()
        {
            return new ResultORCManager();
        }
        public IResultORCManager GetResultORCManager(INHibernateSession session)
        {
            return new ResultORCManager(session);
        }

        public IResultOBRManager GetResultOBRManager()
        {
            return new ResultOBRManager();
        }
        public IResultOBRManager GetResultOBRManager(INHibernateSession session)
        {
            return new ResultOBRManager(session);
        }

        public IResultOBXManager GetResultOBXManager()
        {
            return new ResultOBXManager();
        }
        public IResultOBXManager GetResultOBXManager(INHibernateSession session)
        {
            return new ResultOBXManager(session);
        }

        public IResultNTEManager GetResultNTEManager()
        {
            return new ResultNTEManager();
        }
        public IResultNTEManager GetResultNTEManager(INHibernateSession session)
        {
            return new ResultNTEManager(session);
        }

        public IResultZEFManager GetResultZEFManager()
        {
            return new ResultZEFManager();
        }
        public IResultZEFManager GetResultZEFManager(INHibernateSession session)
        {
            return new ResultZEFManager(session);
        }

        public IResultZPSManager GetResultZPSManager()
        {
            return new ResultZPSManager();
        }
        public IResultZPSManager GetResultZPSManager(INHibernateSession session)
        {
            return new ResultZPSManager(session);
        }

        public IRcopia_MedicationManager GetRcopia_MedicationManager()
        {
            return new Rcopia_MedicationManager();
        }
        public IRcopia_MedicationManager GetRcopia_MedicationManager(INHibernateSession session)
        {
            return new Rcopia_MedicationManager(session);
        }

        public IRcopia_AllergyManager GetRcopia_AllergyManager()
        {
            return new Rcopia_AllergyManager();
        }
        public IRcopia_AllergyManager GetRcopia_AllergyManager(INHibernateSession session)
        {
            return new Rcopia_AllergyManager(session);
        }

        public IRcopia_Prescription_ListManager GetRcopia_Prescription_ListManager()
        {
            return new Rcopia_Prescription_ListManager();
        }
        public IRcopia_Prescription_ListManager GetRcopia_Prescription_ListManager(INHibernateSession session)
        {
            return new Rcopia_Prescription_ListManager();
        }

        public IRcopia_SettingsManager GetRcopia_SettingsManager()
        {
            return new Rcopia_SettingsManager();
        }

        public IRcopia_SettingsManager GetRcopia_SettingsManager(INHibernateSession session)
        {
            return new Rcopia_SettingsManager();
        }

        public IRcopia_Update_InfoManager GetRcopia_Update_InfoManager()
        {
            return new Rcopia_Update_InfoManager();
        }
        public IRcopia_Update_InfoManager GetRcopia_Update_InfoManager(INHibernateSession session)
        {
            return new Rcopia_Update_InfoManager(session);
        }


        public IVaccineInfoStatementManager GetVaccineInfoStatementManager()
        {
            return new VaccineInfoStatementManager();
        }
        public IVaccineInfoStatementManager GetVaccineInfoStatementManager(INHibernateSession session)
        {
            return new VaccineInfoStatementManager(session);
        }

        public IVaccineInfoStatementProcedureManager GetVaccineInfoStatementProcedureManager()
        {
            return new VaccineInfoStatementProcedureManager();
        }
        public IVaccineInfoStatementProcedureManager GetVaccineInfoStatementProcedureManager(INHibernateSession session)
        {
            return new VaccineInfoStatementProcedureManager(session);
        }

        public IPhysicianSpecialtyManager GetPhysicianSpecialtyManager()
        {
            return new PhysicianSpecialtyManager();
        }
        public IPhysicianSpecialtyManager GetPhysicianSpecialtyManager(INHibernateSession session)
        {
            return new PhysicianSpecialtyManager(session);
        }

        public IPQRI_MeasureManager GetPQRIMeasureManager()
        {
            return new PQRI_MeasureManager();
        }

        public IPQRI_MeasureManager GetPQRIMeasureManager(INHibernateSession session)
        {
            return new PQRI_MeasureManager(session);
        }

        public IPQRI_DataManager GetPQRIDataManager()
        {
            return new PQRI_DataManager();
        }
        public IPQRI_DataManager GetPQRIDataManager(INHibernateSession session)
        {
            return new PQRI_DataManager(session);
        }

        public IPQRIManager GetPQRIManager()
        {
            return new PQRIManager();
        }
        public IPQRIManager GetPQRIManager(INHibernateSession session)
        {
            return new PQRIManager(session);
        }

        public ILabCarrierLookUpManager GetLabCarrierLookUpManager()
        {
            return new LabCarrierLookUpManager();
        }
        public ILabCarrierLookUpManager GetLabCarrierLookUpManager(INHibernateSession session)
        {
            return new LabCarrierLookUpManager(session);
        }

        public IDocumentManager GetDocumentManager()
        {
            return new DocumentManager();
        }
        public IDocumentManager GetDocumentManager(INHibernateSession session)
        {
            return new DocumentManager(session);
        }

        public IVaccineManufacturerCodesManager GetVaccineManufacturerCodesManager()
        {
            return new VaccineManufacturerCodesManager();
        }
        public IVaccineManufacturerCodesManager GetVaccineManufacturerCodesManager(INHibernateSession session)
        {
            return new VaccineManufacturerCodesManager(session);
        }

        public IRuleMedicationManager GetRuleMedicationManager()
        {
            return new RuleMedicationManager();
        }
        public IRuleMedicationManager GetRuleMedicationManager(INHibernateSession session)
        {
            return new RuleMedicationManager(session);
        }

        public IRuleMasterManager GetRuleMasterManager()
        {
            return new RuleMasterManager();
        }
        public IRuleMasterManager GetRuleMasterManager(INHibernateSession session)
        {
            return new RuleMasterManager(session);
        }

        public IRuleMedicationAndAllergyManager GetRuleMedicationAndAllergyManager()
        {
            return new RuleMedicationAndAllergyManager();
        }
        public IRuleMedicationAndAllergyManager GetRuleMedicationAndAllergyManager(INHibernateSession session)
        {
            return new RuleMedicationAndAllergyManager(session);
        }

        public IRuleProblemManager GetRuleProblemManager()
        {
            return new RuleProblemManager();
        }
        public IRuleProblemManager GetRuleProblemManager(INHibernateSession session)
        {
            return new RuleProblemManager(session);
        }

        public IRuleLabResultReminderManager GetRuleLabResultReminderManager()
        {
            return new RuleLabResultReminderManager();
        }
        public IRuleLabResultReminderManager GetRuleLabResultReminderManager(INHibernateSession session)
        {
            return new RuleLabResultReminderManager(session);
        }

        public IPrescriptionManager GetPrescriptionManager()
        {
            return new PrescriptionManager();
        }
        public IPrescriptionManager GetPrescriptionManager(INHibernateSession session)
        {
            return new PrescriptionManager(session);

        }

        public IMasterVitalsManager GetMasterVitalsManager()
        {
            return new MasterVitalsManager();
        }
        public IMasterVitalsManager GetMasterVitalsManager(INHibernateSession session)
        {
            return new MasterVitalsManager(session);

        }

        public IMapVitalsPhysicianManager GetMapVitalsPhysicianManager()
        {
            return new MapVitalsPhysicianManager();
        }
        public IMapVitalsPhysicianManager GetMapVitalsPhysicianManager(INHibernateSession session)
        {
            return new MapVitalsPhysicianManager(session);

        }

        public IPercentileLookUpManager GetPercentileLookUpManager()
        {
            return new PercentileLookUpManager();
        }
        public IPercentileLookUpManager GetPercentileLookUpManager(INHibernateSession session)
        {
            return new PercentileLookUpManager(session);

        }

        public IInHouseProcedureManager GetInHouseProcedureManager()
        {
            return new InHouseProcedureManager();
        }
        public IInHouseProcedureManager GetInHouseProcedureManager(INHibernateSession session)
        {
            return new InHouseProcedureManager(session);
        }

        public ILabcorpSettingsManager GetLabcorpSettingsManager()
        {
            return new LabcorpSettingsManager();
        }
        public ILabcorpSettingsManager GetLabcorpSettingsManager(INHibernateSession session)
        {
            return new LabcorpSettingsManager(session);
        }

        public ISpirometryManager GetSpirometryManager()
        {
            return new SpirometryManager();
        }
        public ISpirometryManager GetSpirometryManager(INHibernateSession session)
        {
            return new SpirometryManager(session);
        }

        public IABI_ResultsManager GetAbiManager()
        {
            return new ABI_ResultsManager();
        }

        public IABI_ResultsManager GetAbiManager(INHibernateSession session)
        {
            return new ABI_ResultsManager(session);
        }

        public IPhysician_DrugManager GetPhysician_DrugManager()
        {
            return new Physician_DrugManager();
        }
        public IPhysician_DrugManager GetPhysician_DrugManager(INHibernateSession session)
        {
            return new Physician_DrugManager(session);
        }
        public IProcedureCodeRuleMasterManager GetProcedureCodeRuleMasterManager()
        {
            return new ProcedureCodeRuleMasterManager();
        }
        public IProcedureCodeRuleMasterManager GetProcedureCodeRuleMasterManager(INHibernateSession session)
        {
            return new ProcedureCodeRuleMasterManager(session);
        }

        public IAOELookUpManager AOELookUpManager()
        {
            return new AOELookUpManager();
        }
        public IAOELookUpManager AOELookUpManager(INHibernateSession session)
        {
            return new AOELookUpManager(session);
        }
        public IOrdersQuestionSetAOEManager OrdersQuestionSetAOEManager()
        {
            return new OrdersQuestionSetAOEManager();
        }
        public IOrdersQuestionSetAOEManager OrdersQuestionSetAOEManager(INHibernateSession session)
        {
            return new OrdersQuestionSetAOEManager(session);
        }

        public IOrderComponentsManager OrderComponentsManager()
        {
            return new OrderComponentsManager();
        }
        public IOrderComponentsManager OrderComponentsManager(INHibernateSession session)
        {
            return new OrderComponentsManager(session);
        }

        public IAddendumNotesManager GetAddendumNotesManager()
        {
            return new AddendumNotesManager();
        }

        public IAddendumNotesManager GetAddendumNotesManager(INHibernateSession session)
        {
            return new AddendumNotesManager(session);

        }

        public IPhysicianResultsManager GetPhysicianResultsManager()
        {
            return new PhysicianResultsManager();
        }
        public IPhysicianResultsManager GetPhysicianResultsManager(INHibernateSession session)
        {
            return new PhysicianResultsManager(session);
        }

        public IAcurusResultsMappingManager GetAcurusResultsMappingManager()
        {
            return new AcurusResultsMappingManager();
        }
        public IAcurusResultsMappingManager GetAcurusResultsMappingManager(INHibernateSession session)
        {
            return new AcurusResultsMappingManager(session);
        }

        public IPlanCPTFormRequiredManager GetPlanCPTFormRequiredManager()
        {
            return new PlanCPTFormRequiredManager();
        }
        public IPlanCPTFormRequiredManager GetPlanCPTFormRequiredManager(INHibernateSession session)
        {
            return new PlanCPTFormRequiredManager(session);

        }

        public IOrdersRequiredFormsManager GetOrdersRequiredFormsManager()
        {
            return new OrdersRequiredFormsManager();
        }
        public IOrdersRequiredFormsManager GetOrdersRequiredFormsManager(INHibernateSession session)
        {
            return new OrdersRequiredFormsManager(session);

        }

        public IDictationExceptionManager GetDictationExceptionManager()
        {
            return new DictationExceptionManager();
        }

        public IDictationExceptionManager GetDictationExceptionManager(INHibernateSession session)
        {
            return new DictationExceptionManager(session);
        }

        public IUserScnTabManager GetUserScnTabManager()
        {
            return new UserScnTabManager();
        }
        public IUserScnTabManager GetUserScnTabManager(INHibernateSession session)
        {
            return new UserScnTabManager(session);
        }

        public IErrorLogManager GetErrorLogManager()
        {
            return new ErrorLogManager();
        }
        public IErrorLogManager GetErrorLogManager(INHibernateSession session)
        {
            return new ErrorLogManager(session);
        }
        public IMeasuresRuleMasterManager GetMeasuresRuleMasterManager()
        {
            return new MeasuresRuleMasterManager();
        }
        public IMeasuresRuleMasterManager GetMeasuresRuleMasterManager(INHibernateSession session)
        {
            return new MeasuresRuleMasterManager(session);
        }

        public IActivityLogManager GetActivityLogManager()
        {
            return new ActivityLogManager();
        }
        public IActivityLogManager GetActivityLogManager(INHibernateSession session)
        {
            return new ActivityLogManager(session);
        }

        public IRegisteredNetworkManager GetRegisteredNetworkManager()
        {
            return new RegisteredNetworkManager();
        }
        public IRegisteredNetworkManager GetRegisteredNetworkManager(INHibernateSession session)
        {
            return new RegisteredNetworkManager(session);
        }

        public ICDSRuleMasterManager GetCDSRuleMasterManager()
        {
            return new CDSRuleMasterManager();
        }
        public ICDSRuleMasterManager GetCDSRuleMasterManager(INHibernateSession session)
        {
            return new CDSRuleMasterManager(session);
        }
         public INotificationManager GetNotificationManager()
        {
            return new NotificationManager();
        }
        public INotificationManager GetNotificationManager(INHibernateSession session)
        {
            return new NotificationManager(session);
        }


        public IRegistryLogManager GetRegistryLogManager()
        {
            return new RegistryLogManager();
        }
        public IRegistryLogManager GetRegistryLogManager(INHibernateSession session)
        {
            return new RegistryLogManager(session);
        }

        public IContextManager GetContextManager()
        {
            return new ContextManager();
        }
        public IContextManager GetContextManager(INHibernateSession session)
        {
            return new ContextManager(session);
        }

        public IQuest_Lab_Response_LogManager GetQuest_Lab_Response_LogManager()
        {
            return new Quest_Lab_Response_LogManager();
        }
        public IQuest_Lab_Response_LogManager GetQuest_Lab_Response_LogManager(INHibernateSession session)
        {
            return new Quest_Lab_Response_LogManager(session);
        }

        public IPotentialDiagnosisManager GetPotentialDiagnosisManager()
        {
            return new PotentialDiagnosisManager();
        }
        public IPotentialDiagnosisManager GetPotentialDiagnosisManager(INHibernateSession session)
        {
            return new PotentialDiagnosisManager(session);
        }
        public IProviderReviewTrackerManager GetProviderReviewTrackerManager()
        {
            return new ProviderReviewTrackerManager();
        }
        public IProviderReviewTrackerManager GetProviderReviewTrackerManager(INHibernateSession session)
        {
            return new ProviderReviewTrackerManager(session);
        }
        public IRequestSentLogManager GetRequestSentLogManager()
        {
            return new RequestSentLogManager();
        }
        public IRequestSentLogManager GetRequestSentLogManager(INHibernateSession session)
        {
            return new RequestSentLogManager(session);
        }
        public IResponseReceivedLogManager GetResponseReceivedLogManager()
        {
            return new ResponseReceivedLogManager();
        }
        public IResponseReceivedLogManager GetResponseReceivedLogManager(INHibernateSession session)
        {
            return new ResponseReceivedLogManager(session);
        }

        public IClientManager GetClientManager()
        {
            return new ClientManager();
        }
        public IClientManager GetClientManager(INHibernateSession session)
        {
            return new ClientManager(session);
        }

        public IHumanBlobManager GetHumanBlobManager()
        {
            return new HumanBlobManager();
        }
        public IHumanBlobManager GetHumanBlobManager(INHibernateSession session)
        {
            return new HumanBlobManager(session);
        }

        public IEncounterBlobManager GetEncounterBlobManager()
        {
            return new EncounterBlobManager();
        }
        public IEncounterBlobManager GetEncounterBlobManager(INHibernateSession session)
        {
            return new EncounterBlobManager(session);
        }

        public IMapXMLBlobManager GetMapXMLBlobManager()
        {
            return new MapXMLBlobManager();
        }
        public IMapXMLBlobManager GetMapXMLBlobManager(INHibernateSession session)
        {
            return new MapXMLBlobManager(session);
        }
        public IProcedureModifierLookupManager GetProcedureModifierLookupManager()
        {
            return new ProcedureModifierLookupManager();
        }
        public IProcedureModifierLookupManager GetProcedureModifierLookupManager(INHibernateSession session)
        {
            return new ProcedureModifierLookupManager(session);
        }
        public IRCopiaDeduplicateLogManager GetRCopiaDeduplicateLogManager()
        {
            return new RCopiaDeduplicateLogManager();
        }
        public IRCopiaDeduplicateLogManager GetRCopiaDeduplicateLogManager(INHibernateSession session)
        {
            return new RCopiaDeduplicateLogManager(session);
        }
        public IBlobProgressNoteManager GetBlobProgressNoteManager()
        {
            return new BlobProgressNoteManager();
        }
        public IBlobProgressNoteManager GetBlobProgressNoteManager(INHibernateSession session)
        {
            return new BlobProgressNoteManager(session);
        }
        public ICDCEventTrackerManager GetCDCEventTrackerManager()
        {
            return new CDCEventTrackerManager();
        }
        public ICDCEventTrackerManager GetCDCEventTrackerManager(INHibernateSession session)
        {
            return new CDCEventTrackerManager(session);
        }
        #endregion
    }
}

