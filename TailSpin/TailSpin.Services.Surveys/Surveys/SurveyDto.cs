




 




namespace TailSpin.Services.Surveys.Surveys
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class SurveyDto
    {
        public SurveyDto()
        {
            this.Questions = new List<QuestionDto>();
        }

        [DataMember]
        public string SlugName { get; set; }

        [DataMember]
        public string Tenant { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string IconUrl { get; set; }

        [DataMember]
        public int Length { get; set; }

        [DataMember]
        public List<QuestionDto> Questions { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }
    }
}