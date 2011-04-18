




 




namespace TailSpin.Web.Survey.Public.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using TailSpin.Web.Survey.Public.Models;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Stores;

    public class SurveysController : Controller
    {
        private readonly ISurveyStore surveyStore;
        private readonly ISurveyAnswerStore surveyAnswerStore;

        public SurveysController(ISurveyStore surveyStore, ISurveyAnswerStore surveyAnswerStore)
        {
            this.surveyStore = surveyStore;
            this.surveyAnswerStore = surveyAnswerStore;
        }

        public string TenantName { get; private set; }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new TenantPageViewData<IEnumerable<Survey>>(this.surveyStore.GetRecentSurveys());
            model.Title = "Existing surveys";
            return this.View(model);
        }

        [HttpGet]
        public ActionResult Display(string tenant, string surveySlug)
        {
            var surveyAnswer = CallGetSurveyAndCreateSurveyAnswer(this.surveyStore, tenant, surveySlug);

            var model = new TenantPageViewData<SurveyAnswer>(surveyAnswer);
            model.Title = surveyAnswer.Title;
            return this.View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Display(string tenant, string surveySlug, SurveyAnswer contentModel)
        {
            var surveyAnswer = CallGetSurveyAndCreateSurveyAnswer(this.surveyStore, tenant, surveySlug);

            if (surveyAnswer.QuestionAnswers.Count != contentModel.QuestionAnswers.Count)
            {
                throw new ArgumentException("The survey answers received have different amount of questions then the survey to be filled.");
            }

            for (int i = 0; i < surveyAnswer.QuestionAnswers.Count; i++)
            {
                surveyAnswer.QuestionAnswers[i].Answer = contentModel.QuestionAnswers[i].Answer;
            }

            if (!this.ModelState.IsValid)
            {
                var model = new TenantPageViewData<SurveyAnswer>(surveyAnswer);
                model.Title = surveyAnswer.Title;
                return this.View(model);
            }

            this.surveyAnswerStore.SaveSurveyAnswer(surveyAnswer);

            return this.RedirectToAction("ThankYou");
        }

        [HttpGet]
        public ActionResult ThankYou()
        {
            var model = new TenantMasterPageViewData { Title = "Thank you for filling the survey" };
            return this.View(model);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["tenant"] != null)
            {
                this.TenantName = (string)filterContext.RouteData.Values["tenant"];
                this.ViewData["tenant"] = this.TenantName;
            }

            base.OnActionExecuting(filterContext);
        }

        private static SurveyAnswer CallGetSurveyAndCreateSurveyAnswer(ISurveyStore surveyStore, string tenantName, string surveySlug)
        {
            var survey = surveyStore.GetSurveyByTenantAndSlugName(tenantName, surveySlug, true);

            var surveyAnswer = new SurveyAnswer
            {
                Title = survey.Title,
                SlugName = surveySlug,
                Tenant = tenantName
            };

            foreach (var question in survey.Questions)
            {
                surveyAnswer.QuestionAnswers.Add(new QuestionAnswer
                {
                    QuestionText = question.Text,
                    QuestionType = question.Type,
                    PossibleAnswers = question.PossibleAnswers
                });
            }

            return surveyAnswer;
        }
    }
}

