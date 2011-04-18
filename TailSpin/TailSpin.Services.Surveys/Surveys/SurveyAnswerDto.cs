




 




namespace TailSpin.Services.Surveys.Surveys
{
    public class SurveyAnswerDto
    {
        public SurveyAnswerDto()
        {
            this.QuestionAnswers = new QuestionAnswerDto[0];
        }

        public string Title { get; set; }

        public string SlugName { get; set; }

        public string Tenant { get; set; }

        public QuestionAnswerDto[] QuestionAnswers { get; set; }

        public string StartLocation { get; set; }

        public string CompleteLocation { get; set; }
    }
}