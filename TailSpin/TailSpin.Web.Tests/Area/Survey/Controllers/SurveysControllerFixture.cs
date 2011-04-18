




 




namespace TailSpin.Web.Tests.Area.Survey.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TailSpin.Web.Areas.Survey.Controllers;
    using TailSpin.Web.Areas.Survey.Models;
    using TailSpin.Web.Models;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Stores;

    [TestClass]
    public class SurveysControllerFixture
    {
        [TestMethod]
        public void IndexReturnsEmptyViewName()
        {
            var controller = new SurveysController(new Mock<ISurveyStore>().Object, null, null, null, null);

            var result = controller.Index() as ViewResult;

            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [TestMethod]
        public void IndexReturnsMySurveysAsTitleInTheModel()
        {
            var controller = new SurveysController(new Mock<ISurveyStore>().Object, null, null, null, null);

            var result = controller.Index() as ViewResult;

            var model = result.ViewData.Model as TenantMasterPageViewData;
            Assert.AreSame("My Surveys", model.Title);
        }

        [TestMethod]
        public void IndexCallsGetAllSurveysByTenantFromSurveyStore()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var controller = new SurveysController(mockSurveyStore.Object, null, null, null, null);
            controller.TenantName = "tenant";

            controller.Index();

            mockSurveyStore.Verify(r => r.GetSurveysByTenant(It.Is<string>(actual => "tenant" == actual)), Times.Once());
        }

        [TestMethod]
        public void IndexReturnsTheSurveysForTheTenantInTheModel()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var surveysToReturn = new List<Survey>();
            mockSurveyStore.Setup(r => r.GetSurveysByTenant(It.IsAny<string>())).Returns(surveysToReturn);
            var controller = new SurveysController(mockSurveyStore.Object, null, null, null, null);

            var result = controller.Index() as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<IEnumerable<Survey>>;
            Assert.AreSame(surveysToReturn, model.ContentModel);
        }

        [TestMethod]
        public void NewWhenHttpVerbIsGetReturnsEmptyViewName()
        {
            var controller = new SurveysController(new Mock<ISurveyStore>().Object, null, null, null, null);

            var result = controller.New(null) as ViewResult;

            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [TestMethod]
        public void NewWhenHttpVerbIsGetReturnsNewSurveyAsTitleInTheModel()
        {
            var controller = new SurveysController(new Mock<ISurveyStore>().Object, null, null, null, null);

            var result = controller.New(null) as ViewResult;

            var model = result.ViewData.Model as TenantMasterPageViewData;
            Assert.AreSame("New Survey", model.Title);
        }

        [TestMethod]
        public void NewWhenHttpVerbIsGetReturnsTheHiddenSurveyInTheModelWhenHiddenSurveyExistsInTempDataAndSurveyParameterIsNull()
        {
            var controller = new SurveysController(new Mock<ISurveyStore>().Object, null, null, null, null);
            var survey = new Survey();
            controller.TempData["hiddenSurvey"] = survey;

            var result = controller.New(null) as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<Survey>;
            Assert.AreSame(survey, model.ContentModel);
        }

        [TestMethod]
        public void NewWhenHttpVerbIsGetReturnsTheSurveyParameterInTheModelWhenHiddenSurveyExistsInTempDataAndSurveyParameterIsNotNull()
        {
            var controller = new SurveysController(new Mock<ISurveyStore>().Object, null, null, null, null);
            var survey = new Survey();
            controller.TempData["hiddenSurvey"] = new Survey();

            var result = controller.New(survey) as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<Survey>;
            Assert.AreSame(survey, model.ContentModel);
        }

        [TestMethod]
        public void NewWhenHttpVerbIsGetReturnsTheHiddenSurveyInTheModelWhenHiddenSurveyDoesNotExistsInTempDataAndSurveyParameterIsNull()
        {
            var controller = new SurveysController(new Mock<ISurveyStore>().Object, null, null, null, null);

            var result = controller.New(null) as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<Survey>;
            Assert.IsInstanceOfType(model.ContentModel, typeof(Survey));
        }

        [TestMethod]
        public void NewWhenHttpVerbIsPostCallsSaveFromSurveyStoreWithSurveyParameterWhenModelStateIsValid()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var controller = new SurveysController(mockSurveyStore.Object, null, null, null, null);
            var survey = new Survey("slug-name");
            var hiddenSurvey = new Survey();
            hiddenSurvey.Questions.Add(new Question());

            controller.New(survey, hiddenSurvey);

            mockSurveyStore.Verify(r => r.SaveSurvey(It.Is<Survey>(actual => survey == actual)), Times.Once());
        }

        [TestMethod]
        public void NewWhenHttpVerbIsPostCopiesQuestionsFromHiddenSurveyToSurveyWhenCallingSaveFromSurveyStoreWhenModelStateIsValid()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var controller = new SurveysController(mockSurveyStore.Object, null, null, null, null);
            var survey = new Survey("slug-name");
            var questionsToBeCopied = new List<Question>();
            questionsToBeCopied.Add(new Question());
            var hiddenSurvey = new Survey { Questions = questionsToBeCopied };

            controller.New(survey, hiddenSurvey);

            mockSurveyStore.Verify(r => r.SaveSurvey(It.Is<Survey>(actual => questionsToBeCopied == actual.Questions)));
        }

        [TestMethod]
        public void NewWhenHttpVerbIsPostReturnsRedirectToMySurveysWhenModelStateIsValid()
        {
            var controller = new SurveysController(new Mock<ISurveyStore>().Object, null, null, null, null);
            var hiddenSurvey = new Survey();
            hiddenSurvey.Questions.Add(new Question());
            
            var result = controller.New(new Survey(), hiddenSurvey) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(null, result.RouteValues["controller"]);
        }

        [TestMethod]
        public void NewWhenHttpVerbIsPostReturnsEmptyViewNameWhenModelStateIsNotValid()
        {
            var controller = new SurveysController(new Mock<ISurveyStore>().Object, null, null, null, null);
            controller.ModelState.AddModelError("error for test", @"invalid model state");

            var result = controller.New(new Survey(), new Survey()) as ViewResult;

            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [TestMethod]
        public void NewWhenHttpVerbIsPostReturnsTheSameModelWhenModelStateIsNotValid()
        {
            var controller = new SurveysController(new Mock<ISurveyStore>().Object, null, null, null, null);
            controller.ModelState.AddModelError("error for test", @"invalid model state");
            var survey = new Survey();

            var result = controller.New(survey, new Survey()) as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<Survey>;
            Assert.AreSame(survey, model.ContentModel);
        }

        [TestMethod]
        public void NewWhenHttpVerbIsPostCopiesQuestionsFromHiddenSurveyToSurveyWhenModelStateIsNotValid()
        {
            var questionsToBeCopied = new List<Question>();
            var hiddenSurvey = new Survey { Questions = questionsToBeCopied };
            var controller = new SurveysController(new Mock<ISurveyStore>().Object, null, null, null, null);
            controller.ModelState.AddModelError("error for test", @"invalid model state");

            var result = controller.New(new Survey(), hiddenSurvey) as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<Survey>;
            Assert.AreSame(questionsToBeCopied, model.ContentModel.Questions);
        }

        [TestMethod]
        public void NewQuestionReturnsEmptyViewName()
        {
            var controller = new SurveysController(null, null, null, null, null);

            var result = controller.NewQuestion(new Survey(), new Survey(), string.Empty) as ViewResult;

            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [TestMethod]
        public void NewQuestionReturnsNewQuestionAsTitleInTheModel()
        {
            var controller = new SurveysController(null, null, null, null, null);

            var result = controller.NewQuestion(new Survey(), new Survey(), string.Empty) as ViewResult;

            var model = result.ViewData.Model as TenantMasterPageViewData;
            Assert.AreSame("New Question", model.Title);
        }

        [TestMethod]
        public void NewQuestionCopiesHiddenSurveyToViewData()
        {
            var controller = new SurveysController(null, null, null, null, null);
            var hiddenSurvey = new Survey();

            var result = controller.NewQuestion(new Survey(), hiddenSurvey, string.Empty) as ViewResult;

            Assert.AreSame(hiddenSurvey, result.ViewData["hiddenSurvey"]);
        }

        [TestMethod]
        public void NewQuestionCopiesSurveyTitleToHiddenSurveyThatIsReturnedInViewDataWhenTheReferrerIsNotAddQuestion()
        {
            var controller = new SurveysController(null, null, null, null, null);
            var survey = new Survey { Title = "title" };

            var result = controller.NewQuestion(survey, new Survey(), "not add question") as ViewResult;

            var hiddenSurveyReturnedInViewData = result.ViewData["hiddenSurvey"] as Survey;
            Assert.AreSame(survey.Title, hiddenSurveyReturnedInViewData.Title);
        }

        [TestMethod]
        public void NewQuestionClearsTheModelStateWhenTheReferrerIsNotAddQuestion()
        {
            var controller = new SurveysController(null, null, null, null, null);
            controller.ModelState.AddModelError("error for test", @"invalid model state");

            controller.NewQuestion(new Survey(), new Survey(), "not add question");

            Assert.AreEqual(0, controller.ModelState.Count);
        }

        [TestMethod]
        public void AddQuestionReturnsRedirectToNewSurveyWhenModelIsValid()
        {
            var controller = new SurveysController(null, null, null, null, null);

            var result = controller.AddQuestion(new Survey(), new Question()) as RedirectToRouteResult;

            Assert.AreEqual("New", result.RouteValues["action"]);
            Assert.AreEqual(null, result.RouteValues["controller"]);
        }

        [TestMethod]
        public void AddQuestionAddsTheNewQuestionToTheHiddenSurveyReturnedInTempDataWhenModelIsValid()
        {
            var controller = new SurveysController(null, null, null, null, null);
            var hiddenSurvey = new Survey();
            hiddenSurvey.Questions.Add(new Question());
            var question = new Question();

            controller.AddQuestion(hiddenSurvey, question);

            var hiddenSurveyReturnedInTempData = controller.TempData["hiddenSurvey"] as Survey;
            Assert.IsTrue(hiddenSurveyReturnedInTempData.Questions.IndexOf(question) > -1);
        }

        [TestMethod]
        public void AddQuestionReturnsNewQuestionViewWhenModelIsNotValid()
        {
            var controller = new SurveysController(null, null, null, null, null);
            controller.ModelState.AddModelError("error for test", @"invalid model state");

            var result = controller.AddQuestion(null, null) as ViewResult;

            Assert.AreSame("NewQuestion", result.ViewName);
        }

        [TestMethod]
        public void AddQuestionReturnsNewQuestionAsModelWhenModelIsNotValid()
        {
            var controller = new SurveysController(null, null, null, null, null);
            controller.ModelState.AddModelError("error for test", @"invalid model state");
            var question = new Question();

            var result = controller.AddQuestion(null, question) as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<Question>;
            Assert.AreSame(question, model.ContentModel);
        }

        [TestMethod]
        public void AddQuestionCopiesHiddenSurveyToViewDataWhenModelIsNotValid()
        {
            var controller = new SurveysController(null, null, null, null, null);
            controller.ModelState.AddModelError("error for test", @"invalid model state");
            var hiddenSurvey = new Survey();

            var result = controller.AddQuestion(hiddenSurvey, null) as ViewResult;

            var hiddenSurveyReturnedInViewData = result.ViewData["hiddenSurvey"] as Survey;
            Assert.AreSame(hiddenSurvey, hiddenSurveyReturnedInViewData);
        }

        [TestMethod]
        public void AddQuestionReturnsNewQuestionAsTitleInTheModelWhenModelIsNotValid()
        {
            var controller = new SurveysController(null, null, null, null, null);
            controller.ModelState.AddModelError("error for test", @"invalid model state");

            var result = controller.AddQuestion(null, null) as ViewResult;

            var model = result.ViewData.Model as TenantMasterPageViewData;
            Assert.AreSame("New Question", model.Title);
        }

        [TestMethod]
        public void CancelNewQuestionReturnsRedirectToNewSurvey()
        {
            var controller = new SurveysController(null, null, null, null, null);

            var result = controller.CancelNewQuestion(null) as RedirectToRouteResult;

            Assert.AreEqual("New", result.RouteValues["action"]);
            Assert.AreEqual(null, result.RouteValues["controller"]);
        }

        [TestMethod]
        public void CancelNewQuestionReturnsTheHiddenSurveyInTempData()
        {
            var controller = new SurveysController(null, null, null, null, null);
            var hiddenSurvey = new Survey();

            controller.CancelNewQuestion(hiddenSurvey);

            var hiddenSurveyReturnedInTempData = controller.TempData["hiddenSurvey"] as Survey;
            Assert.AreSame(hiddenSurvey, hiddenSurveyReturnedInTempData);
        }

        [TestMethod]
        public void DeleteCallsDeleteSurveyByTenantAndSlugNameFromSurveyStore()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var controller = new SurveysController(mockSurveyStore.Object, new Mock<ISurveyAnswerStore>().Object, new Mock<ISurveyAnswersSummaryStore>().Object, null, null);

            controller.Delete("tenant", "survey-slug");

            mockSurveyStore.Verify(
                r => r.DeleteSurveyByTenantAndSlugName(
                    It.Is<string>(t => "tenant" == t),
                    It.Is<string>(s => "survey-slug" == s)),
                Times.Once());
        }

        [TestMethod]
        public void DeleteCallsDeleteSurveyAnswersStore()
        {
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(new Mock<ISurveyStore>().Object, mockSurveyAnswerStore.Object, new Mock<ISurveyAnswersSummaryStore>().Object, null, null);

            controller.Delete("tenant", "survey-slug");

            mockSurveyAnswerStore.Verify(r => r.DeleteSurveyAnswers("tenant", "survey-slug"), Times.Once());
        }

        [TestMethod]
        public void DeleteCallsDeleteSurveyAnswersSummariesStore()
        {
            var mockSurveyAnswersSummaryStore = new Mock<ISurveyAnswersSummaryStore>();
            var controller = new SurveysController(new Mock<ISurveyStore>().Object, new Mock<ISurveyAnswerStore>().Object, mockSurveyAnswersSummaryStore.Object, null, null);

            controller.Delete("tenant", "survey-slug");

            mockSurveyAnswersSummaryStore.Verify(r => r.DeleteSurveyAnswersSummary("tenant", "survey-slug"), Times.Once());
        }

        [TestMethod]
        public void DeleteReturnsRedirectToMySurveys()
        {
            var controller = new SurveysController(new Mock<ISurveyStore>().Object, new Mock<ISurveyAnswerStore>().Object, new Mock<ISurveyAnswersSummaryStore>().Object, null, null);

            var result = controller.Delete(string.Empty, string.Empty) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(null, result.RouteValues["controller"]);
        }

        [TestMethod]
        public void BrowseResponsesReturnSlugNameAsTheTitle()
        {
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(null, mockSurveyAnswerStore.Object, null, null, null);
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswerBrowsingContext(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(new SurveyAnswerBrowsingContext { PreviousId = string.Empty, NextId = string.Empty });

            var result = controller.BrowseResponses(string.Empty, "slug-name", string.Empty) as ViewResult;

            var model = result.ViewData.Model as TenantMasterPageViewData;
            Assert.AreSame("slug-name", model.Title);
        }

        [TestMethod]
        public void BrowseResponsesGetsTheAnswerFromTheStoreWhenAnswerIdIsNotEmpty()
        {
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(null, mockSurveyAnswerStore.Object, null, null, null);
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswerBrowsingContext(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(new SurveyAnswerBrowsingContext { PreviousId = string.Empty, NextId = string.Empty });

            controller.BrowseResponses("tenant", "survey-slug", "answer id");

            mockSurveyAnswerStore.Verify(r => r.GetSurveyAnswer("tenant", "survey-slug", "answer id"));
        }

        [TestMethod]
        public void BrowseResponsesGetsTheFirstAnswerIdFromTheStoreWhenAnswerIdIsEmpty()
        {
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(null, mockSurveyAnswerStore.Object, null, null, null);
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswerBrowsingContext(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(new SurveyAnswerBrowsingContext { PreviousId = string.Empty, NextId = string.Empty });

            controller.BrowseResponses("tenant", "survey-slug", string.Empty);

            mockSurveyAnswerStore.Verify(r => r.GetFirstSurveyAnswerId("tenant", "survey-slug"), Times.Once());
        }

        [TestMethod]
        public void BrowseResponsesGetsSurveyAnswerWithTheIdReturnedFromTheStoreWhenAnswerIdIsEmpty()
        {
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(null, mockSurveyAnswerStore.Object, null, null, null);
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswerBrowsingContext(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(new SurveyAnswerBrowsingContext { PreviousId = string.Empty, NextId = string.Empty });
            mockSurveyAnswerStore.Setup(r => r.GetFirstSurveyAnswerId("tenant", "survey-slug"))
                                      .Returns("id");

            controller.BrowseResponses("tenant", "survey-slug", string.Empty);

            mockSurveyAnswerStore.Verify(r => r.GetSurveyAnswer("tenant", "survey-slug", "id"));
        }

        [TestMethod]
        public void BrowseResponsesSetsTheAnswerFromTheStoreInTheModel()
        {
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(null, mockSurveyAnswerStore.Object, null, null, null);
            var surveyAnswer = new SurveyAnswer();
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswerBrowsingContext(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(new SurveyAnswerBrowsingContext { PreviousId = string.Empty, NextId = string.Empty });
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswer("tenant", "survey-slug", "answer id"))
                                      .Returns(surveyAnswer);

            var result = controller.BrowseResponses("tenant", "survey-slug", "answer id") as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<BrowseResponseModel>;
            Assert.AreSame(surveyAnswer, model.ContentModel.SurveyAnswer);
        }

        [TestMethod]
        public void BrowseResponsesCallsGetSurveyAnswerBrowsingContextFromStore()
        {
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(null, mockSurveyAnswerStore.Object, null, null, null);
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswerBrowsingContext(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(new SurveyAnswerBrowsingContext { PreviousId = string.Empty, NextId = string.Empty });

            controller.BrowseResponses("tenant", "survey-slug", "answer id");

            mockSurveyAnswerStore.Verify(
                r => r.GetSurveyAnswerBrowsingContext("tenant", "survey-slug", "answer id"),
                Times.Once());
        }

        [TestMethod]
        public void BrowseResponsesSetsPreviousAndNextIdsInModel()
        {
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(null, mockSurveyAnswerStore.Object, null, null, null);
            mockSurveyAnswerStore.Setup(r => r.GetSurveyAnswerBrowsingContext(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                      .Returns(new SurveyAnswerBrowsingContext { PreviousId = "PreviousId", NextId = "NextId" });

            var result = controller.BrowseResponses("tenant", "survey-slug", "answer id") as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<BrowseResponseModel>;
            Assert.AreEqual("PreviousId", model.ContentModel.PreviousAnswerId);
            Assert.AreEqual("NextId", model.ContentModel.NextAnswerId);
        }

        [TestMethod]
        public void AnalyzeReturnSlugNameAsTheTitle()
        {
            var controller = new SurveysController(null, null, new Mock<ISurveyAnswersSummaryStore>().Object, null, null);

            var result = controller.Analyze(string.Empty, "slug-name") as ViewResult;

            var model = result.ViewData.Model as TenantMasterPageViewData;
            Assert.AreSame("slug-name", model.Title);
        }

        [TestMethod]
        public void AnalyzeGetsTheSummaryFromTheStore()
        {
            var mockSurveyAnswersSummaryStore = new Mock<ISurveyAnswersSummaryStore>();
            var controller = new SurveysController(null, null, mockSurveyAnswersSummaryStore.Object, null, null);

            controller.Analyze("tenant", "slug-name");

            mockSurveyAnswersSummaryStore.Verify(r => r.GetSurveyAnswersSummary("tenant", "slug-name"), Times.Once());
        }

        [TestMethod]
        public void AnalyzeReturnsTheSummaryGetFromTheStoreInTheModel()
        {
            var mockSurveyAnswersSummaryStore = new Mock<ISurveyAnswersSummaryStore>();
            var controller = new SurveysController(null, null, mockSurveyAnswersSummaryStore.Object, null, null);
            var surveyAnswersSummary = new SurveyAnswersSummary();
            mockSurveyAnswersSummaryStore.Setup(r => r.GetSurveyAnswersSummary(It.IsAny<string>(), It.IsAny<string>()))
                                              .Returns(surveyAnswersSummary);

            var result = controller.Analyze(string.Empty, string.Empty) as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<SurveyAnswersSummary>;
            Assert.AreSame(surveyAnswersSummary, model.ContentModel);
        }

        [TestMethod]
        public void ExportGetsTheTenantInformationAndPutsInModel()
        {
            var mockTenantStore = new Mock<ITenantStore>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(null, mockSurveyAnswerStore.Object, null, mockTenantStore.Object, null);
            var tenant = new Tenant();
            mockTenantStore.Setup(r => r.GetTenant(It.IsAny<string>())).Returns(tenant);
            mockSurveyAnswerStore.Setup(r => r.GetFirstSurveyAnswerId(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
            
            var result = controller.ExportResponses(string.Empty) as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<ExportResponseModel>;
            Assert.AreSame(tenant, model.ContentModel.Tenant);
        }

        [TestMethod]
        public void ExportDeterminesIfThereAreResponsesToExport()
        {
            var mockTenantStore = new Mock<ITenantStore>();
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(null, mockSurveyAnswerStore.Object, null, mockTenantStore.Object, null);
            var tenant = new Tenant();
            mockTenantStore.Setup(r => r.GetTenant(It.IsAny<string>())).Returns(tenant);
            mockSurveyAnswerStore.Setup(r => r.GetFirstSurveyAnswerId(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
            
            var result = controller.ExportResponses(string.Empty) as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<ExportResponseModel>;
            Assert.AreEqual(false, model.ContentModel.HasResponses);
        }

        [TestMethod]
        public void ExportPostCallsTransferPostWithTenantAndSlugName()
        {
            var mockSurveyTransferStore = new Mock<ISurveyTransferStore>();
            var controller = new SurveysController(null, null, null, null, mockSurveyTransferStore.Object);
            
            controller.ExportResponses("tenant", "slugName");

            mockSurveyTransferStore.Verify(r => r.Transfer("tenant", "slugName"), Times.Once());
        }

        [TestMethod]
        public void ExportPostRedirectsToBrowseAction()
        {
            var mockSurveyTransferStore = new Mock<ISurveyTransferStore>();
            var controller = new SurveysController(null, null, null, null, mockSurveyTransferStore.Object);

            var result = controller.ExportResponses("tenant", "slugName") as RedirectToRouteResult;

            Assert.AreEqual(result.RouteValues["action"], "BrowseResponses");
        }
    }
}

