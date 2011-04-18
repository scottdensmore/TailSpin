




 




namespace TailSpin.Web.Areas.Survey.Controllers
{
    using System.Data.Services.Client;
    using System.Globalization;
    using System.Web.Mvc;
    using Models;
    using TailSpin.Web.Controllers;
    using TailSpin.Web.Security;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Stores;
    using TailSpin.Web.Utility;

    [RequireHttps]
    [AuthenticateAndAuthorize(Roles = "Survey Administrator")]
    public class SurveysController : TenantController
    {
        private readonly ISurveyStore surveyStore;
        private readonly ISurveyAnswerStore surveyAnswerStore;
        private readonly ISurveyAnswersSummaryStore surveyAnswersSummaryStore;
        private readonly ITenantStore tenantStore;
        private readonly ISurveyTransferStore surveyTransferStore;

        public SurveysController(
            ISurveyStore surveyStore, 
            ISurveyAnswerStore surveyAnswerStore, 
            ISurveyAnswersSummaryStore surveyAnswersSummaryStore, 
            ITenantStore tenantStore, 
            ISurveyTransferStore surveyTransferStore)
                : base(tenantStore)
        {
            this.surveyStore = surveyStore;
            this.surveyAnswerStore = surveyAnswerStore;
            this.surveyAnswersSummaryStore = surveyAnswersSummaryStore;
            this.tenantStore = tenantStore;
            this.surveyTransferStore = surveyTransferStore;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var surveysForTenant = this.surveyStore.GetSurveysByTenant(this.TenantName);

            var model = this.CreateTenantPageViewData(surveysForTenant);
            model.Title = "My Surveys";
            return this.View(model);
        }

        [HttpGet]
        public ActionResult New([Deserialize]Survey hiddenSurvey)
        {
            if (hiddenSurvey == null)
            {
                hiddenSurvey = (Survey)this.TempData["hiddenSurvey"];
            }

            if (hiddenSurvey == null)
            {
                hiddenSurvey = new Survey();  // First time to the page
            }

            var model = this.CreateTenantPageViewData(hiddenSurvey);
            model.Title = "New Survey";
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(Survey contentModel, [Deserialize]Survey hiddenSurvey)
        {
            contentModel.Questions = hiddenSurvey.Questions;

            if (!this.ModelState.IsValid)
            {
                var model = this.CreateTenantPageViewData(contentModel);
                model.Title = "New Survey";
                return this.View(model);
            }

            if (contentModel.Questions.Count <= 0)
            {
                var model = this.CreateTenantPageViewData(contentModel);
                model.Title = "New Survey";
                this.ModelState.AddModelError("ContentModel.Questions", string.Format(CultureInfo.InvariantCulture, "Please add at least one question to the survey."));
                return this.View(model);
            }

            contentModel.Tenant = this.TenantName;
            try
            {
                this.surveyStore.SaveSurvey(contentModel);
            }
            catch (DataServiceRequestException ex)
            {
                var dataServiceClientException = ex.InnerException as DataServiceClientException;
                if (dataServiceClientException != null)
                {
                    if (dataServiceClientException.StatusCode == 409)
                    {
                        var model = this.CreateTenantPageViewData(contentModel);
                        model.Title = "New Survey";
                        this.ModelState.AddModelError("ContentModel.Title", string.Format(CultureInfo.InvariantCulture, "The name '{0}' is already in use. Please choose another name.", model.ContentModel.Title));
                        return this.View(model);
                    }
                }

                throw;
            }
            
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewQuestion(Survey contentModel, [Deserialize]Survey hiddenSurvey, string referrer)
        {
            if (referrer != "addQuestion")
            {
                this.ModelState.Clear();
                hiddenSurvey.Title = contentModel.Title;
            }

            this.ViewData["hiddenSurvey"] = hiddenSurvey;

            var model = this.CreateTenantPageViewData(new Question());
            model.Title = "New Question";
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion([Deserialize]Survey hiddenSurvey, Question contentModel)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewData["hiddenSurvey"] = hiddenSurvey;

                var model = this.CreateTenantPageViewData(contentModel ?? new Question());
                model.Title = "New Question";
                return this.View("NewQuestion", model);
            }

            if (contentModel.PossibleAnswers != null)
            {
                contentModel.PossibleAnswers = contentModel.PossibleAnswers.Replace("\r\n", "\n");
            }
            hiddenSurvey.Questions.Add(contentModel);
            this.TempData["hiddenSurvey"] = hiddenSurvey;
            return RedirectToAction("New");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelNewQuestion([Deserialize]Survey hiddenSurvey)
        {
            this.TempData["hiddenSurvey"] = hiddenSurvey;
            return RedirectToAction("New");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string tenant, string surveySlug)
        {
            this.surveyStore.DeleteSurveyByTenantAndSlugName(tenant, surveySlug);
            this.surveyAnswerStore.DeleteSurveyAnswers(tenant, surveySlug);
            this.surveyAnswersSummaryStore.DeleteSurveyAnswersSummary(tenant, surveySlug);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Analyze(string tenant, string surveySlug)
        {
            var surveyAnswersSummary = this.surveyAnswersSummaryStore.GetSurveyAnswersSummary(tenant, surveySlug);

            var model = this.CreateTenantPageViewData(surveyAnswersSummary);
            model.Title = surveySlug;
            return this.View(model);
        }

        [HttpGet]
        public ActionResult BrowseResponses(string tenant, string surveySlug, string answerId)
        {
            SurveyAnswer surveyAnswer = null;
            if (string.IsNullOrEmpty(answerId))
            {
                answerId = this.surveyAnswerStore.GetFirstSurveyAnswerId(tenant, surveySlug);
            }

            if (!string.IsNullOrEmpty(answerId))
            {
                surveyAnswer = this.surveyAnswerStore.GetSurveyAnswer(tenant, surveySlug, answerId);
            }

            var surveyAnswerBrowsingContext = this.surveyAnswerStore.GetSurveyAnswerBrowsingContext(tenant, surveySlug, answerId);

            var browseResponsesModel = new BrowseResponseModel
                                           {
                                               SurveyAnswer = surveyAnswer,
                                               PreviousAnswerId = surveyAnswerBrowsingContext.PreviousId,
                                               NextAnswerId = surveyAnswerBrowsingContext.NextId
                                           };

            var model = this.CreateTenantPageViewData(browseResponsesModel);
            model.Title = surveySlug;
            return this.View(model);
        }

        [HttpGet]
        public ActionResult ExportResponses(string surveySlug)
        {
            Tenant tenant = this.tenantStore.GetTenant(this.TenantName);
            var exportResponseModel = new ExportResponseModel { Tenant = tenant };
            string answerId = this.surveyAnswerStore.GetFirstSurveyAnswerId(this.TenantName, surveySlug);
            exportResponseModel.HasResponses = !string.IsNullOrEmpty(answerId);

            var model = this.CreateTenantPageViewData(exportResponseModel);
            model.Title = surveySlug;
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportResponses(string tenant, string surveySlug)
        {
            this.surveyTransferStore.Transfer(tenant, surveySlug);
            return RedirectToAction("BrowseResponses");
        }
    }
}

