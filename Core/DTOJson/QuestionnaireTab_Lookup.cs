using System.Collections.Generic;

namespace Acurus.Capella.Core.DTOJson
{
    public class Questionnaire
    {
        public string user_name { get; set; }
        public string Questionnaire_category { get; set; }
    }

    public class QuestionnaireList
    {
        public QuestionnaireList()
        {
            questionnaire = new List<Questionnaire>();
        }
        public List<Questionnaire> questionnaire { get; set; }
    }
}
