




 




namespace TailSpin.Services.Surveys.Surveys
{
    using System.Runtime.Serialization;

    [DataContract]
    public class QuestionDto
    {
        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string PossibleAnswers { get; set; }
    }
}