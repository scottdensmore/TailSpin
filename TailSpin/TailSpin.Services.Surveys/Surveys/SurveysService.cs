




 




namespace TailSpin.Services.Surveys.Surveys
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.Threading;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Stores;
    using TailSpin.Web.Survey.Shared.SurveysFiltering;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, IncludeExceptionDetailInFaults = true)]
    public class SurveysService : ISurveysService
    {
        private readonly IFilteringService filteringService;
        private readonly ITenantStore tenantStore;
        private readonly ISurveyAnswerStore surveyAnswerStore;
        private readonly IMediaAnswerStore mediaAnswerStore;
        private readonly Dictionary<string, string> cachedTenants = new Dictionary<string, string>();

        public SurveysService(
            IFilteringService filteringService, 
            ITenantStore tenantStore, 
            ISurveyAnswerStore surveyAnswerStore, 
            IMediaAnswerStore mediaAnswerStore)
        {
            this.filteringService = filteringService;
            this.tenantStore = tenantStore;
            this.surveyAnswerStore = surveyAnswerStore;
            this.mediaAnswerStore = mediaAnswerStore;
        }
        
        public SurveyDto[] GetSurveys(string lastSyncUtcDate)
        {
            DateTime fromDate;
            if (!string.IsNullOrEmpty(lastSyncUtcDate))
            {
                if (DateTime.TryParse(lastSyncUtcDate, out fromDate))
                {
                    fromDate = DateTime.SpecifyKind(fromDate, DateTimeKind.Utc);
                }
                else
                {
                    throw new FormatException("lastSyncUtcDate is in an incorrect format. The format should be: yyyy-MM-ddTHH:mm:ss");
                }
            }
            else
            {
                fromDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            }

            var username = Thread.CurrentPrincipal.Identity.Name;

            return this.filteringService
                        .GetSurveysForUser(username, fromDate)
                        .Select(s => new SurveyDto
                                         {
                                             SlugName = s.SlugName,
                                             Title = s.Title,
                                             Tenant = s.Tenant,
                                             Length = 5 * s.Questions.Count,
                                             IconUrl = this.GetIconUrlForTenant(s.Tenant),
                                             CreatedOn = s.CreatedOn,
                                             Questions = s.Questions.Select(q => 
                                                                new QuestionDto
                                                                {
                                                                    PossibleAnswers = q.PossibleAnswers,
                                                                    Text = q.Text,
                                                                    Type = Enum.GetName(typeof(QuestionType), q.Type)
                                                                }).ToList()
                                         }).ToArray();
        }

        public void AddSurveyAnswers(SurveyAnswerDto[] surveyAnswers)
        {
            foreach (var surveyAnswerDto in surveyAnswers)
            {
                this.surveyAnswerStore.SaveSurveyAnswer(
                    new SurveyAnswer
                        {
                            Title = surveyAnswerDto.Title,
                            SlugName = surveyAnswerDto.SlugName,
                            Tenant = surveyAnswerDto.Tenant,
                            StartLocation = surveyAnswerDto.StartLocation,
                            CompleteLocation = surveyAnswerDto.CompleteLocation,
                            QuestionAnswers = surveyAnswerDto.QuestionAnswers
                                                             .Select(qa => new QuestionAnswer
                                                                               {
                                                                                   QuestionText = qa.QuestionText,
                                                                                   PossibleAnswers = qa.PossibleAnswers,
                                                                                   QuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), qa.QuestionType),
                                                                                   Answer = qa.Answer
                                                                               }).ToList()
                        });    
            }
        }

        public string AddMediaAnswer(Stream media, string type)
        {
            var questionType = (QuestionType)Enum.Parse(typeof(QuestionType), type);
            return this.mediaAnswerStore.SaveMediaAnswer(media, questionType);
        }

        private string GetIconUrlForTenant(string tenant)
        {
            if (!string.IsNullOrEmpty(tenant))
            {
                if (!this.cachedTenants.ContainsKey(tenant))
                {
                    var logo = this.tenantStore.GetTenant(tenant).PhoneLogo;
                    this.cachedTenants.Add(tenant, logo);
                }

                return this.cachedTenants[tenant];
            }

            return string.Empty;
        }
    }
}